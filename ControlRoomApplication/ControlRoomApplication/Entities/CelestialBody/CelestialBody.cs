using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ControlRoomApplication.Entities
{
    public class CelestialBody
    {
        public CelestialBody()
        {

        }

        public CelestialBody(string name)
        {
            Name = name;
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [ForeignKey("AppointmentId")]
        public virtual Appointment Appointment { get; set; }
        public int AppointmentId { get; set; }

        [Required]
        [Column("name")]
        public string Name { get; set; }
    }
}