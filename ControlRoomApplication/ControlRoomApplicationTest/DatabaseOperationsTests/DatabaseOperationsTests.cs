using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ControlRoomApplication.Constants;
using ControlRoomApplication.Database;
using ControlRoomApplication.Entities;
using ControlRoomApplication.Controllers;
using System.Threading;

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

            DatabaseOperations.AddRadioTelescope(appt.Telescope);
            DatabaseOperations.AddAppointment(appt);

            // RFData initialization
            data1 = new RFData();
            data2 = new RFData();
            data3 = new RFData();
            data4 = new RFData();
            DateTime now = DateTime.UtcNow;
            DateTime date = new DateTime(now.Year, now.Month, now.Day, now.Hour, now.Minute, now.Second);

            Appointment RFappt = appt;
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
            new_appt.Coordinates.Add(new Coordinate(15, 15));
            new_appt.CelestialBody = new CelestialBody();
            new_appt.CelestialBody.Coordinate = new Coordinate();
            new_appt.Orientation = new Orientation();
            new_appt.SpectraCyberConfig = new SpectraCyberConfig(SpectraCyberModeTypeEnum.CONTINUUM);
            new_appt.Telescope = new RadioTelescope(new SpectraCyberController(new SpectraCyber()), new TestPLCDriver(PLCConstants.LOCAL_HOST_IP, PLCConstants.LOCAL_HOST_IP, 8089, 8089, false), new Location(), new Orientation());
            new_appt.User = DatabaseOperations.GetControlRoomUser();

            DatabaseOperations.AddRadioTelescope(new_appt.Telescope);
            DatabaseOperations.AddAppointment(new_appt);

            var output_appts = DatabaseOperations.GetListOfAppointmentsForRadioTelescope(new_appt.Telescope.Id);

            Assert.IsTrue(1 == output_appts.Count()); // Should only be one appointment retrieved

            //Assert.AreEqual(new_appt.start_time.ToString(), output_appts[0].start_time.ToString());
            //Assert.AreEqual(new_appt.end_time.ToString(), output_appts[0].end_time.ToString());
            Assert.AreEqual(new_appt._Status, output_appts[0]._Status);
            Assert.AreEqual(new_appt._Priority, output_appts[0]._Priority);
            Assert.AreEqual(new_appt._Type, output_appts[0]._Type);

            // Coordinates
            Assert.AreEqual(new_appt.Id, output_appts[0].Coordinates.First().apptId);
            Assert.AreEqual(new_appt.Coordinates.First().hours, output_appts[0].Coordinates.First().hours);
            Assert.AreEqual(new_appt.Coordinates.First().minutes, output_appts[0].Coordinates.First().minutes);
            Assert.AreEqual(new_appt.Coordinates.First().Declination, output_appts[0].Coordinates.First().Declination);
            Assert.AreEqual(new_appt.Coordinates.First().RightAscension, output_appts[0].Coordinates.First().RightAscension);

            // Other entities that Appointment uses
            Assert.AreEqual(new_appt.celestial_body_id, output_appts[0].celestial_body_id);
            Assert.AreEqual(new_appt.orientation_id, output_appts[0].orientation_id);
            Assert.AreEqual(new_appt.spectracyber_config_id, output_appts[0].spectracyber_config_id);
            Assert.AreEqual(new_appt.telescope_id, output_appts[0].telescope_id);
            Assert.AreEqual(new_appt.user_id, output_appts[0].user_id);
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
        public void TestUpdateAppointmentPriority()
        {
            var expectedPriority = AppointmentPriorityEnum.MANUAL;
            appt._Priority = expectedPriority;

            DatabaseOperations.UpdateAppointment(appt);

            // update appt
            appt = DatabaseOperations.GetListOfAppointmentsForRadioTelescope(appt.Telescope.Id).Find(x => x.Id == appt.Id);

            var resultPriority = appt._Priority;

            Assert.AreEqual(expectedPriority, resultPriority);
        }

        [TestMethod]
        public void TestUpdateAppointmentCoordinates()
        {
            // Create expected coordinates and add to the appointment
            var coords = new Coordinate(5, 5);
            appt.Coordinates.Add(coords);

            // Add appointment to the database
            DatabaseOperations.UpdateAppointment(appt);

            // Retrieve appointment from the database
            appt = DatabaseOperations.GetListOfAppointmentsForRadioTelescope(appt.Telescope.Id).Find(x => x.Id == appt.Id);
            List<Coordinate> resultCoordinates = appt.Coordinates.ToList<Coordinate>();

            // Verify only two results are returned
            Assert.IsTrue(resultCoordinates.Count == 2);

            // Create expected coordinate list
            List<Coordinate> expectedCoordinates = new List<Coordinate>();
            expectedCoordinates.Add(new Coordinate(0, 0));
            expectedCoordinates.Add(new Coordinate(5, 5));

            // Verify coordinates are correct
            Assert.AreEqual(expectedCoordinates[0].Declination, resultCoordinates[0].Declination);
            Assert.AreEqual(expectedCoordinates[0].RightAscension, resultCoordinates[0].RightAscension);

            Assert.AreEqual(expectedCoordinates[1].Declination, resultCoordinates[1].Declination);
            Assert.AreEqual(expectedCoordinates[1].RightAscension, resultCoordinates[1].RightAscension);

        }

        [TestMethod]
        public void TestAddThenDeleteCoordinates()
        {
            // Create expected coordinates and add to the appointment
            var coords = new Coordinate(5, 5);
            appt.Coordinates.Add(coords);

            // Add appointment to the database
            DatabaseOperations.UpdateAppointment(appt);

            Thread.Sleep(100);

            // Delete coordinates and then update the DB entry
            appt.Coordinates.Remove(coords);
            DatabaseOperations.UpdateAppointment(appt);

            // Retrieve appointment from the database
            appt = DatabaseOperations.GetListOfAppointmentsForRadioTelescope(appt.Telescope.Id).Find(x => x.Id == appt.Id);
            List<Coordinate> resultCoordinates = appt.Coordinates.ToList<Coordinate>();

            // Verify only one result is returned
            Assert.IsTrue(resultCoordinates.Count == 1);

            // Create expected coordinate
            Coordinate expectedCoordinate = new Coordinate(0, 0);

            // Verify coordinates are correct
            Assert.AreEqual(expectedCoordinate.Declination, resultCoordinates[0].Declination);
            Assert.AreEqual(expectedCoordinate.RightAscension, resultCoordinates[0].RightAscension);
        }

        [TestMethod]
        public void TestGetNextAppointment()
        {
            var nextAppt = DatabaseOperations.GetNextAppointment(appt.Telescope.Id);
            Assert.IsTrue(nextAppt != null);
            Assert.IsTrue(nextAppt._Status != AppointmentStatusEnum.COMPLETED);
        }

        [TestMethod]
        public void TestSwitchOverrideForSensor()
        {
            bool before = DatabaseOperations.GetOverrideStatusForSensor(SensorItemEnum.GATE);

            DatabaseOperations.SwitchOverrideForSensor(SensorItemEnum.GATE);

            bool after = DatabaseOperations.GetOverrideStatusForSensor(SensorItemEnum.GATE);

            Assert.IsTrue(before != after);
        }

        [TestMethod]
        public void TestGetThresholdForSensor()
        {
            double wind = DatabaseOperations.GetThresholdForSensor(SensorItemEnum.WIND);
            Assert.IsTrue(wind > 0);

            double az_motor_temp = DatabaseOperations.GetThresholdForSensor(SensorItemEnum.AZ_MOTOR_TEMP);
            Assert.IsTrue(az_motor_temp > 0);

            double elev_motor_temp = DatabaseOperations.GetThresholdForSensor(SensorItemEnum.ELEV_MOTOR_TEMP);
            Assert.IsTrue(elev_motor_temp > 0);

            double az_motor_vibe = DatabaseOperations.GetThresholdForSensor(SensorItemEnum.AZ_MOTOR_VIBRATION);
            Assert.IsTrue(az_motor_vibe > 0);

            double elev_motor_vibe = DatabaseOperations.GetThresholdForSensor(SensorItemEnum.ELEV_MOTOR_VIBRATION);
            Assert.IsTrue(elev_motor_vibe > 0);

            double az_motor_current = DatabaseOperations.GetThresholdForSensor(SensorItemEnum.AZ_MOTOR_CURRENT);
            Assert.IsTrue(az_motor_current > 0);

            double elev_motor_current = DatabaseOperations.GetThresholdForSensor(SensorItemEnum.ELEV_MOTOR_CURRENT);
            Assert.IsTrue(elev_motor_current > 0);

            double counter_vibe = DatabaseOperations.GetThresholdForSensor(SensorItemEnum.COUNTER_BALANCE_VIBRATION);
            Assert.IsTrue(counter_vibe > 0);
        }
    }
}
