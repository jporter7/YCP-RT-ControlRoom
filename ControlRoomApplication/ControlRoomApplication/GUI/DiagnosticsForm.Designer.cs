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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
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
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
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
            this.lblVibrationStatus = new System.Windows.Forms.Label();
            this.lblAbsEncoder = new System.Windows.Forms.Label();
            this.lblEncoderDegrees = new System.Windows.Forms.Label();
            this.lblDisplayDegreesEncoders = new System.Windows.Forms.Label();
            this.lblEncoderTicks = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.button4 = new System.Windows.Forms.Button();
            this.button5 = new System.Windows.Forms.Button();
            this.button6 = new System.Windows.Forms.Button();
            this.button7 = new System.Windows.Forms.Button();
            this.button8 = new System.Windows.Forms.Button();
            this.button9 = new System.Windows.Forms.Button();
            this.label9 = new System.Windows.Forms.Label();
            this.textBox1 = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToDeleteRows = false;
            this.dataGridView1.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dataGridView1.DefaultCellStyle = dataGridViewCellStyle2;
            this.dataGridView1.Location = new System.Drawing.Point(9, 10);
            this.dataGridView1.Margin = new System.Windows.Forms.Padding(2);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowTemplate.Height = 24;
            this.dataGridView1.Size = new System.Drawing.Size(290, 178);
            this.dataGridView1.TabIndex = 0;
            this.dataGridView1.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellContentClick);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(319, 19);
            this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(132, 20);
            this.label1.TabIndex = 1;
            this.label1.Text = "Current Azimuth: ";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(319, 46);
            this.label2.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(139, 20);
            this.label2.TabIndex = 2;
            this.label2.Text = "Current Elevation: ";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(466, 19);
            this.label3.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(31, 20);
            this.label3.TabIndex = 3;
            this.label3.Text = "0.0";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(466, 46);
            this.label4.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(31, 20);
            this.label4.TabIndex = 4;
            this.label4.Text = "0.0";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(5, 211);
            this.label5.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(157, 20);
            this.label5.TabIndex = 5;
            this.label5.Text = "Current Appointment";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(6, 243);
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
            this.label7.Location = new System.Drawing.Point(6, 296);
            this.label7.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(68, 17);
            this.label7.TabIndex = 7;
            this.label7.Text = "End Time";
            // 
            // startTimeTextBox
            // 
            this.startTimeTextBox.Location = new System.Drawing.Point(9, 262);
            this.startTimeTextBox.Margin = new System.Windows.Forms.Padding(2);
            this.startTimeTextBox.Name = "startTimeTextBox";
            this.startTimeTextBox.Size = new System.Drawing.Size(76, 20);
            this.startTimeTextBox.TabIndex = 8;
            // 
            // endTimeTextBox
            // 
            this.endTimeTextBox.Location = new System.Drawing.Point(9, 314);
            this.endTimeTextBox.Margin = new System.Windows.Forms.Padding(2);
            this.endTimeTextBox.Name = "endTimeTextBox";
            this.endTimeTextBox.Size = new System.Drawing.Size(76, 20);
            this.endTimeTextBox.TabIndex = 9;
            // 
            // statusTextBox
            // 
            this.statusTextBox.Location = new System.Drawing.Point(116, 262);
            this.statusTextBox.Margin = new System.Windows.Forms.Padding(2);
            this.statusTextBox.Name = "statusTextBox";
            this.statusTextBox.Size = new System.Drawing.Size(102, 20);
            this.statusTextBox.TabIndex = 10;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label8.Location = new System.Drawing.Point(112, 242);
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
            this.lblAzimuthTemp.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblAzimuthTemp.Location = new System.Drawing.Point(529, 19);
            this.lblAzimuthTemp.Name = "lblAzimuthTemp";
            this.lblAzimuthTemp.Size = new System.Drawing.Size(115, 20);
            this.lblAzimuthTemp.TabIndex = 14;
            this.lblAzimuthTemp.Text = "Azimuth Temp:";
            // 
            // lblElevationTemp
            // 
            this.lblElevationTemp.AutoSize = true;
            this.lblElevationTemp.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblElevationTemp.Location = new System.Drawing.Point(522, 49);
            this.lblElevationTemp.Name = "lblElevationTemp";
            this.lblElevationTemp.Size = new System.Drawing.Size(122, 20);
            this.lblElevationTemp.TabIndex = 15;
            this.lblElevationTemp.Text = "Elevation Temp:";
            // 
            // radioButton1
            // 
            this.radioButton1.AutoSize = true;
            this.radioButton1.Location = new System.Drawing.Point(622, 89);
            this.radioButton1.Name = "radioButton1";
            this.radioButton1.Size = new System.Drawing.Size(59, 17);
            this.radioButton1.TabIndex = 16;
            this.radioButton1.TabStop = true;
            this.radioButton1.Text = "Celcius";
            this.radioButton1.UseVisualStyleBackColor = true;
            // 
            // radioButton2
            // 
            this.radioButton2.AutoSize = true;
            this.radioButton2.Location = new System.Drawing.Point(526, 89);
            this.radioButton2.Name = "radioButton2";
            this.radioButton2.Size = new System.Drawing.Size(75, 17);
            this.radioButton2.TabIndex = 17;
            this.radioButton2.TabStop = true;
            this.radioButton2.Text = "Fahrenheit";
            this.radioButton2.UseVisualStyleBackColor = true;
            // 
            // fldAzTemp
            // 
            this.fldAzTemp.AutoSize = true;
            this.fldAzTemp.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.fldAzTemp.Location = new System.Drawing.Point(650, 19);
            this.fldAzTemp.Name = "fldAzTemp";
            this.fldAzTemp.Size = new System.Drawing.Size(31, 20);
            this.fldAzTemp.TabIndex = 18;
            this.fldAzTemp.Text = "0.0";
            // 
            // fldElTemp
            // 
            this.fldElTemp.AutoSize = true;
            this.fldElTemp.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.fldElTemp.Location = new System.Drawing.Point(650, 49);
            this.fldElTemp.Name = "fldElTemp";
            this.fldElTemp.Size = new System.Drawing.Size(31, 20);
            this.fldElTemp.TabIndex = 19;
            this.fldElTemp.Text = "0.0";
            // 
            // txtTemperature
            // 
            this.txtTemperature.Location = new System.Drawing.Point(833, 23);
            this.txtTemperature.Name = "txtTemperature";
            this.txtTemperature.Size = new System.Drawing.Size(39, 20);
            this.txtTemperature.TabIndex = 20;
            // 
            // btnTest
            // 
            this.btnTest.Location = new System.Drawing.Point(823, 53);
            this.btnTest.Name = "btnTest";
            this.btnTest.Size = new System.Drawing.Size(59, 34);
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
            this.warningLabel.Location = new System.Drawing.Point(706, 46);
            this.warningLabel.Name = "warningLabel";
            this.warningLabel.Size = new System.Drawing.Size(100, 16);
            this.warningLabel.TabIndex = 22;
            this.warningLabel.Text = "warningLabel";
            this.warningLabel.Visible = false;
            // 
            // fanLabel
            // 
            this.fanLabel.AutoSize = true;
            this.fanLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.fanLabel.ForeColor = System.Drawing.Color.Chartreuse;
            this.fanLabel.Location = new System.Drawing.Point(717, 23);
            this.fanLabel.Name = "fanLabel";
            this.fanLabel.Size = new System.Drawing.Size(68, 16);
            this.fanLabel.TabIndex = 23;
            this.fanLabel.Text = "fanLabel";
            this.fanLabel.Visible = false;
            // 
            // btnAddOneTemp
            // 
            this.btnAddOneTemp.Location = new System.Drawing.Point(521, 117);
            this.btnAddOneTemp.Name = "btnAddOneTemp";
            this.btnAddOneTemp.Size = new System.Drawing.Size(35, 35);
            this.btnAddOneTemp.TabIndex = 24;
            this.btnAddOneTemp.Text = "+1";
            this.btnAddOneTemp.UseVisualStyleBackColor = true;
            this.btnAddOneTemp.Click += new System.EventHandler(this.btnAddOneTemp_Click);
            // 
            // btnAddFiveTemp
            // 
            this.btnAddFiveTemp.Location = new System.Drawing.Point(574, 117);
            this.btnAddFiveTemp.Name = "btnAddFiveTemp";
            this.btnAddFiveTemp.Size = new System.Drawing.Size(35, 35);
            this.btnAddFiveTemp.TabIndex = 25;
            this.btnAddFiveTemp.Text = "+5";
            this.btnAddFiveTemp.UseVisualStyleBackColor = true;
            this.btnAddFiveTemp.Click += new System.EventHandler(this.btnAddFiveTemp_Click);
            // 
            // btnAddXTemp
            // 
            this.btnAddXTemp.Location = new System.Drawing.Point(622, 117);
            this.btnAddXTemp.Name = "btnAddXTemp";
            this.btnAddXTemp.Size = new System.Drawing.Size(35, 35);
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
            this.lblShutdown.Location = new System.Drawing.Point(706, 71);
            this.lblShutdown.Name = "lblShutdown";
            this.lblShutdown.Size = new System.Drawing.Size(111, 16);
            this.lblShutdown.TabIndex = 27;
            this.lblShutdown.Text = "shutdownLabel";
            this.lblShutdown.Visible = false;
            // 
            // selectDemo
            // 
            this.selectDemo.AutoSize = true;
            this.selectDemo.Location = new System.Drawing.Point(720, 102);
            this.selectDemo.Name = "selectDemo";
            this.selectDemo.Size = new System.Drawing.Size(77, 17);
            this.selectDemo.TabIndex = 29;
            this.selectDemo.Text = "Run Demo";
            this.selectDemo.UseVisualStyleBackColor = true;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(521, 158);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(35, 35);
            this.button1.TabIndex = 30;
            this.button1.Text = "-1";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(574, 158);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(35, 35);
            this.button2.TabIndex = 31;
            this.button2.Text = "-5";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(622, 158);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(35, 35);
            this.button3.TabIndex = 32;
            this.button3.Text = "-X";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // txtCustTemp
            // 
            this.txtCustTemp.Location = new System.Drawing.Point(673, 158);
            this.txtCustTemp.Name = "txtCustTemp";
            this.txtCustTemp.Size = new System.Drawing.Size(51, 20);
            this.txtCustTemp.TabIndex = 33;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(663, 128);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(72, 13);
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
            this.panel1.Location = new System.Drawing.Point(528, 264);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(377, 344);
            this.panel1.TabIndex = 35;
            // 
            // lblElLimStatus2
            // 
            this.lblElLimStatus2.AutoSize = true;
            this.lblElLimStatus2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblElLimStatus2.Location = new System.Drawing.Point(223, 309);
            this.lblElLimStatus2.Name = "lblElLimStatus2";
            this.lblElLimStatus2.Size = new System.Drawing.Size(56, 15);
            this.lblElLimStatus2.TabIndex = 17;
            this.lblElLimStatus2.Text = "Inactive";
            // 
            // lblElLimStatus1
            // 
            this.lblElLimStatus1.AutoSize = true;
            this.lblElLimStatus1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblElLimStatus1.Location = new System.Drawing.Point(223, 271);
            this.lblElLimStatus1.Name = "lblElLimStatus1";
            this.lblElLimStatus1.Size = new System.Drawing.Size(56, 15);
            this.lblElLimStatus1.TabIndex = 16;
            this.lblElLimStatus1.Text = "Inactive";
            // 
            // lblAzLimStatus2
            // 
            this.lblAzLimStatus2.AutoSize = true;
            this.lblAzLimStatus2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblAzLimStatus2.Location = new System.Drawing.Point(223, 231);
            this.lblAzLimStatus2.Name = "lblAzLimStatus2";
            this.lblAzLimStatus2.Size = new System.Drawing.Size(56, 15);
            this.lblAzLimStatus2.TabIndex = 15;
            this.lblAzLimStatus2.Text = "Inactive";
            this.lblAzLimStatus2.Click += new System.EventHandler(this.lblAzLimStatus2_Click);
            // 
            // lblAzLimStatus1
            // 
            this.lblAzLimStatus1.AutoSize = true;
            this.lblAzLimStatus1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblAzLimStatus1.Location = new System.Drawing.Point(223, 193);
            this.lblAzLimStatus1.Name = "lblAzLimStatus1";
            this.lblAzLimStatus1.Size = new System.Drawing.Size(56, 15);
            this.lblAzLimStatus1.TabIndex = 14;
            this.lblAzLimStatus1.Text = "Inactive";
            // 
            // lblElLimit2
            // 
            this.lblElLimit2.AutoSize = true;
            this.lblElLimit2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblElLimit2.Location = new System.Drawing.Point(14, 309);
            this.lblElLimit2.Name = "lblElLimit2";
            this.lblElLimit2.Size = new System.Drawing.Size(160, 15);
            this.lblElLimit2.TabIndex = 13;
            this.lblElLimit2.Text = "Elevation Limit Switch 2";
            // 
            // lblElLimit1
            // 
            this.lblElLimit1.AutoSize = true;
            this.lblElLimit1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblElLimit1.Location = new System.Drawing.Point(14, 271);
            this.lblElLimit1.Name = "lblElLimit1";
            this.lblElLimit1.Size = new System.Drawing.Size(160, 15);
            this.lblElLimit1.TabIndex = 12;
            this.lblElLimit1.Text = "Elevation Limit Switch 1";
            // 
            // lblAzLimit2
            // 
            this.lblAzLimit2.AutoSize = true;
            this.lblAzLimit2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblAzLimit2.Location = new System.Drawing.Point(14, 231);
            this.lblAzLimit2.Name = "lblAzLimit2";
            this.lblAzLimit2.Size = new System.Drawing.Size(152, 15);
            this.lblAzLimit2.TabIndex = 11;
            this.lblAzLimit2.Text = "Azimuth Limit Switch 1";
            // 
            // lblAzLimit1
            // 
            this.lblAzLimit1.AutoSize = true;
            this.lblAzLimit1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblAzLimit1.Location = new System.Drawing.Point(14, 193);
            this.lblAzLimit1.Name = "lblAzLimit1";
            this.lblAzLimit1.Size = new System.Drawing.Size(152, 15);
            this.lblAzLimit1.TabIndex = 10;
            this.lblAzLimit1.Text = "Azimuth Limit Switch 1";
            // 
            // lblEleProx2
            // 
            this.lblEleProx2.AutoSize = true;
            this.lblEleProx2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblEleProx2.Location = new System.Drawing.Point(223, 155);
            this.lblEleProx2.Name = "lblEleProx2";
            this.lblEleProx2.Size = new System.Drawing.Size(56, 15);
            this.lblEleProx2.TabIndex = 9;
            this.lblEleProx2.Text = "Inactive";
            // 
            // lblEleProx1
            // 
            this.lblEleProx1.AutoSize = true;
            this.lblEleProx1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblEleProx1.Location = new System.Drawing.Point(223, 118);
            this.lblEleProx1.Name = "lblEleProx1";
            this.lblEleProx1.Size = new System.Drawing.Size(56, 15);
            this.lblEleProx1.TabIndex = 8;
            this.lblEleProx1.Text = "Inactive";
            // 
            // lblElProx2
            // 
            this.lblElProx2.AutoSize = true;
            this.lblElProx2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblElProx2.Location = new System.Drawing.Point(14, 155);
            this.lblElProx2.Name = "lblElProx2";
            this.lblElProx2.Size = new System.Drawing.Size(190, 15);
            this.lblElProx2.TabIndex = 7;
            this.lblElProx2.Text = "Elevation Proximity Sensor 2";
            // 
            // lblElProx1
            // 
            this.lblElProx1.AutoSize = true;
            this.lblElProx1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblElProx1.Location = new System.Drawing.Point(14, 118);
            this.lblElProx1.Name = "lblElProx1";
            this.lblElProx1.Size = new System.Drawing.Size(190, 15);
            this.lblElProx1.TabIndex = 6;
            this.lblElProx1.Text = "Elevation Proximity Sensor 1";
            // 
            // lblAzProxStatus3
            // 
            this.lblAzProxStatus3.AutoSize = true;
            this.lblAzProxStatus3.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblAzProxStatus3.Location = new System.Drawing.Point(223, 83);
            this.lblAzProxStatus3.Name = "lblAzProxStatus3";
            this.lblAzProxStatus3.Size = new System.Drawing.Size(56, 15);
            this.lblAzProxStatus3.TabIndex = 5;
            this.lblAzProxStatus3.Text = "Inactive";
            // 
            // lblAzProxStatus2
            // 
            this.lblAzProxStatus2.AutoSize = true;
            this.lblAzProxStatus2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblAzProxStatus2.Location = new System.Drawing.Point(223, 50);
            this.lblAzProxStatus2.Name = "lblAzProxStatus2";
            this.lblAzProxStatus2.Size = new System.Drawing.Size(56, 15);
            this.lblAzProxStatus2.TabIndex = 4;
            this.lblAzProxStatus2.Text = "Inactive";
            // 
            // lblAzProxStatus1
            // 
            this.lblAzProxStatus1.AutoSize = true;
            this.lblAzProxStatus1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblAzProxStatus1.Location = new System.Drawing.Point(223, 18);
            this.lblAzProxStatus1.Name = "lblAzProxStatus1";
            this.lblAzProxStatus1.Size = new System.Drawing.Size(56, 15);
            this.lblAzProxStatus1.TabIndex = 3;
            this.lblAzProxStatus1.Text = "Inactive";
            // 
            // lblAzProx3
            // 
            this.lblAzProx3.AutoSize = true;
            this.lblAzProx3.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblAzProx3.Location = new System.Drawing.Point(14, 83);
            this.lblAzProx3.Name = "lblAzProx3";
            this.lblAzProx3.Size = new System.Drawing.Size(182, 15);
            this.lblAzProx3.TabIndex = 2;
            this.lblAzProx3.Text = "Azimuth Proximity Sensor 3";
            // 
            // lblAzProx2
            // 
            this.lblAzProx2.AutoSize = true;
            this.lblAzProx2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblAzProx2.Location = new System.Drawing.Point(14, 50);
            this.lblAzProx2.Name = "lblAzProx2";
            this.lblAzProx2.Size = new System.Drawing.Size(182, 15);
            this.lblAzProx2.TabIndex = 1;
            this.lblAzProx2.Text = "Azimuth Proximity Sensor 2";
            // 
            // lblAzProx1
            // 
            this.lblAzProx1.AutoSize = true;
            this.lblAzProx1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblAzProx1.Location = new System.Drawing.Point(14, 18);
            this.lblAzProx1.Name = "lblAzProx1";
            this.lblAzProx1.Size = new System.Drawing.Size(182, 15);
            this.lblAzProx1.TabIndex = 0;
            this.lblAzProx1.Text = "Azimuth Proximity Sensor 1";
            // 
            // lblVibrationStatus
            // 
            this.lblVibrationStatus.AutoSize = true;
            this.lblVibrationStatus.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblVibrationStatus.Location = new System.Drawing.Point(319, 113);
            this.lblVibrationStatus.Name = "lblVibrationStatus";
            this.lblVibrationStatus.Size = new System.Drawing.Size(139, 20);
            this.lblVibrationStatus.TabIndex = 0;
            this.lblVibrationStatus.Text = "Vibration Status";
            // 
            // lblAbsEncoder
            // 
            this.lblAbsEncoder.AutoSize = true;
            this.lblAbsEncoder.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblAbsEncoder.Location = new System.Drawing.Point(12, 357);
            this.lblAbsEncoder.Name = "lblAbsEncoder";
            this.lblAbsEncoder.Size = new System.Drawing.Size(119, 15);
            this.lblAbsEncoder.TabIndex = 36;
            this.lblAbsEncoder.Text = "Absolute Encoder";
            // 
            // lblEncoderDegrees
            // 
            this.lblEncoderDegrees.AutoSize = true;
            this.lblEncoderDegrees.Location = new System.Drawing.Point(12, 384);
            this.lblEncoderDegrees.Name = "lblEncoderDegrees";
            this.lblEncoderDegrees.Size = new System.Drawing.Size(47, 13);
            this.lblEncoderDegrees.TabIndex = 37;
            this.lblEncoderDegrees.Text = "Degrees";
            // 
            // lblDisplayDegreesEncoders
            // 
            this.lblDisplayDegreesEncoders.AutoSize = true;
            this.lblDisplayDegreesEncoders.Location = new System.Drawing.Point(76, 384);
            this.lblDisplayDegreesEncoders.Name = "lblDisplayDegreesEncoders";
            this.lblDisplayDegreesEncoders.Size = new System.Drawing.Size(13, 13);
            this.lblDisplayDegreesEncoders.TabIndex = 38;
            this.lblDisplayDegreesEncoders.Text = "0";
            // 
            // lblEncoderTicks
            // 
            this.lblEncoderTicks.AutoSize = true;
            this.lblEncoderTicks.Location = new System.Drawing.Point(11, 407);
            this.lblEncoderTicks.Name = "lblEncoderTicks";
            this.lblEncoderTicks.Size = new System.Drawing.Size(33, 13);
            this.lblEncoderTicks.TabIndex = 39;
            this.lblEncoderTicks.Text = "Ticks";
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(76, 407);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(13, 13);
            this.label13.TabIndex = 40;
            this.label13.Text = "0";
            // 
            // button4
            // 
            this.button4.Location = new System.Drawing.Point(10, 437);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(35, 35);
            this.button4.TabIndex = 41;
            this.button4.Text = "+1";
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new System.EventHandler(this.button4_Click);
            // 
            // button5
            // 
            this.button5.Location = new System.Drawing.Point(54, 437);
            this.button5.Name = "button5";
            this.button5.Size = new System.Drawing.Size(35, 35);
            this.button5.TabIndex = 42;
            this.button5.Text = "+5";
            this.button5.UseVisualStyleBackColor = true;
            // 
            // button6
            // 
            this.button6.Location = new System.Drawing.Point(96, 437);
            this.button6.Name = "button6";
            this.button6.Size = new System.Drawing.Size(35, 35);
            this.button6.TabIndex = 43;
            this.button6.Text = "+X";
            this.button6.UseVisualStyleBackColor = true;
            // 
            // button7
            // 
            this.button7.Location = new System.Drawing.Point(9, 478);
            this.button7.Name = "button7";
            this.button7.Size = new System.Drawing.Size(35, 35);
            this.button7.TabIndex = 44;
            this.button7.Text = "-1";
            this.button7.UseVisualStyleBackColor = true;
            // 
            // button8
            // 
            this.button8.Location = new System.Drawing.Point(54, 478);
            this.button8.Name = "button8";
            this.button8.Size = new System.Drawing.Size(35, 35);
            this.button8.TabIndex = 45;
            this.button8.Text = "-5";
            this.button8.UseVisualStyleBackColor = true;
            // 
            // button9
            // 
            this.button9.Location = new System.Drawing.Point(95, 478);
            this.button9.Name = "button9";
            this.button9.Size = new System.Drawing.Size(35, 35);
            this.button9.TabIndex = 46;
            this.button9.Text = "-X";
            this.button9.UseVisualStyleBackColor = true;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(137, 448);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(72, 13);
            this.label9.TabIndex = 47;
            this.label9.Text = "Custom Value";
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(140, 478);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(51, 20);
            this.textBox1.TabIndex = 48;
            // 
            // DiagnosticsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(905, 608);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.button9);
            this.Controls.Add(this.button8);
            this.Controls.Add(this.button7);
            this.Controls.Add(this.button6);
            this.Controls.Add(this.button5);
            this.Controls.Add(this.button4);
            this.Controls.Add(this.label13);
            this.Controls.Add(this.lblEncoderTicks);
            this.Controls.Add(this.lblDisplayDegreesEncoders);
            this.Controls.Add(this.lblEncoderDegrees);
            this.Controls.Add(this.lblAbsEncoder);
            this.Controls.Add(this.lblVibrationStatus);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.txtCustTemp);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
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
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.dataGridView1);
            this.Margin = new System.Windows.Forms.Padding(2);
            this.MinimumSize = new System.Drawing.Size(615, 249);
            this.Name = "DiagnosticsForm";
            this.Text = "DiagnosticsForm";
            this.Load += new System.EventHandler(this.DiagnosticsForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
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
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.TextBox txtCustTemp;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label lblVibrationStatus;
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
        private System.Windows.Forms.Label lblDisplayDegreesEncoders;
        private System.Windows.Forms.Label lblEncoderTicks;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.Button button5;
        private System.Windows.Forms.Button button6;
        private System.Windows.Forms.Button button7;
        private System.Windows.Forms.Button button8;
        private System.Windows.Forms.Button button9;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox textBox1;
    }
}