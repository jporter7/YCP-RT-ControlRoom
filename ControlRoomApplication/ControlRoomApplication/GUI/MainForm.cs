using ControlRoomApplication.Controllers;
using ControlRoomApplication.Entities;
using ControlRoomApplication.GUI;
using ControlRoomApplication.Simulators.Hardware.WeatherStation;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Windows.Forms;
using ControlRoomApplication.Constants;
using ControlRoomApplication.Database;

namespace ControlRoomApplication.Main
{
    public partial class MainForm : Form
    {
        private static int current_rt_id;

        /// <summary>
        /// Constructor for the main GUI form. Initializes the GUI form by calling the
        /// initialization method in another partial class. Initializes the datagridview
        /// and the lists that track telescope configurations.
        /// </summary>
        public MainForm()
        {
            InitializeComponent();
            DatabaseOperations.DeleteLocalDatabase();
            logger.Info("<--------------- Control Room Application Started --------------->");
            dataGridView1.ColumnCount = 3;
            dataGridView1.Columns[0].HeaderText = "ID";
            dataGridView1.Columns[1].HeaderText = "PLC IP";
            dataGridView1.Columns[2].HeaderText = "PLC Port";

            AbstractRTDriverPairList = new List<KeyValuePair<RadioTelescope, AbstractHardwareReceiver>>();
            ProgramRTControllerList = new List<RadioTelescopeController>();
            ProgramPLCDriverList = new List<AbstractHardwareReceiver>();
            ProgramControlRoomControllerList = new List<ControlRoomController>();
            current_rt_id = 0;
        }

        /// <summary>
        /// Eventhandler for the start button on the main GUI form. This method creates and
        /// initializes the configuration that is specified on the main GUI form if the correct
        /// fields are populated.
        /// </summary>
        /// <param name="sender"> Object specifying the sender of this Event. </param>
        /// <param name="e"> The eventargs from the button being clicked on the GUI. </param>
        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text != null 
                && textBox2.Text != null 
                && comboBox1.SelectedIndex > -1)
            {
                current_rt_id++;
                RadioTelescope ARadioTelescope = BuildRT();
                AbstractHardwareReceiver APLCDriver = BuildPLCDriver();

                // Add the RT/PLC driver pair and the RT controller to their respective lists
                AbstractRTDriverPairList.Add(new KeyValuePair<RadioTelescope, AbstractHardwareReceiver>(ARadioTelescope, APLCDriver));
                ProgramRTControllerList.Add(new RadioTelescopeController(AbstractRTDriverPairList[current_rt_id - 1].Key));
                ProgramPLCDriverList.Add(APLCDriver);

                if (checkBox1.Checked)
                {
                    DatabaseOperations.PopulateLocalDatabase(current_rt_id);
                    ManualControl.Enabled = false;
                    FreeControl.Enabled = false;
                }
                else
                {
                    ManualControl.Enabled = true;
                    FreeControl.Enabled = true;
                }

                // If the main control room controller hasn't been initialized, initialize it.
                if (MainControlRoomController == null)
                {
                    MainControlRoomController = new ControlRoomController(new ControlRoom(BuildWeatherStation()));
                }
                
                // Start plc server and attempt to connect to it.
                ProgramPLCDriverList[current_rt_id - 1].StartAsyncAcceptingClients();
                ProgramRTControllerList[current_rt_id - 1].RadioTelescope.HardwareCommsHandler.StartCommunicationThread();

                MainControlRoomController.AddRadioTelescopeController(ProgramRTControllerList[current_rt_id - 1]);

                MainControlRoomController.StartWeatherMonitoringRoutine();

                // Start RT controller's threaded management
                RadioTelescopeControllerManagementThread ManagementThread = MainControlRoomController.ControlRoom.RTControllerManagementThreads[current_rt_id - 1];

                int RT_ID = ManagementThread.RadioTelescopeID;
                List<Appointment> AllAppointments = DatabaseOperations.GetListOfAppointmentsForRadioTelescope(RT_ID);

                logger.Info("Attempting to queue " + AllAppointments.Count.ToString() + " appointments for RT with ID " + RT_ID.ToString());

                foreach (Appointment appt in AllAppointments)
                {
                    logger.Info("\t[" + appt.Id + "] " + appt.StartTime.ToString() + " -> " + appt.EndTime.ToString());
                }

                if (ManagementThread.Start())
                {
                    logger.Info("Successfully started RT controller management thread [" + RT_ID.ToString() + "]");

                    if (APLCDriver is MCUModbusReceiver)
                    {
                        ProgramRTControllerList[current_rt_id - 1].ConfigureRadioTelescope(500, 500, 0, 0);
                    }
                }
                else
                {
                    logger.Info("ERROR starting RT controller management thread [" + RT_ID.ToString() + "]" );
                }

                AddConfigurationToDataGrid();
            }
        }

        /// <summary>
        /// Adds the configuration specified with the main GUI form to the datagridview.
        /// </summary>
        private void AddConfigurationToDataGrid()
        {
            string[] row = { (current_rt_id).ToString(), textBox2.Text, textBox1.Text };

            dataGridView1.Rows.Add(row);
            dataGridView1.Update();
        }

        /// <summary>
        /// Brings down each telescope instance and exits from the GUI cleanly.
        /// </summary>
        private void button2_Click(object sender, EventArgs e)
        {
            if (MainControlRoomController != null && MainControlRoomController.RequestToKillWeatherMonitoringRoutine())
            {
                logger.Info("Successfully shut down weather monitoring routine.");
            }
            else
            {
                logger.Info("ERROR shutting down weather monitoring routine!");
            }

            // Loop through the list of telescope controllers and call their respective bring down sequences.
            for (int i = 0; i < ProgramRTControllerList.Count; i++)
            {
                if (MainControlRoomController.RemoveRadioTelescopeControllerAt(0, false))
                {
                    logger.Info("Successfully brought down RT controller at index " + i.ToString());
                }
                else
                {
                    logger.Info("ERROR killing RT controller at index " + i.ToString());
                }

                ProgramRTControllerList[0].RadioTelescope.SpectraCyberController.BringDown();
                ProgramRTControllerList[0].RadioTelescope.HardwareCommsHandler.TerminateAndJoinCommunicationThread();
                ProgramPLCDriverList[0].StopAsyncAcceptingClientsAndJoin();
            }

            // End logging
            logger.Info("<--------------- Control Room Application Terminated --------------->");
            Environment.Exit(0);
        }

        /// <summary>
        /// Erases the current text in the plc port textbox. 
        /// </summary>
        private void textBox2_Focus(object sender, EventArgs e)
        {
            textBox2.Text = "";
        }

        /// <summary>
        /// Erases the current text in the plc IP address textbox.
        /// </summary>
        private void textBox1_Focus(object sender, EventArgs e)
        {
            textBox1.Text = "";
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

        /// <summary>
        /// Generates a diagnostic form for whichever telescope configuration is chosen
        /// from the GUI.
        /// </summary>
        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            DiagnosticsForm diagnosticForm = new DiagnosticsForm(MainControlRoomController.ControlRoom, dataGridView1.CurrentCell.RowIndex);
            diagnosticForm.Show();
        }

        /// <summary>
        /// Builds a radio telescope instance based off of the input from the GUI form.
        /// </summary>
        /// <returns> A radio telescope instance representing the configuration chosen. </returns>
        public RadioTelescope BuildRT()
        {
            AbstractHardwareCommunicationHandler HardwareCommsHandler;

            // TODO: Add in logic for selecting other types of HardwareCommsHandler
            if (true)
            {
                HardwareCommsHandler = new TCPIPCommunicationHandler(textBox2.Text, int.Parse(textBox1.Text));
            }
            else
            {
                HardwareCommsHandler = null;
            }

            // Create Radio Telescope Location
            Location location = MiscellaneousConstants.JOHN_RUDY_PARK;

            // Return Radio Telescope
            // This is not permanent functionality
            if (checkBox1.Checked)
            {
                return new RadioTelescope(BuildSpectraCyber(), HardwareCommsHandler, location, new Entities.Orientation(0, 90), current_rt_id);
            }
            else
            {
                return new RadioTelescope(BuildSpectraCyber(), HardwareCommsHandler, location, new Entities.Orientation(0, 90), current_rt_id);
            }
        }

        /// <summary>
        /// Builds a spectracyber instance based off of the input from the GUI.
        /// </summary>
        /// <returns> A spectracyber instance based off of the configuration specified by the GUI. </returns>
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

        /// <summary>
        /// Builds a PLC driver based off of the input from the GUI.
        /// </summary>
        /// <returns> A plc driver based off of the configuration specified by the GUI. </returns>
        public AbstractHardwareReceiver BuildPLCDriver()
        {
            switch (comboBox3.SelectedIndex)
            {
                case 0:
                    // The production telescope
                    return new MCUModbusReceiver(textBox2.Text, int.Parse(textBox1.Text));

                case 1:
                    // Case for the simulated radiotelescope.
                    return new ScaleModelTCPIPReceiver(textBox2.Text, int.Parse(textBox1.Text));

                case 3:
                    // Case for the test radiotelescope.
                    return new TestPLCTCPIPReceiver(textBox2.Text, int.Parse(textBox1.Text));

                case 2:
                default:
                    // Case for the simulation telescope.
                    return new SimulationPLCTCPIPReceiver(textBox2.Text, int.Parse(textBox1.Text));
            }
        }

        /// <summary>
        /// Build a weather station based off of the input from the GUI form.
        /// </summary>
        /// <returns> A weather station instance based off of the configuration specified. </returns>
        public AbstractWeatherStation BuildWeatherStation()
        {
            switch (comboBox2.SelectedIndex)
            {
                case 0:
                    throw new NotImplementedException("The production weather station is not yet supported.");

                case 2:
                    throw new NotImplementedException("The test weather station is not yet supported.");

                case 1:
                default:
                    return new SimulationWeatherStation(1000);
            }
        }

        public List<KeyValuePair<RadioTelescope, AbstractHardwareReceiver>> AbstractRTDriverPairList { get; set; }
        public List<RadioTelescopeController> ProgramRTControllerList { get; set; }
        public List<AbstractHardwareReceiver> ProgramPLCDriverList { get; set; }
        public List<ControlRoomController> ProgramControlRoomControllerList { get; set; }
        private ControlRoomController MainControlRoomController { get; set; }
        private Thread ControlRoomThread { get; set; }
        private CancellationTokenSource CancellationSource { get; set; }
        private static readonly log4net.ILog logger =
            log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Generates a free control form that allows free control access to a radio telescope
        /// instance through the generated form.
        /// </summary>
        private void FreeControl_Click(object sender, EventArgs e)
        {
            FreeControlForm freeControlWindow = new FreeControlForm(MainControlRoomController.ControlRoom, current_rt_id);
            // Create free control thread
            Thread FreeControlThread = new Thread(() => freeControlWindow.ShowDialog())
            {
                Name = "Free Control Thread"
            };
            FreeControlThread.Start();
        }

        /// <summary>
        /// Generates a manual control form that allows manual control access to a radio telescope
        /// instance through the generated form.
        /// </summary>
        private void ManualControl_Click(object sender, EventArgs e)
        {
            ManualControlForm manualControlWindow = new ManualControlForm(MainControlRoomController.ControlRoom, current_rt_id);
            // Create free control thread
            Thread ManualControlThread = new Thread(() => manualControlWindow.ShowDialog())
            {
                Name = "Manual Control Thread"
            };
            ManualControlThread.Start();
        }
    }
}