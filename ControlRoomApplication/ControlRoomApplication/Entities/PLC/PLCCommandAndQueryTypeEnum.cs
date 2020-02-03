using System;

namespace ControlRoomApplication.Entities
{
    public enum PLCCommandAndQueryTypeEnum : byte
    {
        UNDEFINED = 0x0,
        TEST_CONNECTION = 0x1,
        GET_CURRENT_AZEL_POSITIONS = 0x2,// remove, encoders not handled by plc anymore
        GET_CURRENT_LIMIT_SWITCH_STATUSES = 0x3,
        GET_CURRENT_SAFETY_INTERLOCK_STATUS = 0x4,
        CANCEL_ACTIVE_OBJECTIVE_AZEL_POSITION = 0x5,
        SHUTDOWN = 0x6,
        THERMAL_CALIBRATE = 0x7,
        SET_CONFIGURATION = 0x8,
        CONTROLLED_STOP = 0x9,
        IMMEDIATE_STOP= 0xA,
        SET_OBJECTIVE_AZEL_POSITION = 0xB,
        START_JOG_MOVEMENT = 0xC,
        TRANSLATE_AZEL_POSITION = 0xD
    }

    public class PLCCommandAndQueryTypeConversionHelper
    {
        public static PLCCommandAndQueryTypeEnum GetFromByte(byte input)
        {
            if (!Enum.IsDefined(typeof(PLCCommandAndQueryTypeEnum), input))
            {
                return PLCCommandAndQueryTypeEnum.UNDEFINED;
            }
            else
            {
                return (PLCCommandAndQueryTypeEnum)input;
            }
        }

        public static byte ConvertToByte(PLCCommandAndQueryTypeEnum input)
        {
            if (!Enum.IsDefined(typeof(PLCCommandAndQueryTypeEnum), input))
            {
                return ConvertToByte(PLCCommandAndQueryTypeEnum.UNDEFINED);
            }
            else
            {
                return (byte)input;
            }
        }
    }


    /// <summary>
    /// the cctrlroom has a modbus server running on it that is maped to input and output regs on the plc 
    /// 0-20 arr comand bytes that go through the plc to the mcu
    /// warnings, limits and other boolean values are sent as ints with 0 for false and 1 for true
    /// </summary>
    /// <remarks>
    /// the registers in the PLC start at 0 however the registers in the controll room start at one 
    /// so an offset between the register number in the PLC and the number in the controll room is required
    /// </remarks>
    public enum PLC_modbus_server_register_mapping : ushort
    {
        /// <summary>
        /// limit at -10
        /// </summary>
        AZ_0_LIMIT  = 8 + 1,
        /// <summary>
        /// limit at 375 degrees
        /// </summary>
        AZ_375_LIMIT = 9 + 1,


        
        /// <summary>
        /// home sensor at 0 degrees,
        /// will be active between -15 and 0
        /// </summary>
        AZ_0_HOME = 10 + 1,
        /// <summary>
        /// SECONDARY home sensor at 0 degrees,
        /// will be active between -1 and 15
        /// </summary>
        AZ_0_SECONDARY = 11 + 1,


        /// <summary>
        /// limit switch at -10
        /// </summary>
        /// <remarks>
        /// im not 100% certian that the telesope will actually have this switch
        /// </remarks>
        EL_10_LIMIT = 12 + 1,
        /// <summary>
        /// active between 0 and -15 degrees
        /// </summary>
        EL_0_HOME = 13 + 1,
        /// <summary>
        /// limit at 90 degrees
        /// </summary>
        EL_90_LIMIT = 14 + 1,

        /// <summary>
        /// emergency stop the plc will handle disabling the motors and MCU, the controll should re-configure (and posibly re-home) the MCU after this is ended
        /// </summary>
        E_STOP = 15 + 1,
        /// <summary>
        /// high when gate is open
        /// </summary>
        Safety_INTERLOCK = 16 + 1,

        /// <summary>
        /// high when the MCU has a fault on the AZ axsis
        /// </summary>
        AZ_MCU_FAULT =17+1,
        /// <summary>
        /// high when the MCU has a fault on the AZ axsis
        /// </summary>
        EL_MCU_FAULT = 18+1,
        /// <summary>
        /// high when the azimuth motor has a fault
        /// </summary>
        AZ_MTR_DRIVER_FAULT = 17 + 1,
        /// <summary>
        /// high when the ELEVATION motor has a fault
        /// </summary>
        EL_MTR_DRIVER_FAULT = 18 + 1,

        AZ_MOTOR_CURRENT = 40 + 1,
        EL_MOTOR_CURRENT = 42 + 1
    }


}
