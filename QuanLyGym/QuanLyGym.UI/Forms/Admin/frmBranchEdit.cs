using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace QuanLyGym.UI.Forms.Admin
{
    public class BranchModel
    {
        public int Id { get; set; }
        public string Code { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Manager { get; set; } = string.Empty;
        public string OpenTime { get; set; } = "05:30";
        public string CloseTime { get; set; } = "22:30";
        public int AreaSqM { get; set; } = 1000;
        public bool IsOnline { get; set; } = true;
        public string Note { get; set; } = string.Empty;
        public int MemberCount { get; set; } = 0;
        public int StaffCount { get; set; } = 0;
        public long MonthlyRevenue { get; set; } = 0;
    }

    public sealed class frmBranchEdit : Form
    {
        // P/Invoke để kéo thả form không viền
        [DllImport("user32.dll")]
        private static extern bool ReleaseCapture();
        [DllImport("user32.dll")]
        private static extern int SendMessage(IntPtr hWnd, int msg, int wParam, int lParam);
        private const int WM_NCLBUTTONDOWN = 0xA1;
        private const int HTCAPTION = 0x2;

        private readonly BranchModel? _target;
        private readonly bool _isEdit;

        public BranchModel? ResultBranch { get; private set; }

        // Controls
        private TextBox _txtName = null!;
        private TextBox _txtCode = null!;
        private TextBox _txtAddress = null!;
        private TextBox _txtPhone = null!;
        private TextBox _txtEmail = null!;
        private TextBox _txtManager = null!;
        private TextBox _txtOpenTime = null!;
        private TextBox _txtCloseTime = null!;
        private NumericUpDown _numArea = null!;
        private CheckBox _chkStatus = null!;
        private TextBox _txtNote = null!;

        public frmBranchEdit(BranchModel? branchToEdit = null)
        {
            _target = branchToEdit;
            _isEdit = (branchToEdit != null);

            BuildUI();
            PopulateData();
        }

        private void BuildUI()
        {
            FormBorderStyle = FormBorderStyle.None;
            StartPosition = FormStartPosition.CenterParent;
            Size = new Size(580, 640);
            BackColor = Color.FromArgb(15, 15, 17);
            ShowInTaskbar = false;
            DoubleBuffered = true;

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
                if (e.Button == MouseButtons.Left)
                {
                    ReleaseCapture();
                    SendMessage(Handle, WM_NCLBUTTONDOWN, HTCAPTION, 0);
                }
            };

            var lblTitle = new Label
            {
                Text = _isEdit ? "🏢  CHỈNH SỬA THÔNG TIN CHI NHÁNH" : "➕  THÊM CHI NHÁNH MỚI",
                Font = new Font("Segoe UI", 9.5F, FontStyle.Bold),
                ForeColor = Color.FromArgb(244, 244, 245),
                Dock = DockStyle.Left,
                Padding = new Padding(16, 0, 0, 0),
                TextAlign = ContentAlignment.MiddleLeft,
                AutoSize = true
            };
            lblTitle.MouseDown += (s, e) =>
            {
                if (e.Button == MouseButtons.Left)
                {
                    ReleaseCapture();
                    SendMessage(Handle, WM_NCLBUTTONDOWN, HTCAPTION, 0);
                }
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

            // Bottom Buttons
            var pnlBottom = new Panel
            {
                Dock = DockStyle.Bottom,
                Height = 60,
                BackColor = Color.FromArgb(20, 20, 23),
                Padding = new Padding(16, 12, 16, 12)
            };
            pnlBottom.Paint += (s, e) =>
            {
                using var p = new Pen(Color.FromArgb(45, 45, 50));
                e.Graphics.DrawLine(p, 0, 0, pnlBottom.Width, 0);
            };

            var btnSave = new Button
            {
                Text = _isEdit ? "💾  Lưu thay đổi" : "💾  Lưu chi nhánh mới",
                Font = new Font("Segoe UI", 9.5F, FontStyle.Bold),
                ForeColor = Color.White,
                Size = new Size(185, 36),
                BackColor = Color.FromArgb(249, 115, 22),
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand,
                UseVisualStyleBackColor = false
            };
            btnSave.FlatAppearance.BorderSize = 0;
            btnSave.FlatAppearance.MouseOverBackColor = Color.FromArgb(234, 88, 12);
            btnSave.Click += BtnSave_Click;

            var btnCancel = new Button
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

            stack.Controls.Add(BuildSection1(), 0, 0);
            stack.Controls.Add(BuildSection2(), 0, 1);
            stack.Controls.Add(BuildSection3(), 0, 2);

            pnlBody.Controls.Add(stack);

            Controls.Add(pnlBody);
            Controls.Add(pnlBottom);
            Controls.Add(pnlTitle);
        }

        private Control BuildSection1()
        {
            var card = CreateCardPanel("🏢  THÔNG TIN CƠ BẢN CHI NHÁNH", Color.FromArgb(249, 115, 22));

            var tbl = new TableLayoutPanel
            {
                Dock = DockStyle.Top,
                AutoSize = true,
                ColumnCount = 2,
                RowCount = 2,
                BackColor = Color.Transparent,
                Margin = new Padding(0)
            };
            tbl.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 65F));
            tbl.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 35F));

            _txtName = CreateTextBox("ví dụ: GymFit Cầu Giấy");
            tbl.Controls.Add(CreateFieldContainer("Tên chi nhánh (*):", _txtName), 0, 0);

            _txtCode = CreateTextBox("CN-CG");
            tbl.Controls.Add(CreateFieldContainer("Mã chi nhánh (*):", _txtCode), 1, 0);

            _txtAddress = CreateTextBox("Số 88 Duy Tân, P. Dịch Vọng Hậu, Q. Cầu Giấy, Hà Nội");
            tbl.Controls.Add(CreateFieldContainer("Địa chỉ đầy đủ (*):", _txtAddress), 0, 1);

            _txtPhone = CreateTextBox("024 3795 6677");
            tbl.Controls.Add(CreateFieldContainer("Số điện thoại / Hotline (*):", _txtPhone), 1, 1);

            card.Controls.Add(tbl);
            return card;
        }

        private Control BuildSection2()
        {
            var card = CreateCardPanel("⚙️  VẬN HÀNH & QUẢN LÝ", Color.FromArgb(59, 130, 246));

            var tbl = new TableLayoutPanel
            {
                Dock = DockStyle.Top,
                AutoSize = true,
                ColumnCount = 2,
                RowCount = 3,
                BackColor = Color.Transparent,
                Margin = new Padding(0)
            };
            tbl.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tbl.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));

            _txtManager = CreateTextBox("Họ tên quản lý chi nhánh");
            tbl.Controls.Add(CreateFieldContainer("Quản lý chi nhánh:", _txtManager), 0, 0);

            _txtEmail = CreateTextBox("caugiay@gymfit.vn");
            tbl.Controls.Add(CreateFieldContainer("Email liên hệ:", _txtEmail), 1, 0);

            _txtOpenTime = CreateTextBox("05:30");
            tbl.Controls.Add(CreateFieldContainer("Giờ mở cửa:", _txtOpenTime), 0, 1);

            _txtCloseTime = CreateTextBox("22:30");
            tbl.Controls.Add(CreateFieldContainer("Giờ đóng cửa:", _txtCloseTime), 1, 1);

            _numArea = new NumericUpDown
            {
                Dock = DockStyle.Fill,
                Minimum = 50,
                Maximum = 10000,
                Value = 1000,
                BackColor = Color.FromArgb(20, 20, 23),
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 10F)
            };
            tbl.Controls.Add(CreateFieldContainer("Diện tích sàn (m²):", _numArea, false), 0, 2);

            // Checkbox Trạng thái Online
            var pnlStatus = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.Transparent,
                Padding = new Padding(6, 6, 0, 0)
            };
            _chkStatus = new CheckBox
            {
                Text = "⚡ Chi nhánh đang hoạt động (ONLINE)",
                Font = new Font("Segoe UI", 9.5F, FontStyle.Bold),
                ForeColor = Color.FromArgb(34, 197, 94),
                AutoSize = true,
                Checked = true,
                Cursor = Cursors.Hand
            };
            _chkStatus.CheckedChanged += (s, e) =>
            {
                if (_chkStatus.Checked)
                {
                    _chkStatus.Text = "⚡ Chi nhánh đang hoạt động (ONLINE)";
                    _chkStatus.ForeColor = Color.FromArgb(34, 197, 94);
                }
                else
                {
                    _chkStatus.Text = "⏸️ Chi nhánh tạm đóng cửa (OFFLINE)";
                    _chkStatus.ForeColor = Color.FromArgb(239, 68, 68);
                }
            };
            pnlStatus.Controls.Add(_chkStatus);
            tbl.Controls.Add(CreateFieldContainer("Trạng thái cơ sở:", pnlStatus, false), 1, 2);

            card.Controls.Add(tbl);
            return card;
        }

        private Control BuildSection3()
        {
            var card = CreateCardPanel("📝  GHI CHÚ & TIỆN ÍCH CƠ SỞ", Color.FromArgb(168, 85, 247));

            _txtNote = new TextBox
            {
                Dock = DockStyle.Fill,
                Multiline = true,
                Height = 60,
                BackColor = Color.FromArgb(20, 20, 23),
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 9.5F),
                BorderStyle = BorderStyle.None,
                PlaceholderText = "Tiện ích đi kèm: Sauna, Bể bơi, Boxing, Yoga, Bãi đỗ xe ô tô..."
            };

            var pnlNoteBox = new Panel
            {
                Dock = DockStyle.Top,
                Height = 68,
                Padding = new Padding(8, 6, 8, 6),
                BackColor = Color.FromArgb(20, 20, 23)
            };
            pnlNoteBox.Paint += (s, e) =>
            {
                using var p = new Pen(Color.FromArgb(45, 45, 50));
                e.Graphics.DrawRectangle(p, 0, 0, pnlNoteBox.Width - 1, pnlNoteBox.Height - 1);
            };
            pnlNoteBox.Controls.Add(_txtNote);

            card.Controls.Add(pnlNoteBox);
            return card;
        }

        private Panel CreateCardPanel(string title, Color accentColor)
        {
            var card = new Panel
            {
                AutoSize = true,
                Margin = new Padding(0, 0, 0, 12),
                Padding = new Padding(16, 12, 16, 14),
                BackColor = Color.FromArgb(24, 24, 27),
                Dock = DockStyle.Top
            };
            card.Paint += (s, e) =>
            {
                using var pen = new Pen(Color.FromArgb(45, 45, 50));
                e.Graphics.DrawRectangle(pen, 0, 0, card.Width - 1, card.Height - 1);
            };

            var lblHeader = new Label
            {
                Text = title,
                Font = new Font("Segoe UI", 9F, FontStyle.Bold),
                ForeColor = accentColor,
                Dock = DockStyle.Top,
                Height = 26
            };
            card.Controls.Add(lblHeader);
            return card;
        }

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

        private Panel CreateFieldContainer(string labelText, Control inputControl, bool isTextPad = true)
        {
            var pnl = new Panel
            {
                Dock = DockStyle.Top,
                Height = 58,
                Margin = new Padding(0, 2, 8, 4),
                BackColor = Color.Transparent
            };

            var lbl = new Label
            {
                Text = labelText,
                Font = new Font("Segoe UI", 8F),
                ForeColor = Color.FromArgb(161, 161, 170),
                Location = new Point(0, 0),
                AutoSize = true
            };

            var box = new Panel
            {
                Location = new Point(0, 20),
                Size = new Size(244, 34),
                Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right,
                BackColor = Color.FromArgb(20, 20, 23)
            };
            if (isTextPad)
            {
                box.Padding = new Padding(8, 7, 8, 4);
            }
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

        private void PopulateData()
        {
            if (_target != null)
            {
                _txtName.Text = _target.Name;
                _txtCode.Text = _target.Code;
                _txtAddress.Text = _target.Address;
                _txtPhone.Text = _target.Phone;
                _txtEmail.Text = _target.Email;
                _txtManager.Text = _target.Manager;
                _txtOpenTime.Text = _target.OpenTime;
                _txtCloseTime.Text = _target.CloseTime;
                _numArea.Value = Math.Max(50, Math.Min(10000, _target.AreaSqM));
                _chkStatus.Checked = _target.IsOnline;
                _txtNote.Text = _target.Note;
            }
        }

        private void BtnSave_Click(object? sender, EventArgs e)
        {
            string name = _txtName.Text.Trim();
            if (string.IsNullOrWhiteSpace(name))
            {
                MessageBox.Show("Vui lòng nhập tên chi nhánh!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                _txtName.Focus();
                return;
            }

            string code = _txtCode.Text.Trim();
            if (string.IsNullOrWhiteSpace(code))
            {
                code = "CN-" + (name.Length > 2 ? name.Substring(0, 2).ToUpper() : "NEW");
            }

            string address = _txtAddress.Text.Trim();
            if (string.IsNullOrWhiteSpace(address))
            {
                MessageBox.Show("Vui lòng nhập địa chỉ chi nhánh!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                _txtAddress.Focus();
                return;
            }

            string phone = _txtPhone.Text.Trim();
            if (string.IsNullOrWhiteSpace(phone))
            {
                phone = "1900 6868";
            }

            if (_isEdit && _target != null)
            {
                _target.Name = name;
                _target.Code = code;
                _target.Address = address;
                _target.Phone = phone;
                _target.Email = _txtEmail.Text.Trim();
                _target.Manager = _txtManager.Text.Trim();
                _target.OpenTime = _txtOpenTime.Text.Trim();
                _target.CloseTime = _txtCloseTime.Text.Trim();
                _target.AreaSqM = (int)_numArea.Value;
                _target.IsOnline = _chkStatus.Checked;
                _target.Note = _txtNote.Text.Trim();
                ResultBranch = _target;
            }
            else
            {
                ResultBranch = new BranchModel
                {
                    Id = new Random().Next(100, 999),
                    Name = name,
                    Code = code,
                    Address = address,
                    Phone = phone,
                    Email = _txtEmail.Text.Trim(),
                    Manager = _txtManager.Text.Trim(),
                    OpenTime = _txtOpenTime.Text.Trim(),
                    CloseTime = _txtCloseTime.Text.Trim(),
                    AreaSqM = (int)_numArea.Value,
                    IsOnline = _chkStatus.Checked,
                    Note = _txtNote.Text.Trim(),
                    MemberCount = 120,
                    StaffCount = 6,
                    MonthlyRevenue = 150000000
                };
            }

            DialogResult = DialogResult.OK;
            Close();
        }
    }
}
