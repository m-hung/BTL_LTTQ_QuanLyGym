namespace QuanLyGym.UI.Forms.PT
{
    partial class frmDatLichModal
    {
        private System.ComponentModel.IContainer components = null;

        private System.Windows.Forms.Label lblHeaderTitle;
        private System.Windows.Forms.Label lblMember;
        private System.Windows.Forms.ComboBox cboMember;
        private System.Windows.Forms.Label lblDate;
        private System.Windows.Forms.DateTimePicker dtpDate;
        private System.Windows.Forms.Label lblTimeSlot;
        private System.Windows.Forms.ComboBox cboTimeSlot;
        private System.Windows.Forms.Label lblLocation;
        private System.Windows.Forms.ComboBox cboLocation;
        private System.Windows.Forms.Label lblPlan;
        private System.Windows.Forms.TextBox txtPlan;
        private System.Windows.Forms.Button btnConfirm;
        private System.Windows.Forms.Button btnCancel;

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
            this.lblHeaderTitle = new System.Windows.Forms.Label();
            this.lblMember = new System.Windows.Forms.Label();
            this.cboMember = new System.Windows.Forms.ComboBox();
            this.lblDate = new System.Windows.Forms.Label();
            this.dtpDate = new System.Windows.Forms.DateTimePicker();
            this.lblTimeSlot = new System.Windows.Forms.Label();
            this.cboTimeSlot = new System.Windows.Forms.ComboBox();
            this.lblLocation = new System.Windows.Forms.Label();
            this.cboLocation = new System.Windows.Forms.ComboBox();
            this.lblPlan = new System.Windows.Forms.Label();
            this.txtPlan = new System.Windows.Forms.TextBox();
            this.btnConfirm = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // lblHeaderTitle
            // 
            this.lblHeaderTitle.AutoSize = true;
            this.lblHeaderTitle.Font = new System.Drawing.Font("Segoe UI", 13F, System.Drawing.FontStyle.Bold);
            this.lblHeaderTitle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(204)))), ((int)(((byte)(255)))), ((int)(((byte)(0)))));
            this.lblHeaderTitle.Location = new System.Drawing.Point(24, 20);
            this.lblHeaderTitle.Name = "lblHeaderTitle";
            this.lblHeaderTitle.Size = new System.Drawing.Size(328, 25);
            this.lblHeaderTitle.TabIndex = 0;
            this.lblHeaderTitle.Text = "ĐẶT LỊCH BUỔI HUẤN LUYỆN 1-1 MỚI";
            // 
            // lblMember
            // 
            this.lblMember.AutoSize = true;
            this.lblMember.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.lblMember.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(142)))), ((int)(((byte)(149)))), ((int)(((byte)(165)))));
            this.lblMember.Location = new System.Drawing.Point(24, 65);
            this.lblMember.Name = "lblMember";
            this.lblMember.Size = new System.Drawing.Size(107, 15);
            this.lblMember.TabIndex = 1;
            this.lblMember.Text = "CHỌN HỘI VIÊN *";
            // 
            // cboMember
            // 
            this.cboMember.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(27)))), ((int)(((byte)(31)))), ((int)(((byte)(42)))));
            this.cboMember.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboMember.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cboMember.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.cboMember.ForeColor = System.Drawing.Color.White;
            this.cboMember.FormattingEnabled = true;
            this.cboMember.Items.AddRange(new object[] {
            "Nguyễn Đức An (Tăng Cơ Hypertrophy)",
            "Phạm Mai Linh (Siết Mỡ Transformation)",
            "Lê Hoàng Nam (Sức Bền & Thể Lực)",
            "Đỗ Bảo Trâm (Tập Nữ Cơ Bản 1–1)"});
            this.cboMember.Location = new System.Drawing.Point(24, 85);
            this.cboMember.Name = "cboMember";
            this.cboMember.Size = new System.Drawing.Size(432, 25);
            this.cboMember.TabIndex = 2;
            // 
            // lblDate
            // 
            this.lblDate.AutoSize = true;
            this.lblDate.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.lblDate.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(142)))), ((int)(((byte)(149)))), ((int)(((byte)(165)))));
            this.lblDate.Location = new System.Drawing.Point(24, 125);
            this.lblDate.Name = "lblDate";
            this.lblDate.Size = new System.Drawing.Size(74, 15);
            this.lblDate.TabIndex = 3;
            this.lblDate.Text = "NGÀY TẬP *";
            // 
            // dtpDate
            // 
            this.dtpDate.CalendarMonthBackground = System.Drawing.Color.FromArgb(((int)(((byte)(27)))), ((int)(((byte)(31)))), ((int)(((byte)(42)))));
            this.dtpDate.CalendarTitleBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(35)))), ((int)(((byte)(42)))), ((int)(((byte)(56)))));
            this.dtpDate.CalendarTitleForeColor = System.Drawing.Color.White;
            this.dtpDate.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.dtpDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpDate.Location = new System.Drawing.Point(24, 145);
            this.dtpDate.Name = "dtpDate";
            this.dtpDate.Size = new System.Drawing.Size(200, 24);
            this.dtpDate.TabIndex = 4;
            // 
            // lblTimeSlot
            // 
            this.lblTimeSlot.AutoSize = true;
            this.lblTimeSlot.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.lblTimeSlot.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(142)))), ((int)(((byte)(149)))), ((int)(((byte)(165)))));
            this.lblTimeSlot.Location = new System.Drawing.Point(240, 125);
            this.lblTimeSlot.Name = "lblTimeSlot";
            this.lblTimeSlot.Size = new System.Drawing.Size(89, 15);
            this.lblTimeSlot.TabIndex = 5;
            this.lblTimeSlot.Text = "KHUNG GIỜ *";
            // 
            // cboTimeSlot
            // 
            this.cboTimeSlot.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(27)))), ((int)(((byte)(31)))), ((int)(((byte)(42)))));
            this.cboTimeSlot.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboTimeSlot.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cboTimeSlot.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.cboTimeSlot.ForeColor = System.Drawing.Color.White;
            this.cboTimeSlot.FormattingEnabled = true;
            this.cboTimeSlot.Items.AddRange(new object[] {
            "07:00 – 08:30 (Sáng)",
            "09:00 – 10:30 (Sáng)",
            "14:00 – 15:30 (Chiều)",
            "17:00 – 18:30 (Chiều tối)",
            "19:00 – 20:30 (Tối)"});
            this.cboTimeSlot.Location = new System.Drawing.Point(240, 145);
            this.cboTimeSlot.Name = "cboTimeSlot";
            this.cboTimeSlot.Size = new System.Drawing.Size(216, 25);
            this.cboTimeSlot.TabIndex = 6;
            // 
            // lblLocation
            // 
            this.lblLocation.AutoSize = true;
            this.lblLocation.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.lblLocation.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(142)))), ((int)(((byte)(149)))), ((int)(((byte)(165)))));
            this.lblLocation.Location = new System.Drawing.Point(24, 185);
            this.lblLocation.Name = "lblLocation";
            this.lblLocation.Size = new System.Drawing.Size(135, 15);
            this.lblLocation.TabIndex = 7;
            this.lblLocation.Text = "KHU VỰC / ĐỊA ĐIỂM *";
            // 
            // cboLocation
            // 
            this.cboLocation.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(27)))), ((int)(((byte)(31)))), ((int)(((byte)(42)))));
            this.cboLocation.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboLocation.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cboLocation.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.cboLocation.ForeColor = System.Drawing.Color.White;
            this.cboLocation.FormattingEnabled = true;
            this.cboLocation.Items.AddRange(new object[] {
            "Zone A - Khu Tạ Tự Do (Free-weight)",
            "Zone B - Khu Cardio & Giảm Mỡ",
            "Studio 1 - Khu Functional & Giãn Cơ",
            "Tầng 2 - Máy Kháng Lực Tổng Hợp"});
            this.cboLocation.Location = new System.Drawing.Point(24, 205);
            this.cboLocation.Name = "cboLocation";
            this.cboLocation.Size = new System.Drawing.Size(432, 25);
            this.cboLocation.TabIndex = 8;
            // 
            // lblPlan
            // 
            this.lblPlan.AutoSize = true;
            this.lblPlan.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.lblPlan.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(142)))), ((int)(((byte)(149)))), ((int)(((byte)(165)))));
            this.lblPlan.Location = new System.Drawing.Point(24, 245);
            this.lblPlan.Name = "lblPlan";
            this.lblPlan.Size = new System.Drawing.Size(183, 15);
            this.lblPlan.TabIndex = 9;
            this.lblPlan.Text = "GIÁO ÁN / NỘI DUNG BUỔI TẬP";
            // 
            // txtPlan
            // 
            this.txtPlan.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(27)))), ((int)(((byte)(31)))), ((int)(((byte)(42)))));
            this.txtPlan.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtPlan.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.txtPlan.ForeColor = System.Drawing.Color.White;
            this.txtPlan.Location = new System.Drawing.Point(24, 265);
            this.txtPlan.Multiline = true;
            this.txtPlan.Name = "txtPlan";
            this.txtPlan.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtPlan.Size = new System.Drawing.Size(432, 90);
            this.txtPlan.TabIndex = 10;
            this.txtPlan.Text = "1. Khởi động khớp & làm nóng cơ bắp (10p)\r\n2. Bài tập chính theo mục tiêu tuần\r\n3" +
    ". Giãn cơ hạ nhiệt";
            // 
            // btnConfirm
            // 
            this.btnConfirm.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(204)))), ((int)(((byte)(255)))), ((int)(((byte)(0)))));
            this.btnConfirm.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnConfirm.FlatAppearance.BorderSize = 0;
            this.btnConfirm.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnConfirm.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnConfirm.ForeColor = System.Drawing.Color.Black;
            this.btnConfirm.Location = new System.Drawing.Point(24, 380);
            this.btnConfirm.Name = "btnConfirm";
            this.btnConfirm.Size = new System.Drawing.Size(280, 40);
            this.btnConfirm.TabIndex = 11;
            this.btnConfirm.Text = "✓ Xác Nhận Đặt Lịch";
            this.btnConfirm.UseVisualStyleBackColor = false;
            this.btnConfirm.Click += new System.EventHandler(this.btnConfirm_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(28)))), ((int)(((byte)(32)))), ((int)(((byte)(44)))));
            this.btnCancel.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnCancel.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(52)))), ((int)(((byte)(70)))));
            this.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCancel.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold);
            this.btnCancel.ForeColor = System.Drawing.Color.White;
            this.btnCancel.Location = new System.Drawing.Point(318, 380);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(138, 40);
            this.btnCancel.TabIndex = 12;
            this.btnCancel.Text = "Đóng";
            this.btnCancel.UseVisualStyleBackColor = false;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // frmDatLichModal
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(15)))), ((int)(((byte)(17)))), ((int)(((byte)(21)))));
            this.ClientSize = new System.Drawing.Size(480, 445);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnConfirm);
            this.Controls.Add(this.txtPlan);
            this.Controls.Add(this.lblPlan);
            this.Controls.Add(this.cboLocation);
            this.Controls.Add(this.lblLocation);
            this.Controls.Add(this.cboTimeSlot);
            this.Controls.Add(this.lblTimeSlot);
            this.Controls.Add(this.dtpDate);
            this.Controls.Add(this.lblDate);
            this.Controls.Add(this.cboMember);
            this.Controls.Add(this.lblMember);
            this.Controls.Add(this.lblHeaderTitle);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmDatLichModal";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Đặt Lịch Buổi Huấn Luyện 1-1";
            this.ResumeLayout(false);
            this.PerformLayout();
        }
    }
}
