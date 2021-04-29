using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControlRoomApplication.Controllers.PLCCommunication.PLCDrivers.MCUManager
{
    /// <summary>
    /// This will allow us to prioritize movement commands to the MCU. A higher priority movement coming in
    /// will stop the currently-running movement and start the new one. The lower the number, the higher the priority
    /// (with the exception of "None").
    /// </summary>
    public enum MovementPriority
    {
        /// <summary>
        /// This priority is present if one has not yet been initialized, or if no movement is currently running.
        /// </summary>
        None,

        /// <summary>
        /// This is the lowest priority movement, which is reserved only for use with appointments, which are cued
        /// up in <seealso cref="RadioTelescopeControllerManagementThread">RadioTelescopeControllerManagementThread</seealso>.
        /// </summary>
        Appointment,

        /// <summary>
        /// This denotes manual movements that the user would send through scripts or manual az/el movements on the
        /// RT Control Form.
        /// </summary>
        Manual,

        /// <summary>
        /// This denotes a jog movement, which is technically the same priority as Manual, but used so the GeneralStop
        /// can differentiate the two movements.
        /// </summary>
        Jog,

        /// <summary>
        /// This is a movement command for the general stopping of any lower-priority movement commands.
        /// </summary>
        GeneralStop,

        /// <summary>
        /// These are movements that must override all other movements, such as stop commands or limit switch recovery.
        /// </summary>
        Critical
    }
}
