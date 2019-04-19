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
            this.CalibrateButton = new System.Windows.Forms.Button();
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
            this.IncrementButtons.SuspendLayout();
            this.SuspendLayout();
            // 
            // PosDecButton
            // 
            this.PosDecButton.BackColor = System.Drawing.Color.LightGray;
            this.PosDecButton.Location = new System.Drawing.Point(500, 302);
            this.PosDecButton.Margin = new System.Windows.Forms.Padding(4);
            this.PosDecButton.Name = "PosDecButton";
            this.PosDecButton.Size = new System.Drawing.Size(100, 66);
            this.PosDecButton.TabIndex = 0;
            this.PosDecButton.Text = "+ Dec";
            this.PosDecButton.UseVisualStyleBackColor = false;
            this.PosDecButton.Click += new System.EventHandler(this.PosDecButton_Click);
            // 
            // NegDecButton
            // 
            this.NegDecButton.BackColor = System.Drawing.Color.LightGray;
            this.NegDecButton.Location = new System.Drawing.Point(500, 390);
            this.NegDecButton.Margin = new System.Windows.Forms.Padding(4);
            this.NegDecButton.Name = "NegDecButton";
            this.NegDecButton.Size = new System.Drawing.Size(100, 66);
            this.NegDecButton.TabIndex = 1;
            this.NegDecButton.Text = "- Dec";
            this.NegDecButton.UseVisualStyleBackColor = false;
            this.NegDecButton.Click += new System.EventHandler(this.NegDecButton_Click);
            // 
            // NegRAButton
            // 
            this.NegRAButton.BackColor = System.Drawing.Color.LightGray;
            this.NegRAButton.Location = new System.Drawing.Point(375, 390);
            this.NegRAButton.Margin = new System.Windows.Forms.Padding(4);
            this.NegRAButton.Name = "NegRAButton";
            this.NegRAButton.Size = new System.Drawing.Size(100, 66);
            this.NegRAButton.TabIndex = 2;
            this.NegRAButton.Text = "- RA";
            this.NegRAButton.UseVisualStyleBackColor = false;
            this.NegRAButton.Click += new System.EventHandler(this.NegRAButton_Click);
            // 
            // PosRAButton
            // 
            this.PosRAButton.BackColor = System.Drawing.Color.LightGray;
            this.PosRAButton.Location = new System.Drawing.Point(628, 390);
            this.PosRAButton.Margin = new System.Windows.Forms.Padding(4);
            this.PosRAButton.Name = "PosRAButton";
            this.PosRAButton.Size = new System.Drawing.Size(100, 66);
            this.PosRAButton.TabIndex = 3;
            this.PosRAButton.Text = "+ RA";
            this.PosRAButton.UseVisualStyleBackColor = false;
            this.PosRAButton.Click += new System.EventHandler(this.PosRAButton_Click);
            // 
            // CalibrateButton
            // 
            this.CalibrateButton.BackColor = System.Drawing.Color.LightGray;
            this.CalibrateButton.Location = new System.Drawing.Point(59, 401);
            this.CalibrateButton.Margin = new System.Windows.Forms.Padding(4);
            this.CalibrateButton.Name = "CalibrateButton";
            this.CalibrateButton.Size = new System.Drawing.Size(164, 44);
            this.CalibrateButton.TabIndex = 4;
            this.CalibrateButton.Text = "Calibrate";
            this.CalibrateButton.UseVisualStyleBackColor = false;
            this.CalibrateButton.Click += new System.EventHandler(this.CalibrateButton_Click);
            // 
            // ActualRATextBox
            // 
            this.ActualRATextBox.Location = new System.Drawing.Point(76, 183);
            this.ActualRATextBox.Margin = new System.Windows.Forms.Padding(4);
            this.ActualRATextBox.Name = "ActualRATextBox";
            this.ActualRATextBox.ReadOnly = true;
            this.ActualRATextBox.Size = new System.Drawing.Size(132, 22);
            this.ActualRATextBox.TabIndex = 5;
            // 
            // ActualDecTextBox
            // 
            this.ActualDecTextBox.Location = new System.Drawing.Point(76, 248);
            this.ActualDecTextBox.Margin = new System.Windows.Forms.Padding(4);
            this.ActualDecTextBox.Name = "ActualDecTextBox";
            this.ActualDecTextBox.ReadOnly = true;
            this.ActualDecTextBox.Size = new System.Drawing.Size(132, 22);
            this.ActualDecTextBox.TabIndex = 6;
            // 
            // ActualPositionLabel
            // 
            this.ActualPositionLabel.AutoSize = true;
            this.ActualPositionLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.ActualPositionLabel.Location = new System.Drawing.Point(71, 124);
            this.ActualPositionLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.ActualPositionLabel.Name = "ActualPositionLabel";
            this.ActualPositionLabel.Size = new System.Drawing.Size(141, 25);
            this.ActualPositionLabel.TabIndex = 7;
            this.ActualPositionLabel.Text = "Actual Position";
            // 
            // ActualRALabel
            // 
            this.ActualRALabel.AutoSize = true;
            this.ActualRALabel.Location = new System.Drawing.Point(73, 161);
            this.ActualRALabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.ActualRALabel.Name = "ActualRALabel";
            this.ActualRALabel.Size = new System.Drawing.Size(110, 17);
            this.ActualRALabel.TabIndex = 8;
            this.ActualRALabel.Text = "Right Ascension";
            // 
            // ActualDecLabel
            // 
            this.ActualDecLabel.AutoSize = true;
            this.ActualDecLabel.Location = new System.Drawing.Point(73, 227);
            this.ActualDecLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.ActualDecLabel.Name = "ActualDecLabel";
            this.ActualDecLabel.Size = new System.Drawing.Size(78, 17);
            this.ActualDecLabel.TabIndex = 9;
            this.ActualDecLabel.Text = "Declination";
            // 
            // TargetDecLabel
            // 
            this.TargetDecLabel.AutoSize = true;
            this.TargetDecLabel.Location = new System.Drawing.Point(852, 227);
            this.TargetDecLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.TargetDecLabel.Name = "TargetDecLabel";
            this.TargetDecLabel.Size = new System.Drawing.Size(78, 17);
            this.TargetDecLabel.TabIndex = 14;
            this.TargetDecLabel.Text = "Declination";
            // 
            // TargetRALabel
            // 
            this.TargetRALabel.AutoSize = true;
            this.TargetRALabel.Location = new System.Drawing.Point(853, 161);
            this.TargetRALabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.TargetRALabel.Name = "TargetRALabel";
            this.TargetRALabel.Size = new System.Drawing.Size(110, 17);
            this.TargetRALabel.TabIndex = 13;
            this.TargetRALabel.Text = "Right Ascension";
            // 
            // TargetPositionLabel
            // 
            this.TargetPositionLabel.AutoSize = true;
            this.TargetPositionLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.TargetPositionLabel.Location = new System.Drawing.Point(850, 124);
            this.TargetPositionLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.TargetPositionLabel.Name = "TargetPositionLabel";
            this.TargetPositionLabel.Size = new System.Drawing.Size(143, 25);
            this.TargetPositionLabel.TabIndex = 12;
            this.TargetPositionLabel.Text = "Target Position";
            // 
            // TargetDecTextBox
            // 
            this.TargetDecTextBox.Location = new System.Drawing.Point(855, 248);
            this.TargetDecTextBox.Margin = new System.Windows.Forms.Padding(4);
            this.TargetDecTextBox.Name = "TargetDecTextBox";
            this.TargetDecTextBox.ReadOnly = true;
            this.TargetDecTextBox.Size = new System.Drawing.Size(132, 22);
            this.TargetDecTextBox.TabIndex = 11;
            // 
            // TargetRATextBox
            // 
            this.TargetRATextBox.Location = new System.Drawing.Point(855, 183);
            this.TargetRATextBox.Margin = new System.Windows.Forms.Padding(4);
            this.TargetRATextBox.Name = "TargetRATextBox";
            this.TargetRATextBox.ReadOnly = true;
            this.TargetRATextBox.Size = new System.Drawing.Size(132, 22);
            this.TargetRATextBox.TabIndex = 10;
            // 
            // Title
            // 
            this.Title.AutoSize = true;
            this.Title.Font = new System.Drawing.Font("Microsoft Sans Serif", 30F);
            this.Title.Location = new System.Drawing.Point(396, 34);
            this.Title.Name = "Title";
            this.Title.Size = new System.Drawing.Size(304, 58);
            this.Title.TabIndex = 15;
            this.Title.Text = "Free Control";
            // 
            // timer1
            // 
            this.timer1.Enabled = true;
            this.timer1.Interval = 1000;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // IncrementButtons
            // 
            this.IncrementButtons.Controls.Add(this.tenButton);
            this.IncrementButtons.Controls.Add(this.fiveButton);
            this.IncrementButtons.Controls.Add(this.oneButton);
            this.IncrementButtons.Controls.Add(this.oneForthButton);
            this.IncrementButtons.Location = new System.Drawing.Point(391, 139);
            this.IncrementButtons.Name = "IncrementButtons";
            this.IncrementButtons.Size = new System.Drawing.Size(321, 95);
            this.IncrementButtons.TabIndex = 16;
            this.IncrementButtons.TabStop = false;
            this.IncrementButtons.Text = "Increment";
            // 
            // tenButton
            // 
            this.tenButton.BackColor = System.Drawing.Color.LightGray;
            this.tenButton.Location = new System.Drawing.Point(243, 22);
            this.tenButton.Name = "tenButton";
            this.tenButton.Size = new System.Drawing.Size(72, 67);
            this.tenButton.TabIndex = 3;
            this.tenButton.Text = "10";
            this.tenButton.UseVisualStyleBackColor = false;
            this.tenButton.Click += new System.EventHandler(this.tenButton_Click);
            // 
            // fiveButton
            // 
            this.fiveButton.BackColor = System.Drawing.Color.LightGray;
            this.fiveButton.Location = new System.Drawing.Point(165, 22);
            this.fiveButton.Name = "fiveButton";
            this.fiveButton.Size = new System.Drawing.Size(72, 67);
            this.fiveButton.TabIndex = 2;
            this.fiveButton.Text = "5";
            this.fiveButton.UseVisualStyleBackColor = false;
            this.fiveButton.Click += new System.EventHandler(this.fiveButton_Click);
            // 
            // oneButton
            // 
            this.oneButton.BackColor = System.Drawing.Color.LightGray;
            this.oneButton.Location = new System.Drawing.Point(87, 22);
            this.oneButton.Name = "oneButton";
            this.oneButton.Size = new System.Drawing.Size(72, 67);
            this.oneButton.TabIndex = 1;
            this.oneButton.Text = "1";
            this.oneButton.UseVisualStyleBackColor = false;
            this.oneButton.Click += new System.EventHandler(this.oneButton_Click);
            // 
            // oneForthButton
            // 
            this.oneForthButton.BackColor = System.Drawing.Color.LightGray;
            this.oneForthButton.Location = new System.Drawing.Point(9, 22);
            this.oneForthButton.Name = "oneForthButton";
            this.oneForthButton.Size = new System.Drawing.Size(72, 67);
            this.oneForthButton.TabIndex = 0;
            this.oneForthButton.Text = "0.25";
            this.oneForthButton.UseVisualStyleBackColor = false;
            this.oneForthButton.Click += new System.EventHandler(this.oneForthButton_Click);
            // 
            // editButton
            // 
            this.editButton.BackColor = System.Drawing.Color.LightGray;
            this.editButton.Location = new System.Drawing.Point(840, 401);
            this.editButton.Margin = new System.Windows.Forms.Padding(4);
            this.editButton.Name = "editButton";
            this.editButton.Size = new System.Drawing.Size(164, 44);
            this.editButton.TabIndex = 17;
            this.editButton.Text = "Edit Position";
            this.editButton.UseVisualStyleBackColor = false;
            this.editButton.Click += new System.EventHandler(this.editButton_Click);
            // 
            // errorLabel
            // 
            this.errorLabel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.errorLabel.ForeColor = System.Drawing.Color.Red;
            this.errorLabel.Location = new System.Drawing.Point(400, 506);
            this.errorLabel.Name = "errorLabel";
            this.errorLabel.Size = new System.Drawing.Size(306, 23);
            this.errorLabel.TabIndex = 18;
            this.errorLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // FreeControlForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1067, 554);
            this.Controls.Add(this.errorLabel);
            this.Controls.Add(this.editButton);
            this.Controls.Add(this.IncrementButtons);
            this.Controls.Add(this.Title);
            this.Controls.Add(this.TargetDecLabel);
            this.Controls.Add(this.TargetRALabel);
            this.Controls.Add(this.TargetPositionLabel);
            this.Controls.Add(this.TargetDecTextBox);
            this.Controls.Add(this.TargetRATextBox);
            this.Controls.Add(this.ActualDecLabel);
            this.Controls.Add(this.ActualRALabel);
            this.Controls.Add(this.ActualPositionLabel);
            this.Controls.Add(this.ActualDecTextBox);
            this.Controls.Add(this.ActualRATextBox);
            this.Controls.Add(this.CalibrateButton);
            this.Controls.Add(this.PosRAButton);
            this.Controls.Add(this.NegRAButton);
            this.Controls.Add(this.NegDecButton);
            this.Controls.Add(this.PosDecButton);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "FreeControlForm";
            this.Text = "FreeControlForm";
            this.IncrementButtons.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button PosDecButton;
        private System.Windows.Forms.Button NegDecButton;
        private System.Windows.Forms.Button NegRAButton;
        private System.Windows.Forms.Button PosRAButton;
        private System.Windows.Forms.Button CalibrateButton;
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
    }
}