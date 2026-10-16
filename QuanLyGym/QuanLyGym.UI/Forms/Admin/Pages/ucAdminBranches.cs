using System;
using System.Drawing;
using System.Windows.Forms;
using QuanLyGym.UI.Forms.Admin.Controls;

namespace QuanLyGym.UI.Forms.Admin.Pages
{
    public partial class ucAdminBranches : UserControl
    {
        public ucAdminBranches()
        {
            InitializeComponent();
            SetupTable();
            WireEvents();
        }

        private void SetupTable()
        {
            var columns = new[]
            {
                new TableColumnDef("ID", 0.7f, StringAlignment.Near) { TextColor = AdminTheme.PrimaryOrange, IsBold = true },
                new TableColumnDef("TÊN CHI NHÁNH", 1.3f, StringAlignment.Near) { TextColor = Color.White, IsBold = true },
                new TableColumnDef("ĐỊA CHỈ", 2.5f, StringAlignment.Near) { TextColor = AdminTheme.TextMuted },
                new TableColumnDef("QUẢN LÝ", 1.2f, StringAlignment.Near) { TextColor = AdminTheme.TextMuted },
                new TableColumnDef("HỘI VIÊN", 0.8f, StringAlignment.Center) { TextColor = AdminTheme.PrimaryOrange, IsBold = true },
                new TableColumnDef("NV", 0.6f, StringAlignment.Center) { TextColor = Color.White },
                new TableColumnDef("DOANH THU T9", 1.2f, StringAlignment.Near) { TextColor = AdminTheme.Green, IsBold = true },
                new TableColumnDef("TRẠNG THÁI", 1.0f, StringAlignment.Near) { IsStatusBadge = true },
                new TableColumnDef("", 0.7f, StringAlignment.Center) { IsActionColumn = true }
            };

            var rows = new[]
            {
                new TableRowData("CN001", "CN Hoàn Kiếm", "42 Lý Thường Kiệt, Hoàn Kiếm, Hà Nội", "Lê Thị Quân", "312", "18", "287.000.000 đ", "• ACTIVE"),
                new TableRowData("CN002", "CN Cầu Giấy", "88 Trần Thái Tông, Cầu Giấy, Hà Nội", "Nguyễn Thành Nam", "245", "14", "214.500.000 đ", "• ACTIVE"),
                new TableRowData("CN003", "CN Quận 1", "150 Nguyễn Thị Minh Khai, Q.1, TP.HCM", "Phạm Admin Hai", "398", "22", "365.000.000 đ", "• ACTIVE"),
                new TableRowData("CN004", "CN Bình Thạnh", "35 Đinh Tiên Hoàng, Bình Thạnh, TP.HCM", "Trần Thị Hà", "187", "12", "162.300.000 đ", "• ACTIVE"),
                new TableRowData("CN005", "CN Hải Châu", "27 Phan Châu Trinh, Hải Châu, Đà Nẵng", "Võ Quang Hải", "156", "10", "138.700.000 đ", "• INACTIVE")
            };

            tableBranches.SetData(columns, rows);
        }

        private void WireEvents()
        {
            btnAddBranch.Click += (s, e) =>
            {
                MessageBox.Show("Mở form thêm mới chi nhánh", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            };

            tableBranches.EditActionClicked += (s, row) =>
            {
                string id = row.Cells.Length > 0 ? row.Cells[0] : "";
                string name = row.Cells.Length > 1 ? row.Cells[1] : "";
                MessageBox.Show($"Chỉnh sửa thông tin chi nhánh: [{id}] {name}", "Chỉnh sửa", MessageBoxButtons.OK, MessageBoxIcon.Information);
            };

            tableBranches.DeleteActionClicked += (s, row) =>
            {
                string id = row.Cells.Length > 0 ? row.Cells[0] : "";
                string name = row.Cells.Length > 1 ? row.Cells[1] : "";
                var res = MessageBox.Show($"Bạn có chắc chắn muốn xóa chi nhánh [{id}] {name} không?", "Xác nhận xóa", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (res == DialogResult.Yes)
                {
                    tableBranches.Rows.Remove(row);
                    tableBranches.Invalidate();
                }
            };
        }
    }
}
