using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Threading.Tasks;
using System.Windows.Forms;
using QuanLyGym.BLL.Services;

namespace QuanLyGym.UI.Forms.Chat
{
    /// <summary>
    /// UserControl AI Chatbot hỗ trợ tư vấn học viên, bài tập và dịch vụ Gym.
    /// Thiết kế chuẩn WinForms hiện đại, bất đồng bộ (async/await), tự động căn lề và tính kích thước bubble.
    /// </summary>
    public partial class ucChatbot : UserControl
    {
        // Vị trí trục Y để xếp các tin nhắn liên tiếp
        private int _currentContentHeight = 12;

        // Trạng thái hiệu ứng loading
        private int _dotCount = 0;

        // Service xử lý nghiệp vụ kết nối SQL Server Database & BMI
        private readonly ChatbotBLL _chatbotBll = new ChatbotBLL();

        /// <summary>
        /// Cho phép tầng ngoài (Form cha hoặc BLL) đăng ký hàm xử lý AI tùy biến (Gemini, OpenAI, Local LLM,...).
        /// Nếu không gán, hệ thống sẽ sử dụng bộ xử lý Gym AI kết nối Database mặc định.
        /// </summary>
        [System.ComponentModel.Browsable(false)]
        [System.ComponentModel.DesignerSerializationVisibility(System.ComponentModel.DesignerSerializationVisibility.Hidden)]
        public Func<string, Task<string>>? CustomAiHandler { get; set; }

        public ucChatbot()
        {
            InitializeComponent();

            // Bật DoubleBuffered cho container để cuộn mượt mà và chống giật nháy hình (flicker)
            EnableDoubleBuffering(pnlChatContainer);
        }

        private void ucChatbot_Load(object sender, EventArgs e)
        {
            // Định vị subtitle luôn cách nhãn trạng thái 8px, chống đè chữ ở mọi tỷ lệ DPI màn hình
            lblSubtitle.Left = lblStatus.Right + 8;

            // Hiển thị tin nhắn chào mừng ban đầu từ Gym AI
            DisplayInitialGreeting();

            // Focus vào ô nhập liệu để người dùng có thể gõ ngay
            txtMessageInput.Focus();
        }

        /// <summary>
        /// Lời chào mở đầu từ AI
        /// </summary>
        private void DisplayInitialGreeting()
        {
            string greeting = "Xin chào! 👋 Tôi là Trợ lý AI kết nối hệ thống dữ liệu QuanLyGym.\n" +
                              "Tôi có thể hỗ trợ bạn:\n" +
                              "• 📊 Tính chỉ số BMI & tư vấn thể trạng (ví dụ: 'tính bmi cao 1m70 nặng 65kg')\n" +
                              "• 🏋️ Tra cứu danh sách & thông tin Huấn luyện viên PT trong Database\n" +
                              "• 💳 Tra cứu bảng giá & chi tiết các gói tập (Packages)\n" +
                              "• 🏢 Xem địa chỉ chi nhánh, giờ mở cửa & máy tập phòng gym\n\n" +
                              "Hôm nay bạn cần tôi hỗ trợ điều gì?";
            AddMessage(greeting, isUser: false);
        }

        #region Xử lý Gửi Tin Nhắn

        private async void btnSend_Click(object sender, EventArgs e)
        {
            await ExecuteSendMessageAsync();
        }

        private async void txtMessageInput_KeyDown(object sender, KeyEventArgs e)
        {
            // Nhấn Enter mà không giữ phím Shift: Kích hoạt gửi tin nhắn
            if (e.KeyCode == Keys.Enter && !e.Shift)
            {
                e.SuppressKeyPress = true; // Ngăn chặn phát tiếng 'beep' và không tạo dòng mới
                await ExecuteSendMessageAsync();
            }
        }

        /// <summary>
        /// Luồng xử lý gửi tin nhắn chính (Bất đồng bộ - async/await)
        /// </summary>
        private async Task ExecuteSendMessageAsync()
        {
            string query = txtMessageInput.Text.Trim();

            // 1. Validate dữ liệu nhập vào: Không được rỗng hoặc chỉ toàn khoảng trắng
            if (string.IsNullOrWhiteSpace(query))
            {
                return;
            }

            // 2. Thêm tin nhắn của User vào khung chat ngay lập tức
            AddMessage(query, isUser: true);

            // 3. Xóa trắng ô TextBox nhập liệu
            txtMessageInput.Clear();

            // 4. Khóa tạm thời nút gửi và ô nhập liệu trong khi AI đang xử lý (ngăn spam)
            SetInputState(isProcessing: true);

            // 5. Bật trạng thái Loading "AI đang suy nghĩ..."
            SetLoadingState(true);

            // 6. Tự động cuộn xuống tin nhắn mới nhất
            ScrollToBottom();

            try
            {
                // 7. Gọi hàm xử lý AI bất đồng bộ (Non-blocking UI)
                string aiResponse;
                if (CustomAiHandler != null)
                {
                    aiResponse = await CustomAiHandler(query);
                }
                else
                {
                    aiResponse = await GetDefaultGymAiResponseAsync(query);
                }

                // 8. Tắt trạng thái Loading
                SetLoadingState(false);

                // 9. Thêm tin nhắn phản hồi của AI vào khung chat
                AddMessage(aiResponse, isUser: false);
            }
            catch (Exception ex)
            {
                SetLoadingState(false);
                AddMessage($"❌ Đã xảy ra lỗi khi kết nối với AI: {ex.Message}", isUser: false);
            }
            finally
            {
                // 10. Mở lại tương tác nhập liệu và cuộn xuống cuối
                SetInputState(isProcessing: false);
                ScrollToBottom();
                txtMessageInput.Focus();
            }
        }

        #endregion

        #region Hiển thị Bong bóng Chat (Message Bubble)

        /// <summary>
        /// Tạo và thêm một bong bóng tin nhắn vào khung chat
        /// </summary>
        /// <param name="message">Nội dung tin nhắn</param>
        /// <param name="isUser">True: Người dùng (Căn phải); False: AI (Căn trái)</param>
        public void AddMessage(string message, bool isUser)
        {
            pnlChatContainer.SuspendLayout();

            int containerWidth = pnlChatContainer.ClientSize.Width - 30;
            if (containerWidth < 250) containerWidth = 250;

            int maxBubbleWidth = (int)(containerWidth * 0.75); // Bong bóng chiếm tối đa 75% chiều rộng
            Font textFont = new Font("Segoe UI", 9.75F, FontStyle.Regular);
            Font metaFont = new Font("Segoe UI", 8.5F, FontStyle.Regular);

            // Đo kích thước thực tế của đoạn text
            Size proposedSize = new Size(maxBubbleWidth - 28, int.MaxValue);
            Size textSize = TextRenderer.MeasureText(
                message, 
                textFont, 
                proposedSize, 
                TextFormatFlags.WordBreak | TextFormatFlags.TextBoxControl);

            // Đo kích thước nhãn thời gian
            string timeText = DateTime.Now.ToString("HH:mm");
            Size timeSize = TextRenderer.MeasureText(timeText, metaFont);

            // Chiều rộng bong bóng có khoảng đệm thoáng đãng
            int contentWidth = Math.Max(textSize.Width + 28, timeSize.Width + 50);
            int bubbleWidth = Math.Max(contentWidth, 130);

            // Bố cục chiều dọc rõ ràng:
            int senderTop = 8;
            int senderHeight = 18;

            int textTop = senderTop + senderHeight + 2; // 28
            int textHeight = textSize.Height + 4;

            // Thời gian đặt bên dưới hẳn phần nội dung text, lề dưới dư 8px an toàn chống mất nét
            int timeTop = textTop + textHeight + 4;
            int timeHeight = timeSize.Height + 4;

            int bubbleHeight = timeTop + timeHeight + 8;

            // 1. Khung hàng chứa tin nhắn (Row Item Panel)
            Panel pnlRow = new Panel
            {
                Width = containerWidth,
                Height = bubbleHeight + 10,
                Location = new Point(14, _currentContentHeight),
                BackColor = Color.Transparent,
                Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right
            };

            // 2. Khung bong bóng tin nhắn (Bubble Panel)
            Panel pnlBubble = new Panel
            {
                Width = bubbleWidth,
                Height = bubbleHeight,
                BackColor = isUser ? Color.FromArgb(37, 99, 235) : Color.White
            };

            // Tô viền nhẹ cho bong bóng AI trên nền xám
            if (!isUser)
            {
                pnlBubble.Paint += (s, pe) =>
                {
                    using Pen pen = new Pen(Color.FromArgb(226, 232, 240), 1);
                    pe.Graphics.DrawRectangle(pen, 0, 0, pnlBubble.Width - 1, pnlBubble.Height - 1);
                };
            }

            // Đặt vị trí Căn lề: User căn phải, AI căn trái
            if (isUser)
            {
                pnlBubble.Location = new Point(containerWidth - bubbleWidth, 0);
            }
            else
            {
                pnlBubble.Location = new Point(0, 0);
            }

            // 3. Nhãn người gửi (Header nhỏ)
            Label lblSender = new Label
            {
                AutoSize = false,
                Text = isUser ? "Bạn" : "🤖 Gym AI Assistant",
                Font = new Font("Segoe UI", 8.25F, FontStyle.Bold),
                ForeColor = isUser ? Color.FromArgb(224, 242, 254) : Color.FromArgb(30, 41, 59),
                Location = new Point(12, senderTop),
                Size = new Size(bubbleWidth - 24, senderHeight),
                UseMnemonic = false
            };

            // 4. Nội dung tin nhắn
            Label lblText = new Label
            {
                Text = message,
                Font = textFont,
                ForeColor = isUser ? Color.White : Color.FromArgb(15, 23, 42),
                Location = new Point(12, textTop),
                Size = new Size(textSize.Width + 4, textHeight),
                AutoSize = false,
                UseMnemonic = false
            };

            // 5. Thời gian gửi (Timestamp) - Định dạng rõ nét, không bị mất nét đáy hay cạnh
            Label lblTime = new Label
            {
                Text = timeText,
                Font = metaFont,
                ForeColor = isUser ? Color.FromArgb(224, 242, 254) : Color.FromArgb(100, 116, 139),
                Location = new Point(bubbleWidth - timeSize.Width - 14, timeTop),
                Size = new Size(timeSize.Width + 4, timeHeight),
                AutoSize = false,
                TextAlign = ContentAlignment.MiddleRight,
                UseMnemonic = false
            };

            pnlBubble.Controls.Add(lblSender);
            pnlBubble.Controls.Add(lblText);
            pnlBubble.Controls.Add(lblTime);

            pnlRow.Controls.Add(pnlBubble);
            pnlChatContainer.Controls.Add(pnlRow);

            _currentContentHeight += pnlRow.Height + 8;

            pnlChatContainer.ResumeLayout();

            ScrollToBottom();
        }

        #endregion

        #region Trạng thái Loading & Khóa Điều Khiển

        /// <summary>
        /// Bật/tắt trạng thái hiển thị "AI đang suy nghĩ..."
        /// </summary>
        public void SetLoadingState(bool isLoading)
        {
            pnlLoadingIndicator.Visible = isLoading;

            if (isLoading)
            {
                _dotCount = 0;
                lblLoading.Text = "🤖 AI đang suy nghĩ.";
                timerLoadingDots.Start();
            }
            else
            {
                timerLoadingDots.Stop();
            }
        }

        private void timerLoadingDots_Tick(object sender, EventArgs e)
        {
            _dotCount = (_dotCount + 1) % 4;
            string dots = new string('.', _dotCount + 1);
            lblLoading.Text = $"🤖 AI đang suy nghĩ{dots}";
        }

        /// <summary>
        /// Khóa/mở các ô nhập liệu khi đang thực thi
        /// </summary>
        private void SetInputState(bool isProcessing)
        {
            btnSend.Enabled = !isProcessing;
            btnSend.BackColor = isProcessing ? Color.FromArgb(148, 163, 184) : Color.FromArgb(37, 99, 235);
            txtMessageInput.ReadOnly = isProcessing;
        }

        /// <summary>
        /// Tự động cuộn thanh cuộn xuống cuối cùng
        /// </summary>
        private void ScrollToBottom()
        {
            pnlChatContainer.VerticalScroll.Value = pnlChatContainer.VerticalScroll.Maximum;
            pnlChatContainer.PerformLayout();

            if (pnlChatContainer.Controls.Count > 0)
            {
                Control lastControl = pnlChatContainer.Controls[pnlChatContainer.Controls.Count - 1];
                pnlChatContainer.ScrollControlIntoView(lastControl);
            }
        }

        private void btnClearChat_Click(object sender, EventArgs e)
        {
            var confirm = MessageBox.Show(
                "Bạn có chắc chắn muốn làm mới toàn bộ cuộc trò chuyện này không?",
                "Xác nhận làm mới",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (confirm == DialogResult.Yes)
            {
                pnlChatContainer.Controls.Clear();
                _currentContentHeight = 12;
                DisplayInitialGreeting();
                txtMessageInput.Clear();
                txtMessageInput.Focus();
            }
        }

        private void pnlChatContainer_Resize(object sender, EventArgs e)
        {
            // Điều chỉnh lại độ rộng các hàng chat khi cửa sổ thay đổi kích thước
            int newWidth = pnlChatContainer.ClientSize.Width - 30;
            if (newWidth < 200) return;

            pnlChatContainer.SuspendLayout();
            foreach (Control ctrl in pnlChatContainer.Controls)
            {
                if (ctrl is Panel row)
                {
                    row.Width = newWidth;
                }
            }
            pnlChatContainer.ResumeLayout();
        }

        #endregion

        #region Xử lý logic qua tầng QuanLyGym.BLL & Database SQL Server

        /// <summary>
        /// Xử lý câu hỏi thông qua ChatbotBLL (kết nối trực tiếp với Database SQL Server & thuật toán BMI).
        /// Sử dụng async/await đảm bảo UI không bị giật lag.
        /// </summary>
        private async Task<string> GetDefaultGymAiResponseAsync(string prompt)
        {
            // Tạo độ trễ nhẹ (400ms - 800ms) để hiển thị animation "AI đang suy nghĩ..." mượt mà, tự nhiên
            await Task.Delay(500);

            // Chuyển toàn bộ câu hỏi sang tầng nghiệp vụ BLL để xử lý
            return await _chatbotBll.ProcessUserMessageAsync(prompt);
        }

        #endregion

        #region Tiện ích bổ trợ (Double Buffering)

        private static void EnableDoubleBuffering(Control control)
        {
            var property = typeof(Control).GetProperty(
                "DoubleBuffered",
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            property?.SetValue(control, true, null);
        }

        #endregion
    }
}
