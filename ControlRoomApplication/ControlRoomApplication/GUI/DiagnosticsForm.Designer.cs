namespace ControlRoomApplication.GUI
{
    partial class DiagnosticsForm
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
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea1 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend1 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series1 = new System.Windows.Forms.DataVisualization.Charting.Series();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.windSpeedLabel = new System.Windows.Forms.Label();
            this.windDirLabel = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.startTimeTextBox = new System.Windows.Forms.TextBox();
            this.endTimeTextBox = new System.Windows.Forms.TextBox();
            this.statusTextBox = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.lblAzimuthTemp = new System.Windows.Forms.Label();
            this.lblElevationTemp = new System.Windows.Forms.Label();
            this.fldAzTemp = new System.Windows.Forms.Label();
            this.fldElTemp = new System.Windows.Forms.Label();
            this.btnTest = new System.Windows.Forms.Button();
            this.selectDemo = new System.Windows.Forms.CheckBox();
            this.lblElLimStatus2 = new System.Windows.Forms.Label();
            this.lblElLimStatus1 = new System.Windows.Forms.Label();
            this.lblAzLimStatus2 = new System.Windows.Forms.Label();
            this.lblAzLimStatus1 = new System.Windows.Forms.Label();
            this.lblElLimit2 = new System.Windows.Forms.Label();
            this.lblElLimit1 = new System.Windows.Forms.Label();
            this.lblAzLimit2 = new System.Windows.Forms.Label();
            this.lblAzLimit1 = new System.Windows.Forms.Label();
            this.lblELHomeStatus = new System.Windows.Forms.Label();
            this.lblAzHomeStatus2 = new System.Windows.Forms.Label();
            this.lblAzHomeStatus1 = new System.Windows.Forms.Label();
            this.lblElHome = new System.Windows.Forms.Label();
            this.lblAzHome2 = new System.Windows.Forms.Label();
            this.lblAzHome1 = new System.Windows.Forms.Label();
            this.lblAbsEncoder = new System.Windows.Forms.Label();
            this.lblEncoderDegrees = new System.Windows.Forms.Label();
            this.lblAzEncoderDegrees = new System.Windows.Forms.Label();
            this.lblEncoderTicks = new System.Windows.Forms.Label();
            this.lblAzEncoderTicks = new System.Windows.Forms.Label();
            this.btnAddOneEncoder = new System.Windows.Forms.Button();
            this.btnAddFiveEncoder = new System.Windows.Forms.Button();
            this.btnAddXEncoder = new System.Windows.Forms.Button();
            this.btnSubtractOneEncoder = new System.Windows.Forms.Button();
            this.btnSubtractFiveEncoder = new System.Windows.Forms.Button();
            this.btnSubtractXEncoder = new System.Windows.Forms.Button();
            this.label9 = new System.Windows.Forms.Label();
            this.txtCustEncoderVal = new System.Windows.Forms.TextBox();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.button4 = new System.Windows.Forms.Button();
            this.button5 = new System.Windows.Forms.Button();
            this.button6 = new System.Windows.Forms.Button();
            this.lblElEncoderTicks = new System.Windows.Forms.Label();
            this.label14 = new System.Windows.Forms.Label();
            this.lblElEncoderDegrees = new System.Windows.Forms.Label();
            this.label16 = new System.Windows.Forms.Label();
            this.label17 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.label13 = new System.Windows.Forms.Label();
            this.label15 = new System.Windows.Forms.Label();
            this.textBox3 = new System.Windows.Forms.TextBox();
            this.textBox4 = new System.Windows.Forms.TextBox();
            this.label18 = new System.Windows.Forms.Label();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.button7 = new System.Windows.Forms.Button();
            this.groupBox9 = new System.Windows.Forms.GroupBox();
            this.groupBox8 = new System.Windows.Forms.GroupBox();
            this.runDiagScriptsButton = new System.Windows.Forms.Button();
            this.diagnosticScriptCombo = new System.Windows.Forms.ComboBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.groupBox14 = new System.Windows.Forms.GroupBox();
            this.farTempConvert = new System.Windows.Forms.Button();
            this.celTempConvert = new System.Windows.Forms.Button();
            this.groupBox6 = new System.Windows.Forms.GroupBox();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.label32 = new System.Windows.Forms.Label();
            this.label34 = new System.Windows.Forms.Label();
            this.lblCurrentAzOrientation = new System.Windows.Forms.Label();
            this.lblCurrentElOrientation = new System.Windows.Forms.Label();
            this.label25 = new System.Windows.Forms.Label();
            this.label26 = new System.Windows.Forms.Label();
            this.AZTempUnitLabel = new System.Windows.Forms.Label();
            this.ElTempUnitLabel = new System.Windows.Forms.Label();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.InsideTempUnits = new System.Windows.Forms.Label();
            this.rainRateUnits = new System.Windows.Forms.Label();
            this.pressUnits = new System.Windows.Forms.Label();
            this.dailyRainfallUnits = new System.Windows.Forms.Label();
            this.outTempUnits = new System.Windows.Forms.Label();
            this.label35 = new System.Windows.Forms.Label();
            this.windSpeedUnits = new System.Windows.Forms.Label();
            this.insideTempLabel = new System.Windows.Forms.Label();
            this.label23 = new System.Windows.Forms.Label();
            this.rainRateLabel = new System.Windows.Forms.Label();
            this.barometricPressureLabel = new System.Windows.Forms.Label();
            this.dailyRainfallLabel = new System.Windows.Forms.Label();
            this.outsideTempLabel = new System.Windows.Forms.Label();
            this.label19 = new System.Windows.Forms.Label();
            this.label20 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.lbGateStat = new System.Windows.Forms.Label();
            this.lbEstopStat = new System.Windows.Forms.Label();
            this.label31 = new System.Windows.Forms.Label();
            this.label30 = new System.Windows.Forms.Label();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.groupBox13 = new System.Windows.Forms.GroupBox();
            this.ElMotTempSensOverride = new System.Windows.Forms.Button();
            this.label29 = new System.Windows.Forms.Label();
            this.AzMotTempSensOverride = new System.Windows.Forms.Button();
            this.label28 = new System.Windows.Forms.Label();
            this.groupBox12 = new System.Windows.Forms.GroupBox();
            this.MGOverride = new System.Windows.Forms.Button();
            this.label27 = new System.Windows.Forms.Label();
            this.groupBox10 = new System.Windows.Forms.GroupBox();
            this.ElevationProximityOveride2 = new System.Windows.Forms.Button();
            this.ElevationProximityOveride1 = new System.Windows.Forms.Button();
            this.ORAzimuthSens2 = new System.Windows.Forms.Button();
            this.ORAzimuthSens1 = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label21 = new System.Windows.Forms.Label();
            this.label22 = new System.Windows.Forms.Label();
            this.groupBox11 = new System.Windows.Forms.GroupBox();
            this.WSOverride = new System.Windows.Forms.Button();
            this.label24 = new System.Windows.Forms.Label();
            this.tabPage4 = new System.Windows.Forms.TabPage();
            this.spectraCyberScanChart = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.lblIntegrationStep = new System.Windows.Forms.Label();
            this.IntegrationStepVal = new System.Windows.Forms.Label();
            this.lblDCGain = new System.Windows.Forms.Label();
            this.DCGainVal = new System.Windows.Forms.Label();
            this.lblIFGain = new System.Windows.Forms.Label();
            this.IFGainVal = new System.Windows.Forms.Label();
            this.lblBandwidth = new System.Windows.Forms.Label();
            this.BandwidthVal = new System.Windows.Forms.Label();
            this.lblOffsetVoltage = new System.Windows.Forms.Label();
            this.OffsetVoltageVal = new System.Windows.Forms.Label();
            this.tabPage5 = new System.Windows.Forms.TabPage();
            this.consoleLogBox = new System.Windows.Forms.TextBox();
            this.lblFrequency = new System.Windows.Forms.Label();
            this.frequencyVal = new System.Windows.Forms.Label();
            this.lblModeType = new System.Windows.Forms.Label();
            this.spectraModeTypeVal = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.groupBox9.SuspendLayout();
            this.groupBox8.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).BeginInit();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.groupBox14.SuspendLayout();
            this.groupBox6.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.groupBox5.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.tabPage3.SuspendLayout();
            this.groupBox13.SuspendLayout();
            this.groupBox12.SuspendLayout();
            this.groupBox10.SuspendLayout();
            this.groupBox11.SuspendLayout();
            this.tabPage4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.spectraCyberScanChart)).BeginInit();
            this.tabPage5.SuspendLayout();
            this.SuspendLayout();
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToDeleteRows = false;
            this.dataGridView1.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dataGridView1.DefaultCellStyle = dataGridViewCellStyle1;
            this.dataGridView1.Location = new System.Drawing.Point(3, 6);
            this.dataGridView1.Margin = new System.Windows.Forms.Padding(2);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowTemplate.Height = 24;
            this.dataGridView1.Size = new System.Drawing.Size(331, 178);
            this.dataGridView1.TabIndex = 0;
            this.dataGridView1.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellContentClick);
            // 
            // windSpeedLabel
            // 
            this.windSpeedLabel.AutoSize = true;
            this.windSpeedLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.windSpeedLabel.Location = new System.Drawing.Point(203, 46);
            this.windSpeedLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.windSpeedLabel.Name = "windSpeedLabel";
            this.windSpeedLabel.Size = new System.Drawing.Size(22, 18);
            this.windSpeedLabel.TabIndex = 3;
            this.windSpeedLabel.Text = " --";
            // 
            // windDirLabel
            // 
            this.windDirLabel.AutoSize = true;
            this.windDirLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.windDirLabel.Location = new System.Drawing.Point(202, 22);
            this.windDirLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.windDirLabel.Name = "windDirLabel";
            this.windDirLabel.Size = new System.Drawing.Size(23, 20);
            this.windDirLabel.TabIndex = 4;
            this.windDirLabel.Text = " --";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(5, 25);
            this.label6.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(73, 17);
            this.label6.TabIndex = 6;
            this.label6.Text = "Start Time";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.Location = new System.Drawing.Point(84, 25);
            this.label7.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(68, 17);
            this.label7.TabIndex = 7;
            this.label7.Text = "End Time";
            // 
            // startTimeTextBox
            // 
            this.startTimeTextBox.Location = new System.Drawing.Point(2, 44);
            this.startTimeTextBox.Margin = new System.Windows.Forms.Padding(2);
            this.startTimeTextBox.Name = "startTimeTextBox";
            this.startTimeTextBox.Size = new System.Drawing.Size(76, 20);
            this.startTimeTextBox.TabIndex = 8;
            // 
            // endTimeTextBox
            // 
            this.endTimeTextBox.Location = new System.Drawing.Point(85, 44);
            this.endTimeTextBox.Margin = new System.Windows.Forms.Padding(2);
            this.endTimeTextBox.Name = "endTimeTextBox";
            this.endTimeTextBox.Size = new System.Drawing.Size(76, 20);
            this.endTimeTextBox.TabIndex = 9;
            // 
            // statusTextBox
            // 
            this.statusTextBox.Location = new System.Drawing.Point(182, 44);
            this.statusTextBox.Margin = new System.Windows.Forms.Padding(2);
            this.statusTextBox.Name = "statusTextBox";
            this.statusTextBox.Size = new System.Drawing.Size(102, 20);
            this.statusTextBox.TabIndex = 10;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label8.Location = new System.Drawing.Point(186, 25);
            this.label8.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(48, 17);
            this.label8.TabIndex = 11;
            this.label8.Text = "Status";
            // 
            // timer1
            // 
            this.timer1.Enabled = true;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // lblAzimuthTemp
            // 
            this.lblAzimuthTemp.AutoSize = true;
            this.lblAzimuthTemp.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblAzimuthTemp.Location = new System.Drawing.Point(10, 9);
            this.lblAzimuthTemp.Name = "lblAzimuthTemp";
            this.lblAzimuthTemp.Size = new System.Drawing.Size(109, 16);
            this.lblAzimuthTemp.TabIndex = 14;
            this.lblAzimuthTemp.Text = "Azimuth Temp:";
            // 
            // lblElevationTemp
            // 
            this.lblElevationTemp.AutoSize = true;
            this.lblElevationTemp.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblElevationTemp.Location = new System.Drawing.Point(11, 31);
            this.lblElevationTemp.Name = "lblElevationTemp";
            this.lblElevationTemp.Size = new System.Drawing.Size(121, 16);
            this.lblElevationTemp.TabIndex = 15;
            this.lblElevationTemp.Text = "Elevation Temp:";
            // 
            // fldAzTemp
            // 
            this.fldAzTemp.AutoSize = true;
            this.fldAzTemp.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.fldAzTemp.Location = new System.Drawing.Point(212, 7);
            this.fldAzTemp.Name = "fldAzTemp";
            this.fldAzTemp.Size = new System.Drawing.Size(24, 18);
            this.fldAzTemp.TabIndex = 18;
            this.fldAzTemp.Text = "50";
            // 
            // fldElTemp
            // 
            this.fldElTemp.AutoSize = true;
            this.fldElTemp.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.fldElTemp.Location = new System.Drawing.Point(212, 34);
            this.fldElTemp.Name = "fldElTemp";
            this.fldElTemp.Size = new System.Drawing.Size(24, 18);
            this.fldElTemp.TabIndex = 19;
            this.fldElTemp.Text = "50";
            // 
            // btnTest
            // 
            this.btnTest.BackColor = System.Drawing.Color.LimeGreen;
            this.btnTest.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnTest.Location = new System.Drawing.Point(115, 16);
            this.btnTest.Name = "btnTest";
            this.btnTest.Size = new System.Drawing.Size(69, 27);
            this.btnTest.TabIndex = 21;
            this.btnTest.Text = "Test ";
            this.btnTest.UseVisualStyleBackColor = false;
            this.btnTest.Click += new System.EventHandler(this.btnTest_Click);
            // 
            // selectDemo
            // 
            this.selectDemo.AutoSize = true;
            this.selectDemo.Location = new System.Drawing.Point(13, 22);
            this.selectDemo.Name = "selectDemo";
            this.selectDemo.Size = new System.Drawing.Size(77, 17);
            this.selectDemo.TabIndex = 29;
            this.selectDemo.Text = "Run Demo";
            this.selectDemo.UseVisualStyleBackColor = true;
            // 
            // lblElLimStatus2
            // 
            this.lblElLimStatus2.AutoSize = true;
            this.lblElLimStatus2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblElLimStatus2.Location = new System.Drawing.Point(223, 293);
            this.lblElLimStatus2.Name = "lblElLimStatus2";
            this.lblElLimStatus2.Size = new System.Drawing.Size(56, 15);
            this.lblElLimStatus2.TabIndex = 17;
            this.lblElLimStatus2.Text = "Inactive";
            // 
            // lblElLimStatus1
            // 
            this.lblElLimStatus1.AutoSize = true;
            this.lblElLimStatus1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblElLimStatus1.Location = new System.Drawing.Point(223, 241);
            this.lblElLimStatus1.Name = "lblElLimStatus1";
            this.lblElLimStatus1.Size = new System.Drawing.Size(56, 15);
            this.lblElLimStatus1.TabIndex = 16;
            this.lblElLimStatus1.Text = "Inactive";
            // 
            // lblAzLimStatus2
            // 
            this.lblAzLimStatus2.AutoSize = true;
            this.lblAzLimStatus2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblAzLimStatus2.Location = new System.Drawing.Point(223, 192);
            this.lblAzLimStatus2.Name = "lblAzLimStatus2";
            this.lblAzLimStatus2.Size = new System.Drawing.Size(56, 15);
            this.lblAzLimStatus2.TabIndex = 15;
            this.lblAzLimStatus2.Text = "Inactive";
            // 
            // lblAzLimStatus1
            // 
            this.lblAzLimStatus1.AutoSize = true;
            this.lblAzLimStatus1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblAzLimStatus1.Location = new System.Drawing.Point(223, 143);
            this.lblAzLimStatus1.Name = "lblAzLimStatus1";
            this.lblAzLimStatus1.Size = new System.Drawing.Size(56, 15);
            this.lblAzLimStatus1.TabIndex = 14;
            this.lblAzLimStatus1.Text = "Inactive";
            // 
            // lblElLimit2
            // 
            this.lblElLimit2.AutoSize = true;
            this.lblElLimit2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblElLimit2.Location = new System.Drawing.Point(9, 293);
            this.lblElLimit2.Name = "lblElLimit2";
            this.lblElLimit2.Size = new System.Drawing.Size(160, 15);
            this.lblElLimit2.TabIndex = 13;
            this.lblElLimit2.Text = "Elevation Limit Switch 2";
            // 
            // lblElLimit1
            // 
            this.lblElLimit1.AutoSize = true;
            this.lblElLimit1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblElLimit1.Location = new System.Drawing.Point(9, 241);
            this.lblElLimit1.Name = "lblElLimit1";
            this.lblElLimit1.Size = new System.Drawing.Size(160, 15);
            this.lblElLimit1.TabIndex = 12;
            this.lblElLimit1.Text = "Elevation Limit Switch 1";
            // 
            // lblAzLimit2
            // 
            this.lblAzLimit2.AutoSize = true;
            this.lblAzLimit2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblAzLimit2.Location = new System.Drawing.Point(9, 192);
            this.lblAzLimit2.Name = "lblAzLimit2";
            this.lblAzLimit2.Size = new System.Drawing.Size(152, 15);
            this.lblAzLimit2.TabIndex = 11;
            this.lblAzLimit2.Text = "Azimuth Limit Switch 2";
            // 
            // lblAzLimit1
            // 
            this.lblAzLimit1.AutoSize = true;
            this.lblAzLimit1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblAzLimit1.Location = new System.Drawing.Point(9, 143);
            this.lblAzLimit1.Name = "lblAzLimit1";
            this.lblAzLimit1.Size = new System.Drawing.Size(152, 15);
            this.lblAzLimit1.TabIndex = 10;
            this.lblAzLimit1.Text = "Azimuth Limit Switch 1";
            // 
            // lblELHomeStatus
            // 
            this.lblELHomeStatus.AutoSize = true;
            this.lblELHomeStatus.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblELHomeStatus.Location = new System.Drawing.Point(220, 97);
            this.lblELHomeStatus.Name = "lblELHomeStatus";
            this.lblELHomeStatus.Size = new System.Drawing.Size(56, 15);
            this.lblELHomeStatus.TabIndex = 5;
            this.lblELHomeStatus.Text = "Inactive";
            // 
            // lblAzHomeStatus2
            // 
            this.lblAzHomeStatus2.AutoSize = true;
            this.lblAzHomeStatus2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblAzHomeStatus2.Location = new System.Drawing.Point(220, 57);
            this.lblAzHomeStatus2.Name = "lblAzHomeStatus2";
            this.lblAzHomeStatus2.Size = new System.Drawing.Size(56, 15);
            this.lblAzHomeStatus2.TabIndex = 4;
            this.lblAzHomeStatus2.Text = "Inactive";
            // 
            // lblAzHomeStatus1
            // 
            this.lblAzHomeStatus1.AutoSize = true;
            this.lblAzHomeStatus1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblAzHomeStatus1.Location = new System.Drawing.Point(220, 16);
            this.lblAzHomeStatus1.Name = "lblAzHomeStatus1";
            this.lblAzHomeStatus1.Size = new System.Drawing.Size(56, 15);
            this.lblAzHomeStatus1.TabIndex = 3;
            this.lblAzHomeStatus1.Text = "Inactive";
            // 
            // lblElHome
            // 
            this.lblElHome.AutoSize = true;
            this.lblElHome.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblElHome.Location = new System.Drawing.Point(6, 97);
            this.lblElHome.Name = "lblElHome";
            this.lblElHome.Size = new System.Drawing.Size(157, 15);
            this.lblElHome.TabIndex = 2;
            this.lblElHome.Text = "Elevation Home Sensor";
            // 
            // lblAzHome2
            // 
            this.lblAzHome2.AutoSize = true;
            this.lblAzHome2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblAzHome2.Location = new System.Drawing.Point(6, 57);
            this.lblAzHome2.Name = "lblAzHome2";
            this.lblAzHome2.Size = new System.Drawing.Size(161, 15);
            this.lblAzHome2.TabIndex = 1;
            this.lblAzHome2.Text = "Azimuth Home Sensor 2";
            // 
            // lblAzHome1
            // 
            this.lblAzHome1.AutoSize = true;
            this.lblAzHome1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblAzHome1.Location = new System.Drawing.Point(6, 16);
            this.lblAzHome1.Name = "lblAzHome1";
            this.lblAzHome1.Size = new System.Drawing.Size(154, 15);
            this.lblAzHome1.TabIndex = 0;
            this.lblAzHome1.Text = "Azimuth Home Senor 1";
            // 
            // lblAbsEncoder
            // 
            this.lblAbsEncoder.AutoSize = true;
            this.lblAbsEncoder.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblAbsEncoder.Location = new System.Drawing.Point(12, 2);
            this.lblAbsEncoder.Name = "lblAbsEncoder";
            this.lblAbsEncoder.Size = new System.Drawing.Size(115, 15);
            this.lblAbsEncoder.TabIndex = 36;
            this.lblAbsEncoder.Text = "Azimuth Encoder";
            // 
            // lblEncoderDegrees
            // 
            this.lblEncoderDegrees.AutoSize = true;
            this.lblEncoderDegrees.Location = new System.Drawing.Point(13, 29);
            this.lblEncoderDegrees.Name = "lblEncoderDegrees";
            this.lblEncoderDegrees.Size = new System.Drawing.Size(47, 13);
            this.lblEncoderDegrees.TabIndex = 37;
            this.lblEncoderDegrees.Text = "Degrees";
            // 
            // lblAzEncoderDegrees
            // 
            this.lblAzEncoderDegrees.AutoSize = true;
            this.lblAzEncoderDegrees.Location = new System.Drawing.Point(77, 29);
            this.lblAzEncoderDegrees.Name = "lblAzEncoderDegrees";
            this.lblAzEncoderDegrees.Size = new System.Drawing.Size(13, 13);
            this.lblAzEncoderDegrees.TabIndex = 38;
            this.lblAzEncoderDegrees.Text = "0";
            // 
            // lblEncoderTicks
            // 
            this.lblEncoderTicks.AutoSize = true;
            this.lblEncoderTicks.Location = new System.Drawing.Point(12, 52);
            this.lblEncoderTicks.Name = "lblEncoderTicks";
            this.lblEncoderTicks.Size = new System.Drawing.Size(33, 13);
            this.lblEncoderTicks.TabIndex = 39;
            this.lblEncoderTicks.Text = "Ticks";
            // 
            // lblAzEncoderTicks
            // 
            this.lblAzEncoderTicks.AutoSize = true;
            this.lblAzEncoderTicks.Location = new System.Drawing.Point(77, 52);
            this.lblAzEncoderTicks.Name = "lblAzEncoderTicks";
            this.lblAzEncoderTicks.Size = new System.Drawing.Size(13, 13);
            this.lblAzEncoderTicks.TabIndex = 40;
            this.lblAzEncoderTicks.Text = "0";
            // 
            // btnAddOneEncoder
            // 
            this.btnAddOneEncoder.BackColor = System.Drawing.Color.DarkGray;
            this.btnAddOneEncoder.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnAddOneEncoder.Location = new System.Drawing.Point(113, 23);
            this.btnAddOneEncoder.Margin = new System.Windows.Forms.Padding(2);
            this.btnAddOneEncoder.Name = "btnAddOneEncoder";
            this.btnAddOneEncoder.Size = new System.Drawing.Size(33, 24);
            this.btnAddOneEncoder.TabIndex = 41;
            this.btnAddOneEncoder.Text = "+1";
            this.btnAddOneEncoder.UseVisualStyleBackColor = false;
            this.btnAddOneEncoder.Click += new System.EventHandler(this.button4_Click);
            // 
            // btnAddFiveEncoder
            // 
            this.btnAddFiveEncoder.BackColor = System.Drawing.Color.DarkGray;
            this.btnAddFiveEncoder.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnAddFiveEncoder.Location = new System.Drawing.Point(162, 23);
            this.btnAddFiveEncoder.Margin = new System.Windows.Forms.Padding(2);
            this.btnAddFiveEncoder.Name = "btnAddFiveEncoder";
            this.btnAddFiveEncoder.Size = new System.Drawing.Size(33, 24);
            this.btnAddFiveEncoder.TabIndex = 42;
            this.btnAddFiveEncoder.Text = "+5";
            this.btnAddFiveEncoder.UseVisualStyleBackColor = false;
            this.btnAddFiveEncoder.Click += new System.EventHandler(this.button5_Click);
            // 
            // btnAddXEncoder
            // 
            this.btnAddXEncoder.BackColor = System.Drawing.Color.DarkGray;
            this.btnAddXEncoder.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnAddXEncoder.Location = new System.Drawing.Point(209, 23);
            this.btnAddXEncoder.Margin = new System.Windows.Forms.Padding(2);
            this.btnAddXEncoder.Name = "btnAddXEncoder";
            this.btnAddXEncoder.Size = new System.Drawing.Size(33, 24);
            this.btnAddXEncoder.TabIndex = 43;
            this.btnAddXEncoder.Text = "+X";
            this.btnAddXEncoder.UseVisualStyleBackColor = false;
            this.btnAddXEncoder.Click += new System.EventHandler(this.btnAddXEncoder_Click);
            // 
            // btnSubtractOneEncoder
            // 
            this.btnSubtractOneEncoder.BackColor = System.Drawing.Color.DarkGray;
            this.btnSubtractOneEncoder.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnSubtractOneEncoder.Location = new System.Drawing.Point(114, 53);
            this.btnSubtractOneEncoder.Margin = new System.Windows.Forms.Padding(2);
            this.btnSubtractOneEncoder.Name = "btnSubtractOneEncoder";
            this.btnSubtractOneEncoder.Size = new System.Drawing.Size(33, 24);
            this.btnSubtractOneEncoder.TabIndex = 44;
            this.btnSubtractOneEncoder.Text = "-1";
            this.btnSubtractOneEncoder.UseVisualStyleBackColor = false;
            this.btnSubtractOneEncoder.Click += new System.EventHandler(this.btnSubtractOneEncoder_Click);
            // 
            // btnSubtractFiveEncoder
            // 
            this.btnSubtractFiveEncoder.BackColor = System.Drawing.Color.DarkGray;
            this.btnSubtractFiveEncoder.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnSubtractFiveEncoder.Location = new System.Drawing.Point(162, 53);
            this.btnSubtractFiveEncoder.Margin = new System.Windows.Forms.Padding(2);
            this.btnSubtractFiveEncoder.Name = "btnSubtractFiveEncoder";
            this.btnSubtractFiveEncoder.Size = new System.Drawing.Size(33, 24);
            this.btnSubtractFiveEncoder.TabIndex = 45;
            this.btnSubtractFiveEncoder.Text = "-5";
            this.btnSubtractFiveEncoder.UseVisualStyleBackColor = false;
            this.btnSubtractFiveEncoder.Click += new System.EventHandler(this.btnSubtractFiveEncoder_Click);
            // 
            // btnSubtractXEncoder
            // 
            this.btnSubtractXEncoder.BackColor = System.Drawing.Color.DarkGray;
            this.btnSubtractXEncoder.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnSubtractXEncoder.Location = new System.Drawing.Point(209, 53);
            this.btnSubtractXEncoder.Margin = new System.Windows.Forms.Padding(2);
            this.btnSubtractXEncoder.Name = "btnSubtractXEncoder";
            this.btnSubtractXEncoder.Size = new System.Drawing.Size(33, 24);
            this.btnSubtractXEncoder.TabIndex = 46;
            this.btnSubtractXEncoder.Text = "-X";
            this.btnSubtractXEncoder.UseVisualStyleBackColor = false;
            this.btnSubtractXEncoder.Click += new System.EventHandler(this.btnSubtractXEncoder_Click);
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(265, 18);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(72, 13);
            this.label9.TabIndex = 47;
            this.label9.Text = "Custom Value";
            // 
            // txtCustEncoderVal
            // 
            this.txtCustEncoderVal.Location = new System.Drawing.Point(268, 48);
            this.txtCustEncoderVal.Name = "txtCustEncoderVal";
            this.txtCustEncoderVal.Size = new System.Drawing.Size(51, 20);
            this.txtCustEncoderVal.TabIndex = 48;
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(269, 44);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(51, 20);
            this.textBox1.TabIndex = 56;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(266, 14);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(72, 13);
            this.label10.TabIndex = 55;
            this.label10.Text = "Custom Value";
            // 
            // button1
            // 
            this.button1.BackColor = System.Drawing.Color.DarkGray;
            this.button1.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.button1.Location = new System.Drawing.Point(209, 57);
            this.button1.Margin = new System.Windows.Forms.Padding(2);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(33, 24);
            this.button1.TabIndex = 54;
            this.button1.Text = "-X";
            this.button1.UseVisualStyleBackColor = false;
            // 
            // button2
            // 
            this.button2.BackColor = System.Drawing.Color.DarkGray;
            this.button2.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.button2.Location = new System.Drawing.Point(162, 57);
            this.button2.Margin = new System.Windows.Forms.Padding(2);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(33, 24);
            this.button2.TabIndex = 53;
            this.button2.Text = "-5";
            this.button2.UseVisualStyleBackColor = false;
            // 
            // button3
            // 
            this.button3.BackColor = System.Drawing.Color.DarkGray;
            this.button3.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.button3.Location = new System.Drawing.Point(112, 57);
            this.button3.Margin = new System.Windows.Forms.Padding(2);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(33, 24);
            this.button3.TabIndex = 52;
            this.button3.Text = "-1";
            this.button3.UseVisualStyleBackColor = false;
            // 
            // button4
            // 
            this.button4.BackColor = System.Drawing.Color.DarkGray;
            this.button4.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.button4.Location = new System.Drawing.Point(209, 25);
            this.button4.Margin = new System.Windows.Forms.Padding(2);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(33, 24);
            this.button4.TabIndex = 51;
            this.button4.Text = "+X";
            this.button4.UseVisualStyleBackColor = false;
            // 
            // button5
            // 
            this.button5.BackColor = System.Drawing.Color.DarkGray;
            this.button5.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.button5.Location = new System.Drawing.Point(162, 25);
            this.button5.Margin = new System.Windows.Forms.Padding(2);
            this.button5.Name = "button5";
            this.button5.Size = new System.Drawing.Size(33, 24);
            this.button5.TabIndex = 50;
            this.button5.Text = "+5";
            this.button5.UseVisualStyleBackColor = false;
            // 
            // button6
            // 
            this.button6.BackColor = System.Drawing.Color.DarkGray;
            this.button6.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.button6.Location = new System.Drawing.Point(113, 25);
            this.button6.Margin = new System.Windows.Forms.Padding(2);
            this.button6.Name = "button6";
            this.button6.Size = new System.Drawing.Size(33, 24);
            this.button6.TabIndex = 49;
            this.button6.Text = "+1";
            this.button6.UseVisualStyleBackColor = false;
            // 
            // lblElEncoderTicks
            // 
            this.lblElEncoderTicks.AutoSize = true;
            this.lblElEncoderTicks.Location = new System.Drawing.Point(69, 59);
            this.lblElEncoderTicks.Name = "lblElEncoderTicks";
            this.lblElEncoderTicks.Size = new System.Drawing.Size(13, 13);
            this.lblElEncoderTicks.TabIndex = 61;
            this.lblElEncoderTicks.Text = "0";
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(10, 59);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(33, 13);
            this.label14.TabIndex = 60;
            this.label14.Text = "Ticks";
            // 
            // lblElEncoderDegrees
            // 
            this.lblElEncoderDegrees.AutoSize = true;
            this.lblElEncoderDegrees.Location = new System.Drawing.Point(68, 36);
            this.lblElEncoderDegrees.Name = "lblElEncoderDegrees";
            this.lblElEncoderDegrees.Size = new System.Drawing.Size(13, 13);
            this.lblElEncoderDegrees.TabIndex = 59;
            this.lblElEncoderDegrees.Text = "0";
            this.lblElEncoderDegrees.Click += new System.EventHandler(this.lblElEncoderDegrees_Click);
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Location = new System.Drawing.Point(10, 36);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(47, 13);
            this.label16.TabIndex = 58;
            this.label16.Text = "Degrees";
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label17.Location = new System.Drawing.Point(4, 9);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(123, 15);
            this.label17.TabIndex = 57;
            this.label17.Text = "Elevation Encoder";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(3, 105);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(101, 13);
            this.label12.TabIndex = 62;
            this.label12.Text = "Set Bits of Precision";
            // 
            // textBox2
            // 
            this.textBox2.Location = new System.Drawing.Point(137, 42);
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new System.Drawing.Size(39, 20);
            this.textBox2.TabIndex = 63;
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(3, 42);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(48, 13);
            this.label13.TabIndex = 64;
            this.label13.Text = "Set Error";
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Location = new System.Drawing.Point(1158, 74);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(0, 13);
            this.label15.TabIndex = 65;
            // 
            // textBox3
            // 
            this.textBox3.Location = new System.Drawing.Point(137, 72);
            this.textBox3.Name = "textBox3";
            this.textBox3.Size = new System.Drawing.Size(40, 20);
            this.textBox3.TabIndex = 66;
            // 
            // textBox4
            // 
            this.textBox4.Location = new System.Drawing.Point(138, 102);
            this.textBox4.Name = "textBox4";
            this.textBox4.Size = new System.Drawing.Size(39, 20);
            this.textBox4.TabIndex = 67;
            // 
            // label18
            // 
            this.label18.AutoSize = true;
            this.label18.Location = new System.Drawing.Point(3, 72);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(63, 13);
            this.label18.TabIndex = 68;
            this.label18.Text = "Set Position";
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Controls.Add(this.tabPage3);
            this.tabControl1.Controls.Add(this.tabPage4);
            this.tabControl1.Controls.Add(this.tabPage5);
            this.tabControl1.Location = new System.Drawing.Point(2, 11);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(653, 437);
            this.tabControl1.TabIndex = 74;
            // 
            // tabPage1
            // 
            this.tabPage1.BackColor = System.Drawing.Color.Gray;
            this.tabPage1.Controls.Add(this.button7);
            this.tabPage1.Controls.Add(this.groupBox9);
            this.tabPage1.Controls.Add(this.groupBox8);
            this.tabPage1.Controls.Add(this.groupBox3);
            this.tabPage1.Controls.Add(this.groupBox2);
            this.tabPage1.Controls.Add(this.groupBox1);
            this.tabPage1.Controls.Add(this.dataGridView1);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(2);
            this.tabPage1.Size = new System.Drawing.Size(645, 411);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Appointment Control";
            // 
            // button7
            // 
            this.button7.BackColor = System.Drawing.Color.Gainsboro;
            this.button7.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button7.ForeColor = System.Drawing.SystemColors.ControlText;
            this.button7.Location = new System.Drawing.Point(609, 3);
            this.button7.Name = "button7";
            this.button7.Size = new System.Drawing.Size(22, 22);
            this.button7.TabIndex = 78;
            this.button7.Text = "?";
            this.button7.UseVisualStyleBackColor = false;
            this.button7.Click += new System.EventHandler(this.button7_Click);
            // 
            // groupBox9
            // 
            this.groupBox9.BackColor = System.Drawing.Color.Gainsboro;
            this.groupBox9.Controls.Add(this.btnTest);
            this.groupBox9.Controls.Add(this.selectDemo);
            this.groupBox9.Location = new System.Drawing.Point(416, 339);
            this.groupBox9.Name = "groupBox9";
            this.groupBox9.Size = new System.Drawing.Size(210, 66);
            this.groupBox9.TabIndex = 77;
            this.groupBox9.TabStop = false;
            // 
            // groupBox8
            // 
            this.groupBox8.BackColor = System.Drawing.Color.Gainsboro;
            this.groupBox8.Controls.Add(this.runDiagScriptsButton);
            this.groupBox8.Controls.Add(this.diagnosticScriptCombo);
            this.groupBox8.Location = new System.Drawing.Point(344, 120);
            this.groupBox8.Name = "groupBox8";
            this.groupBox8.Size = new System.Drawing.Size(287, 71);
            this.groupBox8.TabIndex = 76;
            this.groupBox8.TabStop = false;
            this.groupBox8.Text = "Diagnostic Scripts";
            // 
            // runDiagScriptsButton
            // 
            this.runDiagScriptsButton.BackColor = System.Drawing.Color.DarkGray;
            this.runDiagScriptsButton.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.runDiagScriptsButton.Location = new System.Drawing.Point(203, 19);
            this.runDiagScriptsButton.Name = "runDiagScriptsButton";
            this.runDiagScriptsButton.Size = new System.Drawing.Size(75, 24);
            this.runDiagScriptsButton.TabIndex = 24;
            this.runDiagScriptsButton.Text = "Run Script";
            this.runDiagScriptsButton.UseVisualStyleBackColor = false;
            this.runDiagScriptsButton.Click += new System.EventHandler(this.editDiagScriptsButton_Click);
            // 
            // diagnosticScriptCombo
            // 
            this.diagnosticScriptCombo.BackColor = System.Drawing.Color.DarkGray;
            this.diagnosticScriptCombo.FormattingEnabled = true;
            this.diagnosticScriptCombo.Items.AddRange(new object[] {
            "Hit Azimuth Counter Clockwise Limit Switch",
            "Hit Azimuth Clockwise Limit Switch",
            "Hit Elevation Lower Limit Switch",
            "Hit Elevation Upper Limit Switch",
            "Hit Clockwise",
            "Hit Counter Clockwise Hardstop"});
            this.diagnosticScriptCombo.Location = new System.Drawing.Point(6, 22);
            this.diagnosticScriptCombo.Name = "diagnosticScriptCombo";
            this.diagnosticScriptCombo.Size = new System.Drawing.Size(180, 21);
            this.diagnosticScriptCombo.TabIndex = 23;
            this.diagnosticScriptCombo.SelectedIndexChanged += new System.EventHandler(this.diagnosticScriptCombo_SelectedIndexChanged);
            // 
            // groupBox3
            // 
            this.groupBox3.BackColor = System.Drawing.Color.Gainsboro;
            this.groupBox3.Controls.Add(this.textBox3);
            this.groupBox3.Controls.Add(this.label12);
            this.groupBox3.Controls.Add(this.textBox2);
            this.groupBox3.Controls.Add(this.label13);
            this.groupBox3.Controls.Add(this.textBox4);
            this.groupBox3.Controls.Add(this.label18);
            this.groupBox3.Location = new System.Drawing.Point(416, 197);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(210, 133);
            this.groupBox3.TabIndex = 75;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Settings";
            // 
            // groupBox2
            // 
            this.groupBox2.BackColor = System.Drawing.Color.Gainsboro;
            this.groupBox2.Controls.Add(this.splitContainer2);
            this.groupBox2.Location = new System.Drawing.Point(6, 197);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(393, 210);
            this.groupBox2.TabIndex = 74;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Encoder Simulation";
            // 
            // splitContainer2
            // 
            this.splitContainer2.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.splitContainer2.Location = new System.Drawing.Point(-6, 19);
            this.splitContainer2.Name = "splitContainer2";
            this.splitContainer2.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.Controls.Add(this.btnAddFiveEncoder);
            this.splitContainer2.Panel1.Controls.Add(this.lblAbsEncoder);
            this.splitContainer2.Panel1.Controls.Add(this.txtCustEncoderVal);
            this.splitContainer2.Panel1.Controls.Add(this.lblEncoderDegrees);
            this.splitContainer2.Panel1.Controls.Add(this.label9);
            this.splitContainer2.Panel1.Controls.Add(this.lblAzEncoderDegrees);
            this.splitContainer2.Panel1.Controls.Add(this.btnSubtractXEncoder);
            this.splitContainer2.Panel1.Controls.Add(this.lblEncoderTicks);
            this.splitContainer2.Panel1.Controls.Add(this.btnSubtractFiveEncoder);
            this.splitContainer2.Panel1.Controls.Add(this.btnSubtractOneEncoder);
            this.splitContainer2.Panel1.Controls.Add(this.lblAzEncoderTicks);
            this.splitContainer2.Panel1.Controls.Add(this.btnAddXEncoder);
            this.splitContainer2.Panel1.Controls.Add(this.btnAddOneEncoder);
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add(this.label17);
            this.splitContainer2.Panel2.Controls.Add(this.label14);
            this.splitContainer2.Panel2.Controls.Add(this.lblElEncoderTicks);
            this.splitContainer2.Panel2.Controls.Add(this.label16);
            this.splitContainer2.Panel2.Controls.Add(this.lblElEncoderDegrees);
            this.splitContainer2.Panel2.Controls.Add(this.button5);
            this.splitContainer2.Panel2.Controls.Add(this.label10);
            this.splitContainer2.Panel2.Controls.Add(this.textBox1);
            this.splitContainer2.Panel2.Controls.Add(this.button2);
            this.splitContainer2.Panel2.Controls.Add(this.button1);
            this.splitContainer2.Panel2.Controls.Add(this.button3);
            this.splitContainer2.Panel2.Controls.Add(this.button6);
            this.splitContainer2.Panel2.Controls.Add(this.button4);
            this.splitContainer2.Size = new System.Drawing.Size(410, 191);
            this.splitContainer2.SplitterDistance = 91;
            this.splitContainer2.TabIndex = 18;
            // 
            // groupBox1
            // 
            this.groupBox1.BackColor = System.Drawing.Color.Gainsboro;
            this.groupBox1.Controls.Add(this.startTimeTextBox);
            this.groupBox1.Controls.Add(this.label8);
            this.groupBox1.Controls.Add(this.statusTextBox);
            this.groupBox1.Controls.Add(this.endTimeTextBox);
            this.groupBox1.Controls.Add(this.label7);
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Location = new System.Drawing.Point(342, 33);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(289, 81);
            this.groupBox1.TabIndex = 73;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Current Appointment";
            // 
            // tabPage2
            // 
            this.tabPage2.BackColor = System.Drawing.Color.Gray;
            this.tabPage2.Controls.Add(this.groupBox14);
            this.tabPage2.Controls.Add(this.groupBox6);
            this.tabPage2.Controls.Add(this.groupBox5);
            this.tabPage2.Controls.Add(this.groupBox4);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(2);
            this.tabPage2.Size = new System.Drawing.Size(645, 411);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Sensor Data";
            // 
            // groupBox14
            // 
            this.groupBox14.BackColor = System.Drawing.Color.Gainsboro;
            this.groupBox14.Controls.Add(this.farTempConvert);
            this.groupBox14.Controls.Add(this.celTempConvert);
            this.groupBox14.Location = new System.Drawing.Point(314, 19);
            this.groupBox14.Name = "groupBox14";
            this.groupBox14.Size = new System.Drawing.Size(324, 54);
            this.groupBox14.TabIndex = 39;
            this.groupBox14.TabStop = false;
            this.groupBox14.Text = "Temperature Conversion";
            // 
            // farTempConvert
            // 
            this.farTempConvert.BackColor = System.Drawing.Color.Silver;
            this.farTempConvert.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.farTempConvert.Location = new System.Drawing.Point(183, 19);
            this.farTempConvert.Name = "farTempConvert";
            this.farTempConvert.Size = new System.Drawing.Size(130, 23);
            this.farTempConvert.TabIndex = 1;
            this.farTempConvert.Text = "Farenheit";
            this.farTempConvert.TextImageRelation = System.Windows.Forms.TextImageRelation.TextBeforeImage;
            this.farTempConvert.UseVisualStyleBackColor = false;
            this.farTempConvert.Click += new System.EventHandler(this.farTempConvert_Click);
            // 
            // celTempConvert
            // 
            this.celTempConvert.BackColor = System.Drawing.Color.Silver;
            this.celTempConvert.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.celTempConvert.Location = new System.Drawing.Point(9, 19);
            this.celTempConvert.Name = "celTempConvert";
            this.celTempConvert.Size = new System.Drawing.Size(130, 23);
            this.celTempConvert.TabIndex = 0;
            this.celTempConvert.Text = "Celsius";
            this.celTempConvert.UseVisualStyleBackColor = false;
            this.celTempConvert.Click += new System.EventHandler(this.celTempConvert_Click);
            // 
            // groupBox6
            // 
            this.groupBox6.BackColor = System.Drawing.Color.Gainsboro;
            this.groupBox6.Controls.Add(this.splitContainer1);
            this.groupBox6.Location = new System.Drawing.Point(314, 267);
            this.groupBox6.Margin = new System.Windows.Forms.Padding(2);
            this.groupBox6.Name = "groupBox6";
            this.groupBox6.Padding = new System.Windows.Forms.Padding(2);
            this.groupBox6.Size = new System.Drawing.Size(324, 140);
            this.groupBox6.TabIndex = 38;
            this.groupBox6.TabStop = false;
            this.groupBox6.Text = "Motor Sensor Data";
            // 
            // splitContainer1
            // 
            this.splitContainer1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.splitContainer1.Location = new System.Drawing.Point(-7, 18);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.label32);
            this.splitContainer1.Panel1.Controls.Add(this.label34);
            this.splitContainer1.Panel1.Controls.Add(this.lblCurrentAzOrientation);
            this.splitContainer1.Panel1.Controls.Add(this.lblCurrentElOrientation);
            this.splitContainer1.Panel1.Controls.Add(this.label25);
            this.splitContainer1.Panel1.Controls.Add(this.label26);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.AZTempUnitLabel);
            this.splitContainer1.Panel2.Controls.Add(this.ElTempUnitLabel);
            this.splitContainer1.Panel2.Controls.Add(this.lblAzimuthTemp);
            this.splitContainer1.Panel2.Controls.Add(this.fldElTemp);
            this.splitContainer1.Panel2.Controls.Add(this.fldAzTemp);
            this.splitContainer1.Panel2.Controls.Add(this.lblElevationTemp);
            this.splitContainer1.Size = new System.Drawing.Size(333, 123);
            this.splitContainer1.SplitterDistance = 54;
            this.splitContainer1.TabIndex = 20;
            // 
            // label32
            // 
            this.label32.AutoSize = true;
            this.label32.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label32.Location = new System.Drawing.Point(262, 32);
            this.label32.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label32.Name = "label32";
            this.label32.Size = new System.Drawing.Size(47, 13);
            this.label32.TabIndex = 27;
            this.label32.Text = "Degrees";
            // 
            // label34
            // 
            this.label34.AutoSize = true;
            this.label34.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label34.Location = new System.Drawing.Point(262, 10);
            this.label34.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label34.Name = "label34";
            this.label34.Size = new System.Drawing.Size(47, 13);
            this.label34.TabIndex = 26;
            this.label34.Text = "Degrees";
            // 
            // lblCurrentAzOrientation
            // 
            this.lblCurrentAzOrientation.AutoSize = true;
            this.lblCurrentAzOrientation.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCurrentAzOrientation.Location = new System.Drawing.Point(10, 7);
            this.lblCurrentAzOrientation.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblCurrentAzOrientation.Name = "lblCurrentAzOrientation";
            this.lblCurrentAzOrientation.Size = new System.Drawing.Size(122, 16);
            this.lblCurrentAzOrientation.TabIndex = 7;
            this.lblCurrentAzOrientation.Text = "Current Azimuth: ";
            // 
            // lblCurrentElOrientation
            // 
            this.lblCurrentElOrientation.AutoSize = true;
            this.lblCurrentElOrientation.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCurrentElOrientation.Location = new System.Drawing.Point(9, 30);
            this.lblCurrentElOrientation.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblCurrentElOrientation.Name = "lblCurrentElOrientation";
            this.lblCurrentElOrientation.Size = new System.Drawing.Size(134, 16);
            this.lblCurrentElOrientation.TabIndex = 8;
            this.lblCurrentElOrientation.Text = "Current Elevation: ";
            // 
            // label25
            // 
            this.label25.AutoSize = true;
            this.label25.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label25.Location = new System.Drawing.Point(208, 32);
            this.label25.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label25.Name = "label25";
            this.label25.Size = new System.Drawing.Size(28, 18);
            this.label25.TabIndex = 10;
            this.label25.Text = "0.0";
            // 
            // label26
            // 
            this.label26.AutoSize = true;
            this.label26.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label26.Location = new System.Drawing.Point(208, 6);
            this.label26.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label26.Name = "label26";
            this.label26.Size = new System.Drawing.Size(28, 18);
            this.label26.TabIndex = 9;
            this.label26.Text = "0.0";
            // 
            // AZTempUnitLabel
            // 
            this.AZTempUnitLabel.AutoSize = true;
            this.AZTempUnitLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.AZTempUnitLabel.Location = new System.Drawing.Point(262, 12);
            this.AZTempUnitLabel.Name = "AZTempUnitLabel";
            this.AZTempUnitLabel.Size = new System.Drawing.Size(51, 13);
            this.AZTempUnitLabel.TabIndex = 32;
            this.AZTempUnitLabel.Text = "Farenheit";
            // 
            // ElTempUnitLabel
            // 
            this.ElTempUnitLabel.AutoSize = true;
            this.ElTempUnitLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ElTempUnitLabel.Location = new System.Drawing.Point(262, 34);
            this.ElTempUnitLabel.Name = "ElTempUnitLabel";
            this.ElTempUnitLabel.Size = new System.Drawing.Size(51, 13);
            this.ElTempUnitLabel.TabIndex = 31;
            this.ElTempUnitLabel.Text = "Farenheit";
            // 
            // groupBox5
            // 
            this.groupBox5.BackColor = System.Drawing.Color.Gainsboro;
            this.groupBox5.Controls.Add(this.InsideTempUnits);
            this.groupBox5.Controls.Add(this.rainRateUnits);
            this.groupBox5.Controls.Add(this.pressUnits);
            this.groupBox5.Controls.Add(this.dailyRainfallUnits);
            this.groupBox5.Controls.Add(this.outTempUnits);
            this.groupBox5.Controls.Add(this.label35);
            this.groupBox5.Controls.Add(this.windSpeedUnits);
            this.groupBox5.Controls.Add(this.insideTempLabel);
            this.groupBox5.Controls.Add(this.label23);
            this.groupBox5.Controls.Add(this.rainRateLabel);
            this.groupBox5.Controls.Add(this.barometricPressureLabel);
            this.groupBox5.Controls.Add(this.dailyRainfallLabel);
            this.groupBox5.Controls.Add(this.outsideTempLabel);
            this.groupBox5.Controls.Add(this.label19);
            this.groupBox5.Controls.Add(this.label20);
            this.groupBox5.Controls.Add(this.label1);
            this.groupBox5.Controls.Add(this.label2);
            this.groupBox5.Controls.Add(this.label5);
            this.groupBox5.Controls.Add(this.windDirLabel);
            this.groupBox5.Controls.Add(this.label11);
            this.groupBox5.Controls.Add(this.windSpeedLabel);
            this.groupBox5.Location = new System.Drawing.Point(314, 78);
            this.groupBox5.Margin = new System.Windows.Forms.Padding(2);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Padding = new System.Windows.Forms.Padding(2);
            this.groupBox5.Size = new System.Drawing.Size(324, 185);
            this.groupBox5.TabIndex = 37;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "Weather Sensor Data";
            // 
            // InsideTempUnits
            // 
            this.InsideTempUnits.AutoSize = true;
            this.InsideTempUnits.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.InsideTempUnits.Location = new System.Drawing.Point(257, 117);
            this.InsideTempUnits.Name = "InsideTempUnits";
            this.InsideTempUnits.Size = new System.Drawing.Size(51, 13);
            this.InsideTempUnits.TabIndex = 30;
            this.InsideTempUnits.Text = "Farenheit";
            // 
            // rainRateUnits
            // 
            this.rainRateUnits.AutoSize = true;
            this.rainRateUnits.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rainRateUnits.Location = new System.Drawing.Point(257, 91);
            this.rainRateUnits.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.rainRateUnits.Name = "rainRateUnits";
            this.rainRateUnits.Size = new System.Drawing.Size(39, 13);
            this.rainRateUnits.TabIndex = 27;
            this.rainRateUnits.Text = "Inches";
            // 
            // pressUnits
            // 
            this.pressUnits.AutoSize = true;
            this.pressUnits.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.pressUnits.Location = new System.Drawing.Point(257, 164);
            this.pressUnits.Name = "pressUnits";
            this.pressUnits.Size = new System.Drawing.Size(58, 13);
            this.pressUnits.TabIndex = 29;
            this.pressUnits.Text = "Inches/Hg";
            // 
            // dailyRainfallUnits
            // 
            this.dailyRainfallUnits.AutoSize = true;
            this.dailyRainfallUnits.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dailyRainfallUnits.Location = new System.Drawing.Point(257, 70);
            this.dailyRainfallUnits.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.dailyRainfallUnits.Name = "dailyRainfallUnits";
            this.dailyRainfallUnits.Size = new System.Drawing.Size(63, 13);
            this.dailyRainfallUnits.TabIndex = 26;
            this.dailyRainfallUnits.Text = "Inches/Day";
            // 
            // outTempUnits
            // 
            this.outTempUnits.AutoSize = true;
            this.outTempUnits.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.outTempUnits.Location = new System.Drawing.Point(257, 139);
            this.outTempUnits.Name = "outTempUnits";
            this.outTempUnits.Size = new System.Drawing.Size(51, 13);
            this.outTempUnits.TabIndex = 28;
            this.outTempUnits.Text = "Farenheit";
            // 
            // label35
            // 
            this.label35.AutoSize = true;
            this.label35.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label35.Location = new System.Drawing.Point(256, 20);
            this.label35.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label35.Name = "label35";
            this.label35.Size = new System.Drawing.Size(23, 20);
            this.label35.TabIndex = 25;
            this.label35.Text = " --";
            // 
            // windSpeedUnits
            // 
            this.windSpeedUnits.AutoSize = true;
            this.windSpeedUnits.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.windSpeedUnits.Location = new System.Drawing.Point(257, 48);
            this.windSpeedUnits.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.windSpeedUnits.Name = "windSpeedUnits";
            this.windSpeedUnits.Size = new System.Drawing.Size(31, 13);
            this.windSpeedUnits.TabIndex = 24;
            this.windSpeedUnits.Text = "MPH";
            // 
            // insideTempLabel
            // 
            this.insideTempLabel.AutoSize = true;
            this.insideTempLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.insideTempLabel.Location = new System.Drawing.Point(202, 115);
            this.insideTempLabel.Name = "insideTempLabel";
            this.insideTempLabel.Size = new System.Drawing.Size(22, 18);
            this.insideTempLabel.TabIndex = 23;
            this.insideTempLabel.Text = " --";
            // 
            // label23
            // 
            this.label23.AutoSize = true;
            this.label23.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label23.Location = new System.Drawing.Point(6, 115);
            this.label23.Name = "label23";
            this.label23.Size = new System.Drawing.Size(136, 15);
            this.label23.TabIndex = 22;
            this.label23.Text = "Inside Temperature ";
            // 
            // rainRateLabel
            // 
            this.rainRateLabel.AutoSize = true;
            this.rainRateLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rainRateLabel.Location = new System.Drawing.Point(203, 91);
            this.rainRateLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.rainRateLabel.Name = "rainRateLabel";
            this.rainRateLabel.Size = new System.Drawing.Size(22, 18);
            this.rainRateLabel.TabIndex = 19;
            this.rainRateLabel.Text = " --";
            // 
            // barometricPressureLabel
            // 
            this.barometricPressureLabel.AutoSize = true;
            this.barometricPressureLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.barometricPressureLabel.Location = new System.Drawing.Point(203, 159);
            this.barometricPressureLabel.Name = "barometricPressureLabel";
            this.barometricPressureLabel.Size = new System.Drawing.Size(22, 18);
            this.barometricPressureLabel.TabIndex = 21;
            this.barometricPressureLabel.Text = " --";
            // 
            // dailyRainfallLabel
            // 
            this.dailyRainfallLabel.AutoSize = true;
            this.dailyRainfallLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dailyRainfallLabel.Location = new System.Drawing.Point(203, 68);
            this.dailyRainfallLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.dailyRainfallLabel.Name = "dailyRainfallLabel";
            this.dailyRainfallLabel.Size = new System.Drawing.Size(22, 18);
            this.dailyRainfallLabel.TabIndex = 18;
            this.dailyRainfallLabel.Text = " --";
            // 
            // outsideTempLabel
            // 
            this.outsideTempLabel.AutoSize = true;
            this.outsideTempLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.outsideTempLabel.Location = new System.Drawing.Point(202, 139);
            this.outsideTempLabel.Name = "outsideTempLabel";
            this.outsideTempLabel.Size = new System.Drawing.Size(22, 18);
            this.outsideTempLabel.TabIndex = 20;
            this.outsideTempLabel.Text = " --";
            // 
            // label19
            // 
            this.label19.AutoSize = true;
            this.label19.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label19.Location = new System.Drawing.Point(5, 164);
            this.label19.Name = "label19";
            this.label19.Size = new System.Drawing.Size(142, 15);
            this.label19.TabIndex = 19;
            this.label19.Text = "Barometric Pressure ";
            this.label19.Click += new System.EventHandler(this.label19_Click);
            // 
            // label20
            // 
            this.label20.AutoSize = true;
            this.label20.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label20.Location = new System.Drawing.Point(6, 142);
            this.label20.Name = "label20";
            this.label20.Size = new System.Drawing.Size(146, 15);
            this.label20.TabIndex = 18;
            this.label20.Text = "Outside Temperature ";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(6, 91);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(75, 15);
            this.label1.TabIndex = 17;
            this.label1.Text = "Rain Rate ";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(5, 68);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(97, 15);
            this.label2.TabIndex = 16;
            this.label2.Text = "Daily Rainfall ";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(4, 46);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(84, 15);
            this.label5.TabIndex = 14;
            this.label5.Text = "Wind Speed";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label11.Location = new System.Drawing.Point(4, 25);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(105, 15);
            this.label11.TabIndex = 15;
            this.label11.Text = "Wind Direction:";
            // 
            // groupBox4
            // 
            this.groupBox4.BackColor = System.Drawing.Color.Gainsboro;
            this.groupBox4.Controls.Add(this.lbGateStat);
            this.groupBox4.Controls.Add(this.lbEstopStat);
            this.groupBox4.Controls.Add(this.label31);
            this.groupBox4.Controls.Add(this.label30);
            this.groupBox4.Controls.Add(this.lblElLimStatus2);
            this.groupBox4.Controls.Add(this.lblElLimStatus1);
            this.groupBox4.Controls.Add(this.lblAzHome1);
            this.groupBox4.Controls.Add(this.lblAzLimStatus2);
            this.groupBox4.Controls.Add(this.lblAzHome2);
            this.groupBox4.Controls.Add(this.lblAzLimStatus1);
            this.groupBox4.Controls.Add(this.lblElHome);
            this.groupBox4.Controls.Add(this.lblElLimit2);
            this.groupBox4.Controls.Add(this.lblELHomeStatus);
            this.groupBox4.Controls.Add(this.lblAzHomeStatus2);
            this.groupBox4.Controls.Add(this.lblElLimit1);
            this.groupBox4.Controls.Add(this.lblAzHomeStatus1);
            this.groupBox4.Controls.Add(this.lblAzLimit1);
            this.groupBox4.Controls.Add(this.lblAzLimit2);
            this.groupBox4.Location = new System.Drawing.Point(6, 3);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(295, 405);
            this.groupBox4.TabIndex = 36;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Sensor Data";
            // 
            // lbGateStat
            // 
            this.lbGateStat.AutoSize = true;
            this.lbGateStat.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbGateStat.Location = new System.Drawing.Point(223, 363);
            this.lbGateStat.Name = "lbGateStat";
            this.lbGateStat.Size = new System.Drawing.Size(56, 15);
            this.lbGateStat.TabIndex = 21;
            this.lbGateStat.Text = "Inactive";
            // 
            // lbEstopStat
            // 
            this.lbEstopStat.AutoSize = true;
            this.lbEstopStat.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbEstopStat.Location = new System.Drawing.Point(223, 333);
            this.lbEstopStat.Name = "lbEstopStat";
            this.lbEstopStat.Size = new System.Drawing.Size(56, 15);
            this.lbEstopStat.TabIndex = 20;
            this.lbEstopStat.Text = "Inactive";
            // 
            // label31
            // 
            this.label31.AutoSize = true;
            this.label31.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label31.Location = new System.Drawing.Point(9, 363);
            this.label31.Name = "label31";
            this.label31.Size = new System.Drawing.Size(37, 15);
            this.label31.TabIndex = 19;
            this.label31.Text = "Gate";
            // 
            // label30
            // 
            this.label30.AutoSize = true;
            this.label30.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label30.Location = new System.Drawing.Point(9, 333);
            this.label30.Name = "label30";
            this.label30.Size = new System.Drawing.Size(43, 15);
            this.label30.TabIndex = 18;
            this.label30.Text = "Estop";
            // 
            // tabPage3
            // 
            this.tabPage3.BackColor = System.Drawing.Color.Gray;
            this.tabPage3.Controls.Add(this.groupBox13);
            this.tabPage3.Controls.Add(this.groupBox12);
            this.tabPage3.Controls.Add(this.groupBox10);
            this.tabPage3.Controls.Add(this.groupBox11);
            this.tabPage3.Location = new System.Drawing.Point(4, 22);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Padding = new System.Windows.Forms.Padding(2);
            this.tabPage3.Size = new System.Drawing.Size(645, 411);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "Sensor Overrides";
            // 
            // groupBox13
            // 
            this.groupBox13.BackColor = System.Drawing.Color.Gainsboro;
            this.groupBox13.Controls.Add(this.ElMotTempSensOverride);
            this.groupBox13.Controls.Add(this.label29);
            this.groupBox13.Controls.Add(this.AzMotTempSensOverride);
            this.groupBox13.Controls.Add(this.label28);
            this.groupBox13.Location = new System.Drawing.Point(3, 233);
            this.groupBox13.Name = "groupBox13";
            this.groupBox13.Size = new System.Drawing.Size(338, 157);
            this.groupBox13.TabIndex = 29;
            this.groupBox13.TabStop = false;
            this.groupBox13.Text = "Motor Temperature Sensors";
            // 
            // ElMotTempSensOverride
            // 
            this.ElMotTempSensOverride.BackColor = System.Drawing.Color.Yellow;
            this.ElMotTempSensOverride.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.ElMotTempSensOverride.Location = new System.Drawing.Point(227, 78);
            this.ElMotTempSensOverride.Margin = new System.Windows.Forms.Padding(2);
            this.ElMotTempSensOverride.Name = "ElMotTempSensOverride";
            this.ElMotTempSensOverride.Size = new System.Drawing.Size(91, 23);
            this.ElMotTempSensOverride.TabIndex = 15;
            this.ElMotTempSensOverride.Text = "NOT LOADED";
            this.ElMotTempSensOverride.UseVisualStyleBackColor = false;
            this.ElMotTempSensOverride.Click += new System.EventHandler(this.ElMotTempSensOverride_Click);
            // 
            // label29
            // 
            this.label29.AutoSize = true;
            this.label29.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label29.Location = new System.Drawing.Point(9, 78);
            this.label29.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label29.Name = "label29";
            this.label29.Size = new System.Drawing.Size(214, 13);
            this.label29.TabIndex = 14;
            this.label29.Text = "Elevation Motor Temperature Sensor";
            // 
            // AzMotTempSensOverride
            // 
            this.AzMotTempSensOverride.BackColor = System.Drawing.Color.Yellow;
            this.AzMotTempSensOverride.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.AzMotTempSensOverride.Location = new System.Drawing.Point(229, 23);
            this.AzMotTempSensOverride.Margin = new System.Windows.Forms.Padding(2);
            this.AzMotTempSensOverride.Name = "AzMotTempSensOverride";
            this.AzMotTempSensOverride.Size = new System.Drawing.Size(89, 23);
            this.AzMotTempSensOverride.TabIndex = 13;
            this.AzMotTempSensOverride.Text = "NOT LOADED";
            this.AzMotTempSensOverride.UseVisualStyleBackColor = false;
            this.AzMotTempSensOverride.Click += new System.EventHandler(this.AzMotTempSensOverride_Click);
            // 
            // label28
            // 
            this.label28.AutoSize = true;
            this.label28.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label28.Location = new System.Drawing.Point(9, 31);
            this.label28.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label28.Name = "label28";
            this.label28.Size = new System.Drawing.Size(205, 13);
            this.label28.TabIndex = 12;
            this.label28.Text = "Azimuth Motor Temperature Sensor";
            // 
            // groupBox12
            // 
            this.groupBox12.BackColor = System.Drawing.Color.Gainsboro;
            this.groupBox12.Controls.Add(this.MGOverride);
            this.groupBox12.Controls.Add(this.label27);
            this.groupBox12.Location = new System.Drawing.Point(3, 135);
            this.groupBox12.Name = "groupBox12";
            this.groupBox12.Size = new System.Drawing.Size(338, 92);
            this.groupBox12.TabIndex = 28;
            this.groupBox12.TabStop = false;
            this.groupBox12.Text = "Gate Sensor";
            // 
            // MGOverride
            // 
            this.MGOverride.BackColor = System.Drawing.Color.Yellow;
            this.MGOverride.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.MGOverride.Location = new System.Drawing.Point(229, 31);
            this.MGOverride.Margin = new System.Windows.Forms.Padding(2);
            this.MGOverride.Name = "MGOverride";
            this.MGOverride.Size = new System.Drawing.Size(89, 23);
            this.MGOverride.TabIndex = 13;
            this.MGOverride.Text = "NOT LOADED";
            this.MGOverride.UseVisualStyleBackColor = false;
            this.MGOverride.Click += new System.EventHandler(this.MGOverride_Click);
            // 
            // label27
            // 
            this.label27.AutoSize = true;
            this.label27.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label27.Location = new System.Drawing.Point(9, 31);
            this.label27.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label27.Name = "label27";
            this.label27.Size = new System.Drawing.Size(108, 13);
            this.label27.TabIndex = 12;
            this.label27.Text = "Main Gate Sensor";
            // 
            // groupBox10
            // 
            this.groupBox10.BackColor = System.Drawing.Color.Gainsboro;
            this.groupBox10.Controls.Add(this.ElevationProximityOveride2);
            this.groupBox10.Controls.Add(this.ElevationProximityOveride1);
            this.groupBox10.Controls.Add(this.ORAzimuthSens2);
            this.groupBox10.Controls.Add(this.ORAzimuthSens1);
            this.groupBox10.Controls.Add(this.label3);
            this.groupBox10.Controls.Add(this.label4);
            this.groupBox10.Controls.Add(this.label21);
            this.groupBox10.Controls.Add(this.label22);
            this.groupBox10.Location = new System.Drawing.Point(347, 24);
            this.groupBox10.Name = "groupBox10";
            this.groupBox10.Padding = new System.Windows.Forms.Padding(2);
            this.groupBox10.Size = new System.Drawing.Size(293, 366);
            this.groupBox10.TabIndex = 0;
            this.groupBox10.TabStop = false;
            this.groupBox10.Text = " Proximity Sensors";
            // 
            // ElevationProximityOveride2
            // 
            this.ElevationProximityOveride2.BackColor = System.Drawing.Color.Yellow;
            this.ElevationProximityOveride2.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.ElevationProximityOveride2.Location = new System.Drawing.Point(194, 282);
            this.ElevationProximityOveride2.Name = "ElevationProximityOveride2";
            this.ElevationProximityOveride2.Size = new System.Drawing.Size(88, 23);
            this.ElevationProximityOveride2.TabIndex = 14;
            this.ElevationProximityOveride2.Text = "NOT LOADED";
            this.ElevationProximityOveride2.UseVisualStyleBackColor = false;
            this.ElevationProximityOveride2.Click += new System.EventHandler(this.ElevationProximityOverideButton2_Click);
            // 
            // ElevationProximityOveride1
            // 
            this.ElevationProximityOveride1.BackColor = System.Drawing.Color.Yellow;
            this.ElevationProximityOveride1.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.ElevationProximityOveride1.Location = new System.Drawing.Point(194, 224);
            this.ElevationProximityOveride1.Name = "ElevationProximityOveride1";
            this.ElevationProximityOveride1.Size = new System.Drawing.Size(90, 23);
            this.ElevationProximityOveride1.TabIndex = 13;
            this.ElevationProximityOveride1.Text = "NOT LOADED";
            this.ElevationProximityOveride1.UseVisualStyleBackColor = false;
            this.ElevationProximityOveride1.Click += new System.EventHandler(this.ElevationProximityOverideButton1_Click);
            // 
            // ORAzimuthSens2
            // 
            this.ORAzimuthSens2.BackColor = System.Drawing.Color.Yellow;
            this.ORAzimuthSens2.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.ORAzimuthSens2.Location = new System.Drawing.Point(194, 134);
            this.ORAzimuthSens2.Name = "ORAzimuthSens2";
            this.ORAzimuthSens2.Size = new System.Drawing.Size(90, 23);
            this.ORAzimuthSens2.TabIndex = 12;
            this.ORAzimuthSens2.Text = "NOT LOADED";
            this.ORAzimuthSens2.UseVisualStyleBackColor = false;
            this.ORAzimuthSens2.Click += new System.EventHandler(this.ORAzimuthSens2_Click);
            // 
            // ORAzimuthSens1
            // 
            this.ORAzimuthSens1.BackColor = System.Drawing.Color.Yellow;
            this.ORAzimuthSens1.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.ORAzimuthSens1.Location = new System.Drawing.Point(194, 53);
            this.ORAzimuthSens1.Name = "ORAzimuthSens1";
            this.ORAzimuthSens1.Size = new System.Drawing.Size(90, 23);
            this.ORAzimuthSens1.TabIndex = 11;
            this.ORAzimuthSens1.Text = "NOT LOADED";
            this.ORAzimuthSens1.UseVisualStyleBackColor = false;
            this.ORAzimuthSens1.Click += new System.EventHandler(this.ORAzimuthSens1_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(9, 282);
            this.label3.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(155, 13);
            this.label3.TabIndex = 10;
            this.label3.Text = "Elevation Limit Switch 90°";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(9, 53);
            this.label4.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(150, 13);
            this.label4.TabIndex = 7;
            this.label4.Text = "Azimuth Limit Switch -10°";
            this.label4.Click += new System.EventHandler(this.label4_Click);
            // 
            // label21
            // 
            this.label21.AutoSize = true;
            this.label21.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label21.Location = new System.Drawing.Point(11, 134);
            this.label21.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label21.Name = "label21";
            this.label21.Size = new System.Drawing.Size(153, 13);
            this.label21.TabIndex = 8;
            this.label21.Text = "Azimuth Limit Switch 375°";
            // 
            // label22
            // 
            this.label22.AutoSize = true;
            this.label22.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label22.Location = new System.Drawing.Point(9, 224);
            this.label22.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label22.Name = "label22";
            this.label22.Size = new System.Drawing.Size(148, 13);
            this.label22.TabIndex = 9;
            this.label22.Text = "Elevation Limit Switch 0°";
            // 
            // groupBox11
            // 
            this.groupBox11.BackColor = System.Drawing.Color.Gainsboro;
            this.groupBox11.Controls.Add(this.WSOverride);
            this.groupBox11.Controls.Add(this.label24);
            this.groupBox11.Location = new System.Drawing.Point(2, 24);
            this.groupBox11.Margin = new System.Windows.Forms.Padding(2);
            this.groupBox11.Name = "groupBox11";
            this.groupBox11.Padding = new System.Windows.Forms.Padding(2);
            this.groupBox11.Size = new System.Drawing.Size(339, 106);
            this.groupBox11.TabIndex = 27;
            this.groupBox11.TabStop = false;
            this.groupBox11.Text = "Weather Station";
            // 
            // WSOverride
            // 
            this.WSOverride.BackColor = System.Drawing.Color.Yellow;
            this.WSOverride.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.WSOverride.Location = new System.Drawing.Point(228, 35);
            this.WSOverride.Margin = new System.Windows.Forms.Padding(2);
            this.WSOverride.Name = "WSOverride";
            this.WSOverride.Size = new System.Drawing.Size(89, 23);
            this.WSOverride.TabIndex = 13;
            this.WSOverride.Text = "NOT LOADED";
            this.WSOverride.UseVisualStyleBackColor = false;
            this.WSOverride.Click += new System.EventHandler(this.WSOverride_Click);
            // 
            // label24
            // 
            this.label24.AutoSize = true;
            this.label24.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label24.Location = new System.Drawing.Point(9, 35);
            this.label24.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label24.Name = "label24";
            this.label24.Size = new System.Drawing.Size(99, 13);
            this.label24.TabIndex = 12;
            this.label24.Text = "Weather Station";
            // 
            // tabPage4
            // 
            this.tabPage4.Controls.Add(this.lblModeType);
            this.tabPage4.Controls.Add(this.spectraModeTypeVal);
            this.tabPage4.Controls.Add(this.lblFrequency);
            this.tabPage4.Controls.Add(this.frequencyVal);
            this.tabPage4.Controls.Add(this.spectraCyberScanChart);
            this.tabPage4.Controls.Add(this.lblIntegrationStep);
            this.tabPage4.Controls.Add(this.IntegrationStepVal);
            this.tabPage4.Controls.Add(this.lblDCGain);
            this.tabPage4.Controls.Add(this.DCGainVal);
            this.tabPage4.Controls.Add(this.lblIFGain);
            this.tabPage4.Controls.Add(this.IFGainVal);
            this.tabPage4.Controls.Add(this.lblBandwidth);
            this.tabPage4.Controls.Add(this.BandwidthVal);
            this.tabPage4.Controls.Add(this.lblOffsetVoltage);
            this.tabPage4.Controls.Add(this.OffsetVoltageVal);
            this.tabPage4.Location = new System.Drawing.Point(4, 22);
            this.tabPage4.Name = "tabPage4";
            this.tabPage4.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage4.Size = new System.Drawing.Size(645, 411);
            this.tabPage4.TabIndex = 3;
            this.tabPage4.Text = "RFData";
            this.tabPage4.UseVisualStyleBackColor = true;
            // 
            // spectraCyberScanChart
            // 
            chartArea1.AxisX.Title = "Time";
            chartArea1.AxisY.Title = "RF Data";
            chartArea1.Name = "ChartArea1";
            this.spectraCyberScanChart.ChartAreas.Add(chartArea1);
            legend1.Name = "Legend1";
            this.spectraCyberScanChart.Legends.Add(legend1);
            this.spectraCyberScanChart.Location = new System.Drawing.Point(36, 0);
            this.spectraCyberScanChart.Name = "spectraCyberScanChart";
            series1.ChartArea = "ChartArea1";
            series1.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Spline;
            series1.Legend = "Legend1";
            series1.Name = "Data/Time";
            this.spectraCyberScanChart.Series.Add(series1);
            this.spectraCyberScanChart.Size = new System.Drawing.Size(571, 279);
            this.spectraCyberScanChart.TabIndex = 16;
            this.spectraCyberScanChart.Text = "spectraCyberScanChart";
            // 
            // lblIntegrationStep
            // 
            this.lblIntegrationStep.AutoSize = true;
            this.lblIntegrationStep.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblIntegrationStep.Location = new System.Drawing.Point(300, 363);
            this.lblIntegrationStep.Name = "lblIntegrationStep";
            this.lblIntegrationStep.Size = new System.Drawing.Size(105, 15);
            this.lblIntegrationStep.TabIndex = 14;
            this.lblIntegrationStep.Text = "IntegrationStep";
            // 
            // IntegrationStepVal
            // 
            this.IntegrationStepVal.AutoSize = true;
            this.IntegrationStepVal.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.IntegrationStepVal.Location = new System.Drawing.Point(514, 363);
            this.IntegrationStepVal.Name = "IntegrationStepVal";
            this.IntegrationStepVal.Size = new System.Drawing.Size(35, 15);
            this.IntegrationStepVal.TabIndex = 15;
            this.IntegrationStepVal.Text = "NaN";
            // 
            // lblDCGain
            // 
            this.lblDCGain.AutoSize = true;
            this.lblDCGain.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDCGain.Location = new System.Drawing.Point(6, 389);
            this.lblDCGain.Name = "lblDCGain";
            this.lblDCGain.Size = new System.Drawing.Size(56, 15);
            this.lblDCGain.TabIndex = 12;
            this.lblDCGain.Text = "DCGain";
            // 
            // DCGainVal
            // 
            this.DCGainVal.AutoSize = true;
            this.DCGainVal.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.DCGainVal.Location = new System.Drawing.Point(220, 389);
            this.DCGainVal.Name = "DCGainVal";
            this.DCGainVal.Size = new System.Drawing.Size(35, 15);
            this.DCGainVal.TabIndex = 13;
            this.DCGainVal.Text = "NaN";
            // 
            // lblIFGain
            // 
            this.lblIFGain.AutoSize = true;
            this.lblIFGain.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblIFGain.Location = new System.Drawing.Point(6, 363);
            this.lblIFGain.Name = "lblIFGain";
            this.lblIFGain.Size = new System.Drawing.Size(53, 15);
            this.lblIFGain.TabIndex = 10;
            this.lblIFGain.Text = "IF Gain";
            this.lblIFGain.Click += new System.EventHandler(this.label39_Click);
            // 
            // IFGainVal
            // 
            this.IFGainVal.AutoSize = true;
            this.IFGainVal.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.IFGainVal.Location = new System.Drawing.Point(220, 363);
            this.IFGainVal.Name = "IFGainVal";
            this.IFGainVal.Size = new System.Drawing.Size(35, 15);
            this.IFGainVal.TabIndex = 11;
            this.IFGainVal.Text = "NaN";
            this.IFGainVal.Click += new System.EventHandler(this.label40_Click);
            // 
            // lblBandwidth
            // 
            this.lblBandwidth.AutoSize = true;
            this.lblBandwidth.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblBandwidth.Location = new System.Drawing.Point(6, 333);
            this.lblBandwidth.Name = "lblBandwidth";
            this.lblBandwidth.Size = new System.Drawing.Size(74, 15);
            this.lblBandwidth.TabIndex = 8;
            this.lblBandwidth.Text = "Bandwidth";
            // 
            // BandwidthVal
            // 
            this.BandwidthVal.AutoSize = true;
            this.BandwidthVal.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BandwidthVal.Location = new System.Drawing.Point(220, 333);
            this.BandwidthVal.Name = "BandwidthVal";
            this.BandwidthVal.Size = new System.Drawing.Size(35, 15);
            this.BandwidthVal.TabIndex = 9;
            this.BandwidthVal.Text = "NaN";
            // 
            // lblOffsetVoltage
            // 
            this.lblOffsetVoltage.AutoSize = true;
            this.lblOffsetVoltage.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblOffsetVoltage.Location = new System.Drawing.Point(300, 389);
            this.lblOffsetVoltage.Name = "lblOffsetVoltage";
            this.lblOffsetVoltage.Size = new System.Drawing.Size(92, 15);
            this.lblOffsetVoltage.TabIndex = 6;
            this.lblOffsetVoltage.Text = "OffsetVoltage";
            // 
            // OffsetVoltageVal
            // 
            this.OffsetVoltageVal.AutoSize = true;
            this.OffsetVoltageVal.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.OffsetVoltageVal.Location = new System.Drawing.Point(514, 389);
            this.OffsetVoltageVal.Name = "OffsetVoltageVal";
            this.OffsetVoltageVal.Size = new System.Drawing.Size(35, 15);
            this.OffsetVoltageVal.TabIndex = 7;
            this.OffsetVoltageVal.Text = "NaN";
            // 
            // tabPage5
            // 
            this.tabPage5.Controls.Add(this.consoleLogBox);
            this.tabPage5.Location = new System.Drawing.Point(4, 22);
            this.tabPage5.Name = "tabPage5";
            this.tabPage5.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage5.Size = new System.Drawing.Size(645, 411);
            this.tabPage5.TabIndex = 4;
            this.tabPage5.Text = "Console Log";
            this.tabPage5.UseVisualStyleBackColor = true;
            // 
            // consoleLogBox
            // 
            this.consoleLogBox.AcceptsReturn = true;
            this.consoleLogBox.AcceptsTab = true;
            this.consoleLogBox.AllowDrop = true;
            this.consoleLogBox.Location = new System.Drawing.Point(6, 6);
            this.consoleLogBox.Multiline = true;
            this.consoleLogBox.Name = "consoleLogBox";
            this.consoleLogBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.consoleLogBox.Size = new System.Drawing.Size(631, 400);
            this.consoleLogBox.TabIndex = 0;
            // 
            // lblFrequency
            // 
            this.lblFrequency.AutoSize = true;
            this.lblFrequency.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblFrequency.Location = new System.Drawing.Point(300, 333);
            this.lblFrequency.Name = "lblFrequency";
            this.lblFrequency.Size = new System.Drawing.Size(73, 15);
            this.lblFrequency.TabIndex = 17;
            this.lblFrequency.Text = "Frequency";
            // 
            // frequencyVal
            // 
            this.frequencyVal.AutoSize = true;
            this.frequencyVal.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.frequencyVal.Location = new System.Drawing.Point(514, 333);
            this.frequencyVal.Name = "frequencyVal";
            this.frequencyVal.Size = new System.Drawing.Size(35, 15);
            this.frequencyVal.TabIndex = 18;
            this.frequencyVal.Text = "NaN";
            // 
            // lblModeType
            // 
            this.lblModeType.AutoSize = true;
            this.lblModeType.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblModeType.Location = new System.Drawing.Point(155, 297);
            this.lblModeType.Name = "lblModeType";
            this.lblModeType.Size = new System.Drawing.Size(128, 15);
            this.lblModeType.TabIndex = 19;
            this.lblModeType.Text = "SpectraCyberMode";
            // 
            // spectraModeTypeVal
            // 
            this.spectraModeTypeVal.AutoSize = true;
            this.spectraModeTypeVal.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.spectraModeTypeVal.Location = new System.Drawing.Point(369, 297);
            this.spectraModeTypeVal.Name = "spectraModeTypeVal";
            this.spectraModeTypeVal.Size = new System.Drawing.Size(35, 15);
            this.spectraModeTypeVal.TabIndex = 20;
            this.spectraModeTypeVal.Text = "NaN";
            // 
            // DiagnosticsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(655, 446);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.label15);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Margin = new System.Windows.Forms.Padding(2);
            this.MinimumSize = new System.Drawing.Size(352, 175);
            this.Name = "DiagnosticsForm";
            this.Text = "DiagnosticsForm";
            this.Load += new System.EventHandler(this.DiagnosticsForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.groupBox9.ResumeLayout(false);
            this.groupBox9.PerformLayout();
            this.groupBox8.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel1.PerformLayout();
            this.splitContainer2.Panel2.ResumeLayout(false);
            this.splitContainer2.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
            this.splitContainer2.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.tabPage2.ResumeLayout(false);
            this.groupBox14.ResumeLayout(false);
            this.groupBox6.ResumeLayout(false);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel1.PerformLayout();
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.groupBox5.ResumeLayout(false);
            this.groupBox5.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.tabPage3.ResumeLayout(false);
            this.groupBox13.ResumeLayout(false);
            this.groupBox13.PerformLayout();
            this.groupBox12.ResumeLayout(false);
            this.groupBox12.PerformLayout();
            this.groupBox10.ResumeLayout(false);
            this.groupBox10.PerformLayout();
            this.groupBox11.ResumeLayout(false);
            this.groupBox11.PerformLayout();
            this.tabPage4.ResumeLayout(false);
            this.tabPage4.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.spectraCyberScanChart)).EndInit();
            this.tabPage5.ResumeLayout(false);
            this.tabPage5.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.Label windSpeedLabel;
        private System.Windows.Forms.Label windDirLabel;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox startTimeTextBox;
        private System.Windows.Forms.TextBox endTimeTextBox;
        private System.Windows.Forms.TextBox statusTextBox;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Label lblAzimuthTemp;
        private System.Windows.Forms.Label lblElevationTemp;
        private System.Windows.Forms.Label fldAzTemp;
        private System.Windows.Forms.Label fldElTemp;
        private System.Windows.Forms.Button btnTest;
        private System.Windows.Forms.CheckBox selectDemo;
        private System.Windows.Forms.Label lblElLimStatus2;
        private System.Windows.Forms.Label lblElLimStatus1;
        private System.Windows.Forms.Label lblAzLimStatus2;
        private System.Windows.Forms.Label lblAzLimStatus1;
        private System.Windows.Forms.Label lblElLimit2;
        private System.Windows.Forms.Label lblElLimit1;
        private System.Windows.Forms.Label lblAzLimit2;
        private System.Windows.Forms.Label lblAzLimit1;
        private System.Windows.Forms.Label lblELHomeStatus;
        private System.Windows.Forms.Label lblAzHomeStatus2;
        private System.Windows.Forms.Label lblAzHomeStatus1;
        private System.Windows.Forms.Label lblElHome;
        private System.Windows.Forms.Label lblAzHome2;
        private System.Windows.Forms.Label lblAzHome1;
        private System.Windows.Forms.Label lblAbsEncoder;
        private System.Windows.Forms.Label lblEncoderDegrees;
        private System.Windows.Forms.Label lblAzEncoderDegrees;
        private System.Windows.Forms.Label lblEncoderTicks;
        private System.Windows.Forms.Label lblAzEncoderTicks;
        private System.Windows.Forms.Button btnAddOneEncoder;
        private System.Windows.Forms.Button btnAddFiveEncoder;
        private System.Windows.Forms.Button btnAddXEncoder;
        private System.Windows.Forms.Button btnSubtractOneEncoder;
        private System.Windows.Forms.Button btnSubtractFiveEncoder;
        private System.Windows.Forms.Button btnSubtractXEncoder;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox txtCustEncoderVal;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.Button button5;
        private System.Windows.Forms.Button button6;
        private System.Windows.Forms.Label lblElEncoderTicks;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.Label lblElEncoderDegrees;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.TextBox textBox3;
        private System.Windows.Forms.TextBox textBox4;
        private System.Windows.Forms.Label label18;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.GroupBox groupBox6;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.Label label19;
        private System.Windows.Forms.Label label20;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label barometricPressureLabel;
        private System.Windows.Forms.Label outsideTempLabel;
        private System.Windows.Forms.Label rainRateLabel;
        private System.Windows.Forms.Label dailyRainfallLabel;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private System.Windows.Forms.Label label25;
        private System.Windows.Forms.Label label26;
        private System.Windows.Forms.Label lblCurrentAzOrientation;
        private System.Windows.Forms.Label lblCurrentElOrientation;
        private System.Windows.Forms.GroupBox groupBox8;
        private System.Windows.Forms.Button runDiagScriptsButton;
        private System.Windows.Forms.ComboBox diagnosticScriptCombo;
        private System.Windows.Forms.GroupBox groupBox9;
        private System.Windows.Forms.Label insideTempLabel;
        private System.Windows.Forms.Label label23;
        private System.Windows.Forms.Button button7;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.GroupBox groupBox10;
        private System.Windows.Forms.Button ElevationProximityOveride2;
        private System.Windows.Forms.Button ElevationProximityOveride1;
        private System.Windows.Forms.Button ORAzimuthSens2;
        private System.Windows.Forms.Button ORAzimuthSens1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label21;
        private System.Windows.Forms.Label label22;
        private System.Windows.Forms.GroupBox groupBox11;
        private System.Windows.Forms.GroupBox groupBox13;
        private System.Windows.Forms.Button ElMotTempSensOverride;
        private System.Windows.Forms.Label label29;
        private System.Windows.Forms.Button AzMotTempSensOverride;
        private System.Windows.Forms.Label label28;
        private System.Windows.Forms.GroupBox groupBox12;
        private System.Windows.Forms.Button MGOverride;
        private System.Windows.Forms.Label label27;
        private System.Windows.Forms.Button WSOverride;
        private System.Windows.Forms.Label label24;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.GroupBox groupBox14;
        private System.Windows.Forms.Button farTempConvert;
        private System.Windows.Forms.Button celTempConvert;
        private System.Windows.Forms.Label InsideTempUnits;
        private System.Windows.Forms.Label rainRateUnits;
        private System.Windows.Forms.Label pressUnits;
        private System.Windows.Forms.Label dailyRainfallUnits;
        private System.Windows.Forms.Label outTempUnits;
        private System.Windows.Forms.Label label35;
        private System.Windows.Forms.Label windSpeedUnits;
        private System.Windows.Forms.Label label32;
        private System.Windows.Forms.Label label34;
        private System.Windows.Forms.Label AZTempUnitLabel;
        private System.Windows.Forms.Label ElTempUnitLabel;
        private System.Windows.Forms.Label lbGateStat;
        private System.Windows.Forms.Label lbEstopStat;
        private System.Windows.Forms.Label label31;
        private System.Windows.Forms.Label label30;
        private System.Windows.Forms.TabPage tabPage4;
        private System.Windows.Forms.TabPage tabPage5;
        private System.Windows.Forms.Label lblDCGain;
        private System.Windows.Forms.Label DCGainVal;
        private System.Windows.Forms.Label lblIFGain;
        private System.Windows.Forms.Label IFGainVal;
        private System.Windows.Forms.Label lblBandwidth;
        private System.Windows.Forms.Label BandwidthVal;
        private System.Windows.Forms.Label lblOffsetVoltage;
        private System.Windows.Forms.Label OffsetVoltageVal;
        private System.Windows.Forms.Label lblIntegrationStep;
        private System.Windows.Forms.Label IntegrationStepVal;
        private System.Windows.Forms.TextBox consoleLogBox;
        private System.Windows.Forms.DataVisualization.Charting.Chart spectraCyberScanChart;
        private System.Windows.Forms.Label lblFrequency;
        private System.Windows.Forms.Label frequencyVal;
        private System.Windows.Forms.Label lblModeType;
        private System.Windows.Forms.Label spectraModeTypeVal;
    }
}