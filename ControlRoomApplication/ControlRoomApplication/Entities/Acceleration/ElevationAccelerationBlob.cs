using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using ControlRoomApplication.Constants;

namespace ControlRoomApplication.Entities
{
    [Table("elevation_acceleration_blob")]
    public class ElevationAccelerationBlob : AccelerationBlob
    {
        public ElevationAccelerationBlob()
        {
            //StringBuilder added to create LongString of accelerartion to be strored in the databse seperated by -
            NumberDataPoints = 0;
            Location = SensorLocationEnum.EL_MOTOR;
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public override int Id { get; set; }

        [Column("acc_blob")]
        public override Byte[] Blob { get; set; }

        [Column("first_time_captured")]
        public override long FirstTimeCaptured { get; set; }

        [NotMapped]
        public int NumberDataPoints { get; set; }

    }
}