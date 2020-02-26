using ControlRoomApplication.Constants;
using ControlRoomApplication.Controllers.PLCCommunication;
using ControlRoomApplication.Entities;
using ControlRoomApplication.Entities.Configuration;
using ControlRoomApplication.Simulators.Hardware;
using Modbus.Device;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ControlRoomApplication.Controllers {
    //TODO: this would be a fairly large amount of work but, when i wrote the class i assumed that the MCU would only ever get comands that affect both axsis at the same time
    // the only place right now that only affects a single axsis is the single axsis jog, but there could be more in the future
    // so the best thing to do would be to split up the AZ and EL components of MCUcomand and then have 2 inststances for running command one for AZ one for EL
    /// <summary>
    /// used to keep track of what comand the MCU is running
    /// </summary>
    public class MCUcomand : IDisposable{
        /// <summary>
        /// similar to priority to keep certian moves from overriding others, 0 is highest
        /// </summary>
        /// <remarks>
        /// if 2 move with same priority conflict the one that is incoming will override the exsisting move
        /// 0 is intended for emergency types of moves like software and hardware stops
        /// 2 is user scheduald moves lie manual jogs,
        /// 4 is normal moves like those comming from appointments
        /// </remarks>
        public int Move_Priority;
        /// <summary>
        /// stores the data that is to be sent to the mcu
        /// </summary>
        public ushort[] comandData;
        /// <summary>
        /// high level information about the comands general purpose
        /// </summary>
        public MCUcomandType ComandType;
        /// <summary>
        /// time at which the comand was sent to the MCU
        /// </summary>
        public DateTime issued;
        /// <summary>
        /// true when comand has completed, used to determine when the next move can be sent
        /// </summary>
        public bool completed = false;
        /// <summary>
        /// this will be set when returnd to the calling function if the move could not be run for some reason
        /// </summary>
        public Exception ComandError;
        /// <summary>
        /// these variables set so that different parts of the MCUManager can calculate how parts of the operation will take
        /// </summary>
        public int AZ_Programed_Speed,EL_Programed_Speed,AZ_ACC = 50,EL_ACC = 50;

        /// <summary>
        /// these are set to determine the direction of motion
        /// </summary>
        public bool AZ_CW, EL_CW;

        /// <summary>
        /// create a MCU command and record the current time
        /// </summary>
        /// <param name="data"></param>
        /// <param name="CMDType"></param>
        public MCUcomand( ushort[] data, MCUcomandType CMDType, int priority ) {
            ComandType = CMDType;
            comandData = data;
            issued = DateTime.Now;
            Move_Priority = priority;
        }

        /// <summary>
        /// creat a comand for movement
        /// </summary>
        /// <param name="data"></param>
        /// <param name="CMDType"></param>
        /// <param name="AZCW"></param>
        /// <param name="ELCW"></param>
        /// <param name="AZSpeed"></param>
        /// <param name="ElSpeed"></param>
        public MCUcomand( ushort[] data , MCUcomandType CMDType, int priority , bool AZCW,bool ELCW,int AZSpeed,int ElSpeed ) {
            ComandType = CMDType;
            comandData = data;
            issued = DateTime.Now;
            AZ_CW = AZCW;
            EL_CW = ELCW;
            AZ_Programed_Speed = AZSpeed;
            EL_Programed_Speed = ElSpeed;
            Move_Priority = priority;
        }

        public CancellationTokenSource timeout;
        public void Dispose() {
            try {
                timeout.Dispose();
            } catch { }
        }
    }
    public enum MCUcomandType {
        JOG,
        RELETIVE_MOVE,
        CONFIGURE,
        CLEAR_LAST_MOVE,
        HOLD_MOVE,
        IMIDEAT_STOP,
        RESET_ERRORS,
        HOME
    }

    public class MCUpositonRegs : MCUpositonStore {
        private ModbusIpMaster MCUModbusMaster;
        public MCUpositonRegs( ModbusIpMaster _MCUModbusMaster ):base() {
            MCUModbusMaster = _MCUModbusMaster;
        }
        public async Task update() {
            ushort[] data = TryReadRegs( 0 , 20 ).GetAwaiter().GetResult();
            AZ_Steps = (data[(ushort)MCUConstants.MCUOutputRegs.AZ_Current_Position_MSW] << 16) + data[(ushort)MCUConstants.MCUOutputRegs.AZ_Current_Position_LSW];
            EL_Steps = -((data[(ushort)MCUConstants.MCUOutputRegs.EL_Current_Position_MSW] << 16) + data[(ushort)MCUConstants.MCUOutputRegs.EL_Current_Position_LSW]);
            AZ_Encoder = (data[(ushort)MCUConstants.MCUOutputRegs.AZ_MTR_Encoder_Pos_MSW] << 16) + data[(ushort)MCUConstants.MCUOutputRegs.AZ_MTR_Encoder_Pos_LSW];
            EL_Encoder = -((data[(ushort)MCUConstants.MCUOutputRegs.EL_MTR_Encoder_Pos_MSW] << 16) + data[(ushort)MCUConstants.MCUOutputRegs.EL_MTR_Encoder_Pos_LSW]);
            return;
        }
        public async Task<MCUpositonStore> updateAndReturnDif( MCUpositonStore previous ) {
            ushort[] data = TryReadRegs( 0 , 20 ).GetAwaiter().GetResult();
            AZ_Steps = (data[(ushort)MCUConstants.MCUOutputRegs.AZ_Current_Position_MSW] << 16) + data[(ushort)MCUConstants.MCUOutputRegs.AZ_Current_Position_LSW];
            EL_Steps = -((data[(ushort)MCUConstants.MCUOutputRegs.EL_Current_Position_MSW] << 16) + data[(ushort)MCUConstants.MCUOutputRegs.EL_Current_Position_LSW]);
            AZ_Encoder = (data[(ushort)MCUConstants.MCUOutputRegs.AZ_MTR_Encoder_Pos_MSW] << 16) + data[(ushort)MCUConstants.MCUOutputRegs.AZ_MTR_Encoder_Pos_LSW];
            EL_Encoder = -((data[(ushort)MCUConstants.MCUOutputRegs.EL_MTR_Encoder_Pos_MSW] << 16) + data[(ushort)MCUConstants.MCUOutputRegs.EL_MTR_Encoder_Pos_LSW]);
            MCUpositonStore dif = new MCUpositonStore( (this as MCUpositonStore) , previous);
            return dif;
        }
        private async Task<ushort[]> TryReadRegs( ushort address , ushort Length ) {
            try {
                return MCUModbusMaster.ReadHoldingRegistersAsync( address , Length ).GetAwaiter().GetResult();
            } catch {
                return new ushort[Length];
            }
        }
    }
    public class MCUpositonStore {
        public int AZ_Steps, EL_Steps;
        public int AZ_Encoder, EL_Encoder;
        public MCUpositonStore() {

        }

        public MCUpositonStore(MCUpositonRegs mCUpositon) {
            this.AZ_Encoder = mCUpositon.AZ_Encoder;
            this.AZ_Steps = mCUpositon.AZ_Steps;
            this.EL_Encoder = mCUpositon.EL_Encoder;
            this.EL_Steps = mCUpositon.EL_Steps;
        }
        public MCUpositonStore(MCUpositonStore current, MCUpositonStore previous) {
            this.AZ_Steps = previous.AZ_Steps - current.AZ_Steps;
            this.EL_Steps = previous.EL_Steps - current.EL_Steps;
            this.AZ_Encoder = previous.AZ_Encoder - current.AZ_Encoder;
            this.EL_Encoder = previous.EL_Encoder - current.EL_Encoder;
        }

        public void SUM(MCUpositonStore current, MCUpositonStore previous) {
            this.AZ_Steps += current.AZ_Steps - previous.AZ_Steps;
            this.EL_Steps += current.EL_Steps - previous.EL_Steps;
            this.AZ_Encoder += current.AZ_Encoder - previous.AZ_Encoder;
            this.EL_Encoder += current.EL_Encoder - previous.EL_Encoder;
        }

        public void SUMAbsolute(MCUpositonStore current, MCUpositonStore previous) {
            this.AZ_Steps += Math.Abs(current.AZ_Steps - previous.AZ_Steps);
            this.EL_Steps += Math.Abs(current.EL_Steps - previous.EL_Steps);
            this.AZ_Encoder += Math.Abs(current.AZ_Encoder - previous.AZ_Encoder);
            this.EL_Encoder += Math.Abs(current.EL_Encoder - previous.EL_Encoder);
        }
    }

    public class FixedSizedQueue<T> : ConcurrentQueue<T> {
        private readonly object syncObject = new object();

        public int Size { get; private set; }

        public FixedSizedQueue( int size ) {
            Size = size;
        }

        public new void Enqueue( T obj ) {
            base.Enqueue( obj );
            lock(syncObject) {
                while(base.Count > Size) {
                    T outObj;
                    base.TryDequeue( out outObj );
                }
            }
        }
        public MCUpositonStore GetAbsolutePosChange() {
            if(typeof(T)==typeof( MCUpositonStore )) {
                var en = base.GetEnumerator();
                MCUpositonStore x, y,sum=new MCUpositonStore();
                try {
                    en.MoveNext();
                    x = en.Current as MCUpositonStore;
                    while(en.MoveNext()) {
                        y = en.Current as MCUpositonStore;
                        sum.SUMAbsolute( y , x );
                        x = y;
                    }
                } catch (Exception err){
                    Console.WriteLine( err );
                }
                return sum;
            }else return new MCUpositonStore();
        }
    }

    public class MCUManager {
        private static readonly log4net.ILog logger = log4net.LogManager.GetLogger( System.Reflection.MethodBase.GetCurrentMethod().DeclaringType );
        /// <summary>
        /// if more errors than this value are thrown in a row and this class cant resolve them subsiquent attempts to send moves to the MCU will throw exception
        /// </summary>
        private static int MaxConscErrors = 5;
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
        private Thread SoftwareStopThread;
        private int AZStartSpeed = 0;
        private int ELStartSpeed = 0;
        private bool keep_modbus_server_alive = true;
        public ModbusIpMaster MCUModbusMaster;
        private TcpClient MCUTCPClient;
        public MCUpositonRegs mCUpositon;
        private MCUConfigurationAxys Current_AZConfiguration;
        private MCUConfigurationAxys Current_ELConfiguration;
        /// <summary>
        /// this value should not be changed from outside the MCU class
        /// </summary>
        public MCUcomand RunningCommand= new MCUcomand(new ushort[20],MCUcomandType.CLEAR_LAST_MOVE,99) { completed = true };
        private MCUcomand PreviousCommand = new MCUcomand( new ushort[20] , MCUcomandType.CLEAR_LAST_MOVE,99 ) { completed = true };
        private int consecutiveErrors = 0;
        private int consecutiveSucsefullMoves = 0;
        private int MCU_port;
        private string MCU_ip;
        private DateTime lastConnectAttempt = DateTime.Now ;
        private bool initialConnect = true;
        private bool SoftWareStopEnabled = false;

        public MCUManager( string _MCU_ip , int _MCU_port ) {
            MCU_port = _MCU_port;
            MCU_ip = _MCU_ip;
            try {
                lastConnectAttempt = DateTime.Now;
                MCUTCPClient = new TcpClient( MCU_ip , MCU_port );
                MCUModbusMaster = ModbusIpMaster.CreateIp( MCUTCPClient );
                mCUpositon = new MCUpositonRegs( MCUModbusMaster );
                MCU_Monitor_Thread = new Thread( new ThreadStart( MonitorMCU ) ) { Name = "MCU Monitor Thread" };
                SoftwareStopThread = new Thread( new ThreadStart( SoftwareStopper ) ) { Name = "softwre stop thread" };
            } catch(Exception e) {
                if((e is ArgumentNullException) || (e is ArgumentOutOfRangeException)) {
                    logger.Error( "[AbstractPLCDriver] ERROR: failure creating PLC TCP server or management thread: " + e.ToString() );
                    return;
                } else { throw e; }// Unexpected exception
            }
        }

        private void attemptConnect() {
            try {
                lastConnectAttempt = DateTime.Now;
                MCUTCPClient = new TcpClient( MCU_ip , MCU_port );
                MCUModbusMaster = ModbusIpMaster.CreateIp( MCUTCPClient );
                mCUpositon = new MCUpositonRegs( MCUModbusMaster );
            } catch(Exception e) {
                logger.Error( "[AbstractPLCDriver] ERROR: failure creating PLC TCP server or management thread: " + e.ToString() );
                return;
            }
        }


        private async Task<ushort[]> TryReadRegs(ushort address, ushort Length ) {
            try {
                return MCUModbusMaster.ReadHoldingRegistersAsync( address , Length ).GetAwaiter().GetResult();
            } catch (InvalidOperationException err){
                if((DateTime.Now - lastConnectAttempt) > TimeSpan.FromSeconds(5)) {
                    attemptConnect();
                }
                return new ushort[Length];
            }catch(Exception err2) {
                throw err2;
            }
        }

        private async Task TryWriteRegs( ushort address , ushort[] data ) {
            try {
                MCUModbusMaster.WriteMultipleRegistersAsync( address , data ).GetAwaiter().GetResult();
                return;
            } catch(InvalidOperationException err) {
                if((DateTime.Now - lastConnectAttempt) > TimeSpan.FromSeconds( 5 )) {
                    attemptConnect();
                }
                return ;
            } catch(Exception err2) {
                throw err2;
            }
        }

        /// <summary>
        /// this thread will read the heartbeat bit in the MCU status to determine if the MCU is still allive
        /// </summary>
        private void MonitorMCU() {
            int lastMCUHeartbeatBit = 0;
            while(keep_modbus_server_alive) {
                ushort network_status = TryReadRegs( (ushort)MCUConstants.MCUOutputRegs.Network_Conectivity , 1 ).GetAwaiter().GetResult()[0];
                int CurrentHeartBeat = (network_status >> 14) & 1;//this bit changes every 500ms
                if(CurrentHeartBeat != lastMCUHeartbeatBit) {
                    MCU_last_contact = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
                }
                lastMCUHeartbeatBit = CurrentHeartBeat;
                if(((network_status >> 13) & 1) == 1) {
                    logger.Warn( "MCU network disconected, reseting errors" );
                    checkForAndResetErrors();
                    //Send_Generic_Command_And_Track( new MCUcomand( MESSAGE_CONTENTS_RESET_ERRORS , MCUcomandType.RESET_ERRORS ) ).GetAwaiter().GetResult();
                }
                Task.Delay( 250 ).Wait();
            }
        }

        /// <summary>
        /// this thread will check if the telescope is about to move outeide of the 
        /// </summary>
        private void SoftwareStopper() {
            while(keep_modbus_server_alive) {
                try {
                    if(SoftWareStopEnabled) {
                        var current = read_Position();
                        var Distance = estimateDistanceToStop( RunningCommand );
                        var expected = new Orientation( current.Azimuth + Distance.Azimuth , current.Elevation + Distance.Elevation );
                        if((expected.Azimuth > 360 || expected.Azimuth < 0) && Math.Abs( Distance.Azimuth ) > 0.1) {
                            Controled_stop(0);
                        }
                        if((expected.Elevation > 90 || expected.Elevation < 0) && Math.Abs( Distance.Elevation ) > 0.1) {
                            Controled_stop(0);
                        }
                    }
                } catch { }
                Task.Delay( 20 ).Wait();
            }
        }

        /// <summary>
        /// starts the MCU monitor thread
        /// </summary>
        /// <returns></returns>
        public bool StartAsyncAcceptingClients() {
            keep_modbus_server_alive = true;
            try {
                MCU_Monitor_Thread.Start();
                SoftwareStopThread.Start();
            } catch(Exception e) {
                if((e is ThreadStateException) || (e is OutOfMemoryException)) {
                    logger.Error( "failed to start prodi=uction plc and mcu threads err:____    {0}" , e );
                    return false;
                } else { throw e; }// Unexpected exception
            }
            return true;
        }


        /// <summary>
        /// kills the MCU monitor thread
        /// </summary>
        /// <returns></returns>
        public bool RequestStopAsyncAcceptingClientsAndJoin() {
            keep_modbus_server_alive = false;
            try {
                MCU_Monitor_Thread.Join();
                SoftwareStopThread.Join();
            } catch(Exception e) {
                if((e is ThreadStateException) || (e is ThreadStartException)) {
                    logger.Error( e );
                    return false;
                } else { throw e; }// Unexpected exception
            }
            return true;
        }

        /// <summary>
        /// allows the PLC driver class to read Modbus registers without giving it the ability to write to the MCU
        /// </summary>
        /// <param name="startadress"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        public ushort[] readModbusReregs( ushort startadress , ushort length ) {
            return TryReadRegs( startadress , length ).GetAwaiter().GetResult();
        }
        /// <summary>
        /// read the position from the motor encoders
        /// </summary>
        /// <returns></returns>
        public Orientation read_Position() {
            mCUpositon.update().Wait();
            return new Orientation(
                ConversionHelper.StepsToDegrees_Encoder(mCUpositon.AZ_Encoder, MotorConstants.GEARING_RATIO_AZIMUTH),
                ConversionHelper.StepsToDegrees_Encoder(mCUpositon.EL_Encoder, MotorConstants.GEARING_RATIO_ELEVATION)
            );
        }
        /// <summary>
        /// gets the position from MCU step count, thsi should be compaired with the value from <see cref="read_Position"/>
        /// </summary>
        /// <remarks>
        /// the MCU traks 2 values for position one that comes from the encoder mounted on the motors shaft, 
        /// the other comes from the MCU keeping track of howmany steps it has told the motor to take
        /// if these numbers get out of sync that means the motors are missing steps and are experiencing a high load,
        /// there are 8_000 encoder counts per revolution of the motor and 20_000 steeps from the MCU so the exact numbers wont match up 
        /// also there will always be some play in the position that the encoder reports +-10 counts should be within acceptabele limts
        /// </remarks>
        /// <returns></returns>
        public Orientation read_Position_steps() {
            mCUpositon.update().Wait();
            return new Orientation(
                ConversionHelper.StepsToDegrees( mCUpositon.AZ_Steps , MotorConstants.GEARING_RATIO_AZIMUTH ) ,
                ConversionHelper.StepsToDegrees( mCUpositon.EL_Steps, MotorConstants.GEARING_RATIO_ELEVATION )
            );
        }


        /// <summary>
        /// clears the previos move comand from mthe PLC, only works for jog moves
        /// </summary>
        /// <returns></returns>
        public bool Cancel_move(int priority) {
            var cmd = new MCUcomand( MESSAGE_CONTENTS_CLEAR_MOVE , MCUcomandType.CLEAR_LAST_MOVE,priority ) { completed = false };
            Send_Generic_Command_And_Track( cmd ).GetAwaiter().GetResult();
            Wait_For_Stop_Motion( cmd );
            return true;
        }

        /// <summary>
        /// attempts to bring the Telescope to a controlled stop certian moves like Homeing are un affected by this
        /// </summary>
        /// <returns></returns>
        public bool Controled_stop( int priority ) {
            if(RunningCommand.ComandType == MCUcomandType.JOG) {
                Cancel_move( priority );
            } else {
                var cmd = new MCUcomand( MESSAGE_CONTENTS_HOLD_MOVE , MCUcomandType.HOLD_MOVE , priority ) { completed = false };
                Send_Generic_Command_And_Track( cmd ).GetAwaiter().GetResult();
                Wait_For_Stop_Motion( cmd );
            }
            return true;
        }

        public bool Immediade_stop( int priority ) {
            Send_Generic_Command_And_Track( new MCUcomand( MESSAGE_CONTENTS_IMMEDIATE_STOP , MCUcomandType.IMIDEAT_STOP , priority ) { completed = true } ).GetAwaiter().GetResult();
            return true;
        }

        private void checkForAndResetErrors() {
            var data = TryReadRegs( 0 , 15 ).GetAwaiter().GetResult();
            bool azCmdErr = ((data[(int)MCUConstants.MCUOutputRegs.AZ_Status_Bist_MSW] >> (int)MCUConstants.MCUStutusBitsMSW.Command_Error) & 0b1) == 1;
            bool elCmdErr = ((data[(int)MCUConstants.MCUOutputRegs.EL_Status_Bist_MSW] >> (int)MCUConstants.MCUStutusBitsMSW.Command_Error) & 0b1) == 1;
            if(elCmdErr || azCmdErr) {
                Send_Generic_Command_And_Track( new MCUcomand( MESSAGE_CONTENTS_RESET_ERRORS , MCUcomandType.RESET_ERRORS , 0 ) { completed = true } ).GetAwaiter().GetResult();
            }
        }


        private async Task<bool> Override_And_Stop_Motion( int priority ) {
            var data = TryReadRegs( 0 , 15 ).GetAwaiter().GetResult();
            if(Is_Moing( data )) {
                if(RunningCommand.ComandType == MCUcomandType.JOG) {
                    Cancel_move( priority );
                    WatTillStopped().Wait();
                } else if(RunningCommand.ComandType == MCUcomandType.RELETIVE_MOVE) {
                    Cancel_move( priority );
                    Task.Delay( 100 ).Wait();
                    Controled_stop( priority );
                    WatTillStopped().Wait();
                } else {
                    Immediade_stop( priority );
                }
            }
            return true;
        }

        private async Task<bool> Wait_For_Stop_Motion( MCUcomand comand) {
            WatTillStopped().Wait();
            comand.completed = true;
            return true;
        }

        /// <summary>
        /// this function assums that you have alread told both Axisi to stop moving otherwise it will timeout
        /// </summary>
        /// <returns>false if the telescope was still running at the end of the timeout</returns>
        private async Task<bool> WatTillStopped() {
            try {
                int mS_To_Decelerate = estimateStopTime( PreviousCommand );
                var timout = new CancellationTokenSource( mS_To_Decelerate ).Token;
                while(!timout.IsCancellationRequested) {
                    var datatask = TryReadRegs( 0 , 12 );
                    Task.Delay( 33 ).Wait();
                    if(!Is_Moing( datatask.GetAwaiter().GetResult() )) {
                        return true;
                    }
                }
                return false;
            } catch {
                return false;
            }

        }

        private int estimateStopTime( MCUcomand CMD ) {
            int mS_To_DecelerateAZ = (int)1.25 * (CMD.AZ_Programed_Speed - AZStartSpeed) / CMD.AZ_ACC;
            int mS_To_DecelerateEL = (int)1.25 * (CMD.EL_Programed_Speed - AZStartSpeed) / CMD.EL_ACC;
            int mS_To_Decelerate;
            if(mS_To_DecelerateAZ > mS_To_DecelerateEL) {
                mS_To_Decelerate = mS_To_DecelerateAZ;
            } else {
                mS_To_Decelerate = mS_To_DecelerateEL;
            }
            return mS_To_Decelerate;
        }

        private Orientation estimateDistanceToStop( MCUcomand CMD ) {
            int StepsAZ = 0, StepsEL = 0;
            if(CMD.AZ_Programed_Speed > 0) {
                int mS_To_DecelerateAZ = (int)((1.25 * (CMD.AZ_Programed_Speed - AZStartSpeed) / (double)CMD.AZ_ACC) / 1000.0);
                StepsAZ = (int)(mS_To_DecelerateAZ * ((CMD.AZ_Programed_Speed + ConversionHelper.RPMToSPS( Current_AZConfiguration.StartSpeed , MotorConstants.GEARING_RATIO_AZIMUTH )) / 2.0));
                StepsAZ += (int)(CMD.AZ_Programed_Speed * 0.25);//add 100 ms worth of steps
                if(!CMD.AZ_CW) {
                    StepsAZ = -StepsAZ;
                }
            }
            if(CMD.EL_Programed_Speed > 0) {
                int mS_To_DecelerateEL = (int)((1.25 * (CMD.EL_Programed_Speed - AZStartSpeed) / (double)CMD.EL_ACC) / 1000.0);
                StepsEL = (int)(mS_To_DecelerateEL * ((CMD.EL_Programed_Speed + ConversionHelper.RPMToSPS( Current_ELConfiguration.StartSpeed , MotorConstants.GEARING_RATIO_ELEVATION )) / 2.0));
                StepsEL += (int)(CMD.EL_Programed_Speed * 0.25);//add 100 ms worth of steps
                if(!CMD.EL_CW) {
                    StepsEL = -StepsEL;
                }
            }
            return new Orientation( ConversionHelper.StepsToDegrees( StepsAZ , MotorConstants.GEARING_RATIO_AZIMUTH ) , ConversionHelper.StepsToDegrees( StepsEL , MotorConstants.GEARING_RATIO_ELEVATION ) );
        }


        /// <summary>
        /// this function assums that you have alread told both Axisi to stop moving otherwise it will timeout
        /// </summary>
        /// <returns>false if the telescope was still running at the end of the timeout</returns>
        private async Task<bool> WatTillStoppedPerAxis(bool is_AZ) {
            if(is_AZ) {
                try {
                    int mS_To_Decelerate = (int)1.25 * (PreviousCommand.AZ_Programed_Speed - AZStartSpeed) / PreviousCommand.AZ_ACC;
                    var timout = new CancellationTokenSource( mS_To_Decelerate ).Token;
                    while(!timout.IsCancellationRequested) {
                        var datatask = TryReadRegs( 0 , 12 );
                        Task.Delay( 33 ).Wait();
                        if(!Is_Moing_Per_Axis( datatask.GetAwaiter().GetResult() , is_AZ )) {
                            return true;
                        }
                    }
                    return false;
                } catch {
                    return false;
                }
            } else {
                try {
                    int mS_To_Decelerate = (int)1.25 * (PreviousCommand.EL_Programed_Speed - ELStartSpeed) / PreviousCommand.EL_ACC;
                    var timout = new CancellationTokenSource( mS_To_Decelerate ).Token;
                    while(!timout.IsCancellationRequested) {
                        var datatask = TryReadRegs( 0 , 12 );
                        Task.Delay( 33 ).Wait();
                        if(!Is_Moing_Per_Axis( datatask.GetAwaiter().GetResult() , is_AZ )) {
                            return true;
                        }
                    }
                    return false;
                } catch {
                    return false;
                }

            }

        }

        public bool Is_Moing(ushort[] data) {
            try {
                bool azMoving = (((data[(int)MCUConstants.MCUOutputRegs.AZ_Status_Bist_MSW] >> (int)MCUConstants.MCUStutusBitsMSW.CCW_Motion) & 0b1) == 1) ||
                                (((data[(int)MCUConstants.MCUOutputRegs.AZ_Status_Bist_MSW] >> (int)MCUConstants.MCUStutusBitsMSW.CW_Motion) & 0b1) == 1);
                bool elMoving = (((data[(int)MCUConstants.MCUOutputRegs.EL_Status_Bist_MSW] >> (int)MCUConstants.MCUStutusBitsMSW.CCW_Motion) & 0b1) == 1) ||
                                (((data[(int)MCUConstants.MCUOutputRegs.EL_Status_Bist_MSW] >> (int)MCUConstants.MCUStutusBitsMSW.CW_Motion) & 0b1) == 1);
                return azMoving || elMoving;
            } catch {
                return false;
            }
        }

        public bool Is_Moing_Per_Axis( ushort[] data ,bool Is_AZ) {
            if(Is_AZ) {
                try {
                    bool azMoving = (((data[(int)MCUConstants.MCUOutputRegs.AZ_Status_Bist_MSW] >> (int)MCUConstants.MCUStutusBitsMSW.CCW_Motion) & 0b1) == 1) ||
                                    (((data[(int)MCUConstants.MCUOutputRegs.AZ_Status_Bist_MSW] >> (int)MCUConstants.MCUStutusBitsMSW.CW_Motion) & 0b1) == 1);
                    return azMoving;
                } catch {
                    return false;
                }
            } else {
                try {
                    bool elMoving = (((data[(int)MCUConstants.MCUOutputRegs.EL_Status_Bist_MSW] >> (int)MCUConstants.MCUStutusBitsMSW.CCW_Motion) & 0b1) == 1) ||
                                    (((data[(int)MCUConstants.MCUOutputRegs.EL_Status_Bist_MSW] >> (int)MCUConstants.MCUStutusBitsMSW.CW_Motion) & 0b1) == 1);
                    return elMoving;
                } catch {
                    return false;
                }
            }

        }


        public async Task<bool> Configure_MCU( MCUConfigurationAxys AZconfig , MCUConfigurationAxys ELconfig , int priority ) {
            Current_AZConfiguration = AZconfig;
            Current_ELConfiguration = ELconfig;
            int gearedSpeedAZ = ConversionHelper.RPMToSPS( AZconfig.StartSpeed , MotorConstants.GEARING_RATIO_AZIMUTH );
            int gearedSpeedEL = ConversionHelper.RPMToSPS( ELconfig.StartSpeed , MotorConstants.GEARING_RATIO_ELEVATION );
            AZStartSpeed = gearedSpeedAZ;
            ELStartSpeed = gearedSpeedEL;
            TestDefaultParams( AZconfig.StartSpeed , ELconfig.StartSpeed , AZconfig.HomeTimeoutSec , ELconfig.HomeTimeoutSec );
            ushort[] data = {   MakeMcuConfMSW(AZconfig), MakeMcuConfLSW(AZconfig) , (ushort)(gearedSpeedAZ >> 0x0010), (ushort)(gearedSpeedAZ & 0xFFFF), 0x0,0x0,0x0,0x0,0x0,0x0,
                                MakeMcuConfMSW(ELconfig), MakeMcuConfLSW(ELconfig), (ushort)(gearedSpeedEL >> 0x0010), (ushort)(gearedSpeedEL & 0xFFFF), 0x0,0x0,0x0,0x0,0x0,0x0 };

            Immediade_stop( priority );
            Task.Delay( 50 ).Wait();
            checkForAndResetErrors();
            Task.Delay( 50 ).Wait();
            Send_Generic_Command_And_Track( new MCUcomand( data , MCUcomandType.CONFIGURE , priority ) { completed = true} ).GetAwaiter().GetResult();
            Task.Delay( 100 ).Wait();
            //TODO: check for configuration Errors
            return true;
        }

        private ushort MakeMcuConfMSW(MCUConfigurationAxys AxysConf) {
            ushort conf = 0x8400;//first byte should be 84 for current hardware setup
            switch (AxysConf.CWinput) {
                case CW_CCW_input_use.LimitSwitch:
                    conf = (ushort)(conf | 0b000_1000);
                    break;
                case CW_CCW_input_use.EStop:
                    conf = (ushort)(conf | 0b001_0000);
                    break;
            }
            switch (AxysConf.CCWinput) {
                case CW_CCW_input_use.LimitSwitch:
                    conf = (ushort)(conf | 0b0010_0000);
                    break;
                case CW_CCW_input_use.EStop:
                    conf = (ushort)(conf | 0b0100_0000);
                    break;
            }
            switch(AxysConf.EncoderType) {
                case EncoderTyprEnum.Quadrature_Encoder:
                    conf = (ushort)(conf | 0b1_0000_0000);
                    break;
                case EncoderTyprEnum.Diagnostic_Feedback:
                    conf = (ushort)(conf | 0b10_0000_0000);
                    break;
            }
            if (AxysConf.UseHomesensors) {
                conf = (ushort)(conf | 0b0100);
            }
            if(AxysConf.UseCapture) {
                conf = (ushort)(conf | 0b0001);
            }
            return conf;
        }

        private ushort MakeMcuConfLSW(MCUConfigurationAxys AxysConf) {
            ushort conf = 0x0000;
            if(AxysConf.CaptureActive_High) {
                conf = (ushort)(conf | 0b0001);
            }
            if (AxysConf.HomeActive_High) {
                conf = (ushort)(conf | 0b0_0100);
            }
            if (AxysConf.CWactive_High) {
                conf = (ushort)(conf | 0b0_1000);
            }
            if (AxysConf.CCWactive_High) {
                conf = (ushort)(conf | 0b1_0000);
            }
            return conf;
        }

        private void TestDefaultParams(double startSpeedDPSAzimuth, double startSpeedDPSElevation, int homeTimeoutSecondsAzimuth, int homeTimeoutSecondsElevation) {
            int gearedSpeedAZ = ConversionHelper.DPSToSPS(startSpeedDPSAzimuth, MotorConstants.GEARING_RATIO_AZIMUTH);
            int gearedSpeedEL = ConversionHelper.DPSToSPS(startSpeedDPSElevation, MotorConstants.GEARING_RATIO_ELEVATION);
            Console.WriteLine(gearedSpeedAZ.ToString() + " :AZ           EL:" + gearedSpeedEL.ToString());
            if ((gearedSpeedEL < 1) || (gearedSpeedEL > MCUConstants.ACTUAL_MCU_DEFAULT_PEAK_VELOCITY)) {
                throw new ArgumentOutOfRangeException("startSpeedDPSElevation", startSpeedDPSElevation,
                    String.Format("startSpeedDPSElevation should be between {0} and {1}",
                    ConversionHelper.SPSToDPS(1, MotorConstants.GEARING_RATIO_ELEVATION),
                    ConversionHelper.SPSToDPS(MCUConstants.ACTUAL_MCU_DEFAULT_PEAK_VELOCITY, MotorConstants.GEARING_RATIO_ELEVATION)));
            }
            if ((gearedSpeedAZ < 1) || (gearedSpeedAZ > MCUConstants.ACTUAL_MCU_DEFAULT_PEAK_VELOCITY)) {
                throw new ArgumentOutOfRangeException("startSpeedDPSAzimuth", startSpeedDPSAzimuth,
                    String.Format("startSpeedDPSAzimuth should be between {0} and {1}",
                    ConversionHelper.SPSToDPS(1, MotorConstants.GEARING_RATIO_AZIMUTH),
                    ConversionHelper.SPSToDPS(MCUConstants.ACTUAL_MCU_DEFAULT_PEAK_VELOCITY, MotorConstants.GEARING_RATIO_AZIMUTH)));
            }
            if ((homeTimeoutSecondsElevation < 0) || (homeTimeoutSecondsElevation > 300)) {
                throw new ArgumentOutOfRangeException("homeTimeoutSecondsElevation", homeTimeoutSecondsElevation,
                    String.Format("homeTimeoutSecondsElevation should be between {0} and {1}", 0, 300));
            }
            if ((homeTimeoutSecondsAzimuth < 0) || (homeTimeoutSecondsAzimuth > 300)) {
                throw new ArgumentOutOfRangeException("homeTimeoutSecondsAzimuth", homeTimeoutSecondsAzimuth,
                    String.Format("homeTimeoutSecondsAzimuth should be between {0} and {1}", 0, 300));
            }
        }






        /// <summary>
        /// sends a home command and waits for the MCU to finish homeing
        /// </summary>
        /// <param name="AZHomeCW"></param>
        /// <param name="ELHomeCW"></param>
        /// <param name="RPM"></param>
        /// <returns></returns>
        public async Task<bool> HomeBothAxyes( bool AZHomeCW , bool ELHomeCW , double RPM,int priority ) {
            int EL_Speed = ConversionHelper.DPSToSPS( ConversionHelper.RPMToDPS( RPM ) , MotorConstants.GEARING_RATIO_ELEVATION );
            int AZ_Speed = ConversionHelper.DPSToSPS( ConversionHelper.RPMToDPS( RPM ) , MotorConstants.GEARING_RATIO_AZIMUTH );
            ushort ACCELERATION = 50;
            ushort CWHome = 0x0020;
            ushort CcWHome = 0x0040;

            ushort azHomeDir = CcWHome;
            ushort elHomeDir = CcWHome;

            if(!ELHomeCW) {//if the MCU dosnt expect the home sensor to be high when a home is initiated this will need to be changed
                elHomeDir = CWHome;
            }

            //set config word to 0x0040 to have the RT home at the minimumum speed// this requires the MCU to be configured properly
            ushort[] data = {
                azHomeDir , 0x0000, 0x0000, 0x0000,(ushort)((AZ_Speed & 0xFFFF0000)>>16),(ushort)(AZ_Speed & 0xFFFF), ACCELERATION, ACCELERATION , 0x0000, 0x0000,
                elHomeDir , 0x0000, 0x0000, 0x0000,(ushort)((EL_Speed & 0xFFFF0000)>>16),(ushort)(EL_Speed & 0xFFFF), ACCELERATION, ACCELERATION , 0x0000, 0x0000
            };
            int timeout;
            if(Current_AZConfiguration.HomeTimeoutSec > Current_ELConfiguration.HomeTimeoutSec) {
                timeout = Current_AZConfiguration.HomeTimeoutSec;
            } else {
                timeout = Current_ELConfiguration.HomeTimeoutSec;
            }
            Cancel_move( priority );
            Task.Delay( 100 ).Wait();//wait to ensure it is porcessed
            Controled_stop( priority );
            Task.Delay( 100 ).Wait();//wait to ensure it is porcessed
            var ThisMove = Send_Generic_Command_And_Track( new MCUcomand( data , MCUcomandType.RELETIVE_MOVE , priority ) {
                AZ_Programed_Speed = AZ_Speed , EL_Programed_Speed = EL_Speed , EL_ACC = ACCELERATION , AZ_ACC = ACCELERATION , timeout = new CancellationTokenSource( (int)(timeout*1200) )//* 1000 for seconds to ms //* 1.2 for a 20% margin 
            } ).GetAwaiter().GetResult();
            Task.Delay( 500 ).Wait();
            FixedSizedQueue<MCUpositonStore> positionHistory = new FixedSizedQueue<MCUpositonStore>( 140 );//140 samples at 1 sample/50mS = 7 seconds of data
            Task<ushort[]> datatask;
            ushort[] MCUdata;
            while(!ThisMove.timeout.IsCancellationRequested) {
                var updatePoss =  mCUpositon.update();
                datatask = TryReadRegs( 0 , 12 );
                Task.Delay( 50 ).Wait();
                MCUdata = datatask.GetAwaiter().GetResult();


                updatePoss.Wait();
                positionHistory.Enqueue(new MCUpositonStore(mCUpositon));
                bool isMoving = Is_Moing( MCUdata );
                if(Math.Abs( mCUpositon.AZ_Steps ) < 4 && Math.Abs( mCUpositon.EL_Steps ) < 4 && !isMoving) {//if the encoders fave been 0'ed out with some error
                    consecutiveSucsefullMoves++;
                    consecutiveErrors = 0;
                    ThisMove.completed = true;
                    ThisMove.Dispose();
                    return true;
                }
                if(positionHistory.Count > positionHistory.Size - 2) {
                    var movement = positionHistory.GetAbsolutePosChange();
                    if(movement.AZ_Encoder <50 && movement.EL_Encoder < 50) {//if the telescope has been still for 7 seconds
                        bool AZCmdErr = ((MCUdata[(int)MCUConstants.MCUOutputRegs.AZ_Status_Bist_MSW] >> (int)MCUConstants.MCUStutusBitsMSW.Command_Error) & 0b1) == 1;
                        bool AZHomeErr = ((MCUdata[(int)MCUConstants.MCUOutputRegs.AZ_Status_Bist_MSW] >> (int)MCUConstants.MCUStutusBitsMSW.Home_Invalid_Error) & 0b1) == 1;
                        bool ELCmdErr = ((MCUdata[(int)MCUConstants.MCUOutputRegs.EL_Status_Bist_MSW] >> (int)MCUConstants.MCUStutusBitsMSW.Command_Error) & 0b1) == 1;
                        bool ELHomeErr = ((MCUdata[(int)MCUConstants.MCUOutputRegs.EL_Status_Bist_MSW] >> (int)MCUConstants.MCUStutusBitsMSW.Home_Invalid_Error) & 0b1) == 1;
                        if (Math.Abs(mCUpositon.AZ_Steps) > 4 || Math.Abs(mCUpositon.EL_Steps) > 4) {//and the pozition is not 0 then homeing has failed
                            consecutiveSucsefullMoves = 0;
                            consecutiveErrors++;
                            ThisMove.completed = true;
                            ThisMove.Dispose();
                            return false;
                            //throw new Exception("Homing faild to reach 0 properly");
                        } else if (ELHomeErr || AZHomeErr || AZCmdErr || ELCmdErr) {
                            consecutiveSucsefullMoves = 0;
                            consecutiveErrors++;
                            ThisMove.completed = true;
                            ThisMove.Dispose();
                            return false;
                            //throw new Exception(String.Format("Homing faild due to an error MCU status bits were    ELHomeErr={0}   AZHomeErr={1}   AZCmdErr={2}   ELCmdErr={3}", ELHomeErr, AZHomeErr, AZCmdErr, ELCmdErr));
                        }
                    }
                }
            }
            ThisMove.Dispose();
            return true;
        }

        private ushort[] prepairRelativeMoveData(int SpeedAZ, int SpeedEL, ushort ACCELERATION, int positionTranslationAZ, int positionTranslationEL) {
            if (SpeedAZ < AZStartSpeed) {
                throw new ArgumentOutOfRangeException("SpeedAZ", SpeedAZ,
                    String.Format("SpeedAZ should be grater than {0} which is the stating speed set when configuring the MCU", AZStartSpeed));
            }
            if (SpeedEL < ELStartSpeed) {
                throw new ArgumentOutOfRangeException("SpeedEL", SpeedEL,
                    String.Format("SpeedAZ should be grater than {0} which is the stating speed set when configuring the MCU", ELStartSpeed));
            }
            ushort[] data = {
                0x0002 , 0x0003, (ushort)((positionTranslationAZ & 0xFFFF0000)>>16),(ushort)(positionTranslationAZ & 0xFFFF),(ushort)((SpeedAZ & 0xFFFF0000)>>16),(ushort)(SpeedAZ & 0xFFFF), ACCELERATION,ACCELERATION ,0,0,
                0x0002 , 0x0003, (ushort)((positionTranslationEL & 0xFFFF0000)>>16),(ushort)(positionTranslationEL & 0xFFFF),(ushort)((SpeedEL & 0xFFFF0000)>>16),(ushort)(SpeedEL & 0xFFFF), ACCELERATION,ACCELERATION ,0,0
            };
            return data;
        }

        /// <summary>
        /// perform the specified move and wait for it to be completted by the telescope
        /// </summary>
        /// <param name="SpeedAZ"></param>
        /// <param name="SpeedEL"></param>
        /// <param name="ACCELERATION"></param>
        /// <param name="positionTranslationAZ"></param>
        /// <param name="positionTranslationEL"></param>
        ///  <param name="priority"></param>
        /// <returns></returns>
        public async Task<bool> MoveAndWaitForCompletion( int SpeedAZ , int SpeedEL , ushort ACCELERATION , int positionTranslationAZ , int positionTranslationEL,int priority ) {
            positionTranslationEL = -positionTranslationEL;
            mCUpositon.update().Wait();
            var startPos =  mCUpositon as MCUpositonStore;
            Cancel_move( priority );
            Task.Delay( 50 ).Wait();//wait to ensure it is porcessed
            ushort[] CMDdata = prepairRelativeMoveData( SpeedAZ , SpeedEL , ACCELERATION , positionTranslationAZ , positionTranslationEL );

            int AZTime = estimateTime( SpeedAZ , ACCELERATION , positionTranslationAZ ), ELTime = estimateTime( SpeedEL , ACCELERATION , positionTranslationEL );
            int TimeToMove;
            if(AZTime > ELTime) {
                TimeToMove = AZTime;
            } else { TimeToMove = ELTime; }
            TimeToMove = ( int)(TimeToMove * 1.2);
            TimeToMove += 100;

            var ThisMove = Send_Generic_Command_And_Track( new MCUcomand( CMDdata , MCUcomandType.RELETIVE_MOVE, priority, positionTranslationAZ > 0, positionTranslationEL > 0, SpeedAZ, SpeedEL ) { EL_ACC = ACCELERATION , AZ_ACC = ACCELERATION,timeout=new CancellationTokenSource( (int)(TimeToMove ) ) } ).GetAwaiter().GetResult();
            Task.Delay( 500 ).Wait();//wait for comand to be read
            bool WasLimitCancled = false;
            PLCLimitChangedEvent handle = ( object sender, limitEventArgs e ) => {
                var data = TryReadRegs( 0 , 12 ).GetAwaiter().GetResult();
                bool isMoving = Is_Moing( data );
                if(!isMoving) {
                    ThisMove.timeout.Cancel();
                    WasLimitCancled = true;
                }
            };
            PLCEvents.DurringOverrideAddSecondary( handle);
            while(!ThisMove.timeout.IsCancellationRequested) {
                var datatask = TryReadRegs( 0 , 12 );
                 Task.Delay( 50 ).Wait();
                var data = datatask.GetAwaiter().GetResult();
                bool azErr = ((data[(int)MCUConstants.MCUOutputRegs.AZ_Status_Bist_MSW] >> (int)MCUConstants.MCUStutusBitsMSW.Command_Error) & 0b1) == 1;
                bool elErr = ((data[(int)MCUConstants.MCUOutputRegs.EL_Status_Bist_MSW] >> (int)MCUConstants.MCUStutusBitsMSW.Command_Error) & 0b1) == 1;
                if(elErr || azErr) {//TODO:add more checks to this 
                    ThisMove.completed = true;
                    ThisMove.ComandError = new Exception( "MCU command error bit was set" );
                    consecutiveSucsefullMoves = 0;
                    consecutiveErrors++;
                    Send_Generic_Command_And_Track( new MCUcomand( MESSAGE_CONTENTS_RESET_ERRORS , MCUcomandType.RESET_ERRORS,0 ) ).Wait();
                    PLCEvents.DurringOverrideRemoveSecondary( handle );
                    ThisMove.Dispose();
                    return false;
                }
                bool azFin = ((data[(int)MCUConstants.MCUOutputRegs.AZ_Status_Bist_MSW] >> (int)MCUConstants.MCUStutusBitsMSW.Move_Complete) & 0b1) == 1;
                bool elFin = ((data[(int)MCUConstants.MCUOutputRegs.EL_Status_Bist_MSW] >> (int)MCUConstants.MCUStutusBitsMSW.Move_Complete) & 0b1) == 1;
                bool isMoving = Is_Moing( data );
                if(azFin && elFin && !isMoving) {
                    //TODO:check that position is correct and there arent any errors

                    consecutiveSucsefullMoves++;
                    consecutiveErrors = 0;
                    ThisMove.completed = true;
                    PLCEvents.DurringOverrideRemoveSecondary( handle );
                    ThisMove.Dispose();
                    return true;
                }

            }
            var data2  = TryReadRegs( 0 , 12 ).GetAwaiter().GetResult();
            if(Is_Moing( data2 )) {
                ThisMove.completed = true;
                ThisMove.ComandError = new Exception( "Move did not complete in the expected time" );
                consecutiveSucsefullMoves = 0;
                consecutiveErrors++;
            }else if(WasLimitCancled) {
                ThisMove.completed = true;
                ThisMove.ComandError = new Exception( "Move ended when a limit switch was hit" );
                consecutiveSucsefullMoves = 0;
                consecutiveErrors++;
            }
            PLCEvents.DurringOverrideRemoveSecondary( handle );
            ThisMove.Dispose();
            return true;
        }

        /// <summary>
        /// estimate time to complete a move based on input values
        /// </summary>
        /// <param name="maxVel"></param>
        /// <param name="acc"></param>
        /// <param name="dist"></param>
        /// <returns></returns>
        public static int estimateTime(int maxVel, int acc, int dist) {
            //acc steps/millisecond/second
            //maxVel steps/second
            //dist steps
            //return millisecond
            dist = Math.Abs( dist );
            maxVel = Math.Abs( maxVel );
            acc = Math.Abs( acc );
            int t1 = ((maxVel) / acc);//ms
            double t1s = t1 / 1000.0;
            int distT1 = (int)(((acc*1000) / 2) * (t1s * t1s) * 2);
            if (distT1 < dist) {
                int t2 = (dist - distT1) / maxVel;
                return t2*1000 + (2 * t1);
            } else {
                return 2 * t1;
            }
        }

        public bool Send_Jog_command( double AZspeed , bool AZClockwise , double ELspeed , bool ELPositive , int priority ) {
            ushort dir;
            ELPositive = !ELPositive;
            if(AZClockwise) {
                dir = 0x0080;
            } else dir = 0x0100;
            int AZstepSpeed = ConversionHelper.RPMToSPS( AZspeed , MotorConstants.GEARING_RATIO_AZIMUTH );
            int ELstepSpeed = ConversionHelper.RPMToSPS( ELspeed , MotorConstants.GEARING_RATIO_ELEVATION );
            ushort[] data = new ushort[10] { dir , 0x0003 , 0x0 , 0x0 , (ushort)(AZstepSpeed >> 16) , (ushort)(AZstepSpeed & 0xffff) , MCUConstants.ACTUAL_MCU_MOVE_ACCELERATION_WITH_GEARING , MCUConstants.ACTUAL_MCU_MOVE_ACCELERATION_WITH_GEARING , 0x0 , 0x0 , };
            ushort[] data2 = new ushort[20];

            if(AZstepSpeed > AZStartSpeed) {
                for(int j = 0; j < data.Length; j++) {
                    data2[j] = data[j];
                }
            } else {
                AZstepSpeed = 0;
            }

            if(ELstepSpeed > ELStartSpeed) {
                for(int j = 0; j < data.Length; j++) {
                    data2[j + 10] = data[j];
                }
                if(ELPositive) {
                    dir = 0x0080;
                } else dir = 0x0100;

                data2[10] = (ushort)(dir);
                data2[14] = (ushort)(ELstepSpeed >> 16);
                data2[15] = (ushort)(ELstepSpeed & 0xffff);
            } else {
                ELstepSpeed = 0;
            }
            bool booth = false;
            if(RunningCommand.ComandType == MCUcomandType.JOG) {
                //if telescope is already joging changing direction requires stopping first
                ushort[] data3 = new ushort[20];
                data2.CopyTo( data3 , 0 );
                booth = (RunningCommand.AZ_CW != AZClockwise) && (RunningCommand.EL_CW != ELPositive);
                if(booth) {//if both axis need to change direction
                    Cancel_move( priority );
                    WatTillStopped().GetAwaiter().GetResult();
                } else if(RunningCommand.EL_CW != ELPositive) {//if only elevation needs to change direction
                    for(int j = 0; j <= data3.Length - 11; j++) {
                        data3[j + 10] = MESSAGE_CONTENTS_CLEAR_MOVE[j + 10];//replace elevation portion of move with controled stop
                    }
                    _ = Send_Generic_Command_And_Track( new MCUcomand( data3 , MCUcomandType.JOG , priority , AZClockwise , ELPositive , AZstepSpeed , ELstepSpeed ) {
                        EL_ACC = MCUConstants.ACTUAL_MCU_MOVE_ACCELERATION_WITH_GEARING ,
                    } ).GetAwaiter().GetResult();
                    WatTillStoppedPerAxis( true ).GetAwaiter().GetResult();
                } else if(RunningCommand.AZ_CW != AZClockwise) {//only Azimuth needs to change direction
                    for(int j = 0; j <= data3.Length - 1; j++) {
                        data3[j] = MESSAGE_CONTENTS_CLEAR_MOVE[j];//replace Azimuth portion of move with controled stop
                    }
                    _ = Send_Generic_Command_And_Track( new MCUcomand( data3 , MCUcomandType.JOG , priority , AZClockwise , ELPositive , AZstepSpeed , ELstepSpeed ) {
                        AZ_ACC = MCUConstants.ACTUAL_MCU_MOVE_ACCELERATION_WITH_GEARING ,
                    } ).GetAwaiter().GetResult();
                    WatTillStoppedPerAxis( false ).GetAwaiter().GetResult();
                }
            }

            _ = Send_Generic_Command_And_Track( new MCUcomand( data2 , MCUcomandType.JOG , priority , AZClockwise , ELPositive , AZstepSpeed , ELstepSpeed ) {//send the portion of the jog move that was previously replaced with a contoroled stop
                EL_ACC = MCUConstants.ACTUAL_MCU_MOVE_ACCELERATION_WITH_GEARING ,
                AZ_ACC = MCUConstants.ACTUAL_MCU_MOVE_ACCELERATION_WITH_GEARING ,
            } ).GetAwaiter().GetResult();
            return true;
        }

        private async Task<MCUcomand> Send_Generic_Command_And_Track( MCUcomand incoming ) {//TODO: implament priority 
            if((incoming.Move_Priority > RunningCommand.Move_Priority)&& !RunningCommand.completed) {
                return incoming;
            }
            Console.WriteLine( "running: {0} ||| {1}     incomming: {2} ||| {3}", RunningCommand.ComandType.ToString(), RunningCommand.Move_Priority , incoming.ComandType.ToString(),incoming.Move_Priority );
            PreviousCommand = RunningCommand;
            if(RunningCommand.ComandType == MCUcomandType.JOG) {
                if(incoming.ComandType == MCUcomandType.CLEAR_LAST_MOVE || incoming.ComandType == MCUcomandType.IMIDEAT_STOP || incoming.ComandType == MCUcomandType.JOG) {
                    try {
                        RunningCommand?.timeout?.Cancel();
                    } catch { }
                    RunningCommand = incoming;
                    TryWriteRegs( MCUConstants.ACTUAL_MCU_WRITE_REGISTER_START_ADDRESS , incoming.comandData ).Wait();
                    return incoming;
                }
                incoming.ComandError = new Exception( "MCU was running a JOG move which could not be overriden" );
                return incoming;
            } else if((RunningCommand.ComandType == MCUcomandType.HOME && !RunningCommand.completed)) {
                if( incoming.ComandType == MCUcomandType.IMIDEAT_STOP) {
                    try {
                        RunningCommand?.timeout?.Cancel();
                    } catch { }
                    RunningCommand = incoming;
                    TryWriteRegs( MCUConstants.ACTUAL_MCU_WRITE_REGISTER_START_ADDRESS , incoming.comandData ).Wait();
                    return incoming;
                }
                incoming.ComandError = new Exception( "MCU was running a home move which could not be overriden" );
                return incoming;
            }
            try {
                RunningCommand?.timeout?.Cancel();
            } catch { }
            RunningCommand = incoming;
            TryWriteRegs( MCUConstants.ACTUAL_MCU_WRITE_REGISTER_START_ADDRESS , incoming.comandData ).Wait();
            return incoming;
        }

        /// <summary>
        /// this should only be used to back off of limit switches for any other jog move use the <see cref="Send_Jog_command"/>
        /// </summary>
        /// <param name="AZ"></param>
        /// <param name="CW"></param>
        /// <param name="speed"></param>
        /// <returns></returns>
        public async Task<bool> SendSingleAxisJog(bool AZ,bool CW, double speed, int priority ) {
            ushort DataOffset, dir;
            int StepSpeed;
            if(CW) {
                dir = 0x0080;
            } else dir = 0x0100;
            if(AZ) {
                StepSpeed = ConversionHelper.RPMToSPS( speed , MotorConstants.GEARING_RATIO_AZIMUTH );
                DataOffset = 0;
            } else {
                StepSpeed = ConversionHelper.RPMToSPS( speed , MotorConstants.GEARING_RATIO_ELEVATION );
                DataOffset = 10;
            }
            ushort[] data = new ushort[10] { dir , 0x0003 , 0x0 , 0x0 , (ushort)(StepSpeed >> 16) , (ushort)(StepSpeed & 0xffff) , MCUConstants.ACTUAL_MCU_MOVE_ACCELERATION_WITH_GEARING , MCUConstants.ACTUAL_MCU_MOVE_ACCELERATION_WITH_GEARING , 0x0 , 0x0 , };
            TryWriteRegs( (ushort)(MCUConstants.ACTUAL_MCU_WRITE_REGISTER_START_ADDRESS + DataOffset ), data ).Wait();
            RunningCommand.Move_Priority = priority;
            RunningCommand.completed = false;
            return true;
        }

        /// <summary>
        /// this should only be used to back off of limit switches for any other uses the <see cref="Send_Jog_command"/>
        /// </summary>
        /// <param name="AZ"></param>
        /// <param name="CW"></param>
        /// <param name="speed"></param>
        /// <returns></returns>
        public async Task<bool> StopSingleAxisJog( bool AZ,int priority ) {
            ushort DataOffset;

            if(AZ) {
                DataOffset = 0;
            } else {
                DataOffset = 10;
            }
            ushort[] data = new ushort[10];
            for(int i = 0; i < data.Length; i++) {
                data[i] = MESSAGE_CONTENTS_CLEAR_MOVE[i];
            }
            TryWriteRegs( (ushort)(MCUConstants.ACTUAL_MCU_WRITE_REGISTER_START_ADDRESS + DataOffset) , data ).Wait();
            RunningCommand.Move_Priority = priority;
            RunningCommand.completed = true;
            return true;
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
