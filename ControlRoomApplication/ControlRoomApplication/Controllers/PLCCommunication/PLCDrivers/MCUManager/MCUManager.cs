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
    public class MCUManager {
        private static readonly log4net.ILog logger = log4net.LogManager.GetLogger( System.Reflection.MethodBase.GetCurrentMethod().DeclaringType );
        /// <summary>
        /// if more errors than this value are thrown in a row and this class cant resolve them subsiquent attempts to send moves to the MCU will throw exception
        /// </summary>
        private static int MaxConscErrors = 5;
        private int consecutiveErrors = 0;
        private int consecutiveSuccessfulMoves = 0;
        public bool MovementInterruptFlag = false;

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
        private MCUCommand RunningCommand= new MCUCommand(new ushort[20],MCUCommandType.CLEAR_LAST_MOVE) { completed = true };
        private MCUCommand PreviousCommand = new MCUCommand( new ushort[20] , MCUCommandType.CLEAR_LAST_MOVE) { completed = true };

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
        
        /// <summary>
        /// Writes data to the MCU registers based on what motor axis we want to send information to.
        /// </summary>
        /// <remarks>
        /// If elevation or azimuth the selected axis type, the ushort size should only be 10.
        /// If "both" are selected, then the ushort size should be 20. This is the default selection.
        /// If elevation is selected, then an additional offset is required to skip the azimuth registers, which
        /// is automatically handled in this function.
        /// </remarks>
        /// <param name="data">The command data we want to send.</param>
        /// <param name="axis">The motor axis registers we want to modify.</param>
        /// <returns>Whether or not the writing was successful.</returns>
        private bool WriteMCURegisters(ushort[] data, RadioTelescopeAxisEnum axis = RadioTelescopeAxisEnum.BOTH)
        {
            // Offset used to skip azimuth registers if we're only writing to elevation
            ushort offset = 0;
            if (axis == RadioTelescopeAxisEnum.ELEVATION) offset = 10;

            try
            {
                MCUModbusMaster.WriteMultipleRegistersAsync((ushort)(MCUConstants.ACTUAL_MCU_WRITE_REGISTER_START_ADDRESS + offset), data).GetAwaiter().GetResult();
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
            bool inError = false;

            while(HeartbeatMonitorRunning) {

                ushort[] networkStatus = ReadMCURegisters((ushort)MCUConstants.MCUOutputRegs.NetworkConnectivity, 1);

                // If the network status length is 50, it means the network has disconnected, and we must attempt to reconnect
                if (networkStatus.Length == 50)
                {
                    // Only try to connect if it's been more than 5 seconds since the last attempt
                    if ((DateTime.Now - lastConnectAttempt) > TimeSpan.FromSeconds(5))
                    {
                        if (!inError)
                        {
                            logger.Error("MCU network disconnected...");
                            inError = true;
                        }
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
                        inError = false;
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
        public Orientation GetMotorEncoderPosition() {

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
        /// gets the position from MCU step count, thsi should be compaired with the value from <see cref="GetMotorEncoderPosition"/>
        /// </summary>
        /// <remarks>
        /// the MCU traks 2 values for position one that comes from the encoder mounted on the motors shaft, 
        /// the other comes from the MCU keeping track of howmany steps it has told the motor to take
        /// if these numbers get out of sync that means the motors are missing steps and are experiencing a high load,
        /// there are 8_000 encoder counts per revolution of the motor and 20_000 steeps from the MCU so the exact numbers wont match up 
        /// also there will always be some play in the position that the encoder reports +-10 counts should be within acceptabele limts
        /// </remarks>
        /// <returns></returns>
        public Orientation GetMotorStepsPosition() {
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
        public bool Cancel_move() {
            var cmd = new MCUCommand(MCUMessages.ClearBothAxesMove, MCUCommandType.CLEAR_LAST_MOVE) { completed = false };
            SendGenericCommand( cmd );
            WaitUntilStopped();
            cmd.completed = true;
            return true;
        }

        /// <summary>
        /// Attempts to bring the Telescope to a controlled stop, ramping down speed.
        /// Certian moves, such as homing, are unaffected by this.
        /// </summary>
        /// <returns></returns>
        public bool ControlledStop() {
            if(RunningCommand.CommandType == MCUCommandType.JOG) {
                Cancel_move();
            } else {
                var cmd = new MCUCommand(MCUMessages.HoldMove , MCUCommandType.HOLD_MOVE) { completed = false };
                SendGenericCommand( cmd );
                WaitUntilStopped();
                cmd.completed = true;
            }
            return true;
        }

        /// <summary>
        /// Immediately stops the telescope movement with no ramp down in speed.
        /// Homing is unaffected by this command.
        /// </summary>
        /// <returns></returns>
        public bool ImmediateStop() {
            SendGenericCommand(new MCUCommand( MCUMessages.ImmediateStop , MCUCommandType.IMMEDIATE_STOP) { completed = true } );
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
            SendGenericCommand(new MCUCommand(MCUMessages.ResetErrors, MCUCommandType.RESET_ERRORS) { completed = true });
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

        /// <summary>
        /// This function assumes that you have already told both axes to stop moving, otherwise it will time out.
        /// </summary>
        /// <returns>False if the telescope is still moving at the end of the timeout.</returns>
        private bool WaitUntilStopped() {
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
                if(CMD.AzimuthDirection == RadioTelescopeDirectionEnum.CounterclockwiseOrPositive) {
                    StepsAZ = -StepsAZ;
                }
            }
            if(CMD.EL_Programed_Speed > 0) {
                int mS_To_DecelerateEL = (int)((1.25 * (CMD.EL_Programed_Speed - AZStartSpeed) / (double)CMD.EL_ACC) / 1000.0);
                StepsEL = (int)(mS_To_DecelerateEL * ((CMD.EL_Programed_Speed + ConversionHelper.RPMToSPS( Current_ELConfiguration.StartSpeed , MotorConstants.GEARING_RATIO_ELEVATION )) / 2.0));
                StepsEL += (int)(CMD.EL_Programed_Speed * 0.25);//add 100 ms worth of steps
                if(CMD.ElevationDirection == RadioTelescopeDirectionEnum.CounterclockwiseOrPositive) {
                    StepsEL = -StepsEL;
                }
            }
            return new Orientation( ConversionHelper.StepsToDegrees( StepsAZ , MotorConstants.GEARING_RATIO_AZIMUTH ) , ConversionHelper.StepsToDegrees( StepsEL , MotorConstants.GEARING_RATIO_ELEVATION ) );
        }

        /// <summary>
        /// This function assumes that you have already told both Axes to stop moving, otherwise it will time out.
        /// </summary>
        /// <returns>False if the telescope was still running at the end of the timeout</returns>
        private bool WaitUntilStoppedPerAxis(RadioTelescopeAxisEnum axis) {

            int msToDecelerate = 0;

            // Calculate timeout based on the axis
            if(axis == RadioTelescopeAxisEnum.AZIMUTH) {
                msToDecelerate = (int)1.25 * (PreviousCommand.AZ_Programed_Speed - AZStartSpeed) / PreviousCommand.AZ_ACC;
            } else {
                msToDecelerate = (int)1.25 * (PreviousCommand.EL_Programed_Speed - ELStartSpeed) / PreviousCommand.EL_ACC;
            }

            CancellationToken timeout = new CancellationTokenSource(msToDecelerate).Token;

            // Wait for the axis to stop. If it takes longer than the estimated time to decelerate, then
            // the movement failed and we return false.
            try
            {
                while (!timeout.IsCancellationRequested)
                {
                    if (!MotorsCurrentlyMoving(axis))
                    {
                        return true;
                    }
                }
            }
            catch
            {
                return false;
            }

            return false;

        }

        /// <summary>
        /// Tells us if the motors are currently moving. This will automatically return if both motors are moving
        /// if no parameter is passed through.
        /// </summary>
        /// <remarks>
        /// This code could, technically, be shortened so we read all the registers in the beginning and decide
        /// what to do with them in the switch, but my idea here was to only read the registers we need to ensure
        /// the fastest possible execution time.
        /// </remarks>
        /// <param name="axis">The axis we want to check is moving.</param>
        /// <returns>True if they are moving clockwise or counter-clockwise, false if they are still.</returns>
        public bool MotorsCurrentlyMoving(RadioTelescopeAxisEnum axis = RadioTelescopeAxisEnum.BOTH) {

            bool isMoving = false;
            ushort[] data;

            switch(axis)
            {
                case RadioTelescopeAxisEnum.AZIMUTH:
                    // Only read the registers we need
                    data = ReadMCURegisters(0, 1);

                    // Check if the azimuth motor is spinning clockwise or counter-clockwise
                    isMoving = (((data[(int)MCUConstants.MCUOutputRegs.AZ_Status_Bist_MSW] >> (int)MCUConstants.MCUStatusBitsMSW.CCW_Motion) & 0b1) == 1) ||
                            (((data[(int)MCUConstants.MCUOutputRegs.AZ_Status_Bist_MSW] >> (int)MCUConstants.MCUStatusBitsMSW.CW_Motion) & 0b1) == 1);
                    break;

                case RadioTelescopeAxisEnum.ELEVATION:
                    // Only read the registers we need
                    data = ReadMCURegisters(10, 1);
                    isMoving = (((data[(int)MCUConstants.MCUOutputRegs.EL_Status_Bist_MSW] >> (int)MCUConstants.MCUStatusBitsMSW.CCW_Motion) & 0b1) == 1) ||
                            (((data[(int)MCUConstants.MCUOutputRegs.EL_Status_Bist_MSW] >> (int)MCUConstants.MCUStatusBitsMSW.CW_Motion) & 0b1) == 1);
                    break;

                case RadioTelescopeAxisEnum.BOTH:
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

        /// <summary>
        /// Will read the MCU bits to tell whether a movement has completed or not.
        /// </summary>
        /// <param name="axis">What axis you want to know the completion status of.</param>
        /// <returns>Whether an axis' movement has completed or not.</returns>
        public bool MovementCompleted(RadioTelescopeAxisEnum axis = RadioTelescopeAxisEnum.BOTH)
        {
            bool isFinished = false;
            ushort[] data;

            switch (axis)
            {
                case RadioTelescopeAxisEnum.AZIMUTH:
                    // Only read the registers we need
                    data = ReadMCURegisters(0, 1);
                    isFinished = ((data[(int)MCUConstants.MCUOutputRegs.AZ_Status_Bist_MSW] >> (int)MCUConstants.MCUStatusBitsMSW.Move_Complete) & 0b1) == 1;
                    break;

                case RadioTelescopeAxisEnum.ELEVATION:
                    // Only read the registers we need
                    data = ReadMCURegisters(10, 1);
                    isFinished = ((data[(int)MCUConstants.MCUOutputRegs.EL_Status_Bist_MSW] >> (int)MCUConstants.MCUStatusBitsMSW.Move_Complete) & 0b1) == 1;
                    break;

                case RadioTelescopeAxisEnum.BOTH:
                    data = ReadMCURegisters(0, 11);
                    ushort dataISent = data[(int)MCUConstants.MCUOutputRegs.AZ_Status_Bist_MSW];
                    var resultOfShift = dataISent >> (int)MCUConstants.MCUStatusBitsMSW.Move_Complete;
                    var resultOfAnd = resultOfShift & 0b1;
                    bool azFinished = ((data[(int)MCUConstants.MCUOutputRegs.AZ_Status_Bist_MSW] >> (int)MCUConstants.MCUStatusBitsMSW.Move_Complete) & 0b1) == 1;
                    bool elFinished = ((data[(int)MCUConstants.MCUOutputRegs.EL_Status_Bist_MSW] >> (int)MCUConstants.MCUStatusBitsMSW.Move_Complete) & 0b1) == 1;

                    // Check if azimuth or elevation are finished with their movements
                    isFinished = azFinished && elFinished;
                    break;
            }

            return isFinished;
        }

        public bool Configure_MCU(MCUConfigurationAxys AZconfig , MCUConfigurationAxys ELconfig) {
            Current_AZConfiguration = AZconfig;
            Current_ELConfiguration = ELconfig;
            int gearedSpeedAZ = ConversionHelper.RPMToSPS( AZconfig.StartSpeed , MotorConstants.GEARING_RATIO_AZIMUTH );
            int gearedSpeedEL = ConversionHelper.RPMToSPS( ELconfig.StartSpeed , MotorConstants.GEARING_RATIO_ELEVATION );
            AZStartSpeed = gearedSpeedAZ;
            ELStartSpeed = gearedSpeedEL;
            TestDefaultParams( AZconfig.StartSpeed , ELconfig.StartSpeed , AZconfig.HomeTimeoutSec , ELconfig.HomeTimeoutSec );
            ushort[] data = {   MakeMcuConfMSW(AZconfig), MakeMcuConfLSW(AZconfig) , (ushort)(gearedSpeedAZ >> 0x0010), (ushort)(gearedSpeedAZ & 0xFFFF), 0x0,0x0,0x0,0x0,0x0,0x0,
                                MakeMcuConfMSW(ELconfig), MakeMcuConfLSW(ELconfig), (ushort)(gearedSpeedEL >> 0x0010), (ushort)(gearedSpeedEL & 0xFFFF), 0x0,0x0,0x0,0x0,0x0,0x0 };
            ImmediateStop();
            SendGenericCommand( new MCUCommand( data , MCUCommandType.CONFIGURE) { completed = true} );

            // Allow the MCU to configure
            Thread.Sleep(250);

            // If there are any errors present, return false
            if (CheckMCUErrors().Count > 0) return false;
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
        /// Sends a home command and waits for the MCU to finish homing.
        /// </summary>
        /// <param name="RPM"></param>
        /// <returns></returns>
        public MovementResult HomeBothAxes(double RPM) {

            MovementResult result = MovementResult.None;

            int EL_Speed = ConversionHelper.DPSToSPS( ConversionHelper.RPMToDPS( RPM ) , MotorConstants.GEARING_RATIO_ELEVATION );
            int AZ_Speed = ConversionHelper.DPSToSPS( ConversionHelper.RPMToDPS( RPM ) , MotorConstants.GEARING_RATIO_AZIMUTH );

            //set config word to 0x0040 to have the RT home at the minimumum speed// this requires the MCU to be configured properly
            ushort[] data = {
                // azimuth data
                (ushort)RadioTelescopeDirectionEnum.CounterclockwiseHoming,
                0x0000,
                0x0000,
                0x0000,
                (ushort)((AZ_Speed & 0xFFFF0000)>>16),
                (ushort)(AZ_Speed & 0xFFFF),
                ACTUAL_MCU_MOVE_ACCELERATION_WITH_GEARING,
                ACTUAL_MCU_MOVE_ACCELERATION_WITH_GEARING,
                0x0000,
                0x0000,

                // elevation data
                (ushort)RadioTelescopeDirectionEnum.CounterclockwiseHoming,
                0x0000,
                0x0000,
                0x0000,
                (ushort)((EL_Speed & 0xFFFF0000)>>16),
                (ushort)(EL_Speed & 0xFFFF),
                ACTUAL_MCU_MOVE_ACCELERATION_WITH_GEARING,
                ACTUAL_MCU_MOVE_ACCELERATION_WITH_GEARING,
                0x0000,
                0x0000
            };

            // Calculate which axis has the longest timeout, then set the timeout as that axis
            int timeout;
            if(Current_AZConfiguration.HomeTimeoutSec > Current_ELConfiguration.HomeTimeoutSec) {
                timeout = Current_AZConfiguration.HomeTimeoutSec;
            } else {
                timeout = Current_ELConfiguration.HomeTimeoutSec;
            }

            // Builds the MCU command and then sends it
            var ThisMove = SendGenericCommand(new MCUCommand(data, MCUCommandType.HOME) {
                AZ_Programed_Speed = AZ_Speed,
                EL_Programed_Speed = EL_Speed,
                EL_ACC = ACTUAL_MCU_MOVE_ACCELERATION_WITH_GEARING,
                AZ_ACC = ACTUAL_MCU_MOVE_ACCELERATION_WITH_GEARING,
                timeout = new CancellationTokenSource( (int)(timeout*1200) ) //* 1000 for seconds to ms //* 1.2 for a 20% margin 
            });

            // ERROR AND STATUS CHECKING BEGINS HERE

            // 140 samples at 1 sample/50mS = 7 seconds of data
            FixedSizedQueue<MCUPositonStore> positionHistory = new FixedSizedQueue<MCUPositonStore>( 140 );
            ushort[] outputRegData;

            // This monitors the homing routine to make sure the motors are still moving and, when they stop,
            // that their position is zeroed out.
            while (!ThisMove.timeout.IsCancellationRequested && result == MovementResult.None)
            {
                // allow the MCU process the register data
                Thread.Sleep(100);
                outputRegData = ReadMCURegisters(0, 16);
                mCUpositon.update(outputRegData);
                positionHistory.Enqueue(new MCUPositonStore(mCUpositon));
                
                // As soon as the motor steps are approximately 0 and no longer moving, we know homing has finished and the motors are zeroed
                if (Math.Abs( mCUpositon.AzSteps ) == 0 && Math.Abs( mCUpositon.ElSteps ) == 0 && !MotorsCurrentlyMoving()) {
                    consecutiveSuccessfulMoves++;
                    consecutiveErrors = 0;
                    result = MovementResult.Success;
                }

                // This checks the new recorded position and compares it against the last
                if(positionHistory.Count > positionHistory.Size - 2) {
                    var movement = positionHistory.GetAbsolutePosChange();

                    // If the motor encoders have not moved at least 50 steps, then they have been still for 7 seconds
                    if(movement.AzEncoder < 50 && movement.ElEncoder < 50)
                    {

                        // If the motors are not zeroed out, then homing did not complete (aka failed)
                        if (Math.Abs(mCUpositon.AzSteps) > 4 || Math.Abs(mCUpositon.ElSteps) > 4)
                        {
                            consecutiveSuccessfulMoves = 0;
                            consecutiveErrors++;
                            result = MovementResult.IncorrectPosition;
                        }

                        // If there are any errors in the registers, something bad happened and homing failed
                        else if (CheckMCUErrors().Count > 0)
                        {
                            consecutiveSuccessfulMoves = 0;
                            consecutiveErrors++;
                            result = MovementResult.McuErrorBitSet;
                        }
                    }
                }
            }

            ThisMove.completed = true;
            ThisMove.Dispose();
            return result;
        }

        private ushort[] PrepareRelativeMoveData(int SpeedAZ, int SpeedEL, int positionTranslationAZ, int positionTranslationEL) {
            if (SpeedAZ < AZStartSpeed) {
                throw new ArgumentOutOfRangeException("SpeedAZ", SpeedAZ,
                    String.Format("SpeedAZ should be greater than {0} which is the stating speed set when configuring the MCU", AZStartSpeed));
            }
            if (SpeedEL < ELStartSpeed) {
                throw new ArgumentOutOfRangeException("SpeedEL", SpeedEL,
                    String.Format("SpeedAZ should be greater than {0} which is the stating speed set when configuring the MCU", ELStartSpeed));
            }

            // This needs flipped so that the elevation axis moves the correct direction
            positionTranslationEL = -positionTranslationEL;

            ushort[] data = {
                // Azimuth data
                0x0002,
                0x0003,
                (ushort)((positionTranslationAZ & 0xFFFF0000)>>16),
                (ushort)(positionTranslationAZ & 0xFFFF),
                (ushort)((SpeedAZ & 0xFFFF0000)>>16),
                (ushort)(SpeedAZ & 0xFFFF),
                ACTUAL_MCU_MOVE_ACCELERATION_WITH_GEARING,
                ACTUAL_MCU_MOVE_ACCELERATION_WITH_GEARING,
                0,
                0,

                // Elevation data
                0x0002,
                0x0003,
                (ushort)((positionTranslationEL & 0xFFFF0000)>>16),
                (ushort)(positionTranslationEL & 0xFFFF),
                (ushort)((SpeedEL & 0xFFFF0000)>>16),
                (ushort)(SpeedEL & 0xFFFF),
                ACTUAL_MCU_MOVE_ACCELERATION_WITH_GEARING,
                ACTUAL_MCU_MOVE_ACCELERATION_WITH_GEARING,
                0,
                0
            };
            return data;
        }

        /// <summary>
        /// perform the specified move and wait for it to be completted by the telescope
        /// </summary>
        /// <param name="SpeedAZ"></param>
        /// <param name="SpeedEL"></param>
        /// <param name="positionTranslationAZ"></param>
        /// <param name="positionTranslationEL"></param>
        /// <param name="targetOrientation"></param>
        /// <returns></returns>
        public MovementResult MoveAndWaitForCompletion(int SpeedAZ, int SpeedEL, int positionTranslationAZ, int positionTranslationEL, 
                Orientation targetOrientation = null) 
        {
            MovementResult result = MovementResult.None;

            // In case another move was running, cancel it
            Cancel_move();
            
            // Update the current position
            mCUpositon.update(ReadMCURegisters(0, 16));

            // Build command data
            ushort[] CMDdata = PrepareRelativeMoveData(
                SpeedAZ, 
                SpeedEL, 
                positionTranslationAZ, 
                positionTranslationEL
            );

            // Estimate the time to move, which will be the axis that takes the longest
            int AZTime = EstimateMovementTime(SpeedAZ, positionTranslationAZ);
            int ELTime = EstimateMovementTime(SpeedEL, positionTranslationEL);
            int TimeToMove;
            if(AZTime > ELTime) {
                TimeToMove = AZTime;
            } else { TimeToMove = ELTime; }

            // Add on some extra time so the timeout doesn't end early
            TimeToMove = ( int)(TimeToMove * 1.2);
            TimeToMove += 100;

            // Calculate azimuth movement direction
            RadioTelescopeDirectionEnum azDirection;
            if (positionTranslationAZ > 0) azDirection = RadioTelescopeDirectionEnum.ClockwiseOrNegative;
            else azDirection = RadioTelescopeDirectionEnum.CounterclockwiseOrPositive;

            // Calculate elevation movement direction
            RadioTelescopeDirectionEnum elDirection;
            if (positionTranslationEL > 0) elDirection = RadioTelescopeDirectionEnum.CounterclockwiseOrPositive;
            else elDirection = RadioTelescopeDirectionEnum.ClockwiseOrNegative;

            // Build and send the MCU Command (the data is the only part sent to the MCU; the rest is for inner tracking)
            var ThisMove = SendGenericCommand( 
                new MCUCommand( 
                    CMDdata, 
                    MCUCommandType.RELATIVE_MOVE, 
                    azDirection, 
                    elDirection, 
                    SpeedAZ, 
                    SpeedEL
                ) {
                    EL_ACC = ACTUAL_MCU_MOVE_ACCELERATION_WITH_GEARING,
                    AZ_ACC = ACTUAL_MCU_MOVE_ACCELERATION_WITH_GEARING,
                    timeout = new CancellationTokenSource(TimeToMove)
                }
            );

            // ERROR CHECKING DURING MOVEMENT. This is the MCU Monitoring Routine described in issue #333

            // This will loop through and monitor MCU movement as long as the following conditions are true:
            // Cannot be timed out
            // Interrupt flag must be false
            // No MCU errors
            // The movement result cannot have been set yet. As soon as it is set, the routine ends.
            while(!ThisMove.timeout.IsCancellationRequested && !MovementInterruptFlag && CheckMCUErrors().Count == 0 && result == MovementResult.None) {
                Thread.Sleep(100);
                if (MovementCompleted() && !MotorsCurrentlyMoving()) {
                    // TODO: Check that the position is correct
                    if (targetOrientation != null)
                    {
                        Orientation encoderOrientation = GetMotorEncoderPosition();
                        // TODO: these have some margin of error, we need to account for that now
                        //       right now these are always "exact" for the VR simulator
                        //       something like a 10th of a degree delta
                        if (((int) encoderOrientation.Azimuth != (int) targetOrientation.Azimuth) || ((int) encoderOrientation.Elevation != (int) targetOrientation.Elevation))
                        {
                            // something bad happened
                            result = MovementResult.IncorrectPosition;
                        } else
                        {
                            consecutiveSuccessfulMoves++;
                            consecutiveErrors = 0;
                            result = MovementResult.Success;
                        }
                    } else
                    {
                        consecutiveSuccessfulMoves++;
                        consecutiveErrors = 0;
                        result = MovementResult.Success;
                    }
                }
            }

            List<Tuple<MCUOutputRegs, MCUStatusBitsMSW>> errors = CheckMCUErrors();

            bool hitLimitSwitch = 
                errors.Contains(new Tuple<MCUOutputRegs, MCUStatusBitsMSW>(MCUOutputRegs.AZ_Status_Bist_MSW, MCUStatusBitsMSW.Input_Error)) ||
                errors.Contains(new Tuple<MCUOutputRegs, MCUStatusBitsMSW>(MCUOutputRegs.EL_Status_Bist_MSW, MCUStatusBitsMSW.Input_Error));

            // If a limit switch gets hit, this is reached. We do NOT want to stop
            // the motors if this happens. All other errors entail stopping the motors.
            if (hitLimitSwitch)
            {
                result = MovementResult.LimitSwitchHit;
                ResetMCUErrors();
            }
            else
            {
                // If the movement times out, this is reached
                if (ThisMove.timeout.IsCancellationRequested)
                {
                    result = MovementResult.TimedOut;
                }

                // If there are errors on the MCU registers, this is reached
                else if (errors.Count > 0)
                {
                    result = MovementResult.McuErrorBitSet;
                }

                // If the movement was voluntarily interrupted, this is reached
                else if (MovementInterruptFlag)
                {
                    MovementInterruptFlag = false;
                    result = MovementResult.Interrupted;
                }

                ControlledStop();
            }
            
            if (result != MovementResult.Success)
            {
                consecutiveSuccessfulMoves = 0;
                consecutiveErrors++;
            }

            ThisMove.completed = true;

            ThisMove.timeout.Cancel();
            ThisMove.Dispose();

            return result;
        }

        /// <summary>
        /// Estimate time to complete a single motor's movement based on input values.
        /// </summary>
        /// <param name="maxVelocity">The maximum number of steps per second a motor will run.</param>
        /// <param name="acceleration"></param>
        /// <param name="distance">The total number of motor steps that a movement will take.</param>
        /// <returns>Estimated time that a movement command will take in milliseconds.</returns>
        public static int EstimateMovementTime(int maxVelocity, int distance) {
            //acc steps/millisecond/second
            distance = Math.Abs(distance);
            maxVelocity = Math.Abs(maxVelocity);

            int t1 = maxVelocity / ACTUAL_MCU_MOVE_ACCELERATION_WITH_GEARING;//ms

            double t1s = t1 / 1000.0;

            int distT1 = (int)(ACTUAL_MCU_MOVE_ACCELERATION_WITH_GEARING * 1000 / 2 * (t1s * t1s) * 2);

            if (distT1 < distance) {

                int t2 = (distance - distT1) / maxVelocity;

                return t2*1000 + (2 * t1);

            } else {
                return 2 * t1;
            }
        }

        public bool SendBothAxesJog( double AZspeed, RadioTelescopeDirectionEnum azDirection, double ELspeed, RadioTelescopeDirectionEnum elDirection) {

            int AZstepSpeed = ConversionHelper.RPMToSPS( AZspeed , MotorConstants.GEARING_RATIO_AZIMUTH );
            int ELstepSpeed = ConversionHelper.RPMToSPS( ELspeed , MotorConstants.GEARING_RATIO_ELEVATION );
            ushort[] data = new ushort[10] { (ushort)azDirection , 0x0003 , 0x0 , 0x0 , (ushort)(AZstepSpeed >> 16) , (ushort)(AZstepSpeed & 0xffff) , MCUConstants.ACTUAL_MCU_MOVE_ACCELERATION_WITH_GEARING , MCUConstants.ACTUAL_MCU_MOVE_ACCELERATION_WITH_GEARING , 0x0 , 0x0 , };
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

                data2[10] = (ushort)(elDirection);
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
                booth = (RunningCommand.AzimuthDirection != azDirection) && (RunningCommand.ElevationDirection != elDirection);
                if(booth) {//if both axis need to change direction
                    Cancel_move();
                    WaitUntilStopped();
                } else if(RunningCommand.ElevationDirection != elDirection) {//if only elevation needs to change direction
                    for(int j = 0; j <= data3.Length - 11; j++) {
                        // TODO: Replace ClearBothAxesMove with simplified code and ClearSingleAxisMove (issue #390)
                        data3[j + 10] = MCUMessages.ClearBothAxesMove[j + 10];//replace elevation portion of move with controled stop
                    }
                    _ = SendGenericCommand( new MCUCommand( data3 , MCUCommandType.JOG, azDirection , elDirection , AZstepSpeed , ELstepSpeed ) {
                        EL_ACC = MCUConstants.ACTUAL_MCU_MOVE_ACCELERATION_WITH_GEARING ,
                    } );
                    WaitUntilStoppedPerAxis(RadioTelescopeAxisEnum.AZIMUTH);
                } else if(RunningCommand.AzimuthDirection != azDirection) {//only Azimuth needs to change direction
                    for(int j = 0; j <= data3.Length - 1; j++)
                    {
                        // TODO: Replace ClearBothAxesMove with simplified code and ClearSingleAxisMove (issue #390)
                        data3[j] = MCUMessages.ClearBothAxesMove[j];//replace Azimuth portion of move with controled stop
                    }
                    _ = SendGenericCommand( new MCUCommand( data3 , MCUCommandType.JOG, azDirection , elDirection , AZstepSpeed , ELstepSpeed ) {
                        AZ_ACC = MCUConstants.ACTUAL_MCU_MOVE_ACCELERATION_WITH_GEARING ,
                    } );
                    WaitUntilStoppedPerAxis(RadioTelescopeAxisEnum.ELEVATION);
                }
            }

            _ = SendGenericCommand( new MCUCommand( data2 , MCUCommandType.JOG, azDirection , elDirection , AZstepSpeed , ELstepSpeed ) {//send the portion of the jog move that was previously replaced with a contoroled stop
                EL_ACC = MCUConstants.ACTUAL_MCU_MOVE_ACCELERATION_WITH_GEARING ,
                AZ_ACC = MCUConstants.ACTUAL_MCU_MOVE_ACCELERATION_WITH_GEARING ,
            } );
            return true;
        }

        private MCUCommand SendGenericCommand(MCUCommand incoming) {

            Console.WriteLine( "running: {0}     incoming: {1}", RunningCommand.CommandType.ToString(), incoming.CommandType.ToString());

            RunningCommand = incoming;
            WriteMCURegisters(incoming.commandData);
            return incoming;
        }

        /// <summary>
        /// This should only be used to back off of limit switches.
        /// For any other jog moves, use the <see cref="SendBothAxesJog"/>
        /// </summary>
        /// <param name="axis">What axis (elevation or azimuth) is spinning.</param>
        /// <param name="direction">Denotes what direction a motor will be spinning.</param>
        /// <param name="speed">What speed the motor will be spinning at.</param>
        /// <returns></returns>
        public bool SendSingleAxisJog(RadioTelescopeAxisEnum axis, RadioTelescopeDirectionEnum direction, double speed) {
            int stepSpeed;

            if(axis == RadioTelescopeAxisEnum.AZIMUTH) {
                stepSpeed = ConversionHelper.RPMToSPS(speed, MotorConstants.GEARING_RATIO_AZIMUTH);
            } else {
                stepSpeed = ConversionHelper.RPMToSPS(speed, MotorConstants.GEARING_RATIO_ELEVATION);
            }

            ushort[] data = new ushort[10] {
                (ushort)direction,
                0x0003,
                0x0,
                0x0,
                (ushort)(stepSpeed >> 16),
                (ushort)(stepSpeed & 0xffff),
                MCUConstants.ACTUAL_MCU_MOVE_ACCELERATION_WITH_GEARING,
                MCUConstants.ACTUAL_MCU_MOVE_ACCELERATION_WITH_GEARING,
                0x0,
                0x0
            };

            WriteMCURegisters(data, axis);

            RunningCommand.completed = false;

            return true;
        }

        /// <summary>
        /// This should only be used to stop the jog command from backing off limit switches.
        /// </summary>
        /// <param name="axis">The motor axis we are stopping.</param>
        /// <returns></returns>
        public bool StopSingleAxisJog(RadioTelescopeAxisEnum axis) {
            WriteMCURegisters(MCUMessages.ClearOneAxisMove, axis);
            RunningCommand.completed = true;
            return true;
        }

        public long getLastContact() {
            return MCU_last_contact;
        }

        /// <summary>
        /// Sets the telescope type so we know if we should be normalizing the azimuth orientation or not.
        /// </summary>
        /// <param name="type">The telescope type we are setting the MCU manager telescope type to.</param>
        public void setTelescopeType(RadioTelescopeTypeEnum type)
        {
            telescopeType = type;
        }
    }
}
