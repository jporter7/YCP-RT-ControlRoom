using ControlRoomApplication.Controllers.Communications;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ControlRoomApplicationTest.CommunicationTests
{
    [TestClass]
    public class EmailPartConstantsTest
    {
        [TestMethod]
        public void TestConstructor()
        {
            string sender = "test.email@test.email.com";
            string subject = "test";
            string text = "This is a test\n";
            string html = @"<html>
<head></head>
<body>
    <h1>This is a test</h1>
</body>
</html";

            EmailPartConstants epc = new EmailPartConstants(sender, subject, text, html);

            Assert.IsTrue(EmailPartConstants.Sender == sender);
            Assert.IsTrue(EmailPartConstants.Subject == subject);
            Assert.IsTrue(EmailPartConstants.Text == text);
            Assert.IsTrue(EmailPartConstants.Html == html);
        }

        [TestMethod]
        public void TestSetSender()
        {
            string sender = "test.email@test.email.com";
            EmailPartConstants.setSender(sender);

            Assert.IsTrue(EmailPartConstants.Sender == sender);
        }

        [TestMethod]
        public void TestSetSubject()
        {
            string subject = "test subject";
            EmailPartConstants.setSubject(subject);

            Assert.IsTrue(EmailPartConstants.Subject == subject);
        }

        [TestMethod]
        public void TestSetText()
        {
            string text = "Test text\n";
            EmailPartConstants.setText(text);

            Assert.IsTrue(EmailPartConstants.Text == text);
        }

        [TestMethod]
        public void TestSetHtml()
        {
            string html = @"<html>
<head></head>
<body>
    <h1>This is a test</h1>
</body>
</html";
            EmailPartConstants.setHtml(html);

            Assert.IsTrue(EmailPartConstants.Html == html);
        }

    }
}
