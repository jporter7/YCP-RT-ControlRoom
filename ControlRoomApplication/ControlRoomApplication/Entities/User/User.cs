using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace ControlRoomApplication.Entities.User
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