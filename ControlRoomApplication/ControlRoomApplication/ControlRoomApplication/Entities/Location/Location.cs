using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ControlRoomApplication.Entities
{
    [Table("location")]
    public class Location
    {
        public Location(double longitude, double latitude, double altitude)
        {
            Longitude = longitude;
            Latitude = latitude;
            Altitude = altitude;
        }

        public Location()
        {

        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [Column("longitude")]
        public double Longitude;

        [Required]
        [Column("latitude")]
        public double Latitude;

        [Required]
        [Column("altitude")]
        public double Altitude;
    }
}