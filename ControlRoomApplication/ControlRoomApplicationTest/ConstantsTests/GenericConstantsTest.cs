using ControlRoomApplication.Constants;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ControlRoomApplicationTest.ConstantsTests
{
    [TestClass]
    class GenericConstantsTest
    {
        [TestMethod]
        public void TestGenericConstants()
        {
            Assert.AreEqual(GenericConstants.MAX_USERNAME_LENGTH, 15);
            Assert.AreEqual(GenericConstants.LOCAL_DATABASE_NAME, "rtdatabase");
        }
    }
}
