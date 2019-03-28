using ControlRoomApplication.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ControlRoomApplicationTest.EntitiesTests
{
    [TestClass]
    public class SpectraCyberConfigTest
    {
        // Class being tested
        private SpectraCyberConfig spectraCyberConfig;
        private SpectraCyberModeTypeEnum mode = SpectraCyberModeTypeEnum.CONTINUUM;
        private SpectraCyberIntegrationTimeEnum integration_time = SpectraCyberIntegrationTimeEnum.MID_TIME_SPAN;
        private double offset_voltage = 0.4;
        private double if_gain = 1;
        private double dc_gain = 1;
        private double bandwidth = 3;

        [TestInitialize]
        public void BuildUp()
        {
            // Initialize appointment entity
            spectraCyberConfig = new SpectraCyberConfig(mode, integration_time, offset_voltage, if_gain, dc_gain, bandwidth);
        }

        [TestMethod]
        public void TestGettersAndSetters()
        {
            Assert.AreEqual(mode, spectraCyberConfig.Mode);
            Assert.AreEqual(integration_time, spectraCyberConfig.IntegrationTime);
            Assert.AreEqual(offset_voltage, spectraCyberConfig.OffsetVoltage);
            Assert.AreEqual(if_gain, spectraCyberConfig.IFGain);
            Assert.AreEqual(dc_gain, spectraCyberConfig.DCGain);
            Assert.AreEqual(bandwidth, spectraCyberConfig.Bandwidth);
        }
    }
}
