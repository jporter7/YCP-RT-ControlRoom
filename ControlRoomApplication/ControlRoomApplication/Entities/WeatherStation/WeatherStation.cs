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

        private struct Weather_Data
        {
            public float windSpeed;
            public String windDirection;
            public float dailyRain;
            public float rainRate;
            public float outsideTemp;
            public float baromPressure;
            public float dewPoint;
            public float windChill;
            public float outsideHumidity;
            public float totalRain;
            public float monthlyRain;
            public float heatIndex;

            public Weather_Data (float windSpeedIN, String windDirectionIN, float dailyRainIN, float rainRateIN,
                                    float outsideTempIN, float baromPressureIN, float dewPointIN, float windChillIN,
                                    float outsideHumidityIN, float totalRainIN, float monthlyRainIN, float heatIndexIN) {
                windSpeed = windSpeedIN;
                windDirection = windDirectionIN;
                dailyRain = dailyRainIN;
                rainRate = rainRateIN;
                outsideTemp = outsideTempIN;
                baromPressure = baromPressureIN;
                dewPoint = dewPointIN;
                windChill = windChillIN;
                outsideHumidity = outsideHumidityIN;
                totalRain = totalRainIN;
                monthlyRain = monthlyRainIN;
                heatIndex = heatIndexIN;
            }
        };

       

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

        Weather_Data data;


        public WeatherStation(int currentWindSpeedScanDelayMS, int commPort = 8)
            : base(currentWindSpeedScanDelayMS)
        {

            data = new Weather_Data(0, "", 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
 
            InitializeStation(commPort);

            ReloadWeatherDataThread = new Thread(new ThreadStart(ReloadWeatherDataRoutine));
            KeepReloadWeatherDataThreadAlive = true;
            ReloadWeatherDataThread.Start();
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

          //  if(LoadCurrentVantageData_V() == COM_ERROR || LoadVantageHiLows_V() == COM_ERROR) {
          //      logger.Info( "failed to collect data from weather station. will attempt again." );
          //  }

            logger.Info("Initialize Concrete Weather Station successful!");
        }

        private void ReloadWeatherDataRoutine()
        {
            while (KeepReloadWeatherDataThreadAlive) {
                if (LoadCurrentVantageData_V() != COM_ERROR)
                {
                    data.windSpeed = GetWindSpeed_V();
                   // data.windDirection = GetWindDirStr_V();
                    data.dailyRain = GetDailyRain_V();
                    data.rainRate = GetRainRate_V();
                    data.outsideTemp = GetOutsideTemp_V();
                    data.baromPressure = GetBarometer_V();
                    data.dewPoint = GetDewPt_V();
                    data.windChill = GetWindChill_V();
                    data.outsideHumidity = GetOutsideHumidity_V();
                    data.totalRain = GetTotalRain_V();
                    data.monthlyRain = GetMonthlyRain_V();
                    data.heatIndex = GetHeatIndex_V();

                    logger.Info("Weather Data update successful");
                }
                else
                {
                    logger.Info("Weather Data update failed");
                }

                Thread.Sleep(1000);
            }
        }

        public bool RequestToKillReloadWeatherDataRoutine()
        {
            KeepReloadWeatherDataThreadAlive = false;

            try
            {
                ReloadWeatherDataThread.Join();
            }
            catch (Exception e)
            {
                if ((e is ThreadStateException) || (e is ThreadInterruptedException))
                {
                    return false;
                }
                else
                {
                    // Unexpected exception
                    throw e;
                }
            }

            return true;
        }

        public override float GetWindSpeed()
        {
            return data.windSpeed;
        }

        public override float GetBarometricPressure()
        {
            return data.baromPressure;
        }

        public override float GetOutsideTemp()
        {
            return data.outsideTemp;
        }

        public override float GetDewPoint()
        {
            return data.dewPoint;
        }

        public override float GetWindChill()
        {
            return data.windChill;
        }

        public override int GetHumidity()
        {
            return (int)data.outsideHumidity;
        }

        public override float GetTotalRain()
        {
            return data.totalRain;
        }

        public override float GetDailyRain()
        {
            return data.dailyRain;
        }

        public override float GetMonthlyRain()
        {
            return data.monthlyRain;
        }

        public override String GetWindDirection()
        { 
            return data.windDirection;
        }

        public override float GetRainRate()
        {
            return data.rainRate;
        }

        public override int GetHeatIndex()
        {
            return (int)data.heatIndex;
        }
    }
}
