namespace QuanLyGym.UI.Forms.Common
{
    partial class frmLogin
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            pnlLeft = new Panel();
            pnlAccent = new Panel();
            lblSlogan = new Label();
            lblBrand = new Label();
            lblLogo = new Label();
            pnlRight = new Panel();
            lblFooter = new Label();
            btnDangNhap = new Button();
            lblLoi = new Label();
            lnkQuenMatKhau = new LinkLabel();
            chkHienMatKhau = new CheckBox();
            pnlMatKhauBorder = new Panel();
            pnlMatKhauInner = new Panel();
            txtMatKhau = new TextBox();
            lblMatKhau = new Label();
            pnlTenDangNhapBorder = new Panel();
            pnlTenDangNhapInner = new Panel();
            txtTenDangNhap = new TextBox();
            lblTenDangNhap = new Label();
            lblSubtitle = new Label();
            lblTitle = new Label();
            pnlLeft.SuspendLayout();
            pnlRight.SuspendLayout();
            pnlMatKhauBorder.SuspendLayout();
            pnlMatKhauInner.SuspendLayout();
            pnlTenDangNhapBorder.SuspendLayout();
            pnlTenDangNhapInner.SuspendLayout();
            SuspendLayout();
            //
            // pnlLeft
            //
            pnlLeft.BackColor = Color.FromArgb(30, 41, 59);
            pnlLeft.Controls.Add(pnlAccent);
            pnlLeft.Controls.Add(lblSlogan);
            pnlLeft.Controls.Add(lblBrand);
            pnlLeft.Controls.Add(lblLogo);
            pnlLeft.Dock = DockStyle.Left;
            pnlLeft.Location = new Point(0, 0);
            pnlLeft.Name = "pnlLeft";
            pnlLeft.Size = new Size(360, 500);
            pnlLeft.TabIndex = 0;
            //
            // pnlAccent
            //
            pnlAccent.BackColor = Color.FromArgb(249, 115, 22);
            pnlAccent.Location = new Point(150, 340);
            pnlAccent.Name = "pnlAccent";
            pnlAccent.Size = new Size(60, 4);
            pnlAccent.TabIndex = 3;
            //
            // lblSlogan
            //
            lblSlogan.Font = new Font("Segoe UI", 10.5F);
            lblSlogan.ForeColor = Color.FromArgb(203, 213, 225);
            lblSlogan.Location = new Point(0, 278);
            lblSlogan.Name = "lblSlogan";
            lblSlogan.Size = new Size(360, 50);
            lblSlogan.TabIndex = 2;
            lblSlogan.Text = "Hệ thống quản lý phòng tập\r\nĐơn giản – Nhanh chóng – Hiệu quả";
            lblSlogan.TextAlign = ContentAlignment.TopCenter;
            //
            // lblBrand
            //
            lblBrand.Font = new Font("Segoe UI", 24F, FontStyle.Bold);
            lblBrand.ForeColor = Color.White;
            lblBrand.Location = new Point(0, 222);
            lblBrand.Name = "lblBrand";
            lblBrand.Size = new Size(360, 48);
            lblBrand.TabIndex = 1;
            lblBrand.Text = "GYM MANAGER";
            lblBrand.TextAlign = ContentAlignment.MiddleCenter;
            //
            // lblLogo
            //
            lblLogo.Font = new Font("Segoe UI Emoji", 54F);
            lblLogo.ForeColor = Color.FromArgb(249, 115, 22);
            lblLogo.Location = new Point(0, 110);
            lblLogo.Name = "lblLogo";
            lblLogo.Size = new Size(360, 105);
            lblLogo.TabIndex = 0;
            lblLogo.Text = "🏋";
            lblLogo.TextAlign = ContentAlignment.MiddleCenter;
            //
            // pnlRight
            //
            pnlRight.BackColor = Color.White;
            pnlRight.Controls.Add(lblFooter);
            pnlRight.Controls.Add(btnDangNhap);
            pnlRight.Controls.Add(lblLoi);
            pnlRight.Controls.Add(lnkQuenMatKhau);
            pnlRight.Controls.Add(chkHienMatKhau);
            pnlRight.Controls.Add(pnlMatKhauBorder);
            pnlRight.Controls.Add(lblMatKhau);
            pnlRight.Controls.Add(pnlTenDangNhapBorder);
            pnlRight.Controls.Add(lblTenDangNhap);
            pnlRight.Controls.Add(lblSubtitle);
            pnlRight.Controls.Add(lblTitle);
            pnlRight.Dock = DockStyle.Fill;
            pnlRight.Location = new Point(360, 0);
            pnlRight.Name = "pnlRight";
            pnlRight.Size = new Size(460, 500);
            pnlRight.TabIndex = 1;
            //
            // lblTitle
            //
            lblTitle.AutoSize = true;
            lblTitle.Font = new Font("Segoe UI", 22F, FontStyle.Bold);
            lblTitle.ForeColor = Color.FromArgb(15, 23, 42);
            lblTitle.Location = new Point(66, 45);
            lblTitle.Name = "lblTitle";
            lblTitle.TabIndex = 0;
            lblTitle.Text = "Đăng nhập";
            //
            // lblSubtitle
            //
            lblSubtitle.AutoSize = true;
            lblSubtitle.Font = new Font("Segoe UI", 10F);
            lblSubtitle.ForeColor = Color.FromArgb(100, 116, 139);
            lblSubtitle.Location = new Point(70, 92);
            lblSubtitle.Name = "lblSubtitle";
            lblSubtitle.TabIndex = 1;
            lblSubtitle.Text = "Chào mừng bạn quay lại! Vui lòng đăng nhập.";
            //
            // lblTenDangNhap
            //
            lblTenDangNhap.AutoSize = true;
            lblTenDangNhap.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
            lblTenDangNhap.ForeColor = Color.FromArgb(15, 23, 42);
            lblTenDangNhap.Location = new Point(70, 135);
            lblTenDangNhap.Name = "lblTenDangNhap";
            lblTenDangNhap.TabIndex = 2;
            lblTenDangNhap.Text = "Tên đăng nhập";
            //
            // pnlTenDangNhapBorder
            //
            pnlTenDangNhapBorder.BackColor = Color.FromArgb(203, 213, 225);
            pnlTenDangNhapBorder.Controls.Add(pnlTenDangNhapInner);
            pnlTenDangNhapBorder.Location = new Point(70, 159);
            pnlTenDangNhapBorder.Name = "pnlTenDangNhapBorder";
            pnlTenDangNhapBorder.Padding = new Padding(1);
            pnlTenDangNhapBorder.Size = new Size(320, 42);
            pnlTenDangNhapBorder.TabIndex = 3;
            //
            // pnlTenDangNhapInner
            //
            pnlTenDangNhapInner.BackColor = Color.White;
            pnlTenDangNhapInner.Controls.Add(txtTenDangNhap);
            pnlTenDangNhapInner.Dock = DockStyle.Fill;
            pnlTenDangNhapInner.Name = "pnlTenDangNhapInner";
            pnlTenDangNhapInner.Padding = new Padding(12, 9, 12, 0);
            pnlTenDangNhapInner.TabIndex = 0;
            //
            // txtTenDangNhap
            //
            txtTenDangNhap.BorderStyle = BorderStyle.None;
            txtTenDangNhap.Dock = DockStyle.Fill;
            txtTenDangNhap.Font = new Font("Segoe UI", 11.5F);
            txtTenDangNhap.ForeColor = Color.FromArgb(15, 23, 42);
            txtTenDangNhap.Name = "txtTenDangNhap";
            txtTenDangNhap.PlaceholderText = "Nhập tên đăng nhập";
            txtTenDangNhap.TabIndex = 0;
            txtTenDangNhap.TextChanged += txtInput_TextChanged;
            txtTenDangNhap.Enter += txtTenDangNhap_Enter;
            txtTenDangNhap.Leave += txtTenDangNhap_Leave;
            //
            // lblMatKhau
            //
            lblMatKhau.AutoSize = true;
            lblMatKhau.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
            lblMatKhau.ForeColor = Color.FromArgb(15, 23, 42);
            lblMatKhau.Location = new Point(70, 215);
            lblMatKhau.Name = "lblMatKhau";
            lblMatKhau.TabIndex = 4;
            lblMatKhau.Text = "Mật khẩu";
            //
            // pnlMatKhauBorder
            //
            pnlMatKhauBorder.BackColor = Color.FromArgb(203, 213, 225);
            pnlMatKhauBorder.Controls.Add(pnlMatKhauInner);
            pnlMatKhauBorder.Location = new Point(70, 239);
            pnlMatKhauBorder.Name = "pnlMatKhauBorder";
            pnlMatKhauBorder.Padding = new Padding(1);
            pnlMatKhauBorder.Size = new Size(320, 42);
            pnlMatKhauBorder.TabIndex = 5;
            //
            // pnlMatKhauInner
            //
            pnlMatKhauInner.BackColor = Color.White;
            pnlMatKhauInner.Controls.Add(txtMatKhau);
            pnlMatKhauInner.Dock = DockStyle.Fill;
            pnlMatKhauInner.Name = "pnlMatKhauInner";
            pnlMatKhauInner.Padding = new Padding(12, 9, 12, 0);
            pnlMatKhauInner.TabIndex = 0;
            //
            // txtMatKhau
            //
            txtMatKhau.BorderStyle = BorderStyle.None;
            txtMatKhau.Dock = DockStyle.Fill;
            txtMatKhau.Font = new Font("Segoe UI", 11.5F);
            txtMatKhau.ForeColor = Color.FromArgb(15, 23, 42);
            txtMatKhau.Name = "txtMatKhau";
            txtMatKhau.PlaceholderText = "Nhập mật khẩu";
            txtMatKhau.TabIndex = 0;
            txtMatKhau.UseSystemPasswordChar = true;
            txtMatKhau.TextChanged += txtInput_TextChanged;
            txtMatKhau.Enter += txtMatKhau_Enter;
            txtMatKhau.Leave += txtMatKhau_Leave;
            //
            // chkHienMatKhau
            //
            chkHienMatKhau.AutoSize = true;
            chkHienMatKhau.Cursor = Cursors.Hand;
            chkHienMatKhau.Font = new Font("Segoe UI", 9.5F);
            chkHienMatKhau.ForeColor = Color.FromArgb(100, 116, 139);
            chkHienMatKhau.Location = new Point(70, 292);
            chkHienMatKhau.Name = "chkHienMatKhau";
            chkHienMatKhau.TabIndex = 6;
            chkHienMatKhau.Text = "Hiện mật khẩu";
            chkHienMatKhau.UseVisualStyleBackColor = true;
            chkHienMatKhau.CheckedChanged += chkHienMatKhau_CheckedChanged;
            //
            // lnkQuenMatKhau
            //
            lnkQuenMatKhau.ActiveLinkColor = Color.FromArgb(234, 88, 12);
            lnkQuenMatKhau.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
            lnkQuenMatKhau.LinkBehavior = LinkBehavior.HoverUnderline;
            lnkQuenMatKhau.LinkColor = Color.FromArgb(249, 115, 22);
            lnkQuenMatKhau.Location = new Point(250, 293);
            lnkQuenMatKhau.Name = "lnkQuenMatKhau";
            lnkQuenMatKhau.Size = new Size(140, 22);
            lnkQuenMatKhau.TabIndex = 8;
            lnkQuenMatKhau.TabStop = true;
            lnkQuenMatKhau.Text = "Quên mật khẩu?";
            lnkQuenMatKhau.TextAlign = ContentAlignment.MiddleRight;
            lnkQuenMatKhau.LinkClicked += lnkQuenMatKhau_LinkClicked;
            //
            // lblLoi
            //
            lblLoi.Font = new Font("Segoe UI", 9.5F);
            lblLoi.ForeColor = Color.FromArgb(220, 38, 38);
            lblLoi.Location = new Point(70, 322);
            lblLoi.Name = "lblLoi";
            lblLoi.Size = new Size(320, 22);
            lblLoi.TabIndex = 9;
            //
            // btnDangNhap
            //
            btnDangNhap.BackColor = Color.FromArgb(249, 115, 22);
            btnDangNhap.Cursor = Cursors.Hand;
            btnDangNhap.FlatAppearance.BorderSize = 0;
            btnDangNhap.FlatAppearance.MouseDownBackColor = Color.FromArgb(194, 65, 12);
            btnDangNhap.FlatAppearance.MouseOverBackColor = Color.FromArgb(234, 88, 12);
            btnDangNhap.FlatStyle = FlatStyle.Flat;
            btnDangNhap.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            btnDangNhap.ForeColor = Color.White;
            btnDangNhap.Location = new Point(70, 348);
            btnDangNhap.Name = "btnDangNhap";
            btnDangNhap.Size = new Size(320, 46);
            btnDangNhap.TabIndex = 7;
            btnDangNhap.Text = "ĐĂNG NHẬP";
            btnDangNhap.UseVisualStyleBackColor = false;
            btnDangNhap.Click += btnDangNhap_Click;
            //
            // lblFooter
            //
            lblFooter.Font = new Font("Segoe UI", 8.5F);
            lblFooter.ForeColor = Color.FromArgb(100, 116, 139);
            lblFooter.Location = new Point(70, 450);
            lblFooter.Name = "lblFooter";
            lblFooter.Size = new Size(320, 20);
            lblFooter.TabIndex = 10;
            lblFooter.Text = "© 2026 Gym Manager";
            lblFooter.TextAlign = ContentAlignment.MiddleCenter;
            //
            // frmLogin
            //
            AcceptButton = btnDangNhap;
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.White;
            ClientSize = new Size(820, 500);
            Controls.Add(pnlRight);
            Controls.Add(pnlLeft);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            Name = "frmLogin";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Đăng nhập - Quản lý phòng Gym";
            Shown += frmLogin_Shown;
            pnlLeft.ResumeLayout(false);
            pnlRight.ResumeLayout(false);
            pnlRight.PerformLayout();
            pnlMatKhauBorder.ResumeLayout(false);
            pnlMatKhauInner.ResumeLayout(false);
            pnlMatKhauInner.PerformLayout();
            pnlTenDangNhapBorder.ResumeLayout(false);
            pnlTenDangNhapInner.ResumeLayout(false);
            pnlTenDangNhapInner.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private Panel pnlLeft;
        private Panel pnlAccent;
        private Label lblSlogan;
        private Label lblBrand;
        private Label lblLogo;
        private Panel pnlRight;
        private Label lblTitle;
        private Label lblSubtitle;
        private Label lblTenDangNhap;
        private Panel pnlTenDangNhapBorder;
        private Panel pnlTenDangNhapInner;
        private TextBox txtTenDangNhap;
        private Label lblMatKhau;
        private Panel pnlMatKhauBorder;
        private Panel pnlMatKhauInner;
        private TextBox txtMatKhau;
        private CheckBox chkHienMatKhau;
        private LinkLabel lnkQuenMatKhau;
        private Label lblLoi;
        private Button btnDangNhap;
        private Label lblFooter;
    }
}
