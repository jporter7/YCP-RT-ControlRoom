using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ControlRoomApplication.Entities
{
    public class Orientation
    {
        public Orientation(long azimuth, long elevation)
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
        public long Azimuth { get; set; }

        [Required]
        [Column("elevation")]
        public long Elevation { get; set; }
    }
}
