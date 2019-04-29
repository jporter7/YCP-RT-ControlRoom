using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ControlRoomApplication.Constants;
using ControlRoomApplication.Controllers;
using ControlRoomApplication.Entities;

namespace ControlRoomApplicationTest.EntityControllersTests
{
    [TestClass]
    public class RadioTelescopeControllerTest
    {
        private static string ip = PLCConstants.LOCAL_HOST_IP;
        private static int port = PLCConstants.PORT_8080;

        private static RadioTelescopeController TestRadioTelescopeController;
        private static TestPLCTCPIPReceiver TestRTPLC;

        [ClassInitialize]
        public static void SetUp(TestContext context)
        {
            AbstractHardwareCommunicationHandler PLCClientCommHandler = new TCPIPCommunicationHandler(ip, port);
            SpectraCyberSimulatorController SCSimController = new SpectraCyberSimulatorController(new SpectraCyberSimulator());
            Location location = MiscellaneousConstants.JOHN_RUDY_PARK;
            RadioTelescope TestRT = new RadioTelescope(SCSimController, PLCClientCommHandler, location, new Orientation(0, 0));
            TestRadioTelescopeController = new RadioTelescopeController(TestRT);

            TestRTPLC = new TestPLCTCPIPReceiver(ip, port);
            TestRTPLC.StartAsyncAcceptingClients();
            TestRT.HardwareCommsHandler.StartCommunicationThread();
        }

        [TestMethod]
        public void TestTestCommunication()
        {
            var test_result = TestRadioTelescopeController.TestCommunication();
            Assert.IsTrue(test_result);
        }

        [TestMethod]
        public void TestGetCurrentOrientation()
        {
            // Create an Orientation object with an azimuth of 311 and elevation of 42
            Orientation Orientation = new Orientation(311.0, 42.0);

            // Set the RadioTelescope's CurrentOrientation field
            var response = TestRadioTelescopeController.MoveRadioTelescopeToOrientation(Orientation);

            // Call the GetCurrentOrientationMethod
            Orientation CurrentOrientation = TestRadioTelescopeController.GetCurrentOrientation();

            // Ensure the objects are identical
            Assert.IsTrue(response);
            Assert.AreEqual(Orientation.Azimuth, CurrentOrientation.Azimuth);
            Assert.AreEqual(Orientation.Elevation, CurrentOrientation.Elevation);
        }

        [TestMethod]
        public void TestGetCurrentLimitSwitchStatuses()
        {
            // Test the limit switch statuses
            var responses = TestRadioTelescopeController.GetCurrentLimitSwitchStatuses();
            
            // Make sure each limit switch is online
            foreach(var response in responses)
            {
                Assert.IsFalse(response);
            }
        }

        [TestMethod]
        public void TestGetCurrentSafetyInterlockStatus()
        {
            // Test the safety interlock status
            var response = TestRadioTelescopeController.GetCurrentSafetyInterlockStatus();

            // Make sure safety interlock is online
            Assert.IsTrue(response);
        }

        [TestMethod]
        public void TestCancelCurrentMoveCommand()
        {
            // Test canceling the the current move command
            TestRadioTelescopeController.MoveRadioTelescopeToOrientation(new Orientation(0, 0));
            var response = TestRadioTelescopeController.CancelCurrentMoveCommand();

            // Make sure it was successful
            Assert.IsTrue(response);
        }

        [TestMethod]
        public void TestShutdownRadioTelescope()
        {
            // Call the ShutdownRadioTelescope method
            var response = TestRadioTelescopeController.ShutdownRadioTelescope();
            Orientation orientation = TestRadioTelescopeController.GetCurrentOrientation();

            // The Radio Telescope should now have a CurrentOrientation of (0, -90)
            Assert.IsTrue(response);
            Assert.AreEqual(orientation.Azimuth, 0.0);
            Assert.AreEqual(orientation.Elevation, 0.0);
        }

        [TestMethod]
        public void TestCalibrateRadioTelescope()
        {
            // Call the CalibrateRadioTelescope method
            var response = TestRadioTelescopeController.CalibrateRadioTelescope();

            // The Radio Telescope should now have a CurrentOrienation of (0,0)
            Assert.IsTrue(response);
            Assert.AreEqual(TestRadioTelescopeController.RadioTelescope.CurrentOrientation.Azimuth, 0.0);
            Assert.AreEqual(TestRadioTelescopeController.RadioTelescope.CurrentOrientation.Elevation, 0.0);
        }

        [TestMethod]
        public void TestMoveRadioTelescope()
        {
            // Create an Orientation object with an azimuth of 311 and elevation of 42
            Orientation Orientation = new Orientation(311.0, 42.0);

            // Set the RadioTelescope's CurrentOrientation field
            var response = TestRadioTelescopeController.MoveRadioTelescopeToOrientation(Orientation);

            // Call the GetCurrentOrientationMethod
            Orientation CurrentOrientation = TestRadioTelescopeController.GetCurrentOrientation();

            // Ensure the objects are identical
            Assert.IsTrue(response);
            Assert.AreEqual(Orientation.Azimuth, CurrentOrientation.Azimuth);
            Assert.AreEqual(Orientation.Elevation, CurrentOrientation.Elevation);
        }

        [ClassCleanup]
        public static void BringDown()
        {
            TestRadioTelescopeController.RadioTelescope.HardwareCommsHandler.TerminateAndJoinCommunicationThread();
            TestRTPLC.StopAsyncAcceptingClientsAndJoin();
        }
    }
}
