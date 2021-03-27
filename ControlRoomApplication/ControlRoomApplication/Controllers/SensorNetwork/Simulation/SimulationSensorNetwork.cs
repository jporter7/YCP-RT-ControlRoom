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
        public SimulationSensorNetwork(string teensyClientIP, int teensyClientPort, IPAddress teensyServerIP, int teensyServerPort)
        {
            ClientIP = teensyClientIP;
            ClientPort = teensyClientPort;
            ServerIP = teensyServerIP;
            ServerPort = teensyServerPort;

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

            // Set indexes to 0
            AzimuthTempDataIndex = 0;
            ElevationTempDataIndex = 0;
            AzimuthAccDataIndex = 0;
            ElevationAccDataIndex = 0;
            CounterbalanceAccDataIndex = 0;
            AzimuthEncoderDataIndex = 0;
            ElevationEncoderDataIndex = 0;
        }

        private TcpListener Server { get; set; }
        private IPAddress ServerIP { get; set; }
        private int ServerPort { get; set; }
        private NetworkStream ServerStream { get; set; }

        private TcpClient Client { get; set; }
        private string ClientIP { get; set; }
        private int ClientPort { get; set; }
        private NetworkStream ClientStream { get; set; }

        private Thread SimulationSensorMonitoringThread { get; set; }

        private bool CurrentlyRunning { get; set; }

        private bool Reboot { get; set; }

        private double[] AzimuthTempData { get; set; }
        private int AzimuthTempDataIndex; // Indexes are so we know what part of the array to read from in the loop

        private double[] ElevationTempData { get; set; }
        private int ElevationTempDataIndex;

        private RawAccelerometerData[] AzimuthAccData { get; set; }
        private int AzimuthAccDataIndex;

        private RawAccelerometerData[] ElevationAccData { get; set; }
        private int ElevationAccDataIndex;

        private RawAccelerometerData[] CounterbalanceAccData { get; set; }
        private int CounterbalanceAccDataIndex;

        private double[] AzimuthEncoderData { get; set; }
        private int AzimuthEncoderDataIndex;

        private double[] ElevationEncoderData { get; set; }
        private int ElevationEncoderDataIndex;

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
            // Start the server that will expect the Sensor Configuration
            Server = new TcpListener(ServerIP, ServerPort);
            Server.Start();

            bool connected = false;

            // Wait for the SensorNetworkServer to be up
            while(!connected)
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

            // At this point, we have the initialization and can initialize the sensors
            InitializeSensors(receivedInit);

            // Now we can grab the CSV data for the initialized sensors...
            ReadFakeDataFromCSV();

            // Now we enter the "super loop"
            while(CurrentlyRunning)
            {
                // Select what index to go to next for each array. Each array will loop back around once it reaches its end.
                if (AzimuthTempData != null && AzimuthTempDataIndex + 1 > AzimuthTempData.Length - 1) AzimuthTempDataIndex = 0;
                if (ElevationTempData != null && ElevationTempDataIndex + 1 > ElevationTempData.Length - 1) ElevationTempDataIndex = 0;
                if (ElevationEncoderData != null && ElevationEncoderDataIndex + 1 > ElevationEncoderData.Length - 1) ElevationEncoderDataIndex = 0;
                if (AzimuthEncoderData != null && AzimuthEncoderDataIndex + 1 > AzimuthEncoderData.Length - 1) AzimuthEncoderDataIndex = 0;

                // Accelerometers are pulling around 200 samples per iteration
                if (ElevationAccData != null && ElevationAccDataIndex + 200 > ElevationAccData.Length - 1) ElevationAccDataIndex = 0;
                if (AzimuthAccData != null && AzimuthAccDataIndex + 200 > AzimuthAccData.Length - 1) AzimuthAccDataIndex = 0;
                if (CounterbalanceAccData != null && CounterbalanceAccDataIndex + 200 > CounterbalanceAccData.Length - 1) CounterbalanceAccDataIndex = 0;

                // Initialize subarrays to be of size 0
                double[] elTemps = new double[0];
                double[] azTemps = new double[0];
                RawAccelerometerData[] elAccl = new RawAccelerometerData[0];
                RawAccelerometerData[] azAccl = new RawAccelerometerData[0];
                RawAccelerometerData[] cbAccl = new RawAccelerometerData[0];
                double[] elEnc = new double[0];
                double[] azEnc = new double[0];

                // If the sensors are initialized, give them their subarrays
                if (ElevationTempData != null)
                {
                    elTemps = new double[1];
                    Array.Copy(ElevationTempData, ElevationTempDataIndex++, elTemps, 0, 1);
                }
                if (AzimuthTempData != null)
                {
                    azTemps = new double[1];
                    Array.Copy(AzimuthTempData, AzimuthTempDataIndex++, azTemps, 0, 1);
                }
                if (ElevationEncoderData != null)
                {
                    elEnc = new double[1];
                    Array.Copy(ElevationEncoderData, ElevationEncoderDataIndex++, elEnc, 0, 1);
                }
                if (AzimuthEncoderData != null)
                {
                    azEnc = new double[1];
                    Array.Copy(AzimuthEncoderData, AzimuthEncoderDataIndex++, azEnc, 0, 1);
                }
                if (AzimuthAccData != null)
                {
                    azAccl = new RawAccelerometerData[100];
                    Array.Copy(AzimuthAccData, AzimuthAccDataIndex++, azAccl, 0, 100);
                }
                if (ElevationAccData != null)
                {
                    elAccl = new RawAccelerometerData[100];
                    Array.Copy(ElevationAccData, ElevationAccDataIndex++, elAccl, 0, 100);
                }
                if (CounterbalanceAccData != null)
                {
                    cbAccl = new RawAccelerometerData[100];
                    Array.Copy(CounterbalanceAccData, CounterbalanceAccDataIndex++, cbAccl, 0, 100);
                }

                // Convert subarrays to bytes
                byte[] dataToSend = PacketEncodingTools.ConvertDataArraysToBytes(elAccl, azAccl, cbAccl, elTemps, azTemps, elEnc, azEnc);

                try
                {
                    // Send arrays
                    ClientStream.Write(dataToSend, 0, dataToSend.Length);
                    Thread.Sleep(SensorNetworkConstants.DataSendingInterval);
                }
                // This may be reached if the connection is unexpectedly terminated (such as during sensor reinitialization)
                catch
                {
                    CurrentlyRunning = false;
                    Reboot = true;
                }
            }

            // If the server disconnects, that triggers a reboot
            if(Reboot)
            {
                CurrentlyRunning = true;
                Client.Dispose();
                ClientStream.Dispose();
                Server.Stop();
                Reboot = false;
                SimulationSensorMonitor();
            }
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
        private void ReadFakeDataFromCSV()
        {
            if (ElevationTempData != null)
            {
                double dbl;

                var values = File.ReadAllLines(SensorNetworkConstants.SimCSVDirectory + "TestElTemp.csv")
                    .SelectMany(a => a.Split(',')
                    .Select(str => double.TryParse(str, out dbl) ? dbl : 0));

                ElevationTempData = values.ToArray();
            }
            
            if (AzimuthTempData != null)
            {
                double dbl;

                var values = File.ReadAllLines(SensorNetworkConstants.SimCSVDirectory + "TestAzTemp.csv")
                    .SelectMany(a => a.Split(',')
                    .Select(str => double.TryParse(str, out dbl) ? dbl : 0));

                AzimuthTempData = values.ToArray();
            }
            
            if (AzimuthAccData != null)
            {
                int tempInt;

                int[] xData = File.ReadAllLines(SensorNetworkConstants.SimCSVDirectory + "TestAzAccX.csv")
                    .SelectMany(a => a.Split(',')
                    .Select(str => int.TryParse(str, out tempInt) ? tempInt : 0)).ToArray();

                int[] yData = File.ReadAllLines(SensorNetworkConstants.SimCSVDirectory + "TestAzAccY.csv")
                    .SelectMany(a => a.Split(',')
                    .Select(str => int.TryParse(str, out tempInt) ? tempInt : 0)).ToArray();

                int[] zData = File.ReadAllLines(SensorNetworkConstants.SimCSVDirectory + "TestAzAccZ.csv")
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

                int[] xData = File.ReadAllLines(SensorNetworkConstants.SimCSVDirectory + "TestElAccX.csv")
                    .SelectMany(a => a.Split(',')
                    .Select(str => int.TryParse(str, out tempInt) ? tempInt : 0)).ToArray();

                int[] yData = File.ReadAllLines(SensorNetworkConstants.SimCSVDirectory + "TestElAccY.csv")
                    .SelectMany(a => a.Split(',')
                    .Select(str => int.TryParse(str, out tempInt) ? tempInt : 0)).ToArray();

                int[] zData = File.ReadAllLines(SensorNetworkConstants.SimCSVDirectory + "TestElAccZ.csv")
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

                int[] xData = File.ReadAllLines(SensorNetworkConstants.SimCSVDirectory + "TestCbAccX.csv")
                    .SelectMany(a => a.Split(',')
                    .Select(str => int.TryParse(str, out tempInt) ? tempInt : 0)).ToArray();

                int[] yData = File.ReadAllLines(SensorNetworkConstants.SimCSVDirectory + "TestCbAccY.csv")
                    .SelectMany(a => a.Split(',')
                    .Select(str => int.TryParse(str, out tempInt) ? tempInt : 0)).ToArray();

                int[] zData = File.ReadAllLines(SensorNetworkConstants.SimCSVDirectory + "TestCbAccZ.csv")
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

                var values = File.ReadAllLines(SensorNetworkConstants.SimCSVDirectory + "TestElEnc.csv")
                    .SelectMany(a => a.Split(',')
                    .Select(str => double.TryParse(str, out dbl) ? dbl : 0));

                ElevationEncoderData = values.ToArray();
            }
            
            if (AzimuthEncoderData != null)
            {
                double dbl;

                var values = File.ReadAllLines(SensorNetworkConstants.SimCSVDirectory + "TestAzEnc.csv")
                    .SelectMany(a => a.Split(',')
                    .Select(str => double.TryParse(str, out dbl) ? dbl : 0));

                AzimuthEncoderData = values.ToArray();
            }
        }
    }
}
