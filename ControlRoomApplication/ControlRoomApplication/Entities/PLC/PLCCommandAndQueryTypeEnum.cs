using System;

namespace ControlRoomApplication.Entities
{
    public enum PLCCommandAndQueryTypeEnum : byte
    {
        UNDEFINED = 0x0,
        TEST_CONNECTION = 0x1,
        GET_CURRENT_AZEL_POSITIONS = 0x2,// remove, encoders not handled by plc anymore
        GET_CURRENT_LIMIT_SWITCH_STATUSES = 0x3,
        GET_CURRENT_SAFETY_INTERLOCK_STATUS = 0x4,
        CANCEL_ACTIVE_OBJECTIVE_AZEL_POSITION = 0x5,
        SHUTDOWN = 0x6,
        CALIBRATE = 0x7,
        SET_CONFIGURATION = 0x8,
        CONTROLLED_STOP = 0x9,
        IMMEDIATE_STOP= 0xA,
        SET_OBJECTIVE_AZEL_POSITION = 0xB,
        START_JOG_MOVEMENT = 0xC,
        TRANSLATE_AZEL_POSITION = 0xD
    }

    public class PLCCommandAndQueryTypeConversionHelper
    {
        public static PLCCommandAndQueryTypeEnum GetFromByte(byte input)
        {
            if (!Enum.IsDefined(typeof(PLCCommandAndQueryTypeEnum), input))
            {
                return PLCCommandAndQueryTypeEnum.UNDEFINED;
            }
            else
            {
                return (PLCCommandAndQueryTypeEnum)input;
            }
        }

        public static byte ConvertToByte(PLCCommandAndQueryTypeEnum input)
        {
            if (!Enum.IsDefined(typeof(PLCCommandAndQueryTypeEnum), input))
            {
                return ConvertToByte(PLCCommandAndQueryTypeEnum.UNDEFINED);
            }
            else
            {
                return (byte)input;
            }
        }
    }


    /// <summary>
    /// the cctrlroom has a modbus server running on it that is maped to input and output regs on the plc 
    /// 0-20 arr comand bytes that go through the plc to the mcu
    /// warnings, limits and other boolean values are sent as ints with 0 for false and 1 for true
    /// 
    /// </summary>
    public enum PLC_modbus_server_register_mapping : ushort
    {
        /// <summary>
        /// plc will write to this register to acknoldge that a comand has been routed through the plc to the mcu
        /// </summary>
        CMD_ACK = 0+1,

        AZ_LEFT_LIMIT  = 8 + 1,
        AZ_LEFT_WARNING = 9 + 1,
        AZ_RIGHT_WARNING = 10 + 1,
        AZ_RIGHT_LIMIT = 11 + 1,

        EL_BOTTOM_LIMIT = 12 + 1,
        EL_BOTTOM_WARNING = 13 + 1,
        EL_TOP_WARNING = 14 + 1,
        EL_TOP_LIMIT = 15 + 1,

        Safty_INTERLOCK = 18 + 1,

        AZ_MOTOR_CURRENT = 40 + 1,
        EL_MOTOR_CURRENT = 42 + 1
    }


}
