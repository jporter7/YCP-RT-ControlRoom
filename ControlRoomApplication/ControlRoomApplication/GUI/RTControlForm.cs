using ControlRoomApplication.Controllers;
using ControlRoomApplication.Database;
using ControlRoomApplication.Entities;
using ControlRoomApplication.GUI;
//using ControlRoomApplication.GUI;
using System;
using System.Threading;
using System.Windows.Forms;

namespace ControlRoomApplication.Main
{
    public partial class FreeControlForm : Form
    {
        public Appointment CurrentAppointment { get; set; }
        public Coordinate TargetCoordinate { get; set; }
        public double Increment { get; set; }
        public CoordinateCalculationController CoordCalc { set; get; }
        public ControlRoom controlRoom { get; set; }
        // private ControlRoomController MainControlRoomController { get; set; }
        private Thread ControlRoomThread { get; set; }
        public int rtId { get; set; }

        
        private static int current_rt_id;
        private static readonly log4net.ILog logger =
            log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public bool freeEditActive;
        public bool manualControlActive;


        public FreeControlForm(ControlRoom new_controlRoom, int new_rtId)
        {
            InitializeComponent();
            // Set ControlRoom
            controlRoom = new_controlRoom;
            // Set RT id
            rtId = new_rtId;
            // Make coordCalc
            CoordCalc = controlRoom.RadioTelescopeControllers[rtId - 1].CoordinateController;
            // Set increment
            Increment = 1;
            UpdateIncrementButtons();
            // Add free control appt
            CurrentAppointment = new Appointment();
            CurrentAppointment.start_time = DateTime.UtcNow.AddSeconds(5);
            CurrentAppointment.end_time = DateTime.UtcNow.AddMinutes(15);
            CurrentAppointment._Status = AppointmentStatusEnum.REQUESTED;
            CurrentAppointment._Type = AppointmentTypeEnum.FREE_CONTROL;
            CurrentAppointment._Priority = AppointmentPriorityEnum.MANUAL;
            CurrentAppointment.SpectraCyberConfig = new SpectraCyberConfig(SpectraCyberModeTypeEnum.CONTINUUM);
            CurrentAppointment.telescope_id = rtId;
            CurrentAppointment.user_id = 1;
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

            logger.Info("Radio Telescope Control Form Initalized");
        }

        private void FreeControlForm_FormClosing(Object sender, FormClosingEventArgs e)
        {
            logger.Info("Radio Telescope Control Form Closing");
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
            CurrentAppointment = DatabaseOperations.GetUpdatedAppointment(CurrentAppointment.Id);
            CurrentAppointment.Orientation = new Entities.Orientation(0, 90);
            DatabaseOperations.UpdateAppointment(CurrentAppointment);
            TargetCoordinate = CoordCalc.OrientationToCoordinate(CurrentAppointment.Orientation, DateTime.UtcNow);
            UpdateText();
        }

        private void CoordMove()
        {
            logger.Info("CoordMove ");
            CurrentAppointment = DatabaseOperations.GetUpdatedAppointment(CurrentAppointment.Id);
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
            Entities.Orientation currentOrienation = controlRoom.RadioTelescopeControllers[rtId - 1].GetCurrentOrientation();
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

            switch (caseSwitch)
            {
                case 0:
                    controlRoom.RadioTelescopeControllers[rtId].ExecuteRadioTelescopeControlledStop();
                    controlRoom.RadioTelescopes[rtId].PLCDriver.Stow();
                    //Stow Script selected (index 0 of control script combo)
                    break;
                case 1:
                    controlRoom.RadioTelescopeControllers[rtId].ExecuteRadioTelescopeControlledStop();
                    controlRoom.RadioTelescopes[rtId].PLCDriver.FullElevationMove();
                    //Full Elevation selected (index 1 of control script combo)
                    break;
                case 2:
                    controlRoom.RadioTelescopeControllers[rtId].ExecuteRadioTelescopeControlledStop();
                    controlRoom.RadioTelescopes[rtId].PLCDriver.Full_360_CW_Rotation();
                    //Full 360 CW selected (index 2 of control script combo)
                    break;
                case 3:
                    controlRoom.RadioTelescopeControllers[rtId].ExecuteRadioTelescopeControlledStop();
                    controlRoom.RadioTelescopes[rtId].PLCDriver.Full_360_CCW_Rotation();
                    //Full 360 CCW  selected (index 3 of control script combo)
                    break;
                case 4:
                    controlRoom.RadioTelescopeControllers[rtId].ExecuteRadioTelescopeControlledStop();
                    controlRoom.RadioTelescopes[rtId].PLCDriver.Thermal_Calibrate();
                    //Thermal Calibration selected (index 4 of control script combo)
                    break;
                case 5:
                    controlRoom.RadioTelescopeControllers[rtId].ExecuteRadioTelescopeControlledStop();
                    controlRoom.RadioTelescopes[rtId].PLCDriver.SnowDump();
                    //Snow Dump selected (index 5 of control script combo)
                    break;
                case 6:
                    controlRoom.RadioTelescopeControllers[rtId].ExecuteRadioTelescopeControlledStop();
                    controlRoom.RadioTelescopes[rtId].PLCDriver.RecoverFromLimitSwitch();
                    //Recover from Limit Switch (index 6 of control script combo)
                    break;
                case 7:
                    controlRoom.RadioTelescopeControllers[rtId].ExecuteRadioTelescopeControlledStop();
                    controlRoom.RadioTelescopes[rtId].PLCDriver.Recover_CW_Hardstop();
                    //Recover from Clockwise Hardstop (index 7 of control script combo)
                    break;
                case 8:
                    controlRoom.RadioTelescopeControllers[rtId].ExecuteRadioTelescopeControlledStop();
                    controlRoom.RadioTelescopes[rtId].PLCDriver.Recover_CCW_Hardstop();
                    //Recover from Counter-Clockwise Hardstop (index 8 of control script combo)
                    break;
                default:

                    //Script cannot be run
                    break;
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
            controlRoom.RadioTelescopeControllers[rtId - 1].StartRadioTelescopeAzimuthJog( speed , false );
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
            controlRoom.RadioTelescopeControllers[rtId - 1].StartRadioTelescopeAzimuthJog( speed , true );
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
                controlRoom.RadioTelescopeControllers[rtId - 1].ExecuteRadioTelescopeControlledStop();
            }
            else if (immediateRadioButton.Checked)
            {
                logger.Info("Executed Immediate Stop");
                controlRoom.RadioTelescopeControllers[rtId - 1].ExecuteRadioTelescopeImmediateStop();
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
            string filename = "C:/Users/RadioTelescopeTWO/Desktop/RadioTelescope/RT-Control/YCP-RT-ControlRoom/ControlRoomApplication/ControlRoomApplication/Documentation/UIDoc.pdf";
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

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void plusElaButton_Down(object sender, MouseEventArgs e ){
            double speed = Convert.ToDouble( speedComboBox.Text);
            logger.Info("Jog PosButton MouseDown");
            // UpdateText("Moving at " + comboBox1.Text);

            // Start CW Jog
            controlRoom.RadioTelescopeControllers[rtId - 1].StartRadioTelescopeElevationJog(speed, true);
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
            controlRoom.RadioTelescopeControllers[rtId - 1].StartRadioTelescopeElevationJog( speed, false);
        }

        private void subElaButton_Up( object sender , MouseEventArgs e ) {
            logger.Info( "Jog PosButton MouseUp" );
            // UpdateText("Manual Control for Radio Telescope " + rt_controller.RadioTelescope.Id.ToString());

            //  Stop Move
            ExecuteCorrectStop();
        }

    }
}







//--------------------------------------------------------------------------------------------------------------------

//- - - - - - - -Below this line is commented out code from the origional Radio Telescope Manual Form - - - - - - - - 

//--------------------------------------------------------------------------------------------------------------------


//  private void PosButton_MouseDown(object sender, MouseEventArgs e)
////  {
//      logger.Info("Jog PosButton MouseDown");
//     // UpdateText("Moving at " + comboBox1.Text);

//      // Start CW Jog
//      rt_controller.StartRadioTelescopeAzimuthJog(speed, true);
// // }

//  private void PosButton_MouseUp(object sender, MouseEventArgs e)
//{
//  logger.Info("Jog PosButton MouseUp");
//  UpdateText("Manual Control for Radio Telescope " + rt_controller.RadioTelescope.Id.ToString());

// Stop Move
//ExecuteCorrectStop();
//   }

// private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
// {
// if(comboBox1.Text == "2 RPM")
// {
//     logger.Info("Speed set to 2 RPM");
//     speed = 333333;
// }
//// else if(comboBox1.Text == "0.1 RPM")
// {
//     logger.Info("Speed set to 0.1 RPM");
//     speed = 16667;
// }

//else
//{
//    logger.Info("Invalid Speed Selected");
//    throw new Exception();
//}
///

//private void ExecuteCorrectStop()
//{
//   // //if (ControledButtonRadio.Checked)
//   // {
//   //     logger.Info("Executed Controlled Stop");
//   //     rt_controller.ExecuteRadioTelescopeControlledStop();
//   // }
//   //// else if (radioButton2.Checked)
//   // {
//   //     logger.Info("Executed Immediate Stop");
//   //     rt_controller.ExecuteRadioTelescopeImmediateStop();
//   // }
//    //else
//    //{
//    //    logger.Info("Invalid Stop Selected");
//    //    throw new Exception();
//    //}
//}

//private void button1_Click(object sender, EventArgs e)
//{//
//  //  logger.Info("Move Relative Button Clicked");
//    //int pos = (int)numericUpDown1.Value * (int)((166 + (2.0 / 3.0)) * 200);
//    //int pos = ConversionHelper.DegreesToSteps( (int)numericUpDown1.Value , MotorConstants.GEARING_RATIO_AZIMUTH );
//   // rt_controller.ExecuteMoveRelativeAzimuth(RadioTelescopeAxisEnum.AZIMUTH,speed, pos);
//}

//private void timer1_Tick(object sender, EventArgs e)
//{
//   // Entities.Orientation currentOrienation = rt_controller.GetCurrentOrientation();
//   // SetActualAZText(currentOrienation.Azimuth.ToString("0.##"));
//  //  SetActualELText(currentOrienation.Elevation.ToString("0.##"));
//}

//  delegate void SetActualAZTextCallback(string text);
//private void SetActualAZText(string text)
//{
//    // InvokeRequired required compares the thread ID of the
//    // calling thread to the thread ID of the creating thread.
//    // If these threads are different, it returns true.
//   // if (ActualAZTextBox.InvokeRequired)
//    {
//        //SetActualAZTextCallback d = new SetActualAZTextCallback(SetActualAZText);
//        var d = new SetActualAZTextCallback(SetActualAZText);
//        try
//        {
//            var task = Task.Run( () => Invoke( d , new object[] { text } ));
//            if(!task.Wait( 300 ))
//                throw new Exception( "Timed out" );

//        }
//        catch  {

//        }
//    }
//    else
//    {
//     //   ActualAZTextBox.Text = text;
//    }
//}

//delegate void SetActualELTextCallback(string text);
//private void SetActualELText(string text)
//{ }
// InvokeRequired required compares the thread ID of the
// calling thread to the thread ID of the creating thread.
// If these threads are different, it returns true.
//    if (ActualELTextBox.InvokeRequired)
//    {
//        //SetActualELTextCallback d = new SetActualELTextCallback(SetActualELText);
//        var d = new SetActualELTextCallback(SetActualELText);
//        try
//        {
//            var task = Task.Run( () => Invoke( d , new object[] { text } ) );
//            if(!task.Wait( 300 ))
//                throw new Exception( "Timed out" );

//        }
//        catch { }
//    }
//    else
//    {
//        ActualELTextBox.Text = text;
//    }
//}

//        private void NegButton_Click(object sender, EventArgs e)
//        {

//        }

//        private void ManualControlForm_Load(object sender, EventArgs e)
//        {

//        }