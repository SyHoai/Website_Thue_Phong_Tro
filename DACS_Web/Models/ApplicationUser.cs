using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace DACS_Web.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string? Avatar { get; set; }
        [Required, MaxLength(50)]
        public string HoTen { get; set; }
        public DateTime NgaySinh { get; set; }


        [Required, MaxLength(50)]
        public string TinhThanh { get; set; }
        [Required, MaxLength(50)]
        public string QuanHuyen { get; set; }
        [Required, MaxLength(50)]
        public string PhuongXa { get; set; }

        [Required,MaxLength(100)]
        public string DiaChi { get; set; }

        public DateTime NgayThamGia { get; set; }
        public bool TrangThai { get; set; }

        [Range(0, (double)decimal.MaxValue)]
        public decimal? SoDu { get; set; }

        public List<DanhGia> DanhGiasCreatedByUser { get; set; }
        public List<DanhGia> DanhGiasReceivedByUser { get; set; }

        public List<BaoCao> BaoCaosCreatedByUser { get; set; }
        public List<BaoCao> BaoCaosReceivedByUser { get; set; }
        

        public List<BaiDang> baiDangs { get; set; }

        public List<DanhSach> DanhSachs { get; set; }

        public List<HoaDon> hoaDons { get; set; }
    }
}
