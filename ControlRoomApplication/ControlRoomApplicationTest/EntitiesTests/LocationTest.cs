using ControlRoomApplication.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ControlRoomApplicationTest.EntitiesTests
{
    [TestClass]
    public class LocationTest
    {
        // Class being tested
        private Location location;
        private int longitude = 100;
        private int latitude = 34;
        private int altitude = 40;

        [TestInitialize]
        public void BuildUp()
        {
            // Initialize appointment entity
            location = new Location(longitude, latitude, altitude);
        }

        [TestMethod]
        public void TestGettersAndSetters()
        {
            Assert.AreEqual(longitude, location.Longitude);
            Assert.AreEqual(latitude, location.Latitude);
            Assert.AreEqual(altitude, location.Altitude);
        }
    }
}
