using System.Collections.Generic;
using System.Linq;
using ControlRoomApplication.Controllers.RadioTelescopeControllers;
using ControlRoomApplication.Main;

namespace ControlRoomApplication.Entities
{
    public class ControlRoom
    {
        public RadioTelescopeController RadioTelescopeController { get; set; }
        //public RTDbContext Context { get; set; }
        //public List<Appointment> Appointments
        //{
        //    get
        //    {
        //        return Context.Appointments.ToList();
        //    }
        //    set
        //    {
        //        foreach (Appointment appt in value)
        //        {
        //            Context.Appointments.Add(appt);
        //            Context.SaveChanges();
        //        }
        //    }
        //}

        public ControlRoom(RadioTelescopeController controller)
        {
            RadioTelescopeController = controller;
            //Context = dbContext;
        }
    }
}