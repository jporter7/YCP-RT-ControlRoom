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
            this.NegButtonAZ = new System.Windows.Forms.Button();
            this.PosButtonAZ = new System.Windows.Forms.Button();
            this.Title = new System.Windows.Forms.Label();
            this.errorLabel = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.radioButton1 = new System.Windows.Forms.RadioButton();
            this.radioButton2 = new System.Windows.Forms.RadioButton();
            this.RelativeMoveSubmit = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.numericUpDown1 = new System.Windows.Forms.NumericUpDown();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.ActualELLabel = new System.Windows.Forms.Label();
            this.ActualAZLabel = new System.Windows.Forms.Label();
            this.ActualPositionLabel = new System.Windows.Forms.Label();
            this.ActualELTextBox = new System.Windows.Forms.TextBox();
            this.ActualAZTextBox = new System.Windows.Forms.TextBox();
            this.DemoButton = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.NegButtonEL = new System.Windows.Forms.Button();
            this.PosButtonEL = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.numericUpDown2 = new System.Windows.Forms.NumericUpDown();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.AbsoluteMoveSubmit = new System.Windows.Forms.Button();
            this.ClearCommandsSubmit = new System.Windows.Forms.Button();
            this.errorProvider1 = new System.Windows.Forms.ErrorProvider(this.components);
            this.SpeedInput = new System.Windows.Forms.NumericUpDown();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.SpeedInput)).BeginInit();
            this.SuspendLayout();
            // 
            // NegButtonAZ
            // 
            this.NegButtonAZ.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.NegButtonAZ.BackColor = System.Drawing.Color.LightGray;
            this.NegButtonAZ.Location = new System.Drawing.Point(300, 175);
            this.NegButtonAZ.Name = "NegButtonAZ";
            this.NegButtonAZ.Size = new System.Drawing.Size(75, 54);
            this.NegButtonAZ.TabIndex = 2;
            this.NegButtonAZ.Text = "- Jog";
            this.NegButtonAZ.UseVisualStyleBackColor = false;
            this.NegButtonAZ.MouseDown += new System.Windows.Forms.MouseEventHandler(this.NegButtonAZ_MouseDown);
            this.NegButtonAZ.MouseUp += new System.Windows.Forms.MouseEventHandler(this.NegButtonAZ_MouseUp);
            // 
            // PosButtonAZ
            // 
            this.PosButtonAZ.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.PosButtonAZ.BackColor = System.Drawing.Color.LightGray;
            this.PosButtonAZ.Location = new System.Drawing.Point(455, 175);
            this.PosButtonAZ.Name = "PosButtonAZ";
            this.PosButtonAZ.Size = new System.Drawing.Size(75, 54);
            this.PosButtonAZ.TabIndex = 3;
            this.PosButtonAZ.Text = "+ Jog";
            this.PosButtonAZ.UseVisualStyleBackColor = false;
            this.PosButtonAZ.MouseDown += new System.Windows.Forms.MouseEventHandler(this.PosButtonAZ_MouseDown);
            this.PosButtonAZ.MouseUp += new System.Windows.Forms.MouseEventHandler(this.PosButtonAZ_MouseUp);
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
            // label1
            // 
            this.label1.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(391, 92);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(38, 13);
            this.label1.TabIndex = 20;
            this.label1.Text = "Speed";
            // 
            // radioButton1
            // 
            this.radioButton1.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.radioButton1.AutoSize = true;
            this.radioButton1.Checked = true;
            this.radioButton1.Location = new System.Drawing.Point(366, 319);
            this.radioButton1.Name = "radioButton1";
            this.radioButton1.Size = new System.Drawing.Size(97, 17);
            this.radioButton1.TabIndex = 21;
            this.radioButton1.TabStop = true;
            this.radioButton1.Text = "Controlled Stop";
            this.radioButton1.UseVisualStyleBackColor = true;
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
            // RelativeMoveSubmit
            // 
            this.RelativeMoveSubmit.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.RelativeMoveSubmit.Location = new System.Drawing.Point(633, 265);
            this.RelativeMoveSubmit.Name = "RelativeMoveSubmit";
            this.RelativeMoveSubmit.Size = new System.Drawing.Size(120, 24);
            this.RelativeMoveSubmit.TabIndex = 23;
            this.RelativeMoveSubmit.Text = "Relative Move";
            this.RelativeMoveSubmit.UseVisualStyleBackColor = true;
            this.RelativeMoveSubmit.Click += new System.EventHandler(this.RelativeMoveSubmit_Click);
            // 
            // label2
            // 
            this.label2.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.label2.Location = new System.Drawing.Point(647, 161);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(94, 20);
            this.label2.TabIndex = 24;
            this.label2.Text = "Set Position";
            // 
            // numericUpDown1
            // 
            this.numericUpDown1.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.numericUpDown1.Location = new System.Drawing.Point(638, 208);
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
            this.numericUpDown1.Size = new System.Drawing.Size(43, 20);
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
            this.ActualELLabel.Location = new System.Drawing.Point(63, 230);
            this.ActualELLabel.Name = "ActualELLabel";
            this.ActualELLabel.Size = new System.Drawing.Size(51, 13);
            this.ActualELLabel.TabIndex = 30;
            this.ActualELLabel.Text = "Elevation";
            // 
            // ActualAZLabel
            // 
            this.ActualAZLabel.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.ActualAZLabel.AutoSize = true;
            this.ActualAZLabel.Location = new System.Drawing.Point(63, 195);
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
            this.ActualPositionLabel.Size = new System.Drawing.Size(122, 20);
            this.ActualPositionLabel.TabIndex = 28;
            this.ActualPositionLabel.Text = "Current Position";
            // 
            // ActualELTextBox
            // 
            this.ActualELTextBox.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.ActualELTextBox.Location = new System.Drawing.Point(124, 227);
            this.ActualELTextBox.Name = "ActualELTextBox";
            this.ActualELTextBox.ReadOnly = true;
            this.ActualELTextBox.Size = new System.Drawing.Size(52, 20);
            this.ActualELTextBox.TabIndex = 27;
            // 
            // ActualAZTextBox
            // 
            this.ActualAZTextBox.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.ActualAZTextBox.Location = new System.Drawing.Point(124, 192);
            this.ActualAZTextBox.Name = "ActualAZTextBox";
            this.ActualAZTextBox.ReadOnly = true;
            this.ActualAZTextBox.Size = new System.Drawing.Size(52, 20);
            this.ActualAZTextBox.TabIndex = 26;
            // 
            // DemoButton
            // 
            this.DemoButton.Location = new System.Drawing.Point(589, 375);
            this.DemoButton.Margin = new System.Windows.Forms.Padding(2);
            this.DemoButton.Name = "DemoButton";
            this.DemoButton.Size = new System.Drawing.Size(171, 55);
            this.DemoButton.TabIndex = 31;
            this.DemoButton.Text = "Start Demo";
            this.DemoButton.UseVisualStyleBackColor = true;
            this.DemoButton.Click += new System.EventHandler(this.DemoButton_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(393, 196);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(44, 13);
            this.label3.TabIndex = 32;
            this.label3.Text = "Azimuth";
            // 
            // NegButtonEL
            // 
            this.NegButtonEL.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.NegButtonEL.BackColor = System.Drawing.Color.LightGray;
            this.NegButtonEL.Location = new System.Drawing.Point(300, 235);
            this.NegButtonEL.Name = "NegButtonEL";
            this.NegButtonEL.Size = new System.Drawing.Size(75, 54);
            this.NegButtonEL.TabIndex = 33;
            this.NegButtonEL.Text = "- Jog";
            this.NegButtonEL.UseVisualStyleBackColor = false;
            this.NegButtonEL.MouseDown += new System.Windows.Forms.MouseEventHandler(this.NegButtonEL_MouseDown);
            this.NegButtonEL.MouseUp += new System.Windows.Forms.MouseEventHandler(this.NegButtonEL_MouseUp);
            // 
            // PosButtonEL
            // 
            this.PosButtonEL.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.PosButtonEL.BackColor = System.Drawing.Color.LightGray;
            this.PosButtonEL.Location = new System.Drawing.Point(455, 235);
            this.PosButtonEL.Name = "PosButtonEL";
            this.PosButtonEL.Size = new System.Drawing.Size(75, 54);
            this.PosButtonEL.TabIndex = 34;
            this.PosButtonEL.Text = "+ Jog";
            this.PosButtonEL.UseVisualStyleBackColor = false;
            this.PosButtonEL.MouseDown += new System.Windows.Forms.MouseEventHandler(this.PosButtonEL_MouseDown);
            this.PosButtonEL.MouseUp += new System.Windows.Forms.MouseEventHandler(this.PosButtonEL_MouseUp);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(390, 256);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(51, 13);
            this.label4.TabIndex = 35;
            this.label4.Text = "Elevation";
            // 
            // numericUpDown2
            // 
            this.numericUpDown2.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.numericUpDown2.Location = new System.Drawing.Point(706, 208);
            this.numericUpDown2.Maximum = new decimal(new int[] {
            90000,
            0,
            0,
            0});
            this.numericUpDown2.Minimum = new decimal(new int[] {
            90000,
            0,
            0,
            -2147483648});
            this.numericUpDown2.Name = "numericUpDown2";
            this.numericUpDown2.Size = new System.Drawing.Size(43, 20);
            this.numericUpDown2.TabIndex = 36;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(649, 192);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(21, 13);
            this.label5.TabIndex = 37;
            this.label5.Text = "AZ";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(717, 192);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(20, 13);
            this.label6.TabIndex = 38;
            this.label6.Text = "EL";
            // 
            // AbsoluteMoveSubmit
            // 
            this.AbsoluteMoveSubmit.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.AbsoluteMoveSubmit.Location = new System.Drawing.Point(633, 238);
            this.AbsoluteMoveSubmit.Name = "AbsoluteMoveSubmit";
            this.AbsoluteMoveSubmit.Size = new System.Drawing.Size(120, 24);
            this.AbsoluteMoveSubmit.TabIndex = 39;
            this.AbsoluteMoveSubmit.Text = "Absolute Move";
            this.AbsoluteMoveSubmit.UseVisualStyleBackColor = true;
            this.AbsoluteMoveSubmit.Click += new System.EventHandler(this.AbsoluteMoveSubmit_Click);
            // 
            // ClearCommandsSubmit
            // 
            this.ClearCommandsSubmit.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.ClearCommandsSubmit.Location = new System.Drawing.Point(633, 292);
            this.ClearCommandsSubmit.Name = "ClearCommandsSubmit";
            this.ClearCommandsSubmit.Size = new System.Drawing.Size(120, 24);
            this.ClearCommandsSubmit.TabIndex = 40;
            this.ClearCommandsSubmit.Text = "Clear Commands";
            this.ClearCommandsSubmit.UseVisualStyleBackColor = true;
            this.ClearCommandsSubmit.Click += new System.EventHandler(this.ClearCommandsSubmit_Click);
            // 
            // errorProvider1
            // 
            this.errorProvider1.ContainerControl = this;
            // 
            // SpeedInput
            // 
            this.SpeedInput.DecimalPlaces = 1;
            this.SpeedInput.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.SpeedInput.Location = new System.Drawing.Point(352, 108);
            this.SpeedInput.Name = "SpeedInput";
            this.SpeedInput.Size = new System.Drawing.Size(120, 20);
            this.SpeedInput.TabIndex = 42;
            this.SpeedInput.Value = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.SpeedInput.ValueChanged += new System.EventHandler(this.SpeedInput_ValueChanged);
            // 
            // ManualControlForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.SpeedInput);
            this.Controls.Add(this.ClearCommandsSubmit);
            this.Controls.Add(this.AbsoluteMoveSubmit);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.numericUpDown2);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.PosButtonEL);
            this.Controls.Add(this.NegButtonEL);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.DemoButton);
            this.Controls.Add(this.ActualELLabel);
            this.Controls.Add(this.ActualAZLabel);
            this.Controls.Add(this.ActualPositionLabel);
            this.Controls.Add(this.ActualELTextBox);
            this.Controls.Add(this.ActualAZTextBox);
            this.Controls.Add(this.numericUpDown1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.RelativeMoveSubmit);
            this.Controls.Add(this.radioButton2);
            this.Controls.Add(this.radioButton1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.errorLabel);
            this.Controls.Add(this.Title);
            this.Controls.Add(this.PosButtonAZ);
            this.Controls.Add(this.NegButtonAZ);
            this.MinimumSize = new System.Drawing.Size(816, 487);
            this.Name = "ManualControlForm";
            this.Text = "ManualControlForm";
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.SpeedInput)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button NegButtonAZ;
        private System.Windows.Forms.Button PosButtonAZ;
        private System.Windows.Forms.Label Title;
        private System.Windows.Forms.Label errorLabel;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.RadioButton radioButton1;
        private System.Windows.Forms.RadioButton radioButton2;
        private System.Windows.Forms.Button RelativeMoveSubmit;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.NumericUpDown numericUpDown1;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Label ActualELLabel;
        private System.Windows.Forms.Label ActualAZLabel;
        private System.Windows.Forms.Label ActualPositionLabel;
        private System.Windows.Forms.TextBox ActualELTextBox;
        private System.Windows.Forms.TextBox ActualAZTextBox;
        private System.Windows.Forms.Button DemoButton;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button NegButtonEL;
        private System.Windows.Forms.Button PosButtonEL;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.NumericUpDown numericUpDown2;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Button AbsoluteMoveSubmit;
        private System.Windows.Forms.Button ClearCommandsSubmit;
        private System.Windows.Forms.ErrorProvider errorProvider1;
        private System.Windows.Forms.NumericUpDown SpeedInput;
    }
}