﻿using System;
using System.Net.Sockets;
using ControlRoomApplication.Constants;
using ControlRoomApplication.Entities;
using ControlRoomApplication.Simulators.Hardware.MCU;

namespace ControlRoomApplication.Controllers
{
    public class SimulationPLCTCPIPReceiver : BaseTCPIPHardwareReceiver
    {
        private SimulationMCU SimMCU;

        public SimulationPLCTCPIPReceiver(string ip, int port) : base(ip, port)
        {
            // Create the Simulation Motor Controller Unit to have absolute encoders with:
            //   1.) 12 bits of precision on the azimuth
            //   2.) 10 bits of precision on the elevation
            SimMCU = new SimulationMCU(12, 10);
        }

        protected override bool ProcessRequest(NetworkStream ActiveClientStream, byte[] query)
        {
            int ExpectedSize = query[0] + (256 * query[1]);
            if (query.Length != ExpectedSize)
            {
                throw new ArgumentException(
                    "SimulationPLCTCPIPReceiver read a package specifying a size [" + ExpectedSize.ToString() + "], but the actual size was different [" + query.Length + "]."
                );
            }

            byte CommandQueryTypeAndExpectedResponseStatus = query[2];
            byte CommandQueryTypeByte = (byte)(CommandQueryTypeAndExpectedResponseStatus & 0x3F);
            byte ExpectedResponseStatusByte = (byte)(CommandQueryTypeAndExpectedResponseStatus >> 6);

            HardwareMessageTypeEnum CommandQueryTypeEnum = HardwareMessageTypeEnumConversionHelper.GetFromByte(CommandQueryTypeByte);
            HardwareMessageResponseExpectationEnum ExpectedResponseStatusEnum = HardwareMessageResponseExpectationConversionHelper.GetFromByte(ExpectedResponseStatusByte);

            byte[] FinalResponseContainer;

            if (ExpectedResponseStatusEnum == HardwareMessageResponseExpectationEnum.FULL_RESPONSE)
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
                    case HardwareMessageTypeEnum.TEST_CONNECTION:
                        {
                            FinalResponseContainer[3] = 0x1;
                            FinalResponseContainer[2] = 0x1;
                            break;
                        }

                    case HardwareMessageTypeEnum.GET_CURRENT_AZEL_POSITIONS:
                        {
                            Orientation CurrentOrientation = SimMCU.GetCurrentOrientationInDegrees();

                            Array.Copy(BitConverter.GetBytes(CurrentOrientation.Azimuth), 0, FinalResponseContainer, 3, 8);
                            Array.Copy(BitConverter.GetBytes(CurrentOrientation.Elevation), 0, FinalResponseContainer, 11, 8);

                            FinalResponseContainer[2] = 0x1;

                            break;
                        }

                    case HardwareMessageTypeEnum.GET_CURRENT_LIMIT_SWITCH_STATUSES:
                        {
                            Orientation CurrentOrientation = SimMCU.GetCurrentOrientationInDegrees();

                            double CurrentAZ = CurrentOrientation.Azimuth;
                            double CurrentEL = CurrentOrientation.Elevation;

                            double ThresholdAZ = MiscellaneousHardwareConstants.LIMIT_SWITCH_AZ_THRESHOLD_DEGREES;
                            double ThresholdEL = MiscellaneousHardwareConstants.LIMIT_SWITCH_EL_THRESHOLD_DEGREES;

                            // Subtracting out those 2 degrees is because of our actual rotational limits of (-2 : 362) and (-2 : 92) degrees in azimuth and elevation respectively
                            LimitSwitchStatusEnum StatusAzimuthUnderRotation = (CurrentAZ < (ThresholdAZ - 2.0)) ? LimitSwitchStatusEnum.WITHIN_WARNING_LIMITS : LimitSwitchStatusEnum.WITHIN_SAFE_LIMITS;
                            LimitSwitchStatusEnum StatusAzimuthOverRotation = (CurrentAZ > (360 + ThresholdAZ - 2.0)) ? LimitSwitchStatusEnum.WITHIN_WARNING_LIMITS : LimitSwitchStatusEnum.WITHIN_SAFE_LIMITS;
                            LimitSwitchStatusEnum StatusElevationUnderRotation = (CurrentEL < (ThresholdEL - 2.0)) ? LimitSwitchStatusEnum.WITHIN_WARNING_LIMITS : LimitSwitchStatusEnum.WITHIN_SAFE_LIMITS;
                            LimitSwitchStatusEnum StatusElevationOverRotation = (CurrentEL > (90 + ThresholdEL - 2.0)) ? LimitSwitchStatusEnum.WITHIN_WARNING_LIMITS : LimitSwitchStatusEnum.WITHIN_SAFE_LIMITS;

                            int PacketSum =
                                LimitSwitchStatusConversionHelper.ConvertToByte(StatusElevationOverRotation)
                                 + (LimitSwitchStatusConversionHelper.ConvertToByte(StatusElevationUnderRotation) * 0x4)
                                 + (LimitSwitchStatusConversionHelper.ConvertToByte(StatusAzimuthOverRotation) * 0x10)
                                 + (LimitSwitchStatusConversionHelper.ConvertToByte(StatusAzimuthUnderRotation) * 0x40)
                            ;

                            FinalResponseContainer[3] = (byte)PacketSum;
                            FinalResponseContainer[2] = 0x1;

                            break;
                        }

                    case HardwareMessageTypeEnum.GET_CURRENT_SAFETY_INTERLOCK_STATUS:
                        {
                            FinalResponseContainer[3] = SafetyInterlockStatusConversionHelper.ConvertToByte(SafetyInterlockStatusEnum.LOCKED);
                            FinalResponseContainer[2] = 0x1;
                            break;
                        }

                    default:
                        {
                            throw new ArgumentException("Invalid HardwareMessageTypeEnum value seen while expecting a response: " + CommandQueryTypeEnum.ToString());
                        }
                }
            }
            else if (ExpectedResponseStatusEnum == HardwareMessageResponseExpectationEnum.MINOR_RESPONSE)
            {
                FinalResponseContainer = new byte[]
                {
                    0x3, 0x0, 0x0
                };

                switch (CommandQueryTypeEnum)
                {
                    case HardwareMessageTypeEnum.CANCEL_ACTIVE_OBJECTIVE_AZEL_POSITION:
                    case HardwareMessageTypeEnum.SHUTDOWN:
                    case HardwareMessageTypeEnum.CALIBRATE:
                        {
                            FinalResponseContainer[2] = 0x1;
                            break;
                        }

                    case HardwareMessageTypeEnum.SET_OBJECTIVE_AZEL_POSITION:
                        {
                            if (SimMCU.HasActiveMove())
                            {
                                // This error code means that there's already an active objective orientation
                                // A CANCEL_ACTIVE_OBJECTIVE_AZEL_POSITION command should have been issued first
                                FinalResponseContainer[2] = 0x2;
                                break;
                            }

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
                                    FinalResponseContainer[2] = 0x3;
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
                                FinalResponseContainer[2] = 0x4;
                                break;
                            }

                            if ((NextEL < 0) || (NextEL > 90))
                            {
                                // This error code means that the objective elevation position is invalid
                                FinalResponseContainer[2] = 0x5;
                                break;
                            }

                            // Otherwise, this is valid
                            // ForceLinearMove is set to true until the simulation MCU is fine-tuned
                            SimMCU.SetActiveObjectiveOrientationAndStartMove(new Orientation(NextAZ, NextEL), true);

                            FinalResponseContainer[2] = 0x1;
                            break;
                        }

                    default:
                        {
                            throw new ArgumentException("Invalid HardwareMessageTypeEnum value seen while NOT expecting a response: " + CommandQueryTypeEnum.ToString());
                        }
                }
            }
            else
            {
                throw new ArgumentException("Invalid HardwareMessageResponseExpectationEnum value seen while processing client request in SimulationPLCTCPIPReceiver: " + ExpectedResponseStatusEnum.ToString());
            }

            return RespondWith(ActiveClientStream, FinalResponseContainer);
        }
    }
}