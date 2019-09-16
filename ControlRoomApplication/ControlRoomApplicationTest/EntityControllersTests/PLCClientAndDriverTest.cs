using System;
using ControlRoomApplication.Controllers;
using ControlRoomApplication.Constants;
using ControlRoomApplication.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ControlRoomApplicationTest.EntityControllersTests
{
    /*
    [TestClass]
    public class PLCClientAndDriverTest
    {
        public static PLCClientCommunicationHandler CommsHandler;
        public static TestPLCDriver TestDriver;

        [ClassInitialize]
        public static void BuildUp(TestContext testContext)
        {
            string ip = PLCConstants.LOCAL_HOST_IP;
            int port = 8091;

            CommsHandler = new PLCClientCommunicationHandler(ip, port);
            TestDriver = new TestPLCDriver(ip,ip, port,port);

            TestDriver.StartAsyncAcceptingClients();

            CommsHandler.ConnectToServer();
        }

        [TestMethod]
        public void TestConnectionTest()
        {
            byte[] expectedResponse = new byte[] { 0x13, 0x0, 0x1, 0x1, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0 };
            byte[] actualResponse = CommsHandler.RequestMessageSend(PLCCommandAndQueryTypeEnum.TEST_CONNECTION);

            Console.WriteLine("Test Connection (actual): [{0}]", string.Join(", ", actualResponse));
            Console.WriteLine("Test Connection (expected): [{0}]", string.Join(", ", expectedResponse));

            CollectionAssert.AreEqual(expectedResponse, actualResponse);
        }

        [TestMethod]
        public void TestGetCurrentLimitSwitchStatuses()
        {
            byte[] expectedResponse = new byte[] { 0x13, 0x0, 0x1, 0x55, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0 };
            byte[] actualResponse = CommsHandler.RequestMessageSend(PLCCommandAndQueryTypeEnum.GET_CURRENT_LIMIT_SWITCH_STATUSES);

            Console.WriteLine("Test Limit Switch (actual): [{0}]", string.Join(", ", actualResponse));
            Console.WriteLine("Test Limit Switch (expected): [{0}]", string.Join(", ", expectedResponse));

            CollectionAssert.AreEqual(expectedResponse, actualResponse);
        }

        [TestMethod]
        public void TestGetCurrentSafetyInterlockStatus()
        {
            byte[] expectedResponse = new byte[] { 0x13, 0x0, 0x1, 0x1, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0 };
            byte[] actualResponse = CommsHandler.RequestMessageSend(PLCCommandAndQueryTypeEnum.GET_CURRENT_SAFETY_INTERLOCK_STATUS);

            Console.WriteLine("Test Interlock (actual): [{0}]", string.Join(", ", actualResponse));
            Console.WriteLine("Test Interlock (expected): [{0}]", string.Join(", ", expectedResponse));

            CollectionAssert.AreEqual(expectedResponse, actualResponse);
        }

        [TestMethod]
        public void TestCancelActiveObjectiveAZELPosition()
        {
            byte[] expectedResponse = new byte[] { 0x3, 0x0, 0x1 };
            byte[] actualResponse = CommsHandler.RequestMessageSend(PLCCommandAndQueryTypeEnum.CANCEL_ACTIVE_OBJECTIVE_AZEL_POSITION);

            Console.WriteLine("Test Cancelling Active Objective AZ EL Position (actual): [{0}]", string.Join(", ", actualResponse));
            Console.WriteLine("Test Cancelling Active Objective AZ EL Position (expected): [{0}]", string.Join(", ", expectedResponse));

            CollectionAssert.AreEqual(expectedResponse, actualResponse);
        }

        [TestMethod]
        public void TestShutdown()
        {
            byte[] expectedResponse = new byte[] { 0x3, 0x0, 0x1 };
            byte[] actualResponse = CommsHandler.RequestMessageSend(PLCCommandAndQueryTypeEnum.SHUTDOWN);

            Console.WriteLine("Test Shutdown (actual): [{0}]", string.Join(", ", actualResponse));
            Console.WriteLine("Test Shutdown (expected): [{0}]", string.Join(", ", expectedResponse));

            CollectionAssert.AreEqual(expectedResponse, actualResponse);
        }

        [TestMethod]
        public void TestCalibrate()
        {
            byte[] expectedResponse = new byte[] { 0x3, 0x0, 0x1 };
            byte[] actualResponse = CommsHandler.RequestMessageSend(PLCCommandAndQueryTypeEnum.CALIBRATE);

            Console.WriteLine("Test Calibration (actual): [{0}]", string.Join(", ", actualResponse));
            Console.WriteLine("Test Calibration (expected): [{0}]", string.Join(", ", expectedResponse));

            CollectionAssert.AreEqual(expectedResponse, actualResponse);
        }

        [TestMethod]
        public void TestSetAndGetObjectiveAZELPosition()
        {
            double NewAzimuth = 23.4;
            double NewElevation = 55.0;

            byte[] expectedResponse = new byte[] { 0x3, 0x0, 0x1 };
            byte[] actualResponse = CommsHandler.RequestMessageSend(PLCCommandAndQueryTypeEnum.SET_OBJECTIVE_AZEL_POSITION, new Orientation(NewAzimuth, NewElevation));

            Console.WriteLine("Test Set Active Objective AZ/EL (actual): [{0}]", string.Join(", ", actualResponse));
            Console.WriteLine("Test Set Active Objective AZ/EL (expected): [{0}]", string.Join(", ", expectedResponse));

            CollectionAssert.AreEqual(expectedResponse, actualResponse);

            Orientation NewOrientation = TestDriver.CurrentOrientation;
            Assert.AreEqual(NewAzimuth, NewOrientation.Azimuth);
            Assert.AreEqual(NewElevation, NewOrientation.Elevation);

            expectedResponse = new byte[] { 0x13, 0x0, 0x1, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0 };
            Array.Copy(BitConverter.GetBytes(NewAzimuth), 0, expectedResponse, 3, 8);
            Array.Copy(BitConverter.GetBytes(NewElevation), 0, expectedResponse, 11, 8);

            actualResponse = CommsHandler.RequestMessageSend(PLCCommandAndQueryTypeEnum.GET_CURRENT_AZEL_POSITIONS);

            Console.WriteLine("Get Current AZ/EL (expected): [{0}]", string.Join(", ", expectedResponse));
            Console.WriteLine("Get Current AZ/EL (actual): [{0}]", string.Join(", ", actualResponse));

            CollectionAssert.AreEqual(expectedResponse, actualResponse);
        }

        [ClassCleanup]
        public static void Cleanup()
        {
            CommsHandler.TerminateTCPServerConnection();
            TestDriver.RequestStopAsyncAcceptingClientsAndJoin();
        }
    }
    */
}
