using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using QuanLyGym.UI.Forms.Admin.Controls;

namespace QuanLyGym.UI.Forms.Admin.Pages
{
    public partial class ucAdminAccounts : UserControl
    {
        private List<TableRowData> _memberAccounts = new List<TableRowData>();
        private List<TableRowData> _ptAccounts = new List<TableRowData>();
        private List<TableRowData> _adminAccounts = new List<TableRowData>();

        public ucAdminAccounts()
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
                new TableColumnDef("HỌ TÊN", 1.2f, StringAlignment.Near) { TextColor = Color.White, IsBold = true },
                new TableColumnDef("TÊN ĐĂNG NHẬP", 1.1f, StringAlignment.Near) { TextColor = Color.FromArgb(56, 189, 248) },
                new TableColumnDef("EMAIL", 1.3f, StringAlignment.Near) { TextColor = AdminTheme.TextMuted },
                new TableColumnDef("CHI NHÁNH", 1.0f, StringAlignment.Near) { TextColor = AdminTheme.TextMuted },
                new TableColumnDef("ĐĂNG NHẬP CUỐI", 1.2f, StringAlignment.Near) { TextColor = AdminTheme.TextMuted },
                new TableColumnDef("TT", 0.8f, StringAlignment.Near) { IsStatusBadge = true },
                new TableColumnDef("", 0.7f, StringAlignment.Center) { IsActionColumn = true }
            };

            // Hội viên accounts (matching Image 5 in Figma)
            _memberAccounts = new List<TableRowData>
            {
                new TableRowData("HV001", "Nguyễn Văn An", "nguyenvanan", "an.nv@email.com", "Hoàn Kiếm", "30/09/2026 07:32", "• ACTIVE"),
                new TableRowData("HV002", "Trần Thị Bình", "tranthib", "binh.tt@email.com", "Quận 1", "29/09/2026 18:45", "• ACTIVE"),
                new TableRowData("HV003", "Lê Minh Cường", "leminhcuong", "cuong.lm@email.com", "Cầu Giấy", "30/09/2026 06:15", "• ACTIVE"),
                new TableRowData("HV004", "Phạm Thị Dung", "phamthidung", "dung.pt@email.com", "Bình Thạnh", "28/09/2026 09:20", "• ACTIVE"),
                new TableRowData("HV005", "Hoàng Văn Em", "hoangvane", "em.hv@email.com", "Hoàn Kiếm", "30/09/2026 08:00", "• ACTIVE"),
                new TableRowData("HV006", "Vũ Thị Phương", "vuthiphuong", "phuong.vt@email.com", "Hải Châu", "25/09/2026 17:30", "• INACTIVE"),
                new TableRowData("HV007", "Đặng Văn Giang", "dangvangiang", "giang.dv@email.com", "Quận 1", "30/09/2026 11:00", "• ACTIVE"),
                new TableRowData("HV008", "Bùi Thị Hằng", "buithihang", "hang.bt@email.com", "Cầu Giấy", "30/09/2026 07:45", "• ACTIVE"),
                new TableRowData("HV009", "Nguyễn Thị Ích", "nguyenthiich", "ich.nt@email.com", "Hoàn Kiếm", "29/09/2026 20:15", "• ACTIVE"),
                new TableRowData("HV010", "Trần Quốc Khánh", "tranquockhanh", "khanh.tq@email.com", "Bình Thạnh", "27/09/2026 14:30", "• ACTIVE"),
                new TableRowData("HV011", "Mai Phương Thảo", "thaomp", "thao.mp@email.com", "Cầu Giấy", "26/09/2026 16:20", "• ACTIVE"),
                new TableRowData("HV012", "Đoàn Tuấn Kiệt", "kietdt", "kiet.dt@email.com", "Quận 1", "24/09/2026 19:10", "• ACTIVE")
            };

            // PT accounts
            _ptAccounts = new List<TableRowData>
            {
                new TableRowData("PT001", "Nguyễn Văn Toàn", "pt.toan", "toan.pt@gymfit.vn", "Hoàn Kiếm", "30/09/2026 07:15", "• ACTIVE"),
                new TableRowData("PT002", "Trần Thị Mai", "pt.mai", "mai.tt@gymfit.vn", "Quận 1", "30/09/2026 08:10", "• ACTIVE"),
                new TableRowData("PT003", "Lê Hoàng Long", "pt.long", "long.lh@gymfit.vn", "Cầu Giấy", "29/09/2026 19:40", "• ACTIVE"),
                new TableRowData("PT004", "Phạm Đức Minh", "pt.minh", "minh.pd@gymfit.vn", "Bình Thạnh", "30/09/2026 06:50", "• ACTIVE"),
                new TableRowData("PT005", "Vũ Quốc Bảo", "pt.bao", "bao.vq@gymfit.vn", "Hải Châu", "28/09/2026 17:05", "• INACTIVE")
            };

            // Admin accounts
            _adminAccounts = new List<TableRowData>
            {
                new TableRowData("AD001", "Nguyễn Quản Trị", "admin.tri", "admin.tri@gymfit.vn", "Hệ thống toàn quốc", "30/09/2026 08:42", "• ACTIVE"),
                new TableRowData("AD002", "Lê Văn Giám Đốc", "admin.director", "director@gymfit.vn", "Hệ thống toàn quốc", "29/09/2026 17:30", "• ACTIVE"),
                new TableRowData("AD003", "Trần Kế Toán Trưởng", "admin.ketoan", "ketoan@gymfit.vn", "Hoàn Kiếm", "30/09/2026 08:00", "• ACTIVE"),
                new TableRowData("AD004", "Phạm Quản Lý HN", "admin.hanoi", "quanly.hn@gymfit.vn", "Cầu Giấy", "28/09/2026 15:45", "• ACTIVE")
            };

            tableAccounts.SetData(columns, _memberAccounts);
            UpdateCounts();
        }

        private void WireEvents()
        {
            btnCreateAccount.Click += (s, e) =>
            {
                MessageBox.Show("Mở form tạo mới tài khoản người dùng", "Tạo tài khoản", MessageBoxButtons.OK, MessageBoxIcon.Information);
            };

            searchBox.SearchTextChanged += (s, e) =>
            {
                FilterData();
            };

            subTabs.SelectedIndexChanged += (s, idx) =>
            {
                FilterData();
            };

            tableAccounts.EditActionClicked += (s, row) =>
            {
                string id = row.Cells.Length > 0 ? row.Cells[0] : "";
                string name = row.Cells.Length > 1 ? row.Cells[1] : "";
                MessageBox.Show($"Chỉnh sửa thông tin tài khoản: [{id}] {name}", "Chỉnh sửa tài khoản", MessageBoxButtons.OK, MessageBoxIcon.Information);
            };

            tableAccounts.DeleteActionClicked += (s, row) =>
            {
                string id = row.Cells.Length > 0 ? row.Cells[0] : "";
                string name = row.Cells.Length > 1 ? row.Cells[1] : "";
                var res = MessageBox.Show($"Bạn có chắc chắn muốn khóa/xóa tài khoản [{id}] {name} không?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (res == DialogResult.Yes)
                {
                    GetCurrentList().Remove(row);
                    UpdateCounts();
                    FilterData();
                }
            };
        }

        private List<TableRowData> GetCurrentList()
        {
            return subTabs.SelectedIndex switch
            {
                1 => _ptAccounts,
                2 => _adminAccounts,
                _ => _memberAccounts
            };
        }

        private void UpdateCounts()
        {
            kpiMembers.KpiValue = _memberAccounts.Count.ToString();
            kpiPT.KpiValue = _ptAccounts.Count.ToString();
            kpiAdmin.KpiValue = _adminAccounts.Count.ToString();
        }

        private void FilterData()
        {
            string keyword = searchBox.SearchText.Trim().ToLowerInvariant();
            var sourceList = GetCurrentList();

            var filtered = sourceList.Where(r =>
            {
                if (string.IsNullOrEmpty(keyword)) return true;
                return r.Cells.Any(c => c.ToLowerInvariant().Contains(keyword));
            }).ToList();

            tableAccounts.Rows.Clear();
            tableAccounts.Rows.AddRange(filtered);

            string roleName = subTabs.SelectedIndex switch
            {
                1 => "PT",
                2 => "Admin",
                _ => "hội viên"
            };
            searchBox.CounterText = $"Tổng: {filtered.Count} tài khoản {roleName}";
            tableAccounts.Invalidate();
        }
    }
}
