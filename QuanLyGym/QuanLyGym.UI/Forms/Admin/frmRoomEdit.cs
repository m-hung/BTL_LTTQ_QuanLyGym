using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using QuanLyGym.BLL.Models;

namespace QuanLyGym.UI.Forms.Admin
{
    public class frmRoomEdit : Form
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
        private static readonly Color BorderColor = Color.FromArgb(45, 45, 50);
        private static readonly Color TextColor = Color.FromArgb(244, 244, 245);
        private static readonly Color MutedColor = Color.FromArgb(161, 161, 170);
        private static readonly Color Orange = Color.FromArgb(249, 115, 22);
        private static readonly Color Cyan = Color.FromArgb(6, 182, 212);

        private readonly RoomDto? _original;
        public RoomDto ResultRoom { get; private set; } = new();

        private TextBox _txtName = null!;
        private TextBox _txtLocation = null!;
        private TextBox _txtCapacity = null!;
        private ComboBox _cboStatus = null!;
        private TextBox _txtDescription = null!;
        private TextBox _txtClasses = null!;

        public frmRoomEdit(RoomDto? initial = null)
        {
            _original = initial;
            if (initial != null)
            {
                ResultRoom = new RoomDto
                {
                    RoomID = initial.RoomID,
                    RoomName = initial.RoomName,
                    Location = initial.Location,
                    Capacity = initial.Capacity,
                    Description = initial.Description,
                    Status = initial.Status,
                    BranchID = initial.BranchID,
                    BranchName = initial.BranchName,
                    EquipmentCount = initial.EquipmentCount,
                    Classes = new System.Collections.Generic.List<string>(initial.Classes)
                };
            }
            else
            {
                ResultRoom = new RoomDto
                {
                    Capacity = 30,
                    Status = true,
                    Location = "Tầng 1 – Khu A",
                    Description = "Phòng tập luyện đa năng trang bị sàn chống trượt, máy lạnh và gương toàn cảnh."
                };
            }

            BuildUI();
            PopulateData();
        }

        private void BuildUI()
        {
            FormBorderStyle = FormBorderStyle.None;
            StartPosition = FormStartPosition.CenterParent;
            Size = new Size(650, 620);
            BackColor = BgColor;
            Padding = new Padding(2);

            var pnlContainer = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = CardBg,
                Padding = new Padding(24)
            };
            pnlContainer.Paint += (s, e) =>
            {
                using var p = new Pen(BorderColor, 1.5f);
                e.Graphics.DrawRectangle(p, 0, 0, pnlContainer.Width - 1, pnlContainer.Height - 1);
            };
            Controls.Add(pnlContainer);

            // Title Bar
            var pnlTitle = new Panel
            {
                Dock = DockStyle.Top,
                Height = 44,
                BackColor = Color.Transparent
            };
            pnlTitle.MouseDown += (s, e) =>
            {
                if (e.Button == MouseButtons.Left)
                {
                    ReleaseCapture();
                    SendMessage(Handle, WM_NCLBUTTONDOWN, HTCAPTION, 0);
                }
            };

            bool isEdit = _original != null && _original.RoomID > 0;
            var lblTitle = new Label
            {
                Text = isEdit ? $"🏢 CHỈNH SỬA PHÒNG TẬP (#{_original!.RoomID})" : "➕ THÊM MỚI PHÒNG TẬP / STUDIO",
                Font = new Font("Segoe UI", 11F, FontStyle.Bold),
                ForeColor = Orange,
                Dock = DockStyle.Left,
                AutoSize = true,
                TextAlign = ContentAlignment.MiddleLeft
            };
            pnlTitle.Controls.Add(lblTitle);

            var btnClose = new Button
            {
                Text = "✕",
                Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                ForeColor = MutedColor,
                BackColor = Color.Transparent,
                FlatStyle = FlatStyle.Flat,
                Size = new Size(32, 32),
                Dock = DockStyle.Right,
                Cursor = Cursors.Hand
            };
            btnClose.FlatAppearance.BorderSize = 0;
            btnClose.FlatAppearance.MouseOverBackColor = Color.FromArgb(39, 39, 42);
            btnClose.Click += (s, e) => { DialogResult = DialogResult.Cancel; Close(); };
            pnlTitle.Controls.Add(btnClose);
            pnlContainer.Controls.Add(pnlTitle);

            // Body
            var pnlBody = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.Transparent,
                Padding = new Padding(0, 10, 0, 0)
            };
            pnlContainer.Controls.Add(pnlBody);
            pnlBody.BringToFront();

            int y = 5;

            // Row 1: Tên phòng tập
            AddLabel(pnlBody, "Tên phòng tập / Studio (*):", 0, y);
            y += 22;

            _txtName = CreateTextBox(0, y, 600);
            pnlBody.Controls.Add(_txtName);
            y += 48;

            // Row 2: Vị trí & Sức chứa
            AddLabel(pnlBody, "Vị trí / Khu vực (ví dụ: Tầng 2 – Khu B) (*):", 0, y);
            AddLabel(pnlBody, "Sức chứa tối đa (người) (*):", 310, y);
            y += 22;

            _txtLocation = CreateTextBox(0, y, 290);
            _txtCapacity = CreateTextBox(310, y, 290);
            pnlBody.Controls.Add(_txtLocation);
            pnlBody.Controls.Add(_txtCapacity);
            y += 48;

            // Row 3: Trạng thái phòng
            AddLabel(pnlBody, "Trạng thái vận hành (*):", 0, y);
            y += 22;

            _cboStatus = new ComboBox
            {
                Location = new Point(0, y),
                Size = new Size(600, 32),
                BackColor = ItemBg,
                ForeColor = TextColor,
                Font = new Font("Segoe UI", 9.5F),
                DropDownStyle = ComboBoxStyle.DropDownList,
                FlatStyle = FlatStyle.Flat
            };
            _cboStatus.Items.AddRange(new object[]
            {
                "Hoạt động (Đón khách tập luyện)",
                "Tạm ngưng (Bảo trì / Sửa chữa)"
            });
            _cboStatus.SelectedIndex = 0;
            pnlBody.Controls.Add(_cboStatus);
            y += 48;

            // Row 4: Mô tả phòng
            AddLabel(pnlBody, "Mô tả cơ sở vật chất & trang thiết bị (*):", 0, y);
            y += 22;

            _txtDescription = new TextBox
            {
                Location = new Point(0, y),
                Size = new Size(600, 72),
                Multiline = true,
                BackColor = ItemBg,
                ForeColor = TextColor,
                Font = new Font("Segoe UI", 9.5F),
                BorderStyle = BorderStyle.FixedSingle,
                ScrollBars = ScrollBars.Vertical
            };
            pnlBody.Controls.Add(_txtDescription);
            y += 92;

            // Row 5: Lớp học diễn ra tại phòng
            AddLabel(pnlBody, "Lớp học tại phòng (nhiều lớp phân cách bằng dấu '|'):", 0, y);
            y += 22;

            _txtClasses = CreateTextBox(0, y, 600);
            pnlBody.Controls.Add(_txtClasses);
            y += 58;

            // Buttons Bar
            var pnlButtons = new Panel
            {
                Dock = DockStyle.Bottom,
                Height = 52,
                BackColor = Color.Transparent
            };
            pnlContainer.Controls.Add(pnlButtons);

            var btnCancel = new Button
            {
                Text = "Hủy bỏ",
                Font = new Font("Segoe UI", 9.5F, FontStyle.Bold),
                ForeColor = MutedColor,
                BackColor = Color.FromArgb(39, 39, 42),
                FlatStyle = FlatStyle.Flat,
                Size = new Size(110, 42),
                Location = new Point(360, 5),
                Cursor = Cursors.Hand
            };
            btnCancel.FlatAppearance.BorderSize = 0;
            btnCancel.Click += (s, e) => { DialogResult = DialogResult.Cancel; Close(); };
            pnlButtons.Controls.Add(btnCancel);

            var btnSave = new Button
            {
                Text = isEdit ? "💾 Cập nhật" : "➕ Thêm phòng",
                Font = new Font("Segoe UI", 9.5F, FontStyle.Bold),
                ForeColor = Color.White,
                BackColor = Orange,
                FlatStyle = FlatStyle.Flat,
                Size = new Size(130, 42),
                Location = new Point(480, 5),
                Cursor = Cursors.Hand
            };
            btnSave.FlatAppearance.BorderSize = 0;
            btnSave.Click += BtnSave_Click;
            pnlButtons.Controls.Add(btnSave);
        }

        private void PopulateData()
        {
            if (_original == null)
            {
                _txtCapacity.Text = "30";
                return;
            }

            _txtName.Text = _original.RoomName;
            _txtLocation.Text = _original.Location;
            _txtCapacity.Text = _original.Capacity.ToString();
            _cboStatus.SelectedIndex = _original.Status ? 0 : 1;
            _txtDescription.Text = _original.Description;
            _txtClasses.Text = _original.ClassesCombined;
        }

        private void BtnSave_Click(object? sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(_txtName.Text))
            {
                MessageBox.Show("Vui lòng nhập tên phòng tập.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                _txtName.Focus();
                return;
            }

            if (!int.TryParse(_txtCapacity.Text.Trim(), out int cap) || cap <= 0)
            {
                MessageBox.Show("Sức chứa phải là số nguyên dương lớn hơn 0.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                _txtCapacity.Focus();
                return;
            }

            ResultRoom.RoomName = _txtName.Text.Trim();
            ResultRoom.Location = string.IsNullOrWhiteSpace(_txtLocation.Text) ? "Tầng 1 – Khu A" : _txtLocation.Text.Trim();
            ResultRoom.Capacity = cap;
            ResultRoom.Status = _cboStatus.SelectedIndex == 0;
            ResultRoom.Description = _txtDescription.Text.Trim();
            ResultRoom.ClassesCombined = _txtClasses.Text.Trim();

            DialogResult = DialogResult.OK;
            Close();
        }

        private static void AddLabel(Panel parent, string text, int x, int y)
        {
            var lbl = new Label
            {
                Text = text,
                Location = new Point(x, y),
                AutoSize = true,
                Font = new Font("Segoe UI", 9F),
                ForeColor = MutedColor
            };
            parent.Controls.Add(lbl);
        }

        private static TextBox CreateTextBox(int x, int y, int width)
        {
            return new TextBox
            {
                Location = new Point(x, y),
                Size = new Size(width, 32),
                BackColor = ItemBg,
                ForeColor = TextColor,
                Font = new Font("Segoe UI", 9.5F),
                BorderStyle = BorderStyle.FixedSingle
            };
        }
    }
}
