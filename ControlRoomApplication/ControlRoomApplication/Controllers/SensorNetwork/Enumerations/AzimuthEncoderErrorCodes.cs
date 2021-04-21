using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControlRoomApplication.Controllers.SensorNetwork.Enumerations
{
    /// <summary>
    /// Denotes the error state of the azimuth absolute encoder.
    /// </summary>
    public enum AzimuthEncoderErrorCodes
    {
        /// <summary>
        /// The sensor is working properly.
        /// </summary>
        NoError,

        /// <summary>
        /// Valid flag returned false.
        /// </summary>
        BadData,

        /// <summary>
        /// Sync flag returned false.
        /// </summary>
        StaleData
    }
}
