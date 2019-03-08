using System;
using System.Net;
using System.Net.Sockets;
using ControlRoomApplication.Entities;

namespace ControlRoomApplication.Controllers.PLCCommunication
{
    public class TestPLCDriver : AbstractPLCDriver
    {
        public TestPLCDriver(IPAddress ip_address, int port) : base(ip_address, port) { }

        public TestPLCDriver(string ip, int port) : this(IPAddress.Parse(ip), port) { }

        protected override bool ProcessRequest(NetworkStream ActiveClientStream, byte[] query)
        {
            int ExpectedSize = query[0] + (16 * query[1]);
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

            if (ExpectedResponseStatusEnum == PLCCommandResponseExpectationEnum.EXPECTING_RESPONSE)
            {
                byte[] FinalResponseContainer =
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
                            PLCLimitSwitchStatusEnum StatusElevation = PLCLimitSwitchStatusEnum.WITHIN_SAFE_LIMITS;
                            PLCLimitSwitchStatusEnum StatusAzimuth = PLCLimitSwitchStatusEnum.WITHIN_SAFE_LIMITS;

                            FinalResponseContainer[3] =
                                (byte)((PLCLimitSwitchStatusConversionHelper.ConvertToByte(StatusAzimuth) * 0x10)
                                + PLCLimitSwitchStatusConversionHelper.ConvertToByte(StatusElevation))
                            ;

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

                return AttemptToWriteDataToServer(ActiveClientStream, FinalResponseContainer);
            }
            else
            {
                switch (CommandQueryTypeEnum)
                {
                    case PLCCommandAndQueryTypeEnum.CANCEL_ACTIVE_OBJECTIVE_AZEL_POSITION:
                    case PLCCommandAndQueryTypeEnum.SHUTDOWN:
                    case PLCCommandAndQueryTypeEnum.CALIBRATE:
                        {
                            break;
                        }

                    default:
                        {
                            throw new ArgumentException("Invalid PLCCommandAndQueryTypeEnum value seen while NOT expecting a response: " + CommandQueryTypeEnum.ToString());
                        }
                }

                return true;
            }
        }
    }
}