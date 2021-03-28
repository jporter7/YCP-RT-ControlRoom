using ControlRoomApplication.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControlRoomApplication.Entities.Exceptions {
    /// <summary>
    /// raised when the MCU is exicuting a move that cannot be overriden
    /// </summary>
    public class MCUMoveOverlapException : Exception {
        /// <summary>
        /// this is the type of move that the MCU is currently running
        /// </summary>
        public MCUCommandType RunningMoveType;
    }
}
