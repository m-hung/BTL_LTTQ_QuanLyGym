namespace QuanLyGym.UI.Forms.Admin.Pages
{
    partial class ucAdminAccounts
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

        #region Component Designer generated code

        private void InitializeComponent()
        {
            this.mainLayout = new System.Windows.Forms.TableLayoutPanel();
            this.pnlHeader = new System.Windows.Forms.Panel();
            this.lblTitle = new System.Windows.Forms.Label();
            this.btnCreateAccount = new QuanLyGym.UI.Forms.Admin.Controls.ModernButton();
            this.tblKpi = new System.Windows.Forms.TableLayoutPanel();
            this.kpiMembers = new QuanLyGym.UI.Forms.Admin.Controls.KpiCardControl();
            this.kpiPT = new QuanLyGym.UI.Forms.Admin.Controls.KpiCardControl();
            this.kpiAdmin = new QuanLyGym.UI.Forms.Admin.Controls.KpiCardControl();
            this.subTabs = new QuanLyGym.UI.Forms.Admin.Controls.SubTabBarControl();
            this.searchBox = new QuanLyGym.UI.Forms.Admin.Controls.AdminSearchBoxControl();
            this.tableAccounts = new QuanLyGym.UI.Forms.Admin.Controls.AdminDataTableControl();
            this.mainLayout.SuspendLayout();
            this.pnlHeader.SuspendLayout();
            this.tblKpi.SuspendLayout();
            this.SuspendLayout();
            // 
            // mainLayout
            // 
            this.mainLayout.ColumnCount = 1;
            this.mainLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.mainLayout.Controls.Add(this.pnlHeader, 0, 0);
            this.mainLayout.Controls.Add(this.tblKpi, 0, 1);
            this.mainLayout.Controls.Add(this.subTabs, 0, 2);
            this.mainLayout.Controls.Add(this.searchBox, 0, 3);
            this.mainLayout.Controls.Add(this.tableAccounts, 0, 4);
            this.mainLayout.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mainLayout.Location = new System.Drawing.Point(0, 0);
            this.mainLayout.Name = "mainLayout";
            this.mainLayout.Padding = new System.Windows.Forms.Padding(20, 14, 20, 14);
            this.mainLayout.RowCount = 5;
            this.mainLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 42F));
            this.mainLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 100F));
            this.mainLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.mainLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 42F));
            this.mainLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.mainLayout.Size = new System.Drawing.Size(1264, 700);
            this.mainLayout.TabIndex = 0;
            // 
            // pnlHeader
            // 
            this.pnlHeader.Controls.Add(this.btnCreateAccount);
            this.pnlHeader.Controls.Add(this.lblTitle);
            this.pnlHeader.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlHeader.Location = new System.Drawing.Point(20, 14);
            this.pnlHeader.Margin = new System.Windows.Forms.Padding(0);
            this.pnlHeader.Name = "pnlHeader";
            this.pnlHeader.Size = new System.Drawing.Size(1224, 42);
            this.pnlHeader.TabIndex = 0;
            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.Dock = System.Windows.Forms.DockStyle.Left;
            this.lblTitle.Font = new System.Drawing.Font("Segoe UI", 16F, System.Drawing.FontStyle.Bold);
            this.lblTitle.ForeColor = System.Drawing.Color.White;
            this.lblTitle.Location = new System.Drawing.Point(0, 0);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(252, 30);
            this.lblTitle.TabIndex = 0;
            this.lblTitle.Text = "QUẢN LÝ TÀI KHOẢN";
            // 
            // btnCreateAccount
            // 
            this.btnCreateAccount.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCreateAccount.BackColor = System.Drawing.Color.Transparent;
            this.btnCreateAccount.BorderRadius = 5;
            this.btnCreateAccount.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnCreateAccount.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.btnCreateAccount.ForeColor = System.Drawing.Color.White;
            this.btnCreateAccount.HoverColor = System.Drawing.Color.FromArgb(255, 133, 38);
            this.btnCreateAccount.Location = new System.Drawing.Point(1074, 4);
            this.btnCreateAccount.Name = "btnCreateAccount";
            this.btnCreateAccount.NormalColor = System.Drawing.Color.FromArgb(255, 107, 0);
            this.btnCreateAccount.PressedColor = System.Drawing.Color.FromArgb(224, 94, 0);
            this.btnCreateAccount.Size = new System.Drawing.Size(150, 32);
            this.btnCreateAccount.TabIndex = 1;
            this.btnCreateAccount.Text = "+ Tạo tài khoản";
            this.btnCreateAccount.UseVisualStyleBackColor = false;
            // 
            // tblKpi
            // 
            this.tblKpi.ColumnCount = 3;
            this.tblKpi.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tblKpi.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tblKpi.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tblKpi.Controls.Add(this.kpiMembers, 0, 0);
            this.tblKpi.Controls.Add(this.kpiPT, 1, 0);
            this.tblKpi.Controls.Add(this.kpiAdmin, 2, 0);
            this.tblKpi.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tblKpi.Location = new System.Drawing.Point(20, 56);
            this.tblKpi.Margin = new System.Windows.Forms.Padding(0, 0, 0, 10);
            this.tblKpi.Name = "tblKpi";
            this.tblKpi.RowCount = 1;
            this.tblKpi.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tblKpi.Size = new System.Drawing.Size(1224, 90);
            this.tblKpi.TabIndex = 1;
            // 
            // kpiMembers
            // 
            this.kpiMembers.CardTitle = "TÀI KHOẢN HỘI VIÊN";
            this.kpiMembers.Dock = System.Windows.Forms.DockStyle.Fill;
            this.kpiMembers.KpiSubtitle = "";
            this.kpiMembers.KpiValue = "12";
            this.kpiMembers.Location = new System.Drawing.Point(0, 0);
            this.kpiMembers.Margin = new System.Windows.Forms.Padding(0, 0, 8, 0);
            this.kpiMembers.Name = "kpiMembers";
            this.kpiMembers.ValueColor = System.Drawing.Color.FromArgb(47, 128, 237);
            // 
            // kpiPT
            // 
            this.kpiPT.CardTitle = "TÀI KHOẢN PT";
            this.kpiPT.Dock = System.Windows.Forms.DockStyle.Fill;
            this.kpiPT.KpiSubtitle = "";
            this.kpiPT.KpiValue = "5";
            this.kpiPT.Location = new System.Drawing.Point(412, 0);
            this.kpiPT.Margin = new System.Windows.Forms.Padding(4, 0, 8, 0);
            this.kpiPT.Name = "kpiPT";
            this.kpiPT.ValueColor = System.Drawing.Color.FromArgb(255, 107, 0);
            // 
            // kpiAdmin
            // 
            this.kpiAdmin.CardTitle = "TÀI KHOẢN ADMIN";
            this.kpiAdmin.Dock = System.Windows.Forms.DockStyle.Fill;
            this.kpiAdmin.KpiSubtitle = "";
            this.kpiAdmin.KpiValue = "4";
            this.kpiAdmin.Location = new System.Drawing.Point(824, 0);
            this.kpiAdmin.Margin = new System.Windows.Forms.Padding(4, 0, 0, 0);
            this.kpiAdmin.Name = "kpiAdmin";
            this.kpiAdmin.ValueColor = System.Drawing.Color.FromArgb(168, 85, 247);
            // 
            // subTabs
            // 
            this.subTabs.BackColor = System.Drawing.Color.FromArgb(14, 15, 18);
            this.subTabs.Dock = System.Windows.Forms.DockStyle.Fill;
            this.subTabs.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold);
            this.subTabs.Location = new System.Drawing.Point(20, 156);
            this.subTabs.Margin = new System.Windows.Forms.Padding(0);
            this.subTabs.Name = "subTabs";
            this.subTabs.SelectedIndex = 0;
            this.subTabs.Size = new System.Drawing.Size(1224, 40);
            this.subTabs.TabIndex = 2;
            this.subTabs.Tabs = new string[] {
        "HỘI VIÊN",
        "PT",
        "ADMIN"};
            // 
            // searchBox
            // 
            this.searchBox.BackColor = System.Drawing.Color.FromArgb(14, 15, 18);
            this.searchBox.CounterText = "Tổng: 12 tài khoản";
            this.searchBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.searchBox.Location = new System.Drawing.Point(20, 196);
            this.searchBox.Margin = new System.Windows.Forms.Padding(0, 0, 0, 8);
            this.searchBox.Name = "searchBox";
            this.searchBox.Placeholder = "Tìm tài khoản...";
            this.searchBox.SearchText = "";
            this.searchBox.Size = new System.Drawing.Size(1224, 34);
            this.searchBox.TabIndex = 3;
            // 
            // tableAccounts
            // 
            this.tableAccounts.BackColor = System.Drawing.Color.FromArgb(22, 23, 29);
            this.tableAccounts.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableAccounts.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.tableAccounts.Location = new System.Drawing.Point(20, 238);
            this.tableAccounts.Margin = new System.Windows.Forms.Padding(0);
            this.tableAccounts.Name = "tableAccounts";
            this.tableAccounts.Size = new System.Drawing.Size(1224, 448);
            this.tableAccounts.TabIndex = 4;
            // 
            // ucAdminAccounts
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(14, 15, 18);
            this.Controls.Add(this.mainLayout);
            this.Name = "ucAdminAccounts";
            this.Size = new System.Drawing.Size(1264, 700);
            this.mainLayout.ResumeLayout(false);
            this.pnlHeader.ResumeLayout(false);
            this.pnlHeader.PerformLayout();
            this.tblKpi.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel mainLayout;
        private System.Windows.Forms.Panel pnlHeader;
        private System.Windows.Forms.Label lblTitle;
        private QuanLyGym.UI.Forms.Admin.Controls.ModernButton btnCreateAccount;
        private System.Windows.Forms.TableLayoutPanel tblKpi;
        private QuanLyGym.UI.Forms.Admin.Controls.KpiCardControl kpiMembers;
        private QuanLyGym.UI.Forms.Admin.Controls.KpiCardControl kpiPT;
        private QuanLyGym.UI.Forms.Admin.Controls.KpiCardControl kpiAdmin;
        private QuanLyGym.UI.Forms.Admin.Controls.SubTabBarControl subTabs;
        private QuanLyGym.UI.Forms.Admin.Controls.AdminSearchBoxControl searchBox;
        private QuanLyGym.UI.Forms.Admin.Controls.AdminDataTableControl tableAccounts;
    }
}
