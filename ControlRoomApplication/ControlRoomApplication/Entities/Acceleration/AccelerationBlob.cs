using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using ControlRoomApplication.Constants;

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
            if (BlobStringCounter + x.Length >= MiscellaneousConstants.BLOB_SIZE)
            {
                //Loop through array of acceleration object EXCEPT the last object 
                for (int i = 0; i < x.Length - 1; i++)
                {
                    
                    //if this is the first of the Accelerometer's returned datapoints, add the time captured to the blob
                    if((BlobStringCounter+i)% MiscellaneousConstants.SUB_BLOB_SIZE == 0)
                    {
                        //Objects appended to StringBuilder with a $ added to sperate each value 
                        //~ breaks up a single acceleration 
                        //$ breaks up acceleratrion objects
                        BlobTheBuilder.Append(x[i].TimeCaptured + "~" + x[i].x + "~"
                            + x[i].y + "~" + x[i].z + "~" + x[i].location_ID + "$");
                    }
                    else
                    {
                        //Objects appended to StringBuilder with a $ added to sperate each value 
                        //~ breaks up a single acceleration 
                        //$ breaks up acceleratrion objects
                        BlobTheBuilder.Append(x[i].x + "~" + x[i].y + "~" + x[i].z + "~" + x[i].location_ID + "$");
                    }
                    
                    
                    
                }

                //Last acceleration added without seperation
                BlobTheBuilder.Append(x[length].x + "~" + x[length].y + "~" + x[length].z + "~" + x[length].location_ID);
            }
            //Under 4000 and needs a $ at the end
            else
            {
                //Loop through array of acceleration object
                for (int i = 0; i < x.Length; i++)
                {

                    //if this is the first of the Accelerometer's returned datapoints, add the time captured to the blob
                    if ((BlobStringCounter + i) % MiscellaneousConstants.SUB_BLOB_SIZE == 0)
                    {
                        //Objects appended to StringBuilder with a $ added to sperate each value 
                        //~ breaks up a single acceleration 
                        //$ breaks up acceleratrion objects
                        BlobTheBuilder.Append(x[i].TimeCaptured + "~" + x[i].x + "~"
                            + x[i].y + "~" + x[i].z + "~" + x[i].location_ID + "$");
                    }
                    else
                    {
                        //Objects appended to StringBuilder with a $ added to sperate each value 
                        //~ breaks up a single acceleration 
                        //$ breaks up acceleratrion objects
                        BlobTheBuilder.Append(x[i].x + "~" + x[i].y + "~" + x[i].z + "~" + x[i].location_ID + "$");
                    }
                }
            }

            BlobStringCounter += length + 1;
            //If the StringBuilder is more than 4000 characters push to database and clear the builder 
            if (BlobStringCounter >= MiscellaneousConstants.BLOB_SIZE || testFlag == true)
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
            //the string based on the $ which seperate each Acceleration data point
            String[] accStrings = AccelerationString.Split('$');

            Acceleration[] accArray = new Acceleration[accStrings.Length];

            //loop through the split array of acceleration points
            for(int i=0; i<accStrings.Length; i++)
            {
                //split each acceleration data point based on the specific values
                //acc~x~y~z~location so that they are each stored individually
                String[] accValues = accStrings[i].Split('~');

                //create an empty Acceleration object
                Acceleration tempAcc = new Acceleration();


                //if this is the first datapoint in the sub blob, it will have the acceleration TimeCaptured value, otherwise the TimeCaptured value will be manually calculated
                if(i % MiscellaneousConstants.SUB_BLOB_SIZE == 0)
                {
                    //set the TimeCaptured value
                    tempAcc.TimeCaptured = long.Parse(accValues[0]);

                    //set the acceleration x value
                    tempAcc.x = Convert.ToInt16(accValues[1]);

                    //set the acceleration y value
                    tempAcc.y = Convert.ToInt16(accValues[2]);

                    //set the acceleration z value
                    tempAcc.z = Convert.ToInt16(accValues[3]);

                    //set the acceleration location ID
                    tempAcc.location_ID = Convert.ToInt16(accValues[4]);

                    //set the acceleration magnitude
                    tempAcc.acc = Math.Sqrt(tempAcc.x * tempAcc.x + tempAcc.y * tempAcc.y + tempAcc.z * tempAcc.z);

                }
                else
                {
                    //Calculate this datapoints TimeCaptured based on the TimeCaptured of the previous datapoint + the Time Delta
                    //I need to look into the (long) cast, as it removes precision...
                    tempAcc.TimeCaptured = (long)(accArray[i - 1].TimeCaptured + MiscellaneousConstants.SAMPLE_TIME_DELTA);

                    //set the acceleration x value
                    tempAcc.x = Convert.ToInt16(accValues[0]);

                    //set the acceleration y value
                    tempAcc.y = Convert.ToInt16(accValues[1]);

                    //set the acceleration z value
                    tempAcc.z = Convert.ToInt16(accValues[2]);

                    //set the acceleration location ID
                    tempAcc.location_ID = Convert.ToInt16(accValues[3]);

                    //set the acceleration magnitude
                    tempAcc.acc = Math.Sqrt(tempAcc.x * tempAcc.x + tempAcc.y * tempAcc.y + tempAcc.z * tempAcc.z);
                }

                //store the parsed acceleration point in the Acceleration[]
                accArray[i] = tempAcc;
            }

            return accArray;
        }
        
    }
}