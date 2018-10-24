using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ControlRoomApplication.Entities
{
    [Table("orientation")]
    public class Orientation
    {
        public Orientation(long azimuth, long elevation)
        {
            Azimuth = azimuth;
            Elevation = elevation;
        }

        public Orientation() : this(0, 0) { }

        [Key, ForeignKey("RFData")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CapturedOrientationId { get; set; }

        [Required]
        [Column("azimuth")]
        public long Azimuth { get; set; }

        [Required]
        [Column("elevation")]
        public long Elevation { get; set; }

        public virtual RFData RFData { get; set; }
    }
}
