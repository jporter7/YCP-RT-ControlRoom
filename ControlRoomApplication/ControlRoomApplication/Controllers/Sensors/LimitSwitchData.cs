using ControlRoomApplication.Controllers.Sensors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ControlRoomApplication.Constants;

namespace ControlRoomApplication.Simulators.Hardware
{
    /// <summary>
    ///  A public struct to store the data regarding the limit switches
    ///  for both the Azimuth and Elevation, for use in the Diagnostics page
    ///  and potentially more
    /// </summary>
    public struct LimitSwitchData
    {
        /// <summary>
        /// limit at 360 degrees
        /// </summary>
        public bool Azimuth_CW_Limit;
        /// <summary>
        /// limit at - 10 degrees
        /// </summary>
        public bool Azimuth_CCW_Limit;
        public bool Elevation_Lower_Limit;
        public bool Elevation_Upper_Limit;
    }

}
