using System;
using System.Drawing;
using System.Windows.Forms;

namespace QuanLyGym.UI.Utils
{
    public static class DisplayHelper
    {
        public static Rectangle GetWorkingArea(Control? ctrl = null)
        {
            if (ctrl != null && ctrl.IsHandleCreated)
            {
                var scr = Screen.FromControl(ctrl);
                if (scr != null) return scr.WorkingArea;
            }
            return Screen.PrimaryScreen?.WorkingArea ?? new Rectangle(0, 0, 1366, 768);
        }

        /// <summary>
        /// Tự động nhận diện độ phân giải và tỉ lệ màn hình hiện tại để cấu hình Form phù hợp.
        /// </summary>
        public static void AutoConfigureForm(Form form)
        {
            var wa = GetWorkingArea(form);
            form.StartPosition = FormStartPosition.CenterScreen;
            form.MinimumSize = new Size(1100, 680);

            int normalW = Math.Clamp((int)(wa.Width * 0.92), 1100, 1550);
            int normalH = Math.Clamp((int)(wa.Height * 0.90), 680, 950);
            form.Size = new Size(normalW, normalH);
            form.Location = new Point(
                wa.Left + Math.Max(0, (wa.Width - normalW) / 2),
                wa.Top + Math.Max(0, (wa.Height - normalH) / 2)
            );

            // Phóng to trong sự kiện Load sau khi HWND và DPI đã được khởi tạo chuẩn xác
            form.Load += (s, e) =>
            {
                form.WindowState = FormWindowState.Maximized;
            };
        }
    }
}
