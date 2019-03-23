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

            // Given the input command line arguments, generate the control room's RTs, its controllers, its corresponding PLC driver, andits weather station
            (List<(RadioTelescope, AbstractPLCDriver)>, AbstractWeatherStation) RTComponentsTuple = ConfigurationManager.BuildRadioTelescopeSeries(args, true);

            // Instantiate the controll room being used.
            ControlRoomController MainControlRoomController = new ControlRoomController(new ControlRoom(RTComponentsTuple.Item2));

            // Extract the list of RTs with their corresponding PLC drivers
            List<(RadioTelescope, AbstractPLCDriver)> RTDriverPairList = RTComponentsTuple.Item1;

            ConfigurationManager.ConfigureLocalDatabase(RTDriverPairList.Count);
            Console.WriteLine("[Program] Local database populated. Number of Appointments: " + DatabaseOperations.GetTotalAppointmentCount());

            // Initialize the lists that the input list will populate
            List<RadioTelescopeController> ProgramRTControllerList = new List<RadioTelescopeController>(RTDriverPairList.Count);
            List<AbstractPLCDriver> ProgramPLCDriverList = new List<AbstractPLCDriver>(RTDriverPairList.Count);

            // Populate those lists and call any bring up sequences
            for (int i = 0; i < RTDriverPairList.Count; i++)
            {
                ProgramRTControllerList.Add(new RadioTelescopeController(RTDriverPairList[i].Item1));
                ProgramPLCDriverList.Add(RTDriverPairList[i].Item2);

                ProgramRTControllerList[i].RadioTelescope.SpectraCyberController.BringUp();
                ProgramPLCDriverList[i].StartAsyncAcceptingClients();
                ProgramRTControllerList[i].RadioTelescope.PLCClient.ConnectToServer();

                MainControlRoomController.AddRadioTelescopeController(ProgramRTControllerList[i]);
            }

            MainControlRoomController.StartWeatherMonitoringRoutine();

            int ErrorIndex = -1;
            // Start each RT controller's threaded management
            int z = 0;
            foreach (RadioTelescopeControllerManagementThread ManagementThread in MainControlRoomController.ControlRoom.RTControllerManagementThreads)
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
                LoopIndex = RTDriverPairList.Count;

                ManualResetEvent SIGINTEvent = new ManualResetEvent(false);
                Console.CancelKeyPress += (sender, eventArgs) =>
                {
                    eventArgs.Cancel = true;
                    SIGINTEvent.Set();
                };

                Console.WriteLine("[Program] Sleeping... Issue SIGINT (CTRL+C) to quit.");

                SIGINTEvent.WaitOne();
            }

            if (MainControlRoomController.RequestToKillWeatherMonitoringRoutine())
            {
                Console.WriteLine("[Program] Successfully shut down weather monitoring routine.");
            }
            else
            {
                Console.WriteLine("[Program] ERROR shutting down weather monitoring routine!");
            }

            for (int i = 0; i < LoopIndex; i++)
            {
                if (MainControlRoomController.RemoveRadioTelescopeControllerAt(i, false))
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
        }
    }
}