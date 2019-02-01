using AASharp;

namespace ControlRoomApplication.Constants
{
    public sealed class RadioTelescopeConstants
    {
        public static readonly double OBSERVATORY_LONGITUDE = AASCoordinateTransformation.DMSToDegrees(40, 1, 27.872);
        public static readonly double OBSERVATORY_LATITUDE = AASCoordinateTransformation.DMSToDegrees(76, 42, 16.430, false);
        public static readonly double OBSERVATORY_HEIGHT = 395.0;
    }
}