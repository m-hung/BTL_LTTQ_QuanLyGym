using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using QuanLyGym.BLL.DTOs.PT;
using QuanLyGym.BLL.Services.PT;

namespace QuanLyGym.UI.Forms.PT
{
    public partial class frmXacNhanLichModal : Form
    {
        // Win32 API hỗ trợ kéo di chuyển Form không viền mượt mà
        [DllImport("user32.dll")]
        public static extern bool ReleaseCapture();
        [DllImport("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);
        private const int WM_NCLBUTTONDOWN = 0xA1;
        private const int HT_CAPTION = 0x2;

        private readonly PTBookingApprovalService _bookingService = new PTBookingApprovalService();
        private readonly int _ptId;
        private List<BookingRequestDto> _requests = new List<BookingRequestDto>();
        private FlowLayoutPanel pnlList = null!;
        private Label lblHeaderSubtitle = null!;
        private Label lblFooterStatus = null!;
        private Button btnHeaderClose = null!;

        public event Action? OnBookingsChanged;

        public frmXacNhanLichModal() : this(1)
        {
        }

        public frmXacNhanLichModal(int ptId)
        {
            _ptId = ptId > 0 ? ptId : 1;
            InitializeComponentManual();
            ReloadRequests();
        }

        private void InitializeComponentManual()
        {
            this.Text = "Yêu Cầu Đặt Lịch Từ Hội Viên";
            this.Size = new Size(780, 620);
            this.StartPosition = FormStartPosition.CenterParent;
            this.FormBorderStyle = FormBorderStyle.None;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.BackColor = PTUITheme.BgDark;
            this.DoubleBuffered = true;
            this.KeyPreview = true;

            // Phím tắt ESC để đóng Form
            this.KeyDown += (s, e) =>
            {
                if (e.KeyCode == Keys.Escape)
                {
                    this.Close();
                }
            };

            // 1. Header Panel
            Panel pnlHeader = new Panel
            {
                Dock = DockStyle.Top,
                Height = 80,
                Width = this.ClientSize.Width,
                BackColor = Color.FromArgb(20, 24, 34),
                Padding = new Padding(24, 16, 24, 12)
            };
            pnlHeader.MouseDown += FormHeader_MouseDown;

            pnlHeader.Paint += (s, e) =>
            {
                e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                using (SolidBrush bLime = new SolidBrush(PTUITheme.PrimaryLime))
                {
                    e.Graphics.FillRectangle(bLime, 0, 0, 4, pnlHeader.Height);
                }
                using (Pen borderPen = new Pen(Color.FromArgb(36, 44, 62), 1))
                {
                    e.Graphics.DrawLine(borderPen, 0, pnlHeader.Height - 1, pnlHeader.Width, pnlHeader.Height - 1);
                }
            };

            Label lblTitle = new Label
            {
                Text = "YÊU CẦU ĐẶT LỊCH TỪ HỘI VIÊN",
                Font = new Font("Segoe UI", 12.5f, FontStyle.Bold),
                ForeColor = PTUITheme.PrimaryLime,
                Location = new Point(24, 18),
                AutoSize = true,
                Cursor = Cursors.Default
            };
            lblTitle.MouseDown += FormHeader_MouseDown;

            lblHeaderSubtitle = new Label
            {
                Text = "Hội viên đã đăng ký ca rảnh của bạn. Vui lòng xác nhận hoặc từ chối để hệ thống chốt lịch.",
                Font = PTUITheme.FontSmall,
                ForeColor = PTUITheme.TextMuted,
                Location = new Point(24, 46),
                AutoSize = true,
                Cursor = Cursors.Default
            };
            lblHeaderSubtitle.MouseDown += FormHeader_MouseDown;

            // Nút đóng (✕)
            btnHeaderClose = new Button
            {
                Text = "✕",
                Font = new Font("Segoe UI", 12f, FontStyle.Bold),
                ForeColor = Color.White,
                BackColor = Color.FromArgb(34, 40, 56),
                FlatStyle = FlatStyle.Flat,
                Size = new Size(38, 38),
                Cursor = Cursors.Hand,
                UseMnemonic = false
            };
            btnHeaderClose.FlatAppearance.BorderSize = 0;
            btnHeaderClose.MouseEnter += (s, e) => { btnHeaderClose.BackColor = Color.FromArgb(220, 38, 38); };
            btnHeaderClose.MouseLeave += (s, e) => { btnHeaderClose.BackColor = Color.FromArgb(34, 40, 56); };
            btnHeaderClose.Click += (s, e) => this.Close();

            btnHeaderClose.Location = new Point(pnlHeader.Width - 52, 20);
            pnlHeader.Resize += (s, e) =>
            {
                btnHeaderClose.Location = new Point(pnlHeader.ClientSize.Width - 52, 20);
            };

            pnlHeader.Controls.Add(btnHeaderClose);
            pnlHeader.Controls.Add(lblTitle);
            pnlHeader.Controls.Add(lblHeaderSubtitle);
            btnHeaderClose.BringToFront();

            // 2. Bottom Panel
            Panel pnlBottom = new Panel
            {
                Dock = DockStyle.Bottom,
                Height = 50,
                Width = this.ClientSize.Width,
                BackColor = Color.FromArgb(20, 24, 34)
            };
            pnlBottom.Paint += (s, e) =>
            {
                using (Pen borderPen = new Pen(Color.FromArgb(36, 44, 62), 1))
                {
                    e.Graphics.DrawLine(borderPen, 0, 0, pnlBottom.Width, 0);
                }
            };

            lblFooterStatus = new Label
            {
                Text = "Đang tải dữ liệu yêu cầu...",
                Font = PTUITheme.FontSmall,
                ForeColor = PTUITheme.TextMuted,
                Location = new Point(24, 16),
                AutoSize = true
            };

            pnlBottom.Controls.Add(lblFooterStatus);

            // 3. Container list
            pnlList = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                AutoScroll = true,
                Padding = new Padding(24, 16, 24, 16),
                FlowDirection = FlowDirection.TopDown,
                WrapContents = false,
                BackColor = PTUITheme.BgDark
            };

            this.Controls.Add(pnlList);
            this.Controls.Add(pnlBottom);
            this.Controls.Add(pnlHeader);

            this.Load += (s, e) =>
            {
                btnHeaderClose.Location = new Point(pnlHeader.ClientSize.Width - 52, 20);
            };

            // Viền ngoài Form
            this.Paint += (s, e) =>
            {
                using (Pen formBorder = new Pen(Color.FromArgb(48, 58, 80), 1))
                {
                    e.Graphics.DrawRectangle(formBorder, 0, 0, this.ClientSize.Width - 1, this.ClientSize.Height - 1);
                }
            };
        }

        private void FormHeader_MouseDown(object? sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ReleaseCapture();
                SendMessage(this.Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
            }
        }

        private void ReloadRequests()
        {
            _requests = _bookingService.GetPendingRequests(_ptId, "all");
            RenderRequests();
        }

        public int PendingCount => _bookingService.GetPendingCount(_ptId);

        private void RenderRequests()
        {
            pnlList.Controls.Clear();
            pnlList.SuspendLayout();

            if (lblFooterStatus != null)
            {
                int lateCount = _requests.FindAll(r => r.IsOverdue).Count;
                if (_requests.Count > 0)
                {
                    lblFooterStatus.Text = lateCount > 0
                        ? $"Hiện có {_requests.Count} yêu cầu đang chờ ({lateCount} yêu cầu đã quá giờ tập)"
                        : $"Hiện có {_requests.Count} yêu cầu đang chờ phản hồi từ bạn";
                }
                else
                {
                    lblFooterStatus.Text = "Không còn yêu cầu nào đang chờ xử lý";
                }
            }

            if (_requests.Count == 0)
            {
                Panel emptyPanel = new Panel
                {
                    Width = pnlList.ClientSize.Width - 48,
                    Height = 220,
                    BackColor = Color.Transparent
                };
                Label lblEmpty = new Label
                {
                    Text = "Không có yêu cầu đặt lịch nào đang chờ xác nhận!",
                    Font = PTUITheme.FontBold,
                    ForeColor = PTUITheme.TextMuted,
                    Dock = DockStyle.Fill,
                    TextAlign = ContentAlignment.MiddleCenter
                };
                emptyPanel.Controls.Add(lblEmpty);
                pnlList.Controls.Add(emptyPanel);
                pnlList.ResumeLayout(true);
                return;
            }

            int itemWidth = Math.Max(690, pnlList.ClientSize.Width - 52);

            foreach (var req in _requests)
            {
                Panel card = CreateRequestCard(req, itemWidth);
                pnlList.Controls.Add(card);
            }

            pnlList.ResumeLayout(true);
        }

        private Panel CreateRequestCard(BookingRequestDto req, int width)
        {
            Color[] avatarPalette =
            {
                PTUITheme.PrimaryLime,
                PTUITheme.Cyan,
                Color.FromArgb(251, 146, 60),
                Color.FromArgb(168, 85, 247),
                Color.FromArgb(56, 189, 248),
                Color.FromArgb(244, 114, 182)
            };
            Color avatarColor = avatarPalette[req.ColorIndex % avatarPalette.Length];

            Panel card = new PTUITheme.DoubleBufferedPanel
            {
                Width = width,
                Height = 146,
                Margin = new Padding(0, 0, 0, 14),
                BackColor = Color.FromArgb(22, 26, 36)
            };

            card.Paint += (s, e) =>
            {
                Graphics g = e.Graphics;
                g.SmoothingMode = SmoothingMode.AntiAlias;
                Rectangle r = card.ClientRectangle;

                Color borderColor = req.IsOverdue ? Color.FromArgb(120, 245, 158, 11) : Color.FromArgb(42, 50, 70);
                PTUITheme.DrawCard(g, new Rectangle(r.X, r.Y, r.Width - 1, r.Height - 1), radius: 10,
                    customBg: Color.FromArgb(22, 26, 36), customBorder: borderColor);

                // Avatar
                Rectangle avRect = new Rectangle(18, 16, 46, 46);
                using (SolidBrush bAv = new SolidBrush(Color.FromArgb(28, avatarColor.R, avatarColor.G, avatarColor.B)))
                {
                    g.FillEllipse(bAv, avRect);
                }
                using (Pen pAv = new Pen(Color.FromArgb(100, avatarColor.R, avatarColor.G, avatarColor.B), 1.5f))
                {
                    g.DrawEllipse(pAv, avRect);
                }

                TextRenderer.DrawText(g, req.MemberInitials, PTUITheme.FontBold, avRect, avatarColor,
                    TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter | TextFormatFlags.NoPrefix);

                // Họ tên và Mã Hội Viên
                int textStartX = 74;
                int infoWidth = card.Width - textStartX - 240;

                Rectangle nameRect = new Rectangle(textStartX, 16, infoWidth, 22);
                TextRenderer.DrawText(g, $"{req.MemberName}  ({req.MemberCode})", PTUITheme.FontBold, nameRect, PTUITheme.TextWhite,
                    TextFormatFlags.Left | TextFormatFlags.VerticalCenter | TextFormatFlags.EndEllipsis | TextFormatFlags.NoPrefix);

                // SĐT và Gói tập
                Rectangle phoneRect = new Rectangle(textStartX, 40, infoWidth, 18);
                TextRenderer.DrawText(g, $"SĐT: {req.Phone}   •   Gói: {req.PackageName}", PTUITheme.FontSmall, phoneRect, PTUITheme.TextMuted,
                    TextFormatFlags.Left | TextFormatFlags.VerticalCenter | TextFormatFlags.EndEllipsis | TextFormatFlags.NoPrefix);

                // Khung giờ yêu cầu
                Rectangle dtRect = new Rectangle(18, 70, card.Width - 36, 32);
                PTUITheme.DrawCard(g, dtRect, radius: 6, customBg: Color.FromArgb(28, 34, 48), customBorder: Color.FromArgb(48, 60, 84));

                string timePrefix = req.IsOverdue ? "[QUÁ GIỜ TẬP]  " : "";
                Color timeColor = req.IsOverdue ? Color.FromArgb(251, 191, 36) : PTUITheme.PrimaryLime;
                Rectangle dtTextRect = new Rectangle(dtRect.X + 12, dtRect.Y, dtRect.Width - 24, dtRect.Height);
                TextRenderer.DrawText(g, $"{timePrefix}KHUNG GIỜ YÊU CẦU:  {req.FullDateTimeText}", PTUITheme.FontSmallBold, dtTextRect, timeColor,
                    TextFormatFlags.Left | TextFormatFlags.VerticalCenter | TextFormatFlags.EndEllipsis | TextFormatFlags.NoPrefix);

                // Mục tiêu buổi tập
                Rectangle goalRect = new Rectangle(18, 110, card.Width - 36, 22);
                string noteDisplay = string.IsNullOrWhiteSpace(req.Notes) ? "Học viên không để lại ghi chú." : $"\"{req.Notes}\"";
                TextRenderer.DrawText(g, $"Ghi chú: {noteDisplay}", PTUITheme.FontSmall, goalRect, Color.FromArgb(195, 205, 222),
                    TextFormatFlags.Left | TextFormatFlags.VerticalCenter | TextFormatFlags.EndEllipsis | TextFormatFlags.NoPrefix);
            };

            // Nút Xác Nhận
            Button btnConfirm = new Button
            {
                Text = "✓ Xác Nhận",
                Font = PTUITheme.FontSmallBold,
                ForeColor = Color.FromArgb(15, 17, 21),
                BackColor = PTUITheme.PrimaryLime,
                FlatStyle = FlatStyle.Flat,
                Size = new Size(115, 36),
                Location = new Point(card.Width - 235, 18),
                Cursor = Cursors.Hand,
                UseMnemonic = false
            };
            btnConfirm.FlatAppearance.BorderSize = 0;
            btnConfirm.MouseEnter += (s, e) => btnConfirm.BackColor = Color.FromArgb(215, 255, 30);
            btnConfirm.MouseLeave += (s, e) => btnConfirm.BackColor = PTUITheme.PrimaryLime;
            btnConfirm.Click += (s, e) =>
            {
                var res = _bookingService.ApproveRequest(req.ScheduleID, _ptId);
                if (!res.Success)
                {
                    MessageBox.Show(res.Message, "Không Thể Duyệt Trùng Lịch", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                MessageBox.Show(res.Message, "Xác Nhận Thành Công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                ReloadRequests();
                OnBookingsChanged?.Invoke();
            };

            // Nút Từ Chối
            Button btnReject = new Button
            {
                Text = "✕ Từ Chối",
                Font = PTUITheme.FontSmallBold,
                ForeColor = Color.FromArgb(248, 113, 113),
                BackColor = Color.FromArgb(36, 20, 26),
                FlatStyle = FlatStyle.Flat,
                Size = new Size(100, 36),
                Location = new Point(card.Width - 112, 18),
                Cursor = Cursors.Hand,
                UseMnemonic = false
            };
            btnReject.FlatAppearance.BorderColor = Color.FromArgb(220, 38, 38);
            btnReject.FlatAppearance.BorderSize = 1;
            btnReject.MouseEnter += (s, e) => { btnReject.BackColor = Color.FromArgb(50, 24, 32); btnReject.ForeColor = Color.White; };
            btnReject.MouseLeave += (s, e) => { btnReject.BackColor = Color.FromArgb(36, 20, 26); btnReject.ForeColor = Color.FromArgb(248, 113, 113); };
            btnReject.Click += (s, e) =>
            {
                var dialog = MessageBox.Show($"Bạn có chắc chắn muốn từ chối lịch tập của hội viên {req.MemberName} không?\nHệ thống sẽ thông báo lại cho hội viên chọn lại PT hoặc giờ khác.",
                    "Xác Nhận Từ Chối", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (dialog == DialogResult.Yes)
                {
                    var res = _bookingService.RejectRequest(req.ScheduleID, null, _ptId);
                    if (!res.Success)
                    {
                        MessageBox.Show(res.Message, "Thông Báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                    ReloadRequests();
                    OnBookingsChanged?.Invoke();
                }
            };

            card.Controls.Add(btnConfirm);
            card.Controls.Add(btnReject);

            return card;
        }
    }
}
