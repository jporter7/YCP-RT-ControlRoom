using ControlRoomApplication.Constants;
using ControlRoomApplication.Controllers.Communications;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControlRoomApplicationTest.CommunicationTests
{
    [TestClass]
    public class AESTest
    {
        private readonly byte[] testKey = { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16 };
        private readonly byte[] testIv = { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16 };

        [TestMethod]
        public void TestDecryption()
        {
            string decryptedText = "1.1|SCRIPT|FULL_CLOCK|2022-02-28T17:43:17.164Z";
            string encryptedText = "5874a5baf786e39e95c35bbf35cee9bb4ae68454ca794c3fafafb5f66ae3c661f9a992520b00e50e1efb4edc97da";

            string testDecryptedText = "1.1" + "|" + AES.Decrypt(encryptedText, AESConstants.KEY, AESConstants.IV);

            Assert.IsTrue(decryptedText.Equals(testDecryptedText));
        }
    }
}
