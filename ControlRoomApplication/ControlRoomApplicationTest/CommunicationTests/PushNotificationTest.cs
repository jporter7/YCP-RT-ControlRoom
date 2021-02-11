using ControlRoomApplication.Controllers.Communications;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ControlRoomApplicationTest.CommunicationTests
{
    [TestClass]
    public class PushNotificationTest
    {

        [TestMethod]
        public void TestPushNotificationFilePresence()
        {
            Assert.IsTrue(pushNotification.sendToAllAdmins("TEST", "This should pass."));
        }

        [TestMethod]
        public void TestSendPushNotification()
        {
            // If sendPush is false, set it to true
            if (!pushNotification.getSendPush()) pushNotification.setSendPush(true);

            Assert.IsTrue(pushNotification.sendToAllAdmins("TEST", "This should pass."));
        }
    }
}
