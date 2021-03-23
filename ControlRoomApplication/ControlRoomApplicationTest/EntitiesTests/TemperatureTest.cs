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
        private int longitude = 100;
        private int latitude = 34;
        private int altitude = 40;

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
    