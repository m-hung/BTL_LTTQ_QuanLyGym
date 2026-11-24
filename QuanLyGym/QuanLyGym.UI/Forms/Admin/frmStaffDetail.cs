using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace QuanLyGym.UI.Forms.Admin
{
    public class frmStaffDetail : Form
    {
        [DllImport("user32.dll")]
        public static extern bool ReleaseCapture();
        [DllImport("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);
        private const int WM_NCLBUTTONDOWN = 0xA1;
        private const int HTCAPTION = 0x2;

        private readonly StaffInfo _staff;

        private Panel _pnlTitleBar = null!;
        private Label _lblTitle = null!;
        private Button _btnClose = null!;

        private Panel _pnlBottom = null!;
        private Button _btnEdit = null!;
        private Button _btnCloseBottom = null!;

        private Panel _pnlBody = null!;
        private TextBox _txtPassword = null!;
        private Button _btnTogglePassword = null!;
        private Button _btnCopyPassword = null!;

        public frmStaffDetail(StaffInfo staff)
        {
            _staff = staff;
            BuildUI();
        }

        private void BuildUI()
        {
            FormBorderStyle = FormBorderStyle.None;
            StartPosition = FormStartPosition.CenterParent;
            Size = new Size(580, 620);
            BackColor = Color.FromArgb(15, 15, 17);
            ShowInTaskbar = false;
            DoubleBuffered = true;

            // Đảm bảo vừa vặn trên mọi màn hình/laptop có độ phân giải hoặc scale DPI khác nhau
            Load += (s, e) =>
            {
                Rectangle workArea = Screen.FromControl(this).WorkingArea;
                int maxH = workArea.Height - 40;
                if (Height > maxH) Height = maxH;
                Location = new Point(
                    workArea.Left + Math.Max(0, (workArea.Width - Width) / 2),
                    workArea.Top + Math.Max(0, (workArea.Height - Height) / 2)
                );
            };

            // Viền ngoài form
            Paint += (s, e) =>
            {
                using var pen = new Pen(Color.FromArgb(63, 63, 70), 1.5f);
                e.Graphics.DrawRectangle(pen, 0, 0, Width - 1, Height - 1);
            };

            // 1. Thanh tiêu đề trên cùng
            _pnlTitleBar = new Panel
            {
                Dock = DockStyle.Top,
                Height = 46,
                BackColor = Color.FromArgb(10, 10, 12)
            };
            _pnlTitleBar.MouseDown += (s, e) =>
            {
                if (e.Button == MouseButtons.Left)
                {
                    ReleaseCapture();
                    SendMessage(Handle, WM_NCLBUTTONDOWN, HTCAPTION, 0);
                }
            };

            _lblTitle = new Label
            {
                Text = "👤  THÔNG TIN CHI TIẾT NHÂN VIÊN",
                Font = new Font("Segoe UI", 9.5F, FontStyle.Bold),
                ForeColor = Color.FromArgb(244, 244, 245),
                Dock = DockStyle.Left,
                Padding = new Padding(16, 0, 0, 0),
                TextAlign = ContentAlignment.MiddleLeft,
                AutoSize = true
            };
            _lblTitle.MouseDown += (s, e) =>
            {
                if (e.Button == MouseButtons.Left)
                {
                    ReleaseCapture();
                    SendMessage(Handle, WM_NCLBUTTONDOWN, HTCAPTION, 0);
                }
            };

            _btnClose = new Button
            {
                Text = "✕",
                Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                ForeColor = Color.FromArgb(161, 161, 170),
                Dock = DockStyle.Right,
                Width = 46,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand,
                UseVisualStyleBackColor = false
            };
            _btnClose.FlatAppearance.BorderSize = 0;
            _btnClose.FlatAppearance.MouseOverBackColor = Color.FromArgb(220, 38, 38);
            _btnClose.FlatAppearance.MouseDownBackColor = Color.FromArgb(185, 28, 28);
            _btnClose.MouseEnter += (s, e) => _btnClose.ForeColor = Color.White;
            _btnClose.MouseLeave += (s, e) => _btnClose.ForeColor = Color.FromArgb(161, 161, 170);
            _btnClose.Click += (s, e) => Close();

            _pnlTitleBar.Controls.Add(_lblTitle);
            _pnlTitleBar.Controls.Add(_btnClose);

            // 2. Thanh hành động bên dưới (Footer)
            _pnlBottom = new Panel
            {
                Dock = DockStyle.Bottom,
                Height = 52,
                BackColor = Color.FromArgb(20, 20, 23),
                Padding = new Padding(16, 8, 16, 8)
            };
            _pnlBottom.Paint += (s, e) =>
            {
                using var pen = new Pen(Color.FromArgb(45, 45, 50));
                e.Graphics.DrawLine(pen, 0, 0, _pnlBottom.Width, 0);
            };

            _btnEdit = new Button
            {
                Text = "✏️  Chỉnh sửa ",
                Font = new Font("Segoe UI", 9F, FontStyle.Bold),
                ForeColor = Color.White,
                Width = 175,
                Height = 36,
                Margin = new Padding(0, 0, 10, 0),
                BackColor = StaffTheme.Orange,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand,
                UseVisualStyleBackColor = false
            };
            _btnEdit.FlatAppearance.BorderSize = 0;
            _btnEdit.FlatAppearance.MouseOverBackColor = StaffTheme.OrangeDark;
            _btnEdit.Click += BtnEdit_Click;

            _btnCloseBottom = new Button
            {
                Text = "Đóng",
                Font = new Font("Segoe UI", 9F, FontStyle.Bold),
                ForeColor = Color.FromArgb(244, 244, 245),
                Width = 100,
                Height = 36,
                Margin = new Padding(0),
                BackColor = Color.FromArgb(39, 39, 42),
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand,
                UseVisualStyleBackColor = false
            };
            _btnCloseBottom.FlatAppearance.BorderColor = Color.FromArgb(63, 63, 70);
            _btnCloseBottom.FlatAppearance.BorderSize = 1;
            _btnCloseBottom.FlatAppearance.MouseOverBackColor = Color.FromArgb(63, 63, 70);
            _btnCloseBottom.Click += (s, e) => Close();

            var pnlBtns = new FlowLayoutPanel
            {
                Dock = DockStyle.Right,
                AutoSize = true,
                FlowDirection = FlowDirection.LeftToRight,
                WrapContents = false,
                Margin = new Padding(0)
            };
            pnlBtns.Controls.Add(_btnEdit);
            pnlBtns.Controls.Add(_btnCloseBottom);
            _pnlBottom.Controls.Add(pnlBtns);

            // 3. Nội dung cuộn chính
            _pnlBody = new Panel
            {
                Dock = DockStyle.Fill,
                AutoScroll = true,
                BackColor = Color.FromArgb(15, 15, 17),
                Padding = new Padding(16, 12, 16, 14)
            };

            Controls.Add(_pnlBody);
            Controls.Add(_pnlBottom);
            Controls.Add(_pnlTitleBar);

            RebuildContent();
        }

        private void BtnEdit_Click(object? sender, EventArgs e)
        {
            using var dlg = new frmStaffEdit(_staff);

            // Ẩn form chi tiết trong lúc sửa để hai form không đè lên nhau.
            // Dùng Opacity thay cho Hide(): Hide() sẽ ẩn luôn cả form con (form sửa) vì nó thuộc sở hữu của form này.
            Opacity = 0;
            DialogResult result;
            try
            {
                result = dlg.ShowDialog(this);
            }
            finally
            {
                Opacity = 1;
                Activate();
            }

            if (result == DialogResult.OK)
            {
                RebuildContent();
                this.Tag = "UPDATED";
                MessageBox.Show(this, "Cập nhật thông tin nhân viên thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        public void RebuildContent()
        {
            _pnlBody.SuspendLayout();
            _pnlBody.Controls.Clear();

            var stack = new TableLayoutPanel
            {
                Dock = DockStyle.Top,
                AutoSize = true,
                AutoSizeMode = AutoSizeMode.GrowAndShrink,
                ColumnCount = 1,
                RowCount = 4,
                BackColor = Color.Transparent,
                Margin = new Padding(0)
            };
            stack.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));

            // Thêm 4 khối thông tin
            stack.Controls.Add(BuildProfileBanner(), 0, 0);
            stack.Controls.Add(BuildBasicInfoCard(), 0, 1);
            stack.Controls.Add(BuildAccountCard(), 0, 2);
            stack.Controls.Add(BuildAttendanceCard(), 0, 3);

            _pnlBody.Controls.Add(stack);
            _pnlBody.ResumeLayout(true);
        }

        // =====================================================================
        // 1. BANNER HỒ SƠ NHÂN VIÊN (Ảnh, Tên, Chức vụ, Chi nhánh, Trạng thái)
        // =====================================================================
        private Control BuildProfileBanner()
        {
            var card = new Panel
            {
                Height = 88,
                Margin = new Padding(0, 0, 0, 12),
                BackColor = Color.FromArgb(24, 24, 27),
                Dock = DockStyle.Top
            };
            card.Paint += DrawCardBorder;

            Color roleCol = StaffTheme.RoleColor(_staff.Role);

            // Avatar 56x56
            var pnlAvatar = new Panel
            {
                Location = new Point(16, 16),
                Size = new Size(56, 56),
                BackColor = Color.Transparent
            };
            pnlAvatar.Paint += (s, e) =>
            {
                Graphics g = e.Graphics;
                g.SmoothingMode = SmoothingMode.AntiAlias;
                var rect = new Rectangle(0, 0, 55, 55);
                using var path = StaffTheme.RoundRect(rect, 6);

                if (_staff.Avatar != null)
                {
                    g.SetClip(path);
                    g.DrawImage(_staff.Avatar, rect);
                    g.ResetClip();
                }
                else
                {
                    using var br = new LinearGradientBrush(rect, Color.FromArgb(63, 63, 70), Color.FromArgb(39, 39, 42), 45f);
                    g.FillPath(br, path);
                    using var font = new Font("Segoe UI", 16F, FontStyle.Bold);
                    TextRenderer.DrawText(g, GetInitials(_staff.Name), font, rect, roleCol,
                        TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter);
                }

                using var pen = new Pen(roleCol, 1.5f);
                g.DrawPath(pen, path);
            };
            card.Controls.Add(pnlAvatar);

            // Họ và tên
            var lblName = new Label
            {
                Text = _staff.Name,
                Font = new Font("Segoe UI", 13.5F, FontStyle.Bold),
                ForeColor = Color.FromArgb(244, 244, 245),
                Location = new Point(86, 15),
                AutoSize = true
            };
            card.Controls.Add(lblName);

            // Badge Chức vụ
            var lblRole = new Label
            {
                Text = StaffTheme.RoleText(_staff.Role).ToUpper(),
                Font = new Font("Segoe UI", 7.5F, FontStyle.Bold),
                ForeColor = Color.White,
                BackColor = roleCol,
                Padding = new Padding(6, 2, 6, 2),
                Location = new Point(88, 44),
                AutoSize = true
            };
            card.Controls.Add(lblRole);

            // Chi nhánh
            var lblBranch = new Label
            {
                Text = "⌂ Chi nhánh " + _staff.Branch,
                Font = new Font("Segoe UI", 8.5F),
                ForeColor = Color.FromArgb(161, 161, 170),
                Location = new Point(lblRole.Right + 12, 45),
                AutoSize = true
            };
            card.Controls.Add(lblBranch);

            // Badge Trạng thái
            var lblStatus = new Label
            {
                Text = "● Đang làm việc",
                Font = new Font("Segoe UI", 8F, FontStyle.Bold),
                ForeColor = Color.FromArgb(34, 197, 94),
                BackColor = Color.FromArgb(20, 35, 25),
                Padding = new Padding(8, 3, 8, 3),
                Location = new Point(card.Width - 140, 18),
                Anchor = AnchorStyles.Top | AnchorStyles.Right,
                AutoSize = true
            };
            card.Controls.Add(lblStatus);

            return card;
        }

        // =====================================================================
        // 2. THÔNG TIN CƠ BẢN (Điện thoại, Email, Lương, Ca làm/Chứng chỉ, PT stats)
        // =====================================================================
        private Control BuildBasicInfoCard()
        {
            var card = new Panel
            {
                AutoSize = true,
                Margin = new Padding(0, 0, 0, 12),
                Padding = new Padding(16, 12, 16, 14),
                BackColor = Color.FromArgb(24, 24, 27),
                Dock = DockStyle.Top
            };
            card.Paint += DrawCardBorder;

            var tbl = new TableLayoutPanel
            {
                Dock = DockStyle.Top,
                AutoSize = true,
                ColumnCount = 2,
                RowCount = 6,
                BackColor = Color.Transparent,
                Margin = new Padding(0)
            };
            tbl.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tbl.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));

            // Tiêu đề khối
            var lblHeader = new Label
            {
                Text = "📋  THÔNG TIN CƠ BẢN",
                Font = new Font("Segoe UI", 9F, FontStyle.Bold),
                ForeColor = StaffTheme.Orange,
                Dock = DockStyle.Top,
                Height = 26
            };

            // Hàng 1: Số điện thoại & Email
            tbl.Controls.Add(CreateInfoItem("Số điện thoại:", _staff.Phone, Color.FromArgb(244, 244, 245)), 0, 0);
            tbl.Controls.Add(CreateInfoItem("Email công việc:", _staff.GetEffectiveEmail(), Color.FromArgb(244, 244, 245)), 1, 0);

            // Hàng 2: Ngày vào làm & Mức lương cơ bản
            string join = string.IsNullOrWhiteSpace(_staff.JoinDate) ? "Đang cập nhật" : _staff.JoinDate;
            tbl.Controls.Add(CreateInfoItem("Ngày vào làm:", join, Color.FromArgb(244, 244, 245)), 0, 1);
            tbl.Controls.Add(CreateInfoItem("Lương cơ bản:", StaffTheme.FormatMoney(_staff.Salary), StaffTheme.Green, true), 1, 1);

            // Hàng 3: Chi nhánh & Chuyên môn/Ca làm
            string extraLabel = _staff.Role == StaffRole.PT ? "Chứng chỉ chuyên môn:" : "Ca làm việc:";
            tbl.Controls.Add(CreateInfoItem("Chi nhánh:", _staff.Branch, Color.FromArgb(244, 244, 245)), 0, 2);
            tbl.Controls.Add(CreateInfoItem(extraLabel, _staff.Extra, Color.FromArgb(244, 244, 245)), 1, 2);

            // Hàng 4 (chỉ cho PT): Đánh giá & Học viên
            if (_staff.Role == StaffRole.PT)
            {
                tbl.Controls.Add(CreateInfoItem("Đánh giá trung bình:", $"★ {_staff.Rating:0.0} / 5.0", StaffTheme.Yellow, true), 0, 3);
                tbl.Controls.Add(CreateInfoItem("Học viên phụ trách:", $"{_staff.Students} học viên", StaffTheme.Blue, true), 1, 3);
            }

            card.Controls.Add(tbl);
            card.Controls.Add(lblHeader);
            return card;
        }

        // =====================================================================
        // 3. THÔNG TIN TÀI KHOẢN & MẬT KHẨU
        // =====================================================================
        private Control BuildAccountCard()
        {
            var card = new Panel
            {
                AutoSize = true,
                Margin = new Padding(0, 0, 0, 12),
                Padding = new Padding(16, 12, 16, 14),
                BackColor = Color.FromArgb(24, 24, 27),
                Dock = DockStyle.Top
            };
            card.Paint += DrawCardBorder;

            var lblHeader = new Label
            {
                Text = "🔐  TÀI KHOẢN & MẬT KHẨU ĐĂNG NHẬP",
                Font = new Font("Segoe UI", 9F, FontStyle.Bold),
                ForeColor = StaffTheme.Blue,
                Dock = DockStyle.Top,
                Height = 26
            };

            var tbl = new TableLayoutPanel
            {
                Dock = DockStyle.Top,
                AutoSize = true,
                ColumnCount = 2,
                RowCount = 2,
                BackColor = Color.Transparent,
                Margin = new Padding(0)
            };
            tbl.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tbl.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));

            // 1. Tên đăng nhập (Username)
            var pnlUser = new Panel { Dock = DockStyle.Fill, Height = 58, Margin = new Padding(0, 4, 8, 4) };
            var lblUserTitle = new Label
            {
                Text = "Tên đăng nhập (Username):",
                Font = new Font("Segoe UI", 8F),
                ForeColor = Color.FromArgb(161, 161, 170),
                Location = new Point(0, 2),
                AutoSize = true
            };
            var pnlUserBox = new Panel
            {
                Location = new Point(0, 22),
                Size = new Size(240, 32),
                BackColor = Color.FromArgb(20, 20, 23)
            };
            pnlUserBox.Paint += (s, e) =>
            {
                using var p = new Pen(Color.FromArgb(45, 45, 50));
                e.Graphics.DrawRectangle(p, 0, 0, pnlUserBox.Width - 1, pnlUserBox.Height - 1);
            };
            var lblUsernameVal = new Label
            {
                Text = _staff.GetEffectiveUsername(),
                Font = new Font("Consolas", 10F, FontStyle.Bold),
                ForeColor = Color.FromArgb(244, 244, 245),
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleLeft,
                Padding = new Padding(8, 0, 0, 0)
            };
            pnlUserBox.Controls.Add(lblUsernameVal);
            pnlUser.Controls.Add(lblUserTitle);
            pnlUser.Controls.Add(pnlUserBox);

            // 2. Quyền hạn hệ thống
            var pnlRole = new Panel { Dock = DockStyle.Fill, Height = 58, Margin = new Padding(8, 4, 0, 4) };
            var lblRoleTitle = new Label
            {
                Text = "Quyền hạn hệ thống:",
                Font = new Font("Segoe UI", 8F),
                ForeColor = Color.FromArgb(161, 161, 170),
                Location = new Point(0, 2),
                AutoSize = true
            };
            var pnlRoleBox = new Panel
            {
                Location = new Point(0, 22),
                Size = new Size(240, 32),
                BackColor = Color.FromArgb(20, 20, 23)
            };
            pnlRoleBox.Paint += (s, e) =>
            {
                using var p = new Pen(Color.FromArgb(45, 45, 50));
                e.Graphics.DrawRectangle(p, 0, 0, pnlRoleBox.Width - 1, pnlRoleBox.Height - 1);
            };
            var lblRoleVal = new Label
            {
                Text = StaffTheme.RoleText(_staff.Role) + " Portal",
                Font = new Font("Segoe UI", 9F, FontStyle.Bold),
                ForeColor = StaffTheme.RoleColor(_staff.Role),
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleLeft,
                Padding = new Padding(8, 0, 0, 0)
            };
            pnlRoleBox.Controls.Add(lblRoleVal);
            pnlRole.Controls.Add(lblRoleTitle);
            pnlRole.Controls.Add(pnlRoleBox);

            // 3. Mật khẩu (Password) có nút ẩn/hiện và nút copy
            var pnlPass = new Panel { Dock = DockStyle.Fill, Height = 58, Margin = new Padding(0, 8, 8, 4) };
            var lblPassTitle = new Label
            {
                Text = "Mật khẩu (Password):",
                Font = new Font("Segoe UI", 8F),
                ForeColor = Color.FromArgb(161, 161, 170),
                Location = new Point(0, 2),
                AutoSize = true
            };

            var pnlPassBox = new Panel
            {
                Location = new Point(0, 22),
                Size = new Size(240, 32),
                BackColor = Color.FromArgb(20, 20, 23)
            };
            pnlPassBox.Paint += (s, e) =>
            {
                using var p = new Pen(Color.FromArgb(45, 45, 50));
                e.Graphics.DrawRectangle(p, 0, 0, pnlPassBox.Width - 1, pnlPassBox.Height - 1);
            };

            _txtPassword = new TextBox
            {
                Text = _staff.GetEffectivePassword(),
                UseSystemPasswordChar = true,
                ReadOnly = true,
                BorderStyle = BorderStyle.None,
                BackColor = Color.FromArgb(20, 20, 23),
                ForeColor = Color.FromArgb(244, 244, 245),
                Font = new Font("Consolas", 10F, FontStyle.Bold),
                Location = new Point(8, 7),
                Width = 145
            };

            _btnTogglePassword = new Button
            {
                Text = "👁",
                Font = new Font("Segoe UI", 8.5F),
                ForeColor = StaffTheme.Orange,
                BackColor = Color.Transparent,
                FlatStyle = FlatStyle.Flat,
                Dock = DockStyle.Right,
                Width = 36,
                Cursor = Cursors.Hand,
                UseVisualStyleBackColor = false
            };
            _btnTogglePassword.FlatAppearance.BorderSize = 0;
            _btnTogglePassword.FlatAppearance.MouseOverBackColor = Color.FromArgb(39, 39, 42);
            _btnTogglePassword.Click += (s, e) =>
            {
                _txtPassword.UseSystemPasswordChar = !_txtPassword.UseSystemPasswordChar;
                _btnTogglePassword.Text = _txtPassword.UseSystemPasswordChar ? "👁" : "🔒";
            };

            _btnCopyPassword = new Button
            {
                Text = "📋",
                Font = new Font("Segoe UI", 8.5F),
                ForeColor = Color.FromArgb(161, 161, 170),
                BackColor = Color.Transparent,
                FlatStyle = FlatStyle.Flat,
                Dock = DockStyle.Right,
                Width = 36,
                Cursor = Cursors.Hand,
                UseVisualStyleBackColor = false
            };
            _btnCopyPassword.FlatAppearance.BorderSize = 0;
            _btnCopyPassword.FlatAppearance.MouseOverBackColor = Color.FromArgb(39, 39, 42);
            _btnCopyPassword.Click += (s, e) =>
            {
                Clipboard.SetText(_staff.GetEffectivePassword());
                MessageBox.Show("Đã sao chép mật khẩu vào bộ nhớ tạm!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            };

            pnlPassBox.Controls.Add(_txtPassword);
            pnlPassBox.Controls.Add(_btnCopyPassword);
            pnlPassBox.Controls.Add(_btnTogglePassword);

            pnlPass.Controls.Add(lblPassTitle);
            pnlPass.Controls.Add(pnlPassBox);

            // 4. Trạng thái tài khoản
            var pnlStatus = new Panel { Dock = DockStyle.Fill, Height = 58, Margin = new Padding(8, 8, 0, 4) };
            var lblStatTitle = new Label
            {
                Text = "Trạng thái đăng nhập:",
                Font = new Font("Segoe UI", 8F),
                ForeColor = Color.FromArgb(161, 161, 170),
                Location = new Point(0, 2),
                AutoSize = true
            };
            var pnlStatBox = new Panel
            {
                Location = new Point(0, 22),
                Size = new Size(240, 32),
                BackColor = Color.FromArgb(20, 20, 23)
            };
            pnlStatBox.Paint += (s, e) =>
            {
                using var p = new Pen(Color.FromArgb(45, 45, 50));
                e.Graphics.DrawRectangle(p, 0, 0, pnlStatBox.Width - 1, pnlStatBox.Height - 1);
            };
            var lblStatVal = new Label
            {
                Text = "● Hoạt động bình thường",
                Font = new Font("Segoe UI", 8.5F, FontStyle.Bold),
                ForeColor = Color.FromArgb(34, 197, 94),
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleLeft,
                Padding = new Padding(8, 0, 0, 0)
            };
            pnlStatBox.Controls.Add(lblStatVal);
            pnlStatus.Controls.Add(lblStatTitle);
            pnlStatus.Controls.Add(pnlStatBox);

            tbl.Controls.Add(pnlUser, 0, 0);
            tbl.Controls.Add(pnlRole, 1, 0);
            tbl.Controls.Add(pnlPass, 0, 1);
            tbl.Controls.Add(pnlStatus, 1, 1);

            card.Controls.Add(tbl);
            card.Controls.Add(lblHeader);
            return card;
        }

        // =====================================================================
        // 4. THÔNG TIN CHẤM CÔNG (Số ngày công, Tỷ lệ chuyên cần, Nghỉ phép, OT)
        // =====================================================================
        private Control BuildAttendanceCard()
        {
            var card = new Panel
            {
                AutoSize = true,
                Margin = new Padding(0, 0, 0, 6),
                Padding = new Padding(16, 12, 16, 16),
                BackColor = Color.FromArgb(24, 24, 27),
                Dock = DockStyle.Top
            };
            card.Paint += DrawCardBorder;

            var lblHeader = new Label
            {
                Text = $"📅  DỮ LIỆU CHẤM CÔNG (THÁNG {DateTime.Now:MM/yyyy})",
                Font = new Font("Segoe UI", 9F, FontStyle.Bold),
                ForeColor = StaffTheme.Green,
                Dock = DockStyle.Top,
                Height = 26
            };

            // Container chứa 2 cột: Cột trái số ngày to nổi bật, Cột phải thanh tiến trình & chi tiết
            var tbl = new TableLayoutPanel
            {
                Dock = DockStyle.Top,
                AutoSize = true,
                ColumnCount = 2,
                RowCount = 1,
                BackColor = Color.Transparent,
                Margin = new Padding(0, 6, 0, 0)
            };
            tbl.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 170F));
            tbl.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));

            // Cột 1: Hộp số ngày công lớn
            var pnlBigCount = new Panel
            {
                Dock = DockStyle.Fill,
                Height = 84,
                BackColor = Color.FromArgb(20, 20, 23)
            };
            pnlBigCount.Paint += (s, e) =>
            {
                using var p = new Pen(Color.FromArgb(45, 45, 50));
                e.Graphics.DrawRectangle(p, 0, 0, pnlBigCount.Width - 1, pnlBigCount.Height - 1);
            };

            var lblDaysBig = new Label
            {
                Text = $"{_staff.AttendanceDays} / {_staff.StandardDays}",
                Font = new Font("Segoe UI", 18F, FontStyle.Bold),
                ForeColor = StaffTheme.Green,
                Location = new Point(0, 10),
                Size = new Size(168, 36),
                TextAlign = ContentAlignment.MiddleCenter
            };
            var lblDaysSub = new Label
            {
                Text = "ngày công đạt chuẩn",
                Font = new Font("Segoe UI", 8F),
                ForeColor = Color.FromArgb(161, 161, 170),
                Location = new Point(0, 48),
                Size = new Size(168, 20),
                TextAlign = ContentAlignment.MiddleCenter
            };
            pnlBigCount.Controls.Add(lblDaysBig);
            pnlBigCount.Controls.Add(lblDaysSub);

            // Cột 2: Thanh tiến trình & chỉ số phụ
            var pnlRight = new Panel
            {
                Dock = DockStyle.Fill,
                Height = 84,
                Padding = new Padding(12, 0, 0, 0)
            };

            double rate = Math.Round((double)_staff.AttendanceDays / Math.Max(1, _staff.StandardDays) * 100, 1);
            var lblRate = new Label
            {
                Text = $"Tỷ lệ chuyên cần: {rate:0.0}% ({_staff.AttendanceDays}/{_staff.StandardDays} ngày)",
                Font = new Font("Segoe UI", 8.5F, FontStyle.Bold),
                ForeColor = Color.FromArgb(244, 244, 245),
                Location = new Point(12, 2),
                AutoSize = true
            };

            // Thanh tiến trình tự vẽ
            var pnlBar = new Panel
            {
                Location = new Point(14, 26),
                Size = new Size(310, 12),
                BackColor = Color.FromArgb(39, 39, 42)
            };
            pnlBar.Paint += (s, e) =>
            {
                Graphics g = e.Graphics;
                g.SmoothingMode = SmoothingMode.AntiAlias;
                int fillW = (int)(pnlBar.Width * Math.Min(1.0, rate / 100.0));
                if (fillW > 0)
                {
                    using var br = new SolidBrush(StaffTheme.Green);
                    g.FillRectangle(br, 0, 0, fillW, pnlBar.Height);
                }
            };

            // Chi tiết ngày nghỉ và tăng ca
            var lblDetail = new Label
            {
                Text = $"• Nghỉ phép: {_staff.LeaveDays} ngày   • Tăng ca (OT): {_staff.OvertimeHours:0.0} giờ   • Đạt chuyên cần: Tốt",
                Font = new Font("Segoe UI", 8F),
                ForeColor = Color.FromArgb(161, 161, 170),
                Location = new Point(12, 48),
                AutoSize = true
            };

            pnlRight.Controls.Add(lblRate);
            pnlRight.Controls.Add(pnlBar);
            pnlRight.Controls.Add(lblDetail);

            tbl.Controls.Add(pnlBigCount, 0, 0);
            tbl.Controls.Add(pnlRight, 1, 0);

            card.Controls.Add(tbl);
            card.Controls.Add(lblHeader);
            return card;
        }

        private Panel CreateInfoItem(string label, string value, Color valueColor, bool isBold = false)
        {
            var pnl = new Panel
            {
                Height = 44,
                Dock = DockStyle.Top,
                Margin = new Padding(0, 0, 0, 4)
            };

            var lbl = new Label
            {
                Text = label,
                Font = new Font("Segoe UI", 8F),
                ForeColor = Color.FromArgb(161, 161, 170),
                Location = new Point(0, 2),
                AutoSize = true
            };

            var val = new Label
            {
                Text = value,
                Font = new Font("Segoe UI", 9F, isBold ? FontStyle.Bold : FontStyle.Regular),
                ForeColor = valueColor,
                Location = new Point(0, 20),
                AutoSize = true
            };

            pnl.Controls.Add(lbl);
            pnl.Controls.Add(val);
            return pnl;
        }

        private void DrawCardBorder(object? sender, PaintEventArgs e)
        {
            var c = (Control)sender!;
            using var pen = new Pen(Color.FromArgb(45, 45, 50));
            e.Graphics.DrawRectangle(pen, 0, 0, c.Width - 1, c.Height - 1);
        }

        private static string GetInitials(string name)
        {
            string[] parts = name.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            if (parts.Length == 0) return "?";
            if (parts.Length == 1) return parts[0].Substring(0, 1).ToUpper();
            return (parts[0].Substring(0, 1) + parts[parts.Length - 1].Substring(0, 1)).ToUpper();
        }
    }
}