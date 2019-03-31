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
            Assert.AreEqual(0x0, PLCCommandResponseExpectationConversionHelper.ConvertToByte(PLCCommandResponseExpectationEnum.UNDEFINED));
            Assert.AreEqual(0x1, PLCCommandResponseExpectationConversionHelper.ConvertToByte(PLCCommandResponseExpectationEnum.MINOR_RESPONSE));
            Assert.AreEqual(0x2, PLCCommandResponseExpectationConversionHelper.ConvertToByte(PLCCommandResponseExpectationEnum.FULL_RESPONSE));
        }

        [TestMethod]
        public void TestByteToEnum()
        {
            Assert.AreEqual(PLCCommandResponseExpectationEnum.UNDEFINED, PLCCommandResponseExpectationConversionHelper.GetFromByte(0x0));
            Assert.AreEqual(PLCCommandResponseExpectationEnum.MINOR_RESPONSE, PLCCommandResponseExpectationConversionHelper.GetFromByte(0x1));
            Assert.AreEqual(PLCCommandResponseExpectationEnum.FULL_RESPONSE, PLCCommandResponseExpectationConversionHelper.GetFromByte(0x2));
            Assert.AreEqual(PLCCommandResponseExpectationEnum.UNDEFINED, PLCCommandResponseExpectationConversionHelper.GetFromByte(0x3));
            Assert.AreEqual(PLCCommandResponseExpectationEnum.UNDEFINED, PLCCommandResponseExpectationConversionHelper.GetFromByte(0xFF));
        }
    }
}