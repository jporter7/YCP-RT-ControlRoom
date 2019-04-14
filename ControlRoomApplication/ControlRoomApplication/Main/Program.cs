using System;
using System.Threading;
//using System.Windows.Forms;
using ControlRoomApplication.Constants;
using ControlRoomApplication.Entities;
using ControlRoomApplication.Controllers;

namespace ControlRoomApplication.Main
{
    public class Program
    {
        private static readonly log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        [STAThread]
        public static void Main(string[] args)
        {
            //Application.Run(new MainForm());

            string ip = PLCConstants.LOCAL_HOST_IP;
            int port = 8080;

            RadioTelescopeController RTController = new RadioTelescopeController(
                new RadioTelescope(
                    new SpectraCyberSimulatorController(new SpectraCyberSimulator()),
                    new PLCClientCommunicationHandler(ip, port),
                    MiscellaneousConstants.JOHN_RUDY_PARK,
                    new Orientation(0, 90)
                )
            );

            IntoTheSpiderversePLCDriver PLCDriver = new IntoTheSpiderversePLCDriver(ip, port, "192.168.0.50", 29999);
            PLCDriver.StartMCUCommsThreadRoutine();

            if (!PLCDriver.StartAsyncAcceptingClients())
            {
                Console.WriteLine("[Program] ERROR starting to async accept clients.");
                return;
            }

            RTController.RadioTelescope.PLCClient.ConnectToServer();

            Thread.Sleep(1000);

            if (!RTController.ConfigureRadioTelescope(500, 500, 210, 210))
            {
                Console.WriteLine("[Program] ERROR sending configuration to MCU.");
                return;
            }

            Console.WriteLine("Done.");
        }
    }
}