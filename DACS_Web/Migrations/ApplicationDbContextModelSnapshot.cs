﻿// <auto-generated />
using System;
using DACS_Web.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace DACS_Web.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    partial class ApplicationDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.4")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("DACS_Web.Models.ApplicationUser", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("AccessFailedCount")
                        .HasColumnType("int");

                    b.Property<string>("Avatar")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("DiaChi")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("Email")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<bool>("EmailConfirmed")
                        .HasColumnType("bit");

                    b.Property<string>("HoTen")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<bool>("LockoutEnabled")
                        .HasColumnType("bit");

                    b.Property<DateTimeOffset?>("LockoutEnd")
                        .HasColumnType("datetimeoffset");

                    b.Property<DateTime>("NgaySinh")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("NgayThamGia")
                        .HasColumnType("datetime2");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("PasswordHash")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("PhoneNumberConfirmed")
                        .HasColumnType("bit");

                    b.Property<string>("PhuongXa")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("QuanHuyen")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("SecurityStamp")
                        .HasColumnType("nvarchar(max)");

                    b.Property<decimal?>("SoDu")
                        .HasColumnType("decimal(18,2)");

                    b.Property<string>("TinhThanh")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<bool>("TrangThai")
                        .HasColumnType("bit");

                    b.Property<bool>("TwoFactorEnabled")
                        .HasColumnType("bit");

                    b.Property<string>("UserName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasDatabaseName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasDatabaseName("UserNameIndex")
                        .HasFilter("[NormalizedUserName] IS NOT NULL");

                    b.ToTable("AspNetUsers", (string)null);
                });

            modelBuilder.Entity("DACS_Web.Models.BaiDang", b =>
                {
                    b.Property<int>("BaiDangId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("BaiDangId"));

                    b.Property<string>("DiaChi")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<float>("DienTich")
                        .HasColumnType("real");

                    b.Property<int>("Duyet")
                        .HasColumnType("int");

                    b.Property<DateTime?>("HanDichVu")
                        .HasColumnType("datetime2");

                    b.Property<int>("LoaiBaiDangId")
                        .HasColumnType("int");

                    b.Property<int>("LoaiPhongId")
                        .HasColumnType("int");

                    b.Property<string>("MoTa")
                        .IsRequired()
                        .HasMaxLength(1000)
                        .HasColumnType("nvarchar(1000)");

                    b.Property<DateTime>("NgayDang")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("NgayHetHan")
                        .HasColumnType("datetime2");

                    b.Property<string>("PhuongXa")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("QuanHuyen")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("TenBD")
                        .IsRequired()
                        .HasMaxLength(1000)
                        .HasColumnType("nvarchar(1000)");

                    b.Property<decimal>("TienCoc")
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal>("TienThue")
                        .HasColumnType("decimal(18,2)");

                    b.Property<string>("TinhThanh")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<bool>("TrangThai")
                        .HasColumnType("bit");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("BaiDangId");

                    b.HasIndex("LoaiBaiDangId");

                    b.HasIndex("LoaiPhongId");

                    b.HasIndex("UserId");

                    b.ToTable("BaiDang");
                });

            modelBuilder.Entity("DACS_Web.Models.BaoCao", b =>
                {
                    b.Property<int>("BaoCaoId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("BaoCaoId"));

                    b.Property<DateTime>("NgayTao")
                        .HasColumnType("datetime2");

                    b.Property<string>("NguoiNhanId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("NguoiTaoId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("NoiDung")
                        .IsRequired()
                        .HasMaxLength(1000)
                        .HasColumnType("nvarchar(1000)");

                    b.HasKey("BaoCaoId");

                    b.HasIndex("NguoiNhanId");

                    b.HasIndex("NguoiTaoId");

                    b.ToTable("BaoCao");
                });

            modelBuilder.Entity("DACS_Web.Models.DanhGia", b =>
                {
                    b.Property<int>("DanhGiaId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("DanhGiaId"));

                    b.Property<DateTime>("NgayDang")
                        .HasColumnType("datetime2");

                    b.Property<string>("NguoiNhanId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("NguoiTaoId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("NoiDung")
                        .IsRequired()
                        .HasMaxLength(1000)
                        .HasColumnType("nvarchar(1000)");

                    b.HasKey("DanhGiaId");

                    b.HasIndex("NguoiNhanId");

                    b.HasIndex("NguoiTaoId");

                    b.ToTable("DanhGia");
                });

            modelBuilder.Entity("DACS_Web.Models.DanhSach", b =>
                {
                    b.Property<int>("DanhSachId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("DanhSachId"));

                    b.Property<string>("TenDS")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("DanhSachId");

                    b.HasIndex("UserId");

                    b.ToTable("DanhSach");
                });

            modelBuilder.Entity("DACS_Web.Models.DanhSachAnh", b =>
                {
                    b.Property<int>("DanhSachAnhId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("DanhSachAnhId"));

                    b.Property<int>("BaiDangId")
                        .HasColumnType("int");

                    b.Property<string>("Url")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("DanhSachAnhId");

                    b.HasIndex("BaiDangId");

                    b.ToTable("DanhSachAnh");
                });

            modelBuilder.Entity("DACS_Web.Models.DanhSach_BaiDang", b =>
                {
                    b.Property<int>("DanhSachId")
                        .HasColumnType("int");

                    b.Property<int>("BaiDangId")
                        .HasColumnType("int");

                    b.HasKey("DanhSachId", "BaiDangId");

                    b.HasIndex("BaiDangId");

                    b.ToTable("DanhSach_BaiDang");
                });

            modelBuilder.Entity("DACS_Web.Models.DichVu", b =>
                {
                    b.Property<int>("DichVuId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("DichVuId"));

                    b.Property<decimal?>("Gia")
                        .IsRequired()
                        .HasColumnType("decimal(18,2)");

                    b.Property<string>("TenDV")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<bool>("TrangThai")
                        .HasColumnType("bit");

                    b.HasKey("DichVuId");

                    b.ToTable("DinhVu");
                });

            modelBuilder.Entity("DACS_Web.Models.HoaDon", b =>
                {
                    b.Property<int>("HoaDonId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("HoaDonId"));

                    b.Property<int?>("BaiDangId")
                        .HasColumnType("int");

                    b.Property<int>("DichVuId")
                        .HasColumnType("int");

                    b.Property<DateTime>("NgayLap")
                        .HasColumnType("datetime2");

                    b.Property<string>("NoiDung")
                        .IsRequired()
                        .HasMaxLength(1000)
                        .HasColumnType("nvarchar(1000)");

                    b.Property<decimal>("TongTien")
                        .HasColumnType("decimal(18,2)");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("HoaDonId");

                    b.HasIndex("BaiDangId");

                    b.HasIndex("DichVuId");

                    b.HasIndex("UserId");

                    b.ToTable("HoaDon");
                });

            modelBuilder.Entity("DACS_Web.Models.LoaiBaiDang", b =>
                {
                    b.Property<int>("LoaiBaiDangId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("LoaiBaiDangId"));

                    b.Property<string>("TenLBD")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("LoaiBaiDangId");

                    b.ToTable("LoaiBaiDang");
                });

            modelBuilder.Entity("DACS_Web.Models.LoaiPhong", b =>
                {
                    b.Property<int>("LoaiPhongId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("LoaiPhongId"));

                    b.Property<string>("TenLP")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.HasKey("LoaiPhongId");

                    b.ToTable("LoaiPhong");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRole", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasDatabaseName("RoleNameIndex")
                        .HasFilter("[NormalizedName] IS NOT NULL");

                    b.ToTable("AspNetRoles", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("ClaimType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("RoleId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("ClaimType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.Property<string>("LoginProvider")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ProviderKey")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ProviderDisplayName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("RoleId")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("LoginProvider")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Value")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens", (string)null);
                });

            modelBuilder.Entity("DACS_Web.Models.BaiDang", b =>
                {
                    b.HasOne("DACS_Web.Models.LoaiBaiDang", "LoaiBaiDang")
                        .WithMany("baiDangs")
                        .HasForeignKey("LoaiBaiDangId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("DACS_Web.Models.LoaiPhong", "LoaiPhong")
                        .WithMany("baiDangs")
                        .HasForeignKey("LoaiPhongId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("DACS_Web.Models.ApplicationUser", "User")
                        .WithMany("baiDangs")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("LoaiBaiDang");

                    b.Navigation("LoaiPhong");

                    b.Navigation("User");
                });

            modelBuilder.Entity("DACS_Web.Models.BaoCao", b =>
                {
                    b.HasOne("DACS_Web.Models.ApplicationUser", "NguoiNhan")
                        .WithMany("BaoCaosReceivedByUser")
                        .HasForeignKey("NguoiNhanId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("DACS_Web.Models.ApplicationUser", "NguoiTao")
                        .WithMany("BaoCaosCreatedByUser")
                        .HasForeignKey("NguoiTaoId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("NguoiNhan");

                    b.Navigation("NguoiTao");
                });

            modelBuilder.Entity("DACS_Web.Models.DanhGia", b =>
                {
                    b.HasOne("DACS_Web.Models.ApplicationUser", "NguoiNhan")
                        .WithMany("DanhGiasReceivedByUser")
                        .HasForeignKey("NguoiNhanId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("DACS_Web.Models.ApplicationUser", "NguoiTao")
                        .WithMany("DanhGiasCreatedByUser")
                        .HasForeignKey("NguoiTaoId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("NguoiNhan");

                    b.Navigation("NguoiTao");
                });

            modelBuilder.Entity("DACS_Web.Models.DanhSach", b =>
                {
                    b.HasOne("DACS_Web.Models.ApplicationUser", "User")
                        .WithMany("DanhSachs")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("DACS_Web.Models.DanhSachAnh", b =>
                {
                    b.HasOne("DACS_Web.Models.BaiDang", "BaiDang")
                        .WithMany("danhSachAnhs")
                        .HasForeignKey("BaiDangId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("BaiDang");
                });

            modelBuilder.Entity("DACS_Web.Models.DanhSach_BaiDang", b =>
                {
                    b.HasOne("DACS_Web.Models.BaiDang", "BaiDang")
                        .WithMany()
                        .HasForeignKey("BaiDangId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("DACS_Web.Models.DanhSach", "DanhSach")
                        .WithMany()
                        .HasForeignKey("DanhSachId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("BaiDang");

                    b.Navigation("DanhSach");
                });

            modelBuilder.Entity("DACS_Web.Models.HoaDon", b =>
                {
                    b.HasOne("DACS_Web.Models.BaiDang", "BaiDang")
                        .WithMany("hoaDons")
                        .HasForeignKey("BaiDangId");

                    b.HasOne("DACS_Web.Models.DichVu", "DichVu")
                        .WithMany("hoaDons")
                        .HasForeignKey("DichVuId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("DACS_Web.Models.ApplicationUser", "User")
                        .WithMany("hoaDons")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("BaiDang");

                    b.Navigation("DichVu");

                    b.Navigation("User");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.HasOne("DACS_Web.Models.ApplicationUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.HasOne("DACS_Web.Models.ApplicationUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("DACS_Web.Models.ApplicationUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.HasOne("DACS_Web.Models.ApplicationUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("DACS_Web.Models.ApplicationUser", b =>
                {
                    b.Navigation("BaoCaosCreatedByUser");

                    b.Navigation("BaoCaosReceivedByUser");

                    b.Navigation("DanhGiasCreatedByUser");

                    b.Navigation("DanhGiasReceivedByUser");

                    b.Navigation("DanhSachs");

                    b.Navigation("baiDangs");

                    b.Navigation("hoaDons");
                });

            modelBuilder.Entity("DACS_Web.Models.BaiDang", b =>
                {
                    b.Navigation("danhSachAnhs");

                    b.Navigation("hoaDons");
                });

            modelBuilder.Entity("DACS_Web.Models.DichVu", b =>
                {
                    b.Navigation("hoaDons");
                });

            modelBuilder.Entity("DACS_Web.Models.LoaiBaiDang", b =>
                {
                    b.Navigation("baiDangs");
                });

            modelBuilder.Entity("DACS_Web.Models.LoaiPhong", b =>
                {
                    b.Navigation("baiDangs");
                });
#pragma warning restore 612, 618
        }
    }
}
