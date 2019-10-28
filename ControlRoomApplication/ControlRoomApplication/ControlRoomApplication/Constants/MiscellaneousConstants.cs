using ControlRoomApplication.Entities;

namespace ControlRoomApplication.Constants
{
    public sealed class MiscellaneousConstants
    {
        public const int MAX_USERNAME_LENGTH = 15;
        public const string LOCAL_DATABASE_NAME = "rtdatabase";

        // Can't create const instances, we have to use this as static readonly, sadly
        public static readonly Location JOHN_RUDY_PARK = new Location(76.7046, 40.0244, 395.0);

        public const double NEGLIGIBLE_POSITION_CHANGE_DEGREES = 0.1;
    }
}