using ControlRoomApplication.Entities;
using ControlRoomApplication.Simulators.Hardware;
using System.Windows.Forms;
using System.Drawing;
using ControlRoomApplication.Simulators.Hardware.AbsoluteEncoder;


namespace ControlRoomApplication.GUI
{
    public partial class DiagnosticsForm : Form
    {
        private ControlRoom controlRoom;
     
        //TemperatureSensor myTemp = new TemperatureSensor();
        FakeTempSensor myTemp = new FakeTempSensor();

        SimulationAbsoluteEncoder myEncoder;
        double _encoderDegrees = 0;
        double _elevationTemp = 0;
        double _azimuthTemp = 0;
        double _encoderTicks = 0;
        
        bool warningSent = false;
        bool shutdownSent = false;
        
        private int rtId;
        private double az;
        private double el;
        private string[] statuses = { "Offline", "Offline", "Offline", "Offline" };
        private static readonly log4net.ILog logger =
            log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Initializes the diagnostic form based off of the specified configuration.
        /// </summary>
        /// 

        

        public DiagnosticsForm(bool tempSensorSimulated = false)
        {
            InitializeComponent();
           
            if (tempSensorSimulated == true)
            {
               // temperatureSensor = new FakeTemperatureSensor();
            }
            else
            {
               // temperatureSensor = new RealTemperatureSensor();
            }

            az = 0.0;
            el = 0.0;
            timer1.Start();
            logger.Info("DiagnosticsForm Initalized");
        }


        /// <summary>
        /// Initializes the diagnostic form based off of the specified configuration.
        /// </summary>
        /// 
        public DiagnosticsForm(ControlRoom controlRoom, int rtId)
        {
            InitializeComponent();
            az = 0.0;
            el = 0.0;

            this.controlRoom = controlRoom;
            

            this.rtId = rtId;

            dataGridView1.ColumnCount = 2;
            dataGridView1.Columns[0].HeaderText = "Hardware";
            dataGridView1.Columns[1].HeaderText = "Status";

            GetHardwareStatuses();
            string[] spectraCyberRow = { "SpectraCyber", statuses[0] };
            string[] weatherStationRow = { "Weather Station", statuses[1] };
            string[] mcuRow = { "MCU", statuses[2] };
            string[] tempSensorRow = { "Temp Sensor", statuses[3] };

            dataGridView1.Rows.Add(spectraCyberRow);
            dataGridView1.Rows.Add(weatherStationRow);
            dataGridView1.Rows.Add(mcuRow);
            dataGridView1.Update();

            SetCurrentAzimuthAndElevation();
            logger.Info("DiagnosticsForm Initalized");
        }

        private void SetCurrentAzimuthAndElevation()
        {
            label3.Text = controlRoom.RadioTelescopeControllers[rtId].GetCurrentOrientation().Azimuth.ToString("0.00");
            label4.Text = controlRoom.RadioTelescopeControllers[rtId].GetCurrentOrientation().Elevation.ToString("0.00");
        }

        /// <summary>
        /// Gets and displays the current statuses of the hardware components for the specified configuration.
        /// </summary>
        private void GetHardwareStatuses()
        {
            if (controlRoom.RadioTelescopes[rtId].SpectraCyberController.IsConsideredAlive())
            {
                statuses[0] = "Online";
            } 

            if (controlRoom.WeatherStation.IsConsideredAlive())
            {
                statuses[1] = "Online";
            }

            

            
        }

       
      

        public delegate void SetStartTimeTextCallback(string text);
        public void SetStartTimeText(string text)
        {
            if (startTimeTextBox.InvokeRequired)
            {
                SetStartTimeTextCallback d = new SetStartTimeTextCallback(SetStartTimeText);
                Invoke(d, new object[] { text });
            }
            else
            {
                startTimeTextBox.Text = text;
            }
        }

        public delegate void SetEndTimeTextCallback(string text);
        public void SetEndTimeText(string text)
        {
            if (endTimeTextBox.InvokeRequired)
            {
                SetEndTimeTextCallback d = new SetEndTimeTextCallback(SetEndTimeText);
                Invoke(d, new object[] { text });
            }
            else
            {
                endTimeTextBox.Text = text;
            }
        }

        public delegate void SetApptStatusTextCallback(string text);
        public void SetApptStatusText(string text)
        {
            if (statusTextBox.InvokeRequired)
            {
                SetApptStatusTextCallback d = new SetApptStatusTextCallback(SetApptStatusText);
                Invoke(d, new object[] { text });
            }
            else
            {
                statusTextBox.Text = text;
            }
        }

        private void timer1_Tick(object sender, System.EventArgs e)
        {
            double elevationTemperature = 0.0;
            double azimuthTemperature = 0.0;
            
            timer1.Interval = 100;
           

            if (selectDemo.Checked == true)
            {
                timer1.Interval = 1000;
                elevationTemperature = myTemp.GetElevationTemperature();
                azimuthTemperature = myTemp.GetAzimuthTemperature();
            }
            
            
            fldElTemp.Text = elevationTemperature.ToString();
            fldAzTemp.Text = azimuthTemperature.ToString();
            lblDisplayDegreesEncoders.Text = _encoderDegrees.ToString();


            /*** Temperature Logic Start***/

            if(elevationTemperature <= 79 && azimuthTemperature <= 79)
            {
                warningLabel.Visible = false;
                lblShutdown.Visible = false;
                fanLabel.Visible = false;
                warningSent = false;
                shutdownSent = false;
            }
            else if(elevationTemperature > 79 && elevationTemperature < 100 || azimuthTemperature > 79 && azimuthTemperature < 100)
            {
                if(warningSent == false)
                {
                    warningLabel.Visible = true;
                }
                else
                {
                    warningLabel.Visible = false;
                }
               
                lblShutdown.Visible = false;
                warningLabel.ForeColor = Color.Yellow;
                warningLabel.Text = "Warning Sent";

                warningSent = true;

                fanLabel.Visible = true;
                fanLabel.ForeColor = Color.Blue;
                fanLabel.Text = "Fans On";
               
            }
            else if(elevationTemperature >= 100 || azimuthTemperature >= 100)
            {
                warningLabel.Visible = false;

                if (shutdownSent == false)
                {
                    lblShutdown.Visible = true;
                }
                else
                {
                    lblShutdown.Visible = false;
                }
               
                lblShutdown.ForeColor = Color.Red;
                lblShutdown.Text = "Shutdown Sent";

                shutdownSent = true;

                fanLabel.Visible = true;
                fanLabel.ForeColor = Color.Blue;
                fanLabel.Text = "Fans Stay On";


            }
            else
            {
                warningLabel.Visible = false;
                warningLabel.ForeColor = Color.Black;
                warningLabel.Text = "";

                fanLabel.Visible = false;
                fanLabel.ForeColor = Color.Blue;
                fanLabel.Text = "Fans On";
            }

            /*** Temperature Logic End***/

            if (controlRoom.RTControllerManagementThreads[rtId].AppointmentToDisplay != null)
            {
                SetStartTimeText(controlRoom.RTControllerManagementThreads[rtId].AppointmentToDisplay.StartTime.ToLocalTime().ToString("hh:mm tt"));
                SetEndTimeText(controlRoom.RTControllerManagementThreads[rtId].AppointmentToDisplay.EndTime.ToLocalTime().ToString("hh:mm tt"));
                SetApptStatusText(controlRoom.RTControllerManagementThreads[rtId].AppointmentToDisplay.Status.ToString());
            }

            GetHardwareStatuses();

            SetCurrentAzimuthAndElevation();

            dataGridView1.Update();
           

        }

        private void DiagnosticsForm_Load(object sender, System.EventArgs e)
        {

        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void btnTest_Click(object sender, System.EventArgs e)
        {
            double temperature = 0;

            if(double.TryParse(txtTemperature.Text, out temperature))
            {
                fldAzTemp.Text = temperature.ToString();
            }
            else
            {
                MessageBox.Show("Invalid entry in Temperature field", "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnAddOneTemp_Click(object sender, System.EventArgs e)
        {
            _elevationTemp += 1;
        }

        private void btnAddFiveTemp_Click(object sender, System.EventArgs e)
        {
            _elevationTemp += 5;
        }

        private void button1_Click(object sender, System.EventArgs e)
        {
            _elevationTemp -= 1;
        }

        private void button2_Click(object sender, System.EventArgs e)
        {
            _elevationTemp -= 5;
        }

        private void btnAddXTemp_Click(object sender, System.EventArgs e)
        {
            double tempVal; //temperature value
           

            if (double.TryParse(txtCustTemp.Text, out tempVal))
            {
                _elevationTemp += tempVal;
            }
            else
            {
                MessageBox.Show("Invalid entry in Temperature field", "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button3_Click(object sender, System.EventArgs e)
        {
            double tempVal; //temperature value


            if (double.TryParse(txtCustTemp.Text, out tempVal))
            {
                _elevationTemp -= tempVal;
            }
            else
            {
                MessageBox.Show("Invalid entry in Temperature field", "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void timer2_Tick(object sender, System.EventArgs e)
        {

        }

        private void lblAzLimStatus2_Click(object sender, System.EventArgs e)
        {

        }

        private void button4_Click(object sender, System.EventArgs e)
        {
            _encoderDegrees += 1;
        }

        private void button5_Click(object sender, System.EventArgs e)
        {
            _encoderDegrees += 5;
        }

        private void btnSubtractOneEncoder_Click(object sender, System.EventArgs e)
        {
            _encoderDegrees -= 1;
        }

        private void btnSubtractFiveEncoder_Click(object sender, System.EventArgs e)
        {
            _encoderDegrees -= 5;
        }

        private void btnAddXEncoder_Click(object sender, System.EventArgs e)
        {
            double encVal; //encoder value


            if (double.TryParse(txtCustEncoderVal.Text, out encVal))
            {
                _encoderDegrees += encVal;
            }
            else
            {
                MessageBox.Show("Invalid entry in Encoder field", "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnSubtractXEncoder_Click(object sender, System.EventArgs e)
        {
            double encVal; //encoder value


            if (double.TryParse(txtCustEncoderVal.Text, out encVal))
            {
                _encoderDegrees -= encVal;
            }
            else
            {
                MessageBox.Show("Invalid entry in Encoder field", "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
