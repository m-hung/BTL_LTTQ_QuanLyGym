namespace QuanLyGym.UI.Forms.Admin.Pages
{
    partial class ucAdminMembers
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
            this.btnAddMember = new QuanLyGym.UI.Forms.Admin.Controls.ModernButton();
            this.tblKpi = new System.Windows.Forms.TableLayoutPanel();
            this.kpiTotal = new QuanLyGym.UI.Forms.Admin.Controls.KpiCardControl();
            this.kpiActive = new QuanLyGym.UI.Forms.Admin.Controls.KpiCardControl();
            this.kpiNew = new QuanLyGym.UI.Forms.Admin.Controls.KpiCardControl();
            this.kpiExpiring = new QuanLyGym.UI.Forms.Admin.Controls.KpiCardControl();
            this.subTabs = new QuanLyGym.UI.Forms.Admin.Controls.SubTabBarControl();
            this.searchBox = new QuanLyGym.UI.Forms.Admin.Controls.AdminSearchBoxControl();
            this.tableMembers = new QuanLyGym.UI.Forms.Admin.Controls.AdminDataTableControl();
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
            this.mainLayout.Controls.Add(this.tableMembers, 0, 4);
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
            this.pnlHeader.Controls.Add(this.btnAddMember);
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
            this.lblTitle.Size = new System.Drawing.Size(217, 30);
            this.lblTitle.TabIndex = 0;
            this.lblTitle.Text = "QUẢN LÝ HỘI VIÊN";
            // 
            // btnAddMember
            // 
            this.btnAddMember.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnAddMember.BackColor = System.Drawing.Color.Transparent;
            this.btnAddMember.BorderRadius = 5;
            this.btnAddMember.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnAddMember.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.btnAddMember.ForeColor = System.Drawing.Color.White;
            this.btnAddMember.HoverColor = System.Drawing.Color.FromArgb(255, 133, 38);
            this.btnAddMember.Location = new System.Drawing.Point(1074, 4);
            this.btnAddMember.Name = "btnAddMember";
            this.btnAddMember.NormalColor = System.Drawing.Color.FromArgb(255, 107, 0);
            this.btnAddMember.PressedColor = System.Drawing.Color.FromArgb(224, 94, 0);
            this.btnAddMember.Size = new System.Drawing.Size(150, 32);
            this.btnAddMember.TabIndex = 1;
            this.btnAddMember.Text = "+ Thêm hội viên";
            this.btnAddMember.UseVisualStyleBackColor = false;
            // 
            // tblKpi
            // 
            this.tblKpi.ColumnCount = 4;
            this.tblKpi.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tblKpi.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tblKpi.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tblKpi.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tblKpi.Controls.Add(this.kpiTotal, 0, 0);
            this.tblKpi.Controls.Add(this.kpiActive, 1, 0);
            this.tblKpi.Controls.Add(this.kpiNew, 2, 0);
            this.tblKpi.Controls.Add(this.kpiExpiring, 3, 0);
            this.tblKpi.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tblKpi.Location = new System.Drawing.Point(20, 56);
            this.tblKpi.Margin = new System.Windows.Forms.Padding(0, 0, 0, 10);
            this.tblKpi.Name = "tblKpi";
            this.tblKpi.RowCount = 1;
            this.tblKpi.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tblKpi.Size = new System.Drawing.Size(1224, 90);
            this.tblKpi.TabIndex = 1;
            // 
            // kpiTotal
            // 
            this.kpiTotal.CardTitle = "TỔNG HỘI VIÊN";
            this.kpiTotal.Dock = System.Windows.Forms.DockStyle.Fill;
            this.kpiTotal.KpiSubtitle = "";
            this.kpiTotal.KpiValue = "12";
            this.kpiTotal.Location = new System.Drawing.Point(0, 0);
            this.kpiTotal.Margin = new System.Windows.Forms.Padding(0, 0, 8, 0);
            this.kpiTotal.Name = "kpiTotal";
            this.kpiTotal.ValueColor = System.Drawing.Color.FromArgb(255, 107, 0);
            // 
            // kpiActive
            // 
            this.kpiActive.CardTitle = "ĐANG HOẠT ĐỘNG";
            this.kpiActive.Dock = System.Windows.Forms.DockStyle.Fill;
            this.kpiActive.KpiSubtitle = "";
            this.kpiActive.KpiValue = "11";
            this.kpiActive.Location = new System.Drawing.Point(306, 0);
            this.kpiActive.Margin = new System.Windows.Forms.Padding(4, 0, 8, 0);
            this.kpiActive.Name = "kpiActive";
            this.kpiActive.ValueColor = System.Drawing.Color.FromArgb(16, 185, 129);
            // 
            // kpiNew
            // 
            this.kpiNew.CardTitle = "HỘI VIÊN MỚI T9";
            this.kpiNew.Dock = System.Windows.Forms.DockStyle.Fill;
            this.kpiNew.KpiSubtitle = "";
            this.kpiNew.KpiValue = "45";
            this.kpiNew.Location = new System.Drawing.Point(612, 0);
            this.kpiNew.Margin = new System.Windows.Forms.Padding(4, 0, 8, 0);
            this.kpiNew.Name = "kpiNew";
            this.kpiNew.ValueColor = System.Drawing.Color.FromArgb(47, 128, 237);
            // 
            // kpiExpiring
            // 
            this.kpiExpiring.CardTitle = "SẮP HẾT HẠN (<=30N)";
            this.kpiExpiring.Dock = System.Windows.Forms.DockStyle.Fill;
            this.kpiExpiring.KpiSubtitle = "";
            this.kpiExpiring.KpiValue = "7";
            this.kpiExpiring.Location = new System.Drawing.Point(918, 0);
            this.kpiExpiring.Margin = new System.Windows.Forms.Padding(4, 0, 0, 0);
            this.kpiExpiring.Name = "kpiExpiring";
            this.kpiExpiring.ValueColor = System.Drawing.Color.FromArgb(168, 85, 247);
            // 
            // subTabs
            // 
            this.subTabs.Dock = System.Windows.Forms.DockStyle.Fill;
            this.subTabs.Height = 36;
            this.subTabs.Location = new System.Drawing.Point(20, 156);
            this.subTabs.Margin = new System.Windows.Forms.Padding(0, 0, 0, 8);
            this.subTabs.Name = "subTabs";
            this.subTabs.SelectedIndex = 0;
            this.subTabs.Size = new System.Drawing.Size(1224, 32);
            this.subTabs.TabIndex = 2;
            this.subTabs.Tabs = new string[] { "TẤT CẢ", "HỘI VIÊN MỚI", "SẮP HẾT HẠN" };
            // 
            // searchBox
            // 
            this.searchBox.CounterText = "Tổng: 12 hội viên";
            this.searchBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.searchBox.Location = new System.Drawing.Point(20, 196);
            this.searchBox.Margin = new System.Windows.Forms.Padding(0, 0, 0, 8);
            this.searchBox.Name = "searchBox";
            this.searchBox.Placeholder = "Tìm hội viên...";
            this.searchBox.Size = new System.Drawing.Size(1224, 34);
            this.searchBox.TabIndex = 3;
            // 
            // tableMembers
            // 
            this.tableMembers.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableMembers.Location = new System.Drawing.Point(20, 238);
            this.tableMembers.Margin = new System.Windows.Forms.Padding(0);
            this.tableMembers.Name = "tableMembers";
            this.tableMembers.Size = new System.Drawing.Size(1224, 448);
            this.tableMembers.TabIndex = 4;
            // 
            // ucAdminMembers
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(14, 15, 18);
            this.Controls.Add(this.mainLayout);
            this.Name = "ucAdminMembers";
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
        private Controls.ModernButton btnAddMember;
        private System.Windows.Forms.TableLayoutPanel tblKpi;
        private Controls.KpiCardControl kpiTotal;
        private Controls.KpiCardControl kpiActive;
        private Controls.KpiCardControl kpiNew;
        private Controls.KpiCardControl kpiExpiring;
        private Controls.SubTabBarControl subTabs;
        private Controls.AdminSearchBoxControl searchBox;
        private Controls.AdminDataTableControl tableMembers;
    }
}
