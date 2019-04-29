using System;

namespace ControlRoomApplication.Entities
{
    public enum RadioTelescopeStatusEnum : byte
    {
        UNDEFINED = 0x0,
        IN_BRINGUP_SEQUENCE = 0x1,
        ONLINE_AND_IDLE = 0x2,
        ONLINE_AND_MOVING = 0x3,
        ONLINE_AND_CANCELLING_ACTIVE_MOVE = 0x4,
        IN_SHUTDOWN_SEQUENCE = 0x5,
        EMERGENCY_STOPPED_BRB = 0x6,
        EMERGENCY_STOPPED_SAFETY_INTERLOCK = 0x7,
        EMERGENCY_STOPPED_CAMERA = 0x8,
        MISCELLANEOUS_ERROR = 0xFF
    }

    public class RadioTelescopeStatusConversionHelper
    {
        public static RadioTelescopeStatusEnum GetFromByte(byte input)
        {
            if (!Enum.IsDefined(typeof(RadioTelescopeStatusEnum), input))
            {
                return RadioTelescopeStatusEnum.UNDEFINED;
            }
            else
            {
                return (RadioTelescopeStatusEnum)input;
            }
        }

        public static byte ConvertToByte(RadioTelescopeStatusEnum input)
        {
            if (!Enum.IsDefined(typeof(RadioTelescopeStatusEnum), input))
            {
                return ConvertToByte(RadioTelescopeStatusEnum.UNDEFINED);
            }
            else
            {
                return (byte)input;
            }
        }
    }
}
