using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControlRoomApplication.Entities.Configuration {
    public class MCUConfigurationAxys {
        /// <summary>
        /// motor acceleration in steps/millisecond/second
        /// </summary>
        public int Acceleration = 50;
        /// <summary>
        /// this is the minimum speed the MCU will travel at (RPM) , it is also start all moves at this speed befor accelerating, this should be fairly small
        /// </summary>
        public double StartSpeed = 0.06;
        /// <summary>
        /// timout of home comand 0 t0 300
        /// </summary>
        public int HomeTimeoutSec = 300;

        public bool UseHomesensors = true;
        public bool HomeActive_High = true;


        public CW_CCW_input_use CWinput = CW_CCW_input_use.LimitSwitch;
        public bool CWactive_High = false;
        public CW_CCW_input_use CCWinput = CW_CCW_input_use.LimitSwitch;
        public bool CCWactive_High = false;


    }
    /// <summary>
    /// used to define the function and active state 
    /// </summary>
    public enum CW_CCW_input_use {
        NotUsed,
        LimitSwitch,
        EStop,
    }
}
