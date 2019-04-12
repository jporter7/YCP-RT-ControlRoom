using Microsoft.VisualStudio.TestTools.UnitTesting;
using ControlRoomApplication.Controllers;
using ControlRoomApplication.Entities;

namespace ControlRoomApplicationTest.EntityControllersTests
{
    [TestClass]
    public class SpectraCyberSimulatorControllerTest
    {
        private SpectraCyberSimulatorController _spectraCyberController;
        private SpectraCyberSimulator _spectraCyberSimulator;

        [TestInitialize]
        public void BuildUp()
        {
            _spectraCyberSimulator = new SpectraCyberSimulator();
            _spectraCyberController = new SpectraCyberSimulatorController(_spectraCyberSimulator);
        }

        [TestMethod]
        public void TestConstructor()
        {
            Assert.IsNotNull(_spectraCyberController);
        }

        [TestMethod]
        public void TestBringUpBringDown()
        {
            Assert.IsTrue(_spectraCyberController.BringUp());
            Assert.IsTrue(_spectraCyberController.BringDown());
        }

    }
}
