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
            components = new System.ComponentModel.Container();
            pnlHeader = new Panel();
            lblLogo = new Label();
            lblBrand = new Label();
            lblBrandSub = new Label();
            flpHeaderRight = new FlowLayoutPanel();
            lblDateTime = new Label();
            btnNotification = new Button();
            lblAvatar = new Label();
            pnlUserInfo = new Panel();
            lblUserName = new Label();
            lblUserRole = new Label();
            btnLogout = new Button();
            flpNav = new FlowLayoutPanel();
            btnNavOverview = new Button();
            btnNavBranch = new Button();
            btnNavStaff = new Button();
            btnNavMember = new Button();
            btnNavAccount = new Button();
            pnlContent = new Panel();
            tblMain = new TableLayoutPanel();
            pnlTitleBar = new Panel();
            btnExportReport = new Button();
            lblPageTitle = new Label();
            tblKpi = new TableLayoutPanel();
            pnlKpiRevenue = new Panel();
            pnlKpiMember = new Panel();
            pnlKpiBranch = new Panel();
            pnlKpiStaff = new Panel();
            tblRow2 = new TableLayoutPanel();
            pnlCardRevenue = new Panel();
            pnlCardExpiring = new Panel();
            tblRow3 = new TableLayoutPanel();
            pnlCardBranchRevenue = new Panel();
            pnlCardNewMember = new Panel();
            lblKpiRevenueTitle = new Label();
            lblKpiRevenueValue = new Label();
            lblKpiRevenueSub = new Label();
            lblKpiMemberTitle = new Label();
            lblKpiMemberValue = new Label();
            lblKpiMemberSub = new Label();
            lblKpiBranchTitle = new Label();
            lblKpiBranchValue = new Label();
            lblKpiBranchSub = new Label();
            lblKpiStaffTitle = new Label();
            lblKpiStaffValue = new Label();
            lblKpiStaffSub = new Label();
            chartRevenue = new System.Windows.Forms.DataVisualization.Charting.Chart();
            lblRevenueTitle = new Label();
            lvExpiring = new ListView();
            colName = new ColumnHeader();
            colBranch = new ColumnHeader();
            colRemain = new ColumnHeader();
            lblExpiringTitle = new Label();
            chartBranch = new System.Windows.Forms.DataVisualization.Charting.Chart();
            lblBranchTitle = new Label();
            chartNewMember = new System.Windows.Forms.DataVisualization.Charting.Chart();
            lblNewMemberTitle = new Label();
            tmrClock = new System.Windows.Forms.Timer(components);
            pnlHeader.SuspendLayout();
            flpHeaderRight.SuspendLayout();
            pnlUserInfo.SuspendLayout();
            flpNav.SuspendLayout();
            pnlContent.SuspendLayout();
            tblMain.SuspendLayout();
            pnlTitleBar.SuspendLayout();
            tblKpi.SuspendLayout();
            tblRow2.SuspendLayout();
            tblRow3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)chartRevenue).BeginInit();
            ((System.ComponentModel.ISupportInitialize)chartBranch).BeginInit();
            ((System.ComponentModel.ISupportInitialize)chartNewMember).BeginInit();
            SuspendLayout();
            // 
            // pnlHeader
            // 
            pnlHeader.BackColor = Color.FromArgb(10, 10, 12);
            pnlHeader.Controls.Add(lblLogo);
            pnlHeader.Controls.Add(lblBrand);
            pnlHeader.Controls.Add(lblBrandSub);
            pnlHeader.Controls.Add(flpHeaderRight);
            pnlHeader.Dock = DockStyle.Top;
            pnlHeader.Location = new Point(0, 0);
            pnlHeader.Margin = new Padding(4, 5, 4, 5);
            pnlHeader.Name = "pnlHeader";
            pnlHeader.Size = new Size(1680, 86);
            pnlHeader.TabIndex = 2;
            // 
            // lblLogo
            // 
            lblLogo.BackColor = Color.FromArgb(249, 115, 22);
            lblLogo.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            lblLogo.ForeColor = Color.White;
            lblLogo.Location = new Point(21, 15);
            lblLogo.Margin = new Padding(4, 0, 4, 0);
            lblLogo.Name = "lblLogo";
            lblLogo.Size = new Size(48, 55);
            lblLogo.TabIndex = 0;
            lblLogo.Text = "G";
            lblLogo.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // lblBrand
            // 
            lblBrand.AutoSize = true;
            lblBrand.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            lblBrand.ForeColor = Color.FromArgb(249, 115, 22);
            lblBrand.Location = new Point(80, 12);
            lblBrand.Margin = new Padding(4, 0, 4, 0);
            lblBrand.Name = "lblBrand";
            lblBrand.Size = new Size(85, 28);
            lblBrand.TabIndex = 1;
            lblBrand.Text = "GYMFIT";
            // 
            // lblBrandSub
            // 
            lblBrandSub.AutoSize = true;
            lblBrandSub.Font = new Font("Segoe UI", 7F);
            lblBrandSub.ForeColor = Color.FromArgb(113, 113, 122);
            lblBrandSub.Location = new Point(83, 46);
            lblBrandSub.Margin = new Padding(4, 0, 4, 0);
            lblBrandSub.Name = "lblBrandSub";
            lblBrandSub.Size = new Size(91, 15);
            lblBrandSub.TabIndex = 2;
            lblBrandSub.Text = "ADMIN PORTAL";
            // 
            // flpHeaderRight
            // 
            flpHeaderRight.AutoSize = true;
            flpHeaderRight.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            flpHeaderRight.BackColor = Color.Transparent;
            flpHeaderRight.Controls.Add(lblDateTime);
            flpHeaderRight.Controls.Add(btnNotification);
            flpHeaderRight.Controls.Add(lblAvatar);
            flpHeaderRight.Controls.Add(pnlUserInfo);
            flpHeaderRight.Controls.Add(btnLogout);
            flpHeaderRight.Dock = DockStyle.Right;
            flpHeaderRight.Location = new Point(1058, 0);
            flpHeaderRight.Margin = new Padding(4, 5, 4, 5);
            flpHeaderRight.Name = "flpHeaderRight";
            flpHeaderRight.Padding = new Padding(0, 0, 21, 0);
            flpHeaderRight.Size = new Size(622, 86);
            flpHeaderRight.TabIndex = 3;
            flpHeaderRight.WrapContents = false;
            // 
            // lblDateTime
            // 
            lblDateTime.AutoSize = true;
            lblDateTime.Font = new Font("Consolas", 9.5F);
            lblDateTime.ForeColor = Color.FromArgb(161, 161, 170);
            lblDateTime.Location = new Point(0, 29);
            lblDateTime.Margin = new Padding(0, 29, 19, 0);
            lblDateTime.Name = "lblDateTime";
            lblDateTime.Size = new Size(171, 19);
            lblDateTime.TabIndex = 0;
            lblDateTime.Text = "30/09/2026 — 08:42";
            lblDateTime.TextAlign = ContentAlignment.MiddleRight;
            // 
            // btnNotification
            // 
            btnNotification.BackColor = Color.Transparent;
            btnNotification.Cursor = Cursors.Hand;
            btnNotification.FlatAppearance.BorderSize = 0;
            btnNotification.FlatAppearance.MouseOverBackColor = Color.FromArgb(24, 24, 27);
            btnNotification.FlatStyle = FlatStyle.Flat;
            btnNotification.Font = new Font("Segoe UI Emoji", 11F);
            btnNotification.ForeColor = Color.FromArgb(161, 161, 170);
            btnNotification.Location = new Point(190, 17);
            btnNotification.Margin = new Padding(0, 17, 19, 0);
            btnNotification.Name = "btnNotification";
            btnNotification.Size = new Size(45, 52);
            btnNotification.TabIndex = 1;
            btnNotification.Text = "🔔";
            btnNotification.UseVisualStyleBackColor = false;
            // 
            // lblAvatar
            // 
            lblAvatar.BackColor = Color.FromArgb(124, 45, 18);
            lblAvatar.Cursor = Cursors.Hand;
            lblAvatar.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
            lblAvatar.ForeColor = Color.FromArgb(249, 115, 22);
            lblAvatar.Location = new Point(254, 15);
            lblAvatar.Margin = new Padding(0, 15, 11, 0);
            lblAvatar.Name = "lblAvatar";
            lblAvatar.Size = new Size(48, 55);
            lblAvatar.TabIndex = 2;
            lblAvatar.Text = "SA";
            lblAvatar.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // pnlUserInfo
            // 
            pnlUserInfo.AutoSize = true;
            pnlUserInfo.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            pnlUserInfo.BackColor = Color.Transparent;
            pnlUserInfo.Controls.Add(lblUserName);
            pnlUserInfo.Controls.Add(lblUserRole);
            pnlUserInfo.Location = new Point(313, 14);
            pnlUserInfo.Margin = new Padding(0, 14, 21, 0);
            pnlUserInfo.Name = "pnlUserInfo";
            pnlUserInfo.Size = new Size(131, 46);
            pnlUserInfo.TabIndex = 3;
            // 
            // lblUserName
            // 
            lblUserName.AutoSize = true;
            lblUserName.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            lblUserName.ForeColor = Color.FromArgb(244, 244, 245);
            lblUserName.Location = new Point(0, 2);
            lblUserName.Margin = new Padding(4, 0, 4, 0);
            lblUserName.Name = "lblUserName";
            lblUserName.Size = new Size(127, 20);
            lblUserName.TabIndex = 0;
            lblUserName.Text = "Nguyễn Quản Trị";
            // 
            // lblUserRole
            // 
            lblUserRole.AutoSize = true;
            lblUserRole.Font = new Font("Segoe UI", 7.5F);
            lblUserRole.ForeColor = Color.FromArgb(249, 115, 22);
            lblUserRole.Location = new Point(0, 29);
            lblUserRole.Margin = new Padding(4, 0, 4, 0);
            lblUserRole.Name = "lblUserRole";
            lblUserRole.Size = new Size(83, 17);
            lblUserRole.TabIndex = 1;
            lblUserRole.Text = "Super Admin";
            // 
            // btnLogout
            // 
            btnLogout.BackColor = Color.FromArgb(24, 24, 27);
            btnLogout.Cursor = Cursors.Hand;
            btnLogout.FlatAppearance.BorderColor = Color.FromArgb(63, 63, 70);
            btnLogout.FlatAppearance.MouseOverBackColor = Color.FromArgb(39, 39, 42);
            btnLogout.FlatStyle = FlatStyle.Flat;
            btnLogout.Font = new Font("Segoe UI", 8.5F, FontStyle.Bold);
            btnLogout.ForeColor = Color.FromArgb(244, 244, 245);
            btnLogout.Location = new Point(465, 17);
            btnLogout.Margin = new Padding(0, 17, 0, 0);
            btnLogout.Name = "btnLogout";
            btnLogout.Size = new Size(136, 52);
            btnLogout.TabIndex = 4;
            btnLogout.Text = "⇥ Đăng xuất";
            btnLogout.UseVisualStyleBackColor = false;
            btnLogout.Click += btnLogout_Click;
            // 
            // flpNav
            // 
            flpNav.BackColor = Color.FromArgb(10, 10, 12);
            flpNav.Controls.Add(btnNavOverview);
            flpNav.Controls.Add(btnNavBranch);
            flpNav.Controls.Add(btnNavStaff);
            flpNav.Controls.Add(btnNavMember);
            flpNav.Controls.Add(btnNavAccount);
            flpNav.Dock = DockStyle.Top;
            flpNav.Location = new Point(0, 86);
            flpNav.Margin = new Padding(4, 5, 4, 5);
            flpNav.Name = "flpNav";
            flpNav.Padding = new Padding(11, 3, 0, 3);
            flpNav.Size = new Size(1680, 68);
            flpNav.TabIndex = 1;
            flpNav.WrapContents = false;
            // 
            // btnNavOverview
            // 
            btnNavOverview.BackColor = Color.FromArgb(10, 10, 12);
            btnNavOverview.Cursor = Cursors.Hand;
            btnNavOverview.FlatAppearance.BorderSize = 0;
            btnNavOverview.FlatStyle = FlatStyle.Flat;
            btnNavOverview.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            btnNavOverview.ForeColor = Color.FromArgb(161, 161, 170);
            btnNavOverview.Location = new Point(11, 3);
            btnNavOverview.Margin = new Padding(0, 0, 4, 0);
            btnNavOverview.Name = "btnNavOverview";
            btnNavOverview.Size = new Size(160, 50);
            btnNavOverview.TabIndex = 0;
            btnNavOverview.Text = "▦  Tổng quan";
            btnNavOverview.UseVisualStyleBackColor = false;
            btnNavOverview.Click += btnNav_Click;
            // 
            // btnNavBranch
            // 
            btnNavBranch.BackColor = Color.FromArgb(10, 10, 12);
            btnNavBranch.Cursor = Cursors.Hand;
            btnNavBranch.FlatAppearance.BorderSize = 0;
            btnNavBranch.FlatStyle = FlatStyle.Flat;
            btnNavBranch.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            btnNavBranch.ForeColor = Color.FromArgb(161, 161, 170);
            btnNavBranch.Location = new Point(175, 3);
            btnNavBranch.Margin = new Padding(0, 0, 4, 0);
            btnNavBranch.Name = "btnNavBranch";
            btnNavBranch.Size = new Size(160, 50);
            btnNavBranch.TabIndex = 1;
            btnNavBranch.Text = "⌂  Chi nhánh";
            btnNavBranch.UseVisualStyleBackColor = false;
            btnNavBranch.Click += btnNav_Click;
            // 
            // btnNavStaff
            // 
            btnNavStaff.BackColor = Color.FromArgb(10, 10, 12);
            btnNavStaff.Cursor = Cursors.Hand;
            btnNavStaff.FlatAppearance.BorderSize = 0;
            btnNavStaff.FlatStyle = FlatStyle.Flat;
            btnNavStaff.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            btnNavStaff.ForeColor = Color.FromArgb(161, 161, 170);
            btnNavStaff.Location = new Point(339, 3);
            btnNavStaff.Margin = new Padding(0, 0, 4, 0);
            btnNavStaff.Name = "btnNavStaff";
            btnNavStaff.Size = new Size(160, 50);
            btnNavStaff.TabIndex = 2;
            btnNavStaff.Text = "☺  Nhân viên";
            btnNavStaff.UseVisualStyleBackColor = false;
            btnNavStaff.Click += btnNav_Click;
            // 
            // btnNavMember
            // 
            btnNavMember.BackColor = Color.FromArgb(10, 10, 12);
            btnNavMember.Cursor = Cursors.Hand;
            btnNavMember.FlatAppearance.BorderSize = 0;
            btnNavMember.FlatStyle = FlatStyle.Flat;
            btnNavMember.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            btnNavMember.ForeColor = Color.FromArgb(161, 161, 170);
            btnNavMember.Location = new Point(503, 3);
            btnNavMember.Margin = new Padding(0, 0, 4, 0);
            btnNavMember.Name = "btnNavMember";
            btnNavMember.Size = new Size(160, 50);
            btnNavMember.TabIndex = 3;
            btnNavMember.Text = "♙  Hội viên";
            btnNavMember.UseVisualStyleBackColor = false;
            btnNavMember.Click += btnNav_Click;
            // 
            // btnNavAccount
            // 
            btnNavAccount.BackColor = Color.FromArgb(10, 10, 12);
            btnNavAccount.Cursor = Cursors.Hand;
            btnNavAccount.FlatAppearance.BorderSize = 0;
            btnNavAccount.FlatStyle = FlatStyle.Flat;
            btnNavAccount.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            btnNavAccount.ForeColor = Color.FromArgb(161, 161, 170);
            btnNavAccount.Location = new Point(667, 3);
            btnNavAccount.Margin = new Padding(0, 0, 4, 0);
            btnNavAccount.Name = "btnNavAccount";
            btnNavAccount.Size = new Size(160, 50);
            btnNavAccount.TabIndex = 4;
            btnNavAccount.Text = "⚿  Tài khoản";
            btnNavAccount.UseVisualStyleBackColor = false;
            btnNavAccount.Click += btnNav_Click;
            // 
            // pnlContent
            // 
            pnlContent.BackColor = Color.FromArgb(15, 15, 17);
            pnlContent.Controls.Add(tblMain);
            pnlContent.Dock = DockStyle.Fill;
            pnlContent.Location = new Point(0, 154);
            pnlContent.Margin = new Padding(4, 5, 4, 5);
            pnlContent.Name = "pnlContent";
            pnlContent.Padding = new Padding(12, 8, 12, 10);
            pnlContent.Size = new Size(1680, 901);
            pnlContent.TabIndex = 0;
            // 
            // tblMain
            // 
            tblMain.ColumnCount = 1;
            tblMain.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tblMain.Controls.Add(pnlTitleBar, 0, 0);
            tblMain.Controls.Add(tblKpi, 0, 1);
            tblMain.Controls.Add(tblRow2, 0, 2);
            tblMain.Controls.Add(tblRow3, 0, 3);
            tblMain.Dock = DockStyle.Fill;
            tblMain.Location = new Point(12, 8);
            tblMain.Margin = new Padding(4, 5, 4, 5);
            tblMain.Name = "tblMain";
            tblMain.RowCount = 4;
            tblMain.RowStyles.Add(new RowStyle(SizeType.Absolute, 46F));
            tblMain.RowStyles.Add(new RowStyle(SizeType.Absolute, 151F));
            tblMain.RowStyles.Add(new RowStyle(SizeType.Percent, 54F));
            tblMain.RowStyles.Add(new RowStyle(SizeType.Percent, 46F));
            tblMain.Size = new Size(1656, 883);
            tblMain.TabIndex = 0;
            // 
            // pnlTitleBar
            // 
            pnlTitleBar.Controls.Add(btnExportReport);
            pnlTitleBar.Controls.Add(lblPageTitle);
            pnlTitleBar.Dock = DockStyle.Fill;
            pnlTitleBar.Location = new Point(0, 0);
            pnlTitleBar.Margin = new Padding(0);
            pnlTitleBar.Name = "pnlTitleBar";
            pnlTitleBar.Padding = new Padding(4, 4, 8, 4);
            pnlTitleBar.Size = new Size(1656, 46);
            pnlTitleBar.TabIndex = 0;
            // 
            // btnExportReport
            // 
            btnExportReport.BackColor = Color.FromArgb(249, 115, 22);
            btnExportReport.Cursor = Cursors.Hand;
            btnExportReport.Dock = DockStyle.Right;
            btnExportReport.FlatAppearance.BorderSize = 0;
            btnExportReport.FlatAppearance.MouseOverBackColor = Color.FromArgb(234, 88, 12);
            btnExportReport.FlatStyle = FlatStyle.Flat;
            btnExportReport.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            btnExportReport.ForeColor = Color.White;
            btnExportReport.Location = new Point(1496, 4);
            btnExportReport.Margin = new Padding(4, 5, 4, 5);
            btnExportReport.Name = "btnExportReport";
            btnExportReport.Size = new Size(152, 38);
            btnExportReport.TabIndex = 0;
            btnExportReport.Text = "📄 Xuất báo cáo";
            btnExportReport.UseVisualStyleBackColor = false;
            btnExportReport.Click += btnExportReport_Click;
            // 
            // lblPageTitle
            // 
            lblPageTitle.AutoSize = true;
            lblPageTitle.Dock = DockStyle.Left;
            lblPageTitle.Font = new Font("Segoe UI", 14F, FontStyle.Bold);
            lblPageTitle.ForeColor = Color.FromArgb(244, 244, 245);
            lblPageTitle.Location = new Point(4, 4);
            lblPageTitle.Margin = new Padding(4, 0, 4, 0);
            lblPageTitle.Name = "lblPageTitle";
            lblPageTitle.Size = new Size(290, 32);
            lblPageTitle.TabIndex = 1;
            lblPageTitle.Text = "TỔNG QUAN HỆ THỐNG";
            lblPageTitle.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // tblKpi
            // 
            tblKpi.ColumnCount = 4;
            tblKpi.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));
            tblKpi.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));
            tblKpi.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));
            tblKpi.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));
            tblKpi.Controls.Add(pnlKpiRevenue, 0, 0);
            tblKpi.Controls.Add(pnlKpiMember, 1, 0);
            tblKpi.Controls.Add(pnlKpiBranch, 2, 0);
            tblKpi.Controls.Add(pnlKpiStaff, 3, 0);
            tblKpi.Dock = DockStyle.Fill;
            tblKpi.Location = new Point(0, 46);
            tblKpi.Margin = new Padding(0);
            tblKpi.Name = "tblKpi";
            tblKpi.RowCount = 1;
            tblKpi.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tblKpi.Size = new Size(1656, 151);
            tblKpi.TabIndex = 1;
            // 
            // pnlKpiRevenue
            // 
            pnlKpiRevenue.Location = new Point(4, 5);
            pnlKpiRevenue.Margin = new Padding(4, 5, 4, 5);
            pnlKpiRevenue.Name = "pnlKpiRevenue";
            pnlKpiRevenue.Size = new Size(52, 141);
            pnlKpiRevenue.TabIndex = 0;
            // 
            // pnlKpiMember
            // 
            pnlKpiMember.Location = new Point(418, 5);
            pnlKpiMember.Margin = new Padding(4, 5, 4, 5);
            pnlKpiMember.Name = "pnlKpiMember";
            pnlKpiMember.Size = new Size(52, 141);
            pnlKpiMember.TabIndex = 1;
            // 
            // pnlKpiBranch
            // 
            pnlKpiBranch.Location = new Point(832, 5);
            pnlKpiBranch.Margin = new Padding(4, 5, 4, 5);
            pnlKpiBranch.Name = "pnlKpiBranch";
            pnlKpiBranch.Size = new Size(52, 141);
            pnlKpiBranch.TabIndex = 2;
            // 
            // pnlKpiStaff
            // 
            pnlKpiStaff.Location = new Point(1246, 5);
            pnlKpiStaff.Margin = new Padding(4, 5, 4, 5);
            pnlKpiStaff.Name = "pnlKpiStaff";
            pnlKpiStaff.Size = new Size(52, 141);
            pnlKpiStaff.TabIndex = 3;
            // 
            // tblRow2
            // 
            tblRow2.ColumnCount = 2;
            tblRow2.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 65F));
            tblRow2.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 35F));
            tblRow2.Controls.Add(pnlCardRevenue, 0, 0);
            tblRow2.Controls.Add(pnlCardExpiring, 1, 0);
            tblRow2.Dock = DockStyle.Fill;
            tblRow2.Location = new Point(0, 197);
            tblRow2.Margin = new Padding(0);
            tblRow2.Name = "tblRow2";
            tblRow2.RowCount = 1;
            tblRow2.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tblRow2.Size = new Size(1656, 370);
            tblRow2.TabIndex = 2;
            // 
            // pnlCardRevenue
            // 
            pnlCardRevenue.Location = new Point(4, 5);
            pnlCardRevenue.Margin = new Padding(4, 5, 4, 5);
            pnlCardRevenue.Name = "pnlCardRevenue";
            pnlCardRevenue.Size = new Size(148, 2);
            pnlCardRevenue.TabIndex = 0;
            // 
            // pnlCardExpiring
            // 
            pnlCardExpiring.Location = new Point(1080, 5);
            pnlCardExpiring.Margin = new Padding(4, 5, 4, 5);
            pnlCardExpiring.Name = "pnlCardExpiring";
            pnlCardExpiring.Size = new Size(76, 2);
            pnlCardExpiring.TabIndex = 1;
            // 
            // tblRow3
            // 
            tblRow3.ColumnCount = 2;
            tblRow3.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tblRow3.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tblRow3.Controls.Add(pnlCardBranchRevenue, 0, 0);
            tblRow3.Controls.Add(pnlCardNewMember, 1, 0);
            tblRow3.Dock = DockStyle.Fill;
            tblRow3.Location = new Point(0, 567);
            tblRow3.Margin = new Padding(0);
            tblRow3.Name = "tblRow3";
            tblRow3.RowCount = 1;
            tblRow3.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tblRow3.Size = new Size(1656, 316);
            tblRow3.TabIndex = 3;
            // 
            // pnlCardBranchRevenue
            // 
            pnlCardBranchRevenue.Location = new Point(4, 5);
            pnlCardBranchRevenue.Margin = new Padding(4, 5, 4, 5);
            pnlCardBranchRevenue.Name = "pnlCardBranchRevenue";
            pnlCardBranchRevenue.Size = new Size(112, 2);
            pnlCardBranchRevenue.TabIndex = 0;
            // 
            // pnlCardNewMember
            // 
            pnlCardNewMember.Location = new Point(832, 5);
            pnlCardNewMember.Margin = new Padding(4, 5, 4, 5);
            pnlCardNewMember.Name = "pnlCardNewMember";
            pnlCardNewMember.Size = new Size(112, 2);
            pnlCardNewMember.TabIndex = 1;
            // 
            // lblKpiRevenueTitle
            // 
            lblKpiRevenueTitle.Location = new Point(0, 0);
            lblKpiRevenueTitle.Name = "lblKpiRevenueTitle";
            lblKpiRevenueTitle.Size = new Size(100, 23);
            lblKpiRevenueTitle.TabIndex = 0;
            // 
            // lblKpiRevenueValue
            // 
            lblKpiRevenueValue.Location = new Point(0, 0);
            lblKpiRevenueValue.Name = "lblKpiRevenueValue";
            lblKpiRevenueValue.Size = new Size(100, 23);
            lblKpiRevenueValue.TabIndex = 0;
            // 
            // lblKpiRevenueSub
            // 
            lblKpiRevenueSub.Location = new Point(0, 0);
            lblKpiRevenueSub.Name = "lblKpiRevenueSub";
            lblKpiRevenueSub.Size = new Size(100, 23);
            lblKpiRevenueSub.TabIndex = 0;
            // 
            // lblKpiMemberTitle
            // 
            lblKpiMemberTitle.Location = new Point(0, 0);
            lblKpiMemberTitle.Name = "lblKpiMemberTitle";
            lblKpiMemberTitle.Size = new Size(100, 23);
            lblKpiMemberTitle.TabIndex = 0;
            // 
            // lblKpiMemberValue
            // 
            lblKpiMemberValue.Location = new Point(0, 0);
            lblKpiMemberValue.Name = "lblKpiMemberValue";
            lblKpiMemberValue.Size = new Size(100, 23);
            lblKpiMemberValue.TabIndex = 0;
            // 
            // lblKpiMemberSub
            // 
            lblKpiMemberSub.Location = new Point(0, 0);
            lblKpiMemberSub.Name = "lblKpiMemberSub";
            lblKpiMemberSub.Size = new Size(100, 23);
            lblKpiMemberSub.TabIndex = 0;
            // 
            // lblKpiBranchTitle
            // 
            lblKpiBranchTitle.Location = new Point(0, 0);
            lblKpiBranchTitle.Name = "lblKpiBranchTitle";
            lblKpiBranchTitle.Size = new Size(100, 23);
            lblKpiBranchTitle.TabIndex = 0;
            // 
            // lblKpiBranchValue
            // 
            lblKpiBranchValue.Location = new Point(0, 0);
            lblKpiBranchValue.Name = "lblKpiBranchValue";
            lblKpiBranchValue.Size = new Size(100, 23);
            lblKpiBranchValue.TabIndex = 0;
            // 
            // lblKpiBranchSub
            // 
            lblKpiBranchSub.Location = new Point(0, 0);
            lblKpiBranchSub.Name = "lblKpiBranchSub";
            lblKpiBranchSub.Size = new Size(100, 23);
            lblKpiBranchSub.TabIndex = 0;
            // 
            // lblKpiStaffTitle
            // 
            lblKpiStaffTitle.Location = new Point(0, 0);
            lblKpiStaffTitle.Name = "lblKpiStaffTitle";
            lblKpiStaffTitle.Size = new Size(100, 23);
            lblKpiStaffTitle.TabIndex = 0;
            // 
            // lblKpiStaffValue
            // 
            lblKpiStaffValue.Location = new Point(0, 0);
            lblKpiStaffValue.Name = "lblKpiStaffValue";
            lblKpiStaffValue.Size = new Size(100, 23);
            lblKpiStaffValue.TabIndex = 0;
            // 
            // lblKpiStaffSub
            // 
            lblKpiStaffSub.Location = new Point(0, 0);
            lblKpiStaffSub.Name = "lblKpiStaffSub";
            lblKpiStaffSub.Size = new Size(100, 23);
            lblKpiStaffSub.TabIndex = 0;
            // 
            // chartRevenue
            // 
            chartRevenue.Location = new Point(0, 0);
            chartRevenue.Name = "chartRevenue";
            chartRevenue.Size = new Size(300, 300);
            chartRevenue.TabIndex = 0;
            // 
            // lblRevenueTitle
            // 
            lblRevenueTitle.Location = new Point(0, 0);
            lblRevenueTitle.Name = "lblRevenueTitle";
            lblRevenueTitle.Size = new Size(100, 23);
            lblRevenueTitle.TabIndex = 0;
            // 
            // lvExpiring
            // 
            lvExpiring.BackColor = Color.FromArgb(24, 24, 27);
            lvExpiring.BorderStyle = BorderStyle.None;
            lvExpiring.Columns.AddRange(new ColumnHeader[] { colName, colBranch, colRemain });
            lvExpiring.Dock = DockStyle.Fill;
            lvExpiring.Font = new Font("Segoe UI", 9.5F);
            lvExpiring.ForeColor = Color.FromArgb(244, 244, 245);
            lvExpiring.FullRowSelect = true;
            lvExpiring.HeaderStyle = ColumnHeaderStyle.None;
            lvExpiring.Location = new Point(0, 0);
            lvExpiring.Name = "lvExpiring";
            lvExpiring.Size = new Size(121, 97);
            lvExpiring.TabIndex = 0;
            lvExpiring.UseCompatibleStateImageBehavior = false;
            lvExpiring.View = View.Details;
            // 
            // colName
            // 
            colName.Text = "Hội viên";
            colName.Width = 140;
            // 
            // colBranch
            // 
            colBranch.Text = "Chi nhánh";
            colBranch.Width = 100;
            // 
            // colRemain
            // 
            colRemain.Text = "Còn lại";
            // 
            // lblExpiringTitle
            // 
            lblExpiringTitle.Location = new Point(0, 0);
            lblExpiringTitle.Name = "lblExpiringTitle";
            lblExpiringTitle.Size = new Size(100, 23);
            lblExpiringTitle.TabIndex = 0;
            // 
            // chartBranch
            // 
            chartBranch.Location = new Point(0, 0);
            chartBranch.Name = "chartBranch";
            chartBranch.Size = new Size(300, 300);
            chartBranch.TabIndex = 0;
            // 
            // lblBranchTitle
            // 
            lblBranchTitle.Location = new Point(0, 0);
            lblBranchTitle.Name = "lblBranchTitle";
            lblBranchTitle.Size = new Size(100, 23);
            lblBranchTitle.TabIndex = 0;
            // 
            // chartNewMember
            // 
            chartNewMember.Location = new Point(0, 0);
            chartNewMember.Name = "chartNewMember";
            chartNewMember.Size = new Size(300, 300);
            chartNewMember.TabIndex = 0;
            // 
            // lblNewMemberTitle
            // 
            lblNewMemberTitle.Location = new Point(0, 0);
            lblNewMemberTitle.Name = "lblNewMemberTitle";
            lblNewMemberTitle.Size = new Size(100, 23);
            lblNewMemberTitle.TabIndex = 0;
            // 
            // tmrClock
            // 
            tmrClock.Interval = 1000;
            tmrClock.Tick += tmrClock_Tick;
            // 
            // frmAdminMain
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(15, 15, 17);
            ClientSize = new Size(1680, 1055);
            Controls.Add(pnlContent);
            Controls.Add(flpNav);
            Controls.Add(pnlHeader);
            Margin = new Padding(4, 5, 4, 5);
            MinimumSize = new Size(1461, 1018);
            Name = "frmAdminMain";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "GYMFIT - Admin Portal";
            Load += frmAdminMain_Load;
            Shown += frmAdminMain_Shown;
            pnlHeader.ResumeLayout(false);
            pnlHeader.PerformLayout();
            flpHeaderRight.ResumeLayout(false);
            flpHeaderRight.PerformLayout();
            pnlUserInfo.ResumeLayout(false);
            pnlUserInfo.PerformLayout();
            flpNav.ResumeLayout(false);
            pnlContent.ResumeLayout(false);
            tblMain.ResumeLayout(false);
            pnlTitleBar.ResumeLayout(false);
            pnlTitleBar.PerformLayout();
            tblKpi.ResumeLayout(false);
            tblRow2.ResumeLayout(false);
            tblRow3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)chartRevenue).EndInit();
            ((System.ComponentModel.ISupportInitialize)chartBranch).EndInit();
            ((System.ComponentModel.ISupportInitialize)chartNewMember).EndInit();
            ResumeLayout(false);
        }

        private void ConfigureNavButton(Button btn, string name, string text)
        {
            btn.Name = name;
            btn.Text = text;
            btn.Size = new System.Drawing.Size(120, 40);
            btn.BackColor = System.Drawing.Color.FromArgb(10, 10, 12);
            btn.Cursor = System.Windows.Forms.Cursors.Hand;
            btn.FlatAppearance.BorderSize = 0;
            btn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            btn.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            btn.ForeColor = System.Drawing.Color.FromArgb(161, 161, 170);
            btn.Margin = new System.Windows.Forms.Padding(0, 0, 4, 0);
            btn.UseVisualStyleBackColor = false;
            btn.Click += new System.EventHandler(this.btnNav_Click);
        }

        private void SetupKpiPanel(Panel pnl, Label title, Label value, Label sub, string titleText, string valText, string subText, System.Drawing.Color valColor)
        {
            pnl.Dock = System.Windows.Forms.DockStyle.Fill;
            pnl.Margin = new System.Windows.Forms.Padding(5);
            pnl.BackColor = System.Drawing.Color.FromArgb(24, 24, 27);
            pnl.Paint += new System.Windows.Forms.PaintEventHandler(this.Card_Paint);
            pnl.Controls.Add(title);
            pnl.Controls.Add(value);
            pnl.Controls.Add(sub);

            title.AutoSize = true;
            title.Font = new System.Drawing.Font("Segoe UI", 7.5F, System.Drawing.FontStyle.Bold);
            title.ForeColor = System.Drawing.Color.FromArgb(161, 161, 170);
            title.Location = new System.Drawing.Point(14, 10);
            title.Text = titleText;

            value.AutoSize = true;
            value.Font = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Bold);
            value.ForeColor = valColor;
            value.Location = new System.Drawing.Point(10, 26);
            value.Text = valText;

            sub.AutoSize = true;
            sub.Font = new System.Drawing.Font("Segoe UI", 8F);
            sub.ForeColor = System.Drawing.Color.FromArgb(113, 113, 122);
            sub.Location = new System.Drawing.Point(14, 66);
            sub.Text = subText;
        }

        private void SetupCard(Panel pnl, Label title, string titleText, Control innerControl)
        {
            pnl.BackColor = System.Drawing.Color.FromArgb(24, 24, 27);
            pnl.Dock = System.Windows.Forms.DockStyle.Fill;
            pnl.Margin = new System.Windows.Forms.Padding(5);
            pnl.Padding = new System.Windows.Forms.Padding(12, 8, 12, 12);
            pnl.Paint += new System.Windows.Forms.PaintEventHandler(this.Card_Paint);

            pnl.Controls.Add(innerControl);
            pnl.Controls.Add(title);

            title.Dock = System.Windows.Forms.DockStyle.Top;
            title.Font = new System.Drawing.Font("Segoe UI", 8.5F, System.Drawing.FontStyle.Bold);
            title.ForeColor = System.Drawing.Color.FromArgb(161, 161, 170);
            title.Height = 28;
            title.Text = titleText;
            title.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;

            innerControl.Dock = System.Windows.Forms.DockStyle.Fill;
        }

        #endregion

        private System.Windows.Forms.Panel pnlHeader;
        private System.Windows.Forms.Label lblLogo;
        private System.Windows.Forms.Label lblBrand;
        private System.Windows.Forms.Label lblBrandSub;
        private System.Windows.Forms.FlowLayoutPanel flpHeaderRight;
        private System.Windows.Forms.Label lblDateTime;
        private System.Windows.Forms.Button btnNotification;
        private System.Windows.Forms.Label lblAvatar;
        private System.Windows.Forms.Panel pnlUserInfo;
        private System.Windows.Forms.Label lblUserName;
        private System.Windows.Forms.Label lblUserRole;
        private System.Windows.Forms.Button btnLogout;

        private System.Windows.Forms.FlowLayoutPanel flpNav;
        private System.Windows.Forms.Button btnNavOverview;
        private System.Windows.Forms.Button btnNavBranch;
        private System.Windows.Forms.Button btnNavStaff;
        private System.Windows.Forms.Button btnNavMember;
        private System.Windows.Forms.Button btnNavAccount;

        private System.Windows.Forms.Panel pnlContent;
        private System.Windows.Forms.TableLayoutPanel tblMain;
        private System.Windows.Forms.Panel pnlTitleBar;
        private System.Windows.Forms.Label lblPageTitle;
        private System.Windows.Forms.Button btnExportReport;

        private System.Windows.Forms.TableLayoutPanel tblKpi;
        private System.Windows.Forms.Panel pnlKpiRevenue;
        private System.Windows.Forms.Label lblKpiRevenueTitle;
        private System.Windows.Forms.Label lblKpiRevenueValue;
        private System.Windows.Forms.Label lblKpiRevenueSub;
        private System.Windows.Forms.Panel pnlKpiMember;
        private System.Windows.Forms.Label lblKpiMemberTitle;
        private System.Windows.Forms.Label lblKpiMemberValue;
        private System.Windows.Forms.Label lblKpiMemberSub;
        private System.Windows.Forms.Panel pnlKpiBranch;
        private System.Windows.Forms.Label lblKpiBranchTitle;
        private System.Windows.Forms.Label lblKpiBranchValue;
        private System.Windows.Forms.Label lblKpiBranchSub;
        private System.Windows.Forms.Panel pnlKpiStaff;
        private System.Windows.Forms.Label lblKpiStaffTitle;
        private System.Windows.Forms.Label lblKpiStaffValue;
        private System.Windows.Forms.Label lblKpiStaffSub;

        private System.Windows.Forms.TableLayoutPanel tblRow2;
        private System.Windows.Forms.Panel pnlCardRevenue;
        private System.Windows.Forms.DataVisualization.Charting.Chart chartRevenue;
        private System.Windows.Forms.Label lblRevenueTitle;
        private System.Windows.Forms.Panel pnlCardExpiring;
        private System.Windows.Forms.ListView lvExpiring;
        private System.Windows.Forms.ColumnHeader colName;
        private System.Windows.Forms.ColumnHeader colBranch;
        private System.Windows.Forms.ColumnHeader colRemain;
        private System.Windows.Forms.Label lblExpiringTitle;

        private System.Windows.Forms.TableLayoutPanel tblRow3;
        private System.Windows.Forms.Panel pnlCardBranchRevenue;
        private System.Windows.Forms.DataVisualization.Charting.Chart chartBranch;
        private System.Windows.Forms.Label lblBranchTitle;
        private System.Windows.Forms.Panel pnlCardNewMember;
        private System.Windows.Forms.DataVisualization.Charting.Chart chartNewMember;
        private System.Windows.Forms.Label lblNewMemberTitle;

        private System.Windows.Forms.Timer tmrClock;
    }
}