using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ControlRoomApplication.Controllers;
using ControlRoomApplication.Constants;

namespace ControlRoomApplicationTest.EntityControllersTests
{
    [TestClass]
    public class AbstractPLCDriverTest
    {
        public static TestPLCDriver DerivedAbstractPLCDriver;

        [ClassInitialize]
        public static void BringUp(TestContext context)
        {
            DerivedAbstractPLCDriver = new TestPLCDriver(PLCConstants.LOCAL_HOST_IP, PLCConstants.LOCAL_HOST_IP,8089, 8089);
        }

        [TestMethod]
        public void TestStartAndStopAsync()
        {
            Assert.AreEqual(true, DerivedAbstractPLCDriver.StartAsyncAcceptingClients());
            Assert.AreEqual(true, DerivedAbstractPLCDriver.RequestStopAsyncAcceptingClientsAndJoin());
        }

        [ClassCleanup]
        public static void Bringdown( ) {
            //DerivedAbstractPLCDriver.Calibrate();
            DerivedAbstractPLCDriver.Bring_down();
            DerivedAbstractPLCDriver = null;
        }
    }
}