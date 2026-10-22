using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace QuanLyGym.UI.Utils
{
    public static class UITheme
    {
        // Primary Palette - Modern Indigo & Royal Blue Accent
        public static readonly Color Primary = Color.FromArgb(79, 70, 229);       // #4F46E5 - Vibrant Indigo
        public static readonly Color PrimaryDark = Color.FromArgb(67, 56, 202);   // #4338CA
        public static readonly Color PrimaryLight = Color.FromArgb(238, 242, 255);// #EEF2FF
        public static readonly Color Secondary = Color.FromArgb(14, 165, 233);    // #0EA5E9 - Sky Blue

        // Dark Sidebar
        public static readonly Color SidebarBg = Color.FromArgb(24, 28, 38);       // #181C26 - Deep Slate
        public static readonly Color SidebarHover = Color.FromArgb(36, 42, 56);    // #242A38
        public static readonly Color SidebarActive = Color.FromArgb(79, 70, 229);   // #4F46E5
        public static readonly Color SidebarText = Color.FromArgb(156, 163, 175);  // #9CA3AF
        public static readonly Color SidebarTextActive = Color.White;

        // Backgrounds & Surfaces
        public static readonly Color Background = Color.FromArgb(248, 250, 252);   // #F8FAFC
        public static readonly Color CardBg = Color.White;
        public static readonly Color Border = Color.FromArgb(226, 232, 240);       // #E2E8F0
        public static readonly Color BorderLight = Color.FromArgb(241, 245, 249);  // #F1F5F9

        // Typography Colors
        public static readonly Color TextDark = Color.FromArgb(15, 23, 42);        // #0F172A - Slate 900
        public static readonly Color TextRegular = Color.FromArgb(51, 65, 85);     // #334155 - Slate 700
        public static readonly Color TextMuted = Color.FromArgb(100, 116, 139);    // #64748B - Slate 500

        // Status Colors
        public static readonly Color Success = Color.FromArgb(16, 185, 129);      // #10B981 - Emerald
        public static readonly Color SuccessLight = Color.FromArgb(236, 253, 245);
        public static readonly Color Warning = Color.FromArgb(245, 158, 11);      // #F59E0B - Amber
        public static readonly Color WarningLight = Color.FromArgb(254, 243, 199);
        public static readonly Color Danger = Color.FromArgb(239, 68, 68);        // #EF4444 - Rose
        public static readonly Color DangerLight = Color.FromArgb(254, 242, 242);
        public static readonly Color Info = Color.FromArgb(59, 130, 246);          // #3B82F6 - Blue
        public static readonly Color InfoLight = Color.FromArgb(239, 246, 255);

        // Fonts
        public static readonly Font FontTitle = new Font("Segoe UI", 16f, FontStyle.Bold);
        public static readonly Font FontSubtitle = new Font("Segoe UI", 12f, FontStyle.Bold);
        public static readonly Font FontSection = new Font("Segoe UI", 11f, FontStyle.Bold);
        public static readonly Font FontRegular = new Font("Segoe UI", 9.5f, FontStyle.Regular);
        public static readonly Font FontBold = new Font("Segoe UI", 9.5f, FontStyle.Bold);
        public static readonly Font FontSmall = new Font("Segoe UI", 8.5f, FontStyle.Regular);
        public static readonly Font FontSmallBold = new Font("Segoe UI", 8.5f, FontStyle.Bold);
        public static readonly Font FontNumber = new Font("Segoe UI", 20f, FontStyle.Bold);
        public static readonly Font FontStat = new Font("Segoe UI", 13f, FontStyle.Bold);

        // Helper to create rounded path
        public static GraphicsPath GetRoundedPath(Rectangle rect, int radius)
        {
            GraphicsPath path = new GraphicsPath();
            float diameter = radius * 2f;
            RectangleF arc = new RectangleF(rect.X, rect.Y, diameter, diameter);

            if (radius <= 0)
            {
                path.AddRectangle(rect);
                return path;
            }

            path.AddArc(arc, 180, 90);
            arc.X = rect.Right - diameter;
            path.AddArc(arc, 270, 90);
            arc.Y = rect.Bottom - diameter;
            path.AddArc(arc, 0, 90);
            arc.X = rect.Left;
            path.AddArc(arc, 90, 90);
            path.CloseFigure();
            return path;
        }

        // Apply modern styling to DataGridView
        public static void StyleDataGridView(DataGridView dgv)
        {
            dgv.BackgroundColor = Color.White;
            dgv.BorderStyle = BorderStyle.None;
            dgv.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            dgv.GridColor = Color.FromArgb(241, 245, 249);
            dgv.EnableHeadersVisualStyles = false;
            dgv.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            dgv.RowHeadersVisible = false;
            dgv.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgv.MultiSelect = false;
            dgv.AllowUserToAddRows = false;
            dgv.AllowUserToDeleteRows = false;
            dgv.AllowUserToResizeRows = false;
            dgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgv.RowTemplate.Height = 44;
            dgv.Font = FontRegular;

            // Column Header Style
            dgv.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(248, 250, 252);
            dgv.ColumnHeadersDefaultCellStyle.ForeColor = TextDark;
            dgv.ColumnHeadersDefaultCellStyle.Font = FontBold;
            dgv.ColumnHeadersDefaultCellStyle.Padding = new Padding(10, 0, 10, 0);
            dgv.ColumnHeadersDefaultCellStyle.WrapMode = DataGridViewTriState.False;
            dgv.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            dgv.ColumnHeadersHeight = 44;

            // Row Style
            dgv.DefaultCellStyle.BackColor = Color.White;
            dgv.DefaultCellStyle.ForeColor = TextRegular;
            dgv.DefaultCellStyle.SelectionBackColor = Color.FromArgb(238, 242, 255);
            dgv.DefaultCellStyle.SelectionForeColor = Primary;
            dgv.DefaultCellStyle.Padding = new Padding(12, 0, 12, 0);
            dgv.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(252, 253, 255);
        }

        // Style standard button as modern flat button
        public static void StylePrimaryButton(Button btn)
        {
            btn.FlatStyle = FlatStyle.Flat;
            btn.FlatAppearance.BorderSize = 0;
            btn.BackColor = Primary;
            btn.ForeColor = Color.White;
            btn.Font = FontBold;
            btn.Cursor = Cursors.Hand;
        }

        public static void StyleSecondaryButton(Button btn)
        {
            btn.FlatStyle = FlatStyle.Flat;
            btn.FlatAppearance.BorderColor = Border;
            btn.FlatAppearance.BorderSize = 1;
            btn.BackColor = Color.White;
            btn.ForeColor = TextRegular;
            btn.Font = FontBold;
            btn.Cursor = Cursors.Hand;
        }
    }
}
