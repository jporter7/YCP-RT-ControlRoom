using System;
using ControlRoomApplication.Controllers.SpectraCyberController;
using ControlRoomApplication.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ControlRoomApplicationTest.EntityControllersTests
{
    [TestClass]
    public class SpectraCyberTestControllerTest
    {
        private SpectraCyberTestController spectraCyberTestController;
        private SpectraCyberSimulator spectraCyberSimulator;

        [TestInitialize]
        public void BuildUp()
        {
            spectraCyberSimulator = new SpectraCyberSimulator();
            spectraCyberTestController = new SpectraCyberTestController(spectraCyberSimulator);
        }

        [TestCleanup]
        public void TearDown()
        {

        }

        [TestCleanup]
        public void TestInitialization()
        {
            Assert.IsNotNull(spectraCyberTestController);
        }

        [TestMethod]
        public void TestBringUp()
        {
            Assert.IsTrue(spectraCyberTestController.BringUp());
        }

        [TestMethod]
        public void TestBringDown()
        {
            Assert.IsTrue(spectraCyberTestController.BringDown());
        }

        [TestMethod]
        public void TestIfComponentIsAlive()
        {

        }
    }
}
