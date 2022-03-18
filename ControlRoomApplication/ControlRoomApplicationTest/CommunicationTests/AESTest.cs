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

            decrypted = Utilities.RemoveCommandPadding(decrypted);

            Assert.IsTrue(rawText == decrypted);
        }

        [TestMethod]
        public void TestDecryption()
        {
            string encrypted = "37da34fd5c3e13eeeef2b72a2aba12969fc8302efbf119cf765e8a620bf7665eb19c61215b1af90329d7a22f36abe0a7";
            string rawText = "1.1|SCRIPT|FULL_EV|2022-03-18T02:06:33.831Z";
            string decrypted = AES.Decrypt(encrypted, AESConstants.KEY, AESConstants.IV);

            decrypted = Utilities.RemoveCommandPadding(decrypted);

            testContext.WriteLine(decrypted);
            Assert.IsTrue(rawText.Equals(decrypted));
        }

        [TestMethod]
        public void SendDylanSomething()
        {
            string text = "This better work or imma jump";

            text = String.Concat(text.Where(c => !Char.IsWhiteSpace(c)));
            
            /*
            while (text.Length % 16 != 0)
            {
                text += "*";
            }
            */
            
            string encrypted = AES.Encrypt(text, AESConstants.KEY, AESConstants.IV);

            testContext.WriteLine("Sending: {0}", text);

            testContext.WriteLine(encrypted);

            testContext.WriteLine("Decrypted: {0}", AES.Decrypt(encrypted, AESConstants.KEY, AESConstants.IV));

        }
    }
}
