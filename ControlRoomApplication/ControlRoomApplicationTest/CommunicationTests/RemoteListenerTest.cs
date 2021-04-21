using ControlRoomApplication.Controllers;
using ControlRoomApplication.Controllers.SensorNetwork;
using ControlRoomApplication.Database;
using ControlRoomApplication.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ControlRoomApplicationTest.CommunicationTests
{
    // TODO: Add additional initialization elements as you need them. I only added what I needed to accomplish the tests currently written here, but more
    // will need to be done to test the rest of the functionality.

    [TestClass]
    public class RemoteListenerTest
    {
        RemoteListener Listener;
        int ListenerPort = 80;
        PrivateObject PrivListener;

        // None of the tests I've written so far require a real Control Room, so until the Weather Station override test
        // is written, this can stay a dummy.
        ControlRoom DummyControlRoom;

        RadioTelescopeController RtController;
        RadioTelescope RadioTelescope;

        [TestInitialize]
        public void Initialize()
        {
            RadioTelescope = new RadioTelescope();
            RtController = new RadioTelescopeController(RadioTelescope);

            Listener = new RemoteListener(ListenerPort, DummyControlRoom);
            Listener.rtController = RtController;

            PrivListener = new PrivateObject(Listener);
        }

        [TestMethod]
        public void TestProcessMessage_SensorInit_RebootsSensorNetworkWithNewInit()
        {

            IPAddress serverIp = IPAddress.Parse("127.0.0.1");
            int serverPort = 3000;
            string clientIp = "127.0.0.1";
            int clientPort = 3001;
            int telescopeId = 3000;

            // Create and start SensorNetwork
            SensorNetworkServer server = new SensorNetworkServer(serverIp, serverPort, clientIp, clientPort, telescopeId, true);
            RadioTelescope.SensorNetworkServer = server;

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
            byte[] resultInitBytes = RadioTelescope.SensorNetworkServer.InitializationClient.config.GetSensorInitAsBytes();
            int resultDataTimeout = RadioTelescope.SensorNetworkServer.InitializationClient.config.TimeoutDataRetrieval / 1000; // ms to seconds
            int resultInitTimeout = RadioTelescope.SensorNetworkServer.InitializationClient.config.TimeoutInitialization / 1000; // ms to seconds

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
            DatabaseOperations.DeleteSensorNetworkConfig(RadioTelescope.SensorNetworkServer.InitializationClient.config);
        }
    }
}
