using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControlRoomApplication.Entities.WeatherStation
{
    public class WeatherStation : AbstractWeatherStation
    {

        public WeatherStation()
        {
            // initialize the variables
          
            // connect to the weather station here
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
            throw new NotImplementedException();
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
