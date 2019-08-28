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
        /// this should only be used for the simulator
        /// </summary>
        /// <param name="plc"></param>
        public AbstractEncoderReader( AbstractPLCDriver plc, string micro_ctrl_IP , int port ) { }

        public abstract Orientation GetCurentOrientation();

    }
}
