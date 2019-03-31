using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ControlRoomApplication.Main;
using ControlRoomApplication.Entities;
using ControlRoomApplication.Controllers.PLCCommunication;
using ControlRoomApplication.Controllers.SpectraCyberController;
using ControlRoomApplication.Simulators.Hardware.WeatherStation;

namespace ControlRoomApplicationTest.ConfigurationTests
{
    [TestClass]
    public class ConfigurationManagerTest
    {
        [TestMethod]
        public void TestNoInputConfiguration()
        {
            string[] InputArgs =
            {
                "0",
                "/SW"
            };

            (List<(RadioTelescope, AbstractPLCDriver)>, AbstractWeatherStation) results = ConfigurationManager.BuildRadioTelescopeSeries(InputArgs, true);

            Assert.IsInstanceOfType(results.Item2, typeof(SimulationWeatherStation));
            Assert.AreEqual(0, results.Item1.Count);
        }

        [TestMethod]
        public void TestOneInputConfiguration()
        {
            string[] InputArgs =
            {
                "1",
                "/SW",
                "/TR,/SS,127.0.0.1,8080"
            };

            (List<(RadioTelescope, AbstractPLCDriver)>, AbstractWeatherStation) results = ConfigurationManager.BuildRadioTelescopeSeries(InputArgs, true);

            Assert.IsInstanceOfType(results.Item2, typeof(SimulationWeatherStation));
            Assert.AreEqual(1, results.Item1.Count);
            Assert.IsInstanceOfType(results.Item1[0].Item2, typeof(TestPLCDriver));
            Assert.IsInstanceOfType(results.Item1[0].Item1.SpectraCyberController, typeof(SpectraCyberSimulatorController));
        }

        [TestMethod]
        public void TestMultipleInputConfigurations()
        {
            string[] InputArgs =
            {
                "4",
                "/SW",
                "/TR,/SS,127.0.0.1,8080",
                "/TR,/SS,127.0.0.1,8081",
                "/TR,/SS,127.0.0.1,8082",
                "/TR,/SS,127.0.0.1,8083"
            };

            (List<(RadioTelescope, AbstractPLCDriver)>, AbstractWeatherStation) results = ConfigurationManager.BuildRadioTelescopeSeries(InputArgs, true);

            Assert.IsInstanceOfType(results.Item2, typeof(SimulationWeatherStation));
            Assert.AreEqual(4, results.Item1.Count);
            Assert.IsInstanceOfType(results.Item1[0].Item2, typeof(TestPLCDriver));
            Assert.IsInstanceOfType(results.Item1[0].Item1.SpectraCyberController, typeof(SpectraCyberSimulatorController));
            Assert.IsInstanceOfType(results.Item1[1].Item2, typeof(TestPLCDriver));
            Assert.IsInstanceOfType(results.Item1[1].Item1.SpectraCyberController, typeof(SpectraCyberSimulatorController));
            Assert.IsInstanceOfType(results.Item1[2].Item2, typeof(TestPLCDriver));
            Assert.IsInstanceOfType(results.Item1[2].Item1.SpectraCyberController, typeof(SpectraCyberSimulatorController));
            Assert.IsInstanceOfType(results.Item1[3].Item2, typeof(TestPLCDriver));
            Assert.IsInstanceOfType(results.Item1[3].Item1.SpectraCyberController, typeof(SpectraCyberSimulatorController));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TestWrongNumberInput()
        {
            string[] InputArgs =
            {
                "5",
                "/SW",
                "/TR,/SS,127.0.0.1,8080",
                "/TR,/SS,127.0.0.1,8081",
                "/TR,/SS,127.0.0.1,8082",
                "/TR,/SS,127.0.0.1,8083"
            };

            ConfigurationManager.BuildRadioTelescopeSeries(InputArgs, true);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TestInvalidNumberInput()
        {
            string[] InputArgs =
            {
                "Four",
                "/SW",
                "/TR,/SS,127.0.0.1,8080",
                "/TR,/SS,127.0.0.1,8081",
                "/TR,/SS,127.0.0.1,8082",
                "/TR,/SS,127.0.0.1,8083"
            };

            ConfigurationManager.BuildRadioTelescopeSeries(InputArgs, true);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TestBadWeatherStationInput()
        {
            string[] InputArgs =
            {
                "4",
                "/W",
                "/TR,/SS,127.0.0.1,8080",
                "/TR,/SS,127.0.0.1,8081",
                "/TR,/SS,127.0.0.1,8082",
                "/TR,/SS,127.0.0.1,8083"
            };

            ConfigurationManager.BuildRadioTelescopeSeries(InputArgs, true);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TestBadPLCClientInput()
        {
            string[] InputArgs =
            {
                "1",
                "/SW",
                "/R,/SS,127.0.0.1,8080"
            };

            ConfigurationManager.BuildRadioTelescopeSeries(InputArgs, true);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TestBadSpectraCyberControllerInput()
        {
            string[] InputArgs =
            {
                "1",
                "/SW",
                "/TR,/SimS,127.0.0.1,8080"
            };

            ConfigurationManager.BuildRadioTelescopeSeries(InputArgs, true);
        }
    }
}
