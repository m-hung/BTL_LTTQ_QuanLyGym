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
            this.components = new System.ComponentModel.Container();
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1340, 840);
            this.MinimumSize = new System.Drawing.Size(1150, 720);
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "TITAN FITNESS CLUB - HỆ THỐNG QUẢN LÝ DÀNH CHO HỘI VIÊN";
        }

        #endregion

        private Panel pnlHeader;
        private Label lblAppName;
        private Label lblXinChao;
        private Button btnDangXuat;
        private Panel pnlContent;
        private Panel pnlCard;
        private Panel pnlCardTop;
        private Label lblIcon;
        private Label lblVaiTro;
        private Label lblTenDangNhap;
        private Label lblGhiChu;
    }
}
