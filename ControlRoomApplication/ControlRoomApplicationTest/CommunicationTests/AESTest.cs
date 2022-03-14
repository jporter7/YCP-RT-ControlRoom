using ControlRoomApplication.Controllers.Communications;
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
        private byte[] testKey = { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16 };
        private byte[] testIv = { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16 };

        [TestMethod]
        public void TestDecryption()
        {
            string rawText = "James Moscola lied about his retirement for some reason.";
            string decryptedText = AES.Decrypt(AES.Encrypt(rawText, testKey, testIv), testKey, testIv);
            Assert.IsTrue(rawText.Equals(decryptedText));
        }
    }
}
