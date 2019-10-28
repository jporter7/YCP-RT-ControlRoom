using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ControlRoomApplication.Entities
{
    [Table("coordinate")]
    public class Coordinate
    {
        public Coordinate(double rightAscension, double declination)
        {
            RightAscension = rightAscension;
            Declination = declination;
        }

        public Coordinate() : this(0, 0) { }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [Column("right_ascension")]
        public double RightAscension { get; set; }

        [Required]
        [Column("declination")]
        public double Declination { get; set; }
    }
}