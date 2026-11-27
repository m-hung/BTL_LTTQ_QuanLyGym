namespace QuanLyGym.UI.Forms.PT
{
    partial class ucThongTinCaNhan
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.Panel pnlScrollHost;

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
            this.pnlScrollHost = new System.Windows.Forms.Panel();
            this.SuspendLayout();
            // 
            // pnlScrollHost
            // 
            this.pnlScrollHost.AutoScroll = true;
            this.pnlScrollHost.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(15)))), ((int)(((byte)(17)))), ((int)(((byte)(21)))));
            this.pnlScrollHost.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlScrollHost.Location = new System.Drawing.Point(0, 0);
            this.pnlScrollHost.Name = "pnlScrollHost";
            this.pnlScrollHost.Padding = new System.Windows.Forms.Padding(24, 18, 24, 24);
            this.pnlScrollHost.Size = new System.Drawing.Size(1134, 756);
            this.pnlScrollHost.TabIndex = 0;
            // 
            // ucThongTinCaNhan
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(15)))), ((int)(((byte)(17)))), ((int)(((byte)(21)))));
            this.Controls.Add(this.pnlScrollHost);
            this.Name = "ucThongTinCaNhan";
            this.Size = new System.Drawing.Size(1134, 756);
            this.ResumeLayout(false);
        }
    }
}
