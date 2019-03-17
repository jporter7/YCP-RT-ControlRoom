using System;
using System.Collections.Generic;
using System.Windows.Forms;
using ControlRoomApplication.Controllers;
using ControlRoomApplication.Controllers.PLCCommunication;
using ControlRoomApplication.Controllers.RadioTelescopeControllers;
using ControlRoomApplication.Database.Operations;
using ControlRoomApplication.Entities;
using ControlRoomApplication.Entities.RadioTelescope;

namespace ControlRoomApplication.Main
{
    public class Program
    {
        [STAThread]
        public static void Main(string[] args)
        {
            Application.Run(new MainForm());

            List<KeyValuePair<AbstractRadioTelescope, AbstractPLCDriver>> AbstractRTDriverPairList = ConfigurationManager.BuildRadioTelescopeSeries(args);
            List<RadioTelescopeController> ProgramRTControllerList = new List<RadioTelescopeController>(AbstractRTDriverPairList.Count);
            List<AbstractPLCDriver> ProgramPLCDriverList = new List<AbstractPLCDriver>(AbstractRTDriverPairList.Count);
            List<ControlRoomController> ProgramControlRoomControllerList = new List<ControlRoomController>(AbstractRTDriverPairList.Count);

            for (int i = 0; i < AbstractRTDriverPairList.Count; i++)
            {
                ProgramRTControllerList.Add(new RadioTelescopeController(AbstractRTDriverPairList[i].Key));
                ProgramPLCDriverList.Add(AbstractRTDriverPairList[i].Value);
                ProgramControlRoomControllerList.Add(new ControlRoomController(new ControlRoom(ProgramRTControllerList[i])));

                ProgramPLCDriverList[i].StartAsyncAcceptingClients();

                ProgramRTControllerList[i].RadioTelescope.PlcController.ConnectToServer();

                ProgramControlRoomControllerList[i].Start();
            }

            for (int i = 0; i < AbstractRTDriverPairList.Count; i++)
            {
                ProgramRTControllerList[i].RadioTelescope.SpectraCyberController.BringDown();
                ProgramRTControllerList[i].RadioTelescope.PlcController.TerminateTCPServerConnection();
                ProgramPLCDriverList[i].RequestStopAsyncAcceptingClients();
            }


            // End logging
            logger.Info("<--------------- Control Room Application Terminated --------------->");
            Console.ReadKey();
        }
        
        private static readonly log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
    }
}