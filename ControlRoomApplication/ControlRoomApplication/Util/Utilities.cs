using System;
using System.Collections.Generic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using ControlRoomApplication.Constants;
using ControlRoomApplication.Util;
using System.Windows.Forms;

namespace ControlRoomApplication.Util
{
    
    public class Utilities
    {
        public static object timeZoneName;

        public static String GetTimeStamp()
        {
            string timeZone = string.Empty;
            string timeZoneString = TimeZone.CurrentTimeZone.IsDaylightSavingTime(DateTime.Now)
                                      ? TimeZone.CurrentTimeZone.StandardName
                                      : TimeZone.CurrentTimeZone.DaylightName;
            
            string[] timeZoneWords = timeZoneString.Split(' ');
            foreach (string timeZoneWord in timeZoneWords)
            {
                if (timeZoneWord[0] != '(')
                {
                    timeZone += timeZoneWord[0];
                }
                else
                {
                    timeZone += timeZoneWord;
                }
            }

            timeZone = timeZone.ToUpper().Trim();
            string dateTime = (DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " " + timeZone);

            return dateTime;
        }

        /// <summary>
        /// This allows us to write to the user interface from a thread other than the thread that created
        /// the user interface.
        /// </summary>
        /// <typeparam name="T">The form class name.</typeparam>
        /// <param name="writeTo">What form class object.</param>
        /// <param name="codeBlock">The code that changes the UI component.</param>
        public static void WriteToGUIFromThread<T>(T writeTo, Action codeBlock) where T:Form
        {
            if (writeTo.InvokeRequired)
            {
                IAsyncResult result = writeTo.BeginInvoke(new MethodInvoker(delegate ()
                {
                    codeBlock();
                }));
            }
            else if (writeTo.IsHandleCreated)
            {
                codeBlock();
            }
        }

        public static string ByteArrayToHexString(byte[] Bytes)
        {
            StringBuilder Result = new StringBuilder(Bytes.Length * 2);
            string HexAlphabet = "0123456789ABCDEF";

            foreach (byte B in Bytes)
            {
                Result.Append(HexAlphabet[(int)(B >> 4)]);
                Result.Append(HexAlphabet[(int)(B & 0xF)]);
            }

            return Result.ToString();
        }

        public static byte[] HexStringToByteArray(string Hex)
        {
            byte[] Bytes = new byte[Hex.Length / 2];
            int[] HexValue = new int[] { 0x00, 0x01, 0x02, 0x03, 0x04, 0x05,
               0x06, 0x07, 0x08, 0x09, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
               0x0A, 0x0B, 0x0C, 0x0D, 0x0E, 0x0F };

            for (int x = 0, i = 0; i < Hex.Length; i += 2, x += 1)
            {
                Bytes[x] = (byte)(HexValue[Char.ToUpper(Hex[i + 0]) - '0'] << 4 |
                                  HexValue[Char.ToUpper(Hex[i + 1]) - '0']);
            }

            return Bytes;
        }

        /// <summary>
        /// Removes the padding from a decrypted command 
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        public static string RemoveCommandPadding(string command)
        {
            if (command[command.Length - 1] != 'Z')
            {
                int stringPos = command.Length - 1;

                while (command[stringPos] != 'Z')
                {
                    stringPos--;
                }

                return command.Remove(stringPos + 1);
            }
            else
            {
                return command;
            }
        }
    }
}

