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


        // TODO: figure out this parameter
        // where the 5 is will be the currentWindSpeedScanDelayMS value
        public WeatherStation() : base(5)
        {
            
        }

        // From the HeartbeatInterface
        protected override bool TestIfComponentIsAlive()
        {
            // either find dll method that returns if it is connected or just simply get data   
            throw new NotImplementedException();
        }

        protected override void InitializeStation()
        {
            try
            {
                // OpenCommPort_V(comPort, baudRate);

                // configures the dll
                // InitStation_V();
            }
            catch(Exception e)
            {
                throw e;
            }
        }

        protected override float GetBarometricPressure()
        {
            throw new NotImplementedException();
        }

        protected override float GetOutsideTemp()
        {
            throw new NotImplementedException();
        }

        protected override float GetDewPoint()
        {
            throw new NotImplementedException();
        }

        protected override float GetWindChill()
        {
            throw new NotImplementedException();
        }

        protected override int GetHumidity()
        {
            throw new NotImplementedException();
        }

        protected override float GetTotalRain()
        {
            throw new NotImplementedException();
        }

        protected override float GetDailyRain()
        {
            throw new NotImplementedException();
        }

        protected override float GetMonthlyRain()
        {
            throw new NotImplementedException();
        }

        protected override float GetWindSpeed()
        {
            return GetWindSpeed_V();
        }

        protected override String GetWindDirection()
        {
            throw new NotImplementedException();
        }

        protected override float GetRainRate()
        {
            throw new NotImplementedException();
        }

        protected override int GetHeatIndex()
        {
            throw new NotImplementedException();
        }
    }
}
