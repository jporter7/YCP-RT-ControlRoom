using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControlRoomApplication.Entities.Configuration {
    public class MCUConfiguration {
        /// <summary>
        /// motor acceleration in steps/millisecond/second
        /// </summary>
        public int Acceleration = 50;
        /// <summary>
        /// this is the minimum speed the MCU will travel at, it is also start all moves at this speed befor accelerating, this should be fairly small
        /// </summary>
        public double StartSpeed = 0.06;
        /// <summary>
        /// timout of home comand 0 t0 300
        /// </summary>
        public int HomeTimeoutSec = 300;
        public bool AZUseHomesensors = true;
        public bool AZHomeActive_High = true;
        public bool ELUseHomesensors = true;
        public bool ELHomeActive_High = true;
        public CW_CCW_input_use AZCW = CW_CCW_input_use.LimitSwitch_High;
        public CW_CCW_input_use AZCCW = CW_CCW_input_use.LimitSwitch_High;
        public CW_CCW_input_use ELCW = CW_CCW_input_use.LimitSwitch_High;
        public CW_CCW_input_use ELCCW = CW_CCW_input_use.LimitSwitch_High;

    }
    /// <summary>
    /// used to define the function and active state 
    /// </summary>
    public enum CW_CCW_input_use {
        NotUsed,
        LimitSwitch_High,
        LimitSwitch_Low,
        EStop_High,
        EStop_Low,
    }
}
