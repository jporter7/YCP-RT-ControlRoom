using ControlRoomApplication.Controllers.SensorNetwork;
using ControlRoomApplication.Controllers.SensorNetwork.Simulation;
using ControlRoomApplication.Database;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ControlRoomApplicationTest.EntityControllersTests.SensorNetworkTests.Simulation
{
    [TestClass]
    public class SimulationSensorNetworkTest
    {
        string ClientIP = "127.0.0.1";
        int ClientPort = 3000;

        IPAddress ServerIP = IPAddress.Parse("127.0.0.2");
        int ServerPort = 3001;

        int TelescopeId = 3000;

        string DataPath = "../../EntityControllersTests/SensorNetworkTests/Simulation/TestCSVData/";

        SimulationSensorNetwork SimSensorNetwork;

        [TestInitialize]
        public void Initialize()
        {
            SimSensorNetwork = new SimulationSensorNetwork(ClientIP, ClientPort, ServerIP, ServerPort, DataPath);
        }

        [TestMethod]
        public void SimulationSensorNetwork_Constructor_AllFieldsCorrect()
        {
            PrivateObject privSim = new PrivateObject(SimSensorNetwork);

            // Gather data
            TcpListener resultListener = (TcpListener)privSim.GetFieldOrProperty("Server");
            string resultClientIP = (string)privSim.GetFieldOrProperty("ClientIP");
            int resultClientPort = (int)privSim.GetFieldOrProperty("ClientPort");
            string resultDataDirectory = (string)privSim.GetFieldOrProperty("DataDirectory");

            // Verify server
            Assert.IsNotNull(resultListener);
            Assert.AreEqual(((IPEndPoint)resultListener.LocalEndpoint).Address, ServerIP);
            Assert.AreEqual(((IPEndPoint)resultListener.LocalEndpoint).Port, ServerPort);

            // Verify client
            Assert.AreEqual(ClientIP, resultClientIP);
            Assert.AreEqual(ClientPort, resultClientPort);

            // Verify directory
            Assert.AreEqual(DataPath, resultDataDirectory);
        }

        [TestMethod]
        public void TestInitializeSensors_InitializeAllSensors_AllSensorsInitialized()
        {
            PrivateObject privSim = new PrivateObject(SimSensorNetwork);

            // Create an initialization that will enable all sensors.
            byte[] init = new byte[9];
            for(int i = 0; i < init.Length; i++) init[i] = 1;

            privSim.Invoke("InitializeSensors", init);

            // Gather result data
            double[] resultElTemp = (double[])privSim.GetFieldOrProperty("ElevationTempData");
            double[] resultAzTemp = (double[])privSim.GetFieldOrProperty("AzimuthTempData");
            RawAccelerometerData[] resultElAcc = (RawAccelerometerData[])privSim.GetFieldOrProperty("ElevationAccData");
            RawAccelerometerData[] resultAzAcc = (RawAccelerometerData[])privSim.GetFieldOrProperty("AzimuthAccData");
            RawAccelerometerData[] resultCbAcc = (RawAccelerometerData[])privSim.GetFieldOrProperty("CounterbalanceAccData");
            double[] resultElEnc = (double[])privSim.GetFieldOrProperty("ElevationEncoderData");
            double[] resultAzEnc = (double[])privSim.GetFieldOrProperty("AzimuthEncoderData");

            // Create expected array length (spoiler: it is zero)
            int expectedArrLength = 0;

            // Verify all arrays still have a length of 0
            Assert.AreEqual(expectedArrLength, resultElTemp.Length);
            Assert.AreEqual(expectedArrLength, resultAzTemp.Length);
            Assert.AreEqual(expectedArrLength, resultElAcc.Length);
            Assert.AreEqual(expectedArrLength, resultAzAcc.Length);
            Assert.AreEqual(expectedArrLength, resultCbAcc.Length);
            Assert.AreEqual(expectedArrLength, resultElEnc.Length);
            Assert.AreEqual(expectedArrLength, resultAzEnc.Length);
        }

        [TestMethod]
        public void TestInitializeSensors_DisableElTemp_OnlyElTempDisabled()
        {
            PrivateObject privSim = new PrivateObject(SimSensorNetwork);

            // Create an initialization that will enable all sensors.
            byte[] init = new byte[SensorNetworkConstants.SensorNetworkSensorCount];

            init[(int)SensorInitializationEnum.ElevationTemp] = 0;
            init[(int)SensorInitializationEnum.AzimuthTemp] = 1;
            init[(int)SensorInitializationEnum.ElevationEncoder] = 1;
            init[(int)SensorInitializationEnum.AzimuthEncoder] = 1;
            init[(int)SensorInitializationEnum.AzimuthAccelerometer] = 1;
            init[(int)SensorInitializationEnum.ElevationAccelerometer] = 1;
            init[(int)SensorInitializationEnum.CounterbalanceAccelerometer] = 1;

            privSim.Invoke("InitializeSensors", init);

            // Gather result data
            double[] resultElTemp = (double[])privSim.GetFieldOrProperty("ElevationTempData");
            double[] resultAzTemp = (double[])privSim.GetFieldOrProperty("AzimuthTempData");
            RawAccelerometerData[] resultElAcc = (RawAccelerometerData[])privSim.GetFieldOrProperty("ElevationAccData");
            RawAccelerometerData[] resultAzAcc = (RawAccelerometerData[])privSim.GetFieldOrProperty("AzimuthAccData");
            RawAccelerometerData[] resultCbAcc = (RawAccelerometerData[])privSim.GetFieldOrProperty("CounterbalanceAccData");
            double[] resultElEnc = (double[])privSim.GetFieldOrProperty("ElevationEncoderData");
            double[] resultAzEnc = (double[])privSim.GetFieldOrProperty("AzimuthEncoderData");

            // Create expected array length (spoiler: it is zero)
            int expectedArrLength = 0;

            // Verify all arrays still have a length of 0 except the null one
            Assert.IsNull(resultElTemp);
            Assert.AreEqual(expectedArrLength, resultAzTemp.Length);
            Assert.AreEqual(expectedArrLength, resultElAcc.Length);
            Assert.AreEqual(expectedArrLength, resultAzAcc.Length);
            Assert.AreEqual(expectedArrLength, resultCbAcc.Length);
            Assert.AreEqual(expectedArrLength, resultElEnc.Length);
            Assert.AreEqual(expectedArrLength, resultAzEnc.Length);
        }

        [TestMethod]
        public void TestInitializeSensors_DisableAzTemp_OnlyAzTempDisabled()
        {
            PrivateObject privSim = new PrivateObject(SimSensorNetwork);

            // Create an initialization that will enable all sensors.
            byte[] init = new byte[SensorNetworkConstants.SensorNetworkSensorCount];

            init[(int)SensorInitializationEnum.ElevationTemp] = 1;
            init[(int)SensorInitializationEnum.AzimuthTemp] = 0;
            init[(int)SensorInitializationEnum.ElevationEncoder] = 1;
            init[(int)SensorInitializationEnum.AzimuthEncoder] = 1;
            init[(int)SensorInitializationEnum.AzimuthAccelerometer] = 1;
            init[(int)SensorInitializationEnum.ElevationAccelerometer] = 1;
            init[(int)SensorInitializationEnum.CounterbalanceAccelerometer] = 1;

            privSim.Invoke("InitializeSensors", init);

            // Gather result data
            double[] resultElTemp = (double[])privSim.GetFieldOrProperty("ElevationTempData");
            double[] resultAzTemp = (double[])privSim.GetFieldOrProperty("AzimuthTempData");
            RawAccelerometerData[] resultElAcc = (RawAccelerometerData[])privSim.GetFieldOrProperty("ElevationAccData");
            RawAccelerometerData[] resultAzAcc = (RawAccelerometerData[])privSim.GetFieldOrProperty("AzimuthAccData");
            RawAccelerometerData[] resultCbAcc = (RawAccelerometerData[])privSim.GetFieldOrProperty("CounterbalanceAccData");
            double[] resultElEnc = (double[])privSim.GetFieldOrProperty("ElevationEncoderData");
            double[] resultAzEnc = (double[])privSim.GetFieldOrProperty("AzimuthEncoderData");

            // Create expected array length (spoiler: it is zero)
            int expectedArrLength = 0;

            // Verify all arrays still have a length of 0 except the null one
            Assert.AreEqual(expectedArrLength, resultElTemp.Length);
            Assert.IsNull(resultAzTemp);
            Assert.AreEqual(expectedArrLength, resultElAcc.Length);
            Assert.AreEqual(expectedArrLength, resultAzAcc.Length);
            Assert.AreEqual(expectedArrLength, resultCbAcc.Length);
            Assert.AreEqual(expectedArrLength, resultElEnc.Length);
            Assert.AreEqual(expectedArrLength, resultAzEnc.Length);
        }

        [TestMethod]
        public void TestInitializeSensors_DisableElEncoder_OnlyElEncoderDisabled()
        {
            PrivateObject privSim = new PrivateObject(SimSensorNetwork);

            // Create an initialization that will enable all sensors.
            byte[] init = new byte[SensorNetworkConstants.SensorNetworkSensorCount];

            init[(int)SensorInitializationEnum.ElevationTemp] = 1;
            init[(int)SensorInitializationEnum.AzimuthTemp] = 1;
            init[(int)SensorInitializationEnum.ElevationEncoder] = 0;
            init[(int)SensorInitializationEnum.AzimuthEncoder] = 1;
            init[(int)SensorInitializationEnum.AzimuthAccelerometer] = 1;
            init[(int)SensorInitializationEnum.ElevationAccelerometer] = 1;
            init[(int)SensorInitializationEnum.CounterbalanceAccelerometer] = 1;

            privSim.Invoke("InitializeSensors", init);

            // Gather result data
            double[] resultElTemp = (double[])privSim.GetFieldOrProperty("ElevationTempData");
            double[] resultAzTemp = (double[])privSim.GetFieldOrProperty("AzimuthTempData");
            RawAccelerometerData[] resultElAcc = (RawAccelerometerData[])privSim.GetFieldOrProperty("ElevationAccData");
            RawAccelerometerData[] resultAzAcc = (RawAccelerometerData[])privSim.GetFieldOrProperty("AzimuthAccData");
            RawAccelerometerData[] resultCbAcc = (RawAccelerometerData[])privSim.GetFieldOrProperty("CounterbalanceAccData");
            double[] resultElEnc = (double[])privSim.GetFieldOrProperty("ElevationEncoderData");
            double[] resultAzEnc = (double[])privSim.GetFieldOrProperty("AzimuthEncoderData");

            // Create expected array length (spoiler: it is zero)
            int expectedArrLength = 0;

            // Verify all arrays still have a length of 0 except the null one
            Assert.AreEqual(expectedArrLength, resultElTemp.Length);
            Assert.AreEqual(expectedArrLength, resultAzTemp.Length);
            Assert.IsNull(resultElEnc);
            Assert.AreEqual(expectedArrLength, resultAzEnc.Length);
            Assert.AreEqual(expectedArrLength, resultElAcc.Length);
            Assert.AreEqual(expectedArrLength, resultAzAcc.Length);
            Assert.AreEqual(expectedArrLength, resultCbAcc.Length);
        }

        [TestMethod]
        public void TestInitializeSensors_DisableAzEncoder_OnlyAzEncoderDisabled()
        {
            PrivateObject privSim = new PrivateObject(SimSensorNetwork);

            // Create an initialization that will enable all sensors.
            byte[] init = new byte[SensorNetworkConstants.SensorNetworkSensorCount];

            init[(int)SensorInitializationEnum.ElevationTemp] = 1;
            init[(int)SensorInitializationEnum.AzimuthTemp] = 1;
            init[(int)SensorInitializationEnum.ElevationEncoder] = 1;
            init[(int)SensorInitializationEnum.AzimuthEncoder] = 0;
            init[(int)SensorInitializationEnum.AzimuthAccelerometer] = 1;
            init[(int)SensorInitializationEnum.ElevationAccelerometer] = 1;
            init[(int)SensorInitializationEnum.CounterbalanceAccelerometer] = 1;

            privSim.Invoke("InitializeSensors", init);

            // Gather result data
            double[] resultElTemp = (double[])privSim.GetFieldOrProperty("ElevationTempData");
            double[] resultAzTemp = (double[])privSim.GetFieldOrProperty("AzimuthTempData");
            RawAccelerometerData[] resultElAcc = (RawAccelerometerData[])privSim.GetFieldOrProperty("ElevationAccData");
            RawAccelerometerData[] resultAzAcc = (RawAccelerometerData[])privSim.GetFieldOrProperty("AzimuthAccData");
            RawAccelerometerData[] resultCbAcc = (RawAccelerometerData[])privSim.GetFieldOrProperty("CounterbalanceAccData");
            double[] resultElEnc = (double[])privSim.GetFieldOrProperty("ElevationEncoderData");
            double[] resultAzEnc = (double[])privSim.GetFieldOrProperty("AzimuthEncoderData");

            // Create expected array length (spoiler: it is zero)
            int expectedArrLength = 0;

            // Verify all arrays still have a length of 0 except the null one
            Assert.AreEqual(expectedArrLength, resultElTemp.Length);
            Assert.AreEqual(expectedArrLength, resultAzTemp.Length);
            Assert.AreEqual(expectedArrLength, resultElEnc.Length);
            Assert.IsNull(resultAzEnc);
            Assert.AreEqual(expectedArrLength, resultElAcc.Length);
            Assert.AreEqual(expectedArrLength, resultAzAcc.Length);
            Assert.AreEqual(expectedArrLength, resultCbAcc.Length);
        }

        [TestMethod]
        public void TestInitializeSensors_DisableAzAcc_OnlyAzAccDisabled()
        {
            PrivateObject privSim = new PrivateObject(SimSensorNetwork);

            // Create an initialization that will enable all sensors.
            byte[] init = new byte[SensorNetworkConstants.SensorNetworkSensorCount];

            init[(int)SensorInitializationEnum.ElevationTemp] = 1;
            init[(int)SensorInitializationEnum.AzimuthTemp] = 1;
            init[(int)SensorInitializationEnum.ElevationEncoder] = 1;
            init[(int)SensorInitializationEnum.AzimuthEncoder] = 1;
            init[(int)SensorInitializationEnum.AzimuthAccelerometer] = 0;
            init[(int)SensorInitializationEnum.ElevationAccelerometer] = 1;
            init[(int)SensorInitializationEnum.CounterbalanceAccelerometer] = 1;

            privSim.Invoke("InitializeSensors", init);

            // Gather result data
            double[] resultElTemp = (double[])privSim.GetFieldOrProperty("ElevationTempData");
            double[] resultAzTemp = (double[])privSim.GetFieldOrProperty("AzimuthTempData");
            RawAccelerometerData[] resultElAcc = (RawAccelerometerData[])privSim.GetFieldOrProperty("ElevationAccData");
            RawAccelerometerData[] resultAzAcc = (RawAccelerometerData[])privSim.GetFieldOrProperty("AzimuthAccData");
            RawAccelerometerData[] resultCbAcc = (RawAccelerometerData[])privSim.GetFieldOrProperty("CounterbalanceAccData");
            double[] resultElEnc = (double[])privSim.GetFieldOrProperty("ElevationEncoderData");
            double[] resultAzEnc = (double[])privSim.GetFieldOrProperty("AzimuthEncoderData");

            // Create expected array length (spoiler: it is zero)
            int expectedArrLength = 0;

            // Verify all arrays still have a length of 0 except the null one
            Assert.AreEqual(expectedArrLength, resultElTemp.Length);
            Assert.AreEqual(expectedArrLength, resultAzTemp.Length);
            Assert.AreEqual(expectedArrLength, resultElEnc.Length);
            Assert.AreEqual(expectedArrLength, resultAzEnc.Length);
            Assert.IsNull(resultAzAcc);
            Assert.AreEqual(expectedArrLength, resultElAcc.Length);
            Assert.AreEqual(expectedArrLength, resultCbAcc.Length);
        }

        [TestMethod]
        public void TestInitializeSensors_DisableElAcc_OnlyElAccDisabled()
        {
            PrivateObject privSim = new PrivateObject(SimSensorNetwork);

            byte[] init = new byte[SensorNetworkConstants.SensorNetworkSensorCount];

            init[(int)SensorInitializationEnum.ElevationTemp] = 1;
            init[(int)SensorInitializationEnum.AzimuthTemp] = 1;
            init[(int)SensorInitializationEnum.ElevationEncoder] = 1;
            init[(int)SensorInitializationEnum.AzimuthEncoder] = 1;
            init[(int)SensorInitializationEnum.AzimuthAccelerometer] = 1;
            init[(int)SensorInitializationEnum.ElevationAccelerometer] = 0;
            init[(int)SensorInitializationEnum.CounterbalanceAccelerometer] = 1;

            privSim.Invoke("InitializeSensors", init);

            // Gather result data
            double[] resultElTemp = (double[])privSim.GetFieldOrProperty("ElevationTempData");
            double[] resultAzTemp = (double[])privSim.GetFieldOrProperty("AzimuthTempData");
            RawAccelerometerData[] resultElAcc = (RawAccelerometerData[])privSim.GetFieldOrProperty("ElevationAccData");
            RawAccelerometerData[] resultAzAcc = (RawAccelerometerData[])privSim.GetFieldOrProperty("AzimuthAccData");
            RawAccelerometerData[] resultCbAcc = (RawAccelerometerData[])privSim.GetFieldOrProperty("CounterbalanceAccData");
            double[] resultElEnc = (double[])privSim.GetFieldOrProperty("ElevationEncoderData");
            double[] resultAzEnc = (double[])privSim.GetFieldOrProperty("AzimuthEncoderData");

            // Create expected array length (spoiler: it is zero)
            int expectedArrLength = 0;

            // Verify all arrays still have a length of 0 except the null one
            Assert.AreEqual(expectedArrLength, resultElTemp.Length);
            Assert.AreEqual(expectedArrLength, resultAzTemp.Length);
            Assert.AreEqual(expectedArrLength, resultElEnc.Length);
            Assert.AreEqual(expectedArrLength, resultAzEnc.Length);
            Assert.AreEqual(expectedArrLength, resultAzAcc.Length);
            Assert.IsNull(resultElAcc);
            Assert.AreEqual(expectedArrLength, resultCbAcc.Length);
        }

        [TestMethod]
        public void TestInitializeSensors_DisableCbAcc_OnlyCbAccDisabled()
        {
            PrivateObject privSim = new PrivateObject(SimSensorNetwork);

            byte[] init = new byte[SensorNetworkConstants.SensorNetworkSensorCount];

            init[(int)SensorInitializationEnum.ElevationTemp] = 1;
            init[(int)SensorInitializationEnum.AzimuthTemp] = 1;
            init[(int)SensorInitializationEnum.ElevationEncoder] = 1;
            init[(int)SensorInitializationEnum.AzimuthEncoder] = 1;
            init[(int)SensorInitializationEnum.AzimuthAccelerometer] = 1;
            init[(int)SensorInitializationEnum.ElevationAccelerometer] = 1;
            init[(int)SensorInitializationEnum.CounterbalanceAccelerometer] = 0;

            privSim.Invoke("InitializeSensors", init);

            // Gather result data
            double[] resultElTemp = (double[])privSim.GetFieldOrProperty("ElevationTempData");
            double[] resultAzTemp = (double[])privSim.GetFieldOrProperty("AzimuthTempData");
            RawAccelerometerData[] resultElAcc = (RawAccelerometerData[])privSim.GetFieldOrProperty("ElevationAccData");
            RawAccelerometerData[] resultAzAcc = (RawAccelerometerData[])privSim.GetFieldOrProperty("AzimuthAccData");
            RawAccelerometerData[] resultCbAcc = (RawAccelerometerData[])privSim.GetFieldOrProperty("CounterbalanceAccData");
            double[] resultElEnc = (double[])privSim.GetFieldOrProperty("ElevationEncoderData");
            double[] resultAzEnc = (double[])privSim.GetFieldOrProperty("AzimuthEncoderData");

            // Create expected array length (spoiler: it is zero)
            int expectedArrLength = 0;

            // Verify all arrays still have a length of 0 except the null one
            Assert.AreEqual(expectedArrLength, resultElTemp.Length);
            Assert.AreEqual(expectedArrLength, resultAzTemp.Length);
            Assert.AreEqual(expectedArrLength, resultElEnc.Length);
            Assert.AreEqual(expectedArrLength, resultAzEnc.Length);
            Assert.AreEqual(expectedArrLength, resultAzAcc.Length);
            Assert.AreEqual(expectedArrLength, resultElAcc.Length);
            Assert.IsNull(resultCbAcc);
        }

        [TestMethod]
        public void TestInitializeSensors_DisableAllSensors_AllSensorsDisabled()
        {
            PrivateObject privSim = new PrivateObject(SimSensorNetwork);

            // Create an initialization
            byte[] init = new byte[SensorNetworkConstants.SensorNetworkSensorCount];

            init[(int)SensorInitializationEnum.ElevationTemp] = 0;
            init[(int)SensorInitializationEnum.AzimuthTemp] = 0;
            init[(int)SensorInitializationEnum.ElevationEncoder] = 0;
            init[(int)SensorInitializationEnum.AzimuthEncoder] = 0;
            init[(int)SensorInitializationEnum.AzimuthAccelerometer] = 0;
            init[(int)SensorInitializationEnum.ElevationAccelerometer] = 0;
            init[(int)SensorInitializationEnum.CounterbalanceAccelerometer] = 0;

            privSim.Invoke("InitializeSensors", init);

            // Gather result data
            double[] resultElTemp = (double[])privSim.GetFieldOrProperty("ElevationTempData");
            double[] resultAzTemp = (double[])privSim.GetFieldOrProperty("AzimuthTempData");
            RawAccelerometerData[] resultElAcc = (RawAccelerometerData[])privSim.GetFieldOrProperty("ElevationAccData");
            RawAccelerometerData[] resultAzAcc = (RawAccelerometerData[])privSim.GetFieldOrProperty("AzimuthAccData");
            RawAccelerometerData[] resultCbAcc = (RawAccelerometerData[])privSim.GetFieldOrProperty("CounterbalanceAccData");
            double[] resultElEnc = (double[])privSim.GetFieldOrProperty("ElevationEncoderData");
            double[] resultAzEnc = (double[])privSim.GetFieldOrProperty("AzimuthEncoderData");

            // Verify all arrays still have a length of 0 except the null one
            Assert.IsNull(resultElTemp);
            Assert.IsNull(resultAzTemp);
            Assert.IsNull(resultElEnc);
            Assert.IsNull(resultAzEnc);
            Assert.IsNull(resultAzAcc);
            Assert.IsNull(resultElAcc);
            Assert.IsNull(resultCbAcc);
        }

        // For these methods, each CSV file contains one entry, with each value
        // being different than another

        [TestMethod]
        public void TestReadFakeDataFromCSV_ElTemp_ElTempsAreCorrect()
        {
            PrivateObject privSim = new PrivateObject(SimSensorNetwork);

            // Initialize only one sensor, so that is the only sensor that gets CSV data.
            byte[] init = new byte[SensorNetworkConstants.SensorNetworkSensorCount];

            init[(int)SensorInitializationEnum.ElevationTemp] = 1;
            init[(int)SensorInitializationEnum.AzimuthTemp] = 0;
            init[(int)SensorInitializationEnum.ElevationEncoder] = 0;
            init[(int)SensorInitializationEnum.AzimuthEncoder] = 0;
            init[(int)SensorInitializationEnum.AzimuthAccelerometer] = 0;
            init[(int)SensorInitializationEnum.ElevationAccelerometer] = 0;
            init[(int)SensorInitializationEnum.CounterbalanceAccelerometer] = 0;

            privSim.Invoke("InitializeSensors", init);

            privSim.Invoke("ReadFakeDataFromCSV");

            // Gather result data
            double[] resultElTemp = (double[])privSim.GetFieldOrProperty("ElevationTempData");
            double[] resultAzTemp = (double[])privSim.GetFieldOrProperty("AzimuthTempData");
            RawAccelerometerData[] resultElAcc = (RawAccelerometerData[])privSim.GetFieldOrProperty("ElevationAccData");
            RawAccelerometerData[] resultAzAcc = (RawAccelerometerData[])privSim.GetFieldOrProperty("AzimuthAccData");
            RawAccelerometerData[] resultCbAcc = (RawAccelerometerData[])privSim.GetFieldOrProperty("CounterbalanceAccData");
            double[] resultElEnc = (double[])privSim.GetFieldOrProperty("ElevationEncoderData");
            double[] resultAzEnc = (double[])privSim.GetFieldOrProperty("AzimuthEncoderData");

            double[] expectedArray = new double[] { 1, 2 };

            // Verify the result array is as expected
            Assert.IsTrue(expectedArray.SequenceEqual(resultElTemp));

            // Verify the rest of the fields are still null
            Assert.IsNull(resultAzTemp);
            Assert.IsNull(resultElEnc);
            Assert.IsNull(resultAzEnc);
            Assert.IsNull(resultAzAcc);
            Assert.IsNull(resultElAcc);
            Assert.IsNull(resultCbAcc);
        }

        [TestMethod]
        public void TestReadFakeDataFromCSV_AzTemp_AzTempsAreCorrect()
        {
            PrivateObject privSim = new PrivateObject(SimSensorNetwork);

            // Initialize only one sensor, so that is the only sensor that gets CSV data.
            byte[] init = new byte[SensorNetworkConstants.SensorNetworkSensorCount];

            init[(int)SensorInitializationEnum.ElevationTemp] = 0;
            init[(int)SensorInitializationEnum.AzimuthTemp] = 1;
            init[(int)SensorInitializationEnum.ElevationEncoder] = 0;
            init[(int)SensorInitializationEnum.AzimuthEncoder] = 0;
            init[(int)SensorInitializationEnum.AzimuthAccelerometer] = 0;
            init[(int)SensorInitializationEnum.ElevationAccelerometer] = 0;
            init[(int)SensorInitializationEnum.CounterbalanceAccelerometer] = 0;

            privSim.Invoke("InitializeSensors", init);

            privSim.Invoke("ReadFakeDataFromCSV");

            // Gather result data
            double[] resultElTemp = (double[])privSim.GetFieldOrProperty("ElevationTempData");
            double[] resultAzTemp = (double[])privSim.GetFieldOrProperty("AzimuthTempData");
            RawAccelerometerData[] resultElAcc = (RawAccelerometerData[])privSim.GetFieldOrProperty("ElevationAccData");
            RawAccelerometerData[] resultAzAcc = (RawAccelerometerData[])privSim.GetFieldOrProperty("AzimuthAccData");
            RawAccelerometerData[] resultCbAcc = (RawAccelerometerData[])privSim.GetFieldOrProperty("CounterbalanceAccData");
            double[] resultElEnc = (double[])privSim.GetFieldOrProperty("ElevationEncoderData");
            double[] resultAzEnc = (double[])privSim.GetFieldOrProperty("AzimuthEncoderData");

            double[] expectedArray = new double[] { 2, 3 };

            // Verify the result array is as expected
            Assert.IsTrue(expectedArray.SequenceEqual(resultAzTemp));

            // Verify the rest of the fields are still null
            Assert.IsNull(resultElTemp);
            Assert.IsNull(resultElEnc);
            Assert.IsNull(resultAzEnc);
            Assert.IsNull(resultAzAcc);
            Assert.IsNull(resultElAcc);
            Assert.IsNull(resultCbAcc);
        }

        [TestMethod]
        public void TestReadFakeDataFromCSV_ElEnc_ElEncPosAreCorrect()
        {
            PrivateObject privSim = new PrivateObject(SimSensorNetwork);

            // Initialize only one sensor, so that is the only sensor that gets CSV data.
            byte[] init = new byte[SensorNetworkConstants.SensorNetworkSensorCount];

            init[(int)SensorInitializationEnum.ElevationTemp] = 0;
            init[(int)SensorInitializationEnum.AzimuthTemp] = 0;
            init[(int)SensorInitializationEnum.ElevationEncoder] = 1;
            init[(int)SensorInitializationEnum.AzimuthEncoder] = 0;
            init[(int)SensorInitializationEnum.AzimuthAccelerometer] = 0;
            init[(int)SensorInitializationEnum.ElevationAccelerometer] = 0;
            init[(int)SensorInitializationEnum.CounterbalanceAccelerometer] = 0;

            privSim.Invoke("InitializeSensors", init);

            privSim.Invoke("ReadFakeDataFromCSV");

            // Gather result data
            double[] resultElTemp = (double[])privSim.GetFieldOrProperty("ElevationTempData");
            double[] resultAzTemp = (double[])privSim.GetFieldOrProperty("AzimuthTempData");
            RawAccelerometerData[] resultElAcc = (RawAccelerometerData[])privSim.GetFieldOrProperty("ElevationAccData");
            RawAccelerometerData[] resultAzAcc = (RawAccelerometerData[])privSim.GetFieldOrProperty("AzimuthAccData");
            RawAccelerometerData[] resultCbAcc = (RawAccelerometerData[])privSim.GetFieldOrProperty("CounterbalanceAccData");
            double[] resultElEnc = (double[])privSim.GetFieldOrProperty("ElevationEncoderData");
            double[] resultAzEnc = (double[])privSim.GetFieldOrProperty("AzimuthEncoderData");

            double[] expectedArray = new double[] { 3, 4 };

            // Verify the result array is as expected
            Assert.IsTrue(expectedArray.SequenceEqual(resultElEnc));

            // Verify the rest of the fields are still null
            Assert.IsNull(resultElTemp);
            Assert.IsNull(resultAzTemp);
            Assert.IsNull(resultAzEnc);
            Assert.IsNull(resultAzAcc);
            Assert.IsNull(resultElAcc);
            Assert.IsNull(resultCbAcc);
        }

        [TestMethod]
        public void TestReadFakeDataFromCSV_AzEnc_AzEncPosAreCorrect()
        {
            PrivateObject privSim = new PrivateObject(SimSensorNetwork);

            // Initialize only one sensor, so that is the only sensor that gets CSV data.
            byte[] init = new byte[SensorNetworkConstants.SensorNetworkSensorCount];

            init[(int)SensorInitializationEnum.ElevationTemp] = 0;
            init[(int)SensorInitializationEnum.AzimuthTemp] = 0;
            init[(int)SensorInitializationEnum.ElevationEncoder] = 0;
            init[(int)SensorInitializationEnum.AzimuthEncoder] = 1;
            init[(int)SensorInitializationEnum.AzimuthAccelerometer] = 0;
            init[(int)SensorInitializationEnum.ElevationAccelerometer] = 0;
            init[(int)SensorInitializationEnum.CounterbalanceAccelerometer] = 0;

            privSim.Invoke("InitializeSensors", init);

            privSim.Invoke("ReadFakeDataFromCSV");

            // Gather result data
            double[] resultElTemp = (double[])privSim.GetFieldOrProperty("ElevationTempData");
            double[] resultAzTemp = (double[])privSim.GetFieldOrProperty("AzimuthTempData");
            RawAccelerometerData[] resultElAcc = (RawAccelerometerData[])privSim.GetFieldOrProperty("ElevationAccData");
            RawAccelerometerData[] resultAzAcc = (RawAccelerometerData[])privSim.GetFieldOrProperty("AzimuthAccData");
            RawAccelerometerData[] resultCbAcc = (RawAccelerometerData[])privSim.GetFieldOrProperty("CounterbalanceAccData");
            double[] resultElEnc = (double[])privSim.GetFieldOrProperty("ElevationEncoderData");
            double[] resultAzEnc = (double[])privSim.GetFieldOrProperty("AzimuthEncoderData");

            double[] expectedArray = new double[] { 4, 5 };

            // Verify the result array is as expected
            Assert.IsTrue(expectedArray.SequenceEqual(resultAzEnc));

            // Verify the rest of the fields are still null
            Assert.IsNull(resultElTemp);
            Assert.IsNull(resultAzTemp);
            Assert.IsNull(resultElEnc);
            Assert.IsNull(resultAzAcc);
            Assert.IsNull(resultElAcc);
            Assert.IsNull(resultCbAcc);
        }

        [TestMethod]
        public void TestReadFakeDataFromCSV_AzAcc_AzAccAreCorrect()
        {
            PrivateObject privSim = new PrivateObject(SimSensorNetwork);

            byte[] init = new byte[SensorNetworkConstants.SensorNetworkSensorCount];

            init[(int)SensorInitializationEnum.ElevationTemp] = 0;
            init[(int)SensorInitializationEnum.AzimuthTemp] = 0;
            init[(int)SensorInitializationEnum.ElevationEncoder] = 0;
            init[(int)SensorInitializationEnum.AzimuthEncoder] = 0;
            init[(int)SensorInitializationEnum.AzimuthAccelerometer] = 1;
            init[(int)SensorInitializationEnum.ElevationAccelerometer] = 0;
            init[(int)SensorInitializationEnum.CounterbalanceAccelerometer] = 0;

            privSim.Invoke("InitializeSensors", init);

            privSim.Invoke("ReadFakeDataFromCSV");

            // Gather result data
            double[] resultElTemp = (double[])privSim.GetFieldOrProperty("ElevationTempData");
            double[] resultAzTemp = (double[])privSim.GetFieldOrProperty("AzimuthTempData");
            RawAccelerometerData[] resultElAcc = (RawAccelerometerData[])privSim.GetFieldOrProperty("ElevationAccData");
            RawAccelerometerData[] resultAzAcc = (RawAccelerometerData[])privSim.GetFieldOrProperty("AzimuthAccData");
            RawAccelerometerData[] resultCbAcc = (RawAccelerometerData[])privSim.GetFieldOrProperty("CounterbalanceAccData");
            double[] resultElEnc = (double[])privSim.GetFieldOrProperty("ElevationEncoderData");
            double[] resultAzEnc = (double[])privSim.GetFieldOrProperty("AzimuthEncoderData");

            RawAccelerometerData[] expectedAcc = new RawAccelerometerData[400];

            for (int i = 0; i < 400; i++)
            {
                if (i <= 199)
                {
                    expectedAcc[i].X = 6;
                    expectedAcc[i].Y = 13;
                    expectedAcc[i].Z = 11;
                }
                else
                {
                    expectedAcc[i].X = 5;
                    expectedAcc[i].Y = 12;
                    expectedAcc[i].Z = 10;
                }
            }

            // Verify the result array is as expected
            Assert.IsTrue(expectedAcc.SequenceEqual(resultAzAcc));

            // Verify the rest of the fields are still null
            Assert.IsNull(resultElTemp);
            Assert.IsNull(resultAzTemp);
            Assert.IsNull(resultElEnc);
            Assert.IsNull(resultAzEnc);
            Assert.IsNull(resultElAcc);
            Assert.IsNull(resultCbAcc);
        }

        [TestMethod]
        public void TestReadFakeDataFromCSV_ElAcc_ElAccAreCorrect()
        {
            PrivateObject privSim = new PrivateObject(SimSensorNetwork);

            byte[] init = new byte[SensorNetworkConstants.SensorNetworkSensorCount];

            init[(int)SensorInitializationEnum.ElevationTemp] = 0;
            init[(int)SensorInitializationEnum.AzimuthTemp] = 0;
            init[(int)SensorInitializationEnum.ElevationEncoder] = 0;
            init[(int)SensorInitializationEnum.AzimuthEncoder] = 0;
            init[(int)SensorInitializationEnum.AzimuthAccelerometer] = 0;
            init[(int)SensorInitializationEnum.ElevationAccelerometer] = 1;
            init[(int)SensorInitializationEnum.CounterbalanceAccelerometer] = 0;

            privSim.Invoke("InitializeSensors", init);

            privSim.Invoke("ReadFakeDataFromCSV");

            // Gather result data
            double[] resultElTemp = (double[])privSim.GetFieldOrProperty("ElevationTempData");
            double[] resultAzTemp = (double[])privSim.GetFieldOrProperty("AzimuthTempData");
            RawAccelerometerData[] resultElAcc = (RawAccelerometerData[])privSim.GetFieldOrProperty("ElevationAccData");
            RawAccelerometerData[] resultAzAcc = (RawAccelerometerData[])privSim.GetFieldOrProperty("AzimuthAccData");
            RawAccelerometerData[] resultCbAcc = (RawAccelerometerData[])privSim.GetFieldOrProperty("CounterbalanceAccData");
            double[] resultElEnc = (double[])privSim.GetFieldOrProperty("ElevationEncoderData");
            double[] resultAzEnc = (double[])privSim.GetFieldOrProperty("AzimuthEncoderData");

            RawAccelerometerData[] expectedAcc = new RawAccelerometerData[400];

            for (int i = 0; i < 400; i++)
            {
                if (i <= 199)
                {
                    expectedAcc[i].X = 6;
                    expectedAcc[i].Y = 13;
                    expectedAcc[i].Z = 11;
                }
                else
                {
                    expectedAcc[i].X = 5;
                    expectedAcc[i].Y = 12;
                    expectedAcc[i].Z = 10;
                }
            }

            // Verify the result array is as expected
            Assert.IsTrue(expectedAcc.SequenceEqual(resultElAcc));

            // Verify the rest of the fields are still null
            Assert.IsNull(resultElTemp);
            Assert.IsNull(resultAzTemp);
            Assert.IsNull(resultElEnc);
            Assert.IsNull(resultAzEnc);
            Assert.IsNull(resultAzAcc);
            Assert.IsNull(resultCbAcc);
        }

        [TestMethod]
        public void TestReadFakeDataFromCSV_CbAcc_CbAccAreCorrect()
        {
            PrivateObject privSim = new PrivateObject(SimSensorNetwork);

            byte[] init = new byte[SensorNetworkConstants.SensorNetworkSensorCount];

            init[(int)SensorInitializationEnum.ElevationTemp] = 0;
            init[(int)SensorInitializationEnum.AzimuthTemp] = 0;
            init[(int)SensorInitializationEnum.ElevationEncoder] = 0;
            init[(int)SensorInitializationEnum.AzimuthEncoder] = 0;
            init[(int)SensorInitializationEnum.AzimuthAccelerometer] = 0;
            init[(int)SensorInitializationEnum.ElevationAccelerometer] = 0;
            init[(int)SensorInitializationEnum.CounterbalanceAccelerometer] = 1;

            privSim.Invoke("InitializeSensors", init);

            privSim.Invoke("ReadFakeDataFromCSV");

            // Gather result data
            double[] resultElTemp = (double[])privSim.GetFieldOrProperty("ElevationTempData");
            double[] resultAzTemp = (double[])privSim.GetFieldOrProperty("AzimuthTempData");
            RawAccelerometerData[] resultElAcc = (RawAccelerometerData[])privSim.GetFieldOrProperty("ElevationAccData");
            RawAccelerometerData[] resultAzAcc = (RawAccelerometerData[])privSim.GetFieldOrProperty("AzimuthAccData");
            RawAccelerometerData[] resultCbAcc = (RawAccelerometerData[])privSim.GetFieldOrProperty("CounterbalanceAccData");
            double[] resultElEnc = (double[])privSim.GetFieldOrProperty("ElevationEncoderData");
            double[] resultAzEnc = (double[])privSim.GetFieldOrProperty("AzimuthEncoderData");

            RawAccelerometerData[] expectedAcc = new RawAccelerometerData[400];

            for (int i = 0; i < 400; i++)
            {
                if (i <= 199)
                {
                    expectedAcc[i].X = 6;
                    expectedAcc[i].Y = 13;
                    expectedAcc[i].Z = 11;
                }
                else
                {
                    expectedAcc[i].X = 5;
                    expectedAcc[i].Y = 12;
                    expectedAcc[i].Z = 10;
                }
            }

            // Verify the result array is as expected
            Assert.IsTrue(expectedAcc.SequenceEqual(resultCbAcc));

            // Verify the rest of the fields are still null
            Assert.IsNull(resultElTemp);
            Assert.IsNull(resultAzTemp);
            Assert.IsNull(resultElEnc);
            Assert.IsNull(resultAzEnc);
            Assert.IsNull(resultAzAcc);
            Assert.IsNull(resultElAcc);
        }

        [TestMethod]
        public void TestWaitForAndConnectToServer_ConnectToServer_AsksForConfiguration()
        {
            PrivateObject privSim = new PrivateObject(SimSensorNetwork);

            byte[] expected = Encoding.ASCII.GetBytes("Send Sensor Configuration");
            byte[] result = new byte[expected.Length];

            // First create server that expects the "Send Sensor Configuration" message
            Thread expectConfThread = new Thread(() =>
            {
                TcpListener confListen = new TcpListener(IPAddress.Parse(ClientIP), ClientPort);
                confListen.Start();

                NetworkStream confStream;
                TcpClient localClient = confListen.AcceptTcpClient();

                confStream = localClient.GetStream();
                confStream.Read(result, 0, expected.Length);

                confListen.Stop();
                confStream.Close();
                confStream.Dispose();
            });
            expectConfThread.Start();

            // This method has a blocking method, so we must run it in a separate thread
            Thread serverThread = new Thread(() => {
                // Set up client on the simulation
                privSim.Invoke("WaitForAndConnectToServer");
            });
            serverThread.Start();

            expectConfThread.Join();

            // Stop the server in the simulation
            ((TcpListener)privSim.GetFieldOrProperty("Server")).Stop();
            serverThread.Join();

            Assert.IsTrue(expected.SequenceEqual(result));
        }
        
        [TestMethod]
        public void TestRequestAndAcquireSensorInitialization_SendsConfiguration_ReceiveCorrectBytes()
        {
            PrivateObject privSim = new PrivateObject(SimSensorNetwork);
            

            // First create server that expects the "Send Sensor Configuration" message
            // This only takes in the first
            Thread expectConfThread = new Thread(() =>
            {

                TcpListener confListen = new TcpListener(IPAddress.Parse(ClientIP), ClientPort);
                confListen.Start();

                NetworkStream confStream;
                TcpClient localClient = confListen.AcceptTcpClient();

                byte[] dontCareVal = new byte[1];

                confStream = localClient.GetStream();
                confStream.Read(dontCareVal, 0, dontCareVal.Length);

                confListen.Stop();
                confStream.Close();
                confStream.Dispose();
            });
            expectConfThread.Start();

            byte[] expected = Encoding.ASCII.GetBytes("1234567");
            byte[] result = new byte[expected.Length];

            // This method has a blocking method, so we must run it in a separate thread
            Thread serverThread = new Thread(() => {

                // Set up client on the simulation
                privSim.Invoke("WaitForAndConnectToServer");

                result = (byte[])privSim.Invoke("RequestAndAcquireSensorInitialization");
            });
            serverThread.Start();

            expectConfThread.Join();

            // Create a client to send something to the server
            // We are using the server IP and port because these must be the same
            // between client/server in order to transmit data
            TcpClient client = new TcpClient(ServerIP.ToString(), ServerPort);
            NetworkStream stream = client.GetStream();
            stream.Write(expected, 0, expected.Length);

            //stream.Write(expected, 0, expected.Length);
            stream.Flush();

            // Dispose client and end thread
            serverThread.Join();
            stream.Close();
            stream.Dispose();
            client.Close();
            client.Dispose();

            Assert.IsTrue(expected.SequenceEqual(result));
        }

        [TestMethod]
        public void TestStartSimulationSensorNetwork_Starts_AllObjectsAsExpected()
        {
            PrivateObject privSim = new PrivateObject(SimSensorNetwork);

            SimSensorNetwork.StartSimulationSensorNetwork();

            bool resultCurrentlyRunning = (bool)privSim.GetFieldOrProperty("CurrentlyRunning");
            Thread resultMonitoringThread = (Thread)privSim.GetFieldOrProperty("SimulationSensorMonitoringThread");

            Assert.IsTrue(resultCurrentlyRunning);
            Assert.IsTrue(resultMonitoringThread.IsAlive);

            SimSensorNetwork.EndSimulationSensorNetwork();
        }

        [TestMethod]
        public void TestEndSimulationSensorNetwork_Ends_AllObjectsBroughtDown()
        {
            PrivateObject privSim = new PrivateObject(SimSensorNetwork);
            SensorNetworkServer SensorNetworkServer = new SensorNetworkServer(IPAddress.Parse(ClientIP), ClientPort, ServerIP.ToString(), ServerPort, TelescopeId, false);
            SensorNetworkServer.StartSensorMonitoringRoutine();
            SimSensorNetwork.StartSimulationSensorNetwork();

            // Give plenty of time for everything to connect
            Thread.Sleep(2000);

            SimSensorNetwork.EndSimulationSensorNetwork();
            SensorNetworkServer.EndSensorMonitoringRoutine();

            bool resultCurrentlyRunning = (bool)privSim.GetFieldOrProperty("CurrentlyRunning");
            Thread resultMonitoringThread = (Thread)privSim.GetFieldOrProperty("SimulationSensorMonitoringThread");
            TcpClient resultClient = (TcpClient)privSim.GetFieldOrProperty("Client");
            NetworkStream resultClientStream = (NetworkStream)privSim.GetFieldOrProperty("ClientStream");
            TcpListener resultServer = (TcpListener)privSim.GetFieldOrProperty("Server");
            NetworkStream resultServerStream = (NetworkStream)privSim.GetFieldOrProperty("ServerStream");

            Assert.IsFalse(resultCurrentlyRunning);
            Assert.IsFalse(resultMonitoringThread.IsAlive);
            Assert.IsFalse(resultClient.Connected);
            Assert.IsFalse(resultClientStream.CanWrite);
            Assert.IsFalse(resultServer.Server.IsBound);
            Assert.IsFalse(resultServerStream.CanRead);

            // Remove SN config after we're done
            DatabaseOperations.DeleteSensorNetworkConfig(SensorNetworkServer.InitializationClient.config);
        }
    }
}
