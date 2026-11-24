using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace QuanLyGym.UI.Forms.Admin
{
    public class frmMemberDetail : Form
    {
        [DllImport("user32.dll")]
        public static extern bool ReleaseCapture();
        [DllImport("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);
        private const int WM_NCLBUTTONDOWN = 0xA1;
        private const int HTCAPTION = 0x2;

        private static readonly Color BgColor = Color.FromArgb(15, 15, 17);
        private static readonly Color CardBg = Color.FromArgb(24, 24, 27);
        private static readonly Color ItemBg = Color.FromArgb(20, 20, 23);
        private static readonly Color Border = Color.FromArgb(45, 45, 50);
        private static readonly Color TextColor = Color.FromArgb(244, 244, 245);
        private static readonly Color MutedColor = Color.FromArgb(161, 161, 170);
        private static readonly Color Orange = Color.FromArgb(249, 115, 22);
        private static readonly Color Green = Color.FromArgb(34, 197, 94);
        private static readonly Color Blue = Color.FromArgb(59, 130, 246);
        private static readonly Color Cyan = Color.FromArgb(6, 182, 212);
        private static readonly Color Gold = Color.FromArgb(234, 179, 8);
        private static readonly Color Purple = Color.FromArgb(168, 85, 247);
        private static readonly CultureInfo ViCulture = new CultureInfo("vi-VN");

        private readonly ucAdminMember.MemberItem _member;
        public bool DataChanged { get; private set; }

        public frmMemberDetail(ucAdminMember.MemberItem member)
        {
            _member = member;
            BuildUI();
        }

        private void BuildUI()
        {
            FormBorderStyle = FormBorderStyle.None;
            StartPosition = FormStartPosition.CenterParent;
            Size = new Size(840, 696);
            BackColor = BgColor;
            ShowInTaskbar = false;
            DoubleBuffered = true;
            KeyPreview = true;

            KeyDown += (s, e) =>
            {
                if (e.KeyCode == Keys.Escape)
                {
                    this.Close();
                    e.Handled = true;
                }
            };

            Paint += (s, e) =>
            {
                using var p = new Pen(Border, 1.5f);
                e.Graphics.DrawRectangle(p, 0, 0, Width - 1, Height - 1);
            };

            // 1. Title bar
            var pnlTitle = new Panel
            {
                Dock = DockStyle.Top,
                Height = 56,
                BackColor = ItemBg,
                Padding = new Padding(16, 0, 16, 0)
            };
            pnlTitle.MouseDown += (s, e) =>
            {
                if (e.Button == MouseButtons.Left)
                {
                    ReleaseCapture();
                    SendMessage(Handle, WM_NCLBUTTONDOWN, HTCAPTION, 0);
                }
            };
            pnlTitle.Paint += (s, e) =>
            {
                using var p = new Pen(Border);
                e.Graphics.DrawLine(p, 0, pnlTitle.Height - 1, pnlTitle.Width, pnlTitle.Height - 1);
            };

            var lblTitle = new Label
            {
                Text = "📋  HỒ SƠ HỘI VIÊN & QUẢN LÝ THỂ TRẠNG",
                Font = new Font("Segoe UI", 12F, FontStyle.Bold),
                ForeColor = TextColor,
                AutoSize = true,
                Location = new Point(16, 10)
            };

            var lblSub = new Label
            {
                Text = $"Mã hội viên: {_member.Code} • Ngày đăng ký: {_member.RegisterDate:dd/MM/yyyy}",
                Font = new Font("Segoe UI", 8.5F),
                ForeColor = MutedColor,
                AutoSize = true,
                Location = new Point(18, 33)
            };

            var btnClose = new Button
            {
                Text = "✕",
                Font = new Font("Segoe UI", 12F, FontStyle.Bold),
                ForeColor = MutedColor,
                BackColor = Color.Transparent,
                FlatStyle = FlatStyle.Flat,
                Size = new Size(38, 38),
                Location = new Point(840 - 16 - 38, 9),
                Cursor = Cursors.Hand
            };
            btnClose.FlatAppearance.BorderSize = 0;
            btnClose.MouseEnter += (s, e) =>
            {
                btnClose.BackColor = Color.FromArgb(239, 68, 68);
                btnClose.ForeColor = Color.White;
            };
            btnClose.MouseLeave += (s, e) =>
            {
                btnClose.BackColor = Color.Transparent;
                btnClose.ForeColor = MutedColor;
            };
            btnClose.Click += (s, e) => this.Close();

            pnlTitle.Resize += (s, e) =>
            {
                btnClose.Location = new Point(pnlTitle.ClientSize.Width - 16 - btnClose.Width, (pnlTitle.Height - btnClose.Height) / 2);
            };

            pnlTitle.Controls.Add(lblTitle);
            pnlTitle.Controls.Add(lblSub);
            pnlTitle.Controls.Add(btnClose);

            // 2. Bottom Toolbar with Fast Actions
            var pnlBottom = new Panel
            {
                Dock = DockStyle.Bottom,
                Height = 58,
                BackColor = ItemBg,
                Padding = new Padding(16, 10, 16, 10)
            };
            pnlBottom.Paint += (s, e) =>
            {
                using var p = new Pen(Border);
                e.Graphics.DrawLine(p, 0, 0, pnlBottom.Width, 0);
            };

            var btnUpgrade = new Button
            {
                Text = "⚡  Nâng cấp / Gia hạn thẻ",
                Font = new Font("Segoe UI", 9F, FontStyle.Bold),
                ForeColor = Color.White,
                BackColor = Orange,
                Size = new Size(200, 38),
                Dock = DockStyle.Right,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            btnUpgrade.FlatAppearance.BorderSize = 0;
            btnUpgrade.Click += (s, e) =>
            {
                using var dlg = new frmMemberUpgrade(_member);
                if (dlg.ShowDialog(this) == DialogResult.OK && dlg.IsUpdated)
                {
                    DataChanged = true;
                    // Refresh view
                    Controls.Clear();
                    BuildUI();
                }
            };

            var btnEdit = new Button
            {
                Text = "✏️ Sửa thông tin",
                Font = new Font("Segoe UI", 9F, FontStyle.Bold),
                ForeColor = TextColor,
                BackColor = CardBg,
                Size = new Size(140, 38),
                Dock = DockStyle.Right,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            btnEdit.FlatAppearance.BorderColor = Border;
            btnEdit.Click += (s, e) =>
            {
                using var dlg = new frmMemberEdit(_member);
                if (dlg.ShowDialog(this) == DialogResult.OK)
                {
                    DataChanged = true;
                    Controls.Clear();
                    BuildUI();
                }
            };

            var btnCloseBottom = new Button
            {
                Text = "✕  Đóng",
                Font = new Font("Segoe UI", 9F, FontStyle.Bold),
                ForeColor = MutedColor,
                BackColor = CardBg,
                Size = new Size(95, 38),
                Dock = DockStyle.Right,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            btnCloseBottom.FlatAppearance.BorderColor = Border;
            btnCloseBottom.MouseEnter += (s, e) =>
            {
                btnCloseBottom.ForeColor = TextColor;
                btnCloseBottom.BackColor = Color.FromArgb(40, 40, 45);
            };
            btnCloseBottom.MouseLeave += (s, e) =>
            {
                btnCloseBottom.ForeColor = MutedColor;
                btnCloseBottom.BackColor = CardBg;
            };
            btnCloseBottom.Click += (s, e) => this.Close();

            var btnSuspend = new Button
            {
                Text = _member._isSuspended ? "🔓 Mở khóa thẻ" : "🔒 Khóa / Bảo lưu thẻ",
                Font = new Font("Segoe UI", 8.5F, FontStyle.Bold),
                ForeColor = _member._isSuspended ? Green : Color.FromArgb(239, 68, 68),
                BackColor = Color.FromArgb(40, 20, 20),
                Size = new Size(160, 38),
                Dock = DockStyle.Left,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            btnSuspend.FlatAppearance.BorderColor = _member._isSuspended ? Green : Color.FromArgb(239, 68, 68);
            btnSuspend.Click += (s, e) =>
            {
                string action = _member._isSuspended ? "mở khóa thẻ" : "tạm khóa / bảo lưu thẻ";
                if (MessageBox.Show($"Bạn có chắc chắn muốn {action} cho hội viên '{_member.FullName}'?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    _member._isSuspended = !_member._isSuspended;
                    DataChanged = true;
                    Controls.Clear();
                    BuildUI();
                }
            };

            var pnlGap = new Panel { Dock = DockStyle.Right, Width = 10, BackColor = Color.Transparent };
            var pnlGap2 = new Panel { Dock = DockStyle.Right, Width = 10, BackColor = Color.Transparent };

            pnlBottom.Controls.Add(btnSuspend);
            pnlBottom.Controls.Add(btnCloseBottom);
            pnlBottom.Controls.Add(pnlGap2);
            pnlBottom.Controls.Add(btnEdit);
            pnlBottom.Controls.Add(pnlGap);
            pnlBottom.Controls.Add(btnUpgrade);

            // 3. Scrollable Main Content
            var pnlBody = new Panel
            {
                Dock = DockStyle.Fill,
                AutoScroll = true,
                BackColor = BgColor
            };

            int marginX = 16;
            int cardWidth = 840 - (marginX * 2); // 808px
            int y = 14;

            // Header Profile Card
            var pnlProfile = BuildProfileHeaderCard(cardWidth);
            pnlProfile.Location = new Point(marginX, y);
            pnlBody.Controls.Add(pnlProfile);
            y += pnlProfile.Height + 10;

            // Membership Validity & Progress Card
            var pnlValidity = BuildValidityCard(cardWidth);
            pnlValidity.Location = new Point(marginX, y);
            pnlBody.Controls.Add(pnlValidity);
            y += pnlValidity.Height + 10;

            // InBody Metrics Strip (5 metric mini-cards)
            var pnlInBody = BuildInBodyCard(cardWidth);
            pnlInBody.Location = new Point(marginX, y);
            pnlBody.Controls.Add(pnlInBody);
            y += pnlInBody.Height + 10;

            // Two columns: Left = Training & Check-in Log, Right = Health & Plan Notes
            var pnlColumns = BuildTwoColumnSection(cardWidth);
            pnlColumns.Location = new Point(marginX, y);
            pnlBody.Controls.Add(pnlColumns);

            Controls.Add(pnlBody);
            Controls.Add(pnlBottom);
            Controls.Add(pnlTitle);
        }

        private Panel BuildProfileHeaderCard(int cardWidth)
        {
            var pnl = new Panel
            {
                Size = new Size(cardWidth, 114),
                BackColor = CardBg,
                Padding = new Padding(16, 12, 16, 12)
            };
            pnl.Paint += (s, e) =>
            {
                using var p = new Pen(Border);
                e.Graphics.DrawRectangle(p, 0, 0, pnl.Width - 1, pnl.Height - 1);
            };

            // Avatar circle with initials
            var lblAvatar = new Label
            {
                Size = new Size(76, 76),
                Location = new Point(16, 18),
                BackColor = GetTierColor(_member.Tier),
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 18F, FontStyle.Bold),
                TextAlign = ContentAlignment.MiddleCenter,
                Text = GetInitials(_member.FullName)
            };
            using (var path = new GraphicsPath())
            {
                path.AddEllipse(0, 0, lblAvatar.Width, lblAvatar.Height);
                lblAvatar.Region = new Region(path);
            }
            pnl.Controls.Add(lblAvatar);

            // Name + Badges
            var flpTop = new FlowLayoutPanel
            {
                Location = new Point(106, 14),
                Size = new Size(cardWidth - 116, 32),
                FlowDirection = FlowDirection.LeftToRight,
                WrapContents = false,
                BackColor = Color.Transparent
            };

            var lblName = new Label
            {
                Text = _member.FullName,
                Font = new Font("Segoe UI", 14F, FontStyle.Bold),
                ForeColor = TextColor,
                AutoSize = true
            };

            var lblTierBadge = new Label
            {
                Text = $"💎 {_member.Tier}",
                Font = new Font("Segoe UI", 8F, FontStyle.Bold),
                ForeColor = GetTierColor(_member.Tier),
                BackColor = Color.FromArgb(30, GetTierColor(_member.Tier).R, GetTierColor(_member.Tier).G, GetTierColor(_member.Tier).B),
                Padding = new Padding(8, 3, 8, 3),
                Margin = new Padding(10, 2, 8, 0),
                AutoSize = true
            };

            Color statusColor = _member._isSuspended ? Color.FromArgb(161, 161, 170) : (_member.DaysRemaining > 15 ? Green : (_member.DaysRemaining > 0 ? Orange : Color.FromArgb(239, 68, 68)));
            string statusText = _member._isSuspended ? "⚪ BẢO LƯU / TẠM KHÓA" : (_member.DaysRemaining > 15 ? "🟢 ĐANG HOẠT ĐỘNG" : (_member.DaysRemaining > 0 ? "⏳ SẮP HẾT HẠN" : "🔴 ĐÃ HẾT HẠN"));

            var lblStatusBadge = new Label
            {
                Text = statusText,
                Font = new Font("Segoe UI", 8F, FontStyle.Bold),
                ForeColor = statusColor,
                BackColor = Color.FromArgb(30, statusColor.R, statusColor.G, statusColor.B),
                Padding = new Padding(8, 3, 8, 3),
                Margin = new Padding(0, 2, 0, 0),
                AutoSize = true
            };

            flpTop.Controls.Add(lblName);
            flpTop.Controls.Add(lblTierBadge);
            flpTop.Controls.Add(lblStatusBadge);
            pnl.Controls.Add(flpTop);

            // Secondary Info row 1: Hotline & Email
            var lblInfo1 = new Label
            {
                Text = $"📞 Hotline: {_member.Phone}   •   ✉️ Email: {_member.Email}",
                Font = new Font("Segoe UI", 8.5F),
                ForeColor = MutedColor,
                Location = new Point(108, 52),
                AutoSize = true
            };
            pnl.Controls.Add(lblInfo1);

            // Secondary Info row 2: Gender/Age & Branch & PT
            int age = DateTime.Today.Year - _member.DateOfBirth.Year;
            var lblInfo2 = new Label
            {
                Text = $"🚻 Giới tính: {_member.Gender} ({age} tuổi)   •   🏢 Cơ sở: {_member.BranchName}   •   👔 HLV: {_member.AssignedPt}",
                Font = new Font("Segoe UI", 8.5F),
                ForeColor = Orange,
                Location = new Point(108, 76),
                AutoSize = true
            };
            pnl.Controls.Add(lblInfo2);

            return pnl;
        }

        private Panel BuildValidityCard(int cardWidth)
        {
            var pnl = new Panel
            {
                Size = new Size(cardWidth, 96),
                BackColor = ItemBg,
                Padding = new Padding(16, 12, 16, 12)
            };
            pnl.Paint += (s, e) =>
            {
                using var p = new Pen(Border);
                e.Graphics.DrawRectangle(p, 0, 0, pnl.Width - 1, pnl.Height - 1);
            };

            int totalDays = Math.Max(1, (int)(_member.EndDate - _member.StartDate).TotalDays);
            int usedDays = Math.Max(0, (int)(DateTime.Today - _member.StartDate).TotalDays);
            int percent = Math.Min(100, Math.Max(0, (int)((usedDays / (float)totalDays) * 100)));
            int remainDays = _member.DaysRemaining;

            var lblT = new Label
            {
                Text = "HẠN SỬ DỤNG GÓI TẬP HIỆN TẠI",
                Font = new Font("Segoe UI", 8.5F, FontStyle.Bold),
                ForeColor = TextColor,
                Location = new Point(14, 12),
                AutoSize = true
            };

            string remainStr = remainDays > 0 ? $"Còn {remainDays} ngày" : "Đã hết hạn";
            var lblDays = new Label
            {
                Text = remainStr,
                Font = new Font("Segoe UI", 12F, FontStyle.Bold),
                ForeColor = remainDays > 15 ? Green : (remainDays > 0 ? Orange : Color.FromArgb(239, 68, 68)),
                Location = new Point(cardWidth - 214, 10),
                Size = new Size(200, 24),
                TextAlign = ContentAlignment.MiddleRight
            };

            pnl.Controls.Add(lblT);
            pnl.Controls.Add(lblDays);

            // Progress bar
            int barW = cardWidth - 28;
            var pnlProgressBg = new Panel
            {
                Location = new Point(14, 40),
                Size = new Size(barW, 10),
                BackColor = Color.FromArgb(39, 39, 42)
            };
            var pnlProgressFill = new Panel
            {
                Dock = DockStyle.Left,
                Width = (int)(barW * (percent / 100f)),
                BackColor = remainDays > 15 ? Blue : (remainDays > 0 ? Orange : Color.FromArgb(239, 68, 68))
            };
            pnlProgressBg.Controls.Add(pnlProgressFill);
            pnl.Controls.Add(pnlProgressBg);

            var lblDates = new Label
            {
                Text = $"Bắt đầu: {_member.StartDate:dd/MM/yyyy}   ➔   Hết hạn: {_member.EndDate:dd/MM/yyyy}   (Đã tập {usedDays}/{totalDays} ngày)",
                Font = new Font("Segoe UI", 8F),
                ForeColor = MutedColor,
                Location = new Point(14, 58),
                AutoSize = true
            };
            pnl.Controls.Add(lblDates);

            return pnl;
        }

        private Panel BuildInBodyCard(int cardWidth)
        {
            var pnl = new Panel
            {
                Size = new Size(cardWidth, 126),
                BackColor = CardBg
            };
            pnl.Paint += (s, e) =>
            {
                using var p = new Pen(Border);
                e.Graphics.DrawRectangle(p, 0, 0, pnl.Width - 1, pnl.Height - 1);
            };

            var lblHeader = new Label
            {
                Text = "📊  CHỈ SỐ THỂ CHẤT INBODY (ĐO GẦN NHẤT)",
                Font = new Font("Segoe UI", 8.5F, FontStyle.Bold),
                ForeColor = Orange,
                Location = new Point(14, 10),
                Size = new Size(cardWidth - 28, 20),
                TextAlign = ContentAlignment.MiddleLeft
            };
            pnl.Controls.Add(lblHeader);

            var tbl = new TableLayoutPanel
            {
                Location = new Point(12, 34),
                Size = new Size(cardWidth - 24, 80),
                ColumnCount = 5,
                RowCount = 1,
                BackColor = Color.Transparent
            };
            for (int i = 0; i < 5; i++) tbl.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20F));

            tbl.Controls.Add(CreateMetricItem("CHIỀU CAO", $"{_member.HeightCm:0.#} cm", Blue), 0, 0);
            tbl.Controls.Add(CreateMetricItem("CÂN NẶNG", $"{_member.WeightKg:0.#} kg", Cyan), 1, 0);
            tbl.Controls.Add(CreateMetricItem("CHỈ SỐ BMI", $"{_member.Bmi:0.#}", _member.Bmi > 25 ? Orange : Green), 2, 0);
            tbl.Controls.Add(CreateMetricItem("TỶ LỆ MỠ", $"{_member.BodyFatPercent:0.#} %", Gold), 3, 0);
            tbl.Controls.Add(CreateMetricItem("KHỐI LƯỢNG CƠ", $"{_member.MuscleMassKg:0.#} kg", Purple), 4, 0);

            pnl.Controls.Add(tbl);
            return pnl;
        }

        private Control CreateMetricItem(string label, string val, Color accent)
        {
            var pnl = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = ItemBg,
                Margin = new Padding(3, 0, 3, 0)
            };
            pnl.Paint += (s, e) =>
            {
                using var p = new Pen(Border);
                e.Graphics.DrawRectangle(p, 0, 0, pnl.Width - 1, pnl.Height - 1);
            };

            var lblT = new Label
            {
                Text = label,
                Font = new Font("Segoe UI", 7.5F, FontStyle.Bold),
                ForeColor = MutedColor,
                Dock = DockStyle.Top,
                Height = 24,
                Padding = new Padding(10, 6, 0, 0),
                TextAlign = ContentAlignment.MiddleLeft,
                UseMnemonic = false
            };
            var lblV = new Label
            {
                Text = val,
                Font = new Font("Segoe UI", 13.5F, FontStyle.Bold),
                ForeColor = accent,
                Dock = DockStyle.Fill,
                Padding = new Padding(10, 0, 0, 8),
                TextAlign = ContentAlignment.MiddleLeft,
                UseMnemonic = false
            };

            pnl.Controls.Add(lblV);
            pnl.Controls.Add(lblT);
            return pnl;
        }

        private Panel BuildTwoColumnSection(int cardWidth)
        {
            int colW = (cardWidth - 14) / 2;
            var pnl = new Panel { Size = new Size(cardWidth, 188), BackColor = Color.Transparent };

            // Cột trái: Lịch sử Check-in gần nhất
            var pnlCheckIn = new Panel
            {
                Location = new Point(0, 0),
                Size = new Size(colW, 188),
                BackColor = CardBg,
                Padding = new Padding(14, 10, 14, 10)
            };
            pnlCheckIn.Paint += (s, e) =>
            {
                using var p = new Pen(Border);
                e.Graphics.DrawRectangle(p, 0, 0, pnlCheckIn.Width - 1, pnlCheckIn.Height - 1);
            };

            var lblCiTitle = new Label
            {
                Text = $"📅  LỊCH SỬ CHECK-IN ({_member.TotalCheckIns} buổi đã tập)",
                Font = new Font("Segoe UI", 8.5F, FontStyle.Bold),
                ForeColor = TextColor,
                Dock = DockStyle.Top,
                Height = 26
            };
            pnlCheckIn.Controls.Add(lblCiTitle);

            var lv = new ListView
            {
                Dock = DockStyle.Fill,
                View = System.Windows.Forms.View.Details,
                FullRowSelect = true,
                BackColor = ItemBg,
                ForeColor = TextColor,
                BorderStyle = BorderStyle.None,
                HeaderStyle = ColumnHeaderStyle.Nonclickable
            };
            lv.Columns.Add("Thời gian", 130);
            lv.Columns.Add("Chi nhánh", 140);
            lv.Columns.Add("Thời lượng", 80);

            // 4 check-in mẫu
            lv.Items.Add(new ListViewItem(new[] { DateTime.Today.AddDays(-1).ToString("dd/MM HH:mm"), "Quận 1", "75 phút" }));
            lv.Items.Add(new ListViewItem(new[] { DateTime.Today.AddDays(-3).ToString("dd/MM HH:mm"), "Quận 1", "90 phút" }));
            lv.Items.Add(new ListViewItem(new[] { DateTime.Today.AddDays(-5).ToString("dd/MM HH:mm"), "Quận 1", "60 phút" }));
            lv.Items.Add(new ListViewItem(new[] { DateTime.Today.AddDays(-8).ToString("dd/MM HH:mm"), "Bình Thạnh", "80 phút" }));

            pnlCheckIn.Controls.Add(lv);

            // Cột phải: Ghi chú sức khỏe & Lộ trình tập
            var pnlNotes = new Panel
            {
                Location = new Point(colW + 14, 0),
                Size = new Size(cardWidth - (colW + 14), 188),
                BackColor = CardBg,
                Padding = new Padding(14, 10, 14, 10)
            };
            pnlNotes.Paint += (s, e) =>
            {
                using var p = new Pen(Border);
                e.Graphics.DrawRectangle(p, 0, 0, pnlNotes.Width - 1, pnlNotes.Height - 1);
            };

            var lblNTitle = new Label
            {
                Text = "🎯  LỘ TRÌNH & GHI CHÚ SỨC KHỎE",
                Font = new Font("Segoe UI", 8.5F, FontStyle.Bold),
                ForeColor = Orange,
                Dock = DockStyle.Top,
                Height = 26
            };

            var txtN = new TextBox
            {
                Dock = DockStyle.Fill,
                Multiline = true,
                ReadOnly = true,
                BackColor = ItemBg,
                ForeColor = TextColor,
                Font = new Font("Segoe UI", 9F),
                BorderStyle = BorderStyle.None,
                Text = string.IsNullOrEmpty(_member.HealthNotes)
                    ? "• Mục tiêu: Tăng cơ giảm mỡ, rèn luyện sức bền tim mạch.\n• Lưu ý: Khởi động kỹ khớp cổ chân trước khi chạy bộ.\n• Chế độ dinh dưỡng: Bổ sung 140g protein/ngày, uống đủ 2.5L nước."
                    : _member.HealthNotes
            };

            pnlNotes.Controls.Add(txtN);
            pnlNotes.Controls.Add(lblNTitle);

            pnl.Controls.Add(pnlCheckIn);
            pnl.Controls.Add(pnlNotes);
            return pnl;
        }

        private static Color GetTierColor(string tier)
        {
            switch (tier)
            {
                case "Silver": return Color.FromArgb(6, 182, 212);
                case "Gold": return Color.FromArgb(234, 179, 8);
                case "Platinum": return Color.FromArgb(168, 85, 247);
                case "VIP Diamond": return Color.FromArgb(249, 115, 22);
                default: return Color.FromArgb(59, 130, 246); // Classic
            }
        }

        private static string GetInitials(string fullName)
        {
            if (string.IsNullOrWhiteSpace(fullName)) return "HV";
            var parts = fullName.Trim().Split(' ', StringSplitOptions.RemoveEmptyEntries);
            if (parts.Length == 1) return parts[0].Substring(0, Math.Min(2, parts[0].Length)).ToUpper();
            return (parts[0].Substring(0, 1) + parts[parts.Length - 1].Substring(0, 1)).ToUpper();
        }
    }
}
