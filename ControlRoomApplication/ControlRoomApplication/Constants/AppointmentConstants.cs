namespace ControlRoomApplication.Constants
{
    public sealed class AppointmentConstants
    {
        public const string REQUESTED = "REQUESTED";
        public const string SCHEDULED = "SCHEDULED";
        public const string IN_PROGRESS = "IN_PROGRESS";
        public const string CANCELLED = "CANCELLED";
        public const string COMPLETED = "COMPLETED";

        public static readonly string[] AppointmentStatuses =
        {
            REQUESTED,
            SCHEDULED,
            IN_PROGRESS,
            CANCELLED,
            COMPLETED
        };
    }
}
