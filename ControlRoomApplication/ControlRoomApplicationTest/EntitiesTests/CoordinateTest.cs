using Microsoft.VisualStudio.TestTools.UnitTesting;
using ControlRoomApplication.Entities;

namespace ControlRoomApplicationTest.EntitiesTests
{
    [TestClass]
    public class CoordinateTest
    {
        private double latitude;
        private double longitude;

        [TestInitialize]
        public void BuildUp()
        {
            latitude = 87.7;
            longitude = 70.5;
        }

        [TestMethod]
        public void TestCoordinate()
        {
            Coordinate coordinate = new Coordinate(87.7, 70.5);
            Assert.AreEqual(latitude, coordinate.RightAscension);
            Assert.AreEqual(longitude, coordinate.Declination);
        }
    }
}
