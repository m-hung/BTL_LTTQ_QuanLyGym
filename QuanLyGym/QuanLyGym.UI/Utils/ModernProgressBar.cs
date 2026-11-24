using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace QuanLyGym.UI.Utils
{
    public class ModernProgressBar : Control
    {
        private int _value = 0;
        private int _maximum = 100;
        private Color _progressColor = UITheme.Primary;
        private Color _progressColor2 = UITheme.Secondary;
        private Color _trackColor = Color.FromArgb(241, 245, 249);
        private int _borderRadius = 6;
        private bool _showPercentage = false;

        [System.ComponentModel.DesignerSerializationVisibility(System.ComponentModel.DesignerSerializationVisibility.Hidden)]
        public int Value
        {
            get => _value;
            set
            {
                _value = Math.Max(0, Math.Min(value, _maximum));
                Invalidate();
            }
        }

        [System.ComponentModel.DesignerSerializationVisibility(System.ComponentModel.DesignerSerializationVisibility.Hidden)]
        public int Maximum
        {
            get => _maximum;
            set
            {
                _maximum = Math.Max(1, value);
                Invalidate();
            }
        }

        [System.ComponentModel.DesignerSerializationVisibility(System.ComponentModel.DesignerSerializationVisibility.Hidden)]
        public Color ProgressColor
        {
            get => _progressColor;
            set { _progressColor = value; Invalidate(); }
        }

        [System.ComponentModel.DesignerSerializationVisibility(System.ComponentModel.DesignerSerializationVisibility.Hidden)]
        public Color ProgressColor2
        {
            get => _progressColor2;
            set { _progressColor2 = value; Invalidate(); }
        }

        [System.ComponentModel.DesignerSerializationVisibility(System.ComponentModel.DesignerSerializationVisibility.Hidden)]
        public Color TrackColor
        {
            get => _trackColor;
            set { _trackColor = value; Invalidate(); }
        }

        [System.ComponentModel.DesignerSerializationVisibility(System.ComponentModel.DesignerSerializationVisibility.Hidden)]
        public int BorderRadius
        {
            get => _borderRadius;
            set { _borderRadius = value; Invalidate(); }
        }

        [System.ComponentModel.DesignerSerializationVisibility(System.ComponentModel.DesignerSerializationVisibility.Hidden)]
        public bool ShowPercentage
        {
            get => _showPercentage;
            set { _showPercentage = value; Invalidate(); }
        }

        public ModernProgressBar()
        {
            SetStyle(ControlStyles.AllPaintingInWmPaint |
                     ControlStyles.UserPaint |
                     ControlStyles.OptimizedDoubleBuffer |
                     ControlStyles.ResizeRedraw, true);
            Size = new Size(200, 12);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            Graphics g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;

            Rectangle rect = new Rectangle(0, 0, Width - 1, Height - 1);
            if (rect.Width <= 0 || rect.Height <= 0) return;

            // Draw track
            using (GraphicsPath trackPath = UITheme.GetRoundedPath(rect, _borderRadius))
            using (SolidBrush trackBrush = new SolidBrush(_trackColor))
            {
                g.FillPath(trackBrush, trackPath);
            }

            // Draw progress
            float ratio = (float)_value / _maximum;
            int progressWidth = (int)(rect.Width * ratio);
            if (progressWidth > 4)
            {
                Rectangle progRect = new Rectangle(rect.X, rect.Y, progressWidth, rect.Height);
                using (GraphicsPath progPath = UITheme.GetRoundedPath(progRect, _borderRadius))
                {
                    if (_progressColor != _progressColor2)
                    {
                        using (LinearGradientBrush progBrush = new LinearGradientBrush(progRect, _progressColor, _progressColor2, 0f))
                        {
                            g.FillPath(progBrush, progPath);
                        }
                    }
                    else
                    {
                        using (SolidBrush progBrush = new SolidBrush(_progressColor))
                        {
                            g.FillPath(progBrush, progPath);
                        }
                    }
                }
            }

            if (_showPercentage)
            {
                string text = $"{(int)(ratio * 100)}%";
                using (Font font = new Font("Segoe UI", 7.5f, FontStyle.Bold))
                using (SolidBrush textBrush = new SolidBrush(UITheme.TextDark))
                {
                    SizeF textSize = g.MeasureString(text, font);
                    g.DrawString(text, font, textBrush, (Width - textSize.Width) / 2, (Height - textSize.Height) / 2);
                }
            }
        }
    }
}
