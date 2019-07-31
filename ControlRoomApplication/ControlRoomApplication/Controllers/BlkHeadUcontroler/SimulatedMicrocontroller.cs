using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.
namespace ControlRoomApplication.Controllers.BlkHeadUcontroler
{

    /// <summary>
    /// 
    /// </summary>
    /// 
    public class SimulatedMicrocontroller : AbstractMicrocontroller
    {
        Random rand = new Random();

        /// <summary>
        /// 
        /// </summary>
        /// 
        public SimulatedMicrocontroller()
        {
            
            //create a fake sensor
        }
           
        public void bringUp()
        {
            Thread connectionThread = new Thread();

            
        }

        /// <summary>
        /// Creates a data point with a random value, the time it was created (now), and type: temp
        /// </summary>
        /// Returns SensorData containing the value, time, and type
        public SensorData generateTemperatureData()
        {

            double temperatureValue = rand.Next(70, 80) * 100; //Generate a random temperatuer between 70 and 80 for variety in db
           
            SensorData temperature = new SensorData(temperatureValue, new DataTime().Now, "temp");

        }

    }
}

public struct SensorData
{
    double value = 0;
    DateTime time = 0;
    String type = "";

        public data(double val, DateTime date, String dataType)
        {
        value = val;
        time = date;
        type = dataType;
        }

}