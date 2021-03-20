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
using ControlRoomApplication.Validation;
using ControlRoomApplication.Util;

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

        // Booleans for user validation of input forms
        public bool MCUIPValid = false;
        public bool MCUPortValid = false;
        public bool PLCPortValid = false;
        public bool WCOMPortValid = false;

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


            logger.Info(Utilities.GetTimeStamp() + ": <--------------- Control Room Application Started --------------->");
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
            acceptSettings.Enabled = false;
            startButton.BackColor = System.Drawing.Color.Gainsboro;
            startButton.Enabled = false;
            shutdownButton.BackColor = System.Drawing.Color.Gainsboro;
            shutdownButton.Enabled = false;
            loopBackBox.Enabled = true;
            checkBox1.Enabled = true;




            logger.Info(Utilities.GetTimeStamp() + ": MainForm Initalized");
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
            logger.Info(Utilities.GetTimeStamp() + ": Start Telescope Button Clicked");

            // This will tell us whether or not the RT is safe to start.
            // It may not be safe to start if, for example, there are
            // validation errors, or no Telescope is found in the DB
            bool runRt = false;

            // retrirve contents of JSON file
            RadioTelescopeConfig RTConfig = RadioTelescopeConfig.DeserializeRTConfig();

            // this will be null if an error occurs in parsing JSON from the file, if the expected types do not match, i.e. a string
            // was given where an integer was expected, or if any of the inputs were null.
            if (RTConfig == null)
            {
                DialogResult result =  MessageBox.Show("An error occured while parsing the RTConfig JSON file. Would you like to recreate the JSON " +
                    "file?", "Error Parsing JSON", MessageBoxButtons.YesNo);
                // If yes, recreate the file and remind the user to set the ID and change the flag back to false
                if(result == DialogResult.Yes)
                {
                    RadioTelescopeConfig.CreateAndWriteToNewJSONFile(RadioTelescopeConfig.DEFAULT_JSON_CONTENTS);
                    MessageBox.Show("JSON file successfully recreated! Do not forget to specify the ID of telescope you want to run inside the file, " +
                        "and set the newTelescope flag to false.",
                        "JSON File Sucessfully Created", MessageBoxButtons.OK);
                }
            }
            // retrieve RT by specified ID, if newTelescope flag set to false (meaning the user is trying to run a pre-existing telescope)
            else if (!RTConfig.newTelescope)
            {
                RadioTelescope RT = DatabaseOperations.FetchRadioTelescopeByID(RTConfig.telescopeID);
                if (RT == null)
                {
                    DialogResult result = MessageBox.Show("The ID of " + RTConfig.telescopeID + 
                        " was not found in the database. Would you like to create a new one?", "No Telescope found", MessageBoxButtons.YesNoCancel);
                    if(result == DialogResult.Yes)
                    {
                        runRt = true;
                    }
                }
                // we cannot run a second telescope if the selected one is already running.
                else if (RT.online == 1)
                {
                    DialogResult result = MessageBox.Show(
                        $"Telescope {RT.Id} is already in use, or the program crashed. Would you like to override this check and run the telescope anyway?",
                        "Telescope in use",
                        MessageBoxButtons.YesNo);

                    if (result == DialogResult.Yes)
                    {
                        runRt = true;
                        current_rt_id = RTConfig.telescopeID;
                    }
                }
                // else the telescope entered by the user is valid and is currently not running. Start it up
                else
                {
                    Console.WriteLine("The Selected RT with id " + RTConfig.telescopeID + " was not null, and is not running. Starting telescope "+RTConfig.telescopeID);
                    current_rt_id = RTConfig.telescopeID;
                    runRt = true;

                }

            }
            // else the user is trying to create a new telescope. Discard the RadioTelescope ID from the file, and ask
            // the user to confirm they would like to create a new one.
            else
            {
                DialogResult result = MessageBox.Show(
                    "The new telescope flag was set to true in the RTConfig File. Please confirm you like to create " +
                    "a new telescope, or go back and input the ID of an existing telescope in the database.",
                    "New Telescope Flag Set to True",
                    MessageBoxButtons.YesNoCancel);

                if (result == DialogResult.Yes)
                {
                    runRt = true;
                }
            }
            

            if (runRt)
            {
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
                        logger.Info(Utilities.GetTimeStamp() + ": Populating Local Database");
                        //     DatabaseOperations.PopulateLocalDatabase(current_rt_id);
                        Console.WriteLine(DatabaseOperations.GetNextAppointment(current_rt_id).start_time.ToString());
                        logger.Info(Utilities.GetTimeStamp() + ": Disabling ManualControl and FreeControl");
                        //ManualControl.Enabled = false;
                        FreeControl.Enabled = false;
                        // createWSButton.Enabled = false;
                    }
                    else
                    {
                        logger.Info(Utilities.GetTimeStamp() + ": Enabling ManualControl and FreeControl");
                        // ManualControl.Enabled = true;
                        FreeControl.Enabled = true;

                    }

                    // If the main control room controller hasn't been initialized, initialize it.
                    if (MainControlRoomController == null)
                    {
                        logger.Info(Utilities.GetTimeStamp() + ": Initializing ControlRoomController");
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
                    logger.Info(Utilities.GetTimeStamp() + ": Starting plc server and attempting to connect to it");
                    ProgramPLCDriverList[ProgramPLCDriverList.Count - 1].StartAsyncAcceptingClients();
                    //ProgramRTControllerList[current_rt_id - 1].RadioTelescope.PLCClient.ConnectToServer();//////####################################

                    logger.Info(Utilities.GetTimeStamp() + ": Adding RadioTelescope Controller");
                    MainControlRoomController.AddRadioTelescopeController(ProgramRTControllerList[ProgramRTControllerList.Count - 1]);
                    ARadioTelescope.SetParent(ProgramRTControllerList[ProgramRTControllerList.Count - 1]);

                    // linking radio telescope controller to tcp listener
                    MainControlRoomController.ControlRoom.mobileControlServer.rtController = ARadioTelescope.GetParent();

                    logger.Info(Utilities.GetTimeStamp() + ": Starting Weather Monitoring Routine");
                    MainControlRoomController.StartWeatherMonitoringRoutine();

                    logger.Info(Utilities.GetTimeStamp() + ": Starting Spectra Cyber Controller");
                    ARadioTelescope.SpectraCyberController.BringUp();

                    logger.Info(Utilities.GetTimeStamp() + ": Setting Default Values for Spectra Cyber Controller");
                    ARadioTelescope.SpectraCyberController.SetSpectraCyberModeType(SpectraCyberModeTypeEnum.SPECTRAL);
                    ARadioTelescope.SpectraCyberController.SetSpectralIntegrationTime(SpectraCyberIntegrationTimeEnum.MID_TIME_SPAN);
                    ARadioTelescope.SpectraCyberController.SetContinuumOffsetVoltage(2.0);

                    // Start RT controller's threaded management
                    logger.Info(Utilities.GetTimeStamp() + ": Starting RT controller's threaded management");
                    RadioTelescopeControllerManagementThread ManagementThread = MainControlRoomController.ControlRoom.RTControllerManagementThreads[MainControlRoomController.ControlRoom.RTControllerManagementThreads.Count - 1];

                    // add telescope to database
                    //DatabaseOperations.AddRadioTelescope(ARadioTelescope);

                    int RT_ID = ManagementThread.RadioTelescopeID;
                    List<Appointment> AllAppointments = DatabaseOperations.GetListOfAppointmentsForRadioTelescope(RT_ID);

                    logger.Info(Utilities.GetTimeStamp() + ": Attempting to queue " + AllAppointments.Count.ToString() + " appointments for RT with ID " + RT_ID.ToString());
                    foreach (Appointment appt in AllAppointments)
                    {
                        logger.Info(Utilities.GetTimeStamp() + ": \t[" + appt.Id + "] " + appt.start_time.ToString() + " -> " + appt.end_time.ToString());
                    }

                    if (ManagementThread.Start())
                    {
                        logger.Info(Utilities.GetTimeStamp() + ": Successfully started RT controller management thread [" + RT_ID.ToString() + "]");

                        ProgramRTControllerList[ProgramRTControllerList.Count - 1].ConfigureRadioTelescope(.06, .06, 300, 300);
                    }
                    else
                    {
                        logger.Info(Utilities.GetTimeStamp() + ": ERROR starting RT controller management thread [" + RT_ID.ToString() + "]");
                    }

                    AddConfigurationToDataGrid();

                    // Set PLC override bits because they may be different than what's in the database
                    bool gateOvr = ProgramRTControllerList[ProgramRTControllerList.Count - 1].overrides.overrideGate;

                    APLCDriver.setregvalue((ushort)PLC_modbus_server_register_mapping.GATE_OVERRIDE, Convert.ToUInt16(gateOvr));
                }
            }



        }

        /// <summary>
        /// Adds the configuration specified with the main GUI form to the datagridview.
        /// </summary>
        private void AddConfigurationToDataGrid()
        {
            logger.Info(Utilities.GetTimeStamp() + ": Adding Configuration To DataGrid");
            string[] row = { (current_rt_id).ToString(), txtPLCIP.Text, txtPLCPort.Text, txtMcuCOMPort.Text, txtWSCOMPort.Text };

            dataGridView1.Rows.Add(row);
            dataGridView1.Update();
        }

        /// <summary>
        /// Brings down each telescope instance and exits from the GUI cleanly.
        /// </summary>
        private void button2_Click(object sender, EventArgs e)
        {
              logger.Info(Utilities.GetTimeStamp() + ": Shut Down Telescope Button Clicked");
            if (MainControlRoomController != null && MainControlRoomController.RequestToKillWeatherMonitoringRoutine())
            {
                logger.Info(Utilities.GetTimeStamp() + ": Successfully shut down weather monitoring routine.");
            }
            else
            {
                logger.Info(Utilities.GetTimeStamp() + ": ERROR shutting down weather monitoring routine!");
            }

            // Loop through the list of telescope controllers and call their respective bring down sequences.
            for (int i = 0; i < ProgramRTControllerList.Count; i++)
            {
                if (MainControlRoomController.RemoveRadioTelescopeControllerAt(0, false))
                {
                    logger.Info(Utilities.GetTimeStamp() + ": Successfully brought down RT controller at index " + i.ToString());
                }
                else
                {
                    logger.Info(Utilities.GetTimeStamp() + ": ERROR killing RT controller at index " + i.ToString());
                }

                //Turn off Telescope in database
                ProgramRTControllerList[i].RadioTelescope.online = 0;
                DatabaseOperations.UpdateTelescope(ProgramRTControllerList[i].RadioTelescope);

                ProgramRTControllerList[i].RadioTelescope.SpectraCyberController.BringDown();
                ProgramRTControllerList[i].RadioTelescope.PLCDriver.Bring_down();
            }

            // End logging
            logger.Info(Utilities.GetTimeStamp() + ": <--------------- Control Room Application Terminated --------------->");
            Environment.Exit(0);
        }

        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            for (int i = 0; i < ProgramRTControllerList.Count; i++)
            {
                //Turn off Telescope in database
                ProgramRTControllerList[i].RadioTelescope.online = 0;
                DatabaseOperations.UpdateTelescope(ProgramRTControllerList[i].RadioTelescope);
            }

        }
        private void textBox3_Focus(object sender, EventArgs e)
        {
            logger.Info(Utilities.GetTimeStamp() + ": textBox3_Focus Event");
            txtWSCOMPort.Text = "";
        }


        /// <summary>
        /// Erases the current text in the plc port textbox. 
        /// </summary>
        private void textBox2_Focus(object sender, EventArgs e)
        {
            logger.Info(Utilities.GetTimeStamp() + ": textBox2_Focus Event");
            txtPLCIP.Text = "";
        }

        /// <summary>
        /// Erases the current text in the plc IP address textbox.
        /// </summary>
        private void textBox1_Focus(object sender, EventArgs e)
        {
            logger.Info(Utilities.GetTimeStamp() + ": textBox1_Focus Event");
            txtPLCPort.Text = "";
        }

        /// <summary>
        /// Generates a diagnostic form for whichever telescope configuration is chosen
        /// from the GUI.
        /// </summary>
        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            logger.Info(Utilities.GetTimeStamp() + ": dataGridView1_CellContent Clicked");
            try
            {
                DiagnosticsForm diagnosticForm = new DiagnosticsForm(MainControlRoomController.ControlRoom, AbstractRTDriverPairList[dataGridView1.CurrentCell.RowIndex].Key.Id, this);
                diagnosticForm.Show();
            }
            catch
            {

            }
        }

        /// <summary>
        /// Builds a radio telescope instance based off of the input from the GUI form.
        /// </summary>
        /// <returns> A radio telescope instance representing the configuration chosen. </returns>
        public RadioTelescope BuildRT(AbstractPLCDriver abstractPLCDriver, AbstractMicrocontroller ctrler, AbstractEncoderReader encoder)
        {
            logger.Info(Utilities.GetTimeStamp() + ": Building RadioTelescope");

            // if this is set to 0, it has not been updated with an existing ID from the database. Therefore, we must create one.
            if (current_rt_id == 0)
            {
                RadioTelescope newRT = new RadioTelescope();
                newRT.Location = new Location(0, 0, 0, "");
                newRT.CalibrationOrientation = new Entities.Orientation(0, 90);
                newRT.CurrentOrientation = new Entities.Orientation(0, 0);

                // This is the TELESCOPE TYPE
                // It is now set to SLIP_RING because we finally removed the hard stops. Isn't that exciting?!
                newRT._TeleType = RadioTelescopeTypeEnum.SLIP_RING;

                DatabaseOperations.AddRadioTelescope(newRT);

                newRT.Id = DatabaseOperations.FetchLastRadioTelescope().Id;

                //Turn telescope on in database 
                newRT.online = 1;
                DatabaseOperations.UpdateTelescope(newRT);

                // These settings are not stored in the database, so they are new every time
                abstractPLCDriver.SetParent(newRT);
                newRT.PLCDriver = abstractPLCDriver;
                newRT.PLCDriver.setTelescopeType(newRT._TeleType);
                newRT.SpectraCyberController = BuildSpectraCyber();
                newRT.Micro_controler = ctrler;
                newRT.Encoders = encoder;
                logger.Info(Utilities.GetTimeStamp() + ": New RadioTelescope built successfully");

                current_rt_id = newRT.Id;

                // update the JSON config file to reflect the newly created telescope
                RadioTelescopeConfig.SerializeRTConfig(new RadioTelescopeConfig(newRT.Id, false));

                return newRT;
            }
            else
            // else there has been a specified RT instance we are retrieving from the database. Do that and build that specific telescope here
            {
                RadioTelescope existingRT = DatabaseOperations.FetchRadioTelescopeByID(current_rt_id);

                // Turn on telescope in database
                existingRT.online = 1;
                DatabaseOperations.UpdateTelescope(existingRT);

                // These settings are not stored in the database, so they are new every time
                abstractPLCDriver.SetParent(existingRT);
                existingRT.PLCDriver = abstractPLCDriver;
                existingRT.PLCDriver.setTelescopeType(existingRT._TeleType);
                existingRT.SpectraCyberController = BuildSpectraCyber();
                existingRT.Micro_controler = ctrler;
                existingRT.Encoders = encoder;
                logger.Info(Utilities.GetTimeStamp() + ": Existing RadioTelescope with ID " +current_rt_id+ " retrieved and built successfully");

                return existingRT;

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
                    logger.Info(Utilities.GetTimeStamp() + ": Building SpectraCyber");
                    return new SpectraCyberController(new SpectraCyber());

                case 1:
                default:
                    logger.Info(Utilities.GetTimeStamp() + ": Building SpectraCyberSimulator");
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
                    logger.Info(Utilities.GetTimeStamp() + ": Building ProductionPLCDriver");
                    return new ProductionPLCDriver(LocalIPCombo.Text, txtPLCIP.Text, int.Parse(txtMcuCOMPort.Text), int.Parse(txtPLCPort.Text));

                case 1:
                    logger.Info(Utilities.GetTimeStamp() + ": Building ScaleModelPLCDriver");
                    return new ScaleModelPLCDriver(LocalIPCombo.Text, txtPLCIP.Text, int.Parse(txtMcuCOMPort.Text), int.Parse(txtPLCPort.Text));

                case 3:
                    logger.Info(Utilities.GetTimeStamp() + ": Building TestPLCDriver");
                    return new TestPLCDriver(LocalIPCombo.Text, txtPLCIP.Text, int.Parse(txtMcuCOMPort.Text), int.Parse(txtPLCPort.Text), false);
                case 2:
                default:
                    logger.Info(Utilities.GetTimeStamp() + ": Building SimulationPLCDriver");
                    return new SimulationPLCDriver(LocalIPCombo.Text, txtPLCIP.Text, int.Parse(txtMcuCOMPort.Text), int.Parse(txtPLCPort.Text), false, false);
            }
        }

        public AbstractEncoderReader build_encoder(AbstractPLCDriver plc)
        {
            return new SimulatedEncoder(plc, LocalIPCombo.Text, 1602);
        }

        public AbstractMicrocontroller build_CTRL()
        {
            switch (comboMicrocontrollerBox.SelectedIndex)
            {
                case 0:
                    logger.Info(Utilities.GetTimeStamp() + ": Building ProductionPLCDriver");
                    return new MicroControlerControler(LocalIPCombo.Text, 1600);

                case 1:
                    logger.Info(Utilities.GetTimeStamp() + ": Building ScaleModelPLCDriver");
                    return new SimulatedMicrocontroller(-20, 100, true);

                default:
                    logger.Info(Utilities.GetTimeStamp() + ": Building SimulationPLCDriver");
                    return new SimulatedMicrocontroller(SimulationConstants.MIN_MOTOR_TEMP, SimulationConstants.MAX_MOTOR_TEMP, true);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns>  </returns>
        public bool IsMicrocontrollerSimulated()
        {
            bool isSimulated = false;

            logger.Info(Utilities.GetTimeStamp() + ": Selected Microcontroller Type: ");

            if (comboMicrocontrollerBox.SelectedIndex - 1 == (int)MicrocontrollerType.Production)
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
                    logger.Info(Utilities.GetTimeStamp() + ": Building ProductionWeatherStation");
                    return new WeatherStation(1000, int.Parse(txtWSCOMPort.Text));

                case 2:
                    logger.Error(Utilities.GetTimeStamp() + ": The test weather station is not yet supported.");
                    throw new NotImplementedException("The test weather station is not yet supported.");

                case 1:
                default:
                    logger.Info(Utilities.GetTimeStamp() + ": Building SimulationWeatherStation");
                    return new SimulationWeatherStation(1000);
            }
        }


        /// <summary>
        /// Generates a free control form that allows free control access to a radio telescope
        /// instance through the generated form.
        /// </summary>
        private void FreeControl_Click(object sender, EventArgs e)
        {
            logger.Info(Utilities.GetTimeStamp() + ": Free Control Button Clicked");
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
        //    logger.Info(Utilities.GetTimeStamp() + ": Manual Control Button Clicked");
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
            this.txtPLCPort.Text = ((int)(8082 + ProgramPLCDriverList.Count * 3)).ToString();
        }

        private void ProdcheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (ProdcheckBox.Checked)
            {
                loopBackBox.Checked = false;
                this.txtWSCOMPort.Text = "222"; //default WS COM port # is 221
                this.txtMcuCOMPort.Text = "502"; //default MCU Port
                this.txtPLCIP.Text = "192.168.0.50";//default IP address
                if (LocalIPCombo.FindStringExact("192.168.0.70") == -1)
                {
                    this.LocalIPCombo.Items.Add(IPAddress.Parse("192.168.0.70"));
                }
                this.LocalIPCombo.SelectedIndex = LocalIPCombo.FindStringExact("192.168.0.70");
            }
            this.txtPLCPort.Text = "502";
            this.comboPLCType.SelectedIndex = this.comboPLCType.FindStringExact("Production PLC");
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

        private void comboBox2_Click(object sender, EventArgs e)
        {

        }

        private void txtPLCPort_TextChanged(object sender, EventArgs e)
        {
            PLCPortValid = Validator.ValidatePort(txtPLCPort.Text);
            if (!PLCPortValid)
            {
                acceptSettings.Enabled = false;
                txtPLCPort.BackColor = System.Drawing.Color.Yellow;
                this.PLCPortToolTip.Show("Enter a valid port number\n" +
                    " between 1 and 65536", label3);
            }
            else
            {
                txtPLCPort.BackColor = System.Drawing.Color.White;
                this.PLCPortToolTip.Hide(label3);
            }
            if (PLCPortValid && MCUPortValid && MCUIPValid && WCOMPortValid)
            {
                acceptSettings.Enabled = true;
            }

        }

        private void LocalIPCombo_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void txtPLCIP_TextChanged(object sender, EventArgs e)
        {
            MCUIPValid = Validator.ValidateIPAddress(txtPLCIP.Text);
            if (!MCUIPValid)
            {
                acceptSettings.Enabled = false;
                txtPLCIP.BackColor = System.Drawing.Color.Yellow;
                this.MCUIPToolTip.Show("Enter a valid IP Address\n" +
                    " (xxx.xxx.xxx.xxx)", label4);
            }
            else
            {
                txtPLCIP.BackColor = System.Drawing.Color.White;
                this.MCUIPToolTip.Hide(label4);
            }
            if (PLCPortValid && MCUPortValid && MCUIPValid && WCOMPortValid)
            {
                acceptSettings.Enabled = true;
            }
        }

        private void label2_MouseHover(object sender, EventArgs e)
        {
            //this.WCOMPortToolTip.Show("Enter a valid port number, between 1 and 65536", label2);
        }
        private void label3_MouseHover(object sender, EventArgs e)
        {
            //this.PLCPortToolTip.Show("Enter a valid port number, between 1 and 65536", label3);
        }

        private void label4_MouseHover(object sender, EventArgs e)
        {
            //this.MCUIPToolTip.Show("Enter a valid IP Address (xxx.xxx.xxx.xxx)", label4);
        }

        private void label5_MouseHover(object sender, EventArgs e)
        {
            //this.MCUPortToolTip.Show("Enter a valid port number, between 1 and 65536", label5);
        }

        private void txtWSCOMPort_TextChanged(object sender, EventArgs e)
        {
            WCOMPortValid = Validator.ValidatePort(txtWSCOMPort.Text);
            if (!WCOMPortValid)
            {
                acceptSettings.Enabled = false;
                txtWSCOMPort.BackColor = System.Drawing.Color.Yellow;
                this.WCOMPortToolTip.Show("Enter a valid port number\n" +
                    " between 1 and 65536", label2);
            }
            else
            {
                txtWSCOMPort.BackColor = System.Drawing.Color.White;
                this.WCOMPortToolTip.Hide(label2);
                
            }
            if (PLCPortValid && MCUPortValid && MCUIPValid && WCOMPortValid)
            {
                acceptSettings.Enabled = true;
            }

        }

        private void groupBox3_Enter(object sender, EventArgs e)
        {

        }

        private void acceptSettings_Click(object sender, EventArgs e)
        {
                finalSettings = !finalSettings;
                if (finalSettings == false)
                {

                    //Editing text to relflect state -- When the settings are being edited, the button will need to say 'finalize'
                    acceptSettings.Text = "Edit Settings";
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
                    ProdcheckBox.Enabled = false;
                    txtPLCIP.Enabled = false;
                    txtMcuCOMPort.Enabled = false;
                    txtWSCOMPort.Enabled = false;
                    txtPLCPort.Enabled = false;
                }
                else if (finalSettings == true)
                {
                    // Editing Text to reflect state -- when finalized, you can click "edit settings"
                    acceptSettings.Text = "Finalize Settings";

                    startRTGroupbox.BackColor = System.Drawing.Color.DarkGray;
                    startButton.Enabled = false;
                    startButton.BackColor = System.Drawing.Color.LightGray;
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
                    ProdcheckBox.Enabled = true;
                    txtPLCIP.Enabled = true;
                    txtMcuCOMPort.Enabled = true;
                    txtWSCOMPort.Enabled = true;
                    txtPLCPort.Enabled = true;
                }
 
        }

        //Help button clicked ( user interface documentation PDF)
        private void helpButton_click(object sender, EventArgs e)
        {
            string filename = Directory.GetCurrentDirectory() + "\\" + "UIDoc.pdf";
            if (File.Exists(filename))
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
            DatabaseOperations.SetOverrideForSensor(SensorItemEnum.WEATHER_STATION, WSO);
        }

        public bool getWSOverride()
        {
            return MainControlRoomController.ControlRoom.weatherStationOverride;
        }

        private void txtMcuCOMPort_TextChanged(object sender, EventArgs e)
        {
            MCUPortValid = Validator.ValidatePort(txtMcuCOMPort.Text);
            if (!MCUPortValid)
            {
                acceptSettings.Enabled = false;
                txtMcuCOMPort.BackColor = System.Drawing.Color.Yellow;
                this.MCUIPToolTip.Show("Enter a valid port number\n" +
                    "between 1 and 65536", label5);
            }
            else
            {
                txtMcuCOMPort.BackColor = System.Drawing.Color.White;
                this.MCUIPToolTip.Hide(label5);
            }
            if (PLCPortValid && MCUPortValid && MCUIPValid && WCOMPortValid)
            {
                acceptSettings.Enabled = true;
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void comboEncoderType_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void comboPLCType_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

      
    }
}