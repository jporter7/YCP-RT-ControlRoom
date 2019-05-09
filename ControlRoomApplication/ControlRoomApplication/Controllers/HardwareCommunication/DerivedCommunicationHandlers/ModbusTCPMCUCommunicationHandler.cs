using System;
using System.Threading;
using ControlRoomApplication.Constants;
using ControlRoomApplication.Entities;

namespace ControlRoomApplication.Controllers
{
    public class ModbusTCPMCUCommunicationHandler : BaseModbusTCPCommunicationHandler
    {
        private static readonly ushort[] MESSAGE_CONTENTS_IMMEDIATE_STOP = new ushort[]
        {
            0x0010, 0x0003, 0x0000, 0x0000, 0x0000, 0x0000, 0x0000, 0x0000, 0x0000, 0x0000,
            0x0010, 0x0003, 0x0000, 0x0000, 0x0000, 0x0000, 0x0000, 0x0000, 0x0000, 0x0000
        };

        private static readonly ushort[] MESSAGE_CONTENTS_CLEAR_MOVE = new ushort[]
        {
            0x0000, 0x0003, 0x0000, 0x0000, 0x0000, 0x0000, 0x0000, 0x0000, 0x0000, 0x0000,
            0x0000, 0x0003, 0x0000, 0x0000, 0x0000, 0x0000, 0x0000, 0x0000, 0x0000, 0x0000
        };

        private static readonly ushort[] MESSAGE_CONTENTS_RESET_ERRORS = new ushort[]
        {
            0x0800, 0x0000, 0x0000, 0x0000, 0x0000, 0x0000, 0x0000, 0x0000, 0x0000, 0x0000,
            0x0800, 0x0000, 0x0000, 0x0000, 0x0000, 0x0000, 0x0000, 0x0000, 0x0000, 0x0000
        };

        private static readonly double DEFAULT_SPEED_IF_UNSPECIFIED_RPMS = 0.2;

        public int ConfiguredStartingSpeedAzimuth { get; private set; }
        public int ConfiguredStartingSpeedElevation { get; private set; }

        public ModbusTCPMCUCommunicationHandler(string ip, int port) : base(ip, port)
        {
            ConfiguredStartingSpeedAzimuth = 0;
            ConfiguredStartingSpeedElevation = 0;
        }

        public ModbusTCPMCUCommunicationHandler() : base(MCUConstants.MCU_IP_ADDRESS, MCUConstants.MCU_MODBUS_TCP_PORT)
        {
            // Does nothing extra
        }

        ~ModbusTCPMCUCommunicationHandler()
        {
            CancelCurrentMoveCommand();
            Thread.Sleep(100);
        }

        private void SendResetErrorsCommand()
        {
            GenericModbusTCPMaster.WriteMultipleRegisters(MCUConstants.MCU_WRITE_REGISTER_START_ADDRESS, MESSAGE_CONTENTS_RESET_ERRORS);
        }

        public override bool TestCommunication()
        {
            // Read the heartbeat register
            ushort[] inputRegisters = GenericModbusTCPMaster.ReadInputRegisters(MCUConstants.MCU_READ_INPUT_REGISTER_HEARTBEAT_ADDRESS, 1);
            return (inputRegisters[0] == 8192) || (inputRegisters[0] == 24576);
        }

        public override Orientation GetCurrentOrientation()
        {
            // Get the MCU's value for the displacement since its power cycle
            // Either issue one read command that's larger than necessary, or issue two of them at the exact - if it's more efficient to do one or the other, that can be changed
            ushort[] ClippedInputRegisterInfo = GenericModbusTCPMaster.ReadInputRegisters(MCUConstants.MCU_READ_INPUT_REGISTER_CURRENT_POSITION_ADDRESS, 12);
            return new Orientation(
                ConversionHelper.StepsToDegrees((65536 * ClippedInputRegisterInfo[0]) + ClippedInputRegisterInfo[1], MotorConstants.GEARING_RATIO_AZIMUTH),
                ConversionHelper.StepsToDegrees((65536 * ClippedInputRegisterInfo[10]) + ClippedInputRegisterInfo[11], MotorConstants.GEARING_RATIO_ELEVATION)
            );
        }

        public override bool[] GetCurrentLimitSwitchStatuses()
        {
            throw new NotImplementedException("This is a currently unsupported operation.");
        }

        public override bool GetCurrentSafetyInterlockStatus()
        {
            throw new NotImplementedException("This is a currently unsupported operation.");
        }

        public override bool CancelCurrentMoveCommand()
        {
            PrintInputRegisterContents("Before cancelling move", MCUConstants.MCU_READ_INPUT_REGISTER_START_ADDRESS, 10);

            GenericModbusTCPMaster.WriteMultipleRegisters(MCUConstants.MCU_WRITE_REGISTER_START_ADDRESS, MESSAGE_CONTENTS_CLEAR_MOVE);

            Thread.Sleep(100);
            PrintInputRegisterContents("After cancelling move", MCUConstants.MCU_READ_INPUT_REGISTER_START_ADDRESS, 10);
            return true;
        }

        public override bool ShutdownRadioTelescope()
        {
            throw new NotImplementedException("This is a currently unsupported operation.");
        }

        public override bool CalibrateRadioTelescope()
        {
            throw new NotImplementedException("This is a currently unsupported operation.");
        }

        public override bool ConfigureRadioTelescope(double startSpeedDPSAzimuth, double startSpeedDPSElevation, int homeTimeoutSecondsAzimuth, int homeTimeoutSecondsElevation)
        {
            int gearedSpeedAZ = ConversionHelper.DPSToSPS(startSpeedDPSAzimuth, MotorConstants.GEARING_RATIO_AZIMUTH);
            int gearedSpeedEL = ConversionHelper.DPSToSPS(startSpeedDPSElevation, MotorConstants.GEARING_RATIO_ELEVATION);

            if ((gearedSpeedAZ < 1) || (gearedSpeedEL < 1) || (homeTimeoutSecondsAzimuth < 0) || (homeTimeoutSecondsElevation < 0)
                || (gearedSpeedAZ > 1000000) || (gearedSpeedEL > 1000000) || (homeTimeoutSecondsAzimuth > 300) || (homeTimeoutSecondsElevation > 300))
            {
                return false;
            }

            ushort[] DataToWrite =
            {
                0x8400,                               // The configuration for the YCP radio telescope's MCU, see the output configuration table of the MCU for the bit-wise info
                0x0000,                               // Configuration info, continued
                (ushort)(gearedSpeedAZ >> 0x0010),    // MSW for azimuth starting speed
                (ushort)(gearedSpeedAZ & 0xFFFF),     // LSW for azimuth starting speed
                (ushort)homeTimeoutSecondsAzimuth,    // The timeout for the azimuth to reach its homing position
                0,                                    // Reserved for configuration commands
                0,                                    // Reserved for configuration commands
                0,                                    // Reserved for configuration commands
                0,                                    // Reserved for configuration commands
                0,                                    // Reserved for configuration commands
                0x8400,                               // Reserved for configuration commands
                0x0000,                               // Reserved for configuration commands
                (ushort)(gearedSpeedEL >> 0x0010),    // MSW for elevation starting speed
                (ushort)(gearedSpeedEL & 0xFFFF),     // LSW for elevation starting speed
                (ushort)homeTimeoutSecondsElevation,  // The timeout for the elevation to reach its homing position
                0,                                    // Reserved for configuration commands
                0,                                    // Reserved for configuration commands
                0,                                    // Reserved for configuration commands
                0,                                    // Reserved for configuration commands
                0                                     // Reserved for configuration commands
            };

            GenericModbusTCPMaster.WriteMultipleRegisters(MCUConstants.MCU_WRITE_REGISTER_START_ADDRESS, DataToWrite);
            //SendResetErrorsCommand();

            ConfiguredStartingSpeedAzimuth = gearedSpeedAZ;
            ConfiguredStartingSpeedElevation = gearedSpeedEL;

            return true;
        }

        public override bool MoveRadioTelescopeToOrientation(Orientation orientation)
        {
            int ObjectivePositionStepsAZ = ConversionHelper.DegreesToSteps(orientation.Azimuth, MotorConstants.GEARING_RATIO_AZIMUTH);
            int ObjectivePositionStepsEL = ConversionHelper.DegreesToSteps(orientation.Elevation, MotorConstants.GEARING_RATIO_ELEVATION);

            Orientation CurrentMCUPosition = GetCurrentOrientation();
            int CurrentPositionStepsAZ = ConversionHelper.DegreesToSteps(CurrentMCUPosition.Azimuth, MotorConstants.GEARING_RATIO_AZIMUTH);
            int CurrentPositionStepsEL = ConversionHelper.DegreesToSteps(CurrentMCUPosition.Elevation, MotorConstants.GEARING_RATIO_ELEVATION);
            
            double PositionTranslationDegreesAZ = ConversionHelper.StepsToDegrees(ObjectivePositionStepsAZ - CurrentPositionStepsAZ, MotorConstants.GEARING_RATIO_AZIMUTH);
            double PositionTranslationDegreesEL = ConversionHelper.StepsToDegrees(ObjectivePositionStepsEL - CurrentPositionStepsEL, MotorConstants.GEARING_RATIO_ELEVATION);

            double FixedSpeedDPS = ConversionHelper.RPMToDPS(DEFAULT_SPEED_IF_UNSPECIFIED_RPMS);
            return ExecuteRelativeMove(FixedSpeedDPS, PositionTranslationDegreesAZ, FixedSpeedDPS, PositionTranslationDegreesEL);
        }

        public override bool StartRadioTelescopeJog(RadioTelescopeAxisEnum axis, double speedDPS, bool clockwise)
        {
            PrintInputRegisterContents("Before jog", MCUConstants.MCU_READ_INPUT_REGISTER_START_ADDRESS, 10);

            int JogMoveIndex, ClearMoveIndex, ApplicableGearingRatio;

            switch (axis)
            {
                case RadioTelescopeAxisEnum.AZIMUTH:
                    JogMoveIndex = 0;
                    ClearMoveIndex = 10;
                    ApplicableGearingRatio = MotorConstants.GEARING_RATIO_AZIMUTH;
                    break;

                case RadioTelescopeAxisEnum.ELEVATION:
                    JogMoveIndex = 10;
                    ClearMoveIndex = 0;
                    speedDPS *= 8;
                    ApplicableGearingRatio = MotorConstants.GEARING_RATIO_ELEVATION;
                    break;

                default:
                    throw new ArgumentException("Invalid axis enum provided: " + axis.ToString());
            }

            int programmedPeakSpeedInt = ConversionHelper.DPSToSPS(speedDPS, ApplicableGearingRatio);
            if ((programmedPeakSpeedInt < 1) || (programmedPeakSpeedInt > 1000000))
            {
                return false;
            }

            ushort[] JogDataToWrite =
            {
                (ushort)(clockwise ? 0x0080 : 0x0100),      // Denotes a jog move, either CW or CCW, in command mode for an axis
                0x3,                                        // Denotes a Trapezoidal S-Curve profile
                0,                                          // Reserved to 0 for a jog command
                0,                                          // Reserved to 0 for a jog command
                (ushort)(programmedPeakSpeedInt >> 0x10),   // MSW for an axis speed
                (ushort)(programmedPeakSpeedInt & 0xFFFF),  // LSW for an axis speed
                MCUConstants.MCU_DEFAULT_ACCELERATION,      // Acceleration for an axis
                MCUConstants.MCU_DEFAULT_ACCELERATION,      // Deceleration for an axis
                0,                                          // Reserved for a relative move
                0                                           // Reserved for a relative move
            };

            ushort[] DataToWrite = new ushort[20];

            try
            {
                Array.Copy(JogDataToWrite, 0, DataToWrite, JogMoveIndex, 10);
                Array.Copy(MESSAGE_CONTENTS_CLEAR_MOVE, 0, DataToWrite, ClearMoveIndex, 10);
            }
            catch (Exception e)
            {
                if ((e is ArgumentNullException) || (e is RankException) || (e is ArrayTypeMismatchException)
                    || (e is InvalidCastException) || (e is ArgumentOutOfRangeException) || (e is ArgumentException))
                {
                    return false;
                }
                else
                {
                    // Unexpected exception
                    throw e;
                }
            }

            GenericModbusTCPMaster.WriteMultipleRegisters(MCUConstants.MCU_WRITE_REGISTER_START_ADDRESS, DataToWrite);

            Thread.Sleep(100);
            PrintInputRegisterContents("After jog", MCUConstants.MCU_READ_INPUT_REGISTER_START_ADDRESS, 10);

            return true;
        }

        public override bool ExecuteRadioTelescopeControlledStop()
        {
            return CancelCurrentMoveCommand();
        }

        public override bool ExecuteRadioTelescopeImmediateStop()
        {
            PrintInputRegisterContents("Before immediate stop", MCUConstants.MCU_READ_INPUT_REGISTER_START_ADDRESS, 10);

            GenericModbusTCPMaster.WriteMultipleRegisters(MCUConstants.MCU_WRITE_REGISTER_START_ADDRESS, MESSAGE_CONTENTS_IMMEDIATE_STOP);

            Thread.Sleep(100);
            PrintInputRegisterContents("After immediate stop", MCUConstants.MCU_READ_INPUT_REGISTER_START_ADDRESS, 10);
            return true;
        }

        public override bool ExecuteRelativeMove(double speedDPSAzimuth, double speedDPSElevation, double translationDegreesAzimuth, double translationDegreesElevation)
        {
            PrintInputRegisterContents("Before relative move", MCUConstants.MCU_READ_INPUT_REGISTER_START_ADDRESS, 10);

            int programmedPeakSpeedAZInt = ConversionHelper.DPSToSPS(speedDPSAzimuth, MotorConstants.GEARING_RATIO_AZIMUTH);
            if ((programmedPeakSpeedAZInt < ConfiguredStartingSpeedAzimuth) || (programmedPeakSpeedAZInt > 1000000))
            {
                return false;
            }

            int positionTranslationAZInt = ConversionHelper.DegreesToSteps(translationDegreesAzimuth, MotorConstants.GEARING_RATIO_AZIMUTH);
            if ((positionTranslationAZInt < -1073741823) || (positionTranslationAZInt > 1073741823))
            {
                return false;
            }

            // Page 59 of the MCU documentation ("ANFI1 & ANF2 AnyNET-I/O 1 or 2 Axis Servo/Stepper Controller with Interpolated Moves") says that
            // a negative relative move of less than 65,535 steps must make the MSW for the position -1. This check is done here.
            ushort positionTranslationAZMSW;
            if ((positionTranslationAZInt < 0) && (positionTranslationAZInt > -65535))
            {
                // For all those CS students out there (and those that "didn't like" Dr. Moscola's Fundamentals of CE), this is Two's-Complement "-1" for 16-bit words
                positionTranslationAZMSW = 0xFFFF;
            }
            else
            {
                positionTranslationAZMSW = (ushort)(positionTranslationAZInt >> 0x10);
            }

            int programmedPeakSpeedELInt = ConversionHelper.DPSToSPS(speedDPSElevation, MotorConstants.GEARING_RATIO_ELEVATION);
            if ((programmedPeakSpeedELInt < ConfiguredStartingSpeedAzimuth) || (programmedPeakSpeedELInt > 1000000))
            {
                return false;
            }

            int positionTranslationELInt = ConversionHelper.DegreesToSteps(translationDegreesElevation, MotorConstants.GEARING_RATIO_ELEVATION);
            if ((positionTranslationELInt < -1073741823) || (positionTranslationELInt > 1073741823))
            {
                return false;
            }
            
            ushort positionTranslationELMSW;
            if ((positionTranslationELInt < 0) && (positionTranslationELInt > -65535))
            {
                positionTranslationELMSW = 0xFFFF;
            }
            else
            {
                positionTranslationELMSW = (ushort)(positionTranslationELInt >> 0x10);
            }

            ushort[] DataToWrite =
            {
                0x2,                                          // Denotes a relative move in azimuth
                0x3,                                          // Denotes a Trapezoidal S-Curve profile
                positionTranslationAZMSW,                     // MSW for azimuth position
                (ushort)(positionTranslationAZInt & 0xFFFF),  // LSW for elevation position
                (ushort)(programmedPeakSpeedAZInt >> 0x10),   // MSW for azimuth speed
                (ushort)(programmedPeakSpeedAZInt & 0xFFFF),  // LSW for azimuth speed
                MCUConstants.MCU_DEFAULT_ACCELERATION,        // Acceleration for azimuth
                MCUConstants.MCU_DEFAULT_ACCELERATION,        // Deceleration for azimuth
                0,                                            // Reserved for a relative move
                0,                                            // Reserved for a relative move
                0x2,                                          // Denotes a relative move in elevation
                0x3,                                          // Denotes a Trapezoidal S-Curve profile
                positionTranslationELMSW,                     // MSW for elevation position
                (ushort)(positionTranslationELInt & 0xFFFF),  // LSW for elevation position
                (ushort)(programmedPeakSpeedELInt >> 0x10),   // MSW for elevation speed
                (ushort)(programmedPeakSpeedELInt & 0xFFFF),  // MSW for elevation speed
                MCUConstants.MCU_DEFAULT_ACCELERATION,        // Acceleration for elevation
                MCUConstants.MCU_DEFAULT_ACCELERATION,        // Deceleration for elevation
                0,                                            // Reserved for a relative move
                0                                             // Reserved for a relative move
            };

            GenericModbusTCPMaster.WriteMultipleRegisters(MCUConstants.MCU_WRITE_REGISTER_START_ADDRESS, DataToWrite);

            Thread.Sleep(100);
            PrintInputRegisterContents("After relative move", MCUConstants.MCU_READ_INPUT_REGISTER_START_ADDRESS, 10);

            return true;
        }
    }
}
