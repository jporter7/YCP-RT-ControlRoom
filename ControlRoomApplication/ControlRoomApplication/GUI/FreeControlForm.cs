using ControlRoomApplication.Controllers;
using ControlRoomApplication.Database;
using ControlRoomApplication.Entities;
using System;
using System.Threading;
using System.Windows.Forms;

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
            speed = 16667;
        }

        private void UpdateText(string text)
        {
            errorLabel.Text = text;
        }

        private void NegButton_MouseDown(object sender, MouseEventArgs e)
        {
            UpdateText("Moving");
            // Start CCW Jog
            rt_controller.StartRadioTelescopeAzimuthJog(100, false);
        }

        private void NegButton_MouseUp(object sender, MouseEventArgs e)
        {
            UpdateText("Free Control for Radio Telescope " + rt_controller.RadioTelescope.Id.ToString());
            // Stop Move
            rt_controller.HoldRadioTelescopeMove();
        }

        private void PosButton_MouseDown(object sender, MouseEventArgs e)
        {
            UpdateText("Moving");
            // Start CW Jog
            rt_controller.StartRadioTelescopeAzimuthJog(100, true);
        }

        private void PosButton_MouseUp(object sender, MouseEventArgs e)
        {
            UpdateText("Free Control for Radio Telescope " + rt_controller.RadioTelescope.Id.ToString());
            // Stop Move
            rt_controller.HoldRadioTelescopeMove();
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
    }
}
