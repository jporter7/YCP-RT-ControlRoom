namespace ControlRoomApplication.Constants
{
    public sealed class MCUConstants
    {
        public const double SIMULATION_MCU_PEAK_VELOCITY = 22.5; // steps/s
        public const double SIMULATION_MCU_PEAK_ACCELERATION = 32.0; // steps/s^2

        public const int ACTUAL_MCU_DEFAULT_PEAK_VELOCITY = 500000; // steps/s
        public const int ACTUAL_MCU_DEFAULT_ACCELERATION = 1000; // steps/ms/s
        public const double ACTUAL_MCU_STEPS_PER_DEGREE = 166 + (2.0 / 3);

        public const string ACTUAL_MCU_IP_ADDRESS = "192.168.0.2";
        public const int ACTUAL_MCU_MODBUS_TCP_PORT = 502;
        public const ushort ACTUAL_MCU_READ_INPUT_REGISTER_START_ADDRESS = 0;
        public const ushort ACTUAL_MCU_READ_INPUT_REGISTER_HEARTBEAT_ADDRESS = 9;
        public const ushort ACTUAL_MCU_READ_INPUT_REGISTER_CURRENT_POSITION_ADDRESS = 2;
        public const ushort ACTUAL_MCU_WRITE_REGISTER_START_ADDRESS = 1024;
        public const int ACTUAL_MCU_AZIMUTH_ENCODER_BIT_RESOLUTION = 12;
        public const int ACTUAL_MCU_MOVE_PEAK_VELOCITY_WITH_GEARING = 100000;
        public const ushort ACTUAL_MCU_MOVE_ACCELERATION_WITH_GEARING = 50;
    }
}
