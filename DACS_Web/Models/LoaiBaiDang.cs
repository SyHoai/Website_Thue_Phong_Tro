using System.ComponentModel.DataAnnotations;

namespace DACS_Web.Models
{
    public class LoaiBaiDang
    {
        [Key]
        public int LoaiBaiDangId { get; set; }

        [Required, MaxLength(50)]
        public string TenLBD { get; set; }

        public List<BaiDang> baiDangs { get; set; }
    }
}
