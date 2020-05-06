namespace ControlRoomApplication.Entities
{
    public enum SpectraCyberIntegrationTimeEnum
    {
        UNDEFINED,
        SHORT_TIME_SPAN,
        MID_TIME_SPAN,
        LONG_TIME_SPAN
    }

    public static class SpectraCyberIntegrationTimeEnumHelper
    {
        public static char GetValue(this SpectraCyberIntegrationTimeEnum time)
        {
            switch (time)
            {
                case SpectraCyberIntegrationTimeEnum.SHORT_TIME_SPAN:
                    return '0';
                case SpectraCyberIntegrationTimeEnum.MID_TIME_SPAN:
                    return '1';
                case SpectraCyberIntegrationTimeEnum.LONG_TIME_SPAN:
                    return '2';
                case SpectraCyberIntegrationTimeEnum.UNDEFINED:
                    throw new System.Exception("UNDEFINED SpectraCyberIntegrationTimeEnum type");
                default:
                    throw new System.Exception("Unexpected SpectraCyberIntegrationTimeEnum type");
            }
        }

        public static SpectraCyberIntegrationTimeEnum GetEnumFromValue(double val)
        {
            switch (val)
            {
                case 0.3:
                    return SpectraCyberIntegrationTimeEnum.SHORT_TIME_SPAN;
                case 0.75:
                    return SpectraCyberIntegrationTimeEnum.MID_TIME_SPAN;
                case 1:
                    return SpectraCyberIntegrationTimeEnum.LONG_TIME_SPAN;
                default:
                    return SpectraCyberIntegrationTimeEnum.UNDEFINED;
            }
        }

        public static double GetDoubleValue(this SpectraCyberIntegrationTimeEnum time)
        {
            switch (time)
            {
                case SpectraCyberIntegrationTimeEnum.SHORT_TIME_SPAN:
                    return 0.3;
                case SpectraCyberIntegrationTimeEnum.MID_TIME_SPAN:
                    return 0.75;
                case SpectraCyberIntegrationTimeEnum.LONG_TIME_SPAN:
                    return 1;
                default:
                    return 0.3;
            }
        }
    }
}
