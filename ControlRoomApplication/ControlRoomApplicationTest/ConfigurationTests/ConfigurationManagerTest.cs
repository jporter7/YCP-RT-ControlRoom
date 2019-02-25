using System;
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
        public void TestConfigureProductionSpectraCyber()
        {
            SpectraCyber = ConfigManager.ConfigureSpectraCyber("/pS");
            SpectraCyber testSpectraCyber = new SpectraCyber();

            Assert.AreEqual(testSpectraCyber.GetType(), SpectraCyber.GetType());
        }

        [TestMethod]
        public void TestConfigureSimulatedSpecraCyber()
        {
            SpectraCyber = ConfigManager.ConfigureSpectraCyber("/Ss");
            SpectraCyberSimulator testSpectraCyber = new SpectraCyberSimulator();

            Assert.AreEqual(testSpectraCyber.GetType(), SpectraCyber.GetType());

            SpectraCyber = ConfigManager.ConfigureSpectraCyber("!@s");

            Assert.AreEqual(testSpectraCyber.GetType(), SpectraCyber.GetType());
        }

        [TestMethod]
        public void TestConfigureScaleRadioTelescope()
        {
            RadioTelescope = ConfigManager.ConfigureRadioTelescope("/sr");
            ScaleRadioTelescope testRT = new ScaleRadioTelescope();

            Assert.AreEqual(testRT.GetType(), RadioTelescope.GetType());

            RadioTelescope = ConfigManager.ConfigureRadioTelescope("0.[");

            Assert.AreEqual(testRT.GetType(), RadioTelescope.GetType());
        }

        [TestMethod]
        public void TestConfigureProductionRadioTelescope()
        {
            RadioTelescope = ConfigManager.ConfigureRadioTelescope("/PR");
            ProductionRadioTelescope testRT = new ProductionRadioTelescope();

            Assert.AreEqual(testRT.GetType(), RadioTelescope.GetType());
        }

        private ConfigurationManager ConfigManager;
        private AbstractPLC PLC;
        private AbstractSpectraCyber SpectraCyber;
        private AbstractRadioTelescope RadioTelescope;
    }
}
