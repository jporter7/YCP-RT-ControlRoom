using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControlRoomApplication.Controllers.PLCCommunication.PLCDrivers.MCUManager.Enumerations
{
    /// <summary>
    /// There are certain commands that we only want to do to one axis, and we can pick which axis we want to do it to.
    /// This allows us to pick one axis or the other, or both.
    /// </summary>
    public enum MotorAxisEnum
    {
        /// <summary>
        /// Undefined or no axis.
        /// </summary>
        None,

        //
        /// <summary>
        /// Both azimuth and elevation.
        /// </summary>
        Both,

        /// <summary>
        /// Only the azimuth axis.
        /// </summary>
        Azimuth,

        /// <summary>
        /// Only the elevation axis
        /// </summary>
        Elevation
    }
}
