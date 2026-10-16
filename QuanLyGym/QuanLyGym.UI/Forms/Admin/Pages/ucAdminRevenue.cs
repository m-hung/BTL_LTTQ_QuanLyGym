using System;
using System.Drawing;
using System.Windows.Forms;

namespace QuanLyGym.UI.Forms.Admin.Pages
{
    public partial class ucAdminRevenue : UserControl
    {
        public ucAdminRevenue()
        {
            InitializeComponent();
            SetupCharts();
        }

        private void SetupCharts()
        {
            // 1. Branch Revenue Horizontal Bar Chart
            chartBranchRevenue.ChartTitle = "DOANH THU CHI NHÁNH T9/2026";
            chartBranchRevenue.BarColor = AdminTheme.PrimaryOrange;
            chartBranchRevenue.SetData(
                categories: new[] { "Hoàn Kiếm", "Cầu Giấy", "Quận 1" },
                values: new float[] { 300, 220, 365 },
                maxX: 380,
                xTicks: new float[] { 0, 95, 190, 285, 380 }
            );

            // 2. New Members Vertical Bar Chart
            chartNewMembers.ChartTitle = "HỘI VIÊN MỚI THEO THÁNG";
            chartNewMembers.BarColor = AdminTheme.Green;
            chartNewMembers.TooltipPrefix = "Tháng";
            chartNewMembers.Unit = "hội viên";
            chartNewMembers.SetData(
                categories: new[] { "T4", "T5", "T6", "T7", "T8", "T9" },
                values: new float[] { 28, 35, 42, 38, 51, 45 },
                maxY: 60,
                yTicks: new float[] { 0, 15, 30, 45, 60 }
            );

            // 3. Trend Spline Chart
            chartTrend.HoverIndex = 7; // Highlights T8/26 tooltip box matching design mockup

            chartTrend.FilterChanged += (s, filter) =>
            {
                // When filter switches between Tháng / Quý / Năm
                if (filter == "Tháng")
                {
                    chartTrend.HoverIndex = 7; // T8/26
                }
                else
                {
                    chartTrend.HoverIndex = -1;
                }
            };
        }
    }
}
