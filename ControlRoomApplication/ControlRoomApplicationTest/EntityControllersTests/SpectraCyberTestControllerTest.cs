using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ControlRoomApplication.Controllers;
using ControlRoomApplication.Entities;

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

        [TestMethod]
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
    }
}
