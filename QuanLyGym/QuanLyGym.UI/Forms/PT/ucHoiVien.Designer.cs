namespace QuanLyGym.UI.Forms.PT
{
    partial class ucHoiVien
    {
        private System.ComponentModel.IContainer components = null;

        private System.Windows.Forms.Panel pnlContent;
        private System.Windows.Forms.Panel pnlFilterBar;
        private System.Windows.Forms.TextBox txtSearch;
        private System.Windows.Forms.Button btnSearch;
        private System.Windows.Forms.Button btnViewCards;
        private System.Windows.Forms.Button btnViewGrid;
        private System.Windows.Forms.FlowLayoutPanel pnlCardsContainer;
        private System.Windows.Forms.DataGridView dgvMembers;

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
            System.Windows.Forms.DataGridViewCellStyle dgvCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dgvCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dgvCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            this.pnlContent = new System.Windows.Forms.Panel();
            this.dgvMembers = new System.Windows.Forms.DataGridView();
            this.pnlCardsContainer = new System.Windows.Forms.FlowLayoutPanel();
            this.pnlFilterBar = new System.Windows.Forms.Panel();
            this.btnViewGrid = new System.Windows.Forms.Button();
            this.btnViewCards = new System.Windows.Forms.Button();
            this.btnSearch = new System.Windows.Forms.Button();
            this.txtSearch = new System.Windows.Forms.TextBox();
            this.pnlContent.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvMembers)).BeginInit();
            this.pnlFilterBar.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlContent
            // 
            this.pnlContent.Controls.Add(this.dgvMembers);
            this.pnlContent.Controls.Add(this.pnlCardsContainer);
            this.pnlContent.Controls.Add(this.pnlFilterBar);
            this.pnlContent.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlContent.Location = new System.Drawing.Point(0, 0);
            this.pnlContent.Name = "pnlContent";
            this.pnlContent.Padding = new System.Windows.Forms.Padding(24, 16, 24, 20);
            this.pnlContent.Size = new System.Drawing.Size(1200, 700);
            this.pnlContent.TabIndex = 0;
            // 
            // dgvMembers
            // 
            this.dgvMembers.AllowUserToAddRows = false;
            this.dgvMembers.AllowUserToDeleteRows = false;
            this.dgvMembers.AllowUserToResizeRows = false;
            this.dgvMembers.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(24)))), ((int)(((byte)(33)))));
            this.dgvMembers.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgvMembers.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.SingleHorizontal;
            this.dgvMembers.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            dgvCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dgvCellStyle1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(28)))), ((int)(((byte)(34)))), ((int)(((byte)(46)))));
            dgvCellStyle1.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold);
            dgvCellStyle1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(204)))), ((int)(((byte)(255)))), ((int)(((byte)(0)))));
            dgvCellStyle1.Padding = new System.Windows.Forms.Padding(8, 0, 0, 0);
            dgvCellStyle1.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(28)))), ((int)(((byte)(34)))), ((int)(((byte)(46)))));
            dgvCellStyle1.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(204)))), ((int)(((byte)(255)))), ((int)(((byte)(0)))));
            dgvCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvMembers.ColumnHeadersDefaultCellStyle = dgvCellStyle1;
            this.dgvMembers.ColumnHeadersHeight = 44;
            this.dgvMembers.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            dgvCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dgvCellStyle2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(24)))), ((int)(((byte)(33)))));
            dgvCellStyle2.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            dgvCellStyle2.ForeColor = System.Drawing.Color.White;
            dgvCellStyle2.Padding = new System.Windows.Forms.Padding(8, 0, 0, 0);
            dgvCellStyle2.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(35)))), ((int)(((byte)(43)))), ((int)(((byte)(58)))));
            dgvCellStyle2.SelectionForeColor = System.Drawing.Color.White;
            dgvCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvMembers.DefaultCellStyle = dgvCellStyle2;
            this.dgvMembers.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvMembers.EnableHeadersVisualStyles = false;
            this.dgvMembers.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(38)))), ((int)(((byte)(46)))), ((int)(((byte)(62)))));
            this.dgvMembers.Location = new System.Drawing.Point(24, 72);
            this.dgvMembers.MultiSelect = false;
            this.dgvMembers.Name = "dgvMembers";
            this.dgvMembers.ReadOnly = true;
            this.dgvMembers.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            dgvCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dgvCellStyle3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(24)))), ((int)(((byte)(33)))));
            dgvCellStyle3.ForeColor = System.Drawing.Color.White;
            dgvCellStyle3.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(35)))), ((int)(((byte)(43)))), ((int)(((byte)(58)))));
            dgvCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvMembers.RowHeadersDefaultCellStyle = dgvCellStyle3;
            this.dgvMembers.RowHeadersVisible = false;
            this.dgvMembers.RowTemplate.Height = 42;
            this.dgvMembers.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvMembers.Size = new System.Drawing.Size(1152, 608);
            this.dgvMembers.TabIndex = 2;
            this.dgvMembers.Visible = false;
            // 
            // pnlCardsContainer
            // 
            this.pnlCardsContainer.AutoScroll = true;
            this.pnlCardsContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlCardsContainer.Location = new System.Drawing.Point(24, 72);
            this.pnlCardsContainer.Name = "pnlCardsContainer";
            this.pnlCardsContainer.Size = new System.Drawing.Size(1152, 608);
            this.pnlCardsContainer.TabIndex = 1;
            // 
            // pnlFilterBar
            // 
            this.pnlFilterBar.Controls.Add(this.btnViewGrid);
            this.pnlFilterBar.Controls.Add(this.btnViewCards);
            this.pnlFilterBar.Controls.Add(this.btnSearch);
            this.pnlFilterBar.Controls.Add(this.txtSearch);
            this.pnlFilterBar.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlFilterBar.Location = new System.Drawing.Point(24, 16);
            this.pnlFilterBar.Name = "pnlFilterBar";
            this.pnlFilterBar.Size = new System.Drawing.Size(1152, 56);
            this.pnlFilterBar.TabIndex = 0;
            // 
            // btnViewGrid
            // 
            this.btnViewGrid.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnViewGrid.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnViewGrid.FlatAppearance.BorderSize = 0;
            this.btnViewGrid.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnViewGrid.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.btnViewGrid.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(142)))), ((int)(((byte)(149)))), ((int)(((byte)(165)))));
            this.btnViewGrid.Location = new System.Drawing.Point(1008, 9);
            this.btnViewGrid.Name = "btnViewGrid";
            this.btnViewGrid.Size = new System.Drawing.Size(144, 36);
            this.btnViewGrid.TabIndex = 4;
            this.btnViewGrid.Text = "☰ Dạng Bảng (Excel)";
            this.btnViewGrid.UseMnemonic = false;
            this.btnViewGrid.UseVisualStyleBackColor = true;
            this.btnViewGrid.Click += new System.EventHandler(this.btnViewGrid_Click);
            // 
            // btnViewCards
            // 
            this.btnViewCards.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnViewCards.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnViewCards.FlatAppearance.BorderSize = 0;
            this.btnViewCards.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnViewCards.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold);
            this.btnViewCards.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(204)))), ((int)(((byte)(255)))), ((int)(((byte)(0)))));
            this.btnViewCards.Location = new System.Drawing.Point(892, 9);
            this.btnViewCards.Name = "btnViewCards";
            this.btnViewCards.Size = new System.Drawing.Size(110, 36);
            this.btnViewCards.TabIndex = 3;
            this.btnViewCards.Text = "⊞ Dạng Thẻ";
            this.btnViewCards.UseMnemonic = false;
            this.btnViewCards.UseVisualStyleBackColor = true;
            this.btnViewCards.Click += new System.EventHandler(this.btnViewCards_Click);
            // 
            // btnSearch
            // 
            this.btnSearch.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(28)))), ((int)(((byte)(34)))), ((int)(((byte)(46)))));
            this.btnSearch.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnSearch.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(55)))), ((int)(((byte)(75)))));
            this.btnSearch.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSearch.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold);
            this.btnSearch.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(204)))), ((int)(((byte)(255)))), ((int)(((byte)(0)))));
            this.btnSearch.Location = new System.Drawing.Point(344, 9);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(86, 36);
            this.btnSearch.TabIndex = 1;
            this.btnSearch.Text = "Tìm Kiếm";
            this.btnSearch.UseMnemonic = false;
            this.btnSearch.UseVisualStyleBackColor = false;
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // txtSearch
            // 
            this.txtSearch.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(24)))), ((int)(((byte)(33)))));
            this.txtSearch.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtSearch.Font = new System.Drawing.Font("Segoe UI", 10.5F);
            this.txtSearch.ForeColor = System.Drawing.Color.White;
            this.txtSearch.Location = new System.Drawing.Point(0, 14);
            this.txtSearch.Name = "txtSearch";
            this.txtSearch.PlaceholderText = "Nhập tên, SĐT hoặc mã hội viên...";
            this.txtSearch.Size = new System.Drawing.Size(336, 26);
            this.txtSearch.TabIndex = 0;
            this.txtSearch.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtSearch_KeyDown);
            // 
            // ucHoiVien
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(15)))), ((int)(((byte)(17)))), ((int)(((byte)(21)))));
            this.Controls.Add(this.pnlContent);
            this.Name = "ucHoiVien";
            this.Size = new System.Drawing.Size(1200, 700);
            this.pnlContent.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvMembers)).EndInit();
            this.pnlFilterBar.ResumeLayout(false);
            this.pnlFilterBar.PerformLayout();
            this.ResumeLayout(false);
        }
    }
}
