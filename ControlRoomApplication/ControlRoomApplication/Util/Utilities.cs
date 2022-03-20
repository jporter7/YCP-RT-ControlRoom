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
    }
}

