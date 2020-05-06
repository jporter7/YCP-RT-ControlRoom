namespace ControlRoomApplication.Entities
{
    public enum SpectraCyberDCGainEnum
    {
        UNDEFINED,
        X1,
        X5,
        X10,
        X20,
        X50,
        X60
    }

    public static class SpectraCyberDCGainEnumHelper
    {
        public static string GetValue(this SpectraCyberDCGainEnum time)
        {
            switch (time)
            {
                case SpectraCyberDCGainEnum.X1:
                    return "X1";
                case SpectraCyberDCGainEnum.X5:
                    return "X5";
                case SpectraCyberDCGainEnum.X10:
                    return "X10";
                case SpectraCyberDCGainEnum.X20:
                    return "X20";
                case SpectraCyberDCGainEnum.X50:
                    return "X50";
                case SpectraCyberDCGainEnum.X60:
                    return "X60";
                case SpectraCyberDCGainEnum.UNDEFINED:
                    throw new System.Exception("UNDEFINED SpectraCyberDCGainEnum type");
                default:
                    throw new System.Exception("Unexpected SpectraCyberDCGainEnum type");
            }
        }

        public static SpectraCyberDCGainEnum GetEnumFromValue(int value)
        {
            switch (value)
            {
                case 1:
                    return SpectraCyberDCGainEnum.X1;
                case 5:
                    return SpectraCyberDCGainEnum.X5;
                case 10:
                    return SpectraCyberDCGainEnum.X10;
                case 20:
                    return SpectraCyberDCGainEnum.X20;
                case 50:
                    return SpectraCyberDCGainEnum.X50;
                case 60:
                    return SpectraCyberDCGainEnum.X60;
                default:
                    return SpectraCyberDCGainEnum.UNDEFINED;
            }
        }

        public static int GetIntegerValue(this SpectraCyberDCGainEnum time)
        {
            switch (time)
            {
                case SpectraCyberDCGainEnum.X1:
                    return 1;
                case SpectraCyberDCGainEnum.X5:
                    return 5;
                case SpectraCyberDCGainEnum.X10:
                    return 10;
                case SpectraCyberDCGainEnum.X20:
                    return 20;
                case SpectraCyberDCGainEnum.X50:
                    return 50;
                case SpectraCyberDCGainEnum.X60:
                    return 60;
                case SpectraCyberDCGainEnum.UNDEFINED:
                    throw new System.Exception("UNDEFINED SpectraCyberDCGainEnum type");
                default:
                    throw new System.Exception("Unexpected SpectraCyberDCGainEnum type");
            }
        }
    }
}
