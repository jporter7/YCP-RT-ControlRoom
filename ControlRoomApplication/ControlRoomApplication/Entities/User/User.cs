using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace ControlRoomApplication.Entities
{
    [Table("user")]
    public class User
    {
        public User()
        {
        }

        public User(string firstName, string lastName, string email)
        {
            first_name = firstName;
            last_name = lastName;
            email_address = email;
        }
    
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        // not nullable
        [Column("first_name")]
        public string first_name { get; set; }

        // not nullable
        [Column("last_name")]
        public string last_name { get; set; }

        // not nullable
        [Column("email_address")]
        public string email_address { get; set; }
    }
}