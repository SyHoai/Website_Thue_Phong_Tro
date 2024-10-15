using System.ComponentModel.DataAnnotations;

namespace DACS_Web.Models
{
    public class HoaDon
    {
        [Key]
        public int HoaDonId { get; set; }
        public DateTime NgayLap { get; set; }

        [Range(0, (double)decimal.MaxValue)]
        public decimal TongTien { get; set; }

        [MaxLength(1000)]
        public string NoiDung { get; set; }


        public int? BaiDangId { get; set; }
        public BaiDang? BaiDang { get; set; }

        public int DichVuId {  get; set; }
        public DichVu DichVu { get; set; }

        public string UserId { get; set; }
        public ApplicationUser User { get; set; }
    }
}
