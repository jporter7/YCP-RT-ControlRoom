using ControlRoomApplication.Constants;
using ControlRoomApplication.Controllers.AASharpControllers;
using ControlRoomApplication.Database.Operations;
using ControlRoomApplication.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace ControlRoomApplication.Controllers
{
    public class ControlRoomController
    {
        public ControlRoomController(ControlRoom controlRoom)
        {
            CRoom = controlRoom;
            coordinateController = new CoordinateCalculationController();
        }

        /// <summary>
        /// Starts the next appointment based on chronological order.
        /// </summary>
        public void Start()
        {
            while (true)
            {
                Appointment app = WaitingForNextAppointment();
                Coordinate coordinate;
                switch (app.Type)
                {
                    case ("POINT"):
                        coordinate = CRoom.Context.Coordinates.ToList()[0];
                        break;
                    default:
                        coordinate = CRoom.Context.Coordinates.ToList()[0];
                        break;
                }
                

                if (coordinate != null)
                {
                    Orientation orientation = coordinateController.CoordinateToOrientation(coordinate, DateTime.Now);
                    logger.Info($"Calculated starting orientation ({orientation.Azimuth}, {orientation.Elevation}). Starting appointment.");

                    // Calibrate telescope
                    CalibrateRadioTelescope();

                    // Start movement thread
                    Thread movementThread = new Thread(() => StartRadioTelescope(app, orientation));
                    movementThread.Start();

                    // Start SpectraCyber
                    //StartReadingData(app);

                    // End PLC thread & SpectraCyber 
                    movementThread.Join();
                    //StopReadingRFData();

                    // Stow Telescope
                    EndAppointment();

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
            Appointment app = null;
            while (app == null)
            {
                app = GetNextAppointment();
                if (app == null)
                {
                    Thread.Sleep(5000); // delay between checking database for new appointments
                }
            }

            TimeSpan diff = app.StartTime - DateTime.Now;

            logger.Info("Waiting for the next appointment to be within 10 minutes.");
            while (diff.TotalMinutes > 10)
            {
                diff = app.StartTime - DateTime.Now;
            }

            logger.Info("The next appointment is now within the correct timeframe.");
            return app;
        }

        /// <summary>
        /// Gets the next appointment by chronological time.
        /// </summary>
        /// <returns> An appointment object that is the next in the database in chronological order. </returns>
        public Appointment GetNextAppointment()
        {
            Appointment appointment = null;
            logger.Debug("Retrieving list of appointments.");
            List<Appointment> appointments = DatabaseOperations.GetListOfAppointments();
            appointments.Sort();
            appointments.RemoveAll(x => x.StartTime < DateTime.Now);
            logger.Debug("Appointment list sorted. Starting to retrieve the next chronological appointment.");
            if (appointments.Count > 0)
            {
                appointment = appointments[0];
                appointment = (appointment.Status == AppointmentConstants.COMPLETED) ? null : appointment;
            }
            else
            {
                logger.Debug("No appointments found");
            }

            return appointment;
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
        /// <param name="app"> The appointment that is currently running. </param>
        /// <param name="orientation"> The orientation that the RT is going to. </param>
        public void StartRadioTelescope(Appointment app, Orientation orientation)
        {
            app.Status = AppointmentConstants.IN_PROGRESS;
            CRoom.Context.SaveChanges();
            CRoom.RadioTelescopeController.MoveRadioTelescope(orientation);
            CRoom.RadioTelescopeController.RadioTelescope.CurrentOrientation = orientation;
            app.Status = AppointmentConstants.COMPLETED;
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
        public void StartReadingData(Appointment app)
        {
            CRoom.RadioTelescopeController.RadioTelescope.SpectraCyberController.BringUp(app.Id);
            CRoom.RadioTelescopeController.RadioTelescope.SpectraCyberController.StartScan();
        }

        /// <summary>
        /// Calls the SpectraCyber controller to stop the SpectraCyber readings.
        /// </summary>
        public void StopReadingRFData()
        {
            CRoom.RadioTelescopeController.RadioTelescope.SpectraCyberController.StopScan();
            CRoom.RadioTelescopeController.RadioTelescope.SpectraCyberController.BringDown();
        }

        public ControlRoom CRoom { get; set; }
        public CoordinateCalculationController coordinateController { get; set; }
        private static readonly log4net.ILog logger =
            log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
    }
}