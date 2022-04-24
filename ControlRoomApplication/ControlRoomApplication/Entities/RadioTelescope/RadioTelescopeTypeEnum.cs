namespace ControlRoomApplication.Entities
{
    /// <summary>
    /// This will tell us whether the radio telescope has a slip ring or if it has hard stops.
    /// </summary>
    public enum RadioTelescopeTypeEnum
    {
        /// <summary>
        /// This telescope has a slip ring, so it will have no limit switches on the azimuth axis.
        /// </summary>
        SLIP_RING,

        /// <summary>
        /// This telescope has hard stops, so it will have limit switches on the azimuth axis.
        /// </summary>
        HARD_STOPS,

        /// <summary>
        /// The telescope type has not been defiend. This should not be seen in production code.
        /// </summary>
        NONE
    }
}