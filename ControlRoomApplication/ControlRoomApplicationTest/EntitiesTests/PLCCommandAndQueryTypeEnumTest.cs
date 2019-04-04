using Microsoft.VisualStudio.TestTools.UnitTesting;
using ControlRoomApplication.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControlRoomApplicationTest.EntitiesTests
{
    [TestClass]   
    public class PLCCommandAndQueryTypeEnumTest
    {
        private PLCCommandAndQueryTypeEnum _input_undefined;
        private PLCCommandAndQueryTypeEnum _input_test_connection;
        private PLCCommandAndQueryTypeEnum _input_get_current_azel_positions;
        private PLCCommandAndQueryTypeEnum _input_get_current_limit_switch_statuses;
        private PLCCommandAndQueryTypeEnum _input_get_current_safety_interlock_status;
        private PLCCommandAndQueryTypeEnum _input_cancel_active_objective_azel_position;
        private PLCCommandAndQueryTypeEnum _input_shutdown;
        private PLCCommandAndQueryTypeEnum _input_calibrate;
        private PLCCommandAndQueryTypeEnum _input_set_objective_azel_position;

        [TestInitialize]
        public void BuildUp()
        {
            _input_undefined = PLCCommandAndQueryTypeEnum.UNDEFINED;
            _input_test_connection = PLCCommandAndQueryTypeEnum.TEST_CONNECTION;
            _input_get_current_azel_positions = PLCCommandAndQueryTypeEnum.GET_CURRENT_AZEL_POSITIONS;
            _input_get_current_limit_switch_statuses = PLCCommandAndQueryTypeEnum.GET_CURRENT_LIMIT_SWITCH_STATUSES;
            _input_get_current_safety_interlock_status = PLCCommandAndQueryTypeEnum.GET_CURRENT_SAFETY_INTERLOCK_STATUS;
            _input_cancel_active_objective_azel_position = PLCCommandAndQueryTypeEnum.CANCEL_ACTIVE_OBJECTIVE_AZEL_POSITION;
            _input_shutdown = PLCCommandAndQueryTypeEnum.SHUTDOWN;
            _input_calibrate = PLCCommandAndQueryTypeEnum.CALIBRATE;
            _input_set_objective_azel_position = PLCCommandAndQueryTypeEnum.SET_OBJECTIVE_AZEL_POSITION;            
        }

        [TestMethod]
        public void TestPLCCommandAndQueryTypeEnumHelper()
        {
            Assert.AreEqual(0x0, PLCCommandAndQueryTypeConversionHelper.ConvertToByte(_input_undefined));
            Assert.AreEqual(PLCCommandAndQueryTypeEnum.UNDEFINED, PLCCommandAndQueryTypeConversionHelper.GetFromByte(0x0));

            Assert.AreEqual(0x1, PLCCommandAndQueryTypeConversionHelper.ConvertToByte(_input_test_connection));
            Assert.AreEqual(PLCCommandAndQueryTypeEnum.TEST_CONNECTION, PLCCommandAndQueryTypeConversionHelper.GetFromByte(0x1));

            Assert.AreEqual(0x2, PLCCommandAndQueryTypeConversionHelper.ConvertToByte(_input_get_current_azel_positions));
            Assert.AreEqual(PLCCommandAndQueryTypeEnum.GET_CURRENT_AZEL_POSITIONS, PLCCommandAndQueryTypeConversionHelper.GetFromByte(0x2));

            Assert.AreEqual(0x3, PLCCommandAndQueryTypeConversionHelper.ConvertToByte(_input_get_current_limit_switch_statuses));
            Assert.AreEqual(PLCCommandAndQueryTypeEnum.GET_CURRENT_LIMIT_SWITCH_STATUSES, PLCCommandAndQueryTypeConversionHelper.GetFromByte(0x3));

            Assert.AreEqual(0x4, PLCCommandAndQueryTypeConversionHelper.ConvertToByte(_input_get_current_safety_interlock_status));
            Assert.AreEqual(PLCCommandAndQueryTypeEnum.GET_CURRENT_SAFETY_INTERLOCK_STATUS, PLCCommandAndQueryTypeConversionHelper.GetFromByte(0x4));

            Assert.AreEqual(0x5, PLCCommandAndQueryTypeConversionHelper.ConvertToByte(_input_cancel_active_objective_azel_position));
            Assert.AreEqual(PLCCommandAndQueryTypeEnum.CANCEL_ACTIVE_OBJECTIVE_AZEL_POSITION, PLCCommandAndQueryTypeConversionHelper.GetFromByte(0x5));

            Assert.AreEqual(0x6, PLCCommandAndQueryTypeConversionHelper.ConvertToByte(_input_shutdown));
            Assert.AreEqual(PLCCommandAndQueryTypeEnum.SHUTDOWN, PLCCommandAndQueryTypeConversionHelper.GetFromByte(0x6));

            Assert.AreEqual(0x7, PLCCommandAndQueryTypeConversionHelper.ConvertToByte(_input_calibrate));
            Assert.AreEqual(PLCCommandAndQueryTypeEnum.CALIBRATE, PLCCommandAndQueryTypeConversionHelper.GetFromByte(0x7));

            Assert.AreEqual(0x8, PLCCommandAndQueryTypeConversionHelper.ConvertToByte(_input_set_objective_azel_position));
            Assert.AreEqual(PLCCommandAndQueryTypeEnum.SET_OBJECTIVE_AZEL_POSITION, PLCCommandAndQueryTypeConversionHelper.GetFromByte(0x8));
        }
    }
}
