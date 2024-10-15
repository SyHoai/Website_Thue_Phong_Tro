using System.ComponentModel.DataAnnotations;

namespace DACS_Web.Models
{
    public class DichVu
    {
        [Key]
        public int DichVuId { get; set; }

        [Required, MaxLength(50)]
        public string TenDV { get; set; }

        [Required, Range(0, (double)decimal.MaxValue)]
        public decimal? Gia { get; set; }

        public bool TrangThai { get; set; }
        public List<HoaDon> hoaDons { get; set; }
    }
}
