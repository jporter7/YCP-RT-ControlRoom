using ControlRoomApplication.Entities;
using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ControlRoomApplicationTest
{
    [TestClass]
    public class ScheduleTest
    {
        private Schedule schedule;
        private DateTime currentDateTime;
        private const double TIME_DELTA = 5.0;

        [TestInitialize]
        public void BuildUp()
        {
            schedule = new Schedule();
            currentDateTime = DateTime.Now;
        }

        [TestMethod]
        public void TestSettersAndGetters()
        {
            Assert.AreEqual(0, schedule.Appointments.Count);
            Assert.AreEqual(currentDateTime.Millisecond, schedule.CurrentDateTime.Millisecond, TIME_DELTA);
        }
    }
}
