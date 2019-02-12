using System.Net;
using System.IO.Pipes;

namespace ControlRoomApplication.Entities.Plc
{
    public class VRPLC : AbstractPLC
    {
        /// <summary>
        /// This is an empty constructor for the VR Radio Telescope PLC for use with the PLC Controller class.
        /// It will use System.IO.Pipes for interprocess communication in order to communicate with the Unity Engine.
        /// </summary>
        public VRPLC()
        {
          
        }

        /// This pipe client is used for communicating with the Unity Engine
        public NamedPipeServerStream pipeClient {get; set;}

        /// <summary>
        /// Method for creating the named pipe server stream which allows for communication with VR telescope.
        /// </summary>

        public void StartClient()
        {
            this.pipeClient = new NamedPipeServerStream("VR-pipe");
        }
    }
}