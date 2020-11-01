namespace MiddleManAESKeyGenerator
{
    partial class frmKeygen
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
            this.btnDocumentation = new System.Windows.Forms.Button();
            this.btnGenKey = new System.Windows.Forms.Button();
            this.lblLastGenerated = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // btnDocumentation
            // 
            this.btnDocumentation.Location = new System.Drawing.Point(147, 12);
            this.btnDocumentation.Name = "btnDocumentation";
            this.btnDocumentation.Size = new System.Drawing.Size(23, 24);
            this.btnDocumentation.TabIndex = 1;
            this.btnDocumentation.Text = "?";
            this.btnDocumentation.UseVisualStyleBackColor = true;
            this.btnDocumentation.Click += new System.EventHandler(this.btnDocumentation_Click);
            // 
            // btnGenKey
            // 
            this.btnGenKey.Location = new System.Drawing.Point(12, 72);
            this.btnGenKey.Name = "btnGenKey";
            this.btnGenKey.Size = new System.Drawing.Size(158, 23);
            this.btnGenKey.TabIndex = 0;
            this.btnGenKey.Text = "Generate New Keys";
            this.btnGenKey.UseVisualStyleBackColor = true;
            this.btnGenKey.Click += new System.EventHandler(this.btnGenKey_Click);
            // 
            // lblLastGenerated
            // 
            this.lblLastGenerated.AutoSize = true;
            this.lblLastGenerated.Location = new System.Drawing.Point(12, 23);
            this.lblLastGenerated.Name = "lblLastGenerated";
            this.lblLastGenerated.Size = new System.Drawing.Size(109, 13);
            this.lblLastGenerated.TabIndex = 2;
            this.lblLastGenerated.Text = "Keys Last Generated:";
            // 
            // frmKeygen
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(182, 107);
            this.Controls.Add(this.lblLastGenerated);
            this.Controls.Add(this.btnGenKey);
            this.Controls.Add(this.btnDocumentation);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmKeygen";
            this.Text = "AES Key Generator";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnDocumentation;
        private System.Windows.Forms.Button btnGenKey;
        private System.Windows.Forms.Label lblLastGenerated;
    }
}

