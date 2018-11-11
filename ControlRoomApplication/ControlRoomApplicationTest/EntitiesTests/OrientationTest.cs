using ControlRoomApplication.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ControlRoomApplicationTest.EntitiesTests
{
    [TestClass]
    public class OrientationTest
    {
        private Orientation orientation;

        private long azimuth;
        private long elevation;

        [TestInitialize]
        public void BuildUp()
        {
            orientation = new Orientation();

            azimuth = 0;
            elevation = 0;
        }

        [TestMethod]
        public void TestInitialization()
        {
            Assert.AreEqual(azimuth, orientation.Azimuth);
            Assert.AreEqual(elevation, orientation.Elevation);
        }

        [TestMethod]
        public void TestSettersAndGetters()
        {
            azimuth = 10;
            elevation = 25;

            orientation.Azimuth = 10;
            orientation.Elevation = 25;

            Assert.AreEqual(azimuth, orientation.Azimuth);
            Assert.AreEqual(elevation, orientation.Elevation);
        }
    }
}