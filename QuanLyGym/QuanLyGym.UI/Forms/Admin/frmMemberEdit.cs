using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace QuanLyGym.UI.Forms.Admin
{
    public class frmMemberEdit : Form
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

        private readonly ucAdminMember.MemberItem? _target;
        private readonly bool _isEdit;

        public ucAdminMember.MemberItem? ResultMember { get; private set; }

        // Form Controls
        private TextBox _txtCode = null!;
        private TextBox _txtName = null!;
        private ComboBox _cboGender = null!;
        private DateTimePicker _dtpDob = null!;
        private TextBox _txtPhone = null!;
        private TextBox _txtEmail = null!;
        private TextBox _txtAddress = null!;

        private ComboBox _cboBranch = null!;
        private ComboBox _cboTier = null!;
        private ComboBox _cboDuration = null!;
        private DateTimePicker _dtpStartDate = null!;
        private ComboBox _cboPt = null!;

        private NumericUpDown _numHeight = null!;
        private NumericUpDown _numWeight = null!;
        private Label _lblBmiVal = null!;
        private Label _lblBmiStatus = null!;
        private NumericUpDown _numBodyFat = null!;
        private NumericUpDown _numMuscle = null!;

        private TextBox _txtNotes = null!;

        public frmMemberEdit(ucAdminMember.MemberItem? memberToEdit = null)
        {
            _target = memberToEdit;
            _isEdit = (memberToEdit != null);
            BuildUI();
            PopulateData();
        }

        private void BuildUI()
        {
            FormBorderStyle = FormBorderStyle.None;
            StartPosition = FormStartPosition.CenterParent;
            Size = new Size(680, 750);
            BackColor = BgColor;
            ShowInTaskbar = false;
            DoubleBuffered = true;

            Paint += (s, e) =>
            {
                using var p = new Pen(Border, 1.5f);
                e.Graphics.DrawRectangle(p, 0, 0, Width - 1, Height - 1);
            };

            // 1. Title bar
            var pnlTitle = new Panel
            {
                Dock = DockStyle.Top,
                Height = 60,
                BackColor = ItemBg,
                Padding = new Padding(20, 0, 16, 0)
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
                Text = _isEdit ? "✏️  CHỈNH SỬA THÔNG TIN HỘI VIÊN" : "➕  ĐĂNG KÝ HỘI VIÊN MỚI",
                Font = new Font("Segoe UI", 12F, FontStyle.Bold),
                ForeColor = TextColor,
                AutoSize = true,
                Location = new Point(20, 12)
            };

            var lblSub = new Label
            {
                Text = _isEdit ? $"Cập nhật hồ sơ thành viên: {_target?.FullName} ({_target?.Code})" : "Thêm hội viên vào hệ thống, thiết lập gói tập và chỉ số thể chất ban đầu",
                Font = new Font("Segoe UI", 8.5F),
                ForeColor = Orange,
                AutoSize = true,
                Location = new Point(22, 35)
            };

            var btnClose = new Button
            {
                Text = "✕",
                Font = new Font("Segoe UI", 11F, FontStyle.Bold),
                ForeColor = MutedColor,
                BackColor = Color.Transparent,
                FlatStyle = FlatStyle.Flat,
                Size = new Size(36, 36),
                Anchor = AnchorStyles.Top | AnchorStyles.Right,
                Location = new Point(pnlTitle.Width - 48, 12),
                Cursor = Cursors.Hand
            };
            btnClose.FlatAppearance.BorderSize = 0;
            btnClose.Click += (s, e) => this.Close();

            pnlTitle.Controls.Add(lblTitle);
            pnlTitle.Controls.Add(lblSub);
            pnlTitle.Controls.Add(btnClose);

            // 2. Bottom Action Bar
            var pnlBottom = new Panel
            {
                Dock = DockStyle.Bottom,
                Height = 60,
                BackColor = ItemBg,
                Padding = new Padding(20, 10, 20, 10)
            };
            pnlBottom.Paint += (s, e) =>
            {
                using var p = new Pen(Border);
                e.Graphics.DrawLine(p, 0, 0, pnlBottom.Width, 0);
            };

            var btnCancel = new Button
            {
                Text = "Hủy bỏ",
                Font = new Font("Segoe UI", 9F, FontStyle.Bold),
                ForeColor = MutedColor,
                BackColor = CardBg,
                Size = new Size(110, 38),
                Dock = DockStyle.Right,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            btnCancel.FlatAppearance.BorderColor = Border;
            btnCancel.Click += (s, e) => this.Close();

            var btnSave = new Button
            {
                Text = _isEdit ? "💾  Lưu thay đổi" : "➕  Đăng ký hội viên",
                Font = new Font("Segoe UI", 9.5F, FontStyle.Bold),
                ForeColor = Color.White,
                BackColor = Orange,
                Size = new Size(180, 38),
                Dock = DockStyle.Right,
                Margin = new Padding(0, 0, 12, 0),
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            btnSave.FlatAppearance.BorderSize = 0;
            btnSave.Click += BtnSave_Click;

            var pnlGap = new Panel { Dock = DockStyle.Right, Width = 12, BackColor = Color.Transparent };

            pnlBottom.Controls.Add(btnSave);
            pnlBottom.Controls.Add(pnlGap);
            pnlBottom.Controls.Add(btnCancel);

            // 3. Scrollable Body
            var pnlBody = new Panel
            {
                Dock = DockStyle.Fill,
                AutoScroll = true,
                BackColor = BgColor,
                Padding = new Padding(20, 14, 20, 14)
            };

            int y = 4;

            // --- SECTION 1: THÔNG TIN CÁ NHÂN ---
            pnlBody.Controls.Add(CreateSectionHeader("👤  1. THÔNG TIN CÁ NHÂN", y));
            y += 32;

            var pnlSec1 = new Panel
            {
                Location = new Point(0, y),
                Size = new Size(620, 150),
                BackColor = CardBg,
                Padding = new Padding(16, 12, 16, 12)
            };
            pnlSec1.Paint += (s, e) =>
            {
                using var p = new Pen(Border);
                e.Graphics.DrawRectangle(p, 0, 0, pnlSec1.Width - 1, pnlSec1.Height - 1);
            };

            // Hàng 1: Mã HV + Họ và tên + Giới tính
            AddFormLabel(pnlSec1, "Mã hội viên *:", 14, 14);
            _txtCode = CreateInputTextBox(110, 10, 110, false);
            _txtCode.ReadOnly = _isEdit;
            pnlSec1.Controls.Add(_txtCode);

            AddFormLabel(pnlSec1, "Họ và tên *:", 236, 14);
            _txtName = CreateInputTextBox(316, 10, 180, true);
            pnlSec1.Controls.Add(_txtName);

            AddFormLabel(pnlSec1, "Giới tính:", 506, 14);
            _cboGender = new ComboBox
            {
                Location = new Point(506, 32),
                Size = new Size(98, 26),
                DropDownStyle = ComboBoxStyle.DropDownList,
                BackColor = ItemBg,
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 9F),
                FlatStyle = FlatStyle.Flat
            };
            _cboGender.Items.AddRange(new object[] { "Nam", "Nữ", "Khác" });
            _cboGender.SelectedIndex = 0;
            pnlSec1.Controls.Add(_cboGender);

            // Hàng 2: Ngày sinh + Số điện thoại + Email
            AddFormLabel(pnlSec1, "Ngày sinh:", 14, 62);
            _dtpDob = new DateTimePicker
            {
                Location = new Point(14, 80),
                Size = new Size(130, 24),
                Format = DateTimePickerFormat.Custom,
                CustomFormat = "dd/MM/yyyy",
                Font = new Font("Segoe UI", 9F)
            };
            pnlSec1.Controls.Add(_dtpDob);

            AddFormLabel(pnlSec1, "Số điện thoại *:", 160, 62);
            _txtPhone = CreateInputTextBox(160, 80, 150, true);
            pnlSec1.Controls.Add(_txtPhone);

            AddFormLabel(pnlSec1, "Email:", 330, 62);
            _txtEmail = CreateInputTextBox(330, 80, 274, true);
            pnlSec1.Controls.Add(_txtEmail);

            // Hàng 3: Địa chỉ thường trú
            AddFormLabel(pnlSec1, "Địa chỉ liên hệ:", 14, 112);
            _txtAddress = CreateInputTextBox(110, 110, 494, true);
            pnlSec1.Controls.Add(_txtAddress);

            pnlBody.Controls.Add(pnlSec1);
            y += 164;

            // --- SECTION 2: GÓI TẬP & CHI NHÁNH ---
            pnlBody.Controls.Add(CreateSectionHeader("⭐  2. GÓI TẬP & CHI NHÁNH ĐĂNG KÝ", y));
            y += 32;

            var pnlSec2 = new Panel
            {
                Location = new Point(0, y),
                Size = new Size(620, 120),
                BackColor = CardBg,
                Padding = new Padding(16, 12, 16, 12)
            };
            pnlSec2.Paint += (s, e) =>
            {
                using var p = new Pen(Border);
                e.Graphics.DrawRectangle(p, 0, 0, pnlSec2.Width - 1, pnlSec2.Height - 1);
            };

            // Hàng 1: Chi nhánh + Hạng thẻ + Thời hạn
            AddFormLabel(pnlSec2, "Chi nhánh tập *:", 14, 14);
            _cboBranch = new ComboBox
            {
                Location = new Point(14, 32),
                Size = new Size(200, 26),
                DropDownStyle = ComboBoxStyle.DropDownList,
                BackColor = ItemBg,
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 9F),
                FlatStyle = FlatStyle.Flat
            };
            _cboBranch.Items.AddRange(new object[] { "GymFit Quận 1 - Bến Nghé", "GymFit Hoàn Kiếm - Tràng Tiền", "GymFit Cầu Giấy - Duy Tân", "GymFit Bình Thạnh - Landmark", "GymFit Hải Châu - Bạch Đằng" });
            _cboBranch.SelectedIndex = 0;
            pnlSec2.Controls.Add(_cboBranch);

            AddFormLabel(pnlSec2, "Hạng thẻ *:", 230, 14);
            _cboTier = new ComboBox
            {
                Location = new Point(230, 32),
                Size = new Size(180, 26),
                DropDownStyle = ComboBoxStyle.DropDownList,
                BackColor = ItemBg,
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 9F),
                FlatStyle = FlatStyle.Flat
            };
            _cboTier.Items.AddRange(new object[] { "Classic", "Silver", "Gold", "Platinum", "VIP Diamond" });
            _cboTier.SelectedIndex = 2; // Gold
            pnlSec2.Controls.Add(_cboTier);

            AddFormLabel(pnlSec2, "Thời gian gói:", 430, 14);
            _cboDuration = new ComboBox
            {
                Location = new Point(430, 32),
                Size = new Size(174, 26),
                DropDownStyle = ComboBoxStyle.DropDownList,
                BackColor = ItemBg,
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 9F),
                FlatStyle = FlatStyle.Flat
            };
            _cboDuration.Items.AddRange(new object[] { "1 Tháng", "3 Tháng", "6 Tháng", "12 Tháng (1 Năm)" });
            _cboDuration.SelectedIndex = 3;
            pnlSec2.Controls.Add(_cboDuration);

            // Hàng 2: Ngày bắt đầu + HLV phụ trách
            AddFormLabel(pnlSec2, "Ngày bắt đầu:", 14, 68);
            _dtpStartDate = new DateTimePicker
            {
                Location = new Point(14, 86),
                Size = new Size(150, 24),
                Format = DateTimePickerFormat.Custom,
                CustomFormat = "dd/MM/yyyy",
                Font = new Font("Segoe UI", 9F)
            };
            pnlSec2.Controls.Add(_dtpStartDate);

            AddFormLabel(pnlSec2, "Huấn luyện viên phụ trách (PT):", 180, 68);
            _cboPt = new ComboBox
            {
                Location = new Point(180, 86),
                Size = new Size(230, 26),
                DropDownStyle = ComboBoxStyle.DropDownList,
                BackColor = ItemBg,
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 9F),
                FlatStyle = FlatStyle.Flat
            };
            _cboPt.Items.AddRange(new object[] { "Chưa đăng ký PT kèm", "Lê Công Vinh (PT Pro)", "Trần Thị Bích (PT Yoga)", "Nguyễn Đức Minh (PT Gym)", "Phan Thị Thảo (PT GroupX)", "Vũ Hoàng Long (Master PT)" });
            _cboPt.SelectedIndex = 0;
            pnlSec2.Controls.Add(_cboPt);

            pnlBody.Controls.Add(pnlSec2);
            y += 134;

            // --- SECTION 3: CHỈ SỐ THỂ LỰC & INBODY ---
            pnlBody.Controls.Add(CreateSectionHeader("💪  3. CHỈ SỐ CƠ THỂ BAN ĐẦU (INBODY)", y));
            y += 32;

            var pnlSec3 = new Panel
            {
                Location = new Point(0, y),
                Size = new Size(620, 120),
                BackColor = CardBg,
                Padding = new Padding(16, 12, 16, 12)
            };
            pnlSec3.Paint += (s, e) =>
            {
                using var p = new Pen(Border);
                e.Graphics.DrawRectangle(p, 0, 0, pnlSec3.Width - 1, pnlSec3.Height - 1);
            };

            // Chiều cao + Cân nặng + BMI kết quả
            AddFormLabel(pnlSec3, "Chiều cao (cm):", 14, 14);
            _numHeight = new NumericUpDown
            {
                Location = new Point(14, 32),
                Size = new Size(100, 24),
                Minimum = 100,
                Maximum = 230,
                Value = 170,
                DecimalPlaces = 1,
                BackColor = ItemBg,
                ForeColor = Color.White
            };
            pnlSec3.Controls.Add(_numHeight);

            AddFormLabel(pnlSec3, "Cân nặng (kg):", 130, 14);
            _numWeight = new NumericUpDown
            {
                Location = new Point(130, 32),
                Size = new Size(100, 24),
                Minimum = 30,
                Maximum = 200,
                Value = 65,
                DecimalPlaces = 1,
                BackColor = ItemBg,
                ForeColor = Color.White
            };
            pnlSec3.Controls.Add(_numWeight);

            // BMI Box
            var pnlBmi = new Panel
            {
                Location = new Point(250, 14),
                Size = new Size(350, 48),
                BackColor = ItemBg,
                Padding = new Padding(10, 6, 10, 6)
            };
            pnlBmi.Paint += (s, e) =>
            {
                using var p = new Pen(Border);
                e.Graphics.DrawRectangle(p, 0, 0, pnlBmi.Width - 1, pnlBmi.Height - 1);
            };

            _lblBmiVal = new Label
            {
                Text = "BMI: 22.5",
                Font = new Font("Segoe UI", 11F, FontStyle.Bold),
                ForeColor = Green,
                Location = new Point(10, 12),
                AutoSize = true
            };
            _lblBmiStatus = new Label
            {
                Text = "(Thể trạng cân đối tiêu chuẩn)",
                Font = new Font("Segoe UI", 8.5F),
                ForeColor = MutedColor,
                Location = new Point(105, 15),
                AutoSize = true
            };
            pnlBmi.Controls.Add(_lblBmiVal);
            pnlBmi.Controls.Add(_lblBmiStatus);
            pnlSec3.Controls.Add(pnlBmi);

            _numHeight.ValueChanged += (s, e) => UpdateBmi();
            _numWeight.ValueChanged += (s, e) => UpdateBmi();

            // Mỡ + Cơ
            AddFormLabel(pnlSec3, "Tỷ lệ mỡ Body Fat (%):", 14, 68);
            _numBodyFat = new NumericUpDown
            {
                Location = new Point(14, 86),
                Size = new Size(130, 24),
                Minimum = 3,
                Maximum = 60,
                Value = 18,
                DecimalPlaces = 1,
                BackColor = ItemBg,
                ForeColor = Color.White
            };
            pnlSec3.Controls.Add(_numBodyFat);

            AddFormLabel(pnlSec3, "Khối lượng cơ (kg):", 160, 68);
            _numMuscle = new NumericUpDown
            {
                Location = new Point(160, 86),
                Size = new Size(130, 24),
                Minimum = 10,
                Maximum = 100,
                Value = 30,
                DecimalPlaces = 1,
                BackColor = ItemBg,
                ForeColor = Color.White
            };
            pnlSec3.Controls.Add(_numMuscle);

            pnlBody.Controls.Add(pnlSec3);
            y += 134;

            // --- SECTION 4: GHI CHÚ SỨC KHỎE & MỤC TIÊU ---
            pnlBody.Controls.Add(CreateSectionHeader("📋  4. MỤC TIÊU TẬP LUYỆN & GHI CHÚ SỨC KHỎE", y));
            y += 32;

            _txtNotes = new TextBox
            {
                Location = new Point(0, y),
                Size = new Size(620, 50),
                Multiline = true,
                BackColor = ItemBg,
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 9F),
                BorderStyle = BorderStyle.FixedSingle,
                Text = "Mục tiêu: Tăng cường thể lực & giảm mỡ bụng. Không có tiền sử bệnh tim mạch."
            };
            pnlBody.Controls.Add(_txtNotes);
            y += 60;

            Controls.Add(pnlBody);
            Controls.Add(pnlBottom);
            Controls.Add(pnlTitle);
        }

        private static Label CreateSectionHeader(string text, int y)
        {
            return new Label
            {
                Text = text,
                Font = new Font("Segoe UI", 9F, FontStyle.Bold),
                ForeColor = Orange,
                Location = new Point(0, y),
                AutoSize = true
            };
        }

        private static void AddFormLabel(Panel parent, string text, int x, int y)
        {
            parent.Controls.Add(new Label
            {
                Text = text,
                Font = new Font("Segoe UI", 8F),
                ForeColor = MutedColor,
                Location = new Point(x, y),
                AutoSize = true
            });
        }

        private static TextBox CreateInputTextBox(int x, int y, int width, bool enabled = true)
        {
            return new TextBox
            {
                Location = new Point(x, y),
                Size = new Size(width, 24),
                BackColor = ItemBg,
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 9F),
                BorderStyle = BorderStyle.FixedSingle,
                Enabled = enabled
            };
        }

        private void UpdateBmi()
        {
            decimal hM = _numHeight.Value / 100m;
            decimal w = _numWeight.Value;
            if (hM <= 0) return;
            decimal bmi = Math.Round(w / (hM * hM), 1);
            _lblBmiVal.Text = $"BMI: {bmi}";

            if (bmi < 18.5m)
            {
                _lblBmiVal.ForeColor = Blue;
                _lblBmiStatus.Text = "(Thiếu cân / Cần tăng cân & cơ)";
            }
            else if (bmi <= 24.9m)
            {
                _lblBmiVal.ForeColor = Green;
                _lblBmiStatus.Text = "(Thể trạng cân đối tiêu chuẩn)";
            }
            else if (bmi <= 29.9m)
            {
                _lblBmiVal.ForeColor = Orange;
                _lblBmiStatus.Text = "(Thừa cân / Cần giảm mỡ & săn chắc)";
            }
            else
            {
                _lblBmiVal.ForeColor = Color.FromArgb(239, 68, 68);
                _lblBmiStatus.Text = "(Béo phì / Cần lộ trình tập & dinh dưỡng)";
            }
        }

        private void PopulateData()
        {
            if (_isEdit && _target != null)
            {
                _txtCode.Text = _target.Code;
                _txtName.Text = _target.FullName;
                _cboGender.SelectedItem = _target.Gender;
                _dtpDob.Value = _target.DateOfBirth;
                _txtPhone.Text = _target.Phone;
                _txtEmail.Text = _target.Email;
                _txtAddress.Text = _target.Address;

                for (int i = 0; i < _cboBranch.Items.Count; i++)
                {
                    if (_cboBranch.Items[i]?.ToString()?.Contains(_target.BranchName) == true)
                    {
                        _cboBranch.SelectedIndex = i;
                        break;
                    }
                }

                _cboTier.SelectedItem = _target.Tier;
                _dtpStartDate.Value = _target.StartDate;

                for (int i = 0; i < _cboPt.Items.Count; i++)
                {
                    if (_cboPt.Items[i]?.ToString()?.Contains(_target.AssignedPt) == true)
                    {
                        _cboPt.SelectedIndex = i;
                        break;
                    }
                }

                _numHeight.Value = Math.Max(100, Math.Min(230, _target.HeightCm));
                _numWeight.Value = Math.Max(30, Math.Min(200, _target.WeightKg));
                _numBodyFat.Value = Math.Max(3, Math.Min(60, _target.BodyFatPercent));
                _numMuscle.Value = Math.Max(10, Math.Min(100, _target.MuscleMassKg));
                _txtNotes.Text = _target.HealthNotes;

                UpdateBmi();
            }
            else
            {
                _txtCode.Text = "HV-" + new Random().Next(100, 999);
                _dtpDob.Value = new DateTime(1995, 5, 20);
                _dtpStartDate.Value = DateTime.Today;
                UpdateBmi();
            }
        }

        private void BtnSave_Click(object? sender, EventArgs e)
        {
            string name = _txtName.Text.Trim();
            string phone = _txtPhone.Text.Trim();

            if (string.IsNullOrEmpty(name))
            {
                MessageBox.Show("Vui lòng nhập họ và tên của hội viên!", "Thiếu thông tin", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                _txtName.Focus();
                return;
            }

            if (string.IsNullOrEmpty(phone))
            {
                MessageBox.Show("Vui lòng nhập số điện thoại hội viên!", "Thiếu thông tin", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                _txtPhone.Focus();
                return;
            }

            int months = 12;
            if (_cboDuration.SelectedIndex == 0) months = 1;
            else if (_cboDuration.SelectedIndex == 1) months = 3;
            else if (_cboDuration.SelectedIndex == 2) months = 6;
            else months = 12;

            DateTime start = _dtpStartDate.Value.Date;
            DateTime end = _isEdit && _target != null ? _target.EndDate : start.AddMonths(months);

            string ptName = _cboPt.SelectedItem?.ToString() ?? "Chưa có PT";
            if (ptName.Contains("(")) ptName = ptName.Substring(0, ptName.IndexOf("(")).Trim();

            if (_isEdit && _target != null)
            {
                _target.FullName = name;
                _target.Gender = _cboGender.SelectedItem?.ToString() ?? "Nam";
                _target.DateOfBirth = _dtpDob.Value.Date;
                _target.Phone = phone;
                _target.Email = _txtEmail.Text.Trim();
                _target.Address = _txtAddress.Text.Trim();
                _target.BranchName = _cboBranch.SelectedItem?.ToString() ?? "";
                _target.Tier = _cboTier.SelectedItem?.ToString() ?? "Gold";
                _target.AssignedPt = ptName;
                _target.HeightCm = _numHeight.Value;
                _target.WeightKg = _numWeight.Value;
                _target.BodyFatPercent = _numBodyFat.Value;
                _target.MuscleMassKg = _numMuscle.Value;
                _target.HealthNotes = _txtNotes.Text.Trim();

                ResultMember = _target;
            }
            else
            {
                ResultMember = new ucAdminMember.MemberItem
                {
                    Id = new Random().Next(1000, 9999),
                    Code = _txtCode.Text.Trim(),
                    FullName = name,
                    Gender = _cboGender.SelectedItem?.ToString() ?? "Nam",
                    DateOfBirth = _dtpDob.Value.Date,
                    Phone = phone,
                    Email = _txtEmail.Text.Trim(),
                    Address = _txtAddress.Text.Trim(),
                    BranchName = _cboBranch.SelectedItem?.ToString() ?? "",
                    Tier = _cboTier.SelectedItem?.ToString() ?? "Gold",
                    PackageName = $"Gói {_cboTier.SelectedItem} {months} Tháng",
                    RegisterDate = DateTime.Today,
                    StartDate = start,
                    EndDate = end,
                    AssignedPt = ptName,
                    HeightCm = _numHeight.Value,
                    WeightKg = _numWeight.Value,
                    BodyFatPercent = _numBodyFat.Value,
                    MuscleMassKg = _numMuscle.Value,
                    HealthNotes = _txtNotes.Text.Trim(),
                    TotalCheckIns = 1,
                    LastCheckIn = DateTime.Now
                };
            }

            DialogResult = DialogResult.OK;
            Close();
        }
    }
}
