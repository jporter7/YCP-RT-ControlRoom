namespace ControlRoomApplication.Constants
{
    public sealed class HardwareConstants
    {
        public static readonly double NEGLIGIBLE_POSITION_CHANGE_DEGREES = 0.1;
        public static readonly double LIMIT_SWITCH_AZ_THRESHOLD_DEGREES = 2.0;
        public static readonly double LIMIT_SWITCH_EL_THRESHOLD_DEGREES = 2.0;

        public static readonly double SIMULATION_MCU_PEAK_VELOCITY = 22.5; // steps/s
        public static readonly double SIMULATION_MCU_PEAK_ACCELERATION = 32.0; // steps/s^2

        public static readonly double WEATHER_STATION_MAXIMUM_ALLOWABLE_WIND_SPEED_MPH = 110.0;
    }
}
