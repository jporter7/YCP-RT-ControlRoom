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
        public PLCSimulator(PLC plc)
        {
            Plc = plc;
            ConnectionEndpoint = Plc.PLCConnector.ConnectionEndpoint;
            TCPServer = new TcpListener(ConnectionEndpoint);
            TCPServer.Start();
            Plc.PLCConnector.TCPClient = TCPServer.AcceptTcpClient();
            Stream = Plc.PLCConnector.TCPClient.GetStream();
        }

        public string ReceiveMessage()
        {
            Message = null;
            int i;

            while ((i = Stream.Read(Data, 0, Data.Length)) != 0)
            {
                Message = System.Text.Encoding.ASCII.GetString(Data, 0, i);
            }

            StopServer();
            return Message;
        }

        public void SendMessage(string message)
        {
            Data = System.Text.Encoding.ASCII.GetBytes(message);
            Stream.Write(Data, 0, Data.Length);
            StopServer();
        }

        public void StopServer()
        {
            TCPServer.Stop();
        }

        // Getters/Setters
        public PLC Plc { get; set; }
        public TcpListener TCPServer { get; set; }
        public NetworkStream Stream { get; set; }
        public IPEndPoint ConnectionEndpoint { get; set; }
        public byte[] Data { get; set; }
        public string Message { get; set; }
    }
}