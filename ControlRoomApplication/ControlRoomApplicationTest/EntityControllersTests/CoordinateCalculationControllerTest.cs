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
            Coordinate = CoordinateCalculationController.CalculateCoordinates("sun", DateTime.Now);

            Assert.AreEqual(Coordinate.Latitude, 12.05, 0.05);
            Assert.AreEqual(Coordinate.Longitude, 67.25, 0.05);
        }

        public CoordinateCalculationController CoordinateCalculationController { get; set; }
        public Coordinate Coordinate { get; set; }
    }
}
