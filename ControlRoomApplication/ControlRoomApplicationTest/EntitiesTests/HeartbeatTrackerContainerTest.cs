using System;
using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ControlRoomApplication.Controllers;
using ControlRoomApplication.Constants;
using ControlRoomApplication.Entities;
using ControlRoomApplication.Simulators.Hardware.WeatherStation;

namespace ControlRoomApplicationTest.EntitiesTests
{
    [TestClass]
    public class HeartbeatTrackerContainerTest
    {
        private SpectraCyberSimulatorController HBISCSController;

        [TestInitialize]
        public void BuildUp()
        {
            HeartbeatTrackerContainer.clearChildren();
            HBISCSController = new SpectraCyberSimulatorController(new SpectraCyberSimulator());
            HBISCSController.SetSpectraCyberModeType(SpectraCyberModeTypeEnum.CONTINUUM);
            HBISCSController.BringUp();

        }

        [TestMethod]
        public void TestLifecycle()
        {
            Assert.AreEqual(1, HeartbeatTrackerContainer.GetNumberOfChildren());

            for (int i = 0; i < 5; i++)
            {
                Thread.Sleep(HeartbeatConstants.INTERFACE_CHECK_IN_RATE_MS);
                Assert.AreEqual(true, HeartbeatTrackerContainer.ChildrenAreAlive());
            }

            HeartbeatTrackerContainer.SafelyKillHeartbeatComponents();

            Assert.AreEqual(1, HeartbeatTrackerContainer.GetNumberOfChildren());

            HeartbeatTrackerContainer.StopTracking(HBISCSController);
            Assert.AreEqual(0, HeartbeatTrackerContainer.GetNumberOfChildren());
        }

        [TestCleanup]
        public void BringDown()
        {
            HBISCSController.BringDown();
        }
    }
}
