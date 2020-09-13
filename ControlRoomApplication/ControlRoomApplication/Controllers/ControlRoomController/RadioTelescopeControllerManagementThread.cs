using System;
using System.Threading;
using System.Collections.Generic;
using ControlRoomApplication.Entities;
using ControlRoomApplication.Database;
using System.Net;


namespace ControlRoomApplication.Controllers
{
    public class RadioTelescopeControllerManagementThread
    {
        private static readonly log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public RadioTelescopeController RTController { get; private set; }
        public Appointment AppointmentToDisplay { get; private set; }

        public Thread ManagementThread;
        private Mutex ManagementMutex;
        private volatile bool KeepThreadAlive;
        private volatile bool InterruptAppointmentFlag;
        private Orientation _NextObjectiveOrientation;

        public RemoteListener TCPListener { get; }

        public List<Override> ActiveOverrides;
        public List<Sensor> Sensors;

        private bool OverallSensorStatus;

        private bool endAppt = false;

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

            ActiveOverrides = new List<Override>();
            Sensors = new List<Sensor>();
            OverallSensorStatus = true;

            Sensors.Add(new Sensor(SensorItemEnum.WIND, SensorStatusEnum.NORMAL));
           
            // Commented out because we will not be using this functionality in the future.
            // We will switch to connecting to a server on the cloud
            // Kate Kennelly 2/14/2020
            // TCPListener = new RemoteListener(8090, IPAddress.Parse("10.127.7.112"), controller);
        }

        public bool Start()
        {
            KeepThreadAlive = true;

            try
            {
               // Sensors.Add(new Sensor(SensorItemEnum.WIND_SPEED, SensorStatusEnum.NORMAL));
                
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
                logger.Info("Waiting for next Appointment");
                Appointment NextAppointment = WaitForNextAppointment();

                if (NextAppointment != null)
                {
                    logger.Info("Starting appointment...");
                    endAppt = false;

                    // Calibrate telescope
                    if (NextAppointment._Type != AppointmentTypeEnum.FREE_CONTROL)
                    {
                        logger.Info("Themal Calibrating RadioTelescope");
                        RTController.ThermalCalibrateRadioTelescope();
                    }

                    // Create movement thread
                    Thread AppointmentMovementThread = new Thread(() => PerformRadioTelescopeMovement(NextAppointment))
                    {
                        Name = "RTControllerIntermediateThread (ID=" + RadioTelescopeID.ToString() + ")"
                    };

                    // Start SpectraCyber if the next appointment is NOT an appointment created by the control form
                    // This is to allow for greater control of the spectra cyber output from the control form
                    if(NextAppointment._Type != AppointmentTypeEnum.FREE_CONTROL)
                        StartReadingData(NextAppointment);

                    // Start movement thread
                    AppointmentMovementThread.Start();

                    if(NextAppointment._Type != AppointmentTypeEnum.FREE_CONTROL)
                    {
                        // End PLC thread & SpectraCyber 
                        AppointmentMovementThread.Join();
                        StopReadingRFData();
                        // Stow Telescope
                        EndAppointment();
                    }
                    else
                    {
                        while (endAppt == false)
                        {
                            ;
                        }
                    }

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
            Appointment NextAppointment = DatabaseOperations.GetNextAppointment(RadioTelescopeID);
            TimeSpan diff;
            while (NextAppointment != null && (diff = NextAppointment.start_time - DateTime.UtcNow).TotalMinutes > 1)
            {
                NextAppointment = DatabaseOperations.GetNextAppointment(RadioTelescopeID);

                if (InterruptAppointmentFlag || (!KeepThreadAlive))
                {
                    return null;
                }

                // Delay between checking database for new appointments
                Thread.Sleep(100);

                logger.Info("Waiting for the next appointment to be within 1 minutes.");
            }

            
  /*          while ((diff = NextAppointment.start_time - DateTime.UtcNow).TotalMinutes > 1)
            {
                if (InterruptAppointmentFlag || (!KeepThreadAlive))
                {
                    return null;
                }

                // Wait more
            }
            */
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
            NextAppointment._Status = AppointmentStatusEnum.IN_PROGRESS;
            DatabaseOperations.UpdateAppointment(NextAppointment);

            // send message to appointment's user
            SNSMessage.sendMessage(NextAppointment.User, MessageTypeEnum.APPOINTMENT_STARTED);
         

            logger.Info("Appointment _Type: " + NextAppointment._Type);

            // Loop through each second or minute of the appointment (depending on appt type)
            TimeSpan length = NextAppointment.end_time - NextAppointment.start_time;
            double duration = NextAppointment._Type == AppointmentTypeEnum.FREE_CONTROL ? length.TotalSeconds : length.TotalMinutes;
            for (int i = 0; i <= (int) duration; i++)
            {
                // before we move, check to see if it is safe
                if (checkCurrentSensorAndOverrideStatus())
                {

                    // Get orientation for current datetime
                    DateTime datetime = NextAppointment._Type == AppointmentTypeEnum.FREE_CONTROL ? NextAppointment.start_time.AddSeconds(i) : NextAppointment.start_time.AddMinutes(i);
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
                    // Kate - removed the check for azumith < 0 in the below if statement due to Todd's request
                    // Reason being, we should not have an azimuth below 0 be given to us. That check is in the
                    // method calling this!
                    if (NextObjectiveOrientation.Elevation < 0)
                    {
                        logger.Warn("Invalid Appt: Az = " + NextObjectiveOrientation.Azimuth + ", El = " + NextObjectiveOrientation.Elevation);
                        InterruptAppointmentFlag = true;
                        break;
                    }

                        logger.Info("Moving to Next Objective: Az = " + NextObjectiveOrientation.Azimuth + ", El = " + NextObjectiveOrientation.Elevation);
                        RTController.MoveRadioTelescopeToOrientation(NextObjectiveOrientation);

                        if (InterruptAppointmentFlag)
                        {
                            break;
                        }

                         //currentOrientation = RTController.GetCurrentOrientation();
                         //logger.Info("Progress Towards Objective: Az = " + currentOrientation.Azimuth + ", El = " + currentOrientation.Elevation);
                         Thread.Sleep(100);

                        NextObjectiveOrientation = null;
                    }
                } else
                {
                    logger.Info("Telescope stopped movement.");
                    i--;
                }
            }

            if (InterruptAppointmentFlag)
            {
                logger.Info("Interrupted appointment [" + NextAppointment.Id.ToString() + "] at " + DateTime.Now.ToString());
                NextAppointment._Status = AppointmentStatusEnum.CANCELED;
                DatabaseOperations.UpdateAppointment(NextAppointment);
                NextObjectiveOrientation = null;
                InterruptAppointmentFlag = false;

                // send message to appointment's user
                SNSMessage.sendMessage(NextAppointment.User, MessageTypeEnum.APPOINTMENT_CANCELLED);
                

            }
            else
            {
                NextAppointment._Status = AppointmentStatusEnum.COMPLETED;
                DatabaseOperations.UpdateAppointment(NextAppointment);

                // send message to appointment's user
                SNSMessage.sendMessage(NextAppointment.User, MessageTypeEnum.APPOINTMENT_COMPLETION);
            }
        }

        /// <summary>
        /// Ends an appointment by returning the RT to the stow position.
        /// </summary>
        public void EndAppointment()
        {
            logger.Info("Ending Appointment");
            endAppt = true;
            RTController.MoveRadioTelescopeToOrientation(new Orientation(0, 90));
        }

        /// <summary>
        /// Calls the SpectraCyber controller to start the SpectraCyber readings.
        /// </summary>
        public void StartReadingData(Appointment appt)
        {
            logger.Info("Starting Reading of RFData");
            RTController.RadioTelescope.SpectraCyberController.SetApptConfig(appt);
            RTController.RadioTelescope.SpectraCyberController.StartScan(appt);
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

        /// <summary>
        /// Checks to see if there are any sensors that are not overriden
        /// calls the stop telescope function if it is not safe
        /// Returns true if the telescope is safe to operate
        /// Returns false if the telescope is not safe to operate
        /// </summary>
        public bool checkCurrentSensorAndOverrideStatus()
        {
            // loop through all the current sensors
            foreach (Sensor curSensor in Sensors)
            {
                // if the sensor is in the ALARM state
                if (curSensor.Status == SensorStatusEnum.ALARM)
                {
                    // check to see if there is an override for that sensor
                    if (ActiveOverrides.Find(i => i.Item == curSensor.Item) == null)
                    {
                        // if not, return false
                        // we should not be operating the telescope
                        logger.Fatal("Telescope in DANGER due to fatal sensors");
                        RTController.ExecuteRadioTelescopeImmediateStop();
                        OverallSensorStatus = false;
                        return false;
                    }                    
                }
            }

            OverallSensorStatus = true;
            logger.Info("Telescope in safe state.");
            return true;
        }

    }
}
