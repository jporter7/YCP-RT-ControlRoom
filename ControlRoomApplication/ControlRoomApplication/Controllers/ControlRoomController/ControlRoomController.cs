using System;
using System.Collections.Generic;
using System.Threading;
using ControlRoomApplication.Constants;
using ControlRoomApplication.Database.Operations;
using ControlRoomApplication.Entities;

namespace ControlRoomApplication.Controllers
{
    public class ControlRoomController
    {
        public ControlRoom CRoom { get; set; }
        private static readonly log4net.ILog logger =
            log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public ControlRoomController(ControlRoom controlRoom)
        {
            CRoom = controlRoom;
        }

        /// <summary>
        /// Starts the next appointment based on chronological order.
        /// </summary>
        public void Start()
        {
            while (true)
            {
                Appointment appt = WaitingForNextAppointment();
                Dictionary<DateTime, Orientation> orientations = CRoom.RadioTelescopeController.CoordinateController.CalculateCoordinates(appt);

                if (orientations.Count > 0)
                {
                    Console.WriteLine("Starting appointment...");

                    // Calibrate telescope
                    CalibrateRadioTelescope();
                    
                    // Start movement thread
                    Thread movementThread = new Thread(() => StartRadioTelescope(appt, orientations));
                    movementThread.Start();
                    // Start SpectraCyber
                    StartReadingData(appt);
                    
                    // End PLC thread & SpectraCyber 
                    movementThread.Join();
                    StopReadingRFData();
                    
                    // Stow Telescope
                    EndAppointment();

                    Console.WriteLine("Appointment completed.");
                    logger.Info("Appointment completed.");
                }
                else
                {
                    logger.Info("Appointment coordinate is null.");
                }
                
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
            CRoom.RadioTelescopeController.CalibrateRadioTelescope();
        }

        /// <summary>
        /// Starts movement of the RT by updating the appointment status and
        /// then calling the RT controller to move the RT to the orientation
        /// it needs to go to. This will have to be refactored to work with a 
        /// set of Orientations as opposed to one orientation.
        /// </summary>
        /// <param name="appt"> The appointment that is currently running. </param>
        /// <param name="orientation"> The orientation that the RT is going to. </param>
        public void StartRadioTelescope(Appointment appt, Dictionary<DateTime, Orientation> orientations)
        {
            appt.Status = AppointmentConstants.IN_PROGRESS;
            CRoom.Context.SaveChanges();
            foreach(DateTime datetime in orientations.Keys)
            {
                while(DateTime.Now < datetime)
                {
                    // wait for timestamp
                    // Console.WriteLine(datetime.ToString() + " vs. " + DateTime.Now.ToString());
                }
                Orientation orientation = orientations[datetime];
                CRoom.RadioTelescopeController.MoveRadioTelescope(orientation);
            }
            appt.Status = AppointmentConstants.COMPLETED;
            CRoom.Context.SaveChanges();
        }

        /// <summary>
        /// Ends an appointment by returning the RT to the stow position.
        /// This probably does not need to be done if an appointment is within
        /// ????? amount of minutes/hours but that can be determined later.
        /// </summary>
        public void EndAppointment()
        {
            Orientation stow = new Orientation(0, 90);
            CRoom.RadioTelescopeController.MoveRadioTelescope(stow);
        }

        /// <summary>
        /// Calls the SpectraCyber controller to start the SpectraCyber readings.
        /// </summary>
        public void StartReadingData(Appointment appt)
        {
            var spectraCyberController = CRoom.RadioTelescopeController.RadioTelescope.SpectraCyberController;
            spectraCyberController.SetSpectraCyberModeType(appt.SpectraCyberModeType);
            spectraCyberController.SetActiveAppointmentID(appt.Id);
            spectraCyberController.StartScan();
        }

        /// <summary>
        /// Calls the SpectraCyber controller to stop the SpectraCyber readings.
        /// </summary>
        public void StopReadingRFData()
        {
            var spectraCyberController = CRoom.RadioTelescopeController.RadioTelescope.SpectraCyberController;
            spectraCyberController.StopScan();
            spectraCyberController.RemoveActiveAppointmentID();
            spectraCyberController.SetSpectraCyberModeType(SpectraCyberModeTypeEnum.UNKNOWN);
        }
    }
}