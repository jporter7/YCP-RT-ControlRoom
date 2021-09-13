using System;

namespace ControlRoomApplication.Main
{
    partial class FreeControlForm
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
            this.PosDecButton = new System.Windows.Forms.Button();
            this.NegDecButton = new System.Windows.Forms.Button();
            this.NegRAButton = new System.Windows.Forms.Button();
            this.PosRAButton = new System.Windows.Forms.Button();
            this.ActualRATextBox = new System.Windows.Forms.TextBox();
            this.ActualPositionLabel = new System.Windows.Forms.Label();
            this.ActualRALabel = new System.Windows.Forms.Label();
            this.ActualDecLabel = new System.Windows.Forms.Label();
            this.TargetDecLabel = new System.Windows.Forms.Label();
            this.TargetRALabel = new System.Windows.Forms.Label();
            this.TargetPositionLabel = new System.Windows.Forms.Label();
            this.TargetDecTextBox = new System.Windows.Forms.TextBox();
            this.TargetRATextBox = new System.Windows.Forms.TextBox();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.RAIncGroupbox = new System.Windows.Forms.GroupBox();
            this.tenButton = new System.Windows.Forms.Button();
            this.fiveButton = new System.Windows.Forms.Button();
            this.oneButton = new System.Windows.Forms.Button();
            this.oneForthButton = new System.Windows.Forms.Button();
            this.editButton = new System.Windows.Forms.Button();
            this.errorLabel = new System.Windows.Forms.Label();
            this.overRideGroupbox = new System.Windows.Forms.GroupBox();
            this.label8 = new System.Windows.Forms.Label();
            this.statusTextBox = new System.Windows.Forms.TextBox();
            this.ActualDecTextBox = new System.Windows.Forms.TextBox();
            this.decIncGroupbox = new System.Windows.Forms.GroupBox();
            this.tenButtonDec = new System.Windows.Forms.Button();
            this.fiveButtonDec = new System.Windows.Forms.Button();
            this.oneButtonDec = new System.Windows.Forms.Button();
            this.oneForthButtonDec = new System.Windows.Forms.Button();
            this.freeControlGroupbox = new System.Windows.Forms.GroupBox();
            this.controlScriptsCombo = new System.Windows.Forms.ComboBox();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.runControlScriptButton = new System.Windows.Forms.Button();
            this.manualGroupBox = new System.Windows.Forms.GroupBox();
            this.speedTrackBar = new System.Windows.Forms.TrackBar();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.manualControlButton = new System.Windows.Forms.Button();
            this.immediateRadioButton = new System.Windows.Forms.RadioButton();
            this.ControlledButtonRadio = new System.Windows.Forms.RadioButton();
            this.speedTextBox = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.ccwAzJogButton = new System.Windows.Forms.Button();
            this.plusElaButton = new System.Windows.Forms.Button();
            this.subElaButton = new System.Windows.Forms.Button();
            this.cwAzJogButton = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.spectraCyberGroupBox = new System.Windows.Forms.GroupBox();
            this.label12 = new System.Windows.Forms.Label();
            this.integrationStepCombo = new System.Windows.Forms.ComboBox();
            this.label10 = new System.Windows.Forms.Label();
            this.IFGainVal = new System.Windows.Forms.TextBox();
            this.lblIFGain = new System.Windows.Forms.Label();
            this.finalizeSettingsButton = new System.Windows.Forms.Button();
            this.DCGain = new System.Windows.Forms.ComboBox();
            this.stopScanButton = new System.Windows.Forms.Button();
            this.startScanButton = new System.Windows.Forms.Button();
            this.frequency = new System.Windows.Forms.TextBox();
            this.lblFrequency = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.offsetVoltage = new System.Windows.Forms.TextBox();
            this.scanTypeComboBox = new System.Windows.Forms.ComboBox();
            this.IFGainToolTip = new System.Windows.Forms.ToolTip(this.components);
            this.frequencyToolTip = new System.Windows.Forms.ToolTip(this.components);
            this.offsetVoltageToolTip = new System.Windows.Forms.ToolTip(this.components);
            this.RAIncGroupbox.SuspendLayout();
            this.overRideGroupbox.SuspendLayout();
            this.decIncGroupbox.SuspendLayout();
            this.freeControlGroupbox.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.manualGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.speedTrackBar)).BeginInit();
            this.spectraCyberGroupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // PosDecButton
            // 
            this.PosDecButton.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.PosDecButton.BackColor = System.Drawing.Color.DarkGray;
            this.PosDecButton.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.PosDecButton.Location = new System.Drawing.Point(396, 69);
            this.PosDecButton.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.PosDecButton.Name = "PosDecButton";
            this.PosDecButton.Size = new System.Drawing.Size(53, 49);
            this.PosDecButton.TabIndex = 0;
            this.PosDecButton.Text = "+ Dec";
            this.PosDecButton.UseVisualStyleBackColor = false;
            this.PosDecButton.Click += new System.EventHandler(this.PosDecButton_Click);
            // 
            // NegDecButton
            // 
            this.NegDecButton.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.NegDecButton.BackColor = System.Drawing.Color.DarkGray;
            this.NegDecButton.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.NegDecButton.Location = new System.Drawing.Point(395, 186);
            this.NegDecButton.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.NegDecButton.Name = "NegDecButton";
            this.NegDecButton.Size = new System.Drawing.Size(53, 49);
            this.NegDecButton.TabIndex = 1;
            this.NegDecButton.Text = "- Dec";
            this.NegDecButton.UseVisualStyleBackColor = false;
            this.NegDecButton.Click += new System.EventHandler(this.NegDecButton_Click);
            // 
            // NegRAButton
            // 
            this.NegRAButton.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.NegRAButton.BackColor = System.Drawing.Color.DarkGray;
            this.NegRAButton.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.NegRAButton.Location = new System.Drawing.Point(333, 129);
            this.NegRAButton.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.NegRAButton.Name = "NegRAButton";
            this.NegRAButton.Size = new System.Drawing.Size(53, 49);
            this.NegRAButton.TabIndex = 2;
            this.NegRAButton.Text = "- RA";
            this.NegRAButton.UseVisualStyleBackColor = false;
            this.NegRAButton.Click += new System.EventHandler(this.NegRAButton_Click);
            // 
            // PosRAButton
            // 
            this.PosRAButton.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.PosRAButton.BackColor = System.Drawing.Color.DarkGray;
            this.PosRAButton.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.PosRAButton.Location = new System.Drawing.Point(455, 129);
            this.PosRAButton.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.PosRAButton.Name = "PosRAButton";
            this.PosRAButton.Size = new System.Drawing.Size(53, 49);
            this.PosRAButton.TabIndex = 3;
            this.PosRAButton.Text = "+ RA";
            this.PosRAButton.UseVisualStyleBackColor = false;
            this.PosRAButton.Click += new System.EventHandler(this.PosRAButton_Click);
            // 
            // ActualRATextBox
            // 
            this.ActualRATextBox.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.ActualRATextBox.Location = new System.Drawing.Point(204, 91);
            this.ActualRATextBox.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.ActualRATextBox.Name = "ActualRATextBox";
            this.ActualRATextBox.ReadOnly = true;
            this.ActualRATextBox.Size = new System.Drawing.Size(132, 22);
            this.ActualRATextBox.TabIndex = 5;
            // 
            // ActualPositionLabel
            // 
            this.ActualPositionLabel.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.ActualPositionLabel.AutoSize = true;
            this.ActualPositionLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.ActualPositionLabel.Location = new System.Drawing.Point(199, 33);
            this.ActualPositionLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.ActualPositionLabel.Name = "ActualPositionLabel";
            this.ActualPositionLabel.Size = new System.Drawing.Size(141, 25);
            this.ActualPositionLabel.TabIndex = 7;
            this.ActualPositionLabel.Text = "Actual Position";
            // 
            // ActualRALabel
            // 
            this.ActualRALabel.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.ActualRALabel.AutoSize = true;
            this.ActualRALabel.Location = new System.Drawing.Point(200, 69);
            this.ActualRALabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.ActualRALabel.Name = "ActualRALabel";
            this.ActualRALabel.Size = new System.Drawing.Size(110, 17);
            this.ActualRALabel.TabIndex = 8;
            this.ActualRALabel.Text = "Right Ascension";
            // 
            // ActualDecLabel
            // 
            this.ActualDecLabel.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.ActualDecLabel.AutoSize = true;
            this.ActualDecLabel.Location = new System.Drawing.Point(201, 126);
            this.ActualDecLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.ActualDecLabel.Name = "ActualDecLabel";
            this.ActualDecLabel.Size = new System.Drawing.Size(78, 17);
            this.ActualDecLabel.TabIndex = 9;
            this.ActualDecLabel.Text = "Declination";
            // 
            // TargetDecLabel
            // 
            this.TargetDecLabel.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.TargetDecLabel.AutoSize = true;
            this.TargetDecLabel.Location = new System.Drawing.Point(24, 126);
            this.TargetDecLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.TargetDecLabel.Name = "TargetDecLabel";
            this.TargetDecLabel.Size = new System.Drawing.Size(78, 17);
            this.TargetDecLabel.TabIndex = 14;
            this.TargetDecLabel.Text = "Declination";
            // 
            // TargetRALabel
            // 
            this.TargetRALabel.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.TargetRALabel.AutoSize = true;
            this.TargetRALabel.Location = new System.Drawing.Point(21, 69);
            this.TargetRALabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.TargetRALabel.Name = "TargetRALabel";
            this.TargetRALabel.Size = new System.Drawing.Size(110, 17);
            this.TargetRALabel.TabIndex = 13;
            this.TargetRALabel.Text = "Right Ascension";
            // 
            // TargetPositionLabel
            // 
            this.TargetPositionLabel.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.TargetPositionLabel.AutoSize = true;
            this.TargetPositionLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.TargetPositionLabel.Location = new System.Drawing.Point(16, 33);
            this.TargetPositionLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.TargetPositionLabel.Name = "TargetPositionLabel";
            this.TargetPositionLabel.Size = new System.Drawing.Size(143, 25);
            this.TargetPositionLabel.TabIndex = 12;
            this.TargetPositionLabel.Text = "Target Position";
            // 
            // TargetDecTextBox
            // 
            this.TargetDecTextBox.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.TargetDecTextBox.Location = new System.Drawing.Point(27, 148);
            this.TargetDecTextBox.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.TargetDecTextBox.Name = "TargetDecTextBox";
            this.TargetDecTextBox.ReadOnly = true;
            this.TargetDecTextBox.Size = new System.Drawing.Size(132, 22);
            this.TargetDecTextBox.TabIndex = 11;
            // 
            // TargetRATextBox
            // 
            this.TargetRATextBox.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.TargetRATextBox.Location = new System.Drawing.Point(25, 91);
            this.TargetRATextBox.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.TargetRATextBox.Name = "TargetRATextBox";
            this.TargetRATextBox.ReadOnly = true;
            this.TargetRATextBox.Size = new System.Drawing.Size(132, 22);
            this.TargetRATextBox.TabIndex = 10;
            // 
            // timer1
            // 
            this.timer1.Enabled = true;
            this.timer1.Interval = 1000;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // RAIncGroupbox
            // 
            this.RAIncGroupbox.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.RAIncGroupbox.BackColor = System.Drawing.Color.Gray;
            this.RAIncGroupbox.Controls.Add(this.tenButton);
            this.RAIncGroupbox.Controls.Add(this.fiveButton);
            this.RAIncGroupbox.Controls.Add(this.oneButton);
            this.RAIncGroupbox.Controls.Add(this.oneForthButton);
            this.RAIncGroupbox.Location = new System.Drawing.Point(5, 55);
            this.RAIncGroupbox.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.RAIncGroupbox.Name = "RAIncGroupbox";
            this.RAIncGroupbox.Padding = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.RAIncGroupbox.Size = new System.Drawing.Size(251, 66);
            this.RAIncGroupbox.TabIndex = 16;
            this.RAIncGroupbox.TabStop = false;
            this.RAIncGroupbox.Text = "Right Ascension Increment";
            // 
            // tenButton
            // 
            this.tenButton.BackColor = System.Drawing.Color.DarkGray;
            this.tenButton.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.tenButton.Location = new System.Drawing.Point(181, 18);
            this.tenButton.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.tenButton.Name = "tenButton";
            this.tenButton.Size = new System.Drawing.Size(53, 37);
            this.tenButton.TabIndex = 3;
            this.tenButton.Text = "10";
            this.tenButton.UseVisualStyleBackColor = false;
            this.tenButton.Click += new System.EventHandler(this.tenButton_Click);
            // 
            // fiveButton
            // 
            this.fiveButton.BackColor = System.Drawing.Color.DarkGray;
            this.fiveButton.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.fiveButton.Location = new System.Drawing.Point(124, 18);
            this.fiveButton.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.fiveButton.Name = "fiveButton";
            this.fiveButton.Size = new System.Drawing.Size(53, 37);
            this.fiveButton.TabIndex = 2;
            this.fiveButton.Text = "5";
            this.fiveButton.UseVisualStyleBackColor = false;
            this.fiveButton.Click += new System.EventHandler(this.fiveButton_Click);
            // 
            // oneButton
            // 
            this.oneButton.BackColor = System.Drawing.Color.DarkGray;
            this.oneButton.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.oneButton.Location = new System.Drawing.Point(65, 18);
            this.oneButton.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.oneButton.Name = "oneButton";
            this.oneButton.Size = new System.Drawing.Size(53, 37);
            this.oneButton.TabIndex = 1;
            this.oneButton.Text = "1";
            this.oneButton.UseVisualStyleBackColor = false;
            this.oneButton.Click += new System.EventHandler(this.oneButton_Click);
            // 
            // oneForthButton
            // 
            this.oneForthButton.BackColor = System.Drawing.Color.DarkGray;
            this.oneForthButton.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.oneForthButton.Location = new System.Drawing.Point(7, 18);
            this.oneForthButton.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.oneForthButton.Name = "oneForthButton";
            this.oneForthButton.Size = new System.Drawing.Size(53, 37);
            this.oneForthButton.TabIndex = 0;
            this.oneForthButton.Text = "0.25";
            this.oneForthButton.UseVisualStyleBackColor = false;
            this.oneForthButton.Click += new System.EventHandler(this.oneForthButton_Click);
            // 
            // editButton
            // 
            this.editButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.editButton.BackColor = System.Drawing.Color.OrangeRed;
            this.editButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.editButton.Location = new System.Drawing.Point(333, 12);
            this.editButton.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.editButton.Name = "editButton";
            this.editButton.Size = new System.Drawing.Size(175, 31);
            this.editButton.TabIndex = 17;
            this.editButton.Text = "Edit Position";
            this.editButton.UseVisualStyleBackColor = false;
            this.editButton.Click += new System.EventHandler(this.editButton_Click);
            // 
            // errorLabel
            // 
            this.errorLabel.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.errorLabel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.errorLabel.ForeColor = System.Drawing.Color.Red;
            this.errorLabel.Location = new System.Drawing.Point(447, 521);
            this.errorLabel.Name = "errorLabel";
            this.errorLabel.Size = new System.Drawing.Size(306, 23);
            this.errorLabel.TabIndex = 18;
            this.errorLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // overRideGroupbox
            // 
            this.overRideGroupbox.BackColor = System.Drawing.Color.Gainsboro;
            this.overRideGroupbox.Controls.Add(this.label8);
            this.overRideGroupbox.Controls.Add(this.statusTextBox);
            this.overRideGroupbox.Controls.Add(this.ActualRATextBox);
            this.overRideGroupbox.Controls.Add(this.ActualDecTextBox);
            this.overRideGroupbox.Controls.Add(this.ActualPositionLabel);
            this.overRideGroupbox.Controls.Add(this.ActualRALabel);
            this.overRideGroupbox.Controls.Add(this.ActualDecLabel);
            this.overRideGroupbox.Controls.Add(this.TargetDecLabel);
            this.overRideGroupbox.Controls.Add(this.TargetRATextBox);
            this.overRideGroupbox.Controls.Add(this.TargetRALabel);
            this.overRideGroupbox.Controls.Add(this.TargetDecTextBox);
            this.overRideGroupbox.Controls.Add(this.TargetPositionLabel);
            this.overRideGroupbox.Location = new System.Drawing.Point(16, 11);
            this.overRideGroupbox.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.overRideGroupbox.Name = "overRideGroupbox";
            this.overRideGroupbox.Padding = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.overRideGroupbox.Size = new System.Drawing.Size(372, 244);
            this.overRideGroupbox.TabIndex = 19;
            this.overRideGroupbox.TabStop = false;
            this.overRideGroupbox.Text = "Position Information";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label8.Location = new System.Drawing.Point(24, 206);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(159, 17);
            this.label8.TabIndex = 16;
            this.label8.Text = "Radio Telescope Status";
            // 
            // statusTextBox
            // 
            this.statusTextBox.Location = new System.Drawing.Point(191, 202);
            this.statusTextBox.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.statusTextBox.Name = "statusTextBox";
            this.statusTextBox.ReadOnly = true;
            this.statusTextBox.Size = new System.Drawing.Size(135, 22);
            this.statusTextBox.TabIndex = 15;
            // 
            // ActualDecTextBox
            // 
            this.ActualDecTextBox.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.ActualDecTextBox.Location = new System.Drawing.Point(204, 146);
            this.ActualDecTextBox.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.ActualDecTextBox.Name = "ActualDecTextBox";
            this.ActualDecTextBox.ReadOnly = true;
            this.ActualDecTextBox.Size = new System.Drawing.Size(100, 22);
            this.ActualDecTextBox.TabIndex = 6;
            // 
            // decIncGroupbox
            // 
            this.decIncGroupbox.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.decIncGroupbox.BackColor = System.Drawing.Color.Gray;
            this.decIncGroupbox.Controls.Add(this.tenButtonDec);
            this.decIncGroupbox.Controls.Add(this.fiveButtonDec);
            this.decIncGroupbox.Controls.Add(this.oneButtonDec);
            this.decIncGroupbox.Controls.Add(this.oneForthButtonDec);
            this.decIncGroupbox.Location = new System.Drawing.Point(5, 128);
            this.decIncGroupbox.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.decIncGroupbox.Name = "decIncGroupbox";
            this.decIncGroupbox.Padding = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.decIncGroupbox.Size = new System.Drawing.Size(251, 69);
            this.decIncGroupbox.TabIndex = 20;
            this.decIncGroupbox.TabStop = false;
            this.decIncGroupbox.Text = "Declanation Increment";
            // 
            // tenButtonDec
            // 
            this.tenButtonDec.BackColor = System.Drawing.Color.DarkGray;
            this.tenButtonDec.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.tenButtonDec.Location = new System.Drawing.Point(181, 18);
            this.tenButtonDec.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.tenButtonDec.Name = "tenButtonDec";
            this.tenButtonDec.Size = new System.Drawing.Size(53, 37);
            this.tenButtonDec.TabIndex = 3;
            this.tenButtonDec.Text = "10";
            this.tenButtonDec.UseVisualStyleBackColor = false;
            // 
            // fiveButtonDec
            // 
            this.fiveButtonDec.BackColor = System.Drawing.Color.DarkGray;
            this.fiveButtonDec.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.fiveButtonDec.Location = new System.Drawing.Point(124, 18);
            this.fiveButtonDec.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.fiveButtonDec.Name = "fiveButtonDec";
            this.fiveButtonDec.Size = new System.Drawing.Size(53, 37);
            this.fiveButtonDec.TabIndex = 2;
            this.fiveButtonDec.Text = "5";
            this.fiveButtonDec.UseVisualStyleBackColor = false;
            // 
            // oneButtonDec
            // 
            this.oneButtonDec.BackColor = System.Drawing.Color.DarkGray;
            this.oneButtonDec.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.oneButtonDec.Location = new System.Drawing.Point(65, 18);
            this.oneButtonDec.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.oneButtonDec.Name = "oneButtonDec";
            this.oneButtonDec.Size = new System.Drawing.Size(53, 37);
            this.oneButtonDec.TabIndex = 1;
            this.oneButtonDec.Text = "1";
            this.oneButtonDec.UseVisualStyleBackColor = false;
            // 
            // oneForthButtonDec
            // 
            this.oneForthButtonDec.BackColor = System.Drawing.Color.DarkGray;
            this.oneForthButtonDec.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.oneForthButtonDec.Location = new System.Drawing.Point(7, 18);
            this.oneForthButtonDec.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.oneForthButtonDec.Name = "oneForthButtonDec";
            this.oneForthButtonDec.Size = new System.Drawing.Size(53, 37);
            this.oneForthButtonDec.TabIndex = 0;
            this.oneForthButtonDec.Text = "0.25";
            this.oneForthButtonDec.UseVisualStyleBackColor = false;
            // 
            // freeControlGroupbox
            // 
            this.freeControlGroupbox.BackColor = System.Drawing.Color.Gainsboro;
            this.freeControlGroupbox.Controls.Add(this.NegRAButton);
            this.freeControlGroupbox.Controls.Add(this.PosDecButton);
            this.freeControlGroupbox.Controls.Add(this.decIncGroupbox);
            this.freeControlGroupbox.Controls.Add(this.NegDecButton);
            this.freeControlGroupbox.Controls.Add(this.PosRAButton);
            this.freeControlGroupbox.Controls.Add(this.RAIncGroupbox);
            this.freeControlGroupbox.Controls.Add(this.editButton);
            this.freeControlGroupbox.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.freeControlGroupbox.Location = new System.Drawing.Point(16, 262);
            this.freeControlGroupbox.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.freeControlGroupbox.Name = "freeControlGroupbox";
            this.freeControlGroupbox.Padding = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.freeControlGroupbox.Size = new System.Drawing.Size(540, 247);
            this.freeControlGroupbox.TabIndex = 22;
            this.freeControlGroupbox.TabStop = false;
            this.freeControlGroupbox.Text = "Edit Target Position";
            // 
            // controlScriptsCombo
            // 
            this.controlScriptsCombo.BackColor = System.Drawing.Color.DarkGray;
            this.controlScriptsCombo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.controlScriptsCombo.FormattingEnabled = true;
            this.controlScriptsCombo.Items.AddRange(new object[] {
            "Radio Telescope Control Scripts",
            "Stow Telescope",
            "Full Elevation",
            "Full 360 Clockwise Rotation",
            "Full 360 Counter-Clockwise Rotation",
            "Thermal Calibration",
            "Snow Dump",
            "Home Telescope",
            "Custom Orientation Movement",
            "Endless Azimuth Rotation"});
            this.controlScriptsCombo.Location = new System.Drawing.Point(5, 34);
            this.controlScriptsCombo.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.controlScriptsCombo.Name = "controlScriptsCombo";
            this.controlScriptsCombo.Size = new System.Drawing.Size(345, 24);
            this.controlScriptsCombo.TabIndex = 23;
            this.controlScriptsCombo.SelectedIndexChanged += new System.EventHandler(this.controlScriptsCombo_SelectedIndexChanged);
            // 
            // groupBox4
            // 
            this.groupBox4.BackColor = System.Drawing.Color.Gainsboro;
            this.groupBox4.Controls.Add(this.runControlScriptButton);
            this.groupBox4.Controls.Add(this.controlScriptsCombo);
            this.groupBox4.Location = new System.Drawing.Point(395, 36);
            this.groupBox4.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Padding = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.groupBox4.Size = new System.Drawing.Size(557, 80);
            this.groupBox4.TabIndex = 24;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Control Scripts and Spectra";
            // 
            // runControlScriptButton
            // 
            this.runControlScriptButton.BackColor = System.Drawing.Color.DarkGray;
            this.runControlScriptButton.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.runControlScriptButton.Location = new System.Drawing.Point(384, 20);
            this.runControlScriptButton.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.runControlScriptButton.Name = "runControlScriptButton";
            this.runControlScriptButton.Size = new System.Drawing.Size(168, 41);
            this.runControlScriptButton.TabIndex = 24;
            this.runControlScriptButton.Text = "Run Script";
            this.runControlScriptButton.UseVisualStyleBackColor = false;
            this.runControlScriptButton.Click += new System.EventHandler(this.runControlScript_Click);
            // 
            // manualGroupBox
            // 
            this.manualGroupBox.BackColor = System.Drawing.Color.Gainsboro;
            this.manualGroupBox.Controls.Add(this.speedTrackBar);
            this.manualGroupBox.Controls.Add(this.label4);
            this.manualGroupBox.Controls.Add(this.label5);
            this.manualGroupBox.Controls.Add(this.label3);
            this.manualGroupBox.Controls.Add(this.manualControlButton);
            this.manualGroupBox.Controls.Add(this.immediateRadioButton);
            this.manualGroupBox.Controls.Add(this.ControlledButtonRadio);
            this.manualGroupBox.Controls.Add(this.speedTextBox);
            this.manualGroupBox.Controls.Add(this.label2);
            this.manualGroupBox.Controls.Add(this.label1);
            this.manualGroupBox.Controls.Add(this.ccwAzJogButton);
            this.manualGroupBox.Controls.Add(this.plusElaButton);
            this.manualGroupBox.Controls.Add(this.subElaButton);
            this.manualGroupBox.Controls.Add(this.cwAzJogButton);
            this.manualGroupBox.Location = new System.Drawing.Point(564, 262);
            this.manualGroupBox.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.manualGroupBox.Name = "manualGroupBox";
            this.manualGroupBox.Padding = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.manualGroupBox.Size = new System.Drawing.Size(372, 247);
            this.manualGroupBox.TabIndex = 25;
            this.manualGroupBox.TabStop = false;
            this.manualGroupBox.Text = "Manual Control";
            // 
            // speedTrackBar
            // 
            this.speedTrackBar.Location = new System.Drawing.Point(144, 178);
            this.speedTrackBar.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.speedTrackBar.Maximum = 20;
            this.speedTrackBar.Name = "speedTrackBar";
            this.speedTrackBar.Size = new System.Drawing.Size(200, 56);
            this.speedTrackBar.SmallChange = 3;
            this.speedTrackBar.TabIndex = 20;
            this.speedTrackBar.Scroll += new System.EventHandler(this.speedTrackBar_Scroll);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(140, 97);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(28, 17);
            this.label4.TabIndex = 28;
            this.label4.Text = "0.0";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(140, 74);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(28, 17);
            this.label5.TabIndex = 27;
            this.label5.Text = "0.0";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(8, 181);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(100, 17);
            this.label3.TabIndex = 26;
            this.label3.Text = "Speed (RPMs)";
            // 
            // manualControlButton
            // 
            this.manualControlButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.manualControlButton.BackColor = System.Drawing.Color.OrangeRed;
            this.manualControlButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.manualControlButton.Location = new System.Drawing.Point(20, 21);
            this.manualControlButton.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.manualControlButton.Name = "manualControlButton";
            this.manualControlButton.Size = new System.Drawing.Size(200, 34);
            this.manualControlButton.TabIndex = 25;
            this.manualControlButton.Text = "Activate Manual Control";
            this.manualControlButton.UseVisualStyleBackColor = false;
            this.manualControlButton.Click += new System.EventHandler(this.manualControlButton_Click);
            // 
            // immediateRadioButton
            // 
            this.immediateRadioButton.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.immediateRadioButton.AutoSize = true;
            this.immediateRadioButton.Location = new System.Drawing.Point(12, 151);
            this.immediateRadioButton.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.immediateRadioButton.Name = "immediateRadioButton";
            this.immediateRadioButton.Size = new System.Drawing.Size(126, 21);
            this.immediateRadioButton.TabIndex = 24;
            this.immediateRadioButton.Text = "Immediate Stop";
            this.immediateRadioButton.UseVisualStyleBackColor = true;
            // 
            // ControlledButtonRadio
            // 
            this.ControlledButtonRadio.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.ControlledButtonRadio.AutoSize = true;
            this.ControlledButtonRadio.Checked = true;
            this.ControlledButtonRadio.Location = new System.Drawing.Point(12, 128);
            this.ControlledButtonRadio.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.ControlledButtonRadio.Name = "ControlledButtonRadio";
            this.ControlledButtonRadio.Size = new System.Drawing.Size(126, 21);
            this.ControlledButtonRadio.TabIndex = 23;
            this.ControlledButtonRadio.TabStop = true;
            this.ControlledButtonRadio.Text = "Controlled Stop";
            this.ControlledButtonRadio.UseVisualStyleBackColor = true;
            // 
            // speedTextBox
            // 
            this.speedTextBox.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.speedTextBox.Location = new System.Drawing.Point(12, 199);
            this.speedTextBox.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.speedTextBox.Name = "speedTextBox";
            this.speedTextBox.ReadOnly = true;
            this.speedTextBox.Size = new System.Drawing.Size(91, 22);
            this.speedTextBox.TabIndex = 10;
            this.speedTextBox.TextChanged += new System.EventHandler(this.speedTextBox_TextChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(8, 98);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(117, 17);
            this.label2.TabIndex = 9;
            this.label2.Text = "Current Azimuth: ";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(8, 74);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(125, 17);
            this.label1.TabIndex = 8;
            this.label1.Text = "Current Elevation: ";
            // 
            // ccwAzJogButton
            // 
            this.ccwAzJogButton.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.ccwAzJogButton.BackColor = System.Drawing.Color.DarkGray;
            this.ccwAzJogButton.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.ccwAzJogButton.Location = new System.Drawing.Point(200, 66);
            this.ccwAzJogButton.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.ccwAzJogButton.Name = "ccwAzJogButton";
            this.ccwAzJogButton.Size = new System.Drawing.Size(53, 49);
            this.ccwAzJogButton.TabIndex = 6;
            this.ccwAzJogButton.Text = "CCW Jog";
            this.ccwAzJogButton.UseVisualStyleBackColor = false;
            this.ccwAzJogButton.MouseDown += new System.Windows.Forms.MouseEventHandler(this.ccwAzJogButton_Down);
            this.ccwAzJogButton.MouseUp += new System.Windows.Forms.MouseEventHandler(this.ccwAzJogButton_Up);
            // 
            // plusElaButton
            // 
            this.plusElaButton.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.plusElaButton.BackColor = System.Drawing.Color.DarkGray;
            this.plusElaButton.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.plusElaButton.Location = new System.Drawing.Point(253, 12);
            this.plusElaButton.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.plusElaButton.Name = "plusElaButton";
            this.plusElaButton.Size = new System.Drawing.Size(53, 49);
            this.plusElaButton.TabIndex = 4;
            this.plusElaButton.Text = "+ Ela";
            this.plusElaButton.UseVisualStyleBackColor = false;
            this.plusElaButton.MouseDown += new System.Windows.Forms.MouseEventHandler(this.plusElaButton_Down);
            this.plusElaButton.MouseUp += new System.Windows.Forms.MouseEventHandler(this.plusElaButton_Up);
            // 
            // subElaButton
            // 
            this.subElaButton.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.subElaButton.BackColor = System.Drawing.Color.DarkGray;
            this.subElaButton.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.subElaButton.Location = new System.Drawing.Point(253, 123);
            this.subElaButton.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.subElaButton.Name = "subElaButton";
            this.subElaButton.Size = new System.Drawing.Size(53, 49);
            this.subElaButton.TabIndex = 5;
            this.subElaButton.Text = "- Ela";
            this.subElaButton.UseVisualStyleBackColor = false;
            this.subElaButton.MouseDown += new System.Windows.Forms.MouseEventHandler(this.subElaButton_Down);
            this.subElaButton.MouseUp += new System.Windows.Forms.MouseEventHandler(this.subElaButton_Up);
            // 
            // cwAzJogButton
            // 
            this.cwAzJogButton.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.cwAzJogButton.BackColor = System.Drawing.Color.DarkGray;
            this.cwAzJogButton.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.cwAzJogButton.Location = new System.Drawing.Point(307, 66);
            this.cwAzJogButton.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.cwAzJogButton.Name = "cwAzJogButton";
            this.cwAzJogButton.Size = new System.Drawing.Size(53, 49);
            this.cwAzJogButton.TabIndex = 7;
            this.cwAzJogButton.Text = "CW Jog";
            this.cwAzJogButton.UseVisualStyleBackColor = false;
            this.cwAzJogButton.MouseDown += new System.Windows.Forms.MouseEventHandler(this.cwAzJogButton_Down);
            this.cwAzJogButton.MouseUp += new System.Windows.Forms.MouseEventHandler(this.cwAzJogButton_UP);
            // 
            // button1
            // 
            this.button1.BackColor = System.Drawing.Color.Gainsboro;
            this.button1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button1.ForeColor = System.Drawing.SystemColors.ControlText;
            this.button1.Location = new System.Drawing.Point(923, 2);
            this.button1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(29, 28);
            this.button1.TabIndex = 26;
            this.button1.Text = "?";
            this.button1.UseVisualStyleBackColor = false;
            this.button1.Click += new System.EventHandler(this.helpButton_click);
            // 
            // spectraCyberGroupBox
            // 
            this.spectraCyberGroupBox.BackColor = System.Drawing.Color.Gainsboro;
            this.spectraCyberGroupBox.Controls.Add(this.label12);
            this.spectraCyberGroupBox.Controls.Add(this.integrationStepCombo);
            this.spectraCyberGroupBox.Controls.Add(this.label10);
            this.spectraCyberGroupBox.Controls.Add(this.IFGainVal);
            this.spectraCyberGroupBox.Controls.Add(this.lblIFGain);
            this.spectraCyberGroupBox.Controls.Add(this.finalizeSettingsButton);
            this.spectraCyberGroupBox.Controls.Add(this.DCGain);
            this.spectraCyberGroupBox.Controls.Add(this.stopScanButton);
            this.spectraCyberGroupBox.Controls.Add(this.startScanButton);
            this.spectraCyberGroupBox.Controls.Add(this.frequency);
            this.spectraCyberGroupBox.Controls.Add(this.lblFrequency);
            this.spectraCyberGroupBox.Controls.Add(this.label9);
            this.spectraCyberGroupBox.Controls.Add(this.offsetVoltage);
            this.spectraCyberGroupBox.Controls.Add(this.scanTypeComboBox);
            this.spectraCyberGroupBox.Location = new System.Drawing.Point(395, 137);
            this.spectraCyberGroupBox.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.spectraCyberGroupBox.Name = "spectraCyberGroupBox";
            this.spectraCyberGroupBox.Padding = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.spectraCyberGroupBox.Size = new System.Drawing.Size(557, 103);
            this.spectraCyberGroupBox.TabIndex = 29;
            this.spectraCyberGroupBox.TabStop = false;
            this.spectraCyberGroupBox.Text = "Spectra Cyber";
            // 
            // label12
            // 
            this.label12.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(177, 53);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(98, 17);
            this.label12.TabIndex = 26;
            this.label12.Text = "Offset Voltage";
            // 
            // integrationStepCombo
            // 
            this.integrationStepCombo.BackColor = System.Drawing.Color.DarkGray;
            this.integrationStepCombo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.integrationStepCombo.FormattingEnabled = true;
            this.integrationStepCombo.Items.AddRange(new object[] {
            "Int Step",
            "0.3",
            "0.5(S)/1.00(C) ",
            "1.00(S)/10.00(C)"});
            this.integrationStepCombo.Location = new System.Drawing.Point(287, 69);
            this.integrationStepCombo.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.integrationStepCombo.MaxDropDownItems = 6;
            this.integrationStepCombo.Name = "integrationStepCombo";
            this.integrationStepCombo.Size = new System.Drawing.Size(104, 24);
            this.integrationStepCombo.TabIndex = 44;
            this.integrationStepCombo.SelectedIndexChanged += new System.EventHandler(this.comboBox1_SelectedIndexChanged);
            // 
            // label10
            // 
            this.label10.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(0, 50);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(88, 17);
            this.label10.TabIndex = 43;
            this.label10.Text = "DCGain (dB)";
            // 
            // IFGainVal
            // 
            this.IFGainVal.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.IFGainVal.Location = new System.Drawing.Point(103, 70);
            this.IFGainVal.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.IFGainVal.Name = "IFGainVal";
            this.IFGainVal.Size = new System.Drawing.Size(57, 22);
            this.IFGainVal.TabIndex = 42;
            this.IFGainVal.TextChanged += new System.EventHandler(this.IFGainVal_TextChanged);
            // 
            // lblIFGain
            // 
            this.lblIFGain.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.lblIFGain.AutoSize = true;
            this.lblIFGain.Location = new System.Drawing.Point(99, 53);
            this.lblIFGain.Name = "lblIFGain";
            this.lblIFGain.Size = new System.Drawing.Size(80, 17);
            this.lblIFGain.TabIndex = 41;
            this.lblIFGain.Text = "IFGain (dB)";
            // 
            // finalizeSettingsButton
            // 
            this.finalizeSettingsButton.BackColor = System.Drawing.Color.DarkGray;
            this.finalizeSettingsButton.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.finalizeSettingsButton.Location = new System.Drawing.Point(412, 12);
            this.finalizeSettingsButton.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.finalizeSettingsButton.Name = "finalizeSettingsButton";
            this.finalizeSettingsButton.Size = new System.Drawing.Size(129, 33);
            this.finalizeSettingsButton.TabIndex = 40;
            this.finalizeSettingsButton.Text = "Finalize Settings";
            this.finalizeSettingsButton.UseVisualStyleBackColor = false;
            this.finalizeSettingsButton.Click += new System.EventHandler(this.finalizeSettings_Click);
            // 
            // DCGain
            // 
            this.DCGain.BackColor = System.Drawing.Color.DarkGray;
            this.DCGain.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.DCGain.FormattingEnabled = true;
            this.DCGain.Items.AddRange(new object[] {
            "Gain",
            "x1",
            "X5",
            "X10",
            "X20",
            "X50",
            "X60"});
            this.DCGain.Location = new System.Drawing.Point(5, 69);
            this.DCGain.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.DCGain.MaxDropDownItems = 6;
            this.DCGain.Name = "DCGain";
            this.DCGain.Size = new System.Drawing.Size(75, 24);
            this.DCGain.TabIndex = 39;
            this.DCGain.SelectedIndexChanged += new System.EventHandler(this.DCGain_SelectedIndexChanged);
            // 
            // stopScanButton
            // 
            this.stopScanButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.stopScanButton.BackColor = System.Drawing.Color.OrangeRed;
            this.stopScanButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.stopScanButton.Location = new System.Drawing.Point(481, 54);
            this.stopScanButton.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.stopScanButton.Name = "stopScanButton";
            this.stopScanButton.Size = new System.Drawing.Size(60, 44);
            this.stopScanButton.TabIndex = 37;
            this.stopScanButton.Text = "Stop Scan";
            this.stopScanButton.UseVisualStyleBackColor = false;
            this.stopScanButton.Click += new System.EventHandler(this.stopScan_Click);
            // 
            // startScanButton
            // 
            this.startScanButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.startScanButton.BackColor = System.Drawing.Color.LimeGreen;
            this.startScanButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.startScanButton.Location = new System.Drawing.Point(412, 54);
            this.startScanButton.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.startScanButton.Name = "startScanButton";
            this.startScanButton.Size = new System.Drawing.Size(64, 44);
            this.startScanButton.TabIndex = 36;
            this.startScanButton.Text = "Start Scan";
            this.startScanButton.UseVisualStyleBackColor = false;
            this.startScanButton.Click += new System.EventHandler(this.startScan_Click);
            // 
            // frequency
            // 
            this.frequency.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.frequency.Location = new System.Drawing.Point(211, 22);
            this.frequency.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.frequency.Name = "frequency";
            this.frequency.Size = new System.Drawing.Size(100, 22);
            this.frequency.TabIndex = 35;
            this.frequency.TextChanged += new System.EventHandler(this.frequency_TextChanged);
            // 
            // lblFrequency
            // 
            this.lblFrequency.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.lblFrequency.AutoSize = true;
            this.lblFrequency.Location = new System.Drawing.Point(207, 0);
            this.lblFrequency.Name = "lblFrequency";
            this.lblFrequency.Size = new System.Drawing.Size(106, 17);
            this.lblFrequency.TabIndex = 34;
            this.lblFrequency.Text = "Frequency (Hz)";
            // 
            // label9
            // 
            this.label9.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(283, 54);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(108, 17);
            this.label9.TabIndex = 32;
            this.label9.Text = "Integration Step";
            // 
            // offsetVoltage
            // 
            this.offsetVoltage.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.offsetVoltage.Location = new System.Drawing.Point(181, 70);
            this.offsetVoltage.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.offsetVoltage.Name = "offsetVoltage";
            this.offsetVoltage.Size = new System.Drawing.Size(57, 22);
            this.offsetVoltage.TabIndex = 27;
            this.offsetVoltage.TextChanged += new System.EventHandler(this.offsetVoltage_TextChanged);
            // 
            // scanTypeComboBox
            // 
            this.scanTypeComboBox.BackColor = System.Drawing.Color.DarkGray;
            this.scanTypeComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.scanTypeComboBox.FormattingEnabled = true;
            this.scanTypeComboBox.Items.AddRange(new object[] {
            "Scan Type",
            "Continuum",
            "Spectral"});
            this.scanTypeComboBox.Location = new System.Drawing.Point(5, 22);
            this.scanTypeComboBox.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.scanTypeComboBox.MaxDropDownItems = 2;
            this.scanTypeComboBox.Name = "scanTypeComboBox";
            this.scanTypeComboBox.Size = new System.Drawing.Size(109, 24);
            this.scanTypeComboBox.TabIndex = 25;
            this.scanTypeComboBox.SelectedIndexChanged += new System.EventHandler(this.scanTypeComboBox_SelectedIndexChanged);
            // 
            // FreeControlForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Gray;
            this.ClientSize = new System.Drawing.Size(967, 540);
            this.Controls.Add(this.spectraCyberGroupBox);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.manualGroupBox);
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.freeControlGroupbox);
            this.Controls.Add(this.overRideGroupbox);
            this.Controls.Add(this.errorLabel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.MinimumSize = new System.Drawing.Size(794, 577);
            this.Name = "FreeControlForm";
            this.Text = "Control Form";
            this.Load += new System.EventHandler(this.FreeControlForm_Load);
            this.RAIncGroupbox.ResumeLayout(false);
            this.overRideGroupbox.ResumeLayout(false);
            this.overRideGroupbox.PerformLayout();
            this.decIncGroupbox.ResumeLayout(false);
            this.freeControlGroupbox.ResumeLayout(false);
            this.groupBox4.ResumeLayout(false);
            this.manualGroupBox.ResumeLayout(false);
            this.manualGroupBox.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.speedTrackBar)).EndInit();
            this.spectraCyberGroupBox.ResumeLayout(false);
            this.spectraCyberGroupBox.PerformLayout();
            this.ResumeLayout(false);

        }

      
        #endregion

        private System.Windows.Forms.Button PosDecButton;
        private System.Windows.Forms.Button NegDecButton;
        private System.Windows.Forms.Button NegRAButton;
        private System.Windows.Forms.Button PosRAButton;
        private System.Windows.Forms.TextBox ActualRATextBox;
        private System.Windows.Forms.Label ActualPositionLabel;
        private System.Windows.Forms.Label ActualRALabel;
        private System.Windows.Forms.Label ActualDecLabel;
        private System.Windows.Forms.Label TargetDecLabel;
        private System.Windows.Forms.Label TargetRALabel;
        private System.Windows.Forms.Label TargetPositionLabel;
        private System.Windows.Forms.TextBox TargetDecTextBox;
        private System.Windows.Forms.TextBox TargetRATextBox;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.GroupBox RAIncGroupbox;
        private System.Windows.Forms.Button tenButton;
        private System.Windows.Forms.Button fiveButton;
        private System.Windows.Forms.Button oneButton;
        private System.Windows.Forms.Button oneForthButton;
        private System.Windows.Forms.Button editButton;
        private System.Windows.Forms.Label errorLabel;
        private System.Windows.Forms.GroupBox overRideGroupbox;
        private System.Windows.Forms.GroupBox decIncGroupbox;
        private System.Windows.Forms.Button tenButtonDec;
        private System.Windows.Forms.Button fiveButtonDec;
        private System.Windows.Forms.Button oneButtonDec;
        private System.Windows.Forms.Button oneForthButtonDec;
        private System.Windows.Forms.GroupBox freeControlGroupbox;
        private System.Windows.Forms.ComboBox controlScriptsCombo;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.GroupBox manualGroupBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button ccwAzJogButton;
        private System.Windows.Forms.Button plusElaButton;
        private System.Windows.Forms.Button subElaButton;
        private System.Windows.Forms.Button cwAzJogButton;
        private System.Windows.Forms.TextBox speedTextBox;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.RadioButton immediateRadioButton;
        private System.Windows.Forms.RadioButton ControlledButtonRadio;
        private System.Windows.Forms.Button manualControlButton;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button runControlScriptButton;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox statusTextBox;
        private System.Windows.Forms.TextBox ActualDecTextBox;
        private System.Windows.Forms.GroupBox spectraCyberGroupBox;
        private System.Windows.Forms.Button finalizeSettingsButton;
        private System.Windows.Forms.ComboBox DCGain;
        private System.Windows.Forms.Button stopScanButton;
        private System.Windows.Forms.Button startScanButton;
        private System.Windows.Forms.TextBox frequency;
        private System.Windows.Forms.Label lblFrequency;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox offsetVoltage;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.ComboBox scanTypeComboBox;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.TextBox IFGainVal;
        private System.Windows.Forms.Label lblIFGain;
        private System.Windows.Forms.ComboBox integrationStepCombo;
        private System.Windows.Forms.ToolTip IFGainToolTip;
        private System.Windows.Forms.ToolTip offsetVoltageToolTip;
        private System.Windows.Forms.ToolTip frequencyToolTip;
        private System.Windows.Forms.TrackBar speedTrackBar;
    

    }
}