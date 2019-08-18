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
        private static TestPLCDriver TestRTPLC;

        [ClassInitialize]
        public static void SetUp(TestContext context)
        {
            //PLCClientCommunicationHandler PLCClientCommHandler = new PLCClientCommunicationHandler(ip, port);
            TestRTPLC = new TestPLCDriver(ip, ip, port, port);

            SpectraCyberSimulatorController SCSimController = new SpectraCyberSimulatorController(new SpectraCyberSimulator());
            Location location = MiscellaneousConstants.JOHN_RUDY_PARK;
            RadioTelescope TestRT = new RadioTelescope(SCSimController, TestRTPLC, location, new Orientation(0, 0));
            TestRadioTelescopeController = new RadioTelescopeController(TestRT);


            TestRTPLC.StartAsyncAcceptingClients();
            //TestRT.PLCClient.ConnectToServer();
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
            Assert.AreEqual(Orientation.Azimuth, CurrentOrientation.Azimuth,0.001);
            Assert.AreEqual(Orientation.Elevation, CurrentOrientation.Elevation, 0.001);

            Orientation = new Orientation(28.0, 42.0);

            // Set the RadioTelescope's CurrentOrientation field
            response = TestRadioTelescopeController.MoveRadioTelescopeToOrientation(Orientation);

            // Call the GetCurrentOrientationMethod
            CurrentOrientation = TestRadioTelescopeController.GetCurrentOrientation();

          //  Orientation = new Orientation(310.0, 42.0);

            // Set the RadioTelescope's CurrentOrientation field
        //    response = TestRadioTelescopeController.MoveRadioTelescopeToOrientation(Orientation);

            // Call the GetCurrentOrientationMethod
          //  CurrentOrientation = TestRadioTelescopeController.GetCurrentOrientation();

            // Ensure the objects are identical
            Assert.IsTrue(response);
            Assert.AreEqual(Orientation.Azimuth, CurrentOrientation.Azimuth, 0.001);
            Assert.AreEqual(Orientation.Elevation, CurrentOrientation.Elevation, 0.001);

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
            Assert.AreEqual(Orientation.Azimuth, CurrentOrientation.Azimuth,0.001);
            Assert.AreEqual(Orientation.Elevation, CurrentOrientation.Elevation,0.001);
        }

        [ClassCleanup]
        public static void BringDown()
        {
            //TestRadioTelescopeController.RadioTelescope.PLCClient.TerminateTCPServerConnection();
            //TestRTPLC.RequestStopAsyncAcceptingClientsAndJoin();
            TestRadioTelescopeController.RadioTelescope.PLCDriver.Bring_down();
        }
    }
}
