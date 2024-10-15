using System.ComponentModel.DataAnnotations;

namespace DACS_Web.Models
{
    public class BaiDang
    {
        [Key]
        public int BaiDangId { get; set; }

        [Required, MaxLength(1000)]
        public string TenBD { get; set; }

        [Range(0, double.MaxValue)]
        public float DienTich { get; set; }

        [Required, MaxLength(50)]
        public string TinhThanh { get; set; }
        [Required, MaxLength(50)]
        public string QuanHuyen { get; set; }
        [Required, MaxLength(50)]
        public string PhuongXa { get; set; }
        [Required, MaxLength(100)]
        public string DiaChi { get; set; }
        [MaxLength(1000)]
        public string MoTa { get; set; }
        [Required, Range(0, (double)decimal.MaxValue)]
        public decimal TienThue { get; set; }
        [Range(0, (double)decimal.MaxValue)]
        public decimal TienCoc { get; set; }

        public DateTime NgayDang { get; set; }

        public DateTime NgayHetHan { get; set; }

        public DateTime? HanDichVu { get; set; }

        public bool TrangThai { get; set; }

        public int Duyet {  get; set; }
        public List<DanhSachAnh> danhSachAnhs { get; set; }

        public int LoaiPhongId { get; set; }
        public LoaiPhong LoaiPhong { get; set; }

        public string UserId { get; set; }
        public ApplicationUser User { get; set; }

        public List<HoaDon> hoaDons { get; set; }

        public int LoaiBaiDangId {  get; set; }
        public LoaiBaiDang LoaiBaiDang { get; set; }  
    }
}
