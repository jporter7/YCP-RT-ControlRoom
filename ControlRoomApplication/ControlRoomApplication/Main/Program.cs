using System;
using System.Threading;
using System.Threading.Tasks;
//using System.Threading;
using System.Windows.Forms;
//using ControlRoomApplication.Constants;
//using ControlRoomApplication.Entities;
//using ControlRoomApplication.Controllers;

namespace ControlRoomApplication.Main
{
    public class Program
    {
        private static readonly log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        [STAThread]
        public static void Main(string[] args)
        {
            Application.Run(new MainForm());




            /*
            new Thread(new ThreadStart(() => {
                while (true)
                {
                    Console.WriteLine("---------------------------------");
                    ControlRoomApplication.Entities.Orientation one = ControlRoomApplication.Controllers.BlkHeadUcontroler.EncoderReader.GetCurentOrientation();
                    if (one != null)
                    {
                        Console.WriteLine(one.Azimuth + " " + one.Elevation);
                    }
                    Thread.Sleep(1000);
                }
            })).Start();
            ControlRoomApplication.Controllers.BlkHeadUcontroler.MicroControlerControler.AsynchronousSocketListener.BringUp();
//*/
            /*
             Console.WriteLine(ushort.MaxValue);
             new Thread(new ThreadStart(() => {
                 Console.WriteLine("---------------------------------");
                 Controllers.ProductionPLCDriver APLCDriver = new ControlRoomApplication.Controllers.ProductionPLCDriver("192.168.0.2", 502);
                 ushort i = 0;//7500
                 while (i< ushort.MaxValue)
                 {
                     //Thread.Sleep(1000);

                     ushort[] one;
                     try
                     {
                        // i = 25;
                         one = APLCDriver.readregisters(i, (ushort)1);
                     }
                     catch(Exception e)
                     {
                         Console.WriteLine("read reg {0,6} failed",i);
                         continue;
                     }
                     finally { i++; }
                     if (one == null) { return; }
                     string outp="";
                     for (int v=0; v < one.Length; v++)
                     {
                         outp += Convert.ToString(one[v], 2).PadLeft(16).Replace(" ", "0") + " , ";
                     }
                     Console.WriteLine("read reg {0,6} suceded {1}   *****************************************************", i,outp);
                     break;
                     //Thread.Sleep(10);
                 }
             })).Start();

 //*/
        /*
            Controllers.ProductionPLCDriver APLCDriver;
            //new Thread(new ThreadStart(() => {
                Console.WriteLine("---------------------------------");
                APLCDriver = new ControlRoomApplication.Controllers.ProductionPLCDriver("192.168.0.2", 502);
                //.Sleep(Timeout.Infinite);
           // })).Start();
            ///

            new Thread(new ThreadStart(() => {
                bool up = true;
                ushort i = 0;
                Random rand = new Random();
                while (true)
                {
                    string outp = "";
                    for (ushort v = 0; v < 10; v++)
                    {
                        outp += Convert.ToString(APLCDriver.readregval(v), 2).PadLeft(16).Replace(" ", "0") + " , ";
                    }
                    Console.WriteLine(outp);
                    if (up) { i++; } else { i--; }
                    if (i>=10){ up = false; }
                    if (i <=0){ up = true; }
                    APLCDriver.setregvalue(i, (ushort) (rand.Next() /(2^16) ));
                    Thread.Sleep(1000);
                }
            })).Start();
            ///
           // var[] list = new var[100];
            var list = new dynamic[100];
            for (int i=0;i< list.Length;i++)
            {
                list[i] = new { val = 1234, time=98342341342};

            }
            var data = new { type = "cwg", data = list };


            Console.WriteLine(data);
            //*/

            //string localhostIP = PLCConstants.LOCAL_HOST_IP;
            //int localhostPort = PLCConstants.PORT_8080;
            //string mcuIP = MCUConstants.ACTUAL_MCU_IP_ADDRESS;
            //int mcuPort = MCUConstants.ACTUAL_MCU_MODBUS_TCP_PORT;

            //RadioTelescopeController RTController = new RadioTelescopeController(
            //    new RadioTelescope(
            //        new SpectraCyberSimulatorController(new SpectraCyberSimulator()),
            //        new PLCClientCommunicationHandler(localhostIP, localhostPort),
            //        MiscellaneousConstants.JOHN_RUDY_PARK,
            //        new Orientation(0, 90)
            //    )
            //);

            //ProductionPLCDriver PLCDriver = new ProductionPLCDriver(localhostIP, localhostPort);

            //if (!PLCDriver.StartAsyncAcceptingClients())
            //{
            //    Console.WriteLine("[Program] ERROR starting to async accept clients.");
            //    return;
            //}

            //RTController.RadioTelescope.PLCClient.ConnectToServer();
            //Thread.Sleep(100);

            //PLCDriver.FetchMCUModbusSlave(mcuIP, mcuPort);
            //Thread.Sleep(100);

            //if (RTController.TestCommunication())
            //{
            //    Console.WriteLine("[Program] Successfully communicated with MCU over Modbus TCP/IP!");
            //}
            //else
            //{
            //    Console.WriteLine("[Program] ERROR communicating with MCU over Modbus TCP/IP.");
            //    return;
            //}

            //Thread.Sleep(100);

            //if (RTController.ConfigureRadioTelescope(100, 100, 210, 210))
            //{
            //    Console.WriteLine("[Program] Successfully configured MCU over Modbus TCP/IP!");
            //}
            //else
            //{
            //    Console.WriteLine("[Program] ERROR configuring MCU over Modbus TCP/IP.");
            //    return;
            //}

            //Thread.Sleep(3000);

            //if (RTController.StartRadioTelescopeAzimuthJog(166667, false))
            //{
            //    Console.WriteLine("[Program] Successfully sent jog move request to MCU over Modbus TCP/IP!");
            //}
            //else
            //{
            //    Console.WriteLine("[Program] ERROR sending jog move request to MCU over Modbus TCP/IP.");
            //    return;
            //}

            //for (int i = 0; i < 5; i++)
            //{
            //    Thread.Sleep(2000);
            //    Console.WriteLine("Current at i=" + i.ToString() + ": " + RTController.GetCurrentOrientation().Azimuth.ToString());
            //}

            //if (PLCDriver.SendHoldMoveCommand())
            //{
            //    Console.WriteLine("[Program] Successfully sent hold move request to MCU over Modbus TCP/IP!");
            //}
            //else
            //{
            //    Console.WriteLine("[Program] ERROR sending hold move request to MCU over Modbus TCP/IP.");
            //    return;
            //}

            //Console.WriteLine("[Program] Done.");

            //Thread.Sleep(10000);
        }
    }
}