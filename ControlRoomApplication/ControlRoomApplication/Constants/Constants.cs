using System.IO.Ports;

namespace ControlRoomApplication
{
    public abstract class Constants
    {
        public static readonly int MAX_USERNAME_LENGTH = 15;

        //
        // Start SpectraCyber constants
        //

        public static readonly int SPECTRA_CYBER_BAUD_RATE = 2400;
        public static readonly int SPECTRA_CYBER_DATA_BITS = 8;
        public static readonly Parity SPECTRA_CYBER_PARITY_BITS = Parity.None;
        public static readonly StopBits SPECTRA_CYBER_STOP_BITS = StopBits.One;
        public static readonly int SPECTRA_CYBER_BUFFER_SIZE = 4; // This value should only have to be 4, but this can change if necessary
        public static readonly int SPECTRA_CYBER_TIMEOUT_MS = 1000;
        public static readonly int SPECTRA_CYBER_WAIT_TIME_MS = 70; // TODO: confirm that this is enough time to wait
        public static readonly bool SPECTRA_CYBER_CLIP_BUFFER_RESPONSE = true;
        public static readonly string SPECTRA_CYBER_DEFAULT_COMM_PORT = "COM1";
    }
}
