using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ControlRoomApplication.Constants;
using ControlRoomApplication.Entities;
using ControlRoomApplication.Entities.RadioTelescope;
using ControlRoomApplication.Controllers;
using ControlRoomApplication.Controllers.SpectraCyberController;
using ControlRoomApplication.Controllers.RadioTelescopeControllers;
using ControlRoomApplication.Controllers.PLCCommunication;
using ControlRoomApplication.Simulators.Hardware.WeatherStation;

namespace ControlRoomApplicationTest.EntityControllersTests
{
    [TestClass]
    public class ControlRoomControllerTest
    {
        public static ControlRoomController CRController;
        public static ControlRoom ControlRoom;
        public static Location JohnRudyPark;
        public static string IP = PLCConstants.LOCAL_HOST_IP;
        public static int Port = PLCConstants.PORT_8080;
        public static RadioTelescopeController RTController0;
        public static RadioTelescopeController RTController1;
        public static RadioTelescopeController RTController2;

        [ClassInitialize]
        public static void BringUp(TestContext context)
        {
            ControlRoom = new ControlRoom(new SimulationWeatherStation(100));
            CRController = new ControlRoomController(ControlRoom);
            JohnRudyPark = new Location(76.7046, 40.0244, 395.0); // John Rudy Park hardcoded for now
        }

        [TestInitialize]
        public void ReinitializeRTs()
        {
            RTController0 = new RadioTelescopeController(
                new RadioTelescope(
                    new SpectraCyberSimulatorController(new SpectraCyberSimulator()),
                    new PLCClientCommunicationHandler(IP, Port),
                    JohnRudyPark
                )
            );

            RTController1 = new RadioTelescopeController(
                new RadioTelescope(
                    new SpectraCyberSimulatorController(new SpectraCyberSimulator()),
                    new PLCClientCommunicationHandler(IP, Port),
                    JohnRudyPark
                )
            );

            RTController2 = new RadioTelescopeController(
                new RadioTelescope(
                    new SpectraCyberSimulatorController(new SpectraCyberSimulator()),
                    new PLCClientCommunicationHandler(IP, Port),
                    JohnRudyPark
                )
            );
        }

        [TestMethod]
        public void TestConstructorAndProperties()
        {
            Assert.AreEqual(ControlRoom, CRController.ControlRoom);
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

            foreach (RadioTelescopeControllerManagementThread ManagementThread in CRController.ControlRoom.RTControllerManagementThreads)
            {
                ManagementThread.Start();
            }

            Assert.AreEqual(true, CRController.RemoveRadioTelescopeController(RTController0, true));
            Assert.AreEqual(true, CRController.RemoveRadioTelescopeController(RTController1, true));
            Assert.AreEqual(true, CRController.RemoveRadioTelescopeController(RTController2, true));
        }

        [TestMethod]
        public void TestAddRTControllerAndHardRemoveIndex()
        {
            Assert.AreEqual(true, CRController.AddRadioTelescopeController(RTController0));
            Assert.AreEqual(true, CRController.AddRadioTelescopeController(RTController1));
            Assert.AreEqual(true, CRController.AddRadioTelescopeController(RTController2));

            foreach (RadioTelescopeControllerManagementThread ManagementThread in CRController.ControlRoom.RTControllerManagementThreads)
            {
                ManagementThread.Start();
            }


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
