using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Windows.Forms;

namespace QuanLyGym.UI.Forms.Admin.Controls
{
    public class AdminTopBarControl : Control
    {
        private readonly System.Windows.Forms.Timer _clockTimer;
        private string _currentTime = "";
        private bool _isLogoutHovered = false;
        private Rectangle _logoutRect;

        public event EventHandler? LogoutClicked;

        public AdminTopBarControl()
        {
            SetStyle(ControlStyles.AllPaintingInWmPaint |
                     ControlStyles.UserPaint |
                     ControlStyles.OptimizedDoubleBuffer |
                     ControlStyles.ResizeRedraw, true);
            DoubleBuffered = true;
            BackColor = AdminTheme.HeaderBg;
            Height = 56;
            Dock = DockStyle.Top;

            UpdateClock();

            _clockTimer = new System.Windows.Forms.Timer { Interval = 1000 };
            _clockTimer.Tick += (s, e) => UpdateClock();
            _clockTimer.Start();
        }

        private void UpdateClock()
        {
            _currentTime = DateTime.Now.ToString("dd/MM/yyyy — HH:mm");
            Invalidate();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _clockTimer?.Stop();
                _clockTimer?.Dispose();
            }
            base.Dispose(disposing);
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
            bool hovered = _logoutRect.Contains(e.Location);
            if (hovered != _isLogoutHovered)
            {
                _isLogoutHovered = hovered;
                Cursor = hovered ? Cursors.Hand : Cursors.Default;
                Invalidate();
            }
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            base.OnMouseLeave(e);
            if (_isLogoutHovered)
            {
                _isLogoutHovered = false;
                Cursor = Cursors.Default;
                Invalidate();
            }
        }

        protected override void OnMouseClick(MouseEventArgs e)
        {
            base.OnMouseClick(e);
            if (_logoutRect.Contains(e.Location))
            {
                LogoutClicked?.Invoke(this, EventArgs.Empty);
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            var g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;

            // Background
            using (var brushBg = new SolidBrush(AdminTheme.HeaderBg))
            {
                g.FillRectangle(brushBg, ClientRectangle);
            }

            // Bottom border
            using (var penBorder = new Pen(Color.FromArgb(28, 30, 38), 1f))
            {
                g.DrawLine(penBorder, 0, Height - 1, Width, Height - 1);
            }

            // 1. LEFT: Gymfit Logo & Title
            int padLeft = 20;

            // Logo Box
            var logoRect = new Rectangle(padLeft, 12, 32, 32);
            using (var path = AdminTheme.GetRoundedRectanglePath(logoRect, 6))
            using (var brushLogo = new SolidBrush(AdminTheme.PrimaryOrange))
            {
                g.FillPath(brushLogo, path);

                // 'G' Letter
                using (var brushG = new SolidBrush(Color.White))
                using (var fontG = new Font("Segoe UI", 16F, FontStyle.Bold))
                {
                    var sfG = new StringFormat
                    {
                        Alignment = StringAlignment.Center,
                        LineAlignment = StringAlignment.Center
                    };
                    g.DrawString("G", fontG, brushG, logoRect, sfG);
                }
            }

            // GYMFIT text
            using (var brushGym = new SolidBrush(AdminTheme.PrimaryOrange))
            using (var fontGym = new Font("Segoe UI", 12F, FontStyle.Bold))
            {
                g.DrawString("GYMFIT", fontGym, brushGym, padLeft + 42, 11);
            }

            // ADMIN PORTAL text
            using (var brushPortal = new SolidBrush(AdminTheme.TextMuted))
            using (var fontPortal = new Font("Segoe UI", 7F, FontStyle.Bold))
            {
                g.DrawString("ADMIN PORTAL", fontPortal, brushPortal, padLeft + 43, 30);
            }

            // 2. RIGHT SIDE: Clock, Bell, User Profile, Logout
            int rightX = Width - 20;

            // Logout Button (rightmost)
            string logoutText = "Đăng xuất";
            int logoutW = 90;
            _logoutRect = new Rectangle(rightX - logoutW, 14, logoutW, 28);

            Color logoutColor = _isLogoutHovered ? AdminTheme.PrimaryOrange : AdminTheme.TextMuted;
            using (var brushLogout = new SolidBrush(logoutColor))
            {
                // Logout Icon [->
                DrawLogoutIcon(g, _logoutRect.X + 4, _logoutRect.Y + 7, logoutColor);

                // Logout Text
                var sfLogout = new StringFormat
                {
                    Alignment = StringAlignment.Near,
                    LineAlignment = StringAlignment.Center
                };
                g.DrawString(logoutText, AdminTheme.FontSmall, brushLogout, new Rectangle(_logoutRect.X + 24, _logoutRect.Y, logoutW - 24, 28), sfLogout);
            }

            rightX -= logoutW + 16;

            // Divider line
            using (var penDiv = new Pen(Color.FromArgb(40, 42, 52), 1f))
            {
                g.DrawLine(penDiv, rightX, 18, rightX, Height - 18);
            }

            rightX -= 16;

            // User Info
            string userName = "Nguyễn Quản Trị";
            string userRole = "Super Admin";

            int userBlockW = 145;
            int userX = rightX - userBlockW;

            // User Avatar box
            var avatarRect = new Rectangle(userX, 14, 28, 28);
            using (var path = AdminTheme.GetRoundedRectanglePath(avatarRect, 5))
            using (var brushAvatar = new SolidBrush(Color.FromArgb(70, 40, 18)))
            using (var penAvatar = new Pen(AdminTheme.PrimaryOrange, 1f))
            {
                g.FillPath(brushAvatar, path);
                g.DrawPath(penAvatar, path);

                // Mini user icon or initials
                DrawUserSilhouette(g, avatarRect, AdminTheme.PrimaryOrange);
            }

            // Name
            using (var brushName = new SolidBrush(AdminTheme.TextWhite))
            {
                g.DrawString(userName, AdminTheme.FontBodyBold, brushName, userX + 36, 12);
            }

            // Role
            using (var brushRole = new SolidBrush(AdminTheme.TextMuted))
            {
                g.DrawString(userRole, AdminTheme.FontSmall, brushRole, userX + 36, 28);
            }

            rightX = userX - 20;

            // Notification Bell
            int bellX = rightX - 24;
            int bellY = 17;
            DrawBellIcon(g, bellX, bellY);

            rightX = bellX - 24;

            // Clock Display
            using (var brushClock = new SolidBrush(AdminTheme.TextMuted))
            {
                var sfClock = new StringFormat
                {
                    Alignment = StringAlignment.Far,
                    LineAlignment = StringAlignment.Center
                };
                var clockRect = new Rectangle(rightX - 180, 14, 180, 28);
                g.DrawString(_currentTime, AdminTheme.FontBody, brushClock, clockRect, sfClock);
            }
        }

        private void DrawLogoutIcon(Graphics g, int x, int y, Color color)
        {
            using (var pen = new Pen(color, 1.6f))
            {
                // Bracket [
                g.DrawLine(pen, x + 6, y, x + 1, y);
                g.DrawLine(pen, x + 1, y, x + 1, y + 14);
                g.DrawLine(pen, x + 1, y + 14, x + 6, y + 14);

                // Arrow ->
                g.DrawLine(pen, x + 5, y + 7, x + 13, y + 7);
                g.DrawLine(pen, x + 10, y + 4, x + 13, y + 7);
                g.DrawLine(pen, x + 10, y + 10, x + 13, y + 7);
            }
        }

        private void DrawBellIcon(Graphics g, int x, int y)
        {
            using (var pen = new Pen(AdminTheme.TextMuted, 1.5f))
            {
                // Bell body
                g.DrawArc(pen, x + 3, y + 2, 10, 8, 180, 180);
                g.DrawLine(pen, x + 3, y + 6, x + 1, y + 12);
                g.DrawLine(pen, x + 13, y + 6, x + 15, y + 12);
                g.DrawLine(pen, x + 1, y + 12, x + 15, y + 12);
                // Bell clapper
                g.DrawArc(pen, x + 6, y + 12, 4, 3, 0, 180);
            }

            // Red dot badge
            using (var brushDot = new SolidBrush(Color.FromArgb(239, 68, 68)))
            {
                g.FillEllipse(brushDot, x + 12, y + 1, 6, 6);
            }
        }

        private void DrawUserSilhouette(Graphics g, Rectangle rect, Color color)
        {
            using (var brush = new SolidBrush(color))
            {
                // Head
                g.FillEllipse(brush, rect.X + 9, rect.Y + 5, 10, 10);
                // Shoulders
                g.FillPie(brush, rect.X + 4, rect.Y + 14, 20, 16, 180, 180);
            }
        }
    }
}
