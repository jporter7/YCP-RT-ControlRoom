using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using ControlRoomApplication.Constants;
using ControlRoomApplication.Entities;
using ControlRoomApplication.Simulators.Hardware.PLC_MCU;

namespace ControlRoomApplication.Controllers
{
    public class TestPLCDriver : SimulationPLCDriver {
        private static readonly log4net.ILog logger =
            log4net.LogManager.GetLogger( System.Reflection.MethodBase.GetCurrentMethod().DeclaringType );


        ///<inheritdoc/>
        public TestPLCDriver( string local_ip , string MCU_ip , int MCU_port , int PLC_port , bool startPLC ) : base( local_ip , MCU_ip , MCU_port , PLC_port , startPLC , true ) {

        }
    }
}