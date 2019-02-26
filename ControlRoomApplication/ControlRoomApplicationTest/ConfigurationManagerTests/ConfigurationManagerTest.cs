using System;
using ControlRoomApplication.Controllers.PLCController;
using ControlRoomApplication.Controllers.SpectraCyberController;
using ControlRoomApplication.Entities;
using ControlRoomApplication.Entities.Plc;
using ControlRoomApplication.Entities.RadioTelescope;
using ControlRoomApplication.Main;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ControlRoomApplicationTest.ConfigurationTests
{
    [TestClass]
    public class ConfigurationManagerTest
    {
        [TestInitialize]
        public void BuildUp()
        {
            ConfigManager = new ConfigurationManager();
        }

        [TestCleanup]
        public void TearDown()
        {
            // Reset the classes we are testing to null
            PLC = null;
            SpectraCyber = null;
            RadioTelescope = null;
        }

        [TestMethod]
        public void TestConfigureScaleModelPLC()
        {
            PLC = ConfigManager.ConfigurePLC("/sP");
            ScaleModelPLC testPlc = new ScaleModelPLC();

            Assert.AreEqual(testPlc.GetType(), PLC.GetType());
        }

        [TestMethod]
        public void TestConfigureTestPLC()
        {
            PLC = ConfigManager.ConfigurePLC("/tP");
            TestPLC testPlc = new TestPLC();

            Assert.AreEqual(testPlc.GetType(), PLC.GetType());

            PLC = ConfigManager.ConfigurePLC("s$a");

            Assert.AreEqual(testPlc.GetType(), PLC.GetType());
        }

        [TestMethod]
        public void TestConfigureVirtualPLC()
        {
            PLC = ConfigManager.ConfigurePLC("/Vp");
            //VRPLC testPlc = new VRPLC();

            // will need to be uncommented when the VR constructor is fixed
            Assert.AreEqual(typeof(VRPLC), PLC.GetType());
        }

        [TestMethod]
        public void TestConfigureSimulatedSpecraCyber()
        {
            AbstractSpectraCyberController spectraCyberController = ConfigManager.ConfigureSpectraCyberController("/Ss");
            SpectraCyberSimulator testSpectraCyber = new SpectraCyberSimulator();

            Assert.AreEqual(testSpectraCyber.GetType(), SpectraCyber.GetType());

            spectraCyberController = ConfigManager.ConfigureSpectraCyberController("!@s");

            Assert.AreEqual(testSpectraCyber.GetType(), SpectraCyber.GetType());
        }

        [TestMethod]
        public void TestConfigureScaleRadioTelescope()
        {
            RadioTelescope = ConfigManager.ConfigureRadioTelescope("/sr", new SpectraCyberController(new SpectraCyber()), new ScaleModelPLC());
            ScaleRadioTelescope testRT = new ScaleRadioTelescope();

            Assert.AreEqual(testRT.GetType(), RadioTelescope.GetType());

            RadioTelescope = ConfigManager.ConfigureRadioTelescope("0.[", new SpectraCyberController(new SpectraCyber()), new ScaleModelPLC());

            Assert.AreEqual(testRT.GetType(), RadioTelescope.GetType());
        }

        [TestMethod]
        public void TestConfigureProductionRadioTelescope()
        {
            RadioTelescope = ConfigManager.ConfigureRadioTelescope("/PR", new SpectraCyberController(new SpectraCyber()), new ScaleModelPLC());
            ProductionRadioTelescope testRT = new ProductionRadioTelescope();

            Assert.AreEqual(testRT.GetType(), RadioTelescope.GetType());
        }

        private ConfigurationManager ConfigManager;
        private AbstractPLC PLC;
        private AbstractSpectraCyber SpectraCyber;
        private AbstractRadioTelescope RadioTelescope;
    }
}
