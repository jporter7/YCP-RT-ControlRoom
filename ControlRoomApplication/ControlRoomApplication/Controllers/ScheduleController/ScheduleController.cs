using ControlRoomApplication.Entities;
using System;
using System.Collections.Generic;

namespace ControlRoomApplication.Controllers.ScheduleController
{
    public class ScheduleController
    {
        public ScheduleController(Schedule schedule)
        {
            Schedule = schedule;
        }

        public Appointment GetNextAppointment()
        {
            List<Appointment> appointments = (List<Appointment>) Schedule.Appointments;

            appointments.Sort((x, y) => DateTime.Compare(y.StartTime, x.StartTime));

            return appointments[0];
        }

        public Schedule Schedule { get; set; }
    }
}
