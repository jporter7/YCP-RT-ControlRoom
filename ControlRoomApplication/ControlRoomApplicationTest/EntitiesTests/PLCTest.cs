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
            Assert.AreEqual(PLCRadioTelescopeStatusEnum.UNDEFINED, PLCRadioTelescopeStatusConversionHelper.GetFromByte(0x0));
            Assert.AreEqual(PLCRadioTelescopeStatusEnum.IN_BRINGUP_SEQUENCE, PLCRadioTelescopeStatusConversionHelper.GetFromByte(0x1));
            Assert.AreEqual(PLCRadioTelescopeStatusEnum.ONLINE_AND_IDLE, PLCRadioTelescopeStatusConversionHelper.GetFromByte(0x2));
            Assert.AreEqual(PLCRadioTelescopeStatusEnum.ONLINE_AND_MOVING, PLCRadioTelescopeStatusConversionHelper.GetFromByte(0x3));
            Assert.AreEqual(PLCRadioTelescopeStatusEnum.ONLINE_AND_CANCELLING_ACTIVE_MOVE, PLCRadioTelescopeStatusConversionHelper.GetFromByte(0x4));
            Assert.AreEqual(PLCRadioTelescopeStatusEnum.IN_SHUTDOWN_SEQUENCE, PLCRadioTelescopeStatusConversionHelper.GetFromByte(0x5));
            Assert.AreEqual(PLCRadioTelescopeStatusEnum.EMERGENCY_STOPPED_BRB, PLCRadioTelescopeStatusConversionHelper.GetFromByte(0x6));
            Assert.AreEqual(PLCRadioTelescopeStatusEnum.EMERGENCY_STOPPED_SAFETY_INTERLOCK, PLCRadioTelescopeStatusConversionHelper.GetFromByte(0x7));
            Assert.AreEqual(PLCRadioTelescopeStatusEnum.EMERGENCY_STOPPED_CAMERA, PLCRadioTelescopeStatusConversionHelper.GetFromByte(0x8));
            Assert.AreEqual(PLCRadioTelescopeStatusEnum.MISCELLANEOUS_ERROR, PLCRadioTelescopeStatusConversionHelper.GetFromByte(0xff));

            //ConvertToByte
            Assert.AreEqual(0x0, PLCRadioTelescopeStatusConversionHelper.ConvertToByte(PLCRadioTelescopeStatusEnum.UNDEFINED));
            Assert.AreEqual(0x1, PLCRadioTelescopeStatusConversionHelper.ConvertToByte(PLCRadioTelescopeStatusEnum.IN_BRINGUP_SEQUENCE));
            Assert.AreEqual(0x2, PLCRadioTelescopeStatusConversionHelper.ConvertToByte(PLCRadioTelescopeStatusEnum.ONLINE_AND_IDLE));
            Assert.AreEqual(0x3, PLCRadioTelescopeStatusConversionHelper.ConvertToByte(PLCRadioTelescopeStatusEnum.ONLINE_AND_MOVING));
            Assert.AreEqual(0x4, PLCRadioTelescopeStatusConversionHelper.ConvertToByte(PLCRadioTelescopeStatusEnum.ONLINE_AND_CANCELLING_ACTIVE_MOVE));
            Assert.AreEqual(0x5, PLCRadioTelescopeStatusConversionHelper.ConvertToByte(PLCRadioTelescopeStatusEnum.IN_SHUTDOWN_SEQUENCE));
            Assert.AreEqual(0x6, PLCRadioTelescopeStatusConversionHelper.ConvertToByte(PLCRadioTelescopeStatusEnum.EMERGENCY_STOPPED_BRB));
            Assert.AreEqual(0x7, PLCRadioTelescopeStatusConversionHelper.ConvertToByte(PLCRadioTelescopeStatusEnum.EMERGENCY_STOPPED_SAFETY_INTERLOCK));
            Assert.AreEqual(0x8, PLCRadioTelescopeStatusConversionHelper.ConvertToByte(PLCRadioTelescopeStatusEnum.EMERGENCY_STOPPED_CAMERA));
            Assert.AreEqual(0xff, PLCRadioTelescopeStatusConversionHelper.ConvertToByte(PLCRadioTelescopeStatusEnum.MISCELLANEOUS_ERROR));

        }

        public void PLCSafetyInterlockStatusEnumTest() {
            //GetFromByte Tests
            Assert.AreEqual(PLCSafetyInterlockStatusEnum.UNDEFINED, PLCSafetyInterlockStatusConversionHelper.GetFromByte(0x0));
            Assert.AreEqual(PLCSafetyInterlockStatusEnum.LOCKED, PLCSafetyInterlockStatusConversionHelper.GetFromByte(0x1));
            Assert.AreEqual(PLCSafetyInterlockStatusEnum.UNLOCKED, PLCSafetyInterlockStatusConversionHelper.GetFromByte(0x2));

            //ConvertToByte Tests
            Assert.AreEqual(0x0, PLCSafetyInterlockStatusConversionHelper.ConvertToByte(PLCSafetyInterlockStatusEnum.UNDEFINED));
            Assert.AreEqual(0x1, PLCSafetyInterlockStatusConversionHelper.ConvertToByte(PLCSafetyInterlockStatusEnum.LOCKED));
            Assert.AreEqual(0x2, PLCSafetyInterlockStatusConversionHelper.ConvertToByte(PLCSafetyInterlockStatusEnum.UNLOCKED));
        }

        public void PLCLimitSwitchStatusEnumTest()
        {
            //GetFromByte Tests
            Assert.AreEqual(PLCLimitSwitchStatusEnum.UNDEFINED, PLCLimitSwitchStatusConversionHelper.GetFromByte(0x0));
            Assert.AreEqual(PLCLimitSwitchStatusEnum.WITHIN_SAFE_LIMITS, PLCLimitSwitchStatusConversionHelper.GetFromByte(0x1));
            Assert.AreEqual(PLCLimitSwitchStatusEnum.WITHIN_WARNING_LIMITS, PLCLimitSwitchStatusConversionHelper.GetFromByte(0x2));

            //ConvertToByte Tests
            Assert.AreEqual(0x0, PLCLimitSwitchStatusConversionHelper.ConvertToByte(PLCLimitSwitchStatusEnum.UNDEFINED));
            Assert.AreEqual(0x1, PLCLimitSwitchStatusConversionHelper.ConvertToByte(PLCLimitSwitchStatusEnum.WITHIN_SAFE_LIMITS));
            Assert.AreEqual(0x2, PLCLimitSwitchStatusConversionHelper.ConvertToByte(PLCLimitSwitchStatusEnum.WITHIN_WARNING_LIMITS));
        }

    }
}
