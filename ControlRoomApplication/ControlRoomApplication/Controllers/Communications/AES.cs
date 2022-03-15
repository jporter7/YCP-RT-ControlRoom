using ControlRoomApplication.Util;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace ControlRoomApplication.Controllers.Communications
{
        public static class AES
        {
            public static string Encrypt(string plainText, byte[] key, byte[] iv)
            {
                byte[] plainTextBytes = ASCIIEncoding.ASCII.GetBytes(plainText);

                AesCryptoServiceProvider endec = new AesCryptoServiceProvider();

                endec.BlockSize = 128;
                endec.KeySize = 256;
                endec.IV = iv;
                endec.Key = key;
                endec.Padding = PaddingMode.None;
                endec.Mode = CipherMode.CFB;

                ICryptoTransform icrypt = endec.CreateEncryptor(endec.Key, endec.IV);

                byte[] encrypted = icrypt.TransformFinalBlock(plainTextBytes, 0, plainTextBytes.Length);

                icrypt.Dispose();

                return Utilities.ByteArrayToHexString(encrypted);
            }
            public static string Decrypt(string cipherText, byte[] key, byte[] IV)
            {
                byte[] cipherTextBytes = Utilities.HexStringToByteArray(cipherText);

                AesCryptoServiceProvider endec = new AesCryptoServiceProvider();

                endec.BlockSize = 128;
                endec.KeySize = 256;
                endec.IV = IV;
                endec.Key = key;
                endec.Padding = PaddingMode.None;
                endec.Mode = CipherMode.CFB;

                System.Diagnostics.Debug.WriteLine(endec.Padding);

                ICryptoTransform icrypt = endec.CreateDecryptor(endec.Key, endec.IV);

                byte[] dec = icrypt.TransformFinalBlock(cipherTextBytes, 0, cipherTextBytes.Length);

                icrypt.Dispose();

                return Encoding.ASCII.GetString(dec);
            }
        }
}

