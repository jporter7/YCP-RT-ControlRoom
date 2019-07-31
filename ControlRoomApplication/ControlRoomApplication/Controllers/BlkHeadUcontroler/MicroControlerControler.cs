using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ControlRoomApplication.Controllers.BlkHeadUcontroler
{
    /// <summary>
    /// controler for micro prosor in the 
    /// </summary>
    public class MicroControlerControler : AbstractMicrocontroller
    {
        public MicroControlerControler() : base() { }
        /// <summary>
        /// state of tcp conection
        /// </summary>
        public class StateObject
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
        /// <summary>
        /// listens for incoming conections from the U prossor
        /// </summary>
        public class AsynchronousSocketListener
        {
            /// Thread signal.  
            public static ManualResetEvent allDone = new ManualResetEvent(false);
            /// <summary>
            /// start listing for tcp conections
            /// </summary>
            public static void BringUp()
            {
                // Establish the local endpoint for the socket.  
                // The DNS name of the computer  
                // running the listener is "host.contoso.com".  
                IPHostEntry ipHostInfo = Dns.GetHostEntry(Dns.GetHostName());
                IPAddress ipAddress = ipHostInfo.AddressList[ipHostInfo.AddressList.Length-1];
                //ipAddress= IPAddress.Parse("169.254.28.40");
                IPEndPoint localEndPoint = new IPEndPoint(ipAddress, 1600);
                Console.WriteLine("this ip "+ localEndPoint);
                // Create a TCP/IP socket.  
                Socket listener = new Socket(ipAddress.AddressFamily,
                    SocketType.Stream, ProtocolType.Tcp);

                // Bind the socket to the local endpoint and listen for incoming connections.  
                try
                {
                    listener.Bind(localEndPoint);
                    listener.Listen(100);
                    while (true)
                    {
                        // Set the event to nonsignaled state.  
                        allDone.Reset();
                        // Start an asynchronous socket to listen for connections.  
                        //Console.WriteLine("Waiting for a connection...");
                        listener.BeginAccept(new AsyncCallback(AcceptCallback),listener);
                        // Wait until a connection is made before continuing.  
                        allDone.WaitOne();
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.ToString());
                }
                Console.WriteLine("\nPress ENTER to continue...");
                Console.Read();
            }
            /// <summary>
            /// callback for accepting tcp conections
            /// </summary>
            /// <param name="ar"></param>
            public static void AcceptCallback(IAsyncResult ar)
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
            public static void ReadCallback(IAsyncResult ar)
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
                            //Console.WriteLine(respobj);
                            interpretData(respobj);
                            
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
                            else if (content == "alert")
                            {

                            }
                            else
                            {
                                Send(handler, "400");//could not parse JSON
                                Console.WriteLine(e+" line  165");
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
                    //Console.WriteLine("Sent {0} bytes to client.", bytesSent);
                    //handler.Shutdown(SocketShutdown.Both);
                    //handler.Close();
                }
                catch (Exception e)
                {
                    if (!(e is System.ObjectDisposedException)){
                        Console.WriteLine(e.ToString()+"   line 195");
                    }
                    
                }
            }

            /*
            static void interpretData(dynamic data)
            {
                double threshold = 0;
                try
                {
                    if (data.type == "temp")
                    {
                        threshold = 80;
                    }
                    else if (data.type== "vibration")
                    {
                        threshold = 1.65;
                    }
                    else
                    {
                        Console.WriteLine("datatype not found");
                        return;
                    }
                    foreach(dynamic element in data.data)
                    {
                        if (element.val > threshold)
                        {
                            Console.WriteLine(element.val);
                            Console.WriteLine(element.time - DateTimeOffset.UtcNow.ToUnixTimeMilliseconds());
                            //Console.WriteLine();
                            //Console.WriteLine(typeof(element.time));
                            //Console.WriteLine( (Int32.Parse(element.time) - DateTime.UtcNow.Millisecond));

                        }

                    }
                }catch(Exception e)
                {
                    Console.WriteLine(e + "line 229");
                }
            }
            */
        }
    }
}
