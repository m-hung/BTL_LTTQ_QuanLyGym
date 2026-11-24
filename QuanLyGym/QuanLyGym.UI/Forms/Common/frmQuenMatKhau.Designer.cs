namespace QuanLyGym.UI.Forms.Common
{
    partial class frmQuenMatKhau
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            pnlTopBar = new Panel();
            lblIcon = new Label();
            lblTitle = new Label();
            lblSubtitle = new Label();
            pnlStep1 = new Panel();
            lblEmail = new Label();
            pnlEmailBorder = new Panel();
            pnlEmailInner = new Panel();
            txtEmail = new TextBox();
            pnlStep2 = new Panel();
            lblMa = new Label();
            pnlMaBorder = new Panel();
            pnlMaInner = new Panel();
            txtMa = new TextBox();
            lblMatKhauMoi = new Label();
            pnlMatKhauMoiBorder = new Panel();
            pnlMatKhauMoiInner = new Panel();
            txtMatKhauMoi = new TextBox();
            lblNhapLai = new Label();
            pnlNhapLaiBorder = new Panel();
            pnlNhapLaiInner = new Panel();
            txtNhapLai = new TextBox();
            chkHienMatKhau = new CheckBox();
            lnkGuiLai = new LinkLabel();
            lnkDoiCheDo = new LinkLabel();
            lblLoi = new Label();
            btnHuy = new Button();
            btnTiepTuc = new Button();
            tmrGuiLai = new System.Windows.Forms.Timer(components);
            pnlStep1.SuspendLayout();
            pnlEmailBorder.SuspendLayout();
            pnlEmailInner.SuspendLayout();
            pnlStep2.SuspendLayout();
            pnlMaBorder.SuspendLayout();
            pnlMaInner.SuspendLayout();
            pnlMatKhauMoiBorder.SuspendLayout();
            pnlMatKhauMoiInner.SuspendLayout();
            pnlNhapLaiBorder.SuspendLayout();
            pnlNhapLaiInner.SuspendLayout();
            SuspendLayout();
            //
            // pnlTopBar
            //
            pnlTopBar.BackColor = Color.FromArgb(249, 115, 22);
            pnlTopBar.Dock = DockStyle.Top;
            pnlTopBar.Location = new Point(0, 0);
            pnlTopBar.Name = "pnlTopBar";
            pnlTopBar.Size = new Size(440, 6);
            pnlTopBar.TabIndex = 0;
            //
            // lblIcon
            //
            lblIcon.AutoSize = true;
            lblIcon.Font = new Font("Segoe UI Emoji", 28F);
            lblIcon.Location = new Point(44, 22);
            lblIcon.Name = "lblIcon";
            lblIcon.Size = new Size(79, 50);
            lblIcon.TabIndex = 1;
            lblIcon.Text = "🔑";
            //
            // lblTitle
            //
            lblTitle.AutoSize = true;
            lblTitle.Font = new Font("Segoe UI", 18F, FontStyle.Bold);
            lblTitle.ForeColor = Color.FromArgb(15, 23, 42);
            lblTitle.Location = new Point(46, 78);
            lblTitle.Name = "lblTitle";
            lblTitle.Size = new Size(215, 32);
            lblTitle.TabIndex = 2;
            lblTitle.Text = "Quên mật khẩu?";
            //
            // lblSubtitle
            //
            lblSubtitle.Font = new Font("Segoe UI", 9.5F);
            lblSubtitle.ForeColor = Color.FromArgb(100, 116, 139);
            lblSubtitle.Location = new Point(50, 116);
            lblSubtitle.Name = "lblSubtitle";
            lblSubtitle.Size = new Size(340, 56);
            lblSubtitle.TabIndex = 3;
            lblSubtitle.Text = "Nhập email đã đăng ký tài khoản.";
            //
            // pnlStep1
            //
            pnlStep1.Controls.Add(pnlEmailBorder);
            pnlStep1.Controls.Add(lblEmail);
            pnlStep1.Location = new Point(50, 178);
            pnlStep1.Name = "pnlStep1";
            pnlStep1.Size = new Size(340, 70);
            pnlStep1.TabIndex = 4;
            //
            // lblEmail
            //
            lblEmail.AutoSize = true;
            lblEmail.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
            lblEmail.ForeColor = Color.FromArgb(15, 23, 42);
            lblEmail.Location = new Point(0, 0);
            lblEmail.Name = "lblEmail";
            lblEmail.Size = new Size(38, 17);
            lblEmail.TabIndex = 0;
            lblEmail.Text = "Email";
            //
            // pnlEmailBorder
            //
            pnlEmailBorder.BackColor = Color.FromArgb(203, 213, 225);
            pnlEmailBorder.Controls.Add(pnlEmailInner);
            pnlEmailBorder.Location = new Point(0, 24);
            pnlEmailBorder.Name = "pnlEmailBorder";
            pnlEmailBorder.Padding = new Padding(1);
            pnlEmailBorder.Size = new Size(340, 42);
            pnlEmailBorder.TabIndex = 1;
            //
            // pnlEmailInner
            //
            pnlEmailInner.BackColor = Color.White;
            pnlEmailInner.Controls.Add(txtEmail);
            pnlEmailInner.Dock = DockStyle.Fill;
            pnlEmailInner.Name = "pnlEmailInner";
            pnlEmailInner.Padding = new Padding(12, 9, 12, 0);
            pnlEmailInner.TabIndex = 0;
            //
            // txtEmail
            //
            txtEmail.BorderStyle = BorderStyle.None;
            txtEmail.Dock = DockStyle.Fill;
            txtEmail.Font = new Font("Segoe UI", 11.5F);
            txtEmail.ForeColor = Color.FromArgb(15, 23, 42);
            txtEmail.Name = "txtEmail";
            txtEmail.PlaceholderText = "vidu@gmail.com";
            txtEmail.TabIndex = 0;
            //
            // pnlStep2
            //
            pnlStep2.Controls.Add(lnkGuiLai);
            pnlStep2.Controls.Add(chkHienMatKhau);
            pnlStep2.Controls.Add(pnlNhapLaiBorder);
            pnlStep2.Controls.Add(lblNhapLai);
            pnlStep2.Controls.Add(pnlMatKhauMoiBorder);
            pnlStep2.Controls.Add(lblMatKhauMoi);
            pnlStep2.Controls.Add(pnlMaBorder);
            pnlStep2.Controls.Add(lblMa);
            pnlStep2.Location = new Point(50, 178);
            pnlStep2.Name = "pnlStep2";
            pnlStep2.Size = new Size(340, 256);
            pnlStep2.TabIndex = 5;
            pnlStep2.Visible = false;
            //
            // lblMa
            //
            lblMa.AutoSize = true;
            lblMa.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
            lblMa.ForeColor = Color.FromArgb(15, 23, 42);
            lblMa.Location = new Point(0, 0);
            lblMa.Name = "lblMa";
            lblMa.Size = new Size(94, 17);
            lblMa.TabIndex = 0;
            lblMa.Text = "Mã xác nhận (6 số)";
            //
            // pnlMaBorder
            //
            pnlMaBorder.BackColor = Color.FromArgb(203, 213, 225);
            pnlMaBorder.Controls.Add(pnlMaInner);
            pnlMaBorder.Location = new Point(0, 24);
            pnlMaBorder.Name = "pnlMaBorder";
            pnlMaBorder.Padding = new Padding(1);
            pnlMaBorder.Size = new Size(340, 42);
            pnlMaBorder.TabIndex = 1;
            //
            // pnlMaInner
            //
            pnlMaInner.BackColor = Color.White;
            pnlMaInner.Controls.Add(txtMa);
            pnlMaInner.Dock = DockStyle.Fill;
            pnlMaInner.Name = "pnlMaInner";
            pnlMaInner.Padding = new Padding(12, 5, 12, 0);
            pnlMaInner.TabIndex = 0;
            //
            // txtMa
            //
            txtMa.BorderStyle = BorderStyle.None;
            txtMa.Dock = DockStyle.Fill;
            txtMa.Font = new Font("Segoe UI", 15F, FontStyle.Bold);
            txtMa.ForeColor = Color.FromArgb(15, 23, 42);
            txtMa.MaxLength = 10;
            txtMa.Name = "txtMa";
            txtMa.PlaceholderText = "______";
            txtMa.TabIndex = 0;
            txtMa.TextAlign = HorizontalAlignment.Center;
            //
            // lblMatKhauMoi
            //
            lblMatKhauMoi.AutoSize = true;
            lblMatKhauMoi.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
            lblMatKhauMoi.ForeColor = Color.FromArgb(15, 23, 42);
            lblMatKhauMoi.Location = new Point(0, 76);
            lblMatKhauMoi.Name = "lblMatKhauMoi";
            lblMatKhauMoi.Size = new Size(80, 17);
            lblMatKhauMoi.TabIndex = 2;
            lblMatKhauMoi.Text = "Mật khẩu mới";
            //
            // pnlMatKhauMoiBorder
            //
            pnlMatKhauMoiBorder.BackColor = Color.FromArgb(203, 213, 225);
            pnlMatKhauMoiBorder.Controls.Add(pnlMatKhauMoiInner);
            pnlMatKhauMoiBorder.Location = new Point(0, 100);
            pnlMatKhauMoiBorder.Name = "pnlMatKhauMoiBorder";
            pnlMatKhauMoiBorder.Padding = new Padding(1);
            pnlMatKhauMoiBorder.Size = new Size(340, 42);
            pnlMatKhauMoiBorder.TabIndex = 3;
            //
            // pnlMatKhauMoiInner
            //
            pnlMatKhauMoiInner.BackColor = Color.White;
            pnlMatKhauMoiInner.Controls.Add(txtMatKhauMoi);
            pnlMatKhauMoiInner.Dock = DockStyle.Fill;
            pnlMatKhauMoiInner.Name = "pnlMatKhauMoiInner";
            pnlMatKhauMoiInner.Padding = new Padding(12, 9, 12, 0);
            pnlMatKhauMoiInner.TabIndex = 0;
            //
            // txtMatKhauMoi
            //
            txtMatKhauMoi.BorderStyle = BorderStyle.None;
            txtMatKhauMoi.Dock = DockStyle.Fill;
            txtMatKhauMoi.Font = new Font("Segoe UI", 11.5F);
            txtMatKhauMoi.ForeColor = Color.FromArgb(15, 23, 42);
            txtMatKhauMoi.Name = "txtMatKhauMoi";
            txtMatKhauMoi.PlaceholderText = "Ít nhất 6 ký tự";
            txtMatKhauMoi.TabIndex = 0;
            txtMatKhauMoi.UseSystemPasswordChar = true;
            //
            // lblNhapLai
            //
            lblNhapLai.AutoSize = true;
            lblNhapLai.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
            lblNhapLai.ForeColor = Color.FromArgb(15, 23, 42);
            lblNhapLai.Location = new Point(0, 152);
            lblNhapLai.Name = "lblNhapLai";
            lblNhapLai.Size = new Size(120, 17);
            lblNhapLai.TabIndex = 4;
            lblNhapLai.Text = "Nhập lại mật khẩu mới";
            //
            // pnlNhapLaiBorder
            //
            pnlNhapLaiBorder.BackColor = Color.FromArgb(203, 213, 225);
            pnlNhapLaiBorder.Controls.Add(pnlNhapLaiInner);
            pnlNhapLaiBorder.Location = new Point(0, 176);
            pnlNhapLaiBorder.Name = "pnlNhapLaiBorder";
            pnlNhapLaiBorder.Padding = new Padding(1);
            pnlNhapLaiBorder.Size = new Size(340, 42);
            pnlNhapLaiBorder.TabIndex = 5;
            //
            // pnlNhapLaiInner
            //
            pnlNhapLaiInner.BackColor = Color.White;
            pnlNhapLaiInner.Controls.Add(txtNhapLai);
            pnlNhapLaiInner.Dock = DockStyle.Fill;
            pnlNhapLaiInner.Name = "pnlNhapLaiInner";
            pnlNhapLaiInner.Padding = new Padding(12, 9, 12, 0);
            pnlNhapLaiInner.TabIndex = 0;
            //
            // txtNhapLai
            //
            txtNhapLai.BorderStyle = BorderStyle.None;
            txtNhapLai.Dock = DockStyle.Fill;
            txtNhapLai.Font = new Font("Segoe UI", 11.5F);
            txtNhapLai.ForeColor = Color.FromArgb(15, 23, 42);
            txtNhapLai.Name = "txtNhapLai";
            txtNhapLai.PlaceholderText = "Nhập lại mật khẩu";
            txtNhapLai.TabIndex = 0;
            txtNhapLai.UseSystemPasswordChar = true;
            //
            // chkHienMatKhau
            //
            chkHienMatKhau.AutoSize = true;
            chkHienMatKhau.Cursor = Cursors.Hand;
            chkHienMatKhau.Font = new Font("Segoe UI", 9.5F);
            chkHienMatKhau.ForeColor = Color.FromArgb(100, 116, 139);
            chkHienMatKhau.Location = new Point(0, 229);
            chkHienMatKhau.Name = "chkHienMatKhau";
            chkHienMatKhau.Size = new Size(104, 21);
            chkHienMatKhau.TabIndex = 6;
            chkHienMatKhau.Text = "Hiện mật khẩu";
            chkHienMatKhau.UseVisualStyleBackColor = true;
            chkHienMatKhau.CheckedChanged += chkHienMatKhau_CheckedChanged;
            //
            // lnkGuiLai
            //
            lnkGuiLai.ActiveLinkColor = Color.FromArgb(234, 88, 12);
            lnkGuiLai.DisabledLinkColor = Color.FromArgb(148, 163, 184);
            lnkGuiLai.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
            lnkGuiLai.LinkBehavior = LinkBehavior.HoverUnderline;
            lnkGuiLai.LinkColor = Color.FromArgb(249, 115, 22);
            lnkGuiLai.Location = new Point(180, 230);
            lnkGuiLai.Name = "lnkGuiLai";
            lnkGuiLai.Size = new Size(160, 22);
            lnkGuiLai.TabIndex = 7;
            lnkGuiLai.TabStop = true;
            lnkGuiLai.Text = "Gửi lại mã";
            lnkGuiLai.TextAlign = ContentAlignment.MiddleRight;
            lnkGuiLai.LinkClicked += lnkGuiLai_LinkClicked;
            //
            // lnkDoiCheDo
            //
            lnkDoiCheDo.ActiveLinkColor = Color.FromArgb(234, 88, 12);
            lnkDoiCheDo.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
            lnkDoiCheDo.LinkBehavior = LinkBehavior.HoverUnderline;
            lnkDoiCheDo.LinkColor = Color.FromArgb(249, 115, 22);
            lnkDoiCheDo.Location = new Point(50, 252);
            lnkDoiCheDo.Name = "lnkDoiCheDo";
            lnkDoiCheDo.Size = new Size(340, 22);
            lnkDoiCheDo.TabIndex = 6;
            lnkDoiCheDo.TabStop = true;
            lnkDoiCheDo.Text = "Quên tên đăng nhập?";
            lnkDoiCheDo.LinkClicked += lnkDoiCheDo_LinkClicked;
            //
            // lblLoi
            //
            lblLoi.Font = new Font("Segoe UI", 9.5F);
            lblLoi.ForeColor = Color.FromArgb(220, 38, 38);
            lblLoi.Location = new Point(50, 278);
            lblLoi.Name = "lblLoi";
            lblLoi.Size = new Size(340, 22);
            lblLoi.TabIndex = 7;
            //
            // btnHuy
            //
            btnHuy.BackColor = Color.FromArgb(241, 245, 249);
            btnHuy.Cursor = Cursors.Hand;
            btnHuy.FlatAppearance.BorderSize = 0;
            btnHuy.FlatAppearance.MouseOverBackColor = Color.FromArgb(226, 232, 240);
            btnHuy.FlatStyle = FlatStyle.Flat;
            btnHuy.Font = new Font("Segoe UI", 10.5F, FontStyle.Bold);
            btnHuy.ForeColor = Color.FromArgb(15, 23, 42);
            btnHuy.Location = new Point(50, 306);
            btnHuy.Name = "btnHuy";
            btnHuy.Size = new Size(120, 44);
            btnHuy.TabIndex = 9;
            btnHuy.Text = "Hủy";
            btnHuy.UseVisualStyleBackColor = false;
            btnHuy.Click += btnHuy_Click;
            //
            // btnTiepTuc
            //
            btnTiepTuc.BackColor = Color.FromArgb(249, 115, 22);
            btnTiepTuc.Cursor = Cursors.Hand;
            btnTiepTuc.FlatAppearance.BorderSize = 0;
            btnTiepTuc.FlatAppearance.MouseDownBackColor = Color.FromArgb(194, 65, 12);
            btnTiepTuc.FlatAppearance.MouseOverBackColor = Color.FromArgb(234, 88, 12);
            btnTiepTuc.FlatStyle = FlatStyle.Flat;
            btnTiepTuc.Font = new Font("Segoe UI", 10.5F, FontStyle.Bold);
            btnTiepTuc.ForeColor = Color.White;
            btnTiepTuc.Location = new Point(182, 306);
            btnTiepTuc.Name = "btnTiepTuc";
            btnTiepTuc.Size = new Size(208, 44);
            btnTiepTuc.TabIndex = 8;
            btnTiepTuc.Text = "GỬI MÃ XÁC NHẬN";
            btnTiepTuc.UseVisualStyleBackColor = false;
            btnTiepTuc.Click += btnTiepTuc_Click;
            //
            // tmrGuiLai
            //
            tmrGuiLai.Interval = 1000;
            tmrGuiLai.Tick += tmrGuiLai_Tick;
            //
            // frmQuenMatKhau
            //
            AcceptButton = btnTiepTuc;
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.White;
            CancelButton = btnHuy;
            ClientSize = new Size(440, 380);
            Controls.Add(btnTiepTuc);
            Controls.Add(btnHuy);
            Controls.Add(lblLoi);
            Controls.Add(lnkDoiCheDo);
            Controls.Add(pnlStep2);
            Controls.Add(pnlStep1);
            Controls.Add(lblSubtitle);
            Controls.Add(lblTitle);
            Controls.Add(lblIcon);
            Controls.Add(pnlTopBar);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "frmQuenMatKhau";
            ShowInTaskbar = false;
            StartPosition = FormStartPosition.CenterParent;
            Text = "Quên mật khẩu";
            Shown += frmQuenMatKhau_Shown;
            pnlStep1.ResumeLayout(false);
            pnlStep1.PerformLayout();
            pnlEmailBorder.ResumeLayout(false);
            pnlEmailInner.ResumeLayout(false);
            pnlEmailInner.PerformLayout();
            pnlStep2.ResumeLayout(false);
            pnlStep2.PerformLayout();
            pnlMaBorder.ResumeLayout(false);
            pnlMaInner.ResumeLayout(false);
            pnlMaInner.PerformLayout();
            pnlMatKhauMoiBorder.ResumeLayout(false);
            pnlMatKhauMoiInner.ResumeLayout(false);
            pnlMatKhauMoiInner.PerformLayout();
            pnlNhapLaiBorder.ResumeLayout(false);
            pnlNhapLaiInner.ResumeLayout(false);
            pnlNhapLaiInner.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Panel pnlTopBar;
        private Label lblIcon;
        private Label lblTitle;
        private Label lblSubtitle;
        private Panel pnlStep1;
        private Label lblEmail;
        private Panel pnlEmailBorder;
        private Panel pnlEmailInner;
        private TextBox txtEmail;
        private Panel pnlStep2;
        private Label lblMa;
        private Panel pnlMaBorder;
        private Panel pnlMaInner;
        private TextBox txtMa;
        private Label lblMatKhauMoi;
        private Panel pnlMatKhauMoiBorder;
        private Panel pnlMatKhauMoiInner;
        private TextBox txtMatKhauMoi;
        private Label lblNhapLai;
        private Panel pnlNhapLaiBorder;
        private Panel pnlNhapLaiInner;
        private TextBox txtNhapLai;
        private CheckBox chkHienMatKhau;
        private LinkLabel lnkGuiLai;
        private LinkLabel lnkDoiCheDo;
        private Label lblLoi;
        private Button btnHuy;
        private Button btnTiepTuc;
        private System.Windows.Forms.Timer tmrGuiLai;
    }
}
