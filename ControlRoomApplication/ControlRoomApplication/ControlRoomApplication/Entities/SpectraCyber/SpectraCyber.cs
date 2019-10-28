using System.IO.Ports;
using ControlRoomApplication.Constants;

namespace ControlRoomApplication.Entities
{
    public class SpectraCyber : AbstractSpectraCyber
    {
        public string CommPort { get; }
        public SerialPort SerialPort { get; set; }

        public SpectraCyber(string commPort) : base()
        {
            CommPort = commPort;
        }

        public SpectraCyber()
        {
            CommPort = AbstractSpectraCyberConstants.DEFAULT_COMM_PORT;
        }
    }
}