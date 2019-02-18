using System;
using System.Threading;
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
            //Thread t1 = new Thread(() => simulator = new PLCSimulator()); 
            //Thread t2 = new Thread(() => connector = new PLCConnector(simulator));

            //t1.Start();
            //t1.Join();
            //t2.Start();
            //t2.Join();
            simulator = new PLCSimulator();
            connector = new PLCConnector(simulator);

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
            var message = "This is a test message.";
            connector.WriteMessage(message);

            byte[] testMessage = System.Text.Encoding.ASCII.GetBytes(message);

            Assert.IsTrue(connector.Stream.DataAvailable);
        }
    }
}
