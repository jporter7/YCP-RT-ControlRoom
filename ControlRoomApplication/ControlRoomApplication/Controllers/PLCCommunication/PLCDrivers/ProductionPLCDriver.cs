using Modbus.Data;
using Modbus.Device;
using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace ControlRoomApplication.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    public class ProductionPLCDriver : AbstractPLCDriver
    {
        private static readonly log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private TcpClient MCUTCPClient;
        private ModbusIpMaster MCUModbusMaster;

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
                MCUTCPClient = new TcpClient(ipLocal, 502);///////
                MCUModbusMaster = ModbusIpMaster.CreateIp(MCUTCPClient);
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
            if (MCUTCPClient != null)
            {
                MCUTCPClient.Close();
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="adr"></param>
        /// <param name="numreg"></param>
        /// <returns></returns>
        public ushort[] readregisters(ushort adr, ushort numreg)
        {
            return MCUModbusMaster.ReadInputRegisters(adr, numreg);
            /*
            try
            {
                //adr = 0;
                //numreg = 1;
                //MCUModbusMaster.WriteSingleRegister(adr, 38);
                //Console.WriteLine(MCUModbusMaster.ReadCoils(adr, numreg));
                //Console.WriteLine(MCUModbusMaster.ReadInputs(adr, numreg));
                //return MCUModbusMaster.ReadHoldingRegisters(adr, numreg);
                //return MCUModbusMaster.ReadInputRegisters(adr, numreg);
                return MCUModbusMaster.ReadHoldingRegisters(adr, numreg);
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message +"\n :: at line 55 \n");
                return null;
            }
            //*/
        }


        /// <summary>
        /// this can be used as a heart beat tracker as the plc will poll the ctrl room every ~100 ms
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ReadReghandler(object sender, ModbusSlaveRequestEventArgs e)
        {
            //Console.WriteLine(e.ToString());
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
