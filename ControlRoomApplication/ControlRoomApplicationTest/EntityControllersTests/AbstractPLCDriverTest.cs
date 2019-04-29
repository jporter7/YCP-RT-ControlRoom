using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ControlRoomApplication.Controllers;
using ControlRoomApplication.Constants;

namespace ControlRoomApplicationTest.EntityControllersTests
{
    [TestClass]
    public class AbstractPLCDriverTest
    {
        private static TestPLCTCPIPReceiver DerivedAbstractPLCDriver;

        [ClassInitialize]
        public static void BringUp(TestContext context)
        {
            DerivedAbstractPLCDriver = new TestPLCTCPIPReceiver(PLCConstants.LOCAL_HOST_IP, 8080);
        }

        [TestMethod]
        public void TestStartAndStopAsync()
        {
            Assert.AreEqual(true, DerivedAbstractPLCDriver.StartAsyncAcceptingClients());
            Assert.AreEqual(true, DerivedAbstractPLCDriver.StopAsyncAcceptingClientsAndJoin());
        }
    }
}