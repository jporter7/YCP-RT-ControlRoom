using ControlRoomApplication.Constants;
using Modbus.Device;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ControlRoomApplication.Controllers.PLCCommunication.PLCDrivers.MCUManager;

namespace ControlRoomApplication.Controllers.PLCCommunication.PLCDrivers.MCUManager
{
    public class MCUPositonRegs : MCUPositonStore
    {
        public MCUPositonRegs() : base()
        {

        }

        public void update(ushort[] data)
        {
            AzSteps = (data[(ushort)MCUConstants.MCUOutputRegs.AZ_Current_Position_MSW] << 16) + data[(ushort)MCUConstants.MCUOutputRegs.AZ_Current_Position_LSW];
            AzEncoder = (data[(ushort)MCUConstants.MCUOutputRegs.AZ_MTR_Encoder_Pos_MSW] << 16) + data[(ushort)MCUConstants.MCUOutputRegs.AZ_MTR_Encoder_Pos_LSW];

            ElSteps = -((data[(ushort)MCUConstants.MCUOutputRegs.EL_Current_Position_MSW] << 16) + data[(ushort)MCUConstants.MCUOutputRegs.EL_Current_Position_LSW]);
            ElEncoder = -((data[(ushort)MCUConstants.MCUOutputRegs.EL_MTR_Encoder_Pos_MSW] << 16) + data[(ushort)MCUConstants.MCUOutputRegs.EL_MTR_Encoder_Pos_LSW]);
        }
    }
}
