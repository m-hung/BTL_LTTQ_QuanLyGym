using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using QuanLyGym.UI.Forms.Admin.Controls;

namespace QuanLyGym.UI.Forms.Admin.Pages
{
    public partial class ucAdminMembers : UserControl
    {
        private List<TableRowData> _allRows = new List<TableRowData>();

        public ucAdminMembers()
        {
            InitializeComponent();
            SetupTable();
            WireEvents();
        }

        private void SetupTable()
        {
            var columns = new[]
            {
                new TableColumnDef("ID", 0.6f, StringAlignment.Near) { TextColor = AdminTheme.PrimaryOrange, IsBold = true },
                new TableColumnDef("HỌ TÊN", 1.1f, StringAlignment.Near) { TextColor = Color.White, IsBold = true },
                new TableColumnDef("SĐT", 1.0f, StringAlignment.Near) { TextColor = AdminTheme.TextMuted },
                new TableColumnDef("EMAIL", 1.2f, StringAlignment.Near) { TextColor = AdminTheme.TextMuted },
                new TableColumnDef("GÓI TẬP", 0.9f, StringAlignment.Center) { IsPackageBadge = true },
                new TableColumnDef("CHI NHÁNH", 0.9f, StringAlignment.Near) { TextColor = AdminTheme.TextMuted },
                new TableColumnDef("TÊN ĐĂNG NHẬP", 1.0f, StringAlignment.Near) { TextColor = Color.FromArgb(56, 189, 248) },
                new TableColumnDef("MẬT KHẨU", 0.7f, StringAlignment.Near) { TextColor = AdminTheme.TextDim },
                new TableColumnDef("LẦN CHECKIN CUỐI", 1.2f, StringAlignment.Near) { TextColor = AdminTheme.Green },
                new TableColumnDef("HẾT HẠN", 0.9f, StringAlignment.Near) { TextColor = Color.White },
                new TableColumnDef("TT", 0.8f, StringAlignment.Near) { IsStatusBadge = true },
                new TableColumnDef("", 0.7f, StringAlignment.Center) { IsActionColumn = true }
            };

            _allRows = new List<TableRowData>
            {
                new TableRowData("HV001", "Nguyễn Văn An", "0912.111.222", "an.nv@email.com", "VIP 12T", "Hoàn Kiếm", "nguyenvanan", "••••••••", "30/09/2026 07:32", "15/12/2026", "• ACTIVE"),
                new TableRowData("HV002", "Trần Thị Bình", "0987.222.333", "binh.tt@email.com", "Standard 6T", "Quận 1", "tranthib", "••••••••", "29/09/2026 18:45", "20/11/2026", "• ACTIVE"),
                new TableRowData("HV003", "Lê Minh Cường", "0934.333.444", "cuong.lm@email.com", "VIP 6T", "Cầu Giấy", "leminhcuong", "••••••••", "30/09/2026 06:15", "05/10/2026", "• ACTIVE"),
                new TableRowData("HV004", "Phạm Thị Dung", "0945.444.555", "dung.pt@email.com", "Standard 3T", "Bình Thạnh", "phamthidung", "••••••••", "28/09/2026 09:20", "12/10/2026", "• ACTIVE"),
                new TableRowData("HV005", "Hoàng Văn Em", "0981.555.666", "em.hv@email.com", "VIP 12T", "Hoàn Kiếm", "hoangvane", "••••••••", "30/09/2026 08:00", "01/03/2027", "• ACTIVE"),
                new TableRowData("HV006", "Vũ Thị Phương", "0978.666.777", "phuong.vt@email.com", "Standard 6T", "Hải Châu", "vuthiphuong", "••••••••", "25/09/2026 17:30", "08/10/2026", "• INACTIVE"),
                new TableRowData("HV007", "Đặng Văn Giang", "0923.777.888", "giang.dv@email.com", "VIP 3T", "Quận 1", "dangvangiang", "••••••••", "30/09/2026 11:00", "25/10/2026", "• ACTIVE"),
                new TableRowData("HV008", "Bùi Thị Hằng", "0956.888.999", "hang.bt@email.com", "Standard 12T", "Cầu Giấy", "buithihang", "••••••••", "30/09/2026 07:45", "18/05/2027", "• ACTIVE"),
                new TableRowData("HV009", "Nguyễn Thị Ích", "0983.999.000", "ich.nt@email.com", "VIP 6T", "Hoàn Kiếm", "nguyenthiich", "••••••••", "29/09/2026 20:15", "03/10/2026", "• ACTIVE"),
                new TableRowData("HV010", "Trần Quốc Khánh", "0989.000.111", "khanh.tq@email.com", "Standard 3T", "Bình Thạnh", "tranquockhanh", "••••••••", "27/09/2026 14:30", "01/10/2026", "• ACTIVE")
            };

            tableMembers.SetData(columns, _allRows);
        }

        private void WireEvents()
        {
            btnAddMember.Click += (s, e) =>
            {
                MessageBox.Show("Mở form thêm mới hội viên", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            };

            searchBox.SearchTextChanged += (s, e) =>
            {
                FilterData();
            };

            subTabs.SelectedIndexChanged += (s, idx) =>
            {
                FilterData();
            };

            tableMembers.EditActionClicked += (s, row) =>
            {
                string id = row.Cells.Length > 0 ? row.Cells[0] : "";
                string name = row.Cells.Length > 1 ? row.Cells[1] : "";
                MessageBox.Show($"Chỉnh sửa thông tin hội viên: [{id}] {name}", "Chỉnh sửa", MessageBoxButtons.OK, MessageBoxIcon.Information);
            };

            tableMembers.DeleteActionClicked += (s, row) =>
            {
                string id = row.Cells.Length > 0 ? row.Cells[0] : "";
                string name = row.Cells.Length > 1 ? row.Cells[1] : "";
                var res = MessageBox.Show($"Bạn có chắc chắn muốn xóa hội viên [{id}] {name} không?", "Xác nhận xóa", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (res == DialogResult.Yes)
                {
                    _allRows.Remove(row);
                    FilterData();
                }
            };
        }

        private void FilterData()
        {
            string keyword = searchBox.SearchText.Trim().ToLowerInvariant();
            int tabIndex = subTabs.SelectedIndex;

            var filtered = _allRows.Where(r =>
            {
                // Tab filter: 0 = Tất cả, 1 = Mới (HV007-HV010), 2 = Sắp hết hạn (HV003, HV006, HV009, HV010)
                if (tabIndex == 1 && !r.Cells[4].Contains("3T") && !r.Cells[4].Contains("VIP 3T")) return false;
                if (tabIndex == 2 && !r.Cells[9].Contains("10/2026")) return false;

                if (string.IsNullOrEmpty(keyword)) return true;

                return r.Cells.Any(c => c.ToLowerInvariant().Contains(keyword));
            }).ToList();

            tableMembers.Rows.Clear();
            tableMembers.Rows.AddRange(filtered);
            searchBox.CounterText = $"Tổng: {filtered.Count} hội viên";
            tableMembers.Invalidate();
        }
    }
}
