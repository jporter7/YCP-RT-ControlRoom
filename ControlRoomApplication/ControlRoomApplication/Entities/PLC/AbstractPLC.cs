using System.Net;
using System.Net.Sockets;

namespace ControlRoomApplication.Entities.Plc
{
    public abstract class AbstractPLC
    {
        public AbstractPLC()
        {
            
        }

        public IPEndPoint IpEndpoint { get; set; }
        public TcpListener Server { get; set; }
        public string ComPort { get; set; }
        public string OutgoingMessage { get; set; }
        public string IncomingState { get; set; }
        public Orientation OutgoingOrientation { get; set; }
    }
}
