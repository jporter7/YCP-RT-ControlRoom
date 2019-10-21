namespace ControlRoomApplication.Main
{
    partial class ManualControlForm
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
            this.NegButton = new System.Windows.Forms.Button();
            this.PosButton = new System.Windows.Forms.Button();
            this.Title = new System.Windows.Forms.Label();
            this.errorLabel = new System.Windows.Forms.Label();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.ControledButtonRadio = new System.Windows.Forms.RadioButton();
            this.radioButton2 = new System.Windows.Forms.RadioButton();
            this.button1 = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.numericUpDown1 = new System.Windows.Forms.NumericUpDown();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.ActualELLabel = new System.Windows.Forms.Label();
            this.ActualAZLabel = new System.Windows.Forms.Label();
            this.ActualPositionLabel = new System.Windows.Forms.Label();
            this.ActualELTextBox = new System.Windows.Forms.TextBox();
            this.ActualAZTextBox = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).BeginInit();
            this.SuspendLayout();
            // 
            // NegButton
            // 
            this.NegButton.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.NegButton.BackColor = System.Drawing.Color.LightGray;
            this.NegButton.Location = new System.Drawing.Point(300, 201);
            this.NegButton.Name = "NegButton";
            this.NegButton.Size = new System.Drawing.Size(75, 54);
            this.NegButton.TabIndex = 2;
            this.NegButton.Text = "- Jog";
            this.NegButton.UseVisualStyleBackColor = false;
            this.NegButton.Click += new System.EventHandler(this.NegButton_Click);
            this.NegButton.MouseDown += new System.Windows.Forms.MouseEventHandler(this.NegButton_MouseDown);
            this.NegButton.MouseUp += new System.Windows.Forms.MouseEventHandler(this.NegButton_MouseUp);
            // 
            // PosButton
            // 
            this.PosButton.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.PosButton.BackColor = System.Drawing.Color.LightGray;
            this.PosButton.Location = new System.Drawing.Point(455, 201);
            this.PosButton.Name = "PosButton";
            this.PosButton.Size = new System.Drawing.Size(75, 54);
            this.PosButton.TabIndex = 3;
            this.PosButton.Text = "+ Jog";
            this.PosButton.UseVisualStyleBackColor = false;
            this.PosButton.Click += new System.EventHandler(this.PosButton_Click);
            this.PosButton.MouseDown += new System.Windows.Forms.MouseEventHandler(this.PosButton_MouseDown);
            this.PosButton.MouseUp += new System.Windows.Forms.MouseEventHandler(this.PosButton_MouseUp);
            // 
            // Title
            // 
            this.Title.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.Title.AutoSize = true;
            this.Title.Font = new System.Drawing.Font("Microsoft Sans Serif", 30F);
            this.Title.Location = new System.Drawing.Point(297, 28);
            this.Title.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.Title.Name = "Title";
            this.Title.Size = new System.Drawing.Size(291, 46);
            this.Title.TabIndex = 15;
            this.Title.Text = "Manual Control";
            // 
            // errorLabel
            // 
            this.errorLabel.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.errorLabel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.errorLabel.ForeColor = System.Drawing.Color.Red;
            this.errorLabel.Location = new System.Drawing.Point(300, 411);
            this.errorLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.errorLabel.Name = "errorLabel";
            this.errorLabel.Size = new System.Drawing.Size(230, 19);
            this.errorLabel.TabIndex = 18;
            this.errorLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // comboBox1
            // 
            this.comboBox1.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Items.AddRange(new object[] {
            "0.1 RPM",
            "2 RPM"});
            this.comboBox1.Location = new System.Drawing.Point(353, 111);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(121, 21);
            this.comboBox1.TabIndex = 19;
            this.comboBox1.SelectedIndexChanged += new System.EventHandler(this.comboBox1_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(391, 95);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(38, 13);
            this.label1.TabIndex = 20;
            this.label1.Text = "Speed";
            // 
            // ControledButtonRadio
            // 
            this.ControledButtonRadio.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.ControledButtonRadio.AutoSize = true;
            this.ControledButtonRadio.Checked = true;
            this.ControledButtonRadio.Location = new System.Drawing.Point(366, 319);
            this.ControledButtonRadio.Name = "ControledButtonRadio";
            this.ControledButtonRadio.Size = new System.Drawing.Size(97, 17);
            this.ControledButtonRadio.TabIndex = 21;
            this.ControledButtonRadio.TabStop = true;
            this.ControledButtonRadio.Text = "Controlled Stop";
            this.ControledButtonRadio.UseVisualStyleBackColor = true;
            this.ControledButtonRadio.CheckedChanged += new System.EventHandler(this.ControledButtonRadio_CheckedChanged);
            // 
            // radioButton2
            // 
            this.radioButton2.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.radioButton2.AutoSize = true;
            this.radioButton2.Location = new System.Drawing.Point(365, 342);
            this.radioButton2.Name = "radioButton2";
            this.radioButton2.Size = new System.Drawing.Size(98, 17);
            this.radioButton2.TabIndex = 22;
            this.radioButton2.Text = "Immediate Stop";
            this.radioButton2.UseVisualStyleBackColor = true;
            // 
            // button1
            // 
            this.button1.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.button1.Location = new System.Drawing.Point(640, 258);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(120, 24);
            this.button1.TabIndex = 23;
            this.button1.Text = "Move Relative";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // label2
            // 
            this.label2.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.label2.Location = new System.Drawing.Point(636, 161);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(115, 20);
            this.label2.TabIndex = 24;
            this.label2.Text = "Target Position";
            // 
            // numericUpDown1
            // 
            this.numericUpDown1.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.numericUpDown1.Location = new System.Drawing.Point(640, 209);
            this.numericUpDown1.Maximum = new decimal(new int[] {
            90000,
            0,
            0,
            0});
            this.numericUpDown1.Minimum = new decimal(new int[] {
            90000,
            0,
            0,
            -2147483648});
            this.numericUpDown1.Name = "numericUpDown1";
            this.numericUpDown1.Size = new System.Drawing.Size(120, 20);
            this.numericUpDown1.TabIndex = 25;
            // 
            // timer1
            // 
            this.timer1.Enabled = true;
            this.timer1.Interval = 1000;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // ActualELLabel
            // 
            this.ActualELLabel.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.ActualELLabel.AutoSize = true;
            this.ActualELLabel.Location = new System.Drawing.Point(63, 244);
            this.ActualELLabel.Name = "ActualELLabel";
            this.ActualELLabel.Size = new System.Drawing.Size(51, 13);
            this.ActualELLabel.TabIndex = 30;
            this.ActualELLabel.Text = "Elevation";
            // 
            // ActualAZLabel
            // 
            this.ActualAZLabel.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.ActualAZLabel.AutoSize = true;
            this.ActualAZLabel.Location = new System.Drawing.Point(63, 191);
            this.ActualAZLabel.Name = "ActualAZLabel";
            this.ActualAZLabel.Size = new System.Drawing.Size(44, 13);
            this.ActualAZLabel.TabIndex = 29;
            this.ActualAZLabel.Text = "Azimuth";
            // 
            // ActualPositionLabel
            // 
            this.ActualPositionLabel.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.ActualPositionLabel.AutoSize = true;
            this.ActualPositionLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.ActualPositionLabel.Location = new System.Drawing.Point(61, 161);
            this.ActualPositionLabel.Name = "ActualPositionLabel";
            this.ActualPositionLabel.Size = new System.Drawing.Size(114, 20);
            this.ActualPositionLabel.TabIndex = 28;
            this.ActualPositionLabel.Text = "Actual Position";
            // 
            // ActualELTextBox
            // 
            this.ActualELTextBox.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.ActualELTextBox.Location = new System.Drawing.Point(65, 262);
            this.ActualELTextBox.Name = "ActualELTextBox";
            this.ActualELTextBox.ReadOnly = true;
            this.ActualELTextBox.Size = new System.Drawing.Size(100, 20);
            this.ActualELTextBox.TabIndex = 27;
            // 
            // ActualAZTextBox
            // 
            this.ActualAZTextBox.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.ActualAZTextBox.Location = new System.Drawing.Point(65, 209);
            this.ActualAZTextBox.Name = "ActualAZTextBox";
            this.ActualAZTextBox.ReadOnly = true;
            this.ActualAZTextBox.Size = new System.Drawing.Size(100, 20);
            this.ActualAZTextBox.TabIndex = 26;
            // 
            // ManualControlForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.ActualELLabel);
            this.Controls.Add(this.ActualAZLabel);
            this.Controls.Add(this.ActualPositionLabel);
            this.Controls.Add(this.ActualELTextBox);
            this.Controls.Add(this.ActualAZTextBox);
            this.Controls.Add(this.numericUpDown1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.radioButton2);
            this.Controls.Add(this.ControledButtonRadio);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.comboBox1);
            this.Controls.Add(this.errorLabel);
            this.Controls.Add(this.Title);
            this.Controls.Add(this.PosButton);
            this.Controls.Add(this.NegButton);
            this.MinimumSize = new System.Drawing.Size(816, 489);
            this.Name = "ManualControlForm";
            this.Text = "ManualControlForm";
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button NegButton;
        private System.Windows.Forms.Button PosButton;
        private System.Windows.Forms.Label Title;
        private System.Windows.Forms.Label errorLabel;
        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.RadioButton ControledButtonRadio;
        private System.Windows.Forms.RadioButton radioButton2;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.NumericUpDown numericUpDown1;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Label ActualELLabel;
        private System.Windows.Forms.Label ActualAZLabel;
        private System.Windows.Forms.Label ActualPositionLabel;
        private System.Windows.Forms.TextBox ActualELTextBox;
        private System.Windows.Forms.TextBox ActualAZTextBox;
    }
}