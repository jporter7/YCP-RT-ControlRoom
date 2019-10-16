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
using System.Net;
using ControlRoomApplication.Controllers.BlkHeadUcontroler;

namespace ControlRoomApplication.Main
{
    public partial class MainForm : Form
    {
        private static int current_rt_id;
        public List<KeyValuePair<RadioTelescope, AbstractPLCDriver>> AbstractRTDriverPairList { get; set; }
        public List<RadioTelescopeController> ProgramRTControllerList { get; set; }
        public List<AbstractPLCDriver> ProgramPLCDriverList { get; set; }
        public List<ControlRoomController> ProgramControlRoomControllerList { get; set; }
        private ControlRoomController MainControlRoomController { get; set; }
        private Thread ControlRoomThread { get; set; }
        private Thread MicroctrlServerThread { get; set; }
        private CancellationTokenSource CancellationSource { get; set; }
        private static readonly log4net.ILog logger =
            log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        enum TempSensorType
        {
            Production,
            Simulation
        }

        enum MCUType
        {
            Production,
            Simulation
        }

        enum PLCType
        {
            Production, 
            Scale,
            Simulation,
            Test

        }

        
        enum MicrocontrollerType
        {
            Production,
            Simulation
        }
        

        /// <summary>
        /// Constructor for the main GUI form. Initializes the GUI form by calling the
        /// initialization method in another partial class. Initializes the datagridview
        /// and the lists that track telescope configurations.
        /// </summary>
        public MainForm()
        {
            InitializeComponent();

            IPHostEntry ipHostInfo = Dns.GetHostEntry(Dns.GetHostName());
            IPAddress[] v4_list = new IPAddress[ipHostInfo.AddressList.Length / 2];
            System.Array.Copy(ipHostInfo.AddressList, ipHostInfo.AddressList.Length / 2, v4_list, 0, ipHostInfo.AddressList.Length / 2);
            this.LocalIPCombo.Items.AddRange(v4_list);


            DatabaseOperations.DeleteLocalDatabase();
            logger.Info("<--------------- Control Room Application Started --------------->");
            dataGridView1.ColumnCount = 3;
            dataGridView1.Columns[0].HeaderText = "ID";
            dataGridView1.Columns[1].HeaderText = "PLC IP";
            dataGridView1.Columns[2].HeaderText = "PLC Port";

            AbstractRTDriverPairList = new List<KeyValuePair<RadioTelescope, AbstractPLCDriver>>();
            ProgramRTControllerList = new List<RadioTelescopeController>();
            ProgramPLCDriverList = new List<AbstractPLCDriver>();
            ProgramControlRoomControllerList = new List<ControlRoomController>();
            current_rt_id = 0;
            logger.Info("MainForm Initalized");
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
            logger.Info("Start Telescope Button Clicked");
            if (txtPLCPort.Text != null 
                && txtPLCIP.Text != null 
                && comboBox1.SelectedIndex > -1)
            {
                current_rt_id++;
                AbstractPLCDriver APLCDriver = BuildPLCDriver();
                AbstractMicrocontroller ctrler= build_CTRL();
                ctrler.BringUp();
                AbstractEncoderReader encoder= build_encoder( APLCDriver );
               RadioTelescope ARadioTelescope = BuildRT(APLCDriver, ctrler, encoder );
                

                // Add the RT/PLC driver pair and the RT controller to their respective lists
                AbstractRTDriverPairList.Add(new KeyValuePair<RadioTelescope, AbstractPLCDriver>(ARadioTelescope, APLCDriver));
                ProgramRTControllerList.Add(new RadioTelescopeController(AbstractRTDriverPairList[current_rt_id - 1].Key));
                ProgramPLCDriverList.Add(APLCDriver);

                if (checkBox1.Checked)
                {
                    logger.Info("Populating Local Database");
                    DatabaseOperations.PopulateLocalDatabase(current_rt_id);
                    Console.WriteLine(DatabaseOperations.GetNextAppointment(current_rt_id).StartTime.ToString());
                    logger.Info("Disabling ManualControl and FreeControl");
                    //ManualControl.Enabled = false;
                    FreeControl.Enabled = false;
                }
                else
                {
                    logger.Info("Enabling ManualControl and FreeControl");
                   // ManualControl.Enabled = true;
                    FreeControl.Enabled = true;
                }

                // If the main control room controller hasn't been initialized, initialize it.
                if (MainControlRoomController == null)
                {
                    logger.Info("Initializing ControlRoomController");
                    MainControlRoomController = new ControlRoomController(new ControlRoom(BuildWeatherStation()));
                }

                // Start plc server and attempt to connect to it.
                logger.Info("Starting plc server and attempting to connect to it");
                ProgramPLCDriverList[current_rt_id - 1].StartAsyncAcceptingClients();
//ProgramRTControllerList[current_rt_id - 1].RadioTelescope.PLCClient.ConnectToServer();//////####################################

                logger.Info("Adding RadioTelescope Controller");
                MainControlRoomController.AddRadioTelescopeController(ProgramRTControllerList[current_rt_id - 1]);

                logger.Info("Starting Weather Monitoring Routine");
                MainControlRoomController.StartWeatherMonitoringRoutine();

                // Start RT controller's threaded management
                logger.Info("Starting RT controller's threaded management");
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

                    ProgramRTControllerList[current_rt_id - 1].ConfigureRadioTelescope(.1, .1, 0, 0);
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
            logger.Info("Adding Configuration To DataGrid");
            string[] row = { (current_rt_id).ToString(), txtPLCIP.Text, txtPLCPort.Text };

            dataGridView1.Rows.Add(row);
            dataGridView1.Update();
        }

        /// <summary>
        /// Brings down each telescope instance and exits from the GUI cleanly.
        /// </summary>
        private void button2_Click(object sender, EventArgs e)
        {
            logger.Info("Shut Down Telescope Button Clicked");
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
                ProgramRTControllerList[0].RadioTelescope.PLCDriver.Bring_down();
                ProgramPLCDriverList[0].RequestStopAsyncAcceptingClientsAndJoin();
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
            logger.Info("textBox2_Focus Event");
            txtPLCIP.Text = "";
        }

        /// <summary>
        /// Erases the current text in the plc IP address textbox.
        /// </summary>
        private void textBox1_Focus(object sender, EventArgs e)
        {
            logger.Info("textBox1_Focus Event");
            txtPLCPort.Text = "";
        }

        /// <summary>
        /// Generates a diagnostic form for whichever telescope configuration is chosen
        /// from the GUI.
        /// </summary>
        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            logger.Info("dataGridView1_CellContent Clicked");
            try {
                DiagnosticsForm diagnosticForm = new DiagnosticsForm( MainControlRoomController.ControlRoom , dataGridView1.CurrentCell.RowIndex );
                diagnosticForm.Show();
            } catch {

            }
        }

        /// <summary>
        /// Builds a radio telescope instance based off of the input from the GUI form.
        /// </summary>
        /// <returns> A radio telescope instance representing the configuration chosen. </returns>
        public RadioTelescope BuildRT(AbstractPLCDriver abstractPLCDriver , AbstractMicrocontroller ctrler , AbstractEncoderReader encoder )
        {
            logger.Info("Building RadioTelescope");

           // PLCClientCommunicationHandler PLCCommsHandler = new PLCClientCommunicationHandler(txtPLCIP.Text, int.Parse(txtPLCPort.Text));///###############################

            // Create Radio Telescope Location
            Location location = MiscellaneousConstants.JOHN_RUDY_PARK;

            // Return Radio Telescope
            RadioTelescope rt = new RadioTelescope(BuildSpectraCyber(), abstractPLCDriver, location, new Entities.Orientation(0,90), current_rt_id, ctrler, encoder );

            logger.Info("RadioTelescope Built Successfully");
            return rt;
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
                    logger.Info("Building SpectraCyber");
                    return new SpectraCyberController(new SpectraCyber());

                case 1:
                default:
                    logger.Info("Building SpectraCyberSimulator");
                    return new SpectraCyberTestController(new SpectraCyberSimulator());
            }
        }

        /// <summary>
        /// Builds a PLC driver based off of the input from the GUI.
        /// </summary>
        /// <returns> A plc driver based off of the configuration specified by the GUI. </returns>
        public AbstractPLCDriver BuildPLCDriver()
        {
            switch (comboPLCType.SelectedIndex)
            {
                case 0:
                    logger.Info("Building ProductionPLCDriver");
                    return new ProductionPLCDriver(LocalIPCombo.Text, txtPLCIP.Text, int.Parse(txtPLCPort.Text), int.Parse(txtPLCPort.Text),false);

                case 1:
                    logger.Info("Building ScaleModelPLCDriver");
                    return new ScaleModelPLCDriver(LocalIPCombo.Text, txtPLCIP.Text, int.Parse(txtPLCPort.Text), int.Parse(txtPLCPort.Text),false);

                case 3:
                    logger.Info("Building TestPLCDriver");
                    return new TestPLCDriver(LocalIPCombo.Text, txtPLCIP.Text, int.Parse(txtPLCPort.Text), int.Parse(txtPLCPort.Text),false);

                case 2:
                default:
                    logger.Info("Building SimulationPLCDriver");
                    return new SimulationPLCDriver(LocalIPCombo.Text, txtPLCIP.Text, int.Parse(txtPLCPort.Text), int.Parse(txtPLCPort.Text),false);
            }
        }

        public AbstractEncoderReader build_encoder( AbstractPLCDriver plc ) {
            return new SimulatedEncoder(plc , LocalIPCombo.Text , 1602 );
        }

        public AbstractMicrocontroller build_CTRL() {
            switch(comboMicrocontrollerBox.SelectedIndex) {
                case 0:
                    logger.Info( "Building ProductionPLCDriver" );
                    return new MicroControlerControler( LocalIPCombo.Text , 1600);

                case 1:
                    logger.Info( "Building ScaleModelPLCDriver" );
                    return new SimulatedMicrocontroller( -20,100 );

                default:
                    logger.Info( "Building SimulationPLCDriver" );
                    return new SimulatedMicrocontroller( -20,100);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns>  </returns>
        public bool IsMicrocontrollerSimulated()
        {
            bool isSimulated = false;

            logger.Info("Selected Microcontroller Type: ");

            if (comboMicrocontrollerBox.SelectedIndex == (int)MicrocontrollerType.Production)
            {
                isSimulated = false;
            }
            else
            {
                isSimulated = true;
            }

            return isSimulated;
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
                    logger.Error("The production weather station is not yet supported.");
                    throw new NotImplementedException("The production weather station is not yet supported.");

                case 2:
                    logger.Error("The test weather station is not yet supported.");
                    throw new NotImplementedException("The test weather station is not yet supported.");

                case 1:
                default:
                    logger.Info("Building SimulationWeatherStation");
                    return new SimulationWeatherStation(1000);
            }
        }


        /// <summary>
        /// Generates a free control form that allows free control access to a radio telescope
        /// instance through the generated form.
        /// </summary>
        private void FreeControl_Click(object sender, EventArgs e)
        {
            logger.Info("Free Control Button Clicked");
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
            logger.Info("Manual Control Button Clicked");
            ProgramRTControllerList[current_rt_id - 1].ConfigureRadioTelescope( .1 , .1 , 0 , 0 );
            ManualControlForm manualControlWindow = new ManualControlForm(MainControlRoomController.ControlRoom, current_rt_id);
            // Create free control thread
            Thread ManualControlThread = new Thread(() => manualControlWindow.ShowDialog())
            {
                Name = "Manual Control Thread"
            };
            ManualControlThread.Start();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {

        }

        private void loopBackBox_CheckedChanged(object sender, EventArgs e)
        {
            if (loopBackBox.Checked)
            {
                this.txtPLCIP.Text = "127.0.0.1";
                if (LocalIPCombo.FindStringExact("127.0.0.1") == -1)
                {
                    this.LocalIPCombo.Items.Add(IPAddress.Parse("127.0.0.1"));
                }
                this.LocalIPCombo.SelectedIndex = LocalIPCombo.FindStringExact("127.0.0.1");
            }
            this.txtPLCPort.Text = ((int)(8080+ ProgramPLCDriverList.Count*3)).ToString();
        }
    }
}