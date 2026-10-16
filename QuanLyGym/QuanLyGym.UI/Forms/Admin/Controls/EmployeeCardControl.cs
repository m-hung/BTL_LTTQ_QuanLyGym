using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Windows.Forms;

namespace QuanLyGym.UI.Forms.Admin.Controls
{
    public class EmployeeCardControl : Control
    {
        private string _empName = "Nguyễn Văn A";
        private string _role = "PT";
        private string _phone = "0900.000.000";
        private string _branch = "CN Hoàn Kiếm";
        private string _certifications = "ACE CPT, NASM";
        private float _rating = 4.9f;
        private int _studentsCount = 20;
        private string _salaryText = "15.000.000 đ";
        private bool _isHovered = false;

        public event EventHandler? CardClicked;

        [Category("Appearance")]
        public string EmployeeName
        {
            get => _empName;
            set { _empName = value; Invalidate(); }
        }

        [Category("Appearance")]
        public string Role
        {
            get => _role;
            set { _role = value; Invalidate(); }
        }

        [Category("Appearance")]
        public string Phone
        {
            get => _phone;
            set { _phone = value; Invalidate(); }
        }

        [Category("Appearance")]
        public string Branch
        {
            get => _branch;
            set { _branch = value; Invalidate(); }
        }

        [Category("Appearance")]
        public string Certifications
        {
            get => _certifications;
            set { _certifications = value; Invalidate(); }
        }

        [Category("Appearance")]
        public float Rating
        {
            get => _rating;
            set { _rating = value; Invalidate(); }
        }

        [Category("Appearance")]
        public int StudentsCount
        {
            get => _studentsCount;
            set { _studentsCount = value; Invalidate(); }
        }

        [Category("Appearance")]
        public string SalaryText
        {
            get => _salaryText;
            set { _salaryText = value; Invalidate(); }
        }

        public EmployeeCardControl()
        {
            SetStyle(ControlStyles.AllPaintingInWmPaint |
                     ControlStyles.UserPaint |
                     ControlStyles.OptimizedDoubleBuffer |
                     ControlStyles.ResizeRedraw |
                     ControlStyles.SupportsTransparentBackColor, true);
            DoubleBuffered = true;
            BackColor = Color.Transparent;
            Size = new Size(350, 140);
            Cursor = Cursors.Hand;
        }

        protected override void OnMouseEnter(EventArgs e)
        {
            base.OnMouseEnter(e);
            _isHovered = true;
            Invalidate();
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            base.OnMouseLeave(e);
            _isHovered = false;
            Invalidate();
        }

        protected override void OnMouseClick(MouseEventArgs e)
        {
            base.OnMouseClick(e);
            CardClicked?.Invoke(this, EventArgs.Empty);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            var g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;

            // Card background & border
            Color bg = _isHovered ? Color.FromArgb(28, 30, 40) : AdminTheme.CardBg;
            Color border = _isHovered ? AdminTheme.PrimaryOrange : AdminTheme.CardBorder;
            AdminTheme.DrawCard(g, ClientRectangle, radius: 8, customBg: bg, customBorder: border);

            int pad = 14;

            // 1. Avatar Placeholder Box
            var avatarRect = new Rectangle(pad, pad, 54, 54);
            using (var path = AdminTheme.GetRoundedRectanglePath(avatarRect, 6))
            {
                using (var brushAv = new SolidBrush(Color.FromArgb(42, 45, 58)))
                {
                    g.FillPath(brushAv, path);
                }
                using (var penAv = new Pen(Color.FromArgb(60, 64, 80), 1f))
                {
                    g.DrawPath(penAv, path);
                }

                // Silhouette icon
                using (var brushSil = new SolidBrush(Color.FromArgb(120, 125, 145)))
                {
                    g.FillEllipse(brushSil, avatarRect.X + 18, avatarRect.Y + 10, 18, 18);
                    g.FillPie(brushSil, avatarRect.X + 9, avatarRect.Y + 28, 36, 32, 180, 180);
                }
            }

            int textX = pad + 54 + 14;
            int textW = Width - textX - pad;

            // 2. Name & Role
            using (var brushName = new SolidBrush(AdminTheme.TextWhite))
            {
                g.DrawString(_empName, AdminTheme.FontBodyBold, brushName, textX, pad - 2);
            }

            using (var brushRole = new SolidBrush(AdminTheme.PrimaryOrange))
            using (var fontRole = new Font("Segoe UI", 7.5F, FontStyle.Bold))
            {
                g.DrawString(_role, fontRole, brushRole, textX, pad + 16);
            }

            // 3. Phone & Branch
            using (var brushSub = new SolidBrush(AdminTheme.TextDim))
            {
                g.DrawString($"{_phone}", AdminTheme.FontSmall, brushSub, textX, pad + 30);
                g.DrawString($"{_branch}", AdminTheme.FontSmall, brushSub, textX, pad + 44);
            }

            // 4. Tags
            int tagY = pad + 60;
            // Orange [PT] tag
            var ptTagRect = new Rectangle(textX, tagY, 26, 16);
            using (var path = AdminTheme.GetRoundedRectanglePath(ptTagRect, 3))
            using (var brushTag = new SolidBrush(AdminTheme.PrimaryOrange))
            {
                g.FillPath(brushTag, path);
                using (var brushTagText = new SolidBrush(Color.White))
                using (var fontTag = new Font("Segoe UI", 7F, FontStyle.Bold))
                {
                    var sf = new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center };
                    g.DrawString("PT", fontTag, brushTagText, ptTagRect, sf);
                }
            }

            // Certifications text
            using (var brushCert = new SolidBrush(AdminTheme.TextDim))
            {
                g.DrawString(_certifications, AdminTheme.FontSmall, brushCert, textX + 32, tagY);
            }

            // 5. Divider Line
            int divY = Height - 36;
            using (var penDiv = new Pen(Color.FromArgb(30, 32, 42), 1f))
            {
                g.DrawLine(penDiv, pad, divY, Width - pad, divY);
            }

            // 6. Bottom Stats
            int statY = divY + 8;

            // Star Rating (Yellow)
            using (var brushStar = new SolidBrush(Color.FromArgb(245, 158, 11)))
            using (var fontStar = new Font("Segoe UI", 8.5F, FontStyle.Bold))
            {
                g.DrawString($"★ {_rating:0.0}", fontStar, brushStar, pad + 4, statY);
            }

            // Student Count (Muted)
            using (var brushMem = new SolidBrush(AdminTheme.TextDim))
            {
                g.DrawString($"{_studentsCount} học viên", AdminTheme.FontSmall, brushMem, Width / 2f - 40, statY);
            }

            // Salary (Green)
            using (var brushSal = new SolidBrush(AdminTheme.Green))
            using (var fontSal = new Font("Segoe UI", 8.5F, FontStyle.Bold))
            {
                var sfRight = new StringFormat { Alignment = StringAlignment.Far, LineAlignment = StringAlignment.Center };
                var salRect = new RectangleF(Width - pad - 120, statY - 1, 120, 20);
                g.DrawString(_salaryText, fontSal, brushSal, salRect, sfRight);
            }
        }
    }
}
