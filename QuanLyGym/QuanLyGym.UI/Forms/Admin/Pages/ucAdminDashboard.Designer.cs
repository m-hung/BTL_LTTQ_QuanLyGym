namespace QuanLyGym.UI.Forms.Admin.Pages
{
    partial class ucAdminDashboard
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
            this.lblTitle = new System.Windows.Forms.Label();
            this.tblKpi = new System.Windows.Forms.TableLayoutPanel();
            this.kpiRevenue = new QuanLyGym.UI.Forms.Admin.Controls.KpiCardControl();
            this.kpiMembers = new QuanLyGym.UI.Forms.Admin.Controls.KpiCardControl();
            this.kpiBranches = new QuanLyGym.UI.Forms.Admin.Controls.KpiCardControl();
            this.kpiEmployees = new QuanLyGym.UI.Forms.Admin.Controls.KpiCardControl();
            this.tblMiddle = new System.Windows.Forms.TableLayoutPanel();
            this.chartMonthlyRevenue = new QuanLyGym.UI.Forms.Admin.Controls.RevenueSplineChart();
            this.expiringMembers = new QuanLyGym.UI.Forms.Admin.Controls.ExpiringMembersControl();
            this.tblBottom = new System.Windows.Forms.TableLayoutPanel();
            this.chartBranchRevenue = new QuanLyGym.UI.Forms.Admin.Controls.BarChartControl();
            this.chartNewMembers = new QuanLyGym.UI.Forms.Admin.Controls.BarChartControl();
            this.mainLayout.SuspendLayout();
            this.tblKpi.SuspendLayout();
            this.tblMiddle.SuspendLayout();
            this.tblBottom.SuspendLayout();
            this.SuspendLayout();
            // 
            // mainLayout
            // 
            this.mainLayout.ColumnCount = 1;
            this.mainLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.mainLayout.Controls.Add(this.lblTitle, 0, 0);
            this.mainLayout.Controls.Add(this.tblKpi, 0, 1);
            this.mainLayout.Controls.Add(this.tblMiddle, 0, 2);
            this.mainLayout.Controls.Add(this.tblBottom, 0, 3);
            this.mainLayout.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mainLayout.Location = new System.Drawing.Point(0, 0);
            this.mainLayout.Name = "mainLayout";
            this.mainLayout.Padding = new System.Windows.Forms.Padding(20, 14, 20, 14);
            this.mainLayout.RowCount = 4;
            this.mainLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 38F));
            this.mainLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 100F));
            this.mainLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 51F));
            this.mainLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 49F));
            this.mainLayout.Size = new System.Drawing.Size(1264, 700);
            this.mainLayout.TabIndex = 0;
            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblTitle.Font = new System.Drawing.Font("Segoe UI", 16F, System.Drawing.FontStyle.Bold);
            this.lblTitle.ForeColor = System.Drawing.Color.White;
            this.lblTitle.Location = new System.Drawing.Point(20, 14);
            this.lblTitle.Margin = new System.Windows.Forms.Padding(0);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(1224, 38);
            this.lblTitle.TabIndex = 0;
            this.lblTitle.Text = "TỔNG QUAN HỆ THỐNG";
            this.lblTitle.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // tblKpi
            // 
            this.tblKpi.ColumnCount = 4;
            this.tblKpi.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tblKpi.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tblKpi.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tblKpi.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tblKpi.Controls.Add(this.kpiRevenue, 0, 0);
            this.tblKpi.Controls.Add(this.kpiMembers, 1, 0);
            this.tblKpi.Controls.Add(this.kpiBranches, 2, 0);
            this.tblKpi.Controls.Add(this.kpiEmployees, 3, 0);
            this.tblKpi.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tblKpi.Location = new System.Drawing.Point(20, 52);
            this.tblKpi.Margin = new System.Windows.Forms.Padding(0, 0, 0, 10);
            this.tblKpi.Name = "tblKpi";
            this.tblKpi.RowCount = 1;
            this.tblKpi.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tblKpi.Size = new System.Drawing.Size(1224, 90);
            this.tblKpi.TabIndex = 1;
            // 
            // kpiRevenue
            // 
            this.kpiRevenue.CardTitle = "TỔNG DOANH THU (2026)";
            this.kpiRevenue.Dock = System.Windows.Forms.DockStyle.Fill;
            this.kpiRevenue.KpiSubtitle = "Cập nhật T9/2026";
            this.kpiRevenue.KpiValue = "6.009 triệu đ";
            this.kpiRevenue.Location = new System.Drawing.Point(0, 0);
            this.kpiRevenue.Margin = new System.Windows.Forms.Padding(0, 0, 8, 0);
            this.kpiRevenue.Name = "kpiRevenue";
            this.kpiRevenue.ValueColor = System.Drawing.Color.FromArgb(255, 107, 0);
            // 
            // kpiMembers
            // 
            this.kpiMembers.CardTitle = "TỔNG HỘI VIÊN";
            this.kpiMembers.Dock = System.Windows.Forms.DockStyle.Fill;
            this.kpiMembers.KpiSubtitle = "11 đang hoạt động";
            this.kpiMembers.KpiValue = "12";
            this.kpiMembers.Location = new System.Drawing.Point(306, 0);
            this.kpiMembers.Margin = new System.Windows.Forms.Padding(4, 0, 8, 0);
            this.kpiMembers.Name = "kpiMembers";
            this.kpiMembers.ValueColor = System.Drawing.Color.FromArgb(47, 128, 237);
            // 
            // kpiBranches
            // 
            this.kpiBranches.CardTitle = "CHI NHÁNH";
            this.kpiBranches.Dock = System.Windows.Forms.DockStyle.Fill;
            this.kpiBranches.KpiSubtitle = "1 đang bảo trì";
            this.kpiBranches.KpiValue = "5";
            this.kpiBranches.Location = new System.Drawing.Point(612, 0);
            this.kpiBranches.Margin = new System.Windows.Forms.Padding(4, 0, 8, 0);
            this.kpiBranches.Name = "kpiBranches";
            this.kpiBranches.ValueColor = System.Drawing.Color.FromArgb(16, 185, 129);
            // 
            // kpiEmployees
            // 
            this.kpiEmployees.CardTitle = "NHÂN VIÊN";
            this.kpiEmployees.Dock = System.Windows.Forms.DockStyle.Fill;
            this.kpiEmployees.KpiSubtitle = "6 PT, 4 thu ngân";
            this.kpiEmployees.KpiValue = "15";
            this.kpiEmployees.Location = new System.Drawing.Point(918, 0);
            this.kpiEmployees.Margin = new System.Windows.Forms.Padding(4, 0, 0, 0);
            this.kpiEmployees.Name = "kpiEmployees";
            this.kpiEmployees.ValueColor = System.Drawing.Color.FromArgb(168, 85, 247);
            // 
            // tblMiddle
            // 
            this.tblMiddle.ColumnCount = 2;
            this.tblMiddle.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 67F));
            this.tblMiddle.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33F));
            this.tblMiddle.Controls.Add(this.chartMonthlyRevenue, 0, 0);
            this.tblMiddle.Controls.Add(this.expiringMembers, 1, 0);
            this.tblMiddle.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tblMiddle.Location = new System.Drawing.Point(20, 152);
            this.tblMiddle.Margin = new System.Windows.Forms.Padding(0, 0, 0, 10);
            this.tblMiddle.Name = "tblMiddle";
            this.tblMiddle.RowCount = 1;
            this.tblMiddle.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tblMiddle.Size = new System.Drawing.Size(1224, 269);
            this.tblMiddle.TabIndex = 2;
            // 
            // chartMonthlyRevenue
            // 
            this.chartMonthlyRevenue.ChartTitle = "DOANH THU THEO THÁNG (TRIỆU đ)";
            this.chartMonthlyRevenue.Dock = System.Windows.Forms.DockStyle.Fill;
            this.chartMonthlyRevenue.Location = new System.Drawing.Point(0, 0);
            this.chartMonthlyRevenue.Margin = new System.Windows.Forms.Padding(0, 0, 8, 0);
            this.chartMonthlyRevenue.Name = "chartMonthlyRevenue";
            // 
            // expiringMembers
            // 
            this.expiringMembers.CardTitle = "SẮP HẾT HẠN";
            this.expiringMembers.Dock = System.Windows.Forms.DockStyle.Fill;
            this.expiringMembers.Location = new System.Drawing.Point(820, 0);
            this.expiringMembers.Margin = new System.Windows.Forms.Padding(4, 0, 0, 0);
            this.expiringMembers.Name = "expiringMembers";
            // 
            // tblBottom
            // 
            this.tblBottom.ColumnCount = 2;
            this.tblBottom.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tblBottom.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tblBottom.Controls.Add(this.chartBranchRevenue, 0, 0);
            this.tblBottom.Controls.Add(this.chartNewMembers, 1, 0);
            this.tblBottom.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tblBottom.Location = new System.Drawing.Point(20, 431);
            this.tblBottom.Margin = new System.Windows.Forms.Padding(0);
            this.tblBottom.Name = "tblBottom";
            this.tblBottom.RowCount = 1;
            this.tblBottom.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tblBottom.Size = new System.Drawing.Size(1224, 255);
            this.tblBottom.TabIndex = 3;
            // 
            // chartBranchRevenue
            // 
            this.chartBranchRevenue.ChartTitle = "DOANH THU THEO CHI NHÁNH (TRIỆU đ)";
            this.chartBranchRevenue.Dock = System.Windows.Forms.DockStyle.Fill;
            this.chartBranchRevenue.Location = new System.Drawing.Point(0, 0);
            this.chartBranchRevenue.Margin = new System.Windows.Forms.Padding(0, 0, 8, 0);
            this.chartBranchRevenue.Name = "chartBranchRevenue";
            // 
            // chartNewMembers
            // 
            this.chartNewMembers.ChartTitle = "HỘI VIÊN MỚI THEO THÁNG";
            this.chartNewMembers.Dock = System.Windows.Forms.DockStyle.Fill;
            this.chartNewMembers.Location = new System.Drawing.Point(612, 0);
            this.chartNewMembers.Margin = new System.Windows.Forms.Padding(4, 0, 0, 0);
            this.chartNewMembers.Name = "chartNewMembers";
            // 
            // ucAdminDashboard
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(14, 15, 18);
            this.Controls.Add(this.mainLayout);
            this.Name = "ucAdminDashboard";
            this.Size = new System.Drawing.Size(1264, 700);
            this.mainLayout.ResumeLayout(false);
            this.mainLayout.PerformLayout();
            this.tblKpi.ResumeLayout(false);
            this.tblMiddle.ResumeLayout(false);
            this.tblBottom.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel mainLayout;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.TableLayoutPanel tblKpi;
        private Controls.KpiCardControl kpiRevenue;
        private Controls.KpiCardControl kpiMembers;
        private Controls.KpiCardControl kpiBranches;
        private Controls.KpiCardControl kpiEmployees;
        private System.Windows.Forms.TableLayoutPanel tblMiddle;
        private Controls.RevenueSplineChart chartMonthlyRevenue;
        private Controls.ExpiringMembersControl expiringMembers;
        private System.Windows.Forms.TableLayoutPanel tblBottom;
        private Controls.BarChartControl chartBranchRevenue;
        private Controls.BarChartControl chartNewMembers;
    }
}
