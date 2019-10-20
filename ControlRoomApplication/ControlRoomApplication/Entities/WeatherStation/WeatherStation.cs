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
        private const int COM_OPEN_ERROR = -103;
        private const int NOT_LOADED_ERROR = -104;
        private const int BAD_DATA = -32768;
    

        public WeatherStation(int currentWindSpeedScanDelayMS)
            : base(currentWindSpeedScanDelayMS)
        {
            InitializeStation();
        }

        // From the HeartbeatInterface
        protected override bool TestIfComponentIsAlive()
        {
            // see if we can connect to the weather station
            // return GetModelNo_V() != COM_ERROR;
            return true;
        }

        protected override void InitializeStation()
        {
            
            // TODO: make com port a parameter for InitializeStation

            if (OpenCommPort_V(13, 19200) != 0)
            {
                logger.Error("OpenCommPort unsuccessful!");
                    throw new ExternalException();
            }

            if(SetCommTimeoutVal_V( 1000 , 500 ) == COM_ERROR)
            {
                logger.Error("OpenCommPort unsuccessful!");
               // throw new ExternalException();
            }

            // BELOW THROWS unable to find access point for setunits error
            /* WeatherUnits units = new WeatherUnits();
             units.tempUnit = '0';
             units.BaromUnit = '0';
             units.elevUnit = '0';
             units.RainUnit = '0';
             units.WindUnit = '0';

             if (SetUnits_V(units) != 0)
             {
                 logger.Error("SetUnits unsuccessful!");
                 throw new ExternalException();
             }*/ 

            if (InitStation_V() == COM_ERROR)
            {
                logger.Error("Initialize Station unsuccessful!");
                throw new ExternalException();
            }

            if(LoadCurrentVantageData_V() == COM_ERROR || LoadVantageHiLows_V() == COM_ERROR) {
                logger.Info( "failed to collect data from weather station" );
            }

            logger.Info("Initialize Concrete Weather Station successful!");


            Console.WriteLine( GetWindSpeed_V() );
            /*
            Console.WriteLine( OpenCommPort_V( 13 , 19200 ) );

            //SetVantageTimeoutVal_V( 8 ) 

            Console.WriteLine( SetCommTimeoutVal_V( 1000 , 500 ) );

            Console.WriteLine( InitStation_V() );

            Console.WriteLine( LoadCurrentVantageData_V() );
            Console.WriteLine( LoadVantageHiLows_V() );

            */
            Console.WriteLine( GetOutsideTemp_V() );

        }

        public override float GetWindSpeed()
        {
            var tem = LoadCurrentVantageData_V();
            var ten = GetWindSpeed_V();
            return ten;
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
