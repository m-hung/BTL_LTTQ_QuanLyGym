using QuanLyGym.UI.Helpers;
using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Windows.Forms;
using QuanLyGym.BLL.DTOs.PT;
using QuanLyGym.BLL.Helpers;
using QuanLyGym.BLL.Services.PT;

namespace QuanLyGym.UI.Forms.PT
{
    public partial class frmPTMain : Form
    {
        private readonly PTProfileService _profileService = new PTProfileService();
        private readonly PTBookingApprovalService _bookingService = new PTBookingApprovalService();

        private ucLichTap? _viewLichTap;
        private ucYeuCauDatLich? _viewYeuCauDatLich;
        private ucHoiVien? _viewHoiVien;
        private ucTheoDoiTheChat? _viewTheoDoiTheChat;
        private ucThongTinCaNhan? _viewThongTinCaNhan;

        private int _currentTabIndex = 0; // 0: Lịch, 1: Yêu cầu đặt lịch, 2: Nhật ký HV, 3: InBody, 4: Thông tin cá nhân
        private int _currentPTId = 1;
        private int _pendingBookingCount = 0;
        private PTProfileDto _currentPTProfile = new PTProfileDto();
        private Image? _sidebarAvatarImage;
        private bool _isProfileHovered;
        private readonly ToolTip _sidebarToolTip = new ToolTip();

        public frmPTMain()
        {
            InitializeComponent();
            this.DoubleBuffered = true;
            this.StartPosition = FormStartPosition.CenterScreen;
            this.MinimumSize = new Size(1100, 680);
            this.Size = new Size(1380, 840);
            this.BackColor = PTUITheme.BgDark;

            _currentPTId = _profileService.GetPTIdByAccountId(UserSession.AccountID);
            RefreshSidebarState();
            SetupSidebar();
            InitViews();
            SwitchTab(0);

            PTPortalSync.DataChanged += OnSharedDataChanged;
            this.FormClosed += (s, e) =>
            {
                PTPortalSync.DataChanged -= OnSharedDataChanged;
                _sidebarAvatarImage?.Dispose();
                _sidebarToolTip.Dispose();
            };
        }

        private void OnSharedDataChanged()
        {
            if (IsDisposed) return;
            if (InvokeRequired)
            {
                BeginInvoke(new Action(RefreshSidebarState));
                return;
            }
            RefreshSidebarState();
        }

        private void RefreshSidebarState()
        {
            _pendingBookingCount = _bookingService.GetPendingCount(_currentPTId);
            _currentPTProfile = _profileService.GetProfile(_currentPTId);

            _sidebarAvatarImage?.Dispose();
            _sidebarAvatarImage = null;
            if (!string.IsNullOrWhiteSpace(_currentPTProfile.AvatarPath) && File.Exists(_currentPTProfile.AvatarPath))
            {
                try
                {
                    byte[] bytes = File.ReadAllBytes(_currentPTProfile.AvatarPath);
                    using var ms = new MemoryStream(bytes);
                    _sidebarAvatarImage = Image.FromStream(ms);
                }
                catch
                {
                    _sidebarAvatarImage = null;
                }
            }

            btnMenuRequests?.Invalidate();
            btnNotificationBell?.Invalidate();
            pnlPTProfile?.Invalidate();

            string tipName = string.IsNullOrWhiteSpace(_currentPTProfile.FullName) ? "Tuấn Minh" : _currentPTProfile.FullName;
            string tipSpec = string.IsNullOrWhiteSpace(_currentPTProfile.Specialization) ? "Sẵn Sàng Nhận Lịch" : _currentPTProfile.Specialization;
            if (pnlPTProfile != null)
            {
                _sidebarToolTip.SetToolTip(pnlPTProfile, $"{tipName}\nChuyên môn: {tipSpec}\nNhấn để mở Thông Tin Cá Nhân");
            }
        }

        private void SetupSidebar()
        {
            pnlSidebar.Paint += PnlSidebar_Paint;
            pnlBrand.Paint += PnlBrand_Paint;
            pnlPTProfile.Paint += PnlPTProfile_Paint;
            pnlTopHeader.Paint += PnlTopHeader_Paint;

            // Cho phép bấm trực tiếp vào thẻ PT ở cuối sidebar để mở trang Thông Tin Cá Nhân
            pnlPTProfile.MouseEnter += (s, e) => { _isProfileHovered = true; pnlPTProfile.Invalidate(); };
            pnlPTProfile.MouseLeave += (s, e) => { _isProfileHovered = false; pnlPTProfile.Invalidate(); };
            pnlPTProfile.Click += (s, e) => SwitchTab(4);

            // Vẽ badge số lượng yêu cầu chờ duyệt trên nút menu Yêu Cầu Đặt Lịch
            btnMenuRequests.Paint += (s, pe) =>
            {
                if (_pendingBookingCount <= 0) return;

                pe.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                string badgeText = _pendingBookingCount > 99 ? "99+" : _pendingBookingCount.ToString();
                int badgeW = badgeText.Length > 2 ? 28 : 22;
                Rectangle badgeRect = new Rectangle(btnMenuRequests.Width - badgeW - 10, (btnMenuRequests.Height - 20) / 2, badgeW, 20);

                Color bgBadge = _currentTabIndex == 1 ? PTUITheme.PrimaryLime : Color.FromArgb(245, 158, 11);
                PTUITheme.DrawBadge(pe.Graphics, badgeText, badgeRect, bgBadge, Color.Black,
                    new Font("Segoe UI", 8f, FontStyle.Bold), 10);
            };

            // Nút Chuông Thông Báo
            btnNotificationBell.MouseEnter += (s, e) => { btnNotificationBell.BackColor = Color.FromArgb(36, 44, 62); btnNotificationBell.Invalidate(); };
            btnNotificationBell.MouseLeave += (s, e) => { btnNotificationBell.BackColor = Color.FromArgb(24, 28, 40); btnNotificationBell.Invalidate(); };
            btnNotificationBell.Paint += (s, pe) =>
            {
                pe.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                pe.Graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;
                Rectangle br = btnNotificationBell.ClientRectangle;

                PTUITheme.DrawCard(pe.Graphics, new Rectangle(br.X, br.Y, br.Width - 1, br.Height - 1), radius: 11,
                    customBg: btnNotificationBell.BackColor, customBorder: Color.FromArgb(55, 68, 96));

                int bellSize = 28;
                Rectangle bellRect = new Rectangle(br.X + (br.Width - bellSize) / 2 - 2, br.Y + (br.Height - bellSize) / 2 + 2, bellSize, bellSize);
                PTUITheme.DrawVectorBell(pe.Graphics, bellRect, PTUITheme.PrimaryLime);

                if (_pendingBookingCount > 0)
                {
                    int badgeSize = 22;
                    Rectangle badgeRect = new Rectangle(br.Right - badgeSize - 3, br.Y + 3, badgeSize, badgeSize);

                    using (Pen pBorder = new Pen(btnNotificationBell.BackColor, 2.2f))
                    {
                        pe.Graphics.DrawEllipse(pBorder, badgeRect);
                    }

                    using (SolidBrush bRed = new SolidBrush(Color.FromArgb(239, 68, 68)))
                    {
                        pe.Graphics.FillEllipse(bRed, badgeRect);
                    }

                    using (Font badgeFont = new Font("Segoe UI", 9.5f, FontStyle.Bold))
                    {
                        Rectangle textRect = new Rectangle(badgeRect.X, badgeRect.Y - 1, badgeRect.Width, badgeRect.Height);
                        TextRenderer.DrawText(pe.Graphics, _pendingBookingCount.ToString(), badgeFont, textRect, Color.White,
                            TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter | TextFormatFlags.NoPrefix);
                    }
                }
            };
        }

        private static string GetInitials(string name)
        {
            var parts = name.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            if (parts.Length == 0) return "PT";
            if (parts.Length == 1) return parts[0].Substring(0, Math.Min(2, parts[0].Length)).ToUpper();
            return (parts[^2][0].ToString() + parts[^1][0]).ToUpper();
        }

        private void InitViews()
        {
            _viewLichTap = new ucLichTap { Dock = DockStyle.Fill };
            _viewYeuCauDatLich = new ucYeuCauDatLich(_currentPTId) { Dock = DockStyle.Fill };
            _viewHoiVien = new ucHoiVien { Dock = DockStyle.Fill };
            _viewTheoDoiTheChat = new ucTheoDoiTheChat { Dock = DockStyle.Fill };
            _viewThongTinCaNhan = new ucThongTinCaNhan(_currentPTId) { Dock = DockStyle.Fill };

            _viewYeuCauDatLich.OnViewScheduleRequested += () => SwitchTab(0);

            _viewHoiVien.OnViewMemberHistoryRequested += (memberName) =>
            {
                SwitchTab(3);
                _viewTheoDoiTheChat?.SelectMemberByName(memberName);
            };

            _viewThongTinCaNhan.OnProfileSaved += RefreshSidebarState;

            pnlMainContainer.Controls.Add(_viewLichTap);
            pnlMainContainer.Controls.Add(_viewYeuCauDatLich);
            pnlMainContainer.Controls.Add(_viewHoiVien);
            pnlMainContainer.Controls.Add(_viewTheoDoiTheChat);
            pnlMainContainer.Controls.Add(_viewThongTinCaNhan);
        }

        public void SwitchTab(int tabIndex)
        {
            _currentTabIndex = tabIndex;

            _viewLichTap?.Hide();
            _viewYeuCauDatLich?.Hide();
            _viewHoiVien?.Hide();
            _viewTheoDoiTheChat?.Hide();
            _viewThongTinCaNhan?.Hide();

            UpdateButtonStyles();

            switch (tabIndex)
            {
                case 0:
                    lblHeaderTitle.Text = "LỊCH LÀM VIỆC & BUỔI TẬP 1-1";
                    _viewLichTap?.Show();
                    _viewLichTap?.BringToFront();
                    break;
                case 1:
                    lblHeaderTitle.Text = "YÊU CẦU ĐẶT LỊCH TỪ HỌC VIÊN";
                    _viewYeuCauDatLich?.RefreshView();
                    _viewYeuCauDatLich?.Show();
                    _viewYeuCauDatLich?.BringToFront();
                    break;
                case 2:
                    lblHeaderTitle.Text = "NHẬT KÝ HỘI VIÊN ĐÃ DẠY";
                    _viewHoiVien?.Show();
                    _viewHoiVien?.BringToFront();
                    break;
                case 3:
                    lblHeaderTitle.Text = "TRA CỨU CHỈ SỐ INBODY HỘI VIÊN";
                    _viewTheoDoiTheChat?.Show();
                    _viewTheoDoiTheChat?.BringToFront();
                    break;
                case 4:
                    lblHeaderTitle.Text = "THÔNG TIN CÁ NHÂN HUẤN LUYỆN VIÊN";
                    _viewThongTinCaNhan?.LoadProfileData();
                    _viewThongTinCaNhan?.Show();
                    _viewThongTinCaNhan?.BringToFront();
                    break;
            }

            pnlTopHeader.Invalidate();
            pnlPTProfile.Invalidate();
        }

        private void UpdateButtonStyles()
        {
            SetMenuButtonStyle(btnMenuSchedule, _currentTabIndex == 0);
            SetMenuButtonStyle(btnMenuRequests, _currentTabIndex == 1);
            SetMenuButtonStyle(btnMenuMembers, _currentTabIndex == 2);
            SetMenuButtonStyle(btnMenuPhysical, _currentTabIndex == 3);
            SetMenuButtonStyle(btnMenuProfile, _currentTabIndex == 4);
        }

        private void SetMenuButtonStyle(Button btn, bool isActive)
        {
            if (isActive)
            {
                btn.BackColor = Color.FromArgb(34, 45, 22);
                btn.ForeColor = PTUITheme.PrimaryLime;
                btn.FlatAppearance.BorderSize = 1;
                btn.FlatAppearance.BorderColor = PTUITheme.PrimaryLime;
                btn.Font = PTUITheme.FontBold;
            }
            else
            {
                btn.BackColor = Color.Transparent;
                btn.ForeColor = PTUITheme.TextMuted;
                btn.FlatAppearance.BorderSize = 0;
                btn.Font = PTUITheme.FontRegular;
            }
        }

        private void PnlBrand_Paint(object? sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;

            int h = pnlBrand.ClientSize.Height;
            int iconSize = 36;
            Rectangle iconRect = new Rectangle(18, (h - iconSize) / 2, iconSize, iconSize);
            using (SolidBrush b = new SolidBrush(PTUITheme.PrimaryLime))
            {
                PTUITheme.FillRoundedRectangle(g, b, iconRect, 8);
            }
            using var iconFont = new Font("Segoe UI", 15f, FontStyle.Bold);
            TextRenderer.DrawText(g, "T", iconFont, iconRect, Color.Black,
                TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter | TextFormatFlags.NoPrefix);

            using var titleFont = new Font("Segoe UI", 12.5f, FontStyle.Bold);
            using var subFont = new Font("Segoe UI", 8.25f, FontStyle.Bold);

            int titleH = TextRenderer.MeasureText(g, "TITAN GYM", titleFont, Size.Empty, TextFormatFlags.NoPadding | TextFormatFlags.NoPrefix).Height;
            int subH = TextRenderer.MeasureText(g, "TRAINER SUITE", subFont, Size.Empty, TextFormatFlags.NoPadding | TextFormatFlags.NoPrefix).Height;
            int gap = 3;
            int totalTextH = titleH + gap + subH;
            int startY = Math.Max(8, (h - totalTextH) / 2);

            int textX = iconRect.Right + 11;
            int textW = Math.Max(80, pnlBrand.ClientSize.Width - textX - 10);

            Rectangle titleRect = new Rectangle(textX, startY, textW, titleH);
            TextRenderer.DrawText(g, "TITAN GYM", titleFont, titleRect, PTUITheme.TextWhite,
                TextFormatFlags.Left | TextFormatFlags.Top | TextFormatFlags.NoPadding | TextFormatFlags.NoPrefix);

            Rectangle subRect = new Rectangle(textX + 1, startY + titleH + gap, textW, subH);
            TextRenderer.DrawText(g, "TRAINER SUITE", subFont, subRect, PTUITheme.PrimaryLime,
                TextFormatFlags.Left | TextFormatFlags.Top | TextFormatFlags.NoPadding | TextFormatFlags.NoPrefix);
        }

        private void PnlPTProfile_Paint(object? sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.PixelOffsetMode = PixelOffsetMode.HighQuality;

            Rectangle r = pnlPTProfile.ClientRectangle;
            bool isActiveProfile = _currentTabIndex == 4;

            using (Pen topLine = new Pen(Color.FromArgb(32, 38, 50), 1f))
            {
                g.DrawLine(topLine, 14, 1, r.Width - 14, 1);
            }

            Rectangle cardRect = new Rectangle(12, 9, r.Width - 24, r.Height - 18);
            if (isActiveProfile || _isProfileHovered)
            {
                Color bg = isActiveProfile ? Color.FromArgb(26, 33, 24) : Color.FromArgb(24, 28, 38);
                Color border = isActiveProfile ? PTUITheme.PrimaryLime : Color.FromArgb(52, 62, 82);
                PTUITheme.DrawCard(g, cardRect, radius: 8, customBg: bg, customBorder: border);
            }

            int avSize = 38;
            Rectangle avRect = new Rectangle(cardRect.X + 8, cardRect.Y + (cardRect.Height - avSize) / 2, avSize, avSize);

            if (_sidebarAvatarImage != null)
            {
                using GraphicsPath clip = PTUITheme.CreateRoundedPath(avRect, 8);
                g.SetClip(clip);
                g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                g.DrawImage(_sidebarAvatarImage, avRect);
                g.ResetClip();

                using Pen pAv = new Pen(isActiveProfile ? PTUITheme.PrimaryLime : Color.FromArgb(58, 68, 88), 1f);
                PTUITheme.DrawRoundedRectangle(g, pAv, avRect, 8);
            }
            else
            {
                using (SolidBrush b = new SolidBrush(Color.FromArgb(26, 31, 42)))
                {
                    PTUITheme.FillRoundedRectangle(g, b, avRect, 8);
                }
                using (Pen pAv = new Pen(isActiveProfile ? PTUITheme.PrimaryLime : Color.FromArgb(52, 62, 80), 1f))
                {
                    PTUITheme.DrawRoundedRectangle(g, pAv, avRect, 8);
                }
                TextRenderer.DrawText(g, PTDisplayHelper.Initials(_currentPTProfile.FullName), PTUITheme.FontBold, avRect, PTUITheme.PrimaryLime,
                    TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter | TextFormatFlags.NoPrefix);
            }

            // Chấm trạng thái online ở góc dưới phải avatar (không chiếm chiều ngang dòng text)
            Rectangle dotRect = new Rectangle(avRect.Right - 9, avRect.Bottom - 9, 9, 9);
            using (SolidBrush bRing = new SolidBrush(PTUITheme.BgSidebar))
                g.FillEllipse(bRing, dotRect.X - 2, dotRect.Y - 2, dotRect.Width + 4, dotRect.Height + 4);
            using (SolidBrush bGreen = new SolidBrush(PTUITheme.Green))
                g.FillEllipse(bGreen, dotRect);

            int textLeft = avRect.Right + 11;
            int textWidth = Math.Max(60, cardRect.Right - textLeft - 8);

            string displayName = string.IsNullOrWhiteSpace(_currentPTProfile.FullName) ? "Tuấn Minh" : _currentPTProfile.FullName;
            string specText = string.IsNullOrWhiteSpace(_currentPTProfile.Specialization)
                ? "Sẵn Sàng Nhận Lịch"
                : _currentPTProfile.Specialization;

            using var specFont = new Font("Segoe UI", 8.25f, FontStyle.Regular);
            int nameH = TextRenderer.MeasureText(g, displayName, PTUITheme.FontBold, Size.Empty, TextFormatFlags.NoPadding | TextFormatFlags.NoPrefix).Height;
            int specH = TextRenderer.MeasureText(g, specText, specFont, Size.Empty, TextFormatFlags.NoPadding | TextFormatFlags.NoPrefix).Height;
            int textGap = 3;
            int textStartY = cardRect.Y + Math.Max(4, (cardRect.Height - (nameH + textGap + specH)) / 2);

            Rectangle nameRect = new Rectangle(textLeft, textStartY, textWidth, nameH + 2);
            TextRenderer.DrawText(g, displayName, PTUITheme.FontBold, nameRect, PTUITheme.TextWhite,
                TextFormatFlags.Left | TextFormatFlags.Top | TextFormatFlags.EndEllipsis | TextFormatFlags.NoPadding | TextFormatFlags.NoPrefix);

            Rectangle statusRect = new Rectangle(textLeft, textStartY + nameH + textGap, textWidth, specH + 2);
            TextRenderer.DrawText(g, specText, specFont, statusRect, PTUITheme.TextMuted,
                TextFormatFlags.Left | TextFormatFlags.Top | TextFormatFlags.EndEllipsis | TextFormatFlags.NoPadding | TextFormatFlags.NoPrefix);
        }

        private void PnlSidebar_Paint(object? sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            using (Pen borderPen = new Pen(PTUITheme.CardBorder, 1))
            {
                g.DrawLine(borderPen, pnlSidebar.Width - 1, 0, pnlSidebar.Width - 1, pnlSidebar.Height);
            }
        }

        private void PnlTopHeader_Paint(object? sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            using (Pen borderPen = new Pen(Color.FromArgb(28, 33, 44), 1))
            {
                g.DrawLine(borderPen, 0, pnlTopHeader.Height - 1, pnlTopHeader.Width, pnlTopHeader.Height - 1);
            }
        }

        private void btnMenuSchedule_Click(object sender, EventArgs e)
        {
            SwitchTab(0);
        }

        private void btnMenuRequests_Click(object sender, EventArgs e)
        {
            SwitchTab(1);
        }

        private void btnMenuMembers_Click(object sender, EventArgs e)
        {
            SwitchTab(2);
        }

        private void btnMenuPhysical_Click(object sender, EventArgs e)
        {
            SwitchTab(3);
        }

        private void btnMenuProfile_Click(object sender, EventArgs e)
        {
            SwitchTab(4);
        }

        private void btnNotificationBell_Click(object sender, EventArgs e)
        {
            using (var modal = new frmXacNhanLichModal(_currentPTId))
            {
                modal.OnBookingsChanged += RefreshSidebarState;
                modal.ShowDialog(this);
                RefreshSidebarState();
            }
        }

        private void frmPTMain_Load(object? sender, EventArgs e)
        {
            //lblXinChao.Text = $"Xin chào, {UserSession.DisplayName}";
            //lblTenDangNhap.Text = $"Tên đăng nhập: {UserSession.Username}";
        }

        private void btnDangXuat_Click(object? sender, EventArgs e)
        {
            UserSession.Logout(this);
        }
    }
}
