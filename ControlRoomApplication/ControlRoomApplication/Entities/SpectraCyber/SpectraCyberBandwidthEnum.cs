namespace ControlRoomApplication.Entities
{
    public enum SpectraCyberBandwidthEnum
    {
        UNDEFINED,
        SMALL_BANDWIDTH,
        LARGE_BANDWIDTH
    }

    public static class SpectraCyberBandwidthEnumHelper
    {
        public static string GetValue(this SpectraCyberBandwidthEnum time)
        {
            switch (time)
            {
                case SpectraCyberBandwidthEnum.SMALL_BANDWIDTH:
                    return "15Khz";
                case SpectraCyberBandwidthEnum.LARGE_BANDWIDTH:
                    return "30Khz";
                case SpectraCyberBandwidthEnum.UNDEFINED:
                    throw new System.Exception("UNDEFINED SpectraCyberBandwidthEnum type");
                default:
                    throw new System.Exception("Unexpected SpectraCyberBandwidthEnum type");
            }
        }
    }
}
