using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControlRoomApplication.Controllers.PLCCommunication.PLCDrivers.MCUManager.Enumerations
{
    /// <summary>
    /// This is returned after a movement is completed, denoting whether it was successful or a failure.
    /// </summary>
    public enum MovementResult
    {
        /// <summary>
        /// Result has not yet been assigned.
        /// </summary>
        None,

        /// <summary>
        /// The movement was successful and the position is correct.
        /// </summary>
        Success,

        /// <summary>
        /// A limit switch was hit during the movement, so it was cancelled.
        /// </summary>
        LimitSwitchHit,

        /// <summary>
        /// The movement took too long to complete, so it timed out.
        /// </summary>
        TimedOut,

        /// <summary>
        /// The movement completed, but the position is not correct.
        /// </summary>
        IncorrectPosition,

        /// <summary>
        /// The movement was interrupted by either a higher-priority movement, or sensor data requested a stop.
        /// </summary>
        Interrupted,

        /// <summary>
        /// The movement was ended early because one or more MCU error bits were set.
        /// </summary>
        McuErrorBitSet,

        /// <summary>
        /// The movement will not be sent to the MCU because a part of the command was invalid.
        /// </summary>
        ValidationError,

        /// <summary>
        /// If you try to run a movement while one with equal or higher priority is running, this will be returned.
        /// </summary>
        AlreadyMoving,

        /// <summary>
        /// If the sensors are communicating unsafe data, blocks the incoming move.
        /// </summary>
        SensorsNotSafe
    }
}
