using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ControlRoomApplication.Entities {
    [Table( "Acceleration" )]
    public class Acceleration {
        public Acceleration() {

        }

        [Key]
        [DatabaseGenerated( DatabaseGeneratedOption.Identity )]
        public int Id { get; set; }


        [Required]
        [Column( "time_captured" )]
        public long TimeCaptured { get; set; }

        [Required]
        [Column( "value" )]
        public double acc { get; set; }

        [Required]
        [Column( "x" )]
        public double x { get; set; }

        [Required]
        [Column( "y" )]
        public double y { get; set; }

        [Required]
        [Column( "z" )]
        public double z { get; set; }

        [Required]
        [Column( "location" )]
        public int location_ID { get; set; }




        public static Acceleration Generate( long UTCtics , double acc , double x , double y , double z , SensorLocationEnum loc ) {
            Acceleration acx = new Acceleration();
            acx.TimeCaptured = UTCtics;// Constants.TIME.UnixEpoch.AddMilliseconds( UTCtics );
            acx.acc = acc;
            acx.x = x;
            acx.y = y;
            acx.z = z;
            acx.location_ID = (int)loc;
            return acx;
        }

    }
}

