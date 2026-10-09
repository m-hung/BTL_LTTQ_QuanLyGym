using Microsoft.EntityFrameworkCore;
using QuanLyGym.DAL.Entities;

namespace QuanLyGym.DAL
{
    public class AppDbContext : DbContext
    {
        public DbSet<Branch> Branches { get; set; }
        public DbSet<Account> Accounts { get; set; }
        public DbSet<Member> Members { get; set; }
        public DbSet<PT> PTs { get; set; }
        public DbSet<Package> Packages { get; set; }
        public DbSet<Membership> Memberships { get; set; }
        public DbSet<Room> Rooms { get; set; }
        public DbSet<Equipment> Equipments { get; set; }
        public DbSet<EquipmentMaintenance> EquipmentMaintenances { get; set; }
        public DbSet<Incident> Incidents { get; set; }
        public DbSet<WorkingSchedule> WorkingSchedules { get; set; }
        public DbSet<Schedule> Schedules { get; set; }
        public DbSet<CheckIn> CheckIns { get; set; }
        public DbSet<CheckInImage> CheckInImages { get; set; }
        public DbSet<Progress> Progresses { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(@"Server=.\SQLEXPRESS;Database=QuanLyGymFullDB;Trusted_Connection=True;TrustServerCertificate=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // 1. Cấu hình OnDelete chặn lỗi vòng lặp khóa ngoại
            modelBuilder.Entity<Progress>().HasOne(p => p.PT).WithMany().HasForeignKey(p => p.PTID).OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<Schedule>().HasOne(s => s.PT).WithMany().HasForeignKey(s => s.PTID).OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<CheckIn>().HasOne(c => c.PT).WithMany().HasForeignKey(c => c.PTID).OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<Incident>().HasOne(i => i.Account).WithMany().HasForeignKey(i => i.ReportedBy).OnDelete(DeleteBehavior.Restrict);

            // 2. TỰ ĐỘNG NẠP TẤT CẢ SEED DATA TỪ THƯ MỤC CONFIGURATIONS
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
        }
    }
}