using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using ControlRoomApplication.Entities;

namespace ControlRoomApplication.Controllers
{
    public class RemoteListener
    {

        private static readonly log4net.ILog logger =
         log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public TcpListener server = null;
        private Thread TCPMonitoringThread;
        private bool KeepTCPMonitoringThreadAlive;
        RadioTelescopeController RTController;


        public RemoteListener(int port, IPAddress localAddr/*, RadioTelescopeController telescopeController*/)
        {
            logger.Debug("Setting up remote listener");

            //RTController = telescopeController;

            //server = new TcpListener(localAddr, port);
            server = new TcpListener(port);

            // Start listening for client requests.
            server.Start();

            KeepTCPMonitoringThreadAlive = true;
            // start the listening thread
            TCPMonitoringThread = new Thread(new ThreadStart(TCPMonitoringRoutine));

            TCPMonitoringThread.Start();
        }

        public void TCPMonitoringRoutine()
        {
            // Buffer for reading data
            Byte[] bytes = new Byte[256];
            String data = null;

            // Enter the listening loop.
            while (KeepTCPMonitoringThreadAlive)
            {
                logger.Debug("Waiting for a connection... ");

                // Perform a blocking call to accept requests.
                // You could also user server.AcceptSocket() here.
                TcpClient client = server.AcceptTcpClient();
                Console.WriteLine("Connected!");

                data = null;

                // Get a stream object for reading and writing
                NetworkStream stream = client.GetStream();

                int i;

                // Loop to receive all the data sent by the client.
                while ((i = stream.Read(bytes, 0, bytes.Length)) != 0)
                {
                    // Translate data bytes to a ASCII string.
                    data = System.Text.Encoding.ASCII.GetString(bytes, 0, i);
                    Console.WriteLine("Received: {0}", data);

                    // Process the data sent by the client.
                    data = data.ToUpper();

                    byte[] myWriteBuffer = null;

                    // if processing the data fails, report an error message
                    if (!processMessage(data))
                    {
                        logger.Error("Processing data from tcp connection failed!");

                        // send back a failure response
                        myWriteBuffer = Encoding.ASCII.GetBytes("FAILURE");
                        
                    }
                    else
                    {
                        // send back a success response
                        myWriteBuffer = Encoding.ASCII.GetBytes("SUCCESS");
                    }

                    stream.Write(myWriteBuffer, 0, myWriteBuffer.Length);
                }

                // Shutdown and end connection
                client.Close();
            }
        }

        public bool RequestToKillTCPMonitoringRoutine()
        {
            logger.Info("Killing TCP Monitoring Routine");

            KeepTCPMonitoringThreadAlive = false;

            try
            {
                TCPMonitoringThread.Join();
            }
            catch (Exception e)
            {
                if ((e is ThreadStateException) || (e is ThreadInterruptedException))
                {
                    return false;
                }
                else
                { 
                    // Unexpected exception
                    throw e;
                }
            }

            return true;
        }

        private bool processMessage(String data)
        {
            if (data.IndexOf("COORDINATE_MOVE") != -1)
            {
                // we have a move command coming in
                RTController.ExecuteRadioTelescopeControlledStop();

                // get azimuth and orientation
                int azimuthIndex = data.IndexOf("AZIM");
                int elevationIndex = data.IndexOf("ELEV");
                int idIndex = data.IndexOf("ID");
                double azimuth = 0.0;
                double elevation = 0.0;
                string userId = "";

                if (azimuthIndex != -1 && elevationIndex != -1 && idIndex != -1)
                {
                    elevation = Convert.ToDouble(data.Substring(elevationIndex + 5, azimuthIndex - elevationIndex - 5));
                    azimuth = Convert.ToDouble(data.Substring(azimuthIndex + 5, idIndex - azimuthIndex - 5));
                    userId = data.Substring(idIndex + 3);
                }
                else
                    return false;

                logger.Debug("Azimuth " + azimuth);
                logger.Debug("Elevation " + elevation);

                Orientation movingTo = new Orientation(azimuth, elevation);

                RTController.MoveRadioTelescopeToOrientation(movingTo);

                // TODO: store the User Id and movement somewhere in the database

                return true;

            }
            else if (data.IndexOf("SET_OVERRIDE") != -1)
            {
                // we have an override command coming in
            }

            // can't find a keyword then we fail
            return false;
        }
    }
}
