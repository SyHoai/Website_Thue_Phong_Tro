namespace DACS_Web.Models
{
    public class DanhSach_BaiDang
    {
        public int DanhSachId { get; set; }
        public DanhSach DanhSach { get; set; }

        public int BaiDangId { get; set; }
        public BaiDang BaiDang { get; set; }
    }
}
