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
            this.SuspendLayout();
            // 
            // PosDecButton
            // 
            this.PosDecButton.Location = new System.Drawing.Point(500, 302);
            this.PosDecButton.Margin = new System.Windows.Forms.Padding(4);
            this.PosDecButton.Name = "PosDecButton";
            this.PosDecButton.Size = new System.Drawing.Size(100, 66);
            this.PosDecButton.TabIndex = 0;
            this.PosDecButton.Text = "+ Dec";
            this.PosDecButton.UseVisualStyleBackColor = true;
            this.PosDecButton.Click += new System.EventHandler(this.PosDecButton_Click);
            // 
            // NegDecButton
            // 
            this.NegDecButton.Location = new System.Drawing.Point(500, 390);
            this.NegDecButton.Margin = new System.Windows.Forms.Padding(4);
            this.NegDecButton.Name = "NegDecButton";
            this.NegDecButton.Size = new System.Drawing.Size(100, 66);
            this.NegDecButton.TabIndex = 1;
            this.NegDecButton.Text = "- Dec";
            this.NegDecButton.UseVisualStyleBackColor = true;
            this.NegDecButton.Click += new System.EventHandler(this.NegDecButton_Click);
            // 
            // NegRAButton
            // 
            this.NegRAButton.Location = new System.Drawing.Point(375, 390);
            this.NegRAButton.Margin = new System.Windows.Forms.Padding(4);
            this.NegRAButton.Name = "NegRAButton";
            this.NegRAButton.Size = new System.Drawing.Size(100, 66);
            this.NegRAButton.TabIndex = 2;
            this.NegRAButton.Text = "- RA";
            this.NegRAButton.UseVisualStyleBackColor = true;
            this.NegRAButton.Click += new System.EventHandler(this.NegRAButton_Click);
            // 
            // PosRAButton
            // 
            this.PosRAButton.Location = new System.Drawing.Point(628, 390);
            this.PosRAButton.Margin = new System.Windows.Forms.Padding(4);
            this.PosRAButton.Name = "PosRAButton";
            this.PosRAButton.Size = new System.Drawing.Size(100, 66);
            this.PosRAButton.TabIndex = 3;
            this.PosRAButton.Text = "+ RA";
            this.PosRAButton.UseVisualStyleBackColor = true;
            this.PosRAButton.Click += new System.EventHandler(this.PosRAButton_Click);
            // 
            // CalibrateButton
            // 
            this.CalibrateButton.Location = new System.Drawing.Point(45, 401);
            this.CalibrateButton.Margin = new System.Windows.Forms.Padding(4);
            this.CalibrateButton.Name = "CalibrateButton";
            this.CalibrateButton.Size = new System.Drawing.Size(164, 44);
            this.CalibrateButton.TabIndex = 4;
            this.CalibrateButton.Text = "Calibrate";
            this.CalibrateButton.UseVisualStyleBackColor = true;
            this.CalibrateButton.Click += new System.EventHandler(this.CalibrateButton_Click);
            // 
            // ActualRATextBox
            // 
            this.ActualRATextBox.Location = new System.Drawing.Point(100, 168);
            this.ActualRATextBox.Margin = new System.Windows.Forms.Padding(4);
            this.ActualRATextBox.Name = "ActualRATextBox";
            this.ActualRATextBox.ReadOnly = true;
            this.ActualRATextBox.Size = new System.Drawing.Size(132, 22);
            this.ActualRATextBox.TabIndex = 5;
            // 
            // ActualDecTextBox
            // 
            this.ActualDecTextBox.Location = new System.Drawing.Point(100, 212);
            this.ActualDecTextBox.Margin = new System.Windows.Forms.Padding(4);
            this.ActualDecTextBox.Name = "ActualDecTextBox";
            this.ActualDecTextBox.ReadOnly = true;
            this.ActualDecTextBox.Size = new System.Drawing.Size(132, 22);
            this.ActualDecTextBox.TabIndex = 6;
            // 
            // ActualPositionLabel
            // 
            this.ActualPositionLabel.AutoSize = true;
            this.ActualPositionLabel.Location = new System.Drawing.Point(100, 144);
            this.ActualPositionLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.ActualPositionLabel.Name = "ActualPositionLabel";
            this.ActualPositionLabel.Size = new System.Drawing.Size(101, 17);
            this.ActualPositionLabel.TabIndex = 7;
            this.ActualPositionLabel.Text = "Actual Position";
            // 
            // ActualRALabel
            // 
            this.ActualRALabel.AutoSize = true;
            this.ActualRALabel.Location = new System.Drawing.Point(62, 171);
            this.ActualRALabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.ActualRALabel.Name = "ActualRALabel";
            this.ActualRALabel.Size = new System.Drawing.Size(27, 17);
            this.ActualRALabel.TabIndex = 8;
            this.ActualRALabel.Text = "RA";
            // 
            // ActualDecLabel
            // 
            this.ActualDecLabel.AutoSize = true;
            this.ActualDecLabel.Location = new System.Drawing.Point(56, 212);
            this.ActualDecLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.ActualDecLabel.Name = "ActualDecLabel";
            this.ActualDecLabel.Size = new System.Drawing.Size(33, 17);
            this.ActualDecLabel.TabIndex = 9;
            this.ActualDecLabel.Text = "Dec";
            // 
            // TargetDecLabel
            // 
            this.TargetDecLabel.AutoSize = true;
            this.TargetDecLabel.Location = new System.Drawing.Point(835, 212);
            this.TargetDecLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.TargetDecLabel.Name = "TargetDecLabel";
            this.TargetDecLabel.Size = new System.Drawing.Size(33, 17);
            this.TargetDecLabel.TabIndex = 14;
            this.TargetDecLabel.Text = "Dec";
            // 
            // TargetRALabel
            // 
            this.TargetRALabel.AutoSize = true;
            this.TargetRALabel.Location = new System.Drawing.Point(841, 171);
            this.TargetRALabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.TargetRALabel.Name = "TargetRALabel";
            this.TargetRALabel.Size = new System.Drawing.Size(27, 17);
            this.TargetRALabel.TabIndex = 13;
            this.TargetRALabel.Text = "RA";
            // 
            // TargetPositionLabel
            // 
            this.TargetPositionLabel.AutoSize = true;
            this.TargetPositionLabel.Location = new System.Drawing.Point(879, 144);
            this.TargetPositionLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.TargetPositionLabel.Name = "TargetPositionLabel";
            this.TargetPositionLabel.Size = new System.Drawing.Size(104, 17);
            this.TargetPositionLabel.TabIndex = 12;
            this.TargetPositionLabel.Text = "Target Position";
            // 
            // TargetDecTextBox
            // 
            this.TargetDecTextBox.Location = new System.Drawing.Point(879, 212);
            this.TargetDecTextBox.Margin = new System.Windows.Forms.Padding(4);
            this.TargetDecTextBox.Name = "TargetDecTextBox";
            this.TargetDecTextBox.ReadOnly = true;
            this.TargetDecTextBox.Size = new System.Drawing.Size(132, 22);
            this.TargetDecTextBox.TabIndex = 11;
            // 
            // TargetRATextBox
            // 
            this.TargetRATextBox.Location = new System.Drawing.Point(879, 168);
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
            this.Title.Location = new System.Drawing.Point(396, 35);
            this.Title.Name = "Title";
            this.Title.Size = new System.Drawing.Size(304, 58);
            this.Title.TabIndex = 15;
            this.Title.Text = "Free Control";
            // 
            // FreeControlForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1067, 554);
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
    }
}