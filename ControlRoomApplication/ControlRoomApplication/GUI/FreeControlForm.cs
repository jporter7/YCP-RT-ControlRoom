using System;
using System.Windows.Forms;
using ControlRoomApplication.Entities;
using ControlRoomApplication.Controllers;

namespace ControlRoomApplication.Main
{
    public partial class FreeControlForm : Form
    {
        public RadioTelescopeController rt_controller { get; set; }
        public ControlRoom controlRoom { get; set; }
        public int speed { get; set; }

        public FreeControlForm(ControlRoom new_controlRoom, int rtId)
        {
            InitializeComponent();

            // Set ControlRoom
            controlRoom = new_controlRoom;

            // Make rt_controller
            rt_controller = controlRoom.RadioTelescopeControllers[rtId - 1];

            // Update Text
            UpdateText("Free Control for Radio Telescope " + rt_controller.RadioTelescope.Id.ToString());

            // Set speed
            comboBox1.Text = "0.1 RPM";
            speed = 16667;
        }

        private void UpdateText(string text)
        {
            errorLabel.Text = text;
        }

        private void NegButton_MouseDown(object sender, MouseEventArgs e)
        {
            UpdateText("Moving at -" + comboBox1.Text);

            // Start CCW Jog
            rt_controller.StartRadioTelescopeAzimuthJog(speed, false);
        }

        private void NegButton_MouseUp(object sender, MouseEventArgs e)
        {
            UpdateText("Free Control for Radio Telescope " + rt_controller.RadioTelescope.Id.ToString());

            // Stop Move
            ExecuteCorrectStop();
        }

        private void PosButton_MouseDown(object sender, MouseEventArgs e)
        {
            UpdateText("Moving at " + comboBox1.Text);

            // Start CW Jog
            rt_controller.StartRadioTelescopeAzimuthJog(speed, true);
        }

        private void PosButton_MouseUp(object sender, MouseEventArgs e)
        {
            UpdateText("Free Control for Radio Telescope " + rt_controller.RadioTelescope.Id.ToString());

            // Stop Move
            ExecuteCorrectStop();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(comboBox1.Text == "2 RPM")
            {
                speed = 333333;
            }
            else if(comboBox1.Text == "0.1 RPM")
            {
                speed = 16667;
            }
            else
            {
                throw new Exception();
            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            // Do nothing
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            // Do nothing
        }

        private void ExecuteCorrectStop()
        {
            if (radioButton1.Checked)
            {
                rt_controller.ExecuteRadioTelescopeControlledStop();
            }
            else if (radioButton2.Checked)
            {
                rt_controller.ExecuteRadioTelescopeImmediateStop();
            }
            else
            {
                throw new Exception();
            }
        }
    }
}
