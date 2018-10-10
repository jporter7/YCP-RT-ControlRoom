namespace ControlRoomApplication.Entities
{
    public class ControlRoom
    {
        public ControlRoom()
        {
            Schedule = new Schedule();
        }

        public ControlRoom(Schedule schedule)
        {
            Schedule = schedule;
        }

        public Schedule Schedule { get; set; }
    }
}
