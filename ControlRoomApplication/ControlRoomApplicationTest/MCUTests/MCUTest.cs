using Microsoft.VisualStudio.TestTools.UnitTesting;
using ControlRoomApplication.Hardware.MCU;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControlRoomApplicationTest.MCUTests
{
    [TestClass]
    public class MCUTest
    {
        [TestInitialize]
        public void BuildUp()
        {
            AzEncoder = new AzimuthEncoder();
            ElEncoder = new SimulationAbsoluteEncoder();

            Mcu = new SimulationMCU();
            Mcu.AzEncoder = AzEncoder;
            Mcu.ElEncoder = ElEncoder;
        }

        [TestCleanup]
        public void TearDown()
        {

        }

        [TestMethod]
        public void TestAzEncoder()
        {
            Mcu.AzEncoder.Degree = 300.0;
            Assert.AreEqual(300.0, Mcu.AzEncoder.Degree, 0.01);
        }

        [TestMethod]
        public void TestElEncoder()
        {
            Mcu.ElEncoder.Degree = 300.0;
            Assert.AreEqual(300.0, Mcu.AzEncoder.Degree, 0.01);
        }

        private SimulationMCU Mcu { get; set; }
        private AzimuthEncoder AzEncoder { get; set; }
        private SimulationAbsoluteEncoder ElEncoder { get; set; }
    }
}