using System;
using System.Threading;
using System.Collections.Generic;
using ControlRoomApplication.Entities;
using ControlRoomApplication.Database;

namespace ControlRoomApplication.Controllers
{
    public class RadioTelescopeControllerManagementThread
    {
        private static readonly log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public RadioTelescopeController RTController { get; private set; }
        public Appointment AppointmentToDisplay { get; private set; }

        private Thread ManagementThread;
        private Mutex ManagementMutex;
        private volatile bool KeepThreadAlive;
        private volatile bool InterruptAppointmentFlag;
        private Orientation _NextObjectiveOrientation;

        public Orientation NextObjectiveOrientation
        {
            get
            {
                return _NextObjectiveOrientation;
            }
            set
            {
                ManagementMutex.WaitOne();
                _NextObjectiveOrientation = value;
                ManagementMutex.ReleaseMutex();
            }
        }
        
        public int RadioTelescopeID
        {
            get
            {
                return RTController.RadioTelescope.Id;
            }
        }

        public bool Busy
        {
            get
            {
                if (!ManagementMutex.WaitOne(100))
                {
                    return true;
                }

                bool busy = _NextObjectiveOrientation != null;
                ManagementMutex.ReleaseMutex();

                return busy;
            }
        }

        public RadioTelescopeControllerManagementThread(RadioTelescopeController controller)
        {
            RTController = controller;

            ManagementThread = new Thread(new ThreadStart(SpinRoutine))
            {
                Name = "RTControllerManagementThread (ID=" + RadioTelescopeID.ToString() + ")"
            };

            ManagementMutex = new Mutex();
            KeepThreadAlive = false;
            _NextObjectiveOrientation = null;
            InterruptAppointmentFlag = false;
        }

        public bool Start()
        {
            KeepThreadAlive = true;

            try
            {
                ManagementThread.Start();
            }
            catch (Exception e)
            {
                if ((e is ThreadStateException) || (e is OutOfMemoryException))
                {
                    return false;
                }
                else
                {
                    // Unexpected exception
                    throw e;
                }
            }

            return true;
        }

        public void RequestToKill()
        {
            KeepThreadAlive = false;
        }

        public void KillWithHardInterrupt()
        {
            KeepThreadAlive = false;
            InterruptAppointmentFlag = true;
        }

        public bool WaitToJoin()
        {
            try
            {
                ManagementThread.Join();
            }
            catch (Exception e)
            {
                if ((e is ThreadStateException) || (e is ThreadInterruptedException))
                {
                    return false;
                }
                else
                {
                    // Unexpected exception
                    throw e;
                }
            }

            return true;
        }

        public void InterruptOnce()
        {
            InterruptAppointmentFlag = true;
        }

        private void SpinRoutine()
        {
            bool KeepAlive = KeepThreadAlive;
            
            while (KeepAlive)
            {
                Appointment NextAppointment = WaitForNextAppointment();

                if (NextAppointment != null)
                {
                    logger.Info("Starting appointment...");

                    // Calibrate telescope
                    if (NextAppointment.Type != AppointmentTypeEnum.FREE_CONTROL)
                    {
                        logger.Info("Calibrating RadioTelescope");
                        RTController.CalibrateRadioTelescope();
                    }

                    // Create movement thread
                    Thread AppointmentMovementThread = new Thread(() => PerformRadioTelescopeMovement(NextAppointment))
                    {
                        Name = "RTControllerIntermediateThread (ID=" + RadioTelescopeID.ToString() + ")"
                    };

                    // Start SpectraCyber
                    StartReadingData(NextAppointment);

                    // Start movement thread
                    AppointmentMovementThread.Start();

                    // End PLC thread & SpectraCyber 
                    AppointmentMovementThread.Join();
                    StopReadingRFData();

                    // Stow Telescope
                    EndAppointment();

                    logger.Info("Appointment completed.");
                }
                else
                {
                    if (InterruptAppointmentFlag)
                    {
                        logger.Info("Appointment interrupted in loading routine.");
                        ManagementMutex.WaitOne();
                        InterruptAppointmentFlag = false;
                        ManagementMutex.ReleaseMutex();
                    }

                    logger.Info("Appointment does not have an orientation associated with it.");
                }

                KeepAlive = KeepThreadAlive;

                Thread.Sleep(100);
            }
        }

        /// <summary>
        /// Waits for the next chronological appointment's start time to be less than 10 minutes
        /// from the current time of day. Once we are 10 minutes from the appointment's start time
        /// we should begin operations such as calibration.
        /// </summary>
        /// <returns> An appointment object that is next in chronological order and is less than 10 minutes away from starting. </returns>
        private Appointment WaitForNextAppointment()
        {
            Appointment NextAppointment;
            while ((NextAppointment = DatabaseOperations.GetNextAppointment(RadioTelescopeID)) == null)
            {
                if (InterruptAppointmentFlag || (!KeepThreadAlive))
                {
                    return null;
                }

                // Delay between checking database for new appointments
                Thread.Sleep(100);
            }

            logger.Info("Waiting for the next appointment to be within 10 minutes.");
            TimeSpan diff;
            while ((diff = NextAppointment.StartTime - DateTime.UtcNow).TotalMinutes > 1)
            {
                if (InterruptAppointmentFlag || (!KeepThreadAlive))
                {
                    return null;
                }

                // Wait more
            }

            logger.Info("The next appointment is now within the correct timeframe.");
            AppointmentToDisplay = NextAppointment;
            return NextAppointment;
        }

        /// <summary>
        /// Starts movement of the RT by updating the appointment status and
        /// then calling the RT controller to move the RT to the orientation
        /// it needs to go to.
        /// </summary>
        /// <param name="NextAppointment"> The appointment that is currently running. </param>
        private void PerformRadioTelescopeMovement(Appointment NextAppointment)
        {
            NextAppointment.Status = AppointmentStatusEnum.IN_PROGRESS;
            DatabaseOperations.UpdateAppointment(NextAppointment);

            logger.Info("Appointment Type: " + NextAppointment.Type);

            // Loop through each second or minute of the appointment (depending on appt type)
            TimeSpan length = NextAppointment.EndTime - NextAppointment.StartTime;
            double duration = NextAppointment.Type == AppointmentTypeEnum.FREE_CONTROL ? length.TotalSeconds : length.TotalMinutes;
            for (int i = 0; i <= (int) duration; i++)
            {
                // Get orientation for current datetime
                DateTime datetime = NextAppointment.Type == AppointmentTypeEnum.FREE_CONTROL ? NextAppointment.StartTime.AddSeconds(i) : NextAppointment.StartTime.AddMinutes(i); 
                NextObjectiveOrientation = RTController.CoordinateController.CalculateOrientation(NextAppointment, datetime);

                // Wait for datetime
                while (DateTime.UtcNow < datetime)
                {
                    if (InterruptAppointmentFlag)
                    {
                        logger.Info("Interrupted appointment [" + NextAppointment.Id.ToString() + "] at " + DateTime.Now.ToString());
                        break;
                    }

                    //logger.Debug(datetime.ToString() + " vs. " + DateTime.UtcNow.ToString());
                    Thread.Sleep(1000);
                }

                if (InterruptAppointmentFlag)
                {
                    break;
                }

                // Move to orientation
                if (NextObjectiveOrientation != null)
                {
                    if (NextObjectiveOrientation.Azimuth < 0 || NextObjectiveOrientation.Elevation < 0)
                    {
                        logger.Warn("Invalid Appt: Az = " + NextObjectiveOrientation.Azimuth + ", El = " + NextObjectiveOrientation.Elevation);
                        InterruptAppointmentFlag = true;
                        break;
                    }

                    logger.Info("Moving to Next Objective: Az = " + NextObjectiveOrientation.Azimuth + ", El = " + NextObjectiveOrientation.Elevation);
                    RTController.MoveRadioTelescopeToOrientation(NextObjectiveOrientation);

                    // Wait until MCU issues finished move status
                    do
                    {
                        if (InterruptAppointmentFlag)
                        {
                            break;
                        }

                        //currentOrientation = RTController.GetCurrentOrientation();
                        //logger.Info("Progress Towards Objective: Az = " + currentOrientation.Azimuth + ", El = " + currentOrientation.Elevation);
                        Thread.Sleep(100);
                    }
                    while (!RTController.finished_exicuting_move( RadioTelescopeAxisEnum.BOTH));

                    NextObjectiveOrientation = null;
                }
            }

            if (InterruptAppointmentFlag)
            {
                logger.Info("Interrupted appointment [" + NextAppointment.Id.ToString() + "] at " + DateTime.Now.ToString());
                NextAppointment.Status = AppointmentStatusEnum.CANCELLED;
                DatabaseOperations.UpdateAppointment(NextAppointment);
                NextObjectiveOrientation = null;
                InterruptAppointmentFlag = false;
            }
            else
            {
                NextAppointment.Status = AppointmentStatusEnum.COMPLETED;
                DatabaseOperations.UpdateAppointment(NextAppointment);
            }

            DatabaseOperations.UpdateAppointment(NextAppointment);
        }

        /// <summary>
        /// Ends an appointment by returning the RT to the stow position.
        /// </summary>
        private void EndAppointment()
        {
            logger.Info("Ending Appointment");
            RTController.MoveRadioTelescopeToOrientation(new Orientation(0, 90));
        }

        /// <summary>
        /// Calls the SpectraCyber controller to start the SpectraCyber readings.
        /// </summary>
        private void StartReadingData(Appointment appt)
        {
            logger.Info("Starting Reading of RFData");
            RTController.RadioTelescope.SpectraCyberController.SetApptConfig(appt);
            RTController.RadioTelescope.SpectraCyberController.StartScan();
        }

        /// <summary>
        /// Calls the SpectraCyber controller to stop the SpectraCyber readings.
        /// </summary>
        private void StopReadingRFData()
        {
            logger.Info("Stoping Reading of RTData");
            RTController.RadioTelescope.SpectraCyberController.StopScan();
            RTController.RadioTelescope.SpectraCyberController.RemoveActiveAppointmentID();
            RTController.RadioTelescope.SpectraCyberController.SetSpectraCyberModeType(SpectraCyberModeTypeEnum.UNKNOWN);
        }

        public static int IndexOf(List<RadioTelescopeControllerManagementThread> ThreadList, RadioTelescope RadioTelescope)
        {
            int i = 0;
            foreach (RadioTelescopeControllerManagementThread rtmt in ThreadList)
            {
                if (rtmt.RTController.RadioTelescope.Equals(RadioTelescope))
                {
                    return i;
                }

                i++;
            }

            return -1;
        }
    }
}
