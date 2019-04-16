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

        public FreeControlForm(ControlRoom new_controlRoom, int rtId)
        {
            InitializeComponent();
            // Set ControlRoom
            controlRoom = new_controlRoom;
            // Make rt_controller
            rt_controller = controlRoom.RadioTelescopeControllers[rtId - 1];
            // Update Text
            UpdateText("Free Control for Radio Telescope " + rt_controller.RadioTelescope.Id.ToString());
        }

        private void UpdateText(string text)
        {
            errorLabel.Text = text;
        }

        private void NegButton_MouseDown(object sender, MouseEventArgs e)
        {
            UpdateText("NegButton_MouseDown");
            // Start CCW Jog
        }

        private void NegButton_MouseUp(object sender, MouseEventArgs e)
        {
            UpdateText("NegButton_MouseUp");
            // Stop Move
        }

        private void PosButton_MouseDown(object sender, MouseEventArgs e)
        {
            UpdateText("PosButton_MouseDown");
            // Start CW Jog
        }

        private void PosButton_MouseUp(object sender, MouseEventArgs e)
        {
            UpdateText("PosButton_MouseUp");
            // Stop Move
        }
    }
}
