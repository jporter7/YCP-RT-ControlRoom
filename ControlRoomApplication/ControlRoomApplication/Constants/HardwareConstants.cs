﻿namespace ControlRoomApplication.Constants
{
    public sealed class HardwareConstants
    {
        public const double NEGLIGIBLE_POSITION_CHANGE_DEGREES = 0.1;
        public const double LIMIT_SWITCH_AZ_THRESHOLD_DEGREES = 2.0;
        public const double LIMIT_SWITCH_EL_THRESHOLD_DEGREES = 2.0;

        public const double SIMULATION_MCU_PEAK_VELOCITY = 22.5; // steps/s
        public const double SIMULATION_MCU_PEAK_ACCELERATION = 32.0; // steps/s^2

        public const double WEATHER_STATION_MAXIMUM_ALLOWABLE_WIND_SPEED_MPH = 50.0;
        public const double SIMULATION_WEATHER_STATION_AVERAGE_WIND_SPEED_MPH = 20.0;
        public const double SIMULATION_WEATHER_STATION_MAXIMUM_ALLOWABLE_WIND_SPEED_MPH_STD_DEV = (WEATHER_STATION_MAXIMUM_ALLOWABLE_WIND_SPEED_MPH - SIMULATION_WEATHER_STATION_AVERAGE_WIND_SPEED_MPH) / 6.0;

    }
}