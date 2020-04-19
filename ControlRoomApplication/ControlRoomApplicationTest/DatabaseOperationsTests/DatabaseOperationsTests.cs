using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ControlRoomApplication.Constants;
using ControlRoomApplication.Database;
using ControlRoomApplication.Entities;
using ControlRoomApplication.Controllers;

namespace ControlRoomApplicationTest.DatabaseOperationsTests
{
    [TestClass]
    public class DatabaseOperationsTests
    {
        // Test RFData objects
        private RFData data1;
        private RFData data2;
        private RFData data3;
        private RFData data4;

        // Test Appointment objects
        private Appointment appt;
        private int NumRTInstances = 1;
        private int NumAppointments = 0;
        private int NumRFData = -1;

        [TestInitialize]
        public void BuildUp()
        { 
            NumAppointments = DatabaseOperations.GetTotalAppointmentCount();
            NumRTInstances = DatabaseOperations.GetTotalRTCount();
            NumRFData = DatabaseOperations.GetTotalRFDataCount();

            appt = new Appointment();
            appt.start_time = DateTime.UtcNow;
            appt.end_time = DateTime.UtcNow.AddMinutes(1);
            appt._Status = AppointmentStatusEnum.IN_PROGRESS;
            appt._Priority = AppointmentPriorityEnum.MANUAL;
            appt._Type = AppointmentTypeEnum.FREE_CONTROL;
            appt.Coordinates.Add(new Coordinate(0, 0));
            appt.CelestialBody = new CelestialBody();
            appt.CelestialBody.Coordinate = new Coordinate();
            appt.Orientation = new Orientation();
            appt.SpectraCyberConfig = new SpectraCyberConfig(SpectraCyberModeTypeEnum.CONTINUUM);
            appt.Telescope = new RadioTelescope(new SpectraCyberController(new SpectraCyber()), new TestPLCDriver(PLCConstants.LOCAL_HOST_IP, PLCConstants.LOCAL_HOST_IP, 8089, 8089, false), new Location(), new Orientation());
            appt.User = DatabaseOperations.GetControlRoomUser();

            DatabaseOperations.AddAppointment(appt);

            // RFData initialization
            data1 = new RFData();
            data2 = new RFData();
            data3 = new RFData();
            data4 = new RFData();
            DateTime now = DateTime.UtcNow;
            DateTime date = new DateTime(now.Year, now.Month, now.Day, now.Hour, now.Minute, now.Second);

            Appointment RFappt = DatabaseOperations.GetUpdatedAppointment(appt.Id);
            data1.Intensity = 9234875;
            data1.TimeCaptured = date;
            data1.Appointment = RFappt;

            data2.Intensity = 8739425;
            data2.TimeCaptured = date.AddSeconds(3);
            data2.Appointment = RFappt;

            data3.Intensity = 12987;
            data3.TimeCaptured = date.AddSeconds(4);
            data3.Appointment = RFappt;

            data4.Intensity = 12987;
            data4.TimeCaptured = date.AddSeconds(5);
            data4.Appointment = RFappt;

            NumAppointments++;
            NumRTInstances++;

        }

 /*       [TestMethod]
        public void TestPopulateLocalDatabase()
        { 
            DatabaseOperations.PopulateLocalDatabase(NumRTInstances);
            var appt_count = DatabaseOperations.GetTotalAppointmentCount();
            Assert.AreEqual(45 * NumRTInstances, appt_count);
        }
*/
        [TestMethod]
        public void TestGetListOfAppointmentsForRadioTelescope()
        {
            var appts = DatabaseOperations.GetListOfAppointmentsForRadioTelescope(appt.Telescope.Id);
            Assert.AreEqual(1, appts.Count);
        }

        [TestMethod]
        public void TestAddAppointment()
        {
            var new_appt = new Appointment();
            new_appt.start_time = DateTime.UtcNow;
            new_appt.end_time = DateTime.UtcNow.AddMinutes(1);
            new_appt._Status = AppointmentStatusEnum.REQUESTED;
            new_appt._Priority = AppointmentPriorityEnum.MANUAL;
            new_appt._Type = AppointmentTypeEnum.POINT;
            new_appt.Coordinates.Add(new Coordinate(0, 0));
            new_appt.CelestialBody = new CelestialBody();
            new_appt.CelestialBody.Coordinate = new Coordinate();
            new_appt.Orientation = new Orientation();
            new_appt.SpectraCyberConfig = new SpectraCyberConfig(SpectraCyberModeTypeEnum.CONTINUUM);
            new_appt.Telescope = new RadioTelescope(new SpectraCyberController(new SpectraCyber()), new TestPLCDriver(PLCConstants.LOCAL_HOST_IP, PLCConstants.LOCAL_HOST_IP, 8089, 8089, false), new Location(), new Orientation());
            new_appt.User = DatabaseOperations.GetControlRoomUser();
            DatabaseOperations.AddAppointment(new_appt);
            NumAppointments++;

            var output_appts = DatabaseOperations.GetListOfAppointmentsForRadioTelescope(new_appt.Telescope.Id);
            Assert.IsTrue(1 == output_appts.Count());
        }

        [TestMethod]
        public void TestGetTotalAppointmentCount()
        {
            var appt_count = DatabaseOperations.GetTotalAppointmentCount();
            Assert.AreEqual(NumAppointments, appt_count);
        }

        [TestMethod]
        public void TestCreateRFData()
        {
            DatabaseOperations.AddRFData(data1);
            DatabaseOperations.AddRFData(data2);

            Assert.AreEqual(DatabaseOperations.GetTotalRFDataCount(), NumRFData + 2);

            List<RFData> datas = DatabaseOperations.GetListOfRFData();
            RFData dbData1 = datas.Find(x => x.Id == data1.Id);
            RFData dbData2 = datas.Find(x => x.Id == data2.Id);

            Assert.IsTrue(dbData1 != null);
            Assert.IsTrue(dbData2 != null);

            Assert.AreEqual(dbData1.Intensity, data1.Intensity);
            Assert.AreEqual(dbData2.Intensity, data2.Intensity);

            Assert.IsTrue(dbData1.TimeCaptured == data1.TimeCaptured);
            Assert.IsTrue(dbData2.TimeCaptured == data2.TimeCaptured);

        }

        [TestMethod]
        public void TestCreateRFData_InvalidDate()
        {
            data3.TimeCaptured = DateTime.UtcNow.AddDays(1);

            DatabaseOperations.AddRFData(data3);

            List<RFData> datas = DatabaseOperations.GetListOfRFData();

            RFData testData = appt.RFDatas.ToList().Find(x => x.Id == data3.Id);

            Assert.AreEqual(null, datas.Find(t => t.Id == data3.Id));
        }

        [TestMethod]
        public void TestCreateRFData_InvalidIntensity()
        {
            data4.Intensity = -239458;

            DatabaseOperations.AddRFData(data4);

            List<RFData> datas = DatabaseOperations.GetListOfRFData();

            RFData testData = appt.RFDatas.ToList().Find(x => x.Id == data4.Id);

            Assert.AreEqual(null, datas.Find(t => t.Id == data4.Id));
        }

        [TestMethod]
        public void TestUpdateAppointmentStatus()
        {
            appt._Status = AppointmentStatusEnum.IN_PROGRESS;

            DatabaseOperations.UpdateAppointment(appt);
            
            // update appt
            appt = DatabaseOperations.GetListOfAppointmentsForRadioTelescope(appt.Telescope.Id).Find(x => x.Id == appt.Id);
            var testStatus = appt._Status;

            Assert.AreEqual(AppointmentStatusEnum.IN_PROGRESS, testStatus);
        }

        [TestMethod]
        public void TestUpdateAppointmentCoordinates()
        {
            var origCoord = new Coordinate(0, 0);
            appt.Coordinates.Add(origCoord);

            DatabaseOperations.UpdateAppointment(appt);

            // update appt
            appt = DatabaseOperations.GetListOfAppointmentsForRadioTelescope(appt.Telescope.Id).Find(x => x.Id == appt.Id);

            var testCoord = appt.Coordinates.First();

            Assert.AreEqual(origCoord.RightAscension, testCoord.RightAscension);
            Assert.AreEqual(origCoord.Declination, testCoord.Declination);
        }

        [TestMethod]
        public void TestGetNextAppointment()
        {
            var nextAppt = DatabaseOperations.GetNextAppointment(appt.Telescope.Id);
            Assert.IsTrue(nextAppt != null);
            Assert.IsTrue(nextAppt._Status != AppointmentStatusEnum.COMPLETED);
        }
    }
}
