using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControlRoomApplication.Controllers.SensorNetwork
{
    /// <summary>
    /// This is used to store SimulationSensorNetwork accelerometer data.
    /// Because the raw data we get from the Sensor Network only contains
    /// x, y and z data, in order to make the simulation as similar
    /// to the real Sensor Network as possible, we are making this separate
    /// from our Acceleration class, which contains extra fields.
    /// </summary>
    public struct RawAccelerometerData
    {
        /// <summary>
        /// Accelerometer x-axis data.
        /// </summary>
        public int X { get; set; }

        /// <summary>
        /// Accelerometer y-axis data.
        /// </summary>
        public int Y { get; set; }

        /// <summary>
        /// Accelerometer z-axis data.
        /// </summary>
        public int Z { get; set; }
    }
}
