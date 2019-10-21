using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ControlRoomApplication.Entities {
    [Table( "Temperature" )]
    public class Temperature {
        public Temperature() {

        }

        [Key]
        [DatabaseGenerated( DatabaseGeneratedOption.Identity )]
        public int Id { get; set; }


        [Required]
        [Column( "time_captured" )]
        public long TimeCapturedUTC { get; set; }

        [Required]
        [Column( "value" )]
        public double temp { get; set; }

        [Required]
        [Column( "location" )]
        public int location_ID { get; set; }

        public static Temperature Generate( long UTCtics, double temperature , SensorLocationEnum loc  ) {
            Temperature temp = new Temperature();
            //Console.WriteLine( UTCtics );
            temp.TimeCapturedUTC = UTCtics;// Constants.TIME.UnixEpoch.AddMilliseconds( UTCtics ); 
            //Console.WriteLine( temp.TimeCaptured );
            temp.temp = temperature;
            temp.location_ID = (int)loc;
            return temp;
        }

    }
}
