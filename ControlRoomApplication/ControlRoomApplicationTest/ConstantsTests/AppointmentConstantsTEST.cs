using ControlRoomApplication.Constants;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ControlRoomApplicationTest.ConstantsTests
{
    [TestClass]
    public class AppointmentConstantsTest
    {
        [TestMethod]
        public void TestAppointmentConstants()
        {
            Assert.AreEqual(AppointmentConstants.REQUESTED, "REQUESTED");
            Assert.AreEqual(AppointmentConstants.SCHEDULED, "SCHEDULED");
            Assert.AreEqual(AppointmentConstants.IN_PROGRESS, "IN_PROGRESS");
            Assert.AreEqual(AppointmentConstants.CANCELLED, "CANCELLED");
            Assert.AreEqual(AppointmentConstants.COMPLETED, "COMPLETED");
        }
    }
}
