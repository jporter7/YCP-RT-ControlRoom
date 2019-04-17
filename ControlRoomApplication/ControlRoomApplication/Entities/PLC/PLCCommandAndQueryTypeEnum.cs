using System;

namespace ControlRoomApplication.Entities
{
    public enum PLCCommandAndQueryTypeEnum : byte
    {
        UNDEFINED = 0x0,
        TEST_CONNECTION = 0x1,
        GET_CURRENT_AZEL_POSITIONS = 0x2,
        GET_CURRENT_LIMIT_SWITCH_STATUSES = 0x3,
        GET_CURRENT_SAFETY_INTERLOCK_STATUS = 0x4,
        CANCEL_ACTIVE_OBJECTIVE_AZEL_POSITION = 0x5,
        SHUTDOWN = 0x6,
        CALIBRATE = 0x7,
        SET_CONFIGURATION = 0x8,
        CONTROLLED_STOP = 0x9,
        IMMEDIATE_STOP= 0xA,
        SET_OBJECTIVE_AZEL_POSITION = 0xB,
        START_JOG_MOVEMENT = 0xC
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
}
