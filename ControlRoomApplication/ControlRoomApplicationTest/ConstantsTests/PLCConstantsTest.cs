using ControlRoomApplication.Constants;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ControlRoomApplicationTest.ConstantsTests
{
    [TestClass]
    public class PLCConstantsTest
    {
        [TestMethod]
        public void TestPLCConstants()
        {
            Assert.AreEqual(PLCConstants.CALIBRATE, "calibrate");
            Assert.AreEqual(PLCConstants.SHUTDOWN, "shutdown");
            Assert.AreEqual(PLCConstants.COM3, "COM3");
            Assert.AreEqual(PLCConstants.COM4, "COM4");
            Assert.AreEqual(PLCConstants.LOCAL_HOST_IP, "127.0.0.1");


            Assert.AreEqual(PLCConstants.PORT_8080, 8080);
            Assert.AreEqual(PLCConstants.SERIAL_PORT_BAUD_RATE, 9600);
            Assert.AreEqual(PLCConstants.RIGHT_ASCENSION_LOWER_LIMIT, -359);
            Assert.AreEqual(PLCConstants.RIGHT_ASCENSION_UPPER_LIMIT, 359);
            Assert.AreEqual(PLCConstants.DECLINATION_LOWER_LIMIT, -90);
            Assert.AreEqual(PLCConstants.DECLINATION_UPPER_LIMIT, 90);
        }
    }
}
