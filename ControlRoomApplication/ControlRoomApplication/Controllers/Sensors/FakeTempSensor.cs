using ControlRoomApplication.Controllers.Sensors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            _elTempDemoIndex++;

            double[] elevationTemp = {71, 76, 81, 86, 101, 106, 70, 71, 72, 73, 74, 75};
           
            if (_elTempDemoIndex >= elevationTemp.Length)
            {
                _elTempDemoIndex = 0;
            }

            if (_elTempDemoIndex >= 0)
            {
                return elevationTemp[_elTempDemoIndex];
            }
            else
            {
                return 0;
            }
            
        }

        public double ReadAzimuthTempDemo()
        {
            _azTempDemoIndex++;
       
            double[] azimuthTemp = { 70, 71, 72, 73, 74, 75, 71, 76, 81, 86, 101, 106 };

            if (_azTempDemoIndex >= azimuthTemp.Length)
            {
                _azTempDemoIndex = 0;
            }

            if (_azTempDemoIndex >= 0)
            {
                return azimuthTemp[_azTempDemoIndex];
            }
            else
            {
                return 0;
            }
        }

        /******END*******/
    }
}
