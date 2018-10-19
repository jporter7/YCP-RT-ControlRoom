using ControlRoomApplication.Entities;
using ControlRoomApplication.Main;
using System;
using AASharp;

namespace ControlRoomApplication.Controllers.AppointmentController
{
    public class AppointmentController
    {
        public AppointmentController(Appointment appointment, RTDbContext context)
        {
            Appointment = appointment;
            Context = context;
        }

        public bool GetAppointmentCoordinates()
        {
            return false;
        }

        public Appointment Appointment { get; set; }
        public RTDbContext Context { get; set; }
    }
}
