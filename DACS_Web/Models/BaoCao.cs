using System.ComponentModel.DataAnnotations;

namespace DACS_Web.Models
{
    public class BaoCao
    {
        [Key]
        public int BaoCaoId { get; set; }

        public DateTime NgayTao { get; set; }


        [Required, MaxLength(1000)]
        public string NoiDung { get; set; }

        public string NguoiNhanId { get; set; }
        public ApplicationUser NguoiNhan { get; set; }

        public string NguoiTaoId { get; set; }
        public ApplicationUser NguoiTao { get; set; }
    }
}
