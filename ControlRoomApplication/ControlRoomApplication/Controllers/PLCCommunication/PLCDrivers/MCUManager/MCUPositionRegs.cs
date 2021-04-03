using ControlRoomApplication.Constants;
using Modbus.Device;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControlRoomApplication.Controllers.PLCCommunication.PLCDrivers.MCUManager
{
    public class MCUPositonRegs : MCUpositonStore
    {
        private ModbusIpMaster MCUModbusMaster;

        public MCUPositonRegs(ModbusIpMaster _MCUModbusMaster) : base()
        {
            MCUModbusMaster = _MCUModbusMaster;
        }

        public async Task update()
        {
            ushort[] data = TryReadRegs(0, 20).GetAwaiter().GetResult();
            AZ_Steps = (data[(ushort)MCUConstants.MCUOutputRegs.AZ_Current_Position_MSW] << 16) + data[(ushort)MCUConstants.MCUOutputRegs.AZ_Current_Position_LSW];
            EL_Steps = -((data[(ushort)MCUConstants.MCUOutputRegs.EL_Current_Position_MSW] << 16) + data[(ushort)MCUConstants.MCUOutputRegs.EL_Current_Position_LSW]);
            AZ_Encoder = (data[(ushort)MCUConstants.MCUOutputRegs.AZ_MTR_Encoder_Pos_MSW] << 16) + data[(ushort)MCUConstants.MCUOutputRegs.AZ_MTR_Encoder_Pos_LSW];
            EL_Encoder = -((data[(ushort)MCUConstants.MCUOutputRegs.EL_MTR_Encoder_Pos_MSW] << 16) + data[(ushort)MCUConstants.MCUOutputRegs.EL_MTR_Encoder_Pos_LSW]);
            return;
        }

        public async Task<MCUpositonStore> updateAndReturnDif(MCUpositonStore previous)
        {
            ushort[] data = TryReadRegs(0, 20).GetAwaiter().GetResult();
            AZ_Steps = (data[(ushort)MCUConstants.MCUOutputRegs.AZ_Current_Position_MSW] << 16) + data[(ushort)MCUConstants.MCUOutputRegs.AZ_Current_Position_LSW];
            EL_Steps = -((data[(ushort)MCUConstants.MCUOutputRegs.EL_Current_Position_MSW] << 16) + data[(ushort)MCUConstants.MCUOutputRegs.EL_Current_Position_LSW]);
            AZ_Encoder = (data[(ushort)MCUConstants.MCUOutputRegs.AZ_MTR_Encoder_Pos_MSW] << 16) + data[(ushort)MCUConstants.MCUOutputRegs.AZ_MTR_Encoder_Pos_LSW];
            EL_Encoder = -((data[(ushort)MCUConstants.MCUOutputRegs.EL_MTR_Encoder_Pos_MSW] << 16) + data[(ushort)MCUConstants.MCUOutputRegs.EL_MTR_Encoder_Pos_LSW]);
            MCUpositonStore dif = new MCUpositonStore((this as MCUpositonStore), previous);
            return dif;
        }

        private async Task<ushort[]> TryReadRegs(ushort address, ushort Length)
        {
            try
            {
                return MCUModbusMaster.ReadHoldingRegistersAsync(address, Length).GetAwaiter().GetResult();
            }
            catch
            {
                return new ushort[Length];
            }
        }
    }
}
