namespace ControlRoomApplication.Constants
{
    public sealed class SimulationConstants
    {
        public const int STABLE_MOTOR_TEMP = 50; // Measured in farenheit
        public const int OVERHEAT_MOTOR_TEMP = 200;
        public const double AZIMUTH_UPDATE_RATE = 0.5; // Measured as degrees per second
        public const double ELEVATION_UPDATE_RATE = 0.25;
        public const double MAX_AZIMUTH_ANGLE = 89.5; //Measured in degrees
        public const double MIN_AZIMUTH_ANGLE = 0.25;
        public const double MAX_ELEVATION_ANGLE = 357.5;
        public const double MIN_ELEVATION_ANGLE = 0.5;
        
    }
}
