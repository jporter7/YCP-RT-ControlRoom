using Microsoft.VisualStudio.TestTools.UnitTesting;
using ControlRoomApplication.Controllers;
using ControlRoomApplication.Entities;

namespace ControlRoomApplicationTest.EntityControllersTests
{
    [TestClass]
    public class SpectraCyberControllerTest
    {
        public AbstractSpectraCyber SpectraCyber { get; set; }
        public AbstractSpectraCyberController SpectraCyberController { get; set; }

        [TestInitialize]
        public void BuildUp()
        {
            // Discuss with team how this should be tested, given that it's a hardware interface
        }
    }
}