using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace QuanLyGym.UI.Forms.PT
{
    public partial class frmPTMain : Form
    {
        private ucLichTap? _viewLichTap;
        private ucHoiVien? _viewHoiVien;
        private ucTheoDoiTheChat? _viewTheoDoiTheChat;
        private int _currentTabIndex = 0; // 0: Lịch làm việc, 1: Nhật ký HV đã dạy, 2: InBody
        private int _pendingBookingCount = 3; // Số lượng thông báo đặt lịch chờ xác nhận

        public frmPTMain()
        {
            InitializeComponent();
            this.DoubleBuffered = true;
            this.StartPosition = FormStartPosition.CenterScreen;
            this.MinimumSize = new Size(1100, 680);
            this.Size = new Size(1380, 840);
            this.BackColor = PTUITheme.BgDark;

            SetupSidebar();
            InitViews();
            SwitchTab(0);
        }

        private void SetupSidebar()
        {
            pnlSidebar.Paint += PnlSidebar_Paint;
            pnlBrand.Paint += PnlBrand_Paint;
            pnlPTProfile.Paint += PnlPTProfile_Paint;
            pnlTopHeader.Paint += PnlTopHeader_Paint;

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

                // Badge số lượng thông báo
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

        private void InitViews()
        {
            _viewLichTap = new ucLichTap { Dock = DockStyle.Fill };
            _viewHoiVien = new ucHoiVien { Dock = DockStyle.Fill };
            _viewTheoDoiTheChat = new ucTheoDoiTheChat { Dock = DockStyle.Fill };

            // Liên kết sự kiện: Khi bấm "Xem Hồ Sơ & Lịch Sử" ở tab Hội Viên, chuyển sang tab Thể Chất & PR của hội viên đó
            _viewHoiVien.OnViewMemberHistoryRequested += (memberName) =>
            {
                SwitchTab(2);
                _viewTheoDoiTheChat?.SelectMemberByName(memberName);
            };

            pnlMainContainer.Controls.Add(_viewLichTap);
            pnlMainContainer.Controls.Add(_viewHoiVien);
            pnlMainContainer.Controls.Add(_viewTheoDoiTheChat);
        }

        public void SwitchTab(int tabIndex)
        {
            _currentTabIndex = tabIndex;

            _viewLichTap?.Hide();
            _viewHoiVien?.Hide();
            _viewTheoDoiTheChat?.Hide();

            UpdateButtonStyles();

            switch (tabIndex)
            {
                case 0:
                    lblHeaderTitle.Text = "LỊCH LÀM VIỆC & BUỔI TẬP 1-1";
                    _viewLichTap?.Show();
                    _viewLichTap?.BringToFront();
                    break;
                case 1:
                    lblHeaderTitle.Text = "NHẬT KÝ HỘI VIÊN ĐÃ DẠY";
                    _viewHoiVien?.Show();
                    _viewHoiVien?.BringToFront();
                    break;
                case 2:
                    lblHeaderTitle.Text = "TRA CỨU CHỈ SỐ INBODY HỘI VIÊN";
                    _viewTheoDoiTheChat?.Show();
                    _viewTheoDoiTheChat?.BringToFront();
                    break;
            }

            pnlTopHeader.Invalidate();
        }

        private void UpdateButtonStyles()
        {
            SetMenuButtonStyle(btnMenuSchedule, _currentTabIndex == 0);
            SetMenuButtonStyle(btnMenuMembers, _currentTabIndex == 1);
            SetMenuButtonStyle(btnMenuPhysical, _currentTabIndex == 2);
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

            // Brand Icon (Lime rounded square)
            Rectangle iconRect = new Rectangle(20, 22, 34, 34);
            using (SolidBrush b = new SolidBrush(PTUITheme.PrimaryLime))
            {
                PTUITheme.FillRoundedRectangle(g, b, iconRect, 8);
            }
            TextRenderer.DrawText(g, "T", new Font("Segoe UI", 16f, FontStyle.Bold), iconRect, Color.Black,
                TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter | TextFormatFlags.NoPrefix);

            // Title: TITAN GYM
            Rectangle titleRect = new Rectangle(62, 20, 170, 22);
            TextRenderer.DrawText(g, "TITAN GYM", new Font("Segoe UI", 13f, FontStyle.Bold), titleRect, PTUITheme.TextWhite,
                TextFormatFlags.Left | TextFormatFlags.VerticalCenter | TextFormatFlags.NoPrefix);

            // Subtitle: TRAINER SUITE
            Rectangle subRect = new Rectangle(62, 40, 170, 16);
            TextRenderer.DrawText(g, "TRAINER SUITE", PTUITheme.FontSmallBold, subRect, PTUITheme.PrimaryLime,
                TextFormatFlags.Left | TextFormatFlags.VerticalCenter | TextFormatFlags.NoPrefix);
        }

        private void PnlPTProfile_Paint(object? sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;

            Rectangle r = pnlPTProfile.ClientRectangle;
            PTUITheme.DrawCard(g, new Rectangle(14, 10, r.Width - 28, r.Height - 20), radius: 10,
                customBg: Color.FromArgb(19, 23, 31), customBorder: Color.FromArgb(35, 42, 56));

            // PT Avatar circle
            Rectangle avRect = new Rectangle(24, 20, 36, 36);
            using (SolidBrush b = new SolidBrush(Color.FromArgb(35, 45, 60)))
            {
                g.FillEllipse(b, avRect);
            }
            TextRenderer.DrawText(g, "TM", PTUITheme.FontBold, avRect, PTUITheme.PrimaryLime,
                TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter | TextFormatFlags.NoPrefix);

            // PT Name
            Rectangle nameRect = new Rectangle(68, 18, r.Width - 85, 20);
            TextRenderer.DrawText(g, "Tuấn Minh", PTUITheme.FontBold, nameRect, PTUITheme.TextWhite,
                TextFormatFlags.Left | TextFormatFlags.VerticalCenter | TextFormatFlags.NoPrefix);

            // PT Status
            Rectangle dotRect = new Rectangle(68, 43, 8, 8);
            using (SolidBrush bGreen = new SolidBrush(PTUITheme.Green))
            {
                g.FillEllipse(bGreen, dotRect);
            }
            Rectangle statusRect = new Rectangle(80, 38, r.Width - 95, 18);
            TextRenderer.DrawText(g, "Sẵn Sàng Nhận Lịch", PTUITheme.FontSmall, statusRect, PTUITheme.Green,
                TextFormatFlags.Left | TextFormatFlags.VerticalCenter | TextFormatFlags.NoPrefix);
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

        private void btnMenuMembers_Click(object sender, EventArgs e)
        {
            SwitchTab(1);
        }

        private void btnMenuPhysical_Click(object sender, EventArgs e)
        {
            SwitchTab(2);
        }

        private void btnNotificationBell_Click(object sender, EventArgs e)
        {
            using (var modal = new frmXacNhanLichModal())
            {
                modal.OnBookingsChanged += () =>
                {
                    _pendingBookingCount = modal.PendingCount;
                    btnNotificationBell.Invalidate();
                };
                modal.ShowDialog(this);
                _pendingBookingCount = modal.PendingCount;
                btnNotificationBell.Invalidate();
            }
        }
    }
}
