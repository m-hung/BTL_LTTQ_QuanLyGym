using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Windows.Forms;
using QuanLyGym.UI.Forms.Admin;
using QuanLyGym.UI.Forms.Common;
using QuanLyGym.UI.Forms.Member;
using QuanLyGym.UI.Forms.PT;
using QuanLyGym.UI.Helpers;

namespace QuanLyGym.UI
{
    internal static class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            ApplicationConfiguration.Initialize();

            if (args != null && args.Length > 0 && args[0] == "--testshot")
            {
                RunTestShot();
                return;
            }

            while (true)
            {
                using (var login = new frmLogin())
                {
                    if (login.ShowDialog() != DialogResult.OK)
                        return;
                }

                Form? mainForm = UserSession.Role switch
                {
                    UserSession.ROLE_ADMIN => new frmAdminMain(),
                    UserSession.ROLE_PT => new frmPTMain(),
                    UserSession.ROLE_MEMBER => new frmMemberMain(),
                    _ => null
                };

                if (mainForm == null)
                {
                    MessageBox.Show($"Vai trò \"{UserSession.Role}\" không hợp lệ.", "Lỗi",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    UserSession.Clear();
                    continue;
                }

                Application.Run(mainForm);

                if (!UserSession.IsLoggingOut) return;
                UserSession.Clear();
            }
        }

        private static void RunTestShot()
        {
            string artifactDir = @"C:\Users\LENOVO\.gemini\antigravity\brain\33e9f033-7b84-45cd-a167-b8c4f2cc55f4";
            try
            {
                // ==========================================
                // 1. TEST PT MEMBER: Nguyễn Khánh Hưng (AccountID 7, VIP Diamond)
                // ==========================================
                UserSession.Role = UserSession.ROLE_MEMBER;
                UserSession.FullName = "Nguyễn Khánh Hưng";
                UserSession.AccountID = 7;

                using (var form = new frmMemberMain())
                {
                    form.Shown += (s, e) =>
                    {
                        var t = new System.Windows.Forms.Timer { Interval = 600 };
                        int step = 0;
                        t.Tick += (ts, te) =>
                        {
                            try
                            {
                                if (step == 0)
                                {
                                    form.NavigateTo("Dashboard");
                                    form.Update();
                                    Application.DoEvents();
                                    using var bmp = new Bitmap(form.Width, form.Height);
                                    form.DrawToBitmap(bmp, new Rectangle(0, 0, form.Width, form.Height));
                                    bmp.Save(Path.Combine(artifactDir, "render_pt_dashboard_live.png"), ImageFormat.Png);
                                    step++;
                                }
                                else if (step == 1)
                                {
                                    form.NavigateTo("Schedule");
                                    form.Update();
                                    Application.DoEvents();
                                    using var bmp = new Bitmap(form.Width, form.Height);
                                    form.DrawToBitmap(bmp, new Rectangle(0, 0, form.Width, form.Height));
                                    bmp.Save(Path.Combine(artifactDir, "render_pt_schedule_live.png"), ImageFormat.Png);
                                    step++;
                                }
                                else if (step == 2)
                                {
                                    form.NavigateTo("Progress");
                                    form.Update();
                                    Application.DoEvents();
                                    using var bmp = new Bitmap(form.Width, form.Height);
                                    form.DrawToBitmap(bmp, new Rectangle(0, 0, form.Width, form.Height));
                                    bmp.Save(Path.Combine(artifactDir, "render_pt_progress_live.png"), ImageFormat.Png);
                                    step++;
                                }
                                else if (step == 3)
                                {
                                    form.NavigateTo("Membership");
                                    form.Update();
                                    Application.DoEvents();
                                    using var bmp = new Bitmap(form.Width, form.Height);
                                    form.DrawToBitmap(bmp, new Rectangle(0, 0, form.Width, form.Height));
                                    bmp.Save(Path.Combine(artifactDir, "render_pt_membership_live.png"), ImageFormat.Png);
                                    t.Stop();
                                    form.Close();
                                }
                            }
                            catch (Exception ex)
                            {
                                File.WriteAllText(Path.Combine(artifactDir, "err_pt.txt"), ex.ToString());
                                t.Stop();
                                form.Close();
                            }
                        };
                        t.Start();
                    };
                    Application.Run(form);
                }

                // ==========================================
                // 2. TEST CLASSIC MEMBER: Võ Bảo Trâm (AccountID 16, Classic)
                // ==========================================
                UserSession.Role = UserSession.ROLE_MEMBER;
                UserSession.FullName = "Võ Bảo Trâm";
                UserSession.AccountID = 16;

                using (var formClassic = new frmMemberMain())
                {
                    formClassic.Shown += (s, e) =>
                    {
                        var t2 = new System.Windows.Forms.Timer { Interval = 600 };
                        int step2 = 0;
                        t2.Tick += (ts, te) =>
                        {
                            try
                            {
                                if (step2 == 0)
                                {
                                    formClassic.NavigateTo("Dashboard");
                                    formClassic.Update();
                                    Application.DoEvents();
                                    using var bmp = new Bitmap(formClassic.Width, formClassic.Height);
                                    formClassic.DrawToBitmap(bmp, new Rectangle(0, 0, formClassic.Width, formClassic.Height));
                                    bmp.Save(Path.Combine(artifactDir, "render_classic_dashboard_live.png"), ImageFormat.Png);
                                    step2++;
                                }
                                else if (step2 == 1)
                                {
                                    formClassic.NavigateTo("Membership");
                                    formClassic.Update();
                                    Application.DoEvents();
                                    using var bmp = new Bitmap(formClassic.Width, formClassic.Height);
                                    formClassic.DrawToBitmap(bmp, new Rectangle(0, 0, formClassic.Width, formClassic.Height));
                                    bmp.Save(Path.Combine(artifactDir, "render_classic_membership_live.png"), ImageFormat.Png);
                                    step2++;
                                }
                                else if (step2 == 2)
                                {
                                    formClassic.NavigateTo("Upgrade");
                                    formClassic.Update();
                                    Application.DoEvents();
                                    using var bmp = new Bitmap(formClassic.Width, formClassic.Height);
                                    formClassic.DrawToBitmap(bmp, new Rectangle(0, 0, formClassic.Width, formClassic.Height));
                                    bmp.Save(Path.Combine(artifactDir, "render_classic_upgrade_live.png"), ImageFormat.Png);
                                    step2++;
                                }
                                else if (step2 == 3)
                                {
                                    formClassic.NavigateTo("CheckIn");
                                    formClassic.Update();
                                    Application.DoEvents();
                                    using var bmp = new Bitmap(formClassic.Width, formClassic.Height);
                                    formClassic.DrawToBitmap(bmp, new Rectangle(0, 0, formClassic.Width, formClassic.Height));
                                    bmp.Save(Path.Combine(artifactDir, "render_classic_checkin_live.png"), ImageFormat.Png);
                                    t2.Stop();
                                    formClassic.Close();
                                }
                            }
                            catch (Exception ex)
                            {
                                File.WriteAllText(Path.Combine(artifactDir, "err_classic.txt"), ex.ToString());
                                t2.Stop();
                                formClassic.Close();
                            }
                        };
                        t2.Start();
                    };
                    Application.Run(formClassic);
                }
            }
            catch (Exception ex)
            {
                File.WriteAllText(Path.Combine(artifactDir, "testshot_err.txt"), ex.ToString());
            }
        }
    }
}
