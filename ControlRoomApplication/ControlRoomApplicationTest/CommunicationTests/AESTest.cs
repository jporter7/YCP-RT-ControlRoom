using ControlRoomApplication.Controllers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ControlRoomApplication.Controllers.Communications.Encryption;

namespace ControlRoomApplicationTest.CommunicationTests
{
    [TestClass]
    public class AESTest
    {
        [TestMethod]
        public void TestAES_Interlace_4Chars()
        {
            String input = "Test";

            String result = AES.Lace(input);

            // Result should have twice length as input
            Assert.IsTrue(result.Length == input.Length * 2);
        }

        [TestMethod]
        public void TestAES_Interlace_NoChars()
        {
            String input = "";

            String result = AES.Lace(input);

            // Result should still have no length
            Assert.IsTrue(result.Length == input.Length * 2);
        }

        [TestMethod]
        public void TestAES_UnInterlace_4Chars()
        {
            String input = "Test";

            String result = AES.UnLace(AES.Lace(input));

            // Plaintext should be the same because interlace was removed
            Assert.AreEqual(input, result);
        }

        [TestMethod]
        public void TestAES_UnInterlace_NoChars()
        {
            String input = "";

            String result = AES.UnLace(AES.Lace(input));

            // Plaintext should be the same because interlace was removed
            Assert.AreEqual(input, result);
        }

        [TestMethod]
        public void TestAES_GenerateTimestamp_4Chars()
        {
            String input = "Test";

            String result = AES.generateTimestamp(input);

            // Timestamp should be generated to reflect current time
            Assert.AreEqual(DateTime.Now.ToString("MM/dd/yy HH:mm:ss") + "," + input, result);
        }

        [TestMethod]
        public void TestAES_GenerateTimestamp_NoChars()
        {
            String input = "";

            String result = AES.generateTimestamp(input);

            // Timestamp should be generated to reflect current time
            Assert.AreEqual(DateTime.Now.ToString("MM/dd/yy HH:mm:ss") + "," + input, result);
        }

        [TestMethod]
        public void TestAES_VerifyAndRemoveTimestamp_TooOldBy1Day()
        {
            // Prepend timestamp to input
            String input = DateTime.Now.AddDays(-1).ToString("MM/dd/yy HH:mm:ss") + ",Test";

            String result = AES.verifyAndRemoveTimestamp(input);

            Assert.AreEqual("ERROR", result);
        }

        [TestMethod]
        public void TestAES_VerifyAndRemoveTimestamp_TooNewBy1Day()
        {
            // Prepend timestamp to input
            String input = DateTime.Now.AddDays(1).ToString("MM/dd/yy HH:mm:ss") + ",Test";

            String result = AES.verifyAndRemoveTimestamp(input);

            Assert.AreEqual("ERROR", result);
        }

        [TestMethod]
        public void TestAES_VerifyAndRemoveTimestamp_TooOldBy1Second()
        {
            // Prepend timestamp to input
            String input = DateTime.Now.AddSeconds(-16).ToString("MM/dd/yy HH:mm:ss") + ",Test";

            String result = AES.verifyAndRemoveTimestamp(input);

            Assert.AreEqual("ERROR", result);
        }

        [TestMethod]
        public void TestAES_VerifyAndRemoveTimestamp_TooNewBy16Seconds()
        {
            // Prepend timestamp to input
            String input = DateTime.Now.AddSeconds(16).ToString("MM/dd/yy HH:mm:ss") + ",Test";

            String result = AES.verifyAndRemoveTimestamp(input);

            Assert.AreEqual("ERROR", result);
        }

        [TestMethod]
        public void TestAES_VerifyAndRemoveTimestamp_TooNewBy1Second()
        {
            // Prepend timestamp to input
            String input = DateTime.Now.AddSeconds(-16).ToString("MM/dd/yy HH:mm:ss") + ",Test";

            String result = AES.verifyAndRemoveTimestamp(input);

            Assert.AreEqual("ERROR", result);
        }

        [TestMethod]
        public void TestAES_VerifyAndRemoveTimestamp_OK14Seconds()
        {
            String plaintext = "Test";

            // Prepend timestamp to input
            String input = DateTime.Now.AddSeconds(-14).ToString("MM/dd/yy HH:mm:ss") + "," + plaintext;

            String result = AES.verifyAndRemoveTimestamp(input);

            Assert.AreEqual(plaintext, result);
        }

        [TestMethod]
        public void TestAES_VerifyAndRemoveTimestamp_OK1Second()
        {
            String plaintext = "Test";

            // Prepend timestamp to input
            String input = DateTime.Now.AddSeconds(-1).ToString("MM/dd/yy HH:mm:ss") + "," + plaintext;

            String result = AES.verifyAndRemoveTimestamp(input);

            Assert.AreEqual(plaintext, result);
        }

        [TestMethod]
        public void TestAES_VerifyAndRemoveTimestamp_OK()
        {
            String plaintext = "Test";

            // Prepend timestamp to input
            String input = DateTime.Now.ToString("MM/dd/yy HH:mm:ss") + "," + plaintext;

            String result = AES.verifyAndRemoveTimestamp(input);

            Assert.AreEqual(plaintext, result);
        }

        /**
         * The following VerifyAndRemoveTimestamp tests will verify that any broken data received
         * will return an error
         * */

        [TestMethod]
        public void TestAES_VerifyAndRemoveTimestamp_MoreThanOneComma()
        {
            String plaintext = "Test,";

            // Prepend timestamp to input
            String input = DateTime.Now.ToString("MM/dd/yy HH:mm:ss") + "," + plaintext;

            String result = AES.verifyAndRemoveTimestamp(input);

            Assert.AreEqual("ERROR", result);
        }

        [TestMethod]
        public void TestAES_VerifyAndRemoveTimestamp_NoComma()
        {
            String plaintext = "Test";

            // Prepend timestamp to input
            String input = DateTime.Now.ToString("MM/dd/yy HH:mm:ss") + plaintext;

            String result = AES.verifyAndRemoveTimestamp(input);

            Assert.AreEqual("ERROR", result);
        }

        [TestMethod]
        public void TestAES_VerifyAndRemoveTimestamp_JunkData()
        {
            String plaintext = "Test";

            // Prepend junk data to input
            String input = "12/535/36 3r3:3k," + plaintext;

            String result = AES.verifyAndRemoveTimestamp(input);

            Assert.AreEqual("ERROR", result);
        }

        [TestMethod]
        public void TestAES_VerifyAndRemoveTimestamp_JunkData2()
        {
            String plaintext = "Test";

            // Prepend junk data to input
            String input = "// ::," + plaintext;

            String result = AES.verifyAndRemoveTimestamp(input);

            Assert.AreEqual("ERROR", result);
        }

        [TestMethod]
        public void TestAES_EncryptAndDecrypt_4Chars()
        {
            String input = "Test";

            String result = AES.Decrypt(AES.Encrypt(input));

            Assert.AreEqual(input, result);
        }

        [TestMethod]
        public void TestAES_EncryptAndDecrypt_200Chars()
        {
            // This is used to generate a String of 200 characters, because
            // C# has no built-in <String>.repeat function
            String input;
            for (input = ""; input.Length < 200; input = input + "A") { }

            String result = AES.Decrypt(AES.Encrypt(input));

            Assert.AreEqual(input, result);
        }

        [TestMethod]
        public void TestAES_EncryptAndDecrypt_NoChars()
        {
            String input = "";

            String result = AES.Decrypt(AES.Encrypt(input));

            Assert.AreEqual(input, result);
        }

        [TestMethod]
        public void TestAES_GetNewKeys_OnlyTwoKeys()
        {
            Assert.IsTrue(AES.getNewKeys().Count == 2);
        }

        [TestMethod]
        public void TestAES_GetNewKeys_KeysAreCorrectSize()
        {
            var keys = AES.getNewKeys();

            // With AES-128, we only want 16-byte keys
            Assert.IsTrue(keys[0].Length == 16);
            Assert.IsTrue(keys[1].Length == 16);
        }

        [TestMethod]
        public void TestAES_GetNewKeys_KeysAreRandom()
        {
            var keys = AES.getNewKeys();
            var keys2 = AES.getNewKeys();

            Assert.AreNotEqual(keys[0], keys2[0]);
            Assert.AreNotEqual(keys[1], keys2[1]);
        }
    }
}
