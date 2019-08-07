﻿using System;
using System.Net;
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
                    //Console.WriteLine("---------------------------------");
                    ControlRoomApplication.Entities.Orientation one = Controllers.BlkHeadUcontroler.EncoderReader.GetCurentOrientation();
                    if (one != null)
                    {
                        Console.WriteLine(one.Azimuth + " " + one.Elevation);
                    }
                    Thread.Sleep(100);
                }
            })).Start();
            //ControlRoomApplication.Controllers.BlkHeadUcontroler.MicroControlerControler.BringUp();
//*/
            /*
            runer();
            async void runer()
            {
                Controllers.ProductionPLCDriver plc = new ControlRoomApplication.Controllers.ProductionPLCDriver(("192.168.0.02"), 502);
                new Thread(plc.HandleClientManagementThread).Start();

                Task task = Task.Delay(15000);
                await task;
                int gearedSpeedAZ = 500, gearedSpeedEL = 50;
                ushort homeTimeoutSecondsElevation = 0, homeTimeoutSecondsAzimuth = 0;

                Controllers.ProductionMCUDriver MCUDriver = new ControlRoomApplication.Controllers.ProductionMCUDriver("192.168.0.50", 502);
               // MCUDriver.configure_axies(gearedSpeedAZ, gearedSpeedEL, homeTimeoutSecondsAzimuth, homeTimeoutSecondsElevation);
                task = Task.Delay(3000);
                await task;
               // MCUDriver.SendEmptyMoveCommand();

                Console.WriteLine(ushort.MaxValue);
                new Thread(new ThreadStart(async () => {// IPAddress.Parse
                    Console.WriteLine("---------------------------------");
                    Thread.Sleep(1000);
                    ushort i = 0, j = 1024;//7500
                    plc.configure_muc(gearedSpeedAZ, gearedSpeedEL, homeTimeoutSecondsAzimuth, homeTimeoutSecondsElevation);
                    task = Task.Delay(10000);
                    await task;
                    //Thread.Sleep(10000);

                    //return;
                    while (true)
                    {
                        Thread.Sleep(5000);
                        ushort[] datax = new ushort[20];
                        datax[1] = 3;
                        datax[11] = 3;
                        //Console.WriteLine(datax.Length);
                        ushort[] inreg, outreg;

                        inreg = MCUDriver.MCUModbusMaster.ReadHoldingRegisters(i, (ushort)20);
                        outreg = MCUDriver.MCUModbusMaster.ReadHoldingRegisters(j, (ushort)20);
                        try
                        {
                            ushort positionTranslationELInt = 1000, positionTranslationELMSW = 0;
                            ushort positionTranslationAZInt = 1000, positionTranslationAZMSW = 0;
                            ushort aCCELERATION = 5, programmedPeakSpeedAZInt = 1200;

                            inreg = MCUDriver.MCUModbusMaster.ReadHoldingRegisters(i, (ushort)20);
                            outreg = MCUDriver.MCUModbusMaster.ReadHoldingRegisters(j, (ushort)20);
                            //Thread.Sleep(1000);
                            ushort[] data = {              0,                                            // Reserved 
                                                        0x0403,                                        // Denotes a relative linear interpolated move and a Trapezoidal S-Curve profile
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

                           await plc.sendmovecomand(programmedPeakSpeedAZInt, aCCELERATION, positionTranslationAZInt, positionTranslationELInt);

                            // MCUDriver.MCUModbusMaster.WriteMultipleRegisters(j, data);
                            // Thread.Sleep(10);
                            // MCUDriver.MCUModbusMaster.WriteMultipleRegisters(j, datax);

                            // data[1] = 0x403;
                            // APLCDriver.PLCModbusMaster.WriteMultipleRegisters(j, data);
                            // i = 25;
                            //MCUDriver.configureSingleaxys(gearedSpeedAZ, (ushort)gearedSpeedEL,0);
                            //MCUDriver.configureaxies(gearedSpeedAZ, gearedSpeedEL, homeTimeoutSecondsAzimuth, homeTimeoutSecondsElevation);
                            //MCUDriver.configureSingleaxys(gearedSpeedAZ, homeTimeoutSecondsAzimuth, 0);
                            //System.Threading.Thread.Sleep(50);
                            //MCUDriver.configureSingleaxys(gearedSpeedEL, homeTimeoutSecondsElevation, 1);
                            inreg = MCUDriver.MCUModbusMaster.ReadHoldingRegisters(i, (ushort)20);
                            outreg = MCUDriver.MCUModbusMaster.ReadHoldingRegisters(j, (ushort)20);
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(e);
                            Console.WriteLine("read reg {0,6} failed", i);
                            continue;
                        }
                        if (inreg == null) { return; }
                        string outstr = " inreg";
                        for (int v = 0; v < inreg.Length; v++)
                        {
                            //outstr += Convert.ToString(inreg[v], 2).PadLeft(16).Replace(" ", "0") + " , ";
                            outstr += Convert.ToString(inreg[v], 10).PadLeft(8) + ",";
                        }
                        outstr += "\noutreg";
                        for (int v = 0; v < outreg.Length; v++)
                        {
                            //outstr += Convert.ToString(outreg[v], 2).PadLeft(16).Replace(" ", "0") + " , ";
                            outstr += Convert.ToString(outreg[v], 10).PadLeft(8) + ",";
                        }
                        Console.WriteLine(outstr);
                        //Console.WriteLine("read reg {0,6} suceded {1}   *****************************************************", i, outstr);
                        //break;
                        //Thread.Sleep(10);
                    }
                })).Start();
            }
            

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