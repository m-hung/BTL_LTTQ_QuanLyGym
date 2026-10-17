using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Globalization;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace QuanLyGym.UI.Forms.Admin
{
    // =====================================================================
    //  MODEL (tạm thời để dựng giao diện – sau này thay bằng entity thật)
    // =====================================================================
    public enum StaffRole
    {
        PT,
        ThuNgan,
        TapVu,
        BaoVe
    }

    public class StaffInfo
    {
        public string Name { get; set; } = "";
        public StaffRole Role { get; set; }
        public string Phone { get; set; } = "";
        public string Branch { get; set; } = "";

        /// <summary>PT: chứng chỉ. Các vai trò khác: ca làm việc.</summary>
        public string Extra { get; set; } = "";

        public double Rating { get; set; }          // chỉ dùng cho PT
        public int Students { get; set; }           // chỉ dùng cho PT
        public string JoinDate { get; set; } = "";  // dùng cho vai trò khác PT
        public long Salary { get; set; }

        /// <summary>Ảnh đại diện (null => hiển thị chữ cái đầu).</summary>
        public Image? Avatar { get; set; }

        // ----- Thông tin tài khoản đăng nhập -----
        public string Email { get; set; } = "";
        public string Username { get; set; } = "";
        public string Password { get; set; } = "";

        // ----- Thông tin chấm công -----
        public int AttendanceDays { get; set; } = 24;
        public int StandardDays { get; set; } = 26;
        public int LeaveDays { get; set; } = 1;
        public double OvertimeHours { get; set; } = 4.0;

        public string GetEffectiveUsername()
        {
            if (!string.IsNullOrWhiteSpace(Username)) return Username;
            string prefix = Role switch
            {
                StaffRole.PT => "pt_",
                StaffRole.ThuNgan => "tn_",
                StaffRole.TapVu => "tv_",
                _ => "bv_"
            };
            return prefix + RemoveDiacritics(Name).ToLower().Replace(" ", "");
        }

        public string GetEffectivePassword()
        {
            if (!string.IsNullOrWhiteSpace(Password)) return Password;
            return "GymFit@2026";
        }

        public string GetEffectiveEmail()
        {
            if (!string.IsNullOrWhiteSpace(Email)) return Email;
            return RemoveDiacritics(Name).ToLower().Replace(" ", ".") + "@gymfit.vn";
        }

        private static string RemoveDiacritics(string text)
        {
            var normalizedString = text.Normalize(System.Text.NormalizationForm.FormD);
            var stringBuilder = new System.Text.StringBuilder();
            foreach (var c in normalizedString)
            {
                var unicodeCategory = System.Globalization.CharUnicodeInfo.GetUnicodeCategory(c);
                if (unicodeCategory != System.Globalization.UnicodeCategory.NonSpacingMark)
                {
                    stringBuilder.Append(c);
                }
            }
            return stringBuilder.ToString().Normalize(System.Text.NormalizationForm.FormC).Replace('đ', 'd').Replace('Đ', 'D');
        }
    }

    // =====================================================================
    //  BẢNG MÀU + TIỆN ÍCH DÙNG CHUNG (khớp với trang Tổng quan)
    // =====================================================================
    internal static class StaffTheme
    {
        public static readonly Color PageBg = Color.FromArgb(15, 15, 17);
        public static readonly Color CardBg = Color.FromArgb(24, 24, 27);
        public static readonly Color ItemBg = Color.FromArgb(20, 20, 23);
        public static readonly Color ItemHoverBg = Color.FromArgb(30, 30, 34);
        public static readonly Color Border = Color.FromArgb(45, 45, 50);
        public static readonly Color Text = Color.FromArgb(244, 244, 245);
        public static readonly Color Muted = Color.FromArgb(161, 161, 170);
        public static readonly Color Dim = Color.FromArgb(113, 113, 122);

        public static readonly Color Orange = Color.FromArgb(249, 115, 22);
        public static readonly Color OrangeDark = Color.FromArgb(234, 88, 12);
        public static readonly Color Blue = Color.FromArgb(59, 130, 246);
        public static readonly Color Green = Color.FromArgb(34, 197, 94);
        public static readonly Color Purple = Color.FromArgb(168, 85, 247);
        public static readonly Color Yellow = Color.FromArgb(234, 179, 8);

        public static readonly CultureInfo Vi = new CultureInfo("vi-VN");

        public static Color RoleColor(StaffRole role)
        {
            switch (role)
            {
                case StaffRole.PT: return Orange;
                case StaffRole.ThuNgan: return Green;
                case StaffRole.TapVu: return Purple;
                default: return Blue;
            }
        }

        public static string RoleText(StaffRole role)
        {
            switch (role)
            {
                case StaffRole.PT: return "PT";
                case StaffRole.ThuNgan: return "Thu ngân";
                case StaffRole.TapVu: return "Tạp vụ";
                default: return "Bảo vệ";
            }
        }

        public static string RoleBadge(StaffRole role)
        {
            switch (role)
            {
                case StaffRole.PT: return "PT";
                case StaffRole.ThuNgan: return "TN";
                case StaffRole.TapVu: return "TV";
                default: return "BV";
            }
        }

        public static string FormatMoney(long value)
        {
            return value.ToString("N0", Vi) + " ₫";
        }

        public static GraphicsPath RoundRect(Rectangle r, int radius)
        {
            int d = radius * 2;
            var p = new GraphicsPath();
            p.AddArc(r.X, r.Y, d, d, 180, 90);
            p.AddArc(r.Right - d, r.Y, d, d, 270, 90);
            p.AddArc(r.Right - d, r.Bottom - d, d, d, 0, 90);
            p.AddArc(r.X, r.Bottom - d, d, d, 90, 90);
            p.CloseFigure();
            return p;
        }
    }

    // =====================================================================
    //  THẺ NHÂN VIÊN (tự vẽ bằng GDI+ cho nhẹ & mượt)
    // =====================================================================
    internal sealed class StaffCard : Control
    {
        private static readonly Font NameFont = new Font("Segoe UI", 10F, FontStyle.Bold);
        private static readonly Font RoleFont = new Font("Segoe UI", 8F, FontStyle.Bold);
        private static readonly Font InfoFont = new Font("Segoe UI", 8F);
        private static readonly Font SmallFont = new Font("Segoe UI", 7.5F);
        private static readonly Font BadgeFont = new Font("Segoe UI", 6.5F, FontStyle.Bold);
        private static readonly Font InitialFont = new Font("Segoe UI", 15F, FontStyle.Bold);
        private static readonly Font RatingFont = new Font("Segoe UI", 9F, FontStyle.Bold);
        private static readonly Font MoneyFont = new Font("Consolas", 9.5F, FontStyle.Bold);

        private bool _hover;

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public StaffInfo Staff { get; }

        public StaffCard(StaffInfo staff)
        {
            Staff = staff;

            SetStyle(ControlStyles.AllPaintingInWmPaint
                   | ControlStyles.OptimizedDoubleBuffer
                   | ControlStyles.UserPaint
                   | ControlStyles.ResizeRedraw, true);

            Height = 142;
            Cursor = Cursors.Hand;
            Margin = new Padding(0, 0, 14, 14);
        }

        protected override void OnMouseEnter(EventArgs e)
        {
            base.OnMouseEnter(e);
            _hover = true;
            Invalidate();
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            base.OnMouseLeave(e);
            _hover = false;
            Invalidate();
        }

        private static void Txt(Graphics g, string text, Font font, Rectangle rect, Color color,
                                TextFormatFlags align = TextFormatFlags.Left)
        {
            TextRenderer.DrawText(g, text, font, rect, color,
                align | TextFormatFlags.VerticalCenter | TextFormatFlags.NoPadding
                      | TextFormatFlags.NoPrefix | TextFormatFlags.EndEllipsis);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.InterpolationMode = InterpolationMode.HighQualityBicubic;

            Color roleColor = StaffTheme.RoleColor(Staff.Role);

            // Nền + viền
            using (var bg = new SolidBrush(_hover ? StaffTheme.ItemHoverBg : StaffTheme.ItemBg))
                g.FillRectangle(bg, ClientRectangle);
            using (var pen = new Pen(_hover ? StaffTheme.Orange : StaffTheme.Border))
                g.DrawRectangle(pen, 0, 0, Width - 1, Height - 1);

            // ----- Avatar -----
            var avatarRect = new Rectangle(14, 14, 54, 54);
            using (GraphicsPath path = StaffTheme.RoundRect(avatarRect, 5))
            {
                if (Staff.Avatar != null)
                {
                    Image img = Staff.Avatar;
                    float scale = Math.Max((float)avatarRect.Width / img.Width, (float)avatarRect.Height / img.Height);
                    float sw = avatarRect.Width / scale;
                    float sh = avatarRect.Height / scale;
                    g.SetClip(path);
                    g.DrawImage(img, avatarRect,
                        (img.Width - sw) / 2f, (img.Height - sh) / 2f, sw, sh, GraphicsUnit.Pixel);
                    g.ResetClip();
                }
                else
                {
                    using (var br = new LinearGradientBrush(avatarRect,
                        Color.FromArgb(63, 63, 70), Color.FromArgb(39, 39, 42), 45f))
                        g.FillPath(br, path);

                    Txt(g, GetInitials(Staff.Name), InitialFont, avatarRect, roleColor,
                        TextFormatFlags.HorizontalCenter);
                }

                using (var pen = new Pen(Color.FromArgb(63, 63, 70)))
                    g.DrawPath(pen, path);
            }

            // Huy hiệu vai trò đè lên đáy avatar
            var badgeRect = new Rectangle(avatarRect.Left + (avatarRect.Width - 28) / 2, avatarRect.Bottom - 6, 28, 15);
            using (GraphicsPath bp = StaffTheme.RoundRect(badgeRect, 3))
            using (var bb = new SolidBrush(roleColor))
            {
                g.FillPath(bb, bp);
                using (var pen = new Pen(StaffTheme.ItemBg, 2f))
                    g.DrawPath(pen, bp);
            }
            Txt(g, StaffTheme.RoleBadge(Staff.Role), BadgeFont, badgeRect, Color.White,
                TextFormatFlags.HorizontalCenter);

            // ----- Thông tin bên phải avatar -----
            int tx = avatarRect.Right + 14;
            int tw = Width - tx - 12;

            Txt(g, Staff.Name, NameFont, new Rectangle(tx, 12, tw, 20), StaffTheme.Text);
            Txt(g, StaffTheme.RoleText(Staff.Role), RoleFont, new Rectangle(tx, 33, tw, 16), roleColor);
            Txt(g, Staff.Phone, InfoFont, new Rectangle(tx, 50, tw, 16), StaffTheme.Muted);
            Txt(g, "CN " + Staff.Branch, SmallFont, new Rectangle(tx, 66, tw, 15), StaffTheme.Dim);

            // PT: chứng chỉ | Vai trò khác: ca làm
            Txt(g, Staff.Extra, InfoFont, new Rectangle(tx, 82, tw, 16), StaffTheme.Muted);

            // ----- Đường kẻ + footer -----
            using (var pen = new Pen(StaffTheme.Border))
                g.DrawLine(pen, 14, 106, Width - 15, 106);

            var footerY = 110;
            var footerH = 22;

            if (Staff.Role == StaffRole.PT)
            {
                Txt(g, "★ " + Staff.Rating.ToString("0.0", CultureInfo.InvariantCulture), RatingFont,
                    new Rectangle(14, footerY, 90, footerH), StaffTheme.Yellow);
                Txt(g, Staff.Students + " học viên", SmallFont,
                    new Rectangle(0, footerY, Width, footerH), StaffTheme.Dim, TextFormatFlags.HorizontalCenter);
            }
            else
            {
                Txt(g, "Vào làm " + Staff.JoinDate, SmallFont,
                    new Rectangle(14, footerY, 140, footerH), StaffTheme.Dim);
            }

            Txt(g, StaffTheme.FormatMoney(Staff.Salary), MoneyFont,
                new Rectangle(Width - 14 - 130, footerY, 130, footerH), StaffTheme.Green, TextFormatFlags.Right);
        }

        private static string GetInitials(string name)
        {
            string[] parts = name.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            if (parts.Length == 0) return "?";
            if (parts.Length == 1) return parts[0].Substring(0, 1).ToUpper();
            return (parts[0].Substring(0, 1) + parts[parts.Length - 1].Substring(0, 1)).ToUpper();
        }
    }

    // =====================================================================
    //  Ô LỌC DẠNG DROPDOWN (tự vẽ, hợp giao diện tối)
    // =====================================================================
    internal sealed class FilterDropdown : Control
    {
        private readonly List<string> _items = new List<string>();
        private int _selectedIndex;
        private bool _hover;
        private bool _open;

        /// <summary>Chỉ phát sinh khi NGƯỜI DÙNG chọn, không phát sinh khi gọi SetItems().</summary>
        public event EventHandler? SelectedIndexChanged;

        public FilterDropdown()
        {
            SetStyle(ControlStyles.AllPaintingInWmPaint
                   | ControlStyles.OptimizedDoubleBuffer
                   | ControlStyles.UserPaint
                   | ControlStyles.ResizeRedraw, true);

            Font = new Font("Segoe UI", 9F);
            Cursor = Cursors.Hand;
            Height = 36;
        }

        /// <summary>Mục đầu tiên (index 0) luôn là "Tất cả ..." = không lọc.</summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool IsDefault => _selectedIndex <= 0;

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public string SelectedText => _items.Count > 0 ? _items[_selectedIndex] : "";

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public int SelectedIndex
        {
            get => _selectedIndex;
            set
            {
                if (_items.Count == 0) { _selectedIndex = 0; Invalidate(); return; }
                value = Math.Max(0, Math.Min(_items.Count - 1, value));
                if (value == _selectedIndex) return;
                _selectedIndex = value;
                Invalidate();
                SelectedIndexChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        public void SetItems(IEnumerable<string> items, int selectedIndex = 0)
        {
            _items.Clear();
            _items.AddRange(items);
            _selectedIndex = (selectedIndex >= 0 && selectedIndex < _items.Count) ? selectedIndex : 0;
            Invalidate();
        }

        protected override void OnMouseEnter(EventArgs e) { base.OnMouseEnter(e); _hover = true; Invalidate(); }
        protected override void OnMouseLeave(EventArgs e) { base.OnMouseLeave(e); _hover = false; Invalidate(); }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);
            if (e.Button == MouseButtons.Left && ClientRectangle.Contains(e.Location))
                ShowMenu();
        }

        private void ShowMenu()
        {
            if (_items.Count == 0) return;

            var menu = new ContextMenuStrip
            {
                ShowImageMargin = false,
                Renderer = new DarkMenuRenderer(),
                BackColor = StaffTheme.CardBg,
                ForeColor = StaffTheme.Text,
                Font = Font,
                MinimumSize = new Size(Width, 0)
            };

            for (int i = 0; i < _items.Count; i++)
            {
                var item = new ToolStripMenuItem(_items[i])
                {
                    Tag = i,
                    BackColor = StaffTheme.CardBg,
                    ForeColor = i == _selectedIndex ? StaffTheme.Orange : StaffTheme.Text,
                    Padding = new Padding(4, 6, 4, 6)
                };
                item.Click += (o, ev) => SelectedIndex = (int)((ToolStripMenuItem)o!).Tag!;
                menu.Items.Add(item);
            }

            menu.Closed += (o, ev) =>
            {
                _open = false;
                Invalidate();
                BeginInvoke(new Action(() => menu.Dispose()));
            };

            _open = true;
            Invalidate();
            menu.Show(this, new Point(0, Height + 2));
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;

            bool active = !IsDefault;

            using (var bg = new SolidBrush(_hover || _open ? StaffTheme.ItemHoverBg : StaffTheme.ItemBg))
                g.FillRectangle(bg, ClientRectangle);

            Color borderColor = (active || _open) ? StaffTheme.Orange
                              : _hover ? Color.FromArgb(82, 82, 91)
                              : StaffTheme.Border;
            using (var pen = new Pen(borderColor))
                g.DrawRectangle(pen, 0, 0, Width - 1, Height - 1);

            TextRenderer.DrawText(g, SelectedText, Font,
                new Rectangle(12, 0, Math.Max(0, Width - 38), Height),
                active ? StaffTheme.Text : StaffTheme.Muted,
                TextFormatFlags.Left | TextFormatFlags.VerticalCenter | TextFormatFlags.NoPadding
                | TextFormatFlags.NoPrefix | TextFormatFlags.EndEllipsis);

            // Mũi tên ▾
            float cx = Width - 18, cy = Height / 2f;
            using (var pen = new Pen(active ? StaffTheme.Orange : StaffTheme.Muted, 1.6f))
            {
                g.DrawLines(pen, new[]
                {
                    new PointF(cx - 4, cy - 2),
                    new PointF(cx, cy + 2),
                    new PointF(cx + 4, cy - 2)
                });
            }
        }

        // ----- Giao diện tối cho menu thả xuống -----
        private sealed class DarkMenuColors : ProfessionalColorTable
        {
            public override Color ToolStripDropDownBackground => StaffTheme.CardBg;
            public override Color ImageMarginGradientBegin => StaffTheme.CardBg;
            public override Color ImageMarginGradientMiddle => StaffTheme.CardBg;
            public override Color ImageMarginGradientEnd => StaffTheme.CardBg;
            public override Color MenuBorder => StaffTheme.Border;
            public override Color MenuItemBorder => Color.FromArgb(39, 39, 42);
            public override Color MenuItemSelected => Color.FromArgb(39, 39, 42);
            public override Color MenuItemSelectedGradientBegin => Color.FromArgb(39, 39, 42);
            public override Color MenuItemSelectedGradientEnd => Color.FromArgb(39, 39, 42);
        }

        private sealed class DarkMenuRenderer : ToolStripProfessionalRenderer
        {
            public DarkMenuRenderer() : base(new DarkMenuColors()) { RoundedEdges = false; }

            protected override void OnRenderItemText(ToolStripItemTextRenderEventArgs e)
            {
                e.TextColor = e.Item.ForeColor;   // giữ màu chữ đã đặt (cam cho mục đang chọn)
                base.OnRenderItemText(e);
            }
        }
    }

    // =====================================================================
    //  TRANG QUẢN LÝ NHÂN VIÊN
    // =====================================================================
    public class ucAdminStaff : UserControl
    {
        [DllImport("uxtheme.dll", CharSet = CharSet.Unicode)]
        private static extern int SetWindowTheme(IntPtr hWnd, string pszSubAppName, string? pszSubIdList);

        // ----- Controls -----
        private TableLayoutPanel _root = null!;
        private Label _lblTitle = null!;
        private Button _btnAdd = null!;

        private Label _lblKpiTotal = null!;
        private Label _lblKpiPt = null!;
        private Label _lblKpiCashier = null!;
        private Label _lblKpiOther = null!;

        private Panel _pnlTabs = null!;
        private FlowLayoutPanel _tabFlow = null!;
        private FlowLayoutPanel _flpStaff = null!;
        private readonly List<Button> _tabButtons = new List<Button>();

        // ----- Tìm kiếm & bộ lọc -----
        private Panel _pnlFilter = null!;
        private Panel _pnlSearch = null!;
        private TextBox _txtSearch = null!;
        private Label _btnClearSearch = null!;
        private FilterDropdown _ddBranch = null!;
        private FilterDropdown _ddSecondary = null!;   // PT: chứng chỉ | vai trò khác: ca làm
        private Button _btnClearFilter = null!;
        private Label _lblCount = null!;
        private System.Windows.Forms.Timer _searchTimer = null!;
        private bool _searchFocused;
        private bool _filterLock;   // true khi đang đặt lại bộ lọc bằng code (tránh dựng lại danh sách nhiều lần)

        private static readonly string[] ShiftOrder = { "ca sang", "ca chieu", "ca ngay", "ca dem" };

        // ----- State -----
        private readonly List<StaffInfo> _staff = new List<StaffInfo>();
        private StaffRole _activeRole = StaffRole.PT;

        // ----- Events (để form cha gắn xử lý sau này) -----
        public event EventHandler? AddStaffClicked;
        public event EventHandler<StaffInfo>? StaffClicked;

        public ucAdminStaff()
        {
            DoubleBuffered = true;
            Dock = DockStyle.Fill;
            BackColor = StaffTheme.PageBg;
            Padding = new Padding(13, 10, 13, 15);

            BuildUi();
            LoadSampleData();
        }

        // -----------------------------------------------------------------
        //  Dựng giao diện
        // -----------------------------------------------------------------
        private void BuildUi()
        {
            SuspendLayout();

            _root = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 1,
                RowCount = 3,
                BackColor = Color.Transparent,
                Margin = new Padding(0)
            };
            _root.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            _root.RowStyles.Add(new RowStyle(SizeType.Absolute, 65F));   // Tiêu đề
            _root.RowStyles.Add(new RowStyle(SizeType.Absolute, 105F));  // KPI
            _root.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));   // Danh sách

            _root.Controls.Add(BuildTitleBar(), 0, 0);
            _root.Controls.Add(BuildKpiRow(), 0, 1);
            _root.Controls.Add(BuildListCard(), 0, 2);

            Controls.Add(_root);
            ResumeLayout(false);
        }

        private Control BuildTitleBar()
        {
            var pnl = new Panel
            {
                Dock = DockStyle.Fill,
                Margin = new Padding(0),
                Padding = new Padding(5, 6, 11, 6)
            };

            _btnAdd = new Button
            {
                Text = "+  Thêm nhân viên",
                Dock = DockStyle.Right,
                Width = 190,
                FlatStyle = FlatStyle.Flat,
                BackColor = StaffTheme.Orange,
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 9F, FontStyle.Bold),
                Cursor = Cursors.Hand,
                UseVisualStyleBackColor = false
            };
            _btnAdd.FlatAppearance.BorderSize = 0;
            _btnAdd.FlatAppearance.MouseOverBackColor = StaffTheme.OrangeDark;
            _btnAdd.Click += (s, e) =>
            {
                AddStaffClicked?.Invoke(this, EventArgs.Empty);
                using var dlg = new frmStaffEdit(null);
                if (dlg.ShowDialog(this) == DialogResult.OK && dlg.CreatedStaff != null)
                {
                    _staff.Add(dlg.CreatedStaff);
                    UpdateKpis();
                    ClearFilters(false);   // tránh việc nhân viên mới bị bộ lọc cũ che mất
                    SelectTab(dlg.CreatedStaff.Role);
                    MessageBox.Show($"Thêm mới nhân viên '{dlg.CreatedStaff.Name}' thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            };

            _lblTitle = new Label
            {
                Text = "QUẢN LÝ NHÂN VIÊN",
                AutoSize = true,
                Dock = DockStyle.Left,
                Font = new Font("Segoe UI", 14F, FontStyle.Bold),
                ForeColor = StaffTheme.Text,
                TextAlign = ContentAlignment.MiddleLeft
            };

            pnl.Controls.Add(_btnAdd);
            pnl.Controls.Add(_lblTitle);
            return pnl;
        }

        private Control BuildKpiRow()
        {
            var tbl = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 4,
                RowCount = 1,
                Margin = new Padding(0)
            };
            for (int i = 0; i < 4; i++)
                tbl.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));
            tbl.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));

            tbl.Controls.Add(CreateKpi("TỔNG NHÂN VIÊN", StaffTheme.Orange, out _lblKpiTotal), 0, 0);
            tbl.Controls.Add(CreateKpi("PT (HUẤN LUYỆN VIÊN)", StaffTheme.Blue, out _lblKpiPt), 1, 0);
            tbl.Controls.Add(CreateKpi("THU NGÂN", StaffTheme.Green, out _lblKpiCashier), 2, 0);
            tbl.Controls.Add(CreateKpi("TẠP VỤ + BẢO VỆ", StaffTheme.Purple, out _lblKpiOther), 3, 0);
            return tbl;
        }

        private Panel CreateKpi(string title, Color valueColor, out Label valueLabel)
        {
            var pnl = new Panel
            {
                Dock = DockStyle.Fill,
                Margin = new Padding(5),
                BackColor = StaffTheme.CardBg
            };
            pnl.Paint += Card_Paint;

            var lblTitle = new Label
            {
                Text = title,
                AutoSize = true,
                Font = new Font("Segoe UI", 7.5F, FontStyle.Bold),
                ForeColor = StaffTheme.Muted,
                Location = new Point(14, 18)
            };

            valueLabel = new Label
            {
                Text = "0",
                AutoSize = true,
                Font = new Font("Segoe UI", 22F, FontStyle.Bold),
                ForeColor = valueColor,
                Location = new Point(11, 40)
            };

            pnl.Controls.Add(lblTitle);
            pnl.Controls.Add(valueLabel);
            return pnl;
        }

        private Control BuildListCard()
        {
            var card = new Panel
            {
                Dock = DockStyle.Fill,
                Margin = new Padding(5),
                Padding = new Padding(18, 12, 18, 16),
                BackColor = StaffTheme.CardBg
            };
            card.Paint += Card_Paint;

            // --- Thanh tab ---
            _pnlTabs = new Panel
            {
                Dock = DockStyle.Top,
                Height = 46,
                BackColor = StaffTheme.CardBg
            };

            var line = new Panel
            {
                Dock = DockStyle.Bottom,
                Height = 1,
                BackColor = StaffTheme.Border
            };

            _tabFlow = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                WrapContents = false,
                FlowDirection = FlowDirection.LeftToRight,
                BackColor = StaffTheme.CardBg,
                Padding = new Padding(6, 0, 0, 0),
                Margin = new Padding(0)
            };

            AddTab("PT", StaffRole.PT);
            AddTab("THU NGÂN", StaffRole.ThuNgan);
            AddTab("TẠP VỤ", StaffRole.TapVu);
            AddTab("BẢO VỆ", StaffRole.BaoVe);

            _pnlTabs.Controls.Add(_tabFlow);
            _pnlTabs.Controls.Add(line);   // thêm sau => dock trước (đáy)

            // --- Danh sách thẻ nhân viên ---
            _flpStaff = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                AutoScroll = true,
                WrapContents = true,
                FlowDirection = FlowDirection.LeftToRight,
                BackColor = StaffTheme.CardBg,
                Padding = new Padding(0, 16, 0, 0)
            };
            _flpStaff.Resize += (s, e) => ResizeCards();
            _flpStaff.HandleCreated += (s, e) =>
            {
                // Thanh cuộn tối màu (Windows 10/11); lỗi thì bỏ qua
                try { SetWindowTheme(_flpStaff.Handle, "DarkMode_Explorer", null); } catch { }
            };

            card.Controls.Add(_flpStaff);
            card.Controls.Add(BuildFilterBar());   // dock dưới thanh tab
            card.Controls.Add(_pnlTabs);           // thêm sau => dock trước (đỉnh)
            return card;
        }

        private void AddTab(string text, StaffRole role)
        {
            var btn = new Button
            {
                Text = text,
                Tag = role,
                Height = 44,
                FlatStyle = FlatStyle.Flat,
                BackColor = StaffTheme.CardBg,
                ForeColor = StaffTheme.Muted,
                Font = new Font("Segoe UI", 8.5F, FontStyle.Bold),
                Cursor = Cursors.Hand,
                Margin = new Padding(0, 0, 6, 0),
                TabStop = false,
                UseVisualStyleBackColor = false
            };
            btn.FlatAppearance.BorderSize = 0;
            btn.FlatAppearance.MouseOverBackColor = StaffTheme.CardBg;
            btn.FlatAppearance.MouseDownBackColor = StaffTheme.CardBg;
            btn.Width = TextRenderer.MeasureText(text, btn.Font).Width + 36;

            btn.Click += (s, e) => SelectTab(role);
            btn.Paint += Tab_Paint;
            btn.MouseEnter += (s, e) =>
            {
                if ((StaffRole)btn.Tag! != _activeRole) btn.ForeColor = StaffTheme.Text;
            };
            btn.MouseLeave += (s, e) =>
            {
                if ((StaffRole)btn.Tag! != _activeRole) btn.ForeColor = StaffTheme.Muted;
            };

            _tabButtons.Add(btn);
            _tabFlow.Controls.Add(btn);
        }

        // -----------------------------------------------------------------
        //  Vẽ viền / gạch chân tab
        // -----------------------------------------------------------------
        private void Card_Paint(object? sender, PaintEventArgs e)
        {
            var c = (Control)sender!;
            using (var pen = new Pen(StaffTheme.Border))
                e.Graphics.DrawRectangle(pen, 0, 0, c.Width - 1, c.Height - 1);
        }

        private void Tab_Paint(object? sender, PaintEventArgs e)
        {
            var btn = (Button)sender!;
            if ((StaffRole)btn.Tag! == _activeRole)
            {
                using (var br = new SolidBrush(StaffTheme.Orange))
                    e.Graphics.FillRectangle(br, 0, btn.Height - 2, btn.Width, 2);
            }
        }

        // -----------------------------------------------------------------
        //  Dữ liệu & hiển thị
        // -----------------------------------------------------------------
        /// <summary>Nạp danh sách nhân viên (gọi từ form cha khi đã có dữ liệu thật).</summary>
        public void SetStaff(IEnumerable<StaffInfo> list)
        {
            _staff.Clear();
            _staff.AddRange(list);
            UpdateKpis();
            SelectTab(_activeRole);
        }

        private void UpdateKpis()
        {
            _lblKpiTotal.Text = _staff.Count.ToString();
            _lblKpiPt.Text = _staff.Count(x => x.Role == StaffRole.PT).ToString();
            _lblKpiCashier.Text = _staff.Count(x => x.Role == StaffRole.ThuNgan).ToString();
            _lblKpiOther.Text = _staff.Count(x => x.Role == StaffRole.TapVu || x.Role == StaffRole.BaoVe).ToString();
        }

        private void SelectTab(StaffRole role)
        {
            bool roleChanged = role != _activeRole;
            _activeRole = role;

            foreach (Button b in _tabButtons)
            {
                b.ForeColor = (StaffRole)b.Tag! == role ? StaffTheme.Orange : StaffTheme.Muted;
                b.Invalidate();
            }

            RefreshFilterOptions(roleChanged);
            RebuildCards();
        }

        private void RebuildCards()
        {
            _flpStaff.SuspendLayout();

            // Giải phóng thẻ cũ
            var old = _flpStaff.Controls.Cast<Control>().ToList();
            _flpStaff.Controls.Clear();
            foreach (Control c in old) c.Dispose();

            int roleTotal = _staff.Count(x => x.Role == _activeRole);
            var items = GetFilteredStaff();
            UpdateFilterStatus(items.Count, roleTotal);

            if (items.Count == 0)
            {
                _flpStaff.Controls.Add(new Label
                {
                    Text = roleTotal > 0
                        ? "Không tìm thấy nhân viên phù hợp với từ khóa / bộ lọc."
                        : "Chưa có nhân viên nào ở vị trí này.",
                    AutoSize = true,
                    ForeColor = StaffTheme.Dim,
                    Font = new Font("Segoe UI", 9F),
                    Margin = new Padding(4, 8, 0, 0)
                });
            }
            else
            {
                foreach (StaffInfo s in items)
                {
                    var card = new StaffCard(s);
                    card.Click += (o, ev) =>
                    {
                        StaffClicked?.Invoke(this, s);
                        using var dlg = new frmStaffDetail(s);
                        dlg.ShowDialog(this);
                        if (dlg.Tag?.ToString() == "UPDATED")
                        {
                            UpdateKpis();
                            SelectTab(_activeRole);
                        }
                    };
                    card.MouseWheel += Card_MouseWheel;
                    _flpStaff.Controls.Add(card);
                }
            }

            _flpStaff.ResumeLayout(true);
            _flpStaff.AutoScrollPosition = new Point(0, 0);
            ResizeCards();
        }

        // -----------------------------------------------------------------
        //  THANH TÌM KIẾM + BỘ LỌC
        // -----------------------------------------------------------------
        private Control BuildFilterBar()
        {
            _pnlFilter = new Panel
            {
                Dock = DockStyle.Top,
                Height = 58,
                BackColor = StaffTheme.CardBg
            };

            var tbl = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 5,
                RowCount = 1,
                BackColor = Color.Transparent,
                Margin = new Padding(0)
            };
            tbl.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));   // Ô tìm kiếm
            tbl.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 210F));  // Chi nhánh
            tbl.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 210F));  // Ca làm / Chứng chỉ
            tbl.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));        // Nút xóa lọc
            tbl.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 170F));  // Đếm kết quả
            tbl.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));

            // --- Ô tìm kiếm ---
            _pnlSearch = new Panel
            {
                Dock = DockStyle.Fill,
                Margin = new Padding(0, 11, 12, 11),
                BackColor = StaffTheme.ItemBg
            };

            _txtSearch = new TextBox
            {
                BorderStyle = BorderStyle.None,
                BackColor = StaffTheme.ItemBg,
                ForeColor = StaffTheme.Text,
                Font = new Font("Segoe UI", 9.5F),
                PlaceholderText = "Tìm theo tên hoặc số điện thoại..."
            };

            _btnClearSearch = new Label
            {
                Text = "✕",
                Size = new Size(24, 24),
                Font = new Font("Segoe UI", 8.5F, FontStyle.Bold),
                ForeColor = StaffTheme.Dim,
                BackColor = StaffTheme.ItemBg,
                TextAlign = ContentAlignment.MiddleCenter,
                Cursor = Cursors.Hand,
                Visible = false
            };
            _btnClearSearch.MouseEnter += (o, e) => _btnClearSearch.ForeColor = StaffTheme.Text;
            _btnClearSearch.MouseLeave += (o, e) => _btnClearSearch.ForeColor = StaffTheme.Dim;
            _btnClearSearch.Click += (o, e) => { _txtSearch.Clear(); _txtSearch.Focus(); };

            _pnlSearch.Controls.Add(_txtSearch);
            _pnlSearch.Controls.Add(_btnClearSearch);

            _pnlSearch.Resize += (o, e) => LayoutSearchBox();
            _pnlSearch.Click += (o, e) => _txtSearch.Focus();
            _pnlSearch.Paint += SearchBox_Paint;

            _txtSearch.Enter += (o, e) => { _searchFocused = true; _pnlSearch.Invalidate(); };
            _txtSearch.Leave += (o, e) => { _searchFocused = false; _pnlSearch.Invalidate(); };
            _txtSearch.TextChanged += TxtSearch_TextChanged;
            _txtSearch.KeyDown += (o, e) =>
            {
                if (e.KeyCode == Keys.Escape && _txtSearch.Text.Length > 0)
                {
                    _txtSearch.Clear();
                    e.SuppressKeyPress = true;
                }
            };

            // Chờ người dùng ngừng gõ ~250ms rồi mới lọc (đỡ giật khi gõ nhanh)
            _searchTimer = new System.Windows.Forms.Timer { Interval = 250 };
            _searchTimer.Tick += (o, e) =>
            {
                _searchTimer.Stop();
                RebuildCards();
            };

            // --- Dropdown ---
            _ddBranch = new FilterDropdown { Dock = DockStyle.Fill, Margin = new Padding(0, 11, 12, 11) };
            _ddBranch.SetItems(new[] { "Tất cả chi nhánh" });
            _ddBranch.SelectedIndexChanged += (o, e) => { if (!_filterLock) RebuildCards(); };

            _ddSecondary = new FilterDropdown { Dock = DockStyle.Fill, Margin = new Padding(0, 11, 12, 11) };
            _ddSecondary.SetItems(new[] { "Tất cả chứng chỉ" });
            _ddSecondary.SelectedIndexChanged += (o, e) => { if (!_filterLock) RebuildCards(); };

            // --- Nút xóa lọc ---
            _btnClearFilter = new Button
            {
                Text = "✕  Xóa lọc",
                Dock = DockStyle.Fill,
                Width = 104,
                Margin = new Padding(0, 11, 12, 11),
                FlatStyle = FlatStyle.Flat,
                BackColor = StaffTheme.ItemBg,
                ForeColor = StaffTheme.Muted,
                Font = new Font("Segoe UI", 8.5F, FontStyle.Bold),
                Cursor = Cursors.Hand,
                TabStop = false,
                UseVisualStyleBackColor = false,
                Visible = false
            };
            _btnClearFilter.FlatAppearance.BorderColor = StaffTheme.Border;
            _btnClearFilter.FlatAppearance.MouseOverBackColor = Color.FromArgb(39, 39, 42);
            _btnClearFilter.FlatAppearance.MouseDownBackColor = Color.FromArgb(39, 39, 42);
            _btnClearFilter.Click += (o, e) => ClearFilters(true);

            // --- Đếm kết quả ---
            _lblCount = new Label
            {
                Dock = DockStyle.Fill,
                Margin = new Padding(0, 0, 4, 0),
                Font = new Font("Segoe UI", 8F),
                ForeColor = StaffTheme.Dim,
                TextAlign = ContentAlignment.MiddleRight
            };

            tbl.Controls.Add(_pnlSearch, 0, 0);
            tbl.Controls.Add(_ddBranch, 1, 0);
            tbl.Controls.Add(_ddSecondary, 2, 0);
            tbl.Controls.Add(_btnClearFilter, 3, 0);
            tbl.Controls.Add(_lblCount, 4, 0);

            _pnlFilter.Controls.Add(tbl);
            return _pnlFilter;
        }

        private void LayoutSearchBox()
        {
            const int left = 36;    // chừa chỗ cho biểu tượng kính lúp
            const int right = 30;   // chừa chỗ cho nút ✕
            int h = _pnlSearch.Height;

            _txtSearch.Width = Math.Max(20, _pnlSearch.Width - left - right);
            _txtSearch.Location = new Point(left, Math.Max(0, (h - _txtSearch.Height) / 2));
            _btnClearSearch.Location = new Point(_pnlSearch.Width - 28, (h - _btnClearSearch.Height) / 2);
        }

        private void SearchBox_Paint(object? sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;

            using (var border = new Pen(_searchFocused ? StaffTheme.Orange : StaffTheme.Border))
                g.DrawRectangle(border, 0, 0, _pnlSearch.Width - 1, _pnlSearch.Height - 1);

            // Biểu tượng kính lúp vẽ bằng GDI+ (không phụ thuộc font)
            float cy = _pnlSearch.Height / 2f;
            using (var icon = new Pen(_searchFocused ? StaffTheme.Orange : StaffTheme.Muted, 1.6f))
            {
                g.DrawEllipse(icon, 12f, cy - 7f, 9f, 9f);
                g.DrawLine(icon, 19.5f, cy + 1.5f, 24f, cy + 6f);
            }
        }

        private void TxtSearch_TextChanged(object? sender, EventArgs e)
        {
            _btnClearSearch.Visible = _txtSearch.TextLength > 0;
            if (_filterLock) return;

            _searchTimer.Stop();
            _searchTimer.Start();
        }

        // -----------------------------------------------------------------
        //  LOGIC LỌC
        // -----------------------------------------------------------------
        /// <summary>Nạp lại danh sách chi nhánh + ca làm/chứng chỉ theo dữ liệu hiện có.</summary>
        private void RefreshFilterOptions(bool resetSecondary)
        {
            _filterLock = true;
            try
            {
                // Chi nhánh: lấy từ toàn bộ nhân viên, giữ nguyên lựa chọn khi đổi tab
                string prevBranch = _ddBranch.IsDefault ? "" : _ddBranch.SelectedText;
                var branches = _staff.Select(x => x.Branch)
                                     .Where(b => !string.IsNullOrWhiteSpace(b))
                                     .Distinct(StringComparer.OrdinalIgnoreCase)
                                     .OrderBy(b => b, StringComparer.CurrentCultureIgnoreCase)
                                     .ToList();
                var branchItems = new List<string> { "Tất cả chi nhánh" };
                branchItems.AddRange(branches);
                int bi = prevBranch.Length == 0 ? 0 : branchItems.FindIndex(b => string.Equals(b, prevBranch, StringComparison.OrdinalIgnoreCase));
                _ddBranch.SetItems(branchItems, bi < 0 ? 0 : bi);

                // Ô lọc thứ hai: PT => chứng chỉ, vai trò khác => ca làm việc
                string prevSecondary = (resetSecondary || _ddSecondary.IsDefault) ? "" : _ddSecondary.SelectedText;
                var inRole = _staff.Where(x => x.Role == _activeRole).ToList();
                var secondaryItems = new List<string>();
                if (_activeRole == StaffRole.PT)
                {
                    secondaryItems.Add("Tất cả chứng chỉ");
                    secondaryItems.AddRange(inRole.SelectMany(x => GetCertBrands(x))
                                                  .Distinct(StringComparer.OrdinalIgnoreCase)
                                                  .OrderBy(c => c, StringComparer.CurrentCultureIgnoreCase));
                }
                else
                {
                    secondaryItems.Add("Tất cả ca làm");
                    secondaryItems.AddRange(inRole.Select(x => GetShiftName(x))
                                                  .Where(n => n.Length > 0)
                                                  .Distinct(StringComparer.OrdinalIgnoreCase)
                                                  .OrderBy(n =>
                                                  {
                                                      int k = Array.IndexOf(ShiftOrder, Fold(n));
                                                      return k < 0 ? 99 : k;
                                                  })
                                                  .ThenBy(n => n, StringComparer.CurrentCultureIgnoreCase));
                }
                int si = prevSecondary.Length == 0 ? 0 : secondaryItems.FindIndex(n => string.Equals(n, prevSecondary, StringComparison.OrdinalIgnoreCase));
                _ddSecondary.SetItems(secondaryItems, si < 0 ? 0 : si);
            }
            finally
            {
                _filterLock = false;
            }
        }

        private List<StaffInfo> GetFilteredStaff()
        {
            string keyword = Fold(_txtSearch.Text.Trim());
            string keywordDigits = DigitsOnly(_txtSearch.Text);
            string? branch = _ddBranch.IsDefault ? null : _ddBranch.SelectedText;
            string? secondary = _ddSecondary.IsDefault ? null : _ddSecondary.SelectedText;

            return _staff
                .Where(x => x.Role == _activeRole)
                .Where(x => branch == null || string.Equals(x.Branch, branch, StringComparison.OrdinalIgnoreCase))
                .Where(x => secondary == null || MatchSecondary(x, secondary))
                .Where(x => keyword.Length == 0
                         || Fold(x.Name).Contains(keyword)
                         || (keywordDigits.Length > 0 && DigitsOnly(x.Phone).Contains(keywordDigits)))
                .ToList();
        }

        private static bool MatchSecondary(StaffInfo s, string value)
        {
            if (s.Role == StaffRole.PT)
                return GetCertBrands(s).Contains(value, StringComparer.OrdinalIgnoreCase);
            return string.Equals(GetShiftName(s), value, StringComparison.OrdinalIgnoreCase);
        }

        /// <summary>"Ca sáng (06:00 - 14:00)" => "Ca sáng".</summary>
        private static string GetShiftName(StaffInfo s)
        {
            string e = s.Extra ?? "";
            int i = e.IndexOf('(');
            return (i > 0 ? e.Substring(0, i) : e).Trim();
        }

        /// <summary>"ACE CPT, NASM" => { "ACE", "NASM" }.</summary>
        private static IEnumerable<string> GetCertBrands(StaffInfo s)
        {
            return (s.Extra ?? "")
                .Split(new[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries)
                .Select(p => p.Trim())
                .Where(p => p.Length > 0)
                .Select(p => p.Split(' ')[0].ToUpperInvariant())
                .Distinct();
        }

        /// <summary>Bỏ dấu + chữ thường, để gõ "vinh" vẫn tìm ra "Vĩnh".</summary>
        private static string Fold(string text)
        {
            string normalized = (text ?? "").Normalize(System.Text.NormalizationForm.FormD);
            var sb = new System.Text.StringBuilder(normalized.Length);
            foreach (char c in normalized)
            {
                if (CharUnicodeInfo.GetUnicodeCategory(c) != UnicodeCategory.NonSpacingMark)
                    sb.Append(c);
            }
            return sb.ToString().ToLowerInvariant().Replace('đ', 'd');
        }

        private static string DigitsOnly(string? text)
        {
            return new string((text ?? "").Where(char.IsDigit).ToArray());
        }

        private bool HasActiveFilter =>
            _txtSearch.TextLength > 0 || !_ddBranch.IsDefault || !_ddSecondary.IsDefault;

        private void UpdateFilterStatus(int shown, int total)
        {
            _lblCount.Text = $"Hiển thị {shown}/{total} nhân viên";
            _btnClearFilter.Visible = HasActiveFilter;
        }

        /// <summary>Xóa từ khóa và đưa các ô lọc về "Tất cả".</summary>
        public void ClearFilters(bool rebuild = true)
        {
            _filterLock = true;
            try
            {
                _searchTimer.Stop();
                _txtSearch.Clear();
                _ddBranch.SelectedIndex = 0;
                _ddSecondary.SelectedIndex = 0;
            }
            finally
            {
                _filterLock = false;
            }

            if (rebuild) RebuildCards();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing) _searchTimer?.Dispose();
            base.Dispose(disposing);
        }

        private void Card_MouseWheel(object? sender, MouseEventArgs e)
        {
            // Cho phép lăn chuột khi đang trỏ trên thẻ
            _flpStaff.AutoScrollPosition = new Point(0, -_flpStaff.AutoScrollPosition.Y - e.Delta);
        }

        /// <summary>Chia đều chiều rộng thẻ theo số cột (mặc định ~3 cột trên màn hình rộng).</summary>
        private void ResizeCards()
        {
            if (_flpStaff == null || _flpStaff.Controls.Count == 0) return;

            const int gap = 14;
            int avail = _flpStaff.ClientSize.Width - _flpStaff.Padding.Horizontal
                        - SystemInformation.VerticalScrollBarWidth;
            if (avail <= 0) return;

            int cols = Math.Max(1, avail / 400);
            int w = Math.Max(300, avail / cols - gap);

            _flpStaff.SuspendLayout();
            foreach (Control c in _flpStaff.Controls)
            {
                if (c is StaffCard && c.Width != w) c.Width = w;
            }
            _flpStaff.ResumeLayout(true);
        }

        // -----------------------------------------------------------------
        //  Dữ liệu mẫu (chỉ để xem giao diện – xoá khi nối CSDL)
        // -----------------------------------------------------------------
        private void LoadSampleData()
        {
            var data = new List<StaffInfo>
            {
                // --- PT ---
                new StaffInfo
                {
                    Role = StaffRole.PT, Name = "Lê Công Vinh", Phone = "0918.765.432", Branch = "Hoàn Kiếm",
                    Extra = "ACE CPT, NASM", Rating = 4.9, Students = 24, Salary = 15000000,
                    Username = "pt_congvinh", Password = "VinhPT@2026", Email = "congvinh.pt@gymfit.vn",
                    AttendanceDays = 24, StandardDays = 26, LeaveDays = 1, OvertimeHours = 8.0
                },
                new StaffInfo
                {
                    Role = StaffRole.PT, Name = "Trần Thị Bích", Phone = "0943.210.987", Branch = "Quận 1",
                    Extra = "ISSA CPT", Rating = 4.8, Students = 18, Salary = 14500000,
                    Username = "pt_bichtran", Password = "BichTran@123", Email = "bichtran.pt@gymfit.vn",
                    AttendanceDays = 25, StandardDays = 26, LeaveDays = 0, OvertimeHours = 12.0
                },
                new StaffInfo
                {
                    Role = StaffRole.PT, Name = "Nguyễn Đức Minh", Phone = "0987.543.210", Branch = "Cầu Giấy",
                    Extra = "ACE CPT", Rating = 4.7, Students = 15, Salary = 13000000,
                    Username = "pt_ducminh", Password = "MinhGym@2026", Email = "ducminh.pt@gymfit.vn",
                    AttendanceDays = 23, StandardDays = 26, LeaveDays = 2, OvertimeHours = 6.0
                },
                new StaffInfo
                {
                    Role = StaffRole.PT, Name = "Phan Thị Thảo", Phone = "0921.098.765", Branch = "Bình Thạnh",
                    Extra = "NASM CPT", Rating = 4.6, Students = 12, Salary = 12500000,
                    Username = "pt_thaophan", Password = "ThaoPT@2026", Email = "thaophan.pt@gymfit.vn",
                    AttendanceDays = 24, StandardDays = 26, LeaveDays = 1, OvertimeHours = 4.5
                },
                new StaffInfo
                {
                    Role = StaffRole.PT, Name = "Vũ Hoàng Long", Phone = "0909.876.543", Branch = "Hải Châu",
                    Extra = "ACE CPT", Rating = 4.5, Students = 9, Salary = 12000000,
                    Username = "pt_hoanglong", Password = "LongGym@123", Email = "hoanglong.pt@gymfit.vn",
                    AttendanceDays = 22, StandardDays = 26, LeaveDays = 2, OvertimeHours = 2.0
                },
 
                // --- Thu ngân ---
                new StaffInfo
                {
                    Role = StaffRole.ThuNgan, Name = "Nguyễn Thị Lan", Phone = "0912.345.678", Branch = "Quận 1",
                    Extra = "Ca sáng (06:00 - 14:00)", JoinDate = "03/2023", Salary = 8500000,
                    Username = "tn_lannguyen", Password = "LanCashier@2026", Email = "lan.thungan@gymfit.vn",
                    AttendanceDays = 26, StandardDays = 26, LeaveDays = 0, OvertimeHours = 5.0
                },
                new StaffInfo
                {
                    Role = StaffRole.ThuNgan, Name = "Trần Văn Hùng", Phone = "0934.567.890", Branch = "Hoàn Kiếm",
                    Extra = "Ca chiều (14:00 - 22:00)", JoinDate = "07/2023", Salary = 8200000,
                    Username = "tn_hungtran", Password = "HungCashier@123", Email = "hung.thungan@gymfit.vn",
                    AttendanceDays = 25, StandardDays = 26, LeaveDays = 1, OvertimeHours = 3.0
                },
                new StaffInfo
                {
                    Role = StaffRole.ThuNgan, Name = "Phạm Thu Hà", Phone = "0978.123.456", Branch = "Cầu Giấy",
                    Extra = "Ca sáng (06:00 - 14:00)", JoinDate = "01/2024", Salary = 8000000,
                    Username = "tn_hatham", Password = "HaThuNgan@2026", Email = "ha.thungan@gymfit.vn",
                    AttendanceDays = 24, StandardDays = 26, LeaveDays = 1, OvertimeHours = 4.0
                },
                new StaffInfo
                {
                    Role = StaffRole.ThuNgan, Name = "Đặng Minh Châu", Phone = "0966.222.333", Branch = "Bình Thạnh",
                    Extra = "Ca chiều (14:00 - 22:00)", JoinDate = "05/2024", Salary = 8000000,
                    Username = "tn_chaudang", Password = "ChauTN@123", Email = "chau.thungan@gymfit.vn",
                    AttendanceDays = 25, StandardDays = 26, LeaveDays = 0, OvertimeHours = 6.0
                },
 
                // --- Tạp vụ ---
                new StaffInfo
                {
                    Role = StaffRole.TapVu, Name = "Lê Thị Mai", Phone = "0905.111.222", Branch = "Quận 1",
                    Extra = "Ca sáng (05:00 - 13:00)", JoinDate = "02/2023", Salary = 6500000,
                    Username = "tv_maile", Password = "MaiTV@2026", Email = "mai.tapvu@gymfit.vn",
                    AttendanceDays = 26, StandardDays = 26, LeaveDays = 0, OvertimeHours = 4.0
                },
                new StaffInfo
                {
                    Role = StaffRole.TapVu, Name = "Nguyễn Thị Sáu", Phone = "0932.333.444", Branch = "Hải Châu",
                    Extra = "Ca chiều (13:00 - 21:00)", JoinDate = "08/2023", Salary = 6300000,
                    Username = "tv_saunguyen", Password = "SauTV@123", Email = "sau.tapvu@gymfit.vn",
                    AttendanceDays = 24, StandardDays = 26, LeaveDays = 2, OvertimeHours = 2.0
                },
                new StaffInfo
                {
                    Role = StaffRole.TapVu, Name = "Hoàng Thị Nga", Phone = "0988.444.555", Branch = "Cầu Giấy",
                    Extra = "Ca sáng (05:00 - 13:00)", JoinDate = "11/2024", Salary = 6300000,
                    Username = "tv_ngahoang", Password = "NgaTV@2026", Email = "nga.tapvu@gymfit.vn",
                    AttendanceDays = 25, StandardDays = 26, LeaveDays = 1, OvertimeHours = 3.0
                },
 
                // --- Bảo vệ ---
                new StaffInfo
                {
                    Role = StaffRole.BaoVe, Name = "Bùi Văn Dũng", Phone = "0911.777.888", Branch = "Quận 1",
                    Extra = "Ca đêm (22:00 - 06:00)", JoinDate = "01/2023", Salary = 7500000,
                    Username = "bv_dungbui", Password = "DungBV@2026", Email = "dung.baove@gymfit.vn",
                    AttendanceDays = 26, StandardDays = 26, LeaveDays = 0, OvertimeHours = 10.0
                },
                new StaffInfo
                {
                    Role = StaffRole.BaoVe, Name = "Ngô Quang Hải", Phone = "0944.888.999", Branch = "Hoàn Kiếm",
                    Extra = "Ca ngày (06:00 - 18:00)", JoinDate = "06/2023", Salary = 7200000,
                    Username = "bv_haingo", Password = "HaiBV@123", Email = "hai.baove@gymfit.vn",
                    AttendanceDays = 25, StandardDays = 26, LeaveDays = 1, OvertimeHours = 8.0
                },
                new StaffInfo
                {
                    Role = StaffRole.BaoVe, Name = "Đỗ Văn Tuấn", Phone = "0977.999.000", Branch = "Bình Thạnh",
                    Extra = "Ca đêm (22:00 - 06:00)", JoinDate = "09/2024", Salary = 7200000,
                    Username = "bv_tuando", Password = "TuanBV@2026", Email = "tuan.baove@gymfit.vn",
                    AttendanceDays = 24, StandardDays = 26, LeaveDays = 1, OvertimeHours = 6.0
                },
            };

            SetStaff(data);
        }
    }
}