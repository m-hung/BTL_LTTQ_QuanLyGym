using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using QuanLyGym.BLL.Models;
using QuanLyGym.BLL.Services;

namespace QuanLyGym.UI.Forms.Admin
{
    public class frmScheduleBooking : Form
    {
        [DllImport("user32.dll")]
        public static extern bool ReleaseCapture();
        [DllImport("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);
        private const int WM_NCLBUTTONDOWN = 0xA1;
        private const int HTCAPTION = 0x2;

        private static readonly Color BgColor = Color.FromArgb(15, 15, 17);
        private static readonly Color CardBg = Color.FromArgb(24, 24, 27);
        private static readonly Color ItemBg = Color.FromArgb(20, 20, 23);
        private static readonly Color BorderColor = Color.FromArgb(45, 45, 50);
        private static readonly Color TextColor = Color.FromArgb(244, 244, 245);
        private static readonly Color MutedColor = Color.FromArgb(161, 161, 170);
        private static readonly Color Orange = Color.FromArgb(249, 115, 22);
        private static readonly Color Cyan = Color.FromArgb(6, 182, 212);
        private static readonly Color Green = Color.FromArgb(34, 197, 94);
        private static readonly Color Red = Color.FromArgb(239, 68, 68);

        private readonly ScheduleDto? _original;
        public ScheduleDto ResultSchedule { get; private set; } = new();

        private ComboBox _cboMember = null!;
        private ComboBox _cboPT = null!;
        private DateTimePicker _dtpDate = null!;
        private ComboBox _cboTimeSlot = null!;
        private ComboBox _cboRoom = null!;
        private TextBox _txtWorkout = null!;
        private ComboBox _cboStatus = null!;
        private TextBox _txtNote = null!;

        private List<MemberLookupDto> _members = new();
        private List<PTLookupDto> _pts = new();
        private List<RoomLookupDto> _rooms = new();

        // Danh sách preset khung giờ chuẩn
        private readonly string[] _presetSlots = new[]
        {
            "07:00 - 08:30 (Sáng sớm)",
            "08:30 - 10:00 (Buổi sáng)",
            "10:00 - 11:30 (Trưa)",
            "14:00 - 15:30 (Đầu giờ chiều)",
            "15:30 - 17:00 (Chiều)",
            "17:30 - 19:00 (Tan sở)",
            "18:30 - 20:00 (Tối cao điểm)",
            "20:00 - 21:30 (Đêm muộn)"
        };

        public frmScheduleBooking(ScheduleDto? initial = null)
        {
            _original = initial;
            if (initial != null)
            {
                ResultSchedule = new ScheduleDto
                {
                    ScheduleID = initial.ScheduleID,
                    MemberID = initial.MemberID,
                    MemberName = initial.MemberName,
                    MemberPhone = initial.MemberPhone,
                    PTID = initial.PTID,
                    PTName = initial.PTName,
                    PTSpecialization = initial.PTSpecialization,
                    RoomID = initial.RoomID,
                    RoomName = initial.RoomName,
                    BranchID = initial.BranchID,
                    BranchName = initial.BranchName,
                    TrainingDate = initial.TrainingDate,
                    StartTime = initial.StartTime,
                    EndTime = initial.EndTime,
                    TrainingType = initial.TrainingType,
                    Status = initial.Status,
                    Note = initial.Note
                };
            }
            else
            {
                ResultSchedule = new ScheduleDto
                {
                    TrainingDate = DateTime.Today,
                    StartTime = new TimeSpan(7, 0, 0),
                    EndTime = new TimeSpan(8, 30, 0),
                    TrainingType = "Overhead Press & Pull-up",
                    Status = "Chờ duyệt",
                    Note = "Hệ thống tự động xác nhận hoàn thành sau 24h"
                };
            }

            LoadLookups();
            BuildUI();
            PopulateData();
        }

        private void LoadLookups()
        {
            try
            {
                _members = ScheduleService.Instance.GetMembersForBooking();
                _pts = ScheduleService.Instance.GetPTsForBooking();
                _rooms = ScheduleService.Instance.GetRoomsForBooking();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[frmScheduleBooking.LoadLookups] Error: {ex.Message}");
            }
        }

        private void BuildUI()
        {
            FormBorderStyle = FormBorderStyle.None;
            StartPosition = FormStartPosition.CenterParent;
            Size = new Size(680, 710);
            BackColor = BgColor;
            Padding = new Padding(2);

            var pnlContainer = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = CardBg,
                Padding = new Padding(24)
            };
            pnlContainer.Paint += (s, e) =>
            {
                using var p = new Pen(BorderColor, 1.5f);
                e.Graphics.DrawRectangle(p, 0, 0, pnlContainer.Width - 1, pnlContainer.Height - 1);
            };
            Controls.Add(pnlContainer);

            // Title Bar
            var pnlTitle = new Panel
            {
                Dock = DockStyle.Top,
                Height = 44,
                BackColor = Color.Transparent
            };
            pnlTitle.MouseDown += (s, e) =>
            {
                if (e.Button == MouseButtons.Left)
                {
                    ReleaseCapture();
                    SendMessage(Handle, WM_NCLBUTTONDOWN, HTCAPTION, 0);
                }
            };

            bool isEdit = _original != null && _original.ScheduleID > 0;
            var lblTitle = new Label
            {
                Text = isEdit ? $"📝 CẬP NHẬT LỊCH HẸN PT (#{_original!.ScheduleID})" : "🏋 ĐẶT LỊCH TẬP 1-1 VỚI HUẤN LUYỆN VIÊN (PT)",
                Font = new Font("Segoe UI", 11F, FontStyle.Bold),
                ForeColor = Orange,
                Dock = DockStyle.Left,
                AutoSize = true,
                TextAlign = ContentAlignment.MiddleLeft
            };
            pnlTitle.Controls.Add(lblTitle);

            var btnClose = new Button
            {
                Text = "✕",
                Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                ForeColor = MutedColor,
                BackColor = Color.Transparent,
                FlatStyle = FlatStyle.Flat,
                Size = new Size(32, 32),
                Dock = DockStyle.Right,
                Cursor = Cursors.Hand
            };
            btnClose.FlatAppearance.BorderSize = 0;
            btnClose.FlatAppearance.MouseOverBackColor = Color.FromArgb(39, 39, 42);
            btnClose.Click += (s, e) => { DialogResult = DialogResult.Cancel; Close(); };
            pnlTitle.Controls.Add(btnClose);
            pnlContainer.Controls.Add(pnlTitle);

            // Body
            var pnlBody = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.Transparent,
                Padding = new Padding(0, 10, 0, 0)
            };
            pnlContainer.Controls.Add(pnlBody);
            pnlBody.BringToFront();

            int y = 5;

            // Row 1: Chọn Hội viên
            AddLabel(pnlBody, "Hội viên tham gia tập (*):", 0, y);
            y += 22;

            _cboMember = new ComboBox
            {
                Location = new Point(0, y),
                Size = new Size(630, 32),
                BackColor = ItemBg,
                ForeColor = TextColor,
                Font = new Font("Segoe UI", 9.5F),
                DropDownStyle = ComboBoxStyle.DropDownList,
                FlatStyle = FlatStyle.Flat
            };
            foreach (var m in _members)
            {
                _cboMember.Items.Add(m.DisplayText);
            }
            if (_cboMember.Items.Count > 0) _cboMember.SelectedIndex = 0;
            pnlBody.Controls.Add(_cboMember);
            y += 48;

            // Row 2: Chọn Huấn luyện viên PT
            AddLabel(pnlBody, "Huấn luyện viên phụ trách (PT) (*):", 0, y);
            y += 22;

            _cboPT = new ComboBox
            {
                Location = new Point(0, y),
                Size = new Size(630, 32),
                BackColor = ItemBg,
                ForeColor = Cyan,
                Font = new Font("Segoe UI", 9.5F, FontStyle.Bold),
                DropDownStyle = ComboBoxStyle.DropDownList,
                FlatStyle = FlatStyle.Flat
            };
            foreach (var pt in _pts)
            {
                _cboPT.Items.Add(pt.DisplayText);
            }
            if (_cboPT.Items.Count > 0) _cboPT.SelectedIndex = 0;
            pnlBody.Controls.Add(_cboPT);
            y += 48;

            // Row 3: Ngày tập & Khung giờ
            AddLabel(pnlBody, "Ngày tập (*):", 0, y);
            AddLabel(pnlBody, "Khung giờ tập (1-1 Slot) (*):", 320, y);
            y += 22;

            _dtpDate = new DateTimePicker
            {
                Location = new Point(0, y),
                Size = new Size(300, 32),
                Font = new Font("Segoe UI", 9.5F),
                Format = DateTimePickerFormat.Custom,
                CustomFormat = "dd/MM/yyyy (dddd)",
                BackColor = ItemBg,
                ForeColor = TextColor
            };
            pnlBody.Controls.Add(_dtpDate);

            _cboTimeSlot = new ComboBox
            {
                Location = new Point(320, y),
                Size = new Size(310, 32),
                BackColor = ItemBg,
                ForeColor = TextColor,
                Font = new Font("Segoe UI", 9.5F),
                DropDownStyle = ComboBoxStyle.DropDownList,
                FlatStyle = FlatStyle.Flat
            };
            _cboTimeSlot.Items.AddRange(_presetSlots);
            _cboTimeSlot.SelectedIndex = 0;
            pnlBody.Controls.Add(_cboTimeSlot);
            y += 48;

            // Row 4: Khu vực / Phòng tập & Trạng thái
            AddLabel(pnlBody, "Khu vực / Phòng tập:", 0, y);
            AddLabel(pnlBody, "Trạng thái lịch hẹn (*):", 320, y);
            y += 22;

            _cboRoom = new ComboBox
            {
                Location = new Point(0, y),
                Size = new Size(300, 32),
                BackColor = ItemBg,
                ForeColor = TextColor,
                Font = new Font("Segoe UI", 9.5F),
                DropDownStyle = ComboBoxStyle.DropDownList,
                FlatStyle = FlatStyle.Flat
            };
            foreach (var r in _rooms)
            {
                _cboRoom.Items.Add(r.DisplayText);
            }
            if (_cboRoom.Items.Count > 0) _cboRoom.SelectedIndex = 0;
            pnlBody.Controls.Add(_cboRoom);

            _cboStatus = new ComboBox
            {
                Location = new Point(320, y),
                Size = new Size(310, 32),
                BackColor = ItemBg,
                ForeColor = TextColor,
                Font = new Font("Segoe UI", 9.5F),
                DropDownStyle = ComboBoxStyle.DropDownList,
                FlatStyle = FlatStyle.Flat
            };
            _cboStatus.Items.AddRange(new object[]
            {
                "Chờ duyệt",
                "Đã duyệt",
                "Hoàn thành",
                "Đã hủy"
            });
            _cboStatus.SelectedIndex = 0;
            pnlBody.Controls.Add(_cboStatus);
            y += 48;

            // Row 5: Nội dung bài tập / Giáo án tập luyện
            AddLabel(pnlBody, "Nội dung bài tập / Giáo án huấn luyện (*):", 0, y);
            y += 22;

            _txtWorkout = CreateTextBox(0, y, 630);
            _txtWorkout.Text = "Overhead Press & Pull-up";
            pnlBody.Controls.Add(_txtWorkout);
            y += 48;

            // Row 6: Ghi chú
            AddLabel(pnlBody, "Ghi chú & Trạng thái hệ thống:", 0, y);
            y += 22;

            _txtNote = new TextBox
            {
                Location = new Point(0, y),
                Size = new Size(630, 68),
                Multiline = true,
                BackColor = ItemBg,
                ForeColor = TextColor,
                Font = new Font("Segoe UI", 9.5F),
                BorderStyle = BorderStyle.FixedSingle,
                ScrollBars = ScrollBars.Vertical
            };
            _txtNote.Text = "Hệ thống tự động xác nhận hoàn thành sau 24h";
            pnlBody.Controls.Add(_txtNote);
            y += 88;

            // Buttons Bar
            var pnlButtons = new Panel
            {
                Dock = DockStyle.Bottom,
                Height = 52,
                BackColor = Color.Transparent
            };
            pnlContainer.Controls.Add(pnlButtons);

            var btnCancel = new Button
            {
                Text = "Hủy bỏ",
                Font = new Font("Segoe UI", 9.5F, FontStyle.Bold),
                ForeColor = MutedColor,
                BackColor = Color.FromArgb(39, 39, 42),
                FlatStyle = FlatStyle.Flat,
                Size = new Size(110, 42),
                Location = new Point(390, 5),
                Cursor = Cursors.Hand
            };
            btnCancel.FlatAppearance.BorderSize = 0;
            btnCancel.Click += (s, e) => { DialogResult = DialogResult.Cancel; Close(); };
            pnlButtons.Controls.Add(btnCancel);

            var btnSave = new Button
            {
                Text = isEdit ? "💾 Cập nhật" : "➕ Đặt lịch ngay",
                Font = new Font("Segoe UI", 9.5F, FontStyle.Bold),
                ForeColor = Color.White,
                BackColor = Orange,
                FlatStyle = FlatStyle.Flat,
                Size = new Size(140, 42),
                Location = new Point(510, 5),
                Cursor = Cursors.Hand
            };
            btnSave.FlatAppearance.BorderSize = 0;
            btnSave.Click += BtnSave_Click;
            pnlButtons.Controls.Add(btnSave);
        }

        private void PopulateData()
        {
            if (_original == null) return;

            // Chọn Hội viên
            for (int i = 0; i < _members.Count; i++)
            {
                if (_members[i].MemberID == _original.MemberID)
                {
                    _cboMember.SelectedIndex = i;
                    break;
                }
            }

            // Chọn PT
            for (int i = 0; i < _pts.Count; i++)
            {
                if (_pts[i].PTID == _original.PTID)
                {
                    _cboPT.SelectedIndex = i;
                    break;
                }
            }

            // Chọn Phòng tập
            for (int i = 0; i < _rooms.Count; i++)
            {
                if (_rooms[i].RoomID == _original.RoomID)
                {
                    _cboRoom.SelectedIndex = i;
                    break;
                }
            }

            // Ngày tập
            _dtpDate.Value = _original.TrainingDate;

            // Khung giờ
            string slotStr = $"{_original.StartTime:hh\\:mm} - {_original.EndTime:hh\\:mm}";
            int slotIdx = -1;
            for (int i = 0; i < _presetSlots.Length; i++)
            {
                if (_presetSlots[i].StartsWith(slotStr))
                {
                    slotIdx = i;
                    break;
                }
            }
            if (slotIdx >= 0) _cboTimeSlot.SelectedIndex = slotIdx;

            // Trạng thái
            int stIdx = _cboStatus.FindStringExact(_original.Status);
            if (stIdx >= 0) _cboStatus.SelectedIndex = stIdx;

            _txtWorkout.Text = _original.TrainingType;
            _txtNote.Text = _original.Note;
        }

        private void BtnSave_Click(object? sender, EventArgs e)
        {
            if (_cboMember.SelectedIndex < 0 || _members.Count == 0)
            {
                MessageBox.Show("Vui lòng chọn hội viên tham gia buổi tập.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (_cboPT.SelectedIndex < 0 || _pts.Count == 0)
            {
                MessageBox.Show("Vui lòng chọn huấn luyện viên (PT).", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (string.IsNullOrWhiteSpace(_txtWorkout.Text))
            {
                MessageBox.Show("Vui lòng nhập nội dung bài tập / giáo án tập luyện.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                _txtWorkout.Focus();
                return;
            }

            var selectedMember = _members[_cboMember.SelectedIndex];
            var selectedPT = _pts[_cboPT.SelectedIndex];
            var selectedRoom = _rooms.Count > 0 && _cboRoom.SelectedIndex >= 0 ? _rooms[_cboRoom.SelectedIndex] : null;

            // Parse khung giờ
            ParseTimeSlot(_cboTimeSlot.SelectedItem?.ToString() ?? "", out TimeSpan start, out TimeSpan end);

            ResultSchedule.MemberID = selectedMember.MemberID;
            ResultSchedule.MemberName = selectedMember.FullName;
            ResultSchedule.MemberPhone = selectedMember.Phone;

            ResultSchedule.PTID = selectedPT.PTID;
            ResultSchedule.PTName = selectedPT.FullName;
            ResultSchedule.PTSpecialization = selectedPT.Specialization;

            ResultSchedule.RoomID = selectedRoom?.RoomID ?? 1;
            ResultSchedule.RoomName = selectedRoom?.RoomName ?? "Phòng PT 1-1";
            ResultSchedule.BranchID = selectedRoom?.BranchID;
            ResultSchedule.BranchName = selectedRoom?.BranchName ?? "Trụ sở chính";

            ResultSchedule.TrainingDate = _dtpDate.Value.Date;
            ResultSchedule.StartTime = start;
            ResultSchedule.EndTime = end;
            ResultSchedule.TrainingType = _txtWorkout.Text.Trim();
            ResultSchedule.Status = _cboStatus.SelectedItem?.ToString() ?? "Chờ duyệt";
            ResultSchedule.Note = _txtNote.Text.Trim();

            DialogResult = DialogResult.OK;
            Close();
        }

        private void ParseTimeSlot(string slotText, out TimeSpan start, out TimeSpan end)
        {
            // Default: 07:00 - 08:30
            start = new TimeSpan(7, 0, 0);
            end = new TimeSpan(8, 30, 0);

            try
            {
                if (slotText.Contains("-"))
                {
                    var parts = slotText.Split('-');
                    var startStr = parts[0].Trim();
                    var endPart = parts[1].Trim();
                    var endStr = endPart.Split(' ')[0].Trim();

                    if (TimeSpan.TryParse(startStr, out var s)) start = s;
                    if (TimeSpan.TryParse(endStr, out var e)) end = e;
                }
            }
            catch
            {
                // Fallback default
            }
        }

        private static void AddLabel(Panel parent, string text, int x, int y)
        {
            var lbl = new Label
            {
                Text = text,
                Location = new Point(x, y),
                AutoSize = true,
                Font = new Font("Segoe UI", 9F, FontStyle.Regular),
                ForeColor = MutedColor
            };
            parent.Controls.Add(lbl);
        }

        private static TextBox CreateTextBox(int x, int y, int width)
        {
            return new TextBox
            {
                Location = new Point(x, y),
                Size = new Size(width, 32),
                BackColor = ItemBg,
                ForeColor = TextColor,
                Font = new Font("Segoe UI", 9.5F),
                BorderStyle = BorderStyle.FixedSingle
            };
        }
    }
}
