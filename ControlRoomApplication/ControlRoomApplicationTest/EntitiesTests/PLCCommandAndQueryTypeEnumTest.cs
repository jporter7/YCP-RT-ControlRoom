using Microsoft.VisualStudio.TestTools.UnitTesting;
using ControlRoomApplication.Entities;

namespace ControlRoomApplicationTest.EntitiesTests
{
    [TestClass]   
    public class PLCCommandAndQueryTypeEnumTest
    {
        private HardwareMessageTypeEnum _input_undefined;
        private HardwareMessageTypeEnum _input_test_connection;
        private HardwareMessageTypeEnum _input_get_current_azel_positions;
        private HardwareMessageTypeEnum _input_get_current_limit_switch_statuses;
        private HardwareMessageTypeEnum _input_get_current_safety_interlock_status;
        private HardwareMessageTypeEnum _input_cancel_active_objective_azel_position;
        private HardwareMessageTypeEnum _input_shutdown;
        private HardwareMessageTypeEnum _input_calibrate;
        private HardwareMessageTypeEnum _input_set_objective_azel_position;

        [TestInitialize]
        public void BuildUp()
        {
            _input_undefined = HardwareMessageTypeEnum.UNDEFINED;
            _input_test_connection = HardwareMessageTypeEnum.TEST_CONNECTION;
            _input_get_current_azel_positions = HardwareMessageTypeEnum.GET_CURRENT_AZEL_POSITIONS;
            _input_get_current_limit_switch_statuses = HardwareMessageTypeEnum.GET_CURRENT_LIMIT_SWITCH_STATUSES;
            _input_get_current_safety_interlock_status = HardwareMessageTypeEnum.GET_CURRENT_SAFETY_INTERLOCK_STATUS;
            _input_cancel_active_objective_azel_position = HardwareMessageTypeEnum.CANCEL_ACTIVE_OBJECTIVE_AZEL_POSITION;
            _input_shutdown = HardwareMessageTypeEnum.SHUTDOWN;
            _input_calibrate = HardwareMessageTypeEnum.CALIBRATE;
            _input_set_objective_azel_position = HardwareMessageTypeEnum.SET_OBJECTIVE_AZEL_POSITION;            
        }

        [TestMethod]
        public void TestPLCCommandAndQueryTypeEnumHelper()
        {
            Assert.AreEqual(0x0, HardwareMessageTypeEnumConversionHelper.ConvertToByte(_input_undefined));
            Assert.AreEqual(HardwareMessageTypeEnum.UNDEFINED, HardwareMessageTypeEnumConversionHelper.GetFromByte(0x0));

            Assert.AreEqual(0x1, HardwareMessageTypeEnumConversionHelper.ConvertToByte(_input_test_connection));
            Assert.AreEqual(HardwareMessageTypeEnum.TEST_CONNECTION, HardwareMessageTypeEnumConversionHelper.GetFromByte(0x1));

            Assert.AreEqual(0x2, HardwareMessageTypeEnumConversionHelper.ConvertToByte(_input_get_current_azel_positions));
            Assert.AreEqual(HardwareMessageTypeEnum.GET_CURRENT_AZEL_POSITIONS, HardwareMessageTypeEnumConversionHelper.GetFromByte(0x2));

            Assert.AreEqual(0x3, HardwareMessageTypeEnumConversionHelper.ConvertToByte(_input_get_current_limit_switch_statuses));
            Assert.AreEqual(HardwareMessageTypeEnum.GET_CURRENT_LIMIT_SWITCH_STATUSES, HardwareMessageTypeEnumConversionHelper.GetFromByte(0x3));

            Assert.AreEqual(0x4, HardwareMessageTypeEnumConversionHelper.ConvertToByte(_input_get_current_safety_interlock_status));
            Assert.AreEqual(HardwareMessageTypeEnum.GET_CURRENT_SAFETY_INTERLOCK_STATUS, HardwareMessageTypeEnumConversionHelper.GetFromByte(0x4));

            Assert.AreEqual(0x5, HardwareMessageTypeEnumConversionHelper.ConvertToByte(_input_cancel_active_objective_azel_position));
            Assert.AreEqual(HardwareMessageTypeEnum.CANCEL_ACTIVE_OBJECTIVE_AZEL_POSITION, HardwareMessageTypeEnumConversionHelper.GetFromByte(0x5));

            Assert.AreEqual(0x6, HardwareMessageTypeEnumConversionHelper.ConvertToByte(_input_shutdown));
            Assert.AreEqual(HardwareMessageTypeEnum.SHUTDOWN, HardwareMessageTypeEnumConversionHelper.GetFromByte(0x6));

            Assert.AreEqual(0x7, HardwareMessageTypeEnumConversionHelper.ConvertToByte(_input_calibrate));
            Assert.AreEqual(HardwareMessageTypeEnum.CALIBRATE, HardwareMessageTypeEnumConversionHelper.GetFromByte(0x7));

            Assert.AreEqual(0x8, HardwareMessageTypeEnumConversionHelper.ConvertToByte(_input_set_objective_azel_position));
            Assert.AreEqual(HardwareMessageTypeEnum.SET_OBJECTIVE_AZEL_POSITION, HardwareMessageTypeEnumConversionHelper.GetFromByte(0x8));
        }
    }
}
