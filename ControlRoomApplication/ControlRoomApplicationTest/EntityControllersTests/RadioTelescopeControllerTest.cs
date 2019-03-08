using System;
using System.Collections.Generic;
using ControlRoomApplication.Controllers.RadioTelescopeControllers;
using ControlRoomApplication.Entities.RadioTelescope;
using ControlRoomApplication.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ControlRoomApplication.Controllers.PLCCommunication;
using ControlRoomApplication.Controllers.SpectraCyberController;
using ControlRoomApplication.Constants;
using ControlRoomApplication.Entities.Plc;
using ControlRoomApplication.Main;

namespace ControlRoomApplicationTest.EntityControllersTests
{
    [TestClass]
    public class RadioTelescopeControllerTest
    {
        [TestInitialize]
        public void SetUp()
        {
            // Instantiate the controller but do not set the RadioTelescope field
            // since this will need to test each type of RadioTelescope to ensure
            // functionality for each is correct
            RadioTelescopeController = new RadioTelescopeController();
        }

        [TestMethod]
        public void TestGetCurrentOrientation()
        {
            // Set the controller's RadioTelescope field. This particular method
            // is RadioTelescope-agnostic, so it does not matter which we use
            RadioTelescopeController.RadioTelescope = new ScaleRadioTelescope();

            // Create an Orientation object with an azimuth of 311 and elevation of 42
            Orientation Orientation = new Orientation(311.0, 42.0);
            Orientation.Id = 1;

            // Set the RadioTelescope's CurrentOrientation field
            RadioTelescopeController.RadioTelescope.CurrentOrientation = Orientation;

            // Call the GetCurrentOrientationMethod
            Orientation CurrentOrientation = RadioTelescopeController.GetCurrentOrientation();

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
            PLCCommunicationHandler PLCController = new PLCCommunicationHandler(PLCConstants.LOCAL_HOST_IP, PLCConstants.PORT_8080);
            RadioTelescopeController.RadioTelescope = new ScaleRadioTelescope(new SpectraCyberTestController(new SpectraCyberSimulator(), new RTDbContext()), PLCController);
            
            // Create the Orientation object that the Controller will tell the
            // Scale RadioTelescope to move to
            Orientation MoveTo = new Orientation(311.0, 42.0);

            // Call the MoveRadioTelescope method
            RadioTelescopeController.MoveRadioTelescope(MoveTo);

            // The Radio Telescope should now have a CurrentOrientation field that 
            // is equivalent to the MoveTo Orientation
            Assert.AreEqual(RadioTelescopeController.RadioTelescope.CurrentOrientation.Id, MoveTo.Id);
            Assert.AreEqual(RadioTelescopeController.RadioTelescope.CurrentOrientation.Azimuth, MoveTo.Azimuth);
            Assert.AreEqual(RadioTelescopeController.RadioTelescope.CurrentOrientation.Elevation, MoveTo.Elevation);

            // It should now be in an idle state
            Assert.AreEqual(RadioTelescopeController.RadioTelescope.Status, RadioTelescopeStatusEnum.IDLE);
        }

        [TestMethod]
        public void TestCalibrateScaleRadioTelescope()
        {
            // Set the controller's RadioTelescope field to the ScaleRadioTelescope
            // Make sure to specify that we are just using the TestPLC since we do
            // not care if what the PLC does, just how this method responds
            PLCCommunicationHandler PLCController = new PLCCommunicationHandler(PLCConstants.LOCAL_HOST_IP, PLCConstants.PORT_8080);
            RadioTelescopeController.RadioTelescope = new ScaleRadioTelescope(new SpectraCyberTestController(new SpectraCyberSimulator(), new RTDbContext()), PLCController);

            // Call the CalibrateRadioTelescope method
            RadioTelescopeController.CalibrateRadioTelescope();

            // The Radio Telescope should now have a CurrentOrienation of (0,0)
            Assert.AreEqual(RadioTelescopeController.RadioTelescope.CurrentOrientation.Azimuth, 0.0);
            Assert.AreEqual(RadioTelescopeController.RadioTelescope.CurrentOrientation.Elevation, 0.0);

            // It should also now be in an idle state
            Assert.AreEqual(RadioTelescopeController.RadioTelescope.Status, RadioTelescopeStatusEnum.IDLE);

        }

        [TestMethod]
        public void TestShutdownScaleRadioTelescope()
        {
            // Set the controller's RadioTelescope field to the ScaleRadioTelescope
            // Make sure to specify that we are just using the TestPLC since we do
            // not care if what the PLC does, just how this method responds
            PLCCommunicationHandler PLCController = new PLCCommunicationHandler(PLCConstants.LOCAL_HOST_IP, PLCConstants.PORT_8080);
            RadioTelescopeController.RadioTelescope = new ScaleRadioTelescope(new SpectraCyberTestController(new SpectraCyberSimulator(), new RTDbContext()), PLCController);

            // Call the ShutdownRadioTelescope method
            RadioTelescopeController.ShutdownRadioTelescope();

            // The Radio Telescope should now have a CurrentOrientation of (0, -90)
            Assert.AreEqual(RadioTelescopeController.RadioTelescope.CurrentOrientation.Azimuth, 0.0);
            Assert.AreEqual(RadioTelescopeController.RadioTelescope.CurrentOrientation.Elevation, -90.0);

            // It should also now be in a shutdown state
            Assert.AreEqual(RadioTelescopeController.RadioTelescope.Status, RadioTelescopeStatusEnum.SHUTDOWN);
        }

        public RadioTelescopeController RadioTelescopeController { get; set; }
    }
}
