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

        public User(string firstName, string lastName, string email, NotificationTypeEnum e)
        {
            first_name = firstName;
            last_name = lastName;
            _Notification_Type = e;
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

        [Index("email_address", 1, IsUnique = true)]
        public string email_address { get; set; }

        /// <summary>
        /// The getter/setter for the status asscociated with this Appointment.
        /// This is the 
        /// </summary>
        [NotMapped]
        public NotificationTypeEnum _Notification_Type
        {
            get
            {
                return (NotificationTypeEnum)Enum.Parse(typeof(NotificationTypeEnum), notification_type);
            }
            set
            {
                this.notification_type = value.ToString();
            }
        }

        private string backingtype { get; set; }

        [Required]
        [Column("notification_type")]
        public string notification_type
        {
            get
            {
                return this.backingtype;
            }
            set
            {
                if (value == null || Enum.IsDefined(typeof(NotificationTypeEnum), value))
                {
                    this.backingtype = value;
                }
                else
                {
                    throw new InvalidCastException();
                }
            }
        }


        [Column("phone_number")]
        [StringLength(25)]
        public string phone_number { get; set; }
    }
}