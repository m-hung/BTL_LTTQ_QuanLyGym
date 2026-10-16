using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Windows.Forms;

namespace QuanLyGym.UI.Forms.Admin.Controls
{
    public class AdminSearchBoxControl : Control
    {
        private readonly TextBox _textBox;
        private string _placeholder = "Tìm kiếm...";
        private string _counterText = "";
        private bool _isFocused = false;

        public event EventHandler? SearchTextChanged;

        [Category("Appearance")]
        public string Placeholder
        {
            get => _placeholder;
            set { _placeholder = value; Invalidate(); }
        }

        [Category("Appearance")]
        public string CounterText
        {
            get => _counterText;
            set { _counterText = value; Invalidate(); }
        }

        public string SearchText
        {
            get => _textBox.Text;
            set => _textBox.Text = value;
        }

        public AdminSearchBoxControl()
        {
            SetStyle(ControlStyles.AllPaintingInWmPaint |
                     ControlStyles.UserPaint |
                     ControlStyles.OptimizedDoubleBuffer |
                     ControlStyles.ResizeRedraw |
                     ControlStyles.SupportsTransparentBackColor, true);
            DoubleBuffered = true;
            BackColor = Color.Transparent;
            Height = 36;
            Size = new Size(400, 36);

            _textBox = new TextBox
            {
                BorderStyle = BorderStyle.None,
                BackColor = Color.FromArgb(20, 22, 28),
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 9F, FontStyle.Regular),
                Location = new Point(34, 9),
                Width = 180
            };

            _textBox.Enter += (s, e) => { _isFocused = true; Invalidate(); };
            _textBox.Leave += (s, e) => { _isFocused = false; Invalidate(); };
            _textBox.TextChanged += (s, e) =>
            {
                Invalidate();
                SearchTextChanged?.Invoke(this, EventArgs.Empty);
            };

            Controls.Add(_textBox);
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            if (_textBox == null) return;
            _textBox.Location = new Point(34, (Height - _textBox.Height) / 2);
            _textBox.Width = Math.Max(120, Width - 200);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            var g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;

            // Search Box Input Rect
            int boxW = Math.Max(160, Width - (string.IsNullOrEmpty(_counterText) ? 10 : 160));
            var boxRect = new Rectangle(0, 0, boxW, Height - 1);

            Color border = _isFocused ? AdminTheme.PrimaryOrange : Color.FromArgb(44, 46, 58);
            using (var path = AdminTheme.GetRoundedRectanglePath(boxRect, 5))
            {
                using (var brush = new SolidBrush(Color.FromArgb(20, 22, 28)))
                {
                    g.FillPath(brush, path);
                }
                using (var pen = new Pen(border, 1f))
                {
                    g.DrawPath(pen, path);
                }
            }

            // Magnifying glass icon
            using (var penIcon = new Pen(_isFocused ? AdminTheme.PrimaryOrange : AdminTheme.TextDim, 1.4f))
            {
                int iconX = 14;
                int iconY = (Height - 14) / 2;
                g.DrawEllipse(penIcon, iconX, iconY, 8, 8);
                g.DrawLine(penIcon, iconX + 6, iconY + 6, iconX + 11, iconY + 11);
            }

            // Placeholder text if empty
            if (string.IsNullOrEmpty(_textBox.Text))
            {
                using (var brushPh = new SolidBrush(AdminTheme.TextDim))
                {
                    g.DrawString(_placeholder, _textBox.Font, brushPh, _textBox.Left + 2, _textBox.Top);
                }
            }

            // Counter label on right
            if (!string.IsNullOrEmpty(_counterText))
            {
                using (var brushCounter = new SolidBrush(AdminTheme.TextDim))
                {
                    var sf = new StringFormat
                    {
                        Alignment = StringAlignment.Far,
                        LineAlignment = StringAlignment.Center
                    };
                    var counterRect = new RectangleF(Width - 150, 0, 140, Height);
                    g.DrawString(_counterText, AdminTheme.FontSmall, brushCounter, counterRect, sf);
                }
            }
        }
    }
}
