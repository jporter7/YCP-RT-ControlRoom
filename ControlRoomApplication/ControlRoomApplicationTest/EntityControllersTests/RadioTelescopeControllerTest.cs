using ControlRoomApplication.Controllers.RadioTelescopeControllers;
using ControlRoomApplication.Entities.RadioTelescope;
using ControlRoomApplication.Entities;
using ControlRoomApplication.Controllers.PLCCommunication;
using ControlRoomApplication.Controllers.SpectraCyberController;
using ControlRoomApplication.Constants;
using Microsoft.VisualStudio.TestTools.UnitTesting;

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
            PLCClientCommunicationHandler PLCClientCommHandler = new PLCClientCommunicationHandler(ip, port);
            SpectraCyberSimulatorController SCSimController = new SpectraCyberSimulatorController(new SpectraCyberSimulator());
            RadioTelescope TestRT = new RadioTelescope(SCSimController, PLCClientCommHandler, new Location(76.7046, 40.0244, 395.0)); // John Rudy Park
            TestRadioTelescopeController = new RadioTelescopeController(TestRT);

            TestRTPLC = new TestPLCDriver(ip, port);

            TestRTPLC.StartAsyncAcceptingClients();
            TestRT.PLCClient.ConnectToServer();
        }

        [TestMethod]
        public void TestGetCurrentOrientation()
        {
            // Create an Orientation object with an azimuth of 311 and elevation of 42
            Orientation Orientation = new Orientation(311.0, 42.0);
            Orientation.Id = 1;

            // Set the RadioTelescope's CurrentOrientation field
            TestRadioTelescopeController.RadioTelescope.CurrentOrientation = Orientation;

            // Call the GetCurrentOrientationMethod
            Orientation CurrentOrientation = TestRadioTelescopeController.GetCurrentOrientation();

            // Ensure the objects are identical
            Assert.AreEqual(Orientation.Id, CurrentOrientation.Id);
            Assert.AreEqual(Orientation.Azimuth, CurrentOrientation.Azimuth);
            Assert.AreEqual(Orientation.Elevation, CurrentOrientation.Elevation);
        }

        [TestMethod]
        public void TestMoveScaleRadioTelescope()
        {
            // Set the controller's RadioTelescope field to the ScaleRadioTelescope
            // Make sure to specify that we are just using the TestPLC since we do
            // not care if what the PLC does, just how this method responds
            PLCClientCommunicationHandler PLCController = new PLCClientCommunicationHandler(PLCConstants.LOCAL_HOST_IP, PLCConstants.PORT_8080);
            Location location = new Location(76.7046, 40.0244, 395.0); // John Rudy Park
            TestRadioTelescopeController.RadioTelescope = new RadioTelescope(new SpectraCyberTestController(new SpectraCyberSimulator()), PLCController, location);

            // Create the Orientation object that the Controller will tell the
            // Scale RadioTelescope to move to
            Orientation MoveTo = new Orientation(311.0, 42.0);

            // Call the MoveRadioTelescope method
            TestRadioTelescopeController.MoveRadioTelescope(MoveTo);

            // The Radio Telescope should now have a CurrentOrientation field that 
            // is equivalent to the MoveTo Orientation
            Assert.AreEqual(TestRadioTelescopeController.RadioTelescope.CurrentOrientation.Id, MoveTo.Id);
            Assert.AreEqual(TestRadioTelescopeController.RadioTelescope.CurrentOrientation.Azimuth, MoveTo.Azimuth);
            Assert.AreEqual(TestRadioTelescopeController.RadioTelescope.CurrentOrientation.Elevation, MoveTo.Elevation);

            // It should now be in an idle state
            Assert.AreEqual(TestRadioTelescopeController.RadioTelescope.Status, RadioTelescopeStatusEnum.IDLE);
        }

        [TestMethod]
        public void TestCalibrateScaleRadioTelescope()
        {
            // Set the controller's RadioTelescope field to the ScaleRadioTelescope
            // Make sure to specify that we are just using the TestPLC since we do
            // not care if what the PLC does, just how this method responds
            PLCClientCommunicationHandler PLCController = new PLCClientCommunicationHandler(PLCConstants.LOCAL_HOST_IP, PLCConstants.PORT_8080);
            Location location = new Location(76.7046, 40.0244, 395.0); // John Rudy Park
            TestRadioTelescopeController.RadioTelescope = new RadioTelescope(new SpectraCyberTestController(new SpectraCyberSimulator()), PLCController, location);

            // Call the CalibrateRadioTelescope method
            TestRadioTelescopeController.CalibrateRadioTelescope();

            // The Radio Telescope should now have a CurrentOrienation of (0,0)
            Assert.AreEqual(TestRadioTelescopeController.RadioTelescope.CurrentOrientation.Azimuth, 0.0);
            Assert.AreEqual(TestRadioTelescopeController.RadioTelescope.CurrentOrientation.Elevation, 0.0);

            // It should also now be in an idle state
            Assert.AreEqual(TestRadioTelescopeController.RadioTelescope.Status, RadioTelescopeStatusEnum.IDLE);

        }

        [TestMethod]
        public void TestShutdownScaleRadioTelescope()
        {
            // Set the controller's RadioTelescope field to the ScaleRadioTelescope
            // Make sure to specify that we are just using the TestPLC since we do
            // not care if what the PLC does, just how this method responds
            PLCClientCommunicationHandler PLCController = new PLCClientCommunicationHandler(PLCConstants.LOCAL_HOST_IP, PLCConstants.PORT_8080);
            Location location = new Location(76.7046, 40.0244, 395.0); // John Rudy Park
            TestRadioTelescopeController.RadioTelescope = new RadioTelescope(new SpectraCyberTestController(new SpectraCyberSimulator()), PLCController, location);

            // Call the ShutdownRadioTelescope method
            TestRadioTelescopeController.ShutdownRadioTelescope();

            // The Radio Telescope should now have a CurrentOrientation of (0, -90)
            Assert.AreEqual(TestRadioTelescopeController.RadioTelescope.CurrentOrientation.Azimuth, 0.0);
            Assert.AreEqual(TestRadioTelescopeController.RadioTelescope.CurrentOrientation.Elevation, -90.0);

            // It should also now be in a shutdown state
            Assert.AreEqual(TestRadioTelescopeController.RadioTelescope.Status, RadioTelescopeStatusEnum.SHUTDOWN);
        }

        [ClassCleanup]
        public static void BringDown()
        {
            TestRadioTelescopeController.RadioTelescope.PLCClient.TerminateTCPServerConnection();
            TestRTPLC.RequestStopAsyncAcceptingClients();
        }
    }
}
