namespace ControlRoomApplication.Constants
{
    public sealed class MCUConstants
    {
        public const double SIMULATION_MCU_PEAK_VELOCITY = 22.5;     // steps/s
        public const double SIMULATION_MCU_PEAK_ACCELERATION = 32.0; // steps/s^2

        public const string MCU_IP_ADDRESS = "192.168.0.50";
        public const int MCU_MODBUS_TCP_PORT = 502;

        public const ushort MCU_READ_INPUT_REGISTER_START_ADDRESS = 0;
        public const ushort MCU_READ_INPUT_REGISTER_HEARTBEAT_ADDRESS = 9;
        public const ushort MCU_READ_INPUT_REGISTER_CURRENT_POSITION_ADDRESS = 2;
        public const ushort MCU_WRITE_REGISTER_START_ADDRESS = 1024;
        
        public const ushort MCU_DEFAULT_ACCELERATION = 50; // steps/ms/s
    }
}
