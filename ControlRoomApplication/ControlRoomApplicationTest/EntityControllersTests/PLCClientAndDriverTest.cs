using System;
using ControlRoomApplication.Entities;
using ControlRoomApplication.Controllers.PLCCommunication;
using ControlRoomApplication.Constants;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ControlRoomApplicationTest.EntityControllersTests
{
    [TestClass]
    public class PLCClientAndDriverTest
    {
        public static PLCClientCommunicationHandler CommsHandler;
        public static TestPLCDriver TestDriver;

        [ClassInitialize]
        public static void BuildUp(TestContext testContext)
        {
            string ip = PLCConstants.LOCAL_HOST_IP;
            int port = PLCConstants.PORT_8080;

            CommsHandler = new PLCClientCommunicationHandler(ip, port);
            TestDriver = new TestPLCDriver(ip, port);

            TestDriver.StartAsyncAcceptingClients();

            CommsHandler.ConnectToServer();
        }

        [TestMethod]
        public void TestConnectionTest()
        {
            byte[] expectedResponse = new byte[] { 0x13, 0x0, 0x1, 0x1, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0 };
            byte[] actualResponse = (byte[])CommsHandler.RequestMessageSend(PLCCommandAndQueryTypeEnum.TEST_CONNECTION);

            Console.WriteLine("Test Connection (actual): [{0}]", string.Join(", ", actualResponse));
            Console.WriteLine("Test Connection (expected): [{0}]", string.Join(", ", expectedResponse));

            CollectionAssert.AreEqual(expectedResponse, actualResponse);
        }

        [TestMethod]
        public void TestGetCurrentAZELPosition()
        {
            byte[] expectedResponse = new byte[] { 0x13, 0x0, 0x1, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0 };
            double expectedAZ = 180.0, expectedEL = 42.0;
            Array.Copy(BitConverter.GetBytes(expectedAZ), 0, expectedResponse, 3, 8);
            Array.Copy(BitConverter.GetBytes(expectedEL), 0, expectedResponse, 11, 8);

            byte[] actualResponse = (byte[])CommsHandler.RequestMessageSend(PLCCommandAndQueryTypeEnum.GET_CURRENT_AZEL_POSITIONS);

            Console.WriteLine("Get Current AZ/EL (expected): [{0}]", string.Join(", ", expectedResponse));
            Console.WriteLine("Get Current AZ/EL (actual): [{0}]", string.Join(", ", actualResponse));

            CollectionAssert.AreEqual(expectedResponse, actualResponse);
        }

        [TestMethod]
        public void TestGetCurrentLimitSwitchStatuses()
        {
            byte[] expectedResponse = new byte[] { 0x13, 0x0, 0x1, 0x55, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0 };
            byte[] actualResponse = (byte[])CommsHandler.RequestMessageSend(PLCCommandAndQueryTypeEnum.GET_CURRENT_LIMIT_SWITCH_STATUSES);

            Console.WriteLine("Test Limit Switch (actual): [{0}]", string.Join(", ", actualResponse));
            Console.WriteLine("Test Limit Switch (expected): [{0}]", string.Join(", ", expectedResponse));

            CollectionAssert.AreEqual(expectedResponse, actualResponse);
        }

        [TestMethod]
        public void TestGetCurrentSafetyInterlockStatus()
        {
            byte[] expectedResponse = new byte[] { 0x13, 0x0, 0x1, 0x1, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0 };
            byte[] actualResponse = (byte[])CommsHandler.RequestMessageSend(PLCCommandAndQueryTypeEnum.GET_CURRENT_SAFETY_INTERLOCK_STATUS);

            Console.WriteLine("Test Interlock (actual): [{0}]", string.Join(", ", actualResponse));
            Console.WriteLine("Test Interlock (expected): [{0}]", string.Join(", ", expectedResponse));

            CollectionAssert.AreEqual(expectedResponse, actualResponse);
        }

        [TestMethod]
        public void TestCancelActiveObjectiveAZELPosition()
        {
            bool actualResponse = (bool)CommsHandler.RequestMessageSend(PLCCommandAndQueryTypeEnum.CANCEL_ACTIVE_OBJECTIVE_AZEL_POSITION);
            Assert.AreEqual(true, actualResponse);
        }

        [TestMethod]
        public void TestShutdown()
        {
            bool actualResponse = (bool)CommsHandler.RequestMessageSend(PLCCommandAndQueryTypeEnum.SHUTDOWN);
            Assert.AreEqual(true, actualResponse);
        }

        [TestMethod]
        public void TestCalibrate()
        {
            bool actualResponse = (bool)CommsHandler.RequestMessageSend(PLCCommandAndQueryTypeEnum.CALIBRATE);
            Assert.AreEqual(true, actualResponse);
        }

        [TestMethod]
        public void TestSetObjectiveAZELPosition()
        {
            bool actualResponse = (bool)CommsHandler.RequestMessageSend(PLCCommandAndQueryTypeEnum.SET_OBJECTIVE_AZEL_POSITION);
            Assert.AreEqual(true, actualResponse);
        }

        [ClassCleanup]
        public static void Cleanup()
        {
            CommsHandler.TerminateTCPServerConnection();
            TestDriver.RequestStopAsyncAcceptingClients();
        }
    }
}
