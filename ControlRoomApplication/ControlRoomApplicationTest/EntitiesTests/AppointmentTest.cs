using ControlRoomApplication.Constants;
using ControlRoomApplication.Entities;
using ControlRoomApplication.Controllers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;

namespace ControlRoomApplicationTest.EntitiesTests
{
    [TestClass]
    public class AppointmentTest
    {
        // Class being tested
        private Appointment appointment_1;
        private Appointment greaterThan;
        private Appointment equalTo;

        // Attributes in the class that need to be tested
        private User controlRoomUser;
        private DateTime startTime_1;
        private DateTime endTime_1;
        private CelestialBody celestial_body;
        private Orientation orientation;
        private Coordinate coordinate;
        private RadioTelescope telescope;
        private RFData rf_data;
        private AppointmentStatusEnum status;
        private AppointmentTypeEnum type;
        private SpectraCyber spectraCyber;
        private SpectraCyberConfig spectracyber_config;

        private DateTime startTime_2;
        private DateTime endTime_2;


        [TestInitialize]
        public void BuildUp()
        {
            // Initialize appointment entity
            appointment_1 = new Appointment();
            greaterThan = new Appointment();

            // Initialize data for fields
            controlRoomUser = new User("control", "room", "controlroom@gmail.com", NotificationTypeEnum.SMS);
            startTime_1 = DateTime.UtcNow;
            endTime_1 = DateTime.UtcNow.AddHours(1);
            celestial_body = new CelestialBody(CelestialBodyConstants.SUN);
            orientation = new Orientation(20, 20);
            coordinate = new Coordinate(20, 20);
            spectraCyber = new SpectraCyber();
            telescope = new RadioTelescope(new SpectraCyberController(spectraCyber), new TestPLCDriver(PLCConstants.LOCAL_HOST_IP, PLCConstants.LOCAL_HOST_IP, 8089, 8089, false), new Location(), new Orientation());
            rf_data = new RFData();
            rf_data.Intensity = 100;
            status = AppointmentStatusEnum.REQUESTED;
            type = AppointmentTypeEnum.POINT;
            spectracyber_config = new SpectraCyberConfig(SpectraCyberModeTypeEnum.CONTINUUM);

            startTime_2 = DateTime.UtcNow.AddDays(1);
            endTime_2 = DateTime.UtcNow.AddDays(1).AddHours(1);

            // Initialize fields we are testing against
            appointment_1.User = controlRoomUser;
            appointment_1.start_time = startTime_1;
            appointment_1.end_time = endTime_1;
            appointment_1.CelestialBody = celestial_body;
            appointment_1.Orientation = orientation;
            appointment_1.Coordinates.Add(coordinate);
            appointment_1.RFDatas.Add(rf_data);
            appointment_1._Status = status;
            appointment_1._Type = type;
            appointment_1.SpectraCyberConfig = spectracyber_config;
            appointment_1.Telescope = telescope;

            greaterThan.start_time = startTime_2;
            greaterThan.end_time = endTime_2;

            equalTo = appointment_1;
        }

        [TestMethod]
        public void TestGettersAndSetters()
        {
            Assert.AreEqual(controlRoomUser.Id, appointment_1.User.Id);
            Assert.AreEqual(startTime_1, appointment_1.start_time);
            Assert.AreEqual(endTime_1, appointment_1.end_time);
            Assert.AreEqual(celestial_body.Id, appointment_1.CelestialBody.Id);
            Assert.AreEqual(orientation.Id, appointment_1.Orientation.Id);
            Assert.AreEqual(coordinate, appointment_1.Coordinates.ToList()[0]);
            Assert.AreEqual(rf_data, appointment_1.RFDatas.ToList()[0]);
            Assert.AreEqual(telescope.Id, appointment_1.Telescope.Id);
            Assert.AreEqual(status, appointment_1._Status);
            Assert.AreEqual(type, appointment_1._Type);
            Assert.AreEqual(spectracyber_config.Id, appointment_1.SpectraCyberConfig.Id);
        }

        [TestMethod]
        public void TestComparable()
        {
            Assert.IsTrue(appointment_1 != greaterThan);
            Assert.IsTrue(appointment_1 < greaterThan);
            Assert.IsTrue(greaterThan > appointment_1);
            Assert.IsTrue(appointment_1 == equalTo);
        }
    }
}
