using System;
using System.Windows.Forms;
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
            speedRPM = ConversionHelper.RPMToDPS(0.1);

            // Set demo thread and flag
            CreateDemoThread();
            DemoRunning = false;

            logger.Info("ManualControlForm Initalized");



            // The designer is... "bad", we'll say, and is driving me nuts - and I prefer to actually write my code anyway, so here's my event binding, go ahead and @ me
            NegButtonEL.MouseDown += new MouseEventHandler(NegButtonEL_MouseDown);
            NegButtonEL.MouseUp += new MouseEventHandler(NegButtonEL_MouseUp);
            PosButtonEL.MouseDown += new MouseEventHandler(PosButtonEL_MouseDown);
            PosButtonEL.MouseUp += new MouseEventHandler(PosButtonEL_MouseUp);

            AbsoluteMoveSubmit.Click += new EventHandler(AbsoluteMoveSubmit_Click);

            ClearCommandsSubmit.Click += new EventHandler(ClearCommandsSubmit_Click);
        }

        private void ManualControlForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            logger.Info("ManualControl Form Closing");
            timer1.Enabled = false;
        }

        private void UpdateText(string text)
        {
            errorLabel.Text = text;
        }

        private void NegButtonAZ_MouseDown(object sender, MouseEventArgs e)
        {
            logger.Info("Jog NegButton Azimuth MouseDown");
            UpdateText("Moving at -" + comboBox1.Text);

            // Start CCW Jog
            rt_controller.StartRadioTelescopeAzimuthJog(speedRPM, false);
        }

        private void NegButtonAZ_MouseUp(object sender, MouseEventArgs e)
        {
            logger.Info("Jog NegButton Azimuth MouseUp");
            UpdateText("Manual Control for Radio Telescope " + rt_controller.RadioTelescope.Id.ToString());

            // Stop Move
            ExecuteCorrectStop();
        }

        private void PosButtonAZ_MouseDown(object sender, MouseEventArgs e)
        {
            logger.Info("Jog PosButton Azimuth MouseDown");
            UpdateText("Moving at " + comboBox1.Text);

            // Start CW Jog
            rt_controller.StartRadioTelescopeAzimuthJog(speedRPM, true);
        }

        private void PosButtonAZ_MouseUp(object sender, MouseEventArgs e)
        {
            logger.Info("Jog PosButton Azimuth MouseUp");
            UpdateText("Manual Control for Radio Telescope " + rt_controller.RadioTelescope.Id.ToString());

            // Stop Move
            ExecuteCorrectStop();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(comboBox1.Text == "2 RPM")
            {
                logger.Info("Speed set to 2 RPM");
                speedRPM = ConversionHelper.RPMToDPS(2.0);
            }
            else if(comboBox1.Text == "0.1 RPM")
            {
                logger.Info("Speed set to 0.1 RPM");
                speedRPM = ConversionHelper.RPMToDPS(0.1);
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

        private void RelativeMoveSubmit_Click(object sender, EventArgs e)
        {
            logger.Info("Move Relative Button Clicked");
            rt_controller.ExecuteRelativeMove(speedRPM, speedRPM, (double)numericUpDown1.Value, (double)numericUpDown2.Value);
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
            RelativeMoveSubmit.Enabled = enabled;
            numericUpDown1.Enabled = enabled;
            NegButtonAZ.Enabled = enabled;
            PosButtonAZ.Enabled = enabled;
            radioButton1.Enabled = enabled;
            radioButton2.Enabled = enabled;
        }

        private void StartDemo()
        {
            logger.Info("Running Demo");
            while (DemoRunning)
            {
                logger.Info("Executing command #0: relative move, elevation, 45 degrees, 80 second sleep.");
                rt_controller.ExecuteRelativeMoveElevation(speedRPM, 45);
                Thread.Sleep(80000);

                logger.Info("Executing command #1: CCW jog move, elevation, 10 seconds, controlled stop.");
                rt_controller.StartRadioTelescopeElevationJog(speedRPM, false);
                Thread.Sleep(10000);
                rt_controller.ExecuteRadioTelescopeControlledStop();
                Thread.Sleep(3000);

                logger.Info("Executing command #2: relative move, elevation, -45 degrees, 80 second sleep.");
                rt_controller.ExecuteRelativeMoveElevation(speedRPM, -45);
                Thread.Sleep(80000);

                logger.Info("Executing command #3: CCW jog move, elevation, 5 seconds, immediate stop.");
                rt_controller.StartRadioTelescopeElevationJog(speedRPM, false);
                Thread.Sleep(5000);
                rt_controller.ExecuteRadioTelescopeImmediateStop();
                Thread.Sleep(1000);

                logger.Info("Executing command #2: relative move, elevation, 90 degrees, 160 second sleep.");
                rt_controller.ExecuteRelativeMoveElevation(speedRPM, 90);
                Thread.Sleep(160000);
                rt_controller.CancelCurrentMoveCommand();

                logger.Info("Executing command #3: relative move, elevation, -180 degrees, 320 second sleep.");
                rt_controller.ExecuteRelativeMoveElevation(speedRPM, -180);
                Thread.Sleep(320000);
                rt_controller.CancelCurrentMoveCommand();

                logger.Info("Executing command #4: absolute move, [0, 0] degrees, 160 second sleep.");
                rt_controller.MoveRadioTelescopeToOrientation(new Entities.Orientation(0, 0));
                Thread.Sleep(160000);
                rt_controller.CancelCurrentMoveCommand();
            }
        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {

        }

        private void NegButtonEL_MouseDown(object sender, MouseEventArgs e)
        {
            logger.Info("Jog NegButton Elevation MouseDown");
            UpdateText("Moving at -" + comboBox1.Text);

            // Start CCW Jog
            rt_controller.StartRadioTelescopeElevationJog(speedRPM, false);
        }

        private void NegButtonEL_MouseUp(object sender, MouseEventArgs e)
        {
            logger.Info("Jog NegButton Elevation MouseUp");
            UpdateText("Manual Control for Radio Telescope " + rt_controller.RadioTelescope.Id.ToString());

            // Stop Move
            ExecuteCorrectStop();
        }

        private void PosButtonEL_MouseDown(object sender, MouseEventArgs e)
        {
            logger.Info("Jog PosButton Elevation MouseDown");
            UpdateText("Moving at " + comboBox1.Text);

            // Start CW Jog
            rt_controller.StartRadioTelescopeElevationJog(speedRPM, true);
        }

        private void PosButtonEL_MouseUp(object sender, MouseEventArgs e)
        {
            logger.Info("Jog PosButton Elevation MouseUp");
            UpdateText("Manual Control for Radio Telescope " + rt_controller.RadioTelescope.Id.ToString());

            // Stop Move
            ExecuteCorrectStop();
        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void ActualAZTextBox_TextChanged(object sender, EventArgs e)
        {

        }

        private void ActualELTextBox_TextChanged(object sender, EventArgs e)
        {

        }

        private void AbsoluteMoveSubmit_Click(object sender, EventArgs e)
        {
            logger.Info("Move Absolute Button Clicked");
            rt_controller.MoveRadioTelescopeToOrientation(new Entities.Orientation((double)numericUpDown1.Value, (double)numericUpDown2.Value));
        }

        private void ClearCommandsSubmit_Click(object sender, EventArgs e)
        {
            logger.Info("Clear Commands Button Clicked");
            rt_controller.CancelCurrentMoveCommand();
        }
    }
}
