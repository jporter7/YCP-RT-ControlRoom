using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using ControlRoomApplication.Entities;
using ControlRoomApplication.Main;
using ControlRoomApplication.Database;
using ControlRoomApplication.Util;
using ControlRoomApplication.Constants;
using ControlRoomApplication.Controllers.PLCCommunication.PLCDrivers.MCUManager;
using ControlRoomApplication.Controllers.SensorNetwork;
using ControlRoomApplication.Controllers.PLCCommunication.PLCDrivers.MCUManager.Enumerations;
using ControlRoomApplication.Controllers.Communications;
using ControlRoomApplication.Controllers.Communications.Enumerations;


namespace ControlRoomApplication.Controllers
{
    public class RemoteListener
    {

        private static readonly log4net.ILog logger =
         log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public TcpListener server = null;
        private Thread TCPMonitoringThread;
        private bool KeepTCPMonitoringThreadAlive;
        public RadioTelescopeController rtController;
        private ControlRoom controlRoom;

        public RemoteListener(int port, ControlRoom control)
        {
            logger.Debug(Utilities.GetTimeStamp() + ": Setting up remote listener");
            server = new TcpListener(port);

            controlRoom = control;

            // Start listening for client requests.
            server.Start();

            KeepTCPMonitoringThreadAlive = true;
            // start the listening thread
            TCPMonitoringThread = new Thread(new ThreadStart(TCPMonitoringRoutine));

            TCPMonitoringThread.Start();
        }

        public void TCPMonitoringRoutine()
        {
            // Buffer for reading data
            Byte[] bytes = new Byte[256];
            String data = null;

            // Enter the listening loop.
            while (KeepTCPMonitoringThreadAlive)
            {
                logger.Debug(Utilities.GetTimeStamp() + ": Waiting for a connection... ");

                // Perform a blocking call to accept requests.
                // You could also user server.AcceptSocket() here.
                TcpClient client = server.AcceptTcpClient();
                logger.Debug(Utilities.GetTimeStamp() + ": TCP Client connected!");

                data = null;

                // Get a stream object for reading and writing
                NetworkStream stream = client.GetStream();

                int i;

                // Loop to receive all the data sent by the client.
                // Add try catch to reading
                while ((i = stream.Read(bytes, 0, bytes.Length)) != 0)
                {
                    // Translate data bytes to ASCII string.
                    data = System.Text.Encoding.ASCII.GetString(bytes, 0, i);

                    logger.Debug(Utilities.GetTimeStamp() + ": Received: " + data);

                    // Process the data sent by the client.
                    data = data.ToUpper();

                    byte[] myWriteBuffer = null;

                    // Inform mobile command received 
                    // TODO: Surround any .Write in a try-catch
                    myWriteBuffer = Encoding.ASCII.GetBytes("Received command: " + data);
                    stream.Write(myWriteBuffer, 0, myWriteBuffer.Length);

                    // if processing the data fails, report an error message
                    TCPCommunicationResult tcpCommunicationResult = processMessage(data);
                    // TODO: Send back error messages
                    if (MovementResult.Success != tcpCommunicationResult.movementResult)
                    {
                        //logger.Error(Utilities.GetTimeStamp() + ": Processing data from tcp connection failed!");

                        // send back a failure response
                        // Invalid command
                        myWriteBuffer = Encoding.ASCII.GetBytes(tcpCommunicationResult.errorMessage);
                        
                    }
                    else
                    {
                        // send back a success response -- finished command
                        myWriteBuffer = Encoding.ASCII.GetBytes("SUCCESSFULLY COMPLETED COMMAND: "+data);
                    }

                   
                    // Send message back -- send final state
                    // Surround stream writes in a try catch
                    stream.Write(myWriteBuffer, 0, myWriteBuffer.Length);
                }

                // Shutdown and end connection
                client.Close();
            }
        }

        public bool RequestToKillTCPMonitoringRoutine()
        {
            logger.Info(Utilities.GetTimeStamp() + ": Killing TCP Monitoring Routine");

            KeepTCPMonitoringThreadAlive = false;

            try
            {
                TCPMonitoringThread.Join();
            }
            catch (Exception e)
            {
                if ((e is ThreadStateException) || (e is ThreadInterruptedException))
                {
                    return false;
                }
                else
                { 
                    // Unexpected exception
                    throw e;
                }
            }

            return true;
        }

        private TCPCommunicationResult processMessage(String data)
        {
            // Break our string into different parts to retrieve respective pieces of command
            // Based on the command, we will choose what path to follow
            String[] splitCommandString = data.Trim().Split('|');
            // first check to make sure we have the mininum number of parameters before beginning parsing
            if (splitCommandString.Length < TCPCommunicationConstants.MIN_NUM_PARAMS) return new TCPCommunicationResult(MovementResult.InvalidCommand, 
                ParseTCPCommandResult.MissingCommandArgs, TCPCommunicationConstants.MISSING_COMMAND_ARGS);

            // proceed if valid
            for(int i = 0; i<splitCommandString.Length; i++)
            {
                splitCommandString[i] = splitCommandString[i].Trim();
            }

            // Convert version from string to double. This is the first value in our string before the "|" character.
            // From here we will direct to the appropriate parsing for said version
            double version = 0.0;
            try
            {
                version = Double.Parse(splitCommandString[TCPCommunicationConstants.VERSION_NUM]);
            }
            catch (Exception e)
            {
                String errMessage = TCPCommunicationConstants.VERSION_CONVERSION_ERR + e.Message;
                return new TCPCommunicationResult(MovementResult.InvalidCommand, ParseTCPCommandResult.InvalidVersion, errMessage);
 
            }

            // Use appropriate parsing for given version
            if (version == 1.0)
            {
                // command is placed after pike and before colon; get it here
                // <VERSION> | <COMMANDTYPE> | <NAME<VALUES>> | TIME
                // TODO: use command instead of "contains" and "indexOf"
                string command = splitCommandString[TCPCommunicationConstants.COMMAND_TYPE];
                
                if (command=="ORIENTATION_MOVE")
                {
                    // check to see if we have a valid number of parameters before attempting to parse
                    if (splitCommandString.Length != TCPCommunicationConstants.NUM_ORIENTATION_MOVE_PARAMS)
                    {
                        return new TCPCommunicationResult(MovementResult.InvalidCommand, ParseTCPCommandResult.MissingCommandArgs,
                            TCPCommunicationConstants.MISSING_COMMAND_ARGS);
                    }

                    // we have a move command coming in
                    rtController.ExecuteRadioTelescopeControlledStop(MovementPriority.GeneralStop);

                    // get azimuth and orientation
                    double azimuth = 0.0;
                    double elevation = 0.0;

                    if (splitCommandString.Length < TCPCommunicationConstants.NUM_ORIENTATION_MOVE_PARAMS)
                    {
                        try
                        {
                            azimuth = Convert.ToDouble(splitCommandString[2]);
                            elevation = Convert.ToDouble(splitCommandString[3]);
                        }
                        catch (Exception e)
                        {
                            String errMessage = TCPCommunicationConstants.AZ_EL_CONVERSION_ERR + e.Message;
                            return new TCPCommunicationResult(MovementResult.InvalidCommand, ParseTCPCommandResult.InvalidCommandArgs, errMessage);
                        }
                    }
                    // else we did not receive either azimuth or elevation arguments
                    else
                    {
                        return new TCPCommunicationResult(MovementResult.InvalidCommand,
                            ParseTCPCommandResult.InvalidCommandArgs, TCPCommunicationConstants.MISSING_AZ_EL_ARGS);
                    }

                    logger.Debug(Utilities.GetTimeStamp() + ": Azimuth " + azimuth);
                    logger.Debug(Utilities.GetTimeStamp() + ": Elevation " + elevation);

                    Orientation movingTo = new Orientation(azimuth, elevation);

                    // check result of movement, if it fails we return the result type along with an error message.
                    // The command parse was successful however, so we indicate that
                    MovementResult result = rtController.MoveRadioTelescopeToOrientation(movingTo, MovementPriority.Manual);
                    if (result != MovementResult.Success)
                    {
                        return new TCPCommunicationResult(result, ParseTCPCommandResult.Success, 
                            TCPCommunicationConstants.ORIENTATION_MOVE_ERR + result.ToString());
                    }
                    else
                    // everything was successful
                    {
                        return new TCPCommunicationResult(result, ParseTCPCommandResult.Success);
                    }

                }
                else if (command=="RELATIVE_MOVE")
                {
                    // check to see if we have a valid number of parameters before attempting to parse
                    if(splitCommandString.Length != TCPCommunicationConstants.NUM_RELATIVE_MOVE_PARAMS)
                    {
                        return new TCPCommunicationResult(MovementResult.InvalidCommand, ParseTCPCommandResult.MissingCommandArgs,
                            TCPCommunicationConstants.MISSING_COMMAND_ARGS);
                    }
                    
                    // we have a relative movement command
                    rtController.ExecuteRadioTelescopeControlledStop(MovementPriority.GeneralStop);

                    // get azimuth and orientation
                    double azimuth = 0.0;
                    double elevation = 0.0;

                    if (splitCommandString.Length != TCPCommunicationConstants.NUM_RELATIVE_MOVE_PARAMS) 
                    {
                        // the azimuth and elevation could not be found; return missing args
                        return new TCPCommunicationResult(MovementResult.InvalidCommand, ParseTCPCommandResult.InvalidCommandArgs,
                            TCPCommunicationConstants.MISSING_AZ_EL_ARGS);
                    }
                    else
                    {
                        try
                        {
                            azimuth = Convert.ToDouble(splitCommandString[2]);
                            elevation = Convert.ToDouble(splitCommandString[3]);
                        }
                        catch (Exception e)
                        {
                            String errMessage = TCPCommunicationConstants.AZ_EL_CONVERSION_ERR + e.Message;
                            return new TCPCommunicationResult(MovementResult.InvalidCommand, ParseTCPCommandResult.InvalidCommandArgs, errMessage);
                        }
                    }
                        

                    logger.Debug(Utilities.GetTimeStamp() + ": Azimuth " + azimuth);
                    logger.Debug(Utilities.GetTimeStamp() + ": Elevation " + elevation);

                    Orientation movingBy = new Orientation(azimuth, elevation);

                    MovementResult result =  rtController.MoveRadioTelescopeByXDegrees(movingBy, MovementPriority.Manual);
                    if (result != MovementResult.Success)
                    {
                        // the actual movement failed
                        return new TCPCommunicationResult(result, ParseTCPCommandResult.Success,
                            TCPCommunicationConstants.RELATIVE_MOVE_ERR + result.ToString());
                    }
                    else
                    {
                        // everything was successful
                        return new TCPCommunicationResult(result, ParseTCPCommandResult.Success);
                    }
                }
                else if (command=="SET_OVERRIDE")
                {
                     // Always check to see if we have our correct number of arguments for command type. Return false if not
                    if (splitCommandString.Length < TCPCommunicationConstants.NUM_SENSOR_OVERRIDE_PARAMS)
                    {
                        return new TCPCommunicationResult(MovementResult.InvalidCommand, ParseTCPCommandResult.MissingCommandArgs,
                            TCPCommunicationConstants.MISSING_COMMAND_ARGS);
                    } 
                    // If the true false value is not given, we don't know what to do. Return error
                    else if (splitCommandString[TCPCommunicationConstants.DO_OVERRIDE] != "TRUE" &&
                        splitCommandString[TCPCommunicationConstants.DO_OVERRIDE] != "FALSE")
                    {
                        return new TCPCommunicationResult(MovementResult.InvalidCommand, ParseTCPCommandResult.MissingCommandArgs,
                            TCPCommunicationConstants.MISSING_SET_OVERRIDE_ARG);
                    }
                    else
                    {
                        string sensorToOverride = splitCommandString[TCPCommunicationConstants.SENSOR_TO_OVERRIDE];
                        bool doOverride = splitCommandString[TCPCommunicationConstants.DO_OVERRIDE] == "TRUE" ? true : false;
                        switch (sensorToOverride)
                        {
                            // Non-PLC Overrides
                            case "WEATHER_STATION":
                                controlRoom.weatherStationOverride = doOverride;
                                rtController.setOverride("weather station", doOverride);
                                DatabaseOperations.SetOverrideForSensor(SensorItemEnum.WEATHER_STATION, doOverride);
                                break;

                            // PLC Overrides
                            case "MAIN_GATE":
                                rtController.setOverride("main gate", doOverride);
                                break;

                            // Proximity overrides
                            case "ELEVATION_LIMIT_0":
                                rtController.setOverride("elevation proximity (1)", doOverride);
                                break;

                            case "ELEVATION_LIMIT_90":
                                rtController.setOverride("elevation proximity (2)", doOverride);
                                break;

                            // Sensor network overrides
                            case "AZ_ABS_ENC":
                                rtController.setOverride("azimuth absolute encoder", doOverride);
                                break;

                            case "EL_ABS_ENC":
                                rtController.setOverride("elevation absolute encoder", doOverride);
                                break;

                            case "AZ_ACC":
                                rtController.setOverride("azimuth motor accelerometer", doOverride);
                                break;

                            case "EL_ACC":
                                rtController.setOverride("elevation motor accelerometer", doOverride);
                                break;

                            case "CB_ACC":
                                rtController.setOverride("counterbalance accelerometer", doOverride);
                                break;

                            case "AZIMUTH_MOT_TEMP":
                                rtController.setOverride("azimuth motor temperature", doOverride);
                                break;

                            case "ELEVATION_MOT_TEMP":
                                rtController.setOverride("elevation motor temperature", doOverride);
                                break;
                        
                           // If no case is reached, the sensor is not valid. Return appropriately
                            default: 
                                return new TCPCommunicationResult(MovementResult.InvalidCommand, ParseTCPCommandResult.InvalidCommandArgs,
                                TCPCommunicationConstants.INVALID_SENSOR_OVERRIDE);
                        }

                        return new TCPCommunicationResult(MovementResult.Success, ParseTCPCommandResult.Success);
                    }

                }
                else if (command=="SCRIPT")
                {
                    // Check if we have the correct number of params in our string. If not, no need to begin parsing.
                    if (splitCommandString.Length != TCPCommunicationConstants.NUM_SCRIPT_PARAMS)
                    {
                        return new TCPCommunicationResult(MovementResult.InvalidCommand, ParseTCPCommandResult.MissingCommandArgs, TCPCommunicationConstants.MISSING_COMMAND_ARGS);
                    }

                    // we have a move command coming in
                    rtController.ExecuteRadioTelescopeControlledStop(MovementPriority.GeneralStop);
                    string script = "";

                    // Retrieve script name used for switch case
                    script = splitCommandString[TCPCommunicationConstants.SCRIPT_NAME];
                    MovementResult result = MovementResult.None;

                    logger.Debug(Utilities.GetTimeStamp() + ": Script " + script);

                    switch (script) {

                        case "DUMP":
                            result = rtController.SnowDump(MovementPriority.Manual);
                            break;

                        case "FULL_EV":
                            result = rtController.FullElevationMove(MovementPriority.Manual);
                            break;

                        case "THERMAL_CALIBRATE":
                            result = rtController.ThermalCalibrateRadioTelescope(MovementPriority.Manual);
                            break;

                        case "STOW":
                            result = rtController.MoveRadioTelescopeToOrientation(MiscellaneousConstants.Stow, MovementPriority.Manual);
                            break;

                        case "FULL_CLOCK":
                            result = rtController.MoveRadioTelescopeByXDegrees(new Orientation(360, 0), MovementPriority.Manual);
                            break;

                        case "FULL_COUNTER":
                            result = rtController.MoveRadioTelescopeByXDegrees(new Orientation(-360, 0), MovementPriority.Manual);
                            break;

                        case "HOME":
                            result = rtController.HomeTelescope(MovementPriority.Manual);
                            break;

                        case "HARDWARE_MVMT_SCRIPT":
                            result = rtController.ExecuteHardwareMovementScript(MovementPriority.Manual);
                            break;

                        default:
                            // If no command is found, result = invalid
                            result = MovementResult.InvalidCommand;
                            break;
                    }
                    

                    // Return based off of movement result
                    if (result != MovementResult.Success)
                    {
                        return new TCPCommunicationResult(result, ParseTCPCommandResult.Success, TCPCommunicationConstants.SCRIPT_ERR + script);
                    }
                    else
                    {
                        // everything was successful
                        return new TCPCommunicationResult(result, ParseTCPCommandResult.Success);
                    }
                }
                else if (command=="STOP_RT")
                {
                    bool success = rtController.ExecuteRadioTelescopeControlledStop(MovementPriority.GeneralStop);

                    if (success) return new TCPCommunicationResult(MovementResult.Success, ParseTCPCommandResult.Success);
                    else return new TCPCommunicationResult(MovementResult.TimedOut, ParseTCPCommandResult.Success,
                        TCPCommunicationConstants.ALL_STOP_ERR); // Uses a semaphore to acquire lock so a false means it has timed out or cannot gain access to movement thread

                }
                else if (command=="SENSOR_INIT")
                {
                    // Check for valid number of parameters before continuing parsing
                    if (splitCommandString.Length != TCPCommunicationConstants.NUM_SENSOR_INIT_PARAMS)
                    {
                        return new TCPCommunicationResult(MovementResult.InvalidCommand, ParseTCPCommandResult.MissingCommandArgs,
                         TCPCommunicationConstants.MISSING_SENSOR_INIT_ARGS);
                    }

                    // Retrieve sensor init values from the comma separated portion of the string
                    string[] splitData = splitCommandString[TCPCommunicationConstants.SENSOR_INIT_VALUES].Split(',');

                    // there should be 10, if not, no bueno
                    if (splitData.Length != 10) return new TCPCommunicationResult(MovementResult.InvalidCommand, ParseTCPCommandResult.MissingCommandArgs,
                        TCPCommunicationConstants.MISSING_SENSOR_INIT_ARGS);

                    var config = rtController.RadioTelescope.SensorNetworkServer.InitializationClient.SensorNetworkConfig;

                    // Set all the sensors to their new initialization
                    config.AzimuthTemp1Init = splitData[(int)SensorInitializationEnum.AzimuthTemp].Equals("1");
                    config.ElevationTemp1Init = splitData[(int)SensorInitializationEnum.ElevationTemp].Equals("1");
                    config.AzimuthAccelerometerInit = splitData[(int)SensorInitializationEnum.AzimuthAccelerometer].Equals("1");
                    config.ElevationAccelerometerInit = splitData[(int)SensorInitializationEnum.ElevationAccelerometer].Equals("1");
                    config.CounterbalanceAccelerometerInit = splitData[(int)SensorInitializationEnum.CounterbalanceAccelerometer].Equals("1");
                    config.AzimuthEncoderInit = splitData[(int)SensorInitializationEnum.AzimuthEncoder].Equals("1");
                    config.ElevationEncoderInit = splitData[(int)SensorInitializationEnum.ElevationEncoder].Equals("1");

                    // Set the timeout values
                    config.TimeoutDataRetrieval = (int)(double.Parse(splitData[7]) * 1000);
                    config.TimeoutInitialization = (int)(double.Parse(splitData[8]) * 1000);

                    // Reboot
                    rtController.RadioTelescope.SensorNetworkServer.RebootSensorNetwork();

                    return new TCPCommunicationResult(MovementResult.Success, ParseTCPCommandResult.Success);
                }

                // can't find a keyword then we return Invalid Command sent
                return new TCPCommunicationResult(MovementResult.InvalidCommand, ParseTCPCommandResult.InvalidCommandType, 
                    TCPCommunicationConstants.COMMAND_NOT_FOUND + command);
            }
            // Version is not found; add new versions here
            else
            {
                return new TCPCommunicationResult(MovementResult.InvalidCommand, ParseTCPCommandResult.InvalidVersion,
                    TCPCommunicationConstants.VERSION_NOT_FOUND + version);
            }
        }
    }
}
