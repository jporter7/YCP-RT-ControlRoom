using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ControlRoomApplication.Entities
{
    [Table("acceleration_blob")]
    public class AccelerationBlob
    {
        public AccelerationBlob()
        {
            //StringBuilder added to create LongString of accelerartion to be strored in the databse seperated by -
            BlobTheBuilder = new StringBuilder();
            BlobStringCounter = 0;
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Column("acc_blob")]
        public string Blob { get; set; }

        [Column("time_captured")]
        public long TimeCaptured { get; set; }

        [NotMapped]
        private StringBuilder BlobTheBuilder { get; set; }

        [NotMapped]
        private int BlobStringCounter { get; set; }

        public void BuildAccelerationString(Acceleration[] x, long time = -1, bool testFlag = false)
        {
            //If no value is passed for time
            if (time == -1)
            {
                time = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
            }

            int length = x.Length - 1;

            //IF the next Acceleration array will bring us over 4000
            if (BlobStringCounter + x.Length > 4000)
            {
                //Loop through array of acceleration object EXCEPT the last object 
                for (int i = 0; i < x.Length - 1; i++)
                {
                    //Objects appended to StringBuilder with a - added to sperate each value 
                    //~ breaks up a single acceleration 
                    //- breaks up acceleratrion objects
                    BlobTheBuilder.Append(x[i].acc + "~" + x[i].x + "~"
                        + x[i].y + "~" + x[i].z + "~" + x[i].location_ID + "-");
                }

                //Last acceleration added without seperation
                BlobTheBuilder.Append(x[length].acc + "~" + x[length].x + "~"
                        + x[length].y + "~" + x[length].z + "~" + x[length].location_ID);
            }
            //Under 4000 and needs a - at the end
            else
            {
                //Loop through array of acceleration object
                for (int i = 0; i < x.Length; i++)
                {
                    //Objects appended to StringBuilder with a - added to sperate each value 
                    //~ breaks up a single acceleration 
                    //- breaks up acceleratrion objects
                    BlobTheBuilder.Append(x[i].acc + "~" + x[i].x + "~"
                        + x[i].y + "~" + x[i].z + "~" + x[i].location_ID + "-");
                }
            }

            BlobStringCounter += length + 1;
            //If the StringBuilder is more than 4000 characters push to database and clear the builder 
            if (BlobStringCounter > 4000 || testFlag == true)
            {
                this.Blob = BlobTheBuilder.ToString();
                Database.DatabaseOperations.AddAccelerationBlobData(this, time, true);
                BlobTheBuilder.Clear();
                BlobStringCounter = 0;
            }
        }

        public Acceleration[] BlobParser(String AccelerationString)
        {
            //Given the blobbed string of acceleration data, split
            //the string based on the - which seperate each Acceleration data point
            String[] accStrings = AccelerationString.Split('-');

            Acceleration[] accArray = new Acceleration[accStrings.Length];

            //loop through the split array of acceleration points
            for(int i=0; i<accStrings.Length; i++)
            {
                //split each acceleration data point based on the specific values
                //acc~x~y~z~location so that they are each stored individually
                String[] accValues = accStrings[i].Split('~');

                //create an empty Acceleration object
                Acceleration tempAcc = new Acceleration();

                //set the acceleration vector value
                tempAcc.acc = Convert.ToDouble(accValues[0]);

                //set the acceleration x value
                tempAcc.x = Convert.ToDouble(accValues[1]);

                //set the acceleration y value
                tempAcc.y = Convert.ToDouble(accValues[2]);

                //set the acceleration z value
                tempAcc.z = Convert.ToDouble(accValues[3]);

                //set the acceleration location ID
                tempAcc.location_ID = Convert.ToInt32(accValues[4]);

                //store the parsed acceleration point in the Acceleration[]
                accArray[i] = tempAcc;
            }

            return accArray;
        }
        
    }
}