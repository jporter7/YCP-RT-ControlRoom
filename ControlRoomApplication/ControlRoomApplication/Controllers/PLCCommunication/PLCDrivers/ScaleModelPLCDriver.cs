﻿using System;
using System.Net;
using System.Net.Sockets;
using ControlRoomApplication.Entities;

namespace ControlRoomApplication.Controllers
{
    public class ScaleModelPLCDriver : AbstractPLCDriver
    {
        private static readonly log4net.ILog logger =  log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public ScaleModelPLCDriver(IPAddress local_ip_address, IPAddress MCU_ip_address, int MCU_port, int PLC_port) : base(local_ip_address, MCU_ip_address,  MCU_port,  PLC_port) { }

        public ScaleModelPLCDriver(string local_ip, string MCU_ip, int MCU_port, int PLC_port) : this(IPAddress.Parse(local_ip), IPAddress.Parse(MCU_ip), MCU_port, PLC_port) { }

        public bool KillClientManagementThreadFlag;
        public TcpListener PLCTCPListener;

        protected override void HandleClientManagementThread()
        {
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

        public bool ProcessRequest(NetworkStream ActiveClientStream, byte[] query)
        {
            int ExpectedSize = query[0] + (256 * query[1]);
            if (query.Length != ExpectedSize)
            {
                throw new ArgumentException(
                    "ScaleModelPLCDriverController read a package specifying a size [" + ExpectedSize.ToString() + "], but the actual size was different [" + query.Length + "]."
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

                    case PLCCommandAndQueryTypeEnum.GET_CURRENT_AZEL_POSITIONS:
                        {
                            double TestAzimuth = 180.0;
                            double TestElevation = 42.0;
                            
                            Array.Copy(BitConverter.GetBytes(TestAzimuth), 0, FinalResponseContainer, 3, 8);
                            Array.Copy(BitConverter.GetBytes(TestElevation), 0, FinalResponseContainer, 11, 8);

                            FinalResponseContainer[2] = 0x1;

                            break;
                        }

                    case PLCCommandAndQueryTypeEnum.GET_CURRENT_LIMIT_SWITCH_STATUSES:
                        {
                            PLCLimitSwitchStatusEnum StatusAzimuthUnderRotation = PLCLimitSwitchStatusEnum.WITHIN_SAFE_LIMITS;
                            PLCLimitSwitchStatusEnum StatusAzimuthOverRotation = PLCLimitSwitchStatusEnum.WITHIN_SAFE_LIMITS;
                            PLCLimitSwitchStatusEnum StatusElevationUnderRotation = PLCLimitSwitchStatusEnum.WITHIN_SAFE_LIMITS;
                            PLCLimitSwitchStatusEnum StatusElevationOverRotation = PLCLimitSwitchStatusEnum.WITHIN_SAFE_LIMITS;

                            int PacketSum =
                                PLCLimitSwitchStatusConversionHelper.ConvertToByte(StatusElevationOverRotation)
                                 + (PLCLimitSwitchStatusConversionHelper.ConvertToByte(StatusElevationUnderRotation) * 0x4)
                                 + (PLCLimitSwitchStatusConversionHelper.ConvertToByte(StatusAzimuthOverRotation) * 0x10)
                                 + (PLCLimitSwitchStatusConversionHelper.ConvertToByte(StatusAzimuthUnderRotation) * 0x40)
                            ;

                            FinalResponseContainer[3] = (byte)PacketSum;
                            FinalResponseContainer[2] = 0x1;

                            break;
                        }

                    case PLCCommandAndQueryTypeEnum.GET_CURRENT_SAFETY_INTERLOCK_STATUS:
                        {
                            FinalResponseContainer[3] = PLCSafetyInterlockStatusConversionHelper.ConvertToByte(PLCSafetyInterlockStatusEnum.LOCKED);
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
                    case PLCCommandAndQueryTypeEnum.CANCEL_ACTIVE_OBJECTIVE_AZEL_POSITION:
                    case PLCCommandAndQueryTypeEnum.SHUTDOWN:
                    case PLCCommandAndQueryTypeEnum.CALIBRATE:
                        {
                            FinalResponseContainer[2] = 0x1;
                            break;
                        }

                    case PLCCommandAndQueryTypeEnum.SET_OBJECTIVE_AZEL_POSITION:
                        {
                            double NextAZ, NextEL;

                            try
                            {
                                NextAZ = BitConverter.ToDouble(query, 3);
                                NextEL = BitConverter.ToDouble(query, 11);
                            }
                            catch (Exception e)
                            {
                                if ((e is ArgumentException) || (e is ArgumentNullException) || (e is ArgumentOutOfRangeException))
                                {
                                    // This error code means that the data could not be converted into a double-precision floating point
                                    FinalResponseContainer[2] = 0x2;
                                    break;
                                }
                                else
                                {
                                    // Unexpected exception
                                    throw e;
                                }
                            }

                            if ((NextAZ < 0) || (NextAZ > 360))
                            {
                                // This error code means that the objective azimuth position is invalid
                                FinalResponseContainer[2] = 0x3;
                                break;
                            }

                            if ((NextEL < 0) || (NextEL > 90))
                            {
                                // This error code means that the objective elevation position is invalid
                                FinalResponseContainer[2] = 0x4;
                                break;
                            }

                            // Otherwise, this is valid
                            // TODO: Perform task(s) to set objective orientation!

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
                throw new ArgumentException("Invalid PLCCommandResponseExpectationEnum value seen while processing client request in ScaleModelPLCDriver: " + ExpectedResponseStatusEnum.ToString());
            }

            return AttemptToWriteDataToServer(ActiveClientStream, FinalResponseContainer);
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
                    logger.Info("[AbstractPLCDriver] ERROR: writing back to client with the PLC's response {" + ResponseData.ToString() + "}");
                    return false;
                }
            }

            return true;
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

        public override bool Configure_MCU(int startSpeedAzimuth, int startSpeedElevation, int homeTimeoutAzimuth, int homeTimeoutElevation)
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

        public override bool[] GET_MCU_Status()
        {
            throw new NotImplementedException();
        }

        protected override bool TestIfComponentIsAlive() {
            throw new NotImplementedException();
        }

    }
}
