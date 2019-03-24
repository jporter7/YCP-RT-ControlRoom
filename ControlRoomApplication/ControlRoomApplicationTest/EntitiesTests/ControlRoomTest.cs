using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ControlRoomApplication.Entities;
using ControlRoomApplication.Main;
using ControlRoomApplication.Simulators.Hardware.WeatherStation;

namespace ControlRoomApplicationTest.EntitiesTests
{
    [TestClass]
    public class ControlRoomTest
    {
        ControlRoom controlRoom;

        [TestInitialize]
        public void BuildUp()
        {
            //Initialize control room object
            controlRoom = new ControlRoom(new SimulationWeatherStation(5));
        }
        
        [TestMethod]
        public void TestMethods()
        {
            throw new NotImplementedException();
        }
    }
}
