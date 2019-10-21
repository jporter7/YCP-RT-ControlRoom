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

        protected override double ReadCurrentWindSpeedMPH()
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
        }

        protected override bool TestIfComponentIsAlive()
        {
            return true;
        }
    }
}