using ControlRoomApplication;
using ControlRoomApplication.Entities;
using ControlRoomApplication.Entities.RadioTelescope;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ControlRoomApplicationTest.EntitiesTests
{
    [TestClass]
    public class RadioTelescopeTest
    {
        private ScaleRadioTelescope scaleRadioTelescope;
        private ProductionRadioTelescope productionRadioTelescope;

        [TestInitialize]
        public void BuildUp()
        {
            scaleRadioTelescope = new ScaleRadioTelescope();
            productionRadioTelescope = new ProductionRadioTelescope();
        }

        [TestMethod]
        public void TestSimulatedTelescopeInitialization()
        {
            Assert.AreEqual(scaleRadioTelescope.Status, RadioTelescopeStatusEnum.UNKNOWN);
        }

        [TestMethod]
        public void TestFullTelescopeInitialization()
        {
            Assert.AreEqual(productionRadioTelescope.Status, RadioTelescopeStatusEnum.UNKNOWN);
        }

        [TestMethod]
        public void TestSettersAndGetters()
        {
            scaleRadioTelescope.Status = RadioTelescopeStatusEnum.SHUTDOWN;
            productionRadioTelescope.Status = RadioTelescopeStatusEnum.SHUTDOWN;
            Assert.AreEqual(scaleRadioTelescope.Status, RadioTelescopeStatusEnum.SHUTDOWN);
            Assert.AreEqual(productionRadioTelescope.Status, RadioTelescopeStatusEnum.SHUTDOWN);

            scaleRadioTelescope.Status = RadioTelescopeStatusEnum.IDLE;
            productionRadioTelescope.Status = RadioTelescopeStatusEnum.IDLE;
            Assert.AreEqual(scaleRadioTelescope.Status, RadioTelescopeStatusEnum.IDLE);
            Assert.AreEqual(productionRadioTelescope.Status, RadioTelescopeStatusEnum.IDLE);

            scaleRadioTelescope.Status = RadioTelescopeStatusEnum.RUNNING_AND_INTEGRATING;
            productionRadioTelescope.Status = RadioTelescopeStatusEnum.RUNNING_AND_INTEGRATING;
            Assert.AreEqual(scaleRadioTelescope.Status, RadioTelescopeStatusEnum.RUNNING_AND_INTEGRATING);
            Assert.AreEqual(productionRadioTelescope.Status, RadioTelescopeStatusEnum.RUNNING_AND_INTEGRATING);
        }
    }
}