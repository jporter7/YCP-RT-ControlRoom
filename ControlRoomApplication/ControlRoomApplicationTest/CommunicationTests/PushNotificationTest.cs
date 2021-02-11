using ControlRoomApplication.Controllers.Communications;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;

namespace ControlRoomApplicationTest.CommunicationTests
{
    [TestClass]
    public class PushNotificationTest
    {
        [TestMethod]
        public void TestSendPushNotificationsToAllAdmins()
        {
            // Execute task
            Task<bool> task = pushNotification.sendToAllAdmins("TEST", "This should pass.");

            // Wait for task to complete so result is up to date
            task.Wait();

            Assert.IsTrue(task.Result);
        }
    }
}
