using ControlRoomApplication.Entities;
using System.Windows.Forms;

namespace ControlRoomApplication.GUI
{
    public partial class DiagnosticsForm : Form
    {
        private ControlRoom controlRoom;
        private int rtId;
        private double az;
        private double el;
        private string[] statuses = { "Offline", "Offline", "Offline" };
        private static readonly log4net.ILog logger =
            log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Initializes the diagnostic form based off of the specified configuration.
        /// </summary>
        public DiagnosticsForm(ControlRoom controlRoom, int rtId)
        {
            InitializeComponent();
            az = 0.0;
            el = 0.0;
            this.controlRoom = controlRoom;
            this.rtId = rtId;

            dataGridView1.ColumnCount = 2;
            dataGridView1.Columns[0].HeaderText = "Hardware";
            dataGridView1.Columns[1].HeaderText = "Status";

            GetHardwareStatuses();
            string[] spectraCyberRow = { "SpectraCyber", statuses[0] };
            string[] weatherStationRow = { "Weather Station", statuses[1] };
            string[] mcuRow = { "MCU", statuses[2] };

            dataGridView1.Rows.Add(spectraCyberRow);
            dataGridView1.Rows.Add(weatherStationRow);
            dataGridView1.Rows.Add(mcuRow);
            dataGridView1.Update();

            SetCurrentAzimuthAndElevation();
            logger.Info("DiagnosticsForm Initalized");
        }

        private void SetCurrentAzimuthAndElevation()
        {
            label3.Text = controlRoom.RadioTelescopeControllers[rtId].GetCurrentOrientation().Azimuth.ToString("0.00");
            label4.Text = controlRoom.RadioTelescopeControllers[rtId].GetCurrentOrientation().Elevation.ToString("0.00");
        }

        /// <summary>
        /// Gets and displays the current statuses of the hardware components for the specified configuration.
        /// </summary>
        private void GetHardwareStatuses()
        {
            if (controlRoom.RadioTelescopes[rtId].SpectraCyberController.IsConsideredAlive())
            {
                statuses[0] = "Online";
            } 

            if (controlRoom.WeatherStation.IsConsideredAlive())
            {
                statuses[1] = "Online";
            }

            if (controlRoom.RTControllerManagementThreads[rtId].RTController.TestCommunication())
            {
                statuses[1] = "Online";
            }
        }

        public delegate void SetStartTimeTextCallback(string text);
        public void SetStartTimeText(string text)
        {
            if (startTimeTextBox.InvokeRequired)
            {
                SetStartTimeTextCallback d = new SetStartTimeTextCallback(SetStartTimeText);
                Invoke(d, new object[] { text });
            }
            else
            {
                startTimeTextBox.Text = text;
            }
        }

        public delegate void SetEndTimeTextCallback(string text);
        public void SetEndTimeText(string text)
        {
            if (endTimeTextBox.InvokeRequired)
            {
                SetEndTimeTextCallback d = new SetEndTimeTextCallback(SetEndTimeText);
                Invoke(d, new object[] { text });
            }
            else
            {
                endTimeTextBox.Text = text;
            }
        }

        public delegate void SetApptStatusTextCallback(string text);
        public void SetApptStatusText(string text)
        {
            if (statusTextBox.InvokeRequired)
            {
                SetApptStatusTextCallback d = new SetApptStatusTextCallback(SetApptStatusText);
                Invoke(d, new object[] { text });
            }
            else
            {
                statusTextBox.Text = text;
            }
        }

        public delegate void SetApptTypeTextCallback(string text);
        public void SetApptTypeText(string text)
        {
            if (typeTextBox.InvokeRequired)
            {
                SetApptTypeTextCallback d = new SetApptTypeTextCallback(SetApptTypeText);
                Invoke(d, new object[] { text });
            }
            else
            {
                typeTextBox.Text = text;
            }
        }

        private void timer1_Tick(object sender, System.EventArgs e)
        {
            if (controlRoom.RTControllerManagementThreads[rtId].AppointmentToDisplay != null)
            {
                SetStartTimeText(controlRoom.RTControllerManagementThreads[rtId].AppointmentToDisplay.StartTime.ToLocalTime().ToString("hh:mm:ss tt"));
                SetEndTimeText(controlRoom.RTControllerManagementThreads[rtId].AppointmentToDisplay.EndTime.ToLocalTime().ToString("hh:mm:ss tt"));
                SetApptStatusText(controlRoom.RTControllerManagementThreads[rtId].AppointmentToDisplay.Status.ToString());
                SetApptTypeText(controlRoom.RTControllerManagementThreads[rtId].AppointmentToDisplay.Type.ToString());
            }

            GetHardwareStatuses();

            SetCurrentAzimuthAndElevation();

            dataGridView1.Update();
        }

        private void DiagnosticsForm_Load(object sender, System.EventArgs e)
        {

        }
    }
}
