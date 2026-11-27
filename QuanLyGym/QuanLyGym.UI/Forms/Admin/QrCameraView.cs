using Microsoft.Web.WebView2.Core;
using Microsoft.Web.WebView2.WinForms;

namespace QuanLyGym.UI.Forms.Admin
{
    internal sealed class QrCameraView : UserControl
    {
        private const string ScannerUrl = "https://gym-qr.example/scanner.html";
        private static Task<CoreWebView2Environment>? _environment;
        private readonly WebView2 _camera = new() { Dock = DockStyle.Fill, DefaultBackgroundColor = Color.FromArgb(24, 24, 27), Visible = false };
        private readonly Label _status = new()
        {
            Dock = DockStyle.Fill, BackColor = Color.FromArgb(24, 24, 27), ForeColor = Color.FromArgb(161, 161, 170),
            TextAlign = ContentAlignment.MiddleCenter, Text = "Đang tải khung camera…"
        };

        public event Action<string>? CodeRead;

        public QrCameraView()
        {
            Controls.Add(_camera);
            Controls.Add(_status);
        }

        internal static async Task WarmUpAsync()
        {
            try
            {
                await GetEnvironmentAsync();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex);
            }
        }

        private static Task<CoreWebView2Environment> GetEnvironmentAsync()
        {
            if (_environment == null || _environment.IsFaulted || _environment.IsCanceled)
            {
                string profile = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "QuanLyGym", "QrCamera");
                _environment = CoreWebView2Environment.CreateAsync(userDataFolder: profile);
            }
            return _environment;
        }

        internal void ResetCamera()
        {
            // Reload releases the old page's media tracks while keeping the WebView instance.
            if (_camera.Visible || _camera.CoreWebView2?.Source == ScannerUrl)
                _camera.CoreWebView2?.Reload();
        }

        protected override async void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            if (DesignMode) return;
            try
            {
                var environment = await GetEnvironmentAsync();
                if (IsDisposed || Disposing) return;
                await _camera.EnsureCoreWebView2Async(environment);
                if (IsDisposed || Disposing) return;

                string scannerFolder = Path.Combine(AppContext.BaseDirectory, "QrCamera");
                if (new[] { "scanner.html", "scanner.js", "html5-qrcode.min.js" }
                    .Any(file => !File.Exists(Path.Combine(scannerFolder, file))))
                {
                    ShowCameraError("Thiếu tệp giao diện camera. Build lại project WinForms.");
                    return;
                }

                var browser = _camera.CoreWebView2;
                browser.SetVirtualHostNameToFolderMapping("gym-qr.example",
                    scannerFolder, CoreWebView2HostResourceAccessKind.Deny);
                browser.Settings.AreDevToolsEnabled = false;
                browser.Settings.AreDefaultContextMenusEnabled = false;
                browser.NavigationStarting += (_, args) => args.Cancel = args.Uri != ScannerUrl;
                browser.NavigationCompleted += (_, args) =>
                {
                    if (!args.IsSuccess)
                        ShowCameraError($"Không tải được khung camera ({args.WebErrorStatus}). Bạn vẫn có thể nhập mã thẻ.");
                };
                browser.ProcessFailed += (_, _) =>
                    ShowCameraError("Khung camera đã dừng. Chuyển sang trang khác và mở lại Check-in QR.");
                browser.NewWindowRequested += (_, args) => args.Handled = true;
                browser.PermissionRequested += (_, args) =>
                {
                    args.SavesInProfile = false;
                    args.State = args.PermissionKind == CoreWebView2PermissionKind.Camera &&
                        Uri.TryCreate(args.Uri, UriKind.Absolute, out var origin) &&
                        origin.Scheme == Uri.UriSchemeHttps && origin.Host == "gym-qr.example"
                        ? CoreWebView2PermissionState.Allow : CoreWebView2PermissionState.Deny;
                };
                browser.WebMessageReceived += (_, args) =>
                {
                    if (args.Source != ScannerUrl || IsDisposed || Disposing || !Visible) return;
                    string code = args.TryGetWebMessageAsString().Trim();
                    if (code.Length is > 0 and <= 80)
                        CodeRead?.Invoke(code);
                };
                _status.Visible = false;
                _camera.Visible = true;
                _camera.BringToFront();
                browser.Navigate(ScannerUrl);
            }
            catch (Exception) when (IsDisposed || Disposing)
            {
                // The operator switched pages while WebView2 was starting.
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex);
                ShowCameraError("Không mở được khung camera. Kiểm tra Microsoft Edge WebView2 Runtime.");
            }
        }
        private void ShowCameraError(string message)
        {
            if (IsDisposed || Disposing) return;
            _camera.Visible = false;
            _status.Text = message;
            _status.Visible = true;
            _status.BringToFront();
        }
    }
}
