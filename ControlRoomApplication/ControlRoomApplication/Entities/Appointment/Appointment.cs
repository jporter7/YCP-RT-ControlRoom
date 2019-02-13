﻿using System;
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
    public class Appointment : IComparable, IComparable<Appointment>
    {
        /// <summary>
        /// This is the empty constructor for the Appointment entity.
        /// </summary>
        public Appointment()
        {

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

        /// <summary>
        /// The getter/setter for the coordinate asscociated with this Appointment.
        /// </summary>
        [Required]
        [Column("coordinate_id")]
        public int CoordinateId { get; set; }

        [Required]
        [Column("telescope_id")]
        public int TelescopeId { get; set; }

        //[Required]
        //[Column("status")]
        //[EnumDataType(typeof(AppointmentStatusEnum))]
        //public AppointmentStatusEnum Status { //get; set;
        //    get { return Status; }
        //    set { Status = (AppointmentStatusEnum)value; }
        //}

        [Required]
        [Column("status")]
        public string Status { get; set; }

        /// <summary>
        /// Compares the current Appointment to another object and it
        /// throws an error if the other object is not an Appointment.
        /// (Implemention of the IComparable Interface)
        /// </summary>
        public int CompareTo(object obj)
        {
            if (obj == null)
            {
                return 1;
            }
            Appointment other = obj as Appointment; // avoid double casting
            if (other == null)
            {
                throw new ArgumentException("A Appointment object is required for comparison.", "obj");
            }
            return this.CompareTo(other);
        }

        /// <summary>
        /// Compares the current Appointment to another Appointment  
        /// and it checks if the other Appointment is null
        /// (Implemention of the IComparable Interface)
        /// </summary>
        public int CompareTo(Appointment other)
        {
            if (object.ReferenceEquals(other, null))
            {
                return 1;
            }
            // Use the DateTime Compare method to compare StartTimes
            return DateTime.Compare(this.StartTime, other.StartTime);
        }

        /// <summary>
        /// Compares one Appointment to another Appointment,   
        /// it checks if the other Appointments are identical, and 
        /// it checks if the left Appointment is null 
        /// (Implemention of the IComparable Interface)
        /// </summary>
        public static int Compare(Appointment left, Appointment right)
        {
            if (object.ReferenceEquals(left, right))
            {
                return 0;
            }
            if (object.ReferenceEquals(left, null))
            {
                return -1;
            }
            return left.CompareTo(right);
        }

        /// <summary>
        /// Checks if the current Appointment is Equal to another Appointment  
        /// and it checks if the other Appointment is null
        /// (Implemention of the IComparable Interface)
        /// </summary>
        public override bool Equals(object obj)
        {
            Appointment other = obj as Appointment; //avoid double casting
            if (object.ReferenceEquals(other, null))
            {
                return false;
            }
            return this.CompareTo(other) == 0;
        }

        /// <summary>
        /// Returns the HashCode of the Appointment's StartTime  
        /// (Implemention of the IComparable Interface)
        /// </summary>
        public override int GetHashCode()
        {
            return this.StartTime.GetHashCode();
        }

        /// <summary>
        /// Overrides the 'equal' operator, including checks 
        /// for whether either Appointment is null
        /// (Implemention of the IComparable Interface)
        /// </summary>
        public static bool operator ==(Appointment left, Appointment right)
        {
            if (object.ReferenceEquals(left, null))
            {
                return object.ReferenceEquals(right, null);
            }
            return left.Equals(right);
        }

        /// <summary>
        /// Overrides the 'not equal' operator
        /// (Implemention of the IComparable Interface)
        /// </summary>
        public static bool operator !=(Appointment left, Appointment right)
        {
            return !(left == right);
        }

        /// <summary>
        /// Overrides the 'less than' operator
        /// (Implemention of the IComparable Interface)
        /// </summary>
        public static bool operator <(Appointment left, Appointment right)
        {
            return (Compare(left, right) < 0);
        }

        /// <summary>
        /// Overrides the 'greater than' operator,
        /// (Implemention of the IComparable Interface)
        /// </summary>
        public static bool operator >(Appointment left, Appointment right)
        {
            return (Compare(left, right) > 0);
        }
    }
}