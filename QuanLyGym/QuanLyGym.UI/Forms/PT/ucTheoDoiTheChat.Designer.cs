namespace QuanLyGym.UI.Forms.PT
{
    partial class ucTheoDoiTheChat
    {
        private System.ComponentModel.IContainer components = null;

        private System.Windows.Forms.Panel pnlContent;
        private System.Windows.Forms.Panel pnlSearchBar;
        private System.Windows.Forms.TextBox txtSearchMember;
        private System.Windows.Forms.Button btnSearchMember;
        private System.Windows.Forms.Panel pnlMemberBanner;
        private System.Windows.Forms.Panel pnlChart;
        private System.Windows.Forms.Panel pnlBottomRow;
        private System.Windows.Forms.Panel pnlMeasurementsCard;
        private System.Windows.Forms.Panel pnlRecordsCard;
        private System.Windows.Forms.Button btnUpdatePR;
        private System.Windows.Forms.Label lblWeight;
        private System.Windows.Forms.TextBox txtWeight;
        private System.Windows.Forms.Label lblHeight;
        private System.Windows.Forms.TextBox txtHeight;
        private System.Windows.Forms.Label lblBodyFat;
        private System.Windows.Forms.TextBox txtBodyFat;
        private System.Windows.Forms.Label lblWaist;
        private System.Windows.Forms.TextBox txtWaist;
        private System.Windows.Forms.Button btnSaveMeasurement;

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
            this.pnlContent = new System.Windows.Forms.Panel();
            this.pnlBottomRow = new System.Windows.Forms.Panel();
            this.pnlRecordsCard = new System.Windows.Forms.Panel();
            this.btnUpdatePR = new System.Windows.Forms.Button();
            this.pnlMeasurementsCard = new System.Windows.Forms.Panel();
            this.btnSaveMeasurement = new System.Windows.Forms.Button();
            this.txtWaist = new System.Windows.Forms.TextBox();
            this.lblWaist = new System.Windows.Forms.Label();
            this.txtBodyFat = new System.Windows.Forms.TextBox();
            this.lblBodyFat = new System.Windows.Forms.Label();
            this.txtHeight = new System.Windows.Forms.TextBox();
            this.lblHeight = new System.Windows.Forms.Label();
            this.txtWeight = new System.Windows.Forms.TextBox();
            this.lblWeight = new System.Windows.Forms.Label();
            this.pnlChart = new System.Windows.Forms.Panel();
            this.pnlMemberBanner = new System.Windows.Forms.Panel();
            this.pnlSearchBar = new System.Windows.Forms.Panel();
            this.btnSearchMember = new System.Windows.Forms.Button();
            this.txtSearchMember = new System.Windows.Forms.TextBox();
            this.pnlContent.SuspendLayout();
            this.pnlBottomRow.SuspendLayout();
            this.pnlRecordsCard.SuspendLayout();
            this.pnlMeasurementsCard.SuspendLayout();
            this.pnlSearchBar.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlContent
            // 
            this.pnlContent.Controls.Add(this.pnlBottomRow);
            this.pnlContent.Controls.Add(this.pnlChart);
            this.pnlContent.Controls.Add(this.pnlMemberBanner);
            this.pnlContent.Controls.Add(this.pnlSearchBar);
            this.pnlContent.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlContent.Location = new System.Drawing.Point(0, 0);
            this.pnlContent.Name = "pnlContent";
            this.pnlContent.Padding = new System.Windows.Forms.Padding(24, 14, 24, 20);
            this.pnlContent.Size = new System.Drawing.Size(1200, 700);
            this.pnlContent.TabIndex = 0;
            // 
            // pnlSearchBar
            // 
            this.pnlSearchBar.Controls.Add(this.btnSearchMember);
            this.pnlSearchBar.Controls.Add(this.txtSearchMember);
            this.pnlSearchBar.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlSearchBar.Location = new System.Drawing.Point(24, 14);
            this.pnlSearchBar.Name = "pnlSearchBar";
            this.pnlSearchBar.Size = new System.Drawing.Size(1152, 50);
            this.pnlSearchBar.TabIndex = 0;
            // 
            // txtSearchMember
            // 
            this.txtSearchMember.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(24)))), ((int)(((byte)(33)))));
            this.txtSearchMember.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtSearchMember.Font = new System.Drawing.Font("Segoe UI", 10.5F);
            this.txtSearchMember.ForeColor = System.Drawing.Color.White;
            this.txtSearchMember.Location = new System.Drawing.Point(0, 10);
            this.txtSearchMember.Name = "txtSearchMember";
            this.txtSearchMember.PlaceholderText = "Nhập tên, SĐT hoặc Mã hội viên để tra cứu InBody & mức tạ...";
            this.txtSearchMember.Size = new System.Drawing.Size(420, 26);
            this.txtSearchMember.TabIndex = 0;
            this.txtSearchMember.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtSearchMember_KeyDown);
            // 
            // btnSearchMember
            // 
            this.btnSearchMember.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(28)))), ((int)(((byte)(34)))), ((int)(((byte)(46)))));
            this.btnSearchMember.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnSearchMember.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(55)))), ((int)(((byte)(75)))));
            this.btnSearchMember.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSearchMember.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold);
            this.btnSearchMember.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(204)))), ((int)(((byte)(255)))), ((int)(((byte)(0)))));
            this.btnSearchMember.Location = new System.Drawing.Point(430, 7);
            this.btnSearchMember.Name = "btnSearchMember";
            this.btnSearchMember.Size = new System.Drawing.Size(95, 34);
            this.btnSearchMember.TabIndex = 1;
            this.btnSearchMember.Text = "Tra Cứu";
            this.btnSearchMember.UseMnemonic = false;
            this.btnSearchMember.UseVisualStyleBackColor = false;
            this.btnSearchMember.Click += new System.EventHandler(this.btnSearchMember_Click);
            // 
            // pnlMemberBanner
            // 
            this.pnlMemberBanner.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(15)))), ((int)(((byte)(17)))), ((int)(((byte)(21)))));
            this.pnlMemberBanner.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlMemberBanner.Location = new System.Drawing.Point(24, 64);
            this.pnlMemberBanner.Name = "pnlMemberBanner";
            this.pnlMemberBanner.Size = new System.Drawing.Size(1152, 64);
            this.pnlMemberBanner.TabIndex = 1;
            // 
            // pnlChart
            // 
            this.pnlChart.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(15)))), ((int)(((byte)(17)))), ((int)(((byte)(21)))));
            this.pnlChart.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlChart.Location = new System.Drawing.Point(24, 128);
            this.pnlChart.Name = "pnlChart";
            this.pnlChart.Size = new System.Drawing.Size(1152, 230);
            this.pnlChart.TabIndex = 2;
            // 
            // pnlBottomRow
            // 
            this.pnlBottomRow.Controls.Add(this.pnlRecordsCard);
            this.pnlBottomRow.Controls.Add(this.pnlMeasurementsCard);
            this.pnlBottomRow.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlBottomRow.Location = new System.Drawing.Point(24, 358);
            this.pnlBottomRow.Name = "pnlBottomRow";
            this.pnlBottomRow.Padding = new System.Windows.Forms.Padding(0, 10, 0, 0);
            this.pnlBottomRow.Size = new System.Drawing.Size(1152, 322);
            this.pnlBottomRow.TabIndex = 3;
            // 
            // pnlRecordsCard
            // 
            this.pnlRecordsCard.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(15)))), ((int)(((byte)(17)))), ((int)(((byte)(21)))));
            this.pnlRecordsCard.Controls.Add(this.btnUpdatePR);
            this.pnlRecordsCard.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlRecordsCard.Location = new System.Drawing.Point(576, 10);
            this.pnlRecordsCard.Name = "pnlRecordsCard";
            this.pnlRecordsCard.Size = new System.Drawing.Size(576, 312);
            this.pnlRecordsCard.TabIndex = 1;
            // 
            // btnUpdatePR
            // 
            this.btnUpdatePR.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnUpdatePR.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(25)))), ((int)(((byte)(30)))), ((int)(((byte)(42)))));
            this.btnUpdatePR.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnUpdatePR.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(52)))), ((int)(((byte)(70)))));
            this.btnUpdatePR.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnUpdatePR.Font = new System.Drawing.Font("Segoe UI", 8.5F, System.Drawing.FontStyle.Bold);
            this.btnUpdatePR.ForeColor = System.Drawing.Color.White;
            this.btnUpdatePR.Location = new System.Drawing.Point(448, 14);
            this.btnUpdatePR.Name = "btnUpdatePR";
            this.btnUpdatePR.Size = new System.Drawing.Size(110, 28);
            this.btnUpdatePR.TabIndex = 0;
            this.btnUpdatePR.Text = "+ Cập Nhật PR";
            this.btnUpdatePR.UseMnemonic = false;
            this.btnUpdatePR.UseVisualStyleBackColor = false;
            this.btnUpdatePR.Click += new System.EventHandler(this.btnUpdatePR_Click);
            // 
            // pnlMeasurementsCard
            // 
            this.pnlMeasurementsCard.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(15)))), ((int)(((byte)(17)))), ((int)(((byte)(21)))));
            this.pnlMeasurementsCard.Controls.Add(this.btnSaveMeasurement);
            this.pnlMeasurementsCard.Controls.Add(this.txtWaist);
            this.pnlMeasurementsCard.Controls.Add(this.lblWaist);
            this.pnlMeasurementsCard.Controls.Add(this.txtBodyFat);
            this.pnlMeasurementsCard.Controls.Add(this.lblBodyFat);
            this.pnlMeasurementsCard.Controls.Add(this.txtHeight);
            this.pnlMeasurementsCard.Controls.Add(this.lblHeight);
            this.pnlMeasurementsCard.Controls.Add(this.txtWeight);
            this.pnlMeasurementsCard.Controls.Add(this.lblWeight);
            this.pnlMeasurementsCard.Dock = System.Windows.Forms.DockStyle.Left;
            this.pnlMeasurementsCard.Location = new System.Drawing.Point(0, 10);
            this.pnlMeasurementsCard.Name = "pnlMeasurementsCard";
            this.pnlMeasurementsCard.Size = new System.Drawing.Size(576, 312);
            this.pnlMeasurementsCard.TabIndex = 0;
            // 
            // btnSaveMeasurement
            // 
            this.btnSaveMeasurement.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(204)))), ((int)(((byte)(255)))), ((int)(((byte)(0)))));
            this.btnSaveMeasurement.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnSaveMeasurement.FlatAppearance.BorderSize = 0;
            this.btnSaveMeasurement.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSaveMeasurement.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold);
            this.btnSaveMeasurement.ForeColor = System.Drawing.Color.Black;
            this.btnSaveMeasurement.Location = new System.Drawing.Point(20, 178);
            this.btnSaveMeasurement.Name = "btnSaveMeasurement";
            this.btnSaveMeasurement.Size = new System.Drawing.Size(536, 40);
            this.btnSaveMeasurement.TabIndex = 8;
            this.btnSaveMeasurement.Text = "Lưu Chỉ Số Mới";
            this.btnSaveMeasurement.UseMnemonic = false;
            this.btnSaveMeasurement.UseVisualStyleBackColor = false;
            this.btnSaveMeasurement.Click += new System.EventHandler(this.btnSaveMeasurement_Click);
            // 
            // txtWaist
            // 
            this.txtWaist.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(27)))), ((int)(((byte)(31)))), ((int)(((byte)(42)))));
            this.txtWaist.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtWaist.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.txtWaist.ForeColor = System.Drawing.Color.White;
            this.txtWaist.Location = new System.Drawing.Point(298, 134);
            this.txtWaist.Name = "txtWaist";
            this.txtWaist.Size = new System.Drawing.Size(258, 25);
            this.txtWaist.TabIndex = 7;
            this.txtWaist.Text = "79";
            // 
            // lblWaist
            // 
            this.lblWaist.AutoSize = true;
            this.lblWaist.Font = new System.Drawing.Font("Segoe UI", 8F, System.Drawing.FontStyle.Bold);
            this.lblWaist.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(142)))), ((int)(((byte)(149)))), ((int)(((byte)(165)))));
            this.lblWaist.Location = new System.Drawing.Point(298, 116);
            this.lblWaist.Name = "lblWaist";
            this.lblWaist.Size = new System.Drawing.Size(89, 13);
            this.lblWaist.TabIndex = 6;
            this.lblWaist.Text = "VÒNG EO (CM)";
            this.lblWaist.UseMnemonic = false;
            // 
            // txtBodyFat
            // 
            this.txtBodyFat.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(27)))), ((int)(((byte)(31)))), ((int)(((byte)(42)))));
            this.txtBodyFat.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtBodyFat.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.txtBodyFat.ForeColor = System.Drawing.Color.White;
            this.txtBodyFat.Location = new System.Drawing.Point(20, 134);
            this.txtBodyFat.Name = "txtBodyFat";
            this.txtBodyFat.Size = new System.Drawing.Size(258, 25);
            this.txtBodyFat.TabIndex = 5;
            this.txtBodyFat.Text = "16.5";
            // 
            // lblBodyFat
            // 
            this.lblBodyFat.AutoSize = true;
            this.lblBodyFat.Font = new System.Drawing.Font("Segoe UI", 8F, System.Drawing.FontStyle.Bold);
            this.lblBodyFat.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(142)))), ((int)(((byte)(149)))), ((int)(((byte)(165)))));
            this.lblBodyFat.Location = new System.Drawing.Point(20, 116);
            this.lblBodyFat.Name = "lblBodyFat";
            this.lblBodyFat.Size = new System.Drawing.Size(124, 13);
            this.lblBodyFat.TabIndex = 4;
            this.lblBodyFat.Text = "TỶ LỆ MỠ BODY FAT (%)";
            this.lblBodyFat.UseMnemonic = false;
            // 
            // txtHeight
            // 
            this.txtHeight.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(27)))), ((int)(((byte)(31)))), ((int)(((byte)(42)))));
            this.txtHeight.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtHeight.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.txtHeight.ForeColor = System.Drawing.Color.White;
            this.txtHeight.Location = new System.Drawing.Point(298, 74);
            this.txtHeight.Name = "txtHeight";
            this.txtHeight.Size = new System.Drawing.Size(258, 25);
            this.txtHeight.TabIndex = 3;
            this.txtHeight.Text = "175";
            // 
            // lblHeight
            // 
            this.lblHeight.AutoSize = true;
            this.lblHeight.Font = new System.Drawing.Font("Segoe UI", 8F, System.Drawing.FontStyle.Bold);
            this.lblHeight.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(142)))), ((int)(((byte)(149)))), ((int)(((byte)(165)))));
            this.lblHeight.Location = new System.Drawing.Point(298, 56);
            this.lblHeight.Name = "lblHeight";
            this.lblHeight.Size = new System.Drawing.Size(95, 13);
            this.lblHeight.TabIndex = 2;
            this.lblHeight.Text = "CHIỀU CAO (CM)";
            this.lblHeight.UseMnemonic = false;
            // 
            // txtWeight
            // 
            this.txtWeight.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(27)))), ((int)(((byte)(31)))), ((int)(((byte)(42)))));
            this.txtWeight.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtWeight.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.txtWeight.ForeColor = System.Drawing.Color.White;
            this.txtWeight.Location = new System.Drawing.Point(20, 74);
            this.txtWeight.Name = "txtWeight";
            this.txtWeight.Size = new System.Drawing.Size(258, 25);
            this.txtWeight.TabIndex = 1;
            this.txtWeight.Text = "72";
            // 
            // lblWeight
            // 
            this.lblWeight.AutoSize = true;
            this.lblWeight.Font = new System.Drawing.Font("Segoe UI", 8F, System.Drawing.FontStyle.Bold);
            this.lblWeight.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(142)))), ((int)(((byte)(149)))), ((int)(((byte)(165)))));
            this.lblWeight.Location = new System.Drawing.Point(20, 56);
            this.lblWeight.Name = "lblWeight";
            this.lblWeight.Size = new System.Drawing.Size(94, 13);
            this.lblWeight.TabIndex = 0;
            this.lblWeight.Text = "CÂN NẶNG (KG)";
            this.lblWeight.UseMnemonic = false;
            // 
            // ucTheoDoiTheChat
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(15)))), ((int)(((byte)(17)))), ((int)(((byte)(21)))));
            this.Controls.Add(this.pnlContent);
            this.Name = "ucTheoDoiTheChat";
            this.Size = new System.Drawing.Size(1200, 700);
            this.pnlContent.ResumeLayout(false);
            this.pnlBottomRow.ResumeLayout(false);
            this.pnlRecordsCard.ResumeLayout(false);
            this.pnlMeasurementsCard.ResumeLayout(false);
            this.pnlMeasurementsCard.PerformLayout();
            this.pnlSearchBar.ResumeLayout(false);
            this.pnlSearchBar.PerformLayout();
            this.ResumeLayout(false);
        }
    }
}
