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
        /// <item><description><para><em>OUT</em></para></description></item>
        /// the PLC will not honor any requests to override the limits,
        /// this is only a formality and testing
        /// </summary>
        LIMIT_OVERRIDE = 0+1,
        /// <summary>
        /// <item><description><para><em>OUT</em></para></description></item>
        /// when this is set the elescope will still move even with the gate open
        /// </summary>
        GATE_OVERRIDE = 1+1,
        /// <summary>
        /// <item><description><para><em>OUT</em></para></description></item>
        /// the PLC will never honor this request under any circumstances,
        /// this mainly serves as a place holder
        /// </summary>
        E_STOP_OVERRIDE = 2+1,


        /// <summary>
        /// <item><description><para><em>IN</em></para></description></item>
        /// limit at -10
        /// </summary>
        AZ_0_LIMIT = 8 + 1,
        /// <summary>
        /// <item><description><para><em>IN</em></para></description></item>
        /// limit at 375 degrees
        /// </summary>
        AZ_375_LIMIT = 9 + 1,
        /// <summary>
        /// <item><description><para><em>IN</em></para></description></item>
        /// home sensor at 0 degrees,
        /// will be active between -15 and 0
        /// </summary>
        AZ_0_HOME = 10 + 1,
        /// <summary>
        /// <item><description><para><em>IN</em></para></description></item>
        /// SECONDARY home sensor at 0 degrees,
        /// will be active between -1 and 15
        /// </summary>
        AZ_0_SECONDARY = 11 + 1,


        /// <summary>
        /// <item><description><para><em>IN</em></para></description></item>
        /// limit switch at -10
        /// </summary>
        /// <remarks>
        /// im not 100% certian that the telesope will actually have this switch
        /// </remarks>
        EL_10_LIMIT = 12 + 1,
        /// <summary>
        /// <item><description><para><em>IN</em></para></description></item>
        /// active between 0 and -15 degrees
        /// </summary>
        EL_0_HOME = 13 + 1,
        /// <summary>
        /// <item><description><para><em>IN</em></para></description></item>
        /// limit at 90 degrees
        /// </summary>
        EL_90_LIMIT = 14 + 1,



        /// <summary>
        /// <item><description><para><em>IN</em></para></description></item>
        /// emergency stop the plc will handle disabling the motors and MCU, the controll should re-configure (and posibly re-home) the MCU after this is ended
        /// </summary>
        E_STOP = 15 + 1,
        /// <summary>
        /// <item><description><para><em>IN</em></para></description></item>
        /// high when gate is open
        /// </summary>
        Safety_INTERLOCK = 16 + 1,

        /// <summary>
        /// <item><description><para><em>IN</em></para></description></item>
        /// high when the MCU has a fault on the AZ axsis
        /// </summary>
        AZ_MCU_FAULT = 17+1,
        /// <summary>
        /// <item><description><para><em>IN</em></para></description></item>
        /// high when the MCU has a fault on the AZ axsis
        /// </summary>
        EL_MCU_FAULT = 18+1,
        /// <summary>
        /// <item><description><para><em>IN</em></para></description></item>
        /// high when the azimuth motor has a fault
        /// </summary>
        AZ_MTR_DRIVER_FAULT = 19 + 1,
        /// <summary>
        /// <item><description><para><em>IN</em></para></description></item>
        /// high when the ELEVATION motor has a fault
        /// </summary>
        EL_MTR_DRIVER_FAULT = 20 + 1,


        /// <summary>
        /// not currerntly used by the PLC
        /// </summary>
        AZ_MOTOR_CURRENT = 40 + 1,
        /// <summary>
        /// not currerntly used by the PLC
        /// </summary>
        EL_MOTOR_CURRENT = 42 + 1
    }


}
