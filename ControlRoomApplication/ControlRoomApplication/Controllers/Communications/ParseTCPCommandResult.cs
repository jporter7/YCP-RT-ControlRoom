using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ControlRoomApplication.Controllers.Communications.Enumerations;
using ControlRoomApplication.Constants;

namespace ControlRoomApplication.Controllers.Communications
{
    public class ParseTCPCommandResult
    {
        public ParseTCPCommandResultEnum parseTCPCommandResultEnum { get; set; }
        public String errorMessage { get; set; }
        public String[] parsedString { get; set; }

        // Optional ErrMessage param that can be set if an error occurred, This will allow descriptive communication between CR and Mobile
        public ParseTCPCommandResult(ParseTCPCommandResultEnum parseResultEnum, String[] parsedStringArr, String errMessage = null)
        {
            parseTCPCommandResultEnum = parseResultEnum;
            parsedString = parsedStringArr;
            errorMessage = errMessage;

        }
    }
}
