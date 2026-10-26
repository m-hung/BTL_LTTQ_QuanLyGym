namespace QuanLyGym.UI.Forms.PT
{
    partial class frmPTMain
    {
        private System.ComponentModel.IContainer components = null;

        private System.Windows.Forms.Panel pnlSidebar;
        private System.Windows.Forms.Panel pnlBrand;
        private System.Windows.Forms.Label lblMenuCategory;
        private System.Windows.Forms.Button btnMenuSchedule;
        private System.Windows.Forms.Button btnMenuMembers;
        private System.Windows.Forms.Button btnMenuPhysical;
        private System.Windows.Forms.Panel pnlPTProfile;
        private System.Windows.Forms.Button btnDangXuat;
        private System.Windows.Forms.Panel pnlMainArea;
        private System.Windows.Forms.Panel pnlTopHeader;
        private System.Windows.Forms.Label lblHeaderTitle;
        private System.Windows.Forms.Button btnCreateSession;
        private System.Windows.Forms.Panel pnlMainContainer;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.pnlSidebar = new System.Windows.Forms.Panel();
            this.pnlPTProfile = new System.Windows.Forms.Panel();
            this.btnDangXuat = new System.Windows.Forms.Button();
            this.btnMenuPhysical = new System.Windows.Forms.Button();
            this.btnMenuMembers = new System.Windows.Forms.Button();
            this.btnMenuSchedule = new System.Windows.Forms.Button();
            this.lblMenuCategory = new System.Windows.Forms.Label();
            this.pnlBrand = new System.Windows.Forms.Panel();
            this.pnlMainArea = new System.Windows.Forms.Panel();
            this.pnlMainContainer = new System.Windows.Forms.Panel();
            this.pnlTopHeader = new System.Windows.Forms.Panel();
            this.btnCreateSession = new System.Windows.Forms.Button();
            this.lblHeaderTitle = new System.Windows.Forms.Label();
            this.pnlSidebar.SuspendLayout();
            this.pnlMainArea.SuspendLayout();
            this.pnlTopHeader.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlSidebar
            // 
            this.pnlSidebar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(11)))), ((int)(((byte)(13)))), ((int)(((byte)(16)))));
            this.pnlSidebar.Controls.Add(this.btnDangXuat);
            this.pnlSidebar.Controls.Add(this.pnlPTProfile);
            this.pnlSidebar.Controls.Add(this.btnMenuPhysical);
            this.pnlSidebar.Controls.Add(this.btnMenuMembers);
            this.pnlSidebar.Controls.Add(this.btnMenuSchedule);
            this.pnlSidebar.Controls.Add(this.lblMenuCategory);
            this.pnlSidebar.Controls.Add(this.pnlBrand);
            this.pnlSidebar.Dock = System.Windows.Forms.DockStyle.Left;
            this.pnlSidebar.Location = new System.Drawing.Point(0, 0);
            this.pnlSidebar.Name = "pnlSidebar";
            this.pnlSidebar.Size = new System.Drawing.Size(250, 821);
            this.pnlSidebar.TabIndex = 0;
            // 
            // pnlPTProfile
            // 
            this.pnlPTProfile.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlPTProfile.Location = new System.Drawing.Point(0, 746);
            this.pnlPTProfile.Name = "pnlPTProfile";
            this.pnlPTProfile.Size = new System.Drawing.Size(250, 75);
            this.pnlPTProfile.TabIndex = 5;
            // 
            // btnDangXuat
            // 
            this.btnDangXuat.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnDangXuat.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(11)))), ((int)(((byte)(13)))), ((int)(((byte)(16)))));
            this.btnDangXuat.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnDangXuat.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(69)))), ((int)(((byte)(39)))), ((int)(((byte)(42)))));
            this.btnDangXuat.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(24)))), ((int)(((byte)(27)))));
            this.btnDangXuat.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(42)))), ((int)(((byte)(20)))), ((int)(((byte)(23)))));
            this.btnDangXuat.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnDangXuat.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold);
            this.btnDangXuat.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(113)))), ((int)(((byte)(113)))));
            this.btnDangXuat.Location = new System.Drawing.Point(14, 692);
            this.btnDangXuat.Name = "btnDangXuat";
            this.btnDangXuat.Padding = new System.Windows.Forms.Padding(12, 0, 0, 0);
            this.btnDangXuat.Size = new System.Drawing.Size(222, 44);
            this.btnDangXuat.TabIndex = 6;
            this.btnDangXuat.Text = "⇥  Đăng Xuất";
            this.btnDangXuat.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnDangXuat.UseMnemonic = false;
            this.btnDangXuat.UseVisualStyleBackColor = false;
            this.btnDangXuat.Click += new System.EventHandler(this.btnDangXuat_Click);
            // 
            // btnMenuPhysical
            // 
            this.btnMenuPhysical.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnMenuPhysical.FlatAppearance.BorderSize = 0;
            this.btnMenuPhysical.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnMenuPhysical.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.btnMenuPhysical.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(142)))), ((int)(((byte)(149)))), ((int)(((byte)(165)))));
            this.btnMenuPhysical.Location = new System.Drawing.Point(14, 206);
            this.btnMenuPhysical.Name = "btnMenuPhysical";
            this.btnMenuPhysical.Padding = new System.Windows.Forms.Padding(12, 0, 0, 0);
            this.btnMenuPhysical.Size = new System.Drawing.Size(222, 44);
            this.btnMenuPhysical.TabIndex = 4;
            this.btnMenuPhysical.Text = "Theo Dõi Thể Chất & PR";
            this.btnMenuPhysical.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnMenuPhysical.UseMnemonic = false;
            this.btnMenuPhysical.UseVisualStyleBackColor = true;
            this.btnMenuPhysical.Click += new System.EventHandler(this.btnMenuPhysical_Click);
            // 
            // btnMenuMembers
            // 
            this.btnMenuMembers.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnMenuMembers.FlatAppearance.BorderSize = 0;
            this.btnMenuMembers.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnMenuMembers.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.btnMenuMembers.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(142)))), ((int)(((byte)(149)))), ((int)(((byte)(165)))));
            this.btnMenuMembers.Location = new System.Drawing.Point(14, 154);
            this.btnMenuMembers.Name = "btnMenuMembers";
            this.btnMenuMembers.Padding = new System.Windows.Forms.Padding(12, 0, 0, 0);
            this.btnMenuMembers.Size = new System.Drawing.Size(222, 44);
            this.btnMenuMembers.TabIndex = 3;
            this.btnMenuMembers.Text = "Hội Viên Phụ Trách";
            this.btnMenuMembers.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnMenuMembers.UseMnemonic = false;
            this.btnMenuMembers.UseVisualStyleBackColor = true;
            this.btnMenuMembers.Click += new System.EventHandler(this.btnMenuMembers_Click);
            // 
            // btnMenuSchedule
            // 
            this.btnMenuSchedule.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnMenuSchedule.FlatAppearance.BorderSize = 0;
            this.btnMenuSchedule.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnMenuSchedule.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold);
            this.btnMenuSchedule.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(204)))), ((int)(((byte)(255)))), ((int)(((byte)(0)))));
            this.btnMenuSchedule.Location = new System.Drawing.Point(14, 102);
            this.btnMenuSchedule.Name = "btnMenuSchedule";
            this.btnMenuSchedule.Padding = new System.Windows.Forms.Padding(12, 0, 0, 0);
            this.btnMenuSchedule.Size = new System.Drawing.Size(222, 44);
            this.btnMenuSchedule.TabIndex = 2;
            this.btnMenuSchedule.Text = "Lịch Làm Việc & Ca Tập";
            this.btnMenuSchedule.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnMenuSchedule.UseMnemonic = false;
            this.btnMenuSchedule.UseVisualStyleBackColor = true;
            this.btnMenuSchedule.Click += new System.EventHandler(this.btnMenuSchedule_Click);
            // 
            // lblMenuCategory
            // 
            this.lblMenuCategory.AutoSize = true;
            this.lblMenuCategory.Font = new System.Drawing.Font("Segoe UI", 8F, System.Drawing.FontStyle.Bold);
            this.lblMenuCategory.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(90)))), ((int)(((byte)(96)))), ((int)(((byte)(110)))));
            this.lblMenuCategory.Location = new System.Drawing.Point(24, 82);
            this.lblMenuCategory.Name = "lblMenuCategory";
            this.lblMenuCategory.Size = new System.Drawing.Size(74, 13);
            this.lblMenuCategory.TabIndex = 1;
            this.lblMenuCategory.Text = "HUẤN LUYỆN";
            this.lblMenuCategory.UseMnemonic = false;
            // 
            // pnlBrand
            // 
            this.pnlBrand.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlBrand.Location = new System.Drawing.Point(0, 0);
            this.pnlBrand.Name = "pnlBrand";
            this.pnlBrand.Size = new System.Drawing.Size(250, 75);
            this.pnlBrand.TabIndex = 0;
            // 
            // pnlMainArea
            // 
            this.pnlMainArea.Controls.Add(this.pnlMainContainer);
            this.pnlMainArea.Controls.Add(this.pnlTopHeader);
            this.pnlMainArea.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlMainArea.Location = new System.Drawing.Point(250, 0);
            this.pnlMainArea.Name = "pnlMainArea";
            this.pnlMainArea.Size = new System.Drawing.Size(1134, 821);
            this.pnlMainArea.TabIndex = 1;
            // 
            // pnlMainContainer
            // 
            this.pnlMainContainer.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(15)))), ((int)(((byte)(17)))), ((int)(((byte)(21)))));
            this.pnlMainContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlMainContainer.Location = new System.Drawing.Point(0, 65);
            this.pnlMainContainer.Name = "pnlMainContainer";
            this.pnlMainContainer.Size = new System.Drawing.Size(1134, 756);
            this.pnlMainContainer.TabIndex = 1;
            // 
            // pnlTopHeader
            // 
            this.pnlTopHeader.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(15)))), ((int)(((byte)(17)))), ((int)(((byte)(21)))));
            this.pnlTopHeader.Controls.Add(this.btnCreateSession);
            this.pnlTopHeader.Controls.Add(this.lblHeaderTitle);
            this.pnlTopHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlTopHeader.Location = new System.Drawing.Point(0, 0);
            this.pnlTopHeader.Name = "pnlTopHeader";
            this.pnlTopHeader.Size = new System.Drawing.Size(1134, 65);
            this.pnlTopHeader.TabIndex = 0;
            // 
            // btnCreateSession
            // 
            this.btnCreateSession.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCreateSession.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(204)))), ((int)(((byte)(255)))), ((int)(((byte)(0)))));
            this.btnCreateSession.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnCreateSession.FlatAppearance.BorderSize = 0;
            this.btnCreateSession.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCreateSession.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold);
            this.btnCreateSession.ForeColor = System.Drawing.Color.Black;
            this.btnCreateSession.Location = new System.Drawing.Point(962, 14);
            this.btnCreateSession.Name = "btnCreateSession";
            this.btnCreateSession.Size = new System.Drawing.Size(148, 38);
            this.btnCreateSession.TabIndex = 1;
            this.btnCreateSession.Text = "+ Đặt Lịch Tập Mới";
            this.btnCreateSession.UseMnemonic = false;
            this.btnCreateSession.UseVisualStyleBackColor = false;
            this.btnCreateSession.Click += new System.EventHandler(this.btnCreateSession_Click);
            // 
            // lblHeaderTitle
            // 
            this.lblHeaderTitle.AutoSize = true;
            this.lblHeaderTitle.Font = new System.Drawing.Font("Segoe UI", 13.5F, System.Drawing.FontStyle.Bold);
            this.lblHeaderTitle.ForeColor = System.Drawing.Color.White;
            this.lblHeaderTitle.Location = new System.Drawing.Point(24, 20);
            this.lblHeaderTitle.Name = "lblHeaderTitle";
            this.lblHeaderTitle.Size = new System.Drawing.Size(278, 25);
            this.lblHeaderTitle.TabIndex = 0;
            this.lblHeaderTitle.Text = "LỊCH LÀM VIỆC & BUỔI TẬP 1-1";
            this.lblHeaderTitle.UseMnemonic = false;
            // 
            // frmPTMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(15)))), ((int)(((byte)(17)))), ((int)(((byte)(21)))));
            this.ClientSize = new System.Drawing.Size(1384, 821);
            this.Controls.Add(this.pnlMainArea);
            this.Controls.Add(this.pnlSidebar);
            this.MinimumSize = new System.Drawing.Size(1280, 800);
            this.Name = "frmPTMain";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Titan Gym - Trainer Suite (Hệ Thống Huấn Luyện Viên PT)";
            this.pnlSidebar.ResumeLayout(false);
            this.pnlSidebar.PerformLayout();
            this.pnlMainArea.ResumeLayout(false);
            this.pnlTopHeader.ResumeLayout(false);
            this.pnlTopHeader.PerformLayout();
            this.ResumeLayout(false);
        }
    }
}
