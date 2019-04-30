using System;

namespace ControlRoomApplication.Entities
{
    public enum HardwareMessageResponseExpectationEnum : byte
    {
        UNDEFINED = 0x0,
        MINOR_RESPONSE = 0x1,
        FULL_RESPONSE = 0x2
    }

    public class HardwareMessageResponseExpectationConversionHelper
    {
        public static HardwareMessageResponseExpectationEnum GetFromByte(byte input)
        {
            if (!Enum.IsDefined(typeof(HardwareMessageResponseExpectationEnum), input))
            {
                return HardwareMessageResponseExpectationEnum.UNDEFINED;
            }
            else
            {
                return (HardwareMessageResponseExpectationEnum)input;
            }
        }

        public static byte ConvertToByte(HardwareMessageResponseExpectationEnum input)
        {
            if (!Enum.IsDefined(typeof(HardwareMessageResponseExpectationEnum), input))
            {
                return ConvertToByte(HardwareMessageResponseExpectationEnum.UNDEFINED);
            }
            else
            {
                return (byte)input;
            }
        }
    }
}
