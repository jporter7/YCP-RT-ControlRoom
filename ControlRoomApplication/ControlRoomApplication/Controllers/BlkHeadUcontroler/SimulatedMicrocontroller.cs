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
        private int count;
        private List<int> ACClocations = new List<int> { 0 , 1 , 2 };
        private Thread simthread;

        /// <summary>
        ///Set the minimum and maximum temperature for the motors and set whether it will be a static run or a testing run
        /// </summary>
        public SimulatedMicrocontroller( double minMotorTemperature , double maxMotorTemperature, bool motorSimType ) {
            _minMotorTemperature = minMotorTemperature;
            _maxMotorTemperature = maxMotorTemperature;
            setStableOrTesting(motorSimType);
            count = 0;
            tempData = new Sensors.TempSensorData();
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
        /// Creates a data point with a constant value, the time it was created (now), and location: 0 for azimuth and 1 for elevation
        /// </summary>
        /// Returns SensorData containing the value, time, and type, and an int representing location
        public dynamic generateTemperatureData() {
            var temperatureList = new dynamic[2];

            if (getStableOrTesting())
            {
                temperatureList[0] = new {val = SimulationConstants.STABLE_MOTOR_TEMP, time = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds(), loc = 0 };
                temperatureList[1] = new { val = SimulationConstants.STABLE_MOTOR_TEMP, time = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds(), loc = 1 };

                tempData.azimuthTemp = SimulationConstants.STABLE_MOTOR_TEMP;
                tempData.azimuthTempTime = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();

                tempData.elevationTemp = SimulationConstants.STABLE_MOTOR_TEMP;
                tempData.elevationTempTime = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();

                count = 0;
            }
            else
            {

                if (count < 10)
                {
                    temperatureList[0] = new { val = SimulationConstants.STABLE_MOTOR_TEMP, time = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds(), loc = 0 };
                    temperatureList[1] = new { val = SimulationConstants.STABLE_MOTOR_TEMP, time = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds(), loc = 1 };

                    tempData.azimuthTemp = SimulationConstants.STABLE_MOTOR_TEMP;
                    tempData.azimuthTempTime = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();

                    tempData.elevationTemp = SimulationConstants.STABLE_MOTOR_TEMP;
                    tempData.elevationTempTime = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();

                    count++;
                }
                else if (count >= 10 && count <= 15)
                {
                    temperatureList[0] = new { val = SimulationConstants.OVERHEAT_MOTOR_TEMP, time = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds(), loc = 0 };
                    temperatureList[1] = new { val = SimulationConstants.OVERHEAT_MOTOR_TEMP, time = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds(), loc = 1 };

                    tempData.azimuthTemp = SimulationConstants.OVERHEAT_MOTOR_TEMP;
                    tempData.azimuthTempTime = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();

                    tempData.elevationTemp = SimulationConstants.OVERHEAT_MOTOR_TEMP;
                    tempData.elevationTempTime = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();

                    count++;
                }
                else
                {
                    temperatureList[0] = new { val = SimulationConstants.STABLE_MOTOR_TEMP, time = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds(), loc = 0 };
                    temperatureList[1] = new { val = SimulationConstants.STABLE_MOTOR_TEMP, time = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds(), loc = 1 };

                    tempData.azimuthTemp = SimulationConstants.STABLE_MOTOR_TEMP;
                    tempData.azimuthTempTime = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();

                    tempData.elevationTemp = SimulationConstants.STABLE_MOTOR_TEMP;
                    tempData.elevationTempTime = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();

                    count = 0;
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
