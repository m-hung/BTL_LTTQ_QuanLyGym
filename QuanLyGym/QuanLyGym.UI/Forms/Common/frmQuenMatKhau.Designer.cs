namespace QuanLyGym.UI.Forms.Common
{
    partial class frmQuenMatKhau
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
            pnlTopBar = new Panel();
            lblIcon = new Label();
            lblTitle = new Label();
            lblSubtitle = new Label();
            lblTenDangNhap = new Label();
            pnlTenDangNhapBorder = new Panel();
            pnlTenDangNhapInner = new Panel();
            txtTenDangNhap = new TextBox();
            lblEmail = new Label();
            pnlEmailBorder = new Panel();
            pnlEmailInner = new Panel();
            txtEmail = new TextBox();
            lblLoi = new Label();
            btnHuy = new Button();
            btnGui = new Button();
            pnlTenDangNhapBorder.SuspendLayout();
            pnlTenDangNhapInner.SuspendLayout();
            pnlEmailBorder.SuspendLayout();
            pnlEmailInner.SuspendLayout();
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
            lblIcon.Location = new Point(44, 24);
            lblIcon.Name = "lblIcon";
            lblIcon.TabIndex = 1;
            lblIcon.Text = "🔑";
            //
            // lblTitle
            //
            lblTitle.AutoSize = true;
            lblTitle.Font = new Font("Segoe UI", 18F, FontStyle.Bold);
            lblTitle.ForeColor = Color.FromArgb(15, 23, 42);
            lblTitle.Location = new Point(46, 84);
            lblTitle.Name = "lblTitle";
            lblTitle.TabIndex = 2;
            lblTitle.Text = "Quên mật khẩu?";
            //
            // lblSubtitle
            //
            lblSubtitle.Font = new Font("Segoe UI", 9.5F);
            lblSubtitle.ForeColor = Color.FromArgb(100, 116, 139);
            lblSubtitle.Location = new Point(50, 122);
            lblSubtitle.Name = "lblSubtitle";
            lblSubtitle.Size = new Size(340, 40);
            lblSubtitle.TabIndex = 3;
            lblSubtitle.Text = "Nhập tên đăng nhập và email đã đăng ký. Chúng tôi sẽ gửi hướng dẫn đặt lại mật khẩu cho bạn.";
            //
            // lblTenDangNhap
            //
            lblTenDangNhap.AutoSize = true;
            lblTenDangNhap.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
            lblTenDangNhap.ForeColor = Color.FromArgb(15, 23, 42);
            lblTenDangNhap.Location = new Point(50, 172);
            lblTenDangNhap.Name = "lblTenDangNhap";
            lblTenDangNhap.TabIndex = 4;
            lblTenDangNhap.Text = "Tên đăng nhập";
            //
            // pnlTenDangNhapBorder
            //
            pnlTenDangNhapBorder.BackColor = Color.FromArgb(203, 213, 225);
            pnlTenDangNhapBorder.Controls.Add(pnlTenDangNhapInner);
            pnlTenDangNhapBorder.Location = new Point(50, 196);
            pnlTenDangNhapBorder.Name = "pnlTenDangNhapBorder";
            pnlTenDangNhapBorder.Padding = new Padding(1);
            pnlTenDangNhapBorder.Size = new Size(340, 42);
            pnlTenDangNhapBorder.TabIndex = 5;
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
            // lblEmail
            //
            lblEmail.AutoSize = true;
            lblEmail.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
            lblEmail.ForeColor = Color.FromArgb(15, 23, 42);
            lblEmail.Location = new Point(50, 250);
            lblEmail.Name = "lblEmail";
            lblEmail.TabIndex = 6;
            lblEmail.Text = "Email";
            //
            // pnlEmailBorder
            //
            pnlEmailBorder.BackColor = Color.FromArgb(203, 213, 225);
            pnlEmailBorder.Controls.Add(pnlEmailInner);
            pnlEmailBorder.Location = new Point(50, 274);
            pnlEmailBorder.Name = "pnlEmailBorder";
            pnlEmailBorder.Padding = new Padding(1);
            pnlEmailBorder.Size = new Size(340, 42);
            pnlEmailBorder.TabIndex = 7;
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
            txtEmail.TextChanged += txtInput_TextChanged;
            txtEmail.Enter += txtEmail_Enter;
            txtEmail.Leave += txtEmail_Leave;
            //
            // lblLoi
            //
            lblLoi.Font = new Font("Segoe UI", 9.5F);
            lblLoi.ForeColor = Color.FromArgb(220, 38, 38);
            lblLoi.Location = new Point(50, 324);
            lblLoi.Name = "lblLoi";
            lblLoi.Size = new Size(340, 22);
            lblLoi.TabIndex = 10;
            //
            // btnHuy
            //
            btnHuy.BackColor = Color.FromArgb(241, 245, 249);
            btnHuy.Cursor = Cursors.Hand;
            btnHuy.DialogResult = DialogResult.Cancel;
            btnHuy.FlatAppearance.BorderSize = 0;
            btnHuy.FlatAppearance.MouseOverBackColor = Color.FromArgb(226, 232, 240);
            btnHuy.FlatStyle = FlatStyle.Flat;
            btnHuy.Font = new Font("Segoe UI", 10.5F, FontStyle.Bold);
            btnHuy.ForeColor = Color.FromArgb(15, 23, 42);
            btnHuy.Location = new Point(50, 354);
            btnHuy.Name = "btnHuy";
            btnHuy.Size = new Size(120, 44);
            btnHuy.TabIndex = 9;
            btnHuy.Text = "Hủy";
            btnHuy.UseVisualStyleBackColor = false;
            //
            // btnGui
            //
            btnGui.BackColor = Color.FromArgb(249, 115, 22);
            btnGui.Cursor = Cursors.Hand;
            btnGui.FlatAppearance.BorderSize = 0;
            btnGui.FlatAppearance.MouseDownBackColor = Color.FromArgb(194, 65, 12);
            btnGui.FlatAppearance.MouseOverBackColor = Color.FromArgb(234, 88, 12);
            btnGui.FlatStyle = FlatStyle.Flat;
            btnGui.Font = new Font("Segoe UI", 10.5F, FontStyle.Bold);
            btnGui.ForeColor = Color.White;
            btnGui.Location = new Point(182, 354);
            btnGui.Name = "btnGui";
            btnGui.Size = new Size(208, 44);
            btnGui.TabIndex = 8;
            btnGui.Text = "GỬI YÊU CẦU";
            btnGui.UseVisualStyleBackColor = false;
            btnGui.Click += btnGui_Click;
            //
            // frmQuenMatKhau
            //
            AcceptButton = btnGui;
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.White;
            CancelButton = btnHuy;
            ClientSize = new Size(440, 430);
            Controls.Add(btnGui);
            Controls.Add(btnHuy);
            Controls.Add(lblLoi);
            Controls.Add(pnlEmailBorder);
            Controls.Add(lblEmail);
            Controls.Add(pnlTenDangNhapBorder);
            Controls.Add(lblTenDangNhap);
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
            pnlTenDangNhapBorder.ResumeLayout(false);
            pnlTenDangNhapInner.ResumeLayout(false);
            pnlTenDangNhapInner.PerformLayout();
            pnlEmailBorder.ResumeLayout(false);
            pnlEmailInner.ResumeLayout(false);
            pnlEmailInner.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Panel pnlTopBar;
        private Label lblIcon;
        private Label lblTitle;
        private Label lblSubtitle;
        private Label lblTenDangNhap;
        private Panel pnlTenDangNhapBorder;
        private Panel pnlTenDangNhapInner;
        private TextBox txtTenDangNhap;
        private Label lblEmail;
        private Panel pnlEmailBorder;
        private Panel pnlEmailInner;
        private TextBox txtEmail;
        private Label lblLoi;
        private Button btnHuy;
        private Button btnGui;
    }
}
