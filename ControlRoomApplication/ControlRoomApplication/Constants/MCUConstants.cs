namespace ControlRoomApplication.Constants {
    public sealed class MCUConstants {
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



        public enum MCUStutusBits : int {
            CW_Motion = 0,
            CCW_Motion = 1,
            Hold_State = 2,
            Axis_Stopped = 3,
            At_Home = 4,
            Move_Accelerating = 5,
            Move_Decelerating = 7,
            Move_Complete = 8,
            Home_Invalid_Error = 9,
            Profile_Invalid = 10,
            Input_Error = 11,
            Command_Error = 12,
            Configuration_Error = 13,
            Axis_Enabled = 14,
            Axis_Configuration_Mode = 15,

            Capture_Input_Active = 16,
            External_Input = 17,
            Home_Input = 18,
            CW_Limit = 19,
            CCW_Limit = 20,
            No_Fault_Output_State = 21,
            On_Line_Output_State = 22,
            Move_Complete_Output_State = 23,
            Running_Blend_Move = 24,
            Blend_Move_Acknowledge = 25,
            NonZero_Registration_Distance = 26,
            /// <summary>
            /// only valid for axis one
            /// </summary>
            Running_Interpolated_Move = 27,
            Backplane_Home_Proximity = 28,
            Encoder_Follower_Mode = 29,
            Reserved1 = 30,
            reserved2 = 31,
        }
    }
}

