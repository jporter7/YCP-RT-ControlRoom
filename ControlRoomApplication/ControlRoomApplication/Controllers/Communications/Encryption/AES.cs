using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace ControlRoomApplication.Controllers.Communications.Encryption
{
    public class AES
    {
        public static byte[] Encrypt(string plainText)
        {
            // Retrieve keys, located in the same directory as the Control Room executable
            byte[] Key = File.ReadAllBytes("AESKey.bin");
            byte[] IV = File.ReadAllBytes("IV.bin");

            byte[] encrypted;

            // Prepend a timestamp and then interlace the text
            plainText = Lace(generateTimestamp(plainText));

            // Create a new AesManaged.    
            using (AesManaged aes = new AesManaged())
            {
                // Create encryptor    
                ICryptoTransform encryptor = aes.CreateEncryptor(Key, IV);

                using (MemoryStream ms = new MemoryStream())
                {    
                    using (CryptoStream cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
                    {
                        // Create StreamWriter and write memory stream to a byte array    
                        using (StreamWriter sw = new StreamWriter(cs))
                            sw.Write(plainText);
                        encrypted = ms.ToArray();
                    }
                }
            }

            // Return encrypted data    
            return encrypted;
        }
        public static string Decrypt(byte[] cipherText)
        {
            // Retrieve keys, located in the same directory as the Control Room executable
            byte[] Key = File.ReadAllBytes("AESKey.bin");
            byte[] IV = File.ReadAllBytes("IV.bin");
            string plaintext = null;

            // Create AesManaged    
            using (AesManaged aes = new AesManaged())
            {
                // Create a decryptor    
                ICryptoTransform decryptor = aes.CreateDecryptor(Key, IV);
                using (MemoryStream ms = new MemoryStream(cipherText))
                {
                    using (CryptoStream cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read))
                    {
                        // convert stream to String
                        using (StreamReader reader = new StreamReader(cs))
                            plaintext = reader.ReadToEnd();
                    }
                }
            }

            // Unlace the text, then verify/remove the timestamp
            return verifyAndRemoveTimestamp(UnLace(plaintext));
        }

        // Inserts a random ASCII character in all ODD positions of the String
        public static string Lace(String text)
        {
            String laced = "";
            Random randASCII = new Random();

            foreach (char c in text)
            {
                laced += c + "" + (char)randASCII.Next(129);
            }

            return laced;
        }

        // Removes all ASCII characters at ODD positions of the String
        public static string UnLace(String text)
        {
            String unlaced = "";

            for (int i = 0; i < text.Length; i += 2) unlaced += text[i];

            return unlaced;
        }

        // Prepends date/timestamp to command
        public static string generateTimestamp(String text)
        {
            return DateTime.Now.ToString("MM/dd/yy HH:mm:ss") + "," + text;
        }

        // Verifies that the date/time is within the timeout time
        public static string verifyAndRemoveTimestamp(String text)
        {
            // Split date (0) and command (1) apart
            string[] half = text.Split(',');

            // Convert date String to date object
            DateTime date;

            // only adds date segment if it is a valid DateTime
            if (!DateTime.TryParse(half[0], out date)) return "ERROR";

            if (
                // accept date from up to 15 seconds ago
                (DateTime.Compare(date, DateTime.Now.AddSeconds(-15)) > 0) &&
                
                // incoming date must be BEFORE now
                (DateTime.Compare(date, DateTime.Now) <= 0) && 


                /* NOW ACCOUNTING FOR BROKEN DATA */

                // number of segments MUST be 2
                (half.Length == 2)
                )
            {
                return half[1];
            }
            else return "ERROR";
        }

    }
}
