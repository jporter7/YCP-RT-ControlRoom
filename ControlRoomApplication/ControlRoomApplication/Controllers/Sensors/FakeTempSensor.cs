using ControlRoomApplication.Controllers.Sensors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ControlRoomApplication.Constants;

namespace ControlRoomApplication.Simulators.Hardware
{
    /// <summary>
    /// Class for the simulated temperature sensor
    /// </summary>
    /// 
    public class FakeTempSensor
    {

        double _elTemperature;
        double _azTemperature;

        /// <summary>
        /// Simulates getting the unstable elevation temperature
        /// </summary>
        /// 
        public double GetElevationTemperatureUnstable()
        {
            return SimulationConstants.OVERHEAT_MOTOR_TEMP;
        }

        /// <summary>
        /// Simulates getting the unstable azimuth temperature
        /// </summary>
        /// 
        public double GetAzimuthTemperatureUnstable()
        {
            return SimulationConstants.OVERHEAT_MOTOR_TEMP;
        }

        /// <summary>
        /// Simulates getting the stable elevation temperature
        /// </summary>
        /// 
        public double GetElevationTemperatureStable()
        {
            return SimulationConstants.STABLE_MOTOR_TEMP;
        }

        /// <summary>
        /// Simulates getting the stable azimuth temperature
        /// </summary>
        /// 
        public double GetAzimuthTemperatureStable()
        {
            return SimulationConstants.STABLE_MOTOR_TEMP;
        }

        public void SetElevationTemp(double elTemp)
        {
            _elTemperature = elTemp;
        }

        public void SetAzimuthTemp(double azTemp)
        {
            _azTemperature = azTemp;
        }

        /******END*******/
    }
}
