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
            controlRoom = new ControlRoom();
        }
    }
}
