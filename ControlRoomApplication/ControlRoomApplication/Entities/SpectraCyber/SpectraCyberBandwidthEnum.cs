namespace ControlRoomApplication.Entities
{
    public enum SpectraCyberBandwidthEnum
    {
        UNDEFINED,
        SMALL_BANDWIDTH,
        MID_BANDWIDTH,
        LARGE_BANDWIDTH
    }

    public static class SpectraCyberBandwidthEnumHelper
    {
        public static string GetValue(this SpectraCyberBandwidthEnum time)
        {
            switch (time)
            {
                case SpectraCyberBandwidthEnum.SMALL_BANDWIDTH:
                    return "7.5";
                case SpectraCyberBandwidthEnum.MID_BANDWIDTH:
                    return "15";
                case SpectraCyberBandwidthEnum.LARGE_BANDWIDTH:
                    return "30";
                case SpectraCyberBandwidthEnum.UNDEFINED:
                    throw new System.Exception("UNDEFINED SpectraCyberBandwidthEnum type");
                default:
                    throw new System.Exception("Unexpected SpectraCyberBandwidthEnum type");
            }
        }
    }
}
