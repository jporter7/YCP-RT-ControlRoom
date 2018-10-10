using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace ControlRoomApplication.Entities
{
    public class Schedule
    {
        public Schedule()
        {
            CurrentDateTime = new DateTime();
            Appointments = new Collection<Appointment>();
        }

        public Schedule(ICollection<Appointment> appointments)
        {
            Appointments = appointments;
        }

        public DateTime CurrentDateTime { get; set; }
        public ICollection<Appointment> Appointments { get; set; }
    }
}
