using System;
using System.Threading;
using ControlRoomApplication.Constants;
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

            /*
			Console.WriteLine("Starting...");
			Console.WriteLine("Initial number of children: " + HeartbeatTrackerContainer.GetNumberOfChildren().ToString());
			
			Console.WriteLine("Creating actual SpectraCyber controller...");
			SpectraCyberController controller0 = new SpectraCyberController(new SpectraCyber(), null, -1);
			controller0.SetSpectraCyberModeType(SpectraCyberModeTypeEnum.CONTINUUM);
			controller0.BringUp(0);
			controller0.ScheduleScan(400, 1000, false);
			
			Console.WriteLine("Creating simulated SpectraCyber controller...");
			FailableSpectraCyberSimulatorController controller1 = new FailableSpectraCyberSimulatorController(new SpectraCyberSimulator(), null, -1);
			controller1.SetSpectraCyberModeType(SpectraCyberModeTypeEnum.SPECTRAL);
			controller1.BringUp(0);
			controller1.StartScan();
			
			Console.WriteLine("Number of children now: " + HeartbeatTrackerContainer.GetNumberOfChildren().ToString());
			Thread.Sleep(1000);
			
			int MSCheckRate = 250;
			Console.WriteLine("Checking for status every " + MSCheckRate.ToString() + " milliseconds...");
			while (true)
			{
				Thread.Sleep(MSCheckRate);
			
				bool ChildrenAreAlive = HeartbeatTrackerContainer.ChildrenAreAlive();
				Console.WriteLine("Alive [" + HeartbeatTrackerContainer.GetNumberOfChildren().ToString() + "]: " + ChildrenAreAlive.ToString());
			
				if (!ChildrenAreAlive)
				{
					Console.WriteLine("ERROR: not all children are alive.");
					break;
				}
			}
			
			Thread.Sleep(1000);
			Console.WriteLine("Attempting to kill remaining component(s)...");
			Thread.Sleep(1000);
			
			HeartbeatTrackerContainer.SafelyKillHeartbeatComponents();
			Console.WriteLine("Successfull...");
			Thread.Sleep(3000);
			*/
        }

        private static ConfigurationManager ConfigManager;
        private static readonly log4net.ILog logger =
            log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
    }
}