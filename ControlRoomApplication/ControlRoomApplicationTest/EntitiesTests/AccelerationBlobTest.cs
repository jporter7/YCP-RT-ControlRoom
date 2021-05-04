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
        private Acceleration Acceleration;

        [TestInitialize]
        public void BuildUp()
        {

        }

        [TestMethod]
        public void TestGettersAndSetters()
        {
            // Initialize appointment entity
            SensorLocationEnum loc1 = SensorLocationEnum.AZ_MOTOR;

            //Generate current time
            long dateTime1 = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
            long dateTime2 = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();

            //Generate Acceleration
            AccelerationBlob a1 = new AccelerationBlob();
            //Acceleration t2 = Acceleration.Generate(dateTime, 2.0, loc1);

            a1.Blob = "abc";
            a1.Id = 17;
            a1.TimeCaptured = dateTime1;

            a1.TimeCaptured = dateTime2;
            Assert.AreEqual(a1.TimeCaptured, dateTime2);
            Assert.AreEqual(a1.Blob, "abc");
            Assert.AreEqual(a1.Id, 17);

        }

        //Acceleration
        [TestMethod]
        public void TestAddAndRetrieveAccelerationBlob()
        {
            Acceleration[] accArray = new Acceleration[1];

            long dateTime1 = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
            SensorLocationEnum loc1 = SensorLocationEnum.AZ_MOTOR;

            Acceleration acc = new Acceleration();
            acc = Acceleration.Generate(dateTime1, 1.1, 2.2, 3.3, loc1);
            accArray[0] = acc;

            AccelerationBlob accBLob = new AccelerationBlob();
            accBLob.BuildAccelerationString(accArray, dateTime1, true);

            //DatabaseOperations.AddAccelerationBlobData(acc, dateTime1, true);
            List<AccelerationBlob> accReturn = DatabaseOperations.GetAccBlobData(dateTime1 - 1, dateTime1 + 1);

            Assert.AreEqual(1,accReturn.Count);

            //Test only acc
            Assert.AreEqual("4.11582312545134~1.1~2.2~3.3~0-", accReturn[0].Blob);
            Assert.AreEqual(dateTime1, accReturn[0].TimeCaptured);
        }
    }
}
    