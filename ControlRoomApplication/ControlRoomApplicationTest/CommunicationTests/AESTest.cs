using ControlRoomApplication.Constants;
using ControlRoomApplication.Controllers.Communications;
using ControlRoomApplication.Util;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControlRoomApplicationTest.CommunicationTests
{
    [TestClass]
    public class AESTest
    {
        private TestContext testContext;

        public TestContext TestContext
        {
            get { return testContext; }
            set { testContext = value; }
        }

        [TestMethod]
        public void TestEncryptionDecryption()
        {
            string rawText = "1.1|SCRIPT|FULL_CLOCK|2022-03-16T20:18:03.636Z";
            string decrypted = AES.Decrypt(AES.Encrypt(rawText, AESConstants.KEY, AESConstants.IV), AESConstants.KEY, AESConstants.IV);

            Assert.IsTrue(rawText == decrypted);
        }

        [TestMethod]
        public void TestRemoteDecryption()
        {
            string command = "1.1|STOP_RT|2022-03-20T22:15:03.660Z";
            string encrypted = "AA9uv3O7ov+eZ2xM9478QpgOxSBhBbyYMf21krHQMZLAdnaAqGwJ2GkZcT8hxE7T";
            string decrypted = AES.Decrypt(encrypted, AESConstants.KEY, AESConstants.IV);

            Assert.IsTrue(decrypted.Equals(command));
        }

        [TestMethod]
        public void SendDylanSomething()
        {
            string text = "This better work or imma jump";

            text = String.Concat(text.Where(c => !Char.IsWhiteSpace(c)));
            
            string encrypted = AES.Encrypt(text, AESConstants.KEY, AESConstants.IV);

            testContext.WriteLine("Sending: {0}", text);
            testContext.WriteLine("Key: {0}", AESConstants.KEY);
            testContext.WriteLine("IV: {0}", AESConstants.IV);

            testContext.WriteLine("Encrypted: {0}", encrypted);

            testContext.WriteLine("Decrypted: {0}", AES.Decrypt(encrypted, AESConstants.KEY, AESConstants.IV));

        }
    }
}
