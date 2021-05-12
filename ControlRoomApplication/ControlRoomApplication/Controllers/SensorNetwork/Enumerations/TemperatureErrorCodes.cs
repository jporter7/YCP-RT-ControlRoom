using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControlRoomApplication.Controllers.SensorNetwork.Enumerations
{
    /// <summary>
    /// Denotes what specific errors a temperature sensor may be having.
    /// </summary>
    public enum TemperatureErrorCodes
    {
        /// <summary>
        /// The sensor is working properly.
        /// </summary>
        NoError,

        /// <summary>
        /// The sensor is not receiving data.
        /// </summary>
        NoData,

        /// <summary>
        /// The cyclic redundancy check for the sensor was invalid.
        /// </summary>
        CrcInvalid
    }
}
