using ControlRoomApplication;
using ControlRoomApplication.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ControlRoomApplicationTest.EntitiesTests
{
    [TestClass]
    public class AbstractRadioTelescopeTest
    {
        private SimulatedTelescope simulatedTelescope;
        private FullTelescope fullTelescope;

        [TestInitialize]
        public void BuildUp()
        {
            //simulatedTelescope = new SimulatedTelescope();
            //fullTelescope = new FullTelescope();
        }

        [TestMethod]
        public void TestSimulatedTelescopeInitialization()
        {
            Assert.AreEqual(simulatedTelescope.CurrentStatus, RadioTelescopeStatusEnum.Unknown);
        }

        [TestMethod]
        public void TestFullTelescopeInitialization()
        {
            Assert.AreEqual(fullTelescope.CurrentStatus, RadioTelescopeStatusEnum.Unknown);
        }

        [TestMethod]
        public void TestSettersAndGetters()
        {
            simulatedTelescope.CurrentStatus = RadioTelescopeStatusEnum.ShutDown;
            fullTelescope.CurrentStatus = RadioTelescopeStatusEnum.ShutDown;
            Assert.AreEqual(simulatedTelescope.CurrentStatus, RadioTelescopeStatusEnum.ShutDown);
            Assert.AreEqual(fullTelescope.CurrentStatus, RadioTelescopeStatusEnum.ShutDown);

            simulatedTelescope.CurrentStatus = RadioTelescopeStatusEnum.Idle;
            fullTelescope.CurrentStatus = RadioTelescopeStatusEnum.Idle;
            Assert.AreEqual(simulatedTelescope.CurrentStatus, RadioTelescopeStatusEnum.Idle);
            Assert.AreEqual(fullTelescope.CurrentStatus, RadioTelescopeStatusEnum.Idle);

            simulatedTelescope.CurrentStatus = RadioTelescopeStatusEnum.MovingAndIntegrating;
            fullTelescope.CurrentStatus = RadioTelescopeStatusEnum.MovingAndIntegrating;
            Assert.AreEqual(simulatedTelescope.CurrentStatus, RadioTelescopeStatusEnum.MovingAndIntegrating);
            Assert.AreEqual(fullTelescope.CurrentStatus, RadioTelescopeStatusEnum.MovingAndIntegrating);
        }
    }
}