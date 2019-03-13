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
            DateTime start = new DateTime(2018, 10, 30, 12, 0, 0);
            DateTime end = new DateTime(2018, 10, 30, 13, 0, 0);

            // Test point appointment
            Appointment point_appt = new Appointment();
            point_appt.Type = AppointmentTypeConstants.POINT;
            point_appt.StartTime = start;
            point_appt.EndTime = end;
            point_appt.Coordinates.Add(new Coordinate(0, 0));
            var point_orientations = CoordinateCalculationController.CalculateCoordinates(point_appt);

            Assert.IsTrue(point_orientations != null);
            Assert.IsTrue(point_orientations.Count == 60);

            // Test celesital body appointment
            Appointment sun_appt = new Appointment();
            sun_appt.Type = AppointmentTypeConstants.CELESTIAL_BODY;
            sun_appt.StartTime = start;
            sun_appt.EndTime = end;
            sun_appt.CelestialBody = CelestialBodyConstants.SUN;
            var sun_orientations = CoordinateCalculationController.CalculateCoordinates(sun_appt);

            Assert.IsTrue(sun_orientations != null);
            Assert.IsTrue(sun_orientations.Count == 60);

            // Test raster appointment
            Appointment raster_appt = new Appointment();
            raster_appt.Type = AppointmentTypeConstants.RASTER;
            raster_appt.StartTime = start;
            raster_appt.EndTime = end;
            raster_appt.Coordinates.Add(new Coordinate(0, 0));
            raster_appt.Coordinates.Add(new Coordinate(5, 5));
            var raster_orientations = CoordinateCalculationController.CalculateCoordinates(raster_appt);

            Assert.IsTrue(raster_orientations != null);
            Assert.IsTrue(raster_orientations.Count == 60);

            // Test orientation appointment
            Appointment orientation_appt = new Appointment();
            orientation_appt.Type = AppointmentTypeConstants.ORIENTATION;
            orientation_appt.StartTime = start;
            orientation_appt.EndTime = end;
            orientation_appt.Orientation = new Orientation(30, 30);
            var orientation_orientations = CoordinateCalculationController.CalculateCoordinates(raster_appt);

            Assert.IsTrue(orientation_orientations != null);
            Assert.IsTrue(orientation_orientations.Count == 60);
        }

        [TestMethod]
        public void TestGetPointCoordinate()
        {
            DateTime time = new DateTime(2018, 10, 30, 12, 0, 0);
            var appt = new Appointment();
            appt.Coordinates = new List<Coordinate>();
            var coord_1 = new Coordinate(12, 12);
            appt.Coordinates.Add(coord_1);
            Coordinate output_coord_1 = CoordinateCalculationController.GetPointCoordinate(appt, time);

            Assert.AreEqual(coord_1.RightAscension, output_coord_1.RightAscension, 0.05);
            Assert.AreEqual(coord_1.Declination, output_coord_1.Declination, 0.05);
        }

        [TestMethod]
        public void TestGetRasterCoordinate()
        {
            DateTime start = new DateTime(2018, 10, 30, 12, 0, 0);
            DateTime end = new DateTime(2018, 10, 30, 13, 0, 0);
            var appt = new Appointment();
            appt.StartTime = start;
            appt.EndTime = end;
            appt.Coordinates = new List<Coordinate>();
            var coord_1 = new Coordinate(12, 12);
            var coord_2 = new Coordinate(15, 15);
            appt.Coordinates.Add(coord_1);
            appt.Coordinates.Add(coord_2);
            Coordinate output_coord_1 = CoordinateCalculationController.GetRasterCoordinate(appt, start);
            Coordinate output_coord_2 = CoordinateCalculationController.GetRasterCoordinate(appt, end);

            Assert.AreEqual(coord_1.RightAscension, output_coord_1.RightAscension, 0.05);
            Assert.AreEqual(coord_1.Declination, output_coord_1.Declination, 0.05);

            Assert.IsTrue(coord_2.RightAscension > output_coord_2.RightAscension);
            Assert.IsTrue(coord_2.Declination > output_coord_2.Declination);
        }

        [TestMethod]
        public void TestGetCelestialBodyCoordinate()
        {
            // Test at noon local time, highest elevation (Converted to UTC) 
            DateTime date = new DateTime(2018, 12, 14, 17, 0, 0);
            Coordinate output_coord_1 = CoordinateCalculationController.GetCelestialBodyCoordinate(CelestialBodyConstants.SUN, date);

            Assert.AreEqual(17.4508, output_coord_1.RightAscension, 0.05);
            Assert.AreEqual(-23.2207, output_coord_1.Declination, 0.05);
        }

        [TestMethod]
        public void TestGetSunCoordinate()
        {
            // Test at noon local time, highest elevation (Converted to UTC) 
            DateTime date = new DateTime(2018, 12, 14, 17, 0, 0);
            Coordinate output_coord_1 = CoordinateCalculationController.GetSunCoordinate(date);

            Assert.AreEqual(17.4508, output_coord_1.RightAscension, 0.05);
            Assert.AreEqual(-23.2207, output_coord_1.Declination, 0.05);

            // Test at sunrise local time (Converted to UTC)  
            date = new DateTime(2018, 12, 14, 12, 0, 0);
            Coordinate output_coord_2 = CoordinateCalculationController.GetSunCoordinate(date);

            Assert.AreEqual(17.4354, output_coord_2.RightAscension, 0.05);
            Assert.AreEqual(-23.2085, output_coord_2.Declination, 0.05);

            // Test at sunset local time (Converted to UTC)  
            date = new DateTime(2018, 12, 14, 22, 0, 0);
            Coordinate output_coord_3 = CoordinateCalculationController.GetSunCoordinate(date);

            Assert.AreEqual(17.4661, output_coord_3.RightAscension, 0.05);
            Assert.AreEqual(-23.2327, output_coord_3.Declination, 0.05);
        }

        [TestMethod]
        public void TestGetMoonCoordinate()
        {
            // Test at 1:00pm local time (Converted to UTC)
            DateTime date = new DateTime(2018, 12, 14, 18, 0, 0);
            Coordinate output_coord_1 = CoordinateCalculationController.GetMoonCoordinate(date);

            Assert.AreEqual(23.1637847, output_coord_1.RightAscension, 1);
            Assert.AreEqual(-9.5685756655, output_coord_1.Declination, 1);

            // Test at sunset local time (Converted to UTC)  
            date = new DateTime(2018, 12, 14, 22, 0, 0);
            Coordinate output_coord_2 = CoordinateCalculationController.GetMoonCoordinate(date);

            Assert.AreEqual(23.2914, output_coord_2.RightAscension, 1);
            Assert.AreEqual(-8.9042, output_coord_2.Declination, 1);

            // Test at moonrise local time (Converted to UTC)
            date = new DateTime(2018, 12, 15, 0, 0, 0);
            Coordinate output_coord_3 = CoordinateCalculationController.GetMoonCoordinate(date);

            Assert.AreEqual(23.3550, output_coord_3.RightAscension, 1);
            Assert.AreEqual(-8.565285, output_coord_3.Declination, 1);
        }

        [TestMethod]

        public void TestCoordinateToOrientation()
        {
            // Test at 10:01:43 PM local time (Converted to UTC) 
            DateTime date_1 = new DateTime(2018, 6, 23, 2, 1, 43);
            Coordinate coord_1 = new Coordinate(1, 1);
            Orientation test_orientation_1 = new Orientation(40.81, -40.75);
            Orientation output_orientation_1 = CoordinateCalculationController.CoordinateToOrientation(coord_1, date_1);

            Assert.AreEqual(test_orientation_1.Azimuth, output_orientation_1.Azimuth, 0.5);
            Assert.AreEqual(test_orientation_1.Elevation, output_orientation_1.Elevation, 0.5);

            // Test at 6:00:00 AM local time (Converted to UTC)
            DateTime date_2 = new DateTime(2018, 12, 14, 11, 0, 0);
            Coordinate coord_2 = new Coordinate(12, 1);
            Orientation test_orientation_2 = new Orientation(166.09, 50.04);
            Orientation output_orientation_2 = CoordinateCalculationController.CoordinateToOrientation(coord_2, date_2);

            Assert.AreEqual(test_orientation_2.Azimuth, output_orientation_2.Azimuth, 0.5);
            Assert.AreEqual(test_orientation_2.Elevation, output_orientation_2.Elevation, 0.5);

            // Test at 5:55:02 PM local time (Converted to UTC)
            DateTime date_3 = new DateTime(2018, 2, 3, 22, 55, 2);
            Coordinate coord_3 = new Coordinate(23, 80);
            Orientation test_orientation_3 = new Orientation(348.36, 45.00);
            Orientation output_orientation_3 = CoordinateCalculationController.CoordinateToOrientation(coord_3, date_3);

            Assert.AreEqual(test_orientation_3.Azimuth, output_orientation_3.Azimuth, 0.5);
            Assert.AreEqual(test_orientation_3.Elevation, output_orientation_3.Elevation, 0.5);
        }

        public CoordinateCalculationController CoordinateCalculationController { get; set; }
    }
}