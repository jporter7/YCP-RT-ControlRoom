using System;
using System.Text;
using System.Net;
using System.Net.Sockets;

namespace ControlRoomApplication.Simulators.Server
{
    class Server
    {
        public Server()
        {
            try
            {
                string localIP = Dns.GetHostEntry(Dns.GetHostName()).AddressList[0].ToString();
                IPAddress ipAd = IPAddress.Parse(localIP);

                // starts listener
                // setting port to 8081 for YCP use
                TcpListener myList = new TcpListener(ipAd, 8081);
                myList.Start();

                Console.WriteLine("Running on " + localIP + ":8081");
                Console.WriteLine("Endpoint:" + myList.LocalEndpoint);
                Console.WriteLine("Waiting for connection");

                Socket s = myList.AcceptSocket();
                Console.WriteLine("Connection Accepted");

                // prepares to receive packet
                byte[] b = new byte[100];
                int k = s.Receive(b);
                Console.WriteLine("Received...");
                for (int i = 0; i < k; i++)
                {
                    Console.Write(Convert.ToChar(b[i]));
                }

                // sends acknowledgment
                ASCIIEncoding asen = new ASCIIEncoding();
                s.Send(asen.GetBytes("The string was received by the server."));
                Console.WriteLine("\nSent ACK");

                // clean up
                s.Close();
                myList.Stop();
            }
            catch (Exception exception)
            {
                Console.WriteLine("Error: " + exception.StackTrace);
            }
        }
    }
}
