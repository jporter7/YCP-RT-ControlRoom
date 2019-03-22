using System;
using ControlRoomApplication.Entities;

namespace ControlRoomApplication.Simulators.Hardware.WeatherStation
{
    public class SimulationWeatherStation : AbstractWeatherStation
    {
        public SimulationWeatherStation(int currentWindSpeedScanDelayMS)
            : base(currentWindSpeedScanDelayMS) { }

        protected override double ReadCurrentWindSpeedMPH()
        {
            throw new NotImplementedException();
        }

        protected override bool TestIfComponentIsAlive()
        {
            throw new NotImplementedException();
        }
    }
}
