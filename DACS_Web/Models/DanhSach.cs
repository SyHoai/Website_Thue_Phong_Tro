using System.ComponentModel.DataAnnotations;

namespace DACS_Web.Models
{
    public class DanhSach
    {
        [Key]
        public int DanhSachId { get; set; }

        [Required, StringLength(50)]
        public string TenDS { get; set; }


        public string UserId { get; set; }
        public ApplicationUser User { get; set; }
    }
}
