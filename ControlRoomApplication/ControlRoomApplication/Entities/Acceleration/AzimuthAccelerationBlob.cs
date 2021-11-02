using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using ControlRoomApplication.Constants;

namespace ControlRoomApplication.Entities
{
    [Table("azimuth_acceleration_blob")]
    public class AzimuthAccelerationBlob : AccelerationBlob
    {
        public AzimuthAccelerationBlob()
        {
            //StringBuilder added to create LongString of accelerartion to be strored in the databse seperated by -
            NumberDataPoints = 0;
            Location = SensorLocationEnum.AZ_MOTOR;
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public override int Id { get; set; }

        [Column("acc_blob")]
        public override Byte[] Blob { get; set; }

        [Column("first_time_captured")]
        public override long FirstTimeCaptured { get; set; }

        [NotMapped]
        private int NumberDataPoints { get; set; }

        [NotMapped]
        private SensorLocationEnum Location { get; set; }


        public override void BuildAccelerationString(Acceleration[] x, bool testFlag = false)
        {
            //TODO - take the Acceleration[] and add it to the Byte[] based on the design
            //more params will need to be passed in order to do this properly

            if (NumberDataPoints >= MiscellaneousConstants.BLOB_SIZE || testFlag == true)
            {
                Database.DatabaseOperations.AddAccelerationBlobData(this, Location, true);
                NumberDataPoints = 0;
            }
        }
    }
}