using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ControlRoomApplication.Entities;
using System.Collections.Generic;
using ControlRoomApplication.Controllers;
using ControlRoomApplication.Controllers.RadioTelescopeControllers;
using ControlRoomApplication.Entities.RadioTelescope;
using ControlRoomApplication.Controllers.SpectraCyberController;
using ControlRoomApplication.Controllers.PLCCommunication;

namespace ControlRoomApplicationTest.EntitiesTests
{
    [TestClass]
    public class ControlRoomTest
    {
        private ControlRoom controlRoom;
        private AbstractWeatherStation weatherStation;
        private List<RadioTelescopeControllerManagementThread> rtManagementThreads;

        [TestInitialize]
        public void BuildUp()
        {
            rtManagementThreads = new List<RadioTelescopeControllerManagementThread>()
            {
                new RadioTelescopeControllerManagementThread(new RadioTelescopeController(
                    new RadioTelescope(new SpectraCyberController(new SpectraCyber()), new PLCClientCommunicationHandler("127.0.0.1", 8080), new Location(), new Orientation(0,0)))),
                new RadioTelescopeControllerManagementThread(new RadioTelescopeController(
                    new RadioTelescope(new SpectraCyberController(new SpectraCyber()), new PLCClientCommunicationHandler("127.0.0.1", 8080), new Location(), new Orientation(0,0)))),
                new RadioTelescopeControllerManagementThread(new RadioTelescopeController(
                    new RadioTelescope(new SpectraCyberController(new SpectraCyber()), new PLCClientCommunicationHandler("127.0.0.1", 8080), new Location(), new Orientation(0,0)))),
            };

            controlRoom = new ControlRoom(weatherStation);
            controlRoom.RTControllerManagementThreads.Add(rtManagementThreads[0]);
            controlRoom.RTControllerManagementThreads.Add(rtManagementThreads[1]);
            controlRoom.RTControllerManagementThreads.Add(rtManagementThreads[2]);
        }

        [TestCleanup]
        public void TearDown()
        {

        }
        
        [TestMethod]
        public void TestGettersAndSetters()
        {
            Assert.AreEqual(weatherStation, controlRoom.WeatherStation);
            Assert.IsTrue(controlRoom.RTControllerManagementThreads != null);

            Assert.IsTrue(controlRoom.RTControllerManagementThreads != null);
            Assert.AreEqual(3, controlRoom.RTControllerManagementThreads.Count);

            Assert.IsTrue(controlRoom.RTControllerManagementThreads[0].RTController != null);
            Assert.IsTrue(controlRoom.RTControllerManagementThreads[1].RTController != null);
            Assert.IsTrue(controlRoom.RTControllerManagementThreads[2].RTController != null);

            Assert.IsTrue(controlRoom.RTControllerManagementThreads[0].RTController.RadioTelescope != null);
            Assert.IsTrue(controlRoom.RTControllerManagementThreads[1].RTController.RadioTelescope != null);
            Assert.IsTrue(controlRoom.RTControllerManagementThreads[2].RTController.RadioTelescope != null);
        }
    }
}
