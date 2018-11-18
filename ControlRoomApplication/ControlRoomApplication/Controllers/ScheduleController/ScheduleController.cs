using ControlRoomApplication.Entities;
using ControlRoomApplication.Main;
using System;
using System.Collections.Generic;
using System.Data.Entity;

namespace ControlRoomApplication.Controllers.ScheduleController
{
    public class ScheduleController
    {
        public ScheduleController(Schedule schedule)
        {
            Schedule = schedule;
        }

        /// <summary>
        /// This method returns the next Appointment chronologically by sorting the list
        /// of Appointments by their StartTime.
        /// </summary>
        /// <returns> The next Appointment chronologically. </returns>
        public Appointment GetNextAppointment()
        {
            List<Appointment> appointments = (List<Appointment>) Schedule.Appointments;

            appointments.Sort((x, y) => DateTime.Compare(y.StartTime, x.StartTime));

            return appointments[0];
        }

        public void StartAppointment()
        {
            // From here, should be calling some PLC controller function(s) to
            // begin the appointment's commands.
        }

        public Schedule Schedule { get; set; }
        public RTDbContext Context { get; set; }
    }
}
