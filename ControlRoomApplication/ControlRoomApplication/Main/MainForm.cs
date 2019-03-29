using ControlRoomApplication.Constants;
using ControlRoomApplication.Controllers;
using ControlRoomApplication.Controllers.PLCCommunication;
using ControlRoomApplication.Controllers.RadioTelescopeControllers;
using ControlRoomApplication.Controllers.SpectraCyberController;
using ControlRoomApplication.Database.Operations;
using ControlRoomApplication.Entities;
using ControlRoomApplication.Entities.RadioTelescope;
using ControlRoomApplication.Simulators.Hardware.WeatherStation;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Windows.Forms;

namespace ControlRoomApplication.Main
{
    public partial class MainForm : Form
    {
        private static int numLocalDBRTInstancesCreated = 1;
        public MainForm()
        {
            InitializeComponent();
            logger.Info("<--------------- Control Room Application Started --------------->");
            dataGridView1.ColumnCount = 2;
            DatabaseOperations.InitializeDatabaseContext();

            AbstractRTDriverPairList = new List<KeyValuePair<RadioTelescope, AbstractPLCDriver>>();
            ProgramRTControllerList = new List<RadioTelescopeController>();
            ProgramPLCDriverList = new List<AbstractPLCDriver>();
            ProgramControlRoomControllerList = new List<ControlRoomController>();

            CancellationSource = new CancellationTokenSource();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text != null 
                && textBox2.Text != null 
                && comboBox1.SelectedIndex > -1)
            {
                RadioTelescope ARadioTelescope = BuildRT();
                AbstractPLCDriver APLCDriver = BuildPLCDriver();

                AbstractRTDriverPairList.Add(new KeyValuePair<RadioTelescope, AbstractPLCDriver>(ARadioTelescope, APLCDriver));
                ProgramRTControllerList.Add(new RadioTelescopeController(AbstractRTDriverPairList[AbstractRTDriverPairList.Count - 1].Key));
                ProgramPLCDriverList.Add(APLCDriver);

                ControlRoomController MainControlRoomController = new ControlRoomController(new ControlRoom(BuildWeatherStation()), CancellationSource.Token);

                ProgramPLCDriverList[ProgramPLCDriverList.Count - 1].StartAsyncAcceptingClients();
                ProgramRTControllerList[ProgramRTControllerList.Count - 1].RadioTelescope.PLCClient.ConnectToServer();

                MainControlRoomController.AddRadioTelescopeController(ProgramRTControllerList[ProgramRTControllerList.Count - 1]);

                if (numLocalDBRTInstancesCreated == 1)
                {
                    MainControlRoomController.StartWeatherMonitoringRoutine();
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
                        numLocalDBRTInstancesCreated++;
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
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            // Notify cancellation token
            //CancellationSource.Cancel();

            if (MainControlRoomController.RequestToKillWeatherMonitoringRoutine())
            {
                Console.WriteLine("[Program] Successfully shut down weather monitoring routine.");
            }
            else
            {
                Console.WriteLine("[Program] ERROR shutting down weather monitoring routine!");
            }

            for (int i = 0; i < numLocalDBRTInstancesCreated; i++)
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

            CancellationSource.Dispose();
            DatabaseOperations.DeleteLocalDatabase();

            // End logging
            logger.Info("<--------------- Control Room Application Terminated --------------->");
            Environment.Exit(0);
        }

        private void MainForm_Load(object sender, EventArgs e)
        {

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
        }

        private void comboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            string[] row1 = { "Current Appointment ID:", "1" };
            string[] row2 = { "Current RT Azimuth:",
                "123.2" };// CRoom.RadioTelescopeController.RadioTelescope.CurrentOrientation.Azimuth.ToString() };
            string[] row3 = { "Current RT Elevation:",
                "12.123" };// CRoom.RadioTelescopeController.RadioTelescope.CurrentOrientation.Elevation.ToString() };

            dataGridView1.Rows.Add(row1);
            dataGridView1.Rows.Add(row2);
            dataGridView1.Rows.Add(row3);

            dataGridView1.Update();
            dataGridView1.Refresh();
        }

        public RadioTelescope BuildRT()
        {
            PLCClientCommunicationHandler PLCCommsHandler = new PLCClientCommunicationHandler(textBox2.Text, int.Parse(textBox1.Text));

            // Create Radio Telescope Location
            Location location = new Location(76.7046, 40.0244, 395.0); // John Rudy Park hardcoded for now


            // Return Radio Telescope
            if (checkBox1.Checked)
            {
                return new RadioTelescope(BuildSpectraCyber(), PLCCommsHandler, location, numLocalDBRTInstancesCreated++);
            }
            else
            {
                return new RadioTelescope(BuildSpectraCyber(), PLCCommsHandler, location);
            }
        }

        public AbstractSpectraCyberController BuildSpectraCyber()
        {
            switch (comboBox1.SelectedIndex)
            {
                case 0:
                    return new SpectraCyberController(new SpectraCyber());

                case 1:
                    return new SpectraCyberTestController(new SpectraCyberSimulator());

                default:
                    // If none of the switches match or there wasn't one declared
                    // for the spectraCyber, assume we are using the simulated/testing one.
                    return new SpectraCyberSimulatorController(new SpectraCyberSimulator());
            }
        }

        public AbstractPLCDriver BuildPLCDriver()
        {
            switch (comboBox3.SelectedIndex)
            {
                case 0:
                    // The production telescope
                    throw new NotImplementedException("There is not yet communication for the real PLC.");

                case 1:
                    // Case for the test/simulated radiotelescope.
                    return new ScaleModelPLCDriver(textBox2.Text, int.Parse(textBox1.Text));

                case 2:
                    return new TestPLCDriver(textBox2.Text, int.Parse(textBox1.Text));
                default:
                    // Should be changed once we have a simulated radiotelescope class implemented
                    return new TestPLCDriver(textBox2.Text, int.Parse(textBox1.Text));
            }
        }

        public AbstractWeatherStation BuildWeatherStation()
        {
            switch (comboBox2.SelectedIndex)
            {
                case 0:
                    throw new NotImplementedException("The production weather station is not yet supported.");

                case 1:
                    return new SimulationWeatherStation(1000);

                case 2:
                    throw new NotImplementedException("The test weather station is not yet supported.");

                default:
                    return new SimulationWeatherStation(1000);
            }
        }

        public List<KeyValuePair<RadioTelescope, AbstractPLCDriver>> AbstractRTDriverPairList { get; set; }
        public List<RadioTelescopeController> ProgramRTControllerList { get; set; }
        public List<AbstractPLCDriver> ProgramPLCDriverList { get; set; }
        public List<ControlRoomController> ProgramControlRoomControllerList { get; set; }
        private ControlRoomController MainControlRoomController { get; set; }
        private Thread ControlRoomThread { get; set; }
        private CancellationTokenSource CancellationSource { get; set; }
        private static readonly log4net.ILog logger =
            log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
    }
}