using System.Net;

namespace ControlRoomApplication.Entities.Plc
{
    public class ScaleModelPLC : AbstractPLC
    {
        ///// <summary>
        ///// Constructor for a PLC object.
        ///// </summary>
        ///// <param name="ip"> The IP address of the PLC to connect to. </param>
        ///// <param name="port"> The port that the PLC should be connected through. </param>
        //public ScaleModelPLC(string ip, int port)
        //{
        //    IpEndpoint = new IPEndPoint(IPAddress.Parse(ip), port);
        //    ComPort = null;
        //    OutgoingMessage = string.Empty;
        //    IncomingState = string.Empty;
        //    OutgoingOrientation = new Orientation();
        //}

        /// <summary>
        /// Empty constructor for the PLC object.
        /// </summary>
        public ScaleModelPLC()
        {
            IpEndpoint = null;
            ComPort = string.Empty;
            OutgoingMessage = string.Empty;
            IncomingState = string.Empty;
            OutgoingOrientation = new Orientation();
        }
    }
}