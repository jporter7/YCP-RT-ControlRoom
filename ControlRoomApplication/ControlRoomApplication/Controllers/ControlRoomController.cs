using ControlRoomApplication.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControlRoomApplication.Controllers
{
    public class ControlRoomController
    {
        public ControlRoomController(ControlRoom controlRoom)
        {
            CRoom = controlRoom;
        }

        public void StartAppointment()
        {
            Appointment app = WaitingForNextAppointment();
            Coordinate coordinate = CRoom.Context.Coordinates.Find(app.CoordinateId);
            Orientation orientation = new Orientation();

            // This stuff should actually be some conversion between right ascension/decl & az/el
            orientation.Azimuth = coordinate.RightAscension;
            orientation.Elevation = coordinate.Declination;
            // but that can wait until later on.

            CRoom.RadioTelescope.MoveRadioTelescope(orientation);
        }

        public Appointment WaitingForNextAppointment()
        {
            TimeSpan diff = new TimeSpan();
            Appointment app = GetNextAppointment();
            diff = app.StartTime - DateTime.Now;

            while (diff.TotalMinutes > 5)
            {
                diff = app.StartTime - DateTime.Now;
            }

            return app;
        }

        public Appointment GetNextAppointment()
        {
            CRoom.Appointments.Sort((x, y) => DateTime.Compare(y.StartTime, x.StartTime));

            return CRoom.Appointments[0];
        }

        public ControlRoom CRoom { get; set; }
    }
}
