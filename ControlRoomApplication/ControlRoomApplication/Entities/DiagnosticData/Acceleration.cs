using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ControlRoomApplication.Entities {
    [Table( "Acceleration" )]
    public class Acceleration : IEquatable<Acceleration>
    {
        public Acceleration() {

        }

        [Key]
        [DatabaseGenerated( DatabaseGeneratedOption.Identity )]
        public int Id { get; set; }


        [Required]
        [Column( "time_captured" )]
        public long TimeCaptured { get; set; }

        [Required]
        [Column( "acceleration_magnitude" )]
        public double acc { get; set; }

        [Required]
        [Column( "acceleration_x" )]
        public double x { get; set; }

        [Required]
        [Column("acceleration_y")]
        public double y { get; set; }

        [Required]
        [Column("acceleration_z")]
        public double z { get; set; }

        [Required]
        [Column( "location" )]
        public int location_ID { get; set; }




        public static Acceleration Generate( long UTCtics , double x , double y , double z , SensorLocationEnum loc ) {
            Acceleration acx = new Acceleration();
            acx.TimeCaptured = UTCtics;// Constants.TIME.UnixEpoch.AddMilliseconds( UTCtics );
            acx.x = x;
            acx.y = y;
            acx.z = z;
            acx.location_ID = (int)loc;
            return acx;
        }

        public bool Equals(Acceleration other)
        {
            if (x != other.x || y != other.y || z != other.z || location_ID != other.location_ID)
            {
                return false;
            }
            return true;
        }
    }
}

