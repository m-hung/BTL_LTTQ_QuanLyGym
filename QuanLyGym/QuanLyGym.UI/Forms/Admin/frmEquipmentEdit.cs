using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using QuanLyGym.BLL.Models;
using QuanLyGym.BLL.Services;

namespace QuanLyGym.UI.Forms.Admin
{
    public class frmEquipmentEdit : Form
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
        private static readonly Color Green = Color.FromArgb(34, 197, 94);
        private static readonly Color Red = Color.FromArgb(239, 68, 68);

        private readonly EquipmentDto? _original;
        public EquipmentDto ResultEquipment { get; private set; } = new();

        private TextBox _txtCode = null!;
        private TextBox _txtName = null!;
        private ComboBox _cboBranch = null!;
        private ComboBox _cboRoom = null!;
        private DateTimePicker _dtpPurchaseDate = null!;
        private TextBox _txtPrice = null!;
        private ComboBox _cboSupplier = null!;
        private ComboBox _cboStatus = null!;
        private TextBox _txtDescription = null!;

        private List<BranchLookupDto> _branches = new();
        private List<RoomLookupDto> _allRooms = new();

        public frmEquipmentEdit(EquipmentDto? initial = null)
        {
            _original = initial;
            if (initial != null)
            {
                ResultEquipment = new EquipmentDto
                {
                    EquipmentID = initial.EquipmentID,
                    EquipmentCode = initial.EquipmentCode,
                    EquipmentName = initial.EquipmentName,
                    RoomID = initial.RoomID,
                    RoomName = initial.RoomName,
                    BranchID = initial.BranchID,
                    BranchName = initial.BranchName,
                    PurchaseDate = initial.PurchaseDate,
                    Price = initial.Price,
                    Supplier = initial.Supplier,
                    Status = initial.Status,
                    Description = initial.Description
                };
            }
            else
            {
                ResultEquipment = new EquipmentDto
                {
                    PurchaseDate = DateTime.Now,
                    Price = 10000000,
                    Status = "Tốt",
                    Supplier = "Impulse Fitness"
                };
            }

            LoadLookups();
            BuildUI();
            PopulateData();
        }

        private void LoadLookups()
        {
            _branches = EquipmentService.Instance.GetBranchLookups();
            _allRooms = EquipmentService.Instance.GetAllRooms();
        }

        private void BuildUI()
        {
            FormBorderStyle = FormBorderStyle.None;
            StartPosition = FormStartPosition.CenterParent;
            Size = new Size(680, 720);
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

            bool isEdit = _original != null && _original.EquipmentID > 0;
            var lblTitle = new Label
            {
                Text = isEdit ? "⚙ CHỈNH SỬA THÔNG TIN THIẾT BỊ" : "➕ THÊM THIẾT BỊ MỚI VÀO PHÒNG TẬP",
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

            // Content Flow / Layout
            var pnlBody = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.Transparent,
                Padding = new Padding(0, 10, 0, 0)
            };
            pnlContainer.Controls.Add(pnlBody);
            pnlBody.BringToFront();

            int y = 5;

            // Row 1: Mã máy (30%) & Tên máy (70%)
            AddLabel(pnlBody, "Mã thiết bị (bỏ trống để tự sinh):", 0, y);
            AddLabel(pnlBody, "Tên máy / Thiết bị (*):", 210, y);
            y += 22;

            _txtCode = CreateTextBox(0, y, 190);
            _txtName = CreateTextBox(210, y, 420);
            pnlBody.Controls.Add(_txtCode);
            pnlBody.Controls.Add(_txtName);
            y += 48;

            // Row 2: Chi nhánh & Phòng tập
            AddLabel(pnlBody, "Chi nhánh đặt máy (*):", 0, y);
            AddLabel(pnlBody, "Phòng tập cụ thể (*):", 320, y);
            y += 22;

            _cboBranch = CreateComboBox(0, y, 300);
            _cboRoom = CreateComboBox(320, y, 310);
            pnlBody.Controls.Add(_cboBranch);
            pnlBody.Controls.Add(_cboRoom);
            y += 48;

            // Row 3: Ngày mua & Đơn giá
            AddLabel(pnlBody, "Ngày mua thiết bị:", 0, y);
            AddLabel(pnlBody, "Đơn giá mua (VNĐ) (*):", 320, y);
            y += 22;

            _dtpPurchaseDate = new DateTimePicker
            {
                Location = new Point(0, y),
                Size = new Size(300, 34),
                Font = new Font("Segoe UI", 9.5F),
                Format = DateTimePickerFormat.Custom,
                CustomFormat = "dd/MM/yyyy",
                CalendarMonthBackground = ItemBg,
                CalendarTitleBackColor = Orange,
                CalendarForeColor = TextColor
            };
            pnlBody.Controls.Add(_dtpPurchaseDate);

            _txtPrice = CreateTextBox(320, y, 310);
            pnlBody.Controls.Add(_txtPrice);
            y += 48;

            // Row 4: Nhà cung cấp & Tình trạng
            AddLabel(pnlBody, "Nhà cung cấp / Thương hiệu (*):", 0, y);
            AddLabel(pnlBody, "Tình trạng hoạt động (*):", 320, y);
            y += 22;

            _cboSupplier = CreateComboBox(0, y, 300);
            _cboSupplier.DropDownStyle = ComboBoxStyle.DropDown; // Cho phép gõ tự do
            _cboSupplier.Items.AddRange(new object[]
            {
                "Technogym Italy",
                "LifeFitness USA",
                "Impulse Fitness",
                "Matrix Fitness",
                "Olympic Pro VN",
                "WaterRower Inc",
                "Precor USA",
                "Hammer Strength",
                "Tuấn Vũ Sport",
                "Việt Nam Sport Co."
            });

            _cboStatus = CreateComboBox(320, y, 310);
            _cboStatus.DropDownStyle = ComboBoxStyle.DropDownList;
            _cboStatus.Items.AddRange(new object[] { "Tốt", "Cần bảo trì", "Hỏng hóc" });

            pnlBody.Controls.Add(_cboSupplier);
            pnlBody.Controls.Add(_cboStatus);
            y += 48;

            // Row 5: Ghi chú / Mô tả chi tiết
            AddLabel(pnlBody, "Mô tả tính năng / Thông số / Ghi chú bảo dưỡng:", 0, y);
            y += 22;

            _txtDescription = new TextBox
            {
                Location = new Point(0, y),
                Size = new Size(630, 80),
                Multiline = true,
                BackColor = ItemBg,
                ForeColor = TextColor,
                Font = new Font("Segoe UI", 9.5F),
                BorderStyle = BorderStyle.FixedSingle,
                ScrollBars = ScrollBars.Vertical
            };
            pnlBody.Controls.Add(_txtDescription);
            y += 95;

            // Information note panel
            var pnlNote = new Panel
            {
                Location = new Point(0, y),
                Size = new Size(630, 48),
                BackColor = Color.FromArgb(28, 25, 23),
                Padding = new Padding(12, 8, 12, 8)
            };
            pnlNote.Paint += (s, e) =>
            {
                using var p = new Pen(Color.FromArgb(120, 53, 15), 1f);
                e.Graphics.DrawRectangle(p, 0, 0, pnlNote.Width - 1, pnlNote.Height - 1);
            };
            var lblHelp = new Label
            {
                Text = "💡 Dữ liệu thiết bị sẽ được lưu trực tiếp vào cơ sở dữ liệu và hiển thị trên Dashboard Tổng quan & Doanh thu.",
                Font = new Font("Segoe UI", 8F),
                ForeColor = Color.FromArgb(253, 186, 116),
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleLeft
            };
            pnlNote.Controls.Add(lblHelp);
            pnlBody.Controls.Add(pnlNote);

            // Footer Buttons
            var pnlFooter = new Panel
            {
                Dock = DockStyle.Bottom,
                Height = 60,
                BackColor = Color.Transparent,
                Padding = new Padding(0, 10, 0, 0)
            };
            pnlContainer.Controls.Add(pnlFooter);
            pnlFooter.BringToFront();

            var btnSave = new Button
            {
                Text = isEdit ? "💾 Cập nhật thiết bị" : "➕ Thêm thiết bị",
                Font = new Font("Segoe UI", 9.5F, FontStyle.Bold),
                ForeColor = Color.White,
                BackColor = Orange,
                Size = new Size(160, 40),
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand,
                Dock = DockStyle.Right
            };
            btnSave.FlatAppearance.BorderSize = 0;
            btnSave.FlatAppearance.MouseOverBackColor = Color.FromArgb(234, 88, 12);
            btnSave.Click += BtnSave_Click;
            pnlFooter.Controls.Add(btnSave);

            var btnCancel = new Button
            {
                Text = "Hủy bỏ",
                Font = new Font("Segoe UI", 9.5F),
                ForeColor = MutedColor,
                BackColor = Color.FromArgb(39, 39, 42),
                Size = new Size(110, 40),
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand,
                Dock = DockStyle.Right
            };
            btnCancel.FlatAppearance.BorderSize = 0;
            btnCancel.FlatAppearance.MouseOverBackColor = Color.FromArgb(63, 63, 70);
            btnCancel.Click += (s, e) => { DialogResult = DialogResult.Cancel; Close(); };
            pnlFooter.Controls.Add(btnCancel);
        }

        private void AddLabel(Panel parent, string text, int x, int y)
        {
            var lbl = new Label
            {
                Text = text,
                Location = new Point(x, y),
                AutoSize = true,
                Font = new Font("Segoe UI", 8.5F, FontStyle.Bold),
                ForeColor = MutedColor
            };
            parent.Controls.Add(lbl);
        }

        private TextBox CreateTextBox(int x, int y, int width)
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

        private ComboBox CreateComboBox(int x, int y, int width)
        {
            return new ComboBox
            {
                Location = new Point(x, y),
                Size = new Size(width, 32),
                BackColor = ItemBg,
                ForeColor = TextColor,
                Font = new Font("Segoe UI", 9.5F),
                FlatStyle = FlatStyle.Flat
            };
        }

        private void PopulateData()
        {
            // Nạp chi nhánh vào ComboBox
            _cboBranch.Items.Clear();
            _cboBranch.DisplayMember = "BranchName";
            _cboBranch.ValueMember = "BranchID";

            foreach (var b in _branches)
            {
                _cboBranch.Items.Add(b);
            }

            _cboBranch.SelectedIndexChanged += (s, e) =>
            {
                if (_cboBranch.SelectedItem is BranchLookupDto selBranch)
                {
                    PopulateRoomsForBranch(selBranch.BranchID);
                }
            };

            if (_cboBranch.Items.Count > 0)
            {
                // Chọn branch theo dữ liệu ban đầu
                if (ResultEquipment.BranchID.HasValue && ResultEquipment.BranchID.Value > 0)
                {
                    var match = _branches.FirstOrDefault(b => b.BranchID == ResultEquipment.BranchID.Value);
                    if (match != null) _cboBranch.SelectedItem = match;
                    else _cboBranch.SelectedIndex = 0;
                }
                else
                {
                    _cboBranch.SelectedIndex = 0;
                }
            }

            // Dữ liệu ban đầu
            _txtCode.Text = ResultEquipment.EquipmentCode;
            _txtName.Text = ResultEquipment.EquipmentName;
            _dtpPurchaseDate.Value = ResultEquipment.PurchaseDate > DateTime.MinValue ? ResultEquipment.PurchaseDate : DateTime.Now;
            _txtPrice.Text = ResultEquipment.Price.ToString("N0");
            _cboSupplier.Text = !string.IsNullOrWhiteSpace(ResultEquipment.Supplier) ? ResultEquipment.Supplier : "Technogym Italy";
            _cboStatus.SelectedItem = !string.IsNullOrWhiteSpace(ResultEquipment.Status) ? ResultEquipment.Status : "Tốt";
            _txtDescription.Text = ResultEquipment.Description;
        }

        private void PopulateRoomsForBranch(int branchId)
        {
            _cboRoom.Items.Clear();
            _cboRoom.DisplayMember = "RoomName";
            _cboRoom.ValueMember = "RoomID";

            var rooms = _allRooms.Where(r => r.BranchID == branchId || branchId == 0).ToList();
            if (!rooms.Any())
            {
                rooms = _allRooms;
            }

            foreach (var r in rooms)
            {
                _cboRoom.Items.Add(r);
            }

            if (rooms.Any())
            {
                var match = rooms.FirstOrDefault(r => r.RoomID == ResultEquipment.RoomID);
                if (match != null) _cboRoom.SelectedItem = match;
                else _cboRoom.SelectedIndex = 0;
            }
        }

        private void BtnSave_Click(object? sender, EventArgs e)
        {
            string name = _txtName.Text.Trim();
            if (string.IsNullOrWhiteSpace(name))
            {
                MessageBox.Show("Vui lòng nhập tên thiết bị.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                _txtName.Focus();
                return;
            }

            if (_cboRoom.SelectedItem is not RoomLookupDto selRoom)
            {
                MessageBox.Show("Vui lòng chọn phòng tập đặt máy.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                _cboRoom.Focus();
                return;
            }

            string priceRaw = _txtPrice.Text.Replace(".", "").Replace(",", "").Replace("₫", "").Trim();
            if (!decimal.TryParse(priceRaw, out decimal price) || price < 0)
            {
                MessageBox.Show("Đơn giá không hợp lệ. Vui lòng nhập số tiền hợp lệ.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                _txtPrice.Focus();
                return;
            }

            string supplier = _cboSupplier.Text.Trim();
            if (string.IsNullOrWhiteSpace(supplier))
            {
                supplier = "Impulse Fitness";
            }

            string status = _cboStatus.SelectedItem?.ToString() ?? "Tốt";

            ResultEquipment.EquipmentCode = _txtCode.Text.Trim();
            ResultEquipment.EquipmentName = name;
            ResultEquipment.RoomID = selRoom.RoomID;
            ResultEquipment.RoomName = selRoom.RoomName;
            ResultEquipment.BranchID = (_cboBranch.SelectedItem as BranchLookupDto)?.BranchID;
            ResultEquipment.BranchName = (_cboBranch.SelectedItem as BranchLookupDto)?.BranchName ?? "";
            ResultEquipment.PurchaseDate = _dtpPurchaseDate.Value;
            ResultEquipment.Price = price;
            ResultEquipment.Supplier = supplier;
            ResultEquipment.Status = status;
            ResultEquipment.Description = _txtDescription.Text.Trim();

            // Thực thi Backend trực tiếp
            bool isEdit = _original != null && _original.EquipmentID > 0;
            if (isEdit)
            {
                var (success, msg) = EquipmentService.Instance.UpdateEquipment(ResultEquipment);
                if (!success)
                {
                    MessageBox.Show(msg, "Lỗi cập nhật", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }
            else
            {
                var (success, msg, newId) = EquipmentService.Instance.AddEquipment(ResultEquipment);
                if (!success)
                {
                    MessageBox.Show(msg, "Lỗi thêm mới", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                ResultEquipment.EquipmentID = newId;
            }

            DialogResult = DialogResult.OK;
            Close();
        }
    }
}
