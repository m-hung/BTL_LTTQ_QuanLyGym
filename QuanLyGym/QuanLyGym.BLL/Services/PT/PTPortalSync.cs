using System;

namespace QuanLyGym.BLL.Services.PT
{
    /// <summary>
    /// Sự kiện đồng bộ dữ liệu dùng chung giữa các màn hình phân hệ PT (Sidebar, Chuông thông báo, Hồ sơ, Yêu cầu đặt lịch).
    /// </summary>
    public static class PTPortalSync
    {
        public static event Action? DataChanged
        {
            add => PTDataStore.Instance.DataChanged += value;
            remove => PTDataStore.Instance.DataChanged -= value;
        }
    }
}
