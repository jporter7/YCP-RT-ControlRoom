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
            this.ActualDecTextBox = new System.Windows.Forms.TextBox();
            this.ActualPositionLabel = new System.Windows.Forms.Label();
            this.ActualRALabel = new System.Windows.Forms.Label();
            this.ActualDecLabel = new System.Windows.Forms.Label();
            this.TargetDecLabel = new System.Windows.Forms.Label();
            this.TargetRALabel = new System.Windows.Forms.Label();
            this.TargetPositionLabel = new System.Windows.Forms.Label();
            this.TargetDecTextBox = new System.Windows.Forms.TextBox();
            this.TargetRATextBox = new System.Windows.Forms.TextBox();
            this.Title = new System.Windows.Forms.Label();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.rightAscensionIncrementButtons = new System.Windows.Forms.GroupBox();
            this.tenButton = new System.Windows.Forms.Button();
            this.fiveButton = new System.Windows.Forms.Button();
            this.oneButton = new System.Windows.Forms.Button();
            this.oneForthButton = new System.Windows.Forms.Button();
            this.editButton = new System.Windows.Forms.Button();
            this.errorLabel = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.incrementDeclanationButtons = new System.Windows.Forms.GroupBox();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.button4 = new System.Windows.Forms.Button();
            this.button5 = new System.Windows.Forms.Button();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.label3 = new System.Windows.Forms.Label();
            this.button10 = new System.Windows.Forms.Button();
            this.immediateRB = new System.Windows.Forms.RadioButton();
            this.controlRB = new System.Windows.Forms.RadioButton();
            this.speedCombo = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.button6 = new System.Windows.Forms.Button();
            this.button7 = new System.Windows.Forms.Button();
            this.button8 = new System.Windows.Forms.Button();
            this.button9 = new System.Windows.Forms.Button();
            this.rightAscensionIncrementButtons.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.incrementDeclanationButtons.SuspendLayout();
            this.groupBox3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.groupBox5.SuspendLayout();
            this.SuspendLayout();
            // 
            // PosDecButton
            // 
            this.PosDecButton.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.PosDecButton.BackColor = System.Drawing.Color.DarkGray;
            this.PosDecButton.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.PosDecButton.Location = new System.Drawing.Point(296, 62);
            this.PosDecButton.Name = "PosDecButton";
            this.PosDecButton.Size = new System.Drawing.Size(40, 40);
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
            this.NegDecButton.Location = new System.Drawing.Point(296, 149);
            this.NegDecButton.Name = "NegDecButton";
            this.NegDecButton.Size = new System.Drawing.Size(40, 40);
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
            this.NegRAButton.Location = new System.Drawing.Point(255, 106);
            this.NegRAButton.Name = "NegRAButton";
            this.NegRAButton.Size = new System.Drawing.Size(40, 40);
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
            this.PosRAButton.Location = new System.Drawing.Point(342, 106);
            this.PosRAButton.Name = "PosRAButton";
            this.PosRAButton.Size = new System.Drawing.Size(40, 40);
            this.PosRAButton.TabIndex = 3;
            this.PosRAButton.Text = "+ RA";
            this.PosRAButton.UseVisualStyleBackColor = false;
            this.PosRAButton.Click += new System.EventHandler(this.PosRAButton_Click);
            // 
            // ActualRATextBox
            // 
            this.ActualRATextBox.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.ActualRATextBox.Location = new System.Drawing.Point(125, 68);
            this.ActualRATextBox.Name = "ActualRATextBox";
            this.ActualRATextBox.ReadOnly = true;
            this.ActualRATextBox.Size = new System.Drawing.Size(100, 20);
            this.ActualRATextBox.TabIndex = 5;
            // 
            // ActualDecTextBox
            // 
            this.ActualDecTextBox.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.ActualDecTextBox.Location = new System.Drawing.Point(125, 121);
            this.ActualDecTextBox.Name = "ActualDecTextBox";
            this.ActualDecTextBox.ReadOnly = true;
            this.ActualDecTextBox.Size = new System.Drawing.Size(100, 20);
            this.ActualDecTextBox.TabIndex = 6;
            // 
            // ActualPositionLabel
            // 
            this.ActualPositionLabel.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.ActualPositionLabel.AutoSize = true;
            this.ActualPositionLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.ActualPositionLabel.Location = new System.Drawing.Point(127, 29);
            this.ActualPositionLabel.Name = "ActualPositionLabel";
            this.ActualPositionLabel.Size = new System.Drawing.Size(114, 20);
            this.ActualPositionLabel.TabIndex = 7;
            this.ActualPositionLabel.Text = "Actual Position";
            // 
            // ActualRALabel
            // 
            this.ActualRALabel.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.ActualRALabel.AutoSize = true;
            this.ActualRALabel.Location = new System.Drawing.Point(123, 50);
            this.ActualRALabel.Name = "ActualRALabel";
            this.ActualRALabel.Size = new System.Drawing.Size(84, 13);
            this.ActualRALabel.TabIndex = 8;
            this.ActualRALabel.Text = "Right Ascension";
            // 
            // ActualDecLabel
            // 
            this.ActualDecLabel.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.ActualDecLabel.AutoSize = true;
            this.ActualDecLabel.Location = new System.Drawing.Point(123, 103);
            this.ActualDecLabel.Name = "ActualDecLabel";
            this.ActualDecLabel.Size = new System.Drawing.Size(60, 13);
            this.ActualDecLabel.TabIndex = 9;
            this.ActualDecLabel.Text = "Declination";
            // 
            // TargetDecLabel
            // 
            this.TargetDecLabel.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.TargetDecLabel.AutoSize = true;
            this.TargetDecLabel.Location = new System.Drawing.Point(6, 103);
            this.TargetDecLabel.Name = "TargetDecLabel";
            this.TargetDecLabel.Size = new System.Drawing.Size(60, 13);
            this.TargetDecLabel.TabIndex = 14;
            this.TargetDecLabel.Text = "Declination";
            // 
            // TargetRALabel
            // 
            this.TargetRALabel.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.TargetRALabel.AutoSize = true;
            this.TargetRALabel.Location = new System.Drawing.Point(7, 50);
            this.TargetRALabel.Name = "TargetRALabel";
            this.TargetRALabel.Size = new System.Drawing.Size(84, 13);
            this.TargetRALabel.TabIndex = 13;
            this.TargetRALabel.Text = "Right Ascension";
            // 
            // TargetPositionLabel
            // 
            this.TargetPositionLabel.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.TargetPositionLabel.AutoSize = true;
            this.TargetPositionLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.TargetPositionLabel.Location = new System.Drawing.Point(6, 29);
            this.TargetPositionLabel.Name = "TargetPositionLabel";
            this.TargetPositionLabel.Size = new System.Drawing.Size(115, 20);
            this.TargetPositionLabel.TabIndex = 12;
            this.TargetPositionLabel.Text = "Target Position";
            // 
            // TargetDecTextBox
            // 
            this.TargetDecTextBox.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.TargetDecTextBox.Location = new System.Drawing.Point(8, 121);
            this.TargetDecTextBox.Name = "TargetDecTextBox";
            this.TargetDecTextBox.ReadOnly = true;
            this.TargetDecTextBox.Size = new System.Drawing.Size(100, 20);
            this.TargetDecTextBox.TabIndex = 11;
            // 
            // TargetRATextBox
            // 
            this.TargetRATextBox.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.TargetRATextBox.Location = new System.Drawing.Point(8, 68);
            this.TargetRATextBox.Name = "TargetRATextBox";
            this.TargetRATextBox.ReadOnly = true;
            this.TargetRATextBox.Size = new System.Drawing.Size(100, 20);
            this.TargetRATextBox.TabIndex = 10;
            // 
            // Title
            // 
            this.Title.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.Title.AutoSize = true;
            this.Title.Font = new System.Drawing.Font("Microsoft Sans Serif", 30F);
            this.Title.Location = new System.Drawing.Point(11, 9);
            this.Title.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.Title.Name = "Title";
            this.Title.Size = new System.Drawing.Size(462, 46);
            this.Title.TabIndex = 15;
            this.Title.Text = "Radio Telescope Control";
            // 
            // timer1
            // 
            this.timer1.Enabled = true;
            this.timer1.Interval = 1000;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // rightAscensionIncrementButtons
            // 
            this.rightAscensionIncrementButtons.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.rightAscensionIncrementButtons.BackColor = System.Drawing.Color.LightGray;
            this.rightAscensionIncrementButtons.Controls.Add(this.tenButton);
            this.rightAscensionIncrementButtons.Controls.Add(this.fiveButton);
            this.rightAscensionIncrementButtons.Controls.Add(this.oneButton);
            this.rightAscensionIncrementButtons.Controls.Add(this.oneForthButton);
            this.rightAscensionIncrementButtons.Location = new System.Drawing.Point(2, 2);
            this.rightAscensionIncrementButtons.Margin = new System.Windows.Forms.Padding(2);
            this.rightAscensionIncrementButtons.Name = "rightAscensionIncrementButtons";
            this.rightAscensionIncrementButtons.Padding = new System.Windows.Forms.Padding(2);
            this.rightAscensionIncrementButtons.Size = new System.Drawing.Size(219, 49);
            this.rightAscensionIncrementButtons.TabIndex = 16;
            this.rightAscensionIncrementButtons.TabStop = false;
            this.rightAscensionIncrementButtons.Text = "Increment Right Ascension";
            // 
            // tenButton
            // 
            this.tenButton.BackColor = System.Drawing.Color.DarkGray;
            this.tenButton.Location = new System.Drawing.Point(182, 18);
            this.tenButton.Margin = new System.Windows.Forms.Padding(2);
            this.tenButton.Name = "tenButton";
            this.tenButton.Size = new System.Drawing.Size(30, 30);
            this.tenButton.TabIndex = 3;
            this.tenButton.Text = "10";
            this.tenButton.UseVisualStyleBackColor = false;
            this.tenButton.Click += new System.EventHandler(this.tenButton_Click);
            // 
            // fiveButton
            // 
            this.fiveButton.BackColor = System.Drawing.Color.DarkGray;
            this.fiveButton.Location = new System.Drawing.Point(124, 18);
            this.fiveButton.Margin = new System.Windows.Forms.Padding(2);
            this.fiveButton.Name = "fiveButton";
            this.fiveButton.Size = new System.Drawing.Size(30, 30);
            this.fiveButton.TabIndex = 2;
            this.fiveButton.Text = "5";
            this.fiveButton.UseVisualStyleBackColor = false;
            this.fiveButton.Click += new System.EventHandler(this.fiveButton_Click);
            // 
            // oneButton
            // 
            this.oneButton.BackColor = System.Drawing.Color.DarkGray;
            this.oneButton.Location = new System.Drawing.Point(65, 18);
            this.oneButton.Margin = new System.Windows.Forms.Padding(2);
            this.oneButton.Name = "oneButton";
            this.oneButton.Size = new System.Drawing.Size(30, 30);
            this.oneButton.TabIndex = 1;
            this.oneButton.Text = "1";
            this.oneButton.UseVisualStyleBackColor = false;
            this.oneButton.Click += new System.EventHandler(this.oneButton_Click);
            // 
            // oneForthButton
            // 
            this.oneForthButton.BackColor = System.Drawing.Color.DarkGray;
            this.oneForthButton.Location = new System.Drawing.Point(7, 18);
            this.oneForthButton.Margin = new System.Windows.Forms.Padding(2);
            this.oneForthButton.Name = "oneForthButton";
            this.oneForthButton.Size = new System.Drawing.Size(30, 30);
            this.oneForthButton.TabIndex = 0;
            this.oneForthButton.Text = "0.25";
            this.oneForthButton.UseVisualStyleBackColor = false;
            this.oneForthButton.Click += new System.EventHandler(this.oneForthButton_Click);
            // 
            // editButton
            // 
            this.editButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.editButton.BackColor = System.Drawing.Color.OrangeRed;
            this.editButton.Location = new System.Drawing.Point(50, 24);
            this.editButton.Name = "editButton";
            this.editButton.Size = new System.Drawing.Size(100, 25);
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
            this.errorLabel.Location = new System.Drawing.Point(283, 422);
            this.errorLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.errorLabel.Name = "errorLabel";
            this.errorLabel.Size = new System.Drawing.Size(230, 19);
            this.errorLabel.TabIndex = 18;
            this.errorLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // groupBox1
            // 
            this.groupBox1.BackColor = System.Drawing.Color.Gainsboro;
            this.groupBox1.Controls.Add(this.ActualRATextBox);
            this.groupBox1.Controls.Add(this.ActualDecTextBox);
            this.groupBox1.Controls.Add(this.ActualPositionLabel);
            this.groupBox1.Controls.Add(this.ActualRALabel);
            this.groupBox1.Controls.Add(this.ActualDecLabel);
            this.groupBox1.Controls.Add(this.TargetDecLabel);
            this.groupBox1.Controls.Add(this.TargetRATextBox);
            this.groupBox1.Controls.Add(this.TargetRALabel);
            this.groupBox1.Controls.Add(this.TargetDecTextBox);
            this.groupBox1.Controls.Add(this.TargetPositionLabel);
            this.groupBox1.Location = new System.Drawing.Point(27, 58);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(272, 149);
            this.groupBox1.TabIndex = 19;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Position Information";
            // 
            // incrementDeclanationButtons
            // 
            this.incrementDeclanationButtons.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.incrementDeclanationButtons.BackColor = System.Drawing.Color.LightGray;
            this.incrementDeclanationButtons.Controls.Add(this.button1);
            this.incrementDeclanationButtons.Controls.Add(this.button2);
            this.incrementDeclanationButtons.Controls.Add(this.button3);
            this.incrementDeclanationButtons.Controls.Add(this.button4);
            this.incrementDeclanationButtons.Location = new System.Drawing.Point(0, 2);
            this.incrementDeclanationButtons.Margin = new System.Windows.Forms.Padding(2);
            this.incrementDeclanationButtons.Name = "incrementDeclanationButtons";
            this.incrementDeclanationButtons.Padding = new System.Windows.Forms.Padding(2);
            this.incrementDeclanationButtons.Size = new System.Drawing.Size(219, 48);
            this.incrementDeclanationButtons.TabIndex = 20;
            this.incrementDeclanationButtons.TabStop = false;
            this.incrementDeclanationButtons.Text = "Increment Declanation";
            // 
            // button1
            // 
            this.button1.BackColor = System.Drawing.Color.DarkGray;
            this.button1.Location = new System.Drawing.Point(182, 18);
            this.button1.Margin = new System.Windows.Forms.Padding(2);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(30, 30);
            this.button1.TabIndex = 3;
            this.button1.Text = "10";
            this.button1.UseVisualStyleBackColor = false;
            // 
            // button2
            // 
            this.button2.BackColor = System.Drawing.Color.DarkGray;
            this.button2.Location = new System.Drawing.Point(124, 18);
            this.button2.Margin = new System.Windows.Forms.Padding(2);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(30, 30);
            this.button2.TabIndex = 2;
            this.button2.Text = "5";
            this.button2.UseVisualStyleBackColor = false;
            // 
            // button3
            // 
            this.button3.BackColor = System.Drawing.Color.DarkGray;
            this.button3.Location = new System.Drawing.Point(65, 18);
            this.button3.Margin = new System.Windows.Forms.Padding(2);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(30, 30);
            this.button3.TabIndex = 1;
            this.button3.Text = "1";
            this.button3.UseVisualStyleBackColor = false;
            // 
            // button4
            // 
            this.button4.BackColor = System.Drawing.Color.DarkGray;
            this.button4.Location = new System.Drawing.Point(7, 18);
            this.button4.Margin = new System.Windows.Forms.Padding(2);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(30, 30);
            this.button4.TabIndex = 0;
            this.button4.Text = "0.25";
            this.button4.UseVisualStyleBackColor = false;
            // 
            // button5
            // 
            this.button5.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.button5.BackColor = System.Drawing.Color.LimeGreen;
            this.button5.Location = new System.Drawing.Point(264, 24);
            this.button5.Name = "button5";
            this.button5.Size = new System.Drawing.Size(100, 25);
            this.button5.TabIndex = 21;
            this.button5.Text = "Apply Changes";
            this.button5.UseVisualStyleBackColor = false;
            // 
            // groupBox3
            // 
            this.groupBox3.BackColor = System.Drawing.Color.Gainsboro;
            this.groupBox3.Controls.Add(this.splitContainer1);
            this.groupBox3.Controls.Add(this.NegRAButton);
            this.groupBox3.Controls.Add(this.button5);
            this.groupBox3.Controls.Add(this.PosDecButton);
            this.groupBox3.Controls.Add(this.NegDecButton);
            this.groupBox3.Controls.Add(this.PosRAButton);
            this.groupBox3.Controls.Add(this.editButton);
            this.groupBox3.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.groupBox3.Location = new System.Drawing.Point(19, 213);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(405, 201);
            this.groupBox3.TabIndex = 22;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Edit Target Position";
            // 
            // splitContainer1
            // 
            this.splitContainer1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.splitContainer1.Location = new System.Drawing.Point(6, 62);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.rightAscensionIncrementButtons);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.incrementDeclanationButtons);
            this.splitContainer1.Size = new System.Drawing.Size(225, 133);
            this.splitContainer1.SplitterDistance = 66;
            this.splitContainer1.TabIndex = 22;
            // 
            // comboBox1
            // 
            this.comboBox1.BackColor = System.Drawing.Color.DarkGray;
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Items.AddRange(new object[] {
            "Calibrate",
            "Snow Dump",
            "Full 360 Rotation"});
            this.comboBox1.Location = new System.Drawing.Point(51, 19);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(218, 21);
            this.comboBox1.TabIndex = 23;
            // 
            // groupBox4
            // 
            this.groupBox4.BackColor = System.Drawing.Color.Gainsboro;
            this.groupBox4.Controls.Add(this.comboBox1);
            this.groupBox4.Location = new System.Drawing.Point(422, 67);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(335, 66);
            this.groupBox4.TabIndex = 24;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Scripts";
            // 
            // groupBox5
            // 
            this.groupBox5.BackColor = System.Drawing.Color.Gainsboro;
            this.groupBox5.Controls.Add(this.label3);
            this.groupBox5.Controls.Add(this.button10);
            this.groupBox5.Controls.Add(this.immediateRB);
            this.groupBox5.Controls.Add(this.controlRB);
            this.groupBox5.Controls.Add(this.speedCombo);
            this.groupBox5.Controls.Add(this.label2);
            this.groupBox5.Controls.Add(this.label1);
            this.groupBox5.Controls.Add(this.button6);
            this.groupBox5.Controls.Add(this.button7);
            this.groupBox5.Controls.Add(this.button8);
            this.groupBox5.Controls.Add(this.button9);
            this.groupBox5.Location = new System.Drawing.Point(455, 186);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(302, 228);
            this.groupBox5.TabIndex = 25;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "Manual Control";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(6, 28);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(38, 13);
            this.label3.TabIndex = 26;
            this.label3.Text = "Speed";
            // 
            // button10
            // 
            this.button10.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.button10.BackColor = System.Drawing.Color.OrangeRed;
            this.button10.Location = new System.Drawing.Point(155, 40);
            this.button10.Name = "button10";
            this.button10.Size = new System.Drawing.Size(136, 25);
            this.button10.TabIndex = 25;
            this.button10.Text = "Activate Manual Control";
            this.button10.UseVisualStyleBackColor = false;
            // 
            // immediateRB
            // 
            this.immediateRB.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.immediateRB.AutoSize = true;
            this.immediateRB.Location = new System.Drawing.Point(28, 192);
            this.immediateRB.Name = "immediateRB";
            this.immediateRB.Size = new System.Drawing.Size(98, 17);
            this.immediateRB.TabIndex = 24;
            this.immediateRB.Text = "Immediate Stop";
            this.immediateRB.UseVisualStyleBackColor = true;
            // 
            // controlRB
            // 
            this.controlRB.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.controlRB.AutoSize = true;
            this.controlRB.Checked = true;
            this.controlRB.Location = new System.Drawing.Point(29, 169);
            this.controlRB.Name = "controlRB";
            this.controlRB.Size = new System.Drawing.Size(97, 17);
            this.controlRB.TabIndex = 23;
            this.controlRB.TabStop = true;
            this.controlRB.Text = "Controlled Stop";
            this.controlRB.UseVisualStyleBackColor = true;
            this.controlRB.CheckedChanged += new System.EventHandler(this.ControledButtonRadio_CheckedChanged);
            // 
            // speedCombo
            // 
            this.speedCombo.FormattingEnabled = true;
            this.speedCombo.Items.AddRange(new object[] {
            "0.1 RPM",
            "2.0 RPM"});
            this.speedCombo.Location = new System.Drawing.Point(6, 44);
            this.speedCombo.Name = "speedCombo";
            this.speedCombo.Size = new System.Drawing.Size(121, 21);
            this.speedCombo.TabIndex = 10;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(15, 133);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(87, 13);
            this.label2.TabIndex = 9;
            this.label2.Text = "Current Azimuth: ";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(15, 106);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(94, 13);
            this.label1.TabIndex = 8;
            this.label1.Text = "Current Elavation: ";
            this.label1.Click += new System.EventHandler(this.label1_Click);
            // 
            // button6
            // 
            this.button6.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.button6.BackColor = System.Drawing.Color.DarkGray;
            this.button6.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.button6.Location = new System.Drawing.Point(155, 119);
            this.button6.Name = "button6";
            this.button6.Size = new System.Drawing.Size(40, 40);
            this.button6.TabIndex = 6;
            this.button6.Text = "- Jog";
            this.button6.UseVisualStyleBackColor = false;
            this.button6.Click += new System.EventHandler(this.button6_Click);
            // 
            // button7
            // 
            this.button7.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.button7.BackColor = System.Drawing.Color.DarkGray;
            this.button7.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.button7.Location = new System.Drawing.Point(196, 71);
            this.button7.Name = "button7";
            this.button7.Size = new System.Drawing.Size(40, 40);
            this.button7.TabIndex = 4;
            this.button7.Text = "+ Ela";
            this.button7.UseVisualStyleBackColor = false;
            // 
            // button8
            // 
            this.button8.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.button8.BackColor = System.Drawing.Color.DarkGray;
            this.button8.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.button8.Location = new System.Drawing.Point(196, 165);
            this.button8.Name = "button8";
            this.button8.Size = new System.Drawing.Size(40, 40);
            this.button8.TabIndex = 5;
            this.button8.Text = "- Ela";
            this.button8.UseVisualStyleBackColor = false;
            // 
            // button9
            // 
            this.button9.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.button9.BackColor = System.Drawing.Color.DarkGray;
            this.button9.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.button9.Location = new System.Drawing.Point(240, 119);
            this.button9.Name = "button9";
            this.button9.Size = new System.Drawing.Size(40, 40);
            this.button9.TabIndex = 7;
            this.button9.Text = "+ Jog";
            this.button9.UseVisualStyleBackColor = false;
            // 
            // FreeControlForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Gray;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.groupBox5);
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.errorLabel);
            this.Controls.Add(this.Title);
            this.MinimumSize = new System.Drawing.Size(816, 489);
            this.Name = "FreeControlForm";
            this.Text = "FreeControlForm";
            this.rightAscensionIncrementButtons.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.incrementDeclanationButtons.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.groupBox4.ResumeLayout(false);
            this.groupBox5.ResumeLayout(false);
            this.groupBox5.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button PosDecButton;
        private System.Windows.Forms.Button NegDecButton;
        private System.Windows.Forms.Button NegRAButton;
        private System.Windows.Forms.Button PosRAButton;
        private System.Windows.Forms.TextBox ActualRATextBox;
        private System.Windows.Forms.TextBox ActualDecTextBox;
        private System.Windows.Forms.Label ActualPositionLabel;
        private System.Windows.Forms.Label ActualRALabel;
        private System.Windows.Forms.Label ActualDecLabel;
        private System.Windows.Forms.Label TargetDecLabel;
        private System.Windows.Forms.Label TargetRALabel;
        private System.Windows.Forms.Label TargetPositionLabel;
        private System.Windows.Forms.TextBox TargetDecTextBox;
        private System.Windows.Forms.TextBox TargetRATextBox;
        private System.Windows.Forms.Label Title;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.GroupBox rightAscensionIncrementButtons;
        private System.Windows.Forms.Button tenButton;
        private System.Windows.Forms.Button fiveButton;
        private System.Windows.Forms.Button oneButton;
        private System.Windows.Forms.Button oneForthButton;
        private System.Windows.Forms.Button editButton;
        private System.Windows.Forms.Label errorLabel;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox incrementDeclanationButtons;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.Button button5;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button button6;
        private System.Windows.Forms.Button button7;
        private System.Windows.Forms.Button button8;
        private System.Windows.Forms.Button button9;
        private System.Windows.Forms.ComboBox speedCombo;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.RadioButton immediateRB;
        private System.Windows.Forms.RadioButton controlRB;
        private System.Windows.Forms.Button button10;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.SplitContainer splitContainer1;
    }
}