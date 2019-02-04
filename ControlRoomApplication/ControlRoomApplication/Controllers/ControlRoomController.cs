using ControlRoomApplication.Constants;
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
        }

        /// <summary>
        /// Starts the next appointment based on chronological order.
        /// </summary>
        public void StartAppointment()
        {
            Appointment app = WaitingForNextAppointment();
            /// Could break here because of coordinate id relationship ????
            Coordinate coordinate = CRoom.Context.Coordinates.Find(app.CoordinateId);
            Orientation orientation = new Orientation();

            // This stuff should actually be some conversion between right ascension/decl & az/el
            orientation.Azimuth = coordinate.RightAscension;
            orientation.Elevation = coordinate.Declination;
            // but that can wait until later on.

            logger.Info($"Calculated starting orientation ({orientation.Azimuth}, {orientation.Elevation}). Starting appointment.");
            Thread movementThread = new Thread(() => StartRadioTelescope(app, orientation));
            movementThread.Start();

            // Start SpectraCyber thread

            // End PLC & SpectraCyber thread
            movementThread.Join();

            logger.Info("Appointment completed.");
        }

        /// <summary>
        /// Waits for the next chronological appointment's start time to be less than 10 minutes
        /// from the current time of day. Once we are 10 minutes from the appointment's start time
        /// we should begin operations such as calibration.
        /// </summary>
        /// <returns> An appointment object that is next in chronological order and is less than 10 minutes away from starting. </returns>
        public Appointment WaitingForNextAppointment()
        {
            TimeSpan diff = new TimeSpan();
            Appointment app = GetNextAppointment();
            diff = app.StartTime - DateTime.Now;

            logger.Info("Waiting for the next appointment to be within 10 minutes.");
            while (diff.TotalDays > 10)
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
            logger.Info("Retrieving list of appointments.");
            var list = new List<DateTime>();

            foreach (Appointment app in CRoom.Context.Appointments)
            {
                list.Add(app.StartTime);
            }

            try
            {
                list.OrderBy(x => Math.Abs((x - DateTime.Now).Ticks)).First();
                list.RemoveAll(x => x < DateTime.Now);
 
                logger.Info("Appointment list sorted. Starting to retrieve the next chronological appointment.");
                foreach(Appointment app in CRoom.Context.Appointments)
                {
                    if (app.StartTime == list[0])
                    {
                        return app;
                    }
                }
            }
            catch (ArgumentException e)
            {
                logger.Error("There was an error with the size of the list of appointments.");
            }


            return CRoom.Context.Appointments.Find(0);
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
            CRoom.Controller.MoveRadioTelescope(orientation);
            CRoom.RadioTelescope.CurrentOrientation = orientation;
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
            Coordinate stow = new Coordinate();
            stow.RightAscension = 0;
            stow.Declination = 90;
            CRoom.RadioTelescope.PlcController.MoveTelescope(CRoom.RadioTelescope.Plc, stow);
        }

        /// <summary>
        /// Calls the SpectraCyber controller to start the SpectraCyber readings.
        /// </summary>
        public void StartReadingData()
        {
            CRoom.RadioTelescope.SpectraCyberController.StartScan();
        }

        /// <summary>
        /// Calls the SpectraCyber controller to stop the SpectraCyber readings.
        /// </summary>
        public void StopReadingRFData()
        {
            CRoom.RadioTelescope.SpectraCyberController.StopScan();
        }

        public ControlRoom CRoom { get; set; }
        private static readonly log4net.ILog logger =
            log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
    }
}