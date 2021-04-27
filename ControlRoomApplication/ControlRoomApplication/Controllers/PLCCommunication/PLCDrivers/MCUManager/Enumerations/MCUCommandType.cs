using ControlRoomApplication.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControlRoomApplication.Controllers.PLCCommunication.PLCDrivers.MCUManager
{
    /// <summary>
    /// These are all for the First Word Azimuth (registerData[0]) OR First word Elevation
    /// </summary>
    public enum MCUCommandType : ushort
    {
        /// <summary>
        /// First register bit (registerData[0])
        /// </summary>
        RelativeMove = 0x0002,
        
        /// <summary>
        /// First register bit (registerData[0])
        /// </summary>
        CancelMove = 0x0003,
        
        /// <summary>
        /// First register bit (registerData[0])
        /// </summary>
        ControlledStop = 0x0004,
        
        /// <summary>
        /// first register bit (registerData[0])
        /// </summary>
        ImmediateStop = 0x0010,
        
        /// <summary>
        /// first register bit (registerData[0])
        /// </summary>
        Home = 0x0040,
        
        /// <summary>
        /// first register bit (registerData[0])
        /// </summary>
        ClearErrors = 0x0800,
        
        /// <summary>
        /// first register bit (registerData[0])
        /// </summary>
        Configure = 0x852c,

        /// <summary>
        /// A jog movement. If a movement is a jog, the first word must be the direction the telescope
        /// is moving. <seealso cref="RadioTelescopeDirectionEnum"/>
        /// </summary>
        Jog,

        /// <summary>
        /// A movement used to stop the telescope. This is to be used only with the "Hold Move" complete message, 
        /// found in <seealso cref="MCUMessages"/>
        /// </summary>
        HoldMove,

        /// <summary>
        /// This is empty data. If we want a register to be empty, this is used.
        /// </summary>
        EmptyData = 0x0000
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
