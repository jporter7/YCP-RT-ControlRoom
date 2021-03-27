using EmbeddedSystemsTest.SensorNetworkSimulation;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControlRoomApplicationTest.EntityControllersTests.SensorNetworkTests.Simulation
{
    [TestClass]
    class PacketEncodingToolsTest
    {
        PacketEncodingTools tools = new PacketEncodingTools();

        [TestMethod]
        public void TestAdd32BitValueToByteArray_Max32BitValue_ConvertsToByteArray()
        {
            PrivateObject privTools = new PrivateObject(tools);
            uint max = 4294967295;
        }
    }
}
