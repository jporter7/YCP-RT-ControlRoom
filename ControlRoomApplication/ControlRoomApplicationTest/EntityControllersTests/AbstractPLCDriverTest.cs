using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ControlRoomApplication.Constants;
using ControlRoomApplication.Controllers.PLCCommunication;

namespace ControlRoomApplicationTest.EntityControllersTests
{
    [TestClass]
    public class AbstractPLCDriverTest
    {
        private static TestPLCDriver DerivedAbstractPLCDriver;

        [ClassInitialize]
        public static void BringUp(TestContext context)
        {
            DerivedAbstractPLCDriver = new TestPLCDriver(PLCConstants.LOCAL_HOST_IP, 8080);
        }

        [TestMethod]
        public void TestStartAndStopAsync()
        {
            Assert.AreEqual(true, DerivedAbstractPLCDriver.StartAsyncAcceptingClients());
            Assert.AreEqual(true, DerivedAbstractPLCDriver.RequestStopAsyncAcceptingClientsAndJoin());
        }
    }
}