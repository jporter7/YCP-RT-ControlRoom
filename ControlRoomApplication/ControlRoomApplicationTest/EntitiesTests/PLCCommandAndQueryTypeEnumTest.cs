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
    class PLCCommandAndQueryTypeEnumTest
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
            byte output = PLCCommandAndQueryTypeConversionHelper.ConvertToByte(_input_undefined);
            Assert.AreEqual(0x0, _input_undefined);
        }
    }
}
