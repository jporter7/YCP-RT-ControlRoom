using System;
using System.Collections.Generic;
using ControlRoomApplication.Controllers.ScheduleController;
using ControlRoomApplication.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ControlRoomApplicationTest.EntityControllersTests
{
    [TestClass]
    public class ScheduleControllerTest
    {
        [TestInitialize]
        public void BuildUp()
        {
            var startTime1 = new DateTime(2020, 5, 1, 12, 0, 0);
            var endTime1 = new DateTime(2020, 5, 1, 1, 0, 0);
            var startTime2 = new DateTime(2020, 4, 1, 1, 0, 0);
            var endTime2 = new DateTime(2020, 4, 1, 2, 0, 0);
            var startTime3 = new DateTime(2020, 3, 11, 1, 0, 0);
            var endTime3 = new DateTime(2020, 3, 11, 2, 0, 0);

            Appointment appointment1 = new Appointment();
            appointment1.StartTime = startTime1;
            appointment1.EndTime = endTime1;
            appointment1.Id = 1;
            
            Appointment appointment2 = new Appointment();
            appointment2.StartTime = startTime2;
            appointment2.EndTime = endTime2;
            appointment2.Id = 2;

            Appointment appointment3 = new Appointment();
            appointment3.StartTime = startTime3;
            appointment3.EndTime = endTime3;
            appointment3.Id = 3;

            List<Appointment> appointments = 
                new List<Appointment>()
                {
                    appointment1,
                    appointment2,
                    appointment3
                };

            Schedule = new Schedule();
            Schedule.Appointments = appointments;
            Schedule.CurrentDateTime = DateTime.Now;

            Controller = new ScheduleController(Schedule);
        }

        [TestMethod]
        public void TestGetNextMethod()
        {
            Appointment appointment = Controller.GetNextAppointment();

            Assert.AreEqual(appointment.Id, 1);
        }

        public Schedule Schedule { get; set; }
        public ScheduleController Controller { get; set; }
    }
}
