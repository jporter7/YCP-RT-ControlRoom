using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControlRoomApplication.Controllers.SensorNetwork.Enumerations
{
    /// <summary>
    /// Denotes whether or not an accelerometer's self test failed or passed.
    /// </summary>
    public enum AccelerometerSelfTestState
    {
        /// <summary>
        /// The self test failed.
        /// </summary>
        Error,

        /// <summary>
        /// The self test succeeded.
        /// </summary>
        Okay
    }
}
