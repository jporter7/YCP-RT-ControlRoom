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
        /**
         * Unfortunately, most of this functionality is handled outside of the control room,
         * so we can only reasonably test that the file itself is present and the required
         * methods are there. If there are any discrepencies, this test will not run at all.
         * If everything is present, it will pass.
         * */

        [TestMethod]
        public void TestPushNotificationFilePresence()
        {
            Assert.IsTrue(pushNotification.send("TEST", "This should pass."));
        }
    }
}
