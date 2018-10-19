using ControlRoomApplication.Entities;
using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ControlRoomApplicationTest.EntitiesTests
{
    [TestClass]
    public class RFDataTest
    {
        private RFData rfdata;

        private int id;
        private DateTime timeCaptured;
        private long intensity;

        [TestInitialize]
        public void BuildUp()
        {
            rfdata = new RFData();

            id = 0;
            timeCaptured = new DateTime();
            intensity = 0;
        }

        [TestMethod]
        public void TestSettersAndGetters()
        {
            id = 512;
            timeCaptured = new DateTime(2018, 6, 10);
            intensity = 123456789;

            rfdata.Id = id;
            rfdata.TimeCaptured = timeCaptured;
            rfdata.Intensity = intensity;

            Assert.AreEqual(id, rfdata.Id);
            Assert.AreEqual(timeCaptured, rfdata.TimeCaptured);
            Assert.AreEqual(intensity, rfdata.Intensity);
        }
    }
}