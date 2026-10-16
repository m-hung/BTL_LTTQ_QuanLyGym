using System;
using System.Drawing;
using System.Windows.Forms;
using QuanLyGym.UI.Forms.Admin.Pages;

namespace QuanLyGym.UI.Forms.Admin
{
    public partial class frmAdminMain : Form
    {
        private ucAdminDashboard? _dashboardView;
        private ucAdminRevenue? _revenueView;
        private ucAdminBranches? _branchesView;
        private ucAdminEmployees? _employeesView;
        private ucAdminMembers? _membersView;
        private ucAdminAccounts? _accountsView;
        private ucAdminPlaceholder? _placeholderView;

        public frmAdminMain()
        {
            InitializeComponent();
            SetStyle(ControlStyles.AllPaintingInWmPaint |
                     ControlStyles.UserPaint |
                     ControlStyles.OptimizedDoubleBuffer, true);
            DoubleBuffered = true;

            InitializeViews();
            WireEvents();
        }

        private void InitializeViews()
        {
            _dashboardView = new ucAdminDashboard { Dock = DockStyle.Fill };
            _revenueView = new ucAdminRevenue { Dock = DockStyle.Fill };
            _branchesView = new ucAdminBranches { Dock = DockStyle.Fill };
            _employeesView = new ucAdminEmployees { Dock = DockStyle.Fill };
            _membersView = new ucAdminMembers { Dock = DockStyle.Fill };
            _accountsView = new ucAdminAccounts { Dock = DockStyle.Fill };

            _placeholderView = new ucAdminPlaceholder { Dock = DockStyle.Fill };
            _placeholderView.BackToDashboardRequested += (s, e) =>
            {
                navBar.SelectedIndex = 0;
            };

            // Load Dashboard by default
            pnlContent.Controls.Add(_dashboardView);
        }

        private void WireEvents()
        {
            // Navigation Bar Tab Click
            navBar.SelectedIndexChanged += (s, index) =>
            {
                SwitchToTab(index);
            };

            // Top Bar Logout
            topBar.LogoutClicked += (s, e) =>
            {
                var result = MessageBox.Show(
                    "Bạn có chắc chắn muốn đăng xuất khỏi hệ thống?",
                    "Xác nhận đăng xuất",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question
                );

                if (result == DialogResult.Yes)
                {
                    this.Close();
                }
            };
        }

        public void SwitchToTab(int tabIndex)
        {
            if (navBar.SelectedIndex != tabIndex)
            {
                navBar.SelectedIndex = tabIndex;
                return; // SelectedIndexChanged will trigger SwitchToTab
            }

            pnlContent.SuspendLayout();
            pnlContent.Controls.Clear();

            switch (tabIndex)
            {
                case 0:
                    if (_dashboardView != null) pnlContent.Controls.Add(_dashboardView);
                    break;
                case 1:
                    if (_revenueView != null) pnlContent.Controls.Add(_revenueView);
                    break;
                case 2:
                    if (_branchesView != null) pnlContent.Controls.Add(_branchesView);
                    break;
                case 3:
                    if (_employeesView != null) pnlContent.Controls.Add(_employeesView);
                    break;
                case 4:
                    if (_membersView != null) pnlContent.Controls.Add(_membersView);
                    break;
                case 5:
                    if (_accountsView != null) pnlContent.Controls.Add(_accountsView);
                    break;
                default:
                    if (_placeholderView != null)
                    {
                        string tabName = navBar.SelectedTabName;
                        string desc = $"Khung giao diện chức năng '{tabName}' đang được chuẩn bị và sẽ hoàn thiện ở các bước tiếp theo.";
                        _placeholderView.SetTabInfo($"QUẢN LÝ {tabName}", desc);
                        pnlContent.Controls.Add(_placeholderView);
                    }
                    break;
            }

            pnlContent.ResumeLayout(true);
        }

        public void SetHoverForDemo(int monthlyIndex, int branchIndex, int memberIndex)
        {
            _dashboardView?.SetHoverForDemo(monthlyIndex, branchIndex, memberIndex);
        }
    }
}
