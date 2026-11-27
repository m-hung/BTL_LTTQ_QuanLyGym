using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using QuanLyGym.BLL.Models;
using QuanLyGym.BLL.Services;

namespace QuanLyGym.UI.Forms.Admin
{
    public class frmPackageEdit : Form
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
        private static readonly Color Green = Color.FromArgb(34, 197, 94);
        private static readonly Color Red = Color.FromArgb(239, 68, 68);

        private readonly PackageDto? _original;
        public PackageDto ResultPackage { get; private set; } = new();

        private TextBox _txtName = null!;
        private TextBox _txtPrice = null!;
        private TextBox _txtDuration = null!;
        private ComboBox _cboServiceType = null!;
        private TextBox _txtPTSessions = null!;
        private ComboBox _cboStatus = null!;
        private TextBox _txtDescription = null!;
        private TextBox _txtBenefits = null!;

        public frmPackageEdit(PackageDto? initial = null)
        {
            _original = initial;
            if (initial != null)
            {
                ResultPackage = new PackageDto
                {
                    PackageID = initial.PackageID,
                    PackageName = initial.PackageName,
                    Duration = initial.Duration,
                    Price = initial.Price,
                    TotalPTSessions = initial.TotalPTSessions,
                    IsPTPackage = initial.IsPTPackage,
                    Benefits = initial.Benefits,
                    Description = initial.Description,
                    Status = initial.Status,
                    BranchID = initial.BranchID,
                    BranchName = initial.BranchName,
                    SubscriberCount = initial.SubscriberCount,
                    TotalRevenue = initial.TotalRevenue
                };
            }
            else
            {
                ResultPackage = new PackageDto
                {
                    Duration = 30,
                    Price = 500000,
                    IsPTPackage = false,
                    TotalPTSessions = 0,
                    Status = true,
                    Description = "Gói tập tiêu chuẩn không giới hạn khung giờ, sử dụng đầy đủ hệ thống máy móc hiện đại.",
                    Benefits = "Tập luyện toàn hệ thống, Tủ đồ Smart Locker, Phòng tắm nóng lạnh"
                };
            }

            BuildUI();
            PopulateData();
        }

        private void BuildUI()
        {
            FormBorderStyle = FormBorderStyle.None;
            StartPosition = FormStartPosition.CenterParent;
            Size = new Size(680, 690);
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

            bool isEdit = _original != null && _original.PackageID > 0;
            var lblTitle = new Label
            {
                Text = isEdit ? "📦 CHỈNH SỬA THÔNG TIN GÓI TẬP" : "➕ THÊM MỚI GÓI TẬP GYM",
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

            // Content Flow
            var pnlBody = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.Transparent,
                Padding = new Padding(0, 10, 0, 0)
            };
            pnlContainer.Controls.Add(pnlBody);
            pnlBody.BringToFront();

            int y = 5;

            // Row 1: Tên gói tập
            AddLabel(pnlBody, "Tên gói tập (*) (ví dụ: Gói Đồng (Basic 1 Tháng), Gói Vàng...):", 0, y);
            y += 22;

            _txtName = CreateTextBox(0, y, 630);
            pnlBody.Controls.Add(_txtName);
            y += 48;

            // Row 2: Thời hạn (ngày) & Giá niêm yết (VNĐ)
            AddLabel(pnlBody, "Thời hạn sử dụng (ngày) (*):", 0, y);
            AddLabel(pnlBody, "Giá niêm yết (VNĐ) (*):", 320, y);
            y += 22;

            _txtDuration = CreateTextBox(0, y, 300);
            _txtPrice = CreateTextBox(320, y, 310);
            _txtPrice.ForeColor = Orange;
            _txtPrice.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            pnlBody.Controls.Add(_txtDuration);
            pnlBody.Controls.Add(_txtPrice);
            y += 48;

            // Row 3: Dịch vụ kèm PT & Số buổi PT
            AddLabel(pnlBody, "Dịch vụ Huấn luyện viên (PT) (*):", 0, y);
            AddLabel(pnlBody, "Số buổi tập kèm PT (nếu có):", 320, y);
            y += 22;

            _cboServiceType = new ComboBox
            {
                Location = new Point(0, y),
                Size = new Size(300, 32),
                BackColor = ItemBg,
                ForeColor = TextColor,
                Font = new Font("Segoe UI", 9.5F),
                DropDownStyle = ComboBoxStyle.DropDownList,
                FlatStyle = FlatStyle.Flat
            };
            _cboServiceType.Items.AddRange(new object[]
            {
                "Tự tập (Không có PT)",
                "Kèm PT (Có HLV riêng 1-1)"
            });
            _cboServiceType.SelectedIndex = 0;
            _cboServiceType.SelectedIndexChanged += (s, e) =>
            {
                bool isPT = _cboServiceType.SelectedIndex == 1;
                _txtPTSessions.Enabled = isPT;
                if (!isPT) _txtPTSessions.Text = "0";
                else if (_txtPTSessions.Text == "0") _txtPTSessions.Text = "12";
            };
            pnlBody.Controls.Add(_cboServiceType);

            _txtPTSessions = CreateTextBox(320, y, 310);
            _txtPTSessions.Text = "0";
            _txtPTSessions.Enabled = false;
            pnlBody.Controls.Add(_txtPTSessions);
            y += 48;

            // Row 4: Trạng thái mở bán
            AddLabel(pnlBody, "Trạng thái kinh doanh:", 0, y);
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
                "Đang mở bán",
                "Tạm dừng mở bán"
            });
            _cboStatus.SelectedIndex = 0;
            pnlBody.Controls.Add(_cboStatus);
            y += 48;

            // Row 5: Mô tả tóm tắt gói tập
            AddLabel(pnlBody, "Mô tả tóm tắt gói tập (hiển thị trên thẻ gói tập) (*):", 0, y);
            y += 22;

            _txtDescription = new TextBox
            {
                Location = new Point(0, y),
                Size = new Size(630, 65),
                Multiline = true,
                BackColor = ItemBg,
                ForeColor = TextColor,
                Font = new Font("Segoe UI", 9.5F),
                BorderStyle = BorderStyle.FixedSingle,
                ScrollBars = ScrollBars.Vertical
            };
            pnlBody.Controls.Add(_txtDescription);
            y += 75;

            // Row 6: Quyền lợi chi tiết
            AddLabel(pnlBody, "Quyền lợi chi tiết & Tiện ích đi kèm (ngăn cách bằng dấu phẩy):", 0, y);
            y += 22;

            _txtBenefits = new TextBox
            {
                Location = new Point(0, y),
                Size = new Size(630, 55),
                Multiline = true,
                BackColor = ItemBg,
                ForeColor = TextColor,
                Font = new Font("Segoe UI", 9.5F),
                BorderStyle = BorderStyle.FixedSingle,
                ScrollBars = ScrollBars.Vertical
            };
            pnlBody.Controls.Add(_txtBenefits);
            y += 68;

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
                Text = isEdit ? "💾 Cập nhật gói tập" : "➕ Thêm gói tập",
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

        private void PopulateData()
        {
            _txtName.Text = ResultPackage.DisplayTitle;
            _txtDuration.Text = ResultPackage.Duration.ToString();
            _txtPrice.Text = ResultPackage.Price.ToString("N0");
            _cboServiceType.SelectedIndex = ResultPackage.IsPTPackage ? 1 : 0;
            _txtPTSessions.Text = ResultPackage.TotalPTSessions.ToString();
            _txtPTSessions.Enabled = ResultPackage.IsPTPackage;
            _cboStatus.SelectedIndex = ResultPackage.Status ? 0 : 1;
            _txtDescription.Text = ResultPackage.Description;
            _txtBenefits.Text = ResultPackage.Benefits;
        }

        private void BtnSave_Click(object? sender, EventArgs e)
        {
            string name = _txtName.Text.Trim();
            if (string.IsNullOrWhiteSpace(name))
            {
                MessageBox.Show("Vui lòng nhập tên gói tập.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                _txtName.Focus();
                return;
            }

            if (!int.TryParse(_txtDuration.Text.Trim(), out int duration) || duration <= 0)
            {
                MessageBox.Show("Thời hạn gói tập phải là số nguyên ngày lớn hơn 0.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                _txtDuration.Focus();
                return;
            }

            string priceRaw = _txtPrice.Text.Replace(".", "").Replace(",", "").Replace("VNĐ", "").Replace("₫", "").Trim();
            if (!decimal.TryParse(priceRaw, out decimal price) || price <= 0)
            {
                MessageBox.Show("Giá niêm yết không hợp lệ. Vui lòng nhập số tiền hợp lệ.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                _txtPrice.Focus();
                return;
            }

            bool isPT = _cboServiceType.SelectedIndex == 1;
            int ptSessions = 0;
            if (isPT)
            {
                int.TryParse(_txtPTSessions.Text.Trim(), out ptSessions);
            }

            bool status = _cboStatus.SelectedIndex == 0;
            string desc = _txtDescription.Text.Trim();
            string benefits = _txtBenefits.Text.Trim();

            ResultPackage.PackageName = name;
            ResultPackage.Duration = duration;
            ResultPackage.Price = price;
            ResultPackage.IsPTPackage = isPT;
            ResultPackage.TotalPTSessions = ptSessions;
            ResultPackage.Status = status;
            ResultPackage.Description = desc;
            ResultPackage.Benefits = benefits;

            // Thực thi Backend trực tiếp
            bool isEdit = _original != null && _original.PackageID > 0;
            if (isEdit)
            {
                var (success, msg) = PackageService.Instance.UpdatePackage(ResultPackage);
                if (!success)
                {
                    MessageBox.Show(msg, "Lỗi cập nhật", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }
            else
            {
                var (success, msg, newId) = PackageService.Instance.AddPackage(ResultPackage);
                if (!success)
                {
                    MessageBox.Show(msg, "Lỗi thêm mới", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                ResultPackage.PackageID = newId;
            }

            DialogResult = DialogResult.OK;
            Close();
        }
    }
}
