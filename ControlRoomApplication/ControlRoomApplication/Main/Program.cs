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
                        // Testing logging
                        logger.Info("<--------------- Control Room Application Started --------------->");
            
                        RTDbContext dbContext = new RTDbContext(AWSConstants.REMOTE_CONNECTION_STRING);
            
                        ScaleModelPLC plc = new ScaleModelPLC();
                        PLCController plcController = new PLCController(plc);
                        Orientation orientation = new Orientation();
                        orientation.Azimuth = 0;
                        orientation.Elevation = 0;
                        //SpectraCyber spectraCyber = new SpectraCyber();
                        //SpectraCyberController spectraCyberController = new SpectraCyberController(spectraCyber, dbContext, 0);
                        //spectraCyberController.SetSpectraCyberModeType(SpectraCyberModeTypeEnum.CONTINUUM);
                        ScaleRadioTelescope scaleModel = new ScaleRadioTelescope(plcController);//, spectraCyberController, orientation);
                        RadioTelescopeController rtController = new RadioTelescopeController(scaleModel);
                        //scaleModel.SpectraCyberController.BringUp();
                        //scaleModel.CurrentOrientation = new Orientation();
            
                        //orientation.Azimuth = 150;
                        //orientation.Elevation = 50;
            
                        //rtController.MoveRadioTelescope(orientation);
            
                        //HardwareFlags.COM3 = true;
                        //HardwareFlags.COM4 = true;
                        //Thread.Sleep(10000);
            
                        //orientation.Azimuth = -150;
                        //orientation.Elevation = -50;
            
                        //rtController.MoveRadioTelescope(orientation);
            
                        ControlRoom CRoom = new ControlRoom(scaleModel, rtController, dbContext);
                        ControlRoomController CRoomController = new ControlRoomController(CRoom);
                        CRoomController.StartAppointment();
            
                        //scaleModel.SpectraCyberController.BringDown();
                        logger.Info("<--------------- Control Room Application Terminated --------------->");
                        Console.ReadKey();
            /*
            Console.WriteLine("Starting...");
            Console.WriteLine("Initial number of children: " + HeartbeatTrackerContainer.GetNumberOfChildren().ToString());

            Console.WriteLine("Creating actual SpectraCyber controller...");
            SpectraCyberController controller0 = new SpectraCyberController(new SpectraCyber(), null, -1);
            controller0.SetSpectraCyberModeType(SpectraCyberModeTypeEnum.CONTINUUM);
            controller0.BringUpSpectraCyber();
            controller0.ScheduleScan(400, 1000, false);

            Console.WriteLine("Creating simulated SpectraCyber controller...");
            FailableSpectraCyberSimulatorController controller1 = new FailableSpectraCyberSimulatorController(new SpectraCyberSimulator(), null, -1);
            controller1.SetSpectraCyberModeType(SpectraCyberModeTypeEnum.SPECTRAL);
            controller1.BringUpSpectraCyber();
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

        private static readonly log4net.ILog logger =
            log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
    }
}