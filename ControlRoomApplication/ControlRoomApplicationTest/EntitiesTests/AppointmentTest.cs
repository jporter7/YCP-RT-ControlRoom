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
        private DateTime startTime_1;
        private DateTime endTime_1;
        private DateTime startTime_2;
        private DateTime endTime_2;
        private string status_1;
        private string type_1;
        private Coordinate coordinate_1;
        private Orientation orientation_1;
        private string celestial_body_1;
        private SpectraCyberModeTypeEnum specracyber_mode_type_1;
        private int telescipe_id_1;
        private int user_id_1;

        [TestInitialize]
        public void BuildUp()
        {
            // Initialize appointment entity
            appointment_1 = new Appointment();
            appointment_2 = new Appointment();

            // Initialize data for fields
            startTime_1 = DateTime.Now;
            endTime_1 = DateTime.Now.AddHours(1);
            startTime_2 = DateTime.Now.AddDays(1);
            endTime_2 = DateTime.Now.AddDays(1).AddHours(1);
            status_1 = AppointmentConstants.REQUESTED;
            type_1 = AppointmentTypeConstants.POINT;
            coordinate_1 = new Coordinate(20, 20);
            orientation_1 = new Orientation(20, 20);
            celestial_body_1 = CelestialBodyConstants.SUN;
            specracyber_mode_type_1 = SpectraCyberModeTypeEnum.CONTINUUM;
            telescipe_id_1 = 1;
            user_id_1 = 1;

            // Initialize fields we are testing against
            appointment_1.StartTime = startTime_1;
            appointment_1.EndTime = endTime_1;
            appointment_1.Status = status_1;
            appointment_1.Type = type_1; ;
            appointment_1.Coordinates.Add(coordinate_1);
            appointment_1.Orientation = orientation_1;
            appointment_1.CelestialBody = celestial_body_1;
            appointment_1.SpectraCyberModeType = specracyber_mode_type_1;
            appointment_1.TelescopeId = telescipe_id_1;
            appointment_1.UserId = user_id_1;

            appointment_2.StartTime = startTime_2;
            appointment_2.EndTime = endTime_2;

            
        }

        [TestMethod]
        public void TestGettersAndSetters()
        {
            Assert.AreEqual(startTime_1, appointment_1.StartTime);
            Assert.AreEqual(endTime_1, appointment_1.EndTime);
            Assert.AreEqual(status_1, appointment_1.Status);
            Assert.AreEqual(type_1, appointment_1.Type);
            Assert.AreEqual(coordinate_1, appointment_1.Coordinates.ToList()[0]);
            Assert.AreEqual(orientation_1, appointment_1.Orientation);
            Assert.AreEqual(celestial_body_1, appointment_1.CelestialBody);
            Assert.AreEqual(specracyber_mode_type_1, appointment_1.SpectraCyberModeType);
            Assert.AreEqual(telescipe_id_1, appointment_1.TelescopeId);
            Assert.AreEqual(user_id_1, appointment_1.UserId);
        }

        [TestMethod]
        public void TestComparable()
        {
            Assert.IsTrue(appointment_1 != appointment_2);
            Assert.IsTrue(appointment_1 < appointment_2);
        }
    }
}
