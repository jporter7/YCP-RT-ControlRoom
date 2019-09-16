#define PRODUCTION_PLC_DRIVER_DEBUG

using System;
using System.Net.Sockets;
using System.Threading.Tasks;
using ControlRoomApplication.Constants;
using ControlRoomApplication.Entities;
using Modbus.Device;

namespace ControlRoomApplication.Controllers
{
    /// <summary>
    /// this is the driver for the MCU
    /// </summary>
    public class ProductionMCUDriver : AbstractPLCDriver
    {
        private static readonly log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private TcpClient MCUTCPClient;
        public ModbusIpMaster MCUModbusMaster;
        public bool KillClientManagementThreadFlag;
        public TcpListener PLCTCPListener;

        /// <summary>
        /// !!!! Depricated !!!! use PLC driver 
        /// </summary>
        /// <param name="local_ip"></param>
        /// <param name="MCU_ip"></param>
        /// <param name="MCU_port"></param>
        /// <param name="PLC_port"></param>
        public ProductionMCUDriver(string local_ip, string MCU_ip, int MCU_port, int PLC_port,bool startPLC ) : base(local_ip, MCU_ip, MCU_port, PLC_port, startPLC )
        {
            MCUTCPClient = new TcpClient("192.168.0.50", MCUConstants.ACTUAL_MCU_MODBUS_TCP_PORT);
            MCUModbusMaster = ModbusIpMaster.CreateIp(MCUTCPClient);
        }
        /// <summary>
        /// 
        /// </summary>
        ~ProductionMCUDriver()
        {
            if (MCUTCPClient != null)
            {
                MCUTCPClient.Close();
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="header"></param>
        public void PrintReadInputRegsiterContents(string header)
        {
#if PRODUCTION_PLC_DRIVER_DEBUG
            System.Threading.Thread.Sleep(50);

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
#endif
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public bool SendResetErrorsCommand()
        {
            PrintReadInputRegsiterContents("Before reset");

            MCUModbusMaster.WriteMultipleRegisters(MCUConstants.ACTUAL_MCU_WRITE_REGISTER_START_ADDRESS, new ushort[] { 0x0800, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0 });

            PrintReadInputRegsiterContents("After reset");

            return true;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public bool SendHoldMoveCommand()
        {
            PrintReadInputRegsiterContents("Before controlled stop");

            MCUModbusMaster.WriteMultipleRegisters(MCUConstants.ACTUAL_MCU_WRITE_REGISTER_START_ADDRESS, new ushort[] { 0x4, 0x3, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0 });

            PrintReadInputRegsiterContents("After controlled stop");

            return true;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public bool SendImmediateStopCommand()
        {
            PrintReadInputRegsiterContents("Before controlled stop");

            MCUModbusMaster.WriteMultipleRegisters(MCUConstants.ACTUAL_MCU_WRITE_REGISTER_START_ADDRESS, new ushort[] { 0x10, 0x3, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0 });

            PrintReadInputRegsiterContents("After controlled stop");

            return true;
        }
        /// <summary>
        /// this stops the mcudirectly in a controld maner
        /// </summary>
        /// <returns></returns>
        public bool SendEmptyMoveCommand()
        {
            PrintReadInputRegsiterContents("Before removing move bits");

            MCUModbusMaster.WriteMultipleRegisters(MCUConstants.ACTUAL_MCU_WRITE_REGISTER_START_ADDRESS, new ushort[] { 0x0, 0x3, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0 });

            PrintReadInputRegsiterContents("After removing move bits");

            return true;
        }
        /// <summary>
        /// configures the axies 
        /// </summary>
        /// <returns></returns>
       public bool configure_axies(int gearedSpeedAZ, int gearedSpeedEL, ushort homeTimeoutSecondsAzimuth, ushort homeTimeoutSecondsElevation)
        {
            ushort[] data = {   0x8400, 0x0000, (ushort)(gearedSpeedEL >> 0x0010), (ushort)(gearedSpeedEL & 0xFFFF), homeTimeoutSecondsElevation,
                                0x0,    0x0,    0x0,                                 0x0,                            0x0,
                                0x8400, 0x0000, (ushort)(gearedSpeedAZ >> 0x0010), (ushort)(gearedSpeedAZ & 0xFFFF), homeTimeoutSecondsAzimuth,
                                0x0,    0x0,    0x0,                                0x0,                             0x0
                                };
            Console.WriteLine(data.Length);
            MCUModbusMaster.WriteMultipleRegisters(1024, data);
            return true;
        }

        /// <summary>
        /// this returns the orientation relative to position when the MCU configured
        /// </summary>
        /// <returns></returns>
        public Orientation read_Position_offset()
        {
            ushort[] inputs= MCUModbusMaster.ReadHoldingRegisters(0,20);
            byte[] inbytes = new byte[inputs.Length * 2];
            Buffer.BlockCopy(inputs, 0, inbytes, 0, inputs.Length * 2);

            ushort msb, lsvb;
            msb = inputs[2];
            lsvb = inputs[3];
            int ofset = lsvb + (msb << 16);
            double azpos= (ofset/(20000*500))*360;

            msb = inputs[12];
            lsvb = inputs[13];
            ofset = lsvb + (msb << 16);
            double elpos = (ofset / (20000 * 50))*90;

            return new Orientation(azpos, elpos);
            //return new Orientation(BitConverter.ToDouble(inbytes, 4), BitConverter.ToDouble(inbytes, 24));
        }


        /// <summary>
        /// handle conection data comming from modbus device
        /// this serves as part of a tcp server along with HandleClientManagementThread
        /// it apears to have been designed for ethernet/ip which we found out our plc cant do 
        /// this is probably unnecicary in the mcu drivers.
        /// </summary>
        /// <param name="ActiveClientStream"></param>
        /// <param name="query"></param>
        /// <returns></returns>
        public bool ProcessRequest(NetworkStream ActiveClientStream, byte[] query)
        {
            int ExpectedSize = query[0] + (256 * query[1]);
            if (query.Length != ExpectedSize)
            {
                throw new ArgumentException(
                    "ProductionPLCDriver read a package specifying a size [" + ExpectedSize.ToString() + "], but the actual size was different [" + query.Length + "]."
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
                            // Read the heartbeat register
                            ushort[] inputRegisters = MCUModbusMaster.ReadInputRegisters(MCUConstants.ACTUAL_MCU_READ_INPUT_REGISTER_HEARTBEAT_ADDRESS, 1);
                            ushort resultValue = (ushort)((inputRegisters.Length == 1) ? inputRegisters[0] : 0);
                            FinalResponseContainer[3] = (byte)(((resultValue == 8192) || (resultValue == 24576)) ? 0x1 : 0x0);

                            FinalResponseContainer[2] = 0x1;
                            break;
                        }

                    case PLCCommandAndQueryTypeEnum.GET_CURRENT_AZEL_POSITIONS:
                        {
                            PrintReadInputRegsiterContents("Before getting current position");

                            // Get the MCU's value for the displacement since its power cycle
                            ushort[] inputRegisters = MCUModbusMaster.ReadInputRegisters(MCUConstants.ACTUAL_MCU_READ_INPUT_REGISTER_CURRENT_POSITION_ADDRESS, 2);
                            int currentStepForMCU = (65536 * inputRegisters[0]) + inputRegisters[1];

                            PrintReadInputRegsiterContents("After getting current position");

                            // Convert that step change into degrees and write the bytes to return
                            Array.Copy(BitConverter.GetBytes(currentStepForMCU * 360 / 10000000.0), 0, FinalResponseContainer, 3, 8);
                            Array.Copy(BitConverter.GetBytes(0.0), 0, FinalResponseContainer, 11, 8);

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
                            
                            PrintReadInputRegsiterContents("After setting configuration");
                            if (SendResetErrorsCommand())
                            {
                                Console.WriteLine("[ProductionPLCDriver] Successfully sent reset command.");
                                PrintReadInputRegsiterContents("After sending reset command");
                                FinalResponseContainer[2] = 0x1;
                            }
                            else
                            {
                                // Send an error code
                                Console.WriteLine("[ProductionPLCDriver] ERROR sending reset command.");
                                FinalResponseContainer[2] = 0x2;
                            }
                            
                            break;
                        }

                    case PLCCommandAndQueryTypeEnum.CONTROLLED_STOP:
                        {
                            // There was already a helper function to execute a controlled stop, so just call that
                            // Send an error code if there's a failure for some reason
                            FinalResponseContainer[2] = (byte)(SendEmptyMoveCommand() ? 0x1 : 0x2);
                            break;
                        }

                    case PLCCommandAndQueryTypeEnum.IMMEDIATE_STOP:
                        {
                            // There was already a helper function to execute a controlled stop, so just call that
                            // Send an error code if there's a failure for some reason
                            FinalResponseContainer[2] = (byte)(SendImmediateStopCommand() ? 0x1 : 0x2);
                            break;
                        }

                    case PLCCommandAndQueryTypeEnum.SET_OBJECTIVE_AZEL_POSITION:
                        {
                            PrintReadInputRegsiterContents("Before setting objective position");

                            // Copy over data we care about, so skip over the data concerning the elevation
                            double discrepancyMultiplier = 1.0;
                            double objectiveAZDouble = BitConverter.ToDouble(query, 3);
                            int stepChange = (int)(discrepancyMultiplier * Math.Pow(2, MCUConstants.ACTUAL_MCU_AZIMUTH_ENCODER_BIT_RESOLUTION) * objectiveAZDouble / 360);
                            ushort stepChangeUShortMSW = (ushort)((stepChange >> 16) & 0xFFFF);
                            ushort stepChangeUShortLSW = (ushort)(stepChange & 0xFFFF);

                            int programmedPeakSpeed = BitConverter.ToInt32(query, 7);
                            ushort programmedPeakSpeedUShortMSW = (ushort)((programmedPeakSpeed >> 16) & 0xFFFF);
                            ushort programmedPeakSpeedUShortLSW = (ushort)(programmedPeakSpeed & 0xFFFF);

                            ushort[] DataToWrite =
                            {
                                0x2, // Denotes a relative move in command mode
                                0x3, // Denotes a Trapezoidal S-Curve profile
                                stepChangeUShortMSW,
                                stepChangeUShortLSW,
                                programmedPeakSpeedUShortMSW,
                                programmedPeakSpeedUShortLSW,
                                MCUConstants.ACTUAL_MCU_MOVE_ACCELERATION_WITH_GEARING,
                                MCUConstants.ACTUAL_MCU_MOVE_ACCELERATION_WITH_GEARING,
                                0,
                                0
                            };

                            MCUModbusMaster.WriteMultipleRegisters(MCUConstants.ACTUAL_MCU_WRITE_REGISTER_START_ADDRESS, DataToWrite);

                            PrintReadInputRegsiterContents("After setting objective position");

                            FinalResponseContainer[2] = 0x1;
                            break;
                        }

                    case PLCCommandAndQueryTypeEnum.START_JOG_MOVEMENT:
                        {
                            PrintReadInputRegsiterContents("Before starting jog command");

                            // Make sure the command is intended for the azimuth
                            if (query[3] != 0x1)
                            {
                                throw new ArgumentException("Unsupported value for axis specified in jog command for ProductionPLCDriver: " + query[3].ToString());
                            }

                            ushort programmedPeakSpeedUShortMSW = (ushort)((256 * query[4]) + query[5]);
                            ushort programmedPeakSpeedUShortLSW = (ushort)((256 * query[6]) + query[7]);

                            ushort commandCode;
                            switch (query[8])
                            {
                                case 0x1:
                                    {
                                        commandCode = 0x80;
                                        break;
                                    }
                                
                                case 0x2:
                                    {
                                        commandCode = 0x100;
                                        break;
                                    }

                                default:
                                    {
                                        throw new ArgumentException("Unsupported value for motor movement direction in jog command for ProductionPLCDriver: " + query[8].ToString());
                                    }
                            }

                            ushort[] DataToWrite =
                            {
                                commandCode,  // Denotes a jog move, either CW or CCW, in command mode
                                0x3,          // Denotes a Trapezoidal S-Curve profile
                                0,            // Reserved to 0 for a jog command
                                0,            // Reserved to 0 for a jog command
                                programmedPeakSpeedUShortMSW,
                                programmedPeakSpeedUShortLSW,
                                MCUConstants.ACTUAL_MCU_MOVE_ACCELERATION_WITH_GEARING,
                                MCUConstants.ACTUAL_MCU_MOVE_ACCELERATION_WITH_GEARING,
                                0,
                                0
                            };

                            MCUModbusMaster.WriteMultipleRegisters(MCUConstants.ACTUAL_MCU_WRITE_REGISTER_START_ADDRESS, DataToWrite);
                            
                            PrintReadInputRegsiterContents("After starting jog command");

                            FinalResponseContainer[2] = 0x1;
                            break;
                        }

                    case PLCCommandAndQueryTypeEnum.TRANSLATE_AZEL_POSITION:
                        {
                            PrintReadInputRegsiterContents("Before starting relative move");

                            // Make sure the command is intended for the azimuth
                            if (query[3] != 0x1)
                            {
                                throw new ArgumentException("Unsupported value for axis specified in move relative command for ProductionPLCDriver: " + query[3].ToString());
                            }

                            ushort programmedPeakSpeedUShortMSW = (ushort)((256 * query[4]) + query[5]);
                            ushort programmedPeakSpeedUShortLSW = (ushort)((256 * query[6]) + query[7]);

                            short programmedPositionUShortMSW = (short)((256 * query[8]) + query[9]);
                            short programmedPositionUShortLSW = (short)((256 * query[10]) + query[11]);

                            ushort[] DataToWrite =
                            {
                                0x2,                                    // Denotes a relative move
                                0x3,                                    // Denotes a Trapezoidal S-Curve profile
                                (ushort)programmedPositionUShortMSW,    // MSW for position
                                (ushort)programmedPositionUShortLSW,    // LSW for position
                                programmedPeakSpeedUShortMSW,
                                programmedPeakSpeedUShortLSW,
                                MCUConstants.ACTUAL_MCU_MOVE_ACCELERATION_WITH_GEARING,
                                MCUConstants.ACTUAL_MCU_MOVE_ACCELERATION_WITH_GEARING,
                                0,
                                0
                            };

                            MCUModbusMaster.WriteMultipleRegisters(MCUConstants.ACTUAL_MCU_WRITE_REGISTER_START_ADDRESS, DataToWrite);

                            PrintReadInputRegsiterContents("After starting relative move command");

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
                throw new ArgumentException("Invalid PLCCommandResponseExpectationEnum value seen while processing client request in ProductionPLCDriver: " + ExpectedResponseStatusEnum.ToString());
            }

            return AttemptToWriteDataToServer(ActiveClientStream, FinalResponseContainer);
        }

        private bool AttemptToWriteDataToServer(NetworkStream activeClientStream, byte[] finalResponseContainer)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 
        /// </summary>
        protected override void HandleClientManagementThread()
        {
            //return;
            TcpClient AcceptedClient = null;
            byte[] StreamBuffer = new byte[256];
            byte[] ClippedData;

            while (!KillClientManagementThreadFlag)
            {
                if (PLCTCPListener.Pending())
                {
                    AcceptedClient = PLCTCPListener.AcceptTcpClient();
                    logger.Info("[AbstractPLCDriver] Connected to new client.");

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
                                logger.Info("[AbstractPLCDriver] FAILED to write server.");
                            }
                            else
                            {
                                //logger.Info("[AbstractPLCDriver] Successfully wrote to server: [{0}]", string.Join(", ", ClippedData));
                                //logger.Info("[AbstractPLCDriver] Successfully wrote to server!");
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
                                logger.Info("[AbstractPLCDriver] ERROR: copying buffer array into clipped array {" + Fd + "}, skipping... [" + e.ToString());
                                continue;
                            }
                            else
                            {// Unexpected exception
                                throw e;
                            }
                        }
                    }

                    ClientStream.Dispose();
                    AcceptedClient.Dispose();
                }
            }
        }

        public override bool StartAsyncAcceptingClients()
        {
            throw new NotImplementedException();
        }

        public override bool RequestStopAsyncAcceptingClientsAndJoin()
        {
            throw new NotImplementedException();
        }

        public override void Bring_down()
        {
            throw new NotImplementedException();
        }

        public override bool Test_Conection()
        {
            throw new NotImplementedException();
        }

        public override Orientation read_Position()
        {
            throw new NotImplementedException();
        }

        public override bool Cancle_move()
        {
            throw new NotImplementedException();
        }

        public override bool Shutdown_PLC_MCU()
        {
            throw new NotImplementedException();
        }

        public override bool Calibrate()
        {
            throw new NotImplementedException();
        }

        public override bool Configure_MCU(double startSpeedAzimuth, double startSpeedElevation , int homeTimeoutAzimuth, int homeTimeoutElevation)
        {
            throw new NotImplementedException();
        }

        public override bool Controled_stop(RadioTelescopeAxisEnum axis, bool both)
        {
            throw new NotImplementedException();
        }

        public override bool Immediade_stop()
        {
            throw new NotImplementedException();
        }

        public override bool relative_move(int programmedPeakSpeedAZInt, ushort ACCELERATION, int positionTranslationAZ, int positionTranslationEL)
        {
            throw new NotImplementedException();
        }

        public override bool Move_to_orientation(Orientation target_orientation, Orientation current_orientation)
        {
            throw new NotImplementedException();
        }

        public override bool Start_jog(RadioTelescopeAxisEnum axis, int speed, bool clockwise)
        {
            throw new NotImplementedException();
        }

        public override bool Get_interlock_status()
        {
            throw new NotImplementedException();
        }

        public override bool[] Get_Limit_switches()
        {
            throw new NotImplementedException();
        }

        public override Task<bool[]> GET_MCU_Status( RadioTelescopeAxisEnum axis )
        {
            throw new NotImplementedException();
        }

        protected override bool TestIfComponentIsAlive() {
            throw new NotImplementedException();
        }

    }
}