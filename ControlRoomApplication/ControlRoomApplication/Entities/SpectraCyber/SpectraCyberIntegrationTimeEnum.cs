namespace ControlRoomApplication.Entities
{
    public enum SpectraCyberIntegrationTimeEnum
    {
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
                default:
                    throw new System.Exception();
            }
        }
    }
}
