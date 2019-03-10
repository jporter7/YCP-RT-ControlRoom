using ControlRoomApplication.Controllers;
using ControlRoomApplication.Controllers.PLCController;
using ControlRoomApplication.Controllers.RadioTelescopeControllers;
using ControlRoomApplication.Controllers.SpectraCyberController;
using ControlRoomApplication.Database.Operations;
using ControlRoomApplication.Entities;
using ControlRoomApplication.Entities.Plc;
using ControlRoomApplication.Entities.RadioTelescope;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
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
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (RadioTelescope != null && SpectraCyberController != null && Plc != null)
            {
                Context = new RTDbContext();
                PlcController = new PLCController(Plc);
                RadioTelescope.PlcController = PlcController;
                RadioTelescope.SpectraCyberController = SpectraCyberController;
                RTController = new RadioTelescopeController(RadioTelescope);

                ControlRoom cRoom = new ControlRoom(RTController, Context);
                ControlRoomController crController = new ControlRoomController(cRoom);
                Thread controlRoomThread = new Thread(() => crController.Start());
                controlRoomThread.Start();
                controlRoomThread.Join();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            DatabaseOperations.DisposeLocalDatabaseOnly();
            Environment.Exit(0);
        }

        private void MainForm_Load(object sender, EventArgs e)
        {

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox1.SelectedIndex > -1)
            {
                if (comboBox1.SelectedIndex == 0)
                {
                    // Production PLC
                }
                else if (comboBox1.SelectedIndex == 1)
                {
                    // Test/simulated PLC
                    Plc = new TestPLC();
                }
                else if (comboBox1.SelectedIndex == 2)
                {
                    // VR PLC
                    Plc = new VRPLC();
                }
                else if (comboBox1.SelectedIndex == 3)
                {
                    // Scale Model PLC
                    Plc = new ScaleModelPLC();
                }
            }
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox2.SelectedIndex > -1)
            {
                if (comboBox2.SelectedIndex == 0)
                {
                    SpectraCyberController = new SpectraCyberController(new SpectraCyber());
                }
                else if (comboBox2.SelectedIndex == 1)
                {
                    SpectraCyberController = new SpectraCyberSimulatorController(new SpectraCyberSimulator());
                }
            }
        }

        private void comboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox3.SelectedIndex > -1)
            {
                if (comboBox3.SelectedIndex == 0)
                {
                    RadioTelescope = new ProductionRadioTelescope();
                }
                else if (comboBox3.SelectedIndex == 1)
                {
                    RadioTelescope = new TestRadioTelescope();
                }
                else if (comboBox3.SelectedIndex == 2)
                {
                    RadioTelescope = new ScaleRadioTelescope();
                }
            }
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

        //private void textBox1_TextChanged(object sender, EventArgs e)
        //{

        //}

        public ConfigurationManager ConfigManager { get; set; }
        public RTDbContext Context { get; set; }
        public AbstractPLC Plc { get; set; }
        public PLCController PlcController { get; set; }
        public AbstractSpectraCyberController SpectraCyberController { get; set; }
        public AbstractRadioTelescope RadioTelescope { get; set; }
        public RadioTelescopeController RTController { get; set; }
        public ControlRoom CRoom { get; set; }
        private static readonly log4net.ILog logger =
            log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
    }
}