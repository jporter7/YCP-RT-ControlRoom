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
            commPort = AbstractSpectraCyberConstants.SPECTRA_CYBER_DEFAULT_COMM_PORT;
            spectraCyber = new SpectraCyber(commPort);
        }

        [TestMethod]
        public void TestInitialization()
        {
            Assert.AreEqual(spectraCyber.CurrentModeType, SpectraCyberModeTypeEnum.UNKNOWN);
            Assert.AreEqual(spectraCyber.CommPort, commPort);
            Assert.AreEqual(spectraCyber.CommunicationThreadActive, false);
            Assert.AreEqual(spectraCyber.KillCommunicationThreadFlag, false);
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