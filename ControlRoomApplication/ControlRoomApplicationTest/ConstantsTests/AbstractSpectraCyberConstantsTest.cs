using ControlRoomApplication.Constants;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO.Ports;

namespace ControlRoomApplicationTest.ConstantsTests
{
    [TestClass]
    public class AbstractSpectraCyberConstantsTest
    {
        [TestMethod]
        public void TestAbstractSpectraCyberConstanst()
        {
            Assert.AreEqual(AbstractSpectraCyberConstants.BAUD_RATE, 2400);
            Assert.AreEqual(AbstractSpectraCyberConstants.DATA_BITS, 8);
            Assert.AreEqual(AbstractSpectraCyberConstants.PARITY_BITS, Parity.None);
            Assert.AreEqual(AbstractSpectraCyberConstants.STOP_BITS, StopBits.One);
            Assert.AreEqual(AbstractSpectraCyberConstants.BUFFER_SIZE, 4);
            Assert.AreEqual(AbstractSpectraCyberConstants.TIMEOUT_MS, 1000);
            Assert.AreEqual(AbstractSpectraCyberConstants.WAIT_TIME_MS, 70);
            Assert.AreEqual(AbstractSpectraCyberConstants.CLIP_BUFFER_RESPONSE, true);
            Assert.AreEqual(AbstractSpectraCyberConstants.DEFAULT_COMM_PORT, "COM5");
            Assert.AreEqual(AbstractSpectraCyberConstants.SIMULATED_RF_INTENSITY_MINIMUM, 0.0);
            Assert.AreEqual(AbstractSpectraCyberConstants.SIMULATED_RF_INTENSITY_MAXIMUM, 10.0);
            Assert.AreEqual(AbstractSpectraCyberConstants.SIMULATED_RF_INTENSITY_DISCRETIZATION, 0.001);
        }

    }
}
