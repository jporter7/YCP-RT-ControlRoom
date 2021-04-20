namespace ControlRoomApplication.Entities
{
    /// <summary>
    /// This will tell us what direction a motor is spinning on the telescope.
    /// </summary>
    public enum RadioTelescopeDirectionEnum
    {
        /// <summary>
        /// The direction has not been specified. This should not be seen in production code.
        /// </summary>
        None,

        /// <summary>
        /// The motor is spinning clockwise (azimuth), or in a negative direction (elevation).
        /// </summary>
        ClockwiseOrNegative = 0x0080,

        /// <summary>
        /// The motor is spinning counter-clockwise (azimuth), or in a positive direction (elevation).
        /// </summary>
        CounterclockwiseOrPositive = 0x0100,

        /// <summary>
        /// The motor is spinning clockwise. This should ONLY be used for homing.
        /// </summary>
        ClockwiseHoming = 0x0020,

        /// <summary>
        /// The motor is spinning counterclockwise. This should ONLY be used for homing.
        /// </summary>
        CounterclockwiseHoming = 0x0040
    }
}
