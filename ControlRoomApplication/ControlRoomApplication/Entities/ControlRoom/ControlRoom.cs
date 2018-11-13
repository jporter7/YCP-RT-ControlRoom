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
        public List<Appointment> Appointments
        {
            get
            {
                List<Appointment> apps = new List<Appointment>();

                foreach (Appointment app in Context.Appointments)
                {
                    apps.Add(app);
                }

                return apps;
            }
            set
            {
                Appointments = value;
            }
        }
        public RTDbContext Context { get; set; }
    }
}