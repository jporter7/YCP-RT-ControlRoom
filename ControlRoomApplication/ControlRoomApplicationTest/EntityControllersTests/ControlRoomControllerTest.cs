using System;
using System.Collections.Generic;
using ControlRoomApplication.Controllers.RadioTelescopeControllers;
using ControlRoomApplication.Entities.RadioTelescope;
using ControlRoomApplication.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ControlRoomApplication.Controllers.PLCController;
using ControlRoomApplication.Controllers.SpectraCyberController;
using ControlRoomApplication.Entities.Plc;
using ControlRoomApplication.Main;
using ControlRoomApplication.Controllers;
using ControlRoomApplication.Database.Operations;

namespace ControlRoomApplicationTest.EntityControllersTests
{
    [TestClass]
    class ControlRoomControllerTest
    {
        public ControlRoomController ControlRoomController { get; set; }

        [TestInitialize]
        public void SetUp()
        {
            // Instantiate the controller
            RTDbContext dbContext = new RTDbContext();
            DatabaseOperations.InitializeLocalConnectionOnly();
            DatabaseOperations.PopulateLocalDatabase();

            AbstractPLC plc = new TestPLC();
            PLCController plcController = new PLCController(plc);
            AbstractSpectraCyberController spectraCyberController = new SpectraCyberTestController(new SpectraCyberSimulator(), dbContext);
            AbstractRadioTelescope radioTelescope = new TestRadioTelescope(spectraCyberController, plcController);
            RadioTelescopeController rtController = new RadioTelescopeController(radioTelescope);
            ControlRoom cRoom = new ControlRoom(rtController, dbContext);
            ControlRoomController = new ControlRoomController(cRoom);
        }

        [TestCleanup]
        public void TearDown()
        {
            DatabaseOperations.DisposeLocalDatabaseOnly();
            ControlRoomController.CRoom.Context.Database.Delete();
            ControlRoomController.CRoom.Context.Dispose();
        }

        [TestMethod]
        public void TestWaitingForNextAppointment()
        {
            Appointment appt = ControlRoomController.WaitingForNextAppointment();
            TimeSpan diff = appt.StartTime - DateTime.Now;

            Assert.AreNotEqual(appt, null);
            Assert.IsTrue(diff.TotalMinutes > 10);
        }

        [TestMethod]
        public void TestGetNextAppointment()
        {

        }

        [TestMethod]
        public void TestCalibrateRadioTelescope()
        {

        }

        [TestMethod]
        public void TestStartRadioTelescope()
        {

        }

        [TestMethod]
        public void TestEndAppointment()
        {

        }

        [TestMethod]
        public void TestStartReadingData()
        {

        }

        [TestMethod]
        public void TestStopReadingRFData()
        {

        }
    }
}
