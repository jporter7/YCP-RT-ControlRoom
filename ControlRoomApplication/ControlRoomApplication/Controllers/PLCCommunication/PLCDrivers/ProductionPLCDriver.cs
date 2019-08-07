using ControlRoomApplication.Entities;
using Modbus.Data;
using Modbus.Device;
using System;
using System.Net;
using System.Net.Sockets;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace ControlRoomApplication.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    public class ProductionPLCDriver : AbstractPLCDriver
    {
        private static readonly log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private TcpClient MCUTCPClient;
        public ModbusIpMaster MCUModbusMaster;
        private SemaphoreSlim comand_acknoledged = new SemaphoreSlim(0, 1);
        private static ushort[] no_op_cmd = {
            0, 3, 0, 0, 0,
            0, 0, 0, 0, 0,
            0, 3, 0, 0, 0,
            0, 0, 0, 0, 0
        };

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ipLocal"></param>
        /// <param name="portLocal"></param>
        public ProductionPLCDriver(string ipLocal, int portLocal) : base(ipLocal, portLocal)
        {
            MCUTCPClient = new TcpClient("192.168.0.50", 502);
            MCUModbusMaster = ModbusIpMaster.CreateIp(MCUTCPClient);
        }
        /// <summary>
        /// 
        /// </summary>
        ~ProductionPLCDriver()
        {

        }



        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public bool SendResetErrorsCommand()
        {
            return true;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public bool SendHoldMoveCommand()
        {
            return true;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public bool SendImmediateStopCommand()
        {
            return true;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public bool SendEmptyMoveCommand()
        {
            return true;
        }

        public bool configure_muc(int gearedSpeedAZ, int gearedSpeedEL, ushort homeTimeoutSecondsAzimuth, ushort homeTimeoutSecondsElevation)
        {
            ushort[] data = {   0x8400, 0x0000, (ushort)(gearedSpeedEL >> 0x0010), (ushort)(gearedSpeedEL & 0xFFFF), homeTimeoutSecondsElevation,
                                0x0,    0x0,    0x0,                                 0x0,                            0x0,
                                0x8400, 0x0000, (ushort)(gearedSpeedAZ >> 0x0010), (ushort)(gearedSpeedAZ & 0xFFFF), homeTimeoutSecondsAzimuth,
                                0x0,    0x0,    0x0,                                0x0,                             0x0
                                };
            //set_multiple_registers( data,  1);
            MCUModbusMaster.WriteMultipleRegisters(1024, data);
            return true;
        }

        public async Task<bool> sendmovecomand(int programmedPeakSpeedAZInt, ushort ACCELERATION, int positionTranslationAZ, int positionTranslationEL)
        {
            bool Sucess = true;
            //the mcu registers need to be reset befor a new comand can be set in case the same comand is sent multiple times in a row
            //set_multiple_registers(no_op_cmd, 0);
            // wait for the plc to write to register 21 we defined as a comand acknoledged register
            //Sucess = await comand_acknoledged.WaitAsync(4000);
            //if (!Sucess) { return Sucess; }


            await MCUModbusMaster.WriteMultipleRegistersAsync(1024, no_op_cmd);//write a no-op to the mcu
            Task task = Task.Delay(20);//wait to ensure it is porcessed
            await task;
            //this is a linearly interpolated relative move
            ushort[] data = {0, 0x0403,
                            (ushort)(programmedPeakSpeedAZInt >> 0x10), (ushort)(programmedPeakSpeedAZInt & 0xFFFF), ACCELERATION, ACCELERATION,
                            (ushort)((positionTranslationAZ & 0xFFFF0000)>>16), (ushort)(positionTranslationAZ & 0xFFFF),
                            0, 0, 0, 0,
                            (ushort)((positionTranslationEL & 0xFFFF0000)>>16) , (ushort)(positionTranslationEL & 0xFFFF),
                            0, 0, 0, 0, 0, 0
            };
            //set_multiple_registers(data, 1);
            //Sucess = await comand_acknoledged.WaitAsync(1000);
            await MCUModbusMaster.WriteMultipleRegistersAsync(1024, data);

            return Sucess;
        }

        private void set_multiple_registers(ushort[] data, ushort starting_adress)
        {
            Console.WriteLine("{0}   dsv  {1} ",data.Length, starting_adress);
            for (int i = 1; i < (data.Length-1); i++)
            {
                Modbusserver.DataStore.HoldingRegisters[i + starting_adress] = data[i];
                Console.Write(" {0},", Modbusserver.DataStore.HoldingRegisters[i + starting_adress]);
            }
        }

        /// <summary>
        /// this can be used as a heart beat tracker as the plc will poll the ctrl room every ~100 ms
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Server_Read_handler(object sender, ModbusSlaveRequestEventArgs e)
        {
           // Console.WriteLine(e.Message);
            return;
            Regex rx = new Regex(@"\b(?:Read )([0-9]+)(?:.+)(?:address )([0-9]+)\b", RegexOptions.Compiled | RegexOptions.IgnoreCase);
            MatchCollection matches = rx.Matches(e.Message.ToString());
            foreach (Match match in matches)
            {
                GroupCollection groups = match.Groups;
                Console.WriteLine("'{0}' repeated at positions {1} and {2}",
                                  groups["word"].Value,
                                  groups[0].Index,
                                  groups[1].Index);
            }
            // Modbusserver.DataStore.HoldingRegisters[e.] += 1;
            //throw new NotImplementedException();
        }

        private void Server_Written_to_handler(object sender, DataStoreEventArgs e)
        {
            //Console.Write("\n modbus request adr: " + e.StartAddress + "\n");
            //Console.WriteLine(Modbusserver.DataStore.HoldingRegisters.Count);
            System.Collections.Generic.IEnumerator<ushort> regs = Modbusserver.DataStore.HoldingRegisters.GetEnumerator();

            //Modbusserver.DataStore.HoldingRegisters[e.StartAddress] += 1;

            //
           // Console.WriteLine("current regs" );
            while (regs.MoveNext())
            {
                if (regs.Current!=0)
                {
                    //Console.Write(" {0},", regs.Current);
                }
                //Console.Write(", " + regs.Current);
            }
            //Console.Write("\n");
            if (e.Data.B.Count != 1)
            {
                Console.WriteLine("plcdriver data writen 1 reg", e.Data.B[0]);

            }
            //for (int j=1;j< Modbusserver.DataStore.HoldingRegisters.Count-1; j++)
            //{
            //    Modbusserver.DataStore.HoldingRegisters[j]++;
            //}
            switch (e.StartAddress)
            {
                case (ushort)PLC_modbus_server_register_mapping.CMD_ACK:
                    {
                       // Console.WriteLine(" data {0} written to 22",Modbusserver.DataStore.HoldingRegisters[e.StartAddress]);
                        try
                        {
                            //comand_acknoledged.Release();
                        }
                        catch (Exception err)
                        {
                            Console.WriteLine(err);
                        }
                        break;

                    }
                case (ushort)PLC_modbus_server_register_mapping.AZ_LEFT_WARNING:
                    {

                        break;
                    }
                case (ushort)PLC_modbus_server_register_mapping.AZ_LEFT_LIMIT:
                    {

                        break;
                    }
                case (ushort)PLC_modbus_server_register_mapping.AZ_RIGHT_WARNING:
                    {

                        break;
                    }
                case (ushort)PLC_modbus_server_register_mapping.AZ_RIGHT_LIMIT:
                    {

                        break;
                    }
                case (ushort)PLC_modbus_server_register_mapping.EL_LEFT_WARNING:
                    {

                        break;
                    }
                case (ushort)PLC_modbus_server_register_mapping.EL_LEFT_LIMIT:
                    {

                        break;
                    }
                case (ushort)PLC_modbus_server_register_mapping.EL_RIGHT_WARNING:
                    {

                        break;
                    }
                case (ushort)PLC_modbus_server_register_mapping.EL_RIGHT_LIMIT:
                    {

                        break;
                    }

            }


            /*
            for (int i =0;i< e.Data.B.Count; i++)
            {
                ushort adr = (ushort)(e.StartAddress + (ushort)i);
                switch (adr)
                {
                    case : 
                }
            }//*/

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="adr"></param>
        /// <param name="value"></param>
        public void setregvalue(ushort adr, ushort value)
        {
            Modbusserver.DataStore.HoldingRegisters[adr] = value;
        }

        /// <summary>
        /// see   ControlRoomApplication.Entities.PLC_modbus_server_register_mapping
        /// for register maping
        /// </summary>
        /// <param name="adr"></param>
        /// <returns></returns>
        public ushort readregval(ushort adr)
        {
            return Modbusserver.DataStore.HoldingRegisters[adr];
        }
        /// <summary>
        /// 
        /// </summary>
        protected override void HandleClientManagementThread()
        {
            byte slaveId = 1;
            // create and start the TCP slave
            Modbusserver = ModbusTcpSlave.CreateTcp(slaveId, PLCTCPListener);
            //coils, inputs, holdingRegisters, inputRegisters
            Modbusserver.DataStore = DataStoreFactory.CreateDefaultDataStore(0, 0, 256, 0);
            // Modbusserver.DataStore.SyncRoot.ToString();

            Modbusserver.ModbusSlaveRequestReceived += new EventHandler<ModbusSlaveRequestEventArgs>(Server_Read_handler);
            Modbusserver.DataStore.DataStoreWrittenTo += new EventHandler<DataStoreEventArgs>(Server_Written_to_handler);

            Modbusserver.Listen();

            //Modbusserver.ListenAsync().GetAwaiter().GetResult();
            
            // prevent the main thread from exiting
            Thread.Sleep(Timeout.Infinite);
        }





        /// <summary>
        /// 
        /// </summary>
        /// <param name="ActiveClientStream"></param>
        /// <param name="query"></param>
        /// <returns></returns>
        protected override bool ProcessRequest(NetworkStream ActiveClientStream, byte[] query)
        {
            throw new NotImplementedException();
        }
    }
}
