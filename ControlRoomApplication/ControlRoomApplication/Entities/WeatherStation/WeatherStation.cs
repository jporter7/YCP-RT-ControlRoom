using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Runtime.InteropServices;

namespace ControlRoomApplication.Entities.WeatherStation
{
    public class WeatherStation : AbstractWeatherStation
    {
        private static readonly log4net.ILog logger =
           log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public struct WeatherUnits
        {
            public char tempUnit;
            public char RainUnit;
            public char BaromUnit;
            public char WindUnit;
            public char elevUnit;
        };

        private float windSpeed = 0;
        private char windDirection = ' ';
        private float dailyRain = 0;
        private float rainRate = 0;
        private float outsideTemp = 0;
        private float baromPressure = 0;

        // declarations of all methods we will use in the dll

        //getters, get data from dll
        [DllImport("VantageProDll242\\VantagePro.dll")] public static extern float GetWindSpeed_V();
        [DllImport("VantageProDll242\\VantagePro.dll")] public static extern float GetBarometer_V();
        [DllImport("VantageProDll242\\VantagePro.dll")] public static extern float GetOutsideTemp_V();
        [DllImport("VantageProDll242\\VantagePro.dll")] public static extern float GetDewPt_V();
        [DllImport("VantageProDll242\\VantagePro.dll")] public static extern float GetWindChill_V();
        [DllImport("VantageProDll242\\VantagePro.dll")] public static extern short GetOutsideHumidity_V();
        [DllImport("VantageProDll242\\VantagePro.dll")] public static extern float GetTotalRain_V();
        [DllImport("VantageProDll242\\VantagePro.dll")] public static extern float GetDailyRain_V();
        [DllImport("VantageProDll242\\VantagePro.dll")] public static extern float GetMonthlyRain_V();
        [DllImport("VantageProDll242\\VantagePro.dll")] public static extern char GetWindDirStr_V();
        [DllImport("VantageProDll242\\VantagePro.dll")] public static extern float GetRainRate_V();
        [DllImport("VantageProDll242\\VantagePro.dll")] public static extern int GetHeatIndex_V();
        [DllImport("VantageProDll242\\VantagePro.dll")] public static extern short GetModelNo_V();

        //setup
        [DllImport( "VantageProDll242\\VantagePro.dll" )] public static extern short OpenCommPort_V( short comPort , int baudRate );
        [DllImport( "VantageProDll242\\VantagePro.dll" )] public static extern short InitStation_V();
        [DllImport( "VantageProDll242\\VantagePro.dll" )] public static extern short OpenUSBPort_V();
        [DllImport( "VantageProDll242\\VantagePro.dll" )] public static extern short SetUnits_V( WeatherUnits units );
        [DllImport("VantageProDll242\\VantagePro.dll")] public static extern short SetVantageTimeoutVal_V(short TimeOutType);
        [DllImport( "VantageProDll242\\VantagePro.dll" )] public static extern short SetCommTimeoutVal_V( short ReadTimeout , short WriteTimeout );

        //loaders, call these to get from weather station into dll
        [DllImport( "VantageProDll242\\VantagePro.dll" )] public static extern short LoadCurrentVantageData_V();
        [DllImport( "VantageProDll242\\VantagePro.dll" )] public static extern int LoadVantageHiLows_V();
        //public static extern short GetArchiveRecord_V(WeatherRecordStruct newRecord, short i);

        private const int COM_ERROR = -32701;
        private const int MEMORY_ERROR = -102;
        private const int COM_OPEN_ERROR = -32701;
        private const int NOT_LOADED_ERROR = -104;
        private const int BAD_DATA = -32704;
    

        public WeatherStation(int currentWindSpeedScanDelayMS, int commPort = 8)
            : base(currentWindSpeedScanDelayMS)
        {
            InitializeStation(commPort);
        }

        // From the HeartbeatInterface
        protected override bool TestIfComponentIsAlive()
        {
            // see if we can connect to the weather station
            return GetModelNo_V() != COM_ERROR;
        }

        /// <summary>
        /// Sets up connection with the production weather station. 
        /// If loading the data fails, its no big deal. We are going to attempt to load the data every time
        /// a getter method is called.
        /// </summary>
        /// <returns> void </returns>
        protected void InitializeStation(int commPort)
        {
            
            // TODO: make com port a parameter for InitializeStation

            if (OpenCommPort_V((short)commPort, 19200) != 0)
            {
                logger.Error("OpenCommPort unsuccessful!");
                    throw new ExternalException();
            }

            if (InitStation_V() == COM_ERROR)
            {
                logger.Error("Initialize Station unsuccessful!");
                throw new ExternalException();
            }

            if(LoadCurrentVantageData_V() == COM_ERROR || LoadVantageHiLows_V() == COM_ERROR) {
                logger.Info( "failed to collect data from weather station. will attempt again." );
            }

            logger.Info("Initialize Concrete Weather Station successful!");
        }

        private Boolean ReloadData()
        {
            return LoadCurrentVantageData_V() != COM_ERROR;
        }

        public override float GetWindSpeed()
        {
            if (ReloadData())
            {
                return windSpeed = GetWindSpeed_V();
            }

            return windSpeed;
        }

        public override float GetBarometricPressure()
        {
            if (ReloadData())
            {
                return baromPressure = GetBarometer_V();
            }

            return baromPressure;
        }

        public override float GetOutsideTemp()
        {
            if (ReloadData())
            {
                return outsideTemp = GetOutsideTemp_V();
            }

            return outsideTemp;
        }

        public override float GetDewPoint()
        {
            return GetDewPt_V();
        }

        public override float GetWindChill()
        {
            return GetWindChill_V();
        }

        public override int GetHumidity()
        {
            return GetOutsideHumidity_V();
        }

        public override float GetTotalRain()
        {
            return GetTotalRain_V();
        }

        public override float GetDailyRain()
        {
            if (ReloadData())
            {
                return dailyRain = GetDailyRain_V();
            }

            return dailyRain;
        }

        public override float GetMonthlyRain()
        {
            return GetMonthlyRain_V();
        }

        public override char GetWindDirection()
        {
            if (ReloadData())
            {
                return windDirection = GetWindDirStr_V();
            }

            return windDirection;
        }

        public override float GetRainRate()
        {
            if (ReloadData())
            {
                return rainRate = GetRainRate_V();
            }

            return rainRate;
        }

        public override int GetHeatIndex()
        {
            return GetHeatIndex_V();
        }
    }
}
