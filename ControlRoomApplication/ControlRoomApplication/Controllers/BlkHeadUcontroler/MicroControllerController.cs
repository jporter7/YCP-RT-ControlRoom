using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ControlRoomApplication.Util;


namespace ControlRoomApplication.Controllers.BlkHeadUcontroler
{
    /// <summary>
    /// controler for micro prosor in the 
    /// </summary>
    public class MicroControlerControler : AbstractMicrocontroller
    {
        private static readonly log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private string local_adress;
        private int port;
        /// <summary>
        /// constructor for the micrcontroler only has bring up method which starts a server listing on ip and port
        /// </summary>
        public MicroControlerControler() : base() { }

        public MicroControlerControler(string IP,int port ) {
            this.port = port;
            local_adress = IP;
        }
        /// <summary>
        /// state of tcp conection
        /// </summary>
        private class StateObject
        {
            /// Client  socket.  
            public Socket workSocket = null;
            /// Size of receive buffer.  
            public const int BufferSize = 1024;
            /// Receive buffer.  
            public byte[] buffer = new byte[BufferSize];
            /// Received data string.  
            public StringBuilder sb = new StringBuilder();
        }

        /// Thread signal.  
        private static ManualResetEvent allDone = new ManualResetEvent(false);
        /// <summary>
        /// start listing for tcp conections
        /// </summary>
        public override bool BringUp()
        {
            // Establish the local endpoint for the socket.  
            // The DNS name of the computer  
            // running the listener is "host.contoso.com".  
            IPHostEntry ipHostInfo = Dns.GetHostEntry(Dns.GetHostName());
            IPAddress ipAddress = ipHostInfo.AddressList[ipHostInfo.AddressList.Length-1];
            ipAddress= IPAddress.Parse( local_adress );
            IPEndPoint localEndPoint = new IPEndPoint(ipAddress, port);
            logger.Info(Utilities.GetTimeStamp() + ": this ip " + localEndPoint);
            // Create a TCP/IP socket.  
            Socket listener = new Socket(ipAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

            // Bind the socket to the local endpoint and listen for incoming connections.  
            try
            {
                listener.Bind(localEndPoint);
                listener.Listen(20);//takes that max number of conections to store in the backlog
                while (true)
                {
                    // Set the event to nonsignaled state.  
                    allDone.Reset();
                    // Start an asynchronous socket to listen for connections.  
                    listener.BeginAccept(new AsyncCallback(AcceptCallback),listener);
                    // Wait until a connection is made before continuing.  
                    allDone.WaitOne();
                }
            }
            catch (Exception e)
            {
                logger.Info(e.ToString());
            }
            return true;
        }
        /// <summary>
        /// callback for accepting tcp conections
        /// </summary>
        /// <param name="ar"></param>
        private static void AcceptCallback(IAsyncResult ar)
        {
            // Signal the main thread to continue.  
            allDone.Set();
            // Get the socket that handles the client request.  
            Socket listener = (Socket)ar.AsyncState;
            Socket handler = listener.EndAccept(ar);
            // Create the state object.  
            StateObject state = new StateObject();
            state.workSocket = handler;
            handler.BeginReceive(state.buffer, 0, StateObject.BufferSize, 0, new AsyncCallback(ReadCallback), state);
        }
        /// <summary>
        /// callback occors when scoket has new data
        /// </summary>
        /// <param name="ar"></param>
        private static void ReadCallback(IAsyncResult ar)
        {
            String content = String.Empty;
            // Retrieve the state object and the handler socket  
            // from the asynchronous state object.  
            StateObject state = (StateObject)ar.AsyncState;
            Socket handler = state.workSocket;
            // Read data from the client socket.   
            int bytesRead=0;
            try
            {
                bytesRead = handler.EndReceive(ar);
            }
            catch
            {

            }
                
            if (bytesRead > 0)
            {
                // There  might be more data, so store the data received so far.  
                state.sb.Append(Encoding.ASCII.GetString(
                    state.buffer, 0, bytesRead));

                // Check for end-of-file tag. If it is not there, read   
                // more data.  
                content = state.sb.ToString();
                int eof = content.IndexOf("<EOF>");
                if (eof > -1)
                {
                    content = content.Substring(0, eof);
                    // All the data has been read from the   
                    // client. Display it on the console. 
                    try
                    {
                        dynamic respobj = JsonConvert.DeserializeObject(content);
                    //     interpretData(respobj);
                            
                        Send(handler, "200-"+ respobj.uuid);

                    }
                    catch (Exception e)
                    {
                        if (content == "getMode")
                        {
                            Send(handler, "run");
                            handler.Shutdown(SocketShutdown.Both);
                            handler.Close();
                        }
                        else if (content == "alert")//TODO: if I ever decide to send alerts from the U prossor this should do something with them
                        {

                        }
                        else
                        {
                            Send(handler, "400");//could not parse JSON
                            logger.Info(e + " line  165");
                        }
                        return;
                    }
                    state.sb.Clear();
                }
                //else
                //{
                // Not all data received. Get more.  
                handler.BeginReceive(state.buffer, 0, StateObject.BufferSize, 0, new AsyncCallback(ReadCallback), state);
                //}
            }
        }

        private static void Send(Socket handler, String data)
        {
            // Convert the string data to byte data using ASCII encoding.  
            byte[] byteData = Encoding.ASCII.GetBytes(data);
            // Begin sending the data to the remote device.  
            handler.BeginSend(byteData, 0, byteData.Length, 0, new AsyncCallback(SendCallback), handler);
        }

        private static void SendCallback(IAsyncResult ar)
        {
            try
            {
                // Retrieve the socket from the state object.  
                Socket handler = (Socket)ar.AsyncState;
                // Complete sending the data to the remote device.  
                int bytesSent = handler.EndSend(ar);
                //handler.Shutdown(SocketShutdown.Both);
                //handler.Close();
            }
            catch (Exception e)
            {
                if (!(e is System.ObjectDisposedException)){
                    logger.Info(e.ToString() + "   line 195");
                }
                    
            }
        }
    }
}
