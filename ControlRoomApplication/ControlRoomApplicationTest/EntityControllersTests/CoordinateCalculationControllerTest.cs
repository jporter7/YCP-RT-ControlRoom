using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ControlRoomApplication.Controllers.AASharpControllers;
using ControlRoomApplication.Entities;

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

            Assert.AreEqual(24.476, Coordinate.Latitude, 0.05);
            Assert.AreEqual(218.75, Coordinate.Longitude, 0.05);
        }

        public CoordinateCalculationController CoordinateCalculationController { get; set; }
        public Coordinate Coordinate { get; set; }
    }
}
