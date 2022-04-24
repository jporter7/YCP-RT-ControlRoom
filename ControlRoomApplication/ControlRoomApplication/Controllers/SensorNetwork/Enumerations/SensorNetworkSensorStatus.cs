using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControlRoomApplication.Entities.DiagnosticData
{
    /// <summary>
    /// Denotes whether or not a sensor is in error.
    /// </summary>
    public enum SensorNetworkSensorStatus
    {
        /// <summary>
        /// The sensor is in error.
        /// </summary>
        Error,

        /// <summary>
        /// The sensor is working properly.
        /// </summary>
        Okay
    }
}
