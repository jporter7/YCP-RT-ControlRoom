using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ControlRoomApplication
{
    [Table("Appointments")]
    public class Appointment
    {
        public Appointment()
        {

        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        //[Required]
        //[Column("user_id")]
        //public virtual int UserId { get; set; }

        [Required]
        [Column("start_time")]
        public DateTime StartTime { get; set; }

        [Required]
        [Column("end_time")]
        public DateTime EndTime { get; set; }
    }
}
