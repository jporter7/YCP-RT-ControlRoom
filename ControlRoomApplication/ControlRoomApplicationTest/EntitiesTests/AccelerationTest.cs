using ControlRoomApplication.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

namespace ControlRoomApplicationTest.EntitiesTests
{
    [TestClass]
    public class AccelerationTest
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
            Acceleration a1 = Acceleration.Generate(dateTime1, 0.0, 0.0, 0.0, loc1);
            //Acceleration t2 = Acceleration.Generate(dateTime, 2.0, loc1);


            a1.TimeCaptured = dateTime2;
            Assert.AreEqual(a1.TimeCaptured, dateTime2);
            a1.x = 1;
            Assert.AreEqual(a1.x, 1);
            a1.y = 1;
            Assert.AreEqual(a1.y, 1);
            a1.z = 1;
            Assert.AreEqual(a1.z, 1);
            a1.location_ID = 1;
            Assert.AreEqual(a1.location_ID, 1);

        }

        [TestMethod]
        public void TestGenerate()
        {
            // Initialize appointment entity
            SensorLocationEnum loc1 = SensorLocationEnum.AZ_MOTOR;

            //Generate current time
            long dateTime1 = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();

            //Generate Acceleration
            Acceleration a1 = Acceleration.Generate(dateTime1, 1, 1, 1, loc1);

            Assert.AreEqual(a1.TimeCaptured, dateTime1);
            Assert.AreEqual(a1.x, 1);
            Assert.AreEqual(a1.y, 1);
            Assert.AreEqual(a1.z, 1);
            Assert.AreEqual(a1.location_ID, (int)loc1);

        }

        [TestMethod]
        public void TestAddAndRetrieveAcceleration()
        {
            List<Acceleration> acc = new List<Acceleration>();
            SensorLocationEnum loc1 = SensorLocationEnum.AZ_MOTOR;

            //Generate current time
            long dateTime1 = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();

            //Generate Acceleration
            Acceleration a1 = Acceleration.Generate(dateTime1, 1, 1, 1, loc1);

            acc.Add(a1);


            //Test acc
            Assert.AreEqual(acc[0].TimeCaptured, dateTime1);
            Assert.AreEqual(acc[0].x, 1);
            Assert.AreEqual(acc[0].y, 1);
            Assert.AreEqual(acc[0].z, 1);
            Assert.AreEqual(acc[0].location_ID, (int)loc1);
        }

        [TestMethod]
        public void TestAddAndRetrieveAccelerations()
        {
            List<Acceleration> acc = new List<Acceleration>();
            SensorLocationEnum loc1 = SensorLocationEnum.AZ_MOTOR;
            SensorLocationEnum loc2 = SensorLocationEnum.EL_MOTOR;

            //Generate current time
            long dateTime1 = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
            long dateTime2 = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();


            //Generate Acceleration
            Acceleration a1 = Acceleration.Generate(dateTime1, 1, 1, 1, loc1);
            Acceleration a2 = Acceleration.Generate(dateTime2, 2, 2, 2, loc2);


            acc.Add(a1);
            acc.Add(a2);

            //Test acc
            Assert.AreEqual(acc[0].TimeCaptured, dateTime1);
            Assert.AreEqual(acc[0].x, 1);
            Assert.AreEqual(acc[0].y, 1);
            Assert.AreEqual(acc[0].z, 1);
            Assert.AreEqual(acc[0].location_ID, (int)loc1);

            Assert.AreEqual(acc[1].TimeCaptured, dateTime2);
            Assert.AreEqual(acc[1].x, 2);
            Assert.AreEqual(acc[1].y, 2);
            Assert.AreEqual(acc[1].z, 2);
            Assert.AreEqual(acc[1].location_ID, (int)loc2);

        }

        [TestMethod]
        public void TestEquals()
        {
            SensorLocationEnum loc1 = SensorLocationEnum.AZ_MOTOR;
            SensorLocationEnum loc2 = SensorLocationEnum.EL_MOTOR;

            //Generate current time
            long dateTime1 = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
            long dateTime2 = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();


            //Generate Temperature
            Acceleration a1 = Acceleration.Generate(dateTime1, 1, 1, 1, loc1);
            Acceleration a2 = Acceleration.Generate(dateTime2, 2, 2, 2, loc2);


            Assert.AreEqual(a1, a1);
            Assert.AreNotEqual(a1, a2);
            Assert.AreNotEqual(a2, a1);
        }
    }
}
    