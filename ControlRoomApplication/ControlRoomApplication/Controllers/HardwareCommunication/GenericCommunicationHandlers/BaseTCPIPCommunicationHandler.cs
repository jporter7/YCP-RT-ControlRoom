using System;
using System.Threading;
using System.Net.Sockets;

namespace ControlRoomApplication.Controllers
{
    public abstract class BaseTCPIPCommunicationHandler : AbstractHardwareCommunicationHandler
    {
        private static readonly log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private Thread CommunicationThread;
        protected Mutex CommunicationThreadMutex;
        protected bool KillCommunicationThreadFlag;
        public bool HasActiveConnection { get; protected set; }

        private bool IncomingDataSet { get; set; }
        private byte[] IncomingData { get; set; }

        private bool OutgoingDataSet { get; set; }
        private byte[] OutgoingData { get; set; }
        private int ExpectedResponseDataSize { get; set; }
        private int ResponseTimeoutMS { get; set; }

        private string CommsIPAddress;
        private int CommsPort;

        public BaseTCPIPCommunicationHandler(string ip, int port)
        {
            CommunicationThread = new Thread(new ThreadStart(CommunicationRoutine));
            CommunicationThreadMutex = new Mutex();
            KillCommunicationThreadFlag = false;
            HasActiveConnection = false;

            CommsIPAddress = ip;
            CommsPort = port;
        }

        public override bool StartHandler()
        {
            try
            {
                CommunicationThread.Start();
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

        public override bool DisposeHandler()
        {
            try
            {
                CommunicationThreadMutex.WaitOne();
                KillCommunicationThreadFlag = true;
                CommunicationThreadMutex.ReleaseMutex();
                CommunicationThread.Join();
            }
            catch (Exception e)
            {
                if ((e is ObjectDisposedException) || (e is AbandonedMutexException) || (e is InvalidOperationException)
                    || (e is ApplicationException) || (e is ThreadStateException) || (e is ThreadInterruptedException))
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

        protected void QueueOutgoingMessage(byte[] ByteData, bool ExpectsResponse, int ExpectedResponseSize = 0, int TimeoutMS = 1000)
        {
            ByteData[2] += (byte)(ExpectsResponse ? 0x80 : 0x40);

            CommunicationThreadMutex.WaitOne();

            ExpectedResponseDataSize = ExpectedResponseSize;
            ResponseTimeoutMS = TimeoutMS;
            OutgoingData = ByteData;
            OutgoingDataSet = true;

            CommunicationThreadMutex.ReleaseMutex();
        }

        protected byte[] ReadResponse()
        {
            if (ExpectedResponseDataSize == 0)
            {
                logger.Info("[TCPIPCommunicationHandler] WARNING: Expected message size was 0.");
                return null;
            }

            DateTime StartTime = DateTime.UtcNow;
            while (true)
            {
                int AllowableTimeout = ResponseTimeoutMS - (int)((DateTime.UtcNow - StartTime).TotalMilliseconds);
                if (AllowableTimeout <= 0)
                {
                    logger.Info("[TCPIPCommunicationHandler] Timed out waiting for response.");
                    return null;
                }

                if (CommunicationThreadMutex.WaitOne(AllowableTimeout))
                {
                    if (IncomingDataSet)
                    {
                        byte[] DataCopy = new byte[IncomingData.Length];
                        Array.Copy(IncomingData, DataCopy, DataCopy.Length);

                        IncomingDataSet = false;

                        CommunicationThreadMutex.ReleaseMutex();

                        return DataCopy;
                    }
                    else
                    {
                        CommunicationThreadMutex.ReleaseMutex();
                    }
                }
            }
        }

        protected byte[] QueueOutgoingMessageAndReadResponse(byte[] ByteData, bool ExpectsResponse, int ExpectedResponseSize = 0, int TimeoutMS = 1000)
        {
            QueueOutgoingMessage(ByteData, ExpectsResponse, ExpectedResponseSize, TimeoutMS);
            return ReadResponse();
        }

        private void CommunicationRoutine()
        {
            if (HasActiveConnection)
            {
                logger.Info("[TCPIPCommunicationHandler] Failed to connect to the TCP server client while attempting to bring up hardware Controller: instance is busy");
                return;
            }

            TcpClient PLCTCPClient;
            NetworkStream PLCTCPStream;
            try
            {
                PLCTCPClient = new TcpClient(CommsIPAddress, CommsPort);
                PLCTCPStream = PLCTCPClient.GetStream();
            }
            catch (Exception e)
            {
                if ((e is ArgumentNullException)
                    || (e is ArgumentOutOfRangeException)
                    || (e is SocketException)
                    || (e is InvalidOperationException)
                    || (e is ObjectDisposedException))
                {
                    logger.Info("[TCPIPCommunicationHandler] Failed to connect to TCP server client while attempting to bring up hardware Controller: error establishing connection");
                    HasActiveConnection = false;
                    return;
                }
                else
                {
                    // Unexpected exception type
                    throw e;
                }
            }

            HasActiveConnection = true;

            byte[] TemporaryResponseBuffer;

            bool KeepAlive = !KillCommunicationThreadFlag;
            while (KeepAlive)
            {
                CommunicationThreadMutex.WaitOne();

                if (PLCTCPStream.DataAvailable)
                {
                    TemporaryResponseBuffer = new byte[ExpectedResponseDataSize];

                    int TotalRead = 0;
                    while (PLCTCPStream.DataAvailable)
                    {
                        TotalRead += PLCTCPStream.Read(TemporaryResponseBuffer, TotalRead, TemporaryResponseBuffer.Length);
                    }

                    if (TotalRead != ExpectedResponseDataSize)
                    {
                        logger.Info("[TCPIPCommunicationHandler] ERROR, inconsistent packet size: " + TotalRead.ToString() + " vs. " + ExpectedResponseDataSize.ToString());
                    }

                    IncomingData = TemporaryResponseBuffer;
                    IncomingDataSet = true;
                }

                if (OutgoingDataSet)
                {
                    if (OutgoingData.Length <= 0)
                    {
                        logger.Info("[TCPIPCommunicationHandler] No data found to be sent.");
                        OutgoingDataSet = false;
                    }

                    PLCTCPStream.Write(OutgoingData, 0, OutgoingData.Length);

                    OutgoingDataSet = false;
                }

                KeepAlive = !KillCommunicationThreadFlag && PLCTCPClient.Connected;

                CommunicationThreadMutex.ReleaseMutex();
            }

            logger.Info("[TCPIPCommunicationHandler] Exited main loop.");
        }
    }
}