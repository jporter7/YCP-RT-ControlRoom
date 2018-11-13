using System;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Collections.ObjectModel;
using ControlRoomApplication.Entities;
using ControlRoomApplication.Entities.RadioTelescope;

namespace ControlRoomApplication.Simulators.Server
{
    class Server
    {
        private string localIP;
        private IPAddress ipAd;
        private TcpListener listener;

        public Server()
        {
            localIP = Dns.GetHostEntry(Dns.GetHostName()).AddressList[0].ToString();
            ipAd = IPAddress.Parse(localIP);

            /*
                // prepares to receive packet
                byte[] b = new byte[100];
                int k = s.Receive(b);

                Console.WriteLine("Received...");
                for (int i = 0; i < k; i++)
                {
                    Console.Write(Convert.ToChar(b[i]));
                } */
        }
        ///<summary>
        /// Instantiates a Socket on which the server will listen
        /// </summary>
        /// <returns> An opened Socket </returns>
        public TcpClient Connect()
        {
            try
            {
                // starts listener
                // setting port to 8081 for YCP use
                listener = new TcpListener(ipAd, 8081);
                listener.Start();

                Console.WriteLine("Running on " + localIP + ":8081");
                Console.WriteLine("Endpoint:" + listener.LocalEndpoint);
                Console.WriteLine("Waiting for connection");

                TcpClient client = listener.AcceptTcpClient();
                Console.WriteLine("Connection Accepted");

                return client;
            }
            catch (Exception exception)
            {
                Console.WriteLine("Error: " + exception.StackTrace);
            }

            return null;
        }
        /// <summary>
        /// Closes the connection and cleans up the Socket passed as well as the listener
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public bool CloseConnection(TcpClient client)
        {
            try
            {   
                // clean up
                client.Close();
                listener.Stop();
            }
            catch (Exception exception)
            {
                Console.WriteLine("Error: " + exception.StackTrace);
            }
            return true;
        }

        public static void SendAppointment(TcpClient client)
        {
           /* NetworkStream stream = client.GetStream();
            byte[] b = new byte[client.ReceiveBufferSize];

            int packet = stream.Read(b, 0, client.ReceiveBufferSize);
            string s = */
            
        }

        public static Collection<Appointment> RetrieveAppointments()
        {
            throw new NotImplementedException("TODO");
        }

        /// <summary>
        /// Sends the Scheduler's current Schedule
        /// </summary>
        /// <param name="schedule"></param>
        public static void SendAppointmentSchedule(Schedule schedule)
        {
            throw new NotImplementedException("TODO");
        }

        /// <summary>
        /// Sends the overriden appointment(s)
        /// </summary>
        /// <param name="overridenAppointmentCollection"></param>
        /// <returns>Returns a collection of those appointments that reflects alterations</returns>
        public static Collection<Appointment> AppointmentOverride(Collection<Appointment> overridenAppointmentCollection)
        {
            throw new NotImplementedException("TODO");
        }

        /// <summary>
        /// Allows the retrieval of the Radio Telescopes current status
        /// </summary>
        /// <param name="status"></param>
        public static void SendRadioTelescopeStatus(RadioTelescopeStatusEnum status)
        {
            throw new NotImplementedException("TODO");
        }


        public static void SendRFData(RFData data)
        {
            throw new NotImplementedException("TODO");
        }

        public static void CurrentRTOrientation(Orientation orientation)
        {
            throw new NotImplementedException("TODO");
        }

        public static void RetrieveCommand(Command command)
        {
            throw new NotImplementedException("TODO");
        }
    }
}
