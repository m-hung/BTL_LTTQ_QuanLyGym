using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Windows.Forms;
using QuanLyGym.BLL.DTOs.PT;
using QuanLyGym.BLL.Helpers;
using QuanLyGym.BLL.Services.PT;

namespace QuanLyGym.UI.Forms.PT
{
    public partial class ucThongTinCaNhan : UserControl
    {
        private readonly PTProfileService _profileService = new PTProfileService();
        private readonly int _ptId;
        private PTProfileDto _savedProfile = new PTProfileDto();
        private string? _pendingAvatarPath;
        private Image? _previewImage;
        private bool _isLoadingData;

        // Toast banner
        private Panel pnlToast = null!;
        private Label lblToastMsg = null!;
        private bool _toastIsSuccess = true;

        // Two-column layout containers
        private Panel pnlMainWrapper = null!;
        private PTUITheme.DoubleBufferedPanel pnlPreviewCard = null!;
        private PTUITheme.DoubleBufferedPanel pnlPhotoBox = null!;
        private Button btnCamera = null!;
        private Label lblPreviewHint = null!;
        private Label lblPreviewName = null!;
        private Label lblPreviewSpec = null!;
        private PTUITheme.DoubleBufferedPanel pnlStatsBox = null!;
        private FlowLayoutPanel pnlPhotoActions = null!;
        private Button btnChoosePhoto = null!;
        private Button btnUndoPhoto = null!;
        private Button btnDeletePhoto = null!;
        private Label lblAvatarNote = null!;

        // Right column form
        private Panel pnlFormColumn = null!;
        private PTUITheme.DoubleBufferedPanel pnlContactCard = null!;
        private PTUITheme.DoubleBufferedPanel pnlCoachCard = null!;
        private Panel pnlActionsBar = null!;

        private TextBox txtFullName = null!;
        private Label lblErrFullName = null!;
        private TextBox txtPhone = null!;
        private Label lblErrPhone = null!;
        private ComboBox cboGender = null!;
        private DateTimePicker dtpDateOfBirth = null!;
        private Label lblErrDob = null!;
        private TextBox txtEmail = null!;

        private TextBox txtSpecialization = null!;
        private Label lblErrSpec = null!;
        private NumericUpDown numExperience = null!;
        private Label lblErrExp = null!;
        private TextBox txtBranch = null!;
        private TextBox txtBio = null!;
        private Label lblBioCount = null!;
        private Label lblErrBio = null!;

        private Button btnSave = null!;
        private Button btnReset = null!;

        public event Action? OnProfileSaved;

        public ucThongTinCaNhan() : this(1)
        {
        }

        public ucThongTinCaNhan(int ptId)
        {
            _ptId = ptId > 0 ? ptId : 1;
            InitializeComponent();
            this.DoubleBuffered = true;
            this.ResizeRedraw = true;
            this.BackColor = PTUITheme.BgDark;

            EnableDoubleBuffer(pnlScrollHost);
            BuildUI();
            LoadProfileData();

            PTPortalSync.DataChanged += OnSharedDataChanged;
            this.Disposed += (s, e) => PTPortalSync.DataChanged -= OnSharedDataChanged;
            pnlScrollHost.Resize += (s, e) => RelayoutResponsive();
            this.Load += (s, e) => RelayoutResponsive();
        }

        private void OnSharedDataChanged()
        {
            if (IsDisposed) return;
            if (InvokeRequired)
            {
                BeginInvoke(new Action(RefreshStatsOnly));
                return;
            }
            RefreshStatsOnly();
        }

        private void RefreshStatsOnly()
        {
            var latest = _profileService.GetProfile(_savedProfile.PTID > 0 ? _savedProfile.PTID : _ptId);
            _savedProfile.MemberCount = latest.MemberCount;
            _savedProfile.CompletedSessions = latest.CompletedSessions;
            _savedProfile.UpcomingSessions = latest.UpcomingSessions;
            pnlStatsBox?.Invalidate();
        }

        private void BuildUI()
        {
            pnlMainWrapper = new Panel
            {
                Location = new Point(24, 18),
                Width = 1060,
                Height = 760,
                BackColor = Color.Transparent
            };

            BuildToastBanner();
            BuildLeftPreviewCard();
            BuildRightFormCards();

            pnlMainWrapper.Controls.Add(pnlToast);
            pnlMainWrapper.Controls.Add(pnlPreviewCard);
            pnlMainWrapper.Controls.Add(pnlFormColumn);
            pnlScrollHost.Controls.Add(pnlMainWrapper);
        }

        private void BuildToastBanner()
        {
            pnlToast = new PTUITheme.DoubleBufferedPanel
            {
                Location = new Point(0, 0),
                Width = 1060,
                Height = 44,
                Visible = false
            };

            pnlToast.Paint += (s, e) =>
            {
                Rectangle r = pnlToast.ClientRectangle;
                Color bg = _toastIsSuccess ? Color.FromArgb(22, 36, 20) : Color.FromArgb(42, 20, 26);
                Color border = _toastIsSuccess ? Color.FromArgb(80, 204, 255, 0) : Color.FromArgb(140, 244, 63, 94);
                Color accent = _toastIsSuccess ? PTUITheme.PrimaryLime : PTUITheme.Red;

                PTUITheme.DrawCard(e.Graphics, new Rectangle(0, 0, r.Width - 1, r.Height - 1), 8, bg, border);
                using var bAccent = new SolidBrush(accent);
                e.Graphics.FillRectangle(bAccent, 0, 0, 4, r.Height);
            };

            lblToastMsg = new Label
            {
                Location = new Point(16, 10),
                Width = 980,
                Height = 24,
                Font = PTUITheme.FontRegular,
                ForeColor = PTUITheme.PrimaryLime,
                AutoEllipsis = true
            };

            Button btnCloseToast = new Button
            {
                Text = "✕",
                FlatStyle = FlatStyle.Flat,
                ForeColor = PTUITheme.TextMuted,
                BackColor = Color.Transparent,
                Font = PTUITheme.FontSmallBold,
                Size = new Size(30, 30),
                Location = new Point(1022, 7),
                Anchor = AnchorStyles.Top | AnchorStyles.Right,
                Cursor = Cursors.Hand
            };
            btnCloseToast.FlatAppearance.BorderSize = 0;
            btnCloseToast.Click += (s, e) => HideToast();

            pnlToast.Controls.Add(lblToastMsg);
            pnlToast.Controls.Add(btnCloseToast);
        }

        private void BuildLeftPreviewCard()
        {
            pnlPreviewCard = new PTUITheme.DoubleBufferedPanel
            {
                Location = new Point(0, 0),
                Width = 336,
                Height = 620,
                BackColor = PTUITheme.CardBg
            };

            pnlPreviewCard.Paint += (s, e) =>
            {
                Rectangle r = pnlPreviewCard.ClientRectangle;
                PTUITheme.DrawCard(e.Graphics, new Rectangle(0, 0, r.Width - 1, r.Height - 1), 12);

                // Góc nhấn Lime tương tự thẻ hồ sơ công khai bên Web
                using Pen pLime = new Pen(PTUITheme.PrimaryLime, 2.5f);
                e.Graphics.DrawLine(pLime, 1, 12, 1, 1);
                e.Graphics.DrawLine(pLime, 1, 1, 34, 1);
                e.Graphics.DrawLine(pLime, 1, 1, 1, 34);
            };

            pnlPhotoBox = new PTUITheme.DoubleBufferedPanel
            {
                Location = new Point(16, 16),
                Width = 304,
                Height = 285,
                BackColor = Color.FromArgb(14, 17, 23)
            };
            pnlPhotoBox.Paint += PnlPhotoBox_Paint;

            btnCamera = new Button
            {
                Text = "",
                Size = new Size(42, 42),
                Location = new Point(pnlPhotoBox.Width - 54, pnlPhotoBox.Height - 54),
                BackColor = PTUITheme.PrimaryLime,
                ForeColor = Color.Black,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand,
                UseMnemonic = false
            };
            btnCamera.FlatAppearance.BorderSize = 0;
            btnCamera.Paint += (s, e) =>
            {
                e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                using SolidBrush b = new SolidBrush(btnCamera.BackColor);
                PTUITheme.FillRoundedRectangle(e.Graphics, b, btnCamera.ClientRectangle, 8);
                // Icon camera vector sắc nét
                int cx = btnCamera.Width / 2;
                int cy = btnCamera.Height / 2;
                using Pen p = new Pen(Color.Black, 1.8f);
                e.Graphics.DrawRoundedRectangle(p, new Rectangle(cx - 9, cy - 5, 18, 13), 2);
                e.Graphics.DrawEllipse(p, cx - 4, cy - 2, 8, 8);
                e.Graphics.DrawLine(p, cx - 4, cy - 5, cx - 2, cy - 8);
                e.Graphics.DrawLine(p, cx - 2, cy - 8, cx + 2, cy - 8);
                e.Graphics.DrawLine(p, cx + 2, cy - 8, cx + 4, cy - 5);
            };
            btnCamera.MouseEnter += (s, e) => { btnCamera.BackColor = PTUITheme.LimeHover; btnCamera.Invalidate(); };
            btnCamera.MouseLeave += (s, e) => { btnCamera.BackColor = PTUITheme.PrimaryLime; btnCamera.Invalidate(); };
            btnCamera.Click += (s, e) => ChooseNewAvatar();

            pnlPhotoBox.Controls.Add(btnCamera);

            lblPreviewHint = new Label
            {
                Text = "Học viên thấy bạn như thế này",
                Location = new Point(16, 312),
                Width = 304,
                Height = 18,
                Font = PTUITheme.FontSmall,
                ForeColor = PTUITheme.TextMuted
            };

            lblPreviewName = new Label
            {
                Text = "TUẤN MINH",
                Location = new Point(16, 334),
                Width = 304,
                Height = 32,
                Font = new Font("Segoe UI", 15f, FontStyle.Bold),
                ForeColor = PTUITheme.TextWhite,
                AutoEllipsis = true
            };

            lblPreviewSpec = new Label
            {
                Text = "Tăng cơ, Giảm mỡ",
                Location = new Point(16, 368),
                Width = 304,
                Height = 22,
                Font = PTUITheme.FontBold,
                ForeColor = PTUITheme.PrimaryLime,
                AutoEllipsis = true
            };

            pnlStatsBox = new PTUITheme.DoubleBufferedPanel
            {
                Location = new Point(16, 400),
                Width = 304,
                Height = 68,
                BackColor = Color.FromArgb(18, 22, 30)
            };
            pnlStatsBox.Paint += PnlStatsBox_Paint;

            pnlPhotoActions = new FlowLayoutPanel
            {
                Location = new Point(16, 480),
                Width = 304,
                Height = 42,
                BackColor = Color.Transparent,
                WrapContents = true
            };

            btnChoosePhoto = CreateStyledButton("Chọn ảnh mới", false, 118, 36);
            btnChoosePhoto.Margin = new Padding(0, 0, 8, 4);
            btnChoosePhoto.Click += (s, e) => ChooseNewAvatar();

            btnUndoPhoto = CreateStyledButton("Huỷ ảnh chọn", false, 114, 36);
            btnUndoPhoto.Margin = new Padding(0, 0, 8, 4);
            btnUndoPhoto.Visible = false;
            btnUndoPhoto.Click += (s, e) => UndoSelectedAvatar();

            btnDeletePhoto = CreateDangerButton("Xoá ảnh", 86, 36);
            btnDeletePhoto.Margin = new Padding(0, 0, 0, 4);
            btnDeletePhoto.Visible = false;
            btnDeletePhoto.Click += (s, e) => DeleteSavedAvatar();

            pnlPhotoActions.Controls.Add(btnChoosePhoto);
            pnlPhotoActions.Controls.Add(btnUndoPhoto);
            pnlPhotoActions.Controls.Add(btnDeletePhoto);

            lblAvatarNote = new Label
            {
                Text = "JPG, PNG hoặc WEBP, tối đa 5 MB. Ảnh rõ mặt nhìn đẹp nhất.",
                Location = new Point(16, 526),
                Width = 304,
                Height = 40,
                Font = PTUITheme.FontSmall,
                ForeColor = PTUITheme.TextMuted
            };

            pnlPreviewCard.Controls.Add(pnlPhotoBox);
            pnlPreviewCard.Controls.Add(lblPreviewHint);
            pnlPreviewCard.Controls.Add(lblPreviewName);
            pnlPreviewCard.Controls.Add(lblPreviewSpec);
            pnlPreviewCard.Controls.Add(pnlStatsBox);
            pnlPreviewCard.Controls.Add(pnlPhotoActions);
            pnlPreviewCard.Controls.Add(lblAvatarNote);
        }

        private void BuildRightFormCards()
        {
            pnlFormColumn = new Panel
            {
                Location = new Point(360, 0),
                Width = 700,
                Height = 680,
                BackColor = Color.Transparent
            };

            // Card 1: Thông tin liên hệ
            pnlContactCard = new PTUITheme.DoubleBufferedPanel
            {
                Location = new Point(0, 0),
                Width = 700,
                Height = 290,
                BackColor = PTUITheme.CardBg
            };
            pnlContactCard.Paint += (s, e) => DrawSectionCard(e.Graphics, pnlContactCard, "THÔNG TIN LIÊN HỆ");

            Label lblFullName = CreateFieldLabel("Họ và tên *", 20, 52);
            txtFullName = CreateDarkTextBox(20, 74, 320, 100);
            txtFullName.TextChanged += (s, e) =>
            {
                if (_isLoadingData) return;
                string val = txtFullName.Text.Trim();
                lblPreviewName.Text = string.IsNullOrWhiteSpace(val) ? "HỌ VÀ TÊN" : val.ToUpperInvariant();
                lblErrFullName.Text = "";
                if (_previewImage == null) pnlPhotoBox.Invalidate();
            };
            lblErrFullName = CreateErrorLabel(20, 108, 320);

            Label lblPhone = CreateFieldLabel("Số điện thoại", 360, 52);
            txtPhone = CreateDarkTextBox(360, 74, 320, 15);
            txtPhone.PlaceholderText = "0988123456";
            txtPhone.TextChanged += (s, e) => { if (!_isLoadingData) lblErrPhone.Text = ""; };
            lblErrPhone = CreateErrorLabel(360, 108, 320);

            Label lblGender = CreateFieldLabel("Giới tính", 20, 132);
            cboGender = new ComboBox
            {
                Location = new Point(20, 154),
                Width = 320,
                Height = 32
            };
            cboGender.Items.AddRange(new object[] { "Nam", "Nữ", "Khác" });
            PTUITheme.StyleDarkComboBox(cboGender);

            Label lblDob = CreateFieldLabel("Ngày sinh", 360, 132);
            dtpDateOfBirth = new DateTimePicker
            {
                Location = new Point(360, 156),
                Width = 320,
                Format = DateTimePickerFormat.Custom,
                CustomFormat = "dd/MM/yyyy",
                Font = PTUITheme.FontRegular,
                CalendarForeColor = Color.White,
                CalendarMonthBackground = PTUITheme.CardBg
            };
            dtpDateOfBirth.ValueChanged += (s, e) => { if (!_isLoadingData) lblErrDob.Text = ""; };
            lblErrDob = CreateErrorLabel(360, 188, 320);

            Label lblEmail = CreateFieldLabel("Email đăng nhập  [CỐ ĐỊNH]", 20, 208);
            txtEmail = CreateDarkTextBox(20, 230, 660, 100);
            txtEmail.ReadOnly = true;
            txtEmail.BackColor = Color.FromArgb(20, 24, 32);
            txtEmail.ForeColor = PTUITheme.TextMuted;
            txtEmail.TabStop = false;

            Label lblEmailHint = new Label
            {
                Text = "Email dùng để đăng nhập và nhận thông báo hệ thống. Liên hệ Admin nếu cần thay đổi.",
                Location = new Point(20, 264),
                Width = 660,
                Height = 18,
                Font = PTUITheme.FontSmall,
                ForeColor = PTUITheme.TextDarkMuted
            };

            pnlContactCard.Controls.AddRange(new Control[]
            {
                lblFullName, txtFullName, lblErrFullName,
                lblPhone, txtPhone, lblErrPhone,
                lblGender, cboGender,
                lblDob, dtpDateOfBirth, lblErrDob,
                lblEmail, txtEmail, lblEmailHint
            });

            // Card 2: Hồ sơ huấn luyện viên
            pnlCoachCard = new PTUITheme.DoubleBufferedPanel
            {
                Location = new Point(0, 306),
                Width = 700,
                Height = 304,
                BackColor = PTUITheme.CardBg
            };
            pnlCoachCard.Paint += (s, e) => DrawSectionCard(e.Graphics, pnlCoachCard, "HỒ SƠ HUẤN LUYỆN VIÊN");

            Label lblSpec = CreateFieldLabel("Chuyên môn *", 20, 52);
            txtSpecialization = CreateDarkTextBox(20, 74, 320, 200);
            txtSpecialization.PlaceholderText = "Ví dụ: Tăng cơ, giảm mỡ, HIIT";
            txtSpecialization.TextChanged += (s, e) =>
            {
                if (_isLoadingData) return;
                string val = txtSpecialization.Text.Trim();
                lblPreviewSpec.Text = string.IsNullOrWhiteSpace(val) ? "Chuyên môn huấn luyện" : val;
                lblErrSpec.Text = "";
            };
            lblErrSpec = CreateErrorLabel(20, 108, 320);

            Label lblExp = CreateFieldLabel("Số năm kinh nghiệm", 360, 52);
            numExperience = new NumericUpDown
            {
                Location = new Point(360, 75),
                Width = 140,
                Minimum = 0,
                Maximum = 50,
                Font = PTUITheme.FontRegular,
                BackColor = PTUITheme.InputBg,
                ForeColor = PTUITheme.TextWhite,
                BorderStyle = BorderStyle.FixedSingle
            };
            numExperience.ValueChanged += (s, e) => { if (!_isLoadingData) lblErrExp.Text = ""; };
            lblErrExp = CreateErrorLabel(360, 108, 140);

            Label lblBranch = CreateFieldLabel("Cơ sở công tác", 516, 52);
            txtBranch = CreateDarkTextBox(516, 74, 164, 100);
            txtBranch.ReadOnly = true;
            txtBranch.BackColor = Color.FromArgb(20, 24, 32);
            txtBranch.ForeColor = PTUITheme.TextMuted;
            txtBranch.TabStop = false;

            Label lblBio = CreateFieldLabel("Giới thiệu bản thân", 20, 130);
            txtBio = new TextBox
            {
                Location = new Point(20, 152),
                Width = 660,
                Height = 104,
                Multiline = true,
                ScrollBars = ScrollBars.Vertical,
                MaxLength = 1000,
                Font = PTUITheme.FontRegular,
                BackColor = PTUITheme.InputBg,
                ForeColor = PTUITheme.TextWhite,
                BorderStyle = BorderStyle.FixedSingle,
                PlaceholderText = "Kinh nghiệm, chứng chỉ, phong cách huấn luyện (ví dụ: 5 năm kinh nghiệm, chứng chỉ NASM-CPT...)"
            };
            txtBio.TextChanged += (s, e) =>
            {
                lblBioCount.Text = $"{txtBio.Text.Length}/1000 ký tự. Mô tả kinh nghiệm và chứng chỉ giúp học viên tin tưởng hơn.";
                if (!_isLoadingData) lblErrBio.Text = "";
            };

            lblBioCount = new Label
            {
                Text = "0/1000 ký tự",
                Location = new Point(20, 262),
                Width = 480,
                Height = 18,
                Font = PTUITheme.FontSmall,
                ForeColor = PTUITheme.TextMuted
            };
            lblErrBio = CreateErrorLabel(20, 280, 660);

            pnlCoachCard.Controls.AddRange(new Control[]
            {
                lblSpec, txtSpecialization, lblErrSpec,
                lblExp, numExperience, lblErrExp,
                lblBranch, txtBranch,
                lblBio, txtBio, lblBioCount, lblErrBio
            });

            // Actions Bar
            pnlActionsBar = new Panel
            {
                Location = new Point(0, 624),
                Width = 700,
                Height = 46,
                BackColor = Color.Transparent
            };

            btnSave = CreateStyledButton("Lưu thay đổi", true, 150, 40);
            btnSave.Location = new Point(0, 2);
            btnSave.Click += BtnSave_Click;

            btnReset = CreateStyledButton("Hoàn tác", false, 110, 40);
            btnReset.Location = new Point(162, 2);
            btnReset.Click += (s, e) =>
            {
                LoadProfileData();
                ShowToast("Đã hoàn tác các thay đổi chưa lưu.", true);
            };

            pnlActionsBar.Controls.Add(btnSave);
            pnlActionsBar.Controls.Add(btnReset);

            pnlFormColumn.Controls.Add(pnlContactCard);
            pnlFormColumn.Controls.Add(pnlCoachCard);
            pnlFormColumn.Controls.Add(pnlActionsBar);
        }

        private void RelayoutResponsive()
        {
            int availW = pnlScrollHost.ClientSize.Width - 48;
            if (availW < 300) return;

            int topOffset = pnlToast.Visible ? 58 : 0;
            pnlToast.Width = availW;

            if (availW >= 900)
            {
                // Bố cục 2 cột cạnh nhau (như WebGym desktop)
                int leftW = 336;
                int gap = 22;
                int rightW = Math.Max(480, availW - leftW - gap);

                pnlPreviewCard.Location = new Point(0, topOffset);
                pnlPreviewCard.Width = leftW;
                LayoutPreviewCardContent(leftW);

                pnlFormColumn.Location = new Point(leftW + gap, topOffset);
                pnlFormColumn.Width = rightW;
                pnlFormColumn.Height = pnlActionsBar.Bottom + 10;
                LayoutRightColumnInputs(rightW);

                pnlMainWrapper.Width = availW;
                pnlMainWrapper.Height = topOffset + Math.Max(pnlPreviewCard.Height, pnlFormColumn.Height) + 20;
            }
            else
            {
                // Xếp chồng 1 cột khi cửa sổ thu hẹp
                pnlPreviewCard.Location = new Point(0, topOffset);
                pnlPreviewCard.Width = availW;
                LayoutPreviewCardContent(availW);

                pnlFormColumn.Location = new Point(0, topOffset + pnlPreviewCard.Height + 18);
                pnlFormColumn.Width = availW;
                pnlFormColumn.Height = pnlActionsBar.Bottom + 10;
                LayoutRightColumnInputs(availW);

                pnlMainWrapper.Width = availW;
                pnlMainWrapper.Height = pnlFormColumn.Bottom + 20;
            }
        }

        private void LayoutPreviewCardContent(int cardW)
        {
            int contentW = Math.Min(360, Math.Max(260, cardW - 32));
            int leftX = Math.Max(16, (cardW - contentW) / 2);

            pnlPhotoBox.Location = new Point(leftX, 16);
            pnlPhotoBox.Width = contentW;
            btnCamera.Location = new Point(pnlPhotoBox.Width - 54, pnlPhotoBox.Height - 54);

            lblPreviewHint.Location = new Point(leftX, 312);
            lblPreviewHint.Width = contentW;
            lblPreviewName.Location = new Point(leftX, 334);
            lblPreviewName.Width = contentW;
            lblPreviewSpec.Location = new Point(leftX, 368);
            lblPreviewSpec.Width = contentW;

            pnlStatsBox.Location = new Point(leftX, 400);
            pnlStatsBox.Width = contentW;
            pnlStatsBox.Invalidate();

            pnlPhotoActions.Location = new Point(leftX, 480);
            pnlPhotoActions.Width = contentW;
            lblAvatarNote.Location = new Point(leftX, 526);
            lblAvatarNote.Width = contentW;
        }

        private void LayoutRightColumnInputs(int rightW)
        {
            pnlContactCard.Width = rightW;
            pnlCoachCard.Width = rightW;
            pnlActionsBar.Width = rightW;

            int innerW = rightW - 40;
            int halfW = (innerW - 18) / 2;

            txtFullName.Width = halfW;
            lblErrFullName.Width = halfW;
            txtPhone.Location = new Point(20 + halfW + 18, txtPhone.Location.Y);
            txtPhone.Width = halfW;
            lblErrPhone.Location = new Point(txtPhone.Left, lblErrPhone.Location.Y);
            lblErrPhone.Width = halfW;

            // Dời label SĐT theo cột phải
            foreach (Control c in pnlContactCard.Controls)
            {
                if (c is Label lbl && lbl.Text == "Số điện thoại")
                    lbl.Location = new Point(txtPhone.Left, lbl.Location.Y);
                else if (c is Label lbl2 && lbl2.Text == "Ngày sinh")
                    lbl2.Location = new Point(txtPhone.Left, lbl2.Location.Y);
            }

            cboGender.Width = halfW;
            dtpDateOfBirth.Location = new Point(txtPhone.Left, dtpDateOfBirth.Location.Y);
            dtpDateOfBirth.Width = halfW;
            lblErrDob.Location = new Point(txtPhone.Left, lblErrDob.Location.Y);
            lblErrDob.Width = halfW;

            txtEmail.Width = innerW;

            // Card 2
            int specW = Math.Max(220, innerW / 2);
            int rightPartW = innerW - specW - 18;
            int expW = Math.Min(140, rightPartW / 2 - 6);
            int branchW = rightPartW - expW - 12;

            txtSpecialization.Width = specW;
            lblErrSpec.Width = specW;

            int expX = 20 + specW + 18;
            int branchX = expX + expW + 12;
            numExperience.Location = new Point(expX, numExperience.Location.Y);
            numExperience.Width = expW;
            lblErrExp.Location = new Point(expX, lblErrExp.Location.Y);
            lblErrExp.Width = expW;

            txtBranch.Location = new Point(branchX, txtBranch.Location.Y);
            txtBranch.Width = Math.Max(100, branchW);

            foreach (Control c in pnlCoachCard.Controls)
            {
                if (c is Label lbl && lbl.Text == "Số năm kinh nghiệm")
                    lbl.Location = new Point(expX, lbl.Location.Y);
                else if (c is Label lbl2 && lbl2.Text == "Cơ sở công tác")
                    lbl2.Location = new Point(branchX, lbl2.Location.Y);
            }

            txtBio.Width = innerW;
            lblBioCount.Width = innerW;
            lblErrBio.Width = innerW;
        }

        public void LoadProfileData()
        {
            _isLoadingData = true;
            ClearAllErrors();

            _savedProfile = _profileService.GetProfile(_ptId);
            _pendingAvatarPath = null;

            txtFullName.Text = _savedProfile.FullName;
            txtPhone.Text = _savedProfile.Phone;
            cboGender.SelectedItem = cboGender.Items.Contains(_savedProfile.Gender) ? _savedProfile.Gender : "Nam";
            dtpDateOfBirth.Value = _savedProfile.DateOfBirth >= dtpDateOfBirth.MinDate
                ? _savedProfile.DateOfBirth
                : new DateTime(1995, 5, 20);
            txtEmail.Text = _savedProfile.Email;

            txtSpecialization.Text = _savedProfile.Specialization;
            numExperience.Value = Math.Clamp(_savedProfile.Experience, 0, 50);
            txtBranch.Text = _savedProfile.BranchName;
            txtBio.Text = _savedProfile.Bio;
            lblBioCount.Text = $"{txtBio.Text.Length}/1000 ký tự. Mô tả kinh nghiệm và chứng chỉ giúp học viên tin tưởng hơn.";

            lblPreviewName.Text = string.IsNullOrWhiteSpace(_savedProfile.FullName)
                ? "HỌ VÀ TÊN"
                : _savedProfile.FullName.ToUpperInvariant();
            lblPreviewSpec.Text = string.IsNullOrWhiteSpace(_savedProfile.Specialization)
                ? "Chuyên môn huấn luyện"
                : _savedProfile.Specialization;

            LoadPreviewImage(_savedProfile.AvatarPath);
            UpdateAvatarActionButtons();
            lblAvatarNote.Text = "JPG, PNG hoặc WEBP, tối đa 5 MB. Ảnh rõ mặt nhìn đẹp nhất.";
            lblAvatarNote.ForeColor = PTUITheme.TextMuted;
            pnlStatsBox.Invalidate();

            _isLoadingData = false;
        }

        private void ChooseNewAvatar()
        {
            using OpenFileDialog dlg = new OpenFileDialog
            {
                Title = "Chọn ảnh đại diện PT",
                Filter = "Ảnh hỗ trợ (*.jpg;*.jpeg;*.png;*.webp)|*.jpg;*.jpeg;*.png;*.webp"
            };

            if (dlg.ShowDialog() != DialogResult.OK) return;

            var check = _profileService.ValidateAvatarFile(dlg.FileName);
            if (!check.Success)
            {
                lblAvatarNote.Text = check.Message;
                lblAvatarNote.ForeColor = PTUITheme.Red;
                return;
            }

            _pendingAvatarPath = dlg.FileName;
            LoadPreviewImage(_pendingAvatarPath);
            UpdateAvatarActionButtons();

            lblAvatarNote.Text = "Ảnh mới sẽ được lưu khi bạn bấm \"Lưu thay đổi\".";
            lblAvatarNote.ForeColor = PTUITheme.PrimaryLime;
        }

        private void UndoSelectedAvatar()
        {
            _pendingAvatarPath = null;
            LoadPreviewImage(_savedProfile.AvatarPath);
            UpdateAvatarActionButtons();

            lblAvatarNote.Text = "JPG, PNG hoặc WEBP, tối đa 5 MB. Ảnh rõ mặt nhìn đẹp nhất.";
            lblAvatarNote.ForeColor = PTUITheme.TextMuted;
        }

        private void DeleteSavedAvatar()
        {
            var confirm = MessageBox.Show(
                "Xoá ảnh đại diện hiện tại? Hồ sơ sẽ hiển thị chữ viết tắt tên của bạn.",
                "Xác Nhận Xoá Ảnh",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (confirm != DialogResult.Yes) return;

            var res = _profileService.DeleteAvatar(_savedProfile.PTID > 0 ? _savedProfile.PTID : _ptId);
            if (res.Success && res.Data != null)
            {
                _savedProfile = res.Data;
                _pendingAvatarPath = null;
                LoadPreviewImage(null);
                UpdateAvatarActionButtons();
                ShowToast(res.Message, true);
                OnProfileSaved?.Invoke();
            }
        }

        private void BtnSave_Click(object? sender, EventArgs e)
        {
            ClearAllErrors();

            var dto = new PTProfileDto
            {
                PTID = _savedProfile.PTID > 0 ? _savedProfile.PTID : _ptId,
                AccountID = _savedProfile.AccountID,
                FullName = txtFullName.Text,
                Email = _savedProfile.Email,
                Phone = txtPhone.Text,
                Gender = cboGender.SelectedItem?.ToString() ?? "Nam",
                DateOfBirth = dtpDateOfBirth.Value,
                Specialization = txtSpecialization.Text,
                Experience = (int)numExperience.Value,
                Bio = txtBio.Text,
                BranchName = _savedProfile.BranchName
            };

            var res = _profileService.UpdateProfile(dto, _pendingAvatarPath, removeAvatar: false);
            if (!res.Success)
            {
                ApplyFieldErrors(res.FieldErrors);
                ShowToast($"Không thể lưu hồ sơ: {res.Message}", false);
                return;
            }

            if (res.Data != null)
            {
                _savedProfile = res.Data;
                _pendingAvatarPath = null;
                LoadPreviewImage(_savedProfile.AvatarPath);
                UpdateAvatarActionButtons();
                pnlStatsBox.Invalidate();
            }

            lblAvatarNote.Text = "JPG, PNG hoặc WEBP, tối đa 5 MB. Ảnh rõ mặt nhìn đẹp nhất.";
            lblAvatarNote.ForeColor = PTUITheme.TextMuted;

            ShowToast(res.Message, true);
            OnProfileSaved?.Invoke();
        }

        private void UpdateAvatarActionButtons()
        {
            bool hasPending = !string.IsNullOrWhiteSpace(_pendingAvatarPath);
            bool hasSavedAvatar = !string.IsNullOrWhiteSpace(_savedProfile.AvatarPath) && File.Exists(_savedProfile.AvatarPath);

            btnUndoPhoto.Visible = hasPending;
            btnDeletePhoto.Visible = !hasPending && hasSavedAvatar;
        }

        private void LoadPreviewImage(string? path)
        {
            _previewImage?.Dispose();
            _previewImage = null;

            if (!string.IsNullOrWhiteSpace(path) && File.Exists(path))
            {
                try
                {
                    // Đọc qua MemoryStream để tránh khóa file gốc trên đĩa
                    byte[] bytes = File.ReadAllBytes(path);
                    using var ms = new MemoryStream(bytes);
                    _previewImage = Image.FromStream(ms);
                }
                catch
                {
                    _previewImage = null;
                }
            }

            pnlPhotoBox.Invalidate();
        }

        private void PnlPhotoBox_Paint(object? sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            Rectangle r = pnlPhotoBox.ClientRectangle;

            if (_previewImage != null)
            {
                // Vẽ ảnh fill tỉ lệ cover (cắt chính tâm)
                float imgRatio = (float)_previewImage.Width / _previewImage.Height;
                float boxRatio = (float)r.Width / r.Height;

                Rectangle srcRect;
                if (imgRatio > boxRatio)
                {
                    int srcW = (int)(_previewImage.Height * boxRatio);
                    int srcX = (_previewImage.Width - srcW) / 2;
                    srcRect = new Rectangle(srcX, 0, srcW, _previewImage.Height);
                }
                else
                {
                    int srcH = (int)(_previewImage.Width / boxRatio);
                    int srcY = (_previewImage.Height - srcH) / 2;
                    srcRect = new Rectangle(0, srcY, _previewImage.Width, srcH);
                }

                g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                g.DrawImage(_previewImage, r, srcRect, GraphicsUnit.Pixel);
            }
            else
            {
                // Nền kẻ sọc chéo tối & chữ viết tắt màu Lime cỡ lớn
                using var bgBrush = new HatchBrush(HatchStyle.ForwardDiagonal, Color.FromArgb(24, 28, 38), Color.FromArgb(14, 17, 23));
                g.FillRectangle(bgBrush, r);

                string initials = PTDisplayHelper.Initials(txtFullName?.Text ?? _savedProfile.FullName);
                using var iniFont = new Font("Segoe UI", 56f, FontStyle.Bold);
                TextRenderer.DrawText(g, initials, iniFont, r, PTUITheme.PrimaryLime,
                    TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter | TextFormatFlags.NoPrefix);
            }

            Color borderCol = !string.IsNullOrWhiteSpace(_pendingAvatarPath) ? PTUITheme.PrimaryLime : PTUITheme.CardBorder;
            using var pen = new Pen(borderCol, !string.IsNullOrWhiteSpace(_pendingAvatarPath) ? 2f : 1f);
            g.DrawRectangle(pen, 0, 0, r.Width - 1, r.Height - 1);
        }

        private void PnlStatsBox_Paint(object? sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            Rectangle r = pnlStatsBox.ClientRectangle;

            PTUITheme.DrawCard(g, new Rectangle(0, 0, r.Width - 1, r.Height - 1), 8,
                Color.FromArgb(18, 22, 30), PTUITheme.CardBorder);

            int colW = r.Width / 3;
            DrawMiniStat(g, new Rectangle(0, 0, colW, r.Height), "HỌC VIÊN", _savedProfile.MemberCount.ToString(), PTUITheme.TextWhite);
            DrawMiniStat(g, new Rectangle(colW, 0, colW, r.Height), "BUỔI ĐÃ DẠY", _savedProfile.CompletedSessions.ToString(), PTUITheme.PrimaryLime);
            DrawMiniStat(g, new Rectangle(colW * 2, 0, r.Width - colW * 2, r.Height), "CA SẮP TỚI", _savedProfile.UpcomingSessions.ToString(), PTUITheme.Cyan);

            using var divPen = new Pen(PTUITheme.CardBorder, 1f);
            g.DrawLine(divPen, colW, 12, colW, r.Height - 12);
            g.DrawLine(divPen, colW * 2, 12, colW * 2, r.Height - 12);
        }

        private static void DrawMiniStat(Graphics g, Rectangle bounds, string label, string value, Color valueColor)
        {
            Rectangle valRect = new Rectangle(bounds.X, bounds.Y + 8, bounds.Width, 30);
            Rectangle lblRect = new Rectangle(bounds.X, bounds.Y + 40, bounds.Width, 18);

            using var valFont = new Font("Segoe UI", 15f, FontStyle.Bold);
            TextRenderer.DrawText(g, value, valFont, valRect, valueColor,
                TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter | TextFormatFlags.NoPrefix);
            TextRenderer.DrawText(g, label, new Font("Segoe UI", 7.25f, FontStyle.Bold), lblRect, PTUITheme.TextMuted,
                TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter | TextFormatFlags.NoPrefix);
        }

        private static void DrawSectionCard(Graphics g, Panel card, string title)
        {
            Rectangle r = card.ClientRectangle;
            PTUITheme.DrawCard(g, new Rectangle(0, 0, r.Width - 1, r.Height - 1), 12);
            TextRenderer.DrawText(g, title, PTUITheme.FontSection, new Rectangle(20, 14, r.Width - 40, 26),
                PTUITheme.PrimaryLime, TextFormatFlags.Left | TextFormatFlags.VerticalCenter | TextFormatFlags.NoPrefix);

            using var linePen = new Pen(Color.FromArgb(34, 40, 54), 1f);
            g.DrawLine(linePen, 20, 44, r.Width - 20, 44);
        }

        private void ApplyFieldErrors(Dictionary<string, string> errors)
        {
            if (errors.TryGetValue("FullName", out var errName)) lblErrFullName.Text = errName;
            if (errors.TryGetValue("Phone", out var errPhone)) lblErrPhone.Text = errPhone;
            if (errors.TryGetValue("Specialization", out var errSpec)) lblErrSpec.Text = errSpec;
            if (errors.TryGetValue("Experience", out var errExp)) lblErrExp.Text = errExp;
            if (errors.TryGetValue("Bio", out var errBio)) lblErrBio.Text = errBio;
            if (errors.TryGetValue("DateOfBirth", out var errDob)) lblErrDob.Text = errDob;
            if (errors.TryGetValue("Avatar", out var errAv))
            {
                lblAvatarNote.Text = errAv;
                lblAvatarNote.ForeColor = PTUITheme.Red;
            }
        }

        private void ClearAllErrors()
        {
            lblErrFullName.Text = "";
            lblErrPhone.Text = "";
            lblErrSpec.Text = "";
            lblErrExp.Text = "";
            lblErrBio.Text = "";
            lblErrDob.Text = "";
        }

        private void ShowToast(string message, bool isSuccess)
        {
            _toastIsSuccess = isSuccess;
            lblToastMsg.Text = message;
            lblToastMsg.ForeColor = isSuccess ? PTUITheme.PrimaryLime : Color.FromArgb(255, 179, 179);
            pnlToast.Visible = true;
            pnlToast.Invalidate();
            RelayoutResponsive();
            pnlScrollHost.ScrollControlIntoView(pnlToast);
        }

        private void HideToast()
        {
            pnlToast.Visible = false;
            RelayoutResponsive();
        }

        private static Label CreateFieldLabel(string text, int x, int y)
        {
            return new Label
            {
                Text = text,
                Location = new Point(x, y),
                AutoSize = true,
                Font = PTUITheme.FontSmallBold,
                ForeColor = PTUITheme.TextMuted
            };
        }

        private static TextBox CreateDarkTextBox(int x, int y, int width, int maxLength)
        {
            return new TextBox
            {
                Location = new Point(x, y),
                Width = width,
                Height = 30,
                MaxLength = maxLength,
                Font = PTUITheme.FontRegular,
                BackColor = PTUITheme.InputBg,
                ForeColor = PTUITheme.TextWhite,
                BorderStyle = BorderStyle.FixedSingle
            };
        }

        private static Label CreateErrorLabel(int x, int y, int width)
        {
            return new Label
            {
                Text = "",
                Location = new Point(x, y),
                Width = width,
                Height = 16,
                Font = new Font("Segoe UI", 8f, FontStyle.Regular),
                ForeColor = PTUITheme.Red,
                AutoEllipsis = true
            };
        }

        private static Button CreateStyledButton(string text, bool isPrimary, int width, int height)
        {
            var btn = new Button
            {
                Text = text,
                Size = new Size(width, height),
                FlatStyle = FlatStyle.Flat,
                Font = PTUITheme.FontSmallBold,
                Cursor = Cursors.Hand,
                UseMnemonic = false,
                BackColor = isPrimary ? PTUITheme.PrimaryLime : Color.FromArgb(26, 31, 44),
                ForeColor = isPrimary ? Color.Black : PTUITheme.TextWhite
            };
            btn.FlatAppearance.BorderSize = isPrimary ? 0 : 1;
            btn.FlatAppearance.BorderColor = Color.FromArgb(52, 62, 82);
            btn.MouseEnter += (s, e) => btn.BackColor = isPrimary ? PTUITheme.LimeHover : Color.FromArgb(36, 44, 62);
            btn.MouseLeave += (s, e) => btn.BackColor = isPrimary ? PTUITheme.PrimaryLime : Color.FromArgb(26, 31, 44);
            return btn;
        }

        private static Button CreateDangerButton(string text, int width, int height)
        {
            var btn = new Button
            {
                Text = text,
                Size = new Size(width, height),
                FlatStyle = FlatStyle.Flat,
                Font = PTUITheme.FontSmallBold,
                Cursor = Cursors.Hand,
                UseMnemonic = false,
                BackColor = Color.FromArgb(36, 20, 26),
                ForeColor = PTUITheme.Red
            };
            btn.FlatAppearance.BorderSize = 1;
            btn.FlatAppearance.BorderColor = PTUITheme.Red;
            btn.MouseEnter += (s, e) => { btn.BackColor = PTUITheme.Red; btn.ForeColor = Color.White; };
            btn.MouseLeave += (s, e) => { btn.BackColor = Color.FromArgb(36, 20, 26); btn.ForeColor = PTUITheme.Red; };
            return btn;
        }

        private static void EnableDoubleBuffer(Control ctrl)
        {
            try
            {
                typeof(Control).InvokeMember("DoubleBuffered",
                    System.Reflection.BindingFlags.SetProperty | System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic,
                    null, ctrl, new object[] { true });
            }
            catch { }
        }
    }

    internal static class GraphicsExt
    {
        public static void DrawRoundedRectangle(this Graphics g, Pen pen, Rectangle rect, int radius)
        {
            PTUITheme.DrawRoundedRectangle(g, pen, rect, radius);
        }
    }
}
