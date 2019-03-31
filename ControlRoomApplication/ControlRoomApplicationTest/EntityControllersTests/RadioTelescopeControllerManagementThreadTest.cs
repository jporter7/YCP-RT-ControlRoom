using System;
using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ControlRoomApplication.Constants;
using ControlRoomApplication.Entities;
using ControlRoomApplication.Controllers;
using ControlRoomApplication.Controllers.RadioTelescopeControllers;
using ControlRoomApplication.Controllers.SpectraCyberController;
using ControlRoomApplication.Controllers.PLCCommunication;

namespace ControlRoomApplicationTest.EntityControllersTests
{
    [TestClass]
    public class RadioTelescopeControllerManagementThreadTest
    {
        private string IP;
        private static int Port0;
        private int Port1;

        private Location JohnRudyPark;
        private Orientation CalibrationOrientation;

        private PLCClientCommunicationHandler PLCCCH0;
        private PLCClientCommunicationHandler PLCCCH1;

        private RadioTelescopeController RTC0;
        private RadioTelescopeController RTC1;

        private RadioTelescopeControllerManagementThread RTCMT0;
        private RadioTelescopeControllerManagementThread RTCMT1;

        [TestInitialize]
        public void BringUp()
        {
            IP = PLCConstants.LOCAL_HOST_IP;
            Port0 = 8080;
            Port1 = 8081;

            JohnRudyPark = new Location(76.7046, 40.0244, 395.0); // John Rudy Park hardcoded for now
            CalibrationOrientation = new Orientation(0, 90);

            PLCCCH0 = new PLCClientCommunicationHandler(IP, Port0);
            RTC0 = new RadioTelescopeController(new RadioTelescope(new SpectraCyberSimulatorController(new SpectraCyberSimulator()), PLCCCH0, JohnRudyPark, CalibrationOrientation, 1));
            RTCMT0 = new RadioTelescopeControllerManagementThread(RTC0);

            PLCCCH1 = new PLCClientCommunicationHandler(IP, Port1);
            RTC1 = new RadioTelescopeController(new RadioTelescope(new SpectraCyberSimulatorController(new SpectraCyberSimulator()), PLCCCH1, JohnRudyPark, CalibrationOrientation, 2));
            RTCMT1 = new RadioTelescopeControllerManagementThread(RTC1);
        }

        [TestMethod]
        public void TestConstructorsAndProperties()
        {
            Assert.AreEqual(RTC0, RTCMT0.RTController);
            Assert.AreEqual(RTC1, RTCMT1.RTController);

            Assert.AreEqual(1, RTCMT0.RadioTelescopeID);
            Assert.AreEqual(2, RTCMT1.RadioTelescopeID);

            Assert.AreEqual(null, RTCMT0.NextObjectiveOrientation);
            Assert.AreEqual(null, RTCMT1.NextObjectiveOrientation);

            Assert.AreEqual(false, RTCMT0.Busy);
            Assert.AreEqual(false, RTCMT1.Busy);
        }

        [TestMethod]
        public void TestLifecycle()
        {
            Assert.AreEqual(true, RTCMT0.Start());
            Assert.AreEqual(true, RTCMT1.Start());

            Thread.Sleep(100);

            Assert.AreEqual(true, RTCMT0.Busy);
            Assert.AreEqual(true, RTCMT1.Busy);

            RTCMT0.KillWithHardInterrupt();
            Assert.AreEqual(true, RTCMT0.WaitToJoin());

            RTCMT1.KillWithHardInterrupt();
            Assert.AreEqual(true, RTCMT1.WaitToJoin());

            Assert.AreEqual(false, RTCMT0.Busy);
            Assert.AreEqual(false, RTCMT1.Busy);
        }
    }
}
