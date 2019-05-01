using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace ControlRoomApplication.Controllers
{
    public abstract class BaseTCPIPHardwareReceiver : AbstractSimulationHardwareReceiver
    {
        private static readonly log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private TcpListener HardwareTCPListener;
        private Thread CommunicationManagmentThread;
        private volatile bool KillClientManagementThreadFlag;

        public BaseTCPIPHardwareReceiver(IPAddress ip_address, int port)
        {
            try
            {
                HardwareTCPListener = new TcpListener(new IPEndPoint(ip_address, port));
                CommunicationManagmentThread = new Thread(new ThreadStart(HandleCommunicationManagementThread));
            }
            catch (Exception e)
            {
                if ((e is ArgumentNullException) || (e is ArgumentOutOfRangeException))
                {
                    logger.Info("[AbstractHardwareReceiver] ERROR: failure creating hardware TCP server or management thread: " + e.ToString());
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
                HardwareTCPListener.Start(1);
            }
            catch (Exception e)
            {
                if ((e is SocketException) || (e is ArgumentOutOfRangeException) || (e is InvalidOperationException))
                {
                    logger.Info("[AbstractHardwareReceiver] ERROR: failure starting hardware TCP server: " + e.ToString());
                    return;
                }
            }
        }

        public BaseTCPIPHardwareReceiver(string ip, int port) : this(IPAddress.Parse(ip), port) { }

        public bool StartAsyncAcceptingClients()
        {
            KillClientManagementThreadFlag = false;

            try
            {
                CommunicationManagmentThread.Start();
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

        public bool StopAsyncAcceptingClientsAndJoin()
        {
            KillClientManagementThreadFlag = true;

            try
            {
                CommunicationManagmentThread.Join();
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

        protected bool RespondWith(NetworkStream ActiveClientStream, byte[] ResponseData)
        {
            try
            {
                ActiveClientStream.Write(ResponseData, 0, ResponseData.Length);
            }
            catch (Exception e)
            {
                if ((e is ArgumentNullException) || (e is ArgumentOutOfRangeException) || (e is System.IO.IOException) || (e is ObjectDisposedException))
                {
                    logger.Info("[AbstractHardwareReceiver] ERROR: writing back to client with the hardware's response {" + ResponseData.ToString() + "}");
                    return false;
                }
            }

            return true;
        }

        private void HandleCommunicationManagementThread()
        {
            TcpClient AcceptedClient = null;
            byte[] StreamBuffer = new byte[256];
            byte[] ClippedData;

            while (!KillClientManagementThreadFlag)
            {
                if (HardwareTCPListener.Pending())
                {
                    AcceptedClient = HardwareTCPListener.AcceptTcpClient();
                    logger.Info("[AbstractHardwareReceiver] Connected to new client.");

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
                                logger.Info("[AbstractHardwareReceiver] FAILED to write server.");
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
                                logger.Info("[AbstractHardwareReceiver] ERROR: copying buffer array into clipped array {" + Fd + "}, skipping... [" + e.ToString());
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

        public override bool StartReceiver()
        {
            return StartAsyncAcceptingClients();
        }

        public override bool DisposeReceiver()
        {
            return StopAsyncAcceptingClientsAndJoin();
        }

        protected abstract bool ProcessRequest(NetworkStream ActiveClientStream, byte[] query);
    }
}