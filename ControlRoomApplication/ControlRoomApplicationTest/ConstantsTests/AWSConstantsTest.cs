using ControlRoomApplication;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ControlRoomApplicationTest.ConstantsTests
{
    [TestClass]
    public class AWSConstantsTest
    {
        [TestMethod]
        public void TestAWSConstants()
        {
            Assert.AreEqual(AWSConstants.REMOTE_CONNECTION_STRING, "server=aa1i104p24z1bqt.cgaa5jndlq2g.us-east-1.rds.amazonaws.com" +
            ";uid=root;password=nV7utVncnTzHeYEuG9X5;database=radio_telescope;port=3306;");
            Assert.AreEqual(AWSConstants.DATABASE_PROVIDER, "MySql.Data.MySqlClient");
            Assert.AreEqual(AWSConstants.REMOTE_DATABASE_NAME, "radio_telescope");
        }
    }
}
