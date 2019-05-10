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
            this.PosDecButton.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.PosDecButton.BackColor = System.Drawing.Color.LightGray;
            this.PosDecButton.Location = new System.Drawing.Point(375, 245);
            this.PosDecButton.Name = "PosDecButton";
            this.PosDecButton.Size = new System.Drawing.Size(75, 54);
            this.PosDecButton.TabIndex = 0;
            this.PosDecButton.Text = "+ Dec";
            this.PosDecButton.UseVisualStyleBackColor = false;
            this.PosDecButton.Click += new System.EventHandler(this.PosDecButton_Click);
            // 
            // NegDecButton
            // 
            this.NegDecButton.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.NegDecButton.BackColor = System.Drawing.Color.LightGray;
            this.NegDecButton.Location = new System.Drawing.Point(375, 317);
            this.NegDecButton.Name = "NegDecButton";
            this.NegDecButton.Size = new System.Drawing.Size(75, 54);
            this.NegDecButton.TabIndex = 1;
            this.NegDecButton.Text = "- Dec";
            this.NegDecButton.UseVisualStyleBackColor = false;
            this.NegDecButton.Click += new System.EventHandler(this.NegDecButton_Click);
            // 
            // NegRAButton
            // 
            this.NegRAButton.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.NegRAButton.BackColor = System.Drawing.Color.LightGray;
            this.NegRAButton.Location = new System.Drawing.Point(281, 317);
            this.NegRAButton.Name = "NegRAButton";
            this.NegRAButton.Size = new System.Drawing.Size(75, 54);
            this.NegRAButton.TabIndex = 2;
            this.NegRAButton.Text = "- RA";
            this.NegRAButton.UseVisualStyleBackColor = false;
            this.NegRAButton.Click += new System.EventHandler(this.NegRAButton_Click);
            // 
            // PosRAButton
            // 
            this.PosRAButton.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.PosRAButton.BackColor = System.Drawing.Color.LightGray;
            this.PosRAButton.Location = new System.Drawing.Point(471, 317);
            this.PosRAButton.Name = "PosRAButton";
            this.PosRAButton.Size = new System.Drawing.Size(75, 54);
            this.PosRAButton.TabIndex = 3;
            this.PosRAButton.Text = "+ RA";
            this.PosRAButton.UseVisualStyleBackColor = false;
            this.PosRAButton.Click += new System.EventHandler(this.PosRAButton_Click);
            // 
            // CalibrateButton
            // 
            this.CalibrateButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.CalibrateButton.BackColor = System.Drawing.Color.LightGray;
            this.CalibrateButton.Location = new System.Drawing.Point(44, 326);
            this.CalibrateButton.Name = "CalibrateButton";
            this.CalibrateButton.Size = new System.Drawing.Size(123, 36);
            this.CalibrateButton.TabIndex = 4;
            this.CalibrateButton.Text = "Calibrate";
            this.CalibrateButton.UseVisualStyleBackColor = false;
            this.CalibrateButton.Click += new System.EventHandler(this.CalibrateButton_Click);
            // 
            // ActualRATextBox
            // 
            this.ActualRATextBox.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.ActualRATextBox.Location = new System.Drawing.Point(57, 149);
            this.ActualRATextBox.Name = "ActualRATextBox";
            this.ActualRATextBox.ReadOnly = true;
            this.ActualRATextBox.Size = new System.Drawing.Size(100, 20);
            this.ActualRATextBox.TabIndex = 5;
            // 
            // ActualDecTextBox
            // 
            this.ActualDecTextBox.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.ActualDecTextBox.Location = new System.Drawing.Point(57, 202);
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
            this.ActualPositionLabel.Location = new System.Drawing.Point(53, 101);
            this.ActualPositionLabel.Name = "ActualPositionLabel";
            this.ActualPositionLabel.Size = new System.Drawing.Size(114, 20);
            this.ActualPositionLabel.TabIndex = 7;
            this.ActualPositionLabel.Text = "Actual Position";
            // 
            // ActualRALabel
            // 
            this.ActualRALabel.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.ActualRALabel.AutoSize = true;
            this.ActualRALabel.Location = new System.Drawing.Point(55, 131);
            this.ActualRALabel.Name = "ActualRALabel";
            this.ActualRALabel.Size = new System.Drawing.Size(84, 13);
            this.ActualRALabel.TabIndex = 8;
            this.ActualRALabel.Text = "Right Ascension";
            // 
            // ActualDecLabel
            // 
            this.ActualDecLabel.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.ActualDecLabel.AutoSize = true;
            this.ActualDecLabel.Location = new System.Drawing.Point(55, 184);
            this.ActualDecLabel.Name = "ActualDecLabel";
            this.ActualDecLabel.Size = new System.Drawing.Size(60, 13);
            this.ActualDecLabel.TabIndex = 9;
            this.ActualDecLabel.Text = "Declination";
            // 
            // TargetDecLabel
            // 
            this.TargetDecLabel.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.TargetDecLabel.AutoSize = true;
            this.TargetDecLabel.Location = new System.Drawing.Point(639, 184);
            this.TargetDecLabel.Name = "TargetDecLabel";
            this.TargetDecLabel.Size = new System.Drawing.Size(60, 13);
            this.TargetDecLabel.TabIndex = 14;
            this.TargetDecLabel.Text = "Declination";
            // 
            // TargetRALabel
            // 
            this.TargetRALabel.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.TargetRALabel.AutoSize = true;
            this.TargetRALabel.Location = new System.Drawing.Point(640, 131);
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
            this.TargetPositionLabel.Location = new System.Drawing.Point(638, 101);
            this.TargetPositionLabel.Name = "TargetPositionLabel";
            this.TargetPositionLabel.Size = new System.Drawing.Size(115, 20);
            this.TargetPositionLabel.TabIndex = 12;
            this.TargetPositionLabel.Text = "Target Position";
            // 
            // TargetDecTextBox
            // 
            this.TargetDecTextBox.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.TargetDecTextBox.Location = new System.Drawing.Point(641, 202);
            this.TargetDecTextBox.Name = "TargetDecTextBox";
            this.TargetDecTextBox.ReadOnly = true;
            this.TargetDecTextBox.Size = new System.Drawing.Size(100, 20);
            this.TargetDecTextBox.TabIndex = 11;
            // 
            // TargetRATextBox
            // 
            this.TargetRATextBox.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.TargetRATextBox.Location = new System.Drawing.Point(641, 149);
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
            this.Title.Location = new System.Drawing.Point(297, 28);
            this.Title.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.Title.Name = "Title";
            this.Title.Size = new System.Drawing.Size(242, 46);
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
            this.IncrementButtons.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.IncrementButtons.Controls.Add(this.tenButton);
            this.IncrementButtons.Controls.Add(this.fiveButton);
            this.IncrementButtons.Controls.Add(this.oneButton);
            this.IncrementButtons.Controls.Add(this.oneForthButton);
            this.IncrementButtons.Location = new System.Drawing.Point(293, 113);
            this.IncrementButtons.Margin = new System.Windows.Forms.Padding(2);
            this.IncrementButtons.Name = "IncrementButtons";
            this.IncrementButtons.Padding = new System.Windows.Forms.Padding(2);
            this.IncrementButtons.Size = new System.Drawing.Size(241, 77);
            this.IncrementButtons.TabIndex = 16;
            this.IncrementButtons.TabStop = false;
            this.IncrementButtons.Text = "Increment";
            // 
            // tenButton
            // 
            this.tenButton.BackColor = System.Drawing.Color.LightGray;
            this.tenButton.Location = new System.Drawing.Point(182, 18);
            this.tenButton.Margin = new System.Windows.Forms.Padding(2);
            this.tenButton.Name = "tenButton";
            this.tenButton.Size = new System.Drawing.Size(54, 54);
            this.tenButton.TabIndex = 3;
            this.tenButton.Text = "10";
            this.tenButton.UseVisualStyleBackColor = false;
            this.tenButton.Click += new System.EventHandler(this.tenButton_Click);
            // 
            // fiveButton
            // 
            this.fiveButton.BackColor = System.Drawing.Color.LightGray;
            this.fiveButton.Location = new System.Drawing.Point(124, 18);
            this.fiveButton.Margin = new System.Windows.Forms.Padding(2);
            this.fiveButton.Name = "fiveButton";
            this.fiveButton.Size = new System.Drawing.Size(54, 54);
            this.fiveButton.TabIndex = 2;
            this.fiveButton.Text = "5";
            this.fiveButton.UseVisualStyleBackColor = false;
            this.fiveButton.Click += new System.EventHandler(this.fiveButton_Click);
            // 
            // oneButton
            // 
            this.oneButton.BackColor = System.Drawing.Color.LightGray;
            this.oneButton.Location = new System.Drawing.Point(65, 18);
            this.oneButton.Margin = new System.Windows.Forms.Padding(2);
            this.oneButton.Name = "oneButton";
            this.oneButton.Size = new System.Drawing.Size(54, 54);
            this.oneButton.TabIndex = 1;
            this.oneButton.Text = "1";
            this.oneButton.UseVisualStyleBackColor = false;
            this.oneButton.Click += new System.EventHandler(this.oneButton_Click);
            // 
            // oneForthButton
            // 
            this.oneForthButton.BackColor = System.Drawing.Color.LightGray;
            this.oneForthButton.Location = new System.Drawing.Point(7, 18);
            this.oneForthButton.Margin = new System.Windows.Forms.Padding(2);
            this.oneForthButton.Name = "oneForthButton";
            this.oneForthButton.Size = new System.Drawing.Size(54, 54);
            this.oneForthButton.TabIndex = 0;
            this.oneForthButton.Text = "0.25";
            this.oneForthButton.UseVisualStyleBackColor = false;
            this.oneForthButton.Click += new System.EventHandler(this.oneForthButton_Click);
            // 
            // editButton
            // 
            this.editButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.editButton.BackColor = System.Drawing.Color.LightGray;
            this.editButton.Location = new System.Drawing.Point(630, 326);
            this.editButton.Name = "editButton";
            this.editButton.Size = new System.Drawing.Size(123, 36);
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
            this.errorLabel.Location = new System.Drawing.Point(300, 411);
            this.errorLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.errorLabel.Name = "errorLabel";
            this.errorLabel.Size = new System.Drawing.Size(230, 19);
            this.errorLabel.TabIndex = 18;
            this.errorLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // FreeControlForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
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
            this.MinimumSize = new System.Drawing.Size(816, 489);
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