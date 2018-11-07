using System;
using System.Net.Sockets;
using System.Net;

namespace ControlRoomApplication.Controllers.PLCController.PLCUtilities
{
    public class PLCConnector
    {
        public PLCConnector()
        {
            ConnectionEndpoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 8080);
            TCPClient = new TcpClient();
        }

        public PLCConnector(string ip, int port)
        {
            // Initialize Connector with information passed in
            ConnectionEndpoint = new IPEndPoint(IPAddress.Parse(ip), port);
            TCPClient = new TcpClient();
        }

        /// <summary>
        /// Connects the control room application to the PLC software through a TCPConnection
        /// that is established over ethernet.
        /// </summary>
        /// <returns> Returns a bool indicating whether or not the connection established successfully. </returns>
        private bool ConnectToPLC()
        {
            // This is one of 3 connect methods that must be used to connect the client 
            // instance with the endpoint (IP address and port number) listed.
            TCPClient.Connect(ConnectionEndpoint);

            // This gets the stream that the client is connected to above.
            // Stream is how we will write our data back and forth between
            // this C# application and whatever application is running the PLC
            // hardware. C# cannot run PLC hardware
            Stream = TCPClient.GetStream();

            return TCPClient.Client.Connected;
        }

        /// <summary>
        /// Writes a message to the network stream that is connecting this application
        /// to the application running the PLC hardware.
        /// </summary>
        /// <param name="message"></param>
        public void WriteMessage(string message)
        {
            // Convert the message passed in into a byte[] array.
            Data = System.Text.Encoding.ASCII.GetBytes(message);

            // If the connection to the PLC is successful, get the NetworkStream 
            // that is being used and write to it.
            if (ConnectToPLC())
            {
                Stream = TCPClient.GetStream();

                Stream.Write(Data, 0, Data.Length);
            }
        }

        //
        public string ReceiveMessage()
        {
            // Create a new byte[] array and initialize the string
            // we will send back
            Data = new byte[256];
            string responseData = string.Empty;

            if (ConnectToPLC())
            {
                int i;
                try
                {
                    while ((i = Stream.Read(Data, 0, Data.Length)) != 0)
                    {
                        Message = System.Text.Encoding.ASCII.GetString(Data, 0, i);
                    }
                }
                catch(SocketException e)
                {
                    Console.WriteLine("Encountered a socket exception: " + e.Message);
                }
            }

            return Message;
        }

        public void DiscconnectFromPLC()
        {
            // Call the dispose() method to close the stream and connection.
            TCPClient.Dispose();
        }

        // Getters/Setters for TCP/IP connection
        public TcpClient TCPClient { get; set; }
        public IPEndPoint ConnectionEndpoint { get; set; }
        public NetworkStream Stream { get; set; }
        public byte[] Data { get; set; }
        public string Message { get; set; }
    }
}
