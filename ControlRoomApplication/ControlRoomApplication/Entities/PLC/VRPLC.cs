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
            ///Creates named pipe for use in the PLCController
            pipeClient = new NamedPipeServerStream("VR-pipe");


        }

        /// This pipe client is used for communicating with the Unity Engine
        public NamedPipeServerStream pipeClient {get; set;}
    }
}