using ControlRoomApplication.Entities;
using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ControlRoomApplicationTest.EntitiesTests
{
    [TestClass]
    public class PLCTest
    {
        public void PLCRadioTelescopeStatusConversionHelperTest() {
            //checking GetFromByte returns
            Assert.AreEqual(RadioTelescopeStatusEnum.UNDEFINED, RadioTelescopeStatusConversionHelper.GetFromByte(0x0));
            Assert.AreEqual(RadioTelescopeStatusEnum.IN_BRINGUP_SEQUENCE, RadioTelescopeStatusConversionHelper.GetFromByte(0x1));
            Assert.AreEqual(RadioTelescopeStatusEnum.ONLINE_AND_IDLE, RadioTelescopeStatusConversionHelper.GetFromByte(0x2));
            Assert.AreEqual(RadioTelescopeStatusEnum.ONLINE_AND_MOVING, RadioTelescopeStatusConversionHelper.GetFromByte(0x3));
            Assert.AreEqual(RadioTelescopeStatusEnum.ONLINE_AND_CANCELLING_ACTIVE_MOVE, RadioTelescopeStatusConversionHelper.GetFromByte(0x4));
            Assert.AreEqual(RadioTelescopeStatusEnum.IN_SHUTDOWN_SEQUENCE, RadioTelescopeStatusConversionHelper.GetFromByte(0x5));
            Assert.AreEqual(RadioTelescopeStatusEnum.EMERGENCY_STOPPED_BRB, RadioTelescopeStatusConversionHelper.GetFromByte(0x6));
            Assert.AreEqual(RadioTelescopeStatusEnum.EMERGENCY_STOPPED_SAFETY_INTERLOCK, RadioTelescopeStatusConversionHelper.GetFromByte(0x7));
            Assert.AreEqual(RadioTelescopeStatusEnum.EMERGENCY_STOPPED_CAMERA, RadioTelescopeStatusConversionHelper.GetFromByte(0x8));
            Assert.AreEqual(RadioTelescopeStatusEnum.MISCELLANEOUS_ERROR, RadioTelescopeStatusConversionHelper.GetFromByte(0xff));

            //ConvertToByte
            Assert.AreEqual(0x0, RadioTelescopeStatusConversionHelper.ConvertToByte(RadioTelescopeStatusEnum.UNDEFINED));
            Assert.AreEqual(0x1, RadioTelescopeStatusConversionHelper.ConvertToByte(RadioTelescopeStatusEnum.IN_BRINGUP_SEQUENCE));
            Assert.AreEqual(0x2, RadioTelescopeStatusConversionHelper.ConvertToByte(RadioTelescopeStatusEnum.ONLINE_AND_IDLE));
            Assert.AreEqual(0x3, RadioTelescopeStatusConversionHelper.ConvertToByte(RadioTelescopeStatusEnum.ONLINE_AND_MOVING));
            Assert.AreEqual(0x4, RadioTelescopeStatusConversionHelper.ConvertToByte(RadioTelescopeStatusEnum.ONLINE_AND_CANCELLING_ACTIVE_MOVE));
            Assert.AreEqual(0x5, RadioTelescopeStatusConversionHelper.ConvertToByte(RadioTelescopeStatusEnum.IN_SHUTDOWN_SEQUENCE));
            Assert.AreEqual(0x6, RadioTelescopeStatusConversionHelper.ConvertToByte(RadioTelescopeStatusEnum.EMERGENCY_STOPPED_BRB));
            Assert.AreEqual(0x7, RadioTelescopeStatusConversionHelper.ConvertToByte(RadioTelescopeStatusEnum.EMERGENCY_STOPPED_SAFETY_INTERLOCK));
            Assert.AreEqual(0x8, RadioTelescopeStatusConversionHelper.ConvertToByte(RadioTelescopeStatusEnum.EMERGENCY_STOPPED_CAMERA));
            Assert.AreEqual(0xff, RadioTelescopeStatusConversionHelper.ConvertToByte(RadioTelescopeStatusEnum.MISCELLANEOUS_ERROR));

        }

        public void PLCSafetyInterlockStatusEnumTest() {
            //GetFromByte Tests
            Assert.AreEqual(SafetyInterlockStatusEnum.UNDEFINED, SafetyInterlockStatusConversionHelper.GetFromByte(0x0));
            Assert.AreEqual(SafetyInterlockStatusEnum.LOCKED, SafetyInterlockStatusConversionHelper.GetFromByte(0x1));
            Assert.AreEqual(SafetyInterlockStatusEnum.UNLOCKED, SafetyInterlockStatusConversionHelper.GetFromByte(0x2));

            //ConvertToByte Tests
            Assert.AreEqual(0x0, SafetyInterlockStatusConversionHelper.ConvertToByte(SafetyInterlockStatusEnum.UNDEFINED));
            Assert.AreEqual(0x1, SafetyInterlockStatusConversionHelper.ConvertToByte(SafetyInterlockStatusEnum.LOCKED));
            Assert.AreEqual(0x2, SafetyInterlockStatusConversionHelper.ConvertToByte(SafetyInterlockStatusEnum.UNLOCKED));
        }

        public void PLCLimitSwitchStatusEnumTest()
        {
            //GetFromByte Tests
            Assert.AreEqual(LimitSwitchStatusEnum.UNDEFINED, LimitSwitchStatusConversionHelper.GetFromByte(0x0));
            Assert.AreEqual(LimitSwitchStatusEnum.WITHIN_SAFE_LIMITS, LimitSwitchStatusConversionHelper.GetFromByte(0x1));
            Assert.AreEqual(LimitSwitchStatusEnum.WITHIN_WARNING_LIMITS, LimitSwitchStatusConversionHelper.GetFromByte(0x2));

            //ConvertToByte Tests
            Assert.AreEqual(0x0, LimitSwitchStatusConversionHelper.ConvertToByte(LimitSwitchStatusEnum.UNDEFINED));
            Assert.AreEqual(0x1, LimitSwitchStatusConversionHelper.ConvertToByte(LimitSwitchStatusEnum.WITHIN_SAFE_LIMITS));
            Assert.AreEqual(0x2, LimitSwitchStatusConversionHelper.ConvertToByte(LimitSwitchStatusEnum.WITHIN_WARNING_LIMITS));
        }

    }
}
