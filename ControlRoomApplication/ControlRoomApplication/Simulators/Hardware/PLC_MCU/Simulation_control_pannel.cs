using ControlRoomApplication.Entities;
using Modbus.Data;
using Modbus.Device;
using System;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace ControlRoomApplication.Simulators.Hardware.PLC_MCU
{
    class Simulation_control_pannel
    {
        private TcpListener MCU_TCPListener;
        private ModbusSlave MCU_Modbusserver;

        private TcpClient PLCTCPClient;
        private ModbusIpMaster PLCModbusMaster;

        private Thread MCU_emulator_thread;
        private Thread PLC_emulator_thread;

        private string PLC_ip;
        private int PLC_port;

        private bool runsimulator = true, mooving = false,jogging=false;

        private int  acc, distAZ, distEL, currentAZ, currentEL, AZ_speed, EL_speed;

        public Simulation_control_pannel(string PLC_ip, string MCU_ip, int MCU_port, int PLC_port)
        {
            // PLCTCPClient = new TcpClient(PLC_ip, PLC_port);
            // PLCModbusMaster = ModbusIpMaster.CreateIp(PLCTCPClient);
            this.PLC_ip = PLC_ip;
            this.PLC_port = PLC_port;

            try
            {
                PLC_emulator_thread = new Thread(new ThreadStart(Run_PLC_emulator_thread));
                //MCU_TCPListener = new TcpListener(new IPEndPoint(IPAddress.Parse("127.0.0.2"), 8080));
                //Console.WriteLine(MCU_ip,)
                MCU_TCPListener = new TcpListener(new IPEndPoint(IPAddress.Parse(MCU_ip), MCU_port));
                MCU_emulator_thread = new Thread(new ThreadStart(Run_MCU_server_thread));
            }
            catch (Exception e){
                if ((e is ArgumentNullException) || (e is ArgumentOutOfRangeException)){
                    Console.WriteLine(e);
                    return;
                }
                else { throw e; }// Unexpected exception
            }
            try{
                MCU_TCPListener.Start(1);
            }catch (Exception e){
                if ((e is SocketException) || (e is ArgumentOutOfRangeException) || (e is InvalidOperationException)){
                    Console.WriteLine(e);
                    return;
                }
            }
            runsimulator = true;
            PLC_emulator_thread.Start();
            MCU_emulator_thread.Start();
        }

        public void Bring_down()
        {
            runsimulator = false;
            PLC_emulator_thread.Join(100);
            MCU_emulator_thread.Join();
        }

        private void Run_PLC_emulator_thread(){
            while (runsimulator){
                try{
                    PLCTCPClient = new TcpClient(this.PLC_ip, this.PLC_port);
                    PLCModbusMaster = ModbusIpMaster.CreateIp(PLCTCPClient);
                }
                catch {//no server setup on control room yet 
                    Console.WriteLine("________________PLC sim awaiting control room");
                    Thread.Sleep(1000);
                }
                Console.WriteLine("________________PLC sim running");
                PLCModbusMaster.WriteMultipleRegisters( (ushort)PLC_modbus_server_register_mapping.Safty_INTERLOCK-1 ,new ushort[] {1} );
                while (runsimulator){
                    Thread.Sleep(50);
                }
            }
        }



        private void Run_MCU_server_thread(){
            byte slaveId = 1;
            // create and start the TCP slave
            MCU_Modbusserver = ModbusTcpSlave.CreateTcp(slaveId, MCU_TCPListener);
            //coils, inputs, holdingRegisters, inputRegisters
            MCU_Modbusserver.DataStore = DataStoreFactory.CreateDefaultDataStore(0, 0, 1054, 0);
            // PLC_Modbusserver.DataStore.SyncRoot.ToString();

            //MCU_Modbusserver.ModbusSlaveRequestReceived += new EventHandler<ModbusSlaveRequestEventArgs>(Server_Read_handler);
            //MCU_Modbusserver.DataStore.DataStoreWrittenTo += new EventHandler<DataStoreEventArgs>(Server_Written_to_handler);

            MCU_Modbusserver.Listen();

            // prevent the main thread from exiting
            ushort[] previos_out,current_out;
            previos_out = Copy_modbus_registers(1025, 20);
            while (runsimulator){
                current_out = Copy_modbus_registers(1025, 20);
                if(!current_out.SequenceEqual(previos_out)){
                    handleCMD(current_out);
                    //Console.WriteLine("data changed");
                }
                if (mooving){
                    if (distAZ != 0 || distEL != 0)
                    {
                        int travAZ = (distAZ < -AZ_speed) ? -AZ_speed : (distAZ > AZ_speed) ? AZ_speed : distAZ;
                        int travEL = (distEL < -EL_speed) ? -EL_speed : (distEL > EL_speed) ? EL_speed : distEL;
                        move( travAZ , travEL );
                    }
                    else
                    {
                        mooving = false;
                        MCU_Modbusserver.DataStore.HoldingRegisters[1] = (ushort)(MCU_Modbusserver.DataStore.HoldingRegisters[1] | 0x0080);
                    }
                }
                if(jogging) {
                    move( AZ_speed , EL_speed );
                }
                previos_out = current_out;
                Thread.Sleep(50);
            }

            
        }

        private bool move(int travAZ , int travEL ){
            distAZ -= travAZ;
            distEL -= travEL;
            currentAZ += travAZ;
            currentEL += travEL;
         //   Console.WriteLine("offset: az" + currentAZ + " el " + currentEL);
            MCU_Modbusserver.DataStore.HoldingRegisters[3]= (ushort)(currentAZ>>16);
            MCU_Modbusserver.DataStore.HoldingRegisters[4] = (ushort)(currentAZ);
            MCU_Modbusserver.DataStore.HoldingRegisters[13] = (ushort)(currentEL >> 16);
            MCU_Modbusserver.DataStore.HoldingRegisters[14] = (ushort)(currentEL);
            //string outstr = "outreg";
          //  for (int v = 0; v < 20; v++)
          //  {
         //       outstr += Convert.ToString(MCU_Modbusserver.DataStore.HoldingRegisters[v], 16).PadLeft(5) + ",";
          //  }
           // Console.WriteLine(outstr);
            return true;
        }


        private bool handleCMD( ushort[] data ) {
            string outstr = " inreg";
            for(int v = 0; v < data.Length; v++) {
                outstr += Convert.ToString( data[v] , 16 ).PadLeft( 5 ) + ",";
            }
            Console.WriteLine( outstr );
            jogging = false;
            if(data[1] == 0x0403) {//move cmd
                mooving = true;
                MCU_Modbusserver.DataStore.HoldingRegisters[1] = (ushort)(MCU_Modbusserver.DataStore.HoldingRegisters[1] & 0xff7f);
                AZ_speed = (data[2] << 16) + data[3];
                AZ_speed /= 5;
                EL_speed = AZ_speed;
                acc = data[4];
                distAZ = (data[6] << 16) + data[7];
                distEL = (data[12] << 16) + data[13];
                return true;
            } else if(data[0] == 0x0080 || data[0] == 0x0100 || data[10] == 0x0080 || data[10] == 0x0100) {
                jogging = true;
                MCU_Modbusserver.DataStore.HoldingRegisters[1] = (ushort)(MCU_Modbusserver.DataStore.HoldingRegisters[1] & 0xff7f);
                if(data[0] == 0x0080) {
                    AZ_speed = ((data[4] << 16) + data[5]) / 20;
                }
                else if(data[0] == 0x0100) {
                    AZ_speed = -((data[4] << 16) + data[5]) / 20;
                } else {
                    AZ_speed = 0;
                }
                if(data[10] == 0x0080) {
                    EL_speed = ((data[14] << 16) + data[15]) / 20;
                }
                else if(data[10] == 0x0100) {
                    EL_speed = -((data[14] << 16) + data[15]) / 20;
                } else {
                    EL_speed = 0;
                }
                return true;
            } else if(data[0] == 0x0002 || data[0] == 0x0002) {//move cmd
                mooving = true;
                MCU_Modbusserver.DataStore.HoldingRegisters[1] = (ushort)(MCU_Modbusserver.DataStore.HoldingRegisters[1] & 0xff7f);
                AZ_speed = ((data[4] << 16) + data[5]) / 5;
                EL_speed = ((data[14] << 16) + data[15]) / 5;
                acc = data[6];
                distAZ = (data[2] << 16) + data[3];
                distEL = (data[12] << 16) + data[13];
                return true;
            }
            return false;
        }


        private ushort[] Copy_modbus_registers(int start_index,int length)
        {
            ushort[] data = new ushort[length];
            for(int i = 0; i < length; i++)
            {
                data[i] = MCU_Modbusserver.DataStore.HoldingRegisters[i+start_index];
            }
            return data;
        }


    }
}
