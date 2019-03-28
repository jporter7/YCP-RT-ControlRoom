﻿using System;
using System.Threading;
using System.Collections.Generic;
using ControlRoomApplication.Constants;
using ControlRoomApplication.Entities;
using ControlRoomApplication.Entities.RadioTelescope;
using ControlRoomApplication.Controllers.RadioTelescopeControllers;
using ControlRoomApplication.Database.Operations;

namespace ControlRoomApplication.Controllers
{
    public class RadioTelescopeControllerManagementThread
    {
        private static readonly log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public RadioTelescopeController RTController { get; private set; }

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
                Name = "RadioTelescopeControllerManagementThread (ID=" + RadioTelescopeID.ToString() + ") overarching management thread"
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
                ManagementMutex.WaitOne();

                Appointment NextAppointment = WaitForNextAppointment();

                if (NextAppointment != null)
                {
                    Console.WriteLine("[RadioTelescopeControllerManagementThread : ID=" + RadioTelescopeID.ToString() + "] Starting appointment...");

                    // Calibrate telescope
                    if (NextAppointment.Type != AppointmentTypeConstants.FREE_CONTROL)
                    {
                        RTController.CalibrateRadioTelescope();
                    }

                    // Create movement thread
                    Thread AppointmentMovementThread = new Thread(() => PerformRadioTelescopeMovement(NextAppointment))
                    {
                        Name = "RadioTelescopeControllerManagementThread (ID=" + RadioTelescopeID.ToString() + ") intermediate movement thread"
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

                    Console.WriteLine("[RadioTelescopeControllerManagementThread : ID=" + RadioTelescopeID.ToString() + "] Appointment completed.");
                    logger.Info("Appointment completed.");
                }
                else
                {
                    if (InterruptAppointmentFlag)
                    {
                        Console.WriteLine("[RadioTelescopeControllerManagementThread : ID=" + RadioTelescopeID.ToString() + "] Appointment interrupted in loading routine.");
                        logger.Info("Appointment interrupted in loading routine.");

                        InterruptAppointmentFlag = false;
                    }

                    Console.WriteLine("[RadioTelescopeControllerManagementThread : ID=" + RadioTelescopeID.ToString() + "] Appointment does not have an orientation associated with it.");
                    logger.Info("Appointment does not have an orientation associated with it.");
                }

                KeepAlive = KeepThreadAlive;

                ManagementMutex.ReleaseMutex();

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
            while ((diff = NextAppointment.StartTime - DateTime.Now).TotalMinutes > 1)
            {
                if (InterruptAppointmentFlag || (!KeepThreadAlive))
                {
                    return null;
                }

                // Wait more
            }

            logger.Info("The next appointment is now within the correct timeframe.");
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
            NextAppointment.Status = AppointmentConstants.IN_PROGRESS;
            DatabaseOperations.UpdateAppointment(NextAppointment);

            Console.WriteLine("[RadioTelescopeControllerManagementThread : ID=" + RadioTelescopeID.ToString() + "] Appointment Type: " + NextAppointment.Type);

            // Loop through each minute of the appointment 
            TimeSpan length = NextAppointment.EndTime - NextAppointment.StartTime;
            for (int i = 0; i < length.TotalMinutes; i++)
            {
                // Get orientation for current datetime
                DateTime datetime = NextAppointment.StartTime.AddMinutes(i);
                NextObjectiveOrientation = RTController.CoordinateController.CalculateOrientation(NextAppointment, datetime);

                // Wait for datetime
                while (DateTime.Now < datetime)
                {
                    if (InterruptAppointmentFlag)
                    {
                        Console.WriteLine("Interrupted appointment [" + NextAppointment.Id.ToString() +"] at " + DateTime.Now.ToString());
                        break;
                    }

                    // Console.WriteLine(datetime.ToString() + " vs. " + DateTime.Now.ToString());

                    Thread.Sleep(1000);
                }

                if (InterruptAppointmentFlag)
                {
                    break;
                }

                // Move to orientation
                if (_NextObjectiveOrientation != null)
                {
                    Console.WriteLine("[RadioTelescopeControllerManagementThread : ID=" + RadioTelescopeID.ToString() + "] Moving to Next Objective: Az = " + _NextObjectiveOrientation.Azimuth + ", El = " + _NextObjectiveOrientation.Elevation);
                    RTController.MoveRadioTelescope(_NextObjectiveOrientation);
                    NextObjectiveOrientation = null;
                }
            }

            if (InterruptAppointmentFlag)
            {
                NextAppointment.Status = AppointmentConstants.CANCELLED;
                NextObjectiveOrientation = null;
                InterruptAppointmentFlag = false;
            }
            else
            {
                NextAppointment.Status = AppointmentConstants.COMPLETED;
            }

            DatabaseOperations.UpdateAppointment(NextAppointment);
        }

        /// <summary>
        /// Ends an appointment by returning the RT to the stow position.
        /// </summary>
        private void EndAppointment()
        {
            RTController.MoveRadioTelescope(new Orientation(0, 90));
        }

        /// <summary>
        /// Calls the SpectraCyber controller to start the SpectraCyber readings.
        /// </summary>
        private void StartReadingData(Appointment appt)
        {
            var spectraCyberController = RTController.RadioTelescope.SpectraCyberController;
            spectraCyberController.SetSpectraCyberModeType(appt.SpectraCyberModeType);
            spectraCyberController.SetActiveAppointmentID(appt.Id);
            spectraCyberController.StartScan();
        }

        /// <summary>
        /// Calls the SpectraCyber controller to stop the SpectraCyber readings.
        /// </summary>
        private void StopReadingRFData()
        {
            var spectraCyberController = RTController.RadioTelescope.SpectraCyberController;
            spectraCyberController.StopScan();
            spectraCyberController.RemoveActiveAppointmentID();
            spectraCyberController.SetSpectraCyberModeType(SpectraCyberModeTypeEnum.UNKNOWN);
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
