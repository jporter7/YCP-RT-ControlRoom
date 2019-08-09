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
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.lblCurrentAzOrientation = new System.Windows.Forms.Label();
            this.lblCurrentElOrientation = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.startTimeTextBox = new System.Windows.Forms.TextBox();
            this.endTimeTextBox = new System.Windows.Forms.TextBox();
            this.statusTextBox = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.lblAzimuthTemp = new System.Windows.Forms.Label();
            this.lblElevationTemp = new System.Windows.Forms.Label();
            this.radioButton1 = new System.Windows.Forms.RadioButton();
            this.radioButton2 = new System.Windows.Forms.RadioButton();
            this.fldAzTemp = new System.Windows.Forms.Label();
            this.fldElTemp = new System.Windows.Forms.Label();
            this.txtTemperature = new System.Windows.Forms.TextBox();
            this.btnTest = new System.Windows.Forms.Button();
            this.warningLabel = new System.Windows.Forms.Label();
            this.fanLabel = new System.Windows.Forms.Label();
            this.btnAddOneTemp = new System.Windows.Forms.Button();
            this.btnAddFiveTemp = new System.Windows.Forms.Button();
            this.btnAddXTemp = new System.Windows.Forms.Button();
            this.lblShutdown = new System.Windows.Forms.Label();
            this.selectDemo = new System.Windows.Forms.CheckBox();
            this.btnSubtractOneTemp = new System.Windows.Forms.Button();
            this.btnSubtractFiveTemp = new System.Windows.Forms.Button();
            this.btnSubtractXTemp = new System.Windows.Forms.Button();
            this.txtCustTemp = new System.Windows.Forms.TextBox();
            this.label11 = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.lblElLimStatus2 = new System.Windows.Forms.Label();
            this.lblElLimStatus1 = new System.Windows.Forms.Label();
            this.lblAzLimStatus2 = new System.Windows.Forms.Label();
            this.lblAzLimStatus1 = new System.Windows.Forms.Label();
            this.lblElLimit2 = new System.Windows.Forms.Label();
            this.lblElLimit1 = new System.Windows.Forms.Label();
            this.lblAzLimit2 = new System.Windows.Forms.Label();
            this.lblAzLimit1 = new System.Windows.Forms.Label();
            this.lblEleProx2 = new System.Windows.Forms.Label();
            this.lblEleProx1 = new System.Windows.Forms.Label();
            this.lblElProx2 = new System.Windows.Forms.Label();
            this.lblElProx1 = new System.Windows.Forms.Label();
            this.lblAzProxStatus3 = new System.Windows.Forms.Label();
            this.lblAzProxStatus2 = new System.Windows.Forms.Label();
            this.lblAzProxStatus1 = new System.Windows.Forms.Label();
            this.lblAzProx3 = new System.Windows.Forms.Label();
            this.lblAzProx2 = new System.Windows.Forms.Label();
            this.lblAzProx1 = new System.Windows.Forms.Label();
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
            this.label19 = new System.Windows.Forms.Label();
            this.label21 = new System.Windows.Forms.Label();
            this.label22 = new System.Windows.Forms.Label();
            this.PLC_regs = new System.Windows.Forms.DataGridView();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.PLC_regs)).BeginInit();
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
            this.dataGridView1.Location = new System.Drawing.Point(12, 12);
            this.dataGridView1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowTemplate.Height = 24;
            this.dataGridView1.Size = new System.Drawing.Size(387, 219);
            this.dataGridView1.TabIndex = 0;
            this.dataGridView1.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellContentClick);
            // 
            // lblCurrentAzOrientation
            // 
            this.lblCurrentAzOrientation.AutoSize = true;
            this.lblCurrentAzOrientation.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCurrentAzOrientation.Location = new System.Drawing.Point(425, 23);
            this.lblCurrentAzOrientation.Name = "lblCurrentAzOrientation";
            this.lblCurrentAzOrientation.Size = new System.Drawing.Size(164, 25);
            this.lblCurrentAzOrientation.TabIndex = 1;
            this.lblCurrentAzOrientation.Text = "Current Azimuth: ";
            // 
            // lblCurrentElOrientation
            // 
            this.lblCurrentElOrientation.AutoSize = true;
            this.lblCurrentElOrientation.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCurrentElOrientation.Location = new System.Drawing.Point(425, 57);
            this.lblCurrentElOrientation.Name = "lblCurrentElOrientation";
            this.lblCurrentElOrientation.Size = new System.Drawing.Size(173, 25);
            this.lblCurrentElOrientation.TabIndex = 2;
            this.lblCurrentElOrientation.Text = "Current Elevation: ";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(621, 23);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(39, 25);
            this.label3.TabIndex = 3;
            this.label3.Text = "0.0";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(621, 57);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(39, 25);
            this.label4.TabIndex = 4;
            this.label4.Text = "0.0";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(7, 260);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(192, 25);
            this.label5.TabIndex = 5;
            this.label5.Text = "Current Appointment";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(8, 299);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(87, 20);
            this.label6.TabIndex = 6;
            this.label6.Text = "Start Time";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.Location = new System.Drawing.Point(8, 364);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(80, 20);
            this.label7.TabIndex = 7;
            this.label7.Text = "End Time";
            // 
            // startTimeTextBox
            // 
            this.startTimeTextBox.Location = new System.Drawing.Point(12, 322);
            this.startTimeTextBox.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.startTimeTextBox.Name = "startTimeTextBox";
            this.startTimeTextBox.Size = new System.Drawing.Size(100, 22);
            this.startTimeTextBox.TabIndex = 8;
            // 
            // endTimeTextBox
            // 
            this.endTimeTextBox.Location = new System.Drawing.Point(12, 386);
            this.endTimeTextBox.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.endTimeTextBox.Name = "endTimeTextBox";
            this.endTimeTextBox.Size = new System.Drawing.Size(100, 22);
            this.endTimeTextBox.TabIndex = 9;
            // 
            // statusTextBox
            // 
            this.statusTextBox.Location = new System.Drawing.Point(155, 322);
            this.statusTextBox.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.statusTextBox.Name = "statusTextBox";
            this.statusTextBox.Size = new System.Drawing.Size(135, 22);
            this.statusTextBox.TabIndex = 10;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label8.Location = new System.Drawing.Point(149, 298);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(57, 20);
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
            this.lblAzimuthTemp.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblAzimuthTemp.Location = new System.Drawing.Point(705, 23);
            this.lblAzimuthTemp.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblAzimuthTemp.Name = "lblAzimuthTemp";
            this.lblAzimuthTemp.Size = new System.Drawing.Size(145, 25);
            this.lblAzimuthTemp.TabIndex = 14;
            this.lblAzimuthTemp.Text = "Azimuth Temp:";
            // 
            // lblElevationTemp
            // 
            this.lblElevationTemp.AutoSize = true;
            this.lblElevationTemp.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblElevationTemp.Location = new System.Drawing.Point(696, 60);
            this.lblElevationTemp.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblElevationTemp.Name = "lblElevationTemp";
            this.lblElevationTemp.Size = new System.Drawing.Size(154, 25);
            this.lblElevationTemp.TabIndex = 15;
            this.lblElevationTemp.Text = "Elevation Temp:";
            // 
            // radioButton1
            // 
            this.radioButton1.AutoSize = true;
            this.radioButton1.Location = new System.Drawing.Point(829, 110);
            this.radioButton1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.radioButton1.Name = "radioButton1";
            this.radioButton1.Size = new System.Drawing.Size(74, 21);
            this.radioButton1.TabIndex = 16;
            this.radioButton1.TabStop = true;
            this.radioButton1.Text = "Celcius";
            this.radioButton1.UseVisualStyleBackColor = true;
            // 
            // radioButton2
            // 
            this.radioButton2.AutoSize = true;
            this.radioButton2.Location = new System.Drawing.Point(701, 110);
            this.radioButton2.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.radioButton2.Name = "radioButton2";
            this.radioButton2.Size = new System.Drawing.Size(97, 21);
            this.radioButton2.TabIndex = 17;
            this.radioButton2.TabStop = true;
            this.radioButton2.Text = "Fahrenheit";
            this.radioButton2.UseVisualStyleBackColor = true;
            // 
            // fldAzTemp
            // 
            this.fldAzTemp.AutoSize = true;
            this.fldAzTemp.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.fldAzTemp.Location = new System.Drawing.Point(867, 23);
            this.fldAzTemp.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.fldAzTemp.Name = "fldAzTemp";
            this.fldAzTemp.Size = new System.Drawing.Size(39, 25);
            this.fldAzTemp.TabIndex = 18;
            this.fldAzTemp.Text = "0.0";
            // 
            // fldElTemp
            // 
            this.fldElTemp.AutoSize = true;
            this.fldElTemp.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.fldElTemp.Location = new System.Drawing.Point(867, 60);
            this.fldElTemp.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.fldElTemp.Name = "fldElTemp";
            this.fldElTemp.Size = new System.Drawing.Size(39, 25);
            this.fldElTemp.TabIndex = 19;
            this.fldElTemp.Text = "0.0";
            // 
            // txtTemperature
            // 
            this.txtTemperature.Location = new System.Drawing.Point(1111, 28);
            this.txtTemperature.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.txtTemperature.Name = "txtTemperature";
            this.txtTemperature.Size = new System.Drawing.Size(51, 22);
            this.txtTemperature.TabIndex = 20;
            // 
            // btnTest
            // 
            this.btnTest.Location = new System.Drawing.Point(1097, 65);
            this.btnTest.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnTest.Name = "btnTest";
            this.btnTest.Size = new System.Drawing.Size(79, 42);
            this.btnTest.TabIndex = 21;
            this.btnTest.Text = "Test Button";
            this.btnTest.UseVisualStyleBackColor = true;
            this.btnTest.Click += new System.EventHandler(this.btnTest_Click);
            // 
            // warningLabel
            // 
            this.warningLabel.AutoSize = true;
            this.warningLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.warningLabel.ForeColor = System.Drawing.Color.Chartreuse;
            this.warningLabel.Location = new System.Drawing.Point(941, 57);
            this.warningLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.warningLabel.Name = "warningLabel";
            this.warningLabel.Size = new System.Drawing.Size(120, 20);
            this.warningLabel.TabIndex = 22;
            this.warningLabel.Text = "warningLabel";
            this.warningLabel.Visible = false;
            // 
            // fanLabel
            // 
            this.fanLabel.AutoSize = true;
            this.fanLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.fanLabel.ForeColor = System.Drawing.Color.Chartreuse;
            this.fanLabel.Location = new System.Drawing.Point(956, 28);
            this.fanLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.fanLabel.Name = "fanLabel";
            this.fanLabel.Size = new System.Drawing.Size(81, 20);
            this.fanLabel.TabIndex = 23;
            this.fanLabel.Text = "fanLabel";
            this.fanLabel.Visible = false;
            // 
            // btnAddOneTemp
            // 
            this.btnAddOneTemp.Location = new System.Drawing.Point(695, 144);
            this.btnAddOneTemp.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnAddOneTemp.Name = "btnAddOneTemp";
            this.btnAddOneTemp.Size = new System.Drawing.Size(47, 43);
            this.btnAddOneTemp.TabIndex = 24;
            this.btnAddOneTemp.Text = "+1";
            this.btnAddOneTemp.UseVisualStyleBackColor = true;
            this.btnAddOneTemp.Click += new System.EventHandler(this.btnAddOneTemp_Click);
            // 
            // btnAddFiveTemp
            // 
            this.btnAddFiveTemp.Location = new System.Drawing.Point(765, 144);
            this.btnAddFiveTemp.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnAddFiveTemp.Name = "btnAddFiveTemp";
            this.btnAddFiveTemp.Size = new System.Drawing.Size(47, 43);
            this.btnAddFiveTemp.TabIndex = 25;
            this.btnAddFiveTemp.Text = "+5";
            this.btnAddFiveTemp.UseVisualStyleBackColor = true;
            this.btnAddFiveTemp.Click += new System.EventHandler(this.btnAddFiveTemp_Click);
            // 
            // btnAddXTemp
            // 
            this.btnAddXTemp.Location = new System.Drawing.Point(829, 144);
            this.btnAddXTemp.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnAddXTemp.Name = "btnAddXTemp";
            this.btnAddXTemp.Size = new System.Drawing.Size(47, 43);
            this.btnAddXTemp.TabIndex = 26;
            this.btnAddXTemp.Text = "+X";
            this.btnAddXTemp.UseVisualStyleBackColor = true;
            this.btnAddXTemp.Click += new System.EventHandler(this.btnAddXTemp_Click);
            // 
            // lblShutdown
            // 
            this.lblShutdown.AutoSize = true;
            this.lblShutdown.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblShutdown.ForeColor = System.Drawing.Color.Chartreuse;
            this.lblShutdown.Location = new System.Drawing.Point(941, 87);
            this.lblShutdown.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblShutdown.Name = "lblShutdown";
            this.lblShutdown.Size = new System.Drawing.Size(134, 20);
            this.lblShutdown.TabIndex = 27;
            this.lblShutdown.Text = "shutdownLabel";
            this.lblShutdown.Visible = false;
            // 
            // selectDemo
            // 
            this.selectDemo.AutoSize = true;
            this.selectDemo.Location = new System.Drawing.Point(960, 126);
            this.selectDemo.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.selectDemo.Name = "selectDemo";
            this.selectDemo.Size = new System.Drawing.Size(97, 21);
            this.selectDemo.TabIndex = 29;
            this.selectDemo.Text = "Run Demo";
            this.selectDemo.UseVisualStyleBackColor = true;
            // 
            // btnSubtractOneTemp
            // 
            this.btnSubtractOneTemp.Location = new System.Drawing.Point(695, 194);
            this.btnSubtractOneTemp.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnSubtractOneTemp.Name = "btnSubtractOneTemp";
            this.btnSubtractOneTemp.Size = new System.Drawing.Size(47, 43);
            this.btnSubtractOneTemp.TabIndex = 30;
            this.btnSubtractOneTemp.Text = "-1";
            this.btnSubtractOneTemp.UseVisualStyleBackColor = true;
            this.btnSubtractOneTemp.Click += new System.EventHandler(this.button1_Click);
            // 
            // btnSubtractFiveTemp
            // 
            this.btnSubtractFiveTemp.Location = new System.Drawing.Point(765, 194);
            this.btnSubtractFiveTemp.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnSubtractFiveTemp.Name = "btnSubtractFiveTemp";
            this.btnSubtractFiveTemp.Size = new System.Drawing.Size(47, 43);
            this.btnSubtractFiveTemp.TabIndex = 31;
            this.btnSubtractFiveTemp.Text = "-5";
            this.btnSubtractFiveTemp.UseVisualStyleBackColor = true;
            this.btnSubtractFiveTemp.Click += new System.EventHandler(this.button2_Click);
            // 
            // btnSubtractXTemp
            // 
            this.btnSubtractXTemp.Location = new System.Drawing.Point(829, 194);
            this.btnSubtractXTemp.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnSubtractXTemp.Name = "btnSubtractXTemp";
            this.btnSubtractXTemp.Size = new System.Drawing.Size(47, 43);
            this.btnSubtractXTemp.TabIndex = 32;
            this.btnSubtractXTemp.Text = "-X";
            this.btnSubtractXTemp.UseVisualStyleBackColor = true;
            this.btnSubtractXTemp.Click += new System.EventHandler(this.button3_Click);
            // 
            // txtCustTemp
            // 
            this.txtCustTemp.Location = new System.Drawing.Point(897, 194);
            this.txtCustTemp.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.txtCustTemp.Name = "txtCustTemp";
            this.txtCustTemp.Size = new System.Drawing.Size(67, 22);
            this.txtCustTemp.TabIndex = 33;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(884, 158);
            this.label11.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(95, 17);
            this.label11.TabIndex = 34;
            this.label11.Text = "Custom Value";
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.panel1.Controls.Add(this.lblElLimStatus2);
            this.panel1.Controls.Add(this.lblElLimStatus1);
            this.panel1.Controls.Add(this.lblAzLimStatus2);
            this.panel1.Controls.Add(this.lblAzLimStatus1);
            this.panel1.Controls.Add(this.lblElLimit2);
            this.panel1.Controls.Add(this.lblElLimit1);
            this.panel1.Controls.Add(this.lblAzLimit2);
            this.panel1.Controls.Add(this.lblAzLimit1);
            this.panel1.Controls.Add(this.lblEleProx2);
            this.panel1.Controls.Add(this.lblEleProx1);
            this.panel1.Controls.Add(this.lblElProx2);
            this.panel1.Controls.Add(this.lblElProx1);
            this.panel1.Controls.Add(this.lblAzProxStatus3);
            this.panel1.Controls.Add(this.lblAzProxStatus2);
            this.panel1.Controls.Add(this.lblAzProxStatus1);
            this.panel1.Controls.Add(this.lblAzProx3);
            this.panel1.Controls.Add(this.lblAzProx2);
            this.panel1.Controls.Add(this.lblAzProx1);
            this.panel1.Location = new System.Drawing.Point(660, 274);
            this.panel1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(503, 423);
            this.panel1.TabIndex = 35;
            // 
            // lblElLimStatus2
            // 
            this.lblElLimStatus2.AutoSize = true;
            this.lblElLimStatus2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblElLimStatus2.Location = new System.Drawing.Point(297, 380);
            this.lblElLimStatus2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblElLimStatus2.Name = "lblElLimStatus2";
            this.lblElLimStatus2.Size = new System.Drawing.Size(65, 18);
            this.lblElLimStatus2.TabIndex = 17;
            this.lblElLimStatus2.Text = "Inactive";
            // 
            // lblElLimStatus1
            // 
            this.lblElLimStatus1.AutoSize = true;
            this.lblElLimStatus1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblElLimStatus1.Location = new System.Drawing.Point(297, 334);
            this.lblElLimStatus1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblElLimStatus1.Name = "lblElLimStatus1";
            this.lblElLimStatus1.Size = new System.Drawing.Size(65, 18);
            this.lblElLimStatus1.TabIndex = 16;
            this.lblElLimStatus1.Text = "Inactive";
            // 
            // lblAzLimStatus2
            // 
            this.lblAzLimStatus2.AutoSize = true;
            this.lblAzLimStatus2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblAzLimStatus2.Location = new System.Drawing.Point(297, 284);
            this.lblAzLimStatus2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblAzLimStatus2.Name = "lblAzLimStatus2";
            this.lblAzLimStatus2.Size = new System.Drawing.Size(65, 18);
            this.lblAzLimStatus2.TabIndex = 15;
            this.lblAzLimStatus2.Text = "Inactive";
            this.lblAzLimStatus2.Click += new System.EventHandler(this.lblAzLimStatus2_Click);
            // 
            // lblAzLimStatus1
            // 
            this.lblAzLimStatus1.AutoSize = true;
            this.lblAzLimStatus1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblAzLimStatus1.Location = new System.Drawing.Point(297, 238);
            this.lblAzLimStatus1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblAzLimStatus1.Name = "lblAzLimStatus1";
            this.lblAzLimStatus1.Size = new System.Drawing.Size(65, 18);
            this.lblAzLimStatus1.TabIndex = 14;
            this.lblAzLimStatus1.Text = "Inactive";
            // 
            // lblElLimit2
            // 
            this.lblElLimit2.AutoSize = true;
            this.lblElLimit2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblElLimit2.Location = new System.Drawing.Point(19, 380);
            this.lblElLimit2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblElLimit2.Name = "lblElLimit2";
            this.lblElLimit2.Size = new System.Drawing.Size(187, 18);
            this.lblElLimit2.TabIndex = 13;
            this.lblElLimit2.Text = "Elevation Limit Switch 2";
            // 
            // lblElLimit1
            // 
            this.lblElLimit1.AutoSize = true;
            this.lblElLimit1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblElLimit1.Location = new System.Drawing.Point(19, 334);
            this.lblElLimit1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblElLimit1.Name = "lblElLimit1";
            this.lblElLimit1.Size = new System.Drawing.Size(187, 18);
            this.lblElLimit1.TabIndex = 12;
            this.lblElLimit1.Text = "Elevation Limit Switch 1";
            // 
            // lblAzLimit2
            // 
            this.lblAzLimit2.AutoSize = true;
            this.lblAzLimit2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblAzLimit2.Location = new System.Drawing.Point(19, 284);
            this.lblAzLimit2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblAzLimit2.Name = "lblAzLimit2";
            this.lblAzLimit2.Size = new System.Drawing.Size(178, 18);
            this.lblAzLimit2.TabIndex = 11;
            this.lblAzLimit2.Text = "Azimuth Limit Switch 1";
            // 
            // lblAzLimit1
            // 
            this.lblAzLimit1.AutoSize = true;
            this.lblAzLimit1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblAzLimit1.Location = new System.Drawing.Point(19, 238);
            this.lblAzLimit1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblAzLimit1.Name = "lblAzLimit1";
            this.lblAzLimit1.Size = new System.Drawing.Size(178, 18);
            this.lblAzLimit1.TabIndex = 10;
            this.lblAzLimit1.Text = "Azimuth Limit Switch 1";
            // 
            // lblEleProx2
            // 
            this.lblEleProx2.AutoSize = true;
            this.lblEleProx2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblEleProx2.Location = new System.Drawing.Point(297, 191);
            this.lblEleProx2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblEleProx2.Name = "lblEleProx2";
            this.lblEleProx2.Size = new System.Drawing.Size(65, 18);
            this.lblEleProx2.TabIndex = 9;
            this.lblEleProx2.Text = "Inactive";
            // 
            // lblEleProx1
            // 
            this.lblEleProx1.AutoSize = true;
            this.lblEleProx1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblEleProx1.Location = new System.Drawing.Point(297, 145);
            this.lblEleProx1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblEleProx1.Name = "lblEleProx1";
            this.lblEleProx1.Size = new System.Drawing.Size(65, 18);
            this.lblEleProx1.TabIndex = 8;
            this.lblEleProx1.Text = "Inactive";
            // 
            // lblElProx2
            // 
            this.lblElProx2.AutoSize = true;
            this.lblElProx2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblElProx2.Location = new System.Drawing.Point(19, 191);
            this.lblElProx2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblElProx2.Name = "lblElProx2";
            this.lblElProx2.Size = new System.Drawing.Size(225, 18);
            this.lblElProx2.TabIndex = 7;
            this.lblElProx2.Text = "Elevation Proximity Sensor 2";
            // 
            // lblElProx1
            // 
            this.lblElProx1.AutoSize = true;
            this.lblElProx1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblElProx1.Location = new System.Drawing.Point(19, 145);
            this.lblElProx1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblElProx1.Name = "lblElProx1";
            this.lblElProx1.Size = new System.Drawing.Size(225, 18);
            this.lblElProx1.TabIndex = 6;
            this.lblElProx1.Text = "Elevation Proximity Sensor 1";
            // 
            // lblAzProxStatus3
            // 
            this.lblAzProxStatus3.AutoSize = true;
            this.lblAzProxStatus3.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblAzProxStatus3.Location = new System.Drawing.Point(297, 102);
            this.lblAzProxStatus3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblAzProxStatus3.Name = "lblAzProxStatus3";
            this.lblAzProxStatus3.Size = new System.Drawing.Size(65, 18);
            this.lblAzProxStatus3.TabIndex = 5;
            this.lblAzProxStatus3.Text = "Inactive";
            // 
            // lblAzProxStatus2
            // 
            this.lblAzProxStatus2.AutoSize = true;
            this.lblAzProxStatus2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblAzProxStatus2.Location = new System.Drawing.Point(297, 62);
            this.lblAzProxStatus2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblAzProxStatus2.Name = "lblAzProxStatus2";
            this.lblAzProxStatus2.Size = new System.Drawing.Size(65, 18);
            this.lblAzProxStatus2.TabIndex = 4;
            this.lblAzProxStatus2.Text = "Inactive";
            // 
            // lblAzProxStatus1
            // 
            this.lblAzProxStatus1.AutoSize = true;
            this.lblAzProxStatus1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblAzProxStatus1.Location = new System.Drawing.Point(297, 22);
            this.lblAzProxStatus1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblAzProxStatus1.Name = "lblAzProxStatus1";
            this.lblAzProxStatus1.Size = new System.Drawing.Size(65, 18);
            this.lblAzProxStatus1.TabIndex = 3;
            this.lblAzProxStatus1.Text = "Inactive";
            // 
            // lblAzProx3
            // 
            this.lblAzProx3.AutoSize = true;
            this.lblAzProx3.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblAzProx3.Location = new System.Drawing.Point(19, 102);
            this.lblAzProx3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblAzProx3.Name = "lblAzProx3";
            this.lblAzProx3.Size = new System.Drawing.Size(216, 18);
            this.lblAzProx3.TabIndex = 2;
            this.lblAzProx3.Text = "Azimuth Proximity Sensor 3";
            // 
            // lblAzProx2
            // 
            this.lblAzProx2.AutoSize = true;
            this.lblAzProx2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblAzProx2.Location = new System.Drawing.Point(19, 62);
            this.lblAzProx2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblAzProx2.Name = "lblAzProx2";
            this.lblAzProx2.Size = new System.Drawing.Size(216, 18);
            this.lblAzProx2.TabIndex = 1;
            this.lblAzProx2.Text = "Azimuth Proximity Sensor 2";
            // 
            // lblAzProx1
            // 
            this.lblAzProx1.AutoSize = true;
            this.lblAzProx1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblAzProx1.Location = new System.Drawing.Point(19, 22);
            this.lblAzProx1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblAzProx1.Name = "lblAzProx1";
            this.lblAzProx1.Size = new System.Drawing.Size(216, 18);
            this.lblAzProx1.TabIndex = 0;
            this.lblAzProx1.Text = "Azimuth Proximity Sensor 1";
            // 
            // lblAbsEncoder
            // 
            this.lblAbsEncoder.AutoSize = true;
            this.lblAbsEncoder.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblAbsEncoder.Location = new System.Drawing.Point(43, 452);
            this.lblAbsEncoder.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblAbsEncoder.Name = "lblAbsEncoder";
            this.lblAbsEncoder.Size = new System.Drawing.Size(136, 18);
            this.lblAbsEncoder.TabIndex = 36;
            this.lblAbsEncoder.Text = "Azimuth Encoder";
            // 
            // lblEncoderDegrees
            // 
            this.lblEncoderDegrees.AutoSize = true;
            this.lblEncoderDegrees.Location = new System.Drawing.Point(43, 485);
            this.lblEncoderDegrees.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblEncoderDegrees.Name = "lblEncoderDegrees";
            this.lblEncoderDegrees.Size = new System.Drawing.Size(62, 17);
            this.lblEncoderDegrees.TabIndex = 37;
            this.lblEncoderDegrees.Text = "Degrees";
            // 
            // lblAzEncoderDegrees
            // 
            this.lblAzEncoderDegrees.AutoSize = true;
            this.lblAzEncoderDegrees.Location = new System.Drawing.Point(128, 485);
            this.lblAzEncoderDegrees.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblAzEncoderDegrees.Name = "lblAzEncoderDegrees";
            this.lblAzEncoderDegrees.Size = new System.Drawing.Size(16, 17);
            this.lblAzEncoderDegrees.TabIndex = 38;
            this.lblAzEncoderDegrees.Text = "0";
            // 
            // lblEncoderTicks
            // 
            this.lblEncoderTicks.AutoSize = true;
            this.lblEncoderTicks.Location = new System.Drawing.Point(41, 513);
            this.lblEncoderTicks.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblEncoderTicks.Name = "lblEncoderTicks";
            this.lblEncoderTicks.Size = new System.Drawing.Size(41, 17);
            this.lblEncoderTicks.TabIndex = 39;
            this.lblEncoderTicks.Text = "Ticks";
            // 
            // lblAzEncoderTicks
            // 
            this.lblAzEncoderTicks.AutoSize = true;
            this.lblAzEncoderTicks.Location = new System.Drawing.Point(128, 513);
            this.lblAzEncoderTicks.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblAzEncoderTicks.Name = "lblAzEncoderTicks";
            this.lblAzEncoderTicks.Size = new System.Drawing.Size(16, 17);
            this.lblAzEncoderTicks.TabIndex = 40;
            this.lblAzEncoderTicks.Text = "0";
            // 
            // btnAddOneEncoder
            // 
            this.btnAddOneEncoder.Location = new System.Drawing.Point(13, 538);
            this.btnAddOneEncoder.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnAddOneEncoder.Name = "btnAddOneEncoder";
            this.btnAddOneEncoder.Size = new System.Drawing.Size(47, 43);
            this.btnAddOneEncoder.TabIndex = 41;
            this.btnAddOneEncoder.Text = "+1";
            this.btnAddOneEncoder.UseVisualStyleBackColor = true;
            this.btnAddOneEncoder.Click += new System.EventHandler(this.button4_Click);
            // 
            // btnAddFiveEncoder
            // 
            this.btnAddFiveEncoder.Location = new System.Drawing.Point(72, 538);
            this.btnAddFiveEncoder.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnAddFiveEncoder.Name = "btnAddFiveEncoder";
            this.btnAddFiveEncoder.Size = new System.Drawing.Size(47, 43);
            this.btnAddFiveEncoder.TabIndex = 42;
            this.btnAddFiveEncoder.Text = "+5";
            this.btnAddFiveEncoder.UseVisualStyleBackColor = true;
            this.btnAddFiveEncoder.Click += new System.EventHandler(this.button5_Click);
            // 
            // btnAddXEncoder
            // 
            this.btnAddXEncoder.Location = new System.Drawing.Point(128, 538);
            this.btnAddXEncoder.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnAddXEncoder.Name = "btnAddXEncoder";
            this.btnAddXEncoder.Size = new System.Drawing.Size(47, 43);
            this.btnAddXEncoder.TabIndex = 43;
            this.btnAddXEncoder.Text = "+X";
            this.btnAddXEncoder.UseVisualStyleBackColor = true;
            this.btnAddXEncoder.Click += new System.EventHandler(this.btnAddXEncoder_Click);
            // 
            // btnSubtractOneEncoder
            // 
            this.btnSubtractOneEncoder.Location = new System.Drawing.Point(12, 588);
            this.btnSubtractOneEncoder.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnSubtractOneEncoder.Name = "btnSubtractOneEncoder";
            this.btnSubtractOneEncoder.Size = new System.Drawing.Size(47, 43);
            this.btnSubtractOneEncoder.TabIndex = 44;
            this.btnSubtractOneEncoder.Text = "-1";
            this.btnSubtractOneEncoder.UseVisualStyleBackColor = true;
            this.btnSubtractOneEncoder.Click += new System.EventHandler(this.btnSubtractOneEncoder_Click);
            // 
            // btnSubtractFiveEncoder
            // 
            this.btnSubtractFiveEncoder.Location = new System.Drawing.Point(72, 588);
            this.btnSubtractFiveEncoder.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnSubtractFiveEncoder.Name = "btnSubtractFiveEncoder";
            this.btnSubtractFiveEncoder.Size = new System.Drawing.Size(47, 43);
            this.btnSubtractFiveEncoder.TabIndex = 45;
            this.btnSubtractFiveEncoder.Text = "-5";
            this.btnSubtractFiveEncoder.UseVisualStyleBackColor = true;
            this.btnSubtractFiveEncoder.Click += new System.EventHandler(this.btnSubtractFiveEncoder_Click);
            // 
            // btnSubtractXEncoder
            // 
            this.btnSubtractXEncoder.Location = new System.Drawing.Point(127, 588);
            this.btnSubtractXEncoder.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnSubtractXEncoder.Name = "btnSubtractXEncoder";
            this.btnSubtractXEncoder.Size = new System.Drawing.Size(47, 43);
            this.btnSubtractXEncoder.TabIndex = 46;
            this.btnSubtractXEncoder.Text = "-X";
            this.btnSubtractXEncoder.UseVisualStyleBackColor = true;
            this.btnSubtractXEncoder.Click += new System.EventHandler(this.btnSubtractXEncoder_Click);
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(183, 551);
            this.label9.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(95, 17);
            this.label9.TabIndex = 47;
            this.label9.Text = "Custom Value";
            // 
            // txtCustEncoderVal
            // 
            this.txtCustEncoderVal.Location = new System.Drawing.Point(187, 588);
            this.txtCustEncoderVal.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.txtCustEncoderVal.Name = "txtCustEncoderVal";
            this.txtCustEncoderVal.Size = new System.Drawing.Size(67, 22);
            this.txtCustEncoderVal.TabIndex = 48;
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(504, 588);
            this.textBox1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(67, 22);
            this.textBox1.TabIndex = 56;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(500, 551);
            this.label10.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(95, 17);
            this.label10.TabIndex = 55;
            this.label10.Text = "Custom Value";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(444, 588);
            this.button1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(47, 43);
            this.button1.TabIndex = 54;
            this.button1.Text = "-X";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(389, 588);
            this.button2.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(47, 43);
            this.button2.TabIndex = 53;
            this.button2.Text = "-5";
            this.button2.UseVisualStyleBackColor = true;
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(329, 588);
            this.button3.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(47, 43);
            this.button3.TabIndex = 52;
            this.button3.Text = "-1";
            this.button3.UseVisualStyleBackColor = true;
            // 
            // button4
            // 
            this.button4.Location = new System.Drawing.Point(445, 538);
            this.button4.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(47, 43);
            this.button4.TabIndex = 51;
            this.button4.Text = "+X";
            this.button4.UseVisualStyleBackColor = true;
            // 
            // button5
            // 
            this.button5.Location = new System.Drawing.Point(389, 538);
            this.button5.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.button5.Name = "button5";
            this.button5.Size = new System.Drawing.Size(47, 43);
            this.button5.TabIndex = 50;
            this.button5.Text = "+5";
            this.button5.UseVisualStyleBackColor = true;
            // 
            // button6
            // 
            this.button6.Location = new System.Drawing.Point(331, 538);
            this.button6.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.button6.Name = "button6";
            this.button6.Size = new System.Drawing.Size(47, 43);
            this.button6.TabIndex = 49;
            this.button6.Text = "+1";
            this.button6.UseVisualStyleBackColor = true;
            // 
            // lblElEncoderTicks
            // 
            this.lblElEncoderTicks.AutoSize = true;
            this.lblElEncoderTicks.Location = new System.Drawing.Point(455, 513);
            this.lblElEncoderTicks.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblElEncoderTicks.Name = "lblElEncoderTicks";
            this.lblElEncoderTicks.Size = new System.Drawing.Size(16, 17);
            this.lblElEncoderTicks.TabIndex = 61;
            this.lblElEncoderTicks.Text = "0";
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(368, 513);
            this.label14.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(41, 17);
            this.label14.TabIndex = 60;
            this.label14.Text = "Ticks";
            // 
            // lblElEncoderDegrees
            // 
            this.lblElEncoderDegrees.AutoSize = true;
            this.lblElEncoderDegrees.Location = new System.Drawing.Point(455, 485);
            this.lblElEncoderDegrees.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblElEncoderDegrees.Name = "lblElEncoderDegrees";
            this.lblElEncoderDegrees.Size = new System.Drawing.Size(16, 17);
            this.lblElEncoderDegrees.TabIndex = 59;
            this.lblElEncoderDegrees.Text = "0";
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Location = new System.Drawing.Point(369, 485);
            this.label16.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(62, 17);
            this.label16.TabIndex = 58;
            this.label16.Text = "Degrees";
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label17.Location = new System.Drawing.Point(369, 452);
            this.label17.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(145, 18);
            this.label17.TabIndex = 57;
            this.label17.Text = "Elevation Encoder";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(16, 668);
            this.label12.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(134, 17);
            this.label12.TabIndex = 62;
            this.label12.Text = "Set Bits of Precision";
            // 
            // textBox2
            // 
            this.textBox2.Location = new System.Drawing.Point(40, 688);
            this.textBox2.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new System.Drawing.Size(51, 22);
            this.textBox2.TabIndex = 63;
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(35, 716);
            this.label13.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(65, 17);
            this.label13.TabIndex = 64;
            this.label13.Text = "Set Error";
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Location = new System.Drawing.Point(1544, 91);
            this.label15.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(0, 17);
            this.label15.TabIndex = 65;
            // 
            // textBox3
            // 
            this.textBox3.Location = new System.Drawing.Point(39, 736);
            this.textBox3.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.textBox3.Name = "textBox3";
            this.textBox3.Size = new System.Drawing.Size(52, 22);
            this.textBox3.TabIndex = 66;
            // 
            // textBox4
            // 
            this.textBox4.Location = new System.Drawing.Point(40, 784);
            this.textBox4.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.textBox4.Name = "textBox4";
            this.textBox4.Size = new System.Drawing.Size(51, 22);
            this.textBox4.TabIndex = 67;
            // 
            // label18
            // 
            this.label18.AutoSize = true;
            this.label18.Location = new System.Drawing.Point(21, 764);
            this.label18.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(83, 17);
            this.label18.TabIndex = 68;
            this.label18.Text = "Set Position";
            // 
            // label19
            // 
            this.label19.AutoSize = true;
            this.label19.Location = new System.Drawing.Point(217, 420);
            this.label19.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label19.Name = "label19";
            this.label19.Size = new System.Drawing.Size(130, 17);
            this.label19.TabIndex = 69;
            this.label19.Text = "Encoder Simulation";
            // 
            // label21
            // 
            this.label21.AutoSize = true;
            this.label21.Location = new System.Drawing.Point(327, 657);
            this.label21.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label21.Name = "label21";
            this.label21.Size = new System.Drawing.Size(121, 17);
            this.label21.TabIndex = 71;
            this.label21.Text = "Has Active Move?";
            // 
            // label22
            // 
            this.label22.AutoSize = true;
            this.label22.Location = new System.Drawing.Point(461, 657);
            this.label22.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label22.Name = "label22";
            this.label22.Size = new System.Drawing.Size(38, 17);
            this.label22.TabIndex = 72;
            this.label22.Text = "True";
            // 
            // PLC_regs
            // 
            this.PLC_regs.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.PLC_regs.Location = new System.Drawing.Point(1257, 12);
            this.PLC_regs.Name = "PLC_regs";
            this.PLC_regs.RowTemplate.Height = 24;
            this.PLC_regs.Size = new System.Drawing.Size(514, 685);
            this.PLC_regs.TabIndex = 73;
            // 
            // DiagnosticsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1892, 839);
            this.Controls.Add(this.PLC_regs);
            this.Controls.Add(this.label22);
            this.Controls.Add(this.label21);
            this.Controls.Add(this.label19);
            this.Controls.Add(this.label18);
            this.Controls.Add(this.textBox4);
            this.Controls.Add(this.textBox3);
            this.Controls.Add(this.label15);
            this.Controls.Add(this.label13);
            this.Controls.Add(this.textBox2);
            this.Controls.Add(this.label12);
            this.Controls.Add(this.lblElEncoderTicks);
            this.Controls.Add(this.label14);
            this.Controls.Add(this.lblElEncoderDegrees);
            this.Controls.Add(this.label16);
            this.Controls.Add(this.label17);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.button4);
            this.Controls.Add(this.button5);
            this.Controls.Add(this.button6);
            this.Controls.Add(this.txtCustEncoderVal);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.btnSubtractXEncoder);
            this.Controls.Add(this.btnSubtractFiveEncoder);
            this.Controls.Add(this.btnSubtractOneEncoder);
            this.Controls.Add(this.btnAddXEncoder);
            this.Controls.Add(this.btnAddFiveEncoder);
            this.Controls.Add(this.btnAddOneEncoder);
            this.Controls.Add(this.lblAzEncoderTicks);
            this.Controls.Add(this.lblEncoderTicks);
            this.Controls.Add(this.lblAzEncoderDegrees);
            this.Controls.Add(this.lblEncoderDegrees);
            this.Controls.Add(this.lblAbsEncoder);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.txtCustTemp);
            this.Controls.Add(this.btnSubtractXTemp);
            this.Controls.Add(this.btnSubtractFiveTemp);
            this.Controls.Add(this.btnSubtractOneTemp);
            this.Controls.Add(this.selectDemo);
            this.Controls.Add(this.lblShutdown);
            this.Controls.Add(this.btnAddXTemp);
            this.Controls.Add(this.btnAddFiveTemp);
            this.Controls.Add(this.btnAddOneTemp);
            this.Controls.Add(this.fanLabel);
            this.Controls.Add(this.warningLabel);
            this.Controls.Add(this.btnTest);
            this.Controls.Add(this.txtTemperature);
            this.Controls.Add(this.fldElTemp);
            this.Controls.Add(this.fldAzTemp);
            this.Controls.Add(this.radioButton2);
            this.Controls.Add(this.radioButton1);
            this.Controls.Add(this.lblElevationTemp);
            this.Controls.Add(this.lblAzimuthTemp);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.statusTextBox);
            this.Controls.Add(this.endTimeTextBox);
            this.Controls.Add(this.startTimeTextBox);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.lblCurrentElOrientation);
            this.Controls.Add(this.lblCurrentAzOrientation);
            this.Controls.Add(this.dataGridView1);
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.MinimumSize = new System.Drawing.Size(814, 296);
            this.Name = "DiagnosticsForm";
            this.Text = "DiagnosticsForm";
            this.Load += new System.EventHandler(this.DiagnosticsForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.PLC_regs)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.Label lblCurrentAzOrientation;
        private System.Windows.Forms.Label lblCurrentElOrientation;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox startTimeTextBox;
        private System.Windows.Forms.TextBox endTimeTextBox;
        private System.Windows.Forms.TextBox statusTextBox;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Label lblAzimuthTemp;
        private System.Windows.Forms.Label lblElevationTemp;
        private System.Windows.Forms.RadioButton radioButton1;
        private System.Windows.Forms.RadioButton radioButton2;
        private System.Windows.Forms.Label fldAzTemp;
        private System.Windows.Forms.Label fldElTemp;
        private System.Windows.Forms.TextBox txtTemperature;
        private System.Windows.Forms.Button btnTest;
        private System.Windows.Forms.Label warningLabel;
        private System.Windows.Forms.Label fanLabel;
        private System.Windows.Forms.Button btnAddOneTemp;
        private System.Windows.Forms.Button btnAddFiveTemp;
        private System.Windows.Forms.Button btnAddXTemp;
        private System.Windows.Forms.Label lblShutdown;
        private System.Windows.Forms.CheckBox selectDemo;
        private System.Windows.Forms.Button btnSubtractOneTemp;
        private System.Windows.Forms.Button btnSubtractFiveTemp;
        private System.Windows.Forms.Button btnSubtractXTemp;
        private System.Windows.Forms.TextBox txtCustTemp;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label lblElLimStatus2;
        private System.Windows.Forms.Label lblElLimStatus1;
        private System.Windows.Forms.Label lblAzLimStatus2;
        private System.Windows.Forms.Label lblAzLimStatus1;
        private System.Windows.Forms.Label lblElLimit2;
        private System.Windows.Forms.Label lblElLimit1;
        private System.Windows.Forms.Label lblAzLimit2;
        private System.Windows.Forms.Label lblAzLimit1;
        private System.Windows.Forms.Label lblEleProx2;
        private System.Windows.Forms.Label lblEleProx1;
        private System.Windows.Forms.Label lblElProx2;
        private System.Windows.Forms.Label lblElProx1;
        private System.Windows.Forms.Label lblAzProxStatus3;
        private System.Windows.Forms.Label lblAzProxStatus2;
        private System.Windows.Forms.Label lblAzProxStatus1;
        private System.Windows.Forms.Label lblAzProx3;
        private System.Windows.Forms.Label lblAzProx2;
        private System.Windows.Forms.Label lblAzProx1;
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
        private System.Windows.Forms.Label label19;
        private System.Windows.Forms.Label label21;
        private System.Windows.Forms.Label label22;
        private System.Windows.Forms.DataGridView PLC_regs;
    }
}