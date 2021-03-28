using ControlRoomApplication.Controllers.SensorNetwork.Simulation;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ControlRoomApplicationTest.EntityControllersTests.SensorNetworkTests.Simulation
{
    [TestClass]
    public class SimulationSensorNetworkTest
    {
        string ClientIP = "127.0.0.1";
        int ClientPort = 3000;

        IPAddress ServerIP = IPAddress.Parse("127.0.0.2");
        int ServerPort = 3001;

        string DataPath = "../../EntityControllersTests/SensorNetworkTests/Simulation/TestCSVData/";

        SimulationSensorNetwork SimSensorNetwork;

        [TestInitialize]
        public void Initialize()
        {
            SimSensorNetwork = new SimulationSensorNetwork(ClientIP, ClientPort, ServerIP, ServerPort, DataPath);
        }

        [TestMethod]
        public void SimulationSensorNetwork_Constructor_ClientAndServerCorrect()
        {
            
        }
    }
}
