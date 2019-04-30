using System;
using ControlRoomApplication.Constants;
using ControlRoomApplication.Entities;

namespace ControlRoomApplication.Controllers
{
    public class ModbusTCPMCUCommunicationHandler : BaseModbusTCPCommunicationHandler
    {
        private static readonly ushort[] MESSAGE_CONTENTS_IMMEDIATE_STOP = new ushort[] { 0x0010, 0x0003, 0x0000, 0x0000, 0x0000, 0x0000, 0x0000, 0x0000, 0x0000, 0x0000 };
        private static readonly ushort[] MESSAGE_CONTENTS_CLEAR_MOVE = new ushort[] { 0x0000, 0x0003, 0x0000, 0x0000, 0x0000, 0x0000, 0x0000, 0x0000, 0x0000, 0x0000 };
        private static readonly ushort[] MESSAGE_CONTENTS_RESET_ERRORS = new ushort[] { 0x0800, 0x0000, 0x0000, 0x0000, 0x0000, 0x0000, 0x0000, 0x0000, 0x0000, 0x0000 };

        public int ConfiguredStartingSpeedAzimuth { get; private set; }
        public int ConfiguredStartingSpeedElevation { get; private set; }

        public ModbusTCPMCUCommunicationHandler(string ip, int port) : base(ip, port)
        {
            ConfiguredStartingSpeedAzimuth = 0;
            ConfiguredStartingSpeedElevation = 0;
        }

        public ModbusTCPMCUCommunicationHandler() : base(MCUConstants.ACTUAL_MCU_IP_ADDRESS, MCUConstants.ACTUAL_MCU_MODBUS_TCP_PORT)
        {
            // Does nothing extra
        }

        private void SendResetErrorsCommand()
        {
            GenericModbusTCPMaster.WriteMultipleRegisters(MCUConstants.ACTUAL_MCU_WRITE_REGISTER_START_ADDRESS, MESSAGE_CONTENTS_RESET_ERRORS);
        }

        public override bool TestCommunication()
        {
            // Read the heartbeat register
            ushort[] inputRegisters = GenericModbusTCPMaster.ReadInputRegisters(MCUConstants.ACTUAL_MCU_READ_INPUT_REGISTER_HEARTBEAT_ADDRESS, 1);
            return (inputRegisters[0] == 8192) || (inputRegisters[0] == 24576);
        }

        public override Orientation GetCurrentOrientation()
        {
            // Get the MCU's value for the displacement since its power cycle
            ushort[] inputRegisters = GenericModbusTCPMaster.ReadInputRegisters(MCUConstants.ACTUAL_MCU_READ_INPUT_REGISTER_CURRENT_POSITION_ADDRESS, 2);
            return new Orientation(
                ((65536 * inputRegisters[0]) + inputRegisters[1]) * 360.0 / MiscellaneousConstants.GEARED_STEPS_PER_REVOLUTION,
                0.0
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
            GenericModbusTCPMaster.WriteMultipleRegisters(MCUConstants.ACTUAL_MCU_WRITE_REGISTER_START_ADDRESS, MESSAGE_CONTENTS_CLEAR_MOVE);
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

        public override bool ConfigureRadioTelescope(double startSpeedAzimuth, double startSpeedElevation, int homeTimeoutAzimuth, int homeTimeoutElevation)
        {
            int gearedSpeedAZ = (int)((startSpeedAzimuth / 360) * MiscellaneousConstants.GEARED_STEPS_PER_REVOLUTION);
            int gearedSpeedEL = (int)((startSpeedElevation / 360) * MiscellaneousConstants.GEARED_STEPS_PER_REVOLUTION);

            if ((gearedSpeedAZ < 1) || (gearedSpeedEL < 1) || (homeTimeoutAzimuth < 0) || (homeTimeoutElevation < 0)
                || (gearedSpeedAZ > 1000000) || (gearedSpeedEL > 1000000) || (homeTimeoutAzimuth > 300) || (homeTimeoutElevation > 300))
            {
                return false;
            }

            ushort[] DataToWrite =
            {
                0x8400,
                0x0000,
                (ushort)(gearedSpeedAZ >> 0x0010),
                (ushort)(gearedSpeedAZ & 0xFFFF),
                (ushort)(homeTimeoutAzimuth),
                0,
                0,
                0,
                0,
                0
            };

            GenericModbusTCPMaster.WriteMultipleRegisters(MCUConstants.ACTUAL_MCU_WRITE_REGISTER_START_ADDRESS, DataToWrite);
            SendResetErrorsCommand();

            ConfiguredStartingSpeedAzimuth = gearedSpeedAZ;
            ConfiguredStartingSpeedElevation = gearedSpeedEL;

            return true;
        }

        public override bool MoveRadioTelescopeToOrientation(Orientation orientation)
        {
            throw new NotImplementedException("This is a currently unsupported operation.");
        }

        public override bool StartRadioTelescopeJog(RadioTelescopeAxisEnum axis, double speed, bool clockwise)
        {
            // Make sure the command is intended for the azimuth
            if (axis != RadioTelescopeAxisEnum.AZIMUTH)
            {
                return false;
            }

            int programmedPeakSpeedInt = (int)((speed / 360) * MiscellaneousConstants.GEARED_STEPS_PER_REVOLUTION);
            if ((programmedPeakSpeedInt < 1) || (programmedPeakSpeedInt > 1000000))
            {
                return false;
            }

            ushort[] DataToWrite =
            {
                (ushort)(clockwise ? 0x0080 : 0x0100),   // Denotes a jog move, either CW or CCW, in command mode
                0x3,                                     // Denotes a Trapezoidal S-Curve profile
                0,                                       // Reserved to 0 for a jog command
                0,                                       // Reserved to 0 for a jog command
                (ushort)(programmedPeakSpeedInt >> 0x10),
                (ushort)(programmedPeakSpeedInt & 0xFFFF),
                MCUConstants.ACTUAL_MCU_MOVE_ACCELERATION_WITH_GEARING,
                MCUConstants.ACTUAL_MCU_MOVE_ACCELERATION_WITH_GEARING,
                0,
                0
            };

            GenericModbusTCPMaster.WriteMultipleRegisters(MCUConstants.ACTUAL_MCU_WRITE_REGISTER_START_ADDRESS, DataToWrite);

            return true;
        }

        public override bool ExecuteRadioTelescopeControlledStop()
        {
            return CancelCurrentMoveCommand();
        }

        public override bool ExecuteRadioTelescopeImmediateStop()
        {
            GenericModbusTCPMaster.WriteMultipleRegisters(MCUConstants.ACTUAL_MCU_WRITE_REGISTER_START_ADDRESS, MESSAGE_CONTENTS_IMMEDIATE_STOP);
            return true;
        }

        public override bool ExecuteRelativeMove(RadioTelescopeAxisEnum axis, double speed, double position)
        {
            // Make sure the command is intended for the azimuth
            if (axis != RadioTelescopeAxisEnum.AZIMUTH)
            {
                return false;
            }

            int programmedPeakSpeedInt = (int)((speed / 360) * MiscellaneousConstants.GEARED_STEPS_PER_REVOLUTION);
            if ((programmedPeakSpeedInt < ConfiguredStartingSpeedAzimuth) || (programmedPeakSpeedInt > 1000000))
            {
                return false;
            }

            int positionTranslationInt = (int)((position / 360) * MiscellaneousConstants.GEARED_STEPS_PER_REVOLUTION);
            if ((positionTranslationInt < -1073741823) || (positionTranslationInt > 1073741823))
            {
                return false;
            }

            ushort[] DataToWrite =
            {
                0x2,                                        // Denotes a relative move
                0x3,                                        // Denotes a Trapezoidal S-Curve profile
                (ushort)(programmedPeakSpeedInt >> 0x10),   // MSW for position
                (ushort)(programmedPeakSpeedInt & 0xFFFF),  // LSW for position
                (ushort)(programmedPeakSpeedInt >> 0x10),
                (ushort)(programmedPeakSpeedInt & 0xFFFF),
                MCUConstants.ACTUAL_MCU_MOVE_ACCELERATION_WITH_GEARING,
                MCUConstants.ACTUAL_MCU_MOVE_ACCELERATION_WITH_GEARING,
                0,
                0
            };

            GenericModbusTCPMaster.WriteMultipleRegisters(MCUConstants.ACTUAL_MCU_WRITE_REGISTER_START_ADDRESS, DataToWrite);

            return true;
        }
    }
}
