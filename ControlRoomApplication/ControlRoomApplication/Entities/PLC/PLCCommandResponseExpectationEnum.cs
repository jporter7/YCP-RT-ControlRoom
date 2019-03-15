using System;

namespace ControlRoomApplication.Entities
{
    public enum PLCCommandResponseExpectationEnum : byte
    {
        UNDEFINED = 0x0,
        MINOR_RESPONSE = 0x1,
        FULL_RESPONSE = 0x2
    }

    public class PLCCommandResponseExpectationConversionHelper
    {
        public static PLCCommandResponseExpectationEnum GetFromByte(byte input)
        {
            if (!Enum.IsDefined(typeof(PLCCommandResponseExpectationEnum), input))
            {
                return PLCCommandResponseExpectationEnum.UNDEFINED;
            }
            else
            {
                return (PLCCommandResponseExpectationEnum)input;
            }
        }

        public static byte ConvertToByte(PLCCommandResponseExpectationEnum input)
        {
            if (!Enum.IsDefined(typeof(PLCCommandResponseExpectationEnum), input))
            {
                return ConvertToByte(PLCCommandResponseExpectationEnum.UNDEFINED);
            }
            else
            {
                return (byte)input;
            }
        }
    }
}
