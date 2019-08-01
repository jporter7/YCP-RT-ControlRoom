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
            // Application.Run(new MainForm());




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
            ControlRoomApplication.Controllers.BlkHeadUcontroler.MicroControlerControler.AsynchronousSocketListener.BringUp();ProductionMCUDriver
//*/
            //*

            Controllers.ProductionMCUDriver MCUDriver = new ControlRoomApplication.Controllers.ProductionMCUDriver("192.168.0.50", 502);
            int gearedSpeedAZ = 2000, gearedSpeedEL = 2000;
            ushort homeTimeoutSecondsElevation = 0, homeTimeoutSecondsAzimuth = 0;
            MCUDriver.configureaxies(gearedSpeedAZ, gearedSpeedEL, homeTimeoutSecondsAzimuth, homeTimeoutSecondsElevation);
            //MCUDriver.


            Console.WriteLine(ushort.MaxValue);
             new Thread(new ThreadStart(() => {
                 Console.WriteLine("---------------------------------");
                 Controllers.ProductionPLCDriver APLCDriver = new ControlRoomApplication.Controllers.ProductionPLCDriver("192.168.0.50", 502);
                 ushort i = 0,j=1024;//7500



                 //return;
                 while (true)
                 {
                     Thread.Sleep(5000);

                     ushort[] inreg,outreg;
                     try
                     {
                         ushort positionTranslationELInt=1000, positionTranslationELMSW=0;
                         ushort positionTranslationAZInt = 1000, positionTranslationAZMSW = 0;
                         ushort aCCELERATION = 5, programmedPeakSpeedAZInt = 1200;

                         ushort[] data2 = {             0x0,                                        // Reserved 
                                                        0x0,0x0,0x0,0x0,0x0,0x0,0x0,0x0,0x0,0x0,0x0,0x0,0x0,0x0,0x0,0x0,                                        // Denotes a relative linear interpolated move and a Trapezoidal S-Curve profile 
                         };

                         APLCDriver.PLCModbusMaster.WriteMultipleRegisters(j, data2);
                         Thread.Sleep(100);
                         ushort[] data = {              0,                                            // Reserved 
                                                        0x403,                                        // Denotes a relative linear interpolated move and a Trapezoidal S-Curve profile
                                                        (ushort)(programmedPeakSpeedAZInt >> 0x10),   // MSW for azimuth speed
                                                        (ushort)(programmedPeakSpeedAZInt & 0xFFFF),  // LSW for azimuth speed
                                                        aCCELERATION,                                 // Acceleration for azimuth
                                                        aCCELERATION,                                 // Deceleration for azimuth
                                                        positionTranslationAZMSW,                     // MSW for azimuth position 6
                                                        (ushort)(positionTranslationAZInt & 0xFFFF),  // LSW for azimuth position 7
                                                        0,                                            // Reserved 
                                                        0,                                            // Reserved 
                                                        0,                                            // Reserved 
                                                        0,                                            // Reserved 
                                                        positionTranslationELMSW,                     // MSW for elevation position 12
                                                        (ushort)(positionTranslationELInt & 0xFFFF),  // LSW for elevation position 13
                                                        0,                                            // Reserved 
                                                        0,                                            // Reserved 
                                                        0,                                            // Reserved 
                                                        0,                                            // Reserved 
                                                        0,                                            // Reserved
                                                        0                                             // Reserved 
                         };

                         APLCDriver.PLCModbusMaster.WriteMultipleRegisters(j, data);
                         // i = 25;
                         inreg = APLCDriver.readregisters(i, (ushort)20);
                         outreg = APLCDriver.readregisters(j, (ushort)20);
                     }
                     catch(Exception e)
                     {
                         Console.WriteLine("read reg {0,6} failed",i);
                         continue;
                     }
                     if (inreg == null) { return; }
                     string outstr="";
                     for (int v=0; v < inreg.Length; v++)
                     {
                         outstr += Convert.ToString(inreg[v], 2).PadLeft(16).Replace(" ", "0") + " , ";
                     }
                     outstr += "\n";
                     for (int v = 0; v < outreg.Length; v++)
                     {
                         outstr += Convert.ToString(outreg[v], 2).PadLeft(16).Replace(" ", "0") + " , ";
                     }
                     Console.WriteLine(outstr);
                     //Console.WriteLine("read reg {0,6} suceded {1}   *****************************************************", i, outstr);
                     //break;
                     //Thread.Sleep(10);
                 }
             })).Start();

 //*/
        /*
            Controllers.ProductionPLCDriver APLCDriver;
            new Thread(new ThreadStart(() => {
                Console.WriteLine("---------------------------------");
                APLCDriver = new ControlRoomApplication.Controllers.ProductionPLCDriver("192.168.0.2", 502);
                Thread.Sleep(Timeout.Infinite);
            })).Start();
            ///

            new Thread(new ThreadStart(() => {
                bool up = true;
                ushort i = 0;
                Random rand = new Random();
                Thread.Sleep(10000);
                while (true)
                {
                    Thread.Sleep(1000);
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
                   
                }
            })).Start();
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