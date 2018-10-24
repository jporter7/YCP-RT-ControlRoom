using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

        [MaxLength(100)]
        [Required]
        [Column("username")]
        public string Username { get; set; }
        // This establishes the User relationship to Appointments
        public virtual Collection<Appointment> Appointments { get; set; }
    }
}