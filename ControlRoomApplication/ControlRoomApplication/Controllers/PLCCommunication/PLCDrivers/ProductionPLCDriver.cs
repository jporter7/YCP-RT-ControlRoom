using Modbus.Data;
using Modbus.Device;
using System;
using System.Net;
using System.Net.Sockets;
using System.Text.RegularExpressions;
using System.Threading;

namespace ControlRoomApplication.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    public class ProductionPLCDriver : AbstractPLCDriver
    {
        private static readonly log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private TcpClient PLCTCPClient;
        public ModbusIpMaster PLCModbusMaster;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ipLocal"></param>
        /// <param name="portLocal"></param>
        public ProductionPLCDriver(string ipLocal, int portLocal) : base(ipLocal, portLocal)
        {

            //IPAddress ipAddress;
            // ipAddress= IPAddress.Parse(ipLocal);
            //IPEndPoint localEndPoint = new IPEndPoint(ipAddress, 1600);
            try
            {
                PLCTCPClient = new TcpClient(ipLocal, 502);///////
                PLCModbusMaster = ModbusIpMaster.CreateIp(PLCTCPClient);
            }
            catch(Exception err)//unknown err
            {
                if (err is SocketException )
                {
                    Console.WriteLine("socket exception " + err.Message);
                }
                else
                {//uncaugt exception
                    throw err;
                }
            }
            Console.WriteLine("\n new client \n");
        }
        /// <summary>
        /// 
        /// </summary>
        ~ProductionPLCDriver()
        {
            if (PLCTCPClient != null)
            {
                PLCTCPClient.Close();
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="header"></param>
        public void PrintReadInputRegsiterContents(string header)
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





        /// <summary>
        /// 
        /// </summary>
        /// <param name="adr"></param>
        /// <param name="numreg"></param>
        /// <returns></returns>
        public ushort[] readregisters(ushort adr, ushort numreg)
        {
            /*
            bool[] bools= PLCModbusMaster.ReadCoils(adr, numreg);
            ushort[] temp2=new ushort[numreg];
            for (int t=0;t< bools.Length; t++)
            {
                if (bools[t])
                {
                    temp2[t] = (ushort)1;
                }else { temp2[t] = (ushort)0; }
            }
            return temp2;




            ushort[] Arr = (new ushort[1]);
            Arr[0] = 54;
            PLCModbusMaster.WriteMultipleRegisters(adr, Arr);
            return new ushort[2];
//*/

            return PLCModbusMaster.ReadHoldingRegisters(adr, numreg);

            /*
            try
            {
                //adr = 0;
                //numreg = 1;
                //MCUModbusMaster.WriteSingleRegister(adr, 38);
                //Console.WriteLine(MCUModbusMaster.ReadCoils(adr, numreg));
                //Console.WriteLine(MCUModbusMaster.ReadInputs(adr, numreg));
                //return PLCModbusMaster.ReadHoldingRegisters(adr, numreg);
                //return PLCModbusMaster.ReadInputRegisters(adr, numreg);
                return PLCModbusMaster.ReadHoldingRegisters(adr, numreg);
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message +"\n :: at line 55 \n");
                return null;
            }
            //*/
        }

        public void wiritemasterregs(ushort adr, ushort[] data)
        {
            PLCModbusMaster.WriteMultipleRegisters(adr,data);
        }


        /// <summary>
        /// this can be used as a heart beat tracker as the plc will poll the ctrl room every ~100 ms
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ReadReghandler(object sender, ModbusSlaveRequestEventArgs e)
        {
            Console.WriteLine(e.Message);

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

        private void WriteReghandler(object sender, DataStoreEventArgs e)
        {
            Console.Write("\n modbus request adr: " + e.StartAddress + "\n");
            //Console.WriteLine(Modbusserver.DataStore.HoldingRegisters.Count);
            System.Collections.Generic.IEnumerator<ushort> regs = Modbusserver.DataStore.HoldingRegisters.GetEnumerator();
            while (regs.MoveNext())
            {
                Console.Write(", " + regs.Current);
            }
            Modbusserver.DataStore.HoldingRegisters[e.StartAddress] += 1;
            //Modbusserver.DataStore.HoldingRegisters.Count;
            //throw new NotImplementedException();
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
        /// 
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
            Modbusserver.DataStore = DataStoreFactory.CreateDefaultDataStore(0, 0, 10, 0);
            // Modbusserver.DataStore.SyncRoot.ToString();

            Modbusserver.ModbusSlaveRequestReceived += new EventHandler<ModbusSlaveRequestEventArgs>(ReadReghandler);
            Modbusserver.DataStore.DataStoreWrittenTo += new EventHandler<DataStoreEventArgs>(WriteReghandler);

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
