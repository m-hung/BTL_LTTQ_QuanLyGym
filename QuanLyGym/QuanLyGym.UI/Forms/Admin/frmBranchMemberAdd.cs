using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace QuanLyGym.UI.Forms.Admin
{
    public class BranchMemberItem
    {
        public string MemberId { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string Package { get; set; } = "Gói VIP Diamond (12T)";
        public string AssignedPt { get; set; } = "Tự tập";
        public string StartDate { get; set; } = DateTime.Now.ToString("dd/MM/yyyy");
        public string ExpireDate { get; set; } = DateTime.Now.AddMonths(12).ToString("dd/MM/yyyy");
        public string Status { get; set; } = "Đang tập";
    }

    public sealed class frmBranchMemberAdd : Form
    {
        [DllImport("user32.dll")]
        private static extern bool ReleaseCapture();
        [DllImport("user32.dll")]
        private static extern int SendMessage(IntPtr hWnd, int msg, int wParam, int lParam);
        private const int WM_NCLBUTTONDOWN = 0xA1;
        private const int HTCAPTION = 0x2;

        public BranchMemberItem? ResultMember { get; private set; }

        private readonly string _branchName;
        private TextBox _txtName = null!;
        private TextBox _txtId = null!;
        private TextBox _txtPhone = null!;
        private ComboBox _cboPackage = null!;
        private TextBox _txtPt = null!;
        private NumericUpDown _numMonths = null!;

        public frmBranchMemberAdd(string branchName)
        {
            _branchName = branchName;
            BuildUI();
        }

        private void BuildUI()
        {
            FormBorderStyle = FormBorderStyle.None;
            StartPosition = FormStartPosition.CenterParent;
            Size = new Size(500, 500);
            BackColor = Color.FromArgb(15, 15, 17);
            ShowInTaskbar = false;
            DoubleBuffered = true;

            Paint += (s, e) =>
            {
                using var pen = new Pen(Color.FromArgb(63, 63, 70), 1.5f);
                e.Graphics.DrawRectangle(pen, 0, 0, Width - 1, Height - 1);
            };

            // Title Bar
            var pnlTitle = new Panel { Dock = DockStyle.Top, Height = 46, BackColor = Color.FromArgb(10, 10, 12) };
            pnlTitle.MouseDown += (s, e) =>
            {
                if (e.Button == MouseButtons.Left) { ReleaseCapture(); SendMessage(Handle, WM_NCLBUTTONDOWN, HTCAPTION, 0); }
            };

            var lblTitle = new Label
            {
                Text = $"👥  ĐĂNG KÝ HỌC VIÊN TẠI {_branchName.ToUpper()}",
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
                Text = "➕  Đăng ký học viên",
                Font = new Font("Segoe UI", 9F, FontStyle.Bold),
                ForeColor = Color.White,
                Size = new Size(165, 36),
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
                Padding = new Padding(24, 18, 24, 16)
            };

            var stack = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 1,
                RowCount = 6,
                BackColor = Color.Transparent
            };

            _txtId = CreateTextBox("HV-" + DateTime.Now.Year + "-" + new Random().Next(100, 999));
            stack.Controls.Add(CreateField("Mã học viên / Mã thẻ:", _txtId), 0, 0);

            _txtName = CreateTextBox("ví dụ: Trần Mai Hương");
            stack.Controls.Add(CreateField("Họ và tên học viên (*):", _txtName), 0, 1);

            _txtPhone = CreateTextBox("0987 654 321");
            stack.Controls.Add(CreateField("Số điện thoại liên hệ (*):", _txtPhone), 0, 2);

            _cboPackage = new ComboBox
            {
                Dock = DockStyle.Fill,
                DropDownStyle = ComboBoxStyle.DropDownList,
                BackColor = Color.FromArgb(20, 20, 23),
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 9.5F),
                FlatStyle = FlatStyle.Flat
            };
            _cboPackage.Items.AddRange(new object[] { "Gói VIP Diamond (Toàn hệ thống)", "Gói Platinum 12 Tháng", "Gói Classic 6 Tháng", "Gói Huấn Luyện Viên PT 1-1 (30 Buổi)", "Gói Học Viên Yoga & GroupX" });
            _cboPackage.SelectedIndex = 0;
            stack.Controls.Add(CreateField("Gói tập đăng ký:", _cboPackage, false), 0, 3);

            _txtPt = CreateTextBox("Tự tập");
            stack.Controls.Add(CreateField("HLV phụ trách kèm cặp (nếu có):", _txtPt), 0, 4);

            _numMonths = new NumericUpDown
            {
                Dock = DockStyle.Fill,
                Minimum = 1,
                Maximum = 60,
                Value = 12,
                BackColor = Color.FromArgb(20, 20, 23),
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 9.5F)
            };
            stack.Controls.Add(CreateField("Thời hạn gói tập (Số tháng):", _numMonths, false), 0, 5);

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
                MessageBox.Show("Vui lòng nhập họ tên học viên!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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

            int months = (int)_numMonths.Value;

            ResultMember = new BranchMemberItem
            {
                MemberId = _txtId.Text.Trim(),
                Name = name,
                Phone = phone,
                Package = _cboPackage.SelectedItem?.ToString() ?? "Gói Standard",
                AssignedPt = string.IsNullOrWhiteSpace(_txtPt.Text) ? "Tự tập" : _txtPt.Text.Trim(),
                StartDate = DateTime.Now.ToString("dd/MM/yyyy"),
                ExpireDate = DateTime.Now.AddMonths(months).ToString("dd/MM/yyyy"),
                Status = "Đang tập"
            };

            DialogResult = DialogResult.OK;
            Close();
        }
    }
}
