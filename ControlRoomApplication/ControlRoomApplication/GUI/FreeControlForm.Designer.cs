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
            this.SuspendLayout();
            // 
            // PosDecButton
            // 
            this.PosDecButton.Location = new System.Drawing.Point(375, 245);
            this.PosDecButton.Name = "PosDecButton";
            this.PosDecButton.Size = new System.Drawing.Size(75, 54);
            this.PosDecButton.TabIndex = 0;
            this.PosDecButton.Text = "+ Dec";
            this.PosDecButton.UseVisualStyleBackColor = true;
            this.PosDecButton.Click += new System.EventHandler(this.PosDecButton_Click);
            // 
            // NegDecButton
            // 
            this.NegDecButton.Location = new System.Drawing.Point(375, 317);
            this.NegDecButton.Name = "NegDecButton";
            this.NegDecButton.Size = new System.Drawing.Size(75, 54);
            this.NegDecButton.TabIndex = 1;
            this.NegDecButton.Text = "- Dec";
            this.NegDecButton.UseVisualStyleBackColor = true;
            this.NegDecButton.Click += new System.EventHandler(this.NegDecButton_Click);
            // 
            // NegRAButton
            // 
            this.NegRAButton.Location = new System.Drawing.Point(281, 317);
            this.NegRAButton.Name = "NegRAButton";
            this.NegRAButton.Size = new System.Drawing.Size(75, 54);
            this.NegRAButton.TabIndex = 2;
            this.NegRAButton.Text = "- RA";
            this.NegRAButton.UseVisualStyleBackColor = true;
            this.NegRAButton.Click += new System.EventHandler(this.NegRAButton_Click);
            // 
            // PosRAButton
            // 
            this.PosRAButton.Location = new System.Drawing.Point(471, 317);
            this.PosRAButton.Name = "PosRAButton";
            this.PosRAButton.Size = new System.Drawing.Size(75, 54);
            this.PosRAButton.TabIndex = 3;
            this.PosRAButton.Text = "+ RA";
            this.PosRAButton.UseVisualStyleBackColor = true;
            this.PosRAButton.Click += new System.EventHandler(this.PosRAButton_Click);
            // 
            // CalibrateButton
            // 
            this.CalibrateButton.Location = new System.Drawing.Point(34, 326);
            this.CalibrateButton.Name = "CalibrateButton";
            this.CalibrateButton.Size = new System.Drawing.Size(123, 36);
            this.CalibrateButton.TabIndex = 4;
            this.CalibrateButton.Text = "Calibrate";
            this.CalibrateButton.UseVisualStyleBackColor = true;
            this.CalibrateButton.Click += new System.EventHandler(this.CalibrateButton_Click);
            // 
            // ActualRATextBox
            // 
            this.ActualRATextBox.Location = new System.Drawing.Point(75, 136);
            this.ActualRATextBox.Name = "ActualRATextBox";
            this.ActualRATextBox.ReadOnly = true;
            this.ActualRATextBox.Size = new System.Drawing.Size(100, 20);
            this.ActualRATextBox.TabIndex = 5;
            // 
            // ActualDecTextBox
            // 
            this.ActualDecTextBox.Location = new System.Drawing.Point(75, 172);
            this.ActualDecTextBox.Name = "ActualDecTextBox";
            this.ActualDecTextBox.ReadOnly = true;
            this.ActualDecTextBox.Size = new System.Drawing.Size(100, 20);
            this.ActualDecTextBox.TabIndex = 6;
            // 
            // ActualPositionLabel
            // 
            this.ActualPositionLabel.AutoSize = true;
            this.ActualPositionLabel.Location = new System.Drawing.Point(75, 117);
            this.ActualPositionLabel.Name = "ActualPositionLabel";
            this.ActualPositionLabel.Size = new System.Drawing.Size(77, 13);
            this.ActualPositionLabel.TabIndex = 7;
            this.ActualPositionLabel.Text = "Actual Position";
            // 
            // ActualRALabel
            // 
            this.ActualRALabel.AutoSize = true;
            this.ActualRALabel.Location = new System.Drawing.Point(46, 139);
            this.ActualRALabel.Name = "ActualRALabel";
            this.ActualRALabel.Size = new System.Drawing.Size(22, 13);
            this.ActualRALabel.TabIndex = 8;
            this.ActualRALabel.Text = "RA";
            // 
            // ActualDecLabel
            // 
            this.ActualDecLabel.AutoSize = true;
            this.ActualDecLabel.Location = new System.Drawing.Point(42, 172);
            this.ActualDecLabel.Name = "ActualDecLabel";
            this.ActualDecLabel.Size = new System.Drawing.Size(27, 13);
            this.ActualDecLabel.TabIndex = 9;
            this.ActualDecLabel.Text = "Dec";
            // 
            // TargetDecLabel
            // 
            this.TargetDecLabel.AutoSize = true;
            this.TargetDecLabel.Location = new System.Drawing.Point(626, 172);
            this.TargetDecLabel.Name = "TargetDecLabel";
            this.TargetDecLabel.Size = new System.Drawing.Size(27, 13);
            this.TargetDecLabel.TabIndex = 14;
            this.TargetDecLabel.Text = "Dec";
            // 
            // TargetRALabel
            // 
            this.TargetRALabel.AutoSize = true;
            this.TargetRALabel.Location = new System.Drawing.Point(631, 139);
            this.TargetRALabel.Name = "TargetRALabel";
            this.TargetRALabel.Size = new System.Drawing.Size(22, 13);
            this.TargetRALabel.TabIndex = 13;
            this.TargetRALabel.Text = "RA";
            // 
            // TargetPositionLabel
            // 
            this.TargetPositionLabel.AutoSize = true;
            this.TargetPositionLabel.Location = new System.Drawing.Point(659, 117);
            this.TargetPositionLabel.Name = "TargetPositionLabel";
            this.TargetPositionLabel.Size = new System.Drawing.Size(78, 13);
            this.TargetPositionLabel.TabIndex = 12;
            this.TargetPositionLabel.Text = "Target Position";
            // 
            // TargetDecTextBox
            // 
            this.TargetDecTextBox.Location = new System.Drawing.Point(659, 172);
            this.TargetDecTextBox.Name = "TargetDecTextBox";
            this.TargetDecTextBox.ReadOnly = true;
            this.TargetDecTextBox.Size = new System.Drawing.Size(100, 20);
            this.TargetDecTextBox.TabIndex = 11;
            // 
            // TargetRATextBox
            // 
            this.TargetRATextBox.Location = new System.Drawing.Point(659, 136);
            this.TargetRATextBox.Name = "TargetRATextBox";
            this.TargetRATextBox.ReadOnly = true;
            this.TargetRATextBox.Size = new System.Drawing.Size(100, 20);
            this.TargetRATextBox.TabIndex = 10;
            // 
            // Title
            // 
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
            // FreeControlForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
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
            this.Name = "FreeControlForm";
            this.Text = "FreeControlForm";
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
    }
}