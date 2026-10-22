namespace QuanLyGym.UI.Forms.PT
{
    partial class ucHoiVien
    {
        private System.ComponentModel.IContainer components = null;

        private ucPTTopStats topStats;
        private System.Windows.Forms.Panel pnlContent;
        private System.Windows.Forms.Panel pnlFilterBar;
        private System.Windows.Forms.TextBox txtSearch;
        private System.Windows.Forms.Label lblStatusFilter;
        private ModernDarkComboBox cboStatusFilter;
        private System.Windows.Forms.FlowLayoutPanel pnlCardsContainer;

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
            this.topStats = new QuanLyGym.UI.Forms.PT.ucPTTopStats();
            this.pnlContent = new System.Windows.Forms.Panel();
            this.pnlCardsContainer = new System.Windows.Forms.FlowLayoutPanel();
            this.pnlFilterBar = new System.Windows.Forms.Panel();
            this.cboStatusFilter = new QuanLyGym.UI.Forms.PT.ModernDarkComboBox();
            this.lblStatusFilter = new System.Windows.Forms.Label();
            this.txtSearch = new System.Windows.Forms.TextBox();
            this.pnlContent.SuspendLayout();
            this.pnlFilterBar.SuspendLayout();
            this.SuspendLayout();
            // 
            // topStats
            // 
            this.topStats.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(15)))), ((int)(((byte)(17)))), ((int)(((byte)(21)))));
            this.topStats.Dock = System.Windows.Forms.DockStyle.Top;
            this.topStats.Location = new System.Drawing.Point(0, 0);
            this.topStats.Name = "topStats";
            this.topStats.Size = new System.Drawing.Size(1200, 148);
            this.topStats.TabIndex = 0;
            // 
            // pnlContent
            // 
            this.pnlContent.Controls.Add(this.pnlCardsContainer);
            this.pnlContent.Controls.Add(this.pnlFilterBar);
            this.pnlContent.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlContent.Location = new System.Drawing.Point(0, 148);
            this.pnlContent.Name = "pnlContent";
            this.pnlContent.Padding = new System.Windows.Forms.Padding(24, 0, 24, 20);
            this.pnlContent.Size = new System.Drawing.Size(1200, 552);
            this.pnlContent.TabIndex = 1;
            // 
            // pnlCardsContainer
            // 
            this.pnlCardsContainer.AutoScroll = true;
            this.pnlCardsContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlCardsContainer.Location = new System.Drawing.Point(24, 52);
            this.pnlCardsContainer.Name = "pnlCardsContainer";
            this.pnlCardsContainer.Size = new System.Drawing.Size(1152, 490);
            this.pnlCardsContainer.TabIndex = 1;
            // 
            // pnlFilterBar
            // 
            this.pnlFilterBar.Controls.Add(this.cboStatusFilter);
            this.pnlFilterBar.Controls.Add(this.lblStatusFilter);
            this.pnlFilterBar.Controls.Add(this.txtSearch);
            this.pnlFilterBar.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlFilterBar.Location = new System.Drawing.Point(24, 0);
            this.pnlFilterBar.Name = "pnlFilterBar";
            this.pnlFilterBar.Size = new System.Drawing.Size(1152, 52);
            this.pnlFilterBar.TabIndex = 0;
            // 
            // cboStatusFilter
            // 
            this.cboStatusFilter.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cboStatusFilter.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.cboStatusFilter.Items.AddRange(new string[] {
            "Tất cả hội viên",
            "Đang tập",
            "Sắp hết gói",
            "Tạm dừng"});
            this.cboStatusFilter.Location = new System.Drawing.Point(972, 6);
            this.cboStatusFilter.Name = "cboStatusFilter";
            this.cboStatusFilter.Size = new System.Drawing.Size(180, 36);
            this.cboStatusFilter.TabIndex = 2;
            this.cboStatusFilter.SelectedIndexChanged += new System.EventHandler(this.cboStatusFilter_SelectedIndexChanged);
            // 
            // lblStatusFilter
            // 
            this.lblStatusFilter.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblStatusFilter.AutoSize = true;
            this.lblStatusFilter.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.lblStatusFilter.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(142)))), ((int)(((byte)(149)))), ((int)(((byte)(165)))));
            this.lblStatusFilter.Location = new System.Drawing.Point(875, 15);
            this.lblStatusFilter.Name = "lblStatusFilter";
            this.lblStatusFilter.Size = new System.Drawing.Size(91, 17);
            this.lblStatusFilter.TabIndex = 1;
            this.lblStatusFilter.Text = "Lọc trạng thái:";
            this.lblStatusFilter.UseMnemonic = false;
            // 
            // txtSearch
            // 
            this.txtSearch.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(22)))), ((int)(((byte)(25)))), ((int)(((byte)(34)))));
            this.txtSearch.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtSearch.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.txtSearch.ForeColor = System.Drawing.Color.White;
            this.txtSearch.Location = new System.Drawing.Point(0, 11);
            this.txtSearch.Name = "txtSearch";
            this.txtSearch.PlaceholderText = "Tìm kiếm hội viên theo tên, SĐT...";
            this.txtSearch.Size = new System.Drawing.Size(320, 25);
            this.txtSearch.TabIndex = 0;
            this.txtSearch.TextChanged += new System.EventHandler(this.txtSearch_TextChanged);
            // 
            // ucHoiVien
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(15)))), ((int)(((byte)(17)))), ((int)(((byte)(21)))));
            this.Controls.Add(this.pnlContent);
            this.Controls.Add(this.topStats);
            this.Name = "ucHoiVien";
            this.Size = new System.Drawing.Size(1200, 700);
            this.pnlContent.ResumeLayout(false);
            this.pnlFilterBar.ResumeLayout(false);
            this.pnlFilterBar.PerformLayout();
            this.ResumeLayout(false);
        }
    }
}
