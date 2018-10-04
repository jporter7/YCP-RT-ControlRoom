using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ControlRoomApplication
{
    [Table("user")]
    public class User
    {
        public User()
        {

        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id")]
        public int Id { get; set; }

        [MaxLength(Constants.MAX_USERNAME_LENGTH)]
        [Required]
        [Column("username")]
        public string Username { get; set; }

        // This establishes the User relationship to Appointments
        public virtual List<Appointment> Appointments { get; set; }
    }
}
