using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControlRoomApplication.Controllers.PLCCommunication.PLCDrivers.MCUManager
{
    public enum MCUCommandType
    {
        JOG,
        RELATIVE_MOVE,
        CONFIGURE,
        CLEAR_LAST_MOVE,
        HOLD_MOVE,
        IMMEDIATE_STOP,
        RESET_ERRORS,
        HOME
    }
}
