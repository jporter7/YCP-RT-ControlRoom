using ControlRoomApplication;
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
            commPort = Constants.SPECTRA_CYBER_DEFAULT_COMM_PORT;
            spectraCyber = new SpectraCyber(commPort);
        }

        [TestMethod]
        public void TestInitialization()
        {
            Assert.AreEqual(spectraCyber.CurrentModeType, SpectraCyberModeTypeEnum.Unknown);
            Assert.AreEqual(commPort, spectraCyber.CommPort);

            Assert.AreEqual(spectraCyber.SCSerialPort.PortName, spectraCyber.CommPort);
            Assert.AreEqual(spectraCyber.SCSerialPort.BaudRate, Constants.SPECTRA_CYBER_BAUD_RATE);
            Assert.AreEqual(spectraCyber.SCSerialPort.Parity, Constants.SPECTRA_CYBER_PARITY_BITS);
            Assert.AreEqual(spectraCyber.SCSerialPort.DataBits, Constants.SPECTRA_CYBER_DATA_BITS);
            Assert.AreEqual(spectraCyber.SCSerialPort.StopBits, Constants.SPECTRA_CYBER_STOP_BITS);
            Assert.AreEqual(spectraCyber.SCSerialPort.ReadTimeout, Constants.SPECTRA_CYBER_TIMEOUT_MS);
            Assert.AreEqual(spectraCyber.SCSerialPort.WriteTimeout, Constants.SPECTRA_CYBER_TIMEOUT_MS);
        }

        [TestMethod]
        public void TestSettersAndGetters()
        {
            spectraCyber.CurrentModeType = SpectraCyberModeTypeEnum.Continuum;
            Assert.AreEqual(spectraCyber.CurrentModeType, SpectraCyberModeTypeEnum.Continuum);

            spectraCyber.CurrentModeType = SpectraCyberModeTypeEnum.Spectral;
            Assert.AreEqual(spectraCyber.CurrentModeType, SpectraCyberModeTypeEnum.Spectral);
        }
    }
}