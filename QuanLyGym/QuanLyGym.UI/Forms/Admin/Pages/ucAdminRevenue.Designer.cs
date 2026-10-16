namespace QuanLyGym.UI.Forms.Admin.Pages
{
    partial class ucAdminRevenue
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
            this.kpiT9 = new QuanLyGym.UI.Forms.Admin.Controls.KpiCardControl();
            this.kpiT8 = new QuanLyGym.UI.Forms.Admin.Controls.KpiCardControl();
            this.kpiYear = new QuanLyGym.UI.Forms.Admin.Controls.KpiCardControl();
            this.kpiAvg = new QuanLyGym.UI.Forms.Admin.Controls.KpiCardControl();
            this.chartTrend = new QuanLyGym.UI.Forms.Admin.Controls.DualLineSplineChart();
            this.tblBottom = new System.Windows.Forms.TableLayoutPanel();
            this.chartBranchRevenue = new QuanLyGym.UI.Forms.Admin.Controls.HorizontalBarChartControl();
            this.chartNewMembers = new QuanLyGym.UI.Forms.Admin.Controls.BarChartControl();
            this.mainLayout.SuspendLayout();
            this.tblKpi.SuspendLayout();
            this.tblBottom.SuspendLayout();
            this.SuspendLayout();
            // 
            // mainLayout
            // 
            this.mainLayout.ColumnCount = 1;
            this.mainLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.mainLayout.Controls.Add(this.lblTitle, 0, 0);
            this.mainLayout.Controls.Add(this.tblKpi, 0, 1);
            this.mainLayout.Controls.Add(this.chartTrend, 0, 2);
            this.mainLayout.Controls.Add(this.tblBottom, 0, 3);
            this.mainLayout.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mainLayout.Location = new System.Drawing.Point(0, 0);
            this.mainLayout.Name = "mainLayout";
            this.mainLayout.Padding = new System.Windows.Forms.Padding(18, 10, 18, 10);
            this.mainLayout.RowCount = 4;
            this.mainLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 34F));
            this.mainLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 86F));
            this.mainLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 52F));
            this.mainLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 48F));
            this.mainLayout.Size = new System.Drawing.Size(1264, 700);
            this.mainLayout.TabIndex = 0;
            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblTitle.Font = new System.Drawing.Font("Segoe UI", 15F, System.Drawing.FontStyle.Bold);
            this.lblTitle.ForeColor = System.Drawing.Color.White;
            this.lblTitle.Location = new System.Drawing.Point(18, 10);
            this.lblTitle.Margin = new System.Windows.Forms.Padding(0);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(1228, 34);
            this.lblTitle.TabIndex = 0;
            this.lblTitle.Text = "QUẢN LÝ DOANH THU";
            this.lblTitle.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // tblKpi
            // 
            this.tblKpi.ColumnCount = 4;
            this.tblKpi.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tblKpi.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tblKpi.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tblKpi.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tblKpi.Controls.Add(this.kpiT9, 0, 0);
            this.tblKpi.Controls.Add(this.kpiT8, 1, 0);
            this.tblKpi.Controls.Add(this.kpiYear, 2, 0);
            this.tblKpi.Controls.Add(this.kpiAvg, 3, 0);
            this.tblKpi.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tblKpi.Location = new System.Drawing.Point(18, 44);
            this.tblKpi.Margin = new System.Windows.Forms.Padding(0, 0, 0, 8);
            this.tblKpi.Name = "tblKpi";
            this.tblKpi.RowCount = 1;
            this.tblKpi.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tblKpi.Size = new System.Drawing.Size(1228, 78);
            this.tblKpi.TabIndex = 1;
            // 
            // kpiT9
            // 
            this.kpiT9.CardTitle = "T9/2026";
            this.kpiT9.Dock = System.Windows.Forms.DockStyle.Fill;
            this.kpiT9.KpiSubtitle = "+6,4% so T8";
            this.kpiT9.KpiValue = "867 triệu đ";
            this.kpiT9.Location = new System.Drawing.Point(0, 0);
            this.kpiT9.Margin = new System.Windows.Forms.Padding(0, 0, 8, 0);
            this.kpiT9.Name = "kpiT9";
            this.kpiT9.ValueColor = System.Drawing.Color.FromArgb(255, 107, 0);
            // 
            // kpiT8
            // 
            this.kpiT8.CardTitle = "T8/2026";
            this.kpiT8.Dock = System.Windows.Forms.DockStyle.Fill;
            this.kpiT8.KpiSubtitle = "+16,7% so T7";
            this.kpiT8.KpiValue = "815 triệu đ";
            this.kpiT8.Location = new System.Drawing.Point(307, 0);
            this.kpiT8.Margin = new System.Windows.Forms.Padding(4, 0, 8, 0);
            this.kpiT8.Name = "kpiT8";
            this.kpiT8.ValueColor = System.Drawing.Color.FromArgb(255, 107, 0);
            // 
            // kpiYear
            // 
            this.kpiYear.CardTitle = "TỔNG NĂM 2026";
            this.kpiYear.Dock = System.Windows.Forms.DockStyle.Fill;
            this.kpiYear.KpiSubtitle = "9 tháng ghi nhận";
            this.kpiYear.KpiValue = "6.009 triệu đ";
            this.kpiYear.Location = new System.Drawing.Point(612, 0);
            this.kpiYear.Margin = new System.Windows.Forms.Padding(4, 0, 8, 0);
            this.kpiYear.Name = "kpiYear";
            this.kpiYear.ValueColor = System.Drawing.Color.FromArgb(16, 185, 129);
            // 
            // kpiAvg
            // 
            this.kpiAvg.CardTitle = "TRUNG BÌNH/THÁNG";
            this.kpiAvg.Dock = System.Windows.Forms.DockStyle.Fill;
            this.kpiAvg.KpiSubtitle = "Năm 2026";
            this.kpiAvg.KpiValue = "668 triệu đ";
            this.kpiAvg.Location = new System.Drawing.Point(918, 0);
            this.kpiAvg.Margin = new System.Windows.Forms.Padding(4, 0, 0, 0);
            this.kpiAvg.Name = "kpiAvg";
            this.kpiAvg.ValueColor = System.Drawing.Color.FromArgb(47, 128, 237);
            // 
            // chartTrend
            // 
            this.chartTrend.ChartTitle = "XU HƯỚNG DOANH THU (TRIỆU đ)";
            this.chartTrend.Dock = System.Windows.Forms.DockStyle.Fill;
            this.chartTrend.Location = new System.Drawing.Point(18, 130);
            this.chartTrend.Margin = new System.Windows.Forms.Padding(0, 0, 0, 8);
            this.chartTrend.Name = "chartTrend";
            this.chartTrend.Size = new System.Drawing.Size(1228, 270);
            this.chartTrend.TabIndex = 2;
            // 
            // tblBottom
            // 
            this.tblBottom.ColumnCount = 2;
            this.tblBottom.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tblBottom.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tblBottom.Controls.Add(this.chartBranchRevenue, 0, 0);
            this.tblBottom.Controls.Add(this.chartNewMembers, 1, 0);
            this.tblBottom.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tblBottom.Location = new System.Drawing.Point(18, 408);
            this.tblBottom.Margin = new System.Windows.Forms.Padding(0);
            this.tblBottom.Name = "tblBottom";
            this.tblBottom.RowCount = 1;
            this.tblBottom.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tblBottom.Size = new System.Drawing.Size(1228, 278);
            this.tblBottom.TabIndex = 3;
            // 
            // chartBranchRevenue
            // 
            this.chartBranchRevenue.ChartTitle = "DOANH THU CHI NHÁNH T9/2026";
            this.chartBranchRevenue.Dock = System.Windows.Forms.DockStyle.Fill;
            this.chartBranchRevenue.Location = new System.Drawing.Point(0, 0);
            this.chartBranchRevenue.Margin = new System.Windows.Forms.Padding(0, 0, 6, 0);
            this.chartBranchRevenue.Name = "chartBranchRevenue";
            // 
            // chartNewMembers
            // 
            this.chartNewMembers.ChartTitle = "HỘI VIÊN MỚI THEO THÁNG";
            this.chartNewMembers.Dock = System.Windows.Forms.DockStyle.Fill;
            this.chartNewMembers.Location = new System.Drawing.Point(618, 0);
            this.chartNewMembers.Margin = new System.Windows.Forms.Padding(6, 0, 0, 0);
            this.chartNewMembers.Name = "chartNewMembers";
            // 
            // ucAdminRevenue
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.AutoScrollMinSize = new System.Drawing.Size(1000, 560);
            this.BackColor = System.Drawing.Color.FromArgb(14, 15, 18);
            this.Controls.Add(this.mainLayout);
            this.Name = "ucAdminRevenue";
            this.Size = new System.Drawing.Size(1264, 700);
            this.mainLayout.ResumeLayout(false);
            this.mainLayout.PerformLayout();
            this.tblKpi.ResumeLayout(false);
            this.tblBottom.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel mainLayout;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.TableLayoutPanel tblKpi;
        private Controls.KpiCardControl kpiT9;
        private Controls.KpiCardControl kpiT8;
        private Controls.KpiCardControl kpiYear;
        private Controls.KpiCardControl kpiAvg;
        private Controls.DualLineSplineChart chartTrend;
        private System.Windows.Forms.TableLayoutPanel tblBottom;
        private Controls.HorizontalBarChartControl chartBranchRevenue;
        private Controls.BarChartControl chartNewMembers;
    }
}
