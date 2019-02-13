namespace ControlRoomApplication.Constants
{
    public sealed class HeartbeatConstants
    {
        public static readonly int INTERFACE_CHECK_IN_RATE_MS = 1000;
        public static readonly int MAXIMUM_ALLOWABLE_DIFFERENCE_IN_LAST_HEARD_MS = 2 * INTERFACE_CHECK_IN_RATE_MS;
    }
}