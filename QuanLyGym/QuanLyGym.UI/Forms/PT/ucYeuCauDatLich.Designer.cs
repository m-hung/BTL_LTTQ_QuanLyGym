namespace QuanLyGym.UI.Forms.PT
{
    partial class ucYeuCauDatLich
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.Panel pnlRoot;

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
            this.pnlRoot = new System.Windows.Forms.Panel();
            this.SuspendLayout();
            // 
            // pnlRoot
            // 
            this.pnlRoot.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(15)))), ((int)(((byte)(17)))), ((int)(((byte)(21)))));
            this.pnlRoot.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlRoot.Location = new System.Drawing.Point(0, 0);
            this.pnlRoot.Name = "pnlRoot";
            this.pnlRoot.Padding = new System.Windows.Forms.Padding(24, 16, 24, 20);
            this.pnlRoot.Size = new System.Drawing.Size(1134, 756);
            this.pnlRoot.TabIndex = 0;
            // 
            // ucYeuCauDatLich
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(15)))), ((int)(((byte)(17)))), ((int)(((byte)(21)))));
            this.Controls.Add(this.pnlRoot);
            this.Name = "ucYeuCauDatLich";
            this.Size = new System.Drawing.Size(1134, 756);
            this.ResumeLayout(false);
        }
    }
}
