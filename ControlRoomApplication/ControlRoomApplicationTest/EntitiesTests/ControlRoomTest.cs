using ControlRoomApplication.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

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
            controlRoom = new ControlRoom();
        }


        [TestMethod]
        public void testMethods()
        {
            //Create independent list of appointments for testing
            List<Appointment> appointments = new List<Appointment>(); 
            //Get appointments from the context and ensure the data is valid
            appointments =  controlRoom.DbSetToList(Context.Appointments);
            //Tests whether any appointments exist in the context
            Assert.IsTrue(appointments.Count != 0);
            //Test to make sure that the defautl constructor creates an instance of the scalemodelPLC
            Assert.IsTrue(controlRoom.RadioTelescope.Status == RadioTelescopeStatusEnum.UNKNOWN);

        }
    }
}
