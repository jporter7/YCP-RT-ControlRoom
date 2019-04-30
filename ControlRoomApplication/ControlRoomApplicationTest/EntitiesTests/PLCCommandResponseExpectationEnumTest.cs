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
            Assert.AreEqual(0x0, HardwareMessageResponseExpectationConversionHelper.ConvertToByte(HardwareMessageResponseExpectationEnum.UNDEFINED));
            Assert.AreEqual(0x1, HardwareMessageResponseExpectationConversionHelper.ConvertToByte(HardwareMessageResponseExpectationEnum.MINOR_RESPONSE));
            Assert.AreEqual(0x2, HardwareMessageResponseExpectationConversionHelper.ConvertToByte(HardwareMessageResponseExpectationEnum.FULL_RESPONSE));
        }

        [TestMethod]
        public void TestByteToEnum()
        {
            Assert.AreEqual(HardwareMessageResponseExpectationEnum.UNDEFINED, HardwareMessageResponseExpectationConversionHelper.GetFromByte(0x0));
            Assert.AreEqual(HardwareMessageResponseExpectationEnum.MINOR_RESPONSE, HardwareMessageResponseExpectationConversionHelper.GetFromByte(0x1));
            Assert.AreEqual(HardwareMessageResponseExpectationEnum.FULL_RESPONSE, HardwareMessageResponseExpectationConversionHelper.GetFromByte(0x2));
            Assert.AreEqual(HardwareMessageResponseExpectationEnum.UNDEFINED, HardwareMessageResponseExpectationConversionHelper.GetFromByte(0x3));
            Assert.AreEqual(HardwareMessageResponseExpectationEnum.UNDEFINED, HardwareMessageResponseExpectationConversionHelper.GetFromByte(0xFF));
        }
    }
}