using ControlRoomApplication.Constants;
using ControlRoomApplication.Controllers.PLCCommunication;
using ControlRoomApplication.Controllers.PLCCommunication.PLCDrivers.MCUManager;
using ControlRoomApplication.Controllers.PLCCommunication.PLCDrivers.MCUManager.Enumerations;
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
using static ControlRoomApplication.Constants.MCUConstants;

namespace ControlRoomApplication.Controllers {
    //TODO: this would be a fairly large amount of work but, when i wrote the class i assumed that the MCU would only ever get comands that affect both axsis at the same time
    // the only place right now that only affects a single axsis is the single axsis jog, but there could be more in the future
    // so the best thing to do would be to split up the AZ and EL components of MCUCommand and then have 2 inststances for running command one for AZ one for EL
    public class MCUManager {
        private static readonly log4net.ILog logger = log4net.LogManager.GetLogger( System.Reflection.MethodBase.GetCurrentMethod().DeclaringType );
        /// <summary>
        /// if more errors than this value are thrown in a row and this class cant resolve them subsiquent attempts to send moves to the MCU will throw exception
        /// </summary>
        private static int MaxConscErrors = 5;
        private int consecutiveErrors = 0;
        private int consecutiveSuccessfulMoves = 0;

        private long MCU_last_contact = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
        private Thread HeartbeatMonitorThread;
        private bool HeartbeatMonitorRunning;
        private int AZStartSpeed = 0;
        private int ELStartSpeed = 0;
        public ModbusIpMaster MCUModbusMaster;
        private TcpClient MCUTCPClient;
        public MCUPositonRegs mCUpositon;
        private MCUConfigurationAxys Current_AZConfiguration;
        private MCUConfigurationAxys Current_ELConfiguration;
        /// <summary>
        /// this value should not be changed from outside the MCU class
        /// </summary>
        private MCUCommand RunningCommand= new MCUCommand(new ushort[20],MCUCommandType.CLEAR_LAST_MOVE,99) { completed = true };
        private MCUCommand PreviousCommand = new MCUCommand( new ushort[20] , MCUCommandType.CLEAR_LAST_MOVE,99 ) { completed = true };

        private int McuPort;
        private string McuIp;
        private RadioTelescopeTypeEnum telescopeType;

        public MCUManager(string ip, int port) {
            McuPort = port;
            McuIp = ip;

            mCUpositon = new MCUPositonRegs();

            ConnectToModbusServer();

            HeartbeatMonitorThread = new Thread(new ThreadStart(HeartbeatMonitor)) { Name = "MCU Heartbeat Monitor Thread" };
        }

        /// <summary>
        /// Attempts to connect to the MCU either for the first time, or after the connection has been lost.
        /// </summary>
        /// <returns>Result of whether or not the connection was successful.</returns>
        private bool ConnectToModbusServer() {
            logger.Info("Attempting to connect to the MCU...");

            // Attempt to connect to the MCU's TCP server
            try
            {
                MCUTCPClient = new TcpClient(McuIp, McuPort);
            }
            catch
            {
                logger.Error("There was an error connecting to the MCU's TCP server. Please shut down the Control Room software " +
                    "and verify that the Ethernet cable connecting the PLC to the MCU is firmly in place.");
                return false;
            }

            try
            {
                MCUModbusMaster = ModbusIpMaster.CreateIp(MCUTCPClient);
            }
            catch
            {
                logger.Error("There was an error creating the Modbus IP Master. Please shut down the Control Room software and verify " +
                    "that there are no loose connections between the PLC and the MCU.");
                return false;
            }

            // Sometimes when we first connect, there may be errors present on the registers still. With a new connect, we want
            // to clear these
            ResetMCUErrors();

            return true;
        }

        /// <summary>
        /// Reads registers from the MCU without writing. This is public so the PLC can also read registers if needed.
        /// </summary>
        /// <param name="address">The starting address that we are reading.</param>
        /// <param name="length">The number of addresses to read.</param>
        /// <returns>Registers that were requested.</returns>
        public ushort[] ReadMCURegisters(ushort address, ushort length)
        {
            ushort[] value = new ushort[length];
            try
            {
                value = MCUModbusMaster.ReadHoldingRegistersAsync(address, length).GetAwaiter().GetResult();
            }

            // This may happen if we lose connection to the MCU
            catch (InvalidOperationException)
            {
                logger.Error("The MCU failed to retrieve the register data, and is either offline or the connection has been terminated.");
                value = new ushort[50];
            }

            return value;
        }
        
        private bool WriteMCURegisters( ushort address , ushort[] data )
        {
            try
            {
                MCUModbusMaster.WriteMultipleRegistersAsync(address, data).GetAwaiter().GetResult();
            }
            catch (InvalidOperationException) {
                
                logger.Error("The MCU failed to receive the command, and is either offline or the connection has been terminated.");
                return false;
            }

            return true;
        }

        /// <summary>
        /// This thread will read the heartbeat bit in the MCU status to determine if the MCU is still alive.
        /// This loops around every 250 ms.
        /// </summary>
        private void HeartbeatMonitor() {

            int lastHeartBeat = 0;
            DateTime lastConnectAttempt = DateTime.Now;

            while(HeartbeatMonitorRunning) {

                ushort[] networkStatus = ReadMCURegisters( (ushort)MCUConstants.MCUOutputRegs.NetworkConnectivity , 1 );

                // If the network status length is 50, it means the network has disconnected, and we must attempt to reconnect
                if (networkStatus.Length == 50)
                {
                    // Only try to connect if it's been more than 5 seconds since the last attempt
                    if ((DateTime.Now - lastConnectAttempt) > TimeSpan.FromSeconds(5))
                    {
                        logger.Error("MCU network disconnected...");
                        ConnectToModbusServer();
                        lastConnectAttempt = DateTime.Now;
                    }
                }
                else
                {
                    // This heartbeat bit flips between 0 and 1 every 500ms to ensure that the MCU is still alive
                    // and doing MCU stuff
                    int currentHeartBeat = (networkStatus[0] >> (ushort)MCUNetworkStatus.MCUHeartBeat) & 1;
                    if (currentHeartBeat != lastHeartBeat)
                    {
                        MCU_last_contact = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
                        lastHeartBeat = currentHeartBeat;
                    }

                    // This bit is used to tell when the control room has reconnected to the MCU after being
                    // disconnected
                    if (((networkStatus[0] >> (ushort)MCUNetworkStatus.MCUNetworkDisconnected) & 1) == 1)
                    {
                        logger.Warn("MCU network recovered from being disconnected.");
                    }
                }

                Task.Delay(250).Wait();
            }
        }

        /// <summary>
        /// Starts the MCU heartbeat monitor thread.
        /// </summary>
        /// <returns>Boolean denoting whether the thread was started successfully or not.</returns>
        public bool StartAsyncAcceptingClients() {
            HeartbeatMonitorRunning = true;

            try
            {
                HeartbeatMonitorThread.Start();
            }
            catch (Exception e)
            {
                if((e is ThreadStateException) || (e is OutOfMemoryException))
                {
                    logger.Error( "Failed to start the MCU heartbeat monitor thread. Please restart the Control Room software.");
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// kills the MCU monitor thread
        /// </summary>
        /// <returns></returns>
        public bool RequestStopAsyncAcceptingClientsAndJoin() {
            HeartbeatMonitorRunning = false;
            try {
                HeartbeatMonitorThread.Join();
            } catch(Exception e) {
                if((e is ThreadStateException) || (e is ThreadStartException)) {
                    logger.Error( e );
                    return false;
                } else { throw e; }// Unexpected exception
            }
            return true;
        }

        /// <summary>
        /// Reads the position from the motor encoders.
        /// </summary>
        /// <returns></returns>
        public Orientation read_Position() {

            // First update the registers to be absolutely sure we have the most recent position
            mCUpositon.update(ReadMCURegisters(0, 16));

            // If the telescope type is SLIP_RING, we want to normalize the azimuth orientation
            if (telescopeType == RadioTelescopeTypeEnum.SLIP_RING)
            {
                return new Orientation(
                    ConversionHelper.StepsToDegrees_Encoder_Normalized(mCUpositon.AzEncoder, MotorConstants.GEARING_RATIO_AZIMUTH),
                    ConversionHelper.StepsToDegrees_Encoder(mCUpositon.ElEncoder, MotorConstants.GEARING_RATIO_ELEVATION)
                );
            }
            else
            {
                return new Orientation(
                    ConversionHelper.StepsToDegrees_Encoder(mCUpositon.AzEncoder, MotorConstants.GEARING_RATIO_AZIMUTH),
                    ConversionHelper.StepsToDegrees_Encoder(mCUpositon.ElEncoder, MotorConstants.GEARING_RATIO_ELEVATION)
                );
            }
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
            mCUpositon.update(ReadMCURegisters(0, 16));

            // If the telescope type is SLIP_RING, we want to normalize the azimuth orientation
            if (telescopeType == RadioTelescopeTypeEnum.SLIP_RING)
            {
                return new Orientation(
                    ConversionHelper.StepsToDegrees_Normalized(mCUpositon.AzSteps, MotorConstants.GEARING_RATIO_AZIMUTH),
                    ConversionHelper.StepsToDegrees(mCUpositon.ElSteps, MotorConstants.GEARING_RATIO_ELEVATION)
                );
            }
            else
            {
                return new Orientation(
                    ConversionHelper.StepsToDegrees(mCUpositon.AzSteps, MotorConstants.GEARING_RATIO_AZIMUTH),
                    ConversionHelper.StepsToDegrees(mCUpositon.ElSteps, MotorConstants.GEARING_RATIO_ELEVATION)
                );
            }
        }


        /// <summary>
        /// clears the previos move comand from mthe PLC, only works for jog moves
        /// </summary>
        /// <returns></returns>
        public bool Cancel_move(int priority) {
            var cmd = new MCUCommand( MCUMessages.ClearMove , MCUCommandType.CLEAR_LAST_MOVE,priority ) { completed = false };
            Send_Generic_Command_And_Track( cmd ).GetAwaiter().GetResult();
            Wait_For_Stop_Motion( cmd );
            return true;
        }

        /// <summary>
        /// attempts to bring the Telescope to a controlled stop certian moves like Homeing are un affected by this
        /// </summary>
        /// <returns></returns>
        public bool ControlledStop( int priority ) {
            if(RunningCommand.CommandType == MCUCommandType.JOG) {
                Cancel_move( priority );
            } else {
                var cmd = new MCUCommand( MCUMessages.HoldMove , MCUCommandType.HOLD_MOVE , priority ) { completed = false };
                Send_Generic_Command_And_Track( cmd ).GetAwaiter().GetResult();
                Wait_For_Stop_Motion( cmd );
            }
            return true;
        }

        public bool ImmediateStop( int priority ) {
            Send_Generic_Command_And_Track( new MCUCommand( MCUMessages.ImmediateStop , MCUCommandType.IMMEDIATE_STOP , priority ) { completed = true } ).GetAwaiter().GetResult();
            return true;
        }

        // This only resets command errors
        private void CheckForAndResetCommandErrors() {

            // Registers 0 and 11 are the most significant words fo the azimuth and elevation statuses
            var data = ReadMCURegisters(0, 11);

            bool azCmdErr = ((data[(int)MCUConstants.MCUOutputRegs.AZ_Status_Bist_MSW] >> (int)MCUConstants.MCUStatusBitsMSW.Command_Error) & 0b1) == 1;
            bool elCmdErr = ((data[(int)MCUConstants.MCUOutputRegs.EL_Status_Bist_MSW] >> (int)MCUConstants.MCUStatusBitsMSW.Command_Error) & 0b1) == 1;

            if(elCmdErr || azCmdErr) {
                ResetMCUErrors();
            }
        }

        /// <summary>
        /// Resets any errors the MCU encounters. This could be for either of the motors.
        /// </summary>
        public void ResetMCUErrors()
        {
            Send_Generic_Command_And_Track(new MCUCommand(MCUMessages.ResetErrors, MCUCommandType.RESET_ERRORS, 0) { completed = true }).GetAwaiter().GetResult();
        }

        /// <summary>
        /// Checks the MCU registers for any errors
        /// </summary>
        /// <returns>Returns all errors that were found.</returns>
        public List<Tuple<MCUOutputRegs, MCUStatusBitsMSW>> CheckMCUErrors()
        {
            var data = ReadMCURegisters(0, 12);

            // We will be storing the values in pairs so we can see which status bit has which error
            List<Tuple<MCUOutputRegs, MCUStatusBitsMSW>> errors = new List<Tuple<MCUOutputRegs, MCUStatusBitsMSW>>();

            // Loop through every MCU output register and check for errors
            for(int i = 0; i <= 11; i++)
            {
                // If the register contains an error, add it to the output list
                // We are only interested in output registers 0, 1, 10 and 11, which contain the
                // azimuth and elevation motor errors

                // Home invalid errors
                if ((i == 0 || i == 1 || i == 10 || i == 11) && ((data[i] >> (int)MCUStatusBitsMSW.Home_Invalid_Error) & 0b1) == 1)
                    errors.Add(new Tuple<MCUOutputRegs, MCUStatusBitsMSW>((MCUOutputRegs)i, MCUStatusBitsMSW.Home_Invalid_Error));

                // Profile invalid errors
                if ((i == 0 || i == 1 || i == 10 || i == 11) && ((data[i] >> (int)MCUStatusBitsMSW.Profile_Invalid) & 0b1) == 1)
                    errors.Add(new Tuple<MCUOutputRegs, MCUStatusBitsMSW>((MCUOutputRegs)i, MCUStatusBitsMSW.Profile_Invalid));

                // Input error
                if ((i == 0 || i == 1 || i == 10 || i == 11) && ((data[i] >> (int)MCUStatusBitsMSW.Input_Error) & 0b1) == 1)
                    errors.Add(new Tuple<MCUOutputRegs, MCUStatusBitsMSW>((MCUOutputRegs)i, MCUStatusBitsMSW.Input_Error));

                // Command errors
                if ((i == 0 || i == 1 || i == 10 || i == 11) && ((data[i] >> (int)MCUStatusBitsMSW.Command_Error) & 0b1) == 1)
                    errors.Add(new Tuple<MCUOutputRegs, MCUStatusBitsMSW>((MCUOutputRegs)i, MCUStatusBitsMSW.Command_Error));

                // Configuration error
                if ((i == 0 || i == 1 || i == 10 || i == 11) && ((data[i] >> (int)MCUStatusBitsMSW.Configuration_Error) & 0b1) == 1)
                    errors.Add(new Tuple<MCUOutputRegs, MCUStatusBitsMSW>((MCUOutputRegs)i, MCUStatusBitsMSW.Configuration_Error));
            }

            return errors;
        }

        private async Task<bool> Wait_For_Stop_Motion( MCUCommand comand) {
            WaitUntilStopped().Wait();
            comand.completed = true;
            return true;
        }

        /// <summary>
        /// this function assums that you have alread told both Axisi to stop moving otherwise it will timeout
        /// </summary>
        /// <returns>false if the telescope was still running at the end of the timeout</returns>
        private async Task<bool> WaitUntilStopped() {
            try {
                int mS_To_Decelerate = estimateStopTime( PreviousCommand );
                var timout = new CancellationTokenSource( mS_To_Decelerate ).Token;
                while(!timout.IsCancellationRequested) {
                    Task.Delay( 33 ).Wait();
                    if(!MotorsCurrentlyMoving()) {
                        return true;
                    }
                }
                return false;
            } catch {
                return false;
            }

        }

        private int estimateStopTime( MCUCommand CMD ) {
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

        private Orientation estimateDistanceToStop( MCUCommand CMD ) {
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
        private async Task<bool> WaitUntilStoppedPerAxis(bool is_AZ) {
            if(is_AZ) {
                try {
                    int mS_To_Decelerate = (int)1.25 * (PreviousCommand.AZ_Programed_Speed - AZStartSpeed) / PreviousCommand.AZ_ACC;
                    var timout = new CancellationTokenSource( mS_To_Decelerate ).Token;
                    while(!timout.IsCancellationRequested) {
                        var datatask = ReadMCURegisters(0 , 12);
                        Task.Delay( 33 ).Wait();
                        if(!MotorsCurrentlyMoving(MotorAxisEnum.Azimuth)) {
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
                        var datatask = ReadMCURegisters(0 , 12);
                        Task.Delay( 33 ).Wait();
                        if(!MotorsCurrentlyMoving(MotorAxisEnum.Elevation)) {
                            return true;
                        }
                    }
                    return false;
                } catch {
                    return false;
                }

            }

        }

        /// <summary>
        /// Tells us if the motors are currently moving. This will automatically return if both motors are moving
        /// if no parameter is passed through.
        /// </summary>
        /// <param name="axis">The axis we want to check is moving.</param>
        /// <returns>True if they are moving clockwise or counter-clockwise, false if they are still.</returns>
        public bool MotorsCurrentlyMoving(MotorAxisEnum axis = MotorAxisEnum.Both) {

            bool isMoving = false;
            ushort[] data;

            switch(axis)
            {
                case MotorAxisEnum.Azimuth:
                    // Only read the registers we need
                    data = ReadMCURegisters(0, 1);

                    // Check if the azimuth motor is spinning clockwise or counter-clockwise
                    isMoving = (((data[(int)MCUConstants.MCUOutputRegs.AZ_Status_Bist_MSW] >> (int)MCUConstants.MCUStatusBitsMSW.CCW_Motion) & 0b1) == 1) ||
                            (((data[(int)MCUConstants.MCUOutputRegs.AZ_Status_Bist_MSW] >> (int)MCUConstants.MCUStatusBitsMSW.CW_Motion) & 0b1) == 1);
                    break;

                case MotorAxisEnum.Elevation:
                    // Only read the registers we need
                    data = ReadMCURegisters(10, 1);
                    isMoving = (((data[(int)MCUConstants.MCUOutputRegs.EL_Status_Bist_MSW] >> (int)MCUConstants.MCUStatusBitsMSW.CCW_Motion) & 0b1) == 1) ||
                            (((data[(int)MCUConstants.MCUOutputRegs.EL_Status_Bist_MSW] >> (int)MCUConstants.MCUStatusBitsMSW.CW_Motion) & 0b1) == 1);
                    break;

                case MotorAxisEnum.Both:
                    // Now we need to capture some more registers to get both az and el
                    data = ReadMCURegisters(0, 11);
                    
                    bool azMoving = (((data[(int)MCUConstants.MCUOutputRegs.AZ_Status_Bist_MSW] >> (int)MCUConstants.MCUStatusBitsMSW.CCW_Motion) & 0b1) == 1) ||
                            (((data[(int)MCUConstants.MCUOutputRegs.AZ_Status_Bist_MSW] >> (int)MCUConstants.MCUStatusBitsMSW.CW_Motion) & 0b1) == 1);

                    bool elMoving = (((data[(int)MCUConstants.MCUOutputRegs.EL_Status_Bist_MSW] >> (int)MCUConstants.MCUStatusBitsMSW.CCW_Motion) & 0b1) == 1) ||
                            (((data[(int)MCUConstants.MCUOutputRegs.EL_Status_Bist_MSW] >> (int)MCUConstants.MCUStatusBitsMSW.CW_Motion) & 0b1) == 1);

                    // Check if azimuth or elevation are moving
                    isMoving = azMoving || elMoving;

                    break;
            }

            return isMoving;
        }

        public bool Configure_MCU( MCUConfigurationAxys AZconfig , MCUConfigurationAxys ELconfig , int priority ) {
            Current_AZConfiguration = AZconfig;
            Current_ELConfiguration = ELconfig;
            int gearedSpeedAZ = ConversionHelper.RPMToSPS( AZconfig.StartSpeed , MotorConstants.GEARING_RATIO_AZIMUTH );
            int gearedSpeedEL = ConversionHelper.RPMToSPS( ELconfig.StartSpeed , MotorConstants.GEARING_RATIO_ELEVATION );
            AZStartSpeed = gearedSpeedAZ;
            ELStartSpeed = gearedSpeedEL;
            TestDefaultParams( AZconfig.StartSpeed , ELconfig.StartSpeed , AZconfig.HomeTimeoutSec , ELconfig.HomeTimeoutSec );
            ushort[] data = {   MakeMcuConfMSW(AZconfig), MakeMcuConfLSW(AZconfig) , (ushort)(gearedSpeedAZ >> 0x0010), (ushort)(gearedSpeedAZ & 0xFFFF), 0x0,0x0,0x0,0x0,0x0,0x0,
                                MakeMcuConfMSW(ELconfig), MakeMcuConfLSW(ELconfig), (ushort)(gearedSpeedEL >> 0x0010), (ushort)(gearedSpeedEL & 0xFFFF), 0x0,0x0,0x0,0x0,0x0,0x0 };

            ImmediateStop( priority );
            Task.Delay( 50 ).Wait();
            CheckForAndResetCommandErrors();
            Send_Generic_Command_And_Track( new MCUCommand( data , MCUCommandType.CONFIGURE , priority ) { completed = true} ).GetAwaiter().GetResult();
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
            ControlledStop( priority );
            Task.Delay( 100 ).Wait();//wait to ensure it is porcessed
            var ThisMove = Send_Generic_Command_And_Track( new MCUCommand( data , MCUCommandType.RELATIVE_MOVE , priority ) {
                AZ_Programed_Speed = AZ_Speed , EL_Programed_Speed = EL_Speed , EL_ACC = ACCELERATION , AZ_ACC = ACCELERATION , timeout = new CancellationTokenSource( (int)(timeout*1200) )//* 1000 for seconds to ms //* 1.2 for a 20% margin 
            } ).GetAwaiter().GetResult();
            Task.Delay( 500 ).Wait();
            FixedSizedQueue<MCUPositonStore> positionHistory = new FixedSizedQueue<MCUPositonStore>( 140 );//140 samples at 1 sample/50mS = 7 seconds of data
            ushort[] outputRegData;
            while(!ThisMove.timeout.IsCancellationRequested)
            {
                outputRegData = ReadMCURegisters(0, 16);
                mCUpositon.update(outputRegData);
                
                positionHistory.Enqueue(new MCUPositonStore(mCUpositon));
                if(Math.Abs( mCUpositon.AzSteps ) < 4 && Math.Abs( mCUpositon.ElSteps ) < 4 && !MotorsCurrentlyMoving()) {//if the encoders fave been 0'ed out with some error
                    consecutiveSuccessfulMoves++;
                    consecutiveErrors = 0;
                    ThisMove.completed = true;
                    ThisMove.Dispose();
                    return true;
                }
                if(positionHistory.Count > positionHistory.Size - 2) {
                    var movement = positionHistory.GetAbsolutePosChange();
                    if(movement.AzEncoder <50 && movement.ElEncoder < 50) {//if the telescope has been still for 7 seconds
                        bool AZCmdErr = ((outputRegData[(int)MCUConstants.MCUOutputRegs.AZ_Status_Bist_MSW] >> (int)MCUConstants.MCUStatusBitsMSW.Command_Error) & 0b1) == 1;
                        bool AZHomeErr = ((outputRegData[(int)MCUConstants.MCUOutputRegs.AZ_Status_Bist_MSW] >> (int)MCUConstants.MCUStatusBitsMSW.Home_Invalid_Error) & 0b1) == 1;
                        bool ELCmdErr = ((outputRegData[(int)MCUConstants.MCUOutputRegs.EL_Status_Bist_MSW] >> (int)MCUConstants.MCUStatusBitsMSW.Command_Error) & 0b1) == 1;
                        bool ELHomeErr = ((outputRegData[(int)MCUConstants.MCUOutputRegs.EL_Status_Bist_MSW] >> (int)MCUConstants.MCUStatusBitsMSW.Home_Invalid_Error) & 0b1) == 1;
                        if (Math.Abs(mCUpositon.AzSteps) > 4 || Math.Abs(mCUpositon.ElSteps) > 4) {//and the pozition is not 0 then homeing has failed
                            consecutiveSuccessfulMoves = 0;
                            consecutiveErrors++;
                            ThisMove.completed = true;
                            ThisMove.Dispose();
                            return false;
                            //throw new Exception("Homing faild to reach 0 properly");
                        } else if (ELHomeErr || AZHomeErr || AZCmdErr || ELCmdErr) {
                            consecutiveSuccessfulMoves = 0;
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
            mCUpositon.update(ReadMCURegisters(0, 16));
            var startPos =  mCUpositon as MCUPositonStore;
            Cancel_move( priority );
            Task.Delay( 50 ).Wait();//wait to ensure it is porcessed
            ushort[] CMDdata = prepairRelativeMoveData( SpeedAZ , SpeedEL , ACCELERATION , positionTranslationAZ , positionTranslationEL );

            int AZTime = EstimateMovementTime( SpeedAZ , ACCELERATION , positionTranslationAZ ), ELTime = EstimateMovementTime( SpeedEL , ACCELERATION , positionTranslationEL );
            int TimeToMove;
            if(AZTime > ELTime) {
                TimeToMove = AZTime;
            } else { TimeToMove = ELTime; }
            TimeToMove = ( int)(TimeToMove * 1.2);
            TimeToMove += 100;

            var ThisMove = Send_Generic_Command_And_Track( new MCUCommand( CMDdata , MCUCommandType.RELATIVE_MOVE, priority, positionTranslationAZ > 0, positionTranslationEL > 0, SpeedAZ, SpeedEL ) { EL_ACC = ACCELERATION , AZ_ACC = ACCELERATION,timeout=new CancellationTokenSource( (int)(TimeToMove ) ) } ).GetAwaiter().GetResult();
            Task.Delay( 500 ).Wait();//wait for comand to be read
            bool WasLimitCancled = false;
            PLCLimitChangedEvent handle = ( object sender, limitEventArgs e ) => {
                //var data = TryReadRegs( 0 , 12 ).GetAwaiter().GetResult();
                if(!MotorsCurrentlyMoving()) {
                    ThisMove.timeout.Cancel();
                    WasLimitCancled = true;
                }
            };
            PLCEvents.DurringOverrideAddSecondary( handle);
            while(!ThisMove.timeout.IsCancellationRequested) {
                var data = ReadMCURegisters(0, 12);
                bool azErr = ((data[(int)MCUConstants.MCUOutputRegs.AZ_Status_Bist_MSW] >> (int)MCUConstants.MCUStatusBitsMSW.Command_Error) & 0b1) == 1;
                bool elErr = ((data[(int)MCUConstants.MCUOutputRegs.EL_Status_Bist_MSW] >> (int)MCUConstants.MCUStatusBitsMSW.Command_Error) & 0b1) == 1;
                if(elErr || azErr) {//TODO:add more checks to this 
                    ThisMove.completed = true;
                    ThisMove.CommandError = new Exception( "MCU command error bit was set" );
                    consecutiveSuccessfulMoves = 0;
                    consecutiveErrors++;
                    Send_Generic_Command_And_Track( new MCUCommand( MCUMessages.ResetErrors , MCUCommandType.RESET_ERRORS,0 ) ).Wait();
                    PLCEvents.DuringOverrideRemoveSecondary( handle );
                    ThisMove.Dispose();
                    return false;
                }
                bool azFin = ((data[(int)MCUConstants.MCUOutputRegs.AZ_Status_Bist_MSW] >> (int)MCUConstants.MCUStatusBitsMSW.Move_Complete) & 0b1) == 1;
                bool elFin = ((data[(int)MCUConstants.MCUOutputRegs.EL_Status_Bist_MSW] >> (int)MCUConstants.MCUStatusBitsMSW.Move_Complete) & 0b1) == 1;
                if(azFin && elFin && !MotorsCurrentlyMoving()) {
                    //TODO:check that position is correct and there arent any errors

                    consecutiveSuccessfulMoves++;
                    consecutiveErrors = 0;
                    ThisMove.completed = true;
                    PLCEvents.DuringOverrideRemoveSecondary( handle );
                    ThisMove.Dispose();
                    return true;
                }

            }
            if(MotorsCurrentlyMoving()) {
                ThisMove.completed = true;
                ThisMove.CommandError = new Exception( "Move did not complete in the expected time" );
                consecutiveSuccessfulMoves = 0;
                consecutiveErrors++;
            }else if(WasLimitCancled) {
                ThisMove.completed = true;
                ThisMove.CommandError = new Exception( "Move ended when a limit switch was hit" );
                consecutiveSuccessfulMoves = 0;
                consecutiveErrors++;
            }
            PLCEvents.DuringOverrideRemoveSecondary( handle );
            ThisMove.Dispose();
            return true;
        }

        /// <summary>
        /// Estimate time to complete a single motor's movement based on input values.
        /// </summary>
        /// <param name="maxVelocity">The maximum number of steps per second a motor will run.</param>
        /// <param name="acceleration"></param>
        /// <param name="distance">The total number of motor steps that a movement will take.</param>
        /// <returns>Estimated time that a movement command will take in milliseconds.</returns>
        public static int EstimateMovementTime(int maxVelocity, int acceleration, int distance) {
            //acc steps/millisecond/second
            distance = Math.Abs(distance);
            maxVelocity = Math.Abs(maxVelocity);
            acceleration = Math.Abs(acceleration);

            int t1 = maxVelocity / acceleration;//ms

            double t1s = t1 / 1000.0;

            int distT1 = (int)(acceleration * 1000 / 2 * (t1s * t1s) * 2);

            if (distT1 < distance) {

                int t2 = (distance - distT1) / maxVelocity;

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
            if(RunningCommand.CommandType == MCUCommandType.JOG) {
                //if telescope is already joging changing direction requires stopping first
                ushort[] data3 = new ushort[20];
                data2.CopyTo( data3 , 0 );
                booth = (RunningCommand.AZ_CW != AZClockwise) && (RunningCommand.EL_CW != ELPositive);
                if(booth) {//if both axis need to change direction
                    Cancel_move( priority );
                    WaitUntilStopped().GetAwaiter().GetResult();
                } else if(RunningCommand.EL_CW != ELPositive) {//if only elevation needs to change direction
                    for(int j = 0; j <= data3.Length - 11; j++) {
                        data3[j + 10] = MCUMessages.ClearMove[j + 10];//replace elevation portion of move with controled stop
                    }
                    _ = Send_Generic_Command_And_Track( new MCUCommand( data3 , MCUCommandType.JOG , priority , AZClockwise , ELPositive , AZstepSpeed , ELstepSpeed ) {
                        EL_ACC = MCUConstants.ACTUAL_MCU_MOVE_ACCELERATION_WITH_GEARING ,
                    } ).GetAwaiter().GetResult();
                    WaitUntilStoppedPerAxis( true ).GetAwaiter().GetResult();
                } else if(RunningCommand.AZ_CW != AZClockwise) {//only Azimuth needs to change direction
                    for(int j = 0; j <= data3.Length - 1; j++) {
                        data3[j] = MCUMessages.ClearMove[j];//replace Azimuth portion of move with controled stop
                    }
                    _ = Send_Generic_Command_And_Track( new MCUCommand( data3 , MCUCommandType.JOG , priority , AZClockwise , ELPositive , AZstepSpeed , ELstepSpeed ) {
                        AZ_ACC = MCUConstants.ACTUAL_MCU_MOVE_ACCELERATION_WITH_GEARING ,
                    } ).GetAwaiter().GetResult();
                    WaitUntilStoppedPerAxis( false ).GetAwaiter().GetResult();
                }
            }

            _ = Send_Generic_Command_And_Track( new MCUCommand( data2 , MCUCommandType.JOG , priority , AZClockwise , ELPositive , AZstepSpeed , ELstepSpeed ) {//send the portion of the jog move that was previously replaced with a contoroled stop
                EL_ACC = MCUConstants.ACTUAL_MCU_MOVE_ACCELERATION_WITH_GEARING ,
                AZ_ACC = MCUConstants.ACTUAL_MCU_MOVE_ACCELERATION_WITH_GEARING ,
            } ).GetAwaiter().GetResult();
            return true;
        }

        private async Task<MCUCommand> Send_Generic_Command_And_Track( MCUCommand incoming ) {//TODO: implament priority 
            if((incoming.Move_Priority > RunningCommand.Move_Priority)&& !RunningCommand.completed) {
                return incoming;
            }
            Console.WriteLine( "running: {0} ||| {1}     incoming: {2} ||| {3}", RunningCommand.CommandType.ToString(), RunningCommand.Move_Priority , incoming.CommandType.ToString(),incoming.Move_Priority );
            PreviousCommand = RunningCommand;
            if(RunningCommand.CommandType == MCUCommandType.JOG) {
                if(incoming.CommandType == MCUCommandType.CLEAR_LAST_MOVE || incoming.CommandType == MCUCommandType.IMMEDIATE_STOP || incoming.CommandType == MCUCommandType.JOG) {
                    try {
                        RunningCommand?.timeout?.Cancel();
                    } catch { }
                    RunningCommand = incoming;
                    WriteMCURegisters( MCUConstants.ACTUAL_MCU_WRITE_REGISTER_START_ADDRESS , incoming.commandData );
                    return incoming;
                }
                incoming.CommandError = new Exception( "MCU was running a JOG move which could not be overriden" );
                return incoming;
            } else if((RunningCommand.CommandType == MCUCommandType.HOME && !RunningCommand.completed)) {
                if( incoming.CommandType == MCUCommandType.IMMEDIATE_STOP) {
                    try {
                        RunningCommand?.timeout?.Cancel();
                    } catch { }
                    RunningCommand = incoming;
                    WriteMCURegisters( MCUConstants.ACTUAL_MCU_WRITE_REGISTER_START_ADDRESS , incoming.commandData );
                    return incoming;
                }
                incoming.CommandError = new Exception( "MCU was running a home move which could not be overriden" );
                return incoming;
            }
            try {
                RunningCommand?.timeout?.Cancel();
            } catch { }
            RunningCommand = incoming;
            WriteMCURegisters( MCUConstants.ACTUAL_MCU_WRITE_REGISTER_START_ADDRESS , incoming.commandData );
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
            WriteMCURegisters( (ushort)(MCUConstants.ACTUAL_MCU_WRITE_REGISTER_START_ADDRESS + DataOffset ), data );
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
                data[i] = MCUMessages.ClearMove[i];
            }
            WriteMCURegisters( (ushort)(MCUConstants.ACTUAL_MCU_WRITE_REGISTER_START_ADDRESS + DataOffset) , data );
            RunningCommand.Move_Priority = priority;
            RunningCommand.completed = true;
            return true;
        }

        public long getLastContact() {
            return MCU_last_contact;
        }

        public void setTelescopeType(RadioTelescopeTypeEnum type)
        {
            telescopeType = type;
        }
    }
}
