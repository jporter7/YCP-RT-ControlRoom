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
            _Status = AppointmentStatusEnum.UNDEFINED;
            _Type = AppointmentTypeEnum.UNDEFINED;
        }

        /// <summary>
        /// The getter/setter for the unique Id field associated with this Appointment.
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        /// <summary>
        /// The getter/setter for the user id associated with this Appointment.
        /// </summary>
        public int user_id { get; set; }
        [ForeignKey("user_id")]
        public virtual User User { get; set; }



        /// <summary>
        /// The getter/setter for the start time associated with this Appointment.
        /// </summary>
        [Required]
        [Column("start_time")]
        public DateTime start_time { get; set; }

        /// <summary>
        /// The getter/setter for the end time associated with this Appointment.
        /// </summary>
        [Required]
        [Column("end_time")]
        public DateTime end_time { get; set; }

        /// <summary>
        /// The getter/setter for the celestial body asscociated with this Appointment.
        /// </summary>
        public int celestial_body_id { get; set; }
        [ForeignKey("celestial_body_id")]
        public virtual CelestialBody CelestialBody { get; set; }

        /// <summary>
        /// The getter/setter for the Orientation asscociated with this Appointment.
        /// </summary>
        public int? orientation_id { get; set; }
        [ForeignKey("orientation_id")]
        public virtual Orientation Orientation { get; set; }

        /// <summary>
        /// The getter/setter for the coordinates asscociated with this Appointment.
        /// </summary>
        // [Required]
        // [Column("coordinates")]
        [NotMapped]
        public virtual ICollection<Coordinate> Coordinates { get; set; } = new List<Coordinate>();

        /// <summary>
        /// The getter/setter for the RFData asscociated with this Appointment.
        /// </summary>
        //[Required]
        //[Column("rf_datas")]
        [NotMapped]
        public virtual ICollection<RFData> RFDatas { get; set; } = new List<RFData>();

        [Column("public")]
        public int Public { get; set; }

        /// <summary>
        /// The getter/setter for the telescope asscociated with this Appointment.
        /// </summary>
        public int telescope_id { get;set; }
        [ForeignKey("telescope_id")]
        public virtual RadioTelescope Telescope { get; set; }

        /// <summary>
        /// The getter/setter for the status asscociated with this Appointment.
        /// This is the 
        /// </summary>
        [NotMapped]
        public AppointmentStatusEnum _Status
        {
            get
            {
                return (AppointmentStatusEnum)Enum.Parse(typeof(AppointmentStatusEnum), status);
            }
            set
            {
                this.status = value.ToString();
            }
        }

        private string backingStatus { get; set; }

        [Required]
        [Column("status")]
        public string status {
            get
            {
                return this.backingStatus;
            }
            set
            {
                if (value == null || Enum.IsDefined(typeof(AppointmentStatusEnum), value))
                {
                    this.backingStatus = value;
                }
                else
                {
                    throw new InvalidCastException();
                }
            }
        }

        /// <summary>
        /// The getter/setter for the Appointment type.
        /// </summary>
        [NotMapped]
        public AppointmentTypeEnum _Type {
            get
            {
                return (AppointmentTypeEnum)Enum.Parse(typeof(AppointmentTypeEnum), type);
            }
            set
            {
                this.type = value.ToString();
            }
        }

        private string backingType { get; set; }

        [Required]
        [Column("type")]
        public string type {
            get
            {
                return this.backingType;
            }
            set
            {
                if (value == null || Enum.IsDefined(typeof(AppointmentTypeEnum), value))
                {
                    this.backingType = value;
                }
                else
                {
                    throw new InvalidCastException();
                }
            }
        }

        /// <summary>
        /// The getter/setter for the Appointment priority type.
        /// </summary>
        [NotMapped]
        public AppointmentPriorityEnum _Priority {
            get {
                return (AppointmentPriorityEnum)Enum.Parse(typeof(AppointmentPriorityEnum), priority);
            }
            set
            {
                this.priority = value.ToString();
            }
        }

        private string backingPriority {get; set;}

        [Required]
        [Column("priority")]
        public string priority {
            get
            {
                return this.backingPriority;
            }
            set
            {
                if (value == null || Enum.IsDefined(typeof(AppointmentPriorityEnum), value))
                {
                    this.backingPriority = value;
                }
                else
                {
                    throw new InvalidCastException();
                }
            }
        }

        /// <summary>
        /// The getter/setter for the SpectraCyberConfig type.
        /// </summary>
        public int? spectracyber_config_id { get; set; }
        [ForeignKey("spectracyber_config_id")]
        public virtual SpectraCyberConfig SpectraCyberConfig { get; set; }


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
            return DateTime.Compare(start_time, other.start_time);
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
        /// Returns the HashCode of the Appointment's start_time  
        /// (Implemention of the IComparable Interface)
        /// </summary>
        public override int GetHashCode()
        {
            return start_time.GetHashCode();
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