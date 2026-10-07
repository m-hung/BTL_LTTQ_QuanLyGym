namespace QuanLyGym.UI
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void btnXinChao_Click(object sender, EventArgs e)
        {
            MessageBox.Show("XIN CHÀO", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}
