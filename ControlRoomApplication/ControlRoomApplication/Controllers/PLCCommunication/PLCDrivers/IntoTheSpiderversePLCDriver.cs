using System;
using System.Net.Sockets;
using ControlRoomApplication.Constants;
using ControlRoomApplication.Entities;
using Modbus.Device;

namespace ControlRoomApplication.Controllers
{
    public class IntoTheSpiderversePLCDriver : AbstractPLCDriver
    {
        private static readonly log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private TcpClient MCUTCPClient;
        private ModbusIpMaster MCUModbusMaster;

        public IntoTheSpiderversePLCDriver(string ipLocal, int portLocal) : base(ipLocal, portLocal)
        {
            MCUTCPClient = null;
            MCUModbusMaster = null;
        }

        ~IntoTheSpiderversePLCDriver()
        {
            if (MCUTCPClient != null)
            {
                MCUTCPClient.Close();
            }
        }

        public void FetchMCUModbusSlave(string ipMCU, int portMCU)
        {
            MCUTCPClient = new TcpClient(ipMCU, portMCU);
            MCUModbusMaster = ModbusIpMaster.CreateIp(MCUTCPClient);
        }

        public void PrintReadInputRegsiterContents(string header)
        {
            ushort[] inputRegisters = MCUModbusMaster.ReadInputRegisters(MCUConstants.ACTUAL_MCU_READ_INPUT_REGISTER_START_ADDRESS, 10);
            Console.WriteLine(header + ":");
            foreach (ushort us in inputRegisters)
            {
                string usString = Convert.ToString(us, 2);
                usString = new string('0', 16 - usString.Length) + usString;
                usString = usString.Insert(4, " ");
                usString = usString.Insert(9, " ");
                usString = usString.Insert(14, " ");

                Console.WriteLine('\t'.ToString() + usString);
            }
        }

        public bool SendResetErrorsCommand()
        {
            PrintReadInputRegsiterContents("Before reset");

            MCUModbusMaster.WriteMultipleRegisters(MCUConstants.ACTUAL_MCU_WRITE_REGISTER_START_ADDRESS, new ushort[] { 0x0800, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0 });

            PrintReadInputRegsiterContents("After reset");

            return true;
        }

        public bool SendHoldMoveCommand()
        {
            MCUModbusMaster.WriteMultipleRegisters(MCUConstants.ACTUAL_MCU_WRITE_REGISTER_START_ADDRESS, new ushort[] { 0x4, 0x3, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0 });
            return true;
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
                            ushort[] inputRegisters = MCUModbusMaster.ReadInputRegisters(MCUConstants.ACTUAL_MCU_READ_INPUT_REGISTER_HEARTBEAT_ADDRESS, 1);
                            ushort resultValue = (ushort)((inputRegisters.Length == 1) ? inputRegisters[0] : 0);
                            FinalResponseContainer[3] = (byte)(((resultValue == 8192) || (resultValue == 24576)) ? 0x1 : 0x0);

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
                            // Copy over data we care about, which for now is only the azimuth
                            // We skip over the data concerning the elevation, hence the gap in element access for query
                            ushort[] DataToWrite =
                            {
                                (ushort)((256 * query[3]) + query[4]),
                                (ushort)((256 * query[5]) + query[6]),
                                (ushort)((256 * query[7]) + query[8]),
                                (ushort)((256 * query[9]) + query[10]),
                                (ushort)((256 * query[15]) + query[16]),
                                0,
                                0,
                                0,
                                0,
                                0
                            };

                            MCUModbusMaster.WriteMultipleRegisters(MCUConstants.ACTUAL_MCU_WRITE_REGISTER_START_ADDRESS, DataToWrite);

                            System.Threading.Thread.Sleep(500);
                            PrintReadInputRegsiterContents("After setting configuration");

                            System.Threading.Thread.Sleep(500);
                            if (!SendResetErrorsCommand())
                            {
                                Console.WriteLine("[IntoTheSpiderversePLCDriver] ERROR sending reset command.");
                            }

                            FinalResponseContainer[2] = 0x1;
                            break;
                        }
                    
                    case PLCCommandAndQueryTypeEnum.SET_OBJECTIVE_AZEL_POSITION:
                        {
                            System.Threading.Thread.Sleep(500);
                            PrintReadInputRegsiterContents("Before setting objective position");

                            // Copy over data we care about, so skip over the data concerning the elevation
                            //double discrepancyMultiplier = 1.0;
                            //double objectiveAZDouble = BitConverter.ToDouble(query, 3);
                            //int stepChange = 5000; // (int)(discrepancyMultiplier * Math.Pow(2, MCUConstants.ACTUAL_MCU_AZIMUTH_ENCODER_BIT_RESOLUTION) * objectiveAZDouble / 360);
                            //ushort stepChangeUShortMSW = (ushort)((stepChange >> 16) & 0xFFFF);
                            //ushort stepChangeUShortLSW = (ushort)(stepChange & 0xFFFF);

                            int programmedPeakSpeed = MCUConstants.ACTUAL_MCU_MOVE_PEAK_VELOCITY_SPIDERVERSE;
                            ushort programmedPeakSpeedUShortMSW = (ushort)((programmedPeakSpeed >> 16) & 0xFFFF);
                            ushort programmedPeakSpeedUShortLSW = (ushort)(programmedPeakSpeed & 0xFFFF);

                            ushort[] DataToWrite =
                            {
                                0x80, // Denotes a jog move in command mode
                                0x3, // Denotes a Trapezoidal S-Curve profile
                                0,
                                0,
                                programmedPeakSpeedUShortMSW,
                                programmedPeakSpeedUShortLSW,
                                MCUConstants.ACTUAL_MCU_MOVE_ACCELERATION_SPIDERVERSE,
                                MCUConstants.ACTUAL_MCU_MOVE_ACCELERATION_SPIDERVERSE,
                                0,
                                0
                            };

                            MCUModbusMaster.WriteMultipleRegisters(MCUConstants.ACTUAL_MCU_WRITE_REGISTER_START_ADDRESS, DataToWrite);

                            PrintReadInputRegsiterContents("After setting objective position");

                            FinalResponseContainer[2] = 0x1;
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