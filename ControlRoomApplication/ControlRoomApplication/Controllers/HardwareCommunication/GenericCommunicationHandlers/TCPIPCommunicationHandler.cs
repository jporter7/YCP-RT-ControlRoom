using System;
using System.Net;
using System.Net.Sockets;
using ControlRoomApplication.Entities;

namespace ControlRoomApplication.Controllers
{
    public class TCPIPCommunicationHandler : AbstractHardwareCommunicationHandler
    {
        private static readonly log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private bool IncomingDataSet { get; set; }
        private byte[] IncomingData { get; set; }

        private bool OutgoingDataSet { get; set; }
        private byte[] OutgoingData { get; set; }
        private int ExpectedResponseDataSize { get; set; }
        private int ResponseTimeoutMS { get; set; }

        private IPAddress CommsIPAddress;
        private int CommsPort;

        public TCPIPCommunicationHandler(IPAddress ip, int port) : base()
        {
            CommsIPAddress = ip;
            CommsPort = port;
        }

        public TCPIPCommunicationHandler(string ip, int port) : this(IPAddress.Parse(ip), port)
        {
            // Does nothing else
        }

        protected override void InitCommunicationThreadElements()
        {
            // Does nothing else
            // There was a scoping issue when trying to create TcpClient instance used for communication in this function,
            // so it's done down in CommunicationRoutine instead
        }
        
        private void PassOffOutgoingMessage(byte[] ByteData, int ExpectedResponseSize = 0, int TimeoutMS = 1000)
        {
            CommunicationThreadMutex.WaitOne();

            ExpectedResponseDataSize = ExpectedResponseSize;
            ResponseTimeoutMS = TimeoutMS;
            OutgoingData = ByteData;
            OutgoingDataSet = true;

            CommunicationThreadMutex.ReleaseMutex();
        }

        public override void SendMessage(HardwareMessageTypeEnum MessageType, params object[] MessageParameters)
        {
            byte[] NetOutgoingMessage =
            {
                0x13, 0x0,
                HardwareMessageTypeEnumConversionHelper.ConvertToByte(MessageType),
                0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0,
                0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0
            };

            HardwareMesageResponseExpectationEnum ResponseExpectationValue;

            switch (MessageType)
            {
                case HardwareMessageTypeEnum.TEST_CONNECTION:
                case HardwareMessageTypeEnum.GET_CURRENT_AZEL_POSITIONS:
                case HardwareMessageTypeEnum.GET_CURRENT_LIMIT_SWITCH_STATUSES:
                case HardwareMessageTypeEnum.GET_CURRENT_SAFETY_INTERLOCK_STATUS:
                    {
                        ResponseExpectationValue = HardwareMesageResponseExpectationEnum.FULL_RESPONSE;
                        break;
                    }

                case HardwareMessageTypeEnum.CANCEL_ACTIVE_OBJECTIVE_AZEL_POSITION:
                case HardwareMessageTypeEnum.SHUTDOWN:
                case HardwareMessageTypeEnum.CALIBRATE:
                case HardwareMessageTypeEnum.CONTROLLED_STOP:
                case HardwareMessageTypeEnum.IMMEDIATE_STOP:
                    {
                        ResponseExpectationValue = HardwareMesageResponseExpectationEnum.MINOR_RESPONSE;
                        break;
                    }

                case HardwareMessageTypeEnum.SET_CONFIGURATION:
                    {
                        ResponseExpectationValue = HardwareMesageResponseExpectationEnum.MINOR_RESPONSE;

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

                case HardwareMessageTypeEnum.SET_OBJECTIVE_AZEL_POSITION:
                    {
                        ResponseExpectationValue = HardwareMesageResponseExpectationEnum.MINOR_RESPONSE;

                        Orientation ObjectiveOrientation = (Orientation)MessageParameters[0];
                        Array.Copy(BitConverter.GetBytes(ObjectiveOrientation.Azimuth), 0, NetOutgoingMessage, 3, 8);
                        Array.Copy(BitConverter.GetBytes(ObjectiveOrientation.Elevation), 0, NetOutgoingMessage, 11, 8);

                        break;
                    }

                case HardwareMessageTypeEnum.START_JOG_MOVEMENT:
                    {
                        ResponseExpectationValue = HardwareMesageResponseExpectationEnum.MINOR_RESPONSE;

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

                case HardwareMessageTypeEnum.TRANSLATE_AZEL_POSITION:
                    {
                        ResponseExpectationValue = HardwareMesageResponseExpectationEnum.MINOR_RESPONSE;

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
                        throw new ArgumentException("Illegal HardwareCommandAndQueryTypeEnum value: " + MessageType.ToString());
                    }
            }

            NetOutgoingMessage[2] += (byte)(HardwareMessageResponseExpectationConversionHelper.ConvertToByte(ResponseExpectationValue) * 0x40);

            // This is the expected response size of anything from the PLC (simulated or real), minor or full response.
            // See the TCP/IP packet contents google sheets file describing this under Wiki Documentation -> Control Room in the shared GDrive
            byte ExpectedResponseSize = (ResponseExpectationValue == HardwareMesageResponseExpectationEnum.FULL_RESPONSE) ? (byte)0x13 : (byte)0x3;

            PassOffOutgoingMessage(NetOutgoingMessage, ExpectedResponseSize);
        }

        public override byte[] ReadResponse()
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

        public override byte[] SendMessageAndReadResponse(HardwareMessageTypeEnum MessageType, params object[] MessageParameters)
        {
            SendMessage(MessageType, MessageParameters);
            return ReadResponse();
        }

        protected override void CommunicationRoutine()
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
                PLCTCPClient = new TcpClient(CommsIPAddress.ToString(), CommsPort);
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
