﻿using ControlRoomApplication.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ControlRoomApplication.GUI
{
    public partial class DiagnosticsForm : Form
    {
        private ControlRoom controlRoom;
        private int rtId;
        private string[] statuses = { "Offline", "Offline" };

        /// <summary>
        /// Initializes the diagnostic form based off of the specified configuration.
        /// </summary>
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

            label3.Text = controlRoom.RadioTelescopeControllers[rtId].GetCurrentOrientation().Azimuth.ToString();
            label4.Text = controlRoom.RadioTelescopeControllers[rtId].GetCurrentOrientation().Elevation.ToString();
        }

        /// <summary>
        /// Gets and displays the current statuses of the hardware components for the specified configuration.
        /// </summary>
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

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}
