using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace QuanLyGym.UI.Forms.Admin
{
    public class BranchStaffItem
    {
        public string StaffId { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Role { get; set; } = "PT";
        public string Phone { get; set; } = string.Empty;
        public string Shift { get; set; } = "Ca sáng (06:00 - 14:00)";
        public string JoinDate { get; set; } = DateTime.Now.ToString("dd/MM/yyyy");
        public bool IsWorking { get; set; } = true;
    }

    public sealed class frmBranchStaffAdd : Form
    {
        [DllImport("user32.dll")]
        private static extern bool ReleaseCapture();
        [DllImport("user32.dll")]
        private static extern int SendMessage(IntPtr hWnd, int msg, int wParam, int lParam);
        private const int WM_NCLBUTTONDOWN = 0xA1;
        private const int HTCAPTION = 0x2;

        public BranchStaffItem? ResultStaff { get; private set; }

        private readonly string _branchName;
        private TextBox _txtName = null!;
        private TextBox _txtId = null!;
        private ComboBox _cboRole = null!;
        private TextBox _txtPhone = null!;
        private ComboBox _cboShift = null!;

        public frmBranchStaffAdd(string branchName)
        {
            _branchName = branchName;
            BuildUI();
        }

        private void BuildUI()
        {
            FormBorderStyle = FormBorderStyle.None;
            StartPosition = FormStartPosition.CenterParent;
            Size = new Size(500, 460);
            BackColor = Color.FromArgb(15, 15, 17);
            ShowInTaskbar = false;
            DoubleBuffered = true;

            Paint += (s, e) =>
            {
                using var pen = new Pen(Color.FromArgb(63, 63, 70), 1.5f);
                e.Graphics.DrawRectangle(pen, 0, 0, Width - 1, Height - 1);
            };

            // Title Bar
            var pnlTitle = new Panel
            {
                Dock = DockStyle.Top,
                Height = 46,
                BackColor = Color.FromArgb(10, 10, 12)
            };
            pnlTitle.MouseDown += (s, e) =>
            {
                if (e.Button == MouseButtons.Left) { ReleaseCapture(); SendMessage(Handle, WM_NCLBUTTONDOWN, HTCAPTION, 0); }
            };

            var lblTitle = new Label
            {
                Text = $"👔  THÊM NHÂN VIÊN VÀO {_branchName.ToUpper()}",
                Font = new Font("Segoe UI", 9F, FontStyle.Bold),
                ForeColor = Color.FromArgb(244, 244, 245),
                Dock = DockStyle.Left,
                Padding = new Padding(16, 0, 0, 0),
                TextAlign = ContentAlignment.MiddleLeft,
                AutoSize = true
            };

            var btnClose = new Button
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
            btnClose.FlatAppearance.BorderSize = 0;
            btnClose.FlatAppearance.MouseOverBackColor = Color.FromArgb(220, 38, 38);
            btnClose.Click += (s, e) => { DialogResult = DialogResult.Cancel; Close(); };

            pnlTitle.Controls.Add(lblTitle);
            pnlTitle.Controls.Add(btnClose);

            // Bottom Actions
            var pnlBottom = new Panel
            {
                Dock = DockStyle.Bottom,
                Height = 56,
                BackColor = Color.FromArgb(20, 20, 23),
                Padding = new Padding(16, 10, 16, 10)
            };
            pnlBottom.Paint += (s, e) =>
            {
                using var p = new Pen(Color.FromArgb(45, 45, 50));
                e.Graphics.DrawLine(p, 0, 0, pnlBottom.Width, 0);
            };

            var btnSave = new Button
            {
                Text = "➕  Thêm nhân viên",
                Font = new Font("Segoe UI", 9F, FontStyle.Bold),
                ForeColor = Color.White,
                Size = new Size(160, 36),
                BackColor = Color.FromArgb(249, 115, 22),
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand,
                UseVisualStyleBackColor = false
            };
            btnSave.FlatAppearance.BorderSize = 0;
            btnSave.Click += BtnSave_Click;

            var btnCancel = new Button
            {
                Text = "Hủy bỏ",
                Font = new Font("Segoe UI", 9F, FontStyle.Bold),
                ForeColor = Color.FromArgb(244, 244, 245),
                Size = new Size(90, 36),
                Margin = new Padding(0, 0, 10, 0),
                BackColor = Color.FromArgb(39, 39, 42),
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand,
                UseVisualStyleBackColor = false
            };
            btnCancel.FlatAppearance.BorderColor = Color.FromArgb(63, 63, 70);
            btnCancel.FlatAppearance.BorderSize = 1;
            btnCancel.Click += (s, e) => { DialogResult = DialogResult.Cancel; Close(); };

            var flp = new FlowLayoutPanel
            {
                Dock = DockStyle.Right,
                AutoSize = true,
                FlowDirection = FlowDirection.LeftToRight,
                WrapContents = false,
                Margin = new Padding(0)
            };
            flp.Controls.Add(btnCancel);
            flp.Controls.Add(btnSave);
            pnlBottom.Controls.Add(flp);

            // Body
            var pnlBody = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.FromArgb(15, 15, 17),
                Padding = new Padding(24, 20, 24, 16)
            };

            var stack = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 1,
                RowCount = 5,
                BackColor = Color.Transparent
            };

            _txtId = CreateTextBox("NV-" + new Random().Next(100, 999));
            stack.Controls.Add(CreateField("Mã nhân viên:", _txtId), 0, 0);

            _txtName = CreateTextBox("ví dụ: Nguyễn Văn Hùng");
            stack.Controls.Add(CreateField("Họ và tên nhân viên (*):", _txtName), 0, 1);

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
            stack.Controls.Add(CreateField("Vị trí / Chức vụ (*):", _cboRole, false), 0, 2);

            _txtPhone = CreateTextBox("0912 345 678");
            stack.Controls.Add(CreateField("Số điện thoại (*):", _txtPhone), 0, 3);

            _cboShift = new ComboBox
            {
                Dock = DockStyle.Fill,
                DropDownStyle = ComboBoxStyle.DropDownList,
                BackColor = Color.FromArgb(20, 20, 23),
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 9.5F),
                FlatStyle = FlatStyle.Flat
            };
            _cboShift.Items.AddRange(new object[] { "Ca sáng (06:00 - 14:00)", "Ca chiều (14:00 - 22:00)", "Ca xoay", "Hành chính (08:00 - 17:00)" });
            _cboShift.SelectedIndex = 0;
            stack.Controls.Add(CreateField("Ca làm việc phân công:", _cboShift, false), 0, 4);

            pnlBody.Controls.Add(stack);

            Controls.Add(pnlBody);
            Controls.Add(pnlBottom);
            Controls.Add(pnlTitle);
        }

        private TextBox CreateTextBox(string defaultText)
        {
            return new TextBox
            {
                Dock = DockStyle.Fill,
                Text = defaultText,
                BackColor = Color.FromArgb(20, 20, 23),
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 9.5F),
                BorderStyle = BorderStyle.None
            };
        }

        private Panel CreateField(string labelText, Control inputControl, bool isTextPad = true)
        {
            var pnl = new Panel { Dock = DockStyle.Top, Height = 56, BackColor = Color.Transparent };
            var lbl = new Label { Text = labelText, Font = new Font("Segoe UI", 8F), ForeColor = Color.FromArgb(161, 161, 170), AutoSize = true, Location = new Point(0, 0) };

            var box = new Panel { Location = new Point(0, 18), Size = new Size(450, 32), Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right, BackColor = Color.FromArgb(20, 20, 23) };
            if (isTextPad) box.Padding = new Padding(8, 6, 8, 4);
            box.Paint += (s, e) =>
            {
                using var p = new Pen(Color.FromArgb(45, 45, 50));
                e.Graphics.DrawRectangle(p, 0, 0, box.Width - 1, box.Height - 1);
            };
            box.Controls.Add(inputControl);

            pnl.Controls.Add(lbl);
            pnl.Controls.Add(box);
            return pnl;
        }

        private void BtnSave_Click(object? sender, EventArgs e)
        {
            string name = _txtName.Text.Trim();
            if (string.IsNullOrWhiteSpace(name))
            {
                MessageBox.Show("Vui lòng nhập tên nhân viên!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                _txtName.Focus();
                return;
            }

            string phone = _txtPhone.Text.Trim();
            if (string.IsNullOrWhiteSpace(phone))
            {
                MessageBox.Show("Vui lòng nhập số điện thoại!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                _txtPhone.Focus();
                return;
            }

            string role = _cboRole.SelectedIndex switch
            {
                0 => "PT",
                1 => "Thu ngân",
                2 => "Tạp vụ",
                _ => "Bảo vệ"
            };

            ResultStaff = new BranchStaffItem
            {
                StaffId = _txtId.Text.Trim(),
                Name = name,
                Role = role,
                Phone = phone,
                Shift = _cboShift.SelectedItem?.ToString() ?? "Ca sáng",
                JoinDate = DateTime.Now.ToString("dd/MM/yyyy"),
                IsWorking = true
            };

            DialogResult = DialogResult.OK;
            Close();
        }
    }
}
