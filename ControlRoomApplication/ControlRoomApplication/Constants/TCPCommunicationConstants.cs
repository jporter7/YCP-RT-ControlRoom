using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControlRoomApplication.Constants
{
    public sealed class TCPCommunicationConstants
    {
        public static String VERSION_CONVERSION_ERR = "Error converting version from string to double: ";

        public static String VERSION_NOT_FOUND = "The specified version was not found: ";

        public static String AZ_EL_CONVERSION_ERR = "Error converting azimuth or elevation from string to double: ";

        public static String MISSING_AZ_EL_ARGS = "Arguments for AZ or EL not supplied.";

        public static String ORIENTATION_MOVE_ERR = "Command ORIENTATION_MOVE failed with result: ";

        public static String RELATIVE_MOVE_ERR = "Command RELATIVE_MOVE failed with result: ";

        public static String MISSING_OVERRIDE_ARG = "No override argument supplied for SET_OVERRIDE. ";

        public static String INVALID_SENSOR_OVERRIDE = "No valid sensor was found to be overridden in the data string";

        public static String SCRIPT_ERR = "The selected script failed with error: ";

        public static String ALL_STOP_ERR = "The All Stop command failed with error: TimedOut ";

        public static String MISSING_SENSOR_INIT_ARGS = "SENSOR_INIT command is missing arguments; supply all 10 of them. 1" +
            "means initialize this sensor, 0 means do not initialize.";
        public static String COMMAND_NOT_FOUND = "The specified command was not found or is invalid: ";






    }
}
