using ControlRoomApplication.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ControlRoomApplication.Controllers.PLCCommunication.PLCDrivers.MCUManager
{
    /// <summary>
    /// used to keep track of what comand the MCU is running
    /// </summary>
    public class MCUCommand : IDisposable
    {
        /// <summary>
        /// Stores the data that is to be sent to the MCU.
        /// </summary>
        public ushort[] commandData;

        /// <summary>
        /// High-level information about the command's general purpose.
        /// </summary>
        public MCUCommandType CommandType;

        /// <summary>
        /// True when a command has completed. Used to determine when the next move can be sent.
        /// </summary>
        public bool completed = false;

        /// <summary>
        /// this will be set when returnd to the calling function if the move could not be run for some reason
        /// </summary>
        public Exception CommandError;

        /// <summary>
        /// these variables set so that different parts of the MCUManager can calculate how parts of the operation will take
        /// </summary>
        public int AZ_Programed_Speed, EL_Programed_Speed, AZ_ACC = 50, EL_ACC = 50;

        /// <summary>
        /// Determines the direction of motion of the azimuth motor.
        /// </summary>
        public RadioTelescopeDirectionEnum AzimuthDirection;

        /// <summary>
        /// Determines the direction of motion fo the elevation motor.
        /// </summary>
        public RadioTelescopeDirectionEnum ElevationDirection;

        /// <summary>
        /// create a MCU command and record the current time
        /// </summary>
        /// <param name="data"></param>
        /// <param name="CMDType"></param>
        public MCUCommand(ushort[] data, MCUCommandType CMDType)
        {
            CommandType = CMDType;
            commandData = data;
        }

        /// <summary>
        /// Create a command for movement. Only the "data" field is sent to the MCU; the rest of the fields
        /// exist for internal tracking and estimations within the MCU manager.
        /// </summary>
        /// <param name="data"></param>
        /// <param name="CMDType"></param>
        /// <param name="azDir"></param>
        /// <param name="elDir"></param>
        /// <param name="AZSpeed"></param>
        /// <param name="ElSpeed"></param>
        public MCUCommand(ushort[] data, MCUCommandType CMDType, RadioTelescopeDirectionEnum azDir, RadioTelescopeDirectionEnum elDir, int AZSpeed, int ElSpeed)
        {
            CommandType = CMDType;
            commandData = data;
            AzimuthDirection = azDir;
            ElevationDirection = elDir;
            AZ_Programed_Speed = AZSpeed;
            EL_Programed_Speed = ElSpeed;
        }

        public CancellationTokenSource timeout;

        public void Dispose()
        {
            try
            {
                timeout.Dispose();
            }
            catch { }
        }
    }
}
