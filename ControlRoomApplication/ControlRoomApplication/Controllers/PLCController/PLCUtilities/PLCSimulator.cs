using ControlRoomApplication.Entities.Plc;
using System;
using System.Net;
using System.Net.Sockets;

namespace ControlRoomApplication.Controllers.PLCController
{
    public class PLCSimulator
    {
        /// <summary>
        /// Takes a PLC object and establishes a connection with it. After calling the constructor,
        /// you can call the simple methods to send/receive info with the control room application 
        /// software that is meant to connect to the PLC software.This class is a simulator for that.
        /// </summary>
        /// <param name="plc"></param>
        public PLCSimulator(PLCConnector plcConnector)
        {
            PlcConnector = plcConnector;
            ConnectionEndpoint = plcConnector.ConnectionEndpoint;
            Server = new TcpListener(ConnectionEndpoint);
            Server.Start();
            PlcConnector.Client = Server.AcceptTcpClient();
            Stream = PlcConnector.Client.GetStream();
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
        public PLCConnector PlcConnector { get; set; }
        public TcpListener Server { get; set; }
        public NetworkStream Stream { get; set; }
        public IPEndPoint ConnectionEndpoint { get; set; }
        public byte[] Data { get; set; }
        public string Message { get; set; }
    }
}