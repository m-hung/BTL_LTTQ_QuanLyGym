using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Windows.Forms;

namespace QuanLyGym.UI.Forms.PT
{
    public partial class ucHoiVien : UserControl
    {
        public class PTAssignedMember
        {
            public string Initials { get; set; } = "";
            public string FullName { get; set; } = "";
            public string PackageName { get; set; } = "";
            public string Phone { get; set; } = "";
            public string NextSession { get; set; } = "";
            public string Status { get; set; } = "Đang tập";
            public int CompletedSessions { get; set; }
            public int TotalSessions { get; set; }
            public string Goal { get; set; } = "";
            public Color AvatarColor { get; set; } = PTUITheme.PrimaryLime;
        }

        private List<PTAssignedMember> _members = new List<PTAssignedMember>();
        public event Action<string>? OnViewMemberHistoryRequested;

        public ucHoiVien()
        {
            InitializeComponent();
            this.DoubleBuffered = true;
            this.ResizeRedraw = true;
            this.BackColor = PTUITheme.BgDark;

            cboStatusFilter.SelectedIndex = 0;

            InitData();
            RenderMemberCards();

            pnlCardsContainer.Resize += (s, e) => RenderMemberCards();
        }

        private void InitData()
        {
            _members = new List<PTAssignedMember>
            {
                new PTAssignedMember
                {
                    Initials = "DA",
                    FullName = "Nguyễn Đức An",
                    PackageName = "Tăng Cơ Hypertrophy",
                    Phone = "0912.345.678",
                    NextSession = "19/10 (07:00)",
                    Status = "Đang tập",
                    CompletedSessions = 18,
                    TotalSessions = 36,
                    Goal = "Tăng 3.5kg cơ bắp thân trên, chỉnh chuẩn form Deadlift.",
                    AvatarColor = PTUITheme.PrimaryLime
                },
                new PTAssignedMember
                {
                    Initials = "ML",
                    FullName = "Phạm Mai Linh",
                    PackageName = "Body Transformation Siết Mỡ",
                    Phone = "0988.765.432",
                    NextSession = "19/10 (17:00)",
                    Status = "Đang tập",
                    CompletedSessions = 15,
                    TotalSessions = 30,
                    Goal = "Giảm 4kg mỡ bụng dưới, săn chắc cơ mông đùi.",
                    AvatarColor = PTUITheme.Cyan
                },
                new PTAssignedMember
                {
                    Initials = "HN",
                    FullName = "Lê Hoàng Nam",
                    PackageName = "Sức Bền & Thể Lực",
                    Phone = "0903.112.233",
                    NextSession = "20/10 (09:00)",
                    Status = "Sắp hết gói",
                    CompletedSessions = 22,
                    TotalSessions = 24,
                    Goal = "Chuẩn bị thể lực chạy bán Marathon 21km.",
                    AvatarColor = Color.FromArgb(251, 146, 60)
                },
                new PTAssignedMember
                {
                    Initials = "BT",
                    FullName = "Đỗ Bảo Trâm",
                    PackageName = "Tập Nữ Cơ Bản 1–1",
                    Phone = "0977.889.900",
                    NextSession = "21/10 (14:00)",
                    Status = "Đang tập",
                    CompletedSessions = 5,
                    TotalSessions = 30,
                    Goal = "Cải thiện tư thế lưng gù và giảm mỡ bắp tay.",
                    AvatarColor = Color.FromArgb(168, 85, 247)
                },
                new PTAssignedMember
                {
                    Initials = "QH",
                    FullName = "Trần Quốc Huy",
                    PackageName = "Giảm Cân Cấp Tốc HIIT",
                    Phone = "0934.556.677",
                    NextSession = "20/10 (15:00)",
                    Status = "Đang tập",
                    CompletedSessions = 8,
                    TotalSessions = 20,
                    Goal = "Đốt mỡ toàn thân, nâng cao dung tích phổi.",
                    AvatarColor = Color.FromArgb(56, 189, 248)
                },
                new PTAssignedMember
                {
                    Initials = "TH",
                    FullName = "Vũ Thị Hương",
                    PackageName = "Dãn Cơ & Phục Hồi Chấn Thương",
                    Phone = "0965.443.322",
                    NextSession = "21/10 (08:00)",
                    Status = "Sắp hết gói",
                    CompletedSessions = 11,
                    TotalSessions = 12,
                    Goal = "Giảm đau mỏi thắt lưng, mở rộng khớp vai.",
                    AvatarColor = Color.FromArgb(244, 114, 182)
                }
            };
        }

        private void RenderMemberCards()
        {
            pnlCardsContainer.Controls.Clear();
            pnlCardsContainer.SuspendLayout();

            string searchText = txtSearch.Text.Trim().ToLower();
            string statusFilter = cboStatusFilter.SelectedItem?.ToString() ?? "Tất cả hội viên";

            var filtered = _members.Where(m =>
            {
                bool matchesSearch = string.IsNullOrEmpty(searchText) ||
                                     m.FullName.ToLower().Contains(searchText) ||
                                     m.PackageName.ToLower().Contains(searchText) ||
                                     m.Phone.Contains(searchText);

                bool matchesStatus = (statusFilter == "Tất cả hội viên") || (m.Status == statusFilter);

                return matchesSearch && matchesStatus;
            }).ToList();

            // Tính toán độ rộng thẻ tự động (responsive)
            int containerW = pnlCardsContainer.ClientSize.Width - 10;
            int gap = 16;
            int columns = 3;
            if (containerW >= 1450) columns = 4;
            else if (containerW < 750) columns = 1;
            else if (containerW < 1100) columns = 2;

            int cardWidth = Math.Max(290, (containerW - (columns - 1) * gap) / columns);

            foreach (var m in filtered)
            {
                Panel card = CreateMemberCard(m, cardWidth);
                pnlCardsContainer.Controls.Add(card);
            }

            pnlCardsContainer.ResumeLayout(true);
        }

        private Panel CreateMemberCard(PTAssignedMember m, int cardWidth)
        {
            var card = new PTUITheme.DoubleBufferedPanel
            {
                Width = cardWidth,
                Height = 250,
                Margin = new Padding(0, 0, 16, 16),
                BackColor = PTUITheme.CardBg
            };

            card.Paint += (s, e) =>
            {
                Graphics g = e.Graphics;
                g.SmoothingMode = SmoothingMode.AntiAlias;
                Rectangle r = card.ClientRectangle;

                // 1. Draw card container
                PTUITheme.DrawCard(g, new Rectangle(r.X, r.Y, r.Width - 1, r.Height - 1), radius: 12);

                // 2. Avatar Box
                Rectangle avRect = new Rectangle(18, 16, 42, 42);
                using (SolidBrush b = new SolidBrush(Color.FromArgb(28, m.AvatarColor.R, m.AvatarColor.G, m.AvatarColor.B)))
                {
                    PTUITheme.FillRoundedRectangle(g, b, avRect, 10);
                }
                using (Pen p = new Pen(Color.FromArgb(60, m.AvatarColor.R, m.AvatarColor.G, m.AvatarColor.B), 1f))
                {
                    PTUITheme.DrawRoundedRectangle(g, p, avRect, 10);
                }
                TextRenderer.DrawText(g, m.Initials, PTUITheme.FontBold, avRect, m.AvatarColor,
                    TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter | TextFormatFlags.NoPrefix);

                // 3. Full Name & Package
                Rectangle nameRect = new Rectangle(70, 15, card.Width - 175, 22);
                TextRenderer.DrawText(g, m.FullName, PTUITheme.FontBold, nameRect, PTUITheme.TextWhite,
                    TextFormatFlags.Left | TextFormatFlags.VerticalCenter | TextFormatFlags.EndEllipsis | TextFormatFlags.NoPrefix);

                Rectangle pkgRect = new Rectangle(70, 37, card.Width - 175, 18);
                TextRenderer.DrawText(g, m.PackageName, PTUITheme.FontSmall, pkgRect, PTUITheme.TextMuted,
                    TextFormatFlags.Left | TextFormatFlags.VerticalCenter | TextFormatFlags.EndEllipsis | TextFormatFlags.NoPrefix);

                // 4. Status Badge
                bool isAlmostDone = (m.Status == "Sắp hết gói");
                bool isPaused = (m.Status == "Tạm dừng");
                Rectangle statusRect = new Rectangle(card.Width - 100, 16, 82, 24);
                Color badgeBg = isAlmostDone ? PTUITheme.RedDark : (isPaused ? Color.FromArgb(48, 40, 16) : PTUITheme.GreenDark);
                Color badgeFg = isAlmostDone ? PTUITheme.Red : (isPaused ? Color.FromArgb(234, 179, 8) : PTUITheme.Green);
                PTUITheme.DrawBadge(g, m.Status, statusRect, badgeBg, badgeFg, PTUITheme.FontSmallBold, 6);

                // 5. Progress Section
                int progressPercent = (int)Math.Round((double)m.CompletedSessions / m.TotalSessions * 100);
                Rectangle progTitleRect = new Rectangle(18, 72, 140, 18);
                TextRenderer.DrawText(g, "Số buổi hoàn thành", PTUITheme.FontSmall, progTitleRect, PTUITheme.TextMuted,
                    TextFormatFlags.Left | TextFormatFlags.VerticalCenter | TextFormatFlags.NoPrefix);

                Rectangle progValRect = new Rectangle(card.Width - 165, 72, 147, 18);
                string progText = $"{m.CompletedSessions} / {m.TotalSessions} Buổi ({progressPercent}%)";
                TextRenderer.DrawText(g, progText, PTUITheme.FontSmallBold, progValRect,
                    isAlmostDone ? PTUITheme.Red : PTUITheme.PrimaryLime,
                    TextFormatFlags.Right | TextFormatFlags.VerticalCenter | TextFormatFlags.NoPrefix);

                // 6. Progress Bar
                Rectangle barBgRect = new Rectangle(18, 96, card.Width - 36, 6);
                using (SolidBrush bBg = new SolidBrush(Color.FromArgb(35, 40, 52)))
                {
                    PTUITheme.FillRoundedRectangle(g, bBg, barBgRect, 3);
                }

                int fillWidth = Math.Max(6, (int)((card.Width - 36) * (progressPercent / 100.0)));
                Rectangle barFillRect = new Rectangle(18, 96, fillWidth, 6);
                using (SolidBrush bFill = new SolidBrush(isAlmostDone ? PTUITheme.Red : PTUITheme.PrimaryLime))
                {
                    PTUITheme.FillRoundedRectangle(g, bFill, barFillRect, 3);
                }

                // 7. Goal Box (cao 60px, viền sáng rõ, text căn giữa dọc, tuyệt đối không bị cắt chữ)
                Rectangle goalRect = new Rectangle(18, 116, card.Width - 36, 60);
                PTUITheme.DrawCard(g, goalRect, radius: 8, customBg: Color.FromArgb(20, 25, 36), customBorder: Color.FromArgb(42, 52, 72));

                Rectangle goalTextRect = new Rectangle(goalRect.X + 12, goalRect.Y + 8, goalRect.Width - 24, goalRect.Height - 16);
                using (Font fontGoal = new Font("Segoe UI", 9f, FontStyle.Regular))
                {
                    TextRenderer.DrawText(g, $"Mục tiêu: {m.Goal}", fontGoal, goalTextRect, Color.FromArgb(240, 244, 252),
                        TextFormatFlags.Left | TextFormatFlags.VerticalCenter | TextFormatFlags.WordBreak | TextFormatFlags.NoPrefix);
                }
            };

            // Button: Xem Hồ Sơ & Lịch Sử (ở Y=192, cách hộp mục tiêu 16px, cách đáy thẻ 20px)
            Button btnView = new Button
            {
                Text = "Xem Hồ Sơ & Lịch Sử",
                Font = PTUITheme.FontSmallBold,
                ForeColor = Color.Black,
                BackColor = PTUITheme.PrimaryLime,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand,
                UseMnemonic = false,
                Height = 38,
                Width = card.Width - 36,
                Location = new Point(18, 192)
            };
            btnView.FlatAppearance.BorderSize = 0;
            btnView.MouseEnter += (s, e) => { btnView.BackColor = PTUITheme.LimeHover; btnView.Invalidate(); };
            btnView.MouseLeave += (s, e) => { btnView.BackColor = PTUITheme.PrimaryLime; btnView.Invalidate(); };
            btnView.Paint += (s, pe) =>
            {
                pe.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                Rectangle br = btnView.ClientRectangle;
                using (SolidBrush sb = new SolidBrush(btnView.BackColor))
                {
                    PTUITheme.FillRoundedRectangle(pe.Graphics, sb, br, 8);
                }
                TextRenderer.DrawText(pe.Graphics, btnView.Text, btnView.Font, br, Color.Black,
                    TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter | TextFormatFlags.NoPrefix);
            };
            btnView.Click += (s, e) =>
            {
                OnViewMemberHistoryRequested?.Invoke(m.FullName);
            };

            card.Controls.Add(btnView);
            return card;
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            RenderMemberCards();
        }

        private void cboStatusFilter_SelectedIndexChanged(object sender, EventArgs e)
        {
            RenderMemberCards();
        }
    }
}
