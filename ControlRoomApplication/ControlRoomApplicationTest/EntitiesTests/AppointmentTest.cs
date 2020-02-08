using ControlRoomApplication.Constants;
using ControlRoomApplication.Entities;
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
        private Appointment appointment_2;

        // Attributes in the class that need to be tested
        private int user_id;
        private DateTime startTime_1;
        private DateTime endTime_1;
        private CelestialBody celestial_body;
        private Orientation orientation;
        private Coordinate coordinate;
        private RFData rf_data;
        private int telescope_id;
        private AppointmentStatusEnum status;
        private AppointmentTypeEnum type;
        private SpectraCyberConfig spectracyber_config;

        private DateTime startTime_2;
        private DateTime endTime_2;


        [TestInitialize]
        public void BuildUp()
        {
            // Initialize appointment entity
            appointment_1 = new Appointment();
            appointment_2 = new Appointment();

            // Initialize data for fields
            user_id = 1;
            startTime_1 = DateTime.UtcNow;
            endTime_1 = DateTime.UtcNow.AddHours(1);
            celestial_body = new CelestialBody(CelestialBodyConstants.SUN);
            orientation = new Orientation(20, 20);
            coordinate = new Coordinate(20, 20);
            rf_data = new RFData();
            rf_data.Intensity = 100;
            telescope_id = 1;
            status = AppointmentStatusEnum.REQUESTED;
            type = AppointmentTypeEnum.POINT;
            spectracyber_config = new SpectraCyberConfig(SpectraCyberModeTypeEnum.CONTINUUM);

            startTime_2 = DateTime.UtcNow.AddDays(1);
            endTime_2 = DateTime.UtcNow.AddDays(1).AddHours(1);

            // Initialize fields we are testing against
            appointment_1.user_id = user_id;
            appointment_1.start_time = startTime_1;
            appointment_1.end_time = endTime_1;
            appointment_1.CelestialBody = celestial_body;
            appointment_1.Orientation = orientation;
            appointment_1.Coordinates.Add(coordinate);
            appointment_1.RFDatas.Add(rf_data);
            appointment_1.telescope_id = telescope_id;
            appointment_1._Status = status;
            appointment_1._Type = type;
            appointment_1.SpectraCyberConfig = spectracyber_config;

            appointment_2.start_time = startTime_2;
            appointment_2.end_time = endTime_2;
        }

        [TestMethod]
        public void TestGettersAndSetters()
        {
            Assert.AreEqual(user_id, appointment_1.user_id);
            Assert.AreEqual(startTime_1, appointment_1.start_time);
            Assert.AreEqual(endTime_1, appointment_1.end_time);
            Assert.AreEqual(celestial_body, appointment_1.CelestialBody);
            Assert.AreEqual(orientation, appointment_1.Orientation);
            Assert.AreEqual(coordinate, appointment_1.Coordinates.ToList()[0]);
            Assert.AreEqual(rf_data, appointment_1.RFDatas.ToList()[0]);
            Assert.AreEqual(telescope_id, appointment_1.telescope_id);
            Assert.AreEqual(status, appointment_1._Status);
            Assert.AreEqual(type, appointment_1._Type);
            Assert.AreEqual(spectracyber_config, appointment_1.SpectraCyberConfig);
        }

        [TestMethod]
        public void TestComparable()
        {
            Assert.IsTrue(appointment_1 != appointment_2);
            Assert.IsTrue(appointment_1 < appointment_2);
        }
    }
}
