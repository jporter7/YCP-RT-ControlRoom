using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using ControlRoomApplication.Constants;

namespace ControlRoomApplication.Controllers.BlkHeadUcontroler {

    /// <summary>
    /// 
    /// </summary>
    public class SimulatedMicrocontroller : AbstractMicrocontroller {
        Random rand = new Random();
        private double _minMotorTemperature;
        private double _maxMotorTemperature;
        private bool _stableOrTesting;
        private int count;
        private List<int> Templocations = new List<int> { 0 , 1 , 3 };
        private List<int> ACClocations = new List<int> { 0 , 1 , 2 };
        private Thread simthread;
        /// <summary>
        ///Set the minimum and maximum temperature for the motors and set whether it will be a static run or a testing run
        /// </summary>
        public SimulatedMicrocontroller( double minMotorTemperature , double maxMotorTemperature, bool motorSimType ) {
            _minMotorTemperature = minMotorTemperature;
            _maxMotorTemperature = maxMotorTemperature;
            _stableOrTesting = motorSimType;
            count = 0;
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
                count++;
            }
        }


        /// <summary>
        /// Creates a data point with a random value, the time it was created (now), and type: temp
        /// </summary>
        /// Returns SensorData containing the value, time, and type, and an int representing location
        public dynamic generateTemperatureData() {
            var temperatureList = new dynamic[1];

            if (_stableOrTesting)
            {
                    int index = rand.Next(Templocations.Count);
                    temperatureList[0] = new {val = SimulationConstants.STABLE_MOTOR_TEMP, time = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds(), loc = Templocations[index] };
            }
            else
            {
                if (count < 10 || count > 15)
                {
                    int index = rand.Next(Templocations.Count);
                    temperatureList[0] = new { val = SimulationConstants.STABLE_MOTOR_TEMP, time = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds(), loc = Templocations[index] };
                }
                else
                {
                    int index = rand.Next(Templocations.Count);
                    temperatureList[0] = new { val = SimulationConstants.OVERHEAT_MOTOR_TEMP, time = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds(), loc = Templocations[index] };
                }
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
