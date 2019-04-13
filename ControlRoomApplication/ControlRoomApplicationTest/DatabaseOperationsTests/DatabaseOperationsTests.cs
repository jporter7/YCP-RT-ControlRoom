using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ControlRoomApplication.Constants;
using ControlRoomApplication.Database;
using ControlRoomApplication.Entities;

namespace ControlRoomApplicationTest.DatabaseOperationsTests
{
    [TestClass]
    public class DatabaseOperationsTests
    {
        // Test RFData objects
        private RFData data1;
        private RFData data2;
        private RFData data3;

        // Test Appointment objects
        private Appointment appt;
        private int NumRTInstances = 1;

        [TestInitialize]
        public void BuildUp()
        {
            // Initialize context
            DatabaseOperations.PopulateLocalDatabase(NumRTInstances);

            // RFData initialization
            data1 = new RFData();
            data2 = new RFData();
            data3 = new RFData();
            DateTime date = DateTime.UtcNow;

            data1.Intensity = 9234875;
            data1.TimeCaptured = date;

            data2.Intensity = 8739425;
            data2.TimeCaptured = date.AddSeconds(5);

            data3.Intensity = 12987;
            data3.TimeCaptured = date.AddSeconds(10);

            // Init appt
            appt = DatabaseOperations.GetListOfAppointmentsForRadioTelescope(NumRTInstances)[0];

        }

        [TestCleanup]
        public void TearDown()
        {
            DatabaseOperations.DeleteLocalDatabase();
        }

        [TestMethod]
        public void TestPopulateLocalDatabase()
        {
            DatabaseOperations.DeleteLocalDatabase();
            DatabaseOperations.PopulateLocalDatabase(NumRTInstances);
            var appt_count = DatabaseOperations.GetTotalAppointmentCount();
            Assert.AreEqual(4 * NumRTInstances, appt_count);
        }

        [TestMethod]
        public void TestDeleteLocalDatabase()
        {
            DatabaseOperations.DeleteLocalDatabase();
            var appt_count = DatabaseOperations.GetTotalAppointmentCount();
            Assert.AreEqual(0, appt_count);
            DatabaseOperations.PopulateLocalDatabase(NumRTInstances);
        }

        [TestMethod]
        public void TestGetListOfAppointmentsForRadioTelescope()
        {
            var appts = DatabaseOperations.GetListOfAppointmentsForRadioTelescope(NumRTInstances);
            Assert.AreEqual(4 * NumRTInstances, appts.Count);
        }

        [TestMethod]
        public void TestAddAppointment()
        {
            var new_appt = new Appointment();
            new_appt.StartTime = DateTime.UtcNow;
            new_appt.EndTime = DateTime.UtcNow.AddMinutes(1);
            new_appt.Status = AppointmentStatusEnum.REQUESTED;
            new_appt.Type = AppointmentTypeEnum.POINT;
            new_appt.Coordinates.Add(new Coordinate(0,0));
            new_appt.SpectraCyberConfig = new SpectraCyberConfig(SpectraCyberModeTypeEnum.CONTINUUM);
            new_appt.TelescopeId = 1;
            new_appt.UserId = 1;

            DatabaseOperations.AddAppointment(new_appt);
            var output_appts = DatabaseOperations.GetListOfAppointmentsForRadioTelescope(1);
            Assert.IsTrue(1 == output_appts.Where(x => x.Id == new_appt.Id).Count());
        }

        [TestMethod]
        public void TestGetTotalAppointmentCount()
        {
            var appt_count = DatabaseOperations.GetTotalAppointmentCount();
            Assert.AreEqual(4 * NumRTInstances, appt_count);
        }

        [TestMethod]
        public void TestCreateRFData()
        {
            DatabaseOperations.CreateRFData(appt.Id, data1);
            DatabaseOperations.CreateRFData(appt.Id, data2);
            DatabaseOperations.CreateRFData(appt.Id, data3);

            // update appt
            appt = DatabaseOperations.GetListOfAppointmentsForRadioTelescope(NumRTInstances).Find(x => x.Id == appt.Id);

            Assert.AreEqual(appt.RFDatas.ToList().Find(x => x.Id == data1.Id).Intensity, data1.Intensity);
            Assert.AreEqual(appt.RFDatas.ToList().Find(x => x.Id == data2.Id).Intensity, data2.Intensity);
            Assert.AreEqual(appt.RFDatas.ToList().Find(x => x.Id == data3.Id).Intensity, data3.Intensity);

            Assert.AreEqual(appt.RFDatas.ToList().Find(x => x.Id == data1.Id).TimeCaptured.Date, data1.TimeCaptured.Date);
            Assert.AreEqual(appt.RFDatas.ToList().Find(x => x.Id == data2.Id).TimeCaptured.Date, data2.TimeCaptured.Date);
            Assert.AreEqual(appt.RFDatas.ToList().Find(x => x.Id == data3.Id).TimeCaptured.Date, data3.TimeCaptured.Date);
        }

        [TestMethod]
        public void TestCreateRFData_InvalidDate()
        {
            data1.TimeCaptured = DateTime.UtcNow.AddDays(1);

            DatabaseOperations.CreateRFData(appt.Id, data1);

            // update appt
            appt = DatabaseOperations.GetListOfAppointmentsForRadioTelescope(NumRTInstances).Find(x => x.Id == appt.Id);

            RFData testData = appt.RFDatas.ToList().Find(x => x.Id == data1.Id);

            Assert.AreEqual(null, testData);
        }

        [TestMethod]
        public void TestCreateRFData_InvalidIntensity()
        {
            data1.Intensity = -239458;

            DatabaseOperations.CreateRFData(appt.Id, data1);

            // update appt
            appt = DatabaseOperations.GetListOfAppointmentsForRadioTelescope(NumRTInstances).Find(x => x.Id == appt.Id);

            RFData testData = appt.RFDatas.ToList().Find(x => x.Id == data1.Id);

            Assert.AreEqual(null, testData);
        }

        [TestMethod]
        public void TestUpdateAppointmentStatus()
        {
            appt.Status = AppointmentStatusEnum.IN_PROGRESS;

            DatabaseOperations.UpdateAppointment(appt);
            
            // update appt
            appt = DatabaseOperations.GetListOfAppointmentsForRadioTelescope(NumRTInstances).Find(x => x.Id == appt.Id);
            var testStatus = appt.Status;

            Assert.AreEqual(AppointmentStatusEnum.IN_PROGRESS, testStatus);
        }

        [TestMethod]
        public void TestUpdateAppointmentCoordinates()
        {
            var origCoord = new Coordinate(0, 0);
            appt.Coordinates.Add(origCoord);

            DatabaseOperations.UpdateAppointment(appt);

            // update appt
            appt = DatabaseOperations.GetListOfAppointmentsForRadioTelescope(NumRTInstances).Find(x => x.Id == appt.Id);

            var testCoord = appt.Coordinates.First();

            Assert.AreEqual(origCoord.RightAscension, testCoord.RightAscension);
            Assert.AreEqual(origCoord.Declination, testCoord.Declination);
        }

        [TestMethod]
        public void TestGetNextAppointment()
        {
            var appt = DatabaseOperations.GetNextAppointment(NumRTInstances);
            Assert.IsTrue(appt != null);
            Assert.IsTrue(appt.StartTime > DateTime.UtcNow);
            Assert.IsTrue(appt.Status != AppointmentStatusEnum.COMPLETED);
        }
    }
}
