using QuanLyGym.UI.Utils;

namespace QuanLyGym.UI.Forms.Member.Views
{
    internal sealed class frmMemberQrPreview : Form
    {
        internal static readonly Color Accent = Color.FromArgb(196, 248, 48);
        private static readonly Color CardColor = Color.FromArgb(19, 27, 23);
        private static readonly Color Muted = Color.FromArgb(177, 190, 180);

        public frmMemberQrPreview(string memberName, string packageName, string cardCode)
        {
            Text = "Mã điểm danh check-in";
            StartPosition = FormStartPosition.CenterParent;
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            ShowInTaskbar = false;
            AutoScaleMode = AutoScaleMode.Dpi;
            ClientSize = new Size(430, 648);
            Padding = new Padding(12);
            BackColor = Color.FromArgb(10, 16, 12);

            var card = new RoundedPanel
            {
                Dock = DockStyle.Fill, CornerRadius = 18, BorderColor = Accent,
                BackColor = CardColor, Padding = new Padding(22, 15, 22, 19)
            };
            Controls.Add(card);

            var content = new TableLayoutPanel { Dock = DockStyle.Fill, ColumnCount = 1, RowCount = 7 };
            content.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
            foreach (int height in new[] { 40, 310, 55, 31, 37, 61, 45 })
                content.RowStyles.Add(new RowStyle(SizeType.Absolute, height));
            card.Controls.Add(content);

            var heading = LabelFor("●  MÃ ĐIỂM DANH CHECK-IN", UITheme.FontSubtitle, Accent);
            heading.TextAlign = ContentAlignment.MiddleLeft;
            content.Controls.Add(heading, 0, 0);
            var preview = new QrPreviewControl(cardCode) { Size = new Size(294, 294), Anchor = AnchorStyles.None };
            content.Controls.Add(preview, 0, 1);
            content.Controls.Add(LabelFor(memberName, UITheme.FontTitle, Color.White), 0, 2);

            var badge = LabelFor(cardCode, UITheme.FontSmallBold, Accent);
            badge.BackColor = Color.FromArgb(45, 61, 24);
            badge.Dock = DockStyle.None;
            badge.Size = new Size(350, 26);
            badge.Anchor = AnchorStyles.None;
            content.Controls.Add(badge, 0, 3);
            content.Controls.Add(LabelFor($"Gói: {packageName}", UITheme.FontRegular, Muted), 0, 4);
            content.Controls.Add(LabelFor("Đưa mã này cho nhân viên tại quầy để quét.\r\nGiữ mã ở chế độ riêng tư.", UITheme.FontRegular, Muted), 0, 5);

            var close = new Button
            {
                Dock = DockStyle.Fill, Text = "ĐÓNG", DialogResult = DialogResult.OK, BackColor = Accent,
                ForeColor = CardColor, Font = UITheme.FontBold, FlatStyle = FlatStyle.Flat, Cursor = Cursors.Hand
            };
            close.FlatAppearance.BorderSize = 0;
            content.Controls.Add(close, 0, 6);
            AcceptButton = close;
            CancelButton = close;
        }

        private static Label LabelFor(string text, Font font, Color color) => new()
        {
            Text = text, Font = font, ForeColor = color, Dock = DockStyle.Fill, AutoEllipsis = true,
            TextAlign = ContentAlignment.MiddleCenter, UseMnemonic = false, Margin = Padding.Empty
        };
    }
}
