using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ControlRoomApplication.Entities
{
    [Table("weather_threshold")]
    public class WeatherThreshold
    {

        public WeatherThreshold(int windSpeed, int snowDumpTime)
        {
            WindSpeed = windSpeed;
            SnowDumpTime = snowDumpTime;
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }


        [Column("wind_speed")]
        public int WindSpeed { get; set; }

        [Column("snow_dump_time")]
        public int SnowDumpTime { get; set; }


    }
}