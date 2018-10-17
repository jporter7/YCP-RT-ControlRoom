using ControlRoomApplication.Entities;
using ControlRoomApplication.Main;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControlRoomApplication.Controllers.AppointmentController
{
    public class AppointmentController
    {
        public AppointmentController(Appointment appointment, RTDbContext context)
        {
            Appointment = appointment;
            Context = context;
        }

        public Boolean ValidateAppointment()
        {

            return false;
        }

        public Appointment Appointment { get; set; }
        public RTDbContext Context { get; set; }
    }
}
