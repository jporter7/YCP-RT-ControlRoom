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

            double[] elevationTemp = { 74.2, 82.3, 105.4, 89.5, 80.6, 83.7, 100.2, 87.4, 76.6, 105.1, 99.3, 89.9, 75.3, 77.4, 76.7, 89.3, 102.2, 88.7, 88.4, 71.3, 81.2, 78.3, 103.9, 105.4, 102.1, 104.2, 81.5, 70.6, 84.7, 87.5, 101.2, 71.3, 101.6, 90.2, 79.3, 77.4, 99.5, 92.3, 86.1, 97.9, 76.2, 97.8, 95.3, 102.7, 83.4, 103.6, 91.5, 86.3, 70.6, 95.9, 102.2, 100.4, 92.6, 96.8, 100.1, 92.3, 96.5, 81.7, 91.9, 101.8, 95.6, 102.4, 81.2, 78.1, 97.5, 92.3, 88.6, 80.3, 88.8, 105.2, 90.7, 72.5, 78.3, 95.2, 82.4, 85.7, 105.2, 95.6, 95.2, 93.6 };
           
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
       
            double[] azimuthTemp = { 91, 76, 89, 80 ,70, 85 ,82, 86 ,74 ,96 ,104 ,82, 86 ,101 ,78 ,86, 87, 75 ,98 ,72 ,105 ,92 ,88, 89 ,103 ,72, 91, 83 ,85 ,96 ,89, 72 ,105, 90, 95, 94 ,71 ,99 ,99 ,74 ,98 ,90 ,100 ,83, 73 ,97 ,81 ,73 ,98, 84 ,88 ,85 ,78 ,105 ,78 ,74 ,71 ,79 ,90, 98, 73, 73, 78, 90, 100, 98, 95, 103, 97, 82, 72,93 ,77 ,86 ,101, 81, 81, 74,75, 97 };

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
