using System;

namespace ControlRoomApplication.Entities
{
    public enum HardwareMesageResponseExpectationEnum : byte
    {
        UNDEFINED = 0x0,
        MINOR_RESPONSE = 0x1,
        FULL_RESPONSE = 0x2
    }

    public class HardwareMessageResponseExpectationConversionHelper
    {
        public static HardwareMesageResponseExpectationEnum GetFromByte(byte input)
        {
            if (!Enum.IsDefined(typeof(HardwareMesageResponseExpectationEnum), input))
            {
                return HardwareMesageResponseExpectationEnum.UNDEFINED;
            }
            else
            {
                return (HardwareMesageResponseExpectationEnum)input;
            }
        }

        public static byte ConvertToByte(HardwareMesageResponseExpectationEnum input)
        {
            if (!Enum.IsDefined(typeof(HardwareMesageResponseExpectationEnum), input))
            {
                return ConvertToByte(HardwareMesageResponseExpectationEnum.UNDEFINED);
            }
            else
            {
                return (byte)input;
            }
        }
    }
}
