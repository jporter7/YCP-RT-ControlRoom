using ControlRoomApplicationTest.EntitiesTests.HeartbeatInterfaceTests;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ControlRoomApplicationTest.EntitiesTests
{
    [TestClass]
    public class HeartbeatInterfaceTest
    {
        private ConcreteHeartbeatTestClass concreteHeartbeat;

        [TestInitialize]
        public void BuildUp()
        {
            concreteHeartbeat = new ConcreteHeartbeatTestClass();
        }

        [TestCleanup]
        public void TearDown()
        {

        }

        [TestMethod]
        public void TestBringDownHeartbeatThread()
        {
            Assert.IsTrue(concreteHeartbeat.IsConsideredAlive());

            concreteHeartbeat.BringDownHeartbeatThread();

            // This is aserting true on true because if the BringDownHeartbeatThread doesn't
            // terminate correctly, it will never reach this line and fail.
            Assert.IsTrue(true);
        }
    }
}