using System;
using ControlRoomApplication.Controllers;
using ControlRoomApplication.Controllers.PLCController;
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
            // Testing logging
            log.Info("<--------------- Control Room Application Started --------------->");

            RTDbContext dbContext = new RTDbContext(AWSConstants.REMOTE_CONNECTION_STRING);

            PLC plc = new PLC();
            PLCController plcController = new PLCController(plc);
            Orientation orientation = new Orientation();

            ScaleRadioTelescope scaleModel = new ScaleRadioTelescope(plcController, orientation);

            ControlRoom CRoom = new ControlRoom(scaleModel, dbContext);
            ControlRoomController CRoomController = new ControlRoomController(CRoom);
            CRoomController.StartAppointment();

            Console.WriteLine("Post-appointment run: ");
            foreach (Appointment app in dbContext.Appointments)
            {
                Console.WriteLine(string.Join(Environment.NewLine,
                    $"Appointment {app.Id} has a start time of {app.StartTime} and an end time of {app.EndTime}... {app.CoordinateId}",
                    $"Appointment status: {app.Status}",
                    ""));
            }

            log.Info("<--------------- Control Room Application Terminated --------------->");
            Console.ReadKey();
        }

        private static readonly log4net.ILog log =
            log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
    }
}