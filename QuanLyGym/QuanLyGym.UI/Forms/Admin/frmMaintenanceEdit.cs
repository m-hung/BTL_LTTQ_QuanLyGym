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
    public class frmMaintenanceEdit : Form
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

        private readonly EquipmentMaintenanceDto? _original;
        public EquipmentMaintenanceDto ResultItem { get; private set; } = new();

        private ComboBox _cboEquipment = null!;
        private DateTimePicker _dtpMaintenanceDate = null!;
        private ComboBox _cboInterval = null!;
        private DateTimePicker _dtpNextDate = null!;
        private TextBox _txtCost = null!;
        private ComboBox _cboStatus = null!;
        private TextBox _txtDescription = null!;

        private List<EquipmentLookupItem> _equipments = new();

        public frmMaintenanceEdit(EquipmentMaintenanceDto? initial = null)
        {
            _original = initial;
            if (initial != null)
            {
                ResultItem = new EquipmentMaintenanceDto
                {
                    MaintenanceID = initial.MaintenanceID,
                    EquipmentID = initial.EquipmentID,
                    EquipmentCode = initial.EquipmentCode,
                    EquipmentName = initial.EquipmentName,
                    RoomID = initial.RoomID,
                    RoomName = initial.RoomName,
                    BranchID = initial.BranchID,
                    BranchName = initial.BranchName,
                    MaintenanceDate = initial.MaintenanceDate,
                    NextMaintenanceDate = initial.NextMaintenanceDate,
                    Description = initial.Description,
                    Cost = initial.Cost,
                    Status = initial.Status
                };
            }
            else
            {
                ResultItem = new EquipmentMaintenanceDto
                {
                    MaintenanceDate = DateTime.Now,
                    NextMaintenanceDate = DateTime.Now.AddMonths(6),
                    Cost = 500000,
                    Status = "Hoàn thành"
                };
            }

            _equipments = MaintenanceService.Instance.GetEquipmentsForDropdown();
            BuildUI();
            PopulateData();
        }

        private void BuildUI()
        {
            FormBorderStyle = FormBorderStyle.None;
            StartPosition = FormStartPosition.CenterParent;
            Size = new Size(680, 680);
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

            bool isEdit = _original != null && _original.MaintenanceID > 0;
            var lblTitle = new Label
            {
                Text = isEdit ? "🔧 CẬP NHẬT PHIẾU BẢO TRÌ - BẢO DƯỠNG" : "➕ LÊN LỊCH BẢO TRÌ THIẾT BỊ / ĐIỀU HÒA",
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

            // Row 1: Chọn Thiết bị
            AddLabel(pnlBody, "Chọn thiết bị cần bảo trì (Máy chạy, Giàn tạ, Hệ thống điều hòa) (*):", 0, y);
            y += 22;

            _cboEquipment = new ComboBox
            {
                Location = new Point(0, y),
                Size = new Size(630, 32),
                BackColor = ItemBg,
                ForeColor = TextColor,
                Font = new Font("Segoe UI", 9.5F),
                DropDownStyle = ComboBoxStyle.DropDownList,
                FlatStyle = FlatStyle.Flat
            };
            pnlBody.Controls.Add(_cboEquipment);
            y += 48;

            // Row 2: Ngày thực hiện bảo trì & Chu kỳ định kỳ
            AddLabel(pnlBody, "Ngày thực hiện bảo trì (*):", 0, y);
            AddLabel(pnlBody, "Chu kỳ bảo dưỡng định kỳ:", 320, y);
            y += 22;

            _dtpMaintenanceDate = new DateTimePicker
            {
                Location = new Point(0, y),
                Size = new Size(300, 32),
                Font = new Font("Segoe UI", 9.5F),
                Format = DateTimePickerFormat.Custom,
                CustomFormat = "dd/MM/yyyy"
            };
            _dtpMaintenanceDate.ValueChanged += (s, e) => RecalculateNextDate();
            pnlBody.Controls.Add(_dtpMaintenanceDate);

            _cboInterval = new ComboBox
            {
                Location = new Point(320, y),
                Size = new Size(310, 32),
                BackColor = ItemBg,
                ForeColor = TextColor,
                Font = new Font("Segoe UI", 9.5F),
                DropDownStyle = ComboBoxStyle.DropDownList,
                FlatStyle = FlatStyle.Flat
            };
            _cboInterval.Items.AddRange(new object[]
            {
                "1 tháng (Kiểm tra định kỳ hàng tháng)",
                "3 tháng (Bảo dưỡng định kỳ quý)",
                "6 tháng (Khuyến nghị máy Gym & Điều hòa)",
                "12 tháng (Đại tu tổng thể hàng năm)"
            });
            _cboInterval.SelectedIndex = 2; // Default 6 months
            _cboInterval.SelectedIndexChanged += (s, e) => RecalculateNextDate();
            pnlBody.Controls.Add(_cboInterval);
            y += 48;

            // Row 3: Ngày bảo trì kế tiếp & Chi phí
            AddLabel(pnlBody, "Hạn bảo trì định kỳ kế tiếp (*):", 0, y);
            AddLabel(pnlBody, "Chi phí bảo trì - tính vào chi phí vận hành (VNĐ) (*):", 320, y);
            y += 22;

            _dtpNextDate = new DateTimePicker
            {
                Location = new Point(0, y),
                Size = new Size(300, 32),
                Font = new Font("Segoe UI", 9.5F),
                Format = DateTimePickerFormat.Custom,
                CustomFormat = "dd/MM/yyyy"
            };
            pnlBody.Controls.Add(_dtpNextDate);

            _txtCost = new TextBox
            {
                Location = new Point(320, y),
                Size = new Size(310, 32),
                BackColor = ItemBg,
                ForeColor = Orange,
                Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                BorderStyle = BorderStyle.FixedSingle
            };
            pnlBody.Controls.Add(_txtCost);
            y += 48;

            // Row 4: Tình trạng bảo trì
            AddLabel(pnlBody, "Tình trạng xử lý bảo dưỡng (*):", 0, y);
            y += 22;

            _cboStatus = new ComboBox
            {
                Location = new Point(0, y),
                Size = new Size(630, 32),
                BackColor = ItemBg,
                ForeColor = TextColor,
                Font = new Font("Segoe UI", 9.5F),
                DropDownStyle = ComboBoxStyle.DropDownList,
                FlatStyle = FlatStyle.Flat
            };
            _cboStatus.Items.AddRange(new object[]
            {
                "Hoàn thành",
                "Đang xử lý",
                "Chờ bảo trì"
            });
            pnlBody.Controls.Add(_cboStatus);
            y += 48;

            // Row 5: Nội dung công việc bảo trì
            AddLabel(pnlBody, "Nội dung bảo trì, bảo dưỡng, thay thế phụ tùng & đơn vị kỹ thuật (*):", 0, y);
            y += 22;

            _txtDescription = new TextBox
            {
                Location = new Point(0, y),
                Size = new Size(630, 85),
                Multiline = true,
                BackColor = ItemBg,
                ForeColor = TextColor,
                Font = new Font("Segoe UI", 9.5F),
                BorderStyle = BorderStyle.FixedSingle,
                ScrollBars = ScrollBars.Vertical
            };
            pnlBody.Controls.Add(_txtDescription);
            y += 95;

            // Notice Box
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
                Text = "💡 Chi phí bảo trì sẽ được tự động kết chuyển vào báo cáo chi phí vận hành (OpEx) theo tháng/năm của từng chi nhánh.",
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
                Text = isEdit ? "💾 Cập nhật bảo trì" : "➕ Lưu phiếu bảo trì",
                Font = new Font("Segoe UI", 9.5F, FontStyle.Bold),
                ForeColor = Color.White,
                BackColor = Orange,
                Size = new Size(165, 40),
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

        private void PopulateData()
        {
            _cboEquipment.Items.Clear();
            _cboEquipment.DisplayMember = "DisplayText";
            _cboEquipment.ValueMember = "EquipmentID";

            foreach (var item in _equipments)
            {
                _cboEquipment.Items.Add(item);
            }

            if (_equipments.Any())
            {
                var match = _equipments.FirstOrDefault(e => e.EquipmentID == ResultItem.EquipmentID);
                if (match != null) _cboEquipment.SelectedItem = match;
                else _cboEquipment.SelectedIndex = 0;
            }

            _dtpMaintenanceDate.Value = ResultItem.MaintenanceDate > DateTime.MinValue ? ResultItem.MaintenanceDate : DateTime.Now;
            _dtpNextDate.Value = ResultItem.NextMaintenanceDate > DateTime.MinValue ? ResultItem.NextMaintenanceDate : DateTime.Now.AddMonths(6);
            _txtCost.Text = ResultItem.Cost.ToString("N0");
            _cboStatus.SelectedItem = !string.IsNullOrWhiteSpace(ResultItem.Status) ? ResultItem.Status : "Hoàn thành";
            _txtDescription.Text = ResultItem.Description;
        }

        private void RecalculateNextDate()
        {
            DateTime baseDate = _dtpMaintenanceDate.Value;
            int months = _cboInterval.SelectedIndex switch
            {
                0 => 1,
                1 => 3,
                2 => 6,
                3 => 12,
                _ => 6
            };
            _dtpNextDate.Value = baseDate.AddMonths(months);
        }

        private void BtnSave_Click(object? sender, EventArgs e)
        {
            if (_cboEquipment.SelectedItem is not EquipmentLookupItem selEq)
            {
                MessageBox.Show("Vui lòng chọn thiết bị bảo trì.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                _cboEquipment.Focus();
                return;
            }

            string costRaw = _txtCost.Text.Replace(".", "").Replace(",", "").Replace("₫", "").Trim();
            if (!decimal.TryParse(costRaw, out decimal cost) || cost < 0)
            {
                MessageBox.Show("Chi phí bảo trì không hợp lệ. Vui lòng nhập số tiền hợp lệ.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                _txtCost.Focus();
                return;
            }

            string desc = _txtDescription.Text.Trim();
            if (string.IsNullOrWhiteSpace(desc))
            {
                MessageBox.Show("Vui lòng nhập nội dung công việc bảo trì.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                _txtDescription.Focus();
                return;
            }

            ResultItem.EquipmentID = selEq.EquipmentID;
            ResultItem.EquipmentCode = selEq.EquipmentCode;
            ResultItem.EquipmentName = selEq.EquipmentName;
            ResultItem.MaintenanceDate = _dtpMaintenanceDate.Value;
            ResultItem.NextMaintenanceDate = _dtpNextDate.Value;
            ResultItem.Cost = cost;
            ResultItem.Status = _cboStatus.SelectedItem?.ToString() ?? "Hoàn thành";
            ResultItem.Description = desc;

            // Ghi nhận trực tiếp vào Backend
            bool isEdit = _original != null && _original.MaintenanceID > 0;
            if (isEdit)
            {
                var (success, msg) = MaintenanceService.Instance.UpdateMaintenance(ResultItem);
                if (!success)
                {
                    MessageBox.Show(msg, "Lỗi cập nhật", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }
            else
            {
                var (success, msg, newId) = MaintenanceService.Instance.AddMaintenance(ResultItem);
                if (!success)
                {
                    MessageBox.Show(msg, "Lỗi thêm mới", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                ResultItem.MaintenanceID = newId;
            }

            DialogResult = DialogResult.OK;
            Close();
        }
    }
}
