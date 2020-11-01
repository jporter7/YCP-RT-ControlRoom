using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ControlRoomApplication.Controllers.Communications;
using ControlRoomApplication.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ControlRoomApplicationTest.CommunicationTests
{

    [TestClass]
    public class PushNotificationTest
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

        [TestMethod]
        public void TestSendPushNotification()
        {
            // If sendPush is false, set it to true
            if (!pushNotification.getSendPush()) pushNotification.setSendPush(true);

            Assert.IsTrue(pushNotification.send("TEST", "This should pass."));
        }

        [TestMethod]
        public void TestSendingEmail()
        {
            EmailFields.setSender("SystemTest@ycpradiotelescope.com");
            EmailFields.setSubject("Amazon SES Test");
            EmailFields.setText("AmazonSES Test (.NET)\r\nThis email was sent through AmazonSES using the AWS SDK for .NET.");
            EmailFields.setHtml(@"<html>
<head></head>
<body>
  <h1>Amazon SES Test (AWS SDK for .NET)</h1>
  <p>This email was sent with
    <a href='https://aws.amazon.com/ses/'>Amazon SES</a> using the
    <a href='https://aws.amazon.com/sdk-for-net/'>
      AWS SDK for .NET</a>.</p>
</body>
</html>");
        Assert.IsTrue(pushNotification.sendEmail(true));
        }
    }
}
