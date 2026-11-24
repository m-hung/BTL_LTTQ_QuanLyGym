using System.Net;
using System.Net.Mail;
using System.Text;

namespace QuanLyGym.BLL.Services
{
    public class MailConfigException : Exception
    {
        public MailConfigException(string message) : base(message) { }
    }

    /// Gửi email qua SMTP. Thông tin tài khoản gửi mail đọc từ file "mail.config"
    /// đặt cạnh file chạy của ứng dụng (xem mail.config.example).
    public class EmailService
    {
        private sealed class MailSettings
        {
            public string Host { get; set; } = "smtp.gmail.com";
            public int Port { get; set; } = 587;
            public bool EnableSsl { get; set; } = true;
            public string User { get; set; } = "";
            public string Password { get; set; } = "";
            public string FromName { get; set; } = "Gym Manager";
        }

        public static string ConfigPath => Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "mail.config");
        public void EnsureConfigured() => LoadSettings();

        private static MailSettings LoadSettings()
        {
            if (!File.Exists(ConfigPath))
            {
                throw new MailConfigException(
                    "Chưa cấu hình gửi email.\n" +
                    "Hãy tạo file mail.config (sao chép từ mail.config.example) trong thư mục chạy của ứng dụng:\n" +
                    ConfigPath);
            }

            var settings = new MailSettings();
            foreach (var rawLine in File.ReadAllLines(ConfigPath))
            {
                string line = rawLine.Trim();
                if (line.Length == 0 || line.StartsWith('#')) continue;

                int eq = line.IndexOf('=');
                if (eq <= 0) continue;

                string key = line.Substring(0, eq).Trim().ToLowerInvariant();
                string value = line.Substring(eq + 1).Trim();

                switch (key)
                {
                    case "host": settings.Host = value; break;
                    case "port":
                        if (int.TryParse(value, out int port)) settings.Port = port;
                        break;
                    case "enablessl":
                        settings.EnableSsl = !value.Equals("false", StringComparison.OrdinalIgnoreCase);
                        break;
                    case "user": settings.User = value; break;
                    case "password": settings.Password = value; break;
                    case "fromname": settings.FromName = value; break;
                }
            }

            if (string.IsNullOrWhiteSpace(settings.User) || string.IsNullOrWhiteSpace(settings.Password))
                throw new MailConfigException("File mail.config thiếu User hoặc Password.\n" + ConfigPath);

            return settings;
        }

        public Task SendResetCodeAsync(string toEmail, string code, int lifetimeMinutes)
        {
            string html = BuildHtml(
                "Đặt lại mật khẩu",
                "Bạn vừa yêu cầu đặt lại mật khẩu cho tài khoản Gym Manager. Mã xác nhận của bạn là:",
                code,
                $"Mã có hiệu lực trong {lifetimeMinutes} phút. Nếu bạn không yêu cầu, hãy bỏ qua email này — mật khẩu của bạn vẫn an toàn.");

            return SendAsync(toEmail, "Mã xác nhận đặt lại mật khẩu - Gym Manager", html);
        }

        public Task SendUsernameAsync(string toEmail, string username)
        {
            string html = BuildHtml(
                "Tên đăng nhập của bạn",
                "Bạn vừa yêu cầu nhắc lại tên đăng nhập Gym Manager. Tên đăng nhập gắn với email này là:",
                WebUtility.HtmlEncode(username),
                "Nếu bạn không yêu cầu, hãy bỏ qua email này.");

            return SendAsync(toEmail, "Tên đăng nhập của bạn - Gym Manager", html);
        }

        private static string BuildHtml(string title, string intro, string highlight, string footer)
        {
            return
                "<div style=\"font-family:Segoe UI,Arial,sans-serif;max-width:460px;margin:0 auto;padding:24px;" +
                "border:1px solid #e2e8f0;border-radius:12px\">" +
                "<div style=\"height:6px;background:#f97316;border-radius:6px;margin-bottom:20px\"></div>" +
                $"<h2 style=\"margin:0 0 12px;color:#0f172a\">{title}</h2>" +
                $"<p style=\"color:#334155;line-height:1.5\">{intro}</p>" +
                "<p style=\"text-align:center;margin:24px 0\">" +
                $"<span style=\"display:inline-block;padding:12px 24px;background:#fff7ed;color:#ea580c;" +
                $"font-size:30px;font-weight:bold;letter-spacing:6px;border-radius:8px\">{highlight}</span></p>" +
                $"<p style=\"color:#64748b;font-size:13px;line-height:1.5\">{footer}</p>" +
                "<p style=\"color:#94a3b8;font-size:12px;margin-top:20px\">Gym Manager</p>" +
                "</div>";
        }

        private static async Task SendAsync(string toEmail, string subject, string htmlBody)
        {
            var settings = LoadSettings();

            try
            {
                using var client = new SmtpClient(settings.Host, settings.Port)
                {
                    EnableSsl = settings.EnableSsl,
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    UseDefaultCredentials = false,
                    Credentials = new NetworkCredential(settings.User, settings.Password),
                    Timeout = 20000
                };

                using var message = new MailMessage
                {
                    From = new MailAddress(settings.User, settings.FromName, Encoding.UTF8),
                    Subject = subject,
                    SubjectEncoding = Encoding.UTF8,
                    Body = htmlBody,
                    BodyEncoding = Encoding.UTF8,
                    IsBodyHtml = true
                };
                message.To.Add(new MailAddress(toEmail));

                await client.SendMailAsync(message);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException(
                    "Không gửi được email. Hãy kiểm tra lại file mail.config (tài khoản, App Password) và kết nối mạng.\n" +
                    "Chi tiết: " + (ex.InnerException?.Message ?? ex.Message), ex);
            }
        }
    }
}
