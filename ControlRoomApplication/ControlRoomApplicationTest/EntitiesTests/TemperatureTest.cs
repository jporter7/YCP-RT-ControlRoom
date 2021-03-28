using ControlRoomApplication.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace ControlRoomApplicationTest.EntitiesTests
{
    [TestClass]
    public class TemperatureTest
    {
        // Class being tested
        private Temperature Temperature;

        [TestInitialize]
        public void BuildUp()
        {

        }

        [TestMethod]
        public void TestGettersAndSetters()
        {
            // Initialize appointment entity
            SensorLocationEnum loc1 = SensorLocationEnum.AZ_MOTOR;
            SensorLocationEnum loc2 = SensorLocationEnum.EL_MOTOR;

            //Generate current time
            long dateTime1 = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
            long dateTime2 = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();

            //Generate Temperature
            Temperature t1 = Temperature.Generate(dateTime1, 0.0, loc1);

            t1.TimeCapturedUTC = dateTime2;
            Assert.AreEqual(t1.TimeCapturedUTC, dateTime2);
            t1.temp = 100.0;
            Assert.AreEqual(t1.temp, 100.0);
            t1.location_ID =1;
            Assert.AreEqual(t1.location_ID, 1);

        }

        [TestMethod]
        public void TestEquals()
        {
            // Initialize appointment entity
            SensorLocationEnum loc1 = SensorLocationEnum.AZ_MOTOR;

            //Generate current time
            long dateTime = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();

            //Generate Temperature
            Temperature t1 = Temperature.Generate(dateTime, 0.0, loc1);
            Temperature t2 = Temperature.Generate(dateTime, 2.0, loc1);


            Assert.AreEqual(t1, t1);
            Assert.AreNotEqual(t1, t2);
            Assert.AreNotEqual(t2, t1);
        }
    }
}
    