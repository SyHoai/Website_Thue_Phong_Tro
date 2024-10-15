using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;


namespace DACS_Web.Models
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }

        public DbSet<BaiDang> BaiDang { get; set; }
        public DbSet<BaoCao> BaoCao { get; set; }

        public DbSet<DanhGia> DanhGia { get; set; }
        public DbSet<DanhSachAnh> DanhSachAnh { get; set; }
        public DbSet<DichVu> DinhVu { get; set; }

        public DbSet<HoaDon> HoaDon { get; set; }
        public DbSet<LoaiPhong> LoaiPhong { get; set; }
        public DbSet<LoaiBaiDang> LoaiBaiDang { get; set; }
        

        public DbSet<DanhSach> DanhSach { get; set; }
        public DbSet<DanhSach_BaiDang> DanhSach_BaiDang { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<DanhSach_BaiDang>()
               .HasKey(bn => new { bn.DanhSachId, bn.BaiDangId });


            modelBuilder.Entity<DanhGia>()
               .HasOne(d => d.NguoiTao)
               .WithMany(u => u.DanhGiasCreatedByUser)
               .HasForeignKey(d => d.NguoiTaoId);


            modelBuilder.Entity<DanhGia>()
                .HasOne(d => d.NguoiNhan)
                .WithMany(u => u.DanhGiasReceivedByUser)
                .HasForeignKey(d => d.NguoiNhanId);


            modelBuilder.Entity<BaoCao>()
               .HasOne(d => d.NguoiTao)
               .WithMany(u => u.BaoCaosCreatedByUser)
               .HasForeignKey(d => d.NguoiTaoId);


            modelBuilder.Entity<BaoCao>()
                .HasOne(d => d.NguoiNhan)
                .WithMany(u => u.BaoCaosReceivedByUser)
                .HasForeignKey(d => d.NguoiNhanId);
        }
    }
}
