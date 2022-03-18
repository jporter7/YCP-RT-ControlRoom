using ControlRoomApplication.Util;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Org.BouncyCastle.Crypto;

namespace ControlRoomApplication.Controllers.Communications
{
    public static class AES
    {
        /*
        public static byte[] Encrypt(string plainText, byte[] Key, byte[] IV)
            {
            // Check arguments.
            if (plainText == null || plainText.Length <= 0)
                throw new ArgumentNullException("plainText");
            if (Key == null || Key.Length <= 0)
                throw new ArgumentNullException("Key");
            if (IV == null || IV.Length <= 0)
                throw new ArgumentNullException("IV");
            byte[] encrypted;
            // Create an RijndaelManaged object
            // with the specified key and IV.
            using (RijndaelManaged rijAlg = new RijndaelManaged())
            {
                rijAlg.Mode = CipherMode.OFB;
                rijAlg.Padding = PaddingMode.None;
                rijAlg.Key = Key;
                rijAlg.IV = IV;
                rijAlg.KeySize = 128;

                // Create an encryptor to perform the stream transform.
                ICryptoTransform encryptor = rijAlg.CreateEncryptor(rijAlg.Key, rijAlg.IV);

                // Create the streams used for encryption.
                using (MemoryStream msEncrypt = new MemoryStream())
                {
                    using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                        {

                            //Write all data to the stream.
                            swEncrypt.Write(plainText);
                        }
                        encrypted = msEncrypt.ToArray();
                    }
                }
            }

            // Return the encrypted bytes from the memory stream.
            return encrypted;
        }

        public static string Decrypt(byte[] cipherText, byte[] Key, byte[] IV)
        {
            // Check arguments.
            if (cipherText == null || cipherText.Length <= 0)
                throw new ArgumentNullException("cipherText");
            if (Key == null || Key.Length <= 0)
                throw new ArgumentNullException("Key");
            if (IV == null || IV.Length <= 0)
                throw new ArgumentNullException("IV");

            // Declare the string used to hold
            // the decrypted text.
            string plaintext = null;

            // Create an RijndaelManaged object
            // with the specified key and IV.
            using (RijndaelManaged rijAlg = new RijndaelManaged())
            {
                rijAlg.Mode = CipherMode.OFB;
                rijAlg.Padding = PaddingMode.None;
                rijAlg.Key = Key;
                rijAlg.IV = IV;
                rijAlg.KeySize = 128;

                // Create a decryptor to perform the stream transform.
                ICryptoTransform decryptor = rijAlg.CreateDecryptor(rijAlg.Key, rijAlg.IV);

                // Create the streams used for decryption.
                using (MemoryStream msDecrypt = new MemoryStream(cipherText))
                {
                    using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                        {
                            // Read the decrypted bytes from the decrypting stream
                            // and place them in a string.
                            plaintext = srDecrypt.ReadToEnd();
                        }
                    }
                }

                return plaintext;
            }
        */
        public static string Encrypt(string plainText, byte[] key, byte[] iv)
        {
            byte[] plainTextBytes = ASCIIEncoding.ASCII.GetBytes(plainText);

            AesCryptoServiceProvider endec = new AesCryptoServiceProvider();

            endec.Mode = CipherMode.CBC;
            //endec.Padding = PaddingMode.None;
            endec.Padding = PaddingMode.PKCS7;
            endec.IV = iv;
            endec.Key = key;

            ICryptoTransform icrypt = endec.CreateEncryptor(endec.Key, endec.IV);

            byte[] encrypted = icrypt.TransformFinalBlock(plainTextBytes, 0, plainTextBytes.Length);

            icrypt.Dispose();

            return Utilities.ByteArrayToHexString(encrypted);
        }
        public static string Decrypt(string cipherText, byte[] key, byte[] IV)
        {
            byte[] cipherTextBytes = Utilities.HexStringToByteArray(cipherText);

            AesCryptoServiceProvider endec = new AesCryptoServiceProvider();

            endec.Mode = CipherMode.CBC;
            //endec.Padding = PaddingMode.Zeros;
            endec.Padding = PaddingMode.PKCS7;
            endec.IV = IV;
            endec.Key = key;

            ICryptoTransform icrypt = endec.CreateDecryptor(endec.Key, endec.IV);

            byte[] dec = icrypt.TransformFinalBlock(cipherTextBytes, 0, cipherTextBytes.Length);

            icrypt.Dispose();

            return Encoding.ASCII.GetString(dec);
        }
    }
}
