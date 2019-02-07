using System;
using System.Collections.Generic;
using System.Linq;
using ControlRoomApplication.Controllers;
using ControlRoomApplication.Controllers.PLCController;
using ControlRoomApplication.Controllers.RadioTelescopeControllers;
using ControlRoomApplication.Controllers.SpectraCyberController;
using ControlRoomApplication.Entities;
using ControlRoomApplication.Entities.Plc;
using ControlRoomApplication.Entities.RadioTelescope;

namespace ControlRoomApplication.Main
{
    public class Program
    {
        [STAThread]
        public static void Main(string[] args)
        {
            // Begin logging
            logger.Info("<--------------- Control Room Application Started --------------->");

            RTDbContext dbContext = new RTDbContext(); // AWSConstants.REMOTE_CONNECTION_STRING);

            var plc = new ScaleModelPLC();
            var plcController = new PLCController(plc);
            var scaleModel = new ScaleRadioTelescope(plcController);
            var rtController = new RadioTelescopeController(scaleModel);

            ControlRoom CRoom = new ControlRoom(rtController, dbContext);
            ControlRoomController CRoomController = new ControlRoomController(CRoom);

            // -------------------------------- TEST --------------------------------  
            // Coordinate
            Coordinate coord = new Coordinate(0, 0);
            dbContext.Coordinates.Add(coord);
            dbContext.SaveChanges();
            var coords = dbContext.Coordinates.ToList();

            // Appointment
            var app = new Appointment();
            app.UserId = 1;
            app.StartTime = DateTime.Now.AddMinutes(1);
            app.EndTime = DateTime.Now.AddMinutes(2);
            app.CoordinateId = 1;
            app.TelescopeId = 1;
            app.Status = "GOOD";
            List<Appointment> apps = CRoom.Appointments;
            apps.Add(app);
            CRoom.Appointments = apps;
            var result_apps = CRoom.Appointments;
            // -------------------------------- TEST -------------------------------- 

            CRoomController.Start();

            logger.Info("<--------------- Control Room Application Terminated --------------->");
            Console.ReadKey();
        }

        private static readonly log4net.ILog logger =
            log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
    }
}