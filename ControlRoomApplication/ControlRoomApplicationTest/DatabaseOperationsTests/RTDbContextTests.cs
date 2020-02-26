using ControlRoomApplication.Constants;
using ControlRoomApplication.Main;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ControlRoomApplicationTest.DatabaseOperationsTests
{
    [TestClass]
    public class RTDbContextTests
    {
        private RTDbContext context1;

        [TestInitialize]
        public void BuildUp()
        {
            context1 = new RTDbContext("server=localhost;database=radio_telescope;uid=root;pwd=ycpRT2018!;");
        }

        [TestMethod]
        public void TestContextInitialization()
        {
            Assert.IsTrue(context1.Appointments != null);
            Assert.IsTrue(context1.RFDatas != null);
            Assert.IsTrue(context1.Orientations != null);
            Assert.IsTrue(context1.Coordinates != null);

            Assert.AreEqual(MiscellaneousConstants.LOCAL_DATABASE_NAME.ToLower(), context1.Database.Connection.Database.ToLower());
            Assert.AreEqual("localhost", context1.Database.Connection.DataSource);
        }
    }
}
