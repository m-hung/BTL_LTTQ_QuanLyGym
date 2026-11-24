using System;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QuanLyGym.UI.Forms.Chat
{
    /// <summary>
    /// Cửa sổ Form Chatbot độc lập, bao bọc ucChatbot.
    /// Có thể hiển thị bằng Show() hoặc ShowDialog() từ menu chính của Admin, PT hoặc Hội viên.
    /// </summary>
    public partial class frmChatbot : Form
    {
        public frmChatbot()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Cho phép thiết lập AI Handler từ bên ngoài Form nếu cần kết nối Service/BLL
        /// </summary>
        public void SetCustomAiHandler(Func<string, Task<string>> handler)
        {
            ucChatbotControl.CustomAiHandler = handler;
        }
    }
}
