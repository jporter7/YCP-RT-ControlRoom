using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ControlRoomApplication.Entities
{
    [Table("sensor_status")]
    public class SensorStatus
    {
        public SensorStatus()
        {

        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }


        [Required]
        [Column("gate")]
        public SByte gate { get; set; }

        [Required]
        [Column("proximity")]
        public SByte proximity { get; set; }

        [Required]
        [Column("azimuth_motor")]
        public SByte az_motor { get; set; }

        [Required]
        [Column("elevation_motor")]
        public SByte el_motor { get; set; }

        [Required]
        [Column("weather_station")]
        public SByte weather { get; set; }


        public static SensorStatus Generate(SensorStatusEnum gateEnum, SensorStatusEnum proximityEnum, SensorStatusEnum az_motorenum, SensorStatusEnum el_motor_enum, SensorStatusEnum weatherEnum)
        {
            SensorStatus status = new SensorStatus();

            status.gate = Convert.ToSByte(gateEnum);
            status.proximity = Convert.ToSByte(proximityEnum);
            status.az_motor = Convert.ToSByte(az_motorenum);
            status.el_motor = Convert.ToSByte(el_motor_enum);
            status.weather = Convert.ToSByte(weatherEnum);

            return status;
        }
    }
}
