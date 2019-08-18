using ControlRoomApplication.Constants;
using ControlRoomApplication.Entities;
using Modbus.Data;
using Modbus.Device;
using System;
using System.Collections;
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

        private TcpListener PLCTCPListener;
        private TcpClient MCUTCPClient;
        private ModbusSlave PLC_Modbusserver;
        private ModbusIpMaster MCUModbusMaster;
        private SemaphoreSlim comand_acknoledged = new SemaphoreSlim(0, 1);
        private static ushort[] no_op_cmd = {
            0, 3, 0, 0, 0,
            0, 0, 0, 0, 0,
            0, 3, 0, 0, 0,
            0, 0, 0, 0, 0
        };
        private bool keep_modbus_server_alive=true;
        private bool is_test= false;
        public bool set_is_test(bool val) { is_test = val;return is_test; }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="local_ip"></param>
        /// <param name="MCU_ip"></param>
        /// <param name="MCU_port"></param>
        /// <param name="PLC_port"></param>
        public ProductionPLCDriver(string local_ip,  string MCU_ip, int MCU_port, int PLC_port) : base(local_ip,  MCU_ip, MCU_port, PLC_port)
        {
            //MCUTCPClient = new TcpClient("127.0.0.2", 8080);
            MCUTCPClient = new TcpClient(MCU_ip, MCU_port);
            MCUModbusMaster = ModbusIpMaster.CreateIp(MCUTCPClient);
            
            try
            {
                PLCTCPListener = new TcpListener(new IPEndPoint(IPAddress.Parse(local_ip), PLC_port));
                ClientManagmentThread = new Thread(new ThreadStart(HandleClientManagementThread));
            }
            catch (Exception e)
            {
                if ((e is ArgumentNullException) || (e is ArgumentOutOfRangeException))
                {
                    logger.Info("[AbstractPLCDriver] ERROR: failure creating PLC TCP server or management thread: " + e.ToString());
                    return;
                }
                else { throw e; }// Unexpected exception
            }
            try
            {
                PLCTCPListener.Start(1);
            }
            catch (Exception e)
            {
                if ((e is SocketException) || (e is ArgumentOutOfRangeException) || (e is InvalidOperationException))
                {
                    logger.Info("[AbstractPLCDriver] ERROR: failure starting PLC TCP server: " + e.ToString());
                    return;
                }
            }

        }
        /// <summary>
        /// 
        /// </summary>
        ~ProductionPLCDriver()
        {

        }

        private void set_Local_registers(ushort[] data, ushort starting_adress)
        {
            Console.WriteLine("{0}   dsv  {1} ",data.Length, starting_adress);
            for (int i = 1; i < (data.Length-1); i++)
            {
                PLC_Modbusserver.DataStore.HoldingRegisters[i + starting_adress] = data[i];
                Console.Write(" {0},", PLC_Modbusserver.DataStore.HoldingRegisters[i + starting_adress]);
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
                Console.WriteLine("'{0}' repeated at positions {1} and {2}", groups["word"].Value, groups[0].Index, groups[1].Index);
            }
            // PLC_Modbusserver.DataStore.HoldingRegisters[e.] += 1;
            //throw new NotImplementedException();
        }

        private void Server_Written_to_handler(object sender, DataStoreEventArgs e)
        {
            switch (e.StartAddress)
            {
                case (ushort)PLC_modbus_server_register_mapping.CMD_ACK:
                    {
                       // Console.WriteLine(" data {0} written to 22",PLC_Modbusserver.DataStore.HoldingRegisters[e.StartAddress]);
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
                case (ushort)PLC_modbus_server_register_mapping.AZ_LEFT_LIMIT:
                    {

                        break;
                    }
                case (ushort)PLC_modbus_server_register_mapping.AZ_LEFT_WARNING:
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
                case (ushort)PLC_modbus_server_register_mapping.EL_BOTTOM_LIMIT:
                    {

                        break;
                    }
                case (ushort)PLC_modbus_server_register_mapping.EL_BOTTOM_WARNING:
                    {

                        break;
                    }
                case (ushort)PLC_modbus_server_register_mapping.EL_TOP_WARNING:
                    {

                        break;
                    }
                case (ushort)PLC_modbus_server_register_mapping.EL_TOP_LIMIT:
                    {

                        break;
                    }

            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="adr"></param>
        /// <param name="value"></param>
        public void setregvalue(ushort adr, ushort value)
        {
            PLC_Modbusserver.DataStore.HoldingRegisters[adr] = value;
        }

        /// <summary>
        /// see   ControlRoomApplication.Entities.PLC_modbus_server_register_mapping
        /// for register maping
        /// </summary>
        /// <param name="adr"></param>
        /// <returns></returns>
        public ushort readregval(ushort adr)
        {
            return PLC_Modbusserver.DataStore.HoldingRegisters[adr];
        }
        /// <summary>
        /// runs the modbus server to interface with the plc
        /// </summary>
        protected override void HandleClientManagementThread()
        {
            byte slaveId = 1;
            // create and start the TCP slave
            PLC_Modbusserver = ModbusTcpSlave.CreateTcp(slaveId, PLCTCPListener);
            //coils, inputs, holdingRegisters, inputRegisters
            PLC_Modbusserver.DataStore = DataStoreFactory.CreateDefaultDataStore(0, 0, 256, 0);
            // PLC_Modbusserver.DataStore.SyncRoot.ToString();

            PLC_Modbusserver.ModbusSlaveRequestReceived += new EventHandler<ModbusSlaveRequestEventArgs>(Server_Read_handler);
            PLC_Modbusserver.DataStore.DataStoreWrittenTo += new EventHandler<DataStoreEventArgs>(Server_Written_to_handler);

            PLC_Modbusserver.Listen();

            //PLC_Modbusserver.ListenAsync().GetAwaiter().GetResult();

            // prevent the main thread from exiting
            while (keep_modbus_server_alive)
            {
                Thread.Sleep(100);
            }
            
        }

        public override bool StartAsyncAcceptingClients()
        {
            keep_modbus_server_alive = true;
            try
            {
                ClientManagmentThread.Start();
            }
            catch (Exception e)
            {
                if ((e is ThreadStateException) || (e is OutOfMemoryException))
                {
                    return false;
                }
                else{throw e;}// Unexpected exception
            }
            return true;
        }

        public override bool RequestStopAsyncAcceptingClientsAndJoin()
        {
            keep_modbus_server_alive = false;
            try
            {
                ClientManagmentThread.Join();
            }
            catch (Exception e)
            {
                if ((e is ThreadStateException) || (e is ThreadStartException))
                {
                    Console.WriteLine(e);
                    return false;
                }
                else { throw e; }// Unexpected exception
            }
            return true;
        }

        public override void Bring_down()
        {
            RequestStopAsyncAcceptingClientsAndJoin();
        }


        public override bool Get_interlock_status()
        {
            return Int_to_bool(PLC_Modbusserver.DataStore.HoldingRegisters[(ushort)PLC_modbus_server_register_mapping.Safty_INTERLOCK]);
        }

        public override bool[] Get_Limit_switches()
        {
            return new bool[] {
                Int_to_bool(PLC_Modbusserver.DataStore.HoldingRegisters[(ushort)PLC_modbus_server_register_mapping.AZ_LEFT_LIMIT]),
                Int_to_bool(PLC_Modbusserver.DataStore.HoldingRegisters[(ushort)PLC_modbus_server_register_mapping.AZ_LEFT_WARNING]),
                Int_to_bool(PLC_Modbusserver.DataStore.HoldingRegisters[(ushort)PLC_modbus_server_register_mapping.AZ_RIGHT_WARNING]),
                Int_to_bool(PLC_Modbusserver.DataStore.HoldingRegisters[(ushort)PLC_modbus_server_register_mapping.AZ_RIGHT_LIMIT]),

                Int_to_bool(PLC_Modbusserver.DataStore.HoldingRegisters[(ushort)PLC_modbus_server_register_mapping.EL_BOTTOM_LIMIT]),
                Int_to_bool(PLC_Modbusserver.DataStore.HoldingRegisters[(ushort)PLC_modbus_server_register_mapping.EL_BOTTOM_WARNING]),
                Int_to_bool(PLC_Modbusserver.DataStore.HoldingRegisters[(ushort)PLC_modbus_server_register_mapping.EL_TOP_WARNING]),
                Int_to_bool(PLC_Modbusserver.DataStore.HoldingRegisters[(ushort)PLC_modbus_server_register_mapping.EL_TOP_LIMIT])
            };
        }



        //above PLC modbus server ___below MCU comands






        public async Task<bool> sendmovecomand(int programmedPeakSpeedAZInt, ushort ACCELERATION, int positionTranslationAZ, int positionTranslationEL)
        {
            bool Sucess = true;
            //the mcu registers need to be reset befor a new comand can be set in case the same comand is sent multiple times in a row
            await MCUModbusMaster.WriteMultipleRegistersAsync(1024, no_op_cmd);//write a no-op to the mcu
            Task task = Task.Delay(100);//wait to ensure it is porcessed
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

        public override bool Test_Conection()//TODO: make moar godder
        {
            return true;
        }

        public override Orientation read_Position()
        {
            //*
            ushort[] ClippedInputRegisterInfo = MCUModbusMaster.ReadHoldingRegisters(2, 12);
            return new Orientation(
                ConversionHelper.StepsToDegrees((65536 * ClippedInputRegisterInfo[0]) + ClippedInputRegisterInfo[1], MotorConstants.GEARING_RATIO_AZIMUTH),
                ConversionHelper.StepsToDegrees((65536 * ClippedInputRegisterInfo[10]) + ClippedInputRegisterInfo[11], MotorConstants.GEARING_RATIO_ELEVATION)
            );//*/
            /*

            ushort[] inputs = MCUModbusMaster.ReadHoldingRegisters(0, 20);
            byte[] inbytes = new byte[inputs.Length * 2];
            Buffer.BlockCopy(inputs, 0, inbytes, 0, inputs.Length * 2);

            ushort msb, lsvb;
            msb = inputs[2];
            lsvb = inputs[3];
            int ofsetaz = lsvb + (msb << 16);
            double azpos = (ofsetaz / (double)(20000 * 500)) * 360;
            azpos = ConversionHelper.StepsToDegrees(ofsetaz, MotorConstants.GEARING_RATIO_AZIMUTH);

            msb = inputs[12];
            lsvb = inputs[13];
            int ofsetel = lsvb + (msb << 16);
            double elpos = (ofsetel / (double)(20000 * 50)) * 360;
            elpos = ConversionHelper.StepsToDegrees(ofsetel, MotorConstants.GEARING_RATIO_ELEVATION);
            // Console.WriteLine("attt: Az = " + azpos + ", El = " + elpos + "  ofset : Az = " + ofsetaz + ", El = " + ofsetel);
            return new Orientation(azpos, elpos);
            //*/
            //return new Orientation(BitConverter.ToDouble(inbytes, 4), BitConverter.ToDouble(inbytes, 24));
        }

        public override bool Cancle_move()
        {
            ushort[] data = {
                0, 3, 0, 0, 0, 0, 0, 0, 0, 0,
                0, 3, 0, 0, 0, 0, 0, 0, 0, 0
            };
            MCUModbusMaster.WriteMultipleRegisters(MCUConstants.ACTUAL_MCU_WRITE_REGISTER_START_ADDRESS, data);
            return true;
        }

        public override bool Shutdown_PLC_MCU()
        {
            if (is_test) { return true; }
            throw new NotImplementedException();
        }

        public override bool Calibrate()
        {
            if (is_test) { return true; }
            throw new NotImplementedException();
        }

        public override bool Configure_MCU(int startSpeedDPSAzimuth, int startSpeedDPSElevation, int homeTimeoutSecondsAzimuth, int homeTimeoutSecondsElevation)
        {
            int gearedSpeedAZ = ConversionHelper.DPSToSPS(startSpeedDPSAzimuth, MotorConstants.GEARING_RATIO_AZIMUTH);
            int gearedSpeedEL = ConversionHelper.DPSToSPS(startSpeedDPSElevation, MotorConstants.GEARING_RATIO_ELEVATION);
            if ((gearedSpeedAZ < 1)       || (gearedSpeedEL < 1)       || (homeTimeoutSecondsAzimuth < 0)   || (homeTimeoutSecondsElevation < 0)
             || (gearedSpeedAZ > 1000000) || (gearedSpeedEL > 1000000) || (homeTimeoutSecondsAzimuth > 300) || (homeTimeoutSecondsElevation > 300))
            {
                return false;
            }
            ushort[] data = {   0x8400, 0x0000, (ushort)(gearedSpeedAZ >> 0x0010), (ushort)(gearedSpeedAZ & 0xFFFF), (ushort)homeTimeoutSecondsAzimuth,
                                0x0,    0x0,    0x0,                                 0x0,                            0x0,
                                0x8400, 0x0000, (ushort)(gearedSpeedEL >> 0x0010), (ushort)(gearedSpeedEL & 0xFFFF), (ushort)homeTimeoutSecondsElevation,
                                0x0,    0x0,    0x0,                                0x0,                             0x0
                                };
            //set_multiple_registers( data,  1);
            MCUModbusMaster.WriteMultipleRegisters(MCUConstants.ACTUAL_MCU_WRITE_REGISTER_START_ADDRESS, data);
            return true;
        }

        public override bool Controled_stop(RadioTelescopeAxisEnum axis, bool both)
        {
            ushort[] data;
            if (both)
            {
                data =new ushort[] {
                    0x4, 0x3, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0,
                    0x4, 0x3, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0
                };
                MCUModbusMaster.WriteMultipleRegisters(MCUConstants.ACTUAL_MCU_WRITE_REGISTER_START_ADDRESS, data);
                return true;
            }
            else { throw new NotImplementedException(); }
        }

        public override bool Immediade_stop()
        {
            throw new NotImplementedException();
        }

        public override bool relative_move(int programmedPeakSpeedAZInt, ushort ACCELERATION, int positionTranslationAZ, int positionTranslationEL)
        {
            return sendmovecomand(programmedPeakSpeedAZInt, ACCELERATION, positionTranslationAZ, positionTranslationEL).GetAwaiter().GetResult();   //.ContinueWith(antecedent => { return antecedent.; });
            //return true;
        }

        public override bool Move_to_orientation(Orientation target_orientation, Orientation current_orientation)
        {

            int positionTranslationAZ, positionTranslationEL;
            positionTranslationAZ = ConversionHelper.DegreesToSteps((target_orientation.Azimuth - current_orientation.Azimuth), MotorConstants.GEARING_RATIO_AZIMUTH);
            positionTranslationEL = ConversionHelper.DegreesToSteps((target_orientation.Elevation - current_orientation.Elevation), MotorConstants.GEARING_RATIO_ELEVATION);
            /*
            ushort msb, lsvb;
            msb = inputs[2];
            lsvb = inputs[3];
            int ofsetaz = lsvb + (msb << 16);
            double azpos = (ofsetaz / (double)(20000 * 500)) * 360;

            msb = inputs[12];
            lsvb = inputs[13];
            int ofsetel = lsvb + (msb << 16);
            double elpos = (ofsetel / (double)(20000 * 50)) * 360;
            //*/

            int ObjectivePositionStepsAZ = ConversionHelper.DegreesToSteps(target_orientation.Azimuth, MotorConstants.GEARING_RATIO_AZIMUTH);
            int ObjectivePositionStepsEL = ConversionHelper.DegreesToSteps(target_orientation.Elevation, MotorConstants.GEARING_RATIO_ELEVATION);

            //Orientation CurrentMCUPosition = GetCurrentOrientation();
            int CurrentPositionStepsAZ = ConversionHelper.DegreesToSteps(current_orientation.Azimuth, MotorConstants.GEARING_RATIO_AZIMUTH);
            int CurrentPositionStepsEL = ConversionHelper.DegreesToSteps(current_orientation.Elevation, MotorConstants.GEARING_RATIO_ELEVATION);

            double PositionTranslationDegreesAZ = ConversionHelper.StepsToDegrees(ObjectivePositionStepsAZ - CurrentPositionStepsAZ, MotorConstants.GEARING_RATIO_AZIMUTH);
            double PositionTranslationDegreesEL = ConversionHelper.StepsToDegrees(ObjectivePositionStepsEL - CurrentPositionStepsEL, MotorConstants.GEARING_RATIO_ELEVATION);

            int FixedSpeedDPS = ConversionHelper.DPSToSPS(ConversionHelper.RPMToDPS(0.2), MotorConstants.GEARING_RATIO_ELEVATION);

            //(ObjectivePositionStepsAZ - CurrentPositionStepsAZ), (ObjectivePositionStepsEL - CurrentPositionStepsEL)
            Console.WriteLine("degrees target az "+target_orientation.Azimuth + " el " + target_orientation.Elevation);
            Console.WriteLine("degrees curren az " + current_orientation.Azimuth + " el " + current_orientation.Elevation);
            Console.WriteLine("degrees dist   az " + PositionTranslationDegreesAZ + " el " + PositionTranslationDegreesEL);

            return sendmovecomand(FixedSpeedDPS*20, 50, ConversionHelper.DegreesToSteps(PositionTranslationDegreesAZ, MotorConstants.GEARING_RATIO_AZIMUTH), ConversionHelper.DegreesToSteps(PositionTranslationDegreesEL, MotorConstants.GEARING_RATIO_ELEVATION)).GetAwaiter().GetResult();
            //return ExecuteRelativeMove(FixedSpeedDPS, PositionTranslationDegreesAZ, FixedSpeedDPS, PositionTranslationDegreesEL);
            //throw new NotImplementedException();
        }

        public override bool Start_jog(RadioTelescopeAxisEnum axis, int speed, bool clockwise)
        {
            ushort adress = MCUConstants.ACTUAL_MCU_WRITE_REGISTER_START_ADDRESS, dir;
            if (clockwise)
            {
                dir = 0x0080;
            } else dir = 0x0100;
            //                                         reserved       msb speed            lsb speed  acc  dcc  reserved
            ushort[] data = new ushort[] {dir, 0x0003, 0x0, 0x0, (ushort)(speed >> 16), (ushort)speed, 50, 25, 0x0, 0x0 };// this is a jog comand for a single axis
            RadioTelescopeAxisEnum jogging_axies = Is_jogging();

            switch (axis)
            {
                case RadioTelescopeAxisEnum.AZIMUTH:
                    {
                        break;
                    }
                case RadioTelescopeAxisEnum.ELEVATION:
                    {
                        adress = MCUConstants.ACTUAL_MCU_WRITE_REGISTER_START_ADDRESS + 10;
                        break;
                    }
                case RadioTelescopeAxisEnum.BOTH:
                    {
                        ushort[] data2 = new ushort[data.Length*2];
                        data.CopyTo(data2, 0);
                        data.CopyTo(data2, data.Length);
                        data = data2;
                        break;
                    }
                default:
                    {
                        throw new ArgumentException("Invalid RadioTelescopeAxisEnum value can be AZIMUTH, ELEVATION or BOTH got: "+axis );
                    }
            }
            MCUModbusMaster.WriteMultipleRegisters(adress, data);
            return true;
            //throw new NotImplementedException();
        }

        public RadioTelescopeAxisEnum Is_jogging()//////////
        {
            ushort[] data = MCUModbusMaster.ReadHoldingRegisters(MCUConstants.ACTUAL_MCU_WRITE_REGISTER_START_ADDRESS, 20);
            //word 0 and 10 indicate a jog comand for a specific axis however if the position registers(2,3  && 12,13) have non zero value then its a REGISTRATION MOVE
            if ((data[10] == 0x0100|| data[10] == 0x0080) && (data[0] == 0x0100 || data[0] == 0x0080) && data[2] == 0x0 && data[3] == 0x0 && data[12] == 0x0 && data[13] == 0x0)//both jogging
            {
                return RadioTelescopeAxisEnum.BOTH;
            }
            else if ((data[10] == 0x0100 || data[10] == 0x0080) && data[12] == 0x0 && data[13] == 0x0)//el is jogging
            {
                return RadioTelescopeAxisEnum.ELEVATION;
            }
            else if ((data[0] == 0x0100 || data[0] == 0x0080) && data[2] == 0x0 && data[3] == 0x0)//az is jogging
            {
                return RadioTelescopeAxisEnum.AZIMUTH;
            }
            else return RadioTelescopeAxisEnum.UNKNOWN;
        }

        private bool Int_to_bool(int val)
        {
            Console.WriteLine(val);
            if (val == 0)
            {
                return false;
            }
            else { return true; }
        }


        public override bool[] GET_MCU_Status()
        {
           ushort data= MCUModbusMaster.ReadHoldingRegisters(0,1)[0];
            bool[] target = new bool[16];
            for (int i = 0; i < 16; i++)
            {
                target[i] = ((data >> i) & 1) == 1;
            }
            return target;

        }
    }
}
