using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ControlRoomApplication.Controllers.AASharpControllers;
using ControlRoomApplication.Entities;
using ControlRoomApplication.Constants;
using System.Collections.Generic;

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
        public void TestCalculateCoordinates()
        {
            // Test point appointment
            Appointment point_appt = new Appointment();
            point_appt.StartTime = DateTime.Now.AddMinutes(1);
            point_appt.EndTime = DateTime.Now.AddMinutes(60);
            point_appt.Coordinates = new List<Coordinate>();
            point_appt.Coordinates.Add(new Coordinate(0, 0));
            var point_orientations = CoordinateCalculationController.CalculateCoordinates(point_appt);

            Assert.IsTrue(point_orientations != null);
            Assert.IsTrue(point_orientations.Count == 59);

            // Test celesital body appointment
            Appointment sun_appt = new Appointment();
            sun_appt.StartTime = DateTime.Now.AddMinutes(1);
            sun_appt.EndTime = DateTime.Now.AddMinutes(60);
            sun_appt.CelestialBody = CelestialBodyConstants.SUN;
            var sun_orientations = CoordinateCalculationController.CalculateCoordinates(sun_appt);

            Assert.IsTrue(sun_orientations != null);
            Assert.IsTrue(sun_orientations.Count == 59);

            // Test raster appointment
            Appointment raster_appt = new Appointment();
            raster_appt.StartTime = DateTime.Now.AddMinutes(1);
            raster_appt.EndTime = DateTime.Now.AddMinutes(60);
            raster_appt.Coordinates = new List<Coordinate>();
            raster_appt.Coordinates.Add(new Coordinate(0, 0));
            raster_appt.Coordinates.Add(new Coordinate(5, 5));
            var raster_orientations = CoordinateCalculationController.CalculateCoordinates(raster_appt);

            Assert.IsTrue(raster_orientations != null);
            Assert.IsTrue(raster_orientations.Count == 59);
        }

        [TestMethod]
        public void TestCalculateSunCoorindates()
        {
            DateTime time = new DateTime(2018, 10, 30, 12, 0, 0);
            Coordinate = CoordinateCalculationController.GetCelestialBodyCoordinate(CelestialBodyConstants.SUN, time);

            Assert.AreEqual(24.476, Coordinate.RightAscension, 0.05);
            Assert.AreEqual(218.75, Coordinate.Declination, 0.05);
        }

        [TestMethod]
        public void TestCoordinateToOrientation()
        {
            DateTime date = new DateTime(2018, 12, 14, 7, 0, 0);
            Coordinate = new Coordinate(0.0, 0.0);
            Orientation = CoordinateCalculationController.CoordinateToOrientation(Coordinate, date);

            //Assert.AreEqual(205.731, Orientation.Azimuth, 1);
            Assert.AreEqual(47.008, Orientation.Elevation, 1);

        }

        [TestMethod]
        public void TestSunCoordinateToOrienation()
        {
            // Test at noon (highest elevation)
            DateTime date = new DateTime(2018, 12, 14, 7, 0, 0);
            //date = date.ToUniversalTime();
            Orientation = CoordinateCalculationController.SunCoordinateToOrientation(date);
            Assert.AreEqual(26.8, Orientation.Elevation, 1);
            Assert.AreEqual(179.6, Orientation.Azimuth, 3);

            // Test at sunrise
            date = new DateTime(2018, 12, 14, 2, 0, 0);
            //date = date.ToUniversalTime();
            Orientation = CoordinateCalculationController.SunCoordinateToOrientation(date);
            Assert.AreEqual(-4.3, Orientation.Elevation, 3);
            Assert.AreEqual(116.9, Orientation.Azimuth, 2);

            // Test at sunset
            date = new DateTime(2018, 12, 14, 12, 0, 0);
            //date = date.ToUniversalTime();
            Orientation = CoordinateCalculationController.SunCoordinateToOrientation(date);
            Assert.AreEqual(-3.8, Orientation.Elevation, 1);
            Assert.AreEqual(242.6, Orientation.Azimuth, 2);
        }

        [TestMethod]
        public void TestMoonCoordinateToOrientation()
        {
            // Test at 11:00pm
            DateTime date = new DateTime(2018, 12, 14, 18, 0, 0);
            // date = date.ToUniversalTime();
            Orientation = CoordinateCalculationController.MoonCoordinateToOrientation(date);
            Assert.AreEqual(5.4, Orientation.Elevation, 6);
            Assert.AreEqual(254.2, Orientation.Azimuth, 4);

            // Test at sunset
            date = new DateTime(2018, 12, 14, 12, 0, 0);
            // date = date.ToUniversalTime();
            Orientation = CoordinateCalculationController.MoonCoordinateToOrientation(date);
            Assert.AreEqual(39.1, Orientation.Elevation, 1);
            Assert.AreEqual(164.0, Orientation.Azimuth, 10);

            // Test at moonrise
            date = new DateTime(2018, 12, 14, 7, 0, 0);
            // date = date.ToUniversalTime();
            Orientation = CoordinateCalculationController.MoonCoordinateToOrientation(date);
            Assert.AreEqual(-3.5, Orientation.Elevation, 5);
            Assert.AreEqual(100.6, Orientation.Azimuth, 6);
        }

        public CoordinateCalculationController CoordinateCalculationController { get; set; }
        public Coordinate Coordinate { get; set; }
        public Orientation Orientation { get; set; }
    }
}