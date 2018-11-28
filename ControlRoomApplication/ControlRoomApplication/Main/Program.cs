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
            logger.Info("<--------------- Control Room Application Started --------------->");

            RTDbContext dbContext = new RTDbContext(AWSConstants.REMOTE_CONNECTION_STRING);

            ScaleModelPLC plc = new ScaleModelPLC();
            PLCController plcController = new PLCController(plc);
            Orientation orientation = new Orientation();

            ScaleRadioTelescope scaleModel = new ScaleRadioTelescope(plcController, orientation);

            ControlRoom CRoom = new ControlRoom(scaleModel, dbContext);
            ControlRoomController CRoomController = new ControlRoomController(CRoom);
            CRoomController.StartAppointment();

            logger.Info("<--------------- Control Room Application Terminated --------------->");
            Console.ReadKey();
        }

        private static readonly log4net.ILog logger =
            log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
    }
}