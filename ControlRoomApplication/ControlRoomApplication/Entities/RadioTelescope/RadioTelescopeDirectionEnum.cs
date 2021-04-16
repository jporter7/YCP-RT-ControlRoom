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
        /// The motor is spinning clockwise.
        /// </summary>
        Clockwise = 0x0080,

        /// <summary>
        /// The motor is spinning counter-clockwise.
        /// </summary>
        Counterclockwise = 0x0100
    }
}
