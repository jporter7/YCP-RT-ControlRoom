namespace ControlRoomApplication.Constants
{
    public sealed class HeartbeatConstants
    {
        public const int INTERFACE_CHECK_IN_RATE_MS = 3000;
        public const int MAXIMUM_ALLOWABLE_DIFFERENCE_IN_LAST_HEARD_MS = 2 * INTERFACE_CHECK_IN_RATE_MS;
    }
}