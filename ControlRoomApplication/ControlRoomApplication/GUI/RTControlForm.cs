using ControlRoomApplication.Controllers;
using ControlRoomApplication.Database;
using ControlRoomApplication.Entities;
using ControlRoomApplication.GUI;
using System.ComponentModel;
//using ControlRoomApplication.GUI;
using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Forms;
using System.Threading.Tasks;

namespace ControlRoomApplication.Main
{
    public partial class FreeControlForm : Form
    {
        public Appointment CurrentAppointment { get; set; }
        public User ControlRoomUser { get; set; }
        public Coordinate TargetCoordinate { get; set; }
        public double Increment { get; set; }
        public CoordinateCalculationController CoordCalc { set; get; }
        public ControlRoom controlRoom { get; set; }
        private RadioTelescopeController rtController { get; set; }
        // private ControlRoomController MainControlRoomController { get; set; }
        private Thread ControlRoomThread { get; set; }
        public int rtId { get; set; }

        
        private static int current_rt_id;
        private static readonly log4net.ILog logger =
            log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public bool freeEditActive;
        public bool manualControlActive;
        public bool spectraEditActive;
        public SpectraCyberDCGainEnum DCGainInput;
        public SpectraCyberIntegrationTimeEnum IntTimeInput;
        public SpectraCyberBandwidthEnum BandwidthInput;

        public FreeControlForm(ControlRoom new_controlRoom, int new_rtId)
        {
            InitializeComponent();
            // Set ControlRoom
            controlRoom = new_controlRoom;
            // Set RT id
            rtId = new_rtId;
            // Make coordCalc
            rtController = controlRoom.RadioTelescopeControllers.Find(x => x.RadioTelescope.Id == rtId);
            CoordCalc = rtController.CoordinateController;
            // Set increment
            Increment = 1;
            UpdateIncrementButtons();

            ControlRoomUser = DatabaseOperations.GetControlRoomUser();

            // Add free control appt
            CurrentAppointment = new Appointment();
            CurrentAppointment.start_time = DateTime.UtcNow.AddSeconds(5);
            CurrentAppointment.end_time = DateTime.UtcNow.AddMinutes(15);
            CurrentAppointment._Status = AppointmentStatusEnum.REQUESTED;
            CurrentAppointment._Type = AppointmentTypeEnum.FREE_CONTROL;
            CurrentAppointment._Priority = AppointmentPriorityEnum.MANUAL;
            CurrentAppointment.SpectraCyberConfig = new SpectraCyberConfig(SpectraCyberModeTypeEnum.SPECTRAL);
            CurrentAppointment.CelestialBody = new CelestialBody();
            CurrentAppointment.CelestialBody.Coordinate = new Coordinate(0, 0);
            CurrentAppointment.Orientation = rtController.GetAbsoluteOrientation();
            CurrentAppointment.Telescope = controlRoom.RadioTelescopes.Find(x => x.Id == rtId);
            CurrentAppointment.User = ControlRoomUser;

            rtController.RadioTelescope.SpectraCyberController.Schedule.Mode = SpectraCyberScanScheduleMode.OFF;

            DatabaseOperations.AddAppointment(CurrentAppointment);

            //Calibrate Move
            CalibrateMove();
            runControlScriptButton.Enabled = false;       

            //Initialize Free control Box as disabled
            freeControlGroupbox.BackColor = System.Drawing.Color.DarkGray;
            PosDecButton.Enabled = false;
            NegDecButton.Enabled = false;
            PosRAButton.Enabled = false;
            NegRAButton.Enabled = false;
            oneForthButton.Enabled = false;
            oneForthButtonDec.Enabled = false;
            oneButton.Enabled = false;
            oneButtonDec.Enabled = false;
            fiveButton.Enabled = false;
            fiveButtonDec.Enabled = false;
            tenButton.Enabled = false;
            tenButtonDec.Enabled = false;

            //Initialize Manual control Box as disabled
            manualGroupBox.BackColor = System.Drawing.Color.DarkGray;
            plusElaButton.Enabled = false;
            plusJogButton.Enabled = false;
            subJogButton.Enabled = false;
            subElaButton.Enabled = false;
            ControledButtonRadio.Enabled = false;
            immediateRadioButton.Enabled = false;
            speedComboBox.Enabled = false;

            //Initialize Start and Stop Scan buttons as disabled
            spectraEditActive = true;
            startScanButton.BackColor = System.Drawing.Color.DarkGray;
            startScanButton.Enabled = false;
            stopScanButton.BackColor = System.Drawing.Color.DarkGray;
            stopScanButton.Enabled = false;

            this.FormClosing += FreeControlForm_Closing;

          //  var threads = controlRoom.RTControllerManagementThreads.Where<RadioTelescopeControllerManagementThread>(t => t.RTController == rtController).ToList<RadioTelescopeControllerManagementThread>();
         //   threads[0].ManagementThread.Abort();
           // controlRoom.RTControllerManagementThreads.Where<RadioControllerManagementThread>( t => t.R == rtController));

            logger.Info("Radio Telescope Control Form Initalized");
        }

        void FreeControlForm_Closing(object sender, CancelEventArgs e)
        {
            logger.Info("Radio Telescope Control Form Closing");
            CurrentAppointment._Status = AppointmentStatusEnum.COMPLETED;
            DatabaseOperations.UpdateAppointment(CurrentAppointment);

            var threads = controlRoom.RTControllerManagementThreads.Where<RadioTelescopeControllerManagementThread>(t => t.RTController == rtController).ToList<RadioTelescopeControllerManagementThread>();
            threads[0].EndAppointment();

            timer1.Enabled = false;
        }
       // logger.Info("Adding RadioTelescope Controller");
       //MainControlRoomController.AddRadioTelescopeController(ProgramRTControllerList[current_rt_id - 1]);

        private void PosDecButton_Click(object sender, EventArgs e)
        {
            logger.Info("Positive Declination Button Clicked");
            Coordinate new_coord = new Coordinate(TargetCoordinate.RightAscension, TargetCoordinate.Declination + Increment);
            Entities.Orientation test_orientation = CoordCalc.CoordinateToOrientation(new_coord, DateTime.UtcNow);
            if(test_orientation.Azimuth > 0 && test_orientation.Elevation > 0)
            {
                TargetCoordinate = new_coord;
                CoordMove();
            }
            else
            {
                errorLabel.Text = "Invalid Coordinate: orienation out of range";
            }
        }

        private void NegDecButton_Click(object sender, EventArgs e)
        {
            logger.Info("Negitive Declination Button Clicked");
            Coordinate new_coord = new Coordinate(TargetCoordinate.RightAscension, TargetCoordinate.Declination - Increment);
            Entities.Orientation test_orientation = CoordCalc.CoordinateToOrientation(new_coord, DateTime.UtcNow);
            if (test_orientation.Azimuth > 0 && test_orientation.Elevation > 0)
            {
                TargetCoordinate = new_coord;
                CoordMove();
            }
            else
            {
                errorLabel.Text = "Invalid Coordinate: orienation out of range";
            }
        }

        private void NegRAButton_Click(object sender, EventArgs e)
        {
            logger.Info("Negitive Right Ascension Button Clicked");
            Coordinate new_coord = new Coordinate(TargetCoordinate.RightAscension - Increment, TargetCoordinate.Declination);
            Entities.Orientation test_orientation = CoordCalc.CoordinateToOrientation(new_coord, DateTime.UtcNow);
            if (test_orientation.Azimuth > 0 && test_orientation.Elevation > 0)
            {
                TargetCoordinate = new_coord;
                CoordMove();
            }
            else
            {
                errorLabel.Text = "Invalid Coordinate: orienation out of range";
            }
        }

        private void PosRAButton_Click(object sender, EventArgs e)
        {
            logger.Info("Positive Right Ascension Button Clicked");
            Coordinate new_coord = new Coordinate(TargetCoordinate.RightAscension + Increment, TargetCoordinate.Declination);
            Entities.Orientation test_orientation = CoordCalc.CoordinateToOrientation(new_coord, DateTime.UtcNow);
            if (test_orientation.Azimuth >= 0 && test_orientation.Elevation >= 0)
            {
                TargetCoordinate = new_coord;
                CoordMove();
            }
            else
            {
                errorLabel.Text = "Invalid Coordinate: orienation out of range";
            }
        }

        private void CalibrateButton_Click(object sender, EventArgs e)
        {
            logger.Info("Calibrate Button Clicked");
            CalibrateMove();
        }

        public void CalibrateMove()
        {
            logger.Info("CalibrateMove ");
            TargetCoordinate = CoordCalc.OrientationToCoordinate(CurrentAppointment.Orientation, DateTime.UtcNow);
            UpdateText();
        }

        private void CoordMove()
        {
            logger.Info("CoordMove ");
            CurrentAppointment.Coordinates.Add(TargetCoordinate);
            DatabaseOperations.UpdateAppointment(CurrentAppointment);
            UpdateText();
        }

        private void UpdateText()
        {
            string RA = TargetCoordinate.RightAscension.ToString("0.##");
            string Dec = TargetCoordinate.Declination.ToString("0.##");
            logger.Info("UpdateText, Target Coordinate = RA:" + RA + ", Dec:" + Dec);
            SetTargetRAText(RA);
            SetTargetDecText(Dec);
            errorLabel.Text = "Free Control for Radio Telescope " + rtId.ToString();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            Entities.Orientation currentOrienation = rtController.GetCurrentOrientation();
            SetAZText(String.Format("{0:N2}",currentOrienation.Azimuth));
            SetELText(String.Format("{0:N2}", currentOrienation.Elevation));
            Coordinate ConvertedPosition = CoordCalc.OrientationToCoordinate(currentOrienation, DateTime.UtcNow);
            SetActualRAText(ConvertedPosition.RightAscension.ToString("0.##"));
            SetActualDecText(ConvertedPosition.Declination.ToString("0.##"));
        }

        delegate void SetTargetRATextCallback(string text);
        private void SetTargetRAText(string text)
        {
            // InvokeRequired required compares the thread ID of the
            // calling thread to the thread ID of the creating thread.
            // If these threads are different, it returns true.
            if (TargetRATextBox.InvokeRequired)
            {
                SetTargetRATextCallback d = new SetTargetRATextCallback(SetTargetRAText);
                Invoke(d, new object[] { text });
            }
            else
            {
                TargetRATextBox.Text = text;
            }
        }

        delegate void SetTargetDecTextCallback(string text);
        private void SetTargetDecText(string text)
        {
            // InvokeRequired required compares the thread ID of the
            // calling thread to the thread ID of the creating thread.
            // If these threads are different, it returns true.
            if (TargetDecTextBox.InvokeRequired)
            {
                SetTargetDecTextCallback d = new SetTargetDecTextCallback(SetTargetDecText);
                Invoke(d, new object[] { text });
            }
            else
            {
                TargetDecTextBox.Text = text;
            }
        }

        delegate void SetActualRATextCallback(string text);
        private void SetActualRAText(string text)
        {
            // InvokeRequired required compares the thread ID of the
            // calling thread to the thread ID of the creating thread.
            // If these threads are different, it returns true.
            if (ActualRATextBox.InvokeRequired)
            {
                SetActualRATextCallback d = new SetActualRATextCallback(SetActualRAText);
                try
                {
                    Invoke(d, new object[] { text });

                }
                catch { }
            }
            else
            {
                ActualRATextBox.Text = text;
            }
        }

        delegate void SetActualDecTextCallback(string text);
        private void SetActualDecText(string text)
        {
            // InvokeRequired required compares the thread ID of the
            // calling thread to the thread ID of the creating thread.
            // If these threads are different, it returns true.
            if (ActualDecTextBox.InvokeRequired)
            {
                SetActualDecTextCallback d = new SetActualDecTextCallback(SetActualDecText);
                try
                {
                    Invoke(d, new object[] { text });

                }
                catch { }
            }
            else
            {
                ActualDecTextBox.Text = text;
            }
        }


        delegate void SetAZTextCallback(string text);
        private void SetAZText(string text) {
            // InvokeRequired required compares the thread ID of the
            // calling thread to the thread ID of the creating thread.
            // If these threads are different, it returns true.
            if (label4.InvokeRequired) {
                SetAZTextCallback d = new SetAZTextCallback(SetAZText);
                try {
                    Invoke(d, new object[] { text });

                }
                catch { }
            } else {
                label4.Text = text;
            }
        }


        delegate void SetELTextCallback(string text);
        private void SetELText(string text) {
            // InvokeRequired required compares the thread ID of the
            // calling thread to the thread ID of the creating thread.
            // If these threads are different, it returns true.
            if (label5.InvokeRequired) {
                SetELTextCallback d = new SetELTextCallback(SetELText);
                try {
                    Invoke(d, new object[] { text });

                }
                catch { }
            } else {
                label5.Text = text;
            }
        }

        private void oneForthButton_Click(object sender, EventArgs e)
        {
            logger.Info("Increment = 0.25 Button Clicked");
            Increment = 0.25;
            UpdateIncrementButtons();
        }

        private void oneButton_Click(object sender, EventArgs e)
        {
            logger.Info("Increment = 1 Button Clicked");
            Increment = 1;
            UpdateIncrementButtons();
        }

        private void fiveButton_Click(object sender, EventArgs e)
        {
            logger.Info("Increment = 5 Button Clicked");
            Increment = 5;
            UpdateIncrementButtons();
        }

        private void tenButton_Click(object sender, EventArgs e)
        {
            logger.Info("Increment = 10 Button Clicked");
            Increment = 10;
            UpdateIncrementButtons();
        }

        private void UpdateIncrementButtons()
        {
            //oneForthButton.BackColor = System.Drawing.Color.LightGray;
            //oneButton.BackColor = System.Drawing.Color.LightGray;
            //fiveButton.BackColor = System.Drawing.Color.LightGray;
            //tenButton.BackColor = System.Drawing.Color.LightGray;

            switch (Increment)
            {
                case 0.25:
                    oneForthButton.BackColor = System.Drawing.Color.DarkGray;
                    break;
                case 1:
                    oneButton.BackColor = System.Drawing.Color.DarkGray;
                    break;
                case 5:
                    fiveButton.BackColor = System.Drawing.Color.DarkGray;
                    break;
                case 10:
                    tenButton.BackColor = System.Drawing.Color.DarkGray;
                    break;
                default:
                    throw new ArgumentException("Invalid Increment");
            }
        }

        private void editButton_Click(object sender, EventArgs e)
        {
            logger.Info("Edit Button Clicked");
            bool save_state = (editButton.Text == "Save Position" );
            freeEditActive = !freeEditActive;
            if (save_state)
            {
                editButton.Text = "Edit Position";
                editButton.BackColor = System.Drawing.Color.Red;
                freeControlGroupbox.BackColor = System.Drawing.Color.DarkGray;
                manualControlButton.BackColor = System.Drawing.Color.Red;
                decIncGroupbox.BackColor = System.Drawing.Color.DarkGray;
                RAIncGroupbox.BackColor = System.Drawing.Color.DarkGray;
                double newRA;
                double newDec;
                double.TryParse(TargetRATextBox.Text, out newRA);
                double.TryParse(TargetDecTextBox.Text, out newDec);
                Coordinate new_coord = new Coordinate(newRA, newDec);
                Entities.Orientation test_orientation = CoordCalc.CoordinateToOrientation(new_coord, DateTime.UtcNow);
                if (test_orientation.Azimuth >= 0 && test_orientation.Elevation >= 0)
                {
                    TargetCoordinate = new_coord;
                    CoordMove();
                }
                else
                {
                    errorLabel.Text = "Invalid Coordinate: orienation out of range";
                }
            }
            else
            {
                editButton.Text = "Save Position";
                manualControlButton.BackColor = System.Drawing.Color.DarkGray;
                editButton.BackColor = System.Drawing.Color.LimeGreen;
                freeControlGroupbox.BackColor = System.Drawing.Color.Gainsboro;
                decIncGroupbox.BackColor = System.Drawing.Color.Gray;
                RAIncGroupbox.BackColor = System.Drawing.Color.Gray;
            }

            PosDecButton.Enabled = !save_state;
            NegDecButton.Enabled = !save_state;
            PosRAButton.Enabled = !save_state;
            NegRAButton.Enabled = !save_state;
            oneForthButton.Enabled = !save_state;
            oneForthButtonDec.Enabled = !save_state;
            oneButton.Enabled = !save_state;
            oneButtonDec.Enabled = !save_state;
            fiveButton.Enabled = !save_state;
            fiveButtonDec.Enabled = !save_state;
            tenButton.Enabled = !save_state;
            tenButtonDec.Enabled = !save_state;
            TargetRATextBox.ReadOnly = save_state;
            TargetDecTextBox.ReadOnly = save_state;

            manualControlButton.Enabled = save_state;
        }
        //
        private void manualControlButton_Click(object sender, EventArgs e)
        {
            logger.Info("Activate Manual Control Clicked");
            bool manual_save_state = (manualControlButton.Text == "Activate Manual Control");
            
            if (!manual_save_state)
            {
                manualControlButton.Text = "Activate Manual Control";
                manualControlButton.BackColor = System.Drawing.Color.Red;
                manualGroupBox.BackColor = System.Drawing.Color.DarkGray;
                editButton.BackColor = System.Drawing.Color.Red;
            }
            else if(manual_save_state)
            {
                manualControlButton.Text = "Deactivate Manual Control";
                manualControlButton.BackColor = System.Drawing.Color.LimeGreen;
                manualGroupBox.BackColor = System.Drawing.Color.Gainsboro;
                editButton.BackColor = System.Drawing.Color.DarkGray;

            }
            plusElaButton.Enabled = manual_save_state;
            plusJogButton.Enabled = manual_save_state;
            subJogButton.Enabled = manual_save_state;
            subElaButton.Enabled = manual_save_state;
            ControledButtonRadio.Enabled = manual_save_state;
            immediateRadioButton.Enabled = manual_save_state;
            speedComboBox.Enabled = manual_save_state;

            editButton.Enabled = !manual_save_state;
        }

        private void speedComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            {
                if (speedComboBox.Text == "2 RPM")
                {
                    logger.Info("Speed set to 2 RPM");
                   // speed = 333333;
                }
                 else if(speedComboBox.Text == "0.1 RPM")
                {
                    logger.Info("Speed set to 0.1 RPM");
                   // speed = 16667;
                }

                else
                {
                    logger.Info("Invalid Speed Selected");
                    throw new Exception();
                }
            }
        }
        //Run Script Button Functionality
        //Case Depends on which script is currently selected 
        private void runControlScript_Click(object sender, EventArgs e)
        {
            logger.Info("Run Script Button Clicked");
            int caseSwitch = controlScriptsCombo.SelectedIndex;

            RadioTelescope tele = rtController.RadioTelescope;

            Thread thread =new Thread(() => { } );

            switch (caseSwitch)
            {
                case 0:
                    thread = new Thread(() =>
                    {
                        rtController.ExecuteRadioTelescopeControlledStop();
                        tele.PLCDriver.Stow().GetAwaiter();
                    });
                    //Stow Script selected (index 0 of control script combo)
                    break;
                case 1:
                    thread = new Thread(() =>
                    {
                        rtController.ExecuteRadioTelescopeControlledStop();
                        tele.PLCDriver.FullElevationMove().GetAwaiter();
                    });
                    //Full Elevation selected (index 1 of control script combo)
                    break;
                case 2:
                    thread = new Thread(() =>
                    {
                        rtController.ExecuteRadioTelescopeControlledStop();
                        tele.PLCDriver.Full_360_CW_Rotation().GetAwaiter();
                    });
                    //Full 360 CW selected (index 2 of control script combo)
                    break;
                case 3:
                    thread = new Thread(() =>
                    {
                        rtController.ExecuteRadioTelescopeControlledStop();
                        tele.PLCDriver.Full_360_CCW_Rotation().GetAwaiter();
                    });
                    //Full 360 CCW  selected (index 3 of control script combo)
                    break;
                case 4:
                    thread = new Thread(() =>
                    {
                        rtController.ExecuteRadioTelescopeControlledStop();
                        tele.PLCDriver.Thermal_Calibrate().GetAwaiter();
                    });
                    //Thermal Calibration selected (index 4 of control script combo)
                    break;
                case 5:
                    thread = new Thread(() =>
                    {
                        rtController.ExecuteRadioTelescopeControlledStop();
                        tele.PLCDriver.SnowDump().GetAwaiter();
                    });
                    //Snow Dump selected (index 5 of control script combo)
                    break;
                case 6:
                    thread = new Thread(() =>
                    {
                        rtController.ExecuteRadioTelescopeControlledStop();
                        tele.PLCDriver.RecoverFromLimitSwitch().GetAwaiter();
                    });
                    //Recover from Limit Switch (index 6 of control script combo)
                    break;
                case 7:
                    thread = new Thread(() =>
                    {
                        rtController.ExecuteRadioTelescopeControlledStop();
                        tele.PLCDriver.Recover_CW_Hardstop().GetAwaiter();
                    });
                    //Recover from Clockwise Hardstop (index 7 of control script combo)
                    break;
                case 8:
                    thread = new Thread(() =>
                    {
                        rtController.ExecuteRadioTelescopeControlledStop();
                        tele.PLCDriver.Recover_CCW_Hardstop().GetAwaiter();
                    });
                    //Recover from Counter-Clockwise Hardstop (index 8 of control script combo)
                    break;
                case 9:
                    thread = new Thread(() =>
                    {
                        tele.PLCDriver.Home();
                    });
                    //Recover from Counter-Clockwise Hardstop (index 9 of control script combo)
                    break;
                default:

                    //Script cannot be run
                    break;
            }
            try {
                thread.Start();
            } catch {

            }
        }

        //Control Script combo box enables run button when a script has been selected
        private void controlScriptsCombo_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (controlScriptsCombo.SelectedIndex >= 0)
            {
                runControlScriptButton.Enabled = true;
                runControlScriptButton.BackColor = System.Drawing.Color.LimeGreen;
            }

        }

        private void subJogButton_Down( object sender , MouseEventArgs e ) {
            double speed = Convert.ToDouble( speedComboBox.Text );
            logger.Info( "Jog PosButton MouseDown" );
            // UpdateText("Moving at " + comboBox1.Text);

            // Start CW Jog
            rtController.StartRadioTelescopeAzimuthJog( speed , false );
        }

        private void subJogButton_Up( object sender , MouseEventArgs e ) {
            logger.Info( "Jog PosButton MouseUp" );
            // UpdateText("Manual Control for Radio Telescope " + rt_controller.RadioTelescope.Id.ToString());

            //Stop Move
            ExecuteCorrectStop();
        }

        private void plusJogButton_Down( object sender , MouseEventArgs e ) {
            double speed = Convert.ToDouble( speedComboBox.Text );
            logger.Info( "Jog PosButton MouseDown" );
            // UpdateText("Moving at " + comboBox1.Text);

            // Start CW Jog
            rtController.StartRadioTelescopeAzimuthJog( speed , true );
        }

        private void plusJogButton_UP( object sender , MouseEventArgs e ) {
            logger.Info( "Jog PosButton MouseUp" );
            // UpdateText("Manual Control for Radio Telescope " + rt_controller.RadioTelescope.Id.ToString());

            //  Stop Move
            ExecuteCorrectStop();
        }

        private void ExecuteCorrectStop()
        {
            if (ControledButtonRadio.Checked)
            {
                logger.Info("Executed Controlled Stop");
                rtController.ExecuteRadioTelescopeStopJog();
            }
            else if (immediateRadioButton.Checked)
            {
                logger.Info("Executed Immediate Stop");
                rtController.ExecuteRadioTelescopeImmediateStop();
            }
            else
            {
                logger.Info("Invalid Stop Selected");
                throw new Exception();
            }
        }

        //This Button executes a system call that opens up the user interface documentation as a PDF
        private void helpButton_click(object sender, EventArgs e)
        {
            string filename = Directory.GetCurrentDirectory() + "\\" + "UIDoc.pdf";
            if (File.Exists(filename))
                System.Diagnostics.Process.Start(filename);
        }

        private void groupBox2_Enter(object sender, EventArgs e)
        {

        }

        private void ActualPositionLabel_Click(object sender, EventArgs e)
        {

        }

        private void TargetPositionLabel_Click(object sender, EventArgs e)
        {

        }

        private void RAIncGroupbox_Enter(object sender, EventArgs e)
        {

        }

        private void manualGroupBox_Enter(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void plusElaButton_Down(object sender, MouseEventArgs e ){
            double speed = Convert.ToDouble( speedComboBox.Text);
            logger.Info("Jog PosButton MouseDown");
            // UpdateText("Moving at " + comboBox1.Text);

            // Start CW Jog
            rtController.StartRadioTelescopeElevationJog(speed, true);
        }

        private void plusElaButton_Up( object sender , MouseEventArgs e ) {
            logger.Info( "Jog PosButton MouseUp" );
            // UpdateText("Manual Control for Radio Telescope " + rt_controller.RadioTelescope.Id.ToString());

            //  Stop Move
            ExecuteCorrectStop();
        }

        private void subElaButton_Down(object sender, MouseEventArgs e ){
            double speed = Convert.ToDouble( speedComboBox.Text);
            logger.Info("Jog PosButton MouseDown");
            //UpdateText("Moving at " + speedComboBox.Text);

            // Start CW Jog
            rtController.StartRadioTelescopeElevationJog( speed, false);
        }

        private void subElaButton_Up( object sender , MouseEventArgs e ) {
            logger.Info( "Jog PosButton MouseUp" );
            // UpdateText("Manual Control for Radio Telescope " + rt_controller.RadioTelescope.Id.ToString());

            //  Stop Move
            ExecuteCorrectStop();
        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void groupBox4_Enter(object sender, EventArgs e)
        {

        }

        private void finalizeSettings_Click(object sender, EventArgs e)
        {
            logger.Info("[SpectraCyberController] Finalize settings button has been clicked");

            if (spectraEditActive)
            {
                scanTypeComboBox.BackColor = System.Drawing.Color.DarkGray;
                scanTypeComboBox.Enabled = false;
                integrationStepCombo.BackColor = System.Drawing.Color.DarkGray;
                integrationStepCombo.Enabled = false;
                offsetVoltage.BackColor = System.Drawing.Color.DarkGray;
                offsetVoltage.Enabled = false;
                frequency.BackColor = System.Drawing.Color.DarkGray;
                frequency.Enabled = false;
                DCGain.BackColor = System.Drawing.Color.DarkGray;
                DCGain.Enabled = false;
                IFGainVal.BackColor = System.Drawing.Color.DarkGray;
                IFGainVal.Enabled = false;

                startScanButton.BackColor = System.Drawing.Color.LimeGreen;
                startScanButton.Enabled = true;

                spectraEditActive = false;

                int caseSwitch = DCGain.SelectedIndex;

                switch (caseSwitch)
                {
                    case 0:
                        DCGainInput = SpectraCyberDCGainEnum.X1;
                        break;
                    case 1:
                        DCGainInput = SpectraCyberDCGainEnum.X5;
                        break;
                    case 2:
                        DCGainInput = SpectraCyberDCGainEnum.X10;
                        break;
                    case 3:
                        DCGainInput = SpectraCyberDCGainEnum.X20;
                        break;
                    case 4:
                        DCGainInput = SpectraCyberDCGainEnum.X50;
                        break;
                    case 5:
                        DCGainInput = SpectraCyberDCGainEnum.X60;
                        break;
                }

                caseSwitch = integrationStepCombo.SelectedIndex;

                switch (caseSwitch)
                {
                    case 0:
                        IntTimeInput = SpectraCyberIntegrationTimeEnum.SHORT_TIME_SPAN;
                        break;
                    case 1:
                        IntTimeInput = SpectraCyberIntegrationTimeEnum.MID_TIME_SPAN;
                        break;
                    case 2:
                        IntTimeInput = SpectraCyberIntegrationTimeEnum.LONG_TIME_SPAN;
                        break;
                }

            }
            else
            {
                scanTypeComboBox.BackColor = System.Drawing.Color.White;
                scanTypeComboBox.Enabled = true;
                integrationStepCombo.BackColor = System.Drawing.Color.White;
                integrationStepCombo.Enabled = true;
                offsetVoltage.BackColor = System.Drawing.Color.White;
                offsetVoltage.Enabled = true;
                frequency.BackColor = System.Drawing.Color.White;
                frequency.Enabled = true;
                DCGain.BackColor = System.Drawing.Color.White;
                DCGain.Enabled = true;
                IFGainVal.BackColor = System.Drawing.Color.White;
                IFGainVal.Enabled = true;

                startScanButton.BackColor = System.Drawing.Color.DarkGray;
                startScanButton.Enabled = false;

                spectraEditActive = true;
            }
        }

        private void startScan_Click(object sender, EventArgs e)
        {
            logger.Info("[SpectraCyberController] Start Scan button has been clicked");
            int caseSwitch = scanTypeComboBox.SelectedIndex;

            switch (caseSwitch)
            {
                case 0:
                    rtController.RadioTelescope.SpectraCyberController.SetSpectraCyberModeType(SpectraCyberModeTypeEnum.CONTINUUM);
                    rtController.RadioTelescope.SpectraCyberController.SetFrequency(Convert.ToDouble(frequency.Text));
                    rtController.RadioTelescope.SpectraCyberController.SetContinuumIntegrationTime(IntTimeInput);
                    rtController.RadioTelescope.SpectraCyberController.SetContinuumOffsetVoltage(Convert.ToDouble(offsetVoltage.Text));
                    rtController.RadioTelescope.SpectraCyberController.SetContGain(DCGainInput);
                    rtController.RadioTelescope.SpectraCyberController.SetSpectraCyberIFGain(Convert.ToDouble(IFGainVal.Text));
                    break;
                case 1:
                    rtController.RadioTelescope.SpectraCyberController.SetSpectraCyberModeType(SpectraCyberModeTypeEnum.SPECTRAL);
                    rtController.RadioTelescope.SpectraCyberController.SetFrequency(Convert.ToDouble(frequency.Text));
                    rtController.RadioTelescope.SpectraCyberController.SetSpectralIntegrationTime(IntTimeInput);
                    rtController.RadioTelescope.SpectraCyberController.SetSpectralOffsetVoltage(Convert.ToDouble(offsetVoltage.Text));
                    rtController.RadioTelescope.SpectraCyberController.SetSpecGain(DCGainInput);
                    rtController.RadioTelescope.SpectraCyberController.SetSpectraCyberIFGain(Convert.ToDouble(IFGainVal.Text));
                    break;
            }
       
            startScanButton.Enabled = false;
            startScanButton.BackColor = System.Drawing.Color.DarkGray;

            stopScanButton.Enabled = true;
            stopScanButton.BackColor = System.Drawing.Color.Red;

             rtController.RadioTelescope.SpectraCyberController.StartScan(CurrentAppointment);
          //  controlRoom.RTControllerManagementThreads.Find(t => t.RTController.RadioTelescope.Id == rtId).StartReadingData(CurrentAppointment);
            logger.Info("[SpectraCyberController] Scan has started");
        }

        private void stopScan_Click(object sender, EventArgs e)
        {
            if (rtController.RadioTelescope.SpectraCyberController.Schedule.Mode == SpectraCyberScanScheduleMode.OFF ||
                rtController.RadioTelescope.SpectraCyberController.Schedule.Mode == SpectraCyberScanScheduleMode.UNKNOWN)
                logger.Info("[SpectraCyberController] There is no scan to stop");
            else
            {
                rtController.RadioTelescope.SpectraCyberController.StopScan();
                logger.Info("[SpectraCyberController] Scan has stopped");
            }

            startScanButton.Enabled = true;
            startScanButton.BackColor = System.Drawing.Color.LimeGreen;

            stopScanButton.Enabled = false;
            stopScanButton.BackColor = System.Drawing.Color.DarkGray;

        }


        private void label11_Click(object sender, EventArgs e)
        {

        }

        private void label9_Click(object sender, EventArgs e)
        {

        }

        private void label12_Click(object sender, EventArgs e)
        {

        }

        private void FreeControlForm_Load(object sender, EventArgs e)
        {

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}