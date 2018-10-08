using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace ControlRoomApplication.Entities.Schedule
{
    [Table("Schedule")]
    public class Schedule
    {
        public Schedule()
        {

        }

        public virtual ICollection<Appointment> Appointments { get; set; }
    }
}
