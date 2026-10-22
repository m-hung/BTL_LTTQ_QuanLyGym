using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace QuanLyGym.UI.Forms.PT
{
    public static class PTUITheme
    {
        // Color Palette matching Figma Dark Neon Theme
        public static readonly Color BgDark = Color.FromArgb(15, 17, 21);        // #0F1115
        public static readonly Color BgSidebar = Color.FromArgb(11, 13, 16);     // #0B0D10
        public static readonly Color CardBg = Color.FromArgb(22, 25, 34);        // #161922
        public static readonly Color CardBorder = Color.FromArgb(40, 46, 62);    // #282E3E
        public static readonly Color CardHover = Color.FromArgb(28, 33, 46);     // #1C212E
        public static readonly Color PrimaryLime = Color.FromArgb(204, 255, 0);  // #CCFF00
        public static readonly Color LimeHover = Color.FromArgb(220, 255, 77);   // #DCFF4D
        public static readonly Color LimeDark = Color.FromArgb(34, 45, 22);      // #222D16
        public static readonly Color Cyan = Color.FromArgb(0, 210, 255);         // #00D2FF
        public static readonly Color CyanDark = Color.FromArgb(16, 42, 54);
        public static readonly Color Red = Color.FromArgb(244, 63, 94);          // #F43F5E
        public static readonly Color RedDark = Color.FromArgb(51, 21, 29);
        public static readonly Color Green = Color.FromArgb(34, 197, 94);
        public static readonly Color GreenDark = Color.FromArgb(20, 48, 30);
        public static readonly Color TextWhite = Color.FromArgb(255, 255, 255);
        public static readonly Color TextMuted = Color.FromArgb(142, 149, 165);
        public static readonly Color TextDarkMuted = Color.FromArgb(90, 96, 110);
        public static readonly Color InputBg = Color.FromArgb(27, 31, 42);       // #1B1F2A
        public static readonly Color InputBorder = Color.FromArgb(48, 54, 72);

        // Standard Fonts with proper weights
        public static readonly Font FontTitle = new Font("Segoe UI", 14f, FontStyle.Bold);
        public static readonly Font FontSection = new Font("Segoe UI", 11f, FontStyle.Bold);
        public static readonly Font FontSub = new Font("Segoe UI", 9f, FontStyle.Regular);
        public static readonly Font FontBold = new Font("Segoe UI", 9.5f, FontStyle.Bold);
        public static readonly Font FontRegular = new Font("Segoe UI", 9.5f, FontStyle.Regular);
        public static readonly Font FontSmall = new Font("Segoe UI", 8.5f, FontStyle.Regular);
        public static readonly Font FontSmallBold = new Font("Segoe UI", 8.5f, FontStyle.Bold);
        public static readonly Font FontStatNumber = new Font("Segoe UI", 22f, FontStyle.Bold);
        public static readonly Font FontStatUnit = new Font("Segoe UI", 12f, FontStyle.Bold);

        public static void StyleDarkComboBox(ComboBox cbo)
        {
            cbo.FlatStyle = FlatStyle.Flat;
            cbo.DropDownStyle = ComboBoxStyle.DropDownList;
            cbo.DrawMode = DrawMode.OwnerDrawFixed;
            cbo.BackColor = Color.FromArgb(20, 24, 33);
            cbo.ForeColor = Color.White;
            cbo.Font = FontRegular;
            cbo.ItemHeight = 28;

            cbo.DrawItem += (s, e) =>
            {
                if (e.Index < 0) return;
                Graphics g = e.Graphics;
                bool isSelected = (e.State & DrawItemState.Selected) == DrawItemState.Selected;
                Color bg = isSelected ? Color.FromArgb(36, 44, 60) : Color.FromArgb(20, 24, 33);
                Color fg = isSelected ? PrimaryLime : Color.White;

                using (SolidBrush b = new SolidBrush(bg))
                {
                    g.FillRectangle(b, e.Bounds);
                }

                string text = cbo.Items[e.Index]?.ToString() ?? "";
                Rectangle textRect = new Rectangle(e.Bounds.X + 8, e.Bounds.Y, e.Bounds.Width - 16, e.Bounds.Height);
                TextRenderer.DrawText(g, text, FontRegular, textRect, fg,
                    TextFormatFlags.Left | TextFormatFlags.VerticalCenter | TextFormatFlags.NoPrefix);
            };
        }

        public class DoubleBufferedPanel : Panel
        {
            public DoubleBufferedPanel()
            {
                this.DoubleBuffered = true;
                this.ResizeRedraw = true;
            }
        }

        public static GraphicsPath CreateRoundedPath(Rectangle rect, int radius)
        {
            GraphicsPath path = new GraphicsPath();
            if (radius <= 0)
            {
                path.AddRectangle(rect);
                return path;
            }

            int diameter = Math.Min(radius * 2, Math.Min(rect.Width, rect.Height));
            Rectangle arc = new Rectangle(rect.X, rect.Y, diameter, diameter);

            // Top-left
            path.AddArc(arc, 180, 90);

            // Top-right
            arc.X = rect.Right - diameter;
            path.AddArc(arc, 270, 90);

            // Bottom-right
            arc.Y = rect.Bottom - diameter;
            path.AddArc(arc, 0, 90);

            // Bottom-left
            arc.X = rect.Left;
            path.AddArc(arc, 90, 90);

            path.CloseFigure();
            return path;
        }

        public static void DrawRoundedRectangle(Graphics g, Pen pen, Rectangle rect, int radius)
        {
            g.SmoothingMode = SmoothingMode.AntiAlias;
            using (GraphicsPath path = CreateRoundedPath(rect, radius))
            {
                g.DrawPath(pen, path);
            }
        }

        public static void FillRoundedRectangle(Graphics g, Brush brush, Rectangle rect, int radius)
        {
            g.SmoothingMode = SmoothingMode.AntiAlias;
            using (GraphicsPath path = CreateRoundedPath(rect, radius))
            {
                g.FillPath(brush, path);
            }
        }

        public static void DrawCard(Graphics g, Rectangle bounds, int radius = 12, Color? customBg = null, Color? customBorder = null)
        {
            Color bg = customBg ?? CardBg;
            Color border = customBorder ?? CardBorder;

            using (SolidBrush b = new SolidBrush(bg))
            {
                FillRoundedRectangle(g, b, bounds, radius);
            }

            using (Pen p = new Pen(border, 1f))
            {
                DrawRoundedRectangle(g, p, bounds, radius);
            }
        }

        public static void DrawBadge(Graphics g, string text, Rectangle bounds, Color bg, Color fg, Font font, int radius = 6)
        {
            using (SolidBrush b = new SolidBrush(bg))
            {
                FillRoundedRectangle(g, b, bounds, radius);
            }

            TextRenderer.DrawText(g, text, font, bounds, fg,
                TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter | TextFormatFlags.SingleLine | TextFormatFlags.NoPrefix);
        }

        // ================= VECTOR ICONS FOR 100% CRISP RENDERING =================

        // 1. Vector User Icon (Silhouette)
        public static void DrawVectorUser(Graphics g, Rectangle bounds, Color color)
        {
            g.SmoothingMode = SmoothingMode.AntiAlias;
            int cx = bounds.X + bounds.Width / 2;
            int cy = bounds.Y + bounds.Height / 2;
            int headRadius = 4;

            using (SolidBrush b = new SolidBrush(color))
            {
                g.FillEllipse(b, cx - headRadius, cy - 8, headRadius * 2, headRadius * 2);

                using (GraphicsPath bodyPath = new GraphicsPath())
                {
                    bodyPath.AddArc(cx - 7, cy - 1, 14, 12, 180, 180);
                    bodyPath.CloseFigure();
                    g.FillPath(b, bodyPath);
                }
            }
        }

        // 2. Vector Dumbbell Icon
        public static void DrawVectorDumbbell(Graphics g, Rectangle bounds, Color color)
        {
            g.SmoothingMode = SmoothingMode.AntiAlias;
            int cx = bounds.X + bounds.Width / 2;
            int cy = bounds.Y + bounds.Height / 2;

            using (Pen p = new Pen(color, 2f))
            using (SolidBrush b = new SolidBrush(color))
            {
                g.DrawLine(p, cx - 7, cy, cx + 7, cy);
                FillRoundedRectangle(g, b, new Rectangle(cx - 9, cy - 5, 2, 10), 1);
                FillRoundedRectangle(g, b, new Rectangle(cx - 7, cy - 4, 2, 8), 1);
                FillRoundedRectangle(g, b, new Rectangle(cx + 7, cy - 5, 2, 10), 1);
                FillRoundedRectangle(g, b, new Rectangle(cx + 5, cy - 4, 2, 8), 1);
            }
        }

        // 3. Vector Calendar Icon
        public static void DrawVectorCalendar(Graphics g, Rectangle bounds, Color color)
        {
            g.SmoothingMode = SmoothingMode.AntiAlias;
            int cx = bounds.X + bounds.Width / 2;
            int cy = bounds.Y + bounds.Height / 2;
            Rectangle calRect = new Rectangle(cx - 7, cy - 7, 14, 14);

            using (Pen p = new Pen(color, 1.5f))
            using (SolidBrush b = new SolidBrush(color))
            {
                DrawRoundedRectangle(g, p, calRect, 3);
                g.DrawLine(p, calRect.Left, calRect.Top + 4, calRect.Right, calRect.Top + 4);
                g.DrawLine(p, calRect.Left + 3, calRect.Top - 2, calRect.Left + 3, calRect.Top + 1);
                g.DrawLine(p, calRect.Right - 3, calRect.Top - 2, calRect.Right - 3, calRect.Top + 1);
                g.FillRectangle(b, cx - 3, cy + 1, 2, 2);
                g.FillRectangle(b, cx + 1, cy + 1, 2, 2);
                g.FillRectangle(b, cx - 3, cy + 4, 2, 2);
                g.FillRectangle(b, cx + 1, cy + 4, 2, 2);
            }
        }

        // 4. Vector Clock Icon
        public static void DrawVectorClock(Graphics g, Rectangle bounds, Color color)
        {
            g.SmoothingMode = SmoothingMode.AntiAlias;
            int cx = bounds.X + bounds.Width / 2;
            int cy = bounds.Y + bounds.Height / 2;
            int r = 7;

            using (Pen p = new Pen(color, 1.5f))
            {
                g.DrawEllipse(p, cx - r, cy - r, r * 2, r * 2);
                g.DrawLine(p, cx, cy, cx, cy - 4);
                g.DrawLine(p, cx, cy, cx + 3, cy);
            }
        }
    }
}
