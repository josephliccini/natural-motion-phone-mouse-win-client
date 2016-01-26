namespace NaturalMotionPhoneMouseWindowsClient
{
    partial class Form1
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
            this.ConnectButton = new System.Windows.Forms.Button();
            this.KillButton = new System.Windows.Forms.Button();
            this.ProgressBar = new System.Windows.Forms.ProgressBar();
            this.SuspendLayout();
            // 
            // ConnectButton
            // 
            this.ConnectButton.Location = new System.Drawing.Point(12, 12);
            this.ConnectButton.Name = "ConnectButton";
            this.ConnectButton.Size = new System.Drawing.Size(276, 46);
            this.ConnectButton.TabIndex = 0;
            this.ConnectButton.Text = "Connect to Phone";
            this.ConnectButton.UseVisualStyleBackColor = true;
            this.ConnectButton.Click += new System.EventHandler(this.ConnectButton_Click);
            // 
            // KillButton
            // 
            this.KillButton.Enabled = false;
            this.KillButton.Location = new System.Drawing.Point(294, 12);
            this.KillButton.Name = "KillButton";
            this.KillButton.Size = new System.Drawing.Size(276, 46);
            this.KillButton.TabIndex = 1;
            this.KillButton.Text = "Terminate Listening";
            this.KillButton.UseVisualStyleBackColor = true;
            this.KillButton.Click += new System.EventHandler(this.KillButton_Click);
            // 
            // ProgressBar
            // 
            this.ProgressBar.Location = new System.Drawing.Point(12, 64);
            this.ProgressBar.Name = "ProgressBar";
            this.ProgressBar.Size = new System.Drawing.Size(553, 37);
            this.ProgressBar.TabIndex = 2;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(577, 120);
            this.Controls.Add(this.ProgressBar);
            this.Controls.Add(this.KillButton);
            this.Controls.Add(this.ConnectButton);
            this.Name = "Form1";
            this.Text = "Natural Motion Phone Mouse Client";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button ConnectButton;
        private System.Windows.Forms.Button KillButton;
        private System.Windows.Forms.ProgressBar ProgressBar;
    }
}

