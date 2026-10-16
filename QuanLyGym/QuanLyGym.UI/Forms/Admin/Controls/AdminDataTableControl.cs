using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Windows.Forms;

namespace QuanLyGym.UI.Forms.Admin.Controls
{
    public class TableColumnDef
    {
        public string Title { get; set; } = "";
        public float WidthWeight { get; set; } = 1f;
        public StringAlignment Alignment { get; set; } = StringAlignment.Near;
        public Color TextColor { get; set; } = Color.White;
        public bool IsBold { get; set; } = false;
        public bool IsStatusBadge { get; set; } = false;
        public bool IsPackageBadge { get; set; } = false;
        public bool IsActionColumn { get; set; } = false;

        public TableColumnDef(string title, float weight = 1f, StringAlignment align = StringAlignment.Near)
        {
            Title = title;
            WidthWeight = weight;
            Alignment = align;
        }
    }

    public class TableRowData
    {
        public string[] Cells { get; set; } = Array.Empty<string>();
        public object? Tag { get; set; }

        public TableRowData(params string[] cells)
        {
            Cells = cells;
        }
    }

    public class AdminDataTableControl : Control
    {
        private readonly List<TableColumnDef> _columns = new List<TableColumnDef>();
        private readonly List<TableRowData> _rows = new List<TableRowData>();

        private int _hoveredRow = -1;
        private int _hoveredActionCol = -1; // 0 = edit, 1 = delete

        public event EventHandler<TableRowData>? EditActionClicked;
        public event EventHandler<TableRowData>? DeleteActionClicked;

        public List<TableColumnDef> Columns => _columns;
        public List<TableRowData> Rows => _rows;

        public AdminDataTableControl()
        {
            SetStyle(ControlStyles.AllPaintingInWmPaint |
                     ControlStyles.UserPaint |
                     ControlStyles.OptimizedDoubleBuffer |
                     ControlStyles.ResizeRedraw |
                     ControlStyles.SupportsTransparentBackColor, true);
            DoubleBuffered = true;
            BackColor = Color.Transparent;
            Font = new Font("Segoe UI", 8.5F, FontStyle.Regular);
            Size = new Size(1150, 400);
        }

        public void SetData(IEnumerable<TableColumnDef> columns, IEnumerable<TableRowData> rows)
        {
            _columns.Clear();
            _columns.AddRange(columns);
            _rows.Clear();
            _rows.AddRange(rows);
            Invalidate();
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
            int headerH = 34;
            int rowH = 36;

            int newHoverRow = -1;
            int newHoverAction = -1;

            if (e.Y >= headerH && _rows.Count > 0)
            {
                int r = (e.Y - headerH) / rowH;
                if (r >= 0 && r < _rows.Count)
                {
                    newHoverRow = r;

                    // Check if hovering over action icons on the right
                    if (e.X >= Width - 60 && e.X <= Width - 36)
                        newHoverAction = 0; // edit
                    else if (e.X > Width - 36 && e.X <= Width - 10)
                        newHoverAction = 1; // delete
                }
            }

            if (newHoverRow != _hoveredRow || newHoverAction != _hoveredActionCol)
            {
                _hoveredRow = newHoverRow;
                _hoveredActionCol = newHoverAction;
                Cursor = (newHoverAction != -1) ? Cursors.Hand : (_hoveredRow != -1 ? Cursors.Default : Cursors.Default);
                Invalidate();
            }
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            base.OnMouseLeave(e);
            if (_hoveredRow != -1 || _hoveredActionCol != -1)
            {
                _hoveredRow = -1;
                _hoveredActionCol = -1;
                Cursor = Cursors.Default;
                Invalidate();
            }
        }

        protected override void OnMouseClick(MouseEventArgs e)
        {
            base.OnMouseClick(e);
            if (_hoveredRow >= 0 && _hoveredRow < _rows.Count)
            {
                var rowData = _rows[_hoveredRow];
                if (_hoveredActionCol == 0)
                {
                    EditActionClicked?.Invoke(this, rowData);
                }
                else if (_hoveredActionCol == 1)
                {
                    DeleteActionClicked?.Invoke(this, rowData);
                }
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            var g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;

            // Container card background
            AdminTheme.DrawCard(g, ClientRectangle, radius: 8);

            if (_columns.Count == 0) return;

            int padX = 16;
            int availW = Width - padX * 2;
            int headerH = 34;
            int rowH = 36;

            // Calculate column widths
            float totalWeight = 0;
            foreach (var col in _columns) totalWeight += col.WidthWeight;
            if (totalWeight <= 0) totalWeight = 1;

            var colWidths = new float[_columns.Count];
            for (int i = 0; i < _columns.Count; i++)
            {
                colWidths[i] = (availW * _columns[i].WidthWeight) / totalWeight;
            }

            // 1. Draw Header
            float curX = padX;
            using (var brushHeader = new SolidBrush(AdminTheme.TextDim))
            using (var fontHeader = new Font("Segoe UI", 7.5F, FontStyle.Bold))
            {
                for (int i = 0; i < _columns.Count; i++)
                {
                    var rect = new RectangleF(curX, 4, colWidths[i], headerH - 8);
                    var sf = new StringFormat
                    {
                        Alignment = _columns[i].Alignment,
                        LineAlignment = StringAlignment.Center
                    };
                    g.DrawString(_columns[i].Title.ToUpperInvariant(), fontHeader, brushHeader, rect, sf);
                    curX += colWidths[i];
                }
            }

            // Header bottom border
            using (var penDiv = new Pen(Color.FromArgb(28, 30, 40), 1f))
            {
                g.DrawLine(penDiv, padX, headerH, Width - padX, headerH);
            }

            // 2. Draw Rows
            int maxRowsVisible = (Height - headerH) / rowH;
            int rowsToDraw = Math.Min(_rows.Count, maxRowsVisible);

            for (int r = 0; r < rowsToDraw; r++)
            {
                int rowY = headerH + r * rowH;
                var rowRect = new Rectangle(padX - 4, rowY, availW + 8, rowH);

                bool isHovered = (r == _hoveredRow);

                // Row hover background
                if (isHovered)
                {
                    using (var path = AdminTheme.GetRoundedRectanglePath(rowRect, 4))
                    using (var brushHover = new SolidBrush(Color.FromArgb(26, 28, 38)))
                    {
                        g.FillPath(brushHover, path);
                    }
                }

                // Row divider
                using (var penRowDiv = new Pen(Color.FromArgb(22, 24, 32), 1f))
                {
                    g.DrawLine(penRowDiv, padX, rowY + rowH - 1, Width - padX, rowY + rowH - 1);
                }

                // Draw cells
                curX = padX;
                var rowData = _rows[r];

                for (int c = 0; c < _columns.Count; c++)
                {
                    var col = _columns[c];
                    var cellRect = new RectangleF(curX, rowY, colWidths[c], rowH);
                    string cellVal = (c < rowData.Cells.Length) ? rowData.Cells[c] : "";

                    if (col.IsActionColumn)
                    {
                        // Draw Action icons: Edit and Delete
                        DrawActionIcons(g, cellRect, r == _hoveredRow ? _hoveredActionCol : -1);
                    }
                    else if (col.IsStatusBadge)
                    {
                        // Draw Status pill: • ACTIVE or • INACTIVE
                        DrawStatusBadge(g, cellRect, cellVal);
                    }
                    else if (col.IsPackageBadge)
                    {
                        // Draw Package pill: VIP 12T or Standard 6T
                        DrawPackageBadge(g, cellRect, cellVal);
                    }
                    else
                    {
                        // Text Cell
                        Color textColor = col.TextColor;
                        if (textColor == Color.White && !col.IsBold) textColor = AdminTheme.TextWhite;

                        using (var brushText = new SolidBrush(textColor))
                        {
                            var font = col.IsBold ? AdminTheme.FontBodyBold : AdminTheme.FontBody;
                            var sf = new StringFormat
                            {
                                Alignment = col.Alignment,
                                LineAlignment = StringAlignment.Center,
                                Trimming = StringTrimming.EllipsisCharacter
                            };
                            g.DrawString(cellVal, font, brushText, cellRect, sf);
                        }
                    }

                    curX += colWidths[c];
                }
            }
        }

        private void DrawStatusBadge(Graphics g, RectangleF cellRect, string statusText)
        {
            bool isActive = statusText.IndexOf("ACTIVE", StringComparison.OrdinalIgnoreCase) >= 0 &&
                           statusText.IndexOf("INACTIVE", StringComparison.OrdinalIgnoreCase) < 0;

            Color bgColor = isActive ? Color.FromArgb(40, 16, 185, 129) : Color.FromArgb(40, 239, 68, 68);
            Color dotColor = isActive ? AdminTheme.Green : Color.FromArgb(239, 68, 68);
            Color textColor = isActive ? AdminTheme.Green : Color.FromArgb(239, 68, 68);

            float badgeW = 74f;
            float badgeH = 20f;
            float badgeX = cellRect.X;
            float badgeY = cellRect.Y + (cellRect.Height - badgeH) / 2f;
            var badgeRect = new RectangleF(badgeX, badgeY, badgeW, badgeH);

            using (var path = AdminTheme.GetRoundedRectanglePath(Rectangle.Round(badgeRect), 4))
            using (var brushBg = new SolidBrush(bgColor))
            {
                g.FillPath(brushBg, path);
            }

            // Dot
            using (var brushDot = new SolidBrush(dotColor))
            {
                g.FillEllipse(brushDot, badgeX + 8, badgeY + 7, 6, 6);
            }

            // Text
            using (var brushText = new SolidBrush(textColor))
            using (var font = new Font("Segoe UI", 7.5F, FontStyle.Bold))
            {
                var sf = new StringFormat
                {
                    Alignment = StringAlignment.Center,
                    LineAlignment = StringAlignment.Center
                };
                var textRect = new RectangleF(badgeX + 14, badgeY, badgeW - 14, badgeH);
                g.DrawString(statusText.Replace("•", "").Trim(), font, brushText, textRect, sf);
            }
        }

        private void DrawPackageBadge(Graphics g, RectangleF cellRect, string pkgText)
        {
            bool isVip = pkgText.IndexOf("VIP", StringComparison.OrdinalIgnoreCase) >= 0;

            Color bgColor = isVip ? Color.FromArgb(50, 255, 107, 0) : Color.FromArgb(40, 180, 83, 9);
            Color textColor = isVip ? AdminTheme.PrimaryOrange : Color.FromArgb(245, 158, 11);
            Color borderColor = isVip ? Color.FromArgb(120, 255, 107, 0) : Color.FromArgb(100, 180, 83, 9);

            float badgeW = 82f;
            float badgeH = 20f;
            float badgeX = cellRect.X;
            float badgeY = cellRect.Y + (cellRect.Height - badgeH) / 2f;
            var badgeRect = new RectangleF(badgeX, badgeY, badgeW, badgeH);

            using (var path = AdminTheme.GetRoundedRectanglePath(Rectangle.Round(badgeRect), 4))
            {
                using (var brushBg = new SolidBrush(bgColor))
                {
                    g.FillPath(brushBg, path);
                }
                using (var pen = new Pen(borderColor, 1f))
                {
                    g.DrawPath(pen, path);
                }
            }

            using (var brushText = new SolidBrush(textColor))
            using (var font = new Font("Segoe UI", 7.5F, FontStyle.Bold))
            {
                var sf = new StringFormat
                {
                    Alignment = StringAlignment.Center,
                    LineAlignment = StringAlignment.Center
                };
                g.DrawString(pkgText, font, brushText, badgeRect, sf);
            }
        }

        private void DrawActionIcons(Graphics g, RectangleF cellRect, int hoveredAction)
        {
            float iconSize = 18f;
            float y = cellRect.Y + (cellRect.Height - iconSize) / 2f;

            // 1. Edit Icon (Pencil)
            float x1 = cellRect.X + 4;
            Color editCol = (hoveredAction == 0) ? AdminTheme.PrimaryOrange : AdminTheme.TextDim;
            using (var pen = new Pen(editCol, 1.4f))
            {
                // Box with pencil
                g.DrawRectangle(pen, x1 + 1, y + 1, 12, 12);
                g.DrawLine(pen, x1 + 4, y + 9, x1 + 9, y + 4);
                g.DrawLine(pen, x1 + 9, y + 4, x1 + 11, y + 6);
                g.DrawLine(pen, x1 + 11, y + 6, x1 + 6, y + 11);
            }

            // 2. Delete Icon (Trash)
            float x2 = x1 + 22;
            Color delCol = (hoveredAction == 1) ? Color.FromArgb(239, 68, 68) : AdminTheme.TextDim;
            using (var pen = new Pen(delCol, 1.4f))
            {
                // Trash lid
                g.DrawLine(pen, x2 + 2, y + 4, x2 + 13, y + 4);
                g.DrawLine(pen, x2 + 5, y + 2, x2 + 10, y + 2);
                // Trash can body
                g.DrawLine(pen, x2 + 3, y + 4, x2 + 4, y + 13);
                g.DrawLine(pen, x2 + 12, y + 4, x2 + 11, y + 13);
                g.DrawLine(pen, x2 + 4, y + 13, x2 + 11, y + 13);
                // Vertical slots
                g.DrawLine(pen, x2 + 6, y + 6, x2 + 6, y + 11);
                g.DrawLine(pen, x2 + 9, y + 6, x2 + 9, y + 11);
            }
        }
    }
}
