namespace ControlRoomApplication.Constants
{
    public sealed class SimulationConstants
    {
        public const double STABLE_MOTOR_TEMP = 50; // Measured in farenheit
        public const double OVERHEAT_MOTOR_TEMP = 150;
        public const double MIN_MOTOR_TEMP = 0;
        public const double MAX_MOTOR_TEMP = 200;

        public const double AZIMUTH_UPDATE_RATE = 1.0; // Measured as degrees per second
        public const double ELEVATION_UPDATE_RATE = 0.5;
        public const double MAX_AZIMUTH_ANGLE = 369; //Measured in degrees
        public const double MIN_AZIMUTH_ANGLE = -9;
        public const double MAX_ELEVATION_ANGLE = 92;
        public const double MIN_ELEVATION_ANGLE = -14;
        
    }
}
