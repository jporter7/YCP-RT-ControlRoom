using ControlRoomApplication.Controllers;
using ControlRoomApplication.Database;
using ControlRoomApplication.Entities;
using System;
using System.Windows.Forms;

namespace ControlRoomApplication.Main
{
    public partial class FreeControlForm : Form
    {
        public RadioTelescopeController rt_controller { get; set; }
        public Appointment CurrentAppointment { get; set; }
        public Coordinate TargetCoordinate { get; set; }
        public double Increment { get; set; }
        public int speed { get; set; }
        public CoordinateCalculationController CoordCalc { set; get; }
        public ControlRoom controlRoom { get; set; }
        public int rtId { get; set; }
        private static readonly log4net.ILog logger =
            log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

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
            CurrentAppointment.StartTime = DateTime.UtcNow.AddSeconds(5);
            CurrentAppointment.EndTime = DateTime.UtcNow.AddMinutes(15);
            CurrentAppointment.Status = AppointmentStatusEnum.REQUESTED;
            CurrentAppointment.Type = AppointmentTypeEnum.FREE_CONTROL;
            CurrentAppointment.SpectraCyberConfig = new SpectraCyberConfig(SpectraCyberModeTypeEnum.CONTINUUM);
            CurrentAppointment.TelescopeId = rtId;
            CurrentAppointment.UserId = 1;
            DatabaseOperations.AddAppointment(CurrentAppointment);
            //Calibrate Move
            CalibrateMove();
            // Set speed
            speedCombo.Text = "0.1 RPM";
            speed = 16667;

            logger.Info("FreeControl Form Initalized");
        }

        private void FreeControlForm_FormClosing(Object sender, FormClosingEventArgs e)
        {
            logger.Info("FreeControl Form Closing");
            timer1.Enabled = false;
        }

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

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox1.Text == "2 RPM")
            {
                logger.Info("Speed set to 2 RPM");
                speed = 333333;
            }
            else if (comboBox1.Text == "0.1 RPM")
            {
                logger.Info("Speed set to 0.1 RPM");
                speed = 16667;
            }
            else
            {
                logger.Info("Invalid Speed Selected");
                throw new Exception();
            }
        }

        //private void CalibrateButton_Click(object sender, EventArgs e)
        //{
        //    logger.Info("Calibrate Button Clicked");
        //    CalibrateMove();
        //}

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
        private void updateTextManual(string text)
        {
            errorLabel.Text = text;
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
        private void NegButton_MouseDown(object sender, MouseEventArgs e)
        {
            logger.Info("Jog NegButton MouseDown");
            updateTextManual("Moving at -" + comboBox1.Text);

            // Start CCW Jog
            rt_controller.StartRadioTelescopeAzimuthJog(speed, false);
        }

        private void NegButton_MouseUp(object sender, MouseEventArgs e)
        {
            logger.Info("Jog NegButton MouseUp");
            updateTextManual("Manual Control for Radio Telescope " + rt_controller.RadioTelescope.Id.ToString());

            // Stop Move
            ExecuteCorrectStop();
        }

        private void PosButton_MouseDown(object sender, MouseEventArgs e)
        {
            logger.Info("Jog PosButton MouseDown");
            updateTextManual("Moving at " + comboBox1.Text);

            // Start CW Jog
            rt_controller.StartRadioTelescopeAzimuthJog(speed, true);
        }

        private void PosButton_MouseUp(object sender, MouseEventArgs e)
        {
            logger.Info("Jog PosButton MouseUp");
            updateTextManual("Manual Control for Radio Telescope " + rt_controller.RadioTelescope.Id.ToString());

            // Stop Move
            ExecuteCorrectStop();
        }

        private void ExecuteCorrectStop()
        {
            if (controlRB.Checked)
            {
                logger.Info("Executed Controlled Stop");
                rt_controller.ExecuteRadioTelescopeControlledStop();
            }
            else if (immediateRB.Checked)
            {
                logger.Info("Executed Immediate Stop");
                rt_controller.ExecuteRadioTelescopeImmediateStop();
            }
            else
            {
                logger.Info("Invalid Stop Selected");
                throw new Exception();
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
            oneForthButton.BackColor = System.Drawing.Color.LightGray;
            oneButton.BackColor = System.Drawing.Color.LightGray;
            fiveButton.BackColor = System.Drawing.Color.LightGray;
            tenButton.BackColor = System.Drawing.Color.LightGray;

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
            bool save_state = (editButton.Text == "Save Position");
            if (save_state)
            {
                editButton.Text = "Edit Position";
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
            }

            PosDecButton.Enabled = save_state;
            NegDecButton.Enabled = save_state;
            PosRAButton.Enabled = save_state;
            NegRAButton.Enabled = save_state;
            //CalibrateButton.Enabled = save_state;
            TargetRATextBox.ReadOnly = save_state;
            TargetDecTextBox.ReadOnly = save_state;
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void ControledButtonRadio_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void button6_Click(object sender, EventArgs e)
        {

        }
    }
}
