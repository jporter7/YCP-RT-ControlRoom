using System.Net;

namespace ControlRoomApplication.Entities.Plc
{
    public abstract class AbstractPLC
    {
        public AbstractPLC()
        {
            
        }

        public IPEndPoint IpEndpoint { get; set; }
        public string ComPort { get; set; }
        public string OutgoingMessage { get; set; }
        public string IncomingState { get; set; }
        public Orientation OutgoingOrientation { get; set; }
    }
}
