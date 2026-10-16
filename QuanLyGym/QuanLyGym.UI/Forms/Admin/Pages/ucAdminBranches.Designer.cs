namespace QuanLyGym.UI.Forms.Admin.Pages
{
    partial class ucAdminBranches
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

        #region Component Designer generated code

        private void InitializeComponent()
        {
            this.mainLayout = new System.Windows.Forms.TableLayoutPanel();
            this.pnlHeader = new System.Windows.Forms.Panel();
            this.lblTitle = new System.Windows.Forms.Label();
            this.btnAddBranch = new QuanLyGym.UI.Forms.Admin.Controls.ModernButton();
            this.tblKpi = new System.Windows.Forms.TableLayoutPanel();
            this.kpiTotal = new QuanLyGym.UI.Forms.Admin.Controls.KpiCardControl();
            this.kpiActive = new QuanLyGym.UI.Forms.Admin.Controls.KpiCardControl();
            this.kpiMaintenance = new QuanLyGym.UI.Forms.Admin.Controls.KpiCardControl();
            this.kpiEmployees = new QuanLyGym.UI.Forms.Admin.Controls.KpiCardControl();
            this.tableBranches = new QuanLyGym.UI.Forms.Admin.Controls.AdminDataTableControl();
            this.mainLayout.SuspendLayout();
            this.pnlHeader.SuspendLayout();
            this.tblKpi.SuspendLayout();
            this.SuspendLayout();
            // 
            // mainLayout
            // 
            this.mainLayout.ColumnCount = 1;
            this.mainLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.mainLayout.Controls.Add(this.pnlHeader, 0, 0);
            this.mainLayout.Controls.Add(this.tblKpi, 0, 1);
            this.mainLayout.Controls.Add(this.tableBranches, 0, 2);
            this.mainLayout.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mainLayout.Location = new System.Drawing.Point(0, 0);
            this.mainLayout.Name = "mainLayout";
            this.mainLayout.Padding = new System.Windows.Forms.Padding(20, 14, 20, 14);
            this.mainLayout.RowCount = 3;
            this.mainLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 42F));
            this.mainLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 100F));
            this.mainLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.mainLayout.Size = new System.Drawing.Size(1264, 700);
            this.mainLayout.TabIndex = 0;
            // 
            // pnlHeader
            // 
            this.pnlHeader.Controls.Add(this.btnAddBranch);
            this.pnlHeader.Controls.Add(this.lblTitle);
            this.pnlHeader.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlHeader.Location = new System.Drawing.Point(20, 14);
            this.pnlHeader.Margin = new System.Windows.Forms.Padding(0);
            this.pnlHeader.Name = "pnlHeader";
            this.pnlHeader.Size = new System.Drawing.Size(1224, 42);
            this.pnlHeader.TabIndex = 0;
            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.Dock = System.Windows.Forms.DockStyle.Left;
            this.lblTitle.Font = new System.Drawing.Font("Segoe UI", 16F, System.Drawing.FontStyle.Bold);
            this.lblTitle.ForeColor = System.Drawing.Color.White;
            this.lblTitle.Location = new System.Drawing.Point(0, 0);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(236, 30);
            this.lblTitle.TabIndex = 0;
            this.lblTitle.Text = "QUẢN LÝ CHI NHÁNH";
            // 
            // btnAddBranch
            // 
            this.btnAddBranch.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnAddBranch.BackColor = System.Drawing.Color.Transparent;
            this.btnAddBranch.BorderRadius = 5;
            this.btnAddBranch.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnAddBranch.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.btnAddBranch.ForeColor = System.Drawing.Color.White;
            this.btnAddBranch.HoverColor = System.Drawing.Color.FromArgb(255, 133, 38);
            this.btnAddBranch.Location = new System.Drawing.Point(1074, 4);
            this.btnAddBranch.Name = "btnAddBranch";
            this.btnAddBranch.NormalColor = System.Drawing.Color.FromArgb(255, 107, 0);
            this.btnAddBranch.PressedColor = System.Drawing.Color.FromArgb(224, 94, 0);
            this.btnAddBranch.Size = new System.Drawing.Size(150, 32);
            this.btnAddBranch.TabIndex = 1;
            this.btnAddBranch.Text = "+ Thêm chi nhánh";
            this.btnAddBranch.UseVisualStyleBackColor = false;
            // 
            // tblKpi
            // 
            this.tblKpi.ColumnCount = 4;
            this.tblKpi.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tblKpi.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tblKpi.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tblKpi.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tblKpi.Controls.Add(this.kpiTotal, 0, 0);
            this.tblKpi.Controls.Add(this.kpiActive, 1, 0);
            this.tblKpi.Controls.Add(this.kpiMaintenance, 2, 0);
            this.tblKpi.Controls.Add(this.kpiEmployees, 3, 0);
            this.tblKpi.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tblKpi.Location = new System.Drawing.Point(20, 56);
            this.tblKpi.Margin = new System.Windows.Forms.Padding(0, 0, 0, 10);
            this.tblKpi.Name = "tblKpi";
            this.tblKpi.RowCount = 1;
            this.tblKpi.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tblKpi.Size = new System.Drawing.Size(1224, 90);
            this.tblKpi.TabIndex = 1;
            // 
            // kpiTotal
            // 
            this.kpiTotal.CardTitle = "TỔNG CHI NHÁNH";
            this.kpiTotal.Dock = System.Windows.Forms.DockStyle.Fill;
            this.kpiTotal.KpiSubtitle = "";
            this.kpiTotal.KpiValue = "5";
            this.kpiTotal.Location = new System.Drawing.Point(0, 0);
            this.kpiTotal.Margin = new System.Windows.Forms.Padding(0, 0, 8, 0);
            this.kpiTotal.Name = "kpiTotal";
            this.kpiTotal.ValueColor = System.Drawing.Color.FromArgb(255, 107, 0);
            // 
            // kpiActive
            // 
            this.kpiActive.CardTitle = "ĐANG HOẠT ĐỘNG";
            this.kpiActive.Dock = System.Windows.Forms.DockStyle.Fill;
            this.kpiActive.KpiSubtitle = "";
            this.kpiActive.KpiValue = "4";
            this.kpiActive.Location = new System.Drawing.Point(306, 0);
            this.kpiActive.Margin = new System.Windows.Forms.Padding(4, 0, 8, 0);
            this.kpiActive.Name = "kpiActive";
            this.kpiActive.ValueColor = System.Drawing.Color.FromArgb(16, 185, 129);
            // 
            // kpiMaintenance
            // 
            this.kpiMaintenance.CardTitle = "BẢO TRÌ";
            this.kpiMaintenance.Dock = System.Windows.Forms.DockStyle.Fill;
            this.kpiMaintenance.KpiSubtitle = "";
            this.kpiMaintenance.KpiValue = "1";
            this.kpiMaintenance.Location = new System.Drawing.Point(612, 0);
            this.kpiMaintenance.Margin = new System.Windows.Forms.Padding(4, 0, 8, 0);
            this.kpiMaintenance.Name = "kpiMaintenance";
            this.kpiMaintenance.ValueColor = System.Drawing.Color.FromArgb(168, 85, 247);
            // 
            // kpiEmployees
            // 
            this.kpiEmployees.CardTitle = "TỔNG NHÂN VIÊN";
            this.kpiEmployees.Dock = System.Windows.Forms.DockStyle.Fill;
            this.kpiEmployees.KpiSubtitle = "";
            this.kpiEmployees.KpiValue = "15";
            this.kpiEmployees.Location = new System.Drawing.Point(918, 0);
            this.kpiEmployees.Margin = new System.Windows.Forms.Padding(4, 0, 0, 0);
            this.kpiEmployees.Name = "kpiEmployees";
            this.kpiEmployees.ValueColor = System.Drawing.Color.FromArgb(47, 128, 237);
            // 
            // tableBranches
            // 
            this.tableBranches.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableBranches.Location = new System.Drawing.Point(20, 156);
            this.tableBranches.Margin = new System.Windows.Forms.Padding(0);
            this.tableBranches.Name = "tableBranches";
            this.tableBranches.Size = new System.Drawing.Size(1224, 530);
            this.tableBranches.TabIndex = 2;
            // 
            // ucAdminBranches
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(14, 15, 18);
            this.Controls.Add(this.mainLayout);
            this.Name = "ucAdminBranches";
            this.Size = new System.Drawing.Size(1264, 700);
            this.mainLayout.ResumeLayout(false);
            this.pnlHeader.ResumeLayout(false);
            this.pnlHeader.PerformLayout();
            this.tblKpi.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel mainLayout;
        private System.Windows.Forms.Panel pnlHeader;
        private System.Windows.Forms.Label lblTitle;
        private Controls.ModernButton btnAddBranch;
        private System.Windows.Forms.TableLayoutPanel tblKpi;
        private Controls.KpiCardControl kpiTotal;
        private Controls.KpiCardControl kpiActive;
        private Controls.KpiCardControl kpiMaintenance;
        private Controls.KpiCardControl kpiEmployees;
        private Controls.AdminDataTableControl tableBranches;
    }
}
