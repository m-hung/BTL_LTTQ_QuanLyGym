namespace QuanLyGym.UI.Forms.Admin.Pages
{
    partial class ucAdminEmployees
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
            this.btnAddEmployee = new QuanLyGym.UI.Forms.Admin.Controls.ModernButton();
            this.tblKpi = new System.Windows.Forms.TableLayoutPanel();
            this.kpiTotal = new QuanLyGym.UI.Forms.Admin.Controls.KpiCardControl();
            this.kpiPt = new QuanLyGym.UI.Forms.Admin.Controls.KpiCardControl();
            this.kpiCashier = new QuanLyGym.UI.Forms.Admin.Controls.KpiCardControl();
            this.kpiStaff = new QuanLyGym.UI.Forms.Admin.Controls.KpiCardControl();
            this.subTabs = new QuanLyGym.UI.Forms.Admin.Controls.SubTabBarControl();
            this.pnlCardsGrid = new System.Windows.Forms.TableLayoutPanel();
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
            this.mainLayout.Controls.Add(this.subTabs, 0, 2);
            this.mainLayout.Controls.Add(this.pnlCardsGrid, 0, 3);
            this.mainLayout.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mainLayout.Location = new System.Drawing.Point(0, 0);
            this.mainLayout.Name = "mainLayout";
            this.mainLayout.Padding = new System.Windows.Forms.Padding(20, 14, 20, 14);
            this.mainLayout.RowCount = 4;
            this.mainLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 42F));
            this.mainLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 100F));
            this.mainLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.mainLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.mainLayout.Size = new System.Drawing.Size(1264, 700);
            this.mainLayout.TabIndex = 0;
            // 
            // pnlHeader
            // 
            this.pnlHeader.Controls.Add(this.btnAddEmployee);
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
            this.lblTitle.Text = "QUẢN LÝ NHÂN VIÊN";
            // 
            // btnAddEmployee
            // 
            this.btnAddEmployee.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnAddEmployee.BackColor = System.Drawing.Color.Transparent;
            this.btnAddEmployee.BorderRadius = 5;
            this.btnAddEmployee.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnAddEmployee.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.btnAddEmployee.ForeColor = System.Drawing.Color.White;
            this.btnAddEmployee.HoverColor = System.Drawing.Color.FromArgb(255, 133, 38);
            this.btnAddEmployee.Location = new System.Drawing.Point(1074, 4);
            this.btnAddEmployee.Name = "btnAddEmployee";
            this.btnAddEmployee.NormalColor = System.Drawing.Color.FromArgb(255, 107, 0);
            this.btnAddEmployee.PressedColor = System.Drawing.Color.FromArgb(224, 94, 0);
            this.btnAddEmployee.Size = new System.Drawing.Size(150, 32);
            this.btnAddEmployee.TabIndex = 1;
            this.btnAddEmployee.Text = "+ Thêm nhân viên";
            this.btnAddEmployee.UseVisualStyleBackColor = false;
            // 
            // tblKpi
            // 
            this.tblKpi.ColumnCount = 4;
            this.tblKpi.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tblKpi.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tblKpi.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tblKpi.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tblKpi.Controls.Add(this.kpiTotal, 0, 0);
            this.tblKpi.Controls.Add(this.kpiPt, 1, 0);
            this.tblKpi.Controls.Add(this.kpiCashier, 2, 0);
            this.tblKpi.Controls.Add(this.kpiStaff, 3, 0);
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
            this.kpiTotal.CardTitle = "TỔNG NHÂN VIÊN";
            this.kpiTotal.Dock = System.Windows.Forms.DockStyle.Fill;
            this.kpiTotal.KpiSubtitle = "";
            this.kpiTotal.KpiValue = "15";
            this.kpiTotal.Location = new System.Drawing.Point(0, 0);
            this.kpiTotal.Margin = new System.Windows.Forms.Padding(0, 0, 8, 0);
            this.kpiTotal.Name = "kpiTotal";
            this.kpiTotal.ValueColor = System.Drawing.Color.FromArgb(255, 107, 0);
            // 
            // kpiPt
            // 
            this.kpiPt.CardTitle = "PT (HUẤN LUYỆN VIÊN)";
            this.kpiPt.Dock = System.Windows.Forms.DockStyle.Fill;
            this.kpiPt.KpiSubtitle = "";
            this.kpiPt.KpiValue = "5";
            this.kpiPt.Location = new System.Drawing.Point(306, 0);
            this.kpiPt.Margin = new System.Windows.Forms.Padding(4, 0, 8, 0);
            this.kpiPt.Name = "kpiPt";
            this.kpiPt.ValueColor = System.Drawing.Color.FromArgb(47, 128, 237);
            // 
            // kpiCashier
            // 
            this.kpiCashier.CardTitle = "THU NGÂN";
            this.kpiCashier.Dock = System.Windows.Forms.DockStyle.Fill;
            this.kpiCashier.KpiSubtitle = "";
            this.kpiCashier.KpiValue = "4";
            this.kpiCashier.Location = new System.Drawing.Point(612, 0);
            this.kpiCashier.Margin = new System.Windows.Forms.Padding(4, 0, 8, 0);
            this.kpiCashier.Name = "kpiCashier";
            this.kpiCashier.ValueColor = System.Drawing.Color.FromArgb(16, 185, 129);
            // 
            // kpiStaff
            // 
            this.kpiStaff.CardTitle = "TẠP VỤ - BẢO VỆ";
            this.kpiStaff.Dock = System.Windows.Forms.DockStyle.Fill;
            this.kpiStaff.KpiSubtitle = "";
            this.kpiStaff.KpiValue = "6";
            this.kpiStaff.Location = new System.Drawing.Point(918, 0);
            this.kpiStaff.Margin = new System.Windows.Forms.Padding(4, 0, 0, 0);
            this.kpiStaff.Name = "kpiStaff";
            this.kpiStaff.ValueColor = System.Drawing.Color.FromArgb(168, 85, 247);
            // 
            // subTabs
            // 
            this.subTabs.Dock = System.Windows.Forms.DockStyle.Fill;
            this.subTabs.Height = 36;
            this.subTabs.Location = new System.Drawing.Point(20, 156);
            this.subTabs.Margin = new System.Windows.Forms.Padding(0, 0, 0, 12);
            this.subTabs.Name = "subTabs";
            this.subTabs.SelectedIndex = 0;
            this.subTabs.Size = new System.Drawing.Size(1224, 28);
            this.subTabs.TabIndex = 2;
            this.subTabs.Tabs = new string[] { "PT", "THU NGÂN", "TẠP VỤ", "BẢO VỆ" };
            // 
            // pnlCardsGrid
            // 
            this.pnlCardsGrid.AutoScroll = true;
            this.pnlCardsGrid.ColumnCount = 3;
            this.pnlCardsGrid.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33F));
            this.pnlCardsGrid.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33F));
            this.pnlCardsGrid.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.34F));
            this.pnlCardsGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlCardsGrid.Location = new System.Drawing.Point(20, 196);
            this.pnlCardsGrid.Margin = new System.Windows.Forms.Padding(0);
            this.pnlCardsGrid.Name = "pnlCardsGrid";
            this.pnlCardsGrid.RowCount = 2;
            this.pnlCardsGrid.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 148F));
            this.pnlCardsGrid.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 148F));
            this.pnlCardsGrid.Size = new System.Drawing.Size(1224, 490);
            this.pnlCardsGrid.TabIndex = 3;
            // 
            // ucAdminEmployees
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(14, 15, 18);
            this.Controls.Add(this.mainLayout);
            this.Name = "ucAdminEmployees";
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
        private Controls.ModernButton btnAddEmployee;
        private System.Windows.Forms.TableLayoutPanel tblKpi;
        private Controls.KpiCardControl kpiTotal;
        private Controls.KpiCardControl kpiPt;
        private Controls.KpiCardControl kpiCashier;
        private Controls.KpiCardControl kpiStaff;
        private Controls.SubTabBarControl subTabs;
        private System.Windows.Forms.TableLayoutPanel pnlCardsGrid;
    }
}
