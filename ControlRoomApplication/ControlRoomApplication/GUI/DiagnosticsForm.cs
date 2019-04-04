using ControlRoomApplication.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ControlRoomApplication.GUI
{
    public partial class DiagnosticsForm : Form
    {
        private ControlRoom controlRoom;
        private int rtId;
        private string[] statuses = { "Offline", "Offline" };

        public DiagnosticsForm(ControlRoom controlRoom, int rtId)
        {
            InitializeComponent();
            this.controlRoom = controlRoom;
            this.rtId = rtId;

            dataGridView1.ColumnCount = 2;
            dataGridView1.Columns[0].HeaderText = "Hardware";
            dataGridView1.Columns[1].HeaderText = "Status";

            GetStatuses();
            string[] spectraCyberRow = { "SpectraCyber", statuses[0] };
            string[] weatherStationRow = { "Weather Station", statuses[1] };

            dataGridView1.Rows.Add(spectraCyberRow);
            dataGridView1.Rows.Add(weatherStationRow);
            dataGridView1.Update();

            label3.Text = controlRoom.RadioTelescopes[rtId].CurrentOrientation.Azimuth.ToString();
            label3.Text = controlRoom.RadioTelescopes[rtId].CurrentOrientation.Elevation.ToString();
        }

        private void GetStatuses()
        {
            if (controlRoom.RadioTelescopes[rtId].SpectraCyberController.IsConsideredAlive())
            {
                statuses[0] = "Online";
            } 

            if (controlRoom.WeatherStation.IsConsideredAlive())
            {
                statuses[1] = "Online";
            }
        }
    }
}
