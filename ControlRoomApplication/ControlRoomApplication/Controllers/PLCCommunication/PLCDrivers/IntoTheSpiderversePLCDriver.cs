using System;
using System.Threading;
using System.Net.Sockets;
using ControlRoomApplication.Constants;
using ControlRoomApplication.Entities;

namespace ControlRoomApplication.Controllers
{
    public class IntoTheSpiderversePLCDriver : AbstractPLCDriver
    {
        private static readonly log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private enum DataTypeSet
        {
            CONFIGURATION,
            COMMAND
        };

        private Thread MCUCommsThread;
        private Mutex MCUCommsMutex;
        private volatile bool KeepMCUCommsThreadAlive;

        private volatile bool IncomingDataSet;
        private volatile bool OutgoingDataSet;
        private byte[] InterprocessData;
        private volatile DataTypeSet InterprocessDataType;

        public IntoTheSpiderversePLCDriver(string ipLocal, int portLocal, string ipMCU, int portMCU) : base(ipLocal, portLocal)
        {
            MCUCommsThread = new Thread(() => MCUCommsThreadRoutine(ipMCU, portMCU))
            {
                Name = "MCU Communication Thread"
            };
            MCUCommsMutex = new Mutex();
            KeepMCUCommsThreadAlive = false;
        }

        public void StartMCUCommsThreadRoutine()
        {
            KeepMCUCommsThreadAlive = true;
            MCUCommsThread.Start();
        }

        private void MCUCommsThreadRoutine(string ipMCU, int portMCU)
        {
            TcpClient MCUTCPClient;
            NetworkStream MCUTCPStream;

            try
            {
                MCUTCPClient = new TcpClient(ipMCU, portMCU);
                MCUTCPStream = MCUTCPClient.GetStream();
            }
            catch (Exception e)
            {
                if ((e is ArgumentNullException)
                    || (e is ArgumentOutOfRangeException)
                    || (e is SocketException)
                    || (e is InvalidOperationException)
                    || (e is ObjectDisposedException))
                {
                    logger.Info("[IntoTheSpiderversePLCDriver] Failed to connect to MCU's TCP server client.");
                    return;
                }
                else
                {
                    // Unexpected exception type
                    throw e;
                }
            }

            IncomingDataSet = false;
            OutgoingDataSet = false;
            InterprocessData = null;

            byte[] MCUCommsPacketBuffer;

            while (KeepMCUCommsThreadAlive)
            {
                if (IncomingDataSet)
                {
                    MCUCommsMutex.WaitOne();

                    IncomingDataSet = false;

                    MCUCommsPacketBuffer = null;

                    if (InterprocessDataType == DataTypeSet.CONFIGURATION)
                    {
                        MCUCommsPacketBuffer = new byte[]
                        {
                            0x84, 0x00,
                            0x00, 0x00,
                            InterprocessData[0], InterprocessData[1],
                            InterprocessData[2], InterprocessData[3],
                            InterprocessData[4], InterprocessData[5],
                            0x00, 0x00,
                            0x00, 0x00,
                            0x00, 0x00,
                            0x00, 0x00,
                            0x00, 0x00
                        };
                    }
                    else if (InterprocessDataType == DataTypeSet.COMMAND)
                    {
                        int PeakSpeed = HardwareConstants.ACTUAL_MCU_DEFAULT_PEAK_VELOCITY;
                        int Acceleration = HardwareConstants.ACTUAL_MCU_DEFAULT_ACCELERATION;

                        //Array.Copy(BitConverter.GetBytes(CurrentOrientation.Azimuth), 0, FinalResponseContainer, 3, 8);

                        MCUCommsPacketBuffer = new byte[]
                        {
                            0x00, 0x01,
                            0x00, 0x03,
                            InterprocessData[0], InterprocessData[1],
                            InterprocessData[2], InterprocessData[3],
                            InterprocessData[4], InterprocessData[5],
                            InterprocessData[6], InterprocessData[7],
                            InterprocessData[8], InterprocessData[9],
                            InterprocessData[10], InterprocessData[11],
                            0x00, 0x00,
                            0x00, 0x00
                        };
                    }

                    InterprocessData = null;

                    try
                    {
                        MCUTCPStream.Write(MCUCommsPacketBuffer, 0, MCUCommsPacketBuffer.Length);
                        InterprocessData = new byte[] { 0x1 };
                    }
                    catch (Exception e)
                    {
                        if ((e is ArgumentNullException) || (e is ArgumentOutOfRangeException) || (e is System.IO.IOException) || (e is ObjectDisposedException))
                        {
                            InterprocessData = new byte[] { 0x0 };
                        }
                        else
                        {
                            // Unexpected exception
                            throw e;
                        }
                    }

                    OutgoingDataSet = true;

                    MCUCommsMutex.ReleaseMutex();
                }

                Thread.Sleep(5);
            }
        }

        protected override bool ProcessRequest(NetworkStream ActiveClientStream, byte[] query)
        {
            int ExpectedSize = query[0] + (256 * query[1]);
            if (query.Length != ExpectedSize)
            {
                throw new ArgumentException(
                    "IntoTheSpiderversePLCDriver read a package specifying a size [" + ExpectedSize.ToString() + "], but the actual size was different [" + query.Length + "]."
                );
            }

            byte CommandQueryTypeAndExpectedResponseStatus = query[2];
            byte CommandQueryTypeByte = (byte)(CommandQueryTypeAndExpectedResponseStatus & 0x3F);
            byte ExpectedResponseStatusByte = (byte)(CommandQueryTypeAndExpectedResponseStatus >> 6);

            PLCCommandAndQueryTypeEnum CommandQueryTypeEnum = PLCCommandAndQueryTypeConversionHelper.GetFromByte(CommandQueryTypeByte);
            PLCCommandResponseExpectationEnum ExpectedResponseStatusEnum = PLCCommandResponseExpectationConversionHelper.GetFromByte(ExpectedResponseStatusByte);

            byte[] FinalResponseContainer;

            if (ExpectedResponseStatusEnum == PLCCommandResponseExpectationEnum.FULL_RESPONSE)
            {
                FinalResponseContainer = new byte[]
                {
                    0x13, 0x0,
                    0x0,
                    0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0,
                    0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0
                };

                switch (CommandQueryTypeEnum)
                {
                    case PLCCommandAndQueryTypeEnum.TEST_CONNECTION:
                        {
                            FinalResponseContainer[3] = 0x1;
                            FinalResponseContainer[2] = 0x1;
                            break;
                        }

                    default:
                        {
                            throw new ArgumentException("Invalid PLCCommandAndQueryTypeEnum value seen while expecting a response: " + CommandQueryTypeEnum.ToString());
                        }
                }
            }
            else if (ExpectedResponseStatusEnum == PLCCommandResponseExpectationEnum.MINOR_RESPONSE)
            {
                FinalResponseContainer = new byte[]
                {
                    0x3, 0x0, 0x0
                };

                switch (CommandQueryTypeEnum)
                {
                    case PLCCommandAndQueryTypeEnum.SET_CONFIGURATION:
                        {
                            MCUCommsMutex.WaitOne();

                            InterprocessData = new byte[]
                            {
                                query[3], query[4], query[5], query[6], query[11], query[12]
                            };

                            MCUCommsMutex.ReleaseMutex();

                            InterprocessDataType = DataTypeSet.CONFIGURATION;
                            IncomingDataSet = true;

                            while (!OutgoingDataSet)
                            {
                                Thread.Sleep(5);
                            }

                            OutgoingDataSet = false;

                            MCUCommsMutex.WaitOne();
                            FinalResponseContainer[2] = InterprocessData[0];
                            InterprocessData = null;
                            MCUCommsMutex.ReleaseMutex();

                            break;
                        }
                    
                    case PLCCommandAndQueryTypeEnum.SET_OBJECTIVE_AZEL_POSITION:
                        {
                            byte[] translationBytes = BitConverter.GetBytes((int)(BitConverter.ToDouble(query, 3) * HardwareConstants.ACTUAL_MCU_STEPS_PER_DEGREE));
                            byte[] speedBytes = BitConverter.GetBytes(HardwareConstants.ACTUAL_MCU_DEFAULT_PEAK_VELOCITY);
                            byte[] accelerationBytes = BitConverter.GetBytes(HardwareConstants.ACTUAL_MCU_DEFAULT_ACCELERATION);

                            MCUCommsMutex.WaitOne();

                            InterprocessData = new byte[12];
                            Array.Copy(translationBytes, 0, InterprocessData, 0, 4);
                            Array.Copy(speedBytes, 0, InterprocessData, 4, 4);
                            Array.Copy(accelerationBytes, 0, InterprocessData, 8, 2);
                            Array.Copy(accelerationBytes, 0, InterprocessData, 10, 2);

                            MCUCommsMutex.ReleaseMutex();

                            InterprocessDataType = DataTypeSet.COMMAND;
                            IncomingDataSet = true;

                            while (!OutgoingDataSet)
                            {
                                Thread.Sleep(5);
                            }

                            OutgoingDataSet = false;

                            MCUCommsMutex.WaitOne();
                            FinalResponseContainer[2] = InterprocessData[0];
                            InterprocessData = null;
                            MCUCommsMutex.ReleaseMutex();

                            break;
                        }

                    default:
                        {
                            throw new ArgumentException("Invalid PLCCommandAndQueryTypeEnum value seen while NOT expecting a response: " + CommandQueryTypeEnum.ToString());
                        }
                }
            }
            else
            {
                throw new ArgumentException("Invalid PLCCommandResponseExpectationEnum value seen while processing client request in IntoTheSpiderversePLCDriver: " + ExpectedResponseStatusEnum.ToString());
            }

            return AttemptToWriteDataToServer(ActiveClientStream, FinalResponseContainer);
        }
    }
}
