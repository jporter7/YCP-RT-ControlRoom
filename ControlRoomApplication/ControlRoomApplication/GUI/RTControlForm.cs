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
            editControlScripts.Enabled = false;

            // If the main control room controller hasn't been initialized, initialize it.
            //if (ControlRoomController == null)
            //{
            //    logger.Info("Initializing ControlRoomController");
            //    ControlRoomController = new ControlRoomController(new ControlRoom());
            //}

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
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void manualControlButton_Click(object sender, EventArgs e)
        {
            logger.Info("Activate Manual Control Clicked");
            bool manual_save_state = (manualControlButton.Text == "Activate Manual Control");
            //manualControlActive = !manualControlActive
            if (!manual_save_state)
            {
                manualControlButton.Text = "Activate Manual Control";
                manualControlButton.BackColor = System.Drawing.Color.Red;
                manualGroupBox.BackColor = System.Drawing.Color.DarkGray;
                //plusElaButton.BackColor = System.Drawing.Color.DarkGray;
                //plusJogButton.BackColor = System.Drawing.Color.DarkGray;
                //subElaButton.BackColor = System.Drawing.Color.DarkGray;
                //subJogButton.BackColor = System.Drawing.Color.DarkGray;
            }
            else if(manual_save_state)
            {
                manualControlButton.Text = "Deactivate Manual Control";
                manualControlButton.BackColor = System.Drawing.Color.LimeGreen;
                manualGroupBox.BackColor = System.Drawing.Color.Gainsboro;
                //plusElaButton.BackColor = System.Drawing.Color.DarkGray;
                //plusJogButton.BackColor = System.Drawing.Color.DarkGray;
                //subElaButton.BackColor = System.Drawing.Color.DarkGray;
                //subJogButton.BackColor = System.Drawing.Color.DarkGray;
            }
            plusElaButton.Enabled = manual_save_state;
            plusJogButton.Enabled = manual_save_state;
            subJogButton.Enabled = manual_save_state;
            subElaButton.Enabled = manual_save_state;
            ControledButtonRadio.Enabled = manual_save_state;
            immediateRadioButton.Enabled = manual_save_state;
            speedComboBox.Enabled = manual_save_state;
        }

        private void speedComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            logger.Info("Edit Control Scripts Button Clicked");

            editTestForm editTestForm = new editTestForm();
            //EditScriptsForm editScriptsForm = new EditScriptsForm(MainControlRoomController.ControlRoom, current_rt_id);
            editTestForm.Show();
     
        }

        private void RAIncGroupbox_Enter(object sender, EventArgs e)
        {

        }

        private void manualGroupBox_Enter(object sender, EventArgs e)
        {

        }

        private void subJogButton_Click(object sender, EventArgs e)
        {

        }
    }
}
