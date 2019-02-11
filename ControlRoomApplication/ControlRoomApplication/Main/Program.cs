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
            AbstractPLC plc;
            AbstractSpectraCyberController spectraCyberController;
            AbstractRadioTelescope radioTelescope;


            if (args.Length > 0)
            {
                plc = ConfigManager.ConfigurePLC(args[0]);
            }
            else
            {
                plc = ConfigManager.ConfigurePLC("");
            }

            if (args.Length > 1)
            {
                spectraCyberController = ConfigManager.ConfigureSpectraCyberController(args[1], dbContext);
            }
            else
            {
                spectraCyberController = ConfigManager.ConfigureSpectraCyberController("", dbContext);
            }

            if (args.Length > 2)
            {
                radioTelescope = ConfigManager.ConfigureRadioTelescope(args[2], spectraCyberController, plc);
            }
            else
            {
                radioTelescope = ConfigManager.ConfigureRadioTelescope("", spectraCyberController, plc);
            }


            PLCController plcController = new PLCController(plc);
            RadioTelescopeController rtController = new RadioTelescopeController(radioTelescope);

            ControlRoom cRoom = new ControlRoom(rtController, dbContext);
            ControlRoomController crController = new ControlRoomController(cRoom);
            crController.Start();

            // End logging
            logger.Info("<--------------- Control Room Application Terminated --------------->");
            Console.ReadKey();
        }

        private static ConfigurationManager ConfigManager;
        private static readonly log4net.ILog logger =
            log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
    }
}