using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace ControlRoomApplication.Controllers.SensorNetwork.Simulation
{
    public class SimulationSensorNetwork
    {
        private static readonly log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public SimulationSensorNetwork(string teensyClientIP, int teensyPortIP, IPAddress teensyServerIP, int teensyServerPort)
        {

        }

        private TcpListener Server { get; set; }

        private TcpClient Client { get; set; }
    }
}
