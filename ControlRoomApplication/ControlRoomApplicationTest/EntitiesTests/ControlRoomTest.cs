using Microsoft.VisualStudio.TestTools.UnitTesting;
using ControlRoomApplication.Entities;
using ControlRoomApplication.Main;

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
            controlRoom = new ControlRoom(new RTDbContext());
        }
        
        [TestMethod]
        public void TestMethods()
        {
            
        }
    }
}
