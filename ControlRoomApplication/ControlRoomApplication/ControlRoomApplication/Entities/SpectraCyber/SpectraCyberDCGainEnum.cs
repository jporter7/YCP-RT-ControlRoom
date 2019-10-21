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
    }
}
