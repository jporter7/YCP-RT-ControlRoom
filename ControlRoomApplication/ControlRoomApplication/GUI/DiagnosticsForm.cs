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
using ControlRoomApplication.Main;

namespace ControlRoomApplication.GUI
{
    public partial class DiagnosticsForm : Form
    {
        private ControlRoom controlRoom;
        EncoderReader encoderReader = new EncoderReader("192.168.7.2", 1602);
        ControlRoomApplication.Entities.Orientation azimuthOrientation = new ControlRoomApplication.Entities.Orientation();


        private int demoIndex = 0;
        //private PLC PLC; This needs to be defined once I can get find the currect import

        FakeEncoderSensor myEncoder = new FakeEncoderSensor();
        /***********DEMO MODE VARIABLES**************/
        private double[] azEncDemo = { 0, 0.2, 0.4, 0.6, 0.8, 1, 1.2, 1.4, 1.6, 1.8, 2.0, 2.2, 2.4, 2.6, 2.8, 3.0, 3.2, 3.4, 3.6, 3.8, 4.0, 4.2, 4.4, 4.6, 4.8, 5.0, 5.2, 5.4, 5.6, 5.8, 6.0, 6.2, 6.4, 6.6, 6.8, 7.0, 7.2, 7.4, 7.6, 7.8, 8.0, 8.2, 8.4, 8.6, 8.8, 9.0, 9.2, 9.4, 9.6, 9.8, 10.0, 10.2, 10.4, 10.6, 10.8, 11.0, 11.2, 11.4, 11.6, 11.8, 12.0, 12.2, 12.4, 12.6, 12.8, 13.0, 13.2, 13.4, 13.6, 13.8, 14.0, 14.2, 14.4, 14.6, 14.8, 15.0, 15.2, 15.4, 15.6, 15.8, 16.0 }; //12    11.3 ticks per degree
        private double[] elEncDemo = { 0, 0.2, 0.4, 0.6, 0.8, 1, 1.2, 1.4, 1.6, 1.8, 2.0, 2.2, 2.4, 2.6, 2.8, 3.0, 3.2, 3.4, 3.6, 3.8, 4.0, 4.2, 4.4, 4.6, 4.8, 5.0, 5.2, 5.4, 5.6, 5.8, 6.0, 6.2, 6.4, 6.6, 6.8, 7.0, 7.2, 7.4, 7.6, 7.8, 8.0, 8.2, 8.4, 8.6, 8.8, 9.0, 9.2, 9.4, 9.6, 9.8, 10.0, 10.2, 10.4, 10.6, 10.8, 11.0, 11.2, 11.4, 11.6, 11.8, 12.0, 12.2, 12.4, 12.6, 12.8, 13.0, 13.2, 13.4, 13.6, 13.8, 14.0, 14.2, 14.4, 14.6, 14.8, 15.0, 15.2, 15.4, 15.6, 15.8, 16.0 }; //10 bits of precision, 2.8 

        DateTime currentEncodDate = DateTime.Now;

        /***********DEMO MODE VARIABLES END*********/
        // Encoder Variables
        double _azEncoderDegrees = 0;
        double _elEncoderDegrees = 0;
        double _elevationTemp = 0;
        int _azEncoderTicks = 0;
        int _elEncoderTicks = 0;

        // Azimuth Limit Switch Variables
        bool _azCCWLimitChange = false;
        bool _azCWLimitOld = false;

        bool _azCWLimitChange = false;
        bool _azCCWLimitOld = false;

        // Elevation Limit Switch Variables
        bool _elLowerLimitChange = false;
        bool _elLowerLimitOld = false;

        bool _elUpperLimitChange = false;
        bool _elUpperLimitOld = false;

        // Azimuth Proximity Sensor Variables
        bool _azCCWProxOld = false;
        bool _azCCWProxChange = false;

        bool _azCWProxOld = false;
        bool _azCWProxChange = false;

        bool _azCloserUpperProx = false;

        // Elevation Proximity Sensor Variables
        bool _elLowerProxOld = false;
        bool _elLowerProxChange = false;

        bool _elUpperProxOld = false;
        bool _elUpperProxChange = false;

        // Alert Flags
        bool farenheit = true;
        

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


            //MCU_Statui.ColumnCount = 2;
            //MCU_Statui.Columns[0].HeaderText = "Status name";
            //MCU_Statui.Columns[1].HeaderText = "value";

            controlRoom.RadioTelescopes[rtId].Micro_controler.BringUp();

            SetCurrentWeatherData();
            runDiagScriptsButton.Enabled = false;
            logger.Info("DiagnosticsForm Initalized");
        }

        private void SetCurrentWeatherData()
        {
            windSpeedLabel.Text = Math.Round(controlRoom.WeatherStation.GetWindSpeed(), 2).ToString();
            windDirLabel.Text = controlRoom.WeatherStation.GetWindDirection();
            dailyRainfallLabel.Text = Math.Round(controlRoom.WeatherStation.GetDailyRain(), 2).ToString();
            rainRateLabel.Text = Math.Round(controlRoom.WeatherStation.GetRainRate(), 2).ToString();
            //outsideTempLabel.Text = Math.Round(controlRoom.WeatherStation.GetOutsideTemp(), 2).ToString();
            //insideTempLabel.Text = Math.Round(controlRoom.WeatherStation.GetInsideTemp(), 2).ToString();
            barometricPressureLabel.Text = Math.Round(controlRoom.WeatherStation.GetBarometricPressure(), 2).ToString();
        }

        /// <summary>
        /// Gets and displays the current statuses of the hardware components for the specified configuration.
        /// </summary>
        private void GetHardwareStatuses() {
            if (controlRoom.RadioTelescopes[rtId].SpectraCyberController.IsConsideredAlive()) {
                statuses[0] = "Online";
            }

            if (controlRoom.WeatherStation.IsConsideredAlive()) {
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
            double currWindSpeed = controlRoom.WeatherStation.GetWindSpeed();//wind speed

            _azEncoderDegrees = controlRoom.RadioTelescopeControllers[rtId].GetAbsoluteOrientation().Azimuth;
            _elEncoderDegrees = controlRoom.RadioTelescopeControllers[rtId].GetAbsoluteOrientation().Elevation;

            timer1.Interval = 200;

            if (selectDemo.Checked == true)
            {
                controlRoom.RadioTelescopes[rtId].Micro_controler.setStableOrTesting(false);

                // Simulating Encoder Sensors
                TimeSpan elapsedEncodTime = DateTime.Now - currentEncodDate;

                if (elapsedEncodTime.TotalSeconds > 15)
                {
                    if (myEncoder.getLeftOrRight() == true && (myEncoder.GetAzimuthAngle() < 340 && myEncoder.GetAzimuthAngle() > 15))
                        myEncoder.SetAzimuthAngle(340);
                    else if (myEncoder.getLeftOrRight() == false && (myEncoder.GetAzimuthAngle() > 15 && myEncoder.GetAzimuthAngle() < 340))
                        myEncoder.SetAzimuthAngle(15);

                    if (myEncoder.getUpOrDown() == true && (myEncoder.GetElevationAngle() < 80 && myEncoder.GetElevationAngle() > 0))
                        myEncoder.SetElevationAngle(80);
                    else if (myEncoder.getUpOrDown() == false && (myEncoder.GetElevationAngle() > 0 && myEncoder.GetElevationAngle() < 80))
                        myEncoder.SetElevationAngle(0);

                    currentEncodDate = DateTime.Now;
                }

                _azEncoderDegrees = myEncoder.GetAzimuthAngle();
                _elEncoderDegrees = myEncoder.GetElevationAngle();

                _azEncoderTicks = (int)(_azEncoderDegrees * 11.38);
                _elEncoderTicks = (int)(_elEncoderDegrees * 2.8);

                // Azimuth Limit Switch sim logic
                if (_azEncoderDegrees < SimulationConstants.LIMIT_CW_AZ_DEGREES)
                {
                    if (!_azCWLimitChange)
                    {
                        _azCWLimitOld = controlRoom.RadioTelescopes[rtId].PLCDriver.limitSwitchData.Azimuth_CW_Limit;
                        _azCWLimitChange = true;
                    }
                    controlRoom.RadioTelescopes[rtId].PLCDriver.limitSwitchData.Azimuth_CW_Limit = true;
                }
                else if (_azEncoderDegrees > SimulationConstants.LIMIT_CCW_AZ_DEGREES)
                {
                    if (!_azCCWLimitChange)
                    {
                        _azCCWLimitOld = controlRoom.RadioTelescopes[rtId].PLCDriver.limitSwitchData.Azimuth_CCW_Limit;
                        _azCCWLimitChange = true;
                    }
                    controlRoom.RadioTelescopes[rtId].PLCDriver.limitSwitchData.Azimuth_CCW_Limit = true;
                }
                else
                {
                    if (!_azCWLimitChange)
                    {
                        _azCWLimitOld = controlRoom.RadioTelescopes[rtId].PLCDriver.limitSwitchData.Azimuth_CW_Limit;
                        _azCWLimitChange = true;
                    }
                    controlRoom.RadioTelescopes[rtId].PLCDriver.limitSwitchData.Azimuth_CW_Limit = false;

                    if (!_azCCWLimitChange)
                    {
                        _azCWLimitOld = controlRoom.RadioTelescopes[rtId].PLCDriver.limitSwitchData.Azimuth_CCW_Limit;
                        _azCCWLimitChange = true;
                    }
                    controlRoom.RadioTelescopes[rtId].PLCDriver.limitSwitchData.Azimuth_CCW_Limit = false;
                }

                // Elevation Limit Switch sim logic
                if (_elEncoderDegrees < SimulationConstants.LIMIT_LOW_EL_DEGREES)
                {
                    if (!_elLowerLimitChange)
                    {
                        _elLowerLimitOld = controlRoom.RadioTelescopes[rtId].PLCDriver.limitSwitchData.Elevation_Lower_Limit;
                        _elLowerLimitChange = true;
                    }
                    controlRoom.RadioTelescopes[rtId].PLCDriver.limitSwitchData.Elevation_Lower_Limit = true;
                }
                else if (_elEncoderDegrees > SimulationConstants.LIMIT_HIGH_EL_DEGREES)
                {
                    if (!_elUpperLimitChange)
                    {
                        _elUpperLimitOld = controlRoom.RadioTelescopes[rtId].PLCDriver.limitSwitchData.Elevation_Upper_Limit;
                        _elUpperLimitChange = true;
                    }
                    controlRoom.RadioTelescopes[rtId].PLCDriver.limitSwitchData.Elevation_Upper_Limit = true;
                }
                else
                {
                    if (!_elLowerLimitChange)
                    {
                        _elLowerLimitOld = controlRoom.RadioTelescopes[rtId].PLCDriver.limitSwitchData.Elevation_Lower_Limit;
                        _elLowerLimitChange = true;
                    }
                    controlRoom.RadioTelescopes[rtId].PLCDriver.limitSwitchData.Elevation_Lower_Limit = false;

                    if (!_elUpperLimitChange)
                    {
                        _elUpperLimitOld = controlRoom.RadioTelescopes[rtId].PLCDriver.limitSwitchData.Elevation_Upper_Limit;
                        _elUpperLimitChange = true;
                    }
                    controlRoom.RadioTelescopes[rtId].PLCDriver.limitSwitchData.Elevation_Upper_Limit = false;
                }

                // Azimuth Proximity Sensor Logic
                if (_azEncoderDegrees < SimulationConstants.PROX_CW_AZ_DEGREES)
                {
                    if (!_azCWProxChange)
                    {
                        _azCWProxOld = controlRoom.RadioTelescopes[rtId].PLCDriver.proximitySensorData.Azimuth_CW_Prox_Sensor;
                        _azCWProxChange = true;
                    }
                    controlRoom.RadioTelescopes[rtId].PLCDriver.proximitySensorData.Azimuth_CW_Prox_Sensor = true;
                }
                else if (_azEncoderDegrees > SimulationConstants.PROX_CCW_AZ_DEGREES)
                {
                    if (!_azCCWProxChange)
                    {
                        _azCCWProxOld = controlRoom.RadioTelescopes[rtId].PLCDriver.proximitySensorData.Azimuth_CCW_Prox_Sensor;
                        _azCCWProxChange = true;
                    }
                    controlRoom.RadioTelescopes[rtId].PLCDriver.proximitySensorData.Azimuth_CCW_Prox_Sensor = true;
                }
                else
                {
                    if (!_azCWProxChange)
                    {
                        _azCWProxOld = controlRoom.RadioTelescopes[rtId].PLCDriver.proximitySensorData.Azimuth_CW_Prox_Sensor;
                        _azCWProxChange = true;
                    }
                    controlRoom.RadioTelescopes[rtId].PLCDriver.proximitySensorData.Azimuth_CW_Prox_Sensor = false;

                    if (!_azCCWProxChange)
                    {
                        _azCCWProxOld = controlRoom.RadioTelescopes[rtId].PLCDriver.proximitySensorData.Azimuth_CCW_Prox_Sensor;
                        _azCCWProxChange = true;
                    }
                    controlRoom.RadioTelescopes[rtId].PLCDriver.proximitySensorData.Azimuth_CCW_Prox_Sensor = false;
                }

                // Elevation Proximity Logic
                if (_elEncoderDegrees < SimulationConstants.LIMIT_LOW_EL_DEGREES)
                {
                    if (!_elLowerProxChange)
                    {
                        _elLowerProxOld = controlRoom.RadioTelescopes[rtId].PLCDriver.proximitySensorData.Elevation_Lower_Prox_Sensor;
                        _elLowerProxChange = true;
                    }
                    controlRoom.RadioTelescopes[rtId].PLCDriver.proximitySensorData.Elevation_Lower_Prox_Sensor = true;
                }
                else if (_elEncoderDegrees > SimulationConstants.LIMIT_HIGH_EL_DEGREES)
                {
                    if (!_elUpperProxChange)
                    {
                        _elUpperProxOld = controlRoom.RadioTelescopes[rtId].PLCDriver.proximitySensorData.Elevation_Upper_Prox_Sensor;
                        _elUpperProxChange = true;
                    }
                    controlRoom.RadioTelescopes[rtId].PLCDriver.proximitySensorData.Elevation_Upper_Prox_Sensor = true;
                }
                else
                {
                    if (!_elUpperProxChange)
                    {
                        _elUpperProxOld = controlRoom.RadioTelescopes[rtId].PLCDriver.proximitySensorData.Elevation_Upper_Prox_Sensor;
                        _elUpperProxChange = true;
                    }
                    controlRoom.RadioTelescopes[rtId].PLCDriver.proximitySensorData.Elevation_Upper_Prox_Sensor = false;

                    if (!_elLowerProxChange)
                    {
                        _elLowerProxOld = controlRoom.RadioTelescopes[rtId].PLCDriver.proximitySensorData.Elevation_Lower_Prox_Sensor;
                        _elLowerProxChange = true;
                    }
                    controlRoom.RadioTelescopes[rtId].PLCDriver.proximitySensorData.Elevation_Lower_Prox_Sensor = false;
                }
            }
            // AKA Run Demo checkbox has not been selected
            else
            {
                controlRoom.RadioTelescopes[rtId].Micro_controler.setStableOrTesting(true);
                
                myEncoder.SetAzimuthAngle(0.0);
                myEncoder.SetElevationAngle(0.0);

                _azEncoderTicks = 0;
                _elEncoderTicks = 0;

                // If the value in the struct has been modified, then it restores it to the value held before the change
                // This will help to prevent the mismatch of values where a true limit switch value is displayed as a false
                // Limit Switch Logic
                if (_azCWLimitChange)
                    controlRoom.RadioTelescopes[rtId].PLCDriver.limitSwitchData.Azimuth_CW_Limit = _azCWLimitOld;

                if (_azCCWLimitChange)
                    controlRoom.RadioTelescopes[rtId].PLCDriver.limitSwitchData.Azimuth_CCW_Limit = _azCCWLimitOld;

                if (_elLowerLimitChange)
                    controlRoom.RadioTelescopes[rtId].PLCDriver.limitSwitchData.Elevation_Lower_Limit = _elLowerLimitOld;

                if (_elUpperLimitChange)
                    controlRoom.RadioTelescopes[rtId].PLCDriver.limitSwitchData.Elevation_Upper_Limit = _elUpperLimitOld;

                // Proximity Sensor Logic
                if (_azCWProxChange)
                    controlRoom.RadioTelescopes[rtId].PLCDriver.proximitySensorData.Azimuth_CW_Prox_Sensor = _azCWProxOld;

                if (_azCCWProxChange)
                    controlRoom.RadioTelescopes[rtId].PLCDriver.proximitySensorData.Azimuth_CCW_Prox_Sensor = _azCCWProxOld;

                if (_elLowerProxChange)
                    controlRoom.RadioTelescopes[rtId].PLCDriver.proximitySensorData.Elevation_Lower_Prox_Sensor = _elLowerProxOld;

                if (_elUpperProxChange)
                    controlRoom.RadioTelescopes[rtId].PLCDriver.proximitySensorData.Elevation_Upper_Prox_Sensor = _elUpperProxOld;
            }

            
            double ElMotTemp = controlRoom.RadioTelescopes[rtId].Micro_controler.tempData.elevationTemp;
            double AzMotTemp = controlRoom.RadioTelescopes[rtId].Micro_controler.tempData.azimuthTemp;
            float insideTemp = controlRoom.WeatherStation.GetInsideTemp();
            float outsideTemp = controlRoom.WeatherStation.GetOutsideTemp();
            /** Conversion from fahrenheit to celsius 
            if(celOrFar)
            {
               
            }**/
            //double ElMotTempFar = (ElMotTemp) * (9 / 5) + 32;
            //double AzMotTempFar = (AzMotTemp) * (9 / 5) + 32;
            //float insideTempFar = (insideTemp) * (9 / 5) + 32;
            //float outsideTempFar = (outsideTemp) * (9 / 5) + 32;
            /** Conversion from celsius to farenheit
             
           }**/
            double ElMotTempCel = (ElMotTemp - 32) * (5.0 / 9);
            double AzMotTempCel = (AzMotTemp - 32) * (5.0 / 9);
            double insideTempCel = (insideTemp - 32) * (5.0 / 9);
            double outsideTempCel = (outsideTemp - 32) * (5.0 / 9);

            if (farenheit == false)
            {
                outsideTempLabel.Text = Math.Round(insideTempCel, 2).ToString();
                insideTempLabel.Text = Math.Round(outsideTempCel, 2).ToString();
                fldElTemp.Text = Math.Round(ElMotTempCel, 2).ToString();
                fldAzTemp.Text = Math.Round(AzMotTempCel, 2).ToString();
            }
            else if (farenheit == true)
            {
                outsideTempLabel.Text = Math.Round(controlRoom.WeatherStation.GetOutsideTemp(), 2).ToString();
                insideTempLabel.Text = Math.Round(controlRoom.WeatherStation.GetInsideTemp(), 2).ToString();
                fldElTemp.Text = Math.Round(ElMotTemp, 2).ToString();
                fldAzTemp.Text = Math.Round(AzMotTemp, 2).ToString();
            }

            /** Temperature of motors **/
            Console.WriteLine("Azimuth temp: " + controlRoom.RadioTelescopes[rtId].Micro_controler.tempData.azimuthTemp.ToString());
            Console.WriteLine("Elevation temp: " + controlRoom.RadioTelescopes[rtId].Micro_controler.tempData.azimuthTemp.ToString());

            //fldElTemp.Text = controlRoom.RadioTelescopes[rtId].Micro_controler.tempData.elevationTemp.ToString();
            //fldAzTemp.Text = controlRoom.RadioTelescopes[rtId].Micro_controler.tempData.azimuthTemp.ToString();

            /** Encoder Position in both degrees and motor ticks **/
            lblAzEncoderDegrees.Text = Math.Round(_azEncoderDegrees, 3).ToString();
            lblAzEncoderTicks.Text = _azEncoderTicks.ToString();

            // lblElEncoderDegrees.Text = _elEncoderDegrees.ToString();
            lblElEncoderDegrees.Text =Math.Round(_elEncoderDegrees, 3).ToString();
            lblElEncoderTicks.Text = _elEncoderTicks.ToString();

            /** Proximity and Limit Switches **/
            lblAzLimStatus1.Text = controlRoom.RadioTelescopes[rtId].PLCDriver.limitSwitchData.Azimuth_CCW_Limit.ToString();
            lblAzLimStatus2.Text = controlRoom.RadioTelescopes[rtId].PLCDriver.limitSwitchData.Azimuth_CW_Limit.ToString();

            lblElLimStatus1.Text = controlRoom.RadioTelescopes[rtId].PLCDriver.limitSwitchData.Elevation_Lower_Limit.ToString();
            lblElLimStatus2.Text = controlRoom.RadioTelescopes[rtId].PLCDriver.limitSwitchData.Elevation_Upper_Limit.ToString();

            lblAzProxStatus1.Text = controlRoom.RadioTelescopes[rtId].PLCDriver.proximitySensorData.Azimuth_CCW_Prox_Sensor.ToString();
            lblAzProxStatus2.Text = controlRoom.RadioTelescopes[rtId].PLCDriver.proximitySensorData.Azimuth_CW_Prox_Sensor.ToString();
            lblAzProxStatus3.Text = "Not Used";

            lblEleProx1.Text = controlRoom.RadioTelescopes[rtId].PLCDriver.proximitySensorData.Elevation_Lower_Prox_Sensor.ToString();
            lblEleProx2.Text = controlRoom.RadioTelescopes[rtId].PLCDriver.proximitySensorData.Elevation_Upper_Prox_Sensor.ToString();

            ///*** Temperature Logic Start***/
            //if (elevationTemperature <= 79 && azimuthTemperature <= 79)
            //{
            //    warningLabel.Visible = false;
            //    lblShutdown.Visible = false;
            //    fanLabel.Visible = false;
            //    warningSent = false;
            //    shutdownSent = false;
            //}
            //else if (elevationTemperature > 79 && elevationTemperature < 100 || azimuthTemperature > 79 && azimuthTemperature < 100)
            //{
            //    if (warningSent == false)
            //    {
            //        warningLabel.Visible = true;
            //    }
            //    else
            //    {
            //        warningLabel.Visible = false;
            //    }

            //    lblShutdown.Visible = false;
            //    warningLabel.ForeColor = Color.Yellow;
            //    warningLabel.Text = "Warning Sent";

            //    warningSent = true;

            //    fanLabel.Visible = true;
            //    fanLabel.ForeColor = Color.Blue;
            //    fanLabel.Text = "Fans On";

            //}
            //else if (elevationTemperature >= 100 || azimuthTemperature >= 100)
            //{
            //    warningLabel.Visible = false;

            //    if (shutdownSent == false)
            //    {
            //        lblShutdown.Visible = true;
            //    }
            //    else
            //    {
            //        lblShutdown.Visible = false;
            //    }

            //    lblShutdown.ForeColor = Color.Red;
            //    lblShutdown.Text = "Shutdown Sent";

            //    shutdownSent = true;

            //    fanLabel.Visible = true;
            //    fanLabel.ForeColor = Color.Blue;
            //    fanLabel.Text = "Fans Stay On";


            //}
            //else
            //{
            //    warningLabel.Visible = false;
            //    warningLabel.ForeColor = Color.Black;
            //    warningLabel.Text = "";

            //    fanLabel.Visible = false;
            //    fanLabel.ForeColor = Color.Blue;
            //    fanLabel.Text = "Fans On";
            //}

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

            SetCurrentWeatherData();

            dataGridView1.Update();


            //if(MCU_Statui.Rows.Count > 5) {
            //    bool[] stuti = controlRoom.RadioTelescopes[rtId].PLCDriver.GET_MCU_Status( RadioTelescopeAxisEnum.AZIMUTH ).GetAwaiter().GetResult();
            //    foreach(MCUConstants.MCUStutusBits stutusBit in (MCUConstants.MCUStutusBits[])Enum.GetValues( typeof( MCUConstants.MCUStutusBits ) )) {
            //        string[] row = { stutusBit.ToString() , stuti[(int)stutusBit].ToString() };
            //        //MCU_Statui.Rows[(int)stutusBit][1] = stuti[(int)stutusBit].ToString();
            //        //string[] row = { ((PLC_modbus_server_register_mapping)reg).ToString() , Convert.ToString( val[reg + 1] ).PadLeft( 5 ) };//.Replace(" ", "0") };
            //        //PLC_regs.Rows.Add(row);
            //        MCU_Statui.Rows[(int)stutusBit].SetValues( row );
            //    }
            //} else {
            //    MCU_Statui.Rows.Clear();
            //    bool[] stuti = controlRoom.RadioTelescopes[rtId].PLCDriver.GET_MCU_Status( RadioTelescopeAxisEnum.AZIMUTH ).GetAwaiter().GetResult();
            //    foreach(MCUConstants.MCUStutusBits stutusBit in (MCUConstants.MCUStutusBits[])Enum.GetValues( typeof( MCUConstants.MCUStutusBits ) )) {
            //        string[] row = { stutusBit.ToString() , stuti[(int)stutusBit].ToString() };
            //        MCU_Statui.Rows.Add( row );
            //    }
            //    MCU_Statui.Update();
            //}

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

            //if(double.TryParse(txtTemperature.Text, out temperature))
            //{
            //    fldAzTemp.Text = temperature.ToString();
            //}
            //else
            //{
            //    MessageBox.Show("Invalid entry in Temperature field", "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            //}
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
           // double tempVal; //temperature value


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

        private void editDiagScriptsButton_Click(object sender, EventArgs e)
        {
            logger.Info("Edit Scripts Button Clicked");
            int caseSwitch = diagnosticScriptCombo.SelectedIndex;

            switch (caseSwitch)
            {
                case 0:
                    controlRoom.RadioTelescopeControllers[rtId].ExecuteRadioTelescopeControlledStop();
                    controlRoom.RadioTelescopes[rtId].PLCDriver.HitAzimuthLeftLimitSwitch();//Change left to CCW
                    //Hit Azimuth Counter-Clockwise Limit Switch (index 0 of control script combo)
                    break;
                case 1:
                    controlRoom.RadioTelescopeControllers[rtId].ExecuteRadioTelescopeControlledStop();
                    controlRoom.RadioTelescopes[rtId].PLCDriver.HitAzimuthRightLimitSwitch();
                    //Hit Azimuth Clockwise Limit Switch (index 1 of control script combo)
                    break;
                case 2:
                    controlRoom.RadioTelescopeControllers[rtId].ExecuteRadioTelescopeControlledStop();
                    controlRoom.RadioTelescopes[rtId].PLCDriver.HitElevationLowerLimitSwitch();
                    //Elevation Lower Limit Switch (index 2 of control script combo)
                    break;
                case 3:
                    controlRoom.RadioTelescopeControllers[rtId].ExecuteRadioTelescopeControlledStop();
                    controlRoom.RadioTelescopes[rtId].PLCDriver.HitElevationUpperLimitSwitch();
                    //Elevation Upper Limit Switch (index 3 of control script combo)
                    break;
                case 4:
                    controlRoom.RadioTelescopeControllers[rtId].ExecuteRadioTelescopeControlledStop();
                    controlRoom.RadioTelescopes[rtId].PLCDriver.Hit_CW_Hardstop();
                    //Hit Clockwise Hardstop (index 4 of control script combo)
                    break;
                case 5:
                    controlRoom.RadioTelescopeControllers[rtId].ExecuteRadioTelescopeControlledStop();
                    controlRoom.RadioTelescopes[rtId].PLCDriver.Hit_CCW_Hardstop();
                    //Hit Counter-Clockwise Hardstop (index 4 of control script combo)
                    break;
                default:

                    //Script cannot be run
                    break;
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

        private void splitContainer2_Panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void windSpeedLabel_Click(object sender, EventArgs e)
        {

        }

        private void windDirLabel_Click(object sender, EventArgs e)
        {

        }

        private void fldAzTemp_Click(object sender, EventArgs e)
        {

        }

        private void label25_Click(object sender, EventArgs e)
        {

        }

        private void fldElTemp_Click(object sender, EventArgs e)
        {

        }

        private void barometricPressureLabel_Click(object sender, EventArgs e)
        {

        }

        private void splitContainer1_Panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void MCU_Statui_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void lblElevationTemp_Click(object sender, EventArgs e)
        {

        }

        private void lblElProx1_Click(object sender, EventArgs e)
        {

        }

        private void lblEleProx1_Click(object sender, EventArgs e)
        {

        }

        private void lblAzProx2_Click(object sender, EventArgs e)
        {

        }

        private void lblAzProxStatus2_Click(object sender, EventArgs e)
        {

        }

        private void lblElLimit1_Click(object sender, EventArgs e)
        {

        }

        private void lblElLimStatus1_Click(object sender, EventArgs e)
        {

        }

        private void lblAzLimit2_Click(object sender, EventArgs e)
        {

        }

        private void fanLabel_Click(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void label24_Click(object sender, EventArgs e)
        {

        }

        private void txtTemperature_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {

        }

        private void startTimeTextBox_TextChanged(object sender, EventArgs e)
        {

        }

        private void button7_Click(object sender, EventArgs e)
        {
            string filename = "C:/Users/RadioTelescopeTWO/Desktop/RadioTelescope/RT-Control/YCP-RT-ControlRoom/ControlRoomApplication/ControlRoomApplication/Documentation/UIDoc.pdf";
            System.Diagnostics.Process.Start(filename);
        }

        private void ORAzimuthSens1_Click(object sender, EventArgs e)
        {
            logger.Info("Over Ride azimuth sensor 1 clicked");
            bool overRideAz1 = (ORAzimuthSens1.Text == "Over Ridden");
            if (!overRideAz1)
            {
                ORAzimuthSens1.Text = "Over Ridden";
                ORAzimuthSens1.BackColor = System.Drawing.Color.LimeGreen;
                
            }
            else if (overRideAz1)
            {
                ORAzimuthSens1.Text = "Over Ride";
                ORAzimuthSens1.BackColor = System.Drawing.Color.Red;

            }
        }

        private void ORAzimuthSens2_Click(object sender, EventArgs e)
        {
                logger.Info("Over Ride azimuth sensor 2 clicked");
                bool overRideAz2= (ORAzimuthSens2.Text == "Over Ridden");
                if (!overRideAz2)
                {
                    ORAzimuthSens2.Text = "Over Ridden";
                    ORAzimuthSens2.BackColor = System.Drawing.Color.LimeGreen;

                }
                else if (overRideAz2)
                {
                    ORAzimuthSens2.Text = "Over Ride";
                    ORAzimuthSens2.BackColor = System.Drawing.Color.Red;

                }
        }

        private void ElevationProximityOverideButton1_Click(object sender, EventArgs e)
        {
            logger.Info("Over Ride elevation Proximity Override 1 clicked");
            bool overRideEl1 = (ElevationProximityOveride1.Text == "Over Ridden");
            if (!overRideEl1)
            {
                ElevationProximityOveride1.Text = "Over Ridden";
                ElevationProximityOveride1.BackColor = System.Drawing.Color.LimeGreen;

            }
            else if (overRideEl1)
            {
                ElevationProximityOveride1.Text = "Over Ride";
                ElevationProximityOveride1.BackColor = System.Drawing.Color.Red;

            }
        }

        private void ElevationProximityOverideButton2_Click(object sender, EventArgs e)
        {
            logger.Info("Over Ride elevation Proximity Override 2 clicked");
            bool overRideEl2 = (ElevationProximityOveride2.Text == "Over Ridden");
            if (!overRideEl2)
            {
                ElevationProximityOveride2.Text = "Over Ridden";
                ElevationProximityOveride2.BackColor = System.Drawing.Color.LimeGreen;

            }
            else if (overRideEl2)
            {
                ElevationProximityOveride2.Text = "Over Ride";
                ElevationProximityOveride2.BackColor = System.Drawing.Color.Red;

            }
        }

        private void splitContainer1_Panel1_Paint_1(object sender, PaintEventArgs e)
        {

        }

        private void diagnosticScriptCombo_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (diagnosticScriptCombo.SelectedIndex >= 0)
            {
                runDiagScriptsButton.Enabled = true;
                runDiagScriptsButton.BackColor = System.Drawing.Color.LimeGreen;
            }
        }

        private void warningLabel_Click(object sender, EventArgs e)
        {

        }

        private void selectDemo_CheckedChanged(object sender, EventArgs e)
        {

        }
        /** Conversion from fahrenheit to celsius (Currently not being used) 
            if(celOrFar)
            {
                elevationTemperature = (elevationTemperature - 32) * (5.0 / 9);
                azimuthTemperature = (azimuthTemperature - 32) * (5.0 / 9);
            }**/
        private void celTempConvert_Click(object sender, EventArgs e)
        {

            

            if (farenheit == true)
            {
                farenheit = false;
                celTempConvert.BackColor = System.Drawing.Color.LimeGreen;
                farTempConvert.BackColor = System.Drawing.Color.DarkGray;
                
            }
        }

        private void farTempConvert_Click(object sender, EventArgs e)
        {
          
            

            if (farenheit == false)
            {
                farenheit = true;
                celTempConvert.BackColor = System.Drawing.Color.DarkGray;
                farTempConvert.BackColor = System.Drawing.Color.LimeGreen;
            
            }
        }

        private void WSOverride_Click(object sender, EventArgs e)
        {
            logger.Info("Over Ride Weather Station clicked");
            bool overRideWS = (WSOverride.Text == "Over Ridden");
            if (!overRideWS)
            {
                WSOverride.Text = "Over Ridden";
                WSOverride.BackColor = System.Drawing.Color.LimeGreen;

            }
            else if (overRideWS)
            {
                WSOverride.Text = "Over Ride";
                WSOverride.BackColor = System.Drawing.Color.Red;

            }
        }

        private void MGOverride_Click(object sender, EventArgs e)
        {
            logger.Info("Over Ride azimuth sensor 1 clicked");
            bool overRideWS = (WSOverride.Text == "Over Ridden");
            if (!overRideWS)
            {
                WSOverride.Text = "Over Ridden";
                WSOverride.BackColor = System.Drawing.Color.LimeGreen;

            }
            else if (overRideWS)
            {
                WSOverride.Text = "Over Ride";
                WSOverride.BackColor = System.Drawing.Color.Red;

            }
        }

        private void lblElEncoderDegrees_Click(object sender, EventArgs e)
        {

        }
    }
}


