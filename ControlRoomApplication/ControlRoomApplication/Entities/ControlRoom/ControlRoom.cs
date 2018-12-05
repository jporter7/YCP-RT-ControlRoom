using ControlRoomApplication.Constants;
using ControlRoomApplication.Controllers.RadioTelescopeControllers;
using ControlRoomApplication.Entities.RadioTelescope;
using ControlRoomApplication.Main;
using System.Collections.Generic;
using System.Data.Entity;

namespace ControlRoomApplication.Entities
{
    public class ControlRoom
    {
        public ControlRoom(AbstractRadioTelescope radioTelescope, RTDbContext dbContext)
        {
            RadioTelescope = radioTelescope;
            Controller = new RadioTelescopeController(RadioTelescope);
            Context = dbContext;
        }

        public ControlRoom()
        {
            RadioTelescope = new ScaleRadioTelescope();
            Context = new RTDbContext(GenericConstants.LOCAL_DATABASE_NAME);
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
        public RadioTelescopeController Controller { get; set; }
        public List<Appointment> Appointments
        {
            get
            {
                return DbSetToList(Context.Appointments);
            }
            set
            {
                Appointments = value;
            }
        }
        public RTDbContext Context { get; set; }
    }
}