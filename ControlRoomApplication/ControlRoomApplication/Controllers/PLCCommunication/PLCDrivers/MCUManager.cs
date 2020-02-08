using ControlRoomApplication.Constants;
using ControlRoomApplication.Entities;
using Modbus.Device;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ControlRoomApplication.Controllers {
    class MCUManager {
        private static readonly log4net.ILog logger = log4net.LogManager.GetLogger( System.Reflection.MethodBase.GetCurrentMethod().DeclaringType );
        private static readonly ushort[] MESSAGE_CONTENTS_IMMEDIATE_STOP = new ushort[] {
            0x0010, 0x0003, 0x0000, 0x0000, 0x0000, 0x0000, 0x0000, 0x0000, 0x0000, 0x0000,
            0x0010, 0x0003, 0x0000, 0x0000, 0x0000, 0x0000, 0x0000, 0x0000, 0x0000, 0x0000
        };

        private static readonly ushort[] MESSAGE_CONTENTS_HOLD_MOVE = new ushort[] {
            0x0004, 0x0003, 0x0000, 0x0000, 0x0000, 0x0000, 0x0000, 0x0000, 0x0000, 0x0000,
            0x0004, 0x0003, 0x0000, 0x0000, 0x0000, 0x0000, 0x0000, 0x0000, 0x0000, 0x0000
        };

        private static readonly ushort[] MESSAGE_CONTENTS_CLEAR_MOVE = new ushort[] {
            0x0000, 0x0003, 0x0000, 0x0000, 0x0000, 0x0000, 0x0000, 0x0000, 0x0000, 0x0000,
            0x0000, 0x0003, 0x0000, 0x0000, 0x0000, 0x0000, 0x0000, 0x0000, 0x0000, 0x0000
        };

        private static readonly ushort[] MESSAGE_CONTENTS_RESET_ERRORS = new ushort[] {
            0x0800, 0x0000, 0x0000, 0x0000, 0x0000, 0x0000, 0x0000, 0x0000, 0x0000, 0x0000,
            0x0800, 0x0000, 0x0000, 0x0000, 0x0000, 0x0000, 0x0000, 0x0000, 0x0000, 0x0000
        };
        private long MCU_last_contact = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
        private Thread MCU_Monitor_Thread;
        private int AZStartSpeed = 0;
        private int ELStartSpeed = 0;
        private bool keep_modbus_server_alive = true;
        public ModbusIpMaster MCUModbusMaster;
        private TcpClient MCUTCPClient;

        public MCUManager( string MCU_ip , int MCU_port ) {

            try {
                MCUTCPClient = new TcpClient( MCU_ip , MCU_port );
                MCUModbusMaster = ModbusIpMaster.CreateIp( MCUTCPClient );
                MCU_Monitor_Thread = new Thread( new ThreadStart( MonitorMCU ) );
            } catch(Exception e) {
                if((e is ArgumentNullException) || (e is ArgumentOutOfRangeException)) {
                    logger.Info( "[AbstractPLCDriver] ERROR: failure creating PLC TCP server or management thread: " + e.ToString() );
                    return;
                } else { throw e; }// Unexpected exception
            }
        }

        private void MonitorMCU() {
            int lastMCUHeartbeatBit = 0;
            while(keep_modbus_server_alive) {
                ushort network_status = MCUModbusMaster.ReadHoldingRegisters( (ushort)MCUConstants.MCUOutputRegs.Network_Conectivity , 1 )[0];
                int CurrentHeartBeat = (network_status >> 14) & 1;//this bit changes every 500ms
                if(CurrentHeartBeat != lastMCUHeartbeatBit) {
                    MCU_last_contact = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
                }
                lastMCUHeartbeatBit = CurrentHeartBeat;
                if(((network_status >> 13) & 1) == 1) {
                    logger.Warn( "MCU network disconected, reseting errors" );
                    MCUModbusMaster.WriteMultipleRegistersAsync( MCUConstants.ACTUAL_MCU_WRITE_REGISTER_START_ADDRESS , MESSAGE_CONTENTS_RESET_ERRORS ).Wait();

                }
                Task.Delay( 250 ).Wait();
            }
        }

        public bool StartAsyncAcceptingClients() {
            keep_modbus_server_alive = true;
            try {
                MCU_Monitor_Thread.Start();
            } catch(Exception e) {
                if((e is ThreadStateException) || (e is OutOfMemoryException)) {
                    logger.Error( "failed to start prodi=uction plc and mcu threads err:____    {0}" , e );
                    return false;
                } else { throw e; }// Unexpected exception
            }
            return true;
        }


        public bool RequestStopAsyncAcceptingClientsAndJoin() {
            keep_modbus_server_alive = false;
            try {
                MCU_Monitor_Thread.Join();
            } catch(Exception e) {
                if((e is ThreadStateException) || (e is ThreadStartException)) {
                    logger.Error( e );
                    return false;
                } else { throw e; }// Unexpected exception
            }
            return true;
        }

        public ushort[] readModbusReregs( ushort startadress , ushort length ) {
            return MCUModbusMaster.ReadHoldingRegistersAsync( startadress , length ).Result;
        }

        /// <summary>
        /// clears the previos move comand from mthe PLC, only works for jog moves
        /// </summary>
        /// <returns></returns>
        public bool Cancel_move() {
            MCUModbusMaster.WriteMultipleRegistersAsync( MCUConstants.ACTUAL_MCU_WRITE_REGISTER_START_ADDRESS , MESSAGE_CONTENTS_CLEAR_MOVE ).Wait();
            return true;
        }


        public bool Controled_stop() {
            MCUModbusMaster.WriteMultipleRegistersAsync( MCUConstants.ACTUAL_MCU_WRITE_REGISTER_START_ADDRESS , MESSAGE_CONTENTS_HOLD_MOVE ).Wait();
            return true;
        }

        public bool Immediade_stop() {
            MCUModbusMaster.WriteMultipleRegistersAsync( MCUConstants.ACTUAL_MCU_WRITE_REGISTER_START_ADDRESS , MESSAGE_CONTENTS_IMMEDIATE_STOP ).Wait();
            return true;
        }


        public async Task<bool> Configure_MCU( double startSpeedDPSAzimuth , double startSpeedDPSElevation , int homeTimeoutSecondsAzimuth , int homeTimeoutSecondsElevation ) {
            int gearedSpeedAZ = ConversionHelper.DPSToSPS( startSpeedDPSAzimuth , MotorConstants.GEARING_RATIO_AZIMUTH );
            int gearedSpeedEL = ConversionHelper.DPSToSPS( startSpeedDPSElevation , MotorConstants.GEARING_RATIO_ELEVATION );
            AZStartSpeed = gearedSpeedAZ;
            ELStartSpeed = gearedSpeedEL;
            Console.WriteLine( gearedSpeedAZ.ToString() + " :AZ           EL:" + gearedSpeedEL.ToString() );
            if((gearedSpeedEL < 1) || (gearedSpeedEL > MCUConstants.ACTUAL_MCU_DEFAULT_PEAK_VELOCITY)) {
                throw new ArgumentOutOfRangeException( "startSpeedDPSElevation" , startSpeedDPSElevation ,
                    String.Format( "startSpeedDPSElevation should be between {0} and {1}" ,
                    ConversionHelper.SPSToDPS( 1 , MotorConstants.GEARING_RATIO_ELEVATION ) ,
                    ConversionHelper.SPSToDPS( MCUConstants.ACTUAL_MCU_DEFAULT_PEAK_VELOCITY , MotorConstants.GEARING_RATIO_ELEVATION ) ) );
            }
            if((gearedSpeedAZ < 1) || (gearedSpeedAZ > MCUConstants.ACTUAL_MCU_DEFAULT_PEAK_VELOCITY)) {
                throw new ArgumentOutOfRangeException( "startSpeedDPSAzimuth" , startSpeedDPSAzimuth ,
                    String.Format( "startSpeedDPSAzimuth should be between {0} and {1}" ,
                    ConversionHelper.SPSToDPS( 1 , MotorConstants.GEARING_RATIO_AZIMUTH ) ,
                    ConversionHelper.SPSToDPS( MCUConstants.ACTUAL_MCU_DEFAULT_PEAK_VELOCITY , MotorConstants.GEARING_RATIO_AZIMUTH ) ) );
            }
            if((homeTimeoutSecondsElevation < 0) || (homeTimeoutSecondsElevation > 300)) {
                throw new ArgumentOutOfRangeException( "homeTimeoutSecondsElevation" , homeTimeoutSecondsElevation ,
                    String.Format( "homeTimeoutSecondsElevation should be between {0} and {1}" , 0 , 300 ) );
            }
            if((homeTimeoutSecondsAzimuth < 0) || (homeTimeoutSecondsAzimuth > 300)) {
                throw new ArgumentOutOfRangeException( "homeTimeoutSecondsAzimuth" , homeTimeoutSecondsAzimuth ,
                    String.Format( "homeTimeoutSecondsAzimuth should be between {0} and {1}" , 0 , 300 ) );
            }
            ushort[] data = {   0x842C,  0x0004 , (ushort)(gearedSpeedAZ >> 0x0010), (ushort)(gearedSpeedAZ & 0xFFFF), 0x0,0x0,0x0,0x0,0x0,0x0,
                                //0x0, 0x0, 0x0, 0x0, 0x0,0x0,0x0,0x0,0x0,0x0,
                                0x842C, 0x0004, (ushort)(gearedSpeedEL >> 0x0010), (ushort)(gearedSpeedEL & 0xFFFF), 0x0,0x0,0x0,0x0,0x0,0x0
                                //      0x001C  //limit active high
                                //      0x0004  //limit active low
                                //anf1-anf2-motion-controller-user-manual.pdf page 50
                                };
            Console.WriteLine( "start" );
            MCUModbusMaster.WriteMultipleRegistersAsync( MCUConstants.ACTUAL_MCU_WRITE_REGISTER_START_ADDRESS , MESSAGE_CONTENTS_CLEAR_MOVE ).Wait();
            Task.Delay( 100 ).Wait();
            MCUModbusMaster.WriteMultipleRegistersAsync( MCUConstants.ACTUAL_MCU_WRITE_REGISTER_START_ADDRESS , data ).Wait();
            Task.Delay( 100 ).Wait();
            MCUModbusMaster.WriteMultipleRegistersAsync( MCUConstants.ACTUAL_MCU_WRITE_REGISTER_START_ADDRESS , MESSAGE_CONTENTS_CLEAR_MOVE ).Wait();
            Console.WriteLine( "stop" );
            return true;
        }


        /// <summary>
        /// home a singel axsis of the RT
        /// </summary>
        /// <param name="axis"></param>
        /// <returns></returns>
        public async Task<bool> HomeBothAxyes(bool AZHomeCW, bool ELHomeCW,double RPM ) {
            int EL_Speed = ConversionHelper.DPSToSPS( ConversionHelper.RPMToDPS( RPM ) , MotorConstants.GEARING_RATIO_ELEVATION );
            int AZ_Speed = ConversionHelper.DPSToSPS( ConversionHelper.RPMToDPS( RPM ) , MotorConstants.GEARING_RATIO_AZIMUTH );
            ushort ACCELERATION = 50;
            ushort CWHome = 0x0020;
            ushort CcWHome = 0x0040;

            ushort azHomeDir = CWHome;
            ushort elHomeDir = CWHome;

            ushort elHomeSpeed = 0x0000;//this is the default value
            if(!ELHomeCW) {//if the MCU dosnt expect the home sensor to be high when a home is initiated this will need to be changed
                elHomeDir = CcWHome;
                elHomeSpeed = 0x0040;//if this bit is set the MCU will home at its configured minimum speed
            }

            //set config word to 0x0040 to have the RT home at the minimumum speed
            ushort[] data = {
                azHomeDir , 0x0000      , 0x0000, 0x0000,(ushort)((AZ_Speed & 0xFFFF0000)>>16),(ushort)(AZ_Speed & 0xFFFF), ACCELERATION, ACCELERATION , 0x0000, 0x0000,
                //0x0000     ,0x0000      , 0x0000, 0x0000,0x0000                                ,0x0000                    , 0x0000       ,0x0000        ,0x0000 ,0x0000   
                elHomeDir , elHomeSpeed , 0x0000, 0x0000,(ushort)((EL_Speed & 0xFFFF0000)>>16),(ushort)(EL_Speed & 0xFFFF), ACCELERATION, ACCELERATION , 0x0000, 0x0000
            };
            MCUModbusMaster.WriteMultipleRegistersAsync( 1024 , MESSAGE_CONTENTS_CLEAR_MOVE ).Wait();//write a no-op to the mcu
            Task.Delay( 100 ).Wait();//wait to ensure it is porcessed
            MCUModbusMaster.WriteMultipleRegistersAsync( 1024 , data ).Wait();
            return true;
        }

        public async Task<bool> send_relative_move_comand( int SpeedAZ , int SpeedEL , ushort ACCELERATION , int positionTranslationAZ , int positionTranslationEL ) {
            MCUModbusMaster.WriteMultipleRegistersAsync( 1024 , MESSAGE_CONTENTS_CLEAR_MOVE ).Wait();//write a no-op to the mcu
            Task.Delay( 100 ).Wait();//wait to ensure it is porcessed
            ushort[] data = prepairRelativeMoveData( SpeedAZ , SpeedEL , ACCELERATION , positionTranslationAZ , positionTranslationEL );
            MCUModbusMaster.WriteMultipleRegistersAsync( 1024 , data ).Wait();
            return true;
        }

        private ushort[] prepairRelativeMoveData( int SpeedAZ , int SpeedEL , ushort ACCELERATION , int positionTranslationAZ , int positionTranslationEL ) {
            if(SpeedAZ < AZStartSpeed) {
                throw new ArgumentOutOfRangeException( "SpeedAZ" , SpeedAZ ,
                    String.Format( "SpeedAZ should be grater than {0} which is the stating speed set when configuring the MCU" , AZStartSpeed ) );
            }
            if(SpeedEL < ELStartSpeed) {
                throw new ArgumentOutOfRangeException( "SpeedEL" , SpeedEL ,
                    String.Format( "SpeedAZ should be grater than {0} which is the stating speed set when configuring the MCU" , ELStartSpeed ) );
            }
            ushort[] data = {
                0x0002 , 0x0003, (ushort)((positionTranslationAZ & 0xFFFF0000)>>16),(ushort)(positionTranslationAZ & 0xFFFF),(ushort)((SpeedAZ & 0xFFFF0000)>>16),(ushort)(SpeedAZ & 0xFFFF), ACCELERATION,ACCELERATION ,0,0,
                0x0002 , 0x0003, (ushort)((positionTranslationEL & 0xFFFF0000)>>16),(ushort)(positionTranslationEL & 0xFFFF),(ushort)((SpeedEL & 0xFFFF0000)>>16),(ushort)(SpeedEL & 0xFFFF), ACCELERATION,ACCELERATION ,0,0
            };
            return data;
        }

        public bool Send_Jog_command( double AZspeed , bool AZClockwise , double ELspeed , bool ELClockwise ) {
            ushort adress = MCUConstants.ACTUAL_MCU_WRITE_REGISTER_START_ADDRESS, dir;
            RadioTelescopeAxisEnum axis = RadioTelescopeAxisEnum.AZIMUTH;
            if(AZClockwise) {
                dir = 0x0080;
            } else dir = 0x0100;
            int stepSpeed = ConversionHelper.RPMToSPS( AZspeed , MotorConstants.GEARING_RATIO_AZIMUTH );
            //                                            reserved       msb speed                     lsb speed                   acc                                                       dcc                                                    reserved
            ushort[] data = new ushort[10] { dir , 0x0003 , 0x0 , 0x0 , (ushort)(stepSpeed >> 16) , (ushort)(stepSpeed & 0xffff) , MCUConstants.ACTUAL_MCU_MOVE_ACCELERATION_WITH_GEARING , MCUConstants.ACTUAL_MCU_MOVE_ACCELERATION_WITH_GEARING , 0x0 , 0x0 , };

            ushort[] data2 = new ushort[20];
            // this is a jog comand for a single axis
            // RadioTelescopeAxisEnum jogging_axies = Is_jogging();
            if(stepSpeed > AZStartSpeed) {
                //throw new ArgumentOutOfRangeException( "speed" , AZspeed ,
                //    String.Format( "speed should be grater than {0} which is the stating speed set when configuring the MCU" ,
                //    ConversionHelper.SPSToRPM( AZStartSpeed , MotorConstants.GEARING_RATIO_AZIMUTH ) ) );
                for(int j = 0; j < data.Length; j++) {
                    data2[j] = data[j];
                }
            }


            stepSpeed = ConversionHelper.RPMToSPS( ELspeed , MotorConstants.GEARING_RATIO_ELEVATION );
            if(stepSpeed > ELStartSpeed) {
                //throw new ArgumentOutOfRangeException( "speed" , ELspeed ,
                //    String.Format( "speed should be grater than {0} which is the stating speed set when configuring the MCU" ,
                //    ConversionHelper.SPSToRPM( ELStartSpeed , MotorConstants.GEARING_RATIO_AZIMUTH ) ) );


                for(int j = 0; j < data.Length; j++) {
                    data2[j + 10] = data[j];
                }
                if(ELClockwise) {
                    dir = 0x0080;
                } else dir = 0x0100;

                data2[10] = (ushort)(dir);
                data2[14] = (ushort)(stepSpeed >> 16);
                data2[15] = (ushort)(stepSpeed & 0xffff);
            }


            MCUModbusMaster.WriteMultipleRegistersAsync( adress , data2 ).Wait();
            return true;
        }

        public RadioTelescopeAxisEnum Is_jogging(){
            ushort[] data = MCUModbusMaster.ReadHoldingRegisters( MCUConstants.ACTUAL_MCU_WRITE_REGISTER_START_ADDRESS , 20 );
            //word 0 and 10 indicate a jog comand for a specific axis however if the position registers(2,3  && 12,13) have non zero value then its a REGISTRATION MOVE
            if((data[10] == 0x0100 || data[10] == 0x0080) && (data[0] == 0x0100 || data[0] == 0x0080) && data[2] == 0x0 && data[3] == 0x0 && data[12] == 0x0 && data[13] == 0x0)//both jogging
            {
                return RadioTelescopeAxisEnum.BOTH;
            } else if((data[10] == 0x0100 || data[10] == 0x0080) && data[12] == 0x0 && data[13] == 0x0)//el is jogging
              {
                return RadioTelescopeAxisEnum.ELEVATION;
            } else if((data[0] == 0x0100 || data[0] == 0x0080) && data[2] == 0x0 && data[3] == 0x0)//az is jogging
              {
                return RadioTelescopeAxisEnum.AZIMUTH;
            } else return RadioTelescopeAxisEnum.UNKNOWN;
        }


        public long getLastContact() {
            return MCU_last_contact;
        }


        private bool Int_to_bool( int val ) {
            logger.Info( val );
            if(val == 0) {
                return false;
            } else { return true; }
        }
    }
}
