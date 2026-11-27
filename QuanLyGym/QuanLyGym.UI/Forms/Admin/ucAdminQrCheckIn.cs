using QuanLyGym.UI.Utils;
using QuanLyGym.UI.Helpers;
using QuanLyGym.BLL.Services;

namespace QuanLyGym.UI.Forms.Admin
{
    internal sealed class ucAdminQrCheckIn : UserControl
    {
        private static readonly Color Surface = Color.FromArgb(24, 24, 27);
        private static readonly Color Input = Color.FromArgb(20, 20, 23);
        private static readonly Color Border = Color.FromArgb(45, 45, 50);
        private static readonly Color TextColor = Color.FromArgb(244, 244, 245);
        private static readonly Color Muted = Color.FromArgb(161, 161, 170);
        private static readonly Color Accent = Color.FromArgb(249, 115, 22);
        private static readonly Color Success = Color.FromArgb(74, 222, 128);
        private static readonly Color Warning = Color.FromArgb(251, 191, 36);
        private static readonly Color Error = Color.FromArgb(248, 113, 113);


        private readonly QrCheckInService _service = new();
        private List<QrCheckInVisit> _visits = new();
        private readonly ComboBox _branch = new() { DropDownStyle = ComboBoxStyle.DropDownList, Width = 235 };
        private readonly Button _reload = new() { Text = "Làm mới", AutoSize = true };
        private readonly Button _submit = new() { Text = "Check-in", Dock = DockStyle.Fill, Margin = new Padding(0, 0, 0, 4) };
        private bool _busy;
        private bool _loadingBranches;
        private int _historyVersion;
        private int? _historyBranchId;
        private readonly TextBox _code = new() { PlaceholderText = "Quét hoặc nhập mã trên thẻ tập", MaxLength = 80 };
        private readonly TextBox _search = new() { PlaceholderText = "Tìm tên hoặc mã thẻ…" };
        private readonly Label _result = LabelFor("Sẵn sàng tiếp nhận hội viên", UITheme.FontTitle);
        private readonly Label _details = LabelFor("Đưa mã QR vào khung camera hoặc nhập mã thẻ bên trái.", UITheme.FontRegular);
        private readonly Label _count = LabelFor("", UITheme.FontSmallBold);
        private readonly Label _empty = LabelFor("", UITheme.FontRegular);
        private readonly DataGridView _history = new();
        private readonly Panel _cameraHost = new() { Dock = DockStyle.Fill, BackColor = Surface, Margin = Padding.Empty };
        private QrCameraView? _camera;

        public ucAdminQrCheckIn()
        {
            BackColor = Color.FromArgb(10, 10, 12);
            ForeColor = TextColor;
            _branch.FlatStyle = FlatStyle.Flat;
            _branch.BackColor = Input;
            _branch.ForeColor = TextColor;
            _code.BackColor = Input;
            _code.ForeColor = TextColor;
            _search.BackColor = Input;
            _search.ForeColor = TextColor;
            StyleButton(_reload);
            Padding = new Padding(20, 14, 20, 18);
            AutoScroll = true;
            AutoScrollMinSize = new Size(950, 717);

            var root = new TableLayoutPanel { Dock = DockStyle.Top, Height = 682, ColumnCount = 2, RowCount = 2, Margin = Padding.Empty };
            root.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 43));
            root.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 57));
            root.RowStyles.Add(new RowStyle(SizeType.Absolute, 94));
            root.RowStyles.Add(new RowStyle(SizeType.Percent, 100));

            var heading = new TableLayoutPanel { Dock = DockStyle.Fill, ColumnCount = 1, RowCount = 3, Margin = Padding.Empty };
            heading.RowStyles.Add(new RowStyle(SizeType.Absolute, 33));
            heading.RowStyles.Add(new RowStyle(SizeType.Absolute, 24));
            heading.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
            heading.Controls.Add(LabelFor("Check-in QR tại quầy", UITheme.FontTitle));
            var description = LabelFor("Quét thẻ hội viên và ghi nhận lượt vào phòng tập.", UITheme.FontRegular);
            description.ForeColor = Muted;
            heading.Controls.Add(description, 0, 1);
            var branchBar = new FlowLayoutPanel { AutoSize = true, Dock = DockStyle.Fill, WrapContents = false };
            branchBar.Controls.Add(new Label { Text = "Chi nhánh tại quầy", ForeColor = Muted, AutoSize = true, Margin = new Padding(0, 7, 8, 0) });
            branchBar.Controls.Add(_branch);
            branchBar.Controls.Add(_reload);
            heading.Controls.Add(branchBar, 0, 2);
            _branch.SelectedIndexChanged += async (_, _) => await RefreshBranchHistoryAsync();
            _reload.Click += async (_, _) =>
            {
                if (_branch.Items.Count == 0) await LoadBranchesAsync();
                else await RefreshBranchHistoryAsync();
            };
            root.Controls.Add(heading, 0, 0);
            root.SetColumnSpan(heading, 2);

            var left = Column(new Padding(0, 0, 8, 0), 409);
            var right = Column(new Padding(8, 0, 0, 0), 255);
            root.Controls.Add(left, 0, 1);
            root.Controls.Add(right, 1, 1);
            var zoom = new Button { Text = "Phóng to camera", Dock = DockStyle.Fill, Margin = new Padding(0, 2, 0, 4) };
            StyleButton(zoom);
            left.Controls.Add(Card("Quét mã bằng camera", _cameraHost, zoom), 0, 0);
            var inputCard = Card("Nhập mã thẻ hoặc quét bằng đầu đọc USB", CreateInput());
            left.Controls.Add(inputCard, 0, 1);
            right.Controls.Add(Card("Kết quả check-in", CreateResult()), 0, 0);
            _search.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            _search.Margin = new Padding(8, 0, 0, 0);
            _search.Font = UITheme.FontSection;
            _search.AccessibleName = "Tìm hội viên trong lịch sử check-in";
            _search.TextChanged += (_, _) => RefreshHistory();
            var historyCard = Card("Lịch sử check-in hôm nay", CreateHistory(), _search, headerActionWidth: 250);
            right.Controls.Add(historyCard, 0, 1);

            bool expanded = false;
            zoom.Click += (_, _) =>
            {
                expanded = !expanded;
                root.SuspendLayout();
                left.SuspendLayout();
                right.SuspendLayout();
                root.ColumnStyles[0].Width = expanded ? 70 : 43;
                root.ColumnStyles[1].Width = expanded ? 30 : 57;
                left.RowStyles[0].SizeType = expanded ? SizeType.Percent : SizeType.Absolute;
                left.RowStyles[0].Height = expanded ? 100 : 409;
                left.RowStyles[1].Height = expanded ? 0 : 100;
                inputCard.Visible = !expanded;
                historyCard.Visible = !expanded;
                zoom.Text = expanded ? "Thu nhỏ camera" : "Phóng to camera";
                right.ResumeLayout();
                left.ResumeLayout();
                root.ResumeLayout();
            };
            Controls.Add(root);
            RefreshHistory();
        }

        protected override async void OnVisibleChanged(EventArgs e)
        {
            base.OnVisibleChanged(e);
            if (!Visible)
            {
                _camera?.ResetCamera();
                return;
            }
            if (DesignMode) return;

            if (_camera == null)
            {
                _camera = new QrCameraView { Dock = DockStyle.Fill };
                _camera.CodeRead += async code =>
                {
                    if (_busy || !Visible) return;
                    _code.Text = code;
                    await ProcessCheckInAsync(fromCamera: true);
                };
                _cameraHost.Controls.Add(_camera);
            }

            if (_branch.Items.Count == 0) await LoadBranchesAsync();
            else await RefreshBranchHistoryAsync();
        }

        private Control CreateInput()
        {
            var layout = new TableLayoutPanel { Dock = DockStyle.Fill, ColumnCount = 2, RowCount = 3, Margin = Padding.Empty };
            layout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
            layout.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 112));
            layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 26));
            layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 38));
            layout.RowStyles.Add(new RowStyle(SizeType.Percent, 100));

            var label = LabelFor("Mã thẻ hội viên", UITheme.FontSmallBold);
            layout.Controls.Add(label, 0, 0);
            layout.SetColumnSpan(label, 2);
            _code.Dock = DockStyle.Fill;
            _code.Font = UITheme.FontSubtitle;
            _code.AccessibleName = label.Text;
            _code.Margin = new Padding(0, 3, 8, 0);
            _code.KeyDown += async (_, e) =>
            {
                if (e.KeyCode == Keys.Enter)
                {
                    e.SuppressKeyPress = true;
                    await ProcessCheckInAsync(fromCamera: false);
                }
            };
            layout.Controls.Add(_code, 0, 1);
            StyleButton(_submit, primary: true);
            _submit.Click += async (_, _) => await ProcessCheckInAsync(fromCamera: false);
            layout.Controls.Add(_submit, 1, 1);

            var hint = LabelFor("Đầu đọc USB có thể gửi mã và Enter tự động.", UITheme.FontSmall);
            hint.ForeColor = Muted;
            layout.Controls.Add(hint, 0, 2);
            layout.SetColumnSpan(hint, 2);
            return layout;
        }

        private Control CreateResult()
        {
            var layout = new TableLayoutPanel { Dock = DockStyle.Fill, ColumnCount = 1, RowCount = 3 };
            layout.RowStyles.Add(new RowStyle(SizeType.Percent, 36));
            layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 46));
            layout.RowStyles.Add(new RowStyle(SizeType.Percent, 64));
            _result.TextAlign = ContentAlignment.MiddleCenter;
            _details.TextAlign = ContentAlignment.TopCenter;
            _details.ForeColor = Muted;
            layout.Controls.Add(_result, 0, 1);
            layout.Controls.Add(_details, 0, 2);
            return layout;
        }

        private Control CreateHistory()
        {
            var layout = new TableLayoutPanel { Dock = DockStyle.Fill, ColumnCount = 1, RowCount = 2, Margin = Padding.Empty };
            layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 28));
            layout.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
            _count.ForeColor = Accent;
            layout.Controls.Add(_count, 0, 0);

            var list = new Panel { Dock = DockStyle.Fill, Margin = Padding.Empty };
            _history.Dock = DockStyle.Fill;
            _history.ReadOnly = true;
            UITheme.StyleDataGridView(_history);
            _history.BackgroundColor = Surface;
            _history.GridColor = Border;
            var headers = _history.ColumnHeadersDefaultCellStyle;
            headers.BackColor = Color.FromArgb(28, 28, 32);
            headers.ForeColor = Muted;
            headers.SelectionBackColor = headers.BackColor;
            headers.SelectionForeColor = headers.ForeColor;
            var cells = _history.DefaultCellStyle;
            cells.BackColor = Input;
            cells.ForeColor = TextColor;
            cells.SelectionBackColor = Color.FromArgb(63, 41, 29);
            cells.SelectionForeColor = TextColor;
            _history.AlternatingRowsDefaultCellStyle.BackColor = Surface;
            _history.Columns.Add(UITheme.CreateFillColumn("Member", "Hội viên", 24, 70));
            _history.Columns.Add(UITheme.CreateFillColumn("Code", "Mã thẻ", 20, 72));
            _history.Columns.Add(UITheme.CreateFillColumn("Package", "Gói tập", 14, 48));
            _history.Columns.Add(UITheme.CreateFillColumn("Time", "Giờ vào", 12, 53));
            _history.Columns.Add(UITheme.CreateFillColumn("Source", "Phương thức", 19, 65));
            _history.Columns.Add(UITheme.CreateFillColumn("Status", "Trạng thái", 15, 61));
            _empty.Dock = DockStyle.None;
            _empty.BackColor = Surface;
            list.Resize += (_, _) =>
            {
                int headerHeight = _history.ColumnHeadersHeight;
                _empty.SetBounds(0, headerHeight, list.ClientSize.Width, Math.Max(0, list.ClientSize.Height - headerHeight));
            };
            _empty.TextAlign = ContentAlignment.MiddleCenter;
            _empty.ForeColor = Muted;
            list.Controls.Add(_history);
            list.Controls.Add(_empty);
            _empty.BringToFront();
            layout.Controls.Add(list, 0, 1);
            return layout;
        }

        private async Task LoadBranchesAsync()
        {
            if (_loadingBranches) return;
            _loadingBranches = true;
            try
            {
                var branches = await _service.GetBranchesAsync();
                if (IsDisposed) return;
                _branch.Items.AddRange(branches.Cast<object>().ToArray());
                if (branches.Count == 1) _branch.SelectedIndex = 0;
                if (branches.Count == 0) ShowError("Chưa có chi nhánh", "Không tìm thấy chi nhánh đang hoạt động.");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex);
                if (!IsDisposed) ShowError("Không tải được chi nhánh", "Kiểm tra kết nối SQL Server rồi bấm Làm mới.");
            }
            finally
            {
                _loadingBranches = false;
            }
        }

        private async Task RefreshBranchHistoryAsync()
        {
            if (_branch.SelectedItem is not QrCheckInBranch branch) return;
            int version = ++_historyVersion;
            if (_historyBranchId != branch.Id)
            {
                _historyBranchId = branch.Id;
                _visits = new();
                RefreshHistory();
            }
            try
            {
                var visits = await _service.GetTodayAsync(branch.Id);
                if (IsDisposed || version != _historyVersion) return;
                _visits = visits;
                RefreshHistory();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex);
                if (!IsDisposed && version == _historyVersion)
                    _empty.Text = "Không tải được lịch sử. Kiểm tra cập nhật cơ sở dữ liệu và bấm Làm mới.";
            }
        }

        private async Task ProcessCheckInAsync(bool fromCamera)
        {
            if (_busy) return;
            if (_branch.SelectedItem is not QrCheckInBranch branch)
            {
                ShowError("Chọn chi nhánh tại quầy", "Chọn chi nhánh trước khi dùng camera hoặc nhập mã thẻ.");
                return;
            }

            _busy = true;
            _branch.Enabled = false;
            _submit.Enabled = false;
            try
            {
                var result = await _service.CheckInAsync(UserSession.AccountID, branch.Id, _code.Text, fromCamera);
                if (IsDisposed) return;
                _details.Text = result.Detail;
                _result.Text = result.Status switch
                {
                    QrCheckInStatus.Accepted => "Check-in thành công",
                    QrCheckInStatus.InvalidCode => "Mã thẻ không hợp lệ",
                    QrCheckInStatus.InvalidAccount => "Tài khoản không hợp lệ",
                    QrCheckInStatus.InvalidBranch => "Chi nhánh không khả dụng",
                    QrCheckInStatus.InvalidMembership => "Gói tập chưa có hiệu lực hoặc đã hết hạn",
                    QrCheckInStatus.WrongBranch => "Gói tập không áp dụng tại chi nhánh này",
                    QrCheckInStatus.Duplicate => "Hội viên vừa check-in",
                    _ => "Chưa ghi nhận được lượt vào"
                };
                _result.ForeColor = result.Status switch
                {
                    QrCheckInStatus.Accepted => Success,
                    QrCheckInStatus.Duplicate => Warning,
                    _ => Error
                };
                if (result.Visit is { } visit)
                {
                    // An in-flight refresh may have read the database before this commit.
                    ++_historyVersion;
                    _visits.Insert(0, visit);
                    RefreshHistory();
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex);
                if (!IsDisposed) ShowError("Không xác nhận được lượt check-in", "Kiểm tra lịch sử trước khi thử lại. Kiểm tra kết nối và cập nhật cơ sở dữ liệu.");
            }
            finally
            {
                _busy = false;
                if (!IsDisposed)
                {
                    _branch.Enabled = true;
                    _submit.Enabled = true;
                    if (!fromCamera && Visible)
                    {
                        _code.Focus();
                        _code.SelectAll();
                    }
                }
            }
        }

        private void ShowError(string title, string detail)
        {
            _result.Text = title;
            _result.ForeColor = Error;
            _details.Text = detail;
        }

        private void RefreshHistory()
        {
            string query = _search.Text.Trim();
            var today = _visits.Where(visit => visit.Time.Date == DateTime.Today).ToList();
            _count.Text = $"{today.Count} lượt";
            _history.Rows.Clear();
            foreach (var visit in today.Where(visit => visit.Name.Contains(query, StringComparison.CurrentCultureIgnoreCase) ||
                visit.Code.Contains(query, StringComparison.OrdinalIgnoreCase)))
            {
                _history.Rows.Add(visit.Name, visit.Code, visit.Package, visit.Time.ToString("HH:mm:ss"), visit.Method, "Hợp lệ");
            }
            _empty.Text = _branch.SelectedIndex < 0 ? "Chọn chi nhánh để xem lịch sử." : today.Count == 0 ? "Chưa có lượt check-in nào hôm nay." : "Không tìm thấy lượt check-in phù hợp.";
            _empty.Visible = _history.Rows.Count == 0;
        }

        private static void StyleButton(Button button, bool primary = false)
        {
            button.FlatStyle = FlatStyle.Flat;
            button.UseVisualStyleBackColor = false;
            button.Font = UITheme.FontBold;
            button.Cursor = Cursors.Hand;
            button.BackColor = primary ? Accent : Color.FromArgb(39, 39, 42);
            button.ForeColor = primary ? Input : TextColor;
            button.FlatAppearance.BorderColor = primary ? Accent : Color.FromArgb(63, 63, 70);
            button.FlatAppearance.MouseOverBackColor = primary ? Color.FromArgb(251, 146, 60) : Border;
        }


        private static Label LabelFor(string text, Font font) => new()
        {
            Text = text, Font = font, ForeColor = TextColor, Dock = DockStyle.Fill,
            TextAlign = ContentAlignment.MiddleLeft, UseMnemonic = false, Margin = Padding.Empty
        };

        private static TableLayoutPanel Column(Padding margin, int topHeight)
        {
            var column = new TableLayoutPanel { Dock = DockStyle.Fill, ColumnCount = 1, RowCount = 2, Margin = margin };
            column.RowStyles.Add(new RowStyle(SizeType.Absolute, topHeight));
            column.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
            return column;
        }

        private static Control Card(string title, Control body, Control? headerAction = null, int headerActionWidth = 148)
        {
            var card = new RoundedPanel
            {
                Dock = DockStyle.Fill, BackColor = Surface, BorderColor = Border,
                CornerRadius = 12, Padding = new Padding(15, 10, 15, 13), Margin = new Padding(0, 0, 0, 12)
            };
            var layout = new TableLayoutPanel { Dock = DockStyle.Fill, ColumnCount = 1, RowCount = 2 };
            layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 37));
            layout.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
            layout.Controls.Add(LabelFor(title, UITheme.FontSubtitle), 0, 0);
            if (headerAction != null)
            {
                layout.ColumnCount = 2;
                layout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
                layout.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, headerActionWidth));
                layout.Controls.Add(headerAction, 1, 0);
            }
            body.Dock = DockStyle.Fill;
            layout.Controls.Add(body, 0, 1);
            if (headerAction != null) layout.SetColumnSpan(body, 2);
            card.Controls.Add(layout);
            return card;
        }
    }
}
