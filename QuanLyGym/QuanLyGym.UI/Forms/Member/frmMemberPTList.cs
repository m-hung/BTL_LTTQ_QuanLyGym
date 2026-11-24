using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using QuanLyGym.DAL.Entities;
using PTEntity = QuanLyGym.DAL.Entities.PT;
using QuanLyGym.UI.Helpers;
using QuanLyGym.UI.Services;
using QuanLyGym.UI.Utils;

namespace QuanLyGym.UI.Forms.Member
{
    public class frmMemberPTList : Form
    {
        private Action<string>? _navigateCallback;
        private List<PTEntity> _allPTs = new List<PTEntity>();
        private FlowLayoutPanel pnlList = null!;
        private TextBox txtSearch = null!;
        private ComboBox cboBranch = null!;
        private Label lblTotalCount = null!;

        public frmMemberPTList(Action<string>? navigateCallback = null)
        {
            _navigateCallback = navigateCallback;
            InitializeComponent();
            LoadData();
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();
            this.AutoScaleMode = AutoScaleMode.None;
            this.Text = "Danh Sách Huấn Luyện Viên (PT) - Titan Fitness";
            this.StartPosition = FormStartPosition.CenterScreen;
            this.ClientSize = new Size(1000, 680);
            this.MinimumSize = new Size(850, 580);
            this.BackColor = UITheme.Background;
            this.Font = UITheme.FontRegular;
            this.ForeColor = UITheme.TextDark;
            this.ShowIcon = false;
            this.ShowInTaskbar = false;

            float scale = Math.Max(1.0f, (float)this.DeviceDpi / 96f);

            // 1. Header Bar
            Panel pnlHeader = new Panel
            {
                Dock = DockStyle.Top,
                Height = (int)(72 * scale),
                BackColor = Color.FromArgb(46, 16, 101), // Deep Purple
                Padding = new Padding((int)(20 * scale), (int)(12 * scale), (int)(20 * scale), (int)(10 * scale))
            };

            Label lblTitle = new Label
            {
                Text = "📋 DANH SÁCH HUẤN LUYỆN VIÊN CÁ NHÂN (PT)",
                Font = new Font("Segoe UI", 13f, FontStyle.Bold),
                ForeColor = Color.White,
                Dock = DockStyle.Top,
                Height = (int)(26 * scale),
                UseMnemonic = false
            };

            Label lblSubtitle = new Label
            {
                Text = "Đội ngũ chuyên gia Thể hình & Dinh dưỡng chuẩn quốc tế, tận tâm đồng hành suốt lộ trình luyện tập 1-1",
                Font = UITheme.FontSmall,
                ForeColor = Color.FromArgb(237, 233, 254),
                Dock = DockStyle.Top,
                Height = (int)(20 * scale),
                UseMnemonic = false
            };

            pnlHeader.Controls.Add(lblSubtitle);
            pnlHeader.Controls.Add(lblTitle);

            // 2. Filter & Search Bar
            Panel pnlFilter = new Panel
            {
                Dock = DockStyle.Top,
                Height = (int)(52 * scale),
                BackColor = Color.White,
                Padding = new Padding((int)(16 * scale), (int)(10 * scale), (int)(16 * scale), (int)(10 * scale))
            };

            txtSearch = new TextBox
            {
                Font = UITheme.FontRegular,
                ForeColor = UITheme.TextDark,
                Size = new Size((int)(280 * scale), (int)(30 * scale)),
                Location = new Point((int)(16 * scale), (int)(11 * scale)),
                PlaceholderText = "🔍 Tìm kiếm theo tên HLV, chuyên môn..."
            };
            txtSearch.TextChanged += (s, e) => ApplyFilter();

            Label lblFilterBranch = new Label
            {
                Text = "Chi nhánh:",
                Font = UITheme.FontSmallBold,
                ForeColor = UITheme.TextMuted,
                AutoSize = true,
                Location = new Point((int)(310 * scale), (int)(16 * scale)),
                UseMnemonic = false
            };

            cboBranch = new ComboBox
            {
                DropDownStyle = ComboBoxStyle.DropDownList,
                Font = UITheme.FontRegular,
                Size = new Size((int)(210 * scale), (int)(30 * scale)),
                Location = new Point((int)(385 * scale), (int)(11 * scale))
            };
            cboBranch.Items.Add("Tất cả chi nhánh");
            cboBranch.SelectedIndex = 0;
            cboBranch.SelectedIndexChanged += (s, e) => ApplyFilter();

            lblTotalCount = new Label
            {
                Text = "Đang tải...",
                Font = UITheme.FontSmallBold,
                ForeColor = UITheme.Primary,
                AutoSize = true,
                Location = new Point((int)(615 * scale), (int)(16 * scale)),
                UseMnemonic = false
            };

            pnlFilter.Controls.Add(txtSearch);
            pnlFilter.Controls.Add(lblFilterBranch);
            pnlFilter.Controls.Add(cboBranch);
            pnlFilter.Controls.Add(lblTotalCount);

            // 3. Bottom Close Bar
            Panel pnlBottom = new Panel
            {
                Dock = DockStyle.Bottom,
                Height = (int)(50 * scale),
                BackColor = Color.White,
                Padding = new Padding((int)(16 * scale), (int)(8 * scale), (int)(16 * scale), (int)(8 * scale))
            };

            Button btnClose = new Button
            {
                Text = "Đóng cửa sổ",
                Font = UITheme.FontBold,
                Size = new Size((int)(130 * scale), (int)(34 * scale)),
                BackColor = UITheme.PrimaryLight,
                ForeColor = UITheme.Primary,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand,
                Dock = DockStyle.Right,
                UseMnemonic = false
            };
            btnClose.FlatAppearance.BorderSize = 0;
            btnClose.Click += (s, e) => this.Close();
            pnlBottom.Controls.Add(btnClose);

            // 4. Main Scrollable List
            pnlList = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                AutoScroll = true,
                WrapContents = true,
                FlowDirection = FlowDirection.LeftToRight,
                Padding = new Padding((int)(16 * scale), (int)(10 * scale), (int)(16 * scale), (int)(10 * scale)),
                BackColor = UITheme.Background
            };

            pnlList.Resize += (s, e) => AdjustCardSizes();

            this.Controls.Add(pnlList);
            this.Controls.Add(pnlBottom);
            this.Controls.Add(pnlFilter);
            this.Controls.Add(pnlHeader);

            this.ResumeLayout(true);
        }

        private void AdjustCardSizes()
        {
            float scale = Math.Max(1.0f, (float)this.DeviceDpi / 96f);
            int availableW = pnlList.ClientSize.Width - (int)(36 * scale);
            if (availableW <= 0) return;

            int targetW;
            if (availableW >= (int)(800 * scale))
            {
                targetW = (availableW - (int)(20 * scale)) / 2;
            }
            else
            {
                targetW = availableW;
            }

            foreach (Control c in pnlList.Controls)
            {
                if (c is RoundedPanel)
                {
                    c.Width = targetW;
                }
            }
        }

        private void LoadData()
        {
            _allPTs = MemberService.Instance.GetAllPTs();

            var branches = _allPTs
                .Select(p => p.Branch?.BranchName)
                .Where(b => !string.IsNullOrEmpty(b))
                .Distinct()
                .OrderBy(b => b)
                .ToList();

            foreach (var b in branches)
            {
                cboBranch.Items.Add(b!);
            }

            ApplyFilter();
        }

        private void ApplyFilter()
        {
            float scale = Math.Max(1.0f, (float)this.DeviceDpi / 96f);
            pnlList.SuspendLayout();
            pnlList.Controls.Clear();

            string search = txtSearch.Text.Trim().ToLower();
            string selectedBranch = cboBranch.SelectedItem?.ToString() ?? "Tất cả chi nhánh";

            var filtered = _allPTs.Where(p =>
            {
                bool matchSearch = string.IsNullOrEmpty(search) ||
                    p.FullName.ToLower().Contains(search) ||
                    p.Specialization.ToLower().Contains(search) ||
                    (p.Branch != null && p.Branch.BranchName.ToLower().Contains(search));

                bool matchBranch = selectedBranch == "Tất cả chi nhánh" ||
                    (p.Branch != null && p.Branch.BranchName == selectedBranch);

                return matchSearch && matchBranch;
            }).ToList();

            lblTotalCount.Text = $"Hiển thị: {filtered.Count} / {_allPTs.Count} Huấn luyện viên";

            if (filtered.Count == 0)
            {
                Label lblEmpty = new Label
                {
                    Text = "Không tìm thấy Huấn luyện viên phù hợp với từ khóa tìm kiếm.",
                    Font = UITheme.FontRegular,
                    ForeColor = UITheme.TextMuted,
                    AutoSize = true,
                    Margin = new Padding((int)(20 * scale))
                };
                pnlList.Controls.Add(lblEmpty);
            }
            else
            {
                int availableW = pnlList.ClientSize.Width - (int)(36 * scale);
                int cardW = availableW >= (int)(800 * scale) ? (availableW - (int)(20 * scale)) / 2 : Math.Max(380, availableW);

                foreach (var pt in filtered)
                {
                    pnlList.Controls.Add(CreatePTCard(pt, cardW, scale));
                }
            }

            pnlList.ResumeLayout(true);
        }

        private RoundedPanel CreatePTCard(PTEntity pt, int width, float scale)
        {
            int cardH = (int)Math.Round(185f * scale);

            RoundedPanel card = new RoundedPanel
            {
                Width = width,
                Height = cardH,
                CornerRadius = 12,
                BackColor = Color.White,
                BorderColor = UITheme.Border,
                Margin = new Padding((int)(6 * scale)),
                Padding = new Padding((int)(12 * scale), (int)(10 * scale), (int)(12 * scale), (int)(10 * scale))
            };

            // Avatar tròn bên trái đặt gọn gàng ở góc trên
            int avatarSize = (int)Math.Round(48f * scale);
            Panel pnlAvatarCol = new Panel
            {
                Dock = DockStyle.Left,
                Width = avatarSize + (int)(4 * scale),
                BackColor = Color.Transparent
            };
            Panel pnlAvatar = new Panel
            {
                Size = new Size(avatarSize, avatarSize),
                Location = new Point(0, (int)(4 * scale)),
                BackColor = pt.Gender == "Nữ" ? Color.FromArgb(254, 242, 242) : UITheme.PrimaryLight
            };
            Label lblAvatar = new Label
            {
                Text = pt.Gender == "Nữ" ? "👩‍🏫" : "👨‍🏫",
                Font = new Font("Segoe UI Emoji", 20f),
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleCenter,
                UseMnemonic = false
            };
            pnlAvatar.Controls.Add(lblAvatar);
            pnlAvatarCol.Controls.Add(pnlAvatar);

            // Container thông tin
            Panel pnlInfo = new Panel
            {
                Dock = DockStyle.Fill,
                Padding = new Padding((int)(12 * scale), 0, 0, 0)
            };

            // Hàng nút tương tác (Dock Bottom)
            Panel pnlActions = new Panel
            {
                Dock = DockStyle.Bottom,
                Height = (int)(32 * scale)
            };

            Button btnZalo = new Button
            {
                Text = "💬 Nhắn tin Zalo",
                Font = UITheme.FontSmallBold,
                BackColor = Color.FromArgb(239, 246, 255), // Blue 50
                ForeColor = Color.FromArgb(37, 99, 235),  // Blue 600
                FlatStyle = FlatStyle.Flat,
                Dock = DockStyle.Left,
                Width = (int)(125 * scale),
                Cursor = Cursors.Hand,
                UseMnemonic = false
            };
            btnZalo.FlatAppearance.BorderSize = 0;
            btnZalo.Click += (s, e) =>
            {
                try { Clipboard.SetText(pt.Phone); } catch { }
                MessageBox.Show(
                    $"Đã kết nối với HLV {pt.FullName}!\n\n• Số điện thoại Zalo: {pt.Phone}\n• Email: {pt.Email}\n\n(Đã tự động sao chép số điện thoại vào bộ nhớ tạm)",
                    "Thông tin liên hệ HLV",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information
                );
            };

            Button btnBook = new Button
            {
                Text = "📅 Đặt lịch với HLV",
                Font = UITheme.FontSmallBold,
                BackColor = UITheme.Primary,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Dock = DockStyle.Right,
                Width = (int)(150 * scale),
                Cursor = Cursors.Hand,
                UseMnemonic = false
            };
            btnBook.FlatAppearance.BorderSize = 0;
            btnBook.Click += (s, e) =>
            {
                this.Close();
                _navigateCallback?.Invoke("Schedule");
            };

            pnlActions.Controls.Add(btnZalo);
            pnlActions.Controls.Add(btnBook);

            // Các label thông tin HLV
            Label lblName = new Label
            {
                Text = $"HLV. {pt.FullName}",
                Font = new Font("Segoe UI", 11.5f, FontStyle.Bold),
                ForeColor = UITheme.Primary,
                Dock = DockStyle.Top,
                Height = (int)(24 * scale),
                UseMnemonic = false
            };

            Label lblBranch = new Label
            {
                Text = $"🏢 Chi nhánh: {pt.Branch?.BranchName ?? "Chi nhánh Titan"}",
                Font = UITheme.FontSmallBold,
                ForeColor = Color.FromArgb(76, 29, 149), // Violet
                Dock = DockStyle.Top,
                Height = (int)(19 * scale),
                UseMnemonic = false
            };

            Label lblSpec = new Label
            {
                Text = $"🎯 Chuyên môn: {pt.Specialization}",
                Font = UITheme.FontSmall,
                ForeColor = UITheme.TextDark,
                Dock = DockStyle.Top,
                Height = (int)(19 * scale),
                UseMnemonic = false
            };

            Label lblExpPhone = new Label
            {
                Text = $"⭐ {pt.Experience} năm kinh nghiệm  |  📞 Hotline: {pt.Phone}",
                Font = UITheme.FontSmallBold,
                ForeColor = Color.FromArgb(217, 119, 6), // Amber
                Dock = DockStyle.Top,
                Height = (int)(19 * scale),
                UseMnemonic = false
            };

            Label lblEmail = new Label
            {
                Text = $"✉️ Email: {pt.Email}",
                Font = UITheme.FontSmall,
                ForeColor = UITheme.TextMuted,
                Dock = DockStyle.Top,
                Height = (int)(18 * scale),
                UseMnemonic = false
            };

            pnlInfo.Controls.Add(pnlActions);
            // Thêm các control Dock.Top theo thứ tự ngược lại để lblName nằm ở trên cùng
            pnlInfo.Controls.Add(lblEmail);
            pnlInfo.Controls.Add(lblExpPhone);
            pnlInfo.Controls.Add(lblSpec);
            pnlInfo.Controls.Add(lblBranch);
            pnlInfo.Controls.Add(lblName);

            card.Controls.Add(pnlInfo);
            card.Controls.Add(pnlAvatarCol);

            return card;
        }
    }
}
