﻿using System;
using System.Threading;
using System.Collections.Generic;
using ControlRoomApplication.Constants;
using ControlRoomApplication.Entities;
using ControlRoomApplication.Entities.RadioTelescope;
using ControlRoomApplication.Controllers.RadioTelescopeControllers;
using ControlRoomApplication.Database.Operations;
using ControlRoomApplication.Main;

namespace ControlRoomApplication.Controllers
{
    public class RadioTelescopeControllerManagementThread
    {
        private static int NumInstances = 0;

        public RadioTelescopeController RTController { get; private set; }
        private RTDbContext Context;
        private Thread ManagementThread;
        private Mutex ManagementMutex;
        private bool KeepThreadAlive;

        private Orientation NextObjective;
        private bool InterruptAppointmentFlag;

        // This is a temporary addition, this should just be synced with the DB's index
        private int DebugRTControllerIndex;

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

        public RadioTelescopeControllerManagementThread(RadioTelescopeController controller, RTDbContext context)
        {
            RTController = controller;
            Context = context;

            ManagementThread = new Thread(new ThreadStart(SpinRoutine));
            ManagementThread.Name = "RadioTelescopeControllerManagementThread overarching management thread";

            ManagementMutex = new Mutex();
            KeepThreadAlive = false;
            NextObjective = null;
            InterruptAppointmentFlag = false;

            DebugRTControllerIndex = NumInstances++;
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

                Appointment NextAppointment = WaitingForNextAppointment();
                Dictionary<DateTime, Orientation> ObjectiveOrientationAtDateTimePairs = RTController.CoordinateController.CalculateCoordinates(NextAppointment);

                if (ObjectiveOrientationAtDateTimePairs.Count > 0)
                {
                    Console.WriteLine("[RadioTelescopeControllerManagementThread] Starting appointment [" + DebugRTControllerIndex + "]...");

                    // Calibrate telescope
                    CalibrateRadioTelescope();

                    // Create movement thread
                    Thread AppointmentMovementThread = new Thread(() => PerformAppointment(NextAppointment, ObjectiveOrientationAtDateTimePairs))
                    {
                        Name = "RadioTelescopeControllerManagementThread next appointment movement thread"
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

                    Console.WriteLine("[RadioTelescopeControllerManagementThread] Appointment completed [" + DebugRTControllerIndex + "].");
                    logger.Info("Appointment completed.");
                }
                else
                {
                    Console.WriteLine("[RadioTelescopeControllerManagementThread] Appointment does not have an orientation associated with it [" + DebugRTControllerIndex + "].");
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
        public Appointment WaitingForNextAppointment()
        {
            Appointment appt = null;
            while (appt == null)
            {
                appt = DatabaseOperations.GetNextAppointment();
                if (appt == null)
                {
                    Thread.Sleep(5000); // delay between checking database for new appointments
                }
            }

            TimeSpan diff = appt.StartTime - DateTime.Now;

            logger.Info("Waiting for the next appointment to be within 10 minutes.");
            while (diff.TotalMinutes > 1)
            {
                diff = appt.StartTime - DateTime.Now;
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
        /// Starts movement of the RT by updating the appointment status and then calling the RT controller to move the RT to the orientation it needs to go to.
        /// </summary>
        /// <param name="NextAppointment"> The appointment that is currently running. </param>
        /// <param name="ObjectiveOrientationAtDateTimePairs"> The orientations that the RT is going to with corresponding date/times that it should start. </param>
        public void PerformAppointment(Appointment NextAppointment, Dictionary<DateTime, Orientation> ObjectiveOrientationAtDateTimePairs)
        {
            NextAppointment.Status = AppointmentConstants.IN_PROGRESS;
            Context.SaveChanges();

            foreach (DateTime MoveStartTime in ObjectiveOrientationAtDateTimePairs.Keys)
            {
                NextObjective = ObjectiveOrientationAtDateTimePairs[MoveStartTime];
                
                while (DateTime.Now < MoveStartTime)
                {
                    if (InterruptAppointmentFlag)
                    {
                        // Console.WriteLine("Interrupted appointment appt.Id at " + DateTime.Now.ToString());
                        break;
                    }

                    // wait for timestamp
                    // Console.WriteLine(datetime.ToString() + " vs. " + DateTime.Now.ToString());

                    Thread.Sleep(10);
                }

                if (InterruptAppointmentFlag)
                {
                    break;
                }

                RTController.MoveRadioTelescope(NextObjective);
            }

            NextObjective = null;

            if (InterruptAppointmentFlag)
            {
                NextAppointment.Status = AppointmentConstants.CANCELLED;
                InterruptAppointmentFlag = false;
            }
            else
            {
                NextAppointment.Status = AppointmentConstants.COMPLETED;
            }

            Context.SaveChanges();
        }

        /// <summary>
        /// Ends an appointment by returning the RT to the stow position.
        /// This probably does not need to be done if an appointment is within
        /// ????? amount of minutes/hours but that can be determined later.
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

        public static int IndexOf(List<RadioTelescopeControllerManagementThread> ThreadList, AbstractRadioTelescope RadioTelescope)
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
