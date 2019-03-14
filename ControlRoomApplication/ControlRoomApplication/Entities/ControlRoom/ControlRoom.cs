using ControlRoomApplication.Controllers.RadioTelescopeControllers;
using ControlRoomApplication.Main;
using System.Collections.Generic;
using System.Linq;

namespace ControlRoomApplication.Entities
{
    public class ControlRoom
    {
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
                foreach (Appointment app in value)
                {
                    Context.Appointments.Add(app);
                    Context.SaveChanges();
                }
            }
        }

        public ControlRoom(RadioTelescopeController controller, RTDbContext dbContext)
        {
            RadioTelescopeController = controller;
            Context = dbContext;
        }
    }
}