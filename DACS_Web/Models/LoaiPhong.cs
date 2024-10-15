using System.ComponentModel.DataAnnotations;

namespace DACS_Web.Models
{
    public class LoaiPhong
    {
        [Key]
        public int LoaiPhongId { get; set; }

        [Required, MaxLength(100)]
        public string TenLP { get; set; }

        public List<BaiDang> baiDangs { get; set; }
    }
}
