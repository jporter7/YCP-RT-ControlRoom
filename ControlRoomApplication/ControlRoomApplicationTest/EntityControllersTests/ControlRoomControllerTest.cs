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
using ControlRoomApplication.Constants;

namespace ControlRoomApplicationTest.EntityControllersTests
{
    [TestClass]
    public class ControlRoomControllerTest
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
            AbstractSpectraCyberController spectraCyberController = new SpectraCyberTestController(new SpectraCyberSimulator());
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
            Assert.IsTrue(diff.TotalMinutes < 10);
        }

        [TestMethod]
        public void TestGetNextAppointment()
        {
            Appointment appt = ControlRoomController.GetNextAppointment();

            Assert.AreNotEqual(appt, null);
            Assert.AreNotEqual(appt.Status, AppointmentConstants.COMPLETED);
            Assert.IsTrue(appt.StartTime > DateTime.Now);
        }

        [TestMethod]
        public void TestCalibrateRadioTelescope()
        {
            ControlRoomController.CalibrateRadioTelescope();

            // Add more checks later
        }

        [TestMethod]
        public void TestStartRadioTelescope()
        {
            Appointment appt = new Appointment();
            Dictionary<DateTime, Orientation> orientations = new Dictionary<DateTime, Orientation>();
            Orientation orientation = new Orientation(0, 0);
            orientations.Add(DateTime.Now.AddSeconds(3), orientation);
            ControlRoomController.StartRadioTelescope(appt, orientations);

            Assert.AreEqual(appt.Status, AppointmentConstants.COMPLETED);
            Assert.AreEqual(ControlRoomController.CRoom.RadioTelescopeController.RadioTelescope.CurrentOrientation.Elevation, orientation.Elevation);
            Assert.AreEqual(ControlRoomController.CRoom.RadioTelescopeController.RadioTelescope.CurrentOrientation.Azimuth, orientation.Azimuth);
        }

        [TestMethod]
        public void TestEndAppointment()
        {
            Orientation orientation = new Orientation(0, 90);
            ControlRoomController.EndAppointment();

            Assert.AreEqual(ControlRoomController.CRoom.RadioTelescopeController.RadioTelescope.CurrentOrientation.Elevation, orientation.Elevation);
            Assert.AreEqual(ControlRoomController.CRoom.RadioTelescopeController.RadioTelescope.CurrentOrientation.Azimuth, orientation.Azimuth);
        }

        [TestMethod]
        public void TestStartReadingData()
        {
            Appointment appt = new Appointment();
            ControlRoomController.StartReadingData(appt);

            // Add more checks later
        }

        [TestMethod]
        public void TestStopReadingRFData()
        {
            ControlRoomController.StopReadingRFData();

            // Add more checks later
        }
    }
}
