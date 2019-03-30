//using ControlRoomApplication.Controllers.PLCCommunication;
//using ControlRoomApplication.Controllers.RadioTelescopeControllers;
//using ControlRoomApplication.Controllers.SpectraCyberController;
//using ControlRoomApplication.Database.Operations;
//using ControlRoomApplication.Entities;
//using ControlRoomApplication.Main;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading;
//using System.Threading.Tasks;
//using System.Windows.Forms;

//namespace ControlRoomApplication.Controllers.GUI
//{
//    public class MainFormController
//    {
//        // This controller will be built upon soon.

//        public MainFormController()
//        {
//            AbstractRTDriverPairList = new List<KeyValuePair<RadioTelescope, AbstractPLCDriver>>();
//            ProgramRTControllerList = new List<RadioTelescopeController>();
//            ProgramPLCDriverList = new List<AbstractPLCDriver>();
//            ProgramControlRoomControllerList = new List<ControlRoomController>();
//        }

//        public void HandleButton1Click()
//        {
//            RadioTelescope ARadioTelescope = BuildRT();
//            AbstractPLCDriver APLCDriver = BuildPLCDriver();

//            AbstractRTDriverPairList.Add(new KeyValuePair<RadioTelescope, AbstractPLCDriver>(ARadioTelescope, APLCDriver));
//            ProgramRTControllerList.Add(new RadioTelescopeController(AbstractRTDriverPairList[AbstractRTDriverPairList.Count - 1].Key));
//            ProgramPLCDriverList.Add(APLCDriver);

//            if (checkBox1.Checked)
//            {
//                ConfigurationManager.ConfigureLocalDatabase(1);
//            }

//            ControlRoomController MainControlRoomController = new ControlRoomController(new ControlRoom(BuildWeatherStation()));

//            ProgramPLCDriverList[ProgramPLCDriverList.Count - 1].StartAsyncAcceptingClients();
//            ProgramRTControllerList[ProgramRTControllerList.Count - 1].RadioTelescope.PLCClient.ConnectToServer();

//            MainControlRoomController.AddRadioTelescopeController(ProgramRTControllerList[ProgramRTControllerList.Count - 1]);

//            if (numLocalDBRTInstancesCreated == 1)
//            {
//                MainControlRoomController.StartWeatherMonitoringRoutine();
//            }

//            int ErrorIndex = -1;
//            // Start each RT controller's threaded management
//            int z = 0;
//            foreach (RadioTelescopeControllerManagementThread ManagementThread in MainControlRoomController.ControlRoom.RTControllerManagementThreads)
//            {
//                int RT_ID = ManagementThread.RadioTelescopeID;
//                List<Appointment> AllAppointments = DatabaseOperations.GetListOfAppointmentsForRadioTelescope(RT_ID);

//                Console.WriteLine("[Program] Attempting to queue " + AllAppointments.Count.ToString() + " appointments for RT with ID " + RT_ID.ToString());

//                foreach (Appointment appt in AllAppointments)
//                {
//                    Console.WriteLine("\t[" + appt.Id + "] " + appt.StartTime.ToString() + " -> " + appt.EndTime.ToString());
//                }

//                if (ManagementThread.Start())
//                {
//                    numLocalDBRTInstancesCreated++;
//                    Console.WriteLine("[Program] Successfully started RT controller management thread [" + RT_ID.ToString() + "]");
//                }
//                else
//                {
//                    Console.WriteLine("[Program] ERROR starting RT controller management thread [" + RT_ID.ToString() + "] [" + z.ToString() + "]");
//                    ErrorIndex = z;
//                    break;
//                }

//                z++;
//            }
//        }

//        public RadioTelescope BuildRT()
//        {
//            PLCClientCommunicationHandler PLCCommsHandler = new PLCClientCommunicationHandler(textBox2.Text, int.Parse(textBox1.Text));

//            // Create Radio Telescope Location
//            Location location = new Location(76.7046, 40.0244, 395.0); // John Rudy Park hardcoded for now


//            // Return Radio Telescope
//            if (checkBox1.Checked)
//            {
//                return new RadioTelescope(BuildSpectraCyber(), PLCCommsHandler, location, new Entities.Orientation(0, 0), numLocalDBRTInstancesCreated++);
//            }
//            else
//            {
//                return new RadioTelescope(BuildSpectraCyber(), PLCCommsHandler, location, new Entities.Orientation(0, 0));
//            }
//        }

//        public AbstractSpectraCyberController BuildSpectraCyber()
//        {
//            switch (comboBox1.SelectedIndex)
//            {
//                case 0:
//                    return new SpectraCyberController(new SpectraCyber());

//                case 1:
//                    return new SpectraCyberTestController(new SpectraCyberSimulator());

//                default:
//                    // If none of the switches match or there wasn't one declared
//                    // for the spectraCyber, assume we are using the simulated/testing one.
//                    return new SpectraCyberSimulatorController(new SpectraCyberSimulator());
//            }
//        }

//        public AbstractPLCDriver BuildPLCDriver()
//        {
//            switch (comboBox3.SelectedIndex)
//            {
//                case 0:
//                    // The production telescope
//                    throw new NotImplementedException("There is not yet communication for the real PLC.");

//                case 1:
//                    // Case for the test/simulated radiotelescope.
//                    return new ScaleModelPLCDriver(textBox2.Text, int.Parse(textBox1.Text));

//                case 2:
//                    return new TestPLCDriver(textBox2.Text, int.Parse(textBox1.Text));
//                default:
//                    // Should be changed once we have a simulated radiotelescope class implemented
//                    return new TestPLCDriver(textBox2.Text, int.Parse(textBox1.Text));
//            }
//        }

//        public AbstractWeatherStation BuildWeatherStation()
//        {
//            switch (comboBox2.SelectedIndex)
//            {
//                case 0:
//                    throw new NotImplementedException("The production weather station is not yet supported.");

//                case 1:
//                    return new SimulationWeatherStation(1000);

//                case 2:
//                    throw new NotImplementedException("The test weather station is not yet supported.");

//                default:
//                    return new SimulationWeatherStation(1000);
//            }
//        }

//        public List<KeyValuePair<RadioTelescope, AbstractPLCDriver>> AbstractRTDriverPairList { get; set; }
//        public List<RadioTelescopeController> ProgramRTControllerList { get; set; }
//        public List<AbstractPLCDriver> ProgramPLCDriverList { get; set; }
//        public List<ControlRoomController> ProgramControlRoomControllerList { get; set; }
//        private ControlRoomController MainControlRoomController { get; set; }
//        private Thread ControlRoomThread { get; set; }
//        private static readonly log4net.ILog logger =
//            log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
//    }
//}
