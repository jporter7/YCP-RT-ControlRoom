namespace ControlRoomApplication.Main
{
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            this.button1 = new System.Windows.Forms.Button();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.button2 = new System.Windows.Forms.Button();
            this.txtPLCPort = new System.Windows.Forms.TextBox();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.txtPLCIP = new System.Windows.Forms.TextBox();
            this.comboBox2 = new System.Windows.Forms.ComboBox();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.comboPLCType = new System.Windows.Forms.ComboBox();
            this.ManualControl = new System.Windows.Forms.Button();
            this.FreeControl = new System.Windows.Forms.Button();
            this.btnGoToDiagnosticsForm = new System.Windows.Forms.Button();
            this.comboTempSensorType = new System.Windows.Forms.ComboBox();
            this.comboEncoderType = new System.Windows.Forms.ComboBox();
            this.comboMCUType = new System.Windows.Forms.ComboBox();
            this.comboMicrocontrollerBox = new System.Windows.Forms.ComboBox();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.button1.BackColor = System.Drawing.Color.LimeGreen;
            this.button1.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F);
            this.button1.ForeColor = System.Drawing.Color.Black;
            this.button1.Location = new System.Drawing.Point(916, 428);
            this.button1.Margin = new System.Windows.Forms.Padding(2);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(187, 72);
            this.button1.TabIndex = 6;
            this.button1.Text = "Start Telescope";
            this.button1.UseVisualStyleBackColor = false;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToDeleteRows = false;
            this.dataGridView1.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F);
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridView1.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dataGridView1.DefaultCellStyle = dataGridViewCellStyle2;
            this.dataGridView1.Location = new System.Drawing.Point(9, 10);
            this.dataGridView1.Margin = new System.Windows.Forms.Padding(2);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowTemplate.Height = 24;
            this.dataGridView1.RowTemplate.ReadOnly = true;
            this.dataGridView1.Size = new System.Drawing.Size(403, 230);
            this.dataGridView1.TabIndex = 7;
            this.dataGridView1.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellContentClick);
            // 
            // button2
            // 
            this.button2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.button2.BackColor = System.Drawing.Color.Red;
            this.button2.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F);
            this.button2.Location = new System.Drawing.Point(916, 520);
            this.button2.Margin = new System.Windows.Forms.Padding(2);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(187, 68);
            this.button2.TabIndex = 7;
            this.button2.Text = "Shut Down Telescope";
            this.button2.UseVisualStyleBackColor = false;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // txtPLCPort
            // 
            this.txtPLCPort.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.txtPLCPort.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F);
            this.txtPLCPort.Location = new System.Drawing.Point(532, 571);
            this.txtPLCPort.Margin = new System.Windows.Forms.Padding(2);
            this.txtPLCPort.Name = "txtPLCPort";
            this.txtPLCPort.Size = new System.Drawing.Size(114, 29);
            this.txtPLCPort.TabIndex = 5;
            this.txtPLCPort.Text = "PLC Port";
            this.txtPLCPort.GotFocus += new System.EventHandler(this.textBox1_Focus);
            // 
            // comboBox1
            // 
            this.comboBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.comboBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F);
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Items.AddRange(new object[] {
            "Production SpectraCyber",
            "Simulated SpectraCyber"});
            this.comboBox1.Location = new System.Drawing.Point(278, 569);
            this.comboBox1.Margin = new System.Windows.Forms.Padding(2);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(237, 32);
            this.comboBox1.TabIndex = 2;
            this.comboBox1.Text = "SpectraCyber Type";
            // 
            // txtPLCIP
            // 
            this.txtPLCIP.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.txtPLCIP.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F);
            this.txtPLCIP.Location = new System.Drawing.Point(532, 529);
            this.txtPLCIP.Margin = new System.Windows.Forms.Padding(2);
            this.txtPLCIP.Name = "txtPLCIP";
            this.txtPLCIP.Size = new System.Drawing.Size(114, 29);
            this.txtPLCIP.TabIndex = 4;
            this.txtPLCIP.Text = "PLC IP";
            this.txtPLCIP.GotFocus += new System.EventHandler(this.textBox2_Focus);
            // 
            // comboBox2
            // 
            this.comboBox2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.comboBox2.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F);
            this.comboBox2.FormattingEnabled = true;
            this.comboBox2.Items.AddRange(new object[] {
            "Production Weather Station",
            "Simulated Weather Station",
            "Test Weather Station"});
            this.comboBox2.Location = new System.Drawing.Point(9, 569);
            this.comboBox2.Margin = new System.Windows.Forms.Padding(2);
            this.comboBox2.Name = "comboBox2";
            this.comboBox2.Size = new System.Drawing.Size(252, 32);
            this.comboBox2.TabIndex = 1;
            this.comboBox2.Text = "Weather Station Type";
            // 
            // checkBox1
            // 
            this.checkBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.checkBox1.AutoSize = true;
            this.checkBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F);
            this.checkBox1.Location = new System.Drawing.Point(9, 533);
            this.checkBox1.Margin = new System.Windows.Forms.Padding(2);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(228, 28);
            this.checkBox1.TabIndex = 0;
            this.checkBox1.Text = "Populate local database";
            this.checkBox1.UseVisualStyleBackColor = true;
            // 
            // comboPLCType
            // 
            this.comboPLCType.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.comboPLCType.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F);
            this.comboPLCType.FormattingEnabled = true;
            this.comboPLCType.Items.AddRange(new object[] {
            "Production PLC",
            "Scale PLC",
            "Simulation PLC",
            "Test PLC"});
            this.comboPLCType.Location = new System.Drawing.Point(532, 479);
            this.comboPLCType.Margin = new System.Windows.Forms.Padding(2);
            this.comboPLCType.Name = "comboPLCType";
            this.comboPLCType.Size = new System.Drawing.Size(153, 32);
            this.comboPLCType.TabIndex = 3;
            this.comboPLCType.Text = "PLC Type";
            // 
            // ManualControl
            // 
            this.ManualControl.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.ManualControl.BackColor = System.Drawing.Color.LightGray;
            this.ManualControl.Enabled = false;
            this.ManualControl.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F);
            this.ManualControl.Location = new System.Drawing.Point(916, 32);
            this.ManualControl.Margin = new System.Windows.Forms.Padding(2);
            this.ManualControl.Name = "ManualControl";
            this.ManualControl.Size = new System.Drawing.Size(185, 72);
            this.ManualControl.TabIndex = 8;
            this.ManualControl.Text = "Manual Control";
            this.ManualControl.UseVisualStyleBackColor = false;
            this.ManualControl.Click += new System.EventHandler(this.ManualControl_Click);
            // 
            // FreeControl
            // 
            this.FreeControl.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.FreeControl.BackColor = System.Drawing.Color.LightGray;
            this.FreeControl.Enabled = false;
            this.FreeControl.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F);
            this.FreeControl.Location = new System.Drawing.Point(708, 32);
            this.FreeControl.Margin = new System.Windows.Forms.Padding(2);
            this.FreeControl.Name = "FreeControl";
            this.FreeControl.Size = new System.Drawing.Size(185, 72);
            this.FreeControl.TabIndex = 9;
            this.FreeControl.Text = "Free Control";
            this.FreeControl.UseVisualStyleBackColor = false;
            this.FreeControl.Click += new System.EventHandler(this.FreeControl_Click);
            // 
            // btnGoToDiagnosticsForm
            // 
            this.btnGoToDiagnosticsForm.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnGoToDiagnosticsForm.Location = new System.Drawing.Point(500, 32);
            this.btnGoToDiagnosticsForm.Name = "btnGoToDiagnosticsForm";
            this.btnGoToDiagnosticsForm.Size = new System.Drawing.Size(185, 72);
            this.btnGoToDiagnosticsForm.TabIndex = 10;
            this.btnGoToDiagnosticsForm.Text = "Diagnostics Form";
            this.btnGoToDiagnosticsForm.UseVisualStyleBackColor = true;
            this.btnGoToDiagnosticsForm.Click += new System.EventHandler(this.btnGoToDiagnosticsForm_Click);
            // 
            // comboTempSensorType
            // 
            this.comboTempSensorType.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.comboTempSensorType.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F);
            this.comboTempSensorType.FormattingEnabled = true;
            this.comboTempSensorType.Items.AddRange(new object[] {
            "Production Temp Sensor",
            "Simulated Temp Sensor",
            "Test Temp Sensor"});
            this.comboTempSensorType.Location = new System.Drawing.Point(278, 526);
            this.comboTempSensorType.Name = "comboTempSensorType";
            this.comboTempSensorType.Size = new System.Drawing.Size(237, 32);
            this.comboTempSensorType.TabIndex = 11;
            this.comboTempSensorType.Text = "Temp Sensor Type";
            // 
            // comboEncoderType
            // 
            this.comboEncoderType.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.comboEncoderType.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F);
            this.comboEncoderType.FormattingEnabled = true;
            this.comboEncoderType.Items.AddRange(new object[] {
            "Production Absolute Encoder",
            "Simulated Absolute Encoder",
            "Test Absolute Encoder"});
            this.comboEncoderType.Location = new System.Drawing.Point(278, 479);
            this.comboEncoderType.Name = "comboEncoderType";
            this.comboEncoderType.Size = new System.Drawing.Size(237, 32);
            this.comboEncoderType.TabIndex = 12;
            this.comboEncoderType.Text = "Absolute Encoder Type";
            // 
            // comboMCUType
            // 
            this.comboMCUType.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F);
            this.comboMCUType.FormattingEnabled = true;
            this.comboMCUType.Items.AddRange(new object[] {
            "Production MCU",
            "Simulated MCU"});
            this.comboMCUType.Location = new System.Drawing.Point(278, 430);
            this.comboMCUType.Name = "comboMCUType";
            this.comboMCUType.Size = new System.Drawing.Size(237, 32);
            this.comboMCUType.TabIndex = 13;
            this.comboMCUType.Text = "MCU Type";
            // 
            // comboMicrocontrollerBox
            // 
            this.comboMicrocontrollerBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F);
            this.comboMicrocontrollerBox.FormattingEnabled = true;
            this.comboMicrocontrollerBox.Items.AddRange(new object[] {
            "Production Microcontroller",
            "Simulated Microcontroller"});
            this.comboMicrocontrollerBox.Location = new System.Drawing.Point(278, 380);
            this.comboMicrocontrollerBox.Name = "comboMicrocontrollerBox";
            this.comboMicrocontrollerBox.Size = new System.Drawing.Size(237, 32);
            this.comboMicrocontrollerBox.TabIndex = 14;
            this.comboMicrocontrollerBox.Text = "Microcontroller Type";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1112, 607);
            this.Controls.Add(this.comboMicrocontrollerBox);
            this.Controls.Add(this.comboMCUType);
            this.Controls.Add(this.comboEncoderType);
            this.Controls.Add(this.comboTempSensorType);
            this.Controls.Add(this.btnGoToDiagnosticsForm);
            this.Controls.Add(this.FreeControl);
            this.Controls.Add(this.ManualControl);
            this.Controls.Add(this.comboPLCType);
            this.Controls.Add(this.checkBox1);
            this.Controls.Add(this.comboBox2);
            this.Controls.Add(this.txtPLCIP);
            this.Controls.Add(this.comboBox1);
            this.Controls.Add(this.txtPLCPort);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.button1);
            this.Margin = new System.Windows.Forms.Padding(2);
            this.MinimumSize = new System.Drawing.Size(922, 367);
            this.Name = "MainForm";
            this.Text = "MainForm";
            this.Load += new System.EventHandler(this.MainForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.TextBox txtPLCPort;
        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.TextBox txtPLCIP;
        private System.Windows.Forms.ComboBox comboBox2;
        private System.Windows.Forms.CheckBox checkBox1;
        private System.Windows.Forms.ComboBox comboPLCType;
        private System.Windows.Forms.Button ManualControl;
        private System.Windows.Forms.Button FreeControl;
        private System.Windows.Forms.Button btnGoToDiagnosticsForm;
        private System.Windows.Forms.ComboBox comboTempSensorType;
        private System.Windows.Forms.ComboBox comboEncoderType;
        private System.Windows.Forms.ComboBox comboMCUType;
        private System.Windows.Forms.ComboBox comboMicrocontrollerBox;
    }
}