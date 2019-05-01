using System;

namespace ControlRoomApplication.Entities
{
    public enum LimitSwitchStatusEnum : byte
    {
        UNDEFINED = 0x0,
        WITHIN_SAFE_LIMITS = 0x1,
        WITHIN_WARNING_LIMITS = 0x2
    }

    public class LimitSwitchStatusConversionHelper
    {
        public static LimitSwitchStatusEnum GetFromByte(byte input)
        {
            if (!Enum.IsDefined(typeof(LimitSwitchStatusEnum), input))
            {
                return LimitSwitchStatusEnum.UNDEFINED;
            }
            else
            {
                return (LimitSwitchStatusEnum)input;
            }
        }

        public static byte ConvertToByte(LimitSwitchStatusEnum input)
        {
            if (!Enum.IsDefined(typeof(LimitSwitchStatusEnum), input))
            {
                return ConvertToByte(LimitSwitchStatusEnum.UNDEFINED);
            }
            else
            {
                return (byte)input;
            }
        }
    }
}
