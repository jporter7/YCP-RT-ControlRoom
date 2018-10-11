using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ControlRoomApplication.Entities
{
    public class RFData
    {
        public RFData()
        {

        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [Column("time_captured")]
        public DateTime TimeCaptured { get; set; }

        [Required]
        [Column("intensity")]
        public long Intensity { get; set; }

        public virtual Orientation AcquisitionOrientation { get; set; }
    }
}
