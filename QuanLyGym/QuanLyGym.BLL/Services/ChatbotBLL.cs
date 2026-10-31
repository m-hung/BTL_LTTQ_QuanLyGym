using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using QuanLyGym.DAL;
using QuanLyGym.DAL.Entities;

namespace QuanLyGym.BLL.Services
{
    /// <summary>
    /// Service xử lý nghiệp vụ cho AI Chatbot phòng Gym.
    /// Kết nối trực tiếp với SQL Server Database (AppDbContext) để lấy thông tin thực tế:
    /// - Huấn luyện viên cá nhân (PT)
    /// - Các gói tập & bảng giá (Packages)
    /// - Chi nhánh & Giờ hoạt động (Branches)
    /// - Thiết bị máy tập (Equipments)
    /// - Tính toán chỉ số BMI & đưa ra phác đồ tập luyện tương ứng
    /// </summary>
    public class ChatbotBLL
    {
        /// <summary>
        /// Xử lý câu hỏi của người dùng và trả về phản hồi thông minh dựa trên Database & logic Gym
        /// </summary>
        public async Task<string> ProcessUserMessageAsync(string prompt)
        {
            if (string.IsNullOrWhiteSpace(prompt))
            {
                return "Bạn vui lòng nhập câu hỏi nhé!";
            }

            string cleanPrompt = prompt.Trim();
            string lower = cleanPrompt.ToLower();

            // 1. Kiểm tra yêu cầu Tính chỉ số BMI
            if (lower.Contains("bmi") || (lower.Contains("cao") && lower.Contains("nặng")))
            {
                return await HandleBmiCalculationAsync(cleanPrompt);
            }

            // 2. Tra cứu thông tin Huấn luyện viên cá nhân (PT) trong Database
            if (lower.Contains("pt") || lower.Contains("huấn luyện viên") || lower.Contains("hlv") || lower.Contains("thầy dạy") || lower.Contains("cô dạy"))
            {
                return await GetPtInformationFromDbAsync(lower);
            }

            // 3. Tra cứu Gói tập & Bảng giá trong Database
            if (lower.Contains("gói") || lower.Contains("giá") || lower.Contains("bảng giá") || lower.Contains("học phí") || lower.Contains("chi phí") || lower.Contains("đăng ký"))
            {
                return await GetPackageInformationFromDbAsync(lower);
            }

            // 4. Tra cứu Chi nhánh, Địa chỉ, Giờ mở cửa trong Database
            if (lower.Contains("chi nhánh") || lower.Contains("địa chỉ") || lower.Contains("ở đâu") || lower.Contains("giờ mở") || lower.Contains("mở cửa") || lower.Contains("đóng cửa") || lower.Contains("sđt") || lower.Contains("liên hệ") || lower.Contains("hotline"))
            {
                return await GetBranchInformationFromDbAsync();
            }

            // 5. Tra cứu Trang thiết bị, Phòng tập trong Database
            if (lower.Contains("thiết bị") || lower.Contains("máy tập") || lower.Contains("dụng cụ") || lower.Contains("phòng tập"))
            {
                return await GetEquipmentInformationFromDbAsync();
            }

            // 6. Chào hỏi
            if (lower.Contains("chào") || lower.Contains("hello") || lower.Contains("hi") || lower == "alo" || lower == "hey")
            {
                return "Chào bạn! 👋 Tôi là Trợ lý AI kết nối trực tiếp với hệ thống dữ liệu QuanLyGym.\n" +
                       "Tôi có thể hỗ trợ bạn:\n" +
                       "• 📊 Tính chỉ số BMI & tư vấn thể trạng (ví dụ: 'tính bmi cao 1m70 nặng 65kg')\n" +
                       "• 🏋️ Tra cứu danh sách & thông tin Huấn luyện viên (PT)\n" +
                       "• 💳 Tra cứu bảng giá & chi tiết các gói tập hiện có\n" +
                       "• 🏢 Xem địa chỉ chi nhánh, giờ mở cửa & cơ sở vật chất phòng tập\n" +
                       "• 🥗 Tư vấn lịch tập (Push-Pull-Legs, Upper/Lower) & dinh dưỡng tăng cơ/giảm mỡ.\n\n" +
                       "Bạn đang quan tâm đến nội dung nào?";
            }

            // 7. Kiến thức dinh dưỡng & Tập luyện
            if (lower.Contains("lịch tập") || lower.Contains("tập gì") || lower.Contains("bài tập"))
            {
                return "💡 Gợi ý lịch tập phù hợp theo mục tiêu:\n" +
                       "1. Lịch Push - Pull - Legs (6 buổi/tuần): Tối ưu tăng cơ cho người tập trung cấp trở lên.\n" +
                       "2. Lịch Upper / Lower (4 buổi/tuần): Cân bằng tối ưu giữa thời gian và hiệu quả.\n" +
                       "3. Lịch Full Body (3 buổi/tuần): Phù hợp người mới bắt đầu (Beginner).\n\n" +
                       "👉 Bạn có thể kết hợp kèm cùng Huấn luyện viên PT tại phòng để được hướng dẫn form chuẩn nhất!";
            }

            if (lower.Contains("giảm mỡ") || lower.Contains("giảm cân") || lower.Contains("cardio"))
            {
                return "🔥 Hướng dẫn Giảm mỡ chuẩn khoa học:\n" +
                       "• Nguyên lý: Calo nạp vào < Calo tiêu thụ (Thâm hụt 300 - 500 kcal/ngày).\n" +
                       "• Tập tạ kháng lực kết hợp 20 - 30 phút Cardio/HIIT sau buổi tập.\n" +
                       "• Bạn hãy gõ: 'tính bmi cao ... nặng ...' để tôi đo chính xác chỉ số cơ thể của bạn nhé!";
            }

            if (lower.Contains("tăng cơ") || lower.Contains("protein") || lower.Contains("whey") || lower.Contains("dinh dưỡng"))
            {
                return "🥩 Nguyên tắc Tăng cơ tối ưu:\n" +
                       "• Nạp đủ đạm: 1.6g - 2.2g Protein / mỗi kg trọng lượng cơ thể.\n" +
                       "• Progressive Overload (tăng tạ tịnh tiến theo thời gian).\n" +
                       "• Uống đủ 2 - 3 lít nước mỗi ngày và ngủ đủ 7 - 8 tiếng.";
            }

            // 8. Trả lời mặc định
            return $"Cảm ơn bạn đã hỏi về: \"{cleanPrompt}\".\n" +
                   "Hệ thống QuanLyGym luôn sẵn sàng hỗ trợ. Bạn có thể hỏi tôi về: 'Danh sách PT', 'Bảng giá gói tập', 'Địa chỉ chi nhánh' hoặc 'Tính BMI cao 1m70 nặng 65kg' nhé! 💪";
        }

        #region Nghiệp vụ Tính toán chỉ số BMI

        /// <summary>
        /// Phân tích và tính chỉ số BMI từ nội dung người dùng nhập
        /// </summary>
        private async Task<string> HandleBmiCalculationAsync(string text)
        {
            // Trích xuất chiều cao và cân nặng bằng Regex
            (double? heightCm, double? weightKg) = ExtractHeightAndWeight(text);

            if (!heightCm.HasValue || !weightKg.HasValue)
            {
                return "📊 Hướng dẫn tính chỉ số BMI:\n" +
                       "Bạn vui lòng nhập theo cú pháp ví dụ:\n" +
                       "👉 \"tính bmi cao 1m70 nặng 65kg\" hoặc \"bmi 170cm 60kg\"\n\n" +
                       "Công thức chuẩn: BMI = Cân nặng (kg) / [Chiều cao (m)]²";
            }

            double hM = heightCm.Value / 100.0;
            double bmi = weightKg.Value / (hM * hM);
            double bmiRounded = Math.Round(bmi, 1);

            // Cân nặng lý tưởng theo chuẩn Châu Á (BMI: 18.5 - 22.9)
            double idealMinWeight = Math.Round(18.5 * hM * hM, 1);
            double idealMaxWeight = Math.Round(22.9 * hM * hM, 1);

            StringBuilder sb = new StringBuilder();
            sb.AppendLine("📋 KẾT QUẢ PHÂN TÍCH CHỈ SỐ BMI CỦA BẠN:");
            sb.AppendLine($"• Chiều cao: {heightCm.Value:0.#} cm ({hM:0.00} m)");
            sb.AppendLine($"• Cân nặng: {weightKg.Value:0.#} kg");
            sb.AppendLine($"• Chỉ số BMI: {bmiRounded:0.0}");
            sb.AppendLine($"• Cân nặng chuẩn lý tưởng: {idealMinWeight} kg - {idealMaxWeight} kg");
            sb.AppendLine();

            string status;
            string advice;
            string recommendedPtSpecialty;

            if (bmi < 18.5)
            {
                status = "Thiếu cân (Gầy) - Underweight";
                advice = "• Mục tiêu: TĂNG CƠ & TĂNG CÂN LÀNH MẠNH.\n" +
                         "• Dinh dưỡng: Thặng dư Calo (+300 đến +500 kcal/ngày), bổ sung Protein (Thịt bò, Ức gà, Trứng, Whey).\n" +
                         "• Tập luyện: Tập tạ kháng lực nặng 8-10 reps/hiệp, hạn chế Cardio quá nhiều.";
                recommendedPtSpecialty = "Tăng cơ";
            }
            else if (bmi <= 22.9)
            {
                status = "Thể trạng Bình thường / Lý tưởng - Normal";
                advice = "• Chúc mừng bạn! Bạn đang sở hữu chỉ số thể hình rất cân đối.\n" +
                         "• Mục tiêu: DUY TRÌ & TĂNG ĐỘ SĂN CHẮC (Recomposition).\n" +
                         "• Tập luyện: Đa dạng các bài tập compound và duy trì chế độ ăn lành mạnh.";
                recommendedPtSpecialty = "Tăng cơ, Giảm mỡ";
            }
            else if (bmi <= 24.9)
            {
                status = "Tiền béo phì (Thừa cân nhẹ) - Overweight";
                advice = "• Mục tiêu: GIẢM MỠ & SIẾT CƠ.\n" +
                         "• Dinh dưỡng: Thâm hụt Calo nhẹ (-300 kcal/ngày), cắt giảm đồ ngọt, nước ngọt có ga và tinh bột nhanh.\n" +
                         "• Tập luyện: Kết hợp tập tạ và 20-30 phút Cardio/HIIT sau buổi tập.";
                recommendedPtSpecialty = "Giảm mỡ";
            }
            else
            {
                status = "Béo phì (Cần chú ý sức khỏe) - Obese";
                advice = "• Mục tiêu: GIẢM MỠ AN TOÀN & BẢO VỆ KHỚP GỐI.\n" +
                         "• Dinh dưỡng: Kiểm soát Calo nghiêm ngặt, ưu tiên rau xanh, chất xơ và đạm nạc.\n" +
                         "• Tập luyện: Bắt đầu với đi bộ dốc, đạp xe, bơi lội, tránh các bài nhảy mạnh dồn áp lực lên khớp gối.\n" +
                         "• Khuyến nghị: Rất nên có Huấn luyện viên PT đồng hành để giám sát nhịp tim và giáo án an toàn.";
                recommendedPtSpecialty = "Giảm mỡ";
            }

            sb.AppendLine($"👉 Đánh giá thể trạng: {status}");
            sb.AppendLine(advice);

            // Gợi ý PT từ Database phù hợp với thể trạng
            try
            {
                using var db = new AppDbContext();
                var ptMatch = await db.PTs
                    .Include(p => p.Branch)
                    .Where(p => p.Status && p.Specialization.Contains(recommendedPtSpecialty))
                    .FirstOrDefaultAsync();

                if (ptMatch != null)
                {
                    sb.AppendLine();
                    sb.AppendLine($"💡 Huấn luyện viên (PT) phù hợp với mục tiêu của bạn:");
                    sb.AppendLine($"• HLV: {ptMatch.FullName} (Chuyên môn: {ptMatch.Specialization}, {ptMatch.Experience} năm kinh nghiệm)");
                    sb.AppendLine($"• Hotline: {ptMatch.Phone} | Chi nhánh: {ptMatch.Branch?.BranchName ?? "Hệ thống"}");
                }
            }
            catch
            {
                // Bỏ qua lỗi DB nếu chưa kết nối được để vẫn trả về kết quả BMI cho người dùng
            }

            return sb.ToString().TrimEnd();
        }

        /// <summary>
        /// Trích xuất chiều cao (cm) và cân nặng (kg) từ câu tự nhiên
        /// </summary>
        private static (double? heightCm, double? weightKg) ExtractHeightAndWeight(string text)
        {
            double? height = null;
            double? weight = null;

            // Pattern nhận diện chiều cao: "cao 1m70", "cao 1.70m", "cao 170cm", "1m75", "175 cm"
            Match heightMatch = Regex.Match(
                text, 
                @"(?:cao\s*)?(?:(\d{1})[m,.](\d{1,2})|(\d{2,3})\s*(?:cm)?)", 
                RegexOptions.IgnoreCase);

            if (heightMatch.Success)
            {
                if (!string.IsNullOrEmpty(heightMatch.Groups[1].Value))
                {
                    // Dạng 1m70 hoặc 1.70
                    int meters = int.Parse(heightMatch.Groups[1].Value);
                    string cmPart = heightMatch.Groups[2].Value.PadRight(2, '0');
                    height = meters * 100 + int.Parse(cmPart);
                }
                else if (!string.IsNullOrEmpty(heightMatch.Groups[3].Value))
                {
                    // Dạng 170
                    double val = double.Parse(heightMatch.Groups[3].Value);
                    if (val > 100 && val < 250)
                    {
                        height = val;
                    }
                }
            }

            // Pattern nhận diện cân nặng: "nặng 65kg", "nặng 65", "65 kg", "65.5kg"
            Match weightMatch = Regex.Match(
                text, 
                @"(?:nặng\s*)?(\d{2,3}(?:[.,]\d{1,2})?)\s*(?:kg|kí|cân)?", 
                RegexOptions.IgnoreCase);

            // Tìm vị trí cân nặng không bị trùng với chiều cao
            foreach (Match m in Regex.Matches(text, @"(\d{2,3}(?:[.,]\d{1,2})?)\s*(?:kg|kí|cân)", RegexOptions.IgnoreCase))
            {
                if (double.TryParse(m.Groups[1].Value.Replace(',', '.'), NumberStyles.Any, CultureInfo.InvariantCulture, out double wVal))
                {
                    if (wVal >= 30 && wVal <= 250)
                    {
                        weight = wVal;
                        break;
                    }
                }
            }

            // Fallback nếu người dùng gõ số liền: "bmi 170 65" hoặc "cao 170 nặng 65"
            if (!height.HasValue || !weight.HasValue)
            {
                var numbers = Regex.Matches(text, @"\b(\d{2,3})\b")
                    .Cast<Match>()
                    .Select(m => double.Parse(m.Value))
                    .ToList();

                foreach (var num in numbers)
                {
                    if (num >= 130 && num <= 230 && !height.HasValue)
                    {
                        height = num;
                    }
                    else if (num >= 35 && num <= 180 && !weight.HasValue)
                    {
                        weight = num;
                    }
                }
            }

            return (height, weight);
        }

        #endregion

        #region Truy vấn Thông tin Database SQL Server (PT, Packages, Branches, Equipments)

        /// <summary>
        /// Lấy thông tin Huấn luyện viên PT trực tiếp từ bảng PTs trong SQL Server
        /// </summary>
        private async Task<string> GetPtInformationFromDbAsync(string query)
        {
            try
            {
                using var db = new AppDbContext();
                var pts = await db.PTs
                    .Include(p => p.Branch)
                    .Where(p => p.Status)
                    .ToListAsync();

                if (pts == null || pts.Count == 0)
                {
                    return "Hiện tại hệ thống chưa cập nhật danh sách Huấn luyện viên (PT) nào khả dụng.";
                }

                // Nếu người dùng hỏi đích danh PT cụ thể
                var filtered = pts.Where(p => query.Contains(p.FullName.ToLower())).ToList();
                if (filtered.Any())
                {
                    pts = filtered;
                }

                StringBuilder sb = new StringBuilder();
                sb.AppendLine("🏋️‍♂️ DANH SÁCH HUẤN LUYỆN VIÊN (PT) ĐANG HOẠT ĐỘNG:");
                sb.AppendLine();

                int index = 1;
                foreach (var pt in pts)
                {
                    sb.AppendLine($"{index++}. HLV {pt.FullName} ({pt.Gender})");
                    sb.AppendLine($"   • Chuyên môn: {pt.Specialization}");
                    sb.AppendLine($"   • Kinh nghiệm: {pt.Experience} năm");
                    sb.AppendLine($"   • Liên hệ: {pt.Phone} | Email: {pt.Email}");
                    if (pt.Branch != null)
                    {
                        sb.AppendLine($"   • Cơ sở: {pt.Branch.BranchName} ({pt.Branch.Address})");
                    }
                    sb.AppendLine();
                }

                sb.AppendLine("👉 Bạn có thể đăng ký gói tập có kèm PT để được hướng dẫn giáo án 1-1 tối ưu nhất!");
                return sb.ToString().TrimEnd();
            }
            catch (Exception ex)
            {
                return $"⚠️ Không thể kết nối đến cơ sở dữ liệu SQL Server: {ex.Message}\n" +
                       "Vui lòng đảm bảo SQL Server (SQLEXPRESS) đang chạy và Database 'QuanLyGymFullDB' đã được tạo.";
            }
        }

        /// <summary>
        /// Lấy thông tin các Gói tập & Bảng giá trực tiếp từ bảng Packages trong SQL Server
        /// </summary>
        private async Task<string> GetPackageInformationFromDbAsync(string query)
        {
            try
            {
                using var db = new AppDbContext();
                var packages = await db.Packages
                    .Include(p => p.Branch)
                    .Where(p => p.Status)
                    .OrderBy(p => p.Price)
                    .ToListAsync();

                if (packages == null || packages.Count == 0)
                {
                    return "Hiện tại hệ thống chưa có thông tin gói tập khả dụng.";
                }

                StringBuilder sb = new StringBuilder();
                sb.AppendLine("💳 BẢNG GIÁ & CÁC GÓI TẬP HIỆN CÓ TẠI PHÒNG GYM:");
                sb.AppendLine();

                int idx = 1;
                foreach (var pkg in packages)
                {
                    sb.AppendLine($"{idx++}. {pkg.PackageName.ToUpper()}");
                    sb.AppendLine($"   • Giá: {pkg.Price:N0} VNĐ");
                    sb.AppendLine($"   • Thời hạn: {pkg.Duration} ngày ({pkg.TotalPTSessions} buổi tập)");
                    if (!string.IsNullOrEmpty(pkg.Benefits))
                    {
                        sb.AppendLine($"   • Quyền lợi: {pkg.Benefits}");
                    }
                    if (!string.IsNullOrEmpty(pkg.Description))
                    {
                        sb.AppendLine($"   • Mô tả: {pkg.Description}");
                    }
                    sb.AppendLine();
                }

                sb.AppendLine("🎁 Đăng ký ngay hôm nay tại quầy Lễ tân hoặc liên hệ Hotline để nhận ưu đãi tập thử!");
                return sb.ToString().TrimEnd();
            }
            catch (Exception ex)
            {
                return $"⚠️ Không thể kết nối cơ sở dữ liệu SQL Server: {ex.Message}";
            }
        }

        /// <summary>
        /// Lấy thông tin Chi nhánh, Địa chỉ, Hotline và Giờ hoạt động từ bảng Branches
        /// </summary>
        private async Task<string> GetBranchInformationFromDbAsync()
        {
            try
            {
                using var db = new AppDbContext();
                var branches = await db.Branches
                    .Where(b => b.Status)
                    .ToListAsync();

                if (branches == null || branches.Count == 0)
                {
                    return "Hiện chưa có thông tin chi nhánh trên hệ thống.";
                }

                StringBuilder sb = new StringBuilder();
                sb.AppendLine("🏢 HỆ THỐNG CHI NHÁNH PHÒNG GYM:");
                sb.AppendLine();

                foreach (var b in branches)
                {
                    sb.AppendLine($"📍 {b.BranchName}");
                    sb.AppendLine($"   • Địa chỉ: {b.Address}");
                    sb.AppendLine($"   • Hotline: {b.Phone} | Email: {b.Email}");
                    sb.AppendLine($"   • Giờ hoạt động: {b.OpenTime:hh\\:mm} - {b.CloseTime:hh\\:mm} hàng ngày");
                    sb.AppendLine();
                }

                return sb.ToString().TrimEnd();
            }
            catch (Exception ex)
            {
                return $"⚠️ Lỗi kết nối cơ sở dữ liệu: {ex.Message}";
            }
        }

        /// <summary>
        /// Lấy thông tin Trang thiết bị từ bảng Equipments
        /// </summary>
        private async Task<string> GetEquipmentInformationFromDbAsync()
        {
            try
            {
                using var db = new AppDbContext();
                var equipments = await db.Equipments
                    .Include(e => e.Room)
                    .ToListAsync();

                if (equipments == null || equipments.Count == 0)
                {
                    return "Phòng tập được trang bị đầy đủ máy tập Cardio, Giàn tạ khối, Tạ tay đa dạng và khu vực dãn cơ chuẩn quốc tế.";
                }

                int total = equipments.Count;
                int good = equipments.Count(e => e.Status == "Tốt");

                StringBuilder sb = new StringBuilder();
                sb.AppendLine("🏋️ TRANG THIẾT BỊ PHÒNG GYM:");
                sb.AppendLine($"• Tổng số máy tập hiện có: {total} thiết bị.");
                sb.AppendLine($"• Trạng thái hoạt động tốt: {good}/{total} thiết bị (được bảo trì định kỳ).");
                sb.AppendLine();
                sb.AppendLine("Một số thiết bị tiêu biểu:");

                foreach (var eq in equipments.Take(5))
                {
                    sb.AppendLine($"   - {eq.EquipmentName} (Mã: {eq.EquipmentCode}, Trạng thái: {eq.Status})");
                }

                return sb.ToString().TrimEnd();
            }
            catch (Exception ex)
            {
                return $"⚠️ Lỗi kết nối cơ sở dữ liệu: {ex.Message}";
            }
        }

        #endregion
    }
}
