using System.Net;

namespace ControlRoomApplication.Entities.Plc
{
    public class PLC
    {
        /// <summary>
        /// Constructor for a PLC object.
        /// </summary>
        /// <param name="ip"> The IP address of the PLC to connect to. </param>
        /// <param name="port"> The port that the PLC should be connected through. </param>
        public PLC(string ip, int port)
        {
            IpEndpoint = new IPEndPoint(IPAddress.Parse(ip), port);
            ComPort = null;
            OutgoingMessage = string.Empty;
            IncomingState = string.Empty;
            OutgoingOrientation = new Orientation();
        }

        /// <summary>
        /// Empty constructor for the PLC object.
        /// </summary>
        public PLC()
        {
            IpEndpoint = null;
            ComPort = string.Empty;
            OutgoingMessage = string.Empty;
            IncomingState = string.Empty;
            OutgoingOrientation = new Orientation();
        }

        public IPEndPoint IpEndpoint { get; set; }
        public string ComPort { get; set; }
        public string OutgoingMessage { get; set; }
        public string IncomingState { get; set; }
        public Orientation OutgoingOrientation { get; set; }
    }
}