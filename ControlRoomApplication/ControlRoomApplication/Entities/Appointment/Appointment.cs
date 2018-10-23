using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ControlRoomApplication.Entities
{
    /// <summary>
    /// This class will model the Appointment structure in our system.
    /// The Appointment entity will use a radio-telescope between the designated
    /// start time and end time and will be created by users.
    /// </summary>
    [Table("appointment")]
    public class Appointment
    {
        /// <summary>
        /// This is the empty constructor for the Appointment entity.
        /// </summary>
        public Appointment()
        {
            Id = -9999;
            StartTime = new DateTime();
            EndTime = new DateTime();
        }

        /// <summary>
        /// The getter/setter for the unique Id field associated with this Appointment.
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [Column("user_id")]
        public int UserId { get; set; }

        /// <summary>
        /// The getter/setter for the start time associated with this Appointment.
        /// </summary>
        [Required]
        [Column("start_time")]
        public DateTime StartTime { get; set; }

        /// <summary>
        /// The getter/setter for the end time associated with this Appointment.
        /// </summary>
        [Required]
        [Column("end_time")]
        public DateTime EndTime { get; set; }
    }
}
