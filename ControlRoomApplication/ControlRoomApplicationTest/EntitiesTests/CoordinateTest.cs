using System;
using ControlRoomApplicationTest.Entities;

namespace ControlRoomApplicationTest.EntitiesTests
{
    [TestClass]
    class CoordinateTest
    {
        private Coordinate coordinate;
        
        private DateTime ra;
        private DateTime dec;

        private Time time;

        [TestInitialize]
        public void BuildUp()
        {
            coordinate = new Coordinate();

            // the Sun's RA and Dec on Jan 1, 2017
            ra = new DateTime(2017, 1, 1, 18, 50, 06);
            dec = new DateTime(2017, 1, 1, -22, 56, 07);
        }

        [TestMethod]
        public void TestGettersAndSetters()
        {
            Assert.AreEqual(ra, new DateTime(2017, 1, 1, 18, 50, 06));
            Assert.AreEqual(dec, new DateTime(2017, 1, 1, -22, 56, 07));
        }
    }
}
