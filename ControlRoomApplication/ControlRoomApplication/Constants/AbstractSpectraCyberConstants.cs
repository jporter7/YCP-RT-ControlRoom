using System.IO.Ports;

namespace ControlRoomApplication.Constants
{
    public sealed class AbstractSpectraCyberConstants
    {
        public const int BAUD_RATE = 2400;
        public const int DATA_BITS = 8;
        public const Parity PARITY_BITS = Parity.None;
        public const StopBits STOP_BITS = StopBits.One;
        public const int BUFFER_SIZE = 4;
        public const int TIMEOUT_MS = 1000;
        public const int WAIT_TIME_MS = 70; // TODO: confirm that this is enough time to wait
        public const bool CLIP_BUFFER_RESPONSE = true;
        public const string DEFAULT_COMM_PORT = "COM5";

        public const double SIMULATED_RF_INTENSITY_MINIMUM = 0.0;
        public const double SIMULATED_RF_INTENSITY_MAXIMUM = 10.0;
        public const double SIMULATED_RF_INTENSITY_DISCRETIZATION = 0.001;
    }
}