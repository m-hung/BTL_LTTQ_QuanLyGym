using System;
using System.Drawing;
using System.Windows.Forms;

namespace QuanLyGym.UI.Forms.Admin.Pages
{
    public partial class ucAdminDashboard : UserControl
    {
        public ucAdminDashboard()
        {
            InitializeComponent();
            SetupCharts();
        }

        private void SetupCharts()
        {
            // Bottom Left Chart: Doanh thu theo chi nhánh
            chartBranchRevenue.ChartTitle = "DOANH THU THEO CHI NHÁNH (TRIỆU đ)";
            chartBranchRevenue.BarColor = AdminTheme.PrimaryOrange;
            chartBranchRevenue.TooltipPrefix = "Chi nhánh";
            chartBranchRevenue.Unit = "triệu đ";
            chartBranchRevenue.SetData(
                categories: new[] { "Hoàn Kiếm", "Cầu Giấy", "Quận 1", "Bình Thạnh", "Hải Châu" },
                values: new float[] { 205, 140, 260, 110, 95 },
                maxY: 260,
                yTicks: new float[] { 0, 65, 130, 195, 260 }
            );

            // Bottom Right Chart: Hội viên mới theo tháng
            chartNewMembers.ChartTitle = "HỘI VIÊN MỚI THEO THÁNG";
            chartNewMembers.BarColor = AdminTheme.Blue;
            chartNewMembers.TooltipPrefix = "Tháng";
            chartNewMembers.Unit = "hội viên";
            chartNewMembers.SetData(
                categories: new[] { "T4", "T5", "T6", "T7", "T8", "T9" },
                values: new float[] { 28, 35, 42, 38, 52, 48 },
                maxY: 60,
                yTicks: new float[] { 0, 15, 30, 45, 60 }
            );
        }

        public void SetHoverForDemo(int monthlyIndex, int branchIndex, int memberIndex)
        {
            chartMonthlyRevenue.HoverIndex = monthlyIndex;
            chartBranchRevenue.HoverIndex = branchIndex;
            chartNewMembers.HoverIndex = memberIndex;
        }
    }
}
