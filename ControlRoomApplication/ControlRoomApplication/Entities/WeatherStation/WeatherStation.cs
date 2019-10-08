using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;

namespace ControlRoomApplication.Entities.WeatherStation
{
    public class WeatherStation : AbstractWeatherStation
    {
        [DllImport("VantageProDll242\\VantagePro.dll")]

        // declarations of all methods we will use in the dll
        public static extern short OpenCommPort_V(short comPort, int baudRate);
        public static extern short InitStation_V();
        public static extern float GetWindSpeed_V();
        public static extern float GetBarometer_V();
        public static extern float GetOutsideTemp_V();
        public static extern float GetDewPt_V();
        public static extern float GetWindChill_V();
        public static extern short GetOutsideHumidity_V();
        public static extern float GetTotalRain_V();
        public static extern float GetDailyRain_V();
        public static extern float GetMonthlyRain_V();
        public static extern char GetWindDirStr_V();
        public static extern float GetRainRate_V();
        public static extern int GetHeatIndex_V();
        public static extern short GetModelNo_V();
        //public static extern short GetArchiveRecord_V(WeatherRecordStruct newRecord, short i);

        private const int COM_ERROR = -101;
        private const int MEMORY_ERROR = -102;
        private const int COM_OPEN_ERROR = -103;
        private const int NOT_LOADED_ERROR = -104;
        private const int BAD_DATA = -32768;
    
        // TODO: figure out this parameter
        // where the 5 is will be the currentWindSpeedScanDelayMS value
        public WeatherStation() : base(5)
        {

        }

        // From the HeartbeatInterface
        protected override bool TestIfComponentIsAlive()
        {
            // see if we can connect to the weather station
            return GetModelNo_V() != COM_ERROR;
        }

        protected override void InitializeStation()
        {
            if (OpenCommPort_V(5, 7600) != 0)
            {
                // we were unsuccesful
                // need to throw an exception
            }

            if (InitStation_V() == COM_ERROR)
            {
                // we were unsuccessful
                // need to throw an exception
            }
        }

        protected override float GetWindSpeed()
        {
            return GetWindSpeed_V();
        }

        protected override float GetBarometricPressure()
        {
            return GetBarometer_V();
        }

        protected override float GetOutsideTemp()
        {
            return GetOutsideTemp_V();
        }

        protected override float GetDewPoint()
        {
            return GetDewPt_V();
        }

        protected override float GetWindChill()
        {
            return GetWindChill_V();
        }

        protected override int GetHumidity()
        {
            return GetOutsideHumidity_V();
        }

        protected override float GetTotalRain()
        {
            return GetTotalRain_V();
        }

        protected override float GetDailyRain()
        {
            return GetDailyRain_V();
        }

        protected override float GetMonthlyRain()
        {
            return GetMonthlyRain_V();
        }

        protected override char GetWindDirection()
        {
            return GetWindDirStr_V();
        }

        protected override float GetRainRate()
        {
            return GetRainRate_V();
        }

        protected override int GetHeatIndex()
        {
            return GetHeatIndex_V();
        }

        // TODO: Need to figure out the structure we want for this in order to put it into 
        // the database with the most ease
        /*protected override short GetAllRecords() 
        {
            // call the GetArchiveRecord_V function that fills a struct with data and check if it was accessible
            throw new NotImplementedException();
        }*/
    }
}
