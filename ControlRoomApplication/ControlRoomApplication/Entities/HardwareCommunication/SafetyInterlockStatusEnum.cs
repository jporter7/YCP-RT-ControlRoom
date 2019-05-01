using System;

namespace ControlRoomApplication.Entities
{
    public enum SafetyInterlockStatusEnum : byte
    {
        UNDEFINED = 0x0,
        LOCKED = 0x1,
        UNLOCKED = 0x2
    }

    public class SafetyInterlockStatusConversionHelper
    {
        public static SafetyInterlockStatusEnum GetFromByte(byte input)
        {
            if (!Enum.IsDefined(typeof(SafetyInterlockStatusEnum), input))
            {
                return SafetyInterlockStatusEnum.UNDEFINED;
            }
            else
            {
                return (SafetyInterlockStatusEnum)input;
            }
        }

        public static byte ConvertToByte(SafetyInterlockStatusEnum input)
        {
            if (!Enum.IsDefined(typeof(SafetyInterlockStatusEnum), input))
            {
                return ConvertToByte(SafetyInterlockStatusEnum.UNDEFINED);
            }
            else
            {
                return (byte)input;
            }
        }
    }
}
