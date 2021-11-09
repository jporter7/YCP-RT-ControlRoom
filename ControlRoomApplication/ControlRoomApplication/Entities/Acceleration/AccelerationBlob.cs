using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using ControlRoomApplication.Constants;

namespace ControlRoomApplication.Entities
{
    public abstract class AccelerationBlob
    {
        public abstract int Id { get; set; }

        public abstract Byte[] Blob { get; set; }

        public abstract long FirstTimeCaptured { get; set; }

        private int NumberDataPoints { get; set; }

        public SensorLocationEnum Location { get; set; }

        public virtual void BuildAccelerationBlob(Acceleration[] x, bool testFlag = false)
        {
            //TODO - take the Acceleration[] and add it to the Byte[] based on the design
            //more params will need to be passed in order to do this properly


            if (NumberDataPoints >= MiscellaneousConstants.BLOB_SIZE || testFlag == true)
            {
                Database.DatabaseOperations.AddAccelerationBlobData(this, Location, true);
                NumberDataPoints = 0;
            }
        }

        public virtual Acceleration[] BlobParser(Byte[] BlobToParse)
        {
            List<Acceleration> accArrayList = new List<Acceleration>();
            //The blob has been designed in a modular way so that
            //different versions can be blobbed and parsed.  The first 
            //byte is dedicated to the version #
            byte version = BlobToParse[0];

            switch (version)
            {
                case 1:

                    //store teh FIFO size, sample frequency, GRange, and Full resolution for later use
                    byte FIFO_Size = BlobToParse[1];
                    short SampleFrequency = BitConverter.ToInt16(BlobToParse, 2);
                    byte GRange = BlobToParse[4];
                    Boolean FullResolution = BitConverter.ToBoolean(BlobToParse, 5);


                    //loop over the rest of the blob, the first byte in this section will be a label to tell us what 
                    //to parse out of the blob next
                    for (int i = 6; i < BlobToParse.Length;)
                    {
                        char label = (char)BlobToParse[i];
                        i++;
                        Acceleration tempAcc = new Acceleration();

                        switch (label)
                        {
                            case 't':
                                //when the label is t, there is one time value and 3 acceleration values
                                tempAcc.TimeCaptured = BitConverter.ToInt64(BlobToParse, i) * 100;
                                i += 8;
                                tempAcc.x = BitConverter.ToInt16(BlobToParse, i);
                                i += 2;
                                tempAcc.y = BitConverter.ToInt16(BlobToParse, i);
                                i += 2;
                                tempAcc.z = BitConverter.ToInt16(BlobToParse, i);
                                i += 2;
                                tempAcc.acc = Math.Sqrt(tempAcc.x * tempAcc.x + tempAcc.y * tempAcc.y + tempAcc.z * tempAcc.z);
                                tempAcc.location_ID = (int)Location;
                                accArrayList.Add(tempAcc);
                                break;

                            case 'a':
                                //when the label is a, there are 3 acceleration values
                                tempAcc.TimeCaptured = accArrayList[accArrayList.Count - 1].TimeCaptured + (100 / SampleFrequency);
                                tempAcc.x = BitConverter.ToInt16(BlobToParse, i);
                                i += 2;
                                tempAcc.y = BitConverter.ToInt16(BlobToParse, i);
                                i += 2;
                                tempAcc.z = BitConverter.ToInt16(BlobToParse, i);
                                i += 2;
                                tempAcc.acc = Math.Sqrt(tempAcc.x * tempAcc.x + tempAcc.y * tempAcc.y + tempAcc.z * tempAcc.z);
                                tempAcc.location_ID = (int)Location;
                                accArrayList.Add(tempAcc);
                                break;
                        }

                    }

                    break;

                case 2:
                    //Parse the blob a different way if there is a 2nd version
                    break;

            }

            return accArrayList.ToArray();
        
        }


        //print the blob in binary form
        public String blobToString(Byte[] blob)
        {

            string blobString = "";
            foreach(byte b in blob)
            {
                string singleByte = Convert.ToString(b, 2).PadLeft(8, '0');
                blobString += singleByte + " ";
            }

            return blobString;
        }


        
    }
}