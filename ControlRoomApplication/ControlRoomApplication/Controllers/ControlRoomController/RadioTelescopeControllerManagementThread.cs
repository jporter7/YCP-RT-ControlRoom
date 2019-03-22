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
        public RadioTelescopeController RTController { get; private set; }
        private Thread ManagementThread;
        private Mutex ManagementMutex;
        private bool KeepThreadAlive;

        private Orientation NextObjective;
        private bool InterruptAppointmentFlag;

        public int RadioTelescopeID
        {
            get
            {
                return RTController.RadioTelescope.Id;
            }
        }

        private static readonly log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public Orientation NextObjectiveOrientation
        {
            get
            {
                return NextObjective;
            }
            set
            {
                ManagementMutex.WaitOne();
                NextObjective = value;
                ManagementMutex.ReleaseMutex();
            }
        }

        public bool Busy
        {
            get
            {
                if (!ManagementMutex.WaitOne(10))
                {
                    return true;
                }

                bool busy = NextObjective == null;
                ManagementMutex.ReleaseMutex();

                return busy;
            }
        }

        public RadioTelescopeControllerManagementThread(RadioTelescopeController controller)
        {
            RTController = controller;

            ManagementThread = new Thread(new ThreadStart(SpinRoutine));
            ManagementThread.Name = "RadioTelescopeControllerManagementThread (ID=" + RadioTelescopeID.ToString() + ") overarching management thread";

            ManagementMutex = new Mutex();
            KeepThreadAlive = false;
            NextObjective = null;
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
            ManagementMutex.WaitOne();
            KeepThreadAlive = false;
            ManagementMutex.ReleaseMutex();
        }

        public void KillWithHardInterrupt()
        {
            ManagementMutex.WaitOne();
            InterruptAppointmentFlag = true;
            KeepThreadAlive = false;
            ManagementMutex.ReleaseMutex();
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
            ManagementMutex.WaitOne();
            InterruptAppointmentFlag = true;
            ManagementMutex.ReleaseMutex();
        }

        public void SpinRoutine()
        {
            bool KeepAlive = KeepThreadAlive;
            
            while (KeepAlive)
            {
                ManagementMutex.WaitOne();

                Appointment appt = WaitForNextAppointment();

                if (appt != null)
                {
                    Console.WriteLine("[RadioTelescopeControllerManagementThread : ID=" + RadioTelescopeID.ToString() + "] Starting appointment...");

                    // Calibrate telescope
                    CalibrateRadioTelescope();

                    // Create movement thread
                    Thread movementThread = new Thread(() => StartRadioTelescope(appt))
                    {
                        Name = "RadioTelescopeControllerManagementThread (ID=" + RadioTelescopeID.ToString() + ") intermediate movement thread"
                    };

                    // Start SpectraCyber
                    StartReadingData(appt);

                    // Start movement thread
                    movementThread.Start();

                    // End PLC thread & SpectraCyber 
                    movementThread.Join();
                    StopReadingRFData();

                    // Stow Telescope
                    EndAppointment();

                    Console.WriteLine("[RadioTelescopeControllerManagementThread : ID=" + RadioTelescopeID.ToString() + "] Appointment completed.");
                    logger.Info("Appointment completed.");
                }
                else
                {
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
        public Appointment WaitForNextAppointment()
        {
            Appointment appt;
            while ((appt = DatabaseOperations.GetNextAppointment(RadioTelescopeID)) == null)
            {
                // Delay between checking database for new appointments
                Thread.Sleep(1000);
            }

            logger.Info("Waiting for the next appointment to be within 10 minutes.");
            TimeSpan diff;
            while ((diff = appt.StartTime - DateTime.Now).TotalMinutes > 1)
            {
                // Wait more
            }

            logger.Info("The next appointment is now within the correct timeframe.");
            return appt;
        }

        /// <summary>
        /// Ends an appointment by returning the RT to the stow position.
        /// This probably does not need to be done if an appointment is within
        /// ????? amount of minutes/hours but that can be determined later.
        /// </summary>
        public void CalibrateRadioTelescope()
        {
            RTController.CalibrateRadioTelescope();
        }

        /// <summary>
        /// Starts movement of the RT by updating the appointment status and
        /// then calling the RT controller to move the RT to the orientation
        /// it needs to go to.
        /// </summary>
        /// <param name="appt"> The appointment that is currently running. </param>
        /// <param name="orientation"> The orientation that the RT is going to. </param>
        public void StartRadioTelescope(Appointment appt)
        {
            appt.Status = AppointmentConstants.IN_PROGRESS;
            DatabaseOperations.UpdateAppointmentStatus(appt);

            Console.WriteLine("[RadioTelescopeControllerManagementThread : ID=" + RadioTelescopeID.ToString() + "] Appointment Type: " + appt.Type);

            // Loop through each minute of the appointment 
            TimeSpan length = appt.EndTime - appt.StartTime;
            for (int i = 0; i < length.TotalMinutes; i++)
            {
                // Get orientation for current datetime
                DateTime datetime = appt.StartTime.AddMinutes(i);
                NextObjective = RTController.CoordinateController.CalculateOrientation(appt, datetime);

                // Wait for datetime
                while (DateTime.Now < datetime)
                {
                    if (InterruptAppointmentFlag)
                    {
                        // Console.WriteLine("Interrupted appointment appt.Id at " + DateTime.Now.ToString());
                        break;
                    }

                    // Console.WriteLine(datetime.ToString() + " vs. " + DateTime.Now.ToString());

                    Thread.Sleep(1000);
                }

                if (InterruptAppointmentFlag)
                {
                    NextObjective = null;
                    break;
                }

                // Move to orientation
                if(NextObjective != null)
                {
                    Console.WriteLine("[RadioTelescopeControllerManagementThread : ID=" + RadioTelescopeID.ToString() + "] Moving to Next Objective: Az = " + NextObjective.Azimuth + ", El = " + NextObjective.Elevation);
                    RTController.MoveRadioTelescope(NextObjective);
                    NextObjective = null;
                }
            }

            if (InterruptAppointmentFlag)
            {
                appt.Status = AppointmentConstants.CANCELLED;
                InterruptAppointmentFlag = false;
            }
            else
            {
                appt.Status = AppointmentConstants.COMPLETED;
            }
            DatabaseOperations.UpdateAppointmentStatus(appt);
        }

        /// <summary>
        /// Ends an appointment by returning the RT to the stow position.
        /// </summary>
        public void EndAppointment()
        {
            RTController.MoveRadioTelescope(new Orientation(0, 90));
        }

        /// <summary>
        /// Calls the SpectraCyber controller to start the SpectraCyber readings.
        /// </summary>
        public void StartReadingData(Appointment appt)
        {
            var spectraCyberController = RTController.RadioTelescope.SpectraCyberController;
            spectraCyberController.SetSpectraCyberModeType(appt.SpectraCyberModeType);
            spectraCyberController.SetActiveAppointmentID(appt.Id);
            spectraCyberController.StartScan();
        }

        /// <summary>
        /// Calls the SpectraCyber controller to stop the SpectraCyber readings.
        /// </summary>
        public void StopReadingRFData()
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