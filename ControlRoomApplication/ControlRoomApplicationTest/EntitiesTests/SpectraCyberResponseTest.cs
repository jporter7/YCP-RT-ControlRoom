using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ControlRoomApplication.Entities;

namespace ControlRoomApplicationTest.EntitiesTests
{
    [TestClass]
    public class SpectraCyberResponseTest
    {
        private SpectraCyberResponse SCResponse;

        [TestInitialize]
        public void Init()
        {
            SCResponse = new SpectraCyberResponse();
        }

        [TestMethod]
        public void TestConstructorAndProperties()
        {
            Assert.AreEqual(false, SCResponse.RequestSuccessful);
            Assert.AreEqual(false, SCResponse.Valid);
            Assert.AreEqual(0, SCResponse.SerialIdentifier);
            Assert.AreEqual(0, SCResponse.DecimalData);
            Assert.AreEqual(DateTime.MinValue, SCResponse.DateTimeCaptured);
        }

        [TestMethod]
        public void TestSetters()
        {
            SCResponse.RequestSuccessful = true;
            Assert.AreEqual(true, SCResponse.RequestSuccessful);

            SCResponse.Valid = true;
            Assert.AreEqual(true, SCResponse.Valid);

            SCResponse.SerialIdentifier = 'a';
            Assert.AreEqual('a', SCResponse.SerialIdentifier);

            SCResponse.DecimalData = 42;
            Assert.AreEqual(42, SCResponse.DecimalData);

            DateTime DayOfWriting = new DateTime(2019, 3, 27);
            SCResponse.DateTimeCaptured = DayOfWriting;
            Assert.AreEqual(DayOfWriting, SCResponse.DateTimeCaptured);
        }
    }
}
