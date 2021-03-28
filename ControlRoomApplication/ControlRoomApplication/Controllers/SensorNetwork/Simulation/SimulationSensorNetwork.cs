using ControlRoomApplication.Util;
using EmbeddedSystemsTest.SensorNetworkSimulation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ControlRoomApplication.Controllers.SensorNetwork.Simulation
{
    /// <summary>
    /// The simulation sensor network, which will respond exactly the same was as the physical SensorNetwork hardware will. The SensorNetworkServer
    /// does not know the difference between this and the physical hardware.
    /// </summary>
    public class SimulationSensorNetwork
    {
        private static readonly log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Constructor to initialize a new instance of the SimulationSensorNetwork. This will NOT start its simulated sensor monitoring.
        /// </summary>
        /// <param name="teensyClientIP">The Teensy's Client IP; this must be set equal to the SensorNetworkServer's Server IP.</param>
        /// <param name="teensyClientPort">The Teensy's Client Port; this must be set equal to the SensorNetworkServer's Server Port.</param>
        /// <param name="teensyServerIP">The Teensy's Server IP; this must be set equal to the SensorNetworkServer's Client IP.</param>
        /// <param name="teensyServerPort">The Teensy's Server Port; this must be set equal to the SensorNetworkServer's Client Port.</param>
        /// <param name="dataPath">The path to CSV data. By default, it looks for it in the SensorNetworkConstants.SimCSVDirectory location.</param>
        public SimulationSensorNetwork(string teensyClientIP, int teensyClientPort, IPAddress teensyServerIP, int teensyServerPort, string dataPath = SensorNetworkConstants.SimCSVDirectory)
        {
            Server = new TcpListener(teensyServerIP, teensyServerPort);

            ClientIP = teensyClientIP;
            ClientPort = teensyClientPort;

            SimulationSensorMonitoringThread = new Thread(() => { SimulationSensorMonitor(); });

            // Initialize all sensors to have a length of 0 so they are not null. We will be using
            // whether or not they're null to tell if they have been initialized or not
            AzimuthTempData = new double[0];
            ElevationTempData = new double[0];
            AzimuthAccData = new RawAccelerometerData[0];
            ElevationAccData = new RawAccelerometerData[0];
            CounterbalanceAccData = new RawAccelerometerData[0];
            AzimuthEncoderData = new double[0];
            ElevationEncoderData = new double[0];
        }

        private TcpListener Server { get; set; }

        private TcpClient Client { get; set; }
        private string ClientIP { get; set; }
        private int ClientPort { get; set; }
        private NetworkStream ClientStream { get; set; }

        private Thread SimulationSensorMonitoringThread { get; set; }

        private bool CurrentlyRunning { get; set; }

        private double[] AzimuthTempData { get; set; }

        private double[] ElevationTempData { get; set; }

        private RawAccelerometerData[] AzimuthAccData { get; set; }

        private RawAccelerometerData[] ElevationAccData { get; set; }

        private RawAccelerometerData[] CounterbalanceAccData { get; set; }

        private double[] AzimuthEncoderData { get; set; }

        private double[] ElevationEncoderData { get; set; }

        /// <summary>
        /// This is used to start the simulation Sensor Network. Calling this is equivalent to powering on the Teensy.
        /// </summary>
        public void StartSimulationSensorNetwork()
        {
            CurrentlyRunning = true;
            SimulationSensorMonitoringThread.Start();
        }

        /// <summary>
        /// This is used to start the simulation Sensor Network. Calling this is equivalent to powering off the Teensy.
        /// </summary>
        public void EndSimulationSensorNetwork()
        {
            CurrentlyRunning = false;
            Client.Dispose();
            ClientStream.Dispose();
            Server.Stop();
            SimulationSensorMonitoringThread.Join();
        }

        private void SimulationSensorMonitor()
        {
            // First, we want to connect to the SensorNetworkServer
            WaitForAndConnectToServer();

            // Next, we want to request initialization and receive it
            byte[] receivedInit = RequestAndAcquireSensorInitialization();

            // At this point, we have the initialization and can initialize the sensors
            InitializeSensors(receivedInit);

            // Now we can grab the CSV data for ONLY the initialized sensors...
            ReadFakeDataFromCSV();

            // Keep track of the indexes for each data array, because we are only extracting a small subsection of each one.
            // We want to know what subsection we just got so we can get the next subsection in the next iteration
            int elTempIdx = 0;
            int azTempIdx = 0;
            int elEncIdx = 0;
            int azEncIdx = 0;
            int elAccIdx = 0;
            int azAccIdx = 0;
            int cbAccIdx = 0;

            // This will tell us if we are rebooting or not. We will only reboot if the connection is randomly terminated.
            bool reboot = false;

            // Now we enter the "super loop"
            while (CurrentlyRunning)
            {
                // Convert subarrays to bytes
                byte[] dataToSend = BuildSubArraysAndEncodeData(ref elTempIdx, ref azTempIdx, ref elEncIdx, ref azEncIdx, ref elAccIdx, ref azAccIdx, ref cbAccIdx);

                // We have to check for CurrentlyRunning down here because we don't know when the connection is going to be terminated, and
                // it could very well be in the middle of the loop.
                if (CurrentlyRunning)
                {
                    try
                    {
                        // Send arrays
                        ClientStream.Write(dataToSend, 0, dataToSend.Length);
                        Thread.Sleep(SensorNetworkConstants.DataSendingInterval);
                    }
                    // This will be reached if the connection is unexpectedly terminated (like it is during sensor reinitialization)
                    catch
                    {
                        CurrentlyRunning = false;
                        reboot = true;
                    }
                }
            }

            // If the server disconnects, that triggers a reboot
            if(reboot)
            {
                CurrentlyRunning = true;
                Client.Dispose();
                ClientStream.Dispose();
                SimulationSensorMonitor();
            }
        }

        /// <summary>
        /// This will read into each data array and pull out a subarray based on the index value, and then encode those subarrays.
        /// </summary>
        /// <param name="azTempIdx">Azimuth temperature data array index that we are pulling from.</param>
        /// <param name="elTempIdx">Elevation temperature data array index that we are pulling from.</param>
        /// <param name="elEncIdx">Elevation encoder data array index that we are pulling from.</param>
        /// <param name="azEncIdx">Azimuth encoder data array index that we are pulling from.</param>
        /// <param name="elAccIdx">Elevation accelerometer data array index that we are pulling from.</param>
        /// <param name="azAccIdx">Azimuth accelerometer data array index that we are pulling from.</param>
        /// <param name="cbAccIdx">Counterbalance accelerometer data array index that we are pulling from.</param>
        /// <returns></returns>
        private byte[] BuildSubArraysAndEncodeData(ref int elTempIdx, ref int azTempIdx, ref int elEncIdx, ref int azEncIdx, ref int elAccIdx, ref int azAccIdx, ref int cbAccIdx)
        {
            // Select what index to go to next for each array. Each array will loop back around once it reaches its end.
            if (AzimuthTempData != null && azTempIdx + 1 > AzimuthTempData.Length - 1) azTempIdx = 0;
            if (ElevationTempData != null && elTempIdx + 1 > ElevationTempData.Length - 1) elTempIdx = 0;
            if (ElevationEncoderData != null && elEncIdx + 1 > ElevationEncoderData.Length - 1) elEncIdx = 0;
            if (AzimuthEncoderData != null && azEncIdx + 1 > AzimuthEncoderData.Length - 1) azEncIdx = 0;

            // Accelerometers are pulling around 200 samples per iteration
            if (ElevationAccData != null && elAccIdx + 100 > ElevationAccData.Length - 1) elAccIdx = 0;
            if (AzimuthAccData != null && azAccIdx + 100 > AzimuthAccData.Length - 1) azAccIdx = 0;
            if (CounterbalanceAccData != null && cbAccIdx + 100 > CounterbalanceAccData.Length - 1) cbAccIdx = 0;

            // Initialize subarrays to be of size 0
            double[] elTemps = new double[0];
            double[] azTemps = new double[0];
            RawAccelerometerData[] elAccl = new RawAccelerometerData[0];
            RawAccelerometerData[] azAccl = new RawAccelerometerData[0];
            RawAccelerometerData[] cbAccl = new RawAccelerometerData[0];
            double[] elEnc = new double[0];
            double[] azEnc = new double[0];

            // If the sensors are initialized, give them their subarrays, while also updating the index so that
            // this  knows what subarrays to go to next
            if (ElevationTempData != null)
            {
                elTemps = new double[1];
                Array.Copy(ElevationTempData, elTempIdx++, elTemps, 0, 1);
            }
            if (AzimuthTempData != null)
            {
                azTemps = new double[1];
                Array.Copy(AzimuthTempData, azTempIdx++, azTemps, 0, 1);
            }
            if (ElevationEncoderData != null)
            {
                elEnc = new double[1];
                Array.Copy(ElevationEncoderData, elEncIdx++, elEnc, 0, 1);
            }
            if (AzimuthEncoderData != null)
            {
                azEnc = new double[1];
                Array.Copy(AzimuthEncoderData, azEncIdx++, azEnc, 0, 1);
            }
            if (AzimuthAccData != null)
            {
                azAccl = new RawAccelerometerData[100];
                Array.Copy(AzimuthAccData, azAccIdx += 100, azAccl, 0, 100);
            }
            if (ElevationAccData != null)
            {
                elAccl = new RawAccelerometerData[100];
                Array.Copy(ElevationAccData, elAccIdx += 100, elAccl, 0, 100);
            }
            if (CounterbalanceAccData != null)
            {
                cbAccl = new RawAccelerometerData[100];
                Array.Copy(CounterbalanceAccData, cbAccIdx += 100, cbAccl, 0, 100);
            }

            // Finally, encode the subarrays and return the result
            return PacketEncodingTools.ConvertDataArraysToBytes(elAccl, azAccl, cbAccl, elTemps, azTemps, elEnc, azEnc);
        }

        /// <summary>
        /// This will wait for the SensorNetworkServer, and when it finds it, it will connect!
        /// This code was directly lifted from how this functionality works in the Teensy's source code.
        /// </summary>
        private void WaitForAndConnectToServer()
        {
            bool connected = false;

            // Wait for the SensorNetworkServer to be up
            while (!connected)
            {
                try
                {
                    Client = new TcpClient(ClientIP, ClientPort);
                    ClientStream = Client.GetStream();
                    connected = true;
                }
                catch
                {
                    logger.Info($"{Utilities.GetTimeStamp()}: SimulationSensorNetwork is waiting for the SensorNetworkServer.");
                }
            }
        }

        /// <summary>
        /// This will send a request for sensor initialization to the SensorNetworkServer, where the
        /// SensorNetworkServer will then respond with sensor initialization.
        /// </summary>
        /// <returns>The sensor initialization byte array.</returns>
        private byte[] RequestAndAcquireSensorInitialization()
        {
            // Start the server that will expect the Sensor Configuration
            Server.Start();
            NetworkStream ServerStream;

            // Ask the SensorNetworkServer for its initialization
            byte[] askForInit = Encoding.ASCII.GetBytes("Send Sensor Configuration");
            ClientStream.Write(askForInit, 0, askForInit.Length);
            ClientStream.Flush();

            // Wait for the SensorNetworkClient to send the initialization
            TcpClient localClient;
            byte[] receivedInit = new byte[SensorNetworkConstants.SensorNetworkSensorCount];

            // Once this line is passed, we have connected and received the initialization
            localClient = Server.AcceptTcpClient();
            logger.Info($"{Utilities.GetTimeStamp()}: Simulation Sensor Network received Sensor Initialization");
            ServerStream = localClient.GetStream();
            ServerStream.Read(receivedInit, 0, receivedInit.Length);

            // We will now dispose of the Server/Stream components because that is the only time we are using it
            ServerStream.Close();
            ServerStream.Dispose();
            Server.Stop();
            localClient.Close();
            localClient.Dispose();

            return receivedInit;
        }

        /// <summary>
        /// This is used to set the initialization of the sensors. Any non-initialized sensors will not be encoded.
        /// </summary>
        /// <param name="init">Sensor initialization we receive from the SensorNetworkServer's InitializationClient</param>
        private void InitializeSensors(byte[] init)
        {
            if (init[0] == 0) ElevationTempData = null;
            // Skip [1] because that is a redundant temp sensor; the SensorNetworkServer should not know about the change
            if (init[2] == 0) AzimuthTempData = null;
            // Skip [3] because that is a redundant temp sensor; the SensorNetworkServer should not know about the change
            if (init[4] == 0) ElevationEncoderData = null;
            if (init[5] == 0) AzimuthEncoderData = null;
            if (init[6] == 0) AzimuthAccData = null;
            if (init[7] == 0) ElevationAccData = null;
            if (init[8] == 0) CounterbalanceAccData = null;
        }

        /// <summary>
        /// This will read fake data from the CSV files based off of the initialization. If the sensor is "initialized" (not null),
        /// then it will read in CSV data for it. Otherwise, it will stay null.
        /// </summary>
        private void ReadFakeDataFromCSV(string directory = SensorNetworkConstants.SimCSVDirectory)
        {
            if (ElevationTempData != null)
            {
                double dbl;

                var values = File.ReadAllLines(directory + "TestElTemp.csv")
                    .SelectMany(a => a.Split(',')
                    .Select(str => double.TryParse(str, out dbl) ? dbl : 0));

                ElevationTempData = values.ToArray();
            }
            
            if (AzimuthTempData != null)
            {
                double dbl;

                var values = File.ReadAllLines(directory + "TestAzTemp.csv")
                    .SelectMany(a => a.Split(',')
                    .Select(str => double.TryParse(str, out dbl) ? dbl : 0));

                AzimuthTempData = values.ToArray();
            }
            
            if (AzimuthAccData != null)
            {
                int tempInt;

                int[] xData = File.ReadAllLines(directory + "TestAzAccX.csv")
                    .SelectMany(a => a.Split(',')
                    .Select(str => int.TryParse(str, out tempInt) ? tempInt : 0)).ToArray();

                int[] yData = File.ReadAllLines(directory + "TestAzAccY.csv")
                    .SelectMany(a => a.Split(',')
                    .Select(str => int.TryParse(str, out tempInt) ? tempInt : 0)).ToArray();

                int[] zData = File.ReadAllLines(directory + "TestAzAccZ.csv")
                    .SelectMany(a => a.Split(',')
                    .Select(str => int.TryParse(str, out tempInt) ? tempInt : 0)).ToArray();

                // Find which axis has the lowest number, which we will use as the size for the overall array
                int lowest = xData.Length;
                if (lowest > yData.Length) lowest = yData.Length;
                if (lowest > zData.Length) lowest = zData.Length;
                
                AzimuthAccData = new RawAccelerometerData[lowest];

                // Populate raw accelerometer data with individual axes
                for(int i = 0; i < lowest; i++)
                {
                    AzimuthAccData[i].X = xData[i];
                    AzimuthAccData[i].Y = yData[i];
                    AzimuthAccData[i].Z = zData[i];
                }
            }
            
            if (ElevationAccData != null)
            {
                int tempInt;

                int[] xData = File.ReadAllLines(directory + "TestElAccX.csv")
                    .SelectMany(a => a.Split(',')
                    .Select(str => int.TryParse(str, out tempInt) ? tempInt : 0)).ToArray();

                int[] yData = File.ReadAllLines(directory + "TestElAccY.csv")
                    .SelectMany(a => a.Split(',')
                    .Select(str => int.TryParse(str, out tempInt) ? tempInt : 0)).ToArray();

                int[] zData = File.ReadAllLines(directory + "TestElAccZ.csv")
                    .SelectMany(a => a.Split(',')
                    .Select(str => int.TryParse(str, out tempInt) ? tempInt : 0)).ToArray();

                // Find which axis has the lowest number, which we will use as the size for the overall array
                int lowest = xData.Length;
                if (lowest > yData.Length) lowest = yData.Length;
                if (lowest > zData.Length) lowest = zData.Length;

                ElevationAccData = new RawAccelerometerData[lowest];

                // Populate raw accelerometer data with individual axes
                for (int i = 0; i < lowest; i++)
                {
                    ElevationAccData[i].X = xData[i];
                    ElevationAccData[i].Y = yData[i];
                    ElevationAccData[i].Z = zData[i];
                }
            }
            
            if (CounterbalanceAccData != null)
            {
                int tempInt;

                int[] xData = File.ReadAllLines(directory + "TestCbAccX.csv")
                    .SelectMany(a => a.Split(',')
                    .Select(str => int.TryParse(str, out tempInt) ? tempInt : 0)).ToArray();

                int[] yData = File.ReadAllLines(directory + "TestCbAccY.csv")
                    .SelectMany(a => a.Split(',')
                    .Select(str => int.TryParse(str, out tempInt) ? tempInt : 0)).ToArray();

                int[] zData = File.ReadAllLines(directory + "TestCbAccZ.csv")
                    .SelectMany(a => a.Split(',')
                    .Select(str => int.TryParse(str, out tempInt) ? tempInt : 0)).ToArray();

                // Find which axis has the lowest number, which we will use as the size for the overall array
                int lowest = xData.Length;
                if (lowest > yData.Length) lowest = yData.Length;
                if (lowest > zData.Length) lowest = zData.Length;

                CounterbalanceAccData = new RawAccelerometerData[lowest];

                // Populate raw accelerometer data with individual axes
                for (int i = 0; i < lowest; i++)
                {
                    CounterbalanceAccData[i].X = xData[i];
                    CounterbalanceAccData[i].Y = yData[i];
                    CounterbalanceAccData[i].Z = zData[i];
                }
            }
            
            if (ElevationEncoderData != null)
            {
                double dbl;

                var values = File.ReadAllLines(directory + "TestElEnc.csv")
                    .SelectMany(a => a.Split(',')
                    .Select(str => double.TryParse(str, out dbl) ? dbl : 0));

                ElevationEncoderData = values.ToArray();
            }
            
            if (AzimuthEncoderData != null)
            {
                double dbl;

                var values = File.ReadAllLines(directory + "TestAzEnc.csv")
                    .SelectMany(a => a.Split(',')
                    .Select(str => double.TryParse(str, out dbl) ? dbl : 0));

                AzimuthEncoderData = values.ToArray();
            }
        }
    }
}
