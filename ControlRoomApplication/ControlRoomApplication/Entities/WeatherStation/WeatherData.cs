using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ControlRoomApplication.Entities
{
    [Table("WeatherData")]
    public class WeatherData
    {
        public WeatherData()
        {

        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }


        [Required]
        [Column("time_captured")]
        public long TimeCaptured { get; set; }

        [Required]
        [Column("wind_speed")]
        public double ws { get; set; }

        [Required]
        [Column("wind_direction_deg")]
        public double wd_deg { get; set; }

        [Required]
        [Column("wind_direction_str")]
        public string wd_str { get; set; }

        [Required]
        [Column("outside_temperature_degF")]
        public double ot { get; set; }

        [Required]
        [Column("inside_temperature_degF")]
        public double it { get; set; }

        [Required]
        [Column("rain_rate")]
        public double rr { get; set; }

        [Required]
        [Column("rain_total")]
        public double rt { get; set; }

        [Required]
        [Column("rain_day")]
        public double rd { get; set; }

        [Required]
        [Column("rain_month")]
        public double rm { get; set; }

        [Required]
        [Column("barometric_pressure")]
        public double bp { get; set; }

        [Required]
        [Column("dew_point")]
        public double dp { get; set; }

        [Required]
        [Column("wind_chill")]
        public double wc { get; set; }

        [Required]
        [Column("humidity")]
        public double hum { get; set; }

        [Required]
        [Column("heat_index")]
        public double heat { get; set; }



        public static WeatherData Generate(AbstractWeatherStation.Weather_Data data)
        {
            WeatherData dbData = new WeatherData();

            dbData.TimeCaptured = DateTime.UtcNow.Ticks;
            dbData.ws = data.windSpeed;
            dbData.wd_str = data.windDirection;
            dbData.wd_deg = data.windDirectionDegrees;
            dbData.rd = data.dailyRain;
            dbData.rr = data.rainRate;
            dbData.ot = data.outsideTemp;
            dbData.it = data.insideTemp;
            dbData.bp = data.baromPressure;
            dbData.dp = data.dewPoint;
            dbData.wc = data.windChill;
            dbData.hum = data.outsideHumidity;
            dbData.rt = data.totalRain;
            dbData.rm = data.monthlyRain;
            dbData.heat = data.heatIndex;

            return dbData;
        }
    }
}

