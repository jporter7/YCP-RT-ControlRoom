using System;
using System.Collections.Generic;
using ControlRoomApplication.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ControlRoomApplication.Controllers.AASharpControllers;

namespace ControlRoomApplicationTest.EntityControllersTests.AASharpControllers
{
    [TestClass]
    public class OrientationCalculationControllerTest
    {
        [TestInitialize]
        public void BuildUp() => Controller = new OrientationCalculationController();

        [TestMethod]
        public void TestCalculateOrientations()
        {
            // Create the appointment timespan from December 3rd, 2018 at 12:00 PM - 2:00 PM
            DateTime startTime = new DateTime(2018, 12, 12, 12, 0, 0);
            DateTime endTime = startTime.AddMinutes(120);

            // Call the method with a right ascension and declination of 248.8917 and -21.0189
            // which is the right ascension and declination of the sun for that day
            Dictionary = Controller.CalculateOrientations(startTime, endTime, 248.8917, -21.0189);

            Assert.AreEqual(30, Dictionary.Keys.Count);
        }

        public Dictionary<DateTime, Orientation> Dictionary { get; set; }
        public OrientationCalculationController Controller { get; set; }
    }
}
