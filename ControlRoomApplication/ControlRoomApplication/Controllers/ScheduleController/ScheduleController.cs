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

        public Schedule Schedule { get; set; }
    }
}
