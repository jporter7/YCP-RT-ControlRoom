﻿using ControlRoomApplication;
using ControlRoomApplication.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ControlRoomApplicationTest.EntitiesTests
{
    [TestClass]
    public class SpectraCyberTest
    {
        private SpectraCyber spectraCyber;

        private string commPort;

        [TestInitialize]
        public void BuildUp()
        {
            commPort = Constants.SPECTRA_CYBER_DEFAULT_COMM_PORT;
            spectraCyber = new SpectraCyber(commPort);
        }

        [TestMethod]
        public void TestInitialization()
        {
            Assert.AreEqual(spectraCyber.CurrentModeType, SpectraCyberModeTypeEnum.Unknown);
            Assert.AreEqual(spectraCyber.CommPort, commPort);
            Assert.AreEqual(spectraCyber.CommunicationThreadActive, false);
            Assert.AreEqual(spectraCyber.KillCommunicationThreadFlag, false);
        }

        [TestMethod]
        public void TestSettersAndGetters()
        {
            spectraCyber.CurrentModeType = SpectraCyberModeTypeEnum.Continuum;
            Assert.AreEqual(spectraCyber.CurrentModeType, SpectraCyberModeTypeEnum.Continuum);

            spectraCyber.CurrentModeType = SpectraCyberModeTypeEnum.Spectral;
            Assert.AreEqual(spectraCyber.CurrentModeType, SpectraCyberModeTypeEnum.Spectral);
        }
    }
}