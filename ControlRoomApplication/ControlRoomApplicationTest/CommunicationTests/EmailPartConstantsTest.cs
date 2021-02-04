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
            string html = $@"<html>
<head></head>
<body>
    <h1>{subject}</h1>
    <p>{text}</p>
</body>
</html>";

            EmailFields epc = new EmailFields(sender, subject, text);

            Assert.IsTrue(EmailFields.Sender == sender);
            Assert.IsTrue(EmailFields.Subject == subject);
            Assert.IsTrue(EmailFields.Text == text);
            Assert.IsTrue(EmailFields.Html == html);
        }

        [TestMethod]
        public void TestSetSender()
        {
            string sender = "test.email@test.email.com";
            EmailFields.setSender(sender);

            Assert.IsTrue(EmailFields.Sender == sender);
        }

        [TestMethod]
        public void TestSetSubject()
        {
            string subject = "test subject";
            EmailFields.setSubject(subject);

            Assert.IsTrue(EmailFields.Subject == subject);
        }

        [TestMethod]
        public void TestSetText()
        {
            string text = "Test text\n";
            EmailFields.setText(text);

            Assert.IsTrue(EmailFields.Text == text);
        }

        [TestMethod]
        public void TestGenerateHtml()
        {
            string sender = "sender";
            string subject = "subject";
            string body = "body";

            string expectedHtml = $@"<html>
<head></head>
<body>
    <h1>{subject}</h1>
    <p>{body}</p>
</body>
</html>";

            EmailFields email = new EmailFields(sender, subject, body);

            Assert.IsTrue(EmailFields.Html.Equals(expectedHtml));
        }

    }
}
