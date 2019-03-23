using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ControlRoomApplication.Entities;
using ControlRoomApplication.Entities.RadioTelescope;
using ControlRoomApplication.Controllers;
using ControlRoomApplication.Controllers.SpectraCyberController;
using ControlRoomApplication.Controllers.RadioTelescopeControllers;
using ControlRoomApplication.Controllers.PLCCommunication;

namespace ControlRoomApplicationTest.EntityControllersTests
{
    [TestClass]
    public class ControlRoomControllerTest
    {
        private static ControlRoomController CRController;

        private static RadioTelescopeController RTController0;
        private static RadioTelescopeController RTController1;
        private static RadioTelescopeController RTController2;

        private static Location JohnRudyPark = new Location(76.7046, 40.0244, 395.0); // John Rudy Park hardcoded for now
        private static ControlRoom CRoom = new ControlRoom();

        [ClassInitialize]
        public static void BringUp(TestContext context)
        {
            CRController = new ControlRoomController(CRoom);

            string ip = "127.0.0.1";
            int port = 8080;

            RTController0 = new RadioTelescopeController(
                new RadioTelescope(
                    new SpectraCyberSimulatorController(new SpectraCyberSimulator()),
                    new PLCClientCommunicationHandler(ip, port),
                    JohnRudyPark
                )
            );

            RTController1 = new RadioTelescopeController(
                new RadioTelescope(
                    new SpectraCyberSimulatorController(new SpectraCyberSimulator()),
                    new PLCClientCommunicationHandler(ip, port),
                    JohnRudyPark
                )
            );

            RTController2 = new RadioTelescopeController(
                new RadioTelescope(
                    new SpectraCyberSimulatorController(new SpectraCyberSimulator()),
                    new PLCClientCommunicationHandler(ip, port),
                    JohnRudyPark
                )
            );
        }

        [TestMethod]
        public void TestConstructorAndProperties()
        {
            Assert.AreEqual(CRController.CRoom, CRoom);
        }

        [TestMethod]
        public void TestAddRTControllerAndStartAndHardRemoveInstance()
        {
            Assert.AreEqual(true, CRController.AddRadioTelescopeControllerAndStart(RTController0));
            Assert.AreEqual(true, CRController.AddRadioTelescopeControllerAndStart(RTController1));
            Assert.AreEqual(true, CRController.AddRadioTelescopeControllerAndStart(RTController2));
            Assert.AreEqual(true, CRController.RemoveRadioTelescopeController(RTController0, false));
            Assert.AreEqual(true, CRController.RemoveRadioTelescopeController(RTController1, false));
            Assert.AreEqual(true, CRController.RemoveRadioTelescopeController(RTController2, false));
        }

        [TestMethod]
        public void TestAddRTControllerAndSoftRemoveInstance()
        {
            Assert.AreEqual(true, CRController.AddRadioTelescopeController(RTController0));
            Assert.AreEqual(true, CRController.AddRadioTelescopeController(RTController1));
            Assert.AreEqual(true, CRController.AddRadioTelescopeController(RTController2));
            Assert.AreEqual(true, CRController.RemoveRadioTelescopeController(RTController0, true));
            Assert.AreEqual(true, CRController.RemoveRadioTelescopeController(RTController1, true));
            Assert.AreEqual(true, CRController.RemoveRadioTelescopeController(RTController2, true));
        }

        [TestMethod]
        public void TestAddRTControllerAndHardRemoveIndex()
        {
            Assert.AreEqual(true, CRController.AddRadioTelescopeControllerAndStart(RTController0));
            Assert.AreEqual(true, CRController.AddRadioTelescopeControllerAndStart(RTController1));
            Assert.AreEqual(true, CRController.AddRadioTelescopeControllerAndStart(RTController2));
            Assert.AreEqual(true, CRController.RemoveRadioTelescopeControllerAt(0, false));
            Assert.AreEqual(true, CRController.RemoveRadioTelescopeControllerAt(0, false));
            Assert.AreEqual(true, CRController.RemoveRadioTelescopeControllerAt(0, false));
        }

        [TestMethod]
        public void TestAddRTControllerAndStartAndSoftRemoveIndex()
        {
            Assert.AreEqual(true, CRController.AddRadioTelescopeControllerAndStart(RTController0));
            Assert.AreEqual(true, CRController.AddRadioTelescopeControllerAndStart(RTController1));
            Assert.AreEqual(true, CRController.AddRadioTelescopeControllerAndStart(RTController2));
            Assert.AreEqual(true, CRController.RemoveRadioTelescopeControllerAt(2, true));
            Assert.AreEqual(true, CRController.RemoveRadioTelescopeControllerAt(1, true));
            Assert.AreEqual(true, CRController.RemoveRadioTelescopeControllerAt(0, true));
        }

        [TestMethod]
        public void TestPreventAddMultipleTimes()
        {
            Assert.AreEqual(true, CRController.AddRadioTelescopeControllerAndStart(RTController0));
            Assert.AreEqual(false, CRController.AddRadioTelescopeControllerAndStart(RTController0));
            Assert.AreEqual(true, CRController.RemoveRadioTelescopeController(RTController0, true));
        }

        [TestMethod]
        public void TestPreventRemoveMultipleTimes()
        {
            Assert.AreEqual(true, CRController.AddRadioTelescopeControllerAndStart(RTController0));
            Assert.AreEqual(true, CRController.RemoveRadioTelescopeController(RTController0, true));
            Assert.AreEqual(false, CRController.RemoveRadioTelescopeController(RTController0, true));
        }
    }
}
