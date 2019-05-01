using System.IO.Ports;

namespace ControlRoomApplication.Constants
{
    public sealed class PLCConstants
    {
        public const int SERIAL_PORT_BAUD_RATE = 9600;
        public const int SERIAL_PORT_DATA_BITS = 8;
        public const Parity SERIAL_PORT_PARITY_BITS = Parity.None;
        public const StopBits SERIAL_PORT_STOP_BITS = StopBits.One;
        public const int SERIAL_PORT_TIMEOUT = 1000;
        public const int RIGHT_ASCENSION_LOWER_LIMIT = -359;
        public const int RIGHT_ASCENSION_UPPER_LIMIT = 359;
        public const int DECLINATION_LOWER_LIMIT = -90;
        public const int DECLINATION_UPPER_LIMIT = 90;
    }
}
