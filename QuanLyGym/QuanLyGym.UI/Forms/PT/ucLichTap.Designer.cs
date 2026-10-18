namespace QuanLyGym.UI.Forms.PT
{
    partial class ucLichTap
    {
        private System.ComponentModel.IContainer components = null;

        private ucPTTopStats topStats;
        private System.Windows.Forms.Panel pnlContent;
        private System.Windows.Forms.Panel pnlRightContainer;
        private System.Windows.Forms.Panel pnlRightDetails;
        private System.Windows.Forms.Panel pnlLeftCalendar;
        private System.Windows.Forms.Panel pnlCalendarHeader;
        private System.Windows.Forms.Button btnPrevWeek;
        private System.Windows.Forms.Label lblWeekRange;
        private System.Windows.Forms.Button btnNextWeek;
        private System.Windows.Forms.Button btnWeekView;
        private System.Windows.Forms.Button btnDayView;
        private System.Windows.Forms.Panel pnlScheduleGrid;
        private System.Windows.Forms.FlowLayoutPanel pnlTodaySessionButtons;
        private System.Windows.Forms.Label lblTodaySessionsHeader;
        private System.Windows.Forms.Button btnCheckIn;
        private System.Windows.Forms.Button btnEditSchedule;
        private System.Windows.Forms.Button btnCancelSchedule;

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
            this.topStats = new QuanLyGym.UI.Forms.PT.ucPTTopStats();
            this.pnlContent = new System.Windows.Forms.Panel();
            this.pnlRightContainer = new System.Windows.Forms.Panel();
            this.pnlRightDetails = new System.Windows.Forms.Panel();
            this.btnCancelSchedule = new System.Windows.Forms.Button();
            this.btnEditSchedule = new System.Windows.Forms.Button();
            this.btnCheckIn = new System.Windows.Forms.Button();
            this.lblTodaySessionsHeader = new System.Windows.Forms.Label();
            this.pnlTodaySessionButtons = new System.Windows.Forms.FlowLayoutPanel();
            this.pnlLeftCalendar = new System.Windows.Forms.Panel();
            this.pnlScheduleGrid = new System.Windows.Forms.Panel();
            this.pnlCalendarHeader = new System.Windows.Forms.Panel();
            this.btnDayView = new System.Windows.Forms.Button();
            this.btnWeekView = new System.Windows.Forms.Button();
            this.btnNextWeek = new System.Windows.Forms.Button();
            this.lblWeekRange = new System.Windows.Forms.Label();
            this.btnPrevWeek = new System.Windows.Forms.Button();
            this.pnlContent.SuspendLayout();
            this.pnlRightContainer.SuspendLayout();
            this.pnlRightDetails.SuspendLayout();
            this.pnlLeftCalendar.SuspendLayout();
            this.pnlCalendarHeader.SuspendLayout();
            this.SuspendLayout();
            // 
            // topStats
            // 
            this.topStats.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(15)))), ((int)(((byte)(17)))), ((int)(((byte)(21)))));
            this.topStats.Dock = System.Windows.Forms.DockStyle.Top;
            this.topStats.Location = new System.Drawing.Point(0, 0);
            this.topStats.Name = "topStats";
            this.topStats.Size = new System.Drawing.Size(1200, 148);
            this.topStats.TabIndex = 0;
            // 
            // pnlContent
            // 
            this.pnlContent.Controls.Add(this.pnlLeftCalendar);
            this.pnlContent.Controls.Add(this.pnlRightContainer);
            this.pnlContent.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlContent.Location = new System.Drawing.Point(0, 148);
            this.pnlContent.Name = "pnlContent";
            this.pnlContent.Padding = new System.Windows.Forms.Padding(24, 0, 24, 20);
            this.pnlContent.Size = new System.Drawing.Size(1200, 552);
            this.pnlContent.TabIndex = 1;
            // 
            // pnlRightContainer
            // 
            this.pnlRightContainer.Controls.Add(this.pnlRightDetails);
            this.pnlRightContainer.Dock = System.Windows.Forms.DockStyle.Right;
            this.pnlRightContainer.Location = new System.Drawing.Point(776, 0);
            this.pnlRightContainer.Name = "pnlRightContainer";
            this.pnlRightContainer.Padding = new System.Windows.Forms.Padding(16, 0, 0, 0);
            this.pnlRightContainer.Size = new System.Drawing.Size(400, 570);
            this.pnlRightContainer.TabIndex = 1;
            // 
            // pnlRightDetails
            // 
            this.pnlRightDetails.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(15)))), ((int)(((byte)(17)))), ((int)(((byte)(21)))));
            this.pnlRightDetails.Controls.Add(this.btnCancelSchedule);
            this.pnlRightDetails.Controls.Add(this.btnEditSchedule);
            this.pnlRightDetails.Controls.Add(this.btnCheckIn);
            this.pnlRightDetails.Controls.Add(this.lblTodaySessionsHeader);
            this.pnlRightDetails.Controls.Add(this.pnlTodaySessionButtons);
            this.pnlRightDetails.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlRightDetails.Location = new System.Drawing.Point(16, 0);
            this.pnlRightDetails.Name = "pnlRightDetails";
            this.pnlRightDetails.Size = new System.Drawing.Size(384, 570);
            this.pnlRightDetails.TabIndex = 0;
            // 
            // btnCancelSchedule
            // 
            this.btnCancelSchedule.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(22)))), ((int)(((byte)(26)))));
            this.btnCancelSchedule.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnCancelSchedule.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(80)))), ((int)(((byte)(35)))), ((int)(((byte)(45)))));
            this.btnCancelSchedule.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCancelSchedule.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.btnCancelSchedule.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(244)))), ((int)(((byte)(63)))), ((int)(((byte)(94)))));
            this.btnCancelSchedule.Location = new System.Drawing.Point(196, 382);
            this.btnCancelSchedule.Name = "btnCancelSchedule";
            this.btnCancelSchedule.Size = new System.Drawing.Size(168, 36);
            this.btnCancelSchedule.TabIndex = 4;
            this.btnCancelSchedule.Text = "Hủy Ca Hẹn";
            this.btnCancelSchedule.UseMnemonic = false;
            this.btnCancelSchedule.UseVisualStyleBackColor = false;
            this.btnCancelSchedule.Click += new System.EventHandler(this.btnCancelSchedule_Click);
            // 
            // btnEditSchedule
            // 
            this.btnEditSchedule.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(28)))), ((int)(((byte)(32)))), ((int)(((byte)(44)))));
            this.btnEditSchedule.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnEditSchedule.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(52)))), ((int)(((byte)(70)))));
            this.btnEditSchedule.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnEditSchedule.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.btnEditSchedule.ForeColor = System.Drawing.Color.White;
            this.btnEditSchedule.Location = new System.Drawing.Point(20, 382);
            this.btnEditSchedule.Name = "btnEditSchedule";
            this.btnEditSchedule.Size = new System.Drawing.Size(168, 36);
            this.btnEditSchedule.TabIndex = 3;
            this.btnEditSchedule.Text = "Sửa Lịch";
            this.btnEditSchedule.UseMnemonic = false;
            this.btnEditSchedule.UseVisualStyleBackColor = false;
            this.btnEditSchedule.Click += new System.EventHandler(this.btnEditSchedule_Click);
            // 
            // btnCheckIn
            // 
            this.btnCheckIn.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(204)))), ((int)(((byte)(255)))), ((int)(((byte)(0)))));
            this.btnCheckIn.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnCheckIn.FlatAppearance.BorderSize = 0;
            this.btnCheckIn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCheckIn.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnCheckIn.ForeColor = System.Drawing.Color.Black;
            this.btnCheckIn.Location = new System.Drawing.Point(20, 335);
            this.btnCheckIn.Name = "btnCheckIn";
            this.btnCheckIn.Size = new System.Drawing.Size(344, 40);
            this.btnCheckIn.TabIndex = 2;
            this.btnCheckIn.Text = "Điểm Danh Hoàn Thành Buổi";
            this.btnCheckIn.UseMnemonic = false;
            this.btnCheckIn.UseVisualStyleBackColor = false;
            this.btnCheckIn.Click += new System.EventHandler(this.btnCheckIn_Click);
            // 
            // lblTodaySessionsHeader
            // 
            this.lblTodaySessionsHeader.AutoSize = true;
            this.lblTodaySessionsHeader.Font = new System.Drawing.Font("Segoe UI", 8.5F, System.Drawing.FontStyle.Bold);
            this.lblTodaySessionsHeader.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(142)))), ((int)(((byte)(149)))), ((int)(((byte)(165)))));
            this.lblTodaySessionsHeader.Location = new System.Drawing.Point(20, 430);
            this.lblTodaySessionsHeader.Name = "lblTodaySessionsHeader";
            this.lblTodaySessionsHeader.Size = new System.Drawing.Size(309, 15);
            this.lblTodaySessionsHeader.TabIndex = 5;
            this.lblTodaySessionsHeader.Text = "CÁC BUỔI TẬP HÔM NAY (ƯU TIÊN THEO KHUNG GIỜ):";
            this.lblTodaySessionsHeader.UseMnemonic = false;
            // 
            // pnlTodaySessionButtons
            // 
            this.pnlTodaySessionButtons.AutoScroll = true;
            this.pnlTodaySessionButtons.Location = new System.Drawing.Point(20, 452);
            this.pnlTodaySessionButtons.Name = "pnlTodaySessionButtons";
            this.pnlTodaySessionButtons.Size = new System.Drawing.Size(344, 105);
            this.pnlTodaySessionButtons.TabIndex = 6;
            // 
            // pnlLeftCalendar
            // 
            this.pnlLeftCalendar.Controls.Add(this.pnlScheduleGrid);
            this.pnlLeftCalendar.Controls.Add(this.pnlCalendarHeader);
            this.pnlLeftCalendar.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlLeftCalendar.Location = new System.Drawing.Point(24, 0);
            this.pnlLeftCalendar.Name = "pnlLeftCalendar";
            this.pnlLeftCalendar.Padding = new System.Windows.Forms.Padding(0, 0, 8, 0);
            this.pnlLeftCalendar.Size = new System.Drawing.Size(752, 570);
            this.pnlLeftCalendar.TabIndex = 0;
            // 
            // pnlScheduleGrid
            // 
            this.pnlScheduleGrid.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(15)))), ((int)(((byte)(17)))), ((int)(((byte)(21)))));
            this.pnlScheduleGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlScheduleGrid.Location = new System.Drawing.Point(0, 48);
            this.pnlScheduleGrid.Name = "pnlScheduleGrid";
            this.pnlScheduleGrid.Size = new System.Drawing.Size(744, 522);
            this.pnlScheduleGrid.TabIndex = 1;
            // 
            // pnlCalendarHeader
            // 
            this.pnlCalendarHeader.Controls.Add(this.btnDayView);
            this.pnlCalendarHeader.Controls.Add(this.btnWeekView);
            this.pnlCalendarHeader.Controls.Add(this.btnNextWeek);
            this.pnlCalendarHeader.Controls.Add(this.lblWeekRange);
            this.pnlCalendarHeader.Controls.Add(this.btnPrevWeek);
            this.pnlCalendarHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlCalendarHeader.Location = new System.Drawing.Point(0, 0);
            this.pnlCalendarHeader.Name = "pnlCalendarHeader";
            this.pnlCalendarHeader.Size = new System.Drawing.Size(744, 48);
            this.pnlCalendarHeader.TabIndex = 0;
            // 
            // btnDayView
            // 
            this.btnDayView.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnDayView.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(22)))), ((int)(((byte)(25)))), ((int)(((byte)(34)))));
            this.btnDayView.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnDayView.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(50)))), ((int)(((byte)(65)))));
            this.btnDayView.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnDayView.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.btnDayView.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(142)))), ((int)(((byte)(149)))), ((int)(((byte)(165)))));
            this.btnDayView.Location = new System.Drawing.Point(648, 8);
            this.btnDayView.Name = "btnDayView";
            this.btnDayView.Size = new System.Drawing.Size(90, 32);
            this.btnDayView.TabIndex = 4;
            this.btnDayView.Text = "Theo Ngày";
            this.btnDayView.UseMnemonic = false;
            this.btnDayView.UseVisualStyleBackColor = false;
            // 
            // btnWeekView
            // 
            this.btnWeekView.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnWeekView.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(34)))), ((int)(((byte)(45)))), ((int)(((byte)(22)))));
            this.btnWeekView.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnWeekView.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(204)))), ((int)(((byte)(255)))), ((int)(((byte)(0)))));
            this.btnWeekView.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnWeekView.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.btnWeekView.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(204)))), ((int)(((byte)(255)))), ((int)(((byte)(0)))));
            this.btnWeekView.Location = new System.Drawing.Point(552, 8);
            this.btnWeekView.Name = "btnWeekView";
            this.btnWeekView.Size = new System.Drawing.Size(90, 32);
            this.btnWeekView.TabIndex = 3;
            this.btnWeekView.Text = "Theo Tuần";
            this.btnWeekView.UseMnemonic = false;
            this.btnWeekView.UseVisualStyleBackColor = false;
            // 
            // btnNextWeek
            // 
            this.btnNextWeek.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(22)))), ((int)(((byte)(25)))), ((int)(((byte)(34)))));
            this.btnNextWeek.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnNextWeek.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(50)))), ((int)(((byte)(65)))));
            this.btnNextWeek.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnNextWeek.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.btnNextWeek.ForeColor = System.Drawing.Color.White;
            this.btnNextWeek.Location = new System.Drawing.Point(294, 8);
            this.btnNextWeek.Name = "btnNextWeek";
            this.btnNextWeek.Size = new System.Drawing.Size(32, 32);
            this.btnNextWeek.TabIndex = 2;
            this.btnNextWeek.Text = ">";
            this.btnNextWeek.UseMnemonic = false;
            this.btnNextWeek.UseVisualStyleBackColor = false;
            // 
            // lblWeekRange
            // 
            this.lblWeekRange.AutoSize = true;
            this.lblWeekRange.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.lblWeekRange.ForeColor = System.Drawing.Color.White;
            this.lblWeekRange.Location = new System.Drawing.Point(44, 14);
            this.lblWeekRange.Name = "lblWeekRange";
            this.lblWeekRange.Size = new System.Drawing.Size(239, 19);
            this.lblWeekRange.TabIndex = 1;
            this.lblWeekRange.Text = "05/10/2026 – 11/10/2026 (Tuần 41)";
            this.lblWeekRange.UseMnemonic = false;
            // 
            // btnPrevWeek
            // 
            this.btnPrevWeek.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(22)))), ((int)(((byte)(25)))), ((int)(((byte)(34)))));
            this.btnPrevWeek.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnPrevWeek.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(50)))), ((int)(((byte)(65)))));
            this.btnPrevWeek.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnPrevWeek.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.btnPrevWeek.ForeColor = System.Drawing.Color.White;
            this.btnPrevWeek.Location = new System.Drawing.Point(4, 8);
            this.btnPrevWeek.Name = "btnPrevWeek";
            this.btnPrevWeek.Size = new System.Drawing.Size(32, 32);
            this.btnPrevWeek.TabIndex = 0;
            this.btnPrevWeek.Text = "<";
            this.btnPrevWeek.UseMnemonic = false;
            this.btnPrevWeek.UseVisualStyleBackColor = false;
            // 
            // ucLichTap
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(15)))), ((int)(((byte)(17)))), ((int)(((byte)(21)))));
            this.Controls.Add(this.pnlContent);
            this.Controls.Add(this.topStats);
            this.Name = "ucLichTap";
            this.Size = new System.Drawing.Size(1200, 700);
            this.pnlContent.ResumeLayout(false);
            this.pnlRightContainer.ResumeLayout(false);
            this.pnlRightDetails.ResumeLayout(false);
            this.pnlRightDetails.PerformLayout();
            this.pnlLeftCalendar.ResumeLayout(false);
            this.pnlCalendarHeader.ResumeLayout(false);
            this.pnlCalendarHeader.PerformLayout();
            this.ResumeLayout(false);
        }
    }
}
