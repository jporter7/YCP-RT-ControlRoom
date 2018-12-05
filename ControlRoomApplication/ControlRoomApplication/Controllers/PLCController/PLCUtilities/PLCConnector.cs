using System;
using System.Net.Sockets;
using System.Net;
using System.IO.Ports;
using System.Threading;
using ControlRoomApplication.Constants;

namespace ControlRoomApplication.Controllers.PLCController
{
    public class PLCConnector
    {
        /// <summary>
        /// Constructor for the PLCConnector. This constructor should be used for 
        /// connecting to a TCP connection at the localhost 127.0.0.1 address and port 8080.
        /// </summary>
        public PLCConnector()
        {
            ConnectionEndpoint = new IPEndPoint(IPAddress.Parse(PLCConstants.LOCAL_HOST_IP), PLCConstants.PORT_8080);
            TCPClient = new TcpClient();
        }

        /// <summary>
        /// Constructor for the PLCConnector. This constructor should be used for
        /// connecting to a serial port connection.
        /// </summary>
        /// <param name="portName"> The serial port name that should be connected to. </param>
        public PLCConnector(string portName)
        {
            SPort = new SerialPort();
            SPort.PortName = portName;
            SPort.BaudRate = PLCConstants.SERIAL_PORT_BAUD_RATE;
            SPort.Open();
            logger.Info($"Serial port ({portName}) opened.");
        }

        /// <summary>
        /// Constructor for the PLCConecctor. This constructor should be used for
        /// connecting to a TCP connection at the specified IP address and port.
        /// </summary>
        /// <param name="ip"> The IP address, in string format, that should be connected to. </param>
        /// <param name="port"> The port that should be connected to. </param>
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

            logger.Info($"Established TCP connection at ({ConnectionEndpoint.Address}, {ConnectionEndpoint.Port}).");
            return TCPClient.Client.Connected;
        }

        /// <summary>
        /// Writes a message to the network stream that is connecting this application
        /// to the application running the PLC hardware.
        /// </summary>
        /// <param name="message"> A string that represents the state of the object. </param>
        public void WriteMessage(string message)
        {
            // Convert the message passed in into a byte[] array.
            Data = System.Text.Encoding.ASCII.GetBytes(message);

            // If the connection to the PLC is successful, get the NetworkStream 
            // that is being used and write to it.
            if (ConnectToPLC())
            {
                try
                {
                    Stream = TCPClient.GetStream();

                    Stream.Write(Data, 0, Data.Length);

                    logger.Info("Sent message to PLC over TCP.");
                }
                catch(SocketException e)
                {
                    Console.WriteLine($"Encountered a socket exception.");
                    logger.Error($"There was an issue with the socket: {e.Message}.");
                } 
                finally
                {
                    DisconnectFromPLC();

                    logger.Info("Disconnected from PLC.");
                }

            }
        }

        /// <summary>
        /// Receives messages from a TCP connection from the IPEndpoint that TCPClient
        /// is connected to.
        /// </summary>
        /// <returns> A string that indicates the state of the operation. </returns>
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

                    logger.Info("Message received from PLC.");
                }
                catch(SocketException e)
                {
                    Console.WriteLine("Encountered a socket exception.");
                    logger.Error($"There was an issue with the socket {e.Message}");
                }
                finally
                {
                    DisconnectFromPLC();
                    logger.Info("Disconnected from PLC.");
                }
            }

            return Message;
        }

        /// <summary>
        /// Disconnects the TCP connection that was established to the PLC.
        /// </summary>
        public void DisconnectFromPLC()
        {
            // Call the dispose() method to close the stream and connection.
            TCPClient.Dispose();
            logger.Info("Disposed of the TCP Client.");
        }

        //*** These methods will be for the arduino scale model. ***//

        /// <summary>
        /// Closes the serial port that was opened in SPort.
        /// </summary>
        private void CloseSerialPort()
        {
            SPort.Close();
            logger.Info("Serial port has been closed.");
        }

        /// <summary>
        /// Gets a message from the specified serial port in SPort.
        /// </summary>
        /// <returns> Returns a string that was read from the serial port. </returns>
        public string GetSerialPortMessage()
        {
            Message = string.Empty;

            Message = SPort.ReadExisting();
            Thread.Sleep(5000);
            SPort.Close();

            logger.Info("Message received from Arduino.");
            return Message;
        }

        public bool SendSerialPortMessage(string jsonOrientation)
        {
            Data = System.Text.ASCIIEncoding.ASCII.GetBytes(jsonOrientation);

            SPort.Write(Data, 0, Data.Length);
            SPort.Close();

            logger.Info("Message sent to Arduino");
            return true;
        }

        // Getters/Setters for TCP/IP connection
        public TcpClient TCPClient { get; set; }
        public IPEndPoint ConnectionEndpoint { get; set; }
        public NetworkStream Stream { get; set; }
        public byte[] Data { get; set; }
        public string Message { get; set; }

        // Getters/Setters for Serial Port connection (for arduino scale model)
        public SerialPort SPort { get; set; }
        private static readonly log4net.ILog logger =
            log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
    }
}