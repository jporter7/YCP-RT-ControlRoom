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
                while ((i = stream.Read(bytes, 0, bytes.Length)) != 0)
                {
                    // Translate data bytes to ASCII string.
                    data = System.Text.Encoding.ASCII.GetString(bytes, 0, i);

                    logger.Debug(Utilities.GetTimeStamp() + ": Received: " + data);

                    // Process the data sent by the client.
                    data = data.ToUpper();

                    byte[] myWriteBuffer = null;

                    // if processing the data fails, report an error message
                    if (!processMessage(data))
                    {
                        logger.Error(Utilities.GetTimeStamp() + ": Processing data from tcp connection failed!");

                        // send back a failure response
                        myWriteBuffer = Encoding.ASCII.GetBytes("FAILURE");
                        
                    }
                    else
                    {
                        // send back a success response
                        myWriteBuffer = Encoding.ASCII.GetBytes("SUCCESS");
                    }

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

        private bool processMessage(String data)
        {

            if (data.IndexOf("COORDINATE_MOVE") != -1)
            {
                // we have a move command coming in
                rtController.ExecuteRadioTelescopeControlledStop(MovementPriority.GeneralStop);

                // get azimuth and orientation
                int azimuthIndex = data.IndexOf("AZIM");
                int elevationIndex = data.IndexOf("ELEV");
                int idIndex = data.IndexOf("ID");
                double azimuth = 0.0;
                double elevation = 0.0;
                string userId = "";

                if (azimuthIndex != -1 && elevationIndex != -1 && idIndex != -1)
                {
                    elevation = Convert.ToDouble(data.Substring(elevationIndex + 5, azimuthIndex - elevationIndex - 5));
                    azimuth = Convert.ToDouble(data.Substring(azimuthIndex + 5, idIndex - azimuthIndex - 5));
                    userId = data.Substring(idIndex + 3);
                }
                else
                    return false;

                logger.Debug(Utilities.GetTimeStamp() + ": Azimuth " + azimuth);
                logger.Debug(Utilities.GetTimeStamp() + ": Elevation " + elevation);

                Orientation movingTo = new Orientation(azimuth, elevation);

                rtController.MoveRadioTelescopeToOrientation(movingTo, MovementPriority.Manual);

                // TODO: store the User Id and movement somewhere in the database (issue #392)

                return true;

            }
            else if (data.IndexOf("SET_OVERRIDE") != -1)
            {
                // false = ENABLED; true = OVERRIDING
                // If "OVR" is contained in the data, will return true
                bool doOverride = data.Contains("OVR");

                // Non-PLC Overrides
                if (data.Contains("WEATHER_STATION"))
                {
                    controlRoom.weatherStationOverride = data.Contains("OVR");
                    rtController.setOverride("weather station", data.Contains("OVR"));
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
                else return false;

                return true;
            }
            else if (data.IndexOf("SCRIPT") != -1)
            {
                // we have a move command coming in
                rtController.ExecuteRadioTelescopeControlledStop(MovementPriority.GeneralStop);

                // get azimuth and orientation
                int colonIndex = data.IndexOf(":");
                string script = "";

                if (colonIndex != -1)
                {
                    script = data.Substring(colonIndex + 2);
                }
                else
                    return false;

                logger.Debug(Utilities.GetTimeStamp() + ": Script " + script);

                if (script.Contains("DUMP"))
                {
                    rtController.SnowDump(MovementPriority.Manual);
                }
                else if (script.Contains("FULL_EV"))
                {
                    rtController.FullElevationMove(MovementPriority.Manual);
                }
                else if (script.Contains("CALIBRATE"))
                {
                    rtController.ThermalCalibrateRadioTelescope(MovementPriority.Manual);
                }
                else if (script.Contains("STOW"))
                {
                    rtController.MoveRadioTelescopeToOrientation(MiscellaneousConstants.Stow, MovementPriority.Manual);
                }
                else if (script.Contains("FULL_CLOCK"))
                {
                    // TODO: Implement with MoveByX function (issue #379)
                }
                else if (script.Contains("FULL_COUNTER"))
                {
                    // TODO: Implement with MoveByX function (issue #379)
                }
                else
                {
                    return false;
                }

                return true;
            }
            else if (data.IndexOf("STOP_RT") != -1)
            {
                rtController.ExecuteRadioTelescopeControlledStop(MovementPriority.GeneralStop);
            }
            else if (data.IndexOf("SENSOR_INIT") != -1)
            {
                string[] splitData = data.Split(',');

                if (splitData.Length != 10) return false;

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

                return true;
            }

            // can't find a keyword then we fail
            return false;
        }
    }
}
