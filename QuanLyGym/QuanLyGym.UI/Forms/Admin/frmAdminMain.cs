using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using QuanLyGym.BLL.Models;
using QuanLyGym.BLL.Services;
using QuanLyGym.UI.Helpers;

namespace QuanLyGym.UI.Forms.Admin
{
    public partial class frmAdminMain : Form
    {
        private static readonly Color HeaderColor = Color.FromArgb(10, 10, 12);
        private static readonly Color CardColor = Color.FromArgb(24, 24, 27);
        private static readonly Color BorderColor = Color.FromArgb(45, 45, 50);
        private static readonly Color MutedColor = Color.FromArgb(161, 161, 170);
        private static readonly Color Orange = Color.FromArgb(249, 115, 22);
        private static readonly Color Blue = Color.FromArgb(59, 130, 246);

        private Button? _activeNav;
        private ToolTip? _chartToolTip;
        private string _activeToolTipText = "";

        public frmAdminMain()
        {
            InitializeComponent();
            InitializeCustomControls();
        }

        private void frmAdminMain_Load(object sender, EventArgs e)
        {
            SetActiveNav(btnNavOverview);

            // Hiển thị tài khoản đang đăng nhập
            lblUserName.Text = UserSession.DisplayName;
            lblUserRole.Text = "Quản trị viên";

            tmrClock.Start();
            tmrClock_Tick(this, EventArgs.Empty);

            // Tạo avatar và logo tròn mềm mại
            MakeCircular(lblLogo);
            MakeCircular(lblAvatar);
            lblLogo.SizeChanged += (s, ev) => MakeCircular(lblLogo);
            lblAvatar.SizeChanged += (s, ev) => MakeCircular(lblAvatar);

            // Hiệu ứng hover cho nút Đăng xuất và Chuông thông báo
            btnLogout.MouseEnter += (s, ev) =>
            {
                btnLogout.BackColor = Color.FromArgb(39, 39, 42);
                btnLogout.FlatAppearance.BorderColor = Orange;
            };
            btnLogout.MouseLeave += (s, ev) =>
            {
                btnLogout.BackColor = Color.FromArgb(24, 24, 27);
                btnLogout.FlatAppearance.BorderColor = Color.FromArgb(63, 63, 70);
            };

            btnNotification.MouseEnter += (s, ev) =>
            {
                btnNotification.ForeColor = Color.White;
                btnNotification.BackColor = Color.FromArgb(24, 24, 27);
            };
            btnNotification.MouseLeave += (s, ev) =>
            {
                btnNotification.ForeColor = MutedColor;
                btnNotification.BackColor = Color.Transparent;
            };

            // Hiệu ứng hover cho nút Xuất báo cáo
            btnExportReport.MouseEnter += (s, ev) =>
            {
                btnExportReport.BackColor = Color.FromArgb(234, 88, 12);
            };
            btnExportReport.MouseLeave += (s, ev) =>
            {
                btnExportReport.BackColor = Orange;
            };

            // Khởi tạo ToolTip tuỳ chỉnh và gắn hiệu ứng hover cho các biểu đồ
            InitChartToolTip();
            AttachChartHover(chartRevenue, pt => $"Tháng {pt.AxisLabel}\nDoanh thu: {pt.YValues[0]:N1} triệu VNĐ");
            AttachChartHover(chartBranch, pt => $"Chi nhánh {pt.AxisLabel}\nDoanh thu: {pt.YValues[0]:N1} triệu VNĐ");
            AttachChartHover(chartNewMember, pt => $"Tháng {pt.AxisLabel}\nHội viên mới: {pt.YValues[0]:N0} người");

            // Nạp dữ liệu thực tế từ Database
            LoadAllChartsAndData();
        }

        private void MakeCircular(Control c)
        {
            if (c.Width <= 0 || c.Height <= 0) return;
            using (var path = new System.Drawing.Drawing2D.GraphicsPath())
            {
                path.AddEllipse(0, 0, c.Width, c.Height);
                c.Region = new Region(path);
            }
        }

        private void lblAvatar_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            using (var pen = new Pen(Orange, 1.5f))
            {
                e.Graphics.DrawEllipse(pen, 1, 1, lblAvatar.Width - 3, lblAvatar.Height - 3);
            }
        }

        private void frmAdminMain_Shown(object sender, EventArgs e)
        {
            // Đảm bảo khi form hiển thị hoàn toàn thì nạp và render lại dữ liệu chuẩn kích thước
            LoadAllChartsAndData();
        }

        public void LoadAllChartsAndData()
        {
            UpdateRevenueKpis();
            DrawRevenueChart();
            DrawBranchChart();
            DrawNewMemberChart();
            LoadExpiringMemberships();
        }

        /// <summary>
        /// Cập nhật 4 thẻ KPI chỉ số doanh thu và quy mô từ Database
        /// </summary>
        private void UpdateRevenueKpis()
        {
            try
            {
                int currentYear = DateTime.Now.Year;
                var summary = RevenueService.Instance.GetRevenueSummary(currentYear);
                if (summary.TotalRevenue == 0)
                {
                    summary = RevenueService.Instance.GetRevenueSummary(null);
                }

                lblKpiRevenueTitle.Text = $"TỔNG DOANH THU ({currentYear})";
                if (summary.TotalRevenue >= 1_000_000_000m)
                    lblKpiRevenueValue.Text = $"{(summary.TotalRevenue / 1_000_000_000m):N2} tỷ đ";
                else if (summary.TotalRevenue >= 1_000_000m)
                    lblKpiRevenueValue.Text = $"{(summary.TotalRevenue / 1_000_000m):N1} triệu đ";
                else
                    lblKpiRevenueValue.Text = $"{summary.TotalRevenue:N0} đ";

                lblKpiRevenueSub.Text = $"Cập nhật T{DateTime.Now.Month}/{currentYear}";

                lblKpiMemberTitle.Text = "TỔNG HỘI VIÊN";
                lblKpiMemberValue.Text = summary.TotalMembers.ToString("N0");
                lblKpiMemberSub.Text = $"{summary.ActiveMembers} đang hoạt động";

                lblKpiBranchTitle.Text = "CHI NHÁNH";
                lblKpiBranchValue.Text = summary.TotalBranches.ToString("N0");
                lblKpiBranchSub.Text = $"{summary.ActiveBranches} đang hoạt động";

                lblKpiStaffTitle.Text = "NHÂN VIÊN";
                lblKpiStaffValue.Text = summary.TotalStaff.ToString("N0");
                lblKpiStaffSub.Text = "Toàn hệ thống";
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[frmAdminMain.UpdateRevenueKpis] Error: {ex.Message}");
            }
        }

        private void ShowPage(Button btn)
        {
            // Trang Tổng quan chỉ hiện khi bấm "Tổng quan"
            pnlContent.Visible = (btn == btnNavOverview);
            if (btn == btnNavOverview)
            {
                LoadAllChartsAndData();
            }

            // Trang Chi nhánh
            if (btn == btnNavBranch)
            {
                if (_ucBranch == null)
                {
                    _ucBranch = new ucAdminBranch { Dock = DockStyle.Fill };
                    Controls.Add(_ucBranch);
                    _ucBranch.BringToFront();
                }
                _ucBranch.Visible = true;
                _ucBranch.ReloadFromDatabase();
            }
            else if (_ucBranch != null)
            {
                _ucBranch.Visible = false;
            }

            // Trang Nhân viên
            if (btn == btnNavStaff)
            {
                if (_ucStaff == null)
                {
                    _ucStaff = new ucAdminStaff { Dock = DockStyle.Fill };
                    Controls.Add(_ucStaff);
                    _ucStaff.BringToFront();
                }
                _ucStaff.Visible = true;
            }
            else if (_ucStaff != null)
            {
                _ucStaff.Visible = false;
            }

            // Trang Hội viên
            if (btn == btnNavMember)
            {
                if (_ucMember == null)
                {
                    _ucMember = new ucAdminMember { Dock = DockStyle.Fill };
                    Controls.Add(_ucMember);
                    _ucMember.BringToFront();
                }
                _ucMember.Visible = true;
            }
            else if (_ucMember != null)
            {
                _ucMember.Visible = false;
            }

            // Trang Tài khoản
            if (btn == btnNavAccount)
            {
                if (_ucAccount == null)
                {
                    _ucAccount = new ucAdminAccount { Dock = DockStyle.Fill };
                    Controls.Add(_ucAccount);
                    _ucAccount.BringToFront();
                }
                _ucAccount.Visible = true;
            }
            else if (_ucAccount != null)
            {
                _ucAccount.Visible = false;
            }
        }

        private void tmrClock_Tick(object sender, EventArgs e)
        {
            lblDateTime.Text = DateTime.Now.ToString("dd/MM/yyyy — HH:mm");
        }

        private void btnNav_Click(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            SetActiveNav(btn);
            ShowPage(btn);
        }

        private void btnLogout_Click(object sender, EventArgs e)
        {
            UserSession.Logout(this);
        }

        private void InitializeCustomControls()
        {
            // Cấu hình các nút điều hướng
            ConfigureNavButton(btnNavOverview, "btnNavOverview", "▦  Tổng quan");
            ConfigureNavButton(btnNavBranch, "btnNavBranch", "⌂  Chi nhánh");
            ConfigureNavButton(btnNavStaff, "btnNavStaff", "☺  Nhân viên");
            ConfigureNavButton(btnNavMember, "btnNavMember", "♙  Hội viên");
            ConfigureNavButton(btnNavAccount, "btnNavAccount", "⚿  Tài khoản");

            // Nút Xuất báo cáo
            if (btnExportReport != null)
            {
                btnExportReport.Text = "📄 Xuất báo cáo";
            }

            // Cấu hình 4 thẻ chỉ số KPI
            SetupKpiPanel(pnlKpiRevenue, lblKpiRevenueTitle, lblKpiRevenueValue, lblKpiRevenueSub, "TỔNG DOANH THU", "0 đ", "Đang tải...", Orange);
            SetupKpiPanel(pnlKpiMember, lblKpiMemberTitle, lblKpiMemberValue, lblKpiMemberSub, "TỔNG HỘI VIÊN", "0", "0 đang hoạt động", Blue);
            SetupKpiPanel(pnlKpiBranch, lblKpiBranchTitle, lblKpiBranchValue, lblKpiBranchSub, "CHI NHÁNH", "0", "0 đang hoạt động", Color.FromArgb(34, 197, 94));
            SetupKpiPanel(pnlKpiStaff, lblKpiStaffTitle, lblKpiStaffValue, lblKpiStaffSub, "NHÂN VIÊN", "0", "Toàn hệ thống", Color.FromArgb(168, 85, 247));

            // Cấu hình 4 thẻ Card biểu đồ & danh sách
            SetupCard(pnlCardRevenue, lblRevenueTitle, $"DOANH THU CÁC THÁNG ({DateTime.Now.Year})", chartRevenue);
            SetupCard(pnlCardExpiring, lblExpiringTitle, "HỘI VIÊN SẮP HẾT HẠN (30 NGÀY TỚI)", lvExpiring);
            SetupCard(pnlCardBranchRevenue, lblBranchTitle, "DOANH THU THEO CHI NHÁNH (TRIỆU Đ)", chartBranch);
            SetupCard(pnlCardNewMember, lblNewMemberTitle, "HỘI VIÊN MỚI THEO THÁNG", chartNewMember);

            // Căn chỉnh khoảng cách sát thanh bar
            pnlContent.Padding = new Padding(12, 8, 12, 10);
            if (tblMain.RowStyles.Count > 0)
            {
                tblMain.RowStyles[0] = new RowStyle(SizeType.Absolute, 46F);
            }
            pnlTitleBar.Padding = new Padding(4, 4, 8, 4);

            // Tương tác nhanh từ Dashboard sang Trang Quản lý Hội viên
            pnlKpiMember.Cursor = Cursors.Hand;
            pnlKpiMember.Click += (s, e) =>
            {
                SetActiveNav(btnNavMember);
                ShowPage(btnNavMember);
            };

            pnlCardExpiring.Cursor = Cursors.Hand;
            pnlCardExpiring.Click += (s, e) =>
            {
                SetActiveNav(btnNavMember);
                ShowPage(btnNavMember);
                _ucMember?.SetQuickFilter(2);
            };
            lvExpiring.DoubleClick += (s, e) =>
            {
                SetActiveNav(btnNavMember);
                ShowPage(btnNavMember);
                _ucMember?.SetQuickFilter(2);
            };

            pnlCardNewMember.Cursor = Cursors.Hand;
            pnlCardNewMember.Click += (s, e) =>
            {
                SetActiveNav(btnNavMember);
                ShowPage(btnNavMember);
                _ucMember?.SetQuickFilter(1);
            };
        }

        private void SetActiveNav(Button btn)
        {
            if (btn == null) return;

            if (_activeNav != null)
            {
                _activeNav.BackColor = HeaderColor;
                _activeNav.ForeColor = MutedColor;
            }
            _activeNav = btn;
            _activeNav.BackColor = CardColor;
            _activeNav.ForeColor = Orange;

            string title = "TỔNG QUAN HỆ THỐNG";
            if (!string.IsNullOrWhiteSpace(btn.Text))
            {
                int spaceIndex = btn.Text.IndexOf(' ');
                if (spaceIndex >= 0 && spaceIndex < btn.Text.Length - 1)
                {
                    title = btn.Text.Substring(spaceIndex).Trim().ToUpper();
                }
                else
                {
                    title = btn.Text.Trim().ToUpper();
                }
            }
            lblPageTitle.Text = title;

            if (btnExportReport != null)
            {
                btnExportReport.Visible = (btn == btnNavOverview);
            }
        }

        private void btnExportReport_Click(object sender, EventArgs e)
        {
            try
            {
                using (SaveFileDialog sfd = new SaveFileDialog())
                {
                    sfd.Title = "Xuất báo cáo tổng quan GymFit từ Database";
                    sfd.Filter = "CSV (Phân cách bằng dấu phẩy) (*.csv)|*.csv|Tập tin văn bản (*.txt)|*.txt";
                    sfd.FileName = $"BaoCao_TongQuan_GymFit_{DateTime.Now:yyyyMMdd_HHmm}.csv";

                    if (sfd.ShowDialog() == DialogResult.OK)
                    {
                        int currentYear = DateTime.Now.Year;
                        string csvContent = RevenueService.Instance.GenerateRevenueReportCsv(currentYear);
                        File.WriteAllText(sfd.FileName, csvContent, new UTF8Encoding(true));

                        DialogResult dr = MessageBox.Show(
                            $"Xuất báo cáo tổng quan thành công từ Database!\n\nĐường dẫn: {sfd.FileName}\n\nBạn có muốn mở tệp ngay bây giờ không?",
                            "Xuất báo cáo thành công",
                            MessageBoxButtons.YesNo,
                            MessageBoxIcon.Information);

                        if (dr == DialogResult.Yes)
                        {
                            Process.Start(new ProcessStartInfo
                            {
                                FileName = sfd.FileName,
                                UseShellExecute = true
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi xuất báo cáo: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void Card_Paint(object sender, PaintEventArgs e)
        {
            Control c = (Control)sender;
            using (Pen pen = new Pen(BorderColor))
            {
                e.Graphics.DrawRectangle(pen, 0, 0, c.Width - 1, c.Height - 1);
            }
        }

        private ChartArea CreateArea(double max, double interval)
        {
            ChartArea area = new ChartArea("main");
            area.BackColor = Color.Transparent;

            area.AxisX.LineColor = BorderColor;
            area.AxisX.MajorGrid.Enabled = false;
            area.AxisX.MajorTickMark.Enabled = false;
            area.AxisX.LabelStyle.ForeColor = MutedColor;
            area.AxisX.LabelStyle.Font = new Font("Segoe UI", 8F);
            area.AxisX.Interval = 1;
            area.AxisX.IsMarginVisible = true;

            area.AxisY.LineColor = BorderColor;
            area.AxisY.MajorGrid.LineColor = Color.FromArgb(35, 45, 45, 50);
            area.AxisY.MajorGrid.LineDashStyle = ChartDashStyle.Dash;
            area.AxisY.MajorTickMark.Enabled = false;
            area.AxisY.LabelStyle.ForeColor = MutedColor;
            area.AxisY.LabelStyle.Font = new Font("Segoe UI", 7.5F);
            area.AxisY.Minimum = 0;
            area.AxisY.Maximum = max;
            area.AxisY.Interval = interval;
            return area;
        }

        private void PrepareChart(Chart chart, SeriesChartType type, Color color,
                                  string[] labels, double[] values, double max, double interval)
        {
            chart.BackColor = CardColor;
            chart.ChartAreas.Clear();
            chart.Series.Clear();
            chart.Legends.Clear();

            ChartArea area = CreateArea(max, interval);
            area.CursorX.AutoScroll = false;
            chart.ChartAreas.Add(area);

            chart.Enabled = true;

            Series s = new Series("data");
            s.ChartType = type;
            s.Color = color;
            s.IsXValueIndexed = true;

            for (int i = 0; i < labels.Length; i++)
            {
                int index = s.Points.AddY(values[i]);
                s.Points[index].AxisLabel = labels[i];
            }
            chart.Series.Add(s);
        }

        /// <summary>
        /// Vẽ biểu đồ doanh thu theo tháng từ Database (bảng Memberships, cột PriceAtPurchase)
        /// </summary>
        private void DrawRevenueChart()
        {
            try
            {
                int currentYear = DateTime.Now.Year;
                var monthly = RevenueService.Instance.GetMonthlyRevenue(currentYear);

                var labels = monthly.Select(m => m.Label).ToArray();
                var values = monthly.Select(m => m.RevenueInMillions).ToArray();

                double maxVal = values.Length > 0 ? values.Max() : 0;
                double maxAxis = Math.Max(10, Math.Ceiling(maxVal * 1.25 / 10) * 10);
                double interval = Math.Max(1, Math.Round(maxAxis / 4));

                PrepareChart(chartRevenue, SeriesChartType.SplineArea, Color.FromArgb(70, Orange), labels, values, maxAxis, interval);
                if (chartRevenue.Series.Count > 0)
                {
                    chartRevenue.Series[0].BorderColor = Orange;
                    chartRevenue.Series[0].BorderWidth = 3;
                    chartRevenue.Series[0].MarkerStyle = MarkerStyle.Circle;
                    chartRevenue.Series[0].MarkerSize = 7;
                    chartRevenue.Series[0].MarkerColor = Orange;
                    chartRevenue.Series[0].MarkerBorderColor = Color.FromArgb(24, 24, 27);
                    chartRevenue.Series[0].MarkerBorderWidth = 2;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[DrawRevenueChart] Error: {ex.Message}");
            }
        }

        /// <summary>
        /// Vẽ biểu đồ doanh thu theo từng chi nhánh từ Database (bảng Branches & Memberships, cột BranchID & PriceAtPurchase)
        /// </summary>
        private void DrawBranchChart()
        {
            try
            {
                var branches = RevenueService.Instance.GetRevenueByBranch();
                if (branches.Count == 0)
                {
                    PrepareChart(chartBranch, SeriesChartType.Column, Orange, new[] { "Chưa có dữ liệu" }, new double[] { 0 }, 10, 5);
                    return;
                }

                var labels = branches.Select(b => b.ShortName).ToArray();
                var values = branches.Select(b => b.RevenueInMillions).ToArray();

                double maxVal = values.Length > 0 ? values.Max() : 0;
                double maxAxis = Math.Max(10, Math.Ceiling(maxVal * 1.3 / 10) * 10);
                double interval = Math.Max(1, Math.Round(maxAxis / 4));

                PrepareChart(chartBranch, SeriesChartType.Column, Orange, labels, values, maxAxis, interval);
                if (chartBranch.Series.Count > 0)
                {
                    chartBranch.Series[0]["PointWidth"] = "0.45";
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[DrawBranchChart] Error: {ex.Message}");
            }
        }

        /// <summary>
        /// Vẽ biểu đồ hội viên mới đăng ký theo tháng từ Database
        /// </summary>
        private void DrawNewMemberChart()
        {
            try
            {
                int currentYear = DateTime.Now.Year;
                var list = RevenueService.Instance.GetNewMembersByMonth(currentYear);

                var recentList = list.Where(x => x.Month <= DateTime.Now.Month).ToList();
                if (recentList.Count < 6)
                {
                    recentList = list.Take(6).ToList();
                }
                else if (recentList.Count > 6)
                {
                    recentList = recentList.Skip(recentList.Count - 6).ToList();
                }

                var labels = recentList.Select(x => x.Label).ToArray();
                var values = recentList.Select(x => (double)x.Count).ToArray();

                double maxVal = values.Length > 0 ? values.Max() : 0;
                double maxAxis = Math.Max(10, Math.Ceiling(maxVal * 1.3 / 5) * 5);
                double interval = Math.Max(1, Math.Round(maxAxis / 4));

                PrepareChart(chartNewMember, SeriesChartType.Column, Blue, labels, values, maxAxis, interval);
                if (chartNewMember.Series.Count > 0)
                {
                    chartNewMember.Series[0]["PointWidth"] = "0.45";
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[DrawNewMemberChart] Error: {ex.Message}");
            }
        }

        private void InitChartToolTip()
        {
            if (_chartToolTip != null) return;

            _chartToolTip = new ToolTip
            {
                InitialDelay = 0,
                ReshowDelay = 0,
                AutoPopDelay = 10000,
                OwnerDraw = true
            };

            _chartToolTip.Popup += (s, e) =>
            {
                using (Font font = new Font("Segoe UI", 9F, FontStyle.Bold))
                {
                    Size sz = TextRenderer.MeasureText(_activeToolTipText, font);
                    e.ToolTipSize = new Size(sz.Width + 24, sz.Height + 14);
                }
            };

            _chartToolTip.Draw += (s, e) =>
            {
                e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                using (SolidBrush bgBrush = new SolidBrush(Color.FromArgb(24, 24, 27)))
                {
                    e.Graphics.FillRectangle(bgBrush, e.Bounds);
                }

                using (Pen pen = new Pen(Orange, 1.5f))
                {
                    e.Graphics.DrawRectangle(pen, 0, 0, e.Bounds.Width - 1, e.Bounds.Height - 1);
                }

                using (Font font = new Font("Segoe UI", 9F, FontStyle.Bold))
                {
                    TextRenderer.DrawText(e.Graphics, _activeToolTipText, font, new Point(12, 7), Color.FromArgb(244, 244, 245));
                }
            };
        }

        private void AttachChartHover(Chart chart, Func<DataPoint, string> formatToolTip)
        {
            DataPoint? lastPoint = null;
            Color origColor = Color.Empty;
            Color origBorderColor = Color.Empty;
            int origBorderWidth = 1;
            int origMarkerSize = 6;
            Color origMarkerColor = Color.Empty;

            chart.MouseMove += (s, e) =>
            {
                HitTestResult result = chart.HitTest(e.X, e.Y);

                if ((result.ChartElementType == ChartElementType.DataPoint || result.ChartElementType == ChartElementType.DataPointLabel)
                    && result.PointIndex >= 0 && result.Series != null && result.PointIndex < result.Series.Points.Count)
                {
                    DataPoint pt = result.Series.Points[result.PointIndex];

                    if (pt != lastPoint)
                    {
                        ResetPoint(lastPoint, origColor, origBorderColor, origBorderWidth, origMarkerSize, origMarkerColor);

                        lastPoint = pt;
                        origColor = pt.Color != Color.Empty ? pt.Color : (result.Series.Color != Color.Empty ? result.Series.Color : Orange);
                        origBorderColor = pt.BorderColor;
                        origBorderWidth = pt.BorderWidth;
                        origMarkerSize = pt.MarkerSize > 0 ? pt.MarkerSize : (result.Series.MarkerSize > 0 ? result.Series.MarkerSize : 6);
                        origMarkerColor = pt.MarkerColor != Color.Empty ? pt.MarkerColor : result.Series.MarkerColor;

                        if (result.Series.ChartType == SeriesChartType.Column || result.Series.ChartType == SeriesChartType.Bar)
                        {
                            pt.Color = ControlPaint.Light(origColor, 0.35f);
                            pt.BorderColor = Color.White;
                            pt.BorderWidth = 2;
                        }
                        else
                        {
                            pt.MarkerStyle = MarkerStyle.Circle;
                            pt.MarkerSize = origMarkerSize + 4;
                            pt.MarkerColor = Color.White;
                            pt.MarkerBorderColor = Orange;
                            pt.MarkerBorderWidth = 2;
                        }

                        chart.Cursor = Cursors.Hand;
                        _activeToolTipText = formatToolTip(pt);

                        // Đo kích thước thực tế của Tooltip để tránh đè lên con trỏ chuột
                        Size tipSize;
                        using (Font font = new Font("Segoe UI", 9F, FontStyle.Bold))
                        {
                            Size sz = TextRenderer.MeasureText(_activeToolTipText, font);
                            tipSize = new Size(sz.Width + 24, sz.Height + 14);
                        }

                        // Tính toạ độ X an toàn: nếu ở sát lề phải thì lùi sang bên trái chuột
                        int posX;
                        if (e.X + 16 + tipSize.Width <= chart.Width - 6)
                        {
                            posX = e.X + 16;
                        }
                        else
                        {
                            posX = e.X - tipSize.Width - 18;
                            if (posX < 6) posX = 6;
                        }

                        // Tính toạ độ Y an toàn
                        int posY = e.Y - tipSize.Height - 12;
                        if (posY < 6)
                        {
                            posY = e.Y + 24;
                        }

                        // Nếu X vẫn giao cắt với vị trí chuột, đảm bảo Y không đè lên chuột
                        if (posX <= e.X && e.X <= posX + tipSize.Width)
                        {
                            if (e.Y - tipSize.Height - 16 >= 6)
                                posY = e.Y - tipSize.Height - 16;
                            else
                                posY = e.Y + 24;
                        }

                        _chartToolTip?.Show(_activeToolTipText, chart, posX, posY);
                    }
                }
                else
                {
                    if (lastPoint != null)
                    {
                        ResetPoint(lastPoint, origColor, origBorderColor, origBorderWidth, origMarkerSize, origMarkerColor);
                        lastPoint = null;
                        chart.Cursor = Cursors.Default;
                        _chartToolTip?.Hide(chart);
                    }
                }
            };

            chart.MouseLeave += (s, e) =>
            {
                Point mousePos = chart.PointToClient(System.Windows.Forms.Cursor.Position);
                if (!chart.ClientRectangle.Contains(mousePos))
                {
                    if (lastPoint != null)
                    {
                        ResetPoint(lastPoint, origColor, origBorderColor, origBorderWidth, origMarkerSize, origMarkerColor);
                        lastPoint = null;
                        chart.Cursor = Cursors.Default;
                        _chartToolTip?.Hide(chart);
                    }
                }
            };
        }

        private void ResetPoint(DataPoint? pt, Color col, Color borderCol, int borderWidth, int markerSize, Color markerCol)
        {
            if (pt == null) return;
            pt.Color = col;
            pt.BorderColor = borderCol;
            pt.BorderWidth = borderWidth;
            pt.MarkerSize = markerSize;
            pt.MarkerColor = markerCol;
            pt.MarkerBorderColor = Color.Transparent;
            pt.MarkerBorderWidth = 1;
        }

        /// <summary>
        /// Nạp danh sách hội viên sắp hết hạn thẻ tập từ Database (trong vòng 30 ngày tới)
        /// </summary>
        private void LoadExpiringMemberships()
        {
            try
            {
                lvExpiring.Items.Clear();
                var list = RevenueService.Instance.GetExpiringMemberships(30);

                if (list.Count == 0)
                {
                    ListViewItem empty = new ListViewItem("Không có hội viên sắp hết hạn (30 ngày tới)");
                    empty.SubItems.Add("");
                    empty.SubItems.Add("");
                    empty.ForeColor = MutedColor;
                    lvExpiring.Items.Add(empty);
                    return;
                }

                foreach (var item in list)
                {
                    Color remainColor;
                    if (item.DaysRemaining <= 3)
                    {
                        remainColor = Color.FromArgb(239, 68, 68); // Đỏ khẩn cấp
                    }
                    else if (item.DaysRemaining <= 7)
                    {
                        remainColor = Orange; // Cam cảnh báo
                    }
                    else
                    {
                        remainColor = Color.FromArgb(234, 179, 8); // Vàng
                    }

                    AddExpiring(item.MemberName, item.BranchName, $"{item.DaysRemaining} ngày", remainColor);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[LoadExpiringMemberships] Error: {ex.Message}");
            }
        }

        private void AddExpiring(string name, string branch, string remain, Color remainColor)
        {
            ListViewItem item = new ListViewItem(name);
            item.SubItems.Add(branch);
            item.SubItems.Add(remain);
            item.SubItems[1].ForeColor = MutedColor;
            item.SubItems[2].ForeColor = remainColor;
            item.UseItemStyleForSubItems = false;
            lvExpiring.Items.Add(item);
        }

        private void tblRow2_Paint(object sender, PaintEventArgs e)
        {

        }

        private ucAdminStaff? _ucStaff;
        private ucAdminBranch? _ucBranch;
        private ucAdminMember? _ucMember;
        private ucAdminAccount? _ucAccount;
    }
}
