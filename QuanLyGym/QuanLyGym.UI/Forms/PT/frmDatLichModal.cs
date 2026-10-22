using System;
using System.Drawing;
using System.Windows.Forms;

namespace QuanLyGym.UI.Forms.PT
{
    public partial class frmDatLichModal : Form
    {
        public frmDatLichModal()
        {
            InitializeComponent();
            this.BackColor = PTUITheme.BgDark;
            this.StartPosition = FormStartPosition.CenterParent;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;

            cboMember.SelectedIndex = 0;
            cboTimeSlot.SelectedIndex = 0;
            cboLocation.SelectedIndex = 0;
        }

        private void btnConfirm_Click(object sender, EventArgs e)
        {
            string member = cboMember.SelectedItem?.ToString() ?? "";
            string time = cboTimeSlot.SelectedItem?.ToString() ?? "";
            string date = dtpDate.Value.ToString("dd/MM/yyyy");

            MessageBox.Show($"Đã tạo lịch huấn luyện 1-1 thành công!\n\nHội viên: {member}\nThời gian: {time} - Ngày: {date}\nĐịa điểm: {cboLocation.SelectedItem}",
                "Thành Công", MessageBoxButtons.OK, MessageBoxIcon.Information);

            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}
