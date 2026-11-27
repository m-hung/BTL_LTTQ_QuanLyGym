using QRCoder;

namespace QuanLyGym.UI.Forms.Member.Views
{
    internal sealed class QrPreviewControl : Control
    {
        private readonly QRCodeData? _code;

        public QrPreviewControl(string? cardCode)
        {
            if (!string.IsNullOrEmpty(cardCode))
                _code = QRCodeGenerator.GenerateQrCode(cardCode, QRCodeGenerator.ECCLevel.M);
            BackColor = _code != null ? Color.White : Color.FromArgb(53, 68, 30);
            AccessibleName = "Mã QR check-in";
            DoubleBuffered = true;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing) _code?.Dispose();
            base.Dispose(disposing);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            if (_code == null) return;
            var modules = _code.ModuleMatrix;
            int cell = Math.Min(ClientSize.Width, ClientSize.Height) / modules.Count;
            if (cell == 0) return;
            int left = (ClientSize.Width - modules.Count * cell) / 2;
            int top = (ClientSize.Height - modules.Count * cell) / 2;
            for (int y = 0; y < modules.Count; y++)
            {
                for (int x = 0; x < modules.Count; x++)
                {
                    if (modules[y][x])
                        e.Graphics.FillRectangle(Brushes.Black, left + x * cell, top + y * cell, cell, cell);
                }
            }
        }
    }
}

