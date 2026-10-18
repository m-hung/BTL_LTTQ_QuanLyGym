namespace QuanLyGym.UI.Forms.PT
{
    partial class frmPTMain
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
            pnlHeader = new Panel();
            lblXinChao = new Label();
            btnDangXuat = new Button();
            lblAppName = new Label();
            pnlContent = new Panel();
            pnlCard = new Panel();
            lblGhiChu = new Label();
            lblTenDangNhap = new Label();
            lblVaiTro = new Label();
            lblIcon = new Label();
            pnlCardTop = new Panel();
            pnlHeader.SuspendLayout();
            pnlContent.SuspendLayout();
            pnlCard.SuspendLayout();
            SuspendLayout();
            // 
            // pnlHeader
            // 
            pnlHeader.BackColor = Color.FromArgb(22, 163, 74);
            pnlHeader.Controls.Add(lblXinChao);
            pnlHeader.Controls.Add(btnDangXuat);
            pnlHeader.Controls.Add(lblAppName);
            pnlHeader.Dock = DockStyle.Top;
            pnlHeader.Location = new Point(0, 0);
            pnlHeader.Name = "pnlHeader";
            pnlHeader.Size = new Size(1000, 64);
            pnlHeader.TabIndex = 0;
            // 
            // lblAppName
            // 
            lblAppName.AutoSize = true;
            lblAppName.Font = new Font("Segoe UI", 14F, FontStyle.Bold);
            lblAppName.ForeColor = Color.White;
            lblAppName.Location = new Point(20, 18);
            lblAppName.Name = "lblAppName";
            lblAppName.TabIndex = 0;
            lblAppName.Text = "GYM MANAGER";
            // 
            // lblXinChao
            // 
            lblXinChao.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            lblXinChao.Font = new Font("Segoe UI", 10F);
            lblXinChao.ForeColor = Color.White;
            lblXinChao.Location = new Point(560, 14);
            lblXinChao.Name = "lblXinChao";
            lblXinChao.Size = new Size(300, 36);
            lblXinChao.TabIndex = 1;
            lblXinChao.Text = "Xin chào";
            lblXinChao.TextAlign = ContentAlignment.MiddleRight;
            // 
            // btnDangXuat
            // 
            btnDangXuat.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnDangXuat.BackColor = Color.White;
            btnDangXuat.Cursor = Cursors.Hand;
            btnDangXuat.FlatAppearance.BorderSize = 0;
            btnDangXuat.FlatAppearance.MouseOverBackColor = Color.FromArgb(240, 253, 244);
            btnDangXuat.FlatStyle = FlatStyle.Flat;
            btnDangXuat.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            btnDangXuat.ForeColor = Color.FromArgb(22, 163, 74);
            btnDangXuat.Location = new Point(872, 14);
            btnDangXuat.Name = "btnDangXuat";
            btnDangXuat.Size = new Size(110, 36);
            btnDangXuat.TabIndex = 2;
            btnDangXuat.Text = "Đăng xuất";
            btnDangXuat.UseVisualStyleBackColor = false;
            btnDangXuat.Click += btnDangXuat_Click;
            // 
            // pnlContent
            // 
            pnlContent.BackColor = Color.FromArgb(248, 250, 252);
            pnlContent.Controls.Add(pnlCard);
            pnlContent.Dock = DockStyle.Fill;
            pnlContent.Location = new Point(0, 64);
            pnlContent.Name = "pnlContent";
            pnlContent.Size = new Size(1000, 556);
            pnlContent.TabIndex = 1;
            // 
            // pnlCard
            // 
            pnlCard.Anchor = AnchorStyles.None;
            pnlCard.BackColor = Color.White;
            pnlCard.BorderStyle = BorderStyle.FixedSingle;
            pnlCard.Controls.Add(lblGhiChu);
            pnlCard.Controls.Add(lblTenDangNhap);
            pnlCard.Controls.Add(lblVaiTro);
            pnlCard.Controls.Add(lblIcon);
            pnlCard.Controls.Add(pnlCardTop);
            pnlCard.Location = new Point(220, 128);
            pnlCard.Name = "pnlCard";
            pnlCard.Size = new Size(560, 300);
            pnlCard.TabIndex = 0;
            // 
            // pnlCardTop
            // 
            pnlCardTop.BackColor = Color.FromArgb(22, 163, 74);
            pnlCardTop.Dock = DockStyle.Top;
            pnlCardTop.Location = new Point(0, 0);
            pnlCardTop.Name = "pnlCardTop";
            pnlCardTop.Size = new Size(558, 6);
            pnlCardTop.TabIndex = 0;
            // 
            // lblIcon
            // 
            lblIcon.Font = new Font("Segoe UI Emoji", 40F);
            lblIcon.Location = new Point(0, 28);
            lblIcon.Name = "lblIcon";
            lblIcon.Size = new Size(558, 85);
            lblIcon.TabIndex = 1;
            lblIcon.Text = "💪";
            lblIcon.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // lblVaiTro
            // 
            lblVaiTro.Font = new Font("Segoe UI", 22F, FontStyle.Bold);
            lblVaiTro.ForeColor = Color.FromArgb(22, 163, 74);
            lblVaiTro.Location = new Point(0, 120);
            lblVaiTro.Name = "lblVaiTro";
            lblVaiTro.Size = new Size(558, 50);
            lblVaiTro.TabIndex = 2;
            lblVaiTro.Text = "ĐÂY LÀ TÀI KHOẢN PT";
            lblVaiTro.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // lblTenDangNhap
            // 
            lblTenDangNhap.Font = new Font("Segoe UI", 11F);
            lblTenDangNhap.ForeColor = Color.FromArgb(15, 23, 42);
            lblTenDangNhap.Location = new Point(0, 180);
            lblTenDangNhap.Name = "lblTenDangNhap";
            lblTenDangNhap.Size = new Size(558, 26);
            lblTenDangNhap.TabIndex = 3;
            lblTenDangNhap.Text = "Tên đăng nhập:";
            lblTenDangNhap.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // lblGhiChu
            // 
            lblGhiChu.Font = new Font("Segoe UI", 9.5F, FontStyle.Italic);
            lblGhiChu.ForeColor = Color.FromArgb(100, 116, 139);
            lblGhiChu.Location = new Point(0, 232);
            lblGhiChu.Name = "lblGhiChu";
            lblGhiChu.Size = new Size(558, 24);
            lblGhiChu.TabIndex = 4;
            lblGhiChu.Text = "Các chức năng sẽ được bổ sung trong phiên bản tiếp theo.";
            lblGhiChu.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // frmPTMain
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(248, 250, 252);
            ClientSize = new Size(1000, 620);
            Controls.Add(pnlContent);
            Controls.Add(pnlHeader);
            MinimumSize = new Size(800, 520);
            Name = "frmPTMain";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Gym Manager - PT";
            Load += frmPTMain_Load;
            pnlHeader.ResumeLayout(false);
            pnlHeader.PerformLayout();
            pnlContent.ResumeLayout(false);
            pnlCard.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private Panel pnlHeader;
        private Label lblAppName;
        private Label lblXinChao;
        private Button btnDangXuat;
        private Panel pnlContent;
        private Panel pnlCard;
        private Panel pnlCardTop;
        private Label lblIcon;
        private Label lblVaiTro;
        private Label lblTenDangNhap;
        private Label lblGhiChu;
    }
}
