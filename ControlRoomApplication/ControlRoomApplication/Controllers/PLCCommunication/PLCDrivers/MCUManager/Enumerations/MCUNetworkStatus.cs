using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControlRoomApplication.Controllers.PLCCommunication.PLCDrivers.MCUManager.Enumerations
{
    /// <summary>
    /// This contains the possible network statuses the MCU may have.
    /// </summary>
    public enum MCUNetworkStatus : ushort
    {
        /// <summary>
        /// If this is set to 1, it means the modbus server has encountered an error, and needs reset.
        /// </summary>
        MCUNetworkDisconnected = 13,
        
        /// <summary>
        /// This is the heart beat bit that alternates between 0 and 1 every 250ms
        /// </summary>
        MCUHeartBeat = 14
    }
}
