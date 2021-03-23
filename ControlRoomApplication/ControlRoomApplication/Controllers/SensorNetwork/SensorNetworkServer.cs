using ControlRoomApplication.Entities;
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
            CurrentElevationMotorTemp = new Temperature();
            CurrentAzimuthMotorTemp = new Temperature();
            CurrentAbsoluteOrientation = new Orientation();
            CurrentElevationMotorAccl = new Acceleration[0];
            CurrentAzimuthMotorAccl = new Acceleration[0];
            CurrentCounterbalanceAccl = new Acceleration[0];
            
            // Initialize threads and additional processes, if applicable
            SensorMonitoringThread = new Thread(() => { SensorMonitoringRoutine(); });
            SensorMonitoringThread.Name = "SensorMonitorThread";

            // We only want to run the internal simulation if the user selected to run the Simulated Sensor Network
            if(isSimulation)
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
        public Temperature CurrentElevationMotorTemp;

        /// <summary>
        /// The current azimuth motor temperature received from the sensor network. 
        /// </summary>
        public Temperature CurrentAzimuthMotorTemp;

        /// <summary>
        /// The current orientation of the telescope based off of the absolute encoders. These
        /// are totally separate from the motor encoders' data that we get from the Read_position
        /// function in the PLC and MCU, and will provide more accurate data regarding the telescope's
        /// position.
        /// </summary>
        public Orientation CurrentAbsoluteOrientation;

        /// <summary>
        /// This tells us the current vibration coming from the azimuth motor. It is always received as
        /// an array to give us higher data accuracy.
        /// </summary>
        public Acceleration[] CurrentElevationMotorAccl;

        /// <summary>
        /// This tells us the current vibration coming from the azimuth motor. It is always received as
        /// an array to give us higher data accuracy.
        /// </summary>
        public Acceleration[] CurrentAzimuthMotorAccl;

        /// <summary>
        /// This tells us the current vibration coming from the counterbalance. It is always received as
        /// an array to give us higher data accuracy. This can also technically be used to calculate the
        /// telescope's elevation position.
        /// </summary>
        public Acceleration[] CurrentCounterbalanceAccl;

        /// <summary>
        /// This is used for sending initialization data to the Sensor Network, and is also used to
        /// access the configuration.
        /// </summary>
        public SensorNetworkClient InitializationClient;

        // TODO: Add SimulationSensorNetwork here

        /// <summary>
        /// This will be used to tell us what the SensorNetwork status using <seealso cref="SensorNetworkStatusEnum"/>.
        /// </summary>
        public SensorNetworkStatusEnum Status;

        private TcpListener Server;

        /// <summary>
        /// This should be true as long as the SensorMonitoringThread is running, and set to false if that thread
        /// is to be terminated.
        /// </summary>
        private bool CurrentlyRunning;
        
        /// <summary>
        /// This thread should always be running and waiting or collecting some kind of data from the Sensor Network unless
        /// the CurrentlyRunning value (above) is set to false.
        /// </summary>
        private Thread SensorMonitoringThread;

        /// <summary>
        /// This will help us detect if the SensorNetworkServer has stopped receiving data.
        /// </summary>
        private System.Timers.Timer Timeout;

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

        private void InterpretData(byte[] data, int buffer)
        {

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

        }
    }
}
