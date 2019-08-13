using System;
using System.Threading;
using System.Net.Sockets;
using ControlRoomApplication.Entities;

namespace ControlRoomApplication.Controllers
{
    public class PLCClientCommunicationHandler
    {
        private static readonly log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

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
                logger.Info("[PLCClientCommunicationHandler] Failed to connect to TCP server client while attempting to bring up PLC Controller: instance is busy");
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
                    logger.Info("[PLCClientCommunicationHandler] Failed to connect to TCP server client while attempting to bring up PLC Controller: error establishing connection");
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
                        logger.Info("[PLCClientCommunicationHandler] ERROR, inconsistent packet size: " + TotalRead.ToString() + " vs. " + ExpectedResponseDataSize.ToString());
                    }

                    IncomingData = TemporaryResponseBuffer;
                    IncomingDataSet = true;
                }

                if (OutgoingDataSet)
                {
                    if (OutgoingData.Length <= 0)
                    {
                        logger.Info("[PLCClientCommunicationHandler] No data found to be sent.");
                        OutgoingDataSet = false;
                    }

                    PLCTCPStream.Write(OutgoingData, 0, OutgoingData.Length);

                    OutgoingDataSet = false;
                }

                KeepAlive = !KillStreamCommunicationThreadFlag && PLCTCPClient.Connected;

                StreamCommunicationThreadMutex.ReleaseMutex();
            }

            logger.Info("[PLCClientCommunicationHandler] Exited main loop.");
        }

        private byte[] ReadResponse()
        {
            if (ExpectedResponseDataSize == 0)
            {
                logger.Info("[PLCClientCommunicationHandler] WARNING: Expected message size was 0.");
                return null;
            }

            DateTime StartTime = DateTime.UtcNow;
            while (true)
            {
                int AllowableTimeout = ResponseTimeoutMS - (int)((DateTime.UtcNow - StartTime).TotalMilliseconds);
                if (AllowableTimeout <= 0)
                {
                    logger.Info("[PLCClientCommunicationHandler] Timed out waiting for response.");
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

        // As far as I'm aware, this is not being used at the moment anymore. However, Garret had mentioned
        // the desire for async functionality, so I won't get rid of this just yet. (And I'll leave
        // SendMessageWithResponse how it for now as a result.)
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

        public byte[] RequestMessageSend(PLCCommandAndQueryTypeEnum MessageType, params object[] MessageParameters)
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
                        ResponseExpectationValue = PLCCommandResponseExpectationEnum.FULL_RESPONSE;
                        break;
                    }

                case PLCCommandAndQueryTypeEnum.CANCEL_ACTIVE_OBJECTIVE_AZEL_POSITION:
                case PLCCommandAndQueryTypeEnum.SHUTDOWN:
                case PLCCommandAndQueryTypeEnum.CALIBRATE:
                case PLCCommandAndQueryTypeEnum.CONTROLLED_STOP:
                case PLCCommandAndQueryTypeEnum.IMMEDIATE_STOP:
                    {
                        ResponseExpectationValue = PLCCommandResponseExpectationEnum.MINOR_RESPONSE;
                        break;
                    }

                case PLCCommandAndQueryTypeEnum.SET_CONFIGURATION:
                    {
                        ResponseExpectationValue = PLCCommandResponseExpectationEnum.MINOR_RESPONSE;

                        int StartSpeedAzimuth = (int)MessageParameters[0];
                        int StartSpeedElevation = (int)MessageParameters[1];
                        int HomeTimeoutAzimuth = (int)MessageParameters[2];
                        int HomeTimeoutElevation = (int)MessageParameters[3];

                        NetOutgoingMessage[3] = 0x84;
                        NetOutgoingMessage[4] = 0x00;
                        NetOutgoingMessage[5] = 0x00;
                        NetOutgoingMessage[6] = 0x00;

                        NetOutgoingMessage[7] = 0x0;
                        NetOutgoingMessage[8] = (byte)(StartSpeedAzimuth / 0xFFFF);
                        NetOutgoingMessage[9] = (byte)((StartSpeedAzimuth >> 8) & 0xFF);
                        NetOutgoingMessage[10] = (byte)(StartSpeedAzimuth & 0xFF);

                        NetOutgoingMessage[11] = 0x0;
                        NetOutgoingMessage[12] = (byte)(StartSpeedElevation / 0xFFFF);
                        NetOutgoingMessage[13] = (byte)((StartSpeedElevation >> 8) & 0xFF);
                        NetOutgoingMessage[14] = (byte)(StartSpeedElevation & 0xFF);

                        NetOutgoingMessage[15] = (byte)(HomeTimeoutAzimuth >> 8);
                        NetOutgoingMessage[16] = (byte)(HomeTimeoutAzimuth & 0xFF);

                        NetOutgoingMessage[17] = (byte)(HomeTimeoutElevation >> 8);
                        NetOutgoingMessage[18] = (byte)(HomeTimeoutElevation & 0xFF);

                        break;
                    }

                case PLCCommandAndQueryTypeEnum.SET_OBJECTIVE_AZEL_POSITION:
                    {
                        ResponseExpectationValue = PLCCommandResponseExpectationEnum.MINOR_RESPONSE;

                        Orientation ObjectiveOrientation = (Orientation)MessageParameters[0];
                        Array.Copy(BitConverter.GetBytes(ObjectiveOrientation.Azimuth), 0, NetOutgoingMessage, 3, 8);
                        Array.Copy(BitConverter.GetBytes(ObjectiveOrientation.Elevation), 0, NetOutgoingMessage, 11, 8);

                        break;
                    }

                case PLCCommandAndQueryTypeEnum.START_JOG_MOVEMENT:
                    {
                        ResponseExpectationValue = PLCCommandResponseExpectationEnum.MINOR_RESPONSE;

                        RadioTelescopeAxisEnum AxisEnum = (RadioTelescopeAxisEnum)MessageParameters[0];
                        int AxisJogSpeed = (int)MessageParameters[1];
                        bool JogClockwise = (bool)MessageParameters[2];

                        switch (AxisEnum)
                        {
                            case RadioTelescopeAxisEnum.AZIMUTH:
                                {
                                    NetOutgoingMessage[3] = 0x1;
                                    break;
                                }

                            case RadioTelescopeAxisEnum.ELEVATION:
                                {
                                    NetOutgoingMessage[3] = 0x2;
                                    break;
                                }

                            default:
                                {
                                    throw new ArgumentException("Invalid RadioTelescopeAxisEnum value seen while preparing jog movement bytes: " + AxisEnum.ToString());
                                }
                        }

                        NetOutgoingMessage[4] = 0x0;
                        NetOutgoingMessage[5] = (byte)(AxisJogSpeed / 0xFFFF);
                        NetOutgoingMessage[6] = (byte)((AxisJogSpeed >> 8) & 0xFF);
                        NetOutgoingMessage[7] = (byte)(AxisJogSpeed & 0xFF);

                        NetOutgoingMessage[8] = (byte)(JogClockwise ? 0x1 : 0x2);

                        break;
                    }

                case PLCCommandAndQueryTypeEnum.TRANSLATE_AZEL_POSITION:
                    {
                        ResponseExpectationValue = PLCCommandResponseExpectationEnum.MINOR_RESPONSE;

                        RadioTelescopeAxisEnum AxisEnum = (RadioTelescopeAxisEnum)MessageParameters[0];
                        int AxisJogSpeed = (int)MessageParameters[1];
                        int position = (int)MessageParameters[2];

                        switch (AxisEnum)
                        {
                            case RadioTelescopeAxisEnum.AZIMUTH:
                                {
                                    NetOutgoingMessage[3] = 0x1;
                                    break;
                                }

                            case RadioTelescopeAxisEnum.ELEVATION:
                                {
                                    NetOutgoingMessage[3] = 0x2;
                                    break;
                                }

                            default:
                                {
                                    throw new ArgumentException("Invalid RadioTelescopeAxisEnum value seen while preparing relative movement bytes: " + AxisEnum.ToString());
                                }
                        }

                        NetOutgoingMessage[4] = 0x0;
                        NetOutgoingMessage[5] = (byte)(AxisJogSpeed / 0xFFFF);
                        NetOutgoingMessage[6] = (byte)((AxisJogSpeed >> 8) & 0xFF);
                        NetOutgoingMessage[7] = (byte)(AxisJogSpeed & 0xFF);

                        if (position > 0)
                        {
                            NetOutgoingMessage[8] = 0x0;
                            NetOutgoingMessage[9] = (byte)(position / 0xFFFF);
                            NetOutgoingMessage[10] = (byte)((position >> 8) & 0xFF);
                            NetOutgoingMessage[11] = (byte)(position & 0xFF);
                        }
                        else
                        {
                            NetOutgoingMessage[8] = 0xFF;
                            NetOutgoingMessage[9] = (byte)((position / 0xFFFF) - 1);
                            NetOutgoingMessage[10] = (byte)((position >> 8) & 0xFF);
                            NetOutgoingMessage[11] = (byte)(position & 0xFF);
                        }

                        break;
                    }

                default:
                    {
                        throw new ArgumentException("Illegal PLCCommandAndQueryTypeEnum value: " + MessageType.ToString());
                    }
            }
            
            NetOutgoingMessage[2] += (byte)(PLCCommandResponseExpectationConversionHelper.ConvertToByte(ResponseExpectationValue) * 0x40);

            // This is the expected response size of anything from the PLC (simulated or real), minor or full response.
            // See the TCP/IP packet contents google sheets file describing this under Wiki Documentation -> Control Room in the shared GDrive
            byte ExpectedResponseSize = (ResponseExpectationValue == PLCCommandResponseExpectationEnum.FULL_RESPONSE) ? (byte)0x13 : (byte)0x3;

            return SendMessageWithResponse(NetOutgoingMessage, ExpectedResponseSize);
        }
    }
}
