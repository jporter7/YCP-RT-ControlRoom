using System;
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

            // Instantiate the configuration manager and the database being used.
            ConfigManager = new ConfigurationManager();
            RTDbContext dbContext = new RTDbContext();

            AbstractPLC plc = ConfigManager.ConfigurePLC(args[0]);
            AbstractSpectraCyber spectraCyber = ConfigManager.ConfigureSpectraCyber(args[1]);
            AbstractRadioTelescope radioTelescope = ConfigManager.ConfigureRadioTelescope(args[2]);

            PLCController plcController = new PLCController(plc);
            // This line and a few things under it will need to be reviewed/refactored.
            AbstractSpectraCyberController spectraCyberController = new SpectraCyberController((SpectraCyber) spectraCyber, dbContext, 0);
            RadioTelescopeController rtController = new RadioTelescopeController(radioTelescope);

            ControlRoom cRoom = new ControlRoom(radioTelescope, rtController, dbContext);
            ControlRoomController crController = new ControlRoomController(cRoom);
            crController.StartAppointment();

            // End logging
            logger.Info("<--------------- Control Room Application Terminated --------------->");
            Console.ReadKey();
        }

        private static ConfigurationManager ConfigManager;
        private static readonly log4net.ILog logger =
            log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
    }
}