using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControlRoomApplication.Controllers.PLCCommunication.PLCDrivers.MCUManager
{
    public enum MCUCommandType
    {
        JOG,
        RELATIVE_MOVE,
        CONFIGURE,
        CLEAR_LAST_MOVE,
        HOLD_MOVE,
        IMMEDIATE_STOP,
        RESET_ERRORS,
        HOME
    }

    /// <summary>
    /// These are all for the First Word Azimuth (registerData[0]) OR First word Elevation
    /// </summary>
    public enum MCUMoveType : ushort
    {
        // first register bit (registerData[0])
        RELATIVE_MOVE = 0x0002,

        // first register bit (registerData[0])
        CANCEL_MOVE = 0x0003,

        // first register bit (registerData[0])
        CONTROLLED_STOP = 0x0004,

        // first register bit (registerData[0])
        IMMEDIATE_STOP = 0x0010,

        // first register bit (registerData[0])
        HOME = 0x0040,

        // first register bit (registerData[0])
        CLEAR_MCU_ERRORS = 0x0800,

        // first register bit (registerData[0])
        CONFIGURE_MCU = 0x852c
    }

    /// <summary>
    /// This enum lines up for the vast majority of the register data we send over, but there are some exceptions. For example, stop moves only use a couple of the registers
    /// to send their information over, so the this entire list does not line up for them (the first or second word does this). The homing command or the reset errors command also do 
    /// not line up with this list. The majority of moves (we really only use relative and jog moves) do line up with this list.
    /// THESE ARE REALLY ONLY FOR RELATIVE MOVES
    /// </summary>
    enum MCURegPos : int
    {
        ///
        /// AZIMUTH REGISTERS
        /// 
        firstWordAzimuth,
        secondWordAzimuth,
        firstPosAzimuth,
        secondPosAzimuth,
        firstSpeedAzimuth,
        secondSpeedAzimuth,
        firstAccelerationAzimuth,
        secondAccelerationAzimuth,

        ///
        /// NOTE: these registers are never used from the control room, so they very well might not line up with the names I've used below. From the data sheet,
        ///       based on the pattern of the previous registers my best guess is that these correspond to spots registerData[8] & registerData[9]
        ///       ( I think this is better than leaving them blank spots in the enum )
        ///
        firstDecelerationAzimuth,
        secondDecelerationAzimuth,

        ///
        /// ELEVATION REGISTERS
        ///
        firstWordElevation,
        secondWordElevation,
        firstPosElevation,
        secondPosElevation,
        firstSpeedElevation,
        secondSpeedElevation,
        firstAccelerationElevation,
        secondAccelerationElevation,

        ///
        /// same note as above, these are never used by the control room, but for completeness sake I have them listed here
        ///
        firstDecelerationElevation,
        secondDecelerationElevation,
    }
}
