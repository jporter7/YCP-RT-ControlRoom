namespace ControlRoomApplication.Constants
{
    public sealed class PLCConstants
    {
        // Constant strings for PLC-related classes
        public static readonly string CALIBRATE = "calibrate";
        public static readonly string SHUTDOWN = "shutdown";
        public static readonly string COM3 = "COM3";
        public static readonly string COM4 = "COM4";
        public static readonly string LOCAL_HOST_IP = "80.80.81.81";

        // Constant ints for PLC-related classes
        public static readonly int PORT_8080 = 8080;
        public static readonly int SERIAL_PORT_BAUD_RATE = 9600;
        public static readonly int RIGHT_ASCENSION_LOWER_LIMIT = -359;
        public static readonly int RIGHT_ASCENSION_UPPER_LIMIT = 359;
        public static readonly int DECLINATION_LOWER_LIMIT = -90;
        public static readonly int DECLINATION_UPPER_LIMIT = 90;

    }
}
