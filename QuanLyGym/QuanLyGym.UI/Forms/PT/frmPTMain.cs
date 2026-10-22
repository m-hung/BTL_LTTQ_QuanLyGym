using QuanLyGym.UI.Helpers;
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
        private int _currentTabIndex = 0; // 0: Lịch làm việc, 1: Hội viên, 2: Thể chất

        public frmPTMain()
        {
            InitializeComponent();
            this.DoubleBuffered = true;
            this.StartPosition = FormStartPosition.CenterScreen;
            this.MinimumSize = new Size(1280, 800);
            this.Size = new Size(1440, 880);
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

            btnCreateSession.MouseEnter += (s, e) => { btnCreateSession.BackColor = PTUITheme.LimeHover; btnCreateSession.Invalidate(); };
            btnCreateSession.MouseLeave += (s, e) => { btnCreateSession.BackColor = PTUITheme.PrimaryLime; btnCreateSession.Invalidate(); };
            btnCreateSession.Paint += (s, pe) =>
            {
                pe.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                Rectangle br = btnCreateSession.ClientRectangle;
                using (SolidBrush sb = new SolidBrush(btnCreateSession.BackColor))
                {
                    PTUITheme.FillRoundedRectangle(pe.Graphics, sb, br, 8);
                }
                TextRenderer.DrawText(pe.Graphics, btnCreateSession.Text, btnCreateSession.Font, br, Color.Black,
                    TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter | TextFormatFlags.NoPrefix);
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
                    lblHeaderTitle.Text = "HỘI VIÊN PHỤ TRÁCH & GÓI TẬP";
                    _viewHoiVien?.Show();
                    _viewHoiVien?.BringToFront();
                    break;
                case 2:
                    lblHeaderTitle.Text = "THEO DÕI THỂ CHẤT, INBODY & MỨC TẠ";
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

            // PT Status (Green dot + Sẵn Sàng Nhận Lịch - Không bị cắt chữ)
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

        private void btnCreateSession_Click(object sender, EventArgs e)
        {
            using (var modal = new frmDatLichModal())
            {
                modal.ShowDialog(this);
            }
        }

        private void frmPTMain_Load(object? sender, EventArgs e)
        {
            lblXinChao.Text = $"Xin chào, {UserSession.DisplayName}";
            lblTenDangNhap.Text = $"Tên đăng nhập: {UserSession.Username}";
        }

        private void btnDangXuat_Click(object? sender, EventArgs e)
        {
            UserSession.Logout(this);
        }
    }
}
