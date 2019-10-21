using ControlRoomApplication.Entities;
using ControlRoomApplication.Simulators.Hardware;
using System.Windows.Forms;
using System.Drawing;
using ControlRoomApplication.Simulators.Hardware.AbsoluteEncoder;
using ControlRoomApplication.Simulators.Hardware.MCU;
using ControlRoomApplication.Controllers;
using ControlRoomApplication.Controllers.BlkHeadUcontroler;
using ControlRoomApplication.Database;
using ControlRoomApplication.Constants;
using System;

namespace ControlRoomApplication.GUI
{
    public partial class DiagnosticsForm : Form
    {
        private ControlRoom controlRoom;
        EncoderReader encoderReader = new EncoderReader("192.168.7.2",1602);
        ControlRoomApplication.Entities.Orientation azimuthOrientation = new ControlRoomApplication.Entities.Orientation();
        

        private int demoIndex = 0;
        //private PLC PLC; This needs to be defined once I can get find the currect import

        //TemperatureSensor myTemp = new TemperatureSensor();
        FakeTempSensor myTemp = new FakeTempSensor();
        /***********DEMO MODE VARIABLES**************/
        private double[] azEncDemo = {0, 0.2, 0.4, 0.6, 0.8, 1, 1.2, 1.4, 1.6, 1.8, 2.0, 2.2, 2.4, 2.6, 2.8, 3.0, 3.2, 3.4, 3.6, 3.8, 4.0, 4.2, 4.4, 4.6, 4.8, 5.0, 5.2, 5.4, 5.6, 5.8, 6.0, 6.2, 6.4, 6.6, 6.8, 7.0, 7.2, 7.4, 7.6, 7.8, 8.0, 8.2, 8.4, 8.6, 8.8, 9.0, 9.2, 9.4, 9.6, 9.8, 10.0, 10.2, 10.4, 10.6, 10.8, 11.0, 11.2, 11.4, 11.6, 11.8, 12.0, 12.2, 12.4, 12.6, 12.8, 13.0, 13.2, 13.4, 13.6, 13.8, 14.0, 14.2, 14.4, 14.6, 14.8, 15.0, 15.2, 15.4, 15.6, 15.8, 16.0 }; //12    11.3 ticks per degree
        private double[] elEncDemo = { 0, 0.2, 0.4, 0.6, 0.8, 1, 1.2, 1.4, 1.6, 1.8, 2.0, 2.2, 2.4, 2.6, 2.8, 3.0, 3.2, 3.4, 3.6, 3.8, 4.0, 4.2, 4.4, 4.6, 4.8, 5.0, 5.2, 5.4, 5.6, 5.8, 6.0, 6.2, 6.4, 6.6, 6.8, 7.0, 7.2, 7.4, 7.6, 7.8, 8.0, 8.2, 8.4, 8.6, 8.8, 9.0, 9.2, 9.4, 9.6, 9.8, 10.0, 10.2, 10.4, 10.6, 10.8, 11.0, 11.2, 11.4, 11.6, 11.8, 12.0, 12.2, 12.4, 12.6, 12.8, 13.0, 13.2, 13.4, 13.6, 13.8, 14.0, 14.2, 14.4, 14.6, 14.8, 15.0, 15.2, 15.4, 15.6, 15.8, 16.0 }; //10 bits of precision, 2.8 
       



         
 
        /***********DEMO MODE VARIABLES END*********/
    
       
        double _azEncoderDegrees = 0;
        double _elEncoderDegrees = 0;
        double _elevationTemp = 0;
        int _azEncoderTicks = 0;
        int _elEncoderTicks = 0;
        
        
        bool warningSent = false;
        bool shutdownSent = false;
        
        private int rtId;
        private string[] statuses = { "Offline", "Offline", "Offline", "Offline" };
        private static readonly log4net.ILog logger =
            log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);


        /// <summary>
        /// Initializes the diagnostic form based off of the specified configuration.
        /// </summary>
        /// 
        public DiagnosticsForm(ControlRoom controlRoom, int rtId)
        {
            InitializeComponent();

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


            MCU_Statui.ColumnCount = 2;
            MCU_Statui.Columns[0].HeaderText = "Status name";
            MCU_Statui.Columns[1].HeaderText = "value";

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
        private void GetHardwareStatuses() {
            if(controlRoom.RadioTelescopes[rtId].SpectraCyberController.IsConsideredAlive()) {
                statuses[0] = "Online";
            }

            if(controlRoom.WeatherStation.IsConsideredAlive()) {
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


        /***************************************************************
         * ***************TIMER TICK FUNCTION STARTS HERE***************
         * ******this comment was added to make this easier to find*****
         * ************************************************************/
        private void timer1_Tick(object sender, System.EventArgs e)
        {

            double elevationTemperature = 0.0;
            double azimuthTemperature = 0.0;
            //int ticks = azEncoder.CurrentPositionTicks;

            //Read actual encoder values
            // _azEncoderDegrees = Controllers.BlkHeadUcontroler.EncoderReader.GetCurentOrientation().Azimuth;
            // _elEncoderDegrees = Controllers.BlkHeadUcontroler.EncoderReader.GetCurentOrientation().Elevation;

            _azEncoderDegrees = controlRoom.RadioTelescopeControllers[rtId].GetAbsoluteOrientation().Azimuth;//.GetCurrentOrientation().Azimuth;
            _elEncoderDegrees = controlRoom.RadioTelescopeControllers[rtId].GetAbsoluteOrientation().Elevation; //GetCurrentOrientation().Elevation;


            elevationTemperature = DatabaseOperations.GetCurrentTemp( SensorLocationEnum.EL_MOTOR ).temp;
            azimuthTemperature = DatabaseOperations.GetCurrentTemp( SensorLocationEnum.AZ_MOTOR ).temp;

            this.label22.Text = (!controlRoom.RadioTelescopeControllers[rtId].finished_exicuting_move( RadioTelescopeAxisEnum.AZIMUTH)).ToString();

            timer1.Interval = 200;
           

            if (selectDemo.Checked == true)
            {
                elevationTemperature = myTemp.GetElevationTemperature();
                azimuthTemperature = myTemp.GetAzimuthTemperature();


                if(demoIndex < 79)
                {
                    _azEncoderDegrees = azEncDemo[demoIndex++];
                    _elEncoderDegrees = elEncDemo[demoIndex];


                    _azEncoderTicks = (int)(_azEncoderDegrees * 11.38);
                    _elEncoderTicks = (int)(_elEncoderDegrees * 2.8);
                }
                else
                {
                    demoIndex = 0;
                }
               

            }
            
            
            fldElTemp.Text = elevationTemperature.ToString();
            fldAzTemp.Text = azimuthTemperature.ToString();
            lblAzEncoderDegrees.Text = _azEncoderDegrees.ToString();
            lblElEncoderDegrees.Text = _elEncoderDegrees.ToString();
            lblAzEncoderTicks.Text = _azEncoderTicks.ToString();
            lblElEncoderTicks.Text = _elEncoderTicks.ToString();


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
            /*
            if (controlRoom.RTControllerManagementThreads[rtId].AppointmentToDisplay != null)
            {
                SetStartTimeText(controlRoom.RTControllerManagementThreads[rtId].AppointmentToDisplay.StartTime.ToLocalTime().ToString("hh:mm tt"));
                SetEndTimeText(controlRoom.RTControllerManagementThreads[rtId].AppointmentToDisplay.EndTime.ToLocalTime().ToString("hh:mm tt"));
                SetApptStatusText(controlRoom.RTControllerManagementThreads[rtId].AppointmentToDisplay.Status.ToString());
            }
            //*/
            GetHardwareStatuses();

            SetCurrentAzimuthAndElevation();

            dataGridView1.Update();


            if(MCU_Statui.Rows.Count > 5) {
                bool[] stuti = controlRoom.RadioTelescopes[rtId].PLCDriver.GET_MCU_Status( RadioTelescopeAxisEnum.AZIMUTH ).GetAwaiter().GetResult();
                foreach(MCUConstants.MCUStutusBits stutusBit in (MCUConstants.MCUStutusBits[])Enum.GetValues( typeof( MCUConstants.MCUStutusBits ) )) {
                    string[] row = { stutusBit.ToString() , stuti[(int)stutusBit].ToString() };
                    //MCU_Statui.Rows[(int)stutusBit][1] = stuti[(int)stutusBit].ToString();
                    //string[] row = { ((PLC_modbus_server_register_mapping)reg).ToString() , Convert.ToString( val[reg + 1] ).PadLeft( 5 ) };//.Replace(" ", "0") };
                    //PLC_regs.Rows.Add(row);
                    MCU_Statui.Rows[(int)stutusBit].SetValues( row );
                }
            } else {
                MCU_Statui.Rows.Clear();
                bool[] stuti = controlRoom.RadioTelescopes[rtId].PLCDriver.GET_MCU_Status( RadioTelescopeAxisEnum.AZIMUTH ).GetAwaiter().GetResult();
                foreach(MCUConstants.MCUStutusBits stutusBit in (MCUConstants.MCUStutusBits[])Enum.GetValues( typeof( MCUConstants.MCUStutusBits ) )) {
                    string[] row = { stutusBit.ToString() , stuti[(int)stutusBit].ToString() };
                    MCU_Statui.Rows.Add( row );
                }
                MCU_Statui.Update();
            }




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

        //private void btnAddXTemp_Click(object sender, System.EventArgs e)
        //{
        //    double tempVal; //temperature value
           

        //    if (double.TryParse(txtCustTemp.Text, out tempVal))
        //    {
        //        _elevationTemp += tempVal;
        //    }
        //    else
        //    {
        //        MessageBox.Show("Invalid entry in Temperature field", "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
        //    }
        //}

        private void button3_Click(object sender, System.EventArgs e)
        {
            double tempVal; //temperature value


            //if (double.TryParse(txtCustTemp.Text, out tempVal))
            //{
            //    _elevationTemp -= tempVal;
            //}
            //else
            //{
            //    MessageBox.Show("Invalid entry in Temperature field", "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            //}
        }

        private void timer2_Tick(object sender, System.EventArgs e)
        {

        }

        private void lblAzLimStatus2_Click(object sender, System.EventArgs e)
        {

        }

        private void button4_Click(object sender, System.EventArgs e)
        {
            _azEncoderDegrees += 1;
            
        }

        private void button5_Click(object sender, System.EventArgs e)
        {
            _azEncoderDegrees += 5;
        }

        private void btnSubtractOneEncoder_Click(object sender, System.EventArgs e)
        {
            _azEncoderDegrees -= 1;
            
        }

        private void btnSubtractFiveEncoder_Click(object sender, System.EventArgs e)
        {
            _azEncoderDegrees -= 5;
        }

        private void btnAddXEncoder_Click(object sender, System.EventArgs e)
        {
            double encVal; //encoder value


            if (double.TryParse(txtCustEncoderVal.Text, out encVal))
            {
                _azEncoderDegrees += encVal;
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
                _azEncoderDegrees -= encVal;
            }
            else
            {
                MessageBox.Show("Invalid entry in Encoder field", "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void tabPage1_Click(object sender, EventArgs e)
        {

        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void groupBox4_Enter(object sender, EventArgs e)
        {

        }

        private void groupBox6_Enter(object sender, EventArgs e)
        {

        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void tabPage3_Click(object sender, EventArgs e)
        {

        }

        private void splitContainer1_Panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void splitContainer1_SplitterMoved(object sender, SplitterEventArgs e)
        {

        }

        private void fanLabel_Click(object sender, EventArgs e)
        {

        }

        private void lblShutdown_Click(object sender, EventArgs e)
        {

        }
    }
}
