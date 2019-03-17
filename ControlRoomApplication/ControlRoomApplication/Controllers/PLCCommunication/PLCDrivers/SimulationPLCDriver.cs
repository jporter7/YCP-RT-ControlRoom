using System;
using System.Net.Sockets;
using ControlRoomApplication.Constants;
using ControlRoomApplication.Entities;
using ControlRoomApplication.Simulators.Hardware.MCU;

namespace ControlRoomApplication.Controllers.PLCCommunication
{
    public class SimulationPLCDriver : AbstractPLCDriver
    {
        private SimulationMCU SimMCU;

        public SimulationPLCDriver(string ip, int port) : base(ip, port)
        {
            // Create the Simulation Motor Controller Unit to have absolute encoders with:
            //   1.) 12 bits of precision on the azimuth
            //   2.) 10 bits of precision on the elevation
            SimMCU = new SimulationMCU(12, 10);
        }

        protected override bool ProcessRequest(NetworkStream ActiveClientStream, byte[] query)
        {
            int ExpectedSize = query[0] + (16 * query[1]);
            if (query.Length != ExpectedSize)
            {
                throw new ArgumentException(
                    "SimulationPLCDriverController read a package specifying a size [" + ExpectedSize.ToString() + "], but the actual size was different [" + query.Length + "]."
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
                            Orientation CurrentOrientation = SimMCU.GetCurrentOrientationInDegrees();

                            Array.Copy(BitConverter.GetBytes(CurrentOrientation.Azimuth), 0, FinalResponseContainer, 3, 8);
                            Array.Copy(BitConverter.GetBytes(CurrentOrientation.Elevation), 0, FinalResponseContainer, 11, 8);

                            FinalResponseContainer[2] = 0x1;

                            break;
                        }

                    case PLCCommandAndQueryTypeEnum.GET_CURRENT_LIMIT_SWITCH_STATUSES:
                        {
                            Orientation CurrentOrientation = SimMCU.GetCurrentOrientationInDegrees();

                            double CurrentAZ = CurrentOrientation.Azimuth;
                            double CurrentEL = CurrentOrientation.Elevation;

                            double ThresholdAZ = HardwareConstants.LIMIT_SWITCH_AZ_THRESHOLD_DEGREES;
                            double ThresholdEL = HardwareConstants.LIMIT_SWITCH_EL_THRESHOLD_DEGREES;

                            // Subtracting out those 2 degrees is because of our actual rotational limits of (-2 : 362) and (-2 : 92) degrees in azimuth and elevation respectively
                            PLCLimitSwitchStatusEnum StatusAzimuthUnderRotation = (CurrentAZ < (ThresholdAZ - 2.0)) ? PLCLimitSwitchStatusEnum.WITHIN_WARNING_LIMITS : PLCLimitSwitchStatusEnum.WITHIN_SAFE_LIMITS;
                            PLCLimitSwitchStatusEnum StatusAzimuthOverRotation = (CurrentAZ > (360 + ThresholdAZ - 2.0)) ? PLCLimitSwitchStatusEnum.WITHIN_WARNING_LIMITS : PLCLimitSwitchStatusEnum.WITHIN_SAFE_LIMITS;
                            PLCLimitSwitchStatusEnum StatusElevationUnderRotation = (CurrentEL < (ThresholdEL - 2.0)) ? PLCLimitSwitchStatusEnum.WITHIN_WARNING_LIMITS : PLCLimitSwitchStatusEnum.WITHIN_SAFE_LIMITS;
                            PLCLimitSwitchStatusEnum StatusElevationOverRotation = (CurrentEL > (90 + ThresholdEL - 2.0)) ? PLCLimitSwitchStatusEnum.WITHIN_WARNING_LIMITS : PLCLimitSwitchStatusEnum.WITHIN_SAFE_LIMITS;

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
