namespace QuanLyGym.UI.Forms.Chat
{
    partial class frmChatbot
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
            ucChatbotControl = new ucChatbot();
            SuspendLayout();
            // 
            // ucChatbotControl
            // 
            ucChatbotControl.BackColor = System.Drawing.Color.FromArgb(248, 250, 252);
            ucChatbotControl.Dock = System.Windows.Forms.DockStyle.Fill;
            ucChatbotControl.Location = new System.Drawing.Point(0, 0);
            ucChatbotControl.Name = "ucChatbotControl";
            ucChatbotControl.Size = new System.Drawing.Size(700, 650);
            ucChatbotControl.TabIndex = 0;
            // 
            // frmChatbot
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(700, 650);
            Controls.Add(ucChatbotControl);
            MinimumSize = new System.Drawing.Size(520, 520);
            Name = "frmChatbot";
            StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            Text = "Gym AI Assistant - Trợ lý thông minh";
            ResumeLayout(false);
        }

        #endregion

        private ucChatbot ucChatbotControl;
    }
}
