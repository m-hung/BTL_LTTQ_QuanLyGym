using System;
using System.Drawing;
using System.Windows.Forms;

namespace QuanLyGym.UI.Forms.Admin.Pages
{
    public partial class ucAdminPlaceholder : UserControl
    {
        public event EventHandler? BackToDashboardRequested;

        public ucAdminPlaceholder()
        {
            InitializeComponent();
            btnBackToDashboard.Click += (s, e) => BackToDashboardRequested?.Invoke(this, EventArgs.Empty);

            // Style Card with rounded border
            pnlCard.Paint += (s, e) =>
            {
                AdminTheme.DrawCard(e.Graphics, pnlCard.ClientRectangle, radius: 10);
            };
        }

        public void SetTabInfo(string tabTitle, string description)
        {
            lblTitle.Text = tabTitle.ToUpperInvariant();
            lblDesc.Text = description;
        }
    }
}
