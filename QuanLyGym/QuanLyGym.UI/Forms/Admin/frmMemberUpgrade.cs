using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace QuanLyGym.UI.Forms.Admin
{
    public class frmMemberUpgrade : Form
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
        private static readonly Color Gold = Color.FromArgb(234, 179, 8);
        private static readonly Color Purple = Color.FromArgb(168, 85, 247);
        private static readonly CultureInfo ViCulture = new CultureInfo("vi-VN");

        private readonly ucAdminMember.MemberItem _member;
        private int _currentMode = 0; // 0: Nâng cấp hạng, 1: Gia hạn thẻ

        // Controls
        private Button _btnTabUpgrade = null!;
        private Button _btnTabRenew = null!;
        private Panel _pnlContainer = null!;

        // Upgrade Controls
        private ComboBox _cboTargetTier = null!;
        private ComboBox _cboUpgradeDuration = null!;
        private Label _lblNewTierPrice = null!;
        private Label _lblCreditAmount = null!;
        private Label _lblTotalUpgradePay = null!;
        private ComboBox _cboUpgradePayMethod = null!;
        private TextBox _txtUpgradeNote = null!;

        // Renew Controls
        private ComboBox _cboRenewDuration = null!;
        private Label _lblRenewOldEnd = null!;
        private Label _lblRenewNewEnd = null!;
        private ComboBox _cboRenewDiscount = null!;
        private Label _lblRenewFinalPay = null!;
        private ComboBox _cboRenewPayMethod = null!;

        public bool IsUpdated { get; private set; }

        public frmMemberUpgrade(ucAdminMember.MemberItem member, int initialTab = 0)
        {
            _member = member;
            _currentMode = initialTab;
            BuildUI();
            SwitchMode(_currentMode);
        }

        private void BuildUI()
        {
            FormBorderStyle = FormBorderStyle.None;
            StartPosition = FormStartPosition.CenterParent;
            Size = new Size(680, 680);
            BackColor = BgColor;
            ShowInTaskbar = false;
            DoubleBuffered = true;

            Paint += (s, e) =>
            {
                using var p = new Pen(Border, 1.5f);
                e.Graphics.DrawRectangle(p, 0, 0, Width - 1, Height - 1);
            };

            // 1. Header Bar
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
                Text = "⚡  NÂNG CẤP HẠNG THẺ & GIA HẠN HỘI VIÊN",
                Font = new Font("Segoe UI", 11.5F, FontStyle.Bold),
                ForeColor = TextColor,
                AutoSize = true,
                Location = new Point(20, 12)
            };

            var lblSub = new Label
            {
                Text = $"Hội viên: {_member.FullName} ({_member.Code}) • Hạng hiện tại: {_member.Tier}",
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

            // 2. Member Summary Ribbon
            var pnlRibbon = new Panel
            {
                Dock = DockStyle.Top,
                Height = 70,
                BackColor = CardBg,
                Padding = new Padding(20, 12, 20, 10)
            };
            pnlRibbon.Paint += (s, e) =>
            {
                using var p = new Pen(Border);
                e.Graphics.DrawLine(p, 0, pnlRibbon.Height - 1, pnlRibbon.Width, pnlRibbon.Height - 1);
            };

            int daysLeft = _member.DaysRemaining;
            string remainText = daysLeft > 0 ? $"Còn {daysLeft} ngày" : "Đã hết hạn";
            Color remainColor = daysLeft > 15 ? Green : (daysLeft > 0 ? Orange : Color.FromArgb(239, 68, 68));

            var lblRibbonLeft = new Label
            {
                Text = $"👤 {_member.FullName}  •  📞 {_member.Phone}  •  🏢 {_member.BranchName}",
                Font = new Font("Segoe UI", 9F, FontStyle.Bold),
                ForeColor = TextColor,
                Location = new Point(20, 12),
                AutoSize = true
            };

            var lblRibbonRight = new Label
            {
                Text = $"Hạng: {_member.Tier}  |  Hạn: {_member.EndDate:dd/MM/yyyy}  |  Trạng thái: {remainText}",
                Font = new Font("Segoe UI", 8.5F),
                ForeColor = remainColor,
                Location = new Point(20, 38),
                AutoSize = true
            };
            pnlRibbon.Controls.Add(lblRibbonLeft);
            pnlRibbon.Controls.Add(lblRibbonRight);

            // 3. Mode Toggle Buttons (Nâng cấp hạng thẻ | Gia hạn thẻ tập)
            var pnlModeBar = new Panel
            {
                Dock = DockStyle.Top,
                Height = 48,
                BackColor = ItemBg,
                Padding = new Padding(20, 6, 20, 6)
            };
            pnlModeBar.Paint += (s, e) =>
            {
                using var p = new Pen(Border);
                e.Graphics.DrawLine(p, 0, pnlModeBar.Height - 1, pnlModeBar.Width, pnlModeBar.Height - 1);
            };

            _btnTabUpgrade = CreateModeBtn("⭐ 1. Nâng cấp hạng thẻ", 0);
            _btnTabRenew = CreateModeBtn("🔄 2. Gia hạn thời gian tập", 1);
            _btnTabUpgrade.Location = new Point(20, 7);
            _btnTabRenew.Location = new Point(220, 7);

            pnlModeBar.Controls.Add(_btnTabUpgrade);
            pnlModeBar.Controls.Add(_btnTabRenew);

            // 4. Dynamic Content Container
            _pnlContainer = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = BgColor,
                Padding = new Padding(20, 16, 20, 16)
            };

            Controls.Add(_pnlContainer);
            Controls.Add(pnlModeBar);
            Controls.Add(pnlRibbon);
            Controls.Add(pnlTitle);
        }

        private Button CreateModeBtn(string text, int mode)
        {
            var btn = new Button
            {
                Text = text,
                Font = new Font("Segoe UI", 9F, FontStyle.Bold),
                Size = new Size(190, 34),
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand,
                UseMnemonic = false,
                UseVisualStyleBackColor = false
            };
            btn.FlatAppearance.BorderSize = 1;
            btn.Click += (s, e) => SwitchMode(mode);
            return btn;
        }

        private void SwitchMode(int mode)
        {
            _currentMode = mode;
            _btnTabUpgrade.ForeColor = (mode == 0) ? Color.White : MutedColor;
            _btnTabUpgrade.BackColor = (mode == 0) ? Orange : CardBg;
            _btnTabUpgrade.FlatAppearance.BorderColor = (mode == 0) ? Orange : Border;

            _btnTabRenew.ForeColor = (mode == 1) ? Color.White : MutedColor;
            _btnTabRenew.BackColor = (mode == 1) ? Blue : CardBg;
            _btnTabRenew.FlatAppearance.BorderColor = (mode == 1) ? Blue : Border;

            _pnlContainer.Controls.Clear();
            if (mode == 0)
            {
                BuildUpgradeView();
            }
            else
            {
                BuildRenewView();
            }
        }

        // =====================================================================
        // VIEW 1: NÂNG CẤP HẠNG THẺ (UPGRADE)
        // =====================================================================
        private void BuildUpgradeView()
        {
            var pnl = new Panel { Dock = DockStyle.Fill, BackColor = Color.Transparent, AutoScroll = true };

            int y = 0;

            // Hướng dẫn
            var lblHelp = new Label
            {
                Text = "Chọn hạng thẻ cao hơn bạn muốn nâng cấp cho hội viên. Hệ thống sẽ tự động trừ đi giá trị thời gian còn lại của thẻ hiện tại để ra số tiền chênh lệch cần thanh toán.",
                Font = new Font("Segoe UI", 8.5F),
                ForeColor = MutedColor,
                Location = new Point(0, y),
                Size = new Size(620, 36)
            };
            pnl.Controls.Add(lblHelp);
            y += 42;

            // Box chọn hạng thẻ mới
            var pnlBox = new Panel
            {
                Location = new Point(0, y),
                Size = new Size(620, 270),
                BackColor = CardBg,
                Padding = new Padding(16, 14, 16, 14)
            };
            pnlBox.Paint += (s, e) =>
            {
                using var p = new Pen(Border);
                e.Graphics.DrawRectangle(p, 0, 0, pnlBox.Width - 1, pnlBox.Height - 1);
            };

            int by = 12;

            // Hạng hiện tại vs Hạng mới
            var lblCurTierTitle = new Label { Text = "Hạng thẻ hiện tại:", Font = new Font("Segoe UI", 8.5F), ForeColor = MutedColor, Location = new Point(14, by), AutoSize = true };
            var lblCurTierVal = new Label { Text = $"{_member.Tier} ({GetTierPrice(_member.Tier):N0} ₫/tháng)", Font = new Font("Segoe UI", 9.5F, FontStyle.Bold), ForeColor = TextColor, Location = new Point(160, by - 2), AutoSize = true };
            pnlBox.Controls.Add(lblCurTierTitle);
            pnlBox.Controls.Add(lblCurTierVal);
            by += 32;

            var lblTargetTierTitle = new Label { Text = "Hạng thẻ nâng cấp:", Font = new Font("Segoe UI", 8.5F, FontStyle.Bold), ForeColor = Orange, Location = new Point(14, by + 4), AutoSize = true };
            _cboTargetTier = new ComboBox
            {
                Location = new Point(160, by),
                Size = new Size(220, 28),
                DropDownStyle = ComboBoxStyle.DropDownList,
                BackColor = ItemBg,
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 9.5F),
                FlatStyle = FlatStyle.Flat
            };
            _cboTargetTier.Items.AddRange(new object[] { "Silver (750k/tháng)", "Gold (1.200k/tháng)", "Platinum (2.000k/tháng)", "VIP Diamond (3.500k/tháng)" });
            
            // Chọn mặc định hạng kế tiếp
            if (_member.Tier == "Classic") _cboTargetTier.SelectedIndex = 1; // Gold
            else if (_member.Tier == "Silver") _cboTargetTier.SelectedIndex = 1; // Gold
            else if (_member.Tier == "Gold") _cboTargetTier.SelectedIndex = 2; // Platinum
            else _cboTargetTier.SelectedIndex = 3; // VIP Diamond

            pnlBox.Controls.Add(lblTargetTierTitle);
            pnlBox.Controls.Add(_cboTargetTier);
            by += 36;

            var lblDurTitle = new Label { Text = "Thời hạn gói nâng cấp:", Font = new Font("Segoe UI", 8.5F), ForeColor = MutedColor, Location = new Point(14, by + 4), AutoSize = true };
            _cboUpgradeDuration = new ComboBox
            {
                Location = new Point(160, by),
                Size = new Size(220, 28),
                DropDownStyle = ComboBoxStyle.DropDownList,
                BackColor = ItemBg,
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 9F),
                FlatStyle = FlatStyle.Flat
            };
            _cboUpgradeDuration.Items.AddRange(new object[] { "1 Tháng", "3 Tháng", "6 Tháng (Giảm 10%)", "12 Tháng (Giảm 15%)" });
            _cboUpgradeDuration.SelectedIndex = 2; // 6 Tháng

            pnlBox.Controls.Add(lblDurTitle);
            pnlBox.Controls.Add(_cboUpgradeDuration);
            by += 44;

            // Tính toán chi phí
            var sep = new Panel { Location = new Point(14, by), Size = new Size(590, 1), BackColor = Border };
            pnlBox.Controls.Add(sep);
            by += 10;

            var lblLine1 = new Label { Text = "Giá gói mới niêm yết:", Font = new Font("Segoe UI", 8.5F), ForeColor = MutedColor, Location = new Point(14, by), AutoSize = true };
            _lblNewTierPrice = new Label { Text = "0 ₫", Font = new Font("Segoe UI", 9F, FontStyle.Bold), ForeColor = TextColor, Location = new Point(440, by), AutoSize = true };
            pnlBox.Controls.Add(lblLine1);
            pnlBox.Controls.Add(_lblNewTierPrice);
            by += 26;

            var lblLine2 = new Label { Text = $"Khấu trừ thẻ cũ ({_member.DaysRemaining} ngày còn lại):", Font = new Font("Segoe UI", 8.5F), ForeColor = MutedColor, Location = new Point(14, by), AutoSize = true };
            _lblCreditAmount = new Label { Text = "-0 ₫", Font = new Font("Segoe UI", 9F, FontStyle.Bold), ForeColor = Green, Location = new Point(440, by), AutoSize = true };
            pnlBox.Controls.Add(lblLine2);
            pnlBox.Controls.Add(_lblCreditAmount);
            by += 28;

            var lblLine3 = new Label { Text = "TỔNG TIỀN NÂNG CẤP CẦN THU:", Font = new Font("Segoe UI", 9.5F, FontStyle.Bold), ForeColor = Orange, Location = new Point(14, by), AutoSize = true };
            _lblTotalUpgradePay = new Label { Text = "0 ₫", Font = new Font("Segoe UI", 12F, FontStyle.Bold), ForeColor = Orange, Location = new Point(436, by - 4), AutoSize = true };
            pnlBox.Controls.Add(lblLine3);
            pnlBox.Controls.Add(_lblTotalUpgradePay);

            _cboTargetTier.SelectedIndexChanged += (s, e) => RecalculateUpgrade();
            _cboUpgradeDuration.SelectedIndexChanged += (s, e) => RecalculateUpgrade();

            pnl.Controls.Add(pnlBox);
            y += 280;

            // Phương thức thanh toán
            var pnlPay = new Panel
            {
                Location = new Point(0, y),
                Size = new Size(620, 80),
                BackColor = CardBg,
                Padding = new Padding(16, 12, 16, 12)
            };
            pnlPay.Paint += (s, e) =>
            {
                using var p = new Pen(Border);
                e.Graphics.DrawRectangle(p, 0, 0, pnlPay.Width - 1, pnlPay.Height - 1);
            };

            var lblPayTitle = new Label { Text = "Hình thức thanh toán:", Font = new Font("Segoe UI", 8.5F, FontStyle.Bold), ForeColor = TextColor, Location = new Point(14, 12), AutoSize = true };
            _cboUpgradePayMethod = new ComboBox
            {
                Location = new Point(160, 8),
                Size = new Size(220, 28),
                DropDownStyle = ComboBoxStyle.DropDownList,
                BackColor = ItemBg,
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 9F),
                FlatStyle = FlatStyle.Flat
            };
            _cboUpgradePayMethod.Items.AddRange(new object[] { "Tiền mặt tại quầy", "Chuyển khoản VietQR", "Quẹt thẻ POS / Thẻ tín dụng", "Ví MoMo / VNPay" });
            _cboUpgradePayMethod.SelectedIndex = 1; // VietQR

            var lblNoteTitle = new Label { Text = "Ghi chú nâng cấp:", Font = new Font("Segoe UI", 8.5F), ForeColor = MutedColor, Location = new Point(14, 44), AutoSize = true };
            _txtUpgradeNote = new TextBox
            {
                Location = new Point(160, 42),
                Size = new Size(430, 24),
                BackColor = ItemBg,
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 9F),
                BorderStyle = BorderStyle.FixedSingle,
                Text = "Khách hàng nâng cấp trải nghiệm bể bơi và xông hơi Sauna."
            };

            pnlPay.Controls.Add(lblPayTitle);
            pnlPay.Controls.Add(_cboUpgradePayMethod);
            pnlPay.Controls.Add(lblNoteTitle);
            pnlPay.Controls.Add(_txtUpgradeNote);

            pnl.Controls.Add(pnlPay);
            y += 92;

            // Nút bấm lưu
            var btnConfirm = new Button
            {
                Text = "⚡  XÁC NHẬN NÂNG CẤP HẠNG THẺ",
                Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                ForeColor = Color.White,
                BackColor = Orange,
                Size = new Size(320, 42),
                Location = new Point(150, y),
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            btnConfirm.FlatAppearance.BorderSize = 0;
            btnConfirm.Click += BtnConfirmUpgrade_Click;
            pnl.Controls.Add(btnConfirm);

            _pnlContainer.Controls.Add(pnl);
            RecalculateUpgrade();
        }

        private void RecalculateUpgrade()
        {
            if (_cboTargetTier == null || _cboUpgradeDuration == null) return;

            string targetName = _cboTargetTier.SelectedItem?.ToString() ?? "";
            string targetTier = "Gold";
            if (targetName.Contains("Silver")) targetTier = "Silver";
            else if (targetName.Contains("Gold")) targetTier = "Gold";
            else if (targetName.Contains("Platinum")) targetTier = "Platinum";
            else if (targetName.Contains("VIP Diamond")) targetTier = "VIP Diamond";

            int months = 6;
            if (_cboUpgradeDuration.SelectedIndex == 0) months = 1;
            else if (_cboUpgradeDuration.SelectedIndex == 1) months = 3;
            else if (_cboUpgradeDuration.SelectedIndex == 2) months = 6;
            else months = 12;

            decimal monthlyPrice = GetTierPrice(targetTier);
            decimal rawTotal = monthlyPrice * months;
            if (months == 6) rawTotal *= 0.9m;
            else if (months == 12) rawTotal *= 0.85m;

            // Khấu trừ thẻ cũ
            decimal oldMonthly = GetTierPrice(_member.Tier);
            decimal oldPerDay = oldMonthly / 30m;
            decimal credit = Math.Round(oldPerDay * _member.DaysRemaining, 0);

            decimal finalPay = Math.Max(0, rawTotal - credit);

            _lblNewTierPrice.Text = rawTotal.ToString("N0", ViCulture) + " ₫";
            _lblCreditAmount.Text = "-" + credit.ToString("N0", ViCulture) + " ₫";
            _lblTotalUpgradePay.Text = finalPay.ToString("N0", ViCulture) + " ₫";
        }

        private void BtnConfirmUpgrade_Click(object? sender, EventArgs e)
        {
            string targetName = _cboTargetTier.SelectedItem?.ToString() ?? "";
            string targetTier = "Gold";
            if (targetName.Contains("Silver")) targetTier = "Silver";
            else if (targetName.Contains("Gold")) targetTier = "Gold";
            else if (targetName.Contains("Platinum")) targetTier = "Platinum";
            else if (targetName.Contains("VIP Diamond")) targetTier = "VIP Diamond";

            int months = 6;
            if (_cboUpgradeDuration.SelectedIndex == 0) months = 1;
            else if (_cboUpgradeDuration.SelectedIndex == 1) months = 3;
            else if (_cboUpgradeDuration.SelectedIndex == 2) months = 6;
            else months = 12;

            string amount = _lblTotalUpgradePay.Text;

            string msg = $"Xác nhận nâng cấp hội viên '{_member.FullName}'\n" +
                         $"• Từ hạng: {_member.Tier} ➔ Hạng mới: {targetTier}\n" +
                         $"• Thời hạn gia hạn mới: {months} tháng\n" +
                         $"• Tổng tiền thanh toán: {amount}\n" +
                         $"• Phương thức: {_cboUpgradePayMethod.SelectedItem}\n\n" +
                         "Tiếp tục xử lý giao dịch?";

            if (MessageBox.Show(msg, "Xác nhận nâng cấp thẻ", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                _member.Tier = targetTier;
                _member.PackageName = $"Gói {targetTier} {months} Tháng";
                // Đặt hạn mới: từ hôm nay cộng số tháng
                _member.StartDate = DateTime.Today;
                _member.EndDate = DateTime.Today.AddMonths(months);
                _member._isSuspended = false;

                IsUpdated = true;
                MessageBox.Show($"Chúc mừng! Hội viên {_member.FullName} đã được nâng cấp thành công lên hạng {targetTier}!", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
        }

        // =====================================================================
        // VIEW 2: GIA HẠN THẺ TẬP (RENEW)
        // =====================================================================
        private void BuildRenewView()
        {
            var pnl = new Panel { Dock = DockStyle.Fill, BackColor = Color.Transparent, AutoScroll = true };

            int y = 0;

            var lblHelp = new Label
            {
                Text = "Gia hạn thêm thời gian tập cho hội viên với hạng thẻ hiện tại. Ngày hết hạn mới sẽ tự động cộng dồn tiếp tục từ hạn cũ (nếu còn hạn) hoặc bắt đầu từ hôm nay (nếu đã hết hạn).",
                Font = new Font("Segoe UI", 8.5F),
                ForeColor = MutedColor,
                Location = new Point(0, y),
                Size = new Size(620, 36)
            };
            pnl.Controls.Add(lblHelp);
            y += 42;

            var pnlBox = new Panel
            {
                Location = new Point(0, y),
                Size = new Size(620, 270),
                BackColor = CardBg,
                Padding = new Padding(16, 14, 16, 14)
            };
            pnlBox.Paint += (s, e) =>
            {
                using var p = new Pen(Border);
                e.Graphics.DrawRectangle(p, 0, 0, pnlBox.Width - 1, pnlBox.Height - 1);
            };

            int by = 12;

            var lblT1 = new Label { Text = "Hạng thẻ gia hạn:", Font = new Font("Segoe UI", 8.5F), ForeColor = MutedColor, Location = new Point(14, by), AutoSize = true };
            var lblT1V = new Label { Text = $"{_member.Tier} (Đơn giá: {GetTierPrice(_member.Tier):N0} ₫/tháng)", Font = new Font("Segoe UI", 9.5F, FontStyle.Bold), ForeColor = Blue, Location = new Point(160, by - 2), AutoSize = true };
            pnlBox.Controls.Add(lblT1);
            pnlBox.Controls.Add(lblT1V);
            by += 32;

            var lblDur = new Label { Text = "Thời gian gia hạn thêm:", Font = new Font("Segoe UI", 8.5F, FontStyle.Bold), ForeColor = TextColor, Location = new Point(14, by + 4), AutoSize = true };
            _cboRenewDuration = new ComboBox
            {
                Location = new Point(160, by),
                Size = new Size(220, 28),
                DropDownStyle = ComboBoxStyle.DropDownList,
                BackColor = ItemBg,
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 9.5F),
                FlatStyle = FlatStyle.Flat
            };
            _cboRenewDuration.Items.AddRange(new object[] { "1 Tháng", "3 Tháng", "6 Tháng (+ Tặng 15 ngày)", "12 Tháng (+ Tặng 1 tháng)" });
            _cboRenewDuration.SelectedIndex = 1; // 3 Tháng
            pnlBox.Controls.Add(lblDur);
            pnlBox.Controls.Add(_cboRenewDuration);
            by += 36;

            var lblDisc = new Label { Text = "Ưu đãi khách hàng:", Font = new Font("Segoe UI", 8.5F), ForeColor = MutedColor, Location = new Point(14, by + 4), AutoSize = true };
            _cboRenewDiscount = new ComboBox
            {
                Location = new Point(160, by),
                Size = new Size(220, 28),
                DropDownStyle = ComboBoxStyle.DropDownList,
                BackColor = ItemBg,
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 9F),
                FlatStyle = FlatStyle.Flat
            };
            _cboRenewDiscount.Items.AddRange(new object[] { "Không giảm giá (0%)", "Khách hàng thân thiết (Giảm 5%)", "Tri ân hội viên VIP (Giảm 10%)", "Voucher sinh nhật / Khuyến mãi (Giảm 15%)" });
            _cboRenewDiscount.SelectedIndex = 1; // 5%
            pnlBox.Controls.Add(lblDisc);
            pnlBox.Controls.Add(_cboRenewDiscount);
            by += 44;

            var sep = new Panel { Location = new Point(14, by), Size = new Size(590, 1), BackColor = Border };
            pnlBox.Controls.Add(sep);
            by += 10;

            var lblCurEndT = new Label { Text = "Ngày hết hạn hiện tại:", Font = new Font("Segoe UI", 8.5F), ForeColor = MutedColor, Location = new Point(14, by), AutoSize = true };
            _lblRenewOldEnd = new Label { Text = $"{_member.EndDate:dd/MM/yyyy}", Font = new Font("Segoe UI", 9F, FontStyle.Bold), ForeColor = TextColor, Location = new Point(440, by), AutoSize = true };
            pnlBox.Controls.Add(lblCurEndT);
            pnlBox.Controls.Add(_lblRenewOldEnd);
            by += 26;

            var lblNewEndT = new Label { Text = "Ngày hết hạn mới sau khi gia hạn:", Font = new Font("Segoe UI", 8.5F, FontStyle.Bold), ForeColor = Green, Location = new Point(14, by), AutoSize = true };
            _lblRenewNewEnd = new Label { Text = "...", Font = new Font("Segoe UI", 10F, FontStyle.Bold), ForeColor = Green, Location = new Point(440, by - 2), AutoSize = true };
            pnlBox.Controls.Add(lblNewEndT);
            pnlBox.Controls.Add(_lblRenewNewEnd);
            by += 28;

            var lblPayT = new Label { Text = "TỔNG TIỀN GIA HẠN CẦN THANH TOÁN:", Font = new Font("Segoe UI", 9.5F, FontStyle.Bold), ForeColor = Blue, Location = new Point(14, by), AutoSize = true };
            _lblRenewFinalPay = new Label { Text = "0 ₫", Font = new Font("Segoe UI", 12F, FontStyle.Bold), ForeColor = Blue, Location = new Point(436, by - 4), AutoSize = true };
            pnlBox.Controls.Add(lblPayT);
            pnlBox.Controls.Add(_lblRenewFinalPay);

            _cboRenewDuration.SelectedIndexChanged += (s, e) => RecalculateRenew();
            _cboRenewDiscount.SelectedIndexChanged += (s, e) => RecalculateRenew();

            pnl.Controls.Add(pnlBox);
            y += 280;

            var pnlPay = new Panel
            {
                Location = new Point(0, y),
                Size = new Size(620, 60),
                BackColor = CardBg,
                Padding = new Padding(16, 14, 16, 14)
            };
            pnlPay.Paint += (s, e) =>
            {
                using var p = new Pen(Border);
                e.Graphics.DrawRectangle(p, 0, 0, pnlPay.Width - 1, pnlPay.Height - 1);
            };

            var lblPayTitle = new Label { Text = "Hình thức thanh toán:", Font = new Font("Segoe UI", 8.5F, FontStyle.Bold), ForeColor = TextColor, Location = new Point(14, 16), AutoSize = true };
            _cboRenewPayMethod = new ComboBox
            {
                Location = new Point(160, 12),
                Size = new Size(220, 28),
                DropDownStyle = ComboBoxStyle.DropDownList,
                BackColor = ItemBg,
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 9F),
                FlatStyle = FlatStyle.Flat
            };
            _cboRenewPayMethod.Items.AddRange(new object[] { "Tiền mặt tại quầy", "Chuyển khoản VietQR", "Quẹt thẻ POS / Thẻ tín dụng", "Ví MoMo / VNPay" });
            _cboRenewPayMethod.SelectedIndex = 1;

            pnlPay.Controls.Add(lblPayTitle);
            pnlPay.Controls.Add(_cboRenewPayMethod);
            pnl.Controls.Add(pnlPay);
            y += 72;

            var btnConfirmRenew = new Button
            {
                Text = "🔄  XÁC NHẬN GIA HẠN THẺ TẬP",
                Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                ForeColor = Color.White,
                BackColor = Blue,
                Size = new Size(320, 42),
                Location = new Point(150, y),
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            btnConfirmRenew.FlatAppearance.BorderSize = 0;
            btnConfirmRenew.Click += BtnConfirmRenew_Click;
            pnl.Controls.Add(btnConfirmRenew);

            _pnlContainer.Controls.Add(pnl);
            RecalculateRenew();
        }

        private void RecalculateRenew()
        {
            if (_cboRenewDuration == null || _cboRenewDiscount == null) return;

            int months = 3;
            if (_cboRenewDuration.SelectedIndex == 0) months = 1;
            else if (_cboRenewDuration.SelectedIndex == 1) months = 3;
            else if (_cboRenewDuration.SelectedIndex == 2) months = 6;
            else months = 12;

            decimal discountPercent = 0.05m;
            if (_cboRenewDiscount.SelectedIndex == 0) discountPercent = 0m;
            else if (_cboRenewDiscount.SelectedIndex == 1) discountPercent = 0.05m;
            else if (_cboRenewDiscount.SelectedIndex == 2) discountPercent = 0.10m;
            else discountPercent = 0.15m;

            decimal monthlyPrice = GetTierPrice(_member.Tier);
            decimal rawTotal = monthlyPrice * months;
            decimal finalPay = Math.Round(rawTotal * (1m - discountPercent), 0);

            // Tính ngày mới
            DateTime baseDate = _member.EndDate > DateTime.Today ? _member.EndDate : DateTime.Today;
            DateTime newEnd = baseDate.AddMonths(months);
            if (months == 6) newEnd = newEnd.AddDays(15);
            else if (months == 12) newEnd = newEnd.AddDays(30);

            _lblRenewNewEnd.Text = $"{newEnd:dd/MM/yyyy}";
            _lblRenewFinalPay.Text = finalPay.ToString("N0", ViCulture) + " ₫";
        }

        private void BtnConfirmRenew_Click(object? sender, EventArgs e)
        {
            int months = 3;
            if (_cboRenewDuration.SelectedIndex == 0) months = 1;
            else if (_cboRenewDuration.SelectedIndex == 1) months = 3;
            else if (_cboRenewDuration.SelectedIndex == 2) months = 6;
            else months = 12;

            DateTime baseDate = _member.EndDate > DateTime.Today ? _member.EndDate : DateTime.Today;
            DateTime newEnd = baseDate.AddMonths(months);
            if (months == 6) newEnd = newEnd.AddDays(15);
            else if (months == 12) newEnd = newEnd.AddDays(30);

            string msg = $"Xác nhận gia hạn cho hội viên '{_member.FullName}'\n" +
                         $"• Hạng thẻ: {_member.Tier}\n" +
                         $"• Thêm thời gian: {months} tháng\n" +
                         $"• Hạn mới: {newEnd:dd/MM/yyyy}\n" +
                         $"• Tổng tiền: {_lblRenewFinalPay.Text}\n" +
                         $"• Hình thức: {_cboRenewPayMethod.SelectedItem}\n\n" +
                         "Hoàn tất thanh toán gia hạn?";

            if (MessageBox.Show(msg, "Xác nhận gia hạn", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                _member.EndDate = newEnd;
                _member._isSuspended = false;

                IsUpdated = true;
                MessageBox.Show($"Gia hạn thẻ thành công cho {_member.FullName}! Hạn mới tới ngày {newEnd:dd/MM/yyyy}.", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
        }

        private static decimal GetTierPrice(string tier)
        {
            switch (tier)
            {
                case "Silver": return 750000;
                case "Gold": return 1200000;
                case "Platinum": return 2000000;
                case "VIP Diamond": return 3500000;
                default: return 450000; // Classic
            }
        }
    }
}
