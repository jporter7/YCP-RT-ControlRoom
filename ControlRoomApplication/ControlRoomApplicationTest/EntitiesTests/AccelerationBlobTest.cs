using ControlRoomApplication.Database;
using ControlRoomApplication.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

namespace ControlRoomApplicationTest.EntitiesTests
{
    [TestClass]
    public class AccelerationBlobTest
    {
        // Class being tested
        Acceleration azAcc, cbAcc, elAcc;
        Acceleration[] azAccArr, cbAccArr, elAccArr;
        AzimuthAccelerationBlob azBlob;
        CounterbalanceAccelerationBlob cbBlob;
        ElevationAccelerationBlob elBlob;
        byte[] blobData;

        [TestInitialize]
        public void BuildUp()
        {

            //byte array to store 32 acceleration data points
            blobData = new byte[238];
            azBlob = new AzimuthAccelerationBlob();
            cbBlob = new CounterbalanceAccelerationBlob();
            elBlob = new ElevationAccelerationBlob();
            azAcc = Acceleration.Generate(DateTimeOffset.UtcNow.ToUnixTimeMilliseconds(), 1, 2, 3, SensorLocationEnum.AZ_MOTOR);
            cbAcc = Acceleration.Generate(DateTimeOffset.UtcNow.ToUnixTimeMilliseconds(), 1, 2, 3, SensorLocationEnum.COUNTERBALANCE);
            elAcc = Acceleration.Generate(DateTimeOffset.UtcNow.ToUnixTimeMilliseconds(), 1, 2, 3, SensorLocationEnum.EL_MOTOR);

            //version
            blobData[0] = 1;

            //FIFO Size
            blobData[1] = 32;

            //SampleFrequency
            byte[] frequency = BitConverter.GetBytes(800);
            blobData[2] = frequency[0];
            blobData[3] = frequency[1];

            //GRange
            blobData[4] = 1;

            //full resolution
            blobData[5] = BitConverter.GetBytes(true)[0];

            //label
            char label = 't';
            blobData[6] = BitConverter.GetBytes(label)[0];


            //time
            byte[] time = BitConverter.GetBytes(azAcc.TimeCaptured);
           
            for (int i = 0; i < 8; i++)
            {
                blobData[7 + i] = time[i];
            }

            //acc x
            byte[] accX = BitConverter.GetBytes(azAcc.x);

            for (int i = 0; i < 2; i++)
            {
                blobData[15 + i] = accX[i];
            }
            //acc x
            byte[] accY = BitConverter.GetBytes(azAcc.y);

            for (int i = 0; i < 2; i++)
            {
                blobData[17 + i] = accY[i];
            }

            //acc z
            byte[] accZ = BitConverter.GetBytes(azAcc.z);
            for(int i=0; i<2; i++)
            {
                blobData[19 + i] = accZ[i];
            }
            

            // write 31 data points without time
            for(int size=0; size < 31; size++)
            {
                //label
                label = 'a';
                blobData[21+(7*(size))] = BitConverter.GetBytes(label)[0];

                //acc x
                for (int i = 0; i < 2; i++)
                {
                    blobData[22 + (7 * (size)) + i] = accX[i];
                }
                //acc y
                for (int i = 0; i < 2; i++)
                {
                    blobData[24 + (7 * (size)) + i] = accY[i];
                }

                //acc z
                for (int i = 0; i < 2; i++)
                {
                    blobData[26 + (7 * (size)) + i] = accZ[i];
                }
            }

            //save the three blobs
            azBlob.Blob = blobData;
            cbBlob.Blob = blobData;
            elBlob.Blob = blobData;


            // create the three comparison acceleration arrays
            azAccArr = new Acceleration[32];
            cbAccArr = new Acceleration[32];
            elAccArr = new Acceleration[32];

            for (int i=0; i<32; i++)
            {
                azAccArr[i] = azAcc;
                cbAccArr[i] = cbAcc;
                elAccArr[i] = elAcc;
            }

        }

        [TestMethod]
        public void TestParsing()
        {
            //Check the first acceleration datapoint
            Assert.IsTrue(azAcc.Equals(azBlob.BlobParser(azBlob.Blob)[0]));
            Assert.IsTrue(cbAcc.Equals(cbBlob.BlobParser(cbBlob.Blob)[0]));
            Assert.IsTrue(elAcc.Equals(elBlob.BlobParser(elBlob.Blob)[0]));

            //check the acceleration array
            Assert.IsTrue(Acceleration.SequenceEquals(azAccArr, azBlob.BlobParser(azBlob.Blob)));
            Assert.IsTrue(Acceleration.SequenceEquals(cbAccArr, cbBlob.BlobParser(cbBlob.Blob)));
            Assert.IsTrue(Acceleration.SequenceEquals(elAccArr, elBlob.BlobParser(elBlob.Blob)));

            //print out the blob
            Console.WriteLine(azBlob.blobToString(azBlob.Blob));

        }
    }
}
    