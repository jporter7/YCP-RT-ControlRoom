using System;
using ControlRoomApplication.Constants;
using ControlRoomApplication.Entities;

namespace ControlRoomApplication.Simulators.Hardware.WeatherStation
{
    public class SimulationWeatherStation : AbstractWeatherStation
    {
        private Random Rand;

        public SimulationWeatherStation(int currentWindSpeedScanDelayMS)
            : base(currentWindSpeedScanDelayMS)
        {
            Rand = new Random();
        }

        /*protected override double ReadCurrentWindSpeedMPH()
        {
            // Generate a Gaussian random distribution through a Box-Muller transform
            double rand1 = 1 - Rand.NextDouble();
            double rand2 = 1 - Rand.NextDouble();

            double randStdNorm = Math.Sqrt(-2 * Math.Log(rand1)) * Math.Sin(2 * Math.PI * rand2);
            double randNormValue = MiscellaneousHardwareConstants.SIMULATION_WEATHER_STATION_AVERAGE_WIND_SPEED_MPH + (MiscellaneousHardwareConstants.SIMULATION_WEATHER_STATION_MAXIMUM_ALLOWABLE_WIND_SPEED_MPH_STD_DEV * randStdNorm);

            if (randNormValue < 0)
            {
                randNormValue = 0.0;
            }

            return randNormValue;
        }*/


        // IMPORTANT!!!!! Returning 0 for now until the simulation is implemented. Without this, the application
        // would not be able to work unless you use production with the new gui changes.

        protected override bool TestIfComponentIsAlive()
        {
            return true;
        }

        protected override float GetBarometricPressure()
        {
            return 0;
        }

        protected override float GetOutsideTemp()
        {
            return 0;
        }

        protected override float GetDewPoint()
        {
            return 0;
        }

        protected override float GetWindChill()
        {
            return 0;
        }

        protected override int GetHumidity()
        {
            return 0;
        }

        protected override float GetTotalRain()
        {
            return 0;
        }

        protected override float GetDailyRain()
        {
            return 0;
        }

        protected override float GetMonthlyRain()
        {
            return 0;
        }

        public override float GetWindSpeed()
        {
            return 0;
        }

        protected override char GetWindDirection()
        {
            return ' ';
        }

        protected override float GetRainRate()
        {
            return 0;
        }

        protected override int GetHeatIndex()
        {
            return 0;
        }
    }
}