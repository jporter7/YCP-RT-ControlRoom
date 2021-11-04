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

        private SensorLocationEnum Location { get; set; }

        public abstract void BuildAccelerationString(Acceleration[] x, bool testFlag = false);

        public virtual List<Acceleration> BlobParser(Byte[] BlobToParse)
        {
            List<Acceleration> accArrayList = new List<Acceleration>();
            //The blob has been designed in a modular way so that
            //different versions can be blobbed and parsed.  The first 
            //byte is dedicated to the version #
            byte version = BlobToParse[0];

            switch (version)
            {
                case 1:
                    byte FIFO_Size = BlobToParse[1];
                    byte SampleFrequency = BlobToParse[2];
                    byte GRange = BlobToParse[3];
                    Boolean FullResolution = BitConverter.ToBoolean(BlobToParse, 4);

                    for (int i = 0; i < BlobToParse.Length - 5;)
                    {
                        char label = BitConverter.ToChar(BlobToParse, i);
                        Acceleration tempAcc = new Acceleration();

                        switch (label)
                        {
                            case 't':
                                //when the label is t, there is one time value and 3 acceleration values
                                tempAcc.TimeCaptured = BitConverter.ToInt64(BlobToParse, i);
                                i += 8;
                                tempAcc.x = BitConverter.ToInt16(BlobToParse, i);
                                i += 2;
                                tempAcc.y = BitConverter.ToInt16(BlobToParse, i);
                                i += 2;
                                tempAcc.z = BitConverter.ToInt16(BlobToParse, i);
                                i += 2;
                                tempAcc.acc = Math.Sqrt(tempAcc.x * tempAcc.x + tempAcc.y * tempAcc.y + tempAcc.z * tempAcc.z);

                                accArrayList.Add(tempAcc);
                                break;

                            case 'a':
                                //when the label is a, there are 3 acceleration values
                                tempAcc.TimeCaptured = accArrayList[accArrayList.Count - 1].TimeCaptured + (1 / SampleFrequency);
                                tempAcc.x = BitConverter.ToInt16(BlobToParse, i);
                                i += 2;
                                tempAcc.y = BitConverter.ToInt16(BlobToParse, i);
                                i += 2;
                                tempAcc.z = BitConverter.ToInt16(BlobToParse, i);
                                i += 2;
                                tempAcc.acc = Math.Sqrt(tempAcc.x * tempAcc.x + tempAcc.y * tempAcc.y + tempAcc.z * tempAcc.z);

                                accArrayList.Add(tempAcc);
                                break;
                        }

                    }

                    break;

                case 2:
                    //Parse the blob a different way if there is a 2nd version
                    break;

            }

            return accArrayList;
        
        }
        
    }
}