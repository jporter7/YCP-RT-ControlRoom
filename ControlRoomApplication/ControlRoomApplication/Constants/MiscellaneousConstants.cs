using ControlRoomApplication.Entities;

namespace ControlRoomApplication.Constants
{
    public sealed class MiscellaneousConstants
    {
        public const int MAX_USERNAME_LENGTH = 15;
        public const string LOCAL_DATABASE_NAME = "rtdatabase";

        public const string LOCAL_HOST_IP = "127.0.0.1";
        public const int PORT_5012 = 5012;
        public const int PORT_8080 = 8080;
        public const int PORT_58006 = 58006;

        public const int GEARED_STEPS_PER_REVOLUTION = 10000000;

        // Can't create const instances of objects, we have to use this as static readonly
        public static readonly Location JOHN_RUDY_PARK = new Location(76.7046, 40.0244, 395.0);

        public const double NEGLIGIBLE_POSITION_CHANGE_DEGREES = 0.1;
    }
}