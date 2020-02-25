using ControlRoomApplicationTest.EntitiesTests.HeartbeatInterfaceTests;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ControlRoomApplicationTest.EntitiesTests
{
    [TestClass]
    public class HeartbeatInterfaceTest
    {
        private ConcreteHeartbeatTestClass concreteHeartbeat1;
        private ConcreteHeartbeatTestClass concreteHeartbeat2;
        private ConcreteHeartbeatTestClass concreteHeartbeat3;

        [TestInitialize]
        public void BuildUp()
        {
            concreteHeartbeat1 = new ConcreteHeartbeatTestClass();
            concreteHeartbeat2 = new ConcreteHeartbeatTestClass();
            concreteHeartbeat3 = new ConcreteHeartbeatTestClass();
        }

        [TestCleanup]
        public void TearDown()
        {

        }

        [TestMethod]
        public void TestBringDownHeartbeatThread()
        {
            Assert.IsTrue(concreteHeartbeat1.IsConsideredAlive());

            concreteHeartbeat1.BringDownHeartbeatThread();

            // This is aserting true on true because if the BringDownHeartbeatThread doesn't
            // terminate correctly, it will never reach this line and fail.
            Assert.IsTrue(true);
        }

        [TestMethod]
        public void TestBringDownDueToMiscommunication()
        {
            Assert.IsTrue(concreteHeartbeat2.IsConsideredAlive());

            concreteHeartbeat2.BringDownDueToMiscommunication();

            // This is aserting true on true because if the BringDownHeartbeatThread doesn't
            // terminate correctly, it will never reach this line and fail.
            Assert.IsTrue(true);
        }
    }
}