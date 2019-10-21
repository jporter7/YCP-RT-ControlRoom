using ControlRoomApplication.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControlRoomApplication.Controllers.BlkHeadUcontroler {
    public  abstract class AbstractEncoderReader {


        public AbstractEncoderReader(string micro_ctrl_IP, int port) { }
        /// <summary>
        /// only used for simulator as the simulator uses the plc to determine its position
        /// </summary>
        /// <param name="plc"></param>
        /// <param name="micro_ctrl_IP"></param>
        /// <param name="port"></param>
        public AbstractEncoderReader( AbstractPLCDriver plc, string micro_ctrl_IP , int port ) { }

        public abstract Orientation GetCurentOrientation();

    }
}
