using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ControlRoomApplication.Entities;

namespace ControlRoomApplicationTest.EntitiesTests
{
    [TestClass]
    public class PLCCommandResponseExpectationEnumTest
    {
        [TestMethod]
        public void TestEnumToByte()
        {
            Assert.AreEqual(0x0, HardwareMessageResponseExpectationConversionHelper.ConvertToByte(HardwareMesageResponseExpectationEnum.UNDEFINED));
            Assert.AreEqual(0x1, HardwareMessageResponseExpectationConversionHelper.ConvertToByte(HardwareMesageResponseExpectationEnum.MINOR_RESPONSE));
            Assert.AreEqual(0x2, HardwareMessageResponseExpectationConversionHelper.ConvertToByte(HardwareMesageResponseExpectationEnum.FULL_RESPONSE));
        }

        [TestMethod]
        public void TestByteToEnum()
        {
            Assert.AreEqual(HardwareMesageResponseExpectationEnum.UNDEFINED, HardwareMessageResponseExpectationConversionHelper.GetFromByte(0x0));
            Assert.AreEqual(HardwareMesageResponseExpectationEnum.MINOR_RESPONSE, HardwareMessageResponseExpectationConversionHelper.GetFromByte(0x1));
            Assert.AreEqual(HardwareMesageResponseExpectationEnum.FULL_RESPONSE, HardwareMessageResponseExpectationConversionHelper.GetFromByte(0x2));
            Assert.AreEqual(HardwareMesageResponseExpectationEnum.UNDEFINED, HardwareMessageResponseExpectationConversionHelper.GetFromByte(0x3));
            Assert.AreEqual(HardwareMesageResponseExpectationEnum.UNDEFINED, HardwareMessageResponseExpectationConversionHelper.GetFromByte(0xFF));
        }
    }
}