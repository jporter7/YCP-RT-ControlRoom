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
            this.IncrementButtons = new System.Windows.Forms.GroupBox();
            this.tenButton = new System.Windows.Forms.Button();
            this.fiveButton = new System.Windows.Forms.Button();
            this.oneButton = new System.Windows.Forms.Button();
            this.oneForthButton = new System.Windows.Forms.Button();
            this.editButton = new System.Windows.Forms.Button();
            this.errorLabel = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.tenButtonDec = new System.Windows.Forms.Button();
            this.fiveButtonDec = new System.Windows.Forms.Button();
            this.oneButtonDec = new System.Windows.Forms.Button();
            this.oneForthButtonDec = new System.Windows.Forms.Button();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.manualControlButton = new System.Windows.Forms.Button();
            this.immediateRadioButton = new System.Windows.Forms.RadioButton();
            this.ControledButtonRadio = new System.Windows.Forms.RadioButton();
            this.speedComboBox = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.subJogButton = new System.Windows.Forms.Button();
            this.plusElaButton = new System.Windows.Forms.Button();
            this.subElaButton = new System.Windows.Forms.Button();
            this.plusJogButton = new System.Windows.Forms.Button();
            this.IncrementButtons.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.groupBox5.SuspendLayout();
            this.SuspendLayout();
            // 
            // PosDecButton
            // 
            this.PosDecButton.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.PosDecButton.BackColor = System.Drawing.Color.DarkGray;
            this.PosDecButton.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.PosDecButton.Location = new System.Drawing.Point(296, 55);
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
            this.NegDecButton.Location = new System.Drawing.Point(296, 151);
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
            this.NegRAButton.Location = new System.Drawing.Point(250, 105);
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
            this.PosRAButton.Location = new System.Drawing.Point(341, 105);
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
            // IncrementButtons
            // 
            this.IncrementButtons.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.IncrementButtons.BackColor = System.Drawing.Color.Gray;
            this.IncrementButtons.Controls.Add(this.tenButton);
            this.IncrementButtons.Controls.Add(this.fiveButton);
            this.IncrementButtons.Controls.Add(this.oneButton);
            this.IncrementButtons.Controls.Add(this.oneForthButton);
            this.IncrementButtons.Location = new System.Drawing.Point(6, 54);
            this.IncrementButtons.Margin = new System.Windows.Forms.Padding(2);
            this.IncrementButtons.Name = "IncrementButtons";
            this.IncrementButtons.Padding = new System.Windows.Forms.Padding(2);
            this.IncrementButtons.Size = new System.Drawing.Size(219, 55);
            this.IncrementButtons.TabIndex = 16;
            this.IncrementButtons.TabStop = false;
            this.IncrementButtons.Text = "Right Ascension Increment";
            // 
            // tenButton
            // 
            this.tenButton.BackColor = System.Drawing.Color.DarkGray;
            this.tenButton.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
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
            this.fiveButton.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
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
            this.oneButton.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
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
            this.oneForthButton.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
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
            this.editButton.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.editButton.Location = new System.Drawing.Point(281, 14);
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
            this.groupBox1.Location = new System.Drawing.Point(25, 58);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(272, 149);
            this.groupBox1.TabIndex = 19;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Position Information";
            // 
            // groupBox2
            // 
            this.groupBox2.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.groupBox2.BackColor = System.Drawing.Color.Gray;
            this.groupBox2.Controls.Add(this.tenButtonDec);
            this.groupBox2.Controls.Add(this.fiveButtonDec);
            this.groupBox2.Controls.Add(this.oneButtonDec);
            this.groupBox2.Controls.Add(this.oneForthButtonDec);
            this.groupBox2.Location = new System.Drawing.Point(6, 128);
            this.groupBox2.Margin = new System.Windows.Forms.Padding(2);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Padding = new System.Windows.Forms.Padding(2);
            this.groupBox2.Size = new System.Drawing.Size(219, 55);
            this.groupBox2.TabIndex = 20;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Declanation Increment";
            // 
            // tenButtonDec
            // 
            this.tenButtonDec.BackColor = System.Drawing.Color.DarkGray;
            this.tenButtonDec.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.tenButtonDec.Location = new System.Drawing.Point(182, 18);
            this.tenButtonDec.Margin = new System.Windows.Forms.Padding(2);
            this.tenButtonDec.Name = "tenButtonDec";
            this.tenButtonDec.Size = new System.Drawing.Size(30, 30);
            this.tenButtonDec.TabIndex = 3;
            this.tenButtonDec.Text = "10";
            this.tenButtonDec.UseVisualStyleBackColor = false;
            // 
            // fiveButtonDec
            // 
            this.fiveButtonDec.BackColor = System.Drawing.Color.DarkGray;
            this.fiveButtonDec.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.fiveButtonDec.Location = new System.Drawing.Point(124, 18);
            this.fiveButtonDec.Margin = new System.Windows.Forms.Padding(2);
            this.fiveButtonDec.Name = "fiveButtonDec";
            this.fiveButtonDec.Size = new System.Drawing.Size(30, 30);
            this.fiveButtonDec.TabIndex = 2;
            this.fiveButtonDec.Text = "5";
            this.fiveButtonDec.UseVisualStyleBackColor = false;
            // 
            // oneButtonDec
            // 
            this.oneButtonDec.BackColor = System.Drawing.Color.DarkGray;
            this.oneButtonDec.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.oneButtonDec.Location = new System.Drawing.Point(65, 18);
            this.oneButtonDec.Margin = new System.Windows.Forms.Padding(2);
            this.oneButtonDec.Name = "oneButtonDec";
            this.oneButtonDec.Size = new System.Drawing.Size(30, 30);
            this.oneButtonDec.TabIndex = 1;
            this.oneButtonDec.Text = "1";
            this.oneButtonDec.UseVisualStyleBackColor = false;
            // 
            // oneForthButtonDec
            // 
            this.oneForthButtonDec.BackColor = System.Drawing.Color.DarkGray;
            this.oneForthButtonDec.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.oneForthButtonDec.Location = new System.Drawing.Point(7, 18);
            this.oneForthButtonDec.Margin = new System.Windows.Forms.Padding(2);
            this.oneForthButtonDec.Name = "oneForthButtonDec";
            this.oneForthButtonDec.Size = new System.Drawing.Size(30, 30);
            this.oneForthButtonDec.TabIndex = 0;
            this.oneForthButtonDec.Text = "0.25";
            this.oneForthButtonDec.UseVisualStyleBackColor = false;
            // 
            // groupBox3
            // 
            this.groupBox3.BackColor = System.Drawing.Color.Gainsboro;
            this.groupBox3.Controls.Add(this.NegRAButton);
            this.groupBox3.Controls.Add(this.PosDecButton);
            this.groupBox3.Controls.Add(this.groupBox2);
            this.groupBox3.Controls.Add(this.NegDecButton);
            this.groupBox3.Controls.Add(this.PosRAButton);
            this.groupBox3.Controls.Add(this.IncrementButtons);
            this.groupBox3.Controls.Add(this.editButton);
            this.groupBox3.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.groupBox3.Location = new System.Drawing.Point(19, 213);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(405, 201);
            this.groupBox3.TabIndex = 22;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Edit Target Position";
            // 
            // comboBox1
            // 
            this.comboBox1.BackColor = System.Drawing.Color.DarkGray;
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Location = new System.Drawing.Point(51, 19);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(234, 21);
            this.comboBox1.TabIndex = 23;
            // 
            // groupBox4
            // 
            this.groupBox4.BackColor = System.Drawing.Color.Gainsboro;
            this.groupBox4.Controls.Add(this.comboBox1);
            this.groupBox4.Location = new System.Drawing.Point(417, 67);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(340, 69);
            this.groupBox4.TabIndex = 24;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Scripts";
            // 
            // groupBox5
            // 
            this.groupBox5.BackColor = System.Drawing.Color.Gainsboro;
            this.groupBox5.Controls.Add(this.label4);
            this.groupBox5.Controls.Add(this.label5);
            this.groupBox5.Controls.Add(this.label3);
            this.groupBox5.Controls.Add(this.manualControlButton);
            this.groupBox5.Controls.Add(this.immediateRadioButton);
            this.groupBox5.Controls.Add(this.ControledButtonRadio);
            this.groupBox5.Controls.Add(this.speedComboBox);
            this.groupBox5.Controls.Add(this.label2);
            this.groupBox5.Controls.Add(this.label1);
            this.groupBox5.Controls.Add(this.subJogButton);
            this.groupBox5.Controls.Add(this.plusElaButton);
            this.groupBox5.Controls.Add(this.subElaButton);
            this.groupBox5.Controls.Add(this.plusJogButton);
            this.groupBox5.Location = new System.Drawing.Point(455, 186);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(302, 228);
            this.groupBox5.TabIndex = 25;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "Manual Control";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(102, 86);
            this.label4.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(22, 13);
            this.label4.TabIndex = 28;
            this.label4.Text = "0.0";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(102, 67);
            this.label5.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(22, 13);
            this.label5.TabIndex = 27;
            this.label5.Text = "0.0";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(6, 182);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(38, 13);
            this.label3.TabIndex = 26;
            this.label3.Text = "Speed";
            // 
            // manualControlButton
            // 
            this.manualControlButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.manualControlButton.BackColor = System.Drawing.Color.OrangeRed;
            this.manualControlButton.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.manualControlButton.Location = new System.Drawing.Point(61, 19);
            this.manualControlButton.Name = "manualControlButton";
            this.manualControlButton.Size = new System.Drawing.Size(186, 25);
            this.manualControlButton.TabIndex = 25;
            this.manualControlButton.Text = "Activate Manual Control";
            this.manualControlButton.UseVisualStyleBackColor = false;
            this.manualControlButton.Click += new System.EventHandler(this.manualControlButton_Click);
            // 
            // immediateRadioButton
            // 
            this.immediateRadioButton.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.immediateRadioButton.AutoSize = true;
            this.immediateRadioButton.Location = new System.Drawing.Point(6, 139);
            this.immediateRadioButton.Name = "immediateRadioButton";
            this.immediateRadioButton.Size = new System.Drawing.Size(98, 17);
            this.immediateRadioButton.TabIndex = 24;
            this.immediateRadioButton.Text = "Immediate Stop";
            this.immediateRadioButton.UseVisualStyleBackColor = true;
            // 
            // ControledButtonRadio
            // 
            this.ControledButtonRadio.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.ControledButtonRadio.AutoSize = true;
            this.ControledButtonRadio.Checked = true;
            this.ControledButtonRadio.Location = new System.Drawing.Point(6, 116);
            this.ControledButtonRadio.Name = "ControledButtonRadio";
            this.ControledButtonRadio.Size = new System.Drawing.Size(97, 17);
            this.ControledButtonRadio.TabIndex = 23;
            this.ControledButtonRadio.TabStop = true;
            this.ControledButtonRadio.Text = "Controlled Stop";
            this.ControledButtonRadio.UseVisualStyleBackColor = true;
            // 
            // speedComboBox
            // 
            this.speedComboBox.FormattingEnabled = true;
            this.speedComboBox.Items.AddRange(new object[] {
            "0.1 RPM",
            "2 RPM"});
            this.speedComboBox.Location = new System.Drawing.Point(9, 198);
            this.speedComboBox.Name = "speedComboBox";
            this.speedComboBox.Size = new System.Drawing.Size(121, 21);
            this.speedComboBox.TabIndex = 10;
            this.speedComboBox.SelectedIndexChanged += new System.EventHandler(this.speedComboBox_SelectedIndexChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(3, 87);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(87, 13);
            this.label2.TabIndex = 9;
            this.label2.Text = "Current Azimuth: ";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 67);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(94, 13);
            this.label1.TabIndex = 8;
            this.label1.Text = "Current Elavation: ";
            this.label1.Click += new System.EventHandler(this.label1_Click);
            // 
            // subJogButton
            // 
            this.subJogButton.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.subJogButton.BackColor = System.Drawing.Color.DarkGray;
            this.subJogButton.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.subJogButton.Location = new System.Drawing.Point(155, 132);
            this.subJogButton.Name = "subJogButton";
            this.subJogButton.Size = new System.Drawing.Size(40, 40);
            this.subJogButton.TabIndex = 6;
            this.subJogButton.Text = "- Jog";
            this.subJogButton.UseVisualStyleBackColor = false;
            // 
            // plusElaButton
            // 
            this.plusElaButton.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.plusElaButton.BackColor = System.Drawing.Color.DarkGray;
            this.plusElaButton.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.plusElaButton.Location = new System.Drawing.Point(196, 81);
            this.plusElaButton.Name = "plusElaButton";
            this.plusElaButton.Size = new System.Drawing.Size(40, 40);
            this.plusElaButton.TabIndex = 4;
            this.plusElaButton.Text = "+ Ela";
            this.plusElaButton.UseVisualStyleBackColor = false;
            // 
            // subElaButton
            // 
            this.subElaButton.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.subElaButton.BackColor = System.Drawing.Color.DarkGray;
            this.subElaButton.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.subElaButton.Location = new System.Drawing.Point(196, 178);
            this.subElaButton.Name = "subElaButton";
            this.subElaButton.Size = new System.Drawing.Size(40, 40);
            this.subElaButton.TabIndex = 5;
            this.subElaButton.Text = "- Ela";
            this.subElaButton.UseVisualStyleBackColor = false;
            // 
            // plusJogButton
            // 
            this.plusJogButton.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.plusJogButton.BackColor = System.Drawing.Color.DarkGray;
            this.plusJogButton.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.plusJogButton.Location = new System.Drawing.Point(241, 132);
            this.plusJogButton.Name = "plusJogButton";
            this.plusJogButton.Size = new System.Drawing.Size(40, 40);
            this.plusJogButton.TabIndex = 7;
            this.plusJogButton.Text = "+ Jog";
            this.plusJogButton.UseVisualStyleBackColor = false;
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
            this.IncrementButtons.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
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
        private System.Windows.Forms.GroupBox IncrementButtons;
        private System.Windows.Forms.Button tenButton;
        private System.Windows.Forms.Button fiveButton;
        private System.Windows.Forms.Button oneButton;
        private System.Windows.Forms.Button oneForthButton;
        private System.Windows.Forms.Button editButton;
        private System.Windows.Forms.Label errorLabel;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button tenButtonDec;
        private System.Windows.Forms.Button fiveButtonDec;
        private System.Windows.Forms.Button oneButtonDec;
        private System.Windows.Forms.Button oneForthButtonDec;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button subJogButton;
        private System.Windows.Forms.Button plusElaButton;
        private System.Windows.Forms.Button subElaButton;
        private System.Windows.Forms.Button plusJogButton;
        private System.Windows.Forms.ComboBox speedComboBox;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.RadioButton immediateRadioButton;
        private System.Windows.Forms.RadioButton ControledButtonRadio;
        private System.Windows.Forms.Button manualControlButton;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
    }
}