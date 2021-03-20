using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControlRoomApplication.Controllers.SensorNetwork
{
    /// <summary>
    /// This will contain all of the various constants that the SensorNetwork components need
    /// to use. These values may contain data conversions, simulation intervals, and so on.
    /// </summary>
    public sealed class SensorNetworkConstants
    {

        /// <summary>
        /// This is a constant needed for converting the raw azimuth data received from the
        /// absolute azimuth encoder to degrees, which the user will see.
        /// This is based on the resolution of the digital data that the encoder gives. 
        /// The angle is related from 0-360 in digital counts from 0-2047.
        /// </summary>
        public const int AzimuthEncoderScaling = 2047;
        
        /// <summary>
        /// This is the approximate interval at which the SensorNetworkServer expects to receive
        /// data from the Sensor Network. Because the transfer isn't perfect, there will be an additional
        /// ~50ms delay. For example, a 250ms interval will yield about a 300ms receive interval.
        /// This is also used in the SensorNetworkSimulation.
        /// </summary>
        public const int DataSendingInterval = 250;
    }
}
