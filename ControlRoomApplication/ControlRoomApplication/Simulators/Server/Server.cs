using System;
using System.Text;
using System.Net;
using System.Net.Sockets;

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
        /// <returns> an open socket </returns>
        public Socket Connect()
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

                Socket s = listener.AcceptSocket();
                Console.WriteLine("Connection Accepted");

                return s;
            }
            catch (Exception exception)
            {
                Console.WriteLine("Error: " + exception.StackTrace);
            }

            return null;
        }
        /// <summary>
        /// Closes the connection and cleans up the Socket passed and the List
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public bool CloseConnection(Socket s)
        {
            try
            {   
                // sends acknowledgment
                ASCIIEncoding asen = new ASCIIEncoding();
                s.Send(asen.GetBytes("The string was received by the server."));
                Console.WriteLine("\nSent ACK");

                // clean up
                s.Close();
                listener.Stop();
            }
            catch (Exception exception)
            {
                Console.WriteLine("Error: " + exception.StackTrace);
            }
            return true;
        }

       /* static void RecieveAppointmentSchedule()
        {

        }*/
    }
}
