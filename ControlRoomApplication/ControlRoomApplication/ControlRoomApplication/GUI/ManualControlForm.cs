using System;
using System.Windows.Forms;
using ControlRoomApplication.Entities;
using ControlRoomApplication.Controllers;
using System.Threading.Tasks;
using ControlRoomApplication.Constants;

namespace ControlRoomApplication.Main
{
    public partial class ManualControlForm : Form
    {
        public RadioTelescopeController rt_controller { get; set; }
        public ControlRoom controlRoom { get; set; }
        public int speed { get; set; }
        private static readonly log4net.ILog logger =
            log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public ManualControlForm(ControlRoom new_controlRoom, int rtId)
        {
            InitializeComponent();

            // Set ControlRoom
            controlRoom = new_controlRoom;

            // Make rt_controller
            rt_controller = controlRoom.RadioTelescopeControllers[rtId - 1];

            // Update Text
            UpdateText("Manual Control for Radio Telescope " + rt_controller.RadioTelescope.Id.ToString());

            // Set speed
            comboBox1.Text = "0.1 RPM";
            speed = 16667;

            logger.Info("ManualControlForm Initalized");
        }

        private void ManualControlForm_FormClosing(Object sender, FormClosingEventArgs e)
        {
            logger.Info("ManualControl Form Closing");
            timer1.Enabled = false;
        }

        private void UpdateText(string text)
        {
            errorLabel.Text = text;
        }

        private void NegButton_MouseDown(object sender, MouseEventArgs e)
        {
            logger.Info("Jog NegButton MouseDown");
            UpdateText("Moving at -" + comboBox1.Text);

            // Start CCW Jog
            rt_controller.StartRadioTelescopeAzimuthJog(speed, false);
        }

        private void NegButton_MouseUp(object sender, MouseEventArgs e)
        {
            logger.Info("Jog NegButton MouseUp");
            UpdateText("Manual Control for Radio Telescope " + rt_controller.RadioTelescope.Id.ToString());

            // Stop Move
            ExecuteCorrectStop();
        }

        private void PosButton_MouseDown(object sender, MouseEventArgs e)
        {
            logger.Info("Jog PosButton MouseDown");
            UpdateText("Moving at " + comboBox1.Text);

            // Start CW Jog
            rt_controller.StartRadioTelescopeAzimuthJog(speed, true);
        }

        private void PosButton_MouseUp(object sender, MouseEventArgs e)
        {
            logger.Info("Jog PosButton MouseUp");
            UpdateText("Manual Control for Radio Telescope " + rt_controller.RadioTelescope.Id.ToString());

            // Stop Move
            ExecuteCorrectStop();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(comboBox1.Text == "2 RPM")
            {
                logger.Info("Speed set to 2 RPM");
                speed = 333333;
            }
            else if(comboBox1.Text == "0.1 RPM")
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

        private void ExecuteCorrectStop()
        {
            if (ControledButtonRadio.Checked)
            {
                logger.Info("Executed Controlled Stop");
                rt_controller.ExecuteRadioTelescopeControlledStop();
            }
            else if (radioButton2.Checked)
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

        private void button1_Click(object sender, EventArgs e)
        {
            logger.Info("Move Relative Button Clicked");
            //int pos = (int)numericUpDown1.Value * (int)((166 + (2.0 / 3.0)) * 200);
            int pos = ConversionHelper.DegreesToSteps( (int)numericUpDown1.Value , MotorConstants.GEARING_RATIO_AZIMUTH );
            rt_controller.ExecuteMoveRelativeAzimuth(RadioTelescopeAxisEnum.AZIMUTH,speed, pos);
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            Entities.Orientation currentOrienation = rt_controller.GetCurrentOrientation();
            SetActualAZText(currentOrienation.Azimuth.ToString("0.##"));
            SetActualELText(currentOrienation.Elevation.ToString("0.##"));
        }

        delegate void SetActualAZTextCallback(string text);
        private void SetActualAZText(string text)
        {
            // InvokeRequired required compares the thread ID of the
            // calling thread to the thread ID of the creating thread.
            // If these threads are different, it returns true.
            if (ActualAZTextBox.InvokeRequired)
            {
                //SetActualAZTextCallback d = new SetActualAZTextCallback(SetActualAZText);
                var d = new SetActualAZTextCallback(SetActualAZText);
                try
                {
                    var task = Task.Run( () => Invoke( d , new object[] { text } ));
                    if(!task.Wait( 300 ))
                        throw new Exception( "Timed out" );

                }
                catch  {

                }
            }
            else
            {
                ActualAZTextBox.Text = text;
            }
        }

        delegate void SetActualELTextCallback(string text);
        private void SetActualELText(string text)
        {
            // InvokeRequired required compares the thread ID of the
            // calling thread to the thread ID of the creating thread.
            // If these threads are different, it returns true.
            if (ActualELTextBox.InvokeRequired)
            {
                //SetActualELTextCallback d = new SetActualELTextCallback(SetActualELText);
                var d = new SetActualELTextCallback(SetActualELText);
                try
                {
                    var task = Task.Run( () => Invoke( d , new object[] { text } ) );
                    if(!task.Wait( 300 ))
                        throw new Exception( "Timed out" );

                }
                catch { }
            }
            else
            {
                ActualELTextBox.Text = text;
            }
        }

        private void ControledButtonRadio_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void NegButton_Click(object sender, EventArgs e)
        {

        }

        private void PosButton_Click(object sender, EventArgs e)
        {

        }
    }
}
