using System;
using ControlRoomApplication.Controllers.PLCController;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ControlRoomApplicationTest.UtilityTests
{
    [TestClass]
    public class PLCConnectorTest
    {
        private PLCConnector connector;
        private PLCSimulator simulator;

        [TestInitialize]
        public void BuildUp()
        {
            connector = new PLCConnector();
            simulator = new PLCSimulator(connector);
        }

        [TestCleanup]
        public void TearDown()
        {
            connector.DisconnectFromPLC();
            simulator.StopServer();
        }

        [TestMethod]
        public void TestWriteMessage()
        {
            var message = "Test message.";
            connector.WriteMessage(message);

            var testMessage = simulator.ReceiveMessage();

            Assert.AreEqual(message, testMessage);
        }
    }
}
