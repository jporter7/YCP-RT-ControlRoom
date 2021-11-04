using ControlRoomApplication.Controllers.PLCCommunication.PLCDrivers.MCUManager.Enumerations;
using ControlRoomApplication.Controllers.Communications.Enumerations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControlRoomApplication.Controllers.Communications
{
    public class ExecuteTCPCommandResult
    {
        public MovementResult movementResult { get; set; }
        public String errorMessage { get; set; }

        // Optional ErrMessage param that can be set if an error occurred, This will allow descriptive communication between CR and Mobile
        public ExecuteTCPCommandResult(MovementResult mvmtResult, String errMessage = null)
        {
            movementResult = mvmtResult;
            errorMessage = errMessage;

        }
    }
}
