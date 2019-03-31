using System;
using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ControlRoomApplication.Constants;
using ControlRoomApplication.Entities;
using ControlRoomApplication.Controllers.SpectraCyberController;
using ControlRoomApplication.Simulators.Hardware.WeatherStation;

namespace ControlRoomApplicationTest.EntitiesTests
{
    [TestClass]
    public class HeartbeatTrackerContainerTest
    {
        private SpectraCyberSimulatorController HBISCSController;
        private SimulationWeatherStation HBISimWeatherStation;

        [TestInitialize]
        public void BuildUp()
        {
            HBISCSController = new SpectraCyberSimulatorController(new SpectraCyberSimulator());
            HBISCSController.SetSpectraCyberModeType(SpectraCyberModeTypeEnum.CONTINUUM);
            HBISCSController.BringUp();

            HBISimWeatherStation = new SimulationWeatherStation(100);
            HBISimWeatherStation.Start();
        }

        [TestMethod]
        public void TestLifecycle()
        {
            Assert.AreEqual(2, HeartbeatTrackerContainer.GetNumberOfChildren());

            for (int i = 0; i < 5; i++)
            {
                Thread.Sleep(HeartbeatConstants.INTERFACE_CHECK_IN_RATE_MS);
                Assert.AreEqual(true, HeartbeatTrackerContainer.ChildrenAreAlive());
            }

            HeartbeatTrackerContainer.SafelyKillHeartbeatComponents();

            Assert.AreEqual(2, HeartbeatTrackerContainer.GetNumberOfChildren());

            HeartbeatTrackerContainer.StopTracking(HBISCSController);
            Assert.AreEqual(1, HeartbeatTrackerContainer.GetNumberOfChildren());

            HeartbeatTrackerContainer.StopTracking(HBISimWeatherStation);
            Assert.AreEqual(0, HeartbeatTrackerContainer.GetNumberOfChildren());
        }

        [TestCleanup]
        public void BringDown()
        {
            HBISCSController.BringDown();
            HBISimWeatherStation.RequestKillAndJoin();
        }
    }
}
