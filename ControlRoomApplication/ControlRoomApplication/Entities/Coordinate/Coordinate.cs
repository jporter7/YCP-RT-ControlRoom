using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ControlRoomApplication.Entities
{
    [Table("coordinate")]
    public class Coordinate
    {

        public Coordinate(double rightAscension, double declination)
        {
            this.rightAscension = rightAscension;
            this.declination = declination;
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [Column("right_ascension")]
        public double rightAscension { get; set; }

        [Required]
        [Column("declination")]
        public double declination { get; set; }
       
    }
}
