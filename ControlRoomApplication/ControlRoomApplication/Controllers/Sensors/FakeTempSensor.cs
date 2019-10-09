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
    public class FakeTempSensor : AbstractTemperatureSensor
    {

        double _elTemperature;
        double _azTemperature;
        int _elTempDemoIndex = 0;
        int _azTempDemoIndex = 0;
        bool stableOrTesting = true;

        /// <summary>
        /// Simulates getting the elevation temperature
        /// </summary>
        /// 
        public override double GetElevationTemperature()
        {
            return ReadElevationTempDemo(); //Iterates through an array to
                                            //simulate reading the temperature like the
                                            //real device will do
        }

        /// <summary>
        /// Simulates getting the azimuth temperature
        /// </summary>
        /// 

        public void setStableOrTesting(bool testOrNot)
        {
            stableOrTesting = testOrNot;
        }

        public bool getStableOrTesting()
        {
            return stableOrTesting;
        }

        public override double GetAzimuthTemperature()
        {
            return ReadAzimuthTempDemo(); 
        }

        public void SetElevationTemp(double elTemp)
        {
            _elTemperature = elTemp;
        }

        public void SetAzimuthTemp(double azTemp)
        {
            _azTemperature = azTemp;
        }

        public double ReadElevationTempDemo()
        {
            return SimulationConstants.OVERHEAT_MOTOR_TEMP;
        }

        public double ReadAzimuthTempDemo()
        {
            return SimulationConstants.OVERHEAT_MOTOR_TEMP;
        }

        /******END*******/
    }
}
