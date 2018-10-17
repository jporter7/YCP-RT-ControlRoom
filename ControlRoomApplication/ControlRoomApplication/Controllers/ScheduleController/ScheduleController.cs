using ControlRoomApplication.Entities;

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
