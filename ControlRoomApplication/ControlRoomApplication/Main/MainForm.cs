using ControlRoomApplication.Controllers;
using ControlRoomApplication.Controllers.PLCCommunication;
using ControlRoomApplication.Controllers.RadioTelescopeControllers;
using ControlRoomApplication.Controllers.SpectraCyberController;
using ControlRoomApplication.Database.Operations;
using ControlRoomApplication.Entities;
using ControlRoomApplication.Entities.RadioTelescope;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Windows.Forms;

namespace ControlRoomApplication.Main
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
            logger.Info("<--------------- Control Room Application Started --------------->");
            dataGridView1.ColumnCount = 2;
            //Console.SetOut(new ControlWriter(textBox1));
            DatabaseOperations.InitializeLocalConnectionOnly();
            DatabaseOperations.PopulateLocalDatabase();

            List<KeyValuePair<AbstractRadioTelescope, AbstractPLCDriver>> AbstractRTDriverPairList = new List<KeyValuePair<AbstractRadioTelescope, AbstractPLCDriver>>();
            List<RadioTelescopeController> ProgramRTControllerList = new List<RadioTelescopeController>();
            List<AbstractPLCDriver> ProgramPLCDriverList = new List<AbstractPLCDriver>();
            List<ControlRoomController> ProgramControlRoomControllerList = new List<ControlRoomController>();

            CancellationSource = new CancellationTokenSource();
            Token = CancellationSource.Token;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text != null 
                && textBox2.Text != null 
                && comboBox3.SelectedIndex > -1
                && comboBox1.SelectedIndex > -1)
            {
                AbstractRadioTelescope ARadioTelescope = BuildRT();
                AbstractPLCDriver APLCDriver = BuildPLCDriver();

                AbstractRTDriverPairList.Add(new KeyValuePair<AbstractRadioTelescope, AbstractPLCDriver>(ARadioTelescope, APLCDriver));
                ProgramRTControllerList.Add(new RadioTelescopeController(AbstractRTDriverPairList[AbstractRTDriverPairList.Count].Key));
                ProgramPLCDriverList.Add(APLCDriver);
                ProgramControlRoomControllerList.Add(new ControlRoomController(new ControlRoom(ProgramRTControllerList[ProgramRTControllerList.Count])));

                ProgramPLCDriverList[ProgramPLCDriverList.Count].StartAsyncAcceptingClients();
                ProgramRTControllerList[ProgramRTControllerList.Count].RadioTelescope.PlcController.ConnectToServer();
                ProgramControlRoomControllerList[ProgramControlRoomControllerList.Count].Start();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            // Notify cancellation token
            CancellationSource.Cancel();

            //
            

            ControlRoomThread.Join();
            DatabaseOperations.DisposeLocalDatabaseOnly();
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
                CRoom.RadioTelescopeController.RadioTelescope.CurrentOrientation.Azimuth.ToString() };
            string[] row3 = { "Current RT Elevation:",
                CRoom.RadioTelescopeController.RadioTelescope.CurrentOrientation.Elevation.ToString() };

            dataGridView1.Rows.Add(row1);
            dataGridView1.Rows.Add(row2);
            dataGridView1.Rows.Add(row3);
        }

        public AbstractRadioTelescope BuildRT()
        {
            PLCClientCommunicationHandler PLCCommsHandler = new PLCClientCommunicationHandler(textBox2.Text, int.Parse(textBox1.Text));

            // Create Radio Telescope Location
            Location location = new Location(76.7046, 40.0244, 395.0); // John Rudy Park hardcoded for now

            switch (comboBox3.SelectedIndex)
            {
                case 0:
                    return new ProductionRadioTelescope(BuildSpectraCyber(), PLCCommsHandler, location);

                case 1:
                    // Case for the test/simulated radiotelescope.
                    return new TestRadioTelescope(BuildSpectraCyber(), PLCCommsHandler, location);

                case 2:
                default:
                    // Should be changed once we have a simulated radiotelescope class implemented
                    return new ScaleRadioTelescope(BuildSpectraCyber(), PLCCommsHandler, location);
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
                default:
                    // Should be changed once we have a simulated radiotelescope class implemented
                    return new TestPLCDriver(textBox2.Text, int.Parse(textBox1.Text));
            }
        }

        public List<KeyValuePair<AbstractRadioTelescope, AbstractPLCDriver>> AbstractRTDriverPairList { get; set; }
        public List<RadioTelescopeController> ProgramRTControllerList { get; set; }
        public List<AbstractPLCDriver> ProgramPLCDriverList { get; set; }
        public List<ControlRoomController> ProgramControlRoomControllerList { get; set; }

        private Thread ControlRoomThread { get; set; }
        private CancellationTokenSource CancellationSource { get; set; }
        private CancellationToken Token { get; set; }
        private static readonly log4net.ILog logger =
            log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
    }
}