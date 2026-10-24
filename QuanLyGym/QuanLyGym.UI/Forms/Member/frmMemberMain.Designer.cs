namespace QuanLyGym.UI.Forms.Member
{
    partial class frmMemberMain
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            SuspendLayout();
            // 
            // frmMemberMain
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1340, 840);
            MinimumSize = new Size(1150, 720);
            Name = "frmMemberMain";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "TITAN FITNESS CLUB - HỆ THỐNG QUẢN LÝ DÀNH CHO HỘI VIÊN";
            ResumeLayout(false);
        }

        #endregion

        // Khai báo pnlHeader dùng chung cho partial class
        private System.Windows.Forms.Panel pnlHeader;
    }
}