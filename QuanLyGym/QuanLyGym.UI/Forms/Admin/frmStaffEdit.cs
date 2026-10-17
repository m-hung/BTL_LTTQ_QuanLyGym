using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Globalization;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace QuanLyGym.UI.Forms.Admin
{
    public class frmStaffEdit : Form
    {
        [DllImport("user32.dll")]
        public static extern bool ReleaseCapture();
        [DllImport("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);
        private const int WM_NCLBUTTONDOWN = 0xA1;
        private const int HTCAPTION = 0x2;

        private readonly StaffInfo? _targetStaff;
        private readonly bool _isEdit;

        public StaffInfo? CreatedStaff { get; private set; }

        // Controls
        private Panel _pnlTitleBar = null!;
        private Label _lblFormTitle = null!;
        private Button _btnClose = null!;

        private Panel _pnlBottom = null!;
        private Button _btnCancel = null!;
        private Button _btnSave = null!;

        private Panel _pnlBody = null!;

        // Inputs - Basic Info
        private TextBox _txtName = null!;
        private ComboBox _cboRole = null!;
        private ComboBox _cboBranch = null!;
        private TextBox _txtPhone = null!;
        private TextBox _txtEmail = null!;
        private TextBox _txtSalary = null!;
        private TextBox _txtJoinDate = null!;
        private Label _lblExtraTitle = null!;
        private TextBox _txtExtra = null!;
        private Panel _pnlPtFields = null!;
        private NumericUpDown _numStudents = null!;
        private NumericUpDown _numRating = null!;

        // Inputs - Account
        private TextBox _txtUsername = null!;
        private TextBox _txtPassword = null!;
        private Button _btnTogglePass = null!;

        // Inputs - Attendance
        private NumericUpDown _numAttendanceDays = null!;
        private NumericUpDown _numStandardDays = null!;
        private NumericUpDown _numLeaveDays = null!;
        private NumericUpDown _numOvertime = null!;

        public frmStaffEdit(StaffInfo? staffToEdit = null)
        {
            _targetStaff = staffToEdit;
            _isEdit = (staffToEdit != null);

            BuildUI();
            PopulateData();
        }

        private void BuildUI()
        {
            FormBorderStyle = FormBorderStyle.None;
            StartPosition = FormStartPosition.CenterParent;
            Size = new Size(600, 720);
            MinimumSize = new Size(600, 480);
            BackColor = Color.FromArgb(15, 15, 17);
            ShowInTaskbar = false;
            DoubleBuffered = true;

            // Đảm bảo vừa vặn trên mọi màn hình/laptop có độ phân giải hoặc scale DPI khác nhau
            Load += (s, e) =>
            {
                Rectangle workArea = Screen.FromControl(this).WorkingArea;
                int maxH = workArea.Height - 30;
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

            // 1. Title bar
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

            _lblFormTitle = new Label
            {
                Text = _isEdit ? "✏️  CHỈNH SỬA THÔNG TIN NHÂN VIÊN " : "➕  THÊM NHÂN VIÊN MỚI",
                Font = new Font("Segoe UI", 9.5F, FontStyle.Bold),
                ForeColor = Color.FromArgb(244, 244, 245),
                Dock = DockStyle.Left,
                Padding = new Padding(16, 0, 0, 0),
                TextAlign = ContentAlignment.MiddleLeft,
                AutoSize = false,
                Width = 460
            };
            _lblFormTitle.MouseDown += (s, e) =>
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
            _btnClose.MouseEnter += (s, e) => _btnClose.ForeColor = Color.White;
            _btnClose.MouseLeave += (s, e) => _btnClose.ForeColor = Color.FromArgb(161, 161, 170);
            _btnClose.Click += (s, e) => { DialogResult = DialogResult.Cancel; Close(); };

            _pnlTitleBar.Controls.Add(_lblFormTitle);
            _pnlTitleBar.Controls.Add(_btnClose);

            // 2. Bottom Actions
            _pnlBottom = new Panel
            {
                Dock = DockStyle.Bottom,
                Height = 56,
                BackColor = Color.FromArgb(20, 20, 23),
                Padding = new Padding(16, 10, 16, 10)
            };
            _pnlBottom.Paint += (s, e) =>
            {
                using var pen = new Pen(Color.FromArgb(45, 45, 50));
                e.Graphics.DrawLine(pen, 0, 0, _pnlBottom.Width, 0);
            };

            _btnSave = new Button
            {
                Text = _isEdit ? "💾  Lưu thay đổi" : "💾  Lưu nhân viên mới",
                Font = new Font("Segoe UI", 9.5F, FontStyle.Bold),
                ForeColor = Color.White,
                Size = new Size(185, 36),
                Margin = new Padding(0),
                BackColor = StaffTheme.Orange,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand,
                UseVisualStyleBackColor = false
            };
            _btnSave.FlatAppearance.BorderSize = 0;
            _btnSave.FlatAppearance.MouseOverBackColor = StaffTheme.OrangeDark;
            _btnSave.Click += BtnSave_Click;

            _btnCancel = new Button
            {
                Text = "Hủy bỏ",
                Font = new Font("Segoe UI", 9F, FontStyle.Bold),
                ForeColor = Color.FromArgb(244, 244, 245),
                Size = new Size(100, 36),
                Margin = new Padding(0, 0, 10, 0),
                BackColor = Color.FromArgb(39, 39, 42),
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand,
                UseVisualStyleBackColor = false
            };
            _btnCancel.FlatAppearance.BorderColor = Color.FromArgb(63, 63, 70);
            _btnCancel.FlatAppearance.BorderSize = 1;
            _btnCancel.FlatAppearance.MouseOverBackColor = Color.FromArgb(63, 63, 70);
            _btnCancel.Click += (s, e) => { DialogResult = DialogResult.Cancel; Close(); };

            var pnlBtns = new FlowLayoutPanel
            {
                Dock = DockStyle.Right,
                AutoSize = true,
                FlowDirection = FlowDirection.LeftToRight,
                WrapContents = false,
                Margin = new Padding(0)
            };
            pnlBtns.Controls.Add(_btnCancel);
            pnlBtns.Controls.Add(_btnSave);
            _pnlBottom.Controls.Add(pnlBtns);

            // 3. Body Content
            _pnlBody = new Panel
            {
                Dock = DockStyle.Fill,
                AutoScroll = true,
                BackColor = Color.FromArgb(15, 15, 17),
                Padding = new Padding(16, 12, 16, 14)
            };

            var stack = new TableLayoutPanel
            {
                Dock = DockStyle.Top,
                AutoSize = true,
                AutoSizeMode = AutoSizeMode.GrowAndShrink,
                ColumnCount = 1,
                RowCount = 3,
                BackColor = Color.Transparent,
                Margin = new Padding(0)
            };
            stack.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));

            stack.Controls.Add(BuildBasicInfoSection(), 0, 0);
            stack.Controls.Add(BuildAccountSection(), 0, 1);
            stack.Controls.Add(BuildAttendanceSection(), 0, 2);

            _pnlBody.Controls.Add(stack);

            // Thứ tự dock: control THÊM SAU được dock TRƯỚC.
            // => Title (Top) và Bottom phải dock trước, Body (Fill) dock cuối để lấy phần còn lại.
            // Tuyệt đối không gọi BringToFront() cho Title/Bottom, nếu không Body sẽ bị chúng đè lên.
            Controls.Add(_pnlBody);
            Controls.Add(_pnlBottom);
            Controls.Add(_pnlTitleBar);
        }

        // =====================================================================
        // SECTION 1: THÔNG TIN CƠ BẢN
        // =====================================================================
        private Control BuildBasicInfoSection()
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
                Text = "📋  THÔNG TIN CƠ BẢN",
                Font = new Font("Segoe UI", 9F, FontStyle.Bold),
                ForeColor = StaffTheme.Orange,
                Dock = DockStyle.Top,
                Height = 26
            };

            var tbl = new TableLayoutPanel
            {
                Dock = DockStyle.Top,
                AutoSize = true,
                ColumnCount = 2,
                RowCount = 5,
                BackColor = Color.Transparent,
                Margin = new Padding(0)
            };
            tbl.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tbl.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));

            // 1. Họ và tên
            _txtName = CreateTextBox("Nhập họ và tên...");
            tbl.Controls.Add(CreateFieldContainer("Họ và tên nhân viên (*):", _txtName, true), 0, 0);

            // 2. Chức vụ / Vai trò
            _cboRole = new ComboBox
            {
                Dock = DockStyle.Fill,
                DropDownStyle = ComboBoxStyle.DropDownList,
                BackColor = Color.FromArgb(20, 20, 23),
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 9.5F),
                FlatStyle = FlatStyle.Flat
            };
            _cboRole.Items.AddRange(new object[] { "PT (Huấn luyện viên)", "Thu ngân", "Tạp vụ", "Bảo vệ" });
            _cboRole.SelectedIndex = 0;
            _cboRole.SelectedIndexChanged += CboRole_SelectedIndexChanged;
            tbl.Controls.Add(CreateFieldContainer("Chức vụ / Vị trí (*):", _cboRole, false), 1, 0);

            // 3. Số điện thoại
            _txtPhone = CreateTextBox("09xx.xxx.xxx");
            tbl.Controls.Add(CreateFieldContainer("Số điện thoại (*):", _txtPhone, true), 0, 1);

            // 4. Chi nhánh
            _cboBranch = new ComboBox
            {
                Dock = DockStyle.Fill,
                DropDownStyle = ComboBoxStyle.DropDownList,
                BackColor = Color.FromArgb(20, 20, 23),
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 9.5F),
                FlatStyle = FlatStyle.Flat
            };
            _cboBranch.Items.AddRange(new object[] { "Quận 1", "Hoàn Kiếm", "Cầu Giấy", "Bình Thạnh", "Hải Châu" });
            _cboBranch.SelectedIndex = 0;
            tbl.Controls.Add(CreateFieldContainer("Chi nhánh làm việc (*):", _cboBranch, false), 1, 1);

            // 5. Email công việc
            _txtEmail = CreateTextBox("nv.gymfit@gymfit.vn");
            tbl.Controls.Add(CreateFieldContainer("Email công việc:", _txtEmail, true), 0, 2);

            // 6. Mức lương cơ bản (VNĐ)
            _txtSalary = CreateTextBox("15000000");
            tbl.Controls.Add(CreateFieldContainer("Lương cơ bản (VNĐ):", _txtSalary, true), 1, 2);

            // 7. Ngày vào làm
            _txtJoinDate = CreateTextBox("MM/yyyy hoặc dd/MM/yyyy");
            tbl.Controls.Add(CreateFieldContainer("Ngày vào làm:", _txtJoinDate, true), 0, 3);

            // 8. Ca làm việc / Chứng chỉ
            _txtExtra = CreateTextBox("Nhập ca làm hoặc chứng chỉ...");
            var pnlExtraField = CreateFieldContainer("Chứng chỉ chuyên môn:", _txtExtra, true);
            _lblExtraTitle = pnlExtraField.Controls.OfType<Label>().First();
            tbl.Controls.Add(pnlExtraField, 1, 3);

            // Khối phụ cho PT: Số học viên & Đánh giá sao
            _pnlPtFields = new Panel
            {
                Dock = DockStyle.Top,
                AutoSize = true,
                Margin = new Padding(0, 4, 0, 0),
                BackColor = Color.Transparent
            };

            var tblPt = new TableLayoutPanel
            {
                Dock = DockStyle.Top,
                AutoSize = true,
                ColumnCount = 2,
                RowCount = 1,
                BackColor = Color.Transparent,
                Margin = new Padding(0)
            };
            tblPt.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tblPt.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));

            _numStudents = new NumericUpDown
            {
                Dock = DockStyle.Fill,
                Minimum = 0,
                Maximum = 500,
                Value = 15,
                BackColor = Color.FromArgb(20, 20, 23),
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 9.5F)
            };
            tblPt.Controls.Add(CreateFieldContainer("Số học viên phụ trách:", _numStudents, false), 0, 0);

            _numRating = new NumericUpDown
            {
                Dock = DockStyle.Fill,
                Minimum = 1.0M,
                Maximum = 5.0M,
                DecimalPlaces = 1,
                Increment = 0.1M,
                Value = 4.8M,
                BackColor = Color.FromArgb(20, 20, 23),
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 9.5F)
            };
            tblPt.Controls.Add(CreateFieldContainer("Điểm đánh giá (1.0 - 5.0 ★):", _numRating, false), 1, 0);

            _pnlPtFields.Controls.Add(tblPt);

            card.Controls.Add(_pnlPtFields);
            card.Controls.Add(tbl);
            card.Controls.Add(lblHeader);
            return card;
        }

        // =====================================================================
        // SECTION 2: TÀI KHOẢN & MẬT KHẨU
        // =====================================================================
        private Control BuildAccountSection()
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
                Text = "🔐  TÀI KHOẢN HỆ THỐNG",
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
                RowCount = 1,
                BackColor = Color.Transparent,
                Margin = new Padding(0)
            };
            tbl.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tbl.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));

            // Tên tài khoản
            _txtUsername = CreateTextBox("pt_username");
            tbl.Controls.Add(CreateFieldContainer("Tên đăng nhập (Username):", _txtUsername, true), 0, 0);

            // Mật khẩu có nút xem/ẩn
            _txtPassword = new TextBox
            {
                Dock = DockStyle.Fill,
                UseSystemPasswordChar = true,
                BackColor = Color.FromArgb(20, 20, 23),
                ForeColor = Color.White,
                Font = new Font("Consolas", 10F, FontStyle.Bold),
                BorderStyle = BorderStyle.None
            };

            _btnTogglePass = new Button
            {
                Text = "👁",
                Font = new Font("Segoe UI Emoji", 9F),
                ForeColor = StaffTheme.Orange,
                BackColor = Color.FromArgb(20, 20, 23),
                FlatStyle = FlatStyle.Flat,
                Dock = DockStyle.Right,
                Width = 36,
                Cursor = Cursors.Hand,
                TabStop = false,
                UseVisualStyleBackColor = false
            };
            _btnTogglePass.FlatAppearance.BorderSize = 0;
            _btnTogglePass.FlatAppearance.MouseOverBackColor = Color.FromArgb(39, 39, 42);
            _btnTogglePass.FlatAppearance.MouseDownBackColor = Color.FromArgb(39, 39, 42);
            _btnTogglePass.Click += (s, e) =>
            {
                _txtPassword.UseSystemPasswordChar = !_txtPassword.UseSystemPasswordChar;
                _btnTogglePass.Text = _txtPassword.UseSystemPasswordChar ? "👁" : "🔒";
            };

            // Khung chứa ô mật khẩu + nút mắt (dùng chung khung/nhãn với các ô khác để không bị lệch/che)
            var pnlPassInner = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.FromArgb(20, 20, 23),
                Padding = new Padding(8, 7, 0, 4)
            };
            pnlPassInner.Controls.Add(_txtPassword);     // Fill (dock sau)
            pnlPassInner.Controls.Add(_btnTogglePass);   // Right (dock trước)

            tbl.Controls.Add(CreateFieldContainer("Mật khẩu đăng nhập:", pnlPassInner, false), 1, 0);

            card.Controls.Add(tbl);
            card.Controls.Add(lblHeader);
            return card;
        }

        // =====================================================================
        // SECTION 3: DỮ LIỆU CHẤM CÔNG
        // =====================================================================
        private Control BuildAttendanceSection()
        {
            var card = new Panel
            {
                AutoSize = true,
                Margin = new Padding(0, 0, 0, 6),
                Padding = new Padding(16, 12, 16, 14),
                BackColor = Color.FromArgb(24, 24, 27),
                Dock = DockStyle.Top
            };
            card.Paint += DrawCardBorder;

            var lblHeader = new Label
            {
                Text = "📅  DỮ LIỆU CHẤM CÔNG (THÁNG NÀY)",
                Font = new Font("Segoe UI", 9F, FontStyle.Bold),
                ForeColor = StaffTheme.Green,
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

            // Số ngày đã chấm công
            _numAttendanceDays = new NumericUpDown
            {
                Dock = DockStyle.Fill,
                Minimum = 0,
                Maximum = 31,
                Value = 24,
                BackColor = Color.FromArgb(20, 20, 23),
                ForeColor = StaffTheme.Green,
                Font = new Font("Segoe UI", 10F, FontStyle.Bold)
            };
            tbl.Controls.Add(CreateFieldContainer("Số ngày đã chấm công:", _numAttendanceDays, false), 0, 0);

            // Số ngày công tiêu chuẩn
            _numStandardDays = new NumericUpDown
            {
                Dock = DockStyle.Fill,
                Minimum = 1,
                Maximum = 31,
                Value = 26,
                BackColor = Color.FromArgb(20, 20, 23),
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 10F)
            };
            tbl.Controls.Add(CreateFieldContainer("Số ngày công chuẩn / tháng:", _numStandardDays, false), 1, 0);

            // Số ngày nghỉ phép
            _numLeaveDays = new NumericUpDown
            {
                Dock = DockStyle.Fill,
                Minimum = 0,
                Maximum = 31,
                Value = 1,
                BackColor = Color.FromArgb(20, 20, 23),
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 9.5F)
            };
            tbl.Controls.Add(CreateFieldContainer("Số ngày nghỉ phép:", _numLeaveDays, false), 0, 1);

            // Giờ tăng ca (OT)
            _numOvertime = new NumericUpDown
            {
                Dock = DockStyle.Fill,
                Minimum = 0.0M,
                Maximum = 100.0M,
                DecimalPlaces = 1,
                Increment = 0.5M,
                Value = 4.0M,
                BackColor = Color.FromArgb(20, 20, 23),
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 9.5F)
            };
            tbl.Controls.Add(CreateFieldContainer("Số giờ tăng ca (OT):", _numOvertime, false), 1, 1);

            card.Controls.Add(tbl);
            card.Controls.Add(lblHeader);
            return card;
        }

        // =====================================================================
        // NẠP DỮ LIỆU BAN ĐẦU
        // =====================================================================
        private void PopulateData()
        {
            if (_targetStaff != null)
            {
                _txtName.Text = _targetStaff.Name;
                _cboRole.SelectedIndex = (int)_targetStaff.Role;
                _cboBranch.SelectedItem = _targetStaff.Branch;
                if (_cboBranch.SelectedIndex == -1) _cboBranch.SelectedIndex = 0;

                _txtPhone.Text = _targetStaff.Phone;
                _txtEmail.Text = _targetStaff.Email;
                _txtSalary.Text = _targetStaff.Salary.ToString();
                _txtJoinDate.Text = _targetStaff.JoinDate;
                _txtExtra.Text = _targetStaff.Extra;

                _numStudents.Value = Math.Max(0, Math.Min(500, _targetStaff.Students));
                _numRating.Value = (decimal)Math.Max(1.0, Math.Min(5.0, _targetStaff.Rating > 0 ? _targetStaff.Rating : 4.8));

                _txtUsername.Text = _targetStaff.GetEffectiveUsername();
                _txtPassword.Text = _targetStaff.GetEffectivePassword();

                _numAttendanceDays.Value = Math.Max(0, Math.Min(31, _targetStaff.AttendanceDays));
                _numStandardDays.Value = Math.Max(1, Math.Min(31, _targetStaff.StandardDays));
                _numLeaveDays.Value = Math.Max(0, Math.Min(31, _targetStaff.LeaveDays));
                _numOvertime.Value = (decimal)Math.Max(0.0, _targetStaff.OvertimeHours);
            }
            else
            {
                _cboRole.SelectedIndex = 0;
                _cboBranch.SelectedIndex = 0;
                _txtJoinDate.Text = DateTime.Now.ToString("MM/yyyy");
                _txtSalary.Text = "12000000";
                _txtPassword.Text = "GymFit@2026";
            }

            UpdateRoleContext();
        }

        private void CboRole_SelectedIndexChanged(object? sender, EventArgs e)
        {
            UpdateRoleContext();
        }

        private void UpdateRoleContext()
        {
            var role = (StaffRole)_cboRole.SelectedIndex;
            bool isPt = (role == StaffRole.PT);

            _lblExtraTitle.Text = isPt ? "Chứng chỉ chuyên môn:" : "Ca làm việc:";
            _pnlPtFields.Visible = isPt;

            if (!_isEdit && string.IsNullOrWhiteSpace(_txtName.Text))
            {
                string prefix = role switch
                {
                    StaffRole.PT => "pt_",
                    StaffRole.ThuNgan => "tn_",
                    StaffRole.TapVu => "tv_",
                    _ => "bv_"
                };
                if (string.IsNullOrWhiteSpace(_txtUsername.Text) || _txtUsername.Text.EndsWith("_"))
                    _txtUsername.Text = prefix;
            }
        }

        private void BtnSave_Click(object? sender, EventArgs e)
        {
            // Kiểm tra dữ liệu hợp lệ
            string name = _txtName.Text.Trim();
            if (string.IsNullOrWhiteSpace(name))
            {
                MessageBox.Show("Vui lòng nhập họ và tên nhân viên!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                _txtName.Focus();
                return;
            }

            string phone = _txtPhone.Text.Trim();
            if (string.IsNullOrWhiteSpace(phone))
            {
                MessageBox.Show("Vui lòng nhập số điện thoại nhân viên!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                _txtPhone.Focus();
                return;
            }

            long salary = 0;
            string rawSalary = _txtSalary.Text.Replace(".", "").Replace(",", "").Replace(" ", "").Replace("₫", "").Trim();
            if (!long.TryParse(rawSalary, out salary) || salary < 0)
            {
                salary = 8000000;
            }

            var role = (StaffRole)_cboRole.SelectedIndex;
            string branch = _cboBranch.SelectedItem?.ToString() ?? "Quận 1";

            string username = _txtUsername.Text.Trim();
            string password = _txtPassword.Text.Trim();
            if (string.IsNullOrWhiteSpace(password)) password = "GymFit@2026";

            string email = _txtEmail.Text.Trim();
            string joinDate = _txtJoinDate.Text.Trim();
            string extra = _txtExtra.Text.Trim();

            int attendance = (int)_numAttendanceDays.Value;
            int standard = (int)_numStandardDays.Value;
            int leave = (int)_numLeaveDays.Value;
            double overtime = (double)_numOvertime.Value;
            int students = (int)_numStudents.Value;
            double rating = (double)_numRating.Value;

            if (_isEdit && _targetStaff != null)
            {
                // Cập nhật nhân viên hiện tại
                _targetStaff.Name = name;
                _targetStaff.Role = role;
                _targetStaff.Branch = branch;
                _targetStaff.Phone = phone;
                _targetStaff.Email = email;
                _targetStaff.Salary = salary;
                _targetStaff.JoinDate = joinDate;
                _targetStaff.Extra = extra;
                _targetStaff.Students = students;
                _targetStaff.Rating = rating;

                _targetStaff.Username = username;
                _targetStaff.Password = password;

                _targetStaff.AttendanceDays = attendance;
                _targetStaff.StandardDays = standard;
                _targetStaff.LeaveDays = leave;
                _targetStaff.OvertimeHours = overtime;

                CreatedStaff = _targetStaff;
            }
            else
            {
                // Tạo mới nhân viên
                CreatedStaff = new StaffInfo
                {
                    Name = name,
                    Role = role,
                    Branch = branch,
                    Phone = phone,
                    Email = email,
                    Salary = salary,
                    JoinDate = joinDate,
                    Extra = extra,
                    Students = students,
                    Rating = rating,
                    Username = username,
                    Password = password,
                    AttendanceDays = attendance,
                    StandardDays = standard,
                    LeaveDays = leave,
                    OvertimeHours = overtime
                };
            }

            DialogResult = DialogResult.OK;
            Close();
        }

        // =====================================================================
        // TIỆN ÍCH DỰNG CONTROL
        // =====================================================================
        private TextBox CreateTextBox(string placeholder)
        {
            return new TextBox
            {
                Dock = DockStyle.Fill,
                BackColor = Color.FromArgb(20, 20, 23),
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 9.5F),
                BorderStyle = BorderStyle.None,
                PlaceholderText = placeholder
            };
        }

        private Panel CreateFieldContainer(string labelText, Control inputControl, bool isTextPad)
        {
            var pnl = new Panel
            {
                Dock = DockStyle.Top,
                Height = 60,
                Margin = new Padding(0, 2, 8, 4),
                BackColor = Color.Transparent
            };

            var lbl = new Label
            {
                Text = labelText,
                Font = new Font("Segoe UI", 8F),
                ForeColor = Color.FromArgb(161, 161, 170),
                Dock = DockStyle.Top,
                Height = 20,
                AutoSize = false,
                TextAlign = ContentAlignment.MiddleLeft
            };

            var box = new Panel
            {
                Dock = DockStyle.Top,
                Height = 36,
                BackColor = Color.FromArgb(20, 20, 23),
                Padding = isTextPad ? new Padding(8, 8, 8, 4) : new Padding(1)
            };
            box.Paint += (s, e) =>
            {
                using var p = new Pen(Color.FromArgb(45, 45, 50));
                e.Graphics.DrawRectangle(p, 0, 0, box.Width - 1, box.Height - 1);
            };

            box.Controls.Add(inputControl);

            // Control thêm sau được dock trước => nhãn nằm trên, ô nhập nằm dưới, không chồng lên nhau
            pnl.Controls.Add(box);
            pnl.Controls.Add(lbl);
            return pnl;
        }

        private void DrawCardBorder(object? sender, PaintEventArgs e)
        {
            var c = (Control)sender!;
            using var pen = new Pen(Color.FromArgb(45, 45, 50));
            e.Graphics.DrawRectangle(pen, 0, 0, c.Width - 1, c.Height - 1);
        }
    }
}