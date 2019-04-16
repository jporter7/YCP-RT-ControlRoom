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

            string localhostIP = PLCConstants.LOCAL_HOST_IP;
            int localhostPort = PLCConstants.PORT_8080;
            string mcuIP = MCUConstants.ACTUAL_MCU_IP_ADDRESS;
            int mcuPort = MCUConstants.ACTUAL_MCU_MODBUS_TCP_PORT;

            RadioTelescopeController RTController = new RadioTelescopeController(
                new RadioTelescope(
                    new SpectraCyberSimulatorController(new SpectraCyberSimulator()),
                    new PLCClientCommunicationHandler(localhostIP, localhostPort),
                    MiscellaneousConstants.JOHN_RUDY_PARK,
                    new Orientation(0, 90)
                )
            );

            IntoTheSpiderversePLCDriver PLCDriver = new IntoTheSpiderversePLCDriver(localhostIP, localhostPort);

            if (!PLCDriver.StartAsyncAcceptingClients())
            {
                Console.WriteLine("[Program] ERROR starting to async accept clients.");
                return;
            }

            RTController.RadioTelescope.PLCClient.ConnectToServer();
            Thread.Sleep(100);

            PLCDriver.FetchMCUModbusSlave(mcuIP, mcuPort);
            Thread.Sleep(100);

            if (RTController.TestCommunication())
            {
                Console.WriteLine("[Program] Successfully communicated with MCU over Modbus TCP/IP!");
            }
            else
            {
                Console.WriteLine("[Program] ERROR communicating with MCU over Modbus TCP/IP.");
                return;
            }

            Thread.Sleep(100);

            if (RTController.ConfigureRadioTelescope(100, 100, 210, 210))
            {
                Console.WriteLine("[Program] Successfully configured MCU over Modbus TCP/IP!");
            }
            else
            {
                Console.WriteLine("[Program] ERROR configuring MCU over Modbus TCP/IP.");
                return;
            }

            Thread.Sleep(4000);

            if (RTController.MoveRadioTelescope(new Orientation(30, 0)))
            {
                Console.WriteLine("[Program] Successfully sent relative move request to MCU over Modbus TCP/IP!");
            }
            else
            {
                Console.WriteLine("[Program] ERROR sending relative move request to MCU over Modbus TCP/IP.");
                return;
            }

            Thread.Sleep(2500);

            if (PLCDriver.SendHoldMoveCommand())
            {
                Console.WriteLine("[Program] Successfully sent hold move request to MCU over Modbus TCP/IP!");
            }
            else
            {
                Console.WriteLine("[Program] ERROR sending hold move request to MCU over Modbus TCP/IP.");
                return;
            }

            Console.WriteLine("[Program] Done.");
        }
    }
}