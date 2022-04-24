using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControlRoomApplication.Controllers.SensorNetwork.Enumerations
{
    /// <summary>
    /// Denotes what the error state of an accelerometer is in.
    /// </summary>
    public enum AccelerometerErrorCodes
    {
        /// <summary>
        /// The sensor is working properly.
        /// </summary>
        NoError,

        /// <summary>
        /// The sensor stopped sampling.
        /// </summary>
        NoSamples,

        /// <summary>
        /// The waterfall interrupt was missed.
        /// </summary>
        WaterfallMissed
    }
}
