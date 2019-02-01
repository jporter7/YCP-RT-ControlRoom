using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ControlRoomApplication.Controllers.AASharpControllers;
using ControlRoomApplication.Entities;

namespace ControlRoomApplicationTest.EntityControllersTests
{
    [TestClass]
    public class CoordinateTransformationTest
    {
        private const double RT_LAT = 40.0244325;
        private const double RT_LONG = -76.7044313;
        private const double RT_ALT = 117;

        [TestInitialize]
        public void Init()
        {

        }

        [TestMethod]
        public void TestCoordinateToOrientation()
        {
            DateTime date = new DateTime(2018, 11, 15, 13, 00, 00, 00);
            Coordinate testCoordinate = new Coordinate(21.5, -14.5);
            Orientation testOrientation = CoordinateTransformation.CoordinateToOrientation(testCoordinate, RT_LAT, RT_LONG, RT_ALT, date);

            Assert.AreEqual(105, testOrientation.Azimuth);
            Assert.AreEqual(-90, testOrientation.Elevation);
        }

        [TestMethod]
        public void TestUTCtoJulian()
        {
            //Jan 1st 2010 5:30:30.1234 AM
            DateTime date = new DateTime(2010, 01, 01, 05, 30, 30, 1234);

            //2455197.729515 (only accurate to 1 millisecond)
            double testJD = CoordinateTransformation.UTCtoJulian(date);

            Assert.AreEqual(2455197.729515, testJD);
        }
    }
}
