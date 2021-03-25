using ControlRoomApplication.Entities;
using ControlRoomApplication.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;

namespace ControlRoomApplication.Controllers.SensorNetwork
{

    /// <summary>
    /// This is the central component of the Sensor Network on the Control Room end. This handles all the main communications
    /// with the main Sensor Network system, such as receiving data, setting statuses based on data it receives, and tells the
    /// client when to send initialization data.
    /// </summary>
    public class SensorNetworkServer
    {
        private static readonly log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Constructor used to set the values needed to initialize the components of the class. It will not start waiting for data until
        /// StartSensorMonitoringRoutine() is called.
        /// 
        /// The reason the client IP address is a string and not an IPAddress is because none of the TCPClient class's
        /// constructors take an IPAddress. :(
        /// </summary>
        /// <param name="serverIPAddress">This is the IP address the SensorNetworkServer is "listening" to.</param>
        /// <param name="serverPort">This is the port the SensorNetworkServer is "listening" to.</param>
        /// <param name="clientIPAddress">This is the IP address of the SensorNetwork that we will be sending the sensor initialization to.</param>
        /// <param name="clientPort">This is the port of the SensorNetwork that we will be sending the sensor initialization to.</param>
        /// <param name="telescopeId">The Radio Telescope that the SensorNetworkConfig will apply to.</param>
        /// <param name="isSimulation">Tells the SensorNetworkServer if it should initialize the SimulationSensorNetwork,
        /// or if it is connecting to the production hardware (or maybe an outside simulation).</param>
        public SensorNetworkServer(IPAddress serverIPAddress, int serverPort, string clientIPAddress, int clientPort, int telescopeId, bool isSimulation)
        {
            // Initialize main parts of the Sensor Network
            Server = new TcpListener(serverIPAddress, serverPort);
            InitializationClient = new SensorNetworkClient(clientIPAddress, clientPort, telescopeId);

            // Sensor data initialization
            CurrentElevationMotorTemp = new Temperature[1];
            CurrentElevationMotorTemp[0] = new Temperature();

            CurrentAzimuthMotorTemp = new Temperature[1];
            CurrentAzimuthMotorTemp[0] = new Temperature();

            CurrentAbsoluteOrientation = new Orientation();

            CurrentElevationMotorAccl = new Acceleration[1];
            CurrentElevationMotorAccl[0] = new Acceleration();

            CurrentAzimuthMotorAccl = new Acceleration[1];
            CurrentAzimuthMotorAccl[0] = new Acceleration();

            CurrentCounterbalanceAccl = new Acceleration[1];
            CurrentCounterbalanceAccl[0] = new Acceleration();

            // Initialize threads and additional processes, if applicable
            SensorMonitoringThread = new Thread(() => { SensorMonitoringRoutine(); });
            SensorMonitoringThread.Name = "SensorMonitorThread";

            // We only want to run the internal simulation if the user selected to run the Simulated Sensor Network
            if (isSimulation)
            {
                // TODO: Initialize the SimulationSensorNetwork here
            }

            // Initialize the timeout timer but don't start it yet
            Timeout = new System.Timers.Timer();
            Timeout.Interval = InitializationClient.config.TimeoutInitialization;
            Timeout.Elapsed += TimedOut; // TimedOut is the function at the bottom that executes when this elapses
            Timeout.AutoReset = false;
        }

        /// <summary>
        /// The current elevation motor temperature received from the sensor network.
        /// </summary>
        public Temperature[] CurrentElevationMotorTemp { get; set; }

        /// <summary>
        /// The current azimuth motor temperature received from the sensor network. 
        /// </summary>
        public Temperature[] CurrentAzimuthMotorTemp { get; set; }

        /// <summary>
        /// The current orientation of the telescope based off of the absolute encoders. These
        /// are totally separate from the motor encoders' data that we get from the Read_position
        /// function in the PLC and MCU, and will provide more accurate data regarding the telescope's
        /// position.
        /// </summary>
        public Orientation CurrentAbsoluteOrientation { get; set; }

        /// <summary>
        /// This tells us the current vibration coming from the azimuth motor. It is always received as
        /// an array to give us higher data accuracy.
        /// </summary>
        public Acceleration[] CurrentElevationMotorAccl { get; set; }

        /// <summary>
        /// This tells us the current vibration coming from the azimuth motor. It is always received as
        /// an array to give us higher data accuracy.
        /// </summary>
        public Acceleration[] CurrentAzimuthMotorAccl { get; set; }

        /// <summary>
        /// This tells us the current vibration coming from the counterbalance. It is always received as
        /// an array to give us higher data accuracy. This can also technically be used to calculate the
        /// telescope's elevation position.
        /// </summary>
        public Acceleration[] CurrentCounterbalanceAccl { get; set; }

        /// <summary>
        /// This is used for sending initialization data to the Sensor Network, and is also used to
        /// access the configuration.
        /// </summary>
        public SensorNetworkClient InitializationClient { get; }

        // TODO: Add SimulationSensorNetwork here

        /// <summary>
        /// This will be used to tell us what the SensorNetwork status using <seealso cref="SensorNetworkStatusEnum"/>.
        /// </summary>
        public SensorNetworkStatusEnum Status { get; set; }

        private TcpListener Server;

        /// <summary>
        /// This should be true as long as the SensorMonitoringThread is running, and set to false if that thread
        /// is to be terminated.
        /// </summary>
        private bool CurrentlyRunning { get; set; }
        
        /// <summary>
        /// This thread should always be running and waiting or collecting some kind of data from the Sensor Network unless
        /// the CurrentlyRunning value (above) is set to false.
        /// </summary>
        private Thread SensorMonitoringThread { get; }

        /// <summary>
        /// This will help us detect if the SensorNetworkServer has stopped receiving data.
        /// </summary>
        private System.Timers.Timer Timeout { get; }

        /// <summary>
        /// This starts the SensorMonitoringRoutine. Immediately after connecting, initialization will begin.
        /// </summary>
        /// <returns>If started successfully, return true. Else, return false.</returns>
        public bool StartSensorMonitoringRoutine()
        {
            return false;
        }

        /// <summary>
        /// This ends the SensorMonitoringRoutine. This should only be executed when "Shutdown RT" is clicked.
        /// </summary>
        /// <returns>If ended successfully, return true. Else, return false.</returns>
        public bool EndSensorMonitoringRoutine()
        {
            return false;
        }

        /// <summary>
        /// This will reboot the Sensor Network, which will update its sensor initialization with what the
        /// user has selected on the GUI.
        /// </summary>
        /// <returns></returns>
        public bool RebootSensorNetwork()
        {
            return false;
        }

        /// <summary>
        /// This is used internally by the SensorNetworkServer to tell what kind of data the Sensor Network
        /// is sending. It may send data, in which case a transit ID will be present, or it may ask for
        /// an initialization.
        /// </summary>
        /// <param name="data">The data we are interpreting.</param>
        /// <param name="receivedDataSize">This will be used to tell if our data is complete.</param>
        private bool InterpretData(byte[] data, int receivedDataSize)
        {
            bool success = false;

            if(Encoding.ASCII.GetString(data, 0, receivedDataSize).Equals("Send Sensor Configuration"))
            { // Reaching here means that we've received a request for sensor initialization
                
                // If the init sending fails, set status to reflect that
                if (!InitializationClient.SendSensorInitialization())
                {
                    Status = SensorNetworkStatusEnum.InitializationSendingFailed;
                    if(Timeout.Enabled) Timeout.Stop();
                }
                else
                {
                    logger.Info($"{Utilities.GetTimeStamp()}: Successfully sent sensor initialization to the Sensor Network.");
                    success = true;
                }
            }
            else
            {
                // Bytes 1-3 of the data contain the overall size of the packet
                UInt32 expectedDataSize = (UInt32)(data[1] << 24 | data[2] << 16 | data[3] << 8 | data[4]);

                // Check that the data we received is the same size as what we expect
                if (expectedDataSize == receivedDataSize)
                {
                    // Byte 0 contains the transmit ID
                    int receivedTransitId = data[0];

                    // Verify that the first byte contains the "success" code (transit ID)
                    if(receivedTransitId == SensorNetworkConstants.TransitIdSuccess)
                    {
                        // At this point, we may begin parsing the data

                        // Acquire the sample sizes for each sensor
                        UInt16 elAcclSize = (UInt16)(data[5] << 8 | data[6]);
                        UInt16 azAcclSize = (UInt16)(data[7] << 8 | data[8]);
                        UInt16 cbAcclSize = (UInt16)(data[9] << 8 | data[10]);
                        UInt16 elTempSensorSize = (UInt16)(data[11] << 8 | data[12]);
                        UInt16 azTempSensorSize = (UInt16)(data[13] << 8 | data[14]);
                        UInt16 elEncoderSize = (UInt16)(data[15] << 8 | data[16]);
                        UInt16 azEncoderSize = (UInt16)(data[17] << 8 | data[18]);

                        // This is the index we start reading sensor data
                        int k = 19;

                        // If no data comes through for a sensor (i.e. the size is 0), then it will not be updated,
                        // otherwise the UI value would temporarily be set to 0, which would be inaccurate

                        // Accelerometer 1 (elevation)
                        if (elAcclSize > 0)
                        {
                            CurrentElevationMotorAccl = PacketDecodingTools.GetAccelerationFromBytes(ref k, data, elAcclSize, SensorLocationEnum.EL_MOTOR);
                        }

                        // Accelerometer 2 (azimuth)
                        if (azAcclSize > 0)
                        {
                            CurrentAzimuthMotorAccl = PacketDecodingTools.GetAccelerationFromBytes(ref k, data, azAcclSize, SensorLocationEnum.AZ_MOTOR);
                        }

                        // Accelerometer 3 (counterbalance)
                        if (cbAcclSize > 0)
                        {
                            CurrentCounterbalanceAccl = PacketDecodingTools.GetAccelerationFromBytes(ref k, data, cbAcclSize, SensorLocationEnum.COUNTERBALANCE);
                        }

                        // Elevation temperature
                        if (elTempSensorSize > 0)
                        {
                            CurrentElevationMotorTemp = PacketDecodingTools.GetTemperatureFromBytes(ref k, data, elTempSensorSize, SensorLocationEnum.EL_MOTOR);
                        }

                        // Azimuth temperature
                        if (azTempSensorSize > 0)
                        {
                            CurrentAzimuthMotorTemp = PacketDecodingTools.GetTemperatureFromBytes(ref k, data, azTempSensorSize, SensorLocationEnum.AZ_MOTOR);
                        }

                        // Elevation absolute encoder
                        if (elEncoderSize > 0)
                        {
                            // This must be converted into degrees, because we are only receiving raw data from the
                            // elevation encoder
                            CurrentAbsoluteOrientation.Elevation = 
                                0.25 * (data[k++] << 8 | data[k++]) - 20.375;
                        }

                        // Azimuth absolute encoder
                        if (azEncoderSize > 0)
                        {
                            // This must be converted into degrees, because we are only receiving raw data from the
                            // azimuth encoder
                            CurrentAbsoluteOrientation.Azimuth = 
                                360 / SensorNetworkConstants.AzimuthEncoderScaling * (data[k++] << 8 | data[k++]);
                        }

                        success = true;
                    }

                    // This may be replaced with different errors at some point (that have different transit IDs), 
                    // though there are currently no plans for this. Right now, it is treated as an error overall:
                    // We should NOT be receiving anything other than TransitIdSuccess.
                    else
                    {
                        logger.Error($"{Utilities.GetTimeStamp()}: Transit ID error: Expected " +
                            $"ID {SensorNetworkConstants.TransitIdSuccess}, received ID {receivedTransitId})");
                    }
                }
                else
                {
                    // If this happens, that indicates a stability problem that likely has to do with the connection.
                    // The most likely reason that this might happen is a faulty Ethernet cable. If it happens once
                    // in a blue moon, I would not be too concerned.
                    logger.Error($"{Utilities.GetTimeStamp()}: Error decoding packet: Packet was the incorrect size. " +
                        $"(expected {expectedDataSize} bytes, received {receivedDataSize})");
                }

            }

            return success;
        }

        /// <summary>
        /// This is the main routine that will constantly be expecting data from the Sensor Network. This will
        /// continuously run in a loop until EndSensorMonitoringRoutine() is called.
        /// </summary>
        private void SensorMonitoringRoutine()
        {

        }

        /// <summary>
        /// This is only reached when a timeout occurs from the Timer. One of the two TimedOut statuses will
        /// be set as a result of this call.
        /// </summary>
        /// <param name="source">unused</param>
        /// <param name="e">unused</param>
        private void TimedOut(Object source, ElapsedEventArgs e)
        {
            if(Status == SensorNetworkStatusEnum.ReceivingData)
            {
                Status = SensorNetworkStatusEnum.TimedOutDataRetrieval;
            }
            else if(Status == SensorNetworkStatusEnum.Initializing)
            {
                Status = SensorNetworkStatusEnum.TimedOutInitialization;
            }
            else
            {
                Status = SensorNetworkStatusEnum.UnknownError;
            }

            logger.Error($"{Utilities.GetTimeStamp()}: Connection to the Sensor Network timed out! Status: {Status}");
        }
    }
}
