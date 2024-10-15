using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;

namespace DACS_Web.Models
{
    public class DanhGia
    {
        [Key]
        public int DanhGiaId { get; set; }

        public DateTime NgayDang { get; set; }

        [Required, MaxLength(1000)]
        public string NoiDung { get; set; }

        public string NguoiNhanId { get; set; }
        public ApplicationUser NguoiNhan { get; set; }

        public string NguoiTaoId {get; set; }
        public ApplicationUser NguoiTao { get; set; }
    }
}
