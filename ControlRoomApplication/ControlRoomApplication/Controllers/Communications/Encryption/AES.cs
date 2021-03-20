using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using ControlRoomApplication.Util;


namespace ControlRoomApplication.Controllers.Communications.Encryption
{
    public class AES
    {
        private static readonly log4net.ILog logger =
            log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public static byte[] Encrypt(string plainText)
        {
            // Retrieve keys, located in the same directory as the Control Room executable
            byte[] Key = retrieveExistingKey();
            byte[] IV = retrieveExistingIv();

            // Verify key and IV are not null
            if (Key == null || IV == null)
            {
                return new byte[0];
            }

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

            // Append 255 byte to the end of the array
            byte[] encryptedMarked = new byte[encrypted.Length + 1];
            encrypted.CopyTo(encryptedMarked, 0);
            encryptedMarked[encryptedMarked.Length - 1] = 255;

            // Return encrypted data    
            return encryptedMarked;
        }
        public static string Decrypt(byte[] cipherText)
        {
            // Retrieve keys, located in the same directory as the Control Room executable
            byte[] Key = retrieveExistingKey();
            byte[] IV = retrieveExistingIv();

            // Verify key and IV are not null
            if (Key == null || IV == null)
            {
                return "Decryption error: Keys were found to be invalid.";
            }

            string plaintext = null;

            // First remove any trailing zeroes
            byte[] cipherTextNoZeroes = removeTrailingZeroes(cipherText);

            // In case the data received is invalid, we want to return an error.
            try
            {
                // Create AesManaged    
                using (AesManaged aes = new AesManaged())
                {
                    // Create a decryptor    
                    ICryptoTransform decryptor = aes.CreateDecryptor(Key, IV);
                    using (MemoryStream ms = new MemoryStream(cipherTextNoZeroes))
                    {
                        using (CryptoStream cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read))
                        {
                            // convert stream to String
                            using (StreamReader reader = new StreamReader(cs))
                                plaintext = reader.ReadToEnd();
                        }
                    }
                }
            }
            catch
            {
                logger.Info(Utilities.GetTimeStamp() + ": Encryption error: Invalid byte array for decryption.");
                return "Error: Invalid byte array for decryption.";
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
                (DateTime.Compare(date, DateTime.Now.AddSeconds(-30)) > 0) &&
                
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

        // Retrieves an existing key from a directory with error checks
        private static byte[] retrieveExistingKey()
        {
            byte[] key = new byte[0];

            // Error check key
            try
            {
                key = File.ReadAllBytes("AESKey.bin");
                if (key.Length != 32)
                {
                    logger.Info(Utilities.GetTimeStamp() + ": Encryption error: AESKey.bin does not contain 32 bytes.");
                    return null;
                }
            }
            catch
            {
                logger.Info(Utilities.GetTimeStamp() + ": Encryption error: Missing AESKey.bin.");
                return null;
            }

            return key;
        }

        private static byte[] retrieveExistingIv()
        {
            byte[] iv = new byte[0];

            // Error check IV
            try
            {
                iv = File.ReadAllBytes("IV.bin");
                if (iv.Length != 16)
                {
                    logger.Info(Utilities.GetTimeStamp() + ": Encryption error: IV.bin does not contain 16 bytes.");
                    return null;
                }
            }
            catch
            {
                logger.Info(Utilities.GetTimeStamp() + ": Encryption error: Missing IV.bin.");
                return null;
            }

            return iv;
        }

        // In list index 0: Key
        // In list index 1: IV
        // Returns randomly-generated new AES keys
        public static List<byte[]> getNewKeys()
        {
            List<byte[]> keys = new List<byte[]>();

            using (Aes aes = Aes.Create())
            {
                aes.KeySize = 256;
                aes.GenerateKey();
                aes.GenerateIV();
                keys.Add(aes.Key);
                keys.Add(aes.IV);
            }

            return keys;
        }

        // This function uses 255 as a marker to know when a byte array's data ends
        // and trailing zeroes begin
        public static byte[] removeTrailingZeroes(byte[] withZeroes)
        {
            // If there are no zeroes, return original array without marker
            if (withZeroes[withZeroes.Length - 1] == 255)
            {
                var noMarker = new byte[withZeroes.Length - 1];
                Array.Copy(withZeroes, noMarker, withZeroes.Length - 1);
                return noMarker;
            }
            else
            {
                // Find index of the last non-zero value
                int i = withZeroes.Length - 1;
                while (withZeroes[i] == 0) i--;

                // Copy only the beginning elements before trailing zeroes to new array
                var noZeroes = new byte[i];
                Array.Copy(withZeroes, noZeroes, i);

                return noZeroes;
            }
        }

    }
}
