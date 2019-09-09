using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace ControlRoomApplication.Controllers.BlkHeadUcontroler {

    /// <summary>
    /// 
    /// </summary>
    public class SimulatedMicrocontroller : AbstractMicrocontroller {
        Random rand = new Random();
        private double _minMotorTemperature;
        private double _maxMotorTemperature;
        private List<int> Templocations = new List<int> { 0 , 1 , 3 };
        private List<int> ACClocations = new List<int> { 0 , 1 , 2 };
        private Thread simthread;
        /// <summary>
        ///Set the minimum and maximum temperature for the motors
        /// </summary>
        public SimulatedMicrocontroller( double minMotorTemperature , double maxMotorTemperature ) {
            _minMotorTemperature = minMotorTemperature;
            _maxMotorTemperature = maxMotorTemperature;

        }

        /// <summary>
        /// start the simulation thread
        /// </summary>
        public override bool BringUp() {
            simthread = new Thread( new ThreadStart( Run_sim ) );
            simthread.Start();
            return true;
        }

        private void Run_sim() {
            while(true) {
                interpretData( generateTemperatureData() );
                interpretData( generateAccelerometerData() );
                Thread.Sleep( 1000 );
            }
        }


        /// <summary>
        /// Creates a data point with a random value, the time it was created (now), and type: temp
        /// </summary>
        /// Returns SensorData containing the value, time, and type, and an int representing location
        public dynamic generateTemperatureData() {
            var temperatureList = new dynamic[1];

            for(int i = 0; i < temperatureList.Length; i++) {
                int index = rand.Next( Templocations.Count );
                temperatureList[i] = new { val = rand.Next( (int)_minMotorTemperature , (int)_maxMotorTemperature ) , time = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds() + i , loc = Templocations[index] };

            }
            var temperatureData = new { type = "temp" , data = temperatureList };
            return temperatureData;
        }


        public dynamic generateAccelerometerData() {
            var list = new dynamic[100];
            double x, y, z;
            for(int i = 0; i < list.Length; i++) {
                x = rand.Next();
                y = rand.Next();
                z = rand.Next();
                int index = rand.Next( ACClocations.Count );
                list[i] = new { x = x , y = y , z = z , val = Math.Sqrt( x * x + y * y + z * z ) , time = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds() + (i * 10) , loc = ACClocations[index] };

            }
            var accelData = new { type = "acc" , data = list };
            return accelData;
        }
    }
}
