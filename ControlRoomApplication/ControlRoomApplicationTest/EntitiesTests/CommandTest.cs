using Microsoft.VisualStudio.TestTools.UnitTesting;
using ControlRoomApplication.Entities;
using System;

namespace ControlRoomApplicationTest.EntitiesTests
{
    [TestClass]
    class CommandTest
    {
        Command command;

        [TestInitialize]
        public void BuildUp()
        {
            command = new Command
            {
                Resolution = 2.0,
                T_Move = 2.0,
                StartTime = DateTime.Today,
                EndTime = DateTime.MaxValue
            };
        }

        [TestMethod]
        public void TestGettersAndSetters()
        {
            Assert.AreEqual(2, command.Resolution);
            Assert.AreEqual(2, command.T_Move);
            command.StartTime = DateTime.UtcNow;
            Assert.AreEqual(DateTime.UtcNow, command.StartTime);
            command.EndTime = DateTime.Today;
            Assert.AreEqual(DateTime.Today, command.EndTime);

        }

    }
}
