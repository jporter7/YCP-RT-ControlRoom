using System;
using System.Collections.Generic;
using System.Threading;
using ControlRoomApplication.Controllers;
using ControlRoomApplication.Controllers.PLCCommunication;
using ControlRoomApplication.Controllers.RadioTelescopeControllers;
using ControlRoomApplication.Controllers.SpectraCyberController;
using ControlRoomApplication.Database.Operations;
using ControlRoomApplication.Entities;
using ControlRoomApplication.Entities.RadioTelescope;
using ControlRoomApplication.Constants;

namespace ControlRoomApplication.Main
{
    public class Program
    {
        [STAThread]
        public static void Main(string[] args)
        {
            // Begin logging
            logger.Info("<--------------- Control Room Application Started --------------->");

            // Instantiate the database being used.
            RTDbContext dbContext = new RTDbContext();

            DatabaseOperations.InitializeLocalConnectionOnly();
            DatabaseOperations.PopulateLocalDatabase();
            Console.WriteLine("Local database populated.");
            Console.WriteLine("Number of Appointments: " + DatabaseOperations.GetListOfAppointments().Count);

            List<KeyValuePair<AbstractRadioTelescope, AbstractPLCDriver>> AbstractRTDriverPairList = ConfigurationManager.BuildRadioTelescopeSeries(args, dbContext);
            List<RadioTelescopeController> ProgramRTControllerList = new List<RadioTelescopeController>(AbstractRTDriverPairList.Count);
            List<AbstractPLCDriver> ProgramPLCDriverList = new List<AbstractPLCDriver>(AbstractRTDriverPairList.Count);
            List<ControlRoomController> ProgramControlRoomControllerList = new List<ControlRoomController>(AbstractRTDriverPairList.Count);

            for (int i = 0; i < AbstractRTDriverPairList.Count; i++)
            {
                ProgramRTControllerList.Add(new RadioTelescopeController(AbstractRTDriverPairList[i].Key));
                ProgramPLCDriverList.Add(AbstractRTDriverPairList[i].Value);
                ProgramControlRoomControllerList.Add(new ControlRoomController(new ControlRoom(ProgramRTControllerList[i], dbContext)));

                ProgramPLCDriverList[i].StartAsyncAcceptingClients();

                ProgramRTControllerList[i].RadioTelescope.PlcController.ConnectToServer();

                ProgramRTControllerList[i].RadioTelescope.SpectraCyberController.BringUp(0);
                ProgramRTControllerList[i].RadioTelescope.SpectraCyberController.SetSpectraCyberModeType(SpectraCyberModeTypeEnum.CONTINUUM);

                ProgramControlRoomControllerList[i].Start();
            }

            for (int i = 0; i < AbstractRTDriverPairList.Count; i++)
            {
                ProgramRTControllerList[i].RadioTelescope.SpectraCyberController.BringDown();
                ProgramRTControllerList[i].RadioTelescope.PlcController.TerminateTCPServerConnection();
                ProgramPLCDriverList[i].RequestStopAsyncAcceptingClients();
            }

            dbContext.Dispose();
            DatabaseOperations.DisposeLocalDatabaseOnly();

            // End logging
            logger.Info("<--------------- Control Room Application Terminated --------------->");
            Console.ReadKey();

            Thread.Sleep(5000);
        }
        
        private static readonly log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
    }
}