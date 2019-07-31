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
            //Thread connectionThread = new Thread();
            dynamic data;

            data = generateTemperatureData();

            

            return true;
        }

        /// <summary>
        /// Creates a data point with a random value, the time it was created (now), and type: temp
        /// </summary>
        /// Returns SensorData containing the value, time, and type
        public dynamic generateTemperatureData()
        {

            var sensorData = new
            {
                type = "temp",
                val = rand.Next(70,100),
                time = new DateTime().Date
            };
            
            return sensorData;
        }


       

    }
}
