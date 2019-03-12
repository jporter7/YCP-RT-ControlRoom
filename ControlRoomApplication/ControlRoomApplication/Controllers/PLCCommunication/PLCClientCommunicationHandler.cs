using System;
using System.Threading;
using System.Net.Sockets;
using ControlRoomApplication.Entities;

namespace ControlRoomApplication.Controllers.PLCCommunication
{
    public class PLCClientCommunicationHandler
    {
        private Thread StreamCommunicationThread;
        private Mutex StreamCommunicationThreadMutex;
        private bool KillStreamCommunicationThreadFlag;
        private bool HasActiveConnection { get; set; }

        private bool IncomingDataSet { get; set; }
        private byte[] IncomingData { get; set; }

        private bool OutgoingDataSet { get; set; }
        private byte[] OutgoingData { get; set; }
        private int ExpectedResponseDataSize { get; set; }
        private int ResponseTimeoutMS { get; set; }

        public PLCClientCommunicationHandler(string ip, int port)
        {
            StreamCommunicationThread = new Thread(() => RunTCPServer(ip, port));
            StreamCommunicationThreadMutex = new Mutex();
            KillStreamCommunicationThreadFlag = false;
            HasActiveConnection = false;
        }

        public void ConnectToServer()
        {
            StreamCommunicationThread.Start();
        }

        public void TerminateTCPServerConnection()
        {
            StreamCommunicationThreadMutex.WaitOne();
            KillStreamCommunicationThreadFlag = true;
            StreamCommunicationThreadMutex.ReleaseMutex();
        }

        private void RunTCPServer(string ip, int port)
        {
            if (HasActiveConnection)
            {
                Console.WriteLine("[PLCClientCommunicationHandler] Failed to connect to TCP server client while attempting to bring up PLC Controller: instance is busy");
                return;
            }

            TcpClient PLCTCPClient;
            NetworkStream PLCTCPStream;
            try
            {
                PLCTCPClient = new TcpClient(ip, port);
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
                    Console.WriteLine("[PLCClientCommunicationHandler] Failed to connect to TCP server client while attempting to bring up PLC Controller: error establishing connection");
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

            bool KeepAlive = !KillStreamCommunicationThreadFlag;
            while (KeepAlive)
            {
                StreamCommunicationThreadMutex.WaitOne();

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
                        Console.WriteLine("[PLCClientCommunicationHandler] UH OH: " + TotalRead.ToString() + " vs. " + ExpectedResponseDataSize.ToString());
                    }

                    IncomingData = TemporaryResponseBuffer;
                    IncomingDataSet = true;
                }

                if (OutgoingDataSet)
                {
                    if (OutgoingData.Length <= 0)
                    {
                        Console.WriteLine("[PLCClientCommunicationHandler] No data found to be sent.");
                        OutgoingDataSet = false;
                    }

                    PLCTCPStream.Write(OutgoingData, 0, OutgoingData.Length);

                    OutgoingDataSet = false;
                }

                KeepAlive = !KillStreamCommunicationThreadFlag && PLCTCPClient.Connected;

                StreamCommunicationThreadMutex.ReleaseMutex();
            }

            Console.WriteLine("[PLCClientCommunicationHandler] Exited main loop.");
        }

        private byte[] ReadResponse()
        {
            if (ExpectedResponseDataSize == 0)
            {
                Console.WriteLine("[PLCClientCommunicationHandler] WARNING: Expected message size was 0.");
                return null;
            }

            DateTime StartTime = DateTime.Now;
            while (true)
            {
                int AllowableTimeout = ResponseTimeoutMS - (int)((DateTime.Now - StartTime).TotalMilliseconds);
                if (AllowableTimeout <= 0)
                {
                    Console.WriteLine("[PLCClientCommunicationHandler] Timed out waiting for response.");
                    return null;
                }

                if (StreamCommunicationThreadMutex.WaitOne(AllowableTimeout))
                {
                    if (IncomingDataSet)
                    {
                        byte[] DataCopy = new byte[IncomingData.Length];
                        Array.Copy(IncomingData, DataCopy, DataCopy.Length);

                        IncomingDataSet = false;

                        StreamCommunicationThreadMutex.ReleaseMutex();

                        return DataCopy;
                    }
                    else
                    {
                        StreamCommunicationThreadMutex.ReleaseMutex();
                    }
                }
            }
        }

        private void SendMessage(byte[] ByteData, int ExpectedResponseSize = 0, int TimeoutMS = 0)
        {
            StreamCommunicationThreadMutex.WaitOne();

            ExpectedResponseDataSize = ExpectedResponseSize;
            ResponseTimeoutMS = TimeoutMS;
            OutgoingData = ByteData;
            OutgoingDataSet = true;

            StreamCommunicationThreadMutex.ReleaseMutex();
        }

        private byte[] SendMessageWithResponse(byte[] ByteData, int ExpectedResponseSize, int TimeoutMS = 10000)
        {
            SendMessage(ByteData, ExpectedResponseSize, TimeoutMS);
            return ReadResponse();
        }

        public object RequestMessageSend(PLCCommandAndQueryTypeEnum MessageType, params object[] MessageParameters)
        {
            byte[] NetOutgoingMessage =
            {
                0x13, 0x0,
                PLCCommandAndQueryTypeConversionHelper.ConvertToByte(MessageType),
                0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0,
                0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0
            };

            PLCCommandResponseExpectationEnum ResponseExpectationValue;

            switch (MessageType)
            {
                case PLCCommandAndQueryTypeEnum.TEST_CONNECTION:
                case PLCCommandAndQueryTypeEnum.GET_CURRENT_AZEL_POSITIONS:
                case PLCCommandAndQueryTypeEnum.GET_CURRENT_LIMIT_SWITCH_STATUSES:
                case PLCCommandAndQueryTypeEnum.GET_CURRENT_SAFETY_INTERLOCK_STATUS:
                    {
                        ResponseExpectationValue = PLCCommandResponseExpectationEnum.EXPECTING_RESPONSE;
                        break;
                    }

                case PLCCommandAndQueryTypeEnum.CANCEL_ACTIVE_OBJECTIVE_AZEL_POSITION:
                case PLCCommandAndQueryTypeEnum.SHUTDOWN:
                case PLCCommandAndQueryTypeEnum.CALIBRATE:
                case PLCCommandAndQueryTypeEnum.SET_OBJECTIVE_AZEL_POSITION:
                    {
                        ResponseExpectationValue = PLCCommandResponseExpectationEnum.NOT_EXPECTING_RESPONSE;
                        break;
                    }

                default:
                    {
                        throw new ArgumentException("Illegal PLCCommandAndQueryTypeEnum value: " + MessageType.ToString());
                    }
            }
            
            NetOutgoingMessage[2] += (byte)(PLCCommandResponseExpectationConversionHelper.ConvertToByte(ResponseExpectationValue) * 0x40);

            Console.WriteLine("[PLCClientCommunicationHandler] About to send command " + MessageType.ToString());

            if (ResponseExpectationValue == PLCCommandResponseExpectationEnum.EXPECTING_RESPONSE)
            {
                // This 0x13 is the expected response size of anything from the PLC (simulated or real)
                // See the google sheets file describing this under Wiki Documentation -> Control Room in the shared GDrive
                return SendMessageWithResponse(NetOutgoingMessage, 0x13);
            }
            else
            {
                SendMessage(NetOutgoingMessage);
                return true;
            }
        }
    }
}