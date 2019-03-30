using ControlRoomApplication.Controllers.RadioTelescopeControllers;
using ControlRoomApplication.Entities;
using ControlRoomApplication.Controllers.PLCCommunication;
using ControlRoomApplication.Controllers.SpectraCyberController;
using ControlRoomApplication.Constants;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace ControlRoomApplicationTest.EntityControllersTests
{
    [TestClass]
    public class RadioTelescopeControllerTest
    {
        private static string ip = "127.0.0.1";
        private static int port = PLCConstants.PORT_8080;

        private static RadioTelescopeController TestRadioTelescopeController;
        private static TestPLCDriver TestRTPLC;

        [ClassInitialize]
        public static void SetUp(TestContext context)
        {
            PLCClientCommunicationHandler PLCClientCommHandler = new PLCClientCommunicationHandler(ip, port);
            SpectraCyberSimulatorController SCSimController = new SpectraCyberSimulatorController(new SpectraCyberSimulator());
            Location location = new Location(76.7046, 40.0244, 395.0); // John Rudy Park
            RadioTelescope TestRT = new RadioTelescope(SCSimController, PLCClientCommHandler, location, new Orientation(0, 0));
            TestRadioTelescopeController = new RadioTelescopeController(TestRT);

            TestRTPLC = new TestPLCDriver(ip, port);
            TestRTPLC.StartAsyncAcceptingClients();
            TestRT.PLCClient.ConnectToServer();
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
            var response = TestRadioTelescopeController.MoveRadioTelescope(Orientation);

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
            TestRadioTelescopeController.MoveRadioTelescope(new Orientation(0, 0));
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
            var response = TestRadioTelescopeController.MoveRadioTelescope(Orientation);

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
            TestRadioTelescopeController.RadioTelescope.PLCClient.TerminateTCPServerConnection();
            TestRTPLC.RequestStopAsyncAcceptingClientsAndJoin();
        }
    }
}
