using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace ControlRoomApplication.Controllers.BlkHeadUcontroler
{

    /// <summary>
    /// 
    /// </summary>
    /// 
    public class SimulatedMicrocontroller : AbstractMicrocontroller
    {
        Random rand = new Random();
        dynamic sensorData;

        /// <summary>
        /// 
        /// </summary>
        /// 
        public SimulatedMicrocontroller()
        {

            //create a fake sensor
        }

        /// <summary>
        /// 
        /// </summary>
        /// 
        public override bool bringUp()
        {

            dynamic data;

            data = generateTemperatureData();

            //interpretData

            

            return true;
        }

        /// <summary>
        /// Creates a data point with a random value, the time it was created (now), and type: temp
        /// </summary>
        /// Returns SensorData containing the value, time, and type
        public dynamic generateTemperatureData()
        {

            var temperatureList = new dynamic[1];

            for (int i = 0; i < temperatureList.Length; i++)
            {
                temperatureList[i] = new { val = rand.Next(70, 100), time = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds() + i };

            }
            var temperatureData = new { type = "temp", data = temperatureList };

           

            return temperatureData;
        }


        public dynamic generateAccelerometerData()
        {
            var list = new dynamic[100];

            double x, y, z;
            
            for (int i = 0; i < list.Length; i++)
            {
                x = rand.Next();
                y = rand.Next();
                z = rand.Next();

                list[i] = new { x = x, y = y, z = z, val = Math.Sqrt(x*x + y*y + z*z) , time = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds() + i*10 };

            }

            var accelData = new { type = "acc", data = list };

            return accelData;
        }


       

    }
}
