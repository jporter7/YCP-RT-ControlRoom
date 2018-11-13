using ControlRoomApplication.Entities.RadioTelescope;
using ControlRoomApplication.Main;
using System;
using System.Collections.Generic;
using System.Data.Entity;

namespace ControlRoomApplication.Entities
{
    public class ControlRoom
    {
        public ControlRoom(AbstractRadioTelescope radioTelescope, AbstractSpectraCyber spectraCyber, RTDbContext dbContext)
        {
            RadioTelescope = radioTelescope;
            SpectraCyber = spectraCyber;
            Context = dbContext;
        }

        public ControlRoom()
        {

        }

        public void StartAppointment()
        {
            Appointment app = WaitingForNextAppointment();
            Coordinate coordinate = Context.Coordinates.Find(app.CoordinateId);
            Orientation orientation = new Orientation();
            orientation.Azimuth = coordinate.RightAscension;
            orientation.Elevation = coordinate.Declination;

            RadioTelescope.MoveRadioTelescope(orientation);
        }

        public Appointment WaitingForNextAppointment()
        {
            TimeSpan diff = new TimeSpan();
            Appointment app = GetNextAppointment();
            diff = app.StartTime - DateTime.Now;

            while(diff.TotalMinutes > 5)
            {
                diff = app.StartTime - DateTime.Now;
            }

            return app;
        }

        public Appointment GetNextAppointment()
        {
            Appointments = Context.Appointments;
            List<Appointment> apps = DbSetToList(Appointments);

            apps.Sort((x, y) => DateTime.Compare(y.StartTime, x.StartTime));

            return apps[0];
        }

        private List<Appointment> DbSetToList(DbSet<Appointment> appointments)
        {
            List<Appointment> apps = new List<Appointment>();

            foreach(Appointment app in appointments)
            {
                apps.Add(app);
            }

            return apps;
        }

        public AbstractRadioTelescope RadioTelescope { get; set; }
        public AbstractSpectraCyber SpectraCyber { get; set; }
        public DbSet<Appointment> Appointments { get; set; }
        public RTDbContext Context { get; set; }
    }
}