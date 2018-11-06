using ControlRoomApplication.Controllers.PLCController.PLCUtilities;

namespace ControlRoomApplication.Entities.PLC
{
    public class PLC
    {
        public PLC(string ip, int port)
        {
            PLCConnector = new PLCConnector(ip, port);
        }

        public void SendMessage()
        {
            PLCConnector.C
        }

        // Getter/Setter for the Connector that connects
        // the control room application to the PLC software.
        public PLCConnector PLCConnector { get; set; }
    }
}
