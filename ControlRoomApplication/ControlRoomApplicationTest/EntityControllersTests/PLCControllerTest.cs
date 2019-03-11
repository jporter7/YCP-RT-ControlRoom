using System;
using ControlRoomApplication.Controllers.PLCCommunication;
using ControlRoomApplication.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ControlRoomApplicationTest.EntityControllersTests
{
    [TestClass]
    public class PLCControllerTest
    {
        [TestInitialize]
        public void BuildUp()
        {

        }

        [TestMethod]
        public void TestCalibrateTelescope()
        {
            //Assert.AreEqual("Radio Telescope successfully calibrated", testcontroller.CalibrateRT());
        }

        [TestMethod]
        public void TestShutdown()
        {
            //Assert.AreEqual("Radio Telescope successfully shut down", testcontroller.CalibrateRT());
        }

        [TestMethod]
        public void TestMoveTelescope()
        {
            //Assert.AreEqual("Oh yea we received a message from the Radio Telescope boyo", testcontroller.CalibrateRT());
        }

        [TestMethod]
        public void TestMoveScaleModel()
        {
            //Assert.AreEqual("Oh yea we received a message from the Radio Telescope boyo", testcontroller.CalibrateRT());
        }

    }
}
