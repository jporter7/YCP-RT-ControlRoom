using ControlRoomApplication.Controllers;
using ControlRoomApplication.Controllers.SensorNetwork;
using ControlRoomApplication.Database;
using ControlRoomApplication.Entities;
using ControlRoomApplication.Simulators.Hardware.WeatherStation;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ControlRoomApplicationTest.CommunicationTests
{
    [TestClass]
    public class RemoteListenerTest
    {
        // This will contain the RemoteListener object we are using for the various tests
        PrivateObject PrivListener;
        
        ControlRoom ControlRoom;

        RadioTelescopeController RtController;
        RadioTelescope RadioTelescope;

        // PLC driver
        readonly string PlcIp = "127.0.0.1";
        readonly int PlcPort = 4000;
        readonly string McuIp = "127.0.0.1";
        readonly int McuPort = 4001;

        // Sensor Network
        readonly IPAddress SnServerIp = IPAddress.Parse("127.0.0.1");
        readonly int SnServerPort = 3000;
        readonly string SnClientIp = "127.0.0.1";
        readonly int SnClientPort = 3001;
        readonly int SnTelescopeId = 3000;

        [TestInitialize]
        public void Initialize()
        {
            RadioTelescope = new RadioTelescope();

            AbstractPLCDriver PLC = new SimulationPLCDriver(PlcIp, McuIp, McuPort, PlcPort, true, false);
            RadioTelescope.PLCDriver = PLC;

            SensorNetworkServer SN = new SensorNetworkServer(SnServerIp, SnServerPort, SnClientIp, SnClientPort, SnTelescopeId, true);
            RadioTelescope.SensorNetworkServer = SN;

            RtController = new RadioTelescopeController(RadioTelescope);

            AbstractWeatherStation WS = new SimulationWeatherStation(1000);

            ControlRoom = new ControlRoom(WS);
            
            ControlRoom.mobileControlServer.rtController = RtController;

            PrivListener = new PrivateObject(ControlRoom.mobileControlServer);
        }

        [TestCleanup]
        public void Cleanup()
        {
            // Until a proper bring-down method is created for the Remote Listener, we must stop the server like this after
            // every test.
            ((TcpListener)PrivListener.GetFieldOrProperty("server")).Stop();
            
            DatabaseOperations.DeleteSensorNetworkConfig(RadioTelescope.SensorNetworkServer.InitializationClient.SensorNetworkConfig);
        }

        [TestMethod]
        public void TestProcessMessage_SensorInit_RebootsSensorNetworkWithNewInit()
        {
            // Create and start SensorNetwork
            RadioTelescope.SensorNetworkServer.StartSensorMonitoringRoutine();

            // Build command to disable all sensors and set the timeouts to specific values
            byte expectedDisabled = 0;
            int expectedDataTimeout = 6;
            int expectedInitTimeout = 5;
            string command = 
                $"{expectedDisabled}," +
                $"{expectedDisabled}," +
                $"{expectedDisabled}," +
                $"{expectedDisabled}," +
                $"{expectedDisabled}," +
                $"{expectedDisabled}," +
                $"{expectedDisabled}," +
                $"{expectedDataTimeout}," +
                $"{expectedInitTimeout}," +
                $"SENSOR_INIT";

            bool result = (bool)PrivListener.Invoke("processMessage", command);
            byte[] resultInitBytes = RadioTelescope.SensorNetworkServer.InitializationClient.SensorNetworkConfig.GetSensorInitAsBytes();
            int resultDataTimeout = RadioTelescope.SensorNetworkServer.InitializationClient.SensorNetworkConfig.TimeoutDataRetrieval / 1000; // ms to seconds
            int resultInitTimeout = RadioTelescope.SensorNetworkServer.InitializationClient.SensorNetworkConfig.TimeoutInitialization / 1000; // ms to seconds

            Assert.IsTrue(result);

            // Verify the init values are as expected
            Assert.AreEqual(expectedDisabled, resultInitBytes[(int)SensorInitializationEnum.AzimuthTemp]);
            Assert.AreEqual(expectedDisabled, resultInitBytes[(int)SensorInitializationEnum.ElevationTemp]);
            Assert.AreEqual(expectedDisabled, resultInitBytes[(int)SensorInitializationEnum.AzimuthAccelerometer]);
            Assert.AreEqual(expectedDisabled, resultInitBytes[(int)SensorInitializationEnum.ElevationAccelerometer]);
            Assert.AreEqual(expectedDisabled, resultInitBytes[(int)SensorInitializationEnum.ElevationAccelerometer]);
            Assert.AreEqual(expectedDisabled, resultInitBytes[(int)SensorInitializationEnum.AzimuthEncoder]);
            Assert.AreEqual(expectedDisabled, resultInitBytes[(int)SensorInitializationEnum.ElevationEncoder]);

            // Verify init values are as expected
            Assert.AreEqual(expectedDataTimeout, resultDataTimeout);
            Assert.AreEqual(expectedInitTimeout, resultInitTimeout);

            // Bring down the server and delete config
            RadioTelescope.SensorNetworkServer.EndSensorMonitoringRoutine();
        }

        [TestMethod]
        public void TestProcessMessage_SensorInitInvalidMessage_Fail()
        {
            string invalidCommand = "SENSOR_INIT,1,2,3,4";

            bool result = (bool)PrivListener.Invoke("processMessage", invalidCommand);

            Assert.IsFalse(result);
        }

        [TestMethod]
        public void TestProcessMessage_SetOverrideSetCounterbalanceAcc_SetsCounterbalanceOverride()
        {
            string command = "SET_OVERRIDE OVR CB_ACC";
            
            bool result = (bool)PrivListener.Invoke("processMessage", command);

            Assert.IsTrue(result);
            Assert.IsTrue(RtController.overrides.overrideCounterbalanceAccelerometer);
        }

        [TestMethod]
        public void TestProcessMessage_SetOverrideRemCounterbalanceAcc_RemovesCounterbalanceOverride()
        {
            string command = "SET_OVERRIDE CB_ACC";

            bool result = (bool)PrivListener.Invoke("processMessage", command);

            Assert.IsTrue(result);
            Assert.IsFalse(RtController.overrides.overrideCounterbalanceAccelerometer);
        }

        [TestMethod]
        public void TestProcessMessage_SetOverrideSetElevationAcc_SetsElevationAccOverride()
        {
            string command = "SET_OVERRIDE OVR EL_ACC";

            bool result = (bool)PrivListener.Invoke("processMessage", command);

            Assert.IsTrue(result);
            Assert.IsTrue(RtController.overrides.overrideElevationAccelerometer);
        }

        [TestMethod]
        public void TestProcessMessage_SetOverrideRemElevationAcc_RemovesElevationAccOverride()
        {
            string command = "SET_OVERRIDE EL_ACC";

            bool result = (bool)PrivListener.Invoke("processMessage", command);

            Assert.IsTrue(result);
            Assert.IsFalse(RtController.overrides.overrideElevationAccelerometer);
        }

        [TestMethod]
        public void TestProcessMessage_SetOverrideSetAzimuthAcc_SetsAzimuthAccOverride()
        {
            string command = "SET_OVERRIDE OVR AZ_ACC";

            bool result = (bool)PrivListener.Invoke("processMessage", command);

            Assert.IsTrue(result);
            Assert.IsTrue(RtController.overrides.overrideAzimuthAccelerometer);
        }

        [TestMethod]
        public void TestProcessMessage_SetOverrideRemAzimuthAcc_RemovesAzimuthAccOverride()
        {
            string command = "SET_OVERRIDE AZ_ACC";

            bool result = (bool)PrivListener.Invoke("processMessage", command);

            Assert.IsTrue(result);
            Assert.IsFalse(RtController.overrides.overrideAzimuthAccelerometer);
        }

        [TestMethod]
        public void TestProcessMessage_SetOverrideSetElevationEnc_SetsElevationEncOverride()
        {
            string command = "SET_OVERRIDE OVR EL_ABS_ENC";

            bool result = (bool)PrivListener.Invoke("processMessage", command);

            Assert.IsTrue(result);
            Assert.IsTrue(RtController.overrides.overrideElevationAbsEncoder);
        }

        [TestMethod]
        public void TestProcessMessage_SetOverrideRemElevationEnc_RemovesElevationEncOverride()
        {
            string command = "SET_OVERRIDE EL_ABS_ENC";

            bool result = (bool)PrivListener.Invoke("processMessage", command);

            Assert.IsTrue(result);
            Assert.IsFalse(RtController.overrides.overrideElevationAbsEncoder);
        }

        [TestMethod]
        public void TestProcessMessage_SetOverrideSetAzimuthEnc_SetsAzimuthEncOverride()
        {
            string command = "SET_OVERRIDE OVR AZ_ABS_ENC";

            bool result = (bool)PrivListener.Invoke("processMessage", command);

            Assert.IsTrue(result);
            Assert.IsTrue(RtController.overrides.overrideAzimuthAbsEncoder);
        }

        [TestMethod]
        public void TestProcessMessage_SetOverrideRemAzimuthEnc_RemovesAzimuthEncOverride()
        {
            string command = "SET_OVERRIDE AZ_ABS_ENC";

            bool result = (bool)PrivListener.Invoke("processMessage", command);

            Assert.IsTrue(result);
            Assert.IsFalse(RtController.overrides.overrideAzimuthAbsEncoder);
        }

        [TestMethod]
        public void TestProcessMessage_SetOverrideSetElProx90_SetsElProx90Override()
        {
            string command = "SET_OVERRIDE OVR ELEVATION_LIMIT_90";

            bool result = (bool)PrivListener.Invoke("processMessage", command);

            Assert.IsTrue(result);
            Assert.IsTrue(RtController.overrides.overrideElevatProx90);
        }

        [TestMethod]
        public void TestProcessMessage_SetOverrideRemElProx90_RemovesElProx90Override()
        {
            string command = "SET_OVERRIDE ELEVATION_LIMIT_90";

            bool result = (bool)PrivListener.Invoke("processMessage", command);

            Assert.IsTrue(result);
            Assert.IsFalse(RtController.overrides.overrideElevatProx90);
        }

        [TestMethod]
        public void TestProcessMessage_SetOverrideSetElProx0_SetsElProx0Override()
        {
            string command = "SET_OVERRIDE OVR ELEVATION_LIMIT_0";

            bool result = (bool)PrivListener.Invoke("processMessage", command);

            Assert.IsTrue(result);
            Assert.IsTrue(RtController.overrides.overrideElevatProx0);
        }

        [TestMethod]
        public void TestProcessMessage_SetOverrideSetAzMotTemp_SetsAzMotTempOverride()
        {
            string command = "SET_OVERRIDE OVR AZIMUTH_MOT_TEMP";

            bool result = (bool)PrivListener.Invoke("processMessage", command);

            Assert.IsTrue(result);
            Assert.IsTrue(RtController.overrides.overrideAzimuthMotTemp);
        }

        [TestMethod]
        public void TestProcessMessage_SetOverrideRemAzMotTemp_RemovesAzMotTempOverride()
        {
            string command = "SET_OVERRIDE AZIMUTH_MOT_TEMP";

            bool result = (bool)PrivListener.Invoke("processMessage", command);

            Assert.IsTrue(result);
            Assert.IsFalse(RtController.overrides.overrideAzimuthMotTemp);
        }

        [TestMethod]
        public void TestProcessMessage_SetOverrideSetElMotTemp_SetsElMotTempOverride()
        {
            string command = "SET_OVERRIDE OVR ELEVATION_MOT_TEMP";

            bool result = (bool)PrivListener.Invoke("processMessage", command);

            Assert.IsTrue(result);
            Assert.IsTrue(RtController.overrides.overrideElevatMotTemp);
        }

        [TestMethod]
        public void TestProcessMessage_SetOverrideRemElMotTemp_RemovesElMotTempOverride()
        {
            string command = "SET_OVERRIDE ELEVATION_MOT_TEMP";

            bool result = (bool)PrivListener.Invoke("processMessage", command);

            Assert.IsTrue(result);
            Assert.IsFalse(RtController.overrides.overrideElevatMotTemp);
        }

        [TestMethod]
        public void TestProcessMessage_SetOverrideSetGate_SetsGateOverride()
        {
            string command = "SET_OVERRIDE OVR MAIN_GATE";

            bool result = (bool)PrivListener.Invoke("processMessage", command);

            Assert.IsTrue(result);
            Assert.IsTrue(RtController.overrides.overrideGate);
        }

        [TestMethod]
        public void TestProcessMessage_SetOverrideRemGate_RemovesGateOverride()
        {
            string command = "SET_OVERRIDE MAIN_GATE";

            bool result = (bool)PrivListener.Invoke("processMessage", command);

            Assert.IsTrue(result);
            Assert.IsFalse(RtController.overrides.overrideGate);
        }

        [TestMethod]
        public void TestProcessMessage_SetOverrideSetWeather_SetsWeatherOverride()
        {
            string command = "SET_OVERRIDE OVR WEATHER_STATION";

            bool result = (bool)PrivListener.Invoke("processMessage", command);

            Assert.IsTrue(result);
            Assert.IsTrue(ControlRoom.weatherStationOverride);
        }

        [TestMethod]
        public void TestProcessMessage_SetOverrideRemWeather_RemovesWeatherOverride()
        {
            string command = "SET_OVERRIDE WEATHER_STATION";

            bool result = (bool)PrivListener.Invoke("processMessage", command);

            Assert.IsTrue(result);
            Assert.IsFalse(ControlRoom.weatherStationOverride);
        }

        [TestMethod]
        public void TestProcessMessage_SetOverrideInvalidOverride_Fail()
        {
            string command = "SET_OVERRIDE CACTUS";

            bool result = (bool)PrivListener.Invoke("processMessage", command);

            Assert.IsFalse(result);
        }
    }
}
