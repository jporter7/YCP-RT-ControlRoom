using System;
using System.Collections.Generic;
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
            coordinate = coord;
        }

        public CelestialBody() : this(CelestialBodyConstants.NONE) { }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id")]
        public int id { get; set; }

        [Required]
        [Column("name")]
        public string Name { get; set; }

        [Column("coordinate_id")]
        public Coordinate coordinate { get; set; }

        [Column("appointment_id")]
        public ICollection<Appointment> appointment { get; set; }
    }
}
