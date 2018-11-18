using System.IO.Ports;

namespace ControlRoomApplication.Constants
{
    public sealed class AbstractSpectraCyberConstants
    {
        public static readonly int BAUD_RATE = 2400;
        public static readonly int DATA_BITS = 8;
        public static readonly Parity PARITY_BITS = Parity.None;
        public static readonly StopBits STOP_BITS = StopBits.One;
        public static readonly int BUFFER_SIZE = 4;
        public static readonly int TIMEOUT_MS = 1000;
        public static readonly int WAIT_TIME_MS = 70; // TODO: confirm that this is enough time to wait
        public static readonly bool CLIP_BUFFER_RESPONSE = true;
        public static readonly string DEFAULT_COMM_PORT = "COM5";

        public static readonly double SIMULATED_RF_INTENSITY_MINIMUM = 0.0;
        public static readonly double SIMULATED_RF_INTENSITY_MAXIMUM = 10.0;
        public static readonly double SIMULATED_RF_INTENSITY_DISCRETIZATION = 0.001;
    }
}