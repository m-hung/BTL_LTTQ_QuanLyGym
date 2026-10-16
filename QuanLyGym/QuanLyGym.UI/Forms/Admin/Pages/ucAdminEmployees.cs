using System;
using System.Drawing;
using System.Windows.Forms;
using QuanLyGym.UI.Forms.Admin.Controls;

namespace QuanLyGym.UI.Forms.Admin.Pages
{
    public partial class ucAdminEmployees : UserControl
    {
        public ucAdminEmployees()
        {
            InitializeComponent();
            PopulateEmployeeCards();
            WireEvents();
        }

        private void PopulateEmployeeCards()
        {
            pnlCardsGrid.Controls.Clear();

            var cards = new[]
            {
                new EmployeeCardControl
                {
                    EmployeeName = "Lê Công Vinh",
                    Role = "PT",
                    Phone = "0910.765.432",
                    Branch = "CN Hoàn Kiếm",
                    Certifications = "ACE CPT, NASM",
                    Rating = 4.9f,
                    StudentsCount = 24,
                    SalaryText = "15.000.000 đ",
                    Dock = DockStyle.Fill,
                    Margin = new Padding(0, 0, 8, 8)
                },
                new EmployeeCardControl
                {
                    EmployeeName = "Trần Thị Bích",
                    Role = "PT",
                    Phone = "0943.210.987",
                    Branch = "CN Quận 1",
                    Certifications = "ISSA CPT",
                    Rating = 4.8f,
                    StudentsCount = 18,
                    SalaryText = "14.500.000 đ",
                    Dock = DockStyle.Fill,
                    Margin = new Padding(4, 0, 8, 8)
                },
                new EmployeeCardControl
                {
                    EmployeeName = "Nguyễn Đức Minh",
                    Role = "PT",
                    Phone = "0967.543.210",
                    Branch = "CN Cầu Giấy",
                    Certifications = "ACE CPT",
                    Rating = 4.7f,
                    StudentsCount = 15,
                    SalaryText = "13.000.000 đ",
                    Dock = DockStyle.Fill,
                    Margin = new Padding(4, 0, 0, 8)
                },
                new EmployeeCardControl
                {
                    EmployeeName = "Phan Thị Thảo",
                    Role = "PT",
                    Phone = "0921.098.765",
                    Branch = "CN Bình Thạnh",
                    Certifications = "NASM CPT",
                    Rating = 4.6f,
                    StudentsCount = 12,
                    SalaryText = "12.500.000 đ",
                    Dock = DockStyle.Fill,
                    Margin = new Padding(0, 0, 8, 0)
                },
                new EmployeeCardControl
                {
                    EmployeeName = "Vũ Hoàng Long",
                    Role = "PT",
                    Phone = "0909.876.543",
                    Branch = "CN Hải Châu",
                    Certifications = "ACE CPT",
                    Rating = 4.5f,
                    StudentsCount = 9,
                    SalaryText = "12.000.000 đ",
                    Dock = DockStyle.Fill,
                    Margin = new Padding(4, 0, 8, 0)
                }
            };

            pnlCardsGrid.Controls.Add(cards[0], 0, 0);
            pnlCardsGrid.Controls.Add(cards[1], 1, 0);
            pnlCardsGrid.Controls.Add(cards[2], 2, 0);
            pnlCardsGrid.Controls.Add(cards[3], 0, 1);
            pnlCardsGrid.Controls.Add(cards[4], 1, 1);

            foreach (var card in cards)
            {
                card.CardClicked += (s, e) =>
                {
                    MessageBox.Show($"Xem chi tiết huấn luyện viên: {card.EmployeeName} ({card.Branch})", "Chi tiết nhân viên", MessageBoxButtons.OK, MessageBoxIcon.Information);
                };
            }
        }

        private void WireEvents()
        {
            btnAddEmployee.Click += (s, e) =>
            {
                MessageBox.Show("Mở form thêm mới nhân viên", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            };

            subTabs.SelectedIndexChanged += (s, idx) =>
            {
                string tab = subTabs.SelectedTabName;
                // Instant filter feedback
            };
        }
    }
}
