using System;
using System.Collections.Generic;
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
            Coordinates = new List<Coordinate>();
            RFDatas = new List<RFData>();
            SpectraCyberConfig = new SpectraCyberConfig();
            Status = AppointmentStatusEnum.UNDEFINED;
            Type = AppointmentTypeEnum.UNDEFINED;
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
        /// The getter/setter for the celestial body asscociated with this Appointment.
        /// </summary>
        [Column("celestial_body")]
        public CelestialBody CelestialBody { get; set; }

        /// <summary>
        /// The getter/setter for the Orientation asscociated with this Appointment.
        /// </summary>
        [Column("orientation")]
        public Orientation Orientation { get; set; }

        /// <summary>
        /// The getter/setter for the coordinates asscociated with this Appointment.
        /// </summary>
        [Required]
        [Column("coordinates")]
        public virtual ICollection<Coordinate> Coordinates { get; set; }

        /// <summary>
        /// The getter/setter for the RFData asscociated with this Appointment.
        /// </summary>
        [Required]
        [Column("rf_datas")]
        public virtual ICollection<RFData> RFDatas { get; set; }

        /// <summary>
        /// The getter/setter for the telescope asscociated with this Appointment.
        /// </summary>
        [Required]
        [Column("telescope_id")]
        public int TelescopeId { get; set; }

        /// <summary>
        /// The getter/setter for the status asscociated with this Appointment.
        /// </summary>
        [Required]
        [Column("status")]
        public AppointmentStatusEnum Status { get; set; }

        /// <summary>
        /// The getter/setter for the Appointment type.
        /// </summary>
        [Required]
        [Column("type")]
        public AppointmentTypeEnum Type { get; set; }

        /// <summary>
        /// The getter/setter for the SpectraCyberConfig type.
        /// </summary>
        [Required]
        [Column("spectracyber_config")]
        public SpectraCyberConfig SpectraCyberConfig { get; set; }

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
            return CompareTo(other);
        }

        /// <summary>
        /// Compares the current Appointment to another Appointment  
        /// and it checks if the other Appointment is null
        /// (Implemention of the IComparable Interface)
        /// </summary>
        public int CompareTo(Appointment other)
        {
            if (ReferenceEquals(other, null))
            {
                return 1;
            }
            // Use the DateTime Compare method to compare StartTimes
            return DateTime.Compare(StartTime, other.StartTime);
        }

        /// <summary>
        /// Compares one Appointment to another Appointment,   
        /// it checks if the other Appointments are identical, and 
        /// it checks if the left Appointment is null 
        /// (Implemention of the IComparable Interface)
        /// </summary>
        public static int Compare(Appointment left, Appointment right)
        {
            if (ReferenceEquals(left, right))
            {
                return 0;
            }
            if (ReferenceEquals(left, null))
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
            if (ReferenceEquals(other, null))
            {
                return false;
            }
            return CompareTo(other) == 0;
        }

        /// <summary>
        /// Returns the HashCode of the Appointment's StartTime  
        /// (Implemention of the IComparable Interface)
        /// </summary>
        public override int GetHashCode()
        {
            return StartTime.GetHashCode();
        }

        /// <summary>
        /// Overrides the 'equal' operator, including checks 
        /// for whether either Appointment is null
        /// (Implemention of the IComparable Interface)
        /// </summary>
        public static bool operator ==(Appointment left, Appointment right)
        {
            if (ReferenceEquals(left, null))
            {
                return ReferenceEquals(right, null);
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