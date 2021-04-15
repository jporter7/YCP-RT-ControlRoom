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
        /// stores the data that is to be sent to the mcu
        /// </summary>
        public ushort[] commandData;
        /// <summary>
        /// high level information about the comands general purpose
        /// </summary>
        public MCUCommandType CommandType;
        /// <summary>
        /// true when comand has completed, used to determine when the next move can be sent
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
        /// these are set to determine the direction of motion
        /// </summary>
        public bool AZ_CW, EL_CW;

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
        /// creat a comand for movement
        /// </summary>
        /// <param name="data"></param>
        /// <param name="CMDType"></param>
        /// <param name="AZCW"></param>
        /// <param name="ELCW"></param>
        /// <param name="AZSpeed"></param>
        /// <param name="ElSpeed"></param>
        public MCUCommand(ushort[] data, MCUCommandType CMDType, bool AZCW, bool ELCW, int AZSpeed, int ElSpeed)
        {
            CommandType = CMDType;
            commandData = data;
            AZ_CW = AZCW;
            EL_CW = ELCW;
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
