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

        public static SpectraCyberBandwidthEnum GetEnumFromValue(int value)
        {
            switch (value)
            {
                case 15:
                    return SpectraCyberBandwidthEnum.SMALL_BANDWIDTH;
                case 1200:
                    return SpectraCyberBandwidthEnum.LARGE_BANDWIDTH;
                default:
                    return SpectraCyberBandwidthEnum.UNDEFINED;
            }
        }

        public static int GetIntegerValue(this SpectraCyberBandwidthEnum time)
        {
            switch (time)
            {
                case SpectraCyberBandwidthEnum.SMALL_BANDWIDTH:
                    return 15;
                case SpectraCyberBandwidthEnum.LARGE_BANDWIDTH:
                    return 1200;
              //  case SpectraCyberBandwidthEnum.UNDEFINED:
               //     throw new System.Exception("UNDEFINED SpectraCyberBandwidthEnum type");
                default:
                    return 30;
                //    throw new System.Exception("Unexpected SpectraCyberBandwidthEnum type");
            }
        }
    }
}
