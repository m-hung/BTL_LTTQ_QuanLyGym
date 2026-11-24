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
        public class PTTaughtMember
        {
            public string MemberCode { get; set; } = "";
            public string Initials { get; set; } = "";
            public string FullName { get; set; } = "";
            public string Phone { get; set; } = "";
            public string PackageName { get; set; } = "";
            public int SessionsTaughtByPT { get; set; } // Số buổi PT này đã kèm
            public DateTime LastTrainingDate { get; set; } // Ngày tập gần nhất
            public string Goal { get; set; } = "";
            public Color AvatarColor { get; set; } = PTUITheme.PrimaryLime;
        }

        private List<PTTaughtMember> _members = new List<PTTaughtMember>();
        private bool _isGridView = false; // false: Cards, true: DataGridView (Excel)
        public event Action<string>? OnViewMemberHistoryRequested;

        public ucHoiVien()
        {
            InitializeComponent();
            this.DoubleBuffered = true;
            this.ResizeRedraw = true;
            this.BackColor = PTUITheme.BgDark;

            // Kích hoạt DoubleBuffered cho FlowLayoutPanel để triệt tiêu hiện tượng nhấp nháy khi co giãn cửa sổ
            try
            {
                typeof(FlowLayoutPanel).InvokeMember("DoubleBuffered",
                    System.Reflection.BindingFlags.SetProperty | System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic,
                    null, pnlCardsContainer, new object[] { true });
            }
            catch { }

            InitData();
            SetupGridColumns();
            UpdateViewMode();

            pnlCardsContainer.Resize += (s, e) =>
            {
                if (!_isGridView && pnlCardsContainer.ClientSize.Width > 0)
                {
                    RenderMemberCards();
                }
            };

            pnlFilterBar.Resize += (s, e) =>
            {
                int rightButtonsWidth = btnViewGrid.Width + btnViewCards.Width + 24;
                int maxSearchWidth = pnlFilterBar.ClientSize.Width - rightButtonsWidth - btnSearch.Width - 20;
                if (maxSearchWidth < 140) maxSearchWidth = 140;
                txtSearch.Width = Math.Min(336, maxSearchWidth);
                btnSearch.Location = new Point(txtSearch.Right + 8, btnSearch.Location.Y);
            };
        }

        private void InitData()
        {
            // Danh sách hội viên đã từng được PT này dạy/xác nhận
            _members = new List<PTTaughtMember>
            {
                new PTTaughtMember
                {
                    MemberCode = "HV001",
                    Initials = "DA",
                    FullName = "Nguyễn Đức An",
                    Phone = "0912.345.678",
                    PackageName = "Tăng Cơ Hypertrophy",
                    SessionsTaughtByPT = 8,
                    LastTrainingDate = new DateTime(2026, 11, 4),
                    Goal = "Tăng 3.5kg cơ bắp thân trên, chuẩn form Deadlift và tăng mức tạ.",
                    AvatarColor = PTUITheme.PrimaryLime
                },
                new PTTaughtMember
                {
                    MemberCode = "HV002",
                    Initials = "ML",
                    FullName = "Phạm Mai Linh",
                    Phone = "0988.765.432",
                    PackageName = "Body Transformation Siết Mỡ",
                    SessionsTaughtByPT = 6,
                    LastTrainingDate = new DateTime(2026, 11, 2),
                    Goal = "Giảm 4kg mỡ bụng dưới, săn chắc mông đùi và cải thiện thể lực cardio.",
                    AvatarColor = PTUITheme.Cyan
                },
                new PTTaughtMember
                {
                    MemberCode = "HV003",
                    Initials = "HN",
                    FullName = "Lê Hoàng Nam",
                    Phone = "0903.112.233",
                    PackageName = "Sức Bền & Thể Lực",
                    SessionsTaughtByPT = 12,
                    LastTrainingDate = new DateTime(2026, 10, 30),
                    Goal = "Chuẩn bị thể lực chạy bán Marathon 21km và tăng độ dẻo dai cơ gân kheo.",
                    AvatarColor = Color.FromArgb(251, 146, 60)
                },
                new PTTaughtMember
                {
                    MemberCode = "HV004",
                    Initials = "BT",
                    FullName = "Đỗ Bảo Trâm",
                    Phone = "0977.889.900",
                    PackageName = "Tập Nữ Cơ Bản 1–1",
                    SessionsTaughtByPT = 5,
                    LastTrainingDate = new DateTime(2026, 10, 28),
                    Goal = "Cải thiện tư thế lưng gù, mở rộng khớp vai và giảm mỡ bắp tay.",
                    AvatarColor = Color.FromArgb(168, 85, 247)
                },
                new PTTaughtMember
                {
                    MemberCode = "HV005",
                    Initials = "QH",
                    FullName = "Trần Quốc Huy",
                    Phone = "0934.556.677",
                    PackageName = "Giảm Cân Cấp Tốc HIIT",
                    SessionsTaughtByPT = 4,
                    LastTrainingDate = new DateTime(2026, 10, 24),
                    Goal = "Đốt mỡ toàn thân, tăng dung tích phổi và sức bền cơ tim.",
                    AvatarColor = Color.FromArgb(56, 189, 248)
                },
                new PTTaughtMember
                {
                    MemberCode = "HV006",
                    Initials = "TH",
                    FullName = "Vũ Thị Hương",
                    Phone = "0965.443.322",
                    PackageName = "Dãn Cơ & Phục Hồi Chấn Thương",
                    SessionsTaughtByPT = 7,
                    LastTrainingDate = new DateTime(2026, 10, 20),
                    Goal = "Giảm đau mỏi vùng thắt lưng L4-L5, dãn cơ đùi sau sau giờ làm việc văn phòng.",
                    AvatarColor = Color.FromArgb(244, 114, 182)
                }
            };
        }

        private void SetupGridColumns()
        {
            dgvMembers.Columns.Clear();
            dgvMembers.AutoGenerateColumns = false;

            dgvMembers.Columns.Add(new DataGridViewTextBoxColumn
            {
                HeaderText = "MÃ HV",
                DataPropertyName = "MemberCode",
                Width = 100
            });

            dgvMembers.Columns.Add(new DataGridViewTextBoxColumn
            {
                HeaderText = "HỌ VÀ TÊN",
                DataPropertyName = "FullName",
                Width = 200
            });

            dgvMembers.Columns.Add(new DataGridViewTextBoxColumn
            {
                HeaderText = "SỐ ĐIỆN THOẠI",
                DataPropertyName = "Phone",
                Width = 140
            });

            dgvMembers.Columns.Add(new DataGridViewTextBoxColumn
            {
                HeaderText = "GÓI TẬP",
                DataPropertyName = "PackageName",
                Width = 220
            });

            dgvMembers.Columns.Add(new DataGridViewTextBoxColumn
            {
                HeaderText = "SỐ BUỔI ĐÃ DẠY",
                DataPropertyName = "SessionsTaughtByPTText",
                Width = 150
            });

            dgvMembers.Columns.Add(new DataGridViewTextBoxColumn
            {
                HeaderText = "NGÀY TẬP GẦN NHẤT",
                DataPropertyName = "LastTrainingDateText",
                Width = 180
            });

            dgvMembers.Columns.Add(new DataGridViewTextBoxColumn
            {
                HeaderText = "MỤC TIÊU & GHI CHÚ",
                DataPropertyName = "Goal",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
            });

            var btnCol = new DataGridViewButtonColumn
            {
                HeaderText = "THAO TÁC",
                Text = "Xem Hồ Sơ",
                UseColumnTextForButtonValue = true,
                Width = 130,
                FlatStyle = FlatStyle.Flat
            };
            btnCol.DefaultCellStyle.BackColor = PTUITheme.PrimaryLime;
            btnCol.DefaultCellStyle.ForeColor = Color.Black;
            btnCol.DefaultCellStyle.SelectionBackColor = PTUITheme.LimeHover;
            btnCol.DefaultCellStyle.SelectionForeColor = Color.Black;
            btnCol.DefaultCellStyle.Font = PTUITheme.FontSmallBold;
            dgvMembers.Columns.Add(btnCol);

            dgvMembers.CellContentClick += (s, e) =>
            {
                if (e.RowIndex >= 0 && e.ColumnIndex == dgvMembers.Columns.Count - 1)
                {
                    string memberName = dgvMembers.Rows[e.RowIndex].Cells[1].Value?.ToString() ?? "";
                    if (!string.IsNullOrEmpty(memberName))
                    {
                        OnViewMemberHistoryRequested?.Invoke(memberName);
                    }
                }
            };
        }

        private List<PTTaughtMember> GetFilteredMembers()
        {
            string keyword = txtSearch.Text.Trim().ToLower();

            return _members
                .Where(m => string.IsNullOrEmpty(keyword) ||
                            m.FullName.ToLower().Contains(keyword) ||
                            m.Phone.Contains(keyword) ||
                            m.MemberCode.ToLower().Contains(keyword) ||
                            m.PackageName.ToLower().Contains(keyword))
                .OrderByDescending(m => m.LastTrainingDate) // Sắp xếp theo thứ tự thời gian gần nhất
                .ToList();
        }

        private void UpdateViewMode()
        {
            if (_isGridView)
            {
                btnViewGrid.Font = PTUITheme.FontBold;
                btnViewGrid.ForeColor = PTUITheme.PrimaryLime;
                btnViewCards.Font = PTUITheme.FontRegular;
                btnViewCards.ForeColor = PTUITheme.TextMuted;

                pnlCardsContainer.Visible = false;
                dgvMembers.Visible = true;
                RenderGridView();
            }
            else
            {
                btnViewCards.Font = PTUITheme.FontBold;
                btnViewCards.ForeColor = PTUITheme.PrimaryLime;
                btnViewGrid.Font = PTUITheme.FontRegular;
                btnViewGrid.ForeColor = PTUITheme.TextMuted;

                dgvMembers.Visible = false;
                pnlCardsContainer.Visible = true;
                RenderMemberCards();
            }
        }

        private void RenderGridView()
        {
            var list = GetFilteredMembers();

            var displayList = list.Select(m => new
            {
                m.MemberCode,
                m.FullName,
                m.Phone,
                m.PackageName,
                SessionsTaughtByPTText = $"Đã dạy {m.SessionsTaughtByPT} buổi",
                LastTrainingDateText = m.LastTrainingDate.ToString("dd/MM/yyyy"),
                m.Goal
            }).ToList();

            dgvMembers.DataSource = displayList;
        }

        private void RenderMemberCards()
        {
            if (pnlCardsContainer.ClientSize.Width <= 0) return;

            var filtered = GetFilteredMembers();

            int vScrollW = SystemInformation.VerticalScrollBarWidth;
            int containerW = pnlCardsContainer.ClientSize.Width;

            // Dự phòng chiều rộng thanh cuộn dọc để tránh tự động ngắt dòng
            int safeAvailableW = containerW - (pnlCardsContainer.VerticalScroll.Visible ? 4 : (vScrollW + 6));
            if (safeAvailableW < 280) safeAvailableW = 280;

            int gap = 16;
            int columns;
            if (safeAvailableW >= 1550) columns = 4;
            else if (safeAvailableW >= 1040) columns = 3;
            else if (safeAvailableW >= 660) columns = 2;
            else columns = 1;

            // Tính cardWidth với lề đối xứng gap/2 để dàn đều bề ngang
            int cardWidth = (safeAvailableW / columns) - gap;
            if (cardWidth < 280) cardWidth = 280;

            pnlCardsContainer.SuspendLayout();

            // Tái sử dụng control có sẵn để tối ưu hiệu năng khi thay đổi kích thước
            bool canReuse = (pnlCardsContainer.Controls.Count == filtered.Count);
            if (canReuse)
            {
                for (int i = 0; i < filtered.Count; i++)
                {
                    if (!(pnlCardsContainer.Controls[i] is Panel p && p.Tag == filtered[i]))
                    {
                        canReuse = false;
                        break;
                    }
                }
            }

            if (canReuse)
            {
                for (int i = 0; i < filtered.Count; i++)
                {
                    if (pnlCardsContainer.Controls[i] is Panel card)
                    {
                        card.SuspendLayout();
                        card.Width = cardWidth;
                        card.Margin = new Padding(gap / 2, 0, gap / 2, gap);
                        if (card.Controls.Count > 0 && card.Controls[0] is Button btn)
                        {
                            btn.Width = card.Width - 32;
                            btn.Location = new Point(16, 202);
                        }
                        card.ResumeLayout();
                        card.Invalidate();
                    }
                }
            }
            else
            {
                pnlCardsContainer.Controls.Clear();
                foreach (var m in filtered)
                {
                    Panel card = CreateMemberCard(m, cardWidth, gap);
                    pnlCardsContainer.Controls.Add(card);
                }
            }

            pnlCardsContainer.ResumeLayout(true);
        }

        private Panel CreateMemberCard(PTTaughtMember m, int cardWidth, int gap)
        {
            var card = new PTUITheme.DoubleBufferedPanel
            {
                Tag = m,
                Width = cardWidth,
                Height = 255,
                Margin = new Padding(gap / 2, 0, gap / 2, gap),
                BackColor = PTUITheme.CardBg
            };

            bool isHovered = false;
            card.MouseEnter += (s, e) => { isHovered = true; card.Invalidate(); };
            card.MouseLeave += (s, e) => { isHovered = false; card.Invalidate(); };

            card.Paint += (s, e) =>
            {
                Graphics g = e.Graphics;
                g.SmoothingMode = SmoothingMode.AntiAlias;
                Rectangle r = card.ClientRectangle;

                // 1. Draw card container với hiệu ứng hover nhẹ
                Color borderColor = isHovered ? PTUITheme.PrimaryLime : PTUITheme.CardBorder;
                Color bgColor = isHovered ? Color.FromArgb(26, 30, 42) : PTUITheme.CardBg;
                PTUITheme.DrawCard(g, new Rectangle(r.X, r.Y, r.Width - 1, r.Height - 1), radius: 12, customBg: bgColor, customBorder: borderColor);

                // 2. Avatar Box
                Rectangle avRect = new Rectangle(16, 16, 44, 44);
                using (SolidBrush b = new SolidBrush(Color.FromArgb(28, m.AvatarColor.R, m.AvatarColor.G, m.AvatarColor.B)))
                {
                    PTUITheme.FillRoundedRectangle(g, b, avRect, 10);
                }
                using (Pen p = new Pen(Color.FromArgb(60, m.AvatarColor.R, m.AvatarColor.G, m.AvatarColor.B), 1.2f))
                {
                    PTUITheme.DrawRoundedRectangle(g, p, avRect, 10);
                }
                TextRenderer.DrawText(g, m.Initials, PTUITheme.FontBold, avRect, m.AvatarColor,
                    TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter | TextFormatFlags.NoPrefix);

                // 3. Phone Pill Badge (bên phải)
                int phoneW = 92;
                int phoneX = card.Width - 16 - phoneW;
                Rectangle phoneRect = new Rectangle(phoneX, 16, phoneW, 24);
                PTUITheme.DrawBadge(g, m.Phone, phoneRect, Color.FromArgb(24, 28, 38), PTUITheme.TextMuted, PTUITheme.FontSmall, 6);

                // 4. Full Name & Member Code (vùng giữa avatar và phone badge)
                int textLeft = 68;
                int textW = Math.Max(60, phoneX - textLeft - 8);
                Rectangle nameRect = new Rectangle(textLeft, 14, textW, 22);
                TextRenderer.DrawText(g, m.FullName, PTUITheme.FontBold, nameRect, PTUITheme.TextWhite,
                    TextFormatFlags.Left | TextFormatFlags.VerticalCenter | TextFormatFlags.EndEllipsis | TextFormatFlags.NoPrefix);

                Rectangle pkgRect = new Rectangle(textLeft, 38, textW, 18);
                TextRenderer.DrawText(g, $"{m.MemberCode}  •  {m.PackageName}", PTUITheme.FontSmall, pkgRect, PTUITheme.TextMuted,
                    TextFormatFlags.Left | TextFormatFlags.VerticalCenter | TextFormatFlags.EndEllipsis | TextFormatFlags.NoPrefix);

                // 5. Khối thông tin: Số buổi PT này đã kèm & Ngày tập gần nhất
                Rectangle statsBox = new Rectangle(16, 68, card.Width - 32, 50);
                PTUITheme.DrawCard(g, statsBox, radius: 8, customBg: Color.FromArgb(20, 24, 34), customBorder: Color.FromArgb(36, 44, 60));

                int halfW = (statsBox.Width - 2) / 2;

                // Cột trái: Số buổi PT đã kèm
                Rectangle leftStatRect = new Rectangle(statsBox.X + 8, statsBox.Y + 5, halfW - 10, 18);
                TextRenderer.DrawText(g, "SỐ BUỔI ĐÃ KÈM", PTUITheme.FontSmall, leftStatRect, PTUITheme.TextMuted,
                    TextFormatFlags.Left | TextFormatFlags.VerticalCenter | TextFormatFlags.EndEllipsis);

                Rectangle leftValRect = new Rectangle(statsBox.X + 8, statsBox.Y + 24, halfW - 10, 22);
                TextRenderer.DrawText(g, $"{m.SessionsTaughtByPT} buổi", PTUITheme.FontBold, leftValRect, PTUITheme.PrimaryLime,
                    TextFormatFlags.Left | TextFormatFlags.VerticalCenter | TextFormatFlags.EndEllipsis);

                // Đường phân cách giữa 2 cột thông số
                using (Pen pDivider = new Pen(Color.FromArgb(38, 46, 62), 1f))
                {
                    g.DrawLine(pDivider, statsBox.X + halfW, statsBox.Y + 8, statsBox.X + halfW, statsBox.Bottom - 8);
                }

                // Cột phải: Ngày tập gần nhất
                Rectangle rightStatRect = new Rectangle(statsBox.X + halfW + 10, statsBox.Y + 5, halfW - 14, 18);
                TextRenderer.DrawText(g, "NGÀY TẬP GẦN NHẤT", PTUITheme.FontSmall, rightStatRect, PTUITheme.TextMuted,
                    TextFormatFlags.Left | TextFormatFlags.VerticalCenter | TextFormatFlags.EndEllipsis);

                Rectangle rightValRect = new Rectangle(statsBox.X + halfW + 10, statsBox.Y + 24, halfW - 14, 22);
                TextRenderer.DrawText(g, m.LastTrainingDate.ToString("dd/MM/yyyy"), PTUITheme.FontBold, rightValRect, PTUITheme.Cyan,
                    TextFormatFlags.Left | TextFormatFlags.VerticalCenter | TextFormatFlags.EndEllipsis);

                // 6. Goal Box
                Rectangle goalRect = new Rectangle(16, 126, card.Width - 32, 64);
                PTUITheme.DrawCard(g, goalRect, radius: 8, customBg: Color.FromArgb(18, 22, 30), customBorder: Color.FromArgb(32, 40, 56));

                Rectangle goalTextRect = new Rectangle(goalRect.X + 10, goalRect.Y + 6, goalRect.Width - 20, goalRect.Height - 12);
                using (Font fontGoal = new Font("Segoe UI", 8.5f, FontStyle.Regular))
                {
                    TextRenderer.DrawText(g, $"Mục tiêu: {m.Goal}", fontGoal, goalTextRect, Color.FromArgb(220, 226, 238),
                        TextFormatFlags.Left | TextFormatFlags.Top | TextFormatFlags.WordBreak | TextFormatFlags.EndEllipsis | TextFormatFlags.NoPrefix);
                }
            };

            // Button: Xem Hồ Sơ & Lịch Sử (chuyển sang tab InBody & Thể chất)
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
                Width = card.Width - 32,
                Location = new Point(16, 202)
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

        private void btnSearch_Click(object sender, EventArgs e)
        {
            if (_isGridView) RenderGridView();
            else RenderMemberCards();
        }

        private void txtSearch_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true;
                btnSearch_Click(sender, EventArgs.Empty);
            }
        }

        private void btnViewCards_Click(object sender, EventArgs e)
        {
            _isGridView = false;
            UpdateViewMode();
        }

        private void btnViewGrid_Click(object sender, EventArgs e)
        {
            _isGridView = true;
            UpdateViewMode();
        }
    }
}
