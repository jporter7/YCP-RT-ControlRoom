using ControlRoomApplication.Controllers.SensorNetwork;
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

namespace ControlRoomApplicationTest.EntityControllersTests
{
    [TestClass]
    public class SensorNetworkServerTest
    {

        // These are fields that will remain the same for all tests
        IPAddress ServerIpAddress = IPAddress.Parse("127.0.0.1");
        string ClientIpAddress = "127.0.0.2";
        int ClientPort = 3000;
        int ServerPort = 3001;
        int TelescopeId = 5;
        bool IsSimulation = true;
        SensorNetworkServer Server;

        [TestInitialize]
        public void Initialize()
        {
            Server = new SensorNetworkServer(ServerIpAddress, ServerPort, ClientIpAddress, ClientPort, TelescopeId, IsSimulation);
        }

        [TestCleanup]
        public void Cleanup()
        {
            DatabaseOperations.DeleteSensorNetworkConfig(Server.InitializationClient.config);
        }

        [TestMethod]
        public void TestSensorNetworkServer_Initialization_Success()
        {
            PrivateObject privServer = new PrivateObject(Server);
            PrivateObject privClient = new PrivateObject(Server.InitializationClient);

            // Arrange/Act
            TcpListener resultListener = (TcpListener)privServer.GetFieldOrProperty("Server");
            Thread resultMonitorThread = (Thread)privServer.GetFieldOrProperty("SensorMonitoringThread");
            System.Timers.Timer resultTimer = (System.Timers.Timer)privServer.GetFieldOrProperty("Timeout");

            // Verify SensorNetworkServer is correct
            Assert.IsNotNull(resultListener);
            Assert.AreEqual(((IPEndPoint)resultListener.LocalEndpoint).Address, ServerIpAddress);
            Assert.AreEqual(((IPEndPoint)resultListener.LocalEndpoint).Port,  ServerPort);

            // Verify SensorNetworkClient is correct
            Assert.IsNotNull(Server.InitializationClient);
            Assert.AreEqual((string)privClient.GetFieldOrProperty("IPAddress"), ClientIpAddress);
            Assert.AreEqual((int)privClient.GetFieldOrProperty("Port"), ClientPort);
            Assert.AreEqual(Server.InitializationClient.config.TelescopeId, TelescopeId);

            // Verify temperatures are correct
            Assert.IsNotNull(Server.CurrentElevationMotorTemp);
            Assert.IsNotNull(Server.CurrentAzimuthMotorTemp);

            // Verify orientation is correct
            Assert.IsNotNull(Server.CurrentAbsoluteOrientation);

            // Verify acceleration is correct
            Assert.IsNotNull(Server.CurrentAzimuthMotorAccl);
            Assert.IsNotNull(Server.CurrentElevationMotorAccl);
            Assert.IsNotNull(Server.CurrentCounterbalanceAccl);

            // Verify thread is correct
            Assert.IsNotNull(resultMonitorThread);
            Assert.AreEqual(resultMonitorThread.Name, "SensorMonitorThread");

            // Verify timer is correct
            Assert.IsNotNull(resultTimer);
            Assert.AreEqual(resultTimer.Interval, Server.InitializationClient.config.TimeoutInitialization);
            Assert.IsFalse(resultTimer.AutoReset);
        }
    }
}
