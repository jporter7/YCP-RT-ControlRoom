using System;

namespace ControlRoomApplication.Entities
{
    public enum PLCSafetyInterlockStatusEnum : byte
    {
        UNDEFINED = 0x0,
        LOCKED = 0x1,
        UNLOCKED = 0x2
    }

    public class PLCSafetyInterlockStatusConversionHelper
    {
        public static PLCSafetyInterlockStatusEnum GetFromByte(byte input)
        {
            if (!Enum.IsDefined(typeof(PLCSafetyInterlockStatusEnum), input))
            {
                return PLCSafetyInterlockStatusEnum.UNDEFINED;
            }
            else
            {
                return (PLCSafetyInterlockStatusEnum)input;
            }
        }

        public static byte ConvertToByte(PLCSafetyInterlockStatusEnum input)
        {
            if (!Enum.IsDefined(typeof(PLCSafetyInterlockStatusEnum), input))
            {
                return ConvertToByte(PLCSafetyInterlockStatusEnum.UNDEFINED);
            }
            else
            {
                return (byte)input;
            }
        }
    }
}
