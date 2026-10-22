namespace QuanLyGym.UI.Forms.Chat
{
    partial class ucChatbot
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            pnlHeader = new System.Windows.Forms.Panel();
            btnClearChat = new System.Windows.Forms.Button();
            lblStatus = new System.Windows.Forms.Label();
            lblSubtitle = new System.Windows.Forms.Label();
            lblTitle = new System.Windows.Forms.Label();
            pnlInputArea = new System.Windows.Forms.Panel();
            pnlInputBorder = new System.Windows.Forms.Panel();
            txtMessageInput = new System.Windows.Forms.TextBox();
            btnSend = new System.Windows.Forms.Button();
            pnlLoadingIndicator = new System.Windows.Forms.Panel();
            lblLoading = new System.Windows.Forms.Label();
            pnlChatContainer = new System.Windows.Forms.Panel();
            timerLoadingDots = new System.Windows.Forms.Timer(components);
            pnlHeader.SuspendLayout();
            pnlInputArea.SuspendLayout();
            pnlInputBorder.SuspendLayout();
            pnlLoadingIndicator.SuspendLayout();
            SuspendLayout();
            // 
            // pnlHeader
            // 
            pnlHeader.BackColor = System.Drawing.Color.FromArgb(30, 41, 59);
            pnlHeader.Controls.Add(btnClearChat);
            pnlHeader.Controls.Add(lblStatus);
            pnlHeader.Controls.Add(lblSubtitle);
            pnlHeader.Controls.Add(lblTitle);
            pnlHeader.Dock = System.Windows.Forms.DockStyle.Top;
            pnlHeader.Location = new System.Drawing.Point(0, 0);
            pnlHeader.Name = "pnlHeader";
            pnlHeader.Padding = new System.Windows.Forms.Padding(16, 12, 16, 12);
            pnlHeader.Size = new System.Drawing.Size(650, 75);
            pnlHeader.TabIndex = 0;
            // 
            // btnClearChat
            // 
            btnClearChat.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            btnClearChat.BackColor = System.Drawing.Color.FromArgb(51, 65, 85);
            btnClearChat.Cursor = System.Windows.Forms.Cursors.Hand;
            btnClearChat.FlatAppearance.BorderSize = 0;
            btnClearChat.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            btnClearChat.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            btnClearChat.ForeColor = System.Drawing.Color.White;
            btnClearChat.Location = new System.Drawing.Point(542, 20);
            btnClearChat.Name = "btnClearChat";
            btnClearChat.Size = new System.Drawing.Size(92, 34);
            btnClearChat.TabIndex = 3;
            btnClearChat.Text = "Làm mới";
            btnClearChat.UseVisualStyleBackColor = false;
            btnClearChat.Click += btnClearChat_Click;
            // 
            // lblTitle
            // 
            lblTitle.AutoSize = true;
            lblTitle.Font = new System.Drawing.Font("Segoe UI", 13F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            lblTitle.ForeColor = System.Drawing.Color.White;
            lblTitle.Location = new System.Drawing.Point(16, 12);
            lblTitle.Name = "lblTitle";
            lblTitle.Size = new System.Drawing.Size(205, 25);
            lblTitle.TabIndex = 0;
            lblTitle.Text = "🤖 Gym AI Assistant";
            lblTitle.UseMnemonic = false;
            // 
            // lblStatus
            // 
            lblStatus.AutoSize = true;
            lblStatus.Font = new System.Drawing.Font("Segoe UI", 8.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            lblStatus.ForeColor = System.Drawing.Color.FromArgb(52, 211, 153);
            lblStatus.Location = new System.Drawing.Point(18, 44);
            lblStatus.Name = "lblStatus";
            lblStatus.Size = new System.Drawing.Size(82, 15);
            lblStatus.TabIndex = 2;
            lblStatus.Text = "● Trực tuyến";
            lblStatus.UseMnemonic = false;
            // 
            // lblSubtitle
            // 
            lblSubtitle.AutoSize = true;
            lblSubtitle.Font = new System.Drawing.Font("Segoe UI", 8.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            lblSubtitle.ForeColor = System.Drawing.Color.FromArgb(148, 163, 184);
            lblSubtitle.Location = new System.Drawing.Point(105, 44);
            lblSubtitle.Name = "lblSubtitle";
            lblSubtitle.Size = new System.Drawing.Size(285, 15);
            lblSubtitle.TabIndex = 1;
            lblSubtitle.Text = "|   Tư vấn bài tập, dinh dưỡng & thông tin phòng tập";
            lblSubtitle.UseMnemonic = false;
            // 
            // pnlInputArea
            // 
            pnlInputArea.BackColor = System.Drawing.Color.White;
            pnlInputArea.Controls.Add(pnlInputBorder);
            pnlInputArea.Controls.Add(btnSend);
            pnlInputArea.Dock = System.Windows.Forms.DockStyle.Bottom;
            pnlInputArea.Location = new System.Drawing.Point(0, 520);
            pnlInputArea.Name = "pnlInputArea";
            pnlInputArea.Padding = new System.Windows.Forms.Padding(14, 10, 14, 12);
            pnlInputArea.Size = new System.Drawing.Size(650, 70);
            pnlInputArea.TabIndex = 2;
            // 
            // pnlInputBorder
            // 
            pnlInputBorder.BackColor = System.Drawing.Color.FromArgb(241, 245, 249);
            pnlInputBorder.Controls.Add(txtMessageInput);
            pnlInputBorder.Dock = System.Windows.Forms.DockStyle.Fill;
            pnlInputBorder.Location = new System.Drawing.Point(14, 10);
            pnlInputBorder.Name = "pnlInputBorder";
            pnlInputBorder.Padding = new System.Windows.Forms.Padding(10, 8, 10, 8);
            pnlInputBorder.Size = new System.Drawing.Size(526, 48);
            pnlInputBorder.TabIndex = 0;
            // 
            // txtMessageInput
            // 
            txtMessageInput.BackColor = System.Drawing.Color.FromArgb(241, 245, 249);
            txtMessageInput.BorderStyle = System.Windows.Forms.BorderStyle.None;
            txtMessageInput.Dock = System.Windows.Forms.DockStyle.Fill;
            txtMessageInput.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            txtMessageInput.ForeColor = System.Drawing.Color.FromArgb(30, 41, 59);
            txtMessageInput.Location = new System.Drawing.Point(10, 8);
            txtMessageInput.Multiline = true;
            txtMessageInput.Name = "txtMessageInput";
            txtMessageInput.PlaceholderText = "Nhập tin nhắn... (Nhấn Enter để gửi, Shift+Enter để xuống dòng)";
            txtMessageInput.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            txtMessageInput.Size = new System.Drawing.Size(506, 32);
            txtMessageInput.TabIndex = 0;
            txtMessageInput.KeyDown += txtMessageInput_KeyDown;
            // 
            // btnSend
            // 
            btnSend.BackColor = System.Drawing.Color.FromArgb(37, 99, 235);
            btnSend.Cursor = System.Windows.Forms.Cursors.Hand;
            btnSend.Dock = System.Windows.Forms.DockStyle.Right;
            btnSend.FlatAppearance.BorderSize = 0;
            btnSend.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            btnSend.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            btnSend.ForeColor = System.Drawing.Color.White;
            btnSend.Location = new System.Drawing.Point(546, 10);
            btnSend.Margin = new System.Windows.Forms.Padding(6, 0, 0, 0);
            btnSend.Name = "btnSend";
            btnSend.Size = new System.Drawing.Size(90, 48);
            btnSend.TabIndex = 1;
            btnSend.Text = "Gửi  ➤";
            btnSend.UseVisualStyleBackColor = false;
            btnSend.Click += btnSend_Click;
            // 
            // pnlLoadingIndicator
            // 
            pnlLoadingIndicator.BackColor = System.Drawing.Color.FromArgb(241, 245, 249);
            pnlLoadingIndicator.Controls.Add(lblLoading);
            pnlLoadingIndicator.Dock = System.Windows.Forms.DockStyle.Bottom;
            pnlLoadingIndicator.Location = new System.Drawing.Point(0, 488);
            pnlLoadingIndicator.Name = "pnlLoadingIndicator";
            pnlLoadingIndicator.Padding = new System.Windows.Forms.Padding(18, 6, 14, 6);
            pnlLoadingIndicator.Size = new System.Drawing.Size(650, 32);
            pnlLoadingIndicator.TabIndex = 3;
            pnlLoadingIndicator.Visible = false;
            // 
            // lblLoading
            // 
            lblLoading.AutoSize = true;
            lblLoading.Font = new System.Drawing.Font("Segoe UI Semibold", 9.5F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point);
            lblLoading.ForeColor = System.Drawing.Color.FromArgb(71, 85, 105);
            lblLoading.Location = new System.Drawing.Point(18, 6);
            lblLoading.Name = "lblLoading";
            lblLoading.Size = new System.Drawing.Size(161, 17);
            lblLoading.TabIndex = 0;
            lblLoading.Text = "🤖 AI đang suy nghĩ...";
            // 
            // pnlChatContainer
            // 
            pnlChatContainer.AutoScroll = true;
            pnlChatContainer.BackColor = System.Drawing.Color.FromArgb(248, 250, 252);
            pnlChatContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            pnlChatContainer.Location = new System.Drawing.Point(0, 75);
            pnlChatContainer.Name = "pnlChatContainer";
            pnlChatContainer.Padding = new System.Windows.Forms.Padding(14, 14, 14, 14);
            pnlChatContainer.Size = new System.Drawing.Size(650, 413);
            pnlChatContainer.TabIndex = 1;
            pnlChatContainer.Resize += pnlChatContainer_Resize;
            // 
            // timerLoadingDots
            // 
            timerLoadingDots.Interval = 350;
            timerLoadingDots.Tick += timerLoadingDots_Tick;
            // 
            // ucChatbot
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            BackColor = System.Drawing.Color.FromArgb(248, 250, 252);
            Controls.Add(pnlChatContainer);
            Controls.Add(pnlLoadingIndicator);
            Controls.Add(pnlInputArea);
            Controls.Add(pnlHeader);
            Name = "ucChatbot";
            Size = new System.Drawing.Size(650, 590);
            Load += ucChatbot_Load;
            pnlHeader.ResumeLayout(false);
            pnlHeader.PerformLayout();
            pnlInputArea.ResumeLayout(false);
            pnlInputBorder.ResumeLayout(false);
            pnlInputBorder.PerformLayout();
            pnlLoadingIndicator.ResumeLayout(false);
            pnlLoadingIndicator.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.Panel pnlHeader;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Label lblSubtitle;
        private System.Windows.Forms.Label lblStatus;
        private System.Windows.Forms.Button btnClearChat;
        private System.Windows.Forms.Panel pnlInputArea;
        private System.Windows.Forms.Panel pnlInputBorder;
        private System.Windows.Forms.TextBox txtMessageInput;
        private System.Windows.Forms.Button btnSend;
        private System.Windows.Forms.Panel pnlLoadingIndicator;
        private System.Windows.Forms.Label lblLoading;
        private System.Windows.Forms.Panel pnlChatContainer;
        private System.Windows.Forms.Timer timerLoadingDots;
    }
}
