using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ControlRoomApplication.Controllers.AASharpControllers;
using ControlRoomApplication.Entities;
using static ControlRoomApplication.Constants.RadioTelescopeConstants;

namespace ControlRoomApplicationTest.EntityControllersTests
{
    [TestClass]
    public class CoordinateCalculationControllerTest
    {
        [TestInitialize]
        public void Up()
        {
            CoordinateCalculationController = new CoordinateCalculationController();
        }

        [TestMethod]
        public void TestCalculateSunCoorindates()
        {
            DateTime time = new DateTime(2018, 10, 30, 12, 0, 0);
            Coordinate = CoordinateCalculationController.CalculateCoordinates("sun", time);

            Assert.AreEqual(24.476, Coordinate.RightAscension, 0.05);
            Assert.AreEqual(218.75, Coordinate.Declination, 0.05);
        }

        [TestMethod]
        public void TestCoordinateToOrientation()
        {
            DateTime date = new DateTime(2018, 12, 14, 4, 45, 0);
            Coordinate = new Coordinate(0.0, 90.0);
            Orientation = CoordinateCalculationController.CoordinateToOrientation(Coordinate, RT_LAT, RT_LONG, RT_ALT, date);

            //Assert.AreEqual(360, Orientation.Azimuth, 1);
            //Assert.AreEqual(40.024, Orientation.Elevation, 1);

            date = new DateTime(2018, 12, 14, 12, 0, 0);
            Coordinate = new Coordinate(12, 10);
            Orientation = CoordinateCalculationController.CoordinateToOrientation(Coordinate, RT_LAT, RT_LONG, RT_ALT, date);

            Assert.AreEqual(60.26, Orientation.Azimuth, 1);
            Assert.AreEqual(193.73, Orientation.Elevation, 1);
        }

        [TestMethod]
        public void TestSunCoordinateToOrienation()
        {
            // Test at noon (highest elevation)
            DateTime date = new DateTime(2018, 12, 14, 7, 0, 0);
            //date = date.ToUniversalTime();
            Orientation = CoordinateCalculationController.SunCoordinateToOrientation(RT_LAT, RT_LONG, RT_ALT, date);
            Assert.AreEqual(26.8, Orientation.Elevation, 1);
            Assert.AreEqual(179.6, Orientation.Azimuth, 3);

            // Test at sunrise
            date = new DateTime(2018, 12, 14, 2, 0, 0);
            //date = date.ToUniversalTime();
            Orientation = CoordinateCalculationController.SunCoordinateToOrientation(RT_LAT, RT_LONG, RT_ALT, date);
            Assert.AreEqual(-4.3, Orientation.Elevation, 3);
            Assert.AreEqual(116.9, Orientation.Azimuth, 2);

            // Test at sunset
            date = new DateTime(2018, 12, 14, 12, 0, 0);
            //date = date.ToUniversalTime();
            Orientation = CoordinateCalculationController.SunCoordinateToOrientation(RT_LAT, RT_LONG, RT_ALT, date);
            Assert.AreEqual(-3.8, Orientation.Elevation, 1);
            Assert.AreEqual(242.6, Orientation.Azimuth, 2);
        }

        [TestMethod]
        public void TestMoonCoordinateToOrientation()
        {
            // Test at 11:00pm
            DateTime date = new DateTime(2018, 12, 14, 18, 0, 0);
            // date = date.ToUniversalTime();
            Orientation = CoordinateCalculationController.MoonCoordinateToOrientation(RT_LAT, RT_LONG, RT_ALT, date);
            Assert.AreEqual(5.4, Orientation.Elevation, 6);
            Assert.AreEqual(254.2, Orientation.Azimuth, 4);

            // Test at sunset
            date = new DateTime(2018, 12, 14, 12, 0, 0);
            // date = date.ToUniversalTime();
            Orientation = CoordinateCalculationController.MoonCoordinateToOrientation(RT_LAT, RT_LONG, RT_ALT, date);
            Assert.AreEqual(39.1, Orientation.Elevation, 1);
            Assert.AreEqual(164.0, Orientation.Azimuth, 10);

            // Test at moonrise
            date = new DateTime(2018, 12, 14, 7, 0, 0);
            // date = date.ToUniversalTime();
            Orientation = CoordinateCalculationController.MoonCoordinateToOrientation(RT_LAT, RT_LONG, RT_ALT, date);
            Assert.AreEqual(-3.5, Orientation.Elevation, 5);
            Assert.AreEqual(100.6, Orientation.Azimuth, 6);
        }

        public CoordinateCalculationController CoordinateCalculationController { get; set; }
        public Coordinate Coordinate { get; set; }
        public Orientation Orientation { get; set; }
    }
}
