#define MODBUS_TCP_COMMUNICATION_HANDLER_DEBUG

using System;
using System.Net.Sockets;
using Modbus.Device;

namespace ControlRoomApplication.Controllers
{
    public abstract class BaseModbusTCPCommunicationHandler : AbstractHardwareCommunicationHandler
    {
        protected ModbusIpMaster GenericModbusTCPMaster;

        private string CommsIPAddress;
        private int CommsPort;

        protected BaseModbusTCPCommunicationHandler(string ip, int port) : base()
        {
            CommsIPAddress = ip;
            CommsPort = port;
        }

        protected void PrintInputRegisterContents(string header, ushort startAddress, ushort numValues)
        {
#if MODBUS_TCP_COMMUNICATION_HANDLER_DEBUG
            System.Threading.Thread.Sleep(50);

            ushort[] inputRegisters = GenericModbusTCPMaster.ReadInputRegisters(startAddress, numValues);
            Console.WriteLine(header + ":");
            foreach (ushort us in inputRegisters)
            {
                string usString = Convert.ToString(us, 2);
                usString = new string('0', 16 - usString.Length) + usString;
                usString = usString.Insert(4, " ");
                usString = usString.Insert(9, " ");
                usString = usString.Insert(14, " ");

                Console.WriteLine('\t'.ToString() + usString);
            }
#endif
        }

        public override bool StartHandler()
        {
            try
            {
                GenericModbusTCPMaster = ModbusIpMaster.CreateIp(new TcpClient(CommsIPAddress, CommsPort));
            }
            catch (Exception e)
            {
                if ((e is ArgumentNullException) || (e is ArgumentOutOfRangeException) || (e is SocketException))
                {
                    return false;
                }
                else
                {
                    // Unexpected exception
                    throw e;
                }
            }

            return true;
        }

        public override bool DisposeHandler()
        {
            if (GenericModbusTCPMaster != null)
            {
                GenericModbusTCPMaster.Dispose();
            }

            return true;
        }
    }
}
