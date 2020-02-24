using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControlRoomApplication.Controllers.Sensors {
    public struct MiscPlcInput {
        public bool Gate_Sensor;
        public bool Estop;
        /// <summary>
        /// the PLC and MCU are both connected to this sensor when it goes high the MCU will capture its current position this will let us track if the triangle connector has slipped on the axsis
        /// </summary>
        public bool EL_Slip_CAPTURE;
    }
}
