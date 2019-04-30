﻿using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ControlRoomApplication.Entities;
using ControlRoomApplication.Controllers;
using ControlRoomApplication.Constants;

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
            string IP = MiscellaneousConstants.LOCAL_HOST_IP;

            rtManagementThreads = new List<RadioTelescopeControllerManagementThread>()
            {
                new RadioTelescopeControllerManagementThread(new RadioTelescopeController(
                    new RadioTelescope(new SpectraCyberController(new SpectraCyber()), new YCPBaseTCPIPCommunicationHandler(IP, 8080), new Location(), new Orientation()))),
                new RadioTelescopeControllerManagementThread(new RadioTelescopeController(
                    new RadioTelescope(new SpectraCyberController(new SpectraCyber()), new YCPBaseTCPIPCommunicationHandler(IP, 8081), new Location(), new Orientation()))),
                new RadioTelescopeControllerManagementThread(new RadioTelescopeController(
                    new RadioTelescope(new SpectraCyberController(new SpectraCyber()), new YCPBaseTCPIPCommunicationHandler(IP, 8082), new Location(), new Orientation()))),
            };

            controlRoom = new ControlRoom(weatherStation);
            controlRoom.RTControllerManagementThreads.Add(rtManagementThreads[0]);
            controlRoom.RTControllerManagementThreads.Add(rtManagementThreads[1]);
            controlRoom.RTControllerManagementThreads.Add(rtManagementThreads[2]);
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
