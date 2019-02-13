using ControlRoomApplication.Entities;
using ControlRoomApplication.Entities.RadioTelescope;
using ControlRoomApplication.Controllers.RadioTelescopeControllers;
using ControlRoomApplication.Main;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

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
            controlRoom = new ControlRoom(new RadioTelescopeController(new TestRadioTelescope()), new RTDbContext());
        }

        
        [TestMethod]
        public void TestMethods()
        {
            //Create independent list of appointments for testing
            List<Appointment> appointments = new List<Appointment>(); 
            //Tests whether any appointments exist in the context
            Assert.IsTrue(appointments.Count >= 0);
            //Test to make sure that the defautl constructor creates an instance of the scalemodelPLC
            Assert.IsTrue(controlRoom.RadioTelescopeController.RadioTelescope.Status == RadioTelescopeStatusEnum.UNKNOWN);

        }
    }
}
