using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ControlRoomApplication.Entities
{
    /// <summary>
    /// This class will model the appointment structure in our system.
    /// The appointment entity will use a radio-telescope between the designated
    /// start time and end time and will be created by users.
    /// </summary>
    [Table("appointment")]
    public class Appointment : IComparable, IComparable<Appointment>
    {
        /// <summary>
        /// This is the empty constructor for the appointment entity.
        /// </summary>
        public Appointment()
        {
            coordinates = new List<Coordinate>();
            RFDatas = new List<RFData>();
            SpectraCyberConfig = new SpectraCyberConfig();
            Status = "NUDEFINED";// AppointmentStatusEnum.UNDEFINED.ToString();
            Type = "UNDEFINED";// AppointmentTypeEnum.UNDEFINED.ToString();
        }

        /// <summary>
        /// The getter/setter for the unique id field associated with this appointment.
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id")]
        public int id { get; set; }

        [Required]
        [Column("user_id")]
        public int UserId { get; set; }

        /// <summary>
        /// The getter/setter for the start time associated with this appointment.
        /// </summary>
        [Required]
        [Column("start_time")]
        public DateTime StartTime { get; set; }

        /// <summary>
        /// The getter/setter for the end time associated with this appointment.
        /// </summary>
        [Required]
        [Column("end_time")]
        public DateTime EndTime { get; set; }

        /// <summary>
        /// The getter/setter for the celestial body asscociated with this appointment.
        /// </summary>
        [Column("celestial_body_id")]
        public CelestialBody celestial_body { get; set; }
        //public int celestial_body_id { get; set; }

        /// <summary>
        /// The getter/setter for the orientation asscociated with this appointment.
        /// </summary>
        [Column("orientation_id")]
        public Orientation orientation { get; set; }
        //public int OrientationId { get; set; }

        /// <summary>
        /// The getter/setter for the coordinates asscociated with this appointment.
        /// </summary>
        [Column("coordinates")]
        public virtual ICollection<Coordinate> coordinates { get; set; }

        /// <summary>
        /// The getter/setter for the RFData asscociated with this appointment.
        /// </summary>
        [Required]
        [Column("rf_datas")]
        public virtual ICollection<RFData> RFDatas { get; set; }

        /// <summary>
        /// The getter/setter for the telescope asscociated with this appointment.
        /// </summary>
        [Required]
        [Column("telescope_id")]
        public int TelescopeId { get; set; }

        /// <summary>
        /// The getter/setter for the status asscociated with this appointment.
        /// </summary>
        [Required]
        [Column("status")]
        public string Status { get; set; }

        /// <summary>
        /// The getter/setter for the appointment type.
        /// </summary>
        [Required]
        [Column("type")]
        public string Type { get; set; }

        /// <summary>
        /// The getter/setter for the SpectraCyberConfig type.
        /// </summary>
        //[Column("spectracyber_config_id")]
        [NotMapped]
        public SpectraCyberConfig SpectraCyberConfig { get; set; }

        /// <summary>
        /// Compares the current appointment to another object and it
        /// throws an error if the other object is not an appointment.
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
                throw new ArgumentException("A appointment object is required for comparison.", "obj");
            }
            return CompareTo(other);
        }

        /// <summary>
        /// Compares the current appointment to another appointment  
        /// and it checks if the other appointment is null
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
        /// Compares one appointment to another appointment,   
        /// it checks if the other Appointments are identical, and 
        /// it checks if the left appointment is null 
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
        /// Checks if the current appointment is Equal to another appointment  
        /// and it checks if the other appointment is null
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
        /// Returns the HashCode of the appointment's StartTime  
        /// (Implemention of the IComparable Interface)
        /// </summary>
        public override int GetHashCode()
        {
            return StartTime.GetHashCode();
        }

        /// <summary>
        /// Overrides the 'equal' operator, including checks 
        /// for whether either appointment is null
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