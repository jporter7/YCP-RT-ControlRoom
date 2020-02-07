namespace ControlRoomApplication.Constants
{
    public sealed class MotorConstants
    {
        /// <summary>
        /// set by the as a the dip switches on the motordriver default = 20_000
        /// </summary>
        public static int STEPS_PER_REVOLUTION_BEFORE_GEARING = 20_000;

        public const int GEARING_RATIO_AZIMUTH = 500;
        public const int GEARING_RATIO_ELEVATION = 50;
    }
}