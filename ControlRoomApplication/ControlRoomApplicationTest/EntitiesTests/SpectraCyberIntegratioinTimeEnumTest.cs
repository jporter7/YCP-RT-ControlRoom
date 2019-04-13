using Microsoft.VisualStudio.TestTools.UnitTesting;
using ControlRoomApplication.Entities;

namespace ControlRoomApplicationTest.EntitiesTests
{
    [TestClass]
    public class SpectraCyberIntegratioinTimeEnumTest
    {
        private SpectraCyberIntegrationTimeEnum _time_1;
        private SpectraCyberIntegrationTimeEnum _time_2;
        private SpectraCyberIntegrationTimeEnum _time_3;

        [TestInitialize]
        public void BuildUp()
        {
            _time_1 = SpectraCyberIntegrationTimeEnum.SHORT_TIME_SPAN;
            _time_2 = SpectraCyberIntegrationTimeEnum.MID_TIME_SPAN;
            _time_3 = SpectraCyberIntegrationTimeEnum.LONG_TIME_SPAN;
        }

        [TestMethod]
        public void TestSpectraCyberIntegratioinTimeEnumHelper()
        {
            Assert.AreEqual('0', SpectraCyberIntegrationTimeEnumHelper.GetValue(_time_1));
            Assert.AreEqual('1', SpectraCyberIntegrationTimeEnumHelper.GetValue(_time_2));
            Assert.AreEqual('2', SpectraCyberIntegrationTimeEnumHelper.GetValue(_time_3));
        }
    }
}
