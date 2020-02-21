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
            this.startButton = new System.Windows.Forms.Button();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.shutdownButton = new System.Windows.Forms.Button();
            this.txtPLCPort = new System.Windows.Forms.TextBox();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.txtPLCIP = new System.Windows.Forms.TextBox();
            this.comboBox2 = new System.Windows.Forms.ComboBox();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.comboPLCType = new System.Windows.Forms.ComboBox();
            this.FreeControl = new System.Windows.Forms.Button();
            this.comboEncoderType = new System.Windows.Forms.ComboBox();
            this.comboMicrocontrollerBox = new System.Windows.Forms.ComboBox();
            this.loopBackBox = new System.Windows.Forms.CheckBox();
            this.LocalIPCombo = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.simulationSettingsGroupbox = new System.Windows.Forms.GroupBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txtWSCOMPort = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.portGroupbox = new System.Windows.Forms.GroupBox();
            this.txtMcuCOMPort = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.createWSButton = new System.Windows.Forms.Button();
            this.acceptSettings = new System.Windows.Forms.Button();
            this.startRTGroupbox = new System.Windows.Forms.GroupBox();
            this.helpButton = new System.Windows.Forms.Button();
            this.ProdcheckBox = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.simulationSettingsGroupbox.SuspendLayout();
            this.portGroupbox.SuspendLayout();
            this.startRTGroupbox.SuspendLayout();
            this.SuspendLayout();
            // 
            // startButton
            // 
            this.startButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.startButton.BackColor = System.Drawing.Color.LimeGreen;
            this.startButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.startButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F);
            this.startButton.ForeColor = System.Drawing.Color.Black;
            this.startButton.Location = new System.Drawing.Point(263, 84);
            this.startButton.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.startButton.Name = "startButton";
            this.startButton.Size = new System.Drawing.Size(227, 49);
            this.startButton.TabIndex = 6;
            this.startButton.Text = "Start RT";
            this.startButton.UseVisualStyleBackColor = false;
            this.startButton.Click += new System.EventHandler(this.startButton_Click);
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToDeleteRows = false;
            this.dataGridView1.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridView1.BackgroundColor = System.Drawing.Color.Gainsboro;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.GridColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.dataGridView1.Location = new System.Drawing.Point(15, 27);
            this.dataGridView1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.AutoSizeToAllHeaders;
            this.dataGridView1.RowTemplate.Height = 24;
            this.dataGridView1.RowTemplate.ReadOnly = true;
            this.dataGridView1.Size = new System.Drawing.Size(679, 283);
            this.dataGridView1.TabIndex = 7;
            this.dataGridView1.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellContentClick);
            // 
            // shutdownButton
            // 
            this.shutdownButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.shutdownButton.BackColor = System.Drawing.Color.Red;
            this.shutdownButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.shutdownButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F);
            this.shutdownButton.Location = new System.Drawing.Point(17, 84);
            this.shutdownButton.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.shutdownButton.Name = "shutdownButton";
            this.shutdownButton.Size = new System.Drawing.Size(227, 49);
            this.shutdownButton.TabIndex = 7;
            this.shutdownButton.Text = "Shutdown RT";
            this.shutdownButton.UseVisualStyleBackColor = false;
            this.shutdownButton.Click += new System.EventHandler(this.button2_Click);
            // 
            // txtPLCPort
            // 
            this.txtPLCPort.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.txtPLCPort.BackColor = System.Drawing.Color.Gainsboro;
            this.txtPLCPort.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F);
            this.txtPLCPort.Location = new System.Drawing.Point(321, 71);
            this.txtPLCPort.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.txtPLCPort.Name = "txtPLCPort";
            this.txtPLCPort.Size = new System.Drawing.Size(141, 34);
            this.txtPLCPort.TabIndex = 5;
            this.txtPLCPort.TextChanged += new System.EventHandler(this.txtPLCPort_TextChanged);
            this.txtPLCPort.GotFocus += new System.EventHandler(this.textBox1_Focus);
            // 
            // comboBox1
            // 
            this.comboBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.comboBox1.BackColor = System.Drawing.Color.WhiteSmoke;
            this.comboBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Items.AddRange(new object[] {
            "Production SpectraCyber",
            "Simulated SpectraCyber"});
            this.comboBox1.Location = new System.Drawing.Point(347, 37);
            this.comboBox1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(311, 33);
            this.comboBox1.TabIndex = 2;
            this.comboBox1.Text = "Simulated SpectraCyber";
            // 
            // txtPLCIP
            // 
            this.txtPLCIP.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.txtPLCIP.BackColor = System.Drawing.Color.Gainsboro;
            this.txtPLCIP.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F);
            this.txtPLCIP.Location = new System.Drawing.Point(321, 31);
            this.txtPLCIP.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.txtPLCIP.Name = "txtPLCIP";
            this.txtPLCIP.Size = new System.Drawing.Size(141, 34);
            this.txtPLCIP.TabIndex = 4;
            this.txtPLCIP.TextChanged += new System.EventHandler(this.txtPLCIP_TextChanged);
            this.txtPLCIP.GotFocus += new System.EventHandler(this.textBox2_Focus);
            // 
            // comboBox2
            // 
            this.comboBox2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.comboBox2.BackColor = System.Drawing.Color.WhiteSmoke;
            this.comboBox2.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.comboBox2.FormattingEnabled = true;
            this.comboBox2.Items.AddRange(new object[] {
            "Production Weather Station",
            "Simulated Weather Station",
            "Test Weather Station"});
            this.comboBox2.Location = new System.Drawing.Point(7, 94);
            this.comboBox2.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.comboBox2.Name = "comboBox2";
            this.comboBox2.Size = new System.Drawing.Size(328, 33);
            this.comboBox2.TabIndex = 1;
            this.comboBox2.Text = "Simulated Weather Station";
            // 
            // checkBox1
            // 
            this.checkBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.checkBox1.AutoSize = true;
            this.checkBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.checkBox1.Location = new System.Drawing.Point(740, 272);
            this.checkBox1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(182, 21);
            this.checkBox1.TabIndex = 0;
            this.checkBox1.Text = "Populate local database";
            this.checkBox1.UseVisualStyleBackColor = true;
            // 
            // comboPLCType
            // 
            this.comboPLCType.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.comboPLCType.BackColor = System.Drawing.Color.WhiteSmoke;
            this.comboPLCType.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.comboPLCType.FormattingEnabled = true;
            this.comboPLCType.Items.AddRange(new object[] {
            "Production PLC",
            "Scale PLC",
            "Simulated PLC",
            "Test PLC"});
            this.comboPLCType.Location = new System.Drawing.Point(347, 94);
            this.comboPLCType.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.comboPLCType.Name = "comboPLCType";
            this.comboPLCType.Size = new System.Drawing.Size(311, 33);
            this.comboPLCType.TabIndex = 3;
            this.comboPLCType.Text = "Simulated PLC";
            // 
            // FreeControl
            // 
            this.FreeControl.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.FreeControl.BackColor = System.Drawing.Color.LightGray;
            this.FreeControl.Enabled = false;
            this.FreeControl.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.FreeControl.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F);
            this.FreeControl.Location = new System.Drawing.Point(17, 18);
            this.FreeControl.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.FreeControl.Name = "FreeControl";
            this.FreeControl.Size = new System.Drawing.Size(472, 54);
            this.FreeControl.TabIndex = 9;
            this.FreeControl.Text = "Radio Telescope Control";
            this.FreeControl.UseVisualStyleBackColor = false;
            this.FreeControl.Click += new System.EventHandler(this.FreeControl_Click);
            // 
            // comboEncoderType
            // 
            this.comboEncoderType.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.comboEncoderType.BackColor = System.Drawing.Color.WhiteSmoke;
            this.comboEncoderType.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.comboEncoderType.FormattingEnabled = true;
            this.comboEncoderType.Items.AddRange(new object[] {
            "Production Absolute Encoder",
            "Simulated Absolute Encoder",
            "Test Absolute Encoder"});
            this.comboEncoderType.Location = new System.Drawing.Point(8, 151);
            this.comboEncoderType.Margin = new System.Windows.Forms.Padding(4);
            this.comboEncoderType.Name = "comboEncoderType";
            this.comboEncoderType.Size = new System.Drawing.Size(327, 33);
            this.comboEncoderType.TabIndex = 12;
            this.comboEncoderType.Text = "Simulated Absolute Encoder";
            // 
            // comboMicrocontrollerBox
            // 
            this.comboMicrocontrollerBox.BackColor = System.Drawing.Color.WhiteSmoke;
            this.comboMicrocontrollerBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.comboMicrocontrollerBox.FormattingEnabled = true;
            this.comboMicrocontrollerBox.Items.AddRange(new object[] {
            "Production Microcontroller",
            "Simulated Microcontroller"});
            this.comboMicrocontrollerBox.Location = new System.Drawing.Point(8, 37);
            this.comboMicrocontrollerBox.Margin = new System.Windows.Forms.Padding(4);
            this.comboMicrocontrollerBox.Name = "comboMicrocontrollerBox";
            this.comboMicrocontrollerBox.Size = new System.Drawing.Size(327, 33);
            this.comboMicrocontrollerBox.TabIndex = 14;
            this.comboMicrocontrollerBox.Text = "Simulated Microcontroller";
            this.comboMicrocontrollerBox.SelectedIndexChanged += new System.EventHandler(this.comboMicrocontrollerBox_SelectedIndexChanged);
            // 
            // loopBackBox
            // 
            this.loopBackBox.AutoSize = true;
            this.loopBackBox.Location = new System.Drawing.Point(928, 255);
            this.loopBackBox.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.loopBackBox.Name = "loopBackBox";
            this.loopBackBox.Size = new System.Drawing.Size(124, 55);
            this.loopBackBox.TabIndex = 15;
            this.loopBackBox.Text = "Loop back \r\n(for simulation)\r\n ";
            this.loopBackBox.UseVisualStyleBackColor = true;
            this.loopBackBox.CheckedChanged += new System.EventHandler(this.loopBackBox_CheckedChanged);
            // 
            // LocalIPCombo
            // 
            this.LocalIPCombo.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.LocalIPCombo.BackColor = System.Drawing.Color.WhiteSmoke;
            this.LocalIPCombo.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LocalIPCombo.FormattingEnabled = true;
            this.LocalIPCombo.Items.AddRange(new object[] {
            "127.0.0.1"});
            this.LocalIPCombo.Location = new System.Drawing.Point(347, 151);
            this.LocalIPCombo.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.LocalIPCombo.Name = "LocalIPCombo";
            this.LocalIPCombo.Size = new System.Drawing.Size(311, 33);
            this.LocalIPCombo.TabIndex = 16;
            this.LocalIPCombo.Text = "RT IP Address";
            this.LocalIPCombo.SelectedIndexChanged += new System.EventHandler(this.LocalIPCombo_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(359, 17);
            this.label1.TabIndex = 17;
            this.label1.Text = "Click on the IP adress of the RT to open diagnostic form";
            // 
            // simulationSettingsGroupbox
            // 
            this.simulationSettingsGroupbox.BackColor = System.Drawing.Color.Gray;
            this.simulationSettingsGroupbox.Controls.Add(this.comboBox2);
            this.simulationSettingsGroupbox.Controls.Add(this.LocalIPCombo);
            this.simulationSettingsGroupbox.Controls.Add(this.comboMicrocontrollerBox);
            this.simulationSettingsGroupbox.Controls.Add(this.comboEncoderType);
            this.simulationSettingsGroupbox.Controls.Add(this.comboPLCType);
            this.simulationSettingsGroupbox.Controls.Add(this.comboBox1);
            this.simulationSettingsGroupbox.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.simulationSettingsGroupbox.Location = new System.Drawing.Point(16, 318);
            this.simulationSettingsGroupbox.Margin = new System.Windows.Forms.Padding(4);
            this.simulationSettingsGroupbox.Name = "simulationSettingsGroupbox";
            this.simulationSettingsGroupbox.Padding = new System.Windows.Forms.Padding(4);
            this.simulationSettingsGroupbox.Size = new System.Drawing.Size(665, 209);
            this.simulationSettingsGroupbox.TabIndex = 18;
            this.simulationSettingsGroupbox.TabStop = false;
            this.simulationSettingsGroupbox.Text = "Individual Component Simulation settings";
            this.simulationSettingsGroupbox.Enter += new System.EventHandler(this.simulationSettingsGroupbox_Enter);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(9, 162);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(255, 24);
            this.label2.TabIndex = 17;
            this.label2.Text = "Weather station COM port:";
            this.label2.Click += new System.EventHandler(this.label2_Click);
            // 
            // txtWSCOMPort
            // 
            this.txtWSCOMPort.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.txtWSCOMPort.BackColor = System.Drawing.Color.Gainsboro;
            this.txtWSCOMPort.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F);
            this.txtWSCOMPort.Location = new System.Drawing.Point(321, 153);
            this.txtWSCOMPort.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.txtWSCOMPort.Name = "txtWSCOMPort";
            this.txtWSCOMPort.Size = new System.Drawing.Size(141, 34);
            this.txtWSCOMPort.TabIndex = 22;
            this.txtWSCOMPort.TextChanged += new System.EventHandler(this.txtWSCOMPort_TextChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(3, 71);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(102, 24);
            this.label3.TabIndex = 19;
            this.label3.Text = " PLC port:";
            this.label3.Click += new System.EventHandler(this.label3_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(8, 31);
            this.label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(168, 24);
            this.label4.TabIndex = 20;
            this.label4.Text = "MCU IP Address:";
            this.label4.Click += new System.EventHandler(this.label4_Click);
            // 
            // portGroupbox
            // 
            this.portGroupbox.Controls.Add(this.txtMcuCOMPort);
            this.portGroupbox.Controls.Add(this.label5);
            this.portGroupbox.Controls.Add(this.txtPLCPort);
            this.portGroupbox.Controls.Add(this.label4);
            this.portGroupbox.Controls.Add(this.txtPLCIP);
            this.portGroupbox.Controls.Add(this.label3);
            this.portGroupbox.Controls.Add(this.label2);
            this.portGroupbox.Controls.Add(this.txtWSCOMPort);
            this.portGroupbox.Location = new System.Drawing.Point(727, 50);
            this.portGroupbox.Margin = new System.Windows.Forms.Padding(4);
            this.portGroupbox.Name = "portGroupbox";
            this.portGroupbox.Padding = new System.Windows.Forms.Padding(4);
            this.portGroupbox.Size = new System.Drawing.Size(472, 202);
            this.portGroupbox.TabIndex = 21;
            this.portGroupbox.TabStop = false;
            this.portGroupbox.Text = "System IP Address and Port Numbers";
            this.portGroupbox.Enter += new System.EventHandler(this.portGroupbox_Enter);
            // 
            // txtMcuCOMPort
            // 
            this.txtMcuCOMPort.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.txtMcuCOMPort.BackColor = System.Drawing.Color.Gainsboro;
            this.txtMcuCOMPort.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F);
            this.txtMcuCOMPort.Location = new System.Drawing.Point(321, 112);
            this.txtMcuCOMPort.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.txtMcuCOMPort.Name = "txtMcuCOMPort";
            this.txtMcuCOMPort.Size = new System.Drawing.Size(141, 34);
            this.txtMcuCOMPort.TabIndex = 18;
            this.txtMcuCOMPort.TextChanged += new System.EventHandler(this.txtMcuCOMPort_TextChanged);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(9, 118);
            this.label5.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(110, 24);
            this.label5.TabIndex = 21;
            this.label5.Text = "MCU Port: ";
            // 
            // createWSButton
            // 
            this.createWSButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.createWSButton.BackColor = System.Drawing.Color.LightGray;
            this.createWSButton.Enabled = false;
            this.createWSButton.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.createWSButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.createWSButton.Location = new System.Drawing.Point(964, 309);
            this.createWSButton.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.createWSButton.Name = "createWSButton";
            this.createWSButton.Size = new System.Drawing.Size(227, 63);
            this.createWSButton.TabIndex = 22;
            this.createWSButton.Text = "Create Production Weather Station";
            this.createWSButton.UseVisualStyleBackColor = false;
            this.createWSButton.Click += new System.EventHandler(this.createWSButton_Click);
            // 
            // acceptSettings
            // 
            this.acceptSettings.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.acceptSettings.BackColor = System.Drawing.Color.LightGray;
            this.acceptSettings.Enabled = false;
            this.acceptSettings.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.acceptSettings.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.acceptSettings.Location = new System.Drawing.Point(719, 309);
            this.acceptSettings.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.acceptSettings.Name = "acceptSettings";
            this.acceptSettings.Size = new System.Drawing.Size(227, 63);
            this.acceptSettings.TabIndex = 23;
            this.acceptSettings.Text = "Finalize settings";
            this.acceptSettings.UseVisualStyleBackColor = false;
            this.acceptSettings.Click += new System.EventHandler(this.acceptSettings_Click);
            // 
            // startRTGroupbox
            // 
            this.startRTGroupbox.Controls.Add(this.FreeControl);
            this.startRTGroupbox.Controls.Add(this.shutdownButton);
            this.startRTGroupbox.Controls.Add(this.startButton);
            this.startRTGroupbox.Location = new System.Drawing.Point(701, 378);
            this.startRTGroupbox.Margin = new System.Windows.Forms.Padding(4);
            this.startRTGroupbox.Name = "startRTGroupbox";
            this.startRTGroupbox.Padding = new System.Windows.Forms.Padding(4);
            this.startRTGroupbox.Size = new System.Drawing.Size(508, 139);
            this.startRTGroupbox.TabIndex = 24;
            this.startRTGroupbox.TabStop = false;
            this.startRTGroupbox.Enter += new System.EventHandler(this.groupBox3_Enter);
            // 
            // helpButton
            // 
            this.helpButton.BackColor = System.Drawing.Color.Gainsboro;
            this.helpButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.helpButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.helpButton.ForeColor = System.Drawing.SystemColors.Desktop;
            this.helpButton.Location = new System.Drawing.Point(1169, 9);
            this.helpButton.Margin = new System.Windows.Forms.Padding(4);
            this.helpButton.Name = "helpButton";
            this.helpButton.Size = new System.Drawing.Size(29, 30);
            this.helpButton.TabIndex = 27;
            this.helpButton.Text = "?";
            this.helpButton.UseVisualStyleBackColor = false;
            this.helpButton.Click += new System.EventHandler(this.helpButton_click);
            // 
            // ProdcheckBox
            // 
            this.ProdcheckBox.AutoSize = true;
            this.ProdcheckBox.Location = new System.Drawing.Point(1061, 255);
            this.ProdcheckBox.Name = "ProdcheckBox";
            this.ProdcheckBox.Size = new System.Drawing.Size(128, 38);
            this.ProdcheckBox.TabIndex = 28;
            this.ProdcheckBox.Text = "Default Vals \r\n(for production)";
            this.ProdcheckBox.UseVisualStyleBackColor = true;
            this.ProdcheckBox.CheckedChanged += new System.EventHandler(this.ProdcheckBox_CheckedChanged);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Gray;
            this.ClientSize = new System.Drawing.Size(1215, 532);
            this.Controls.Add(this.ProdcheckBox);
            this.Controls.Add(this.helpButton);
            this.Controls.Add(this.loopBackBox);
            this.Controls.Add(this.startRTGroupbox);
            this.Controls.Add(this.checkBox1);
            this.Controls.Add(this.acceptSettings);
            this.Controls.Add(this.createWSButton);
            this.Controls.Add(this.portGroupbox);
            this.Controls.Add(this.simulationSettingsGroupbox);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.dataGridView1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.MinimumSize = new System.Drawing.Size(1222, 440);
            this.Name = "MainForm";
            this.Text = "MainForm";
            this.Load += new System.EventHandler(this.MainForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.simulationSettingsGroupbox.ResumeLayout(false);
            this.portGroupbox.ResumeLayout(false);
            this.portGroupbox.PerformLayout();
            this.startRTGroupbox.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button startButton;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.Button shutdownButton;
        private System.Windows.Forms.TextBox txtPLCPort;
        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.TextBox txtPLCIP;
        private System.Windows.Forms.ComboBox comboBox2;
        private System.Windows.Forms.CheckBox checkBox1;
        private System.Windows.Forms.ComboBox comboPLCType;
        private System.Windows.Forms.Button FreeControl;
        private System.Windows.Forms.ComboBox comboEncoderType;
        private System.Windows.Forms.ComboBox comboMicrocontrollerBox;
        private System.Windows.Forms.CheckBox loopBackBox;
        private System.Windows.Forms.ComboBox LocalIPCombo;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox simulationSettingsGroupbox;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtWSCOMPort;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.GroupBox portGroupbox;
        private System.Windows.Forms.TextBox txtMcuCOMPort;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button createWSButton;
        private System.Windows.Forms.Button acceptSettings;
        private System.Windows.Forms.GroupBox startRTGroupbox;
        private System.Windows.Forms.Button helpButton;
        private System.Windows.Forms.CheckBox ProdcheckBox;
    }
}