﻿using System;
using System.Windows.Forms;
using ControlRoomApplication.Constants;
using ControlRoomApplication.Entities;
using ControlRoomApplication.Controllers;
using System.Threading;

namespace ControlRoomApplication.Main
{
    public partial class ManualControlForm : Form
    {
        public RadioTelescopeController rt_controller { get; set; }
        public ControlRoom controlRoom { get; set; }
        public double speedRPM { get; set; }
        public bool DemoRunning { get; set; }
        public Thread DemoThread { get; set; }
        private static readonly log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

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
            speedRPM = fromRPMsToDegreesPerSecond(0.1);

            // Set demo thread and flag
            CreateDemoThread();
            DemoRunning = false;

            logger.Info("ManualControlForm Initalized");
        }

        private static int fromDegreesToSteps(double degs)
        {
            return (int)(degs / 360 * MiscellaneousConstants.GEARED_STEPS_PER_REVOLUTION);
        }

        private static double fromRevolutionsToDegrees(double revs)
        {
            return revs * 360;
        }

        private static double fromRPMsToDegreesPerSecond(double rpms)
        {
            return rpms * 6;
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
            rt_controller.StartRadioTelescopeAzimuthJog(speedRPM, false);
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
            rt_controller.StartRadioTelescopeAzimuthJog(speedRPM, true);
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
                speedRPM = fromRPMsToDegreesPerSecond(2.0);
            }
            else if(comboBox1.Text == "0.1 RPM")
            {
                logger.Info("Speed set to 0.1 RPM");
                speedRPM = fromRPMsToDegreesPerSecond(0.1);
            }
            else
            {
                logger.Info("Invalid Speed Selected");
                throw new Exception();
            }
        }

        private void ExecuteCorrectStop()
        {
            if (radioButton1.Checked)
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
            rt_controller.ExecuteRelativeMoveAzimuth(speedRPM, (double)numericUpDown1.Value);
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
                SetActualAZTextCallback d = new SetActualAZTextCallback(SetActualAZText);
                Invoke(d, new object[] { text });
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
                SetActualELTextCallback d = new SetActualELTextCallback(SetActualELText);
                Invoke(d, new object[] { text });
            }
            else
            {
                ActualELTextBox.Text = text;
            }
        }

        private void DemoButton_Click(object sender, EventArgs e)
        {
            if (!DemoRunning)
            {
                SetButtonsEnabled(false);
                DemoButton.Text = "Stop Demo";
                DemoRunning = true;
                DemoThread.Start();
            }
            else
            {
                SetButtonsEnabled(true);
                DemoButton.Text = "Start Demo";
                DemoRunning = false;
                DemoThread.Join();
                CreateDemoThread();
            }
        }

        private void CreateDemoThread()
        {
            DemoThread = new Thread(() => StartDemo())
            {
                Name = "Demo Thread"
            };
        }

        private void SetButtonsEnabled(bool enabled)
        {
            comboBox1.Enabled = enabled;
            button1.Enabled = enabled;
            numericUpDown1.Enabled = enabled;
            NegButton.Enabled = enabled;
            PosButton.Enabled = enabled;
            radioButton1.Enabled = enabled;
            radioButton2.Enabled = enabled;
        }

        private void StartDemo()
        {
            logger.Info("Running Demo");
            while (DemoRunning)
            {
                //rt_controller.ExecuteRelativeMoveAzimuth(speedRPM, 45);
                //Thread.Sleep(5000);
                //rt_controller.ExecuteRelativeMoveAzimuth(speedRPM, -45);
                //Thread.Sleep(5000);
                //rt_controller.ExecuteRelativeMoveAzimuth(speedRPM, 90);
                //Thread.Sleep(10000);
                //rt_controller.ExecuteRelativeMoveAzimuth(speedRPM, -180);
                //Thread.Sleep(15000);
                //rt_controller.ExecuteRelativeMoveAzimuth(speedRPM, 90);
                //Thread.Sleep(10000);
            }
        }
    }
}
