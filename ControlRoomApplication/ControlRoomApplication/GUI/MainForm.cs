using ControlRoomApplication.Controllers;
using ControlRoomApplication.Entities;
using ControlRoomApplication.GUI;
using ControlRoomApplication.Simulators.Hardware.WeatherStation;
using System;
using System.IO;
using System.Collections.Generic;
using System.Threading;
using System.Windows.Forms;
using System.Windows;
using ControlRoomApplication.Constants;
using ControlRoomApplication.Database;
using System.Net;
using ControlRoomApplication.Controllers.BlkHeadUcontroler;
using ControlRoomApplication.Entities.WeatherStation;
using log4net.Appender;
using ControlRoomApplication.Documentation;

namespace ControlRoomApplication.Main
{
    public partial class MainForm : Form, IAppender
    {
        private static int current_rt_id;
        public List<KeyValuePair<RadioTelescope, AbstractPLCDriver>> AbstractRTDriverPairList { get; set; }
        public List<RadioTelescopeController> ProgramRTControllerList { get; set; }
        public List<AbstractPLCDriver> ProgramPLCDriverList { get; set; }
        public List<ControlRoomController> ProgramControlRoomControllerList { get; set; }
        public ControlRoomController MainControlRoomController { get; set; }
        private Thread ControlRoomThread { get; set; }
        private Thread MicroctrlServerThread { get; set; }
        private CancellationTokenSource CancellationSource { get; set; }
        private static readonly log4net.ILog logger =
            log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public AbstractWeatherStation lastCreatedProductionWeatherStation = null;

        public bool finalSettings = true;
        public LoggerQueue log = new LoggerQueue();

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

            
            logger.Info("<--------------- Control Room Application Started --------------->");
            dataGridView1.ColumnCount = 5;
            dataGridView1.Columns[0].HeaderText = "ID";
            dataGridView1.Columns[1].HeaderText = "PLC IP";
            dataGridView1.Columns[2].HeaderText = "PLC Port";
            dataGridView1.Columns[3].HeaderText = "MCU Port";
            dataGridView1.Columns[4].HeaderText = "WS Port";
            //dataGridView1.Columns[3].HeaderText = "MCU Port";

            AbstractRTDriverPairList = new List<KeyValuePair<RadioTelescope, AbstractPLCDriver>>();
            ProgramRTControllerList = new List<RadioTelescopeController>();
            ProgramPLCDriverList = new List<AbstractPLCDriver>();
            ProgramControlRoomControllerList = new List<ControlRoomController>();
            current_rt_id = 0;

            // Initialize Button Settings 
            startRTGroupbox.BackColor = System.Drawing.Color.DarkGray;
            createWSButton.Enabled = true;
            acceptSettings.Enabled = true;
            startButton.BackColor = System.Drawing.Color.Gainsboro;
            startButton.Enabled = false;
            shutdownButton.BackColor = System.Drawing.Color.Gainsboro;
            shutdownButton.Enabled = false;
            loopBackBox.Enabled = true;
            checkBox1.Enabled = true;

      
            

            logger.Info("MainForm Initalized");
        }

        public void DoAppend(log4net.Core.LoggingEvent loggingEvent)
        {
            log.loggerQueue += loggingEvent.Level.Name + ": " + loggingEvent.MessageObject.ToString() + Environment.NewLine;
        }

        /// <summary>
        /// Eventhandler for the start button on the main GUI form. This method creates and
        /// initializes the configuration that is specified on the main GUI form if the correct
        /// fields are populated.
        /// </summary>
        /// <param name="sender"> Object specifying the sender of this Event. </param>
        /// <param name="e"> The eventargs from the button being clicked on the GUI. </param>
        private void startButton_Click(object sender, EventArgs e)
        {
            logger.Info("Start Telescope Button Clicked");

            // This will tell us whether or not the RT is safe to start.
            // It may not be safe to start if, for example, there are
            // validation errors, or no Telescope is found in the DB
            bool runRt = false;

            if (DatabaseOperations.FetchFirstRadioTelescope() == null)
            {
                DialogResult result = MessageBox.Show(
                    "No Radio Telescope found in the database. Would you like to create one?",
                    "No Telescope Found",
                    MessageBoxButtons.YesNoCancel);

                if (result == DialogResult.Yes)
                {
                    runRt = true;
                }
            }
            else runRt = true;

            if(runRt) { 
                shutdownButton.BackColor = System.Drawing.Color.Red;
                shutdownButton.Enabled = true;
                simulationSettingsGroupbox.BackColor = System.Drawing.Color.Gray;
                comboMicrocontrollerBox.Enabled = true;
                comboBox2.Enabled = true;
                comboBox1.Enabled = true;
                comboEncoderType.Enabled = true;
                comboPLCType.Enabled = true;
                LocalIPCombo.Enabled = true;

                portGroupbox.BackColor = System.Drawing.Color.Gray;
                txtPLCIP.Enabled = true;
                txtMcuCOMPort.Enabled = true;
                txtWSCOMPort.Enabled = true;
                txtPLCPort.Enabled = true;

                if (txtPLCPort.Text != null
                    && txtPLCIP.Text != null
                    && comboBox1.SelectedIndex > -1)
                {


                    if (checkBox1.Checked)
                    {
                        logger.Info("Populating Local Database");
                        //     DatabaseOperations.PopulateLocalDatabase(current_rt_id);
                        Console.WriteLine(DatabaseOperations.GetNextAppointment(current_rt_id).start_time.ToString());
                        logger.Info("Disabling ManualControl and FreeControl");
                        //ManualControl.Enabled = false;
                        FreeControl.Enabled = false;
                        // createWSButton.Enabled = false;
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
                        if (lastCreatedProductionWeatherStation == null)
                            MainControlRoomController = new ControlRoomController(new ControlRoom(BuildWeatherStation()));
                        else
                            MainControlRoomController = new ControlRoomController(new ControlRoom(lastCreatedProductionWeatherStation));
                    }

                    //current_rt_id++;
                    AbstractPLCDriver APLCDriver = BuildPLCDriver();
                    AbstractMicrocontroller ctrler = build_CTRL();
                    ctrler.BringUp();
                    AbstractEncoderReader encoder = build_encoder(APLCDriver);
                    RadioTelescope ARadioTelescope = BuildRT(APLCDriver, ctrler, encoder);
                    ARadioTelescope.WeatherStation = MainControlRoomController.ControlRoom.WeatherStation;

                    // Add the RT/PLC driver pair and the RT controller to their respective lists
                    AbstractRTDriverPairList.Add(new KeyValuePair<RadioTelescope, AbstractPLCDriver>(ARadioTelescope, APLCDriver));
                    ProgramRTControllerList.Add(new RadioTelescopeController(AbstractRTDriverPairList[AbstractRTDriverPairList.Count - 1].Key));
                    ProgramPLCDriverList.Add(APLCDriver);

                    // Start plc server and attempt to connect to it.
                    logger.Info("Starting plc server and attempting to connect to it");
                    ProgramPLCDriverList[ProgramPLCDriverList.Count - 1].StartAsyncAcceptingClients();
                    //ProgramRTControllerList[current_rt_id - 1].RadioTelescope.PLCClient.ConnectToServer();//////####################################

                    logger.Info("Adding RadioTelescope Controller");
                    MainControlRoomController.AddRadioTelescopeController(ProgramRTControllerList[ProgramRTControllerList.Count - 1]);
                    ARadioTelescope.SetParent(ProgramRTControllerList[ProgramRTControllerList.Count - 1]);

                    // linking radio telescope controller to tcp listener
                    MainControlRoomController.ControlRoom.mobileControlServer.rtController = ARadioTelescope.GetParent();

                    logger.Info("Starting Weather Monitoring Routine");
                    MainControlRoomController.StartWeatherMonitoringRoutine();

                    logger.Info("Starting Spectra Cyber Controller");
                    ARadioTelescope.SpectraCyberController.BringUp();

                    logger.Info("Setting Default Values for Spectra Cyber Controller");
                    ARadioTelescope.SpectraCyberController.SetSpectraCyberModeType(SpectraCyberModeTypeEnum.SPECTRAL);
                    ARadioTelescope.SpectraCyberController.SetSpectralIntegrationTime(SpectraCyberIntegrationTimeEnum.MID_TIME_SPAN);
                    ARadioTelescope.SpectraCyberController.SetContinuumOffsetVoltage(2.0);

                    // Start RT controller's threaded management
                    logger.Info("Starting RT controller's threaded management");
                    RadioTelescopeControllerManagementThread ManagementThread = MainControlRoomController.ControlRoom.RTControllerManagementThreads[MainControlRoomController.ControlRoom.RTControllerManagementThreads.Count - 1];

                    // add telescope to database
                    //DatabaseOperations.AddRadioTelescope(ARadioTelescope);

                    int RT_ID = ManagementThread.RadioTelescopeID;
                    List<Appointment> AllAppointments = DatabaseOperations.GetListOfAppointmentsForRadioTelescope(RT_ID);

                    logger.Info("Attempting to queue " + AllAppointments.Count.ToString() + " appointments for RT with ID " + RT_ID.ToString());
                    foreach (Appointment appt in AllAppointments)
                    {
                        logger.Info("\t[" + appt.Id + "] " + appt.start_time.ToString() + " -> " + appt.end_time.ToString());
                    }

                    if (ManagementThread.Start())
                    {
                        logger.Info("Successfully started RT controller management thread [" + RT_ID.ToString() + "]");

                        ProgramRTControllerList[ProgramRTControllerList.Count - 1].ConfigureRadioTelescope(.06, .06, 300, 300);
                    }
                    else
                    {
                        logger.Info("ERROR starting RT controller management thread [" + RT_ID.ToString() + "]");
                    }

                    AddConfigurationToDataGrid();
                }
            }
        }

        /// <summary>
        /// Adds the configuration specified with the main GUI form to the datagridview.
        /// </summary>
        private void AddConfigurationToDataGrid()
        {
            logger.Info("Adding Configuration To DataGrid");
            string[] row = { (current_rt_id).ToString(), txtPLCIP.Text, txtPLCPort.Text,txtMcuCOMPort.Text, txtWSCOMPort.Text };

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
        private void textBox3_Focus(object sender, EventArgs e)
        {
            logger.Info("textBox3_Focus Event");
            txtWSCOMPort.Text = "";
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
                DiagnosticsForm diagnosticForm = new DiagnosticsForm(MainControlRoomController.ControlRoom, AbstractRTDriverPairList[dataGridView1.CurrentCell.RowIndex].Key.Id, this);
                diagnosticForm.Show();
            }
            catch {

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

            // If no Telescope is present in the database, create a new one
            RadioTelescope newRT = DatabaseOperations.FetchFirstRadioTelescope();
            if(newRT == null)
            {
                // Create Radio Telescope
                newRT = new RadioTelescope();
                newRT.CalibrationOrientation = new Entities.Orientation(0, 90);

                DatabaseOperations.AddRadioTelescope(newRT);

                newRT.Id = DatabaseOperations.FetchFirstRadioTelescope().Id;
            }

            // These settings are not stored in the database, so they are new every time
            newRT.Location = location;
            abstractPLCDriver.SetParent(newRT);
            newRT.PLCDriver = abstractPLCDriver;
            newRT.SpectraCyberController = BuildSpectraCyber();
            newRT.Micro_controler = ctrler;
            newRT.Encoders = encoder;
            logger.Info("New RadioTelescope built successfully");

            // 
            current_rt_id = newRT.Id;

            return newRT;
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
                    return new SpectraCyberSimulatorController(new SpectraCyberSimulator());
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
                    return new ProductionPLCDriver(LocalIPCombo.Text, txtPLCIP.Text, int.Parse(txtMcuCOMPort.Text), int.Parse(txtPLCPort.Text));

                case 1:
                    logger.Info("Building ScaleModelPLCDriver");
                    return new ScaleModelPLCDriver(LocalIPCombo.Text, txtPLCIP.Text, int.Parse(txtMcuCOMPort.Text), int.Parse(txtPLCPort.Text));

                case 3:
                    logger.Info("Building TestPLCDriver");
                    return new TestPLCDriver(LocalIPCombo.Text, txtPLCIP.Text, int.Parse(txtMcuCOMPort.Text), int.Parse(txtPLCPort.Text),false);

                case 2:
                default:
                    logger.Info("Building SimulationPLCDriver");
                    return new SimulationPLCDriver(LocalIPCombo.Text, txtPLCIP.Text, int.Parse(txtMcuCOMPort.Text), int.Parse(txtPLCPort.Text),false , false );
            }
        }

        public AbstractEncoderReader build_encoder( AbstractPLCDriver plc )
        {
            return new SimulatedEncoder(plc , LocalIPCombo.Text , 1602 );
        }

        public AbstractMicrocontroller build_CTRL() {
            switch(comboMicrocontrollerBox.SelectedIndex) {
                case 0:
                    logger.Info( "Building ProductionPLCDriver" );
                    return new MicroControlerControler( LocalIPCombo.Text , 1600);

                case 1:
                    logger.Info( "Building ScaleModelPLCDriver" );
                    return new SimulatedMicrocontroller( -20,100,true );

                default:
                    logger.Info( "Building SimulationPLCDriver" );
                    return new SimulatedMicrocontroller( SimulationConstants.MIN_MOTOR_TEMP, SimulationConstants.MAX_MOTOR_TEMP, true);
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
                    logger.Info("Building ProductionWeatherStation");
                    return new WeatherStation(1000, int.Parse(txtWSCOMPort.Text));

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
            int rtIDforControl = AbstractRTDriverPairList[dataGridView1.CurrentCell.RowIndex].Key.Id;
            FreeControlForm freeControlWindow = new FreeControlForm(MainControlRoomController.ControlRoom, rtIDforControl);
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
        //private void ManualControl_Click(object sender, EventArgs e)
        //{
        //    logger.Info("Manual Control Button Clicked");
        //    ProgramRTControllerList[current_rt_id - 1].ConfigureRadioTelescope( .1 , .1 , 0 , 0 );
        //    ManualControlForm manualControlWindow = new ManualControlForm(MainControlRoomController.ControlRoom, current_rt_id);
        //    // Create free control thread
        //    Thread ManualControlThread = new Thread(() => manualControlWindow.ShowDialog())
        //    {
        //        Name = "Manual Control Thread"
        //    };
        //    ManualControlThread.Start();
        //}

        private void MainForm_Load(object sender, EventArgs e)
        {

        }

        private void loopBackBox_CheckedChanged(object sender, EventArgs e)
        {
           
            if (loopBackBox.Checked)
            {
                ProdcheckBox.Checked = false;
                this.txtWSCOMPort.Text = "222"; //default WS COM port # is 221
                this.txtMcuCOMPort.Text = ((int)(8083 + ProgramPLCDriverList.Count * 3)).ToString(); ; //default MCU Port
                this.txtPLCIP.Text = "127.0.0.1";//default IP address
                if (LocalIPCombo.FindStringExact("127.0.0.1") == -1)
                {
                    this.LocalIPCombo.Items.Add(IPAddress.Parse("127.0.0.1"));
                }
                this.LocalIPCombo.SelectedIndex = LocalIPCombo.FindStringExact("127.0.0.1");
            }
            this.txtPLCPort.Text = ((int)(8082 + ProgramPLCDriverList.Count*3)).ToString();
        }

        private void ProdcheckBox_CheckedChanged( object sender , EventArgs e ) {
            if(ProdcheckBox.Checked) {
                loopBackBox.Checked = false;
                this.txtWSCOMPort.Text = "222"; //default WS COM port # is 221
                this.txtMcuCOMPort.Text = "502"; //default MCU Port
                this.txtPLCIP.Text = "192.168.0.50";//default IP address
                if(LocalIPCombo.FindStringExact( "192.168.0.70" ) == -1) {
                    this.LocalIPCombo.Items.Add( IPAddress.Parse( "192.168.0.70" ) );
                }
                this.LocalIPCombo.SelectedIndex = LocalIPCombo.FindStringExact( "192.168.0.70" );
            }
            this.txtPLCPort.Text = "502";
            this.comboPLCType.SelectedIndex = this.comboPLCType.FindStringExact( "Production PLC" );
        }

        private void createWSButton_Click(object sender, EventArgs e)
        {
            startButton.BackColor = System.Drawing.Color.LimeGreen;
            startButton.Enabled = true;
            lastCreatedProductionWeatherStation = BuildWeatherStation();
        }

        private void comboMicrocontrollerBox_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void comboBox2_Click(object sender, EventArgs e) { 
}

        private void txtPLCPort_TextChanged(object sender, EventArgs e)
        {

        }

        private void LocalIPCombo_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void txtPLCIP_TextChanged(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void txtWSCOMPort_TextChanged(object sender, EventArgs e)
        {

        }

        private void groupBox3_Enter(object sender, EventArgs e)
        {

        }

        private void acceptSettings_Click(object sender, EventArgs e)
        {
            finalSettings = !finalSettings;
            if (finalSettings == false)
            {
                if (comboBox2.Text != "Production Weather Station")
                {
                    startButton.BackColor = System.Drawing.Color.LimeGreen;
                    startButton.Enabled = true;
                }
                startRTGroupbox.BackColor = System.Drawing.Color.Gray;
                loopBackBox.Enabled = false;
                checkBox1.Enabled = false;

                simulationSettingsGroupbox.BackColor = System.Drawing.Color.DarkGray;
                comboMicrocontrollerBox.Enabled = false;
                comboBox2.Enabled = false;
                comboBox1.Enabled = false;
                comboEncoderType.Enabled = false;
                comboPLCType.Enabled = false;
                LocalIPCombo.Enabled = false;

                portGroupbox.BackColor = System.Drawing.Color.DarkGray;
                txtPLCIP.Enabled = false;
                txtMcuCOMPort.Enabled = false;
                txtWSCOMPort.Enabled = false;
                txtPLCPort.Enabled = false;
            }
            else if (finalSettings == true)
            {
                startRTGroupbox.BackColor = System.Drawing.Color.DarkGray;
                loopBackBox.Enabled = true;
                checkBox1.Enabled = true;

                simulationSettingsGroupbox.BackColor = System.Drawing.Color.Gray;
                comboMicrocontrollerBox.Enabled = true;
                comboBox2.Enabled = true;
                comboBox1.Enabled = true;
                comboEncoderType.Enabled = true;
                comboPLCType.Enabled = true;
                LocalIPCombo.Enabled = true;

                portGroupbox.BackColor = System.Drawing.Color.Gray;
                txtPLCIP.Enabled = false;
                txtMcuCOMPort.Enabled = false;
                txtWSCOMPort.Enabled = false;
                txtPLCPort.Enabled = false;
            }
        }

        //Help button clicked ( user interface documentation PDF)
        private void helpButton_click(object sender, EventArgs e)
        {
            string filename = Directory.GetCurrentDirectory() + "\\" + "UIDoc.pdf";
            if(File.Exists(filename))
                System.Diagnostics.Process.Start(filename);
        }

        private void simulationSettingsGroupbox_Enter(object sender, EventArgs e)
        {

        }

        private void portGroupbox_Enter(object sender, EventArgs e)
        {

        }

        // Get and set Weather Station override status

        public void setWSOverride(bool WSO)
        {
            MainControlRoomController.ControlRoom.weatherStationOverride = WSO;
        }

        public bool getWSOverride()
        {
            return MainControlRoomController.ControlRoom.weatherStationOverride;
        }

        private void txtMcuCOMPort_TextChanged(object sender, EventArgs e)
        {

        }


    }
}