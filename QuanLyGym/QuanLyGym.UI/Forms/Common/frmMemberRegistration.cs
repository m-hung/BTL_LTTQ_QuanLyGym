using QuanLyGym.BLL.Services;

namespace QuanLyGym.UI.Forms.Common
{
    public class frmMemberRegistration : Form
    {
        private readonly MemberRegistrationService _service = new();
        private readonly TextBox _fullName = new() { MaxLength = 100 };
        private readonly TextBox _username = new() { MaxLength = 50 };
        private readonly TextBox _email = new() { MaxLength = 100 };
        private readonly TextBox _phone = new() { MaxLength = 15 };
        private readonly DateTimePicker _birthDate = new()
        {
            Format = DateTimePickerFormat.Custom,
            CustomFormat = "dd/MM/yyyy",
            MaxDate = DateTime.Today.AddDays(-1),
            ShowCheckBox = true,
            Checked = false
        };
        private readonly ComboBox _gender = new() { DropDownStyle = ComboBoxStyle.DropDownList };
        private readonly ComboBox _branch = new() { DropDownStyle = ComboBoxStyle.DropDownList };
        private readonly TextBox _password = new() { MaxLength = 128, UseSystemPasswordChar = true };
        private readonly TextBox _confirmPassword = new() { MaxLength = 128, UseSystemPasswordChar = true };
        private readonly Button _register = new() { Text = "ĐĂNG KÝ", Height = 44, Dock = DockStyle.Top, Enabled = false };
        private readonly Label _feedback = new()
        {
            AutoSize = true,
            ForeColor = Color.FromArgb(185, 28, 28),
            MaximumSize = new Size(390, 0)
        };

        public string RegisteredUsername { get; private set; } = string.Empty;

        public frmMemberRegistration()
        {
            SuspendLayout();
            Text = "Đăng ký tài khoản hội viên";
            StartPosition = FormStartPosition.CenterScreen;
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            BackColor = Color.White;

            var content = new Panel { Dock = DockStyle.Fill, AutoScroll = true };

            var fields = new TableLayoutPanel
            {
                Dock = DockStyle.Top,
                AutoSize = true,
                ColumnCount = 2,
                Padding = new Padding(28, 24, 28, 16)
            };
            fields.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50));
            fields.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50));

            var title = new Label
            {
                Text = "Đăng ký tài khoản hội viên",
                Font = new Font("Segoe UI", 22, FontStyle.Bold),
                ForeColor = Color.FromArgb(15, 23, 42),
                AutoSize = true,
                Margin = new Padding(3, 0, 3, 12)
            };
            fields.Controls.Add(title, 0, 0);
            fields.SetColumnSpan(title, 2);

            _gender.Items.AddRange(new object[] { "Nam", "Nữ", "Khác" });
            _branch.DisplayMember = nameof(RegistrationBranch.Name);

            (string Label, Control Input)[] inputs =
            {
                ("Họ và tên", _fullName),
                ("Tên đăng nhập", _username),
                ("Email", _email),
                ("Số điện thoại", _phone),
                ("Ngày sinh", _birthDate),
                ("Giới tính", _gender),
                ("Mật khẩu", _password),
                ("Nhập lại mật khẩu", _confirmPassword),
                ("Chi nhánh", _branch)
            };

            for (int index = 0; index < inputs.Length; index++)
            {
                var (label, input) = inputs[index];
                int column = index % 2;
                int row = 1 + index / 2 * 2;
                int rightMargin = column == 0 ? 12 : 3;

                fields.Controls.Add(new Label
                {
                    Text = label,
                    AutoSize = true,
                    ForeColor = Color.FromArgb(51, 65, 85),
                    Margin = new Padding(3, 0, rightMargin, 3)
                }, column, row);

                input.AccessibleName = label;
                input.Dock = DockStyle.Top;
                input.Margin = new Padding(3, 0, rightMargin, 7);
                fields.Controls.Add(input, column, row + 1);
                if (index == inputs.Length - 1)
                    fields.SetColumnSpan(input, 2);
            }

            int feedbackRow = 1 + (inputs.Length + 1) / 2 * 2;
            fields.Controls.Add(_feedback, 0, feedbackRow);
            fields.SetColumnSpan(_feedback, 2);

            var back = new LinkLabel
            {
                Text = "Quay lại đăng nhập",
                Dock = DockStyle.Top,
                Height = 27,
                Margin = new Padding(3, 7, 3, 0),
                TextAlign = ContentAlignment.MiddleCenter,
                LinkColor = Color.FromArgb(194, 65, 12),
                LinkBehavior = LinkBehavior.HoverUnderline,
                TabStop = true
            };
            back.LinkClicked += (_, _) => Close();
            _register.BackColor = Color.FromArgb(249, 115, 22);
            _register.ForeColor = Color.White;
            _register.Font = new Font("Segoe UI", 11, FontStyle.Bold);
            _register.FlatStyle = FlatStyle.Flat;
            _register.FlatAppearance.BorderSize = 0;
            _register.Cursor = Cursors.Hand;
            _register.Click += Register_Click;

            fields.Controls.Add(_register, 0, feedbackRow + 1);
            fields.SetColumnSpan(_register, 2);
            fields.Controls.Add(back, 0, feedbackRow + 2);
            fields.SetColumnSpan(back, 2);

            content.Controls.Add(fields);
            Controls.Add(content);
            Controls.Add(CreateBrandPanel());
            AcceptButton = _register;
            ActiveControl = _fullName;
            Shown += LoadBranches;

            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(820, 500);
            ResumeLayout(false);
        }

        private static Panel CreateBrandPanel()
        {
            var brand = new Panel { Dock = DockStyle.Left, Width = 360, BackColor = Color.FromArgb(30, 41, 59) };
            brand.Controls.Add(new Label
            {
                Text = "🏋",
                Font = new Font("Segoe UI Emoji", 54),
                ForeColor = Color.FromArgb(249, 115, 22),
                TextAlign = ContentAlignment.MiddleCenter,
                Bounds = new Rectangle(0, 110, 360, 105)
            });
            brand.Controls.Add(new Label
            {
                Text = "GYM MANAGER",
                Font = new Font("Segoe UI", 24, FontStyle.Bold),
                ForeColor = Color.White,
                TextAlign = ContentAlignment.MiddleCenter,
                Bounds = new Rectangle(0, 222, 360, 48)
            });
            brand.Controls.Add(new Label
            {
                Text = "Hệ thống quản lý phòng tập\r\nĐơn giản – Nhanh chóng – Hiệu quả",
                Font = new Font("Segoe UI", 10.5F),
                ForeColor = Color.FromArgb(203, 213, 225),
                TextAlign = ContentAlignment.TopCenter,
                Bounds = new Rectangle(0, 278, 360, 50)
            });
            brand.Controls.Add(new Panel
            {
                BackColor = Color.FromArgb(249, 115, 22),
                Bounds = new Rectangle(150, 340, 60, 4)
            });
            return brand;
        }

        private async void LoadBranches(object? sender, EventArgs e)
        {
            try
            {
                var branches = await Task.Run(_service.GetBranches);
                if (IsDisposed)
                    return;

                _branch.DataSource = branches;
                _branch.SelectedIndex = -1;
                _register.Enabled = branches.Count > 0;
                if (branches.Count == 0)
                    _feedback.Text = "Hiện chưa có chi nhánh đang hoạt động.";
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.TraceError(ex.ToString());
                if (!IsDisposed)
                    _feedback.Text = "Không thể tải danh sách chi nhánh. Vui lòng mở lại form để thử lại.";
            }
        }

        private void Register_Click(object? sender, EventArgs e)
        {
            _feedback.Text = string.Empty;
            if (!_birthDate.Checked || _branch.SelectedItem is not RegistrationBranch branch)
            {
                _feedback.Text = "Vui lòng chọn ngày sinh và chi nhánh.";
                return;
            }

            var registration = new MemberRegistration(_fullName.Text, _username.Text, _email.Text,
                _phone.Text, _birthDate.Value, _gender.Text, branch.Id, _password.Text, _confirmPassword.Text);
            _register.Enabled = false;
            UseWaitCursor = true;

            try
            {
                string? error = _service.Register(registration);
                if (error != null)
                {
                    _feedback.Text = error;
                    return;
                }

                RegisteredUsername = registration.Username.Trim();
                DialogResult = DialogResult.OK;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.TraceError(ex.ToString());
                _feedback.Text = "Không thể hoàn tất đăng ký. Vui lòng thử lại.";
            }
            finally
            {
                UseWaitCursor = false;
                _register.Enabled = true;
            }
        }
    }
}
