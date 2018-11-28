using ControlRoomApplication.Constants;
using ControlRoomApplication.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

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
            app.Status = AppointmentConstants.IN_PROGRESS;
            CRoom.Context.SaveChanges();
            CRoom.Controller.MoveRadioTelescope(orientation);
            app.Status = AppointmentConstants.COMPLETED;
            CRoom.Context.SaveChanges();
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
            logger.Info("Retrieving list of appointments.");
            var list = new List<DateTime>();
            foreach(Appointment app in CRoom.Context.Appointments)
            {
                list.Add(app.StartTime);
            }

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

            return CRoom.Context.Appointments.Find(0);
        }

        public ControlRoom CRoom { get; set; }
        private static readonly log4net.ILog logger =
            log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
    }
}