using ControlRoomApplication.Constants;
using ControlRoomApplication.Main;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ControlRoomApplicationTest.DatabaseOperationsTests
{
    [TestClass]
    public class RTDbContextTests
    {
        private RTDbContext context1;
        private RTDbContext context2;

        [TestInitialize]
        public void BuildUp()
        {
            context1 = new RTDbContext();
            context2 = new RTDbContext("server=localhost;uid=root;persistsecurityinfo=True;database=RTDatabase;allowuservariables=True");
        }

        [TestMethod]
        public void TestContextInitialization()
        {
            Assert.IsTrue(context1.Appointments != null);
            Assert.IsTrue(context1.RFDatas != null);
            Assert.IsTrue(context1.Orientations != null);
            Assert.IsTrue(context1.Coordinates != null);

            Assert.AreEqual(GenericConstants.LOCAL_DATABASE_NAME.ToLower(), context1.Database.Connection.Database.ToLower());
            Assert.AreEqual("localhost", context1.Database.Connection.DataSource);

            Assert.AreEqual(GenericConstants.LOCAL_DATABASE_NAME.ToLower(), context2.Database.Connection.Database.ToLower());
            Assert.IsFalse(context2.Configuration.LazyLoadingEnabled);
        }
    }
}
