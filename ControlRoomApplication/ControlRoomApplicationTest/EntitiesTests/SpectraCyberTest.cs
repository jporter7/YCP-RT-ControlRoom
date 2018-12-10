using ControlRoomApplication.Constants;
using ControlRoomApplication.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ControlRoomApplicationTest.EntitiesTests
{
    [TestClass]
    public class SpectraCyberTest
    {
        private SpectraCyber spectraCyber;

        private string commPort;

        [TestInitialize]
        public void BuildUp()
        {
            commPort = AbstractSpectraCyberConstants.DEFAULT_COMM_PORT;
            spectraCyber = new SpectraCyber(commPort);
        }

        [TestMethod]
        public void TestInitialization()
        {
            Assert.AreEqual(spectraCyber.CurrentModeType, SpectraCyberModeTypeEnum.UNKNOWN);
            Assert.AreEqual(spectraCyber.CommPort, commPort);
        }

        [TestMethod]
        public void TestSettersAndGetters()
        {
            spectraCyber.CurrentModeType = SpectraCyberModeTypeEnum.CONTINUUM;
            Assert.AreEqual(spectraCyber.CurrentModeType, SpectraCyberModeTypeEnum.CONTINUUM);

            spectraCyber.CurrentModeType = SpectraCyberModeTypeEnum.SPECTRAL;
            Assert.AreEqual(spectraCyber.CurrentModeType, SpectraCyberModeTypeEnum.SPECTRAL);
        }
    }
}