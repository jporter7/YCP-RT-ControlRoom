using ControlRoomApplication.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace ControlRoomApplicationTest.EntitiesTests
{
    [TestClass]
    public class AppointmentTest
    {
        // Class being tested
        private Appointment appointment_1;
        private Appointment appointment_2;

        // Attributes in the class that need to be tested
        private DateTime startTime_1;
        private DateTime endTime_1;
        private DateTime startTime_2;
        private DateTime endTime_2;

        [TestInitialize]
        public void BuildUp()
        {
            // Initialize appointment entity
            appointment_1 = new Appointment();
            appointment_2 = new Appointment();

            // Initialize fields we are testing against
            startTime_1 = DateTime.Now;
            endTime_1 = DateTime.Now.AddHours(1);
            startTime_2 = DateTime.Now.AddDays(1);
            endTime_2 = DateTime.Now.AddDays(1).AddHours(1);

            // add start and end times to appointment
            appointment_1.StartTime = startTime_1;
            appointment_1.EndTime = endTime_1;
            appointment_2.StartTime = startTime_2;
            appointment_2.EndTime = endTime_2;
        }

        [TestMethod]
        public void TestGettersAndSetters()
        {
            Assert.AreEqual(startTime_1.Year, appointment_1.StartTime.Year);
            Assert.AreEqual(endTime_1.Year, appointment_1.EndTime.Year);
        }

        [TestMethod]
        public void TestComparable()
        {
            Assert.IsTrue(appointment_1 != appointment_2);
            Assert.IsTrue(appointment_1 < appointment_2);
        }
    }
}
