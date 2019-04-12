using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ControlRoomApplication.Constants;

namespace ControlRoomApplication.Entities
{
    [Table("celestial_body")]
    [Serializable]
    public class CelestialBody
    {
        public CelestialBody(string name)
        {
            Name = name;
        }

        public CelestialBody(string name, Coordinate coord)
        {
            Name = name;
            Coordinate = coord;
        }

        public CelestialBody() : this(CelestialBodyConstants.NONE) { }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [Column("name")]
        public string Name { get; set; }

        [Column("coordinate")]
        public Coordinate Coordinate { get; set; }
    }
}
