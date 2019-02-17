using ControlRoomApplication.Controllers.RadioTelescopeControllers;
using ControlRoomApplication.Main;
using System.Collections.Generic;
using System.Linq;

namespace ControlRoomApplication.Entities
{
    public class ControlRoom
    {
        public ControlRoom( RadioTelescopeController controller, RTDbContext dbContext)
        {
            RadioTelescopeController = controller;
            Context = dbContext;
        }

        public ControlRoom()
        {
            // By default no radio telescope will be added to the radioTelescopeController
            RadioTelescopeController =  new RadioTelescopeController();
            Context = new RTDbContext();
        }

        public RadioTelescopeController RadioTelescopeController { get; set; }
        public RTDbContext Context { get; set; }
        public List<Appointment> Appointments
        {
            get
            {
                return Context.Appointments.ToList();
            }
            set
            {
                foreach(Appointment appt in value)
                {
                    Context.Appointments.Add(appt);
                    Context.SaveChanges();
                }
            }
        }
    }
}