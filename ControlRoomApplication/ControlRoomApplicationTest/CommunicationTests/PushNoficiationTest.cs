using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ControlRoomApplication.Controllers.Communications;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ControlRoomApplicationTest.CommunicationTests
{
    /**
     * WARNING: I highly recommend setting the sendPush variable at the top of
     * pushNotification.cs to false, that way admins don't get a push notification
     * every time these tests are run.
     * */

    [TestClass]
    public class PushNoficiationTest
    {
        [TestMethod]
        public void TestSendPushNotification()
        {
            Assert.IsTrue(pushNotification.send("TEST", "This should pass."));
        }
    }
}
