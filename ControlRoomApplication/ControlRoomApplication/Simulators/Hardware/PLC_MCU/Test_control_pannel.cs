﻿using ControlRoomApplication.Entities;
using Modbus.Data;
using Modbus.Device;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ControlRoomApplication.Simulators.Hardware.PLC_MCU
{
    class Test_control_pannel
    {
        private TcpListener MCU_TCPListener;
        private ModbusSlave MCU_Modbusserver;

        private TcpClient PLCTCPClient;
        public ModbusIpMaster PLCModbusMaster;

        private Thread MCU_emulator_thread;
        private Thread PLC_emulator_thread;

        private string PLC_ip;
        private int PLC_port;

        private bool runsimulator = true;

        private int distAZ, distEL, currentAZ, currentEL;

        public Test_control_pannel(string PLC_ip, string MCU_ip, int MCU_port, int PLC_port)
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
            catch (Exception e)
            {
                if ((e is ArgumentNullException) || (e is ArgumentOutOfRangeException))
                {
                    Console.WriteLine(e);
                    return;
                }
                else { throw e; }// Unexpected exception
            }
            try
            {
                MCU_TCPListener.Start(1);
            }
            catch (Exception e)
            {
                if ((e is SocketException) || (e is ArgumentOutOfRangeException) || (e is InvalidOperationException))
                {
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
            PLC_emulator_thread.Join();
            MCU_emulator_thread.Join();
        }

        private void Run_PLC_emulator_thread()
        {
            while (runsimulator)
            {
                try
                {
                    PLCTCPClient = new TcpClient(this.PLC_ip, this.PLC_port);
                    PLCModbusMaster = ModbusIpMaster.CreateIp(PLCTCPClient);
                }
                catch
                {//no server setup on control room yet 
                    Thread.Sleep(10);
                }
                Console.WriteLine("________________PLC sim running");
                PLCModbusMaster.WriteMultipleRegisters((ushort)PLC_modbus_server_register_mapping.Safty_INTERLOCK-1,new ushort[] { 12});
                while (runsimulator)
                {
                    Thread.Sleep(50);
                }
            }

        }

        private void Server_Written_to_handler(object sender, DataStoreEventArgs e)
        {
            MCU_Modbusserver.DataStore.HoldingRegisters[1] = (ushort)(MCU_Modbusserver.DataStore.HoldingRegisters[1] & 0xff7f);
            //Console.WriteLine("plcdriver data writen 1 reg "+ e.Data.B[0]+" start adr "+ e.StartAddress);
            ushort[] data = new ushort[e.Data.B.Count];
            for (int i = 0; i < e.Data.B.Count; i++)
            {
                data[i] = e.Data.B[i];
            }
            handleCMD(data);
        }

        private void Run_MCU_server_thread()
        {
            byte slaveId = 1;
            // create and start the TCP slave
            MCU_Modbusserver = ModbusTcpSlave.CreateTcp(slaveId, MCU_TCPListener);
            //coils, inputs, holdingRegisters, inputRegisters
            MCU_Modbusserver.DataStore = DataStoreFactory.CreateDefaultDataStore(0, 0, 1054, 0);
            // PLC_Modbusserver.DataStore.SyncRoot.ToString();

            //MCU_Modbusserver.ModbusSlaveRequestReceived += new EventHandler<ModbusSlaveRequestEventArgs>(Server_Read_handler);
            MCU_Modbusserver.DataStore.DataStoreWrittenTo += new EventHandler<DataStoreEventArgs>(Server_Written_to_handler);

            MCU_Modbusserver.Listen();

            //PLC_Modbusserver.ListenAsync().GetAwaiter().GetResult();

            // prevent the main thread from exiting
            ushort[] previos_out, current_out;
            previos_out = Copy_modbus_registers(1025, 20);
            while (runsimulator)
            {
                Thread.Sleep(10);
            }


        }

        private bool handleCMD(ushort[] data)
        {
            string outstr = " inreg";
            for (int v = 0; v < data.Length; v++)
            {
                outstr += Convert.ToString(data[v], 16).PadLeft(5) + ",";
            }
           // Console.WriteLine(outstr);
            if (data[1] == 0x0403)//move cmd
            {
                distAZ = (data[6] << 16) + data[7];
                distEL = (data[12] << 16) + data[13];
                // MCU_Modbusserver.DataStore.HoldingRegisters[3] = data[6];
                // MCU_Modbusserver.DataStore.HoldingRegisters[4] = data[7];
                // MCU_Modbusserver.DataStore.HoldingRegisters[13] = data[12];
                // MCU_Modbusserver.DataStore.HoldingRegisters[14] = data[13];
                //Console.WriteLine("AZ_step2 {0,10} EL_step2 {1,10}", addaz, addel);
                Console.WriteLine("AZ_22 {0,16} EL_22 {1,16}", (MCU_Modbusserver.DataStore.HoldingRegisters[3] << 16) + MCU_Modbusserver.DataStore.HoldingRegisters[4], (MCU_Modbusserver.DataStore.HoldingRegisters[13] << 16) + MCU_Modbusserver.DataStore.HoldingRegisters[14]);

                currentAZ += distAZ;
                currentEL += distEL;

                MCU_Modbusserver.DataStore.HoldingRegisters[3] = (ushort)((currentAZ & 0xffff0000) >> 16);
                MCU_Modbusserver.DataStore.HoldingRegisters[4] = (ushort)(currentAZ & 0xffff);
               MCU_Modbusserver.DataStore.HoldingRegisters[13] = (ushort)((currentEL & 0xffff0000) >> 16);
               MCU_Modbusserver.DataStore.HoldingRegisters[14] = (ushort)(currentEL & 0xffff);
                MCU_Modbusserver.DataStore.HoldingRegisters[1] = (ushort)(MCU_Modbusserver.DataStore.HoldingRegisters[1] | 0x0080);

                Console.WriteLine("AZ_finni1 {0,10} EL_finni1 {1,10}", (MCU_Modbusserver.DataStore.HoldingRegisters[3]<<16)+ MCU_Modbusserver.DataStore.HoldingRegisters[4], (MCU_Modbusserver.DataStore.HoldingRegisters[13] << 16) + MCU_Modbusserver.DataStore.HoldingRegisters[14]);

                return true;
            } else if(data[0] == 0x0002 || data[0] == 0x0002) {//move cmd
                MCU_Modbusserver.DataStore.HoldingRegisters[1] = (ushort)(MCU_Modbusserver.DataStore.HoldingRegisters[1] & 0xff7f);
               // AZ_speed = ((data[4] << 16) + data[5]) / 20;
                //AZ_speed = ((data[14] << 16) + data[15]) / 20;
               // acc = data[6];
                distAZ = (data[2] << 16) + data[3];
                distEL = (data[12] << 16) + data[13];
                currentAZ += distAZ;
                currentEL += distEL;

                MCU_Modbusserver.DataStore.HoldingRegisters[3] = (ushort)((currentAZ & 0xffff0000) >> 16);
                MCU_Modbusserver.DataStore.HoldingRegisters[4] = (ushort)(currentAZ & 0xffff);
                MCU_Modbusserver.DataStore.HoldingRegisters[13] = (ushort)((currentEL & 0xffff0000) >> 16);
                MCU_Modbusserver.DataStore.HoldingRegisters[14] = (ushort)(currentEL & 0xffff);
                MCU_Modbusserver.DataStore.HoldingRegisters[1] = (ushort)(MCU_Modbusserver.DataStore.HoldingRegisters[1] | 0x0080);

                return true;
            }
            return false;
        }

        private ushort[] Copy_modbus_registers(int start_index, int length)
        {
            ushort[] data = new ushort[length];
            for (int i = 0; i < length; i++)
            {
                data[i] = MCU_Modbusserver.DataStore.HoldingRegisters[i + start_index];
            }
            return data;
        }
    }
}
