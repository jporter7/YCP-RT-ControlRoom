using System;

namespace ControlRoomApplication.Entities
{
    public enum PLCLimitSwitchStatusEnum : byte
    {
        UNDEFINED = 0x0,
        WITHIN_SAFE_LIMITS = 0x1,
        WITHIN_WARNING_LIMITS = 0x2
    }

    public class PLCLimitSwitchStatusConversionHelper
    {
        public static PLCLimitSwitchStatusEnum GetFromByte(byte input)
        {
            if (!Enum.IsDefined(typeof(PLCLimitSwitchStatusEnum), input))
            {
                return PLCLimitSwitchStatusEnum.UNDEFINED;
            }
            else
            {
                return (PLCLimitSwitchStatusEnum)input;
            }
        }

        public static byte ConvertToByte(PLCLimitSwitchStatusEnum input)
        {
            if (!Enum.IsDefined(typeof(PLCLimitSwitchStatusEnum), input))
            {
                return ConvertToByte(PLCLimitSwitchStatusEnum.UNDEFINED);
            }
            else
            {
                return (byte)input;
            }
        }
    }
}
