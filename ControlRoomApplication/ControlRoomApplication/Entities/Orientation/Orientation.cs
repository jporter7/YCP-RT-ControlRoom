using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ControlRoomApplication.Entities
{
    [Table("orientation")]
    [Serializable]
    public class Orientation
    {
        public Orientation(double azimuth, double elevation)
        {
            Azimuth = azimuth;
            Elevation = elevation;
        }

        public Orientation() : this(0, 0) { }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [Column("azimuth")]
        public double Azimuth { get; set; }

        [Required]
        [Column("elevation")]
        public double Elevation { get; set; }

        /// <summary>
        /// Checks if the current Orientation is Equal to another Orientation  
        /// and it checks if the other Orientation is null
        /// </summary>
        public override bool Equals(object obj)
        {
            Orientation other = obj as Orientation; //avoid double casting
            if (ReferenceEquals(other, null))
            {
                return false;
            }
            bool az_equal = Math.Abs(Azimuth - other.Azimuth) <= 0.01;
            bool el_equal = Math.Abs(Elevation - other.Elevation) <= 0.01;
            return az_equal && el_equal;
        }

        /// <summary>
        /// Returns the HashCode of the Orientation's Id
        /// </summary>
        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }
}