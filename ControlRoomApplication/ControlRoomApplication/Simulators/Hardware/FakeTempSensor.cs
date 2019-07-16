using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControlRoomApplication.Simulators.Hardware
{
    public class FakeTempSensor
    {

        double temperature;
        int elTempDemoIndex = 0;
        int azTempDemoIndex = 0;

        public double getElevationTempFahrenheit()
        {
            return readElevationTempDemo(); 
        }

        public double getAzimuthTempFahrenheit()
        {
            return readAzimuthTempDemo(); //This will return slider value
        }

        public void setElevationTempFahrenheit(double temperature)
        {
            this.temperature = temperature;
        }



        public double readElevationTempDemo()
        {
            elTempDemoIndex++;

            double[] elevationTemp = {71, 76, 81, 86, 101, 106, 70, 71, 72, 73, 74, 75};
           
            if (elTempDemoIndex >= elevationTemp.Length)
            {
                elTempDemoIndex = 0;
            }

            if (elTempDemoIndex > 0)
            {
                return elevationTemp[elTempDemoIndex];
            }
            else
            {
                return 0;
            }
            
        }

        public double readAzimuthTempDemo()
        {
            azTempDemoIndex++;
       
            double[] azimuthTemp = { 70, 71, 72, 73, 74, 75, 71, 76, 81, 86, 101, 106 };

            if (azTempDemoIndex >= azimuthTemp.Length)
            {
                azTempDemoIndex = 0;
            }

            if (azTempDemoIndex > 0)
            {
                return azimuthTemp[azTempDemoIndex];
            }
            else
            {
                return 0;
            }
        }

        /******END*******/
    }
}
