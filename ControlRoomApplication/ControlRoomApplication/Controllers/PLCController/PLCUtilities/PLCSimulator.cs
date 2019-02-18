using ControlRoomApplication.Constants;
using ControlRoomApplication.Entities.Plc;
using System;
using System.Net;
using System.Net.Sockets;

namespace ControlRoomApplication.Controllers.PLCController
{
    public class PLCSimulator : AbstractPLC
    {
        public PLCSimulator()
        {
            ConnectionEndpoint = new IPEndPoint(IPAddress.Any, PLCConstants.PORT_5012);
            Server = new TcpListener(ConnectionEndpoint);

            Server.Start(1);
        }

        public string ReceiveMessage()
        {
            Message = null;
            int i;

            while ((i = Stream.Read(Data, 0, Data.Length)) != 0)
            {
                Message = System.Text.Encoding.ASCII.GetString(Data, 0, i);
            }

            return Message;
        }

        public void SendMessage(string message)
        {
            Data = System.Text.Encoding.ASCII.GetBytes(message);
            Stream.Write(Data, 0, Data.Length);
        }

        public void StopServer()
        {
            Server.Stop();
        }

        // Getters/Setters
        public NetworkStream Stream { get; set; }
        public IPEndPoint ConnectionEndpoint { get; set; }
        public byte[] Data { get; set; }
        public string Message { get; set; }
    }
}