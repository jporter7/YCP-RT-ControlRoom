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
            this.NegButton = new System.Windows.Forms.Button();
            this.PosButton = new System.Windows.Forms.Button();
            this.Title = new System.Windows.Forms.Label();
            this.errorLabel = new System.Windows.Forms.Label();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // NegButton
            // 
            this.NegButton.BackColor = System.Drawing.Color.LightGray;
            this.NegButton.Location = new System.Drawing.Point(300, 201);
            this.NegButton.Name = "NegButton";
            this.NegButton.Size = new System.Drawing.Size(75, 54);
            this.NegButton.TabIndex = 2;
            this.NegButton.Text = "- Jog";
            this.NegButton.UseVisualStyleBackColor = false;
            this.NegButton.MouseDown += new System.Windows.Forms.MouseEventHandler(this.NegButton_MouseDown);
            this.NegButton.MouseUp += new System.Windows.Forms.MouseEventHandler(this.NegButton_MouseUp);
            // 
            // PosButton
            // 
            this.PosButton.BackColor = System.Drawing.Color.LightGray;
            this.PosButton.Location = new System.Drawing.Point(455, 201);
            this.PosButton.Name = "PosButton";
            this.PosButton.Size = new System.Drawing.Size(75, 54);
            this.PosButton.TabIndex = 3;
            this.PosButton.Text = "+ Jog";
            this.PosButton.UseVisualStyleBackColor = false;
            this.PosButton.MouseDown += new System.Windows.Forms.MouseEventHandler(this.PosButton_MouseDown);
            this.PosButton.MouseUp += new System.Windows.Forms.MouseEventHandler(this.PosButton_MouseUp);
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
            // errorLabel
            // 
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
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Items.AddRange(new object[] {
            "0.1 RPM",
            "2 RPM"});
            this.comboBox1.Location = new System.Drawing.Point(339, 114);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(121, 21);
            this.comboBox1.TabIndex = 19;
            this.comboBox1.SelectedIndexChanged += new System.EventHandler(this.comboBox1_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(339, 95);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(38, 13);
            this.label1.TabIndex = 20;
            this.label1.Text = "Speed";
            // 
            // FreeControlForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.comboBox1);
            this.Controls.Add(this.errorLabel);
            this.Controls.Add(this.Title);
            this.Controls.Add(this.PosButton);
            this.Controls.Add(this.NegButton);
            this.Name = "FreeControlForm";
            this.Text = "FreeControlForm";
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
    }
}