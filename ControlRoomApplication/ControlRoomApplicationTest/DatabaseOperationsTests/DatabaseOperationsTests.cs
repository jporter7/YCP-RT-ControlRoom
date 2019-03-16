using ControlRoomApplication.Constants;
using ControlRoomApplication.Database.Operations;
using ControlRoomApplication.Entities;
using ControlRoomApplication.Main;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

namespace ControlRoomApplicationTest.DatabaseOperationsTests
{
    [TestClass]
    public class DatabaseOperationsTests
    {
        // Test database context
        private RTDbContext testContext;

        // Test RFData objects
        private RFData data1;
        private RFData data2;
        private RFData data3;

        // Test Appointment objects
        private Appointment appt1;
        private Appointment appt2;
        private Appointment appt3;

        [TestInitialize]
        public void BuildUp()
        {
            // Initialize context and test context
            DatabaseOperations.InitializeLocalConnectionOnly();
            testContext = new RTDbContext();

            // RFData initialization
            data1 = new RFData();
            data2 = new RFData();
            data3 = new RFData();
            DateTime date = DateTime.Now;

            data1.Intensity = 9234875;
            data1.TimeCaptured = date;

            data2.Intensity = 8739425;
            data2.TimeCaptured = date.AddSeconds(5);

            data3.Intensity = 12987;
            data3.TimeCaptured = date.AddSeconds(10);

            // Appointment initialization
            appt1 = new Appointment();
            appt2 = new Appointment();
            appt3 = new Appointment();

            appt1.StartTime = date;
            appt1.EndTime = date.AddHours(1);
            appt1.Status = AppointmentConstants.IN_PROGRESS;
            appt1.Type = AppointmentTypeConstants.POINT;
            appt1.Coordinates = new List<Coordinate>();
            appt1.CelestialBody = CelestialBodyConstants.NONE;
            appt1.TelescopeId = 1;
            appt1.UserId = 1;

            appt2.StartTime = date.AddHours(2);
            appt2.EndTime = date.AddHours(3);
            appt2.Status = AppointmentConstants.REQUESTED;
            appt2.Type = AppointmentTypeConstants.POINT;
            appt2.Coordinates = new List<Coordinate>();
            appt2.CelestialBody = CelestialBodyConstants.NONE;
            appt2.TelescopeId = 1;
            appt2.UserId = 1;

            appt3.StartTime = date.AddHours(3);
            appt3.EndTime = date.AddHours(4);
            appt3.Status = AppointmentConstants.REQUESTED;
            appt3.Type = AppointmentTypeConstants.POINT;
            appt3.Coordinates = new List<Coordinate>();
            appt3.CelestialBody = CelestialBodyConstants.NONE;
            appt3.TelescopeId = 1;
            appt3.UserId = 1;
        }

        [TestCleanup]
        public void TearDown()
        {
            DatabaseOperations.DisposeLocalDatabaseOnly();
            testContext.Database.Delete();
            testContext.Dispose();
        }

        [TestMethod]
        public void TestCreateRFData()
        {
            DatabaseOperations.CreateRFData(data1);
            DatabaseOperations.CreateRFData(data2);
            DatabaseOperations.CreateRFData(data3);

            Assert.AreEqual(testContext.RFDatas.Find(1).Intensity, data1.Intensity);
            Assert.AreEqual(testContext.RFDatas.Find(2).Intensity, data2.Intensity);
            Assert.AreEqual(testContext.RFDatas.Find(3).Intensity, data3.Intensity);

            Assert.AreEqual(testContext.RFDatas.Find(1).TimeCaptured.Date, data1.TimeCaptured.Date);
            Assert.AreEqual(testContext.RFDatas.Find(2).TimeCaptured.Date, data2.TimeCaptured.Date);
            Assert.AreEqual(testContext.RFDatas.Find(3).TimeCaptured.Date, data3.TimeCaptured.Date);
        }

        [TestMethod]
        public void TestCreateRFData_InvalidDate()
        {
            data1.TimeCaptured = DateTime.Now.AddDays(1);

            DatabaseOperations.CreateRFData(data1);

            RFData testData = testContext.RFDatas.Find(1);

            Assert.AreEqual(null, testData);
        }

        [TestMethod]
        public void TestCreateRFData_InvalidIntensity()
        {
            data1.Intensity = -239458;

            DatabaseOperations.CreateRFData(data1);

            RFData testData = testContext.RFDatas.Find(1);

            Assert.AreEqual(null, testData);
        }

        [TestMethod]
        public void TestUpdateAppointmentStatus()
        {
            testContext.Appointments.Add(appt1);
            testContext.SaveChanges();

            appt1.Status = AppointmentConstants.IN_PROGRESS;

            DatabaseOperations.UpdateAppointmentStatus(appt1);

            var testStatus = testContext.Appointments.Find(1).Status;

            Assert.AreEqual(AppointmentConstants.IN_PROGRESS, testStatus);
        }

        [TestMethod]
        public void TestUpdateAppointmentStatus_InvalidStatus()
        {
            testContext.Appointments.Add(appt1);
            testContext.SaveChanges();

            appt1.Status = "INVALID_STATUS";

            DatabaseOperations.UpdateAppointmentStatus(appt1);

            var testStatus = testContext.Appointments.Find(1).Status;

            Assert.AreNotEqual(AppointmentConstants.IN_PROGRESS, testStatus);
        }
    }
}
