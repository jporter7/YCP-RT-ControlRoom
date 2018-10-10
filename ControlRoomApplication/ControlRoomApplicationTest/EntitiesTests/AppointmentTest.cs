using ControlRoomApplication.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace ControlRoomApplicationTest.EntitiesTests
{
    [TestClass]
    public class AppointmentTest
    {
        // Class being tested
        private Appointment appointment;

        // Attributes in the class that need to be tested
        private DateTime startTime;
        private DateTime endTime;

        [TestInitialize]
        public void BuildUp()
        {
            // Initialize appointment entity
            appointment = new Appointment();

            // Initialize fields we are testing against
            startTime = new DateTime();
            endTime = new DateTime();
        }

        [TestMethod]
        public void TestGettersAndSetters()
        {
            Assert.AreEqual(startTime.Year, appointment.StartTime.Year);
            Assert.AreEqual(endTime.Year, appointment.EndTime.Year);
        }
    }
}
