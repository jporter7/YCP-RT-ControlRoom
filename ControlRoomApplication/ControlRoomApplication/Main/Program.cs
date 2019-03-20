using System;
using System.Collections.Generic;
using System.Threading;
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
        private static readonly log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        [STAThread]
        public static void Main(string[] args)
        {
            // Begin logging
            logger.Info("<--------------- Control Room Application Started --------------->");

            // Instantiate the database being used.
            ControlRoomController MainControlRoomController = new ControlRoomController(new ControlRoom());

            // Given the input command line arguments, generate the control room's RTs, its controllers, and the PLC drivers
            List<KeyValuePair<RadioTelescope, AbstractPLCDriver>> AbstractRTDriverPairList = ConfigurationManager.BuildRadioTelescopeSeries(args, true);

            ConfigurationManager.ConfigureLocalDatabase(AbstractRTDriverPairList.Count);
            Console.WriteLine("[Program] Local database populated. Number of Appointments: " + DatabaseOperations.GetTotalAppointmentCount());

            // Initialize the lists that the input list will populate
            List<RadioTelescopeController> ProgramRTControllerList = new List<RadioTelescopeController>(AbstractRTDriverPairList.Count);
            List<AbstractPLCDriver> ProgramPLCDriverList = new List<AbstractPLCDriver>(AbstractRTDriverPairList.Count);

            // Populate those lists and call any bring up sequences
            for (int i = 0; i < AbstractRTDriverPairList.Count; i++)
            {
                ProgramRTControllerList.Add(new RadioTelescopeController(AbstractRTDriverPairList[i].Key));
                ProgramPLCDriverList.Add(AbstractRTDriverPairList[i].Value);

                ProgramRTControllerList[i].RadioTelescope.SpectraCyberController.BringUp();
                ProgramPLCDriverList[i].StartAsyncAcceptingClients();
                ProgramRTControllerList[i].RadioTelescope.PLCClient.ConnectToServer();

                MainControlRoomController.AddRadioTelescopeController(ProgramRTControllerList[i]);
            }

            int ErrorIndex = -1;
            // Start each RT controller's threaded management
            int z = 0;
            foreach (RadioTelescopeControllerManagementThread ManagementThread in MainControlRoomController.CRoom.RTControllerManagementThreads)
            {
                int RT_ID = ManagementThread.RadioTelescopeID;
                List<Appointment> AllAppointments = DatabaseOperations.GetListOfAppointmentsForRadioTelescope(RT_ID);

                Console.WriteLine("[Program] Attempting to queue " + AllAppointments.Count.ToString() + " appointments for RT with ID " + RT_ID.ToString());

                foreach (Appointment appt in AllAppointments)
                {
                    Console.WriteLine("\t[" + appt.Id + "] " + appt.StartTime.ToString() + " -> " + appt.EndTime.ToString());
                }

                if (ManagementThread.Start())
                {
                    Console.WriteLine("[Program] Successfully started RT controller management thread [" + RT_ID.ToString() + "]");
                }
                else
                {
                    Console.WriteLine("[Program] ERROR starting RT controller management thread [" + RT_ID.ToString() + "] [" + z.ToString() + "]");
                    ErrorIndex = z;
                    break;
                }

                z++;
            }

            int LoopIndex;
            if (ErrorIndex != -1)
            {
                LoopIndex = ErrorIndex;
            }
            else
            {
                LoopIndex = AbstractRTDriverPairList.Count;
                Console.WriteLine("[Program] Sleeping...");
                Thread.Sleep(960000);
            }

            for (int i = 0; i < LoopIndex; i++)
            {
                if (MainControlRoomController.RemoveRadioTelescopeControllerAt(i, true))
                {
                    Console.WriteLine("[Program] Successfully brought down RT controller at index " + i.ToString());
                }
                else
                {
                    Console.WriteLine("[Program] ERROR killing RT controller at index " + i.ToString());
                }

                ProgramRTControllerList[i].RadioTelescope.SpectraCyberController.BringDown();
                ProgramRTControllerList[i].RadioTelescope.PLCClient.TerminateTCPServerConnection();
                ProgramPLCDriverList[i].RequestStopAsyncAcceptingClients();
            }

            // End logging
            logger.Info("<--------------- Control Room Application Terminated --------------->");
            Console.ReadKey();

            Thread.Sleep(5000);
        }
    }
}