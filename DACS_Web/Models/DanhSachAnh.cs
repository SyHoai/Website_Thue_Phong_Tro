using System.ComponentModel.DataAnnotations;

namespace DACS_Web.Models
{
    public class DanhSachAnh
    {
        [Key]
        public int DanhSachAnhId { get; set; }

        [Required]
        public string Url { get; set; }

        public int BaiDangId { get; set; }
        public BaiDang BaiDang { get; set; }
    }
}
