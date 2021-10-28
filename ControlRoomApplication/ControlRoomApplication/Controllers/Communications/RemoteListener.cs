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

                    // if processing the data fails, report an error message
                    TCPCommunicationResult tcpCommunicationResult = processMessage(data);
                    // TODO: Send back error messages
                    if (MovementResult.Success != tcpCommunicationResult.movementResult)
                    {
                        logger.Error(Utilities.GetTimeStamp() + ": Processing data from tcp connection failed!");

                        // send back a failure response
                        // Invalid command
                        myWriteBuffer = Encoding.ASCII.GetBytes("FAILURE");
                        
                    }
                    else
                    {
                        // send back a success response -- finished command
                        myWriteBuffer = Encoding.ASCII.GetBytes("SUCCESS");
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
            // Convert version from string to double. This is the first value in our string before the "|" character.
            // From here we will direct to the appropriate parsing for said version
            string versionString = data.Substring(0, data.IndexOf('|'));
            double version = 0.0;
            try
            {
                version = Double.Parse(versionString);
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
                // <VERSION> | <COMMANDTYPE>: <NAME<VALUES>> | TIME
                // TODO: use command instead of "contains" and "indexOf"
                String command = data.Substring(data.IndexOf('|')+1, data.IndexOf('|') - data.IndexOf(":"));
                if (data.IndexOf("ORIENTATION_MOVE:") != -1)
                {
                    // we have a move command coming in
                    rtController.ExecuteRadioTelescopeControlledStop(MovementPriority.GeneralStop);

                    // get azimuth and orientation
                    int azimuthIndex = data.IndexOf("AZ");
                    int elevationIndex = data.IndexOf("EL");
                    double azimuth = 0.0;
                    double elevation = 0.0;

                    if (azimuthIndex != -1 && elevationIndex != -1)
                    {
                        try
                        {
                            azimuth = Convert.ToDouble(data.Substring(azimuthIndex + 3, elevationIndex));
                            elevation = Convert.ToDouble(data.Substring(elevationIndex + 3, data.IndexOf("|")-elevationIndex-3));
                        }
                        catch (Exception e)
                        {
                            String errMessage = TCPCommunicationConstants.AZ_EL_CONVERSION_ERR+e.Message;
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
                else if (data.IndexOf("RELATIVE_MOVE:") != -1)
                {

                    // we have a relative movement command
                    rtController.ExecuteRadioTelescopeControlledStop(MovementPriority.GeneralStop);

                    // get azimuth and orientation
                    int azimuthIndex = data.IndexOf("AZ");
                    int elevationIndex = data.IndexOf("EL");
                    double azimuth = 0.0;
                    double elevation = 0.0;

                    if (azimuthIndex != -1 && elevationIndex != -1)
                    {
                        try
                        {
                            azimuth = Convert.ToDouble(data.Substring(azimuthIndex + 3, elevationIndex));
                            elevation = Convert.ToDouble(data.Substring(elevationIndex + 3, data.IndexOf("|") - elevationIndex - 3));
                        }
                        catch (Exception e)
                        {
                            String errMessage = TCPCommunicationConstants.AZ_EL_CONVERSION_ERR + e.Message;
                            return new TCPCommunicationResult(MovementResult.InvalidCommand, ParseTCPCommandResult.InvalidCommandArgs, errMessage);
                        }
                    }
                    else
                    {
                        // the azimuth and elevation could not be found; return missing args
                        return new TCPCommunicationResult(MovementResult.InvalidCommand, ParseTCPCommandResult.InvalidCommandArgs,
                            TCPCommunicationConstants.MISSING_AZ_EL_ARGS);
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
                else if (data.IndexOf("SET_OVERRIDE:") != -1)
                {
                    // Discovered edge case where specifying nothing would set false. This checks for the user specifying something
                    bool containsOverrideArg = data.Contains("TRUE") || data.Contains("FALSE");

                    if (!containsOverrideArg)
                    {
                        return new TCPCommunicationResult(MovementResult.InvalidCommand, ParseTCPCommandResult.MissingCommandArgs,
                            TCPCommunicationConstants.MISSING_OVERRIDE_ARG);
                    }
                    else
                    {
                        bool doOverride = data.Contains("TRUE");
                        // Non-PLC Overrides
                        if (data.Contains("WEATHER_STATION"))
                        {
                            controlRoom.weatherStationOverride = doOverride;
                            rtController.setOverride("weather station", doOverride);
                            DatabaseOperations.SetOverrideForSensor(SensorItemEnum.WEATHER_STATION, doOverride);
                        }

                        // PLC Overrides
                        else if (data.Contains("MAIN_GATE"))
                        {
                            rtController.setOverride("main gate", doOverride);
                        }

                        // Proximity overrides
                        else if (data.Contains("ELEVATION_LIMIT_0"))
                        {
                            rtController.setOverride("elevation proximity (1)", doOverride);
                        }
                        else if (data.Contains("ELEVATION_LIMIT_90"))
                        {
                            rtController.setOverride("elevation proximity (2)", doOverride);
                        }

                        // Sensor network overrides
                        else if (data.Contains("AZ_ABS_ENC"))
                        {
                            rtController.setOverride("azimuth absolute encoder", doOverride);
                        }
                        else if (data.Contains("EL_ABS_ENC"))
                        {
                            rtController.setOverride("elevation absolute encoder", doOverride);
                        }
                        else if (data.Contains("AZ_ACC"))
                        {
                            rtController.setOverride("azimuth motor accelerometer", doOverride);
                        }
                        else if (data.Contains("EL_ACC"))
                        {
                            rtController.setOverride("elevation motor accelerometer", doOverride);
                        }
                        else if (data.Contains("CB_ACC"))
                        {
                            rtController.setOverride("counterbalance accelerometer", doOverride);
                        }
                        else if (data.Contains("AZIMUTH_MOT_TEMP"))
                        {
                            rtController.setOverride("azimuth motor temperature", doOverride);
                        }
                        else if (data.Contains("ELEVATION_MOT_TEMP"))
                        {
                            rtController.setOverride("elevation motor temperature", doOverride);
                        }
                        else
                        {
                            return new TCPCommunicationResult(MovementResult.InvalidCommand, ParseTCPCommandResult.InvalidCommandArgs,
                                TCPCommunicationConstants.INVALID_SENSOR_OVERRIDE);
                        }

                        return new TCPCommunicationResult(MovementResult.Success, ParseTCPCommandResult.Success);
                    }

                }
                else if (data.IndexOf("SCRIPT:") != -1)
                {
                    // TODO: parse script out of the string and store in variable to be used in switch case
                    // we have a move command coming in
                    rtController.ExecuteRadioTelescopeControlledStop(MovementPriority.GeneralStop);

                    // get azimuth and orientation
                    int colonIndex = data.IndexOf(":");
                    string script = "";
                    MovementResult result = MovementResult.None;

                    script = data.Substring(colonIndex + 2);

                    logger.Debug(Utilities.GetTimeStamp() + ": Script " + script);

                    if (script.Contains("DUMP"))
                    {
                        script = "DUMP";
                        result = rtController.SnowDump(MovementPriority.Manual);
                    }
                    else if (script.Contains("FULL_EV"))
                    {
                        script = "FULL_EV";
                        result = rtController.FullElevationMove(MovementPriority.Manual);
                    }
                    else if (script.Contains("THERMAL_CALIBRATE"))
                    {
                        script = "THERMAL_CALIBRATE";
                        result = rtController.ThermalCalibrateRadioTelescope(MovementPriority.Manual);
                    }
                    else if (script.Contains("STOW"))
                    {
                        script = "STOW";
                        result = rtController.MoveRadioTelescopeToOrientation(MiscellaneousConstants.Stow, MovementPriority.Manual);
                    }
                    else if (script.Contains("FULL_CLOCK"))
                    {
                        script = "FULL_CLOCK";
                        result = rtController.MoveRadioTelescopeByXDegrees(new Orientation(360, 0), MovementPriority.Manual);
                    }
                    else if (script.Contains("FULL_COUNTER"))
                    {
                        script = "FULL_COUNTER";
                        result = rtController.MoveRadioTelescopeByXDegrees(new Orientation(-360, 0), MovementPriority.Manual);
                    }
                    else if (script.Contains("HOME"))
                    {
                        script = "HOME";
                        result = rtController.HomeTelescope(MovementPriority.Manual);
                    }
                    else if (script.Contains("HARDWARE_MVMT_SCRIPT"))
                    {
                        script = "HARDWARE_MVMT_SCRIPT";
                        result = rtController.ExecuteHardwareMovementScript(MovementPriority.Manual);
                    }
                    else
                    {
                        // If no command is found, result = invalid
                        script = "SCRIPT_NOT_FOUND";
                        result = MovementResult.InvalidCommand;
                    }

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
                else if (data.IndexOf("STOP_RT") != -1)
                {
                    bool success = rtController.ExecuteRadioTelescopeControlledStop(MovementPriority.GeneralStop);

                    if (success) return new TCPCommunicationResult(MovementResult.Success, ParseTCPCommandResult.Success);
                    else return new TCPCommunicationResult(MovementResult.TimedOut, ParseTCPCommandResult.Success,
                        TCPCommunicationConstants.ALL_STOP_ERR); // Uses a semaphore to acquire lock so a false means it has timed out or cannot gain access to movement thread

                }
                else if (data.IndexOf("SENSOR_INIT") != -1)
                {
                    string[] splitData = data.Split(',');

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
                    TCPCommunicationConstants.COMMAND_NOT_FOUND);
            }
            // Version is not found; add new versions here
            else
            {
                return new TCPCommunicationResult(MovementResult.InvalidCommand, ParseTCPCommandResult.InvalidVersion,
                    TCPCommunicationConstants.VERSION_NOT_FOUND + versionString);
            }
        }
    }
}
