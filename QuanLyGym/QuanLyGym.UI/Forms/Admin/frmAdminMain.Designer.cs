namespace QuanLyGym.UI.Forms.Admin
{
    partial class frmAdminMain
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
            this.pnlContent = new System.Windows.Forms.Panel();
            this.navBar = new QuanLyGym.UI.Forms.Admin.Controls.AdminNavBarControl();
            this.topBar = new QuanLyGym.UI.Forms.Admin.Controls.AdminTopBarControl();
            this.SuspendLayout();
            // 
            // pnlContent
            // 
            this.pnlContent.BackColor = System.Drawing.Color.FromArgb(14, 15, 18);
            this.pnlContent.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlContent.Location = new System.Drawing.Point(0, 100);
            this.pnlContent.Name = "pnlContent";
            this.pnlContent.Size = new System.Drawing.Size(1264, 681);
            this.pnlContent.TabIndex = 2;
            // 
            // navBar
            // 
            this.navBar.BackColor = System.Drawing.Color.FromArgb(18, 19, 24);
            this.navBar.Dock = System.Windows.Forms.DockStyle.Top;
            this.navBar.Height = 44;
            this.navBar.Location = new System.Drawing.Point(0, 56);
            this.navBar.Name = "navBar";
            this.navBar.SelectedIndex = 0;
            this.navBar.Size = new System.Drawing.Size(1264, 44);
            this.navBar.TabIndex = 1;
            // 
            // topBar
            // 
            this.topBar.BackColor = System.Drawing.Color.FromArgb(18, 19, 24);
            this.topBar.Dock = System.Windows.Forms.DockStyle.Top;
            this.topBar.Height = 56;
            this.topBar.Location = new System.Drawing.Point(0, 0);
            this.topBar.Name = "topBar";
            this.topBar.Size = new System.Drawing.Size(1264, 56);
            this.topBar.TabIndex = 0;
            // 
            // frmAdminMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(14, 15, 18);
            this.ClientSize = new System.Drawing.Size(1264, 720);
            this.Controls.Add(this.pnlContent);
            this.Controls.Add(this.navBar);
            this.Controls.Add(this.topBar);
            this.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.MinimumSize = new System.Drawing.Size(1080, 620);
            this.Name = "frmAdminMain";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "GYMFIT - Quản Trị Hệ Thống";
            this.ResumeLayout(false);

        }

        #endregion

        private Controls.AdminTopBarControl topBar;
        private Controls.AdminNavBarControl navBar;
        private System.Windows.Forms.Panel pnlContent;
    }
}