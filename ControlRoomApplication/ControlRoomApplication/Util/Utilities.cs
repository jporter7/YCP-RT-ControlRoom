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


    }

}

