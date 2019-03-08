using System.IO.Ports;

namespace ControlRoomApplication.Constants
{
    public sealed class PLCConstants
    {
        // Constant strings for PLC-related classes
        public static readonly string CALIBRATE = "calibrate";
        public static readonly string SHUTDOWN = "shutdown";
        public static readonly string COM3 = "COM3";
        public static readonly string COM4 = "COM4";
        public static readonly string LOCAL_HOST_IP = "80.80.80.80";

        // Constant ints for PLC-related classes
        public static readonly int PORT_5012 = 5012;
        public static readonly int PORT_8080 = 8080;
        public static readonly int PORT_58006 = 58006;
        public static readonly int SERIAL_PORT_BAUD_RATE = 9600;
        public static readonly int SERIAL_PORT_DATA_BITS = 8;
        public static readonly Parity SERIAL_PORT_PARITY_BITS = Parity.None;
        public static readonly StopBits SERIAL_PORT_STOP_BITS = StopBits.One;
        public static readonly int SERIAL_PORT_TIMEOUT = 1000;
        public static readonly int RIGHT_ASCENSION_LOWER_LIMIT = -359;
        public static readonly int RIGHT_ASCENSION_UPPER_LIMIT = 359;
        public static readonly int DECLINATION_LOWER_LIMIT = -90;
        public static readonly int DECLINATION_UPPER_LIMIT = 90;

    }
}
