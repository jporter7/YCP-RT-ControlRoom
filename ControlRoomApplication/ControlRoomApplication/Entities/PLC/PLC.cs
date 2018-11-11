using ControlRoomApplication.Controllers.PLCController.PLCUtilities;

namespace ControlRoomApplication.Entities.PLC
{
    public class PLC
    {
        public PLC(string ip, int port)
        {
            PLCConnector = new PLCConnector(ip, port);
        }

        public string SendMessage(string message)
        {
            PLCConnector.WriteMessage(message);

            return GetMessage();
        }

        public string GetMessage()
        {
            // I have this inside the while loop because I believe that
            // we will need to perform the call multiple times while the PLC 
            // software/fhardware perform their tasks so this is a form of waiting.
            string result = null;
            while(result == null)
            {
                result = PLCConnector.ReceiveMessage();
            }

            return result;
        }

        // Getter/Setter for the Connector that connects
        // the control room application to the PLC software.
        public PLCConnector PLCConnector { get; set; }
    }
}
