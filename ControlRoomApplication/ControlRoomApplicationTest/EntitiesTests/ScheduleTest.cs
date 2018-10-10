using ControlRoomApplication.Entities;
using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ControlRoomApplicationTest.EntitiesTests
{
    [TestClass]
    public class ScheduleTest
    {
        private Schedule schedule;
        private DateTime currentDateTime;

        [TestInitialize]
        public void BuildUp()
        {
            schedule = new Schedule();
            currentDateTime = new DateTime();
        }

        [TestMethod]
        public void TestSettersAndGetters()
        {
            Assert.AreEqual(0, schedule.Appointments.Count);
            Assert.AreEqual(currentDateTime.Year, schedule.CurrentDateTime.Year);
        }
    }
}
