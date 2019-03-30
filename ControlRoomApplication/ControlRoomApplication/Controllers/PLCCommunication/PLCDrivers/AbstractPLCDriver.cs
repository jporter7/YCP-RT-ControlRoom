using System;
using System.Threading;
using System.Net;
using System.Net.Sockets;

namespace ControlRoomApplication.Controllers.PLCCommunication
{
    public abstract class AbstractPLCDriver
    {
        private TcpListener PLCTCPListener;
        private Thread ClientManagmentThread;
        private volatile bool KillClientManagementThreadFlag;

        public AbstractPLCDriver(IPAddress ip_address, int port)
        {
            try
            {
                PLCTCPListener = new TcpListener(new IPEndPoint(ip_address, port));
                ClientManagmentThread = new Thread(new ThreadStart(HandleClientManagementThread));
            }
            catch (Exception e)
            {
                if ((e is ArgumentNullException) || (e is ArgumentOutOfRangeException))
                {
                    Console.WriteLine("[AbstractPLCDriver] ERROR: failure creating PLC TCP server or management thread: " + e.ToString());
                    return;
                }
                else
                {
                    // Unexpected exception
                    throw e;
                }
            }

            try
            {
                PLCTCPListener.Start(1);
            }
            catch (Exception e)
            {
                if ((e is SocketException) || (e is ArgumentOutOfRangeException) || (e is InvalidOperationException))
                {
                    Console.WriteLine("[AbstractPLCDriver] ERROR: failure starting PLC TCP server: " + e.ToString());
                    return;
                }
            }
        }

        public AbstractPLCDriver(string ip, int port) : this(IPAddress.Parse(ip), port) { }

        public bool StartAsyncAcceptingClients()
        {
            KillClientManagementThreadFlag = false;

            try
            {
                ClientManagmentThread.Start();
            }
            catch (Exception e)
            {
                if ((e is ThreadStateException) || (e is OutOfMemoryException))
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

        public bool RequestStopAsyncAcceptingClientsAndJoin()
        {
            KillClientManagementThreadFlag = true;

            try
            {
                ClientManagmentThread.Join();
            }
            catch (Exception e)
            {
                if ((e is ThreadStateException) || (e is ThreadStartException))
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

        protected bool AttemptToWriteDataToServer(NetworkStream ActiveClientStream, byte[] ResponseData)
        {
            try
            {
                ActiveClientStream.Write(ResponseData, 0, ResponseData.Length);
            }
            catch (Exception e)
            {
                if ((e is ArgumentNullException) || (e is ArgumentOutOfRangeException) || (e is System.IO.IOException) || (e is ObjectDisposedException))
                {
                    Console.WriteLine("[AbstractPLCDriver] ERROR: writing back to client with the PLC's response {" + ResponseData.ToString() + "}");
                    return false;
                }
            }

            return true;
        }

        private void HandleClientManagementThread()
        {
            TcpClient AcceptedClient = null;
            byte[] StreamBuffer = new byte[256];
            byte[] ClippedData;

            while (!KillClientManagementThreadFlag)
            {
                if (PLCTCPListener.Pending())
                {
                    AcceptedClient = PLCTCPListener.AcceptTcpClient();
                    Console.WriteLine("[AbstractPLCDriver] Connected to new client.");

                    NetworkStream ClientStream = AcceptedClient.GetStream();

                    int Fd;
                    while ((!KillClientManagementThreadFlag) && (ClientStream != null))
                    {
                        if ((!ClientStream.CanRead) || (!ClientStream.DataAvailable))
                        {
                            continue;
                        }

                        Fd = ClientStream.Read(StreamBuffer, 0, StreamBuffer.Length);

                        if (Fd == 0)
                        {
                            continue;
                        }

                        try
                        {
                            ClippedData = new byte[Fd];
                            Array.Copy(StreamBuffer, ClippedData, ClippedData.Length);

                            if (!ProcessRequest(ClientStream, ClippedData))
                            {
                                Console.WriteLine("[AbstractPLCDriver] FAILED to write server.");
                            }
                            else
                            {
                                //Console.WriteLine("[AbstractPLCDriver] Successfully wrote to server: [{0}]", string.Join(", ", ClippedData));
                                //Console.WriteLine("[AbstractPLCDriver] Successfully wrote to server!");
                            }
                        }
                        catch (Exception e)
                        {
                            if ((e is ArgumentNullException)
                                || (e is RankException)
                                || (e is ArrayTypeMismatchException)
                                || (e is InvalidCastException)
                                || (e is ArgumentOutOfRangeException)
                                || (e is ArgumentException))
                            {
                                Console.WriteLine("[AbstractPLCDriver] ERROR: copying buffer array into clipped array {" + Fd + "}, skipping... [" + e.ToString());
                                continue;
                            }
                            else
                            {
                                // Unexpected exception
                                throw e;
                            }
                        }
                    }

                    ClientStream.Dispose();
                    AcceptedClient.Dispose();
                }
            }
        }

        protected abstract bool ProcessRequest(NetworkStream ActiveClientStream, byte[] query);
    }
}