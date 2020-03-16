using System;
using System.Linq;
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

        // Test Appointment objects
        private Appointment appt;
        private Appointment initial;
        private int NumRTInstances = 1;
        private int NumAppointments = 0;

        [TestInitialize]
        public void BuildUp()
        {
            // Initialize context
            //   DatabaseOperations.PopulateLocalDatabase(NumRTInstances);

            NumAppointments = DatabaseOperations.GetTotalAppointmentCount();
            NumRTInstances = DatabaseOperations.GetTotalRTCount();

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

            initial = new Appointment();
            initial.start_time = DateTime.UtcNow;
            initial.end_time = DateTime.UtcNow.AddMinutes(1);
            initial._Status = AppointmentStatusEnum.REQUESTED;
            initial._Priority = AppointmentPriorityEnum.MANUAL;
            initial._Type = AppointmentTypeEnum.POINT;
            initial.Coordinates.Add(new Coordinate(0, 0));
            initial.SpectraCyberConfig = new SpectraCyberConfig(SpectraCyberModeTypeEnum.CONTINUUM);
            initial.Telescope = new RadioTelescope(new SpectraCyberController(new SpectraCyber()), new TestPLCDriver(PLCConstants.LOCAL_HOST_IP, PLCConstants.LOCAL_HOST_IP, 8089, 8089, false), new Location(), new Orientation());
            initial.User = new User("control", "room", "controlroom@gmail.com"); ;

            DatabaseOperations.AddAppointment(initial);
            NumAppointments++;
            NumRTInstances++;

            // Init appt
            appt = DatabaseOperations.GetListOfAppointmentsForRadioTelescope(NumRTInstances)[0];

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
            var appts = DatabaseOperations.GetListOfAppointmentsForRadioTelescope(initial.Telescope.Id);
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
            new_appt.Coordinates.Add(new Coordinate(0,0));
            new_appt.SpectraCyberConfig = new SpectraCyberConfig(SpectraCyberModeTypeEnum.CONTINUUM);
            new_appt.Telescope = new RadioTelescope(new SpectraCyberController(new SpectraCyber()), new TestPLCDriver(PLCConstants.LOCAL_HOST_IP, PLCConstants.LOCAL_HOST_IP, 8089, 8089, false), new Location(), new Orientation());
            new_appt.User = new User("control", "room", "controlroom@gmail.com"); ;

            DatabaseOperations.AddAppointment(new_appt);
            var output_appts = DatabaseOperations.GetListOfAppointmentsForRadioTelescope(1);
            Assert.IsTrue(1 == output_appts.Where(x => x.Id == new_appt.Id).Count());
        }

        [TestMethod]
        public void TestGetTotalAppointmentCount()
        {
            var appt_count = DatabaseOperations.GetTotalAppointmentCount();
            Assert.AreEqual(33 * NumRTInstances, appt_count);
        }

        [TestMethod]
        public void TestCreateRFData()
        {
            DatabaseOperations.CreateRFData(appt.Id, data1);
            DatabaseOperations.CreateRFData(appt.Id, data2);
            DatabaseOperations.CreateRFData(appt.Id, data3);

            // update appt
            appt = DatabaseOperations.GetListOfAppointmentsForRadioTelescope(1).Find(x => x.Id == appt.Id);

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
            appt._Status = AppointmentStatusEnum.IN_PROGRESS;

            DatabaseOperations.UpdateAppointment(appt);
            
            // update appt
            appt = DatabaseOperations.GetListOfAppointmentsForRadioTelescope(NumRTInstances).Find(x => x.Id == appt.Id);
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
            Assert.IsTrue(appt.start_time > DateTime.UtcNow);
            Assert.IsTrue(appt._Status != AppointmentStatusEnum.COMPLETED);
        }
    }
}
