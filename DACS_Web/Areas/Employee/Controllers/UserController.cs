using DACS_Web.Models;
using DACS_Web.Orthers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.EntityFrameworkCore.Query.Internal;
using MoMo;
using Newtonsoft.Json.Linq;
using System.Security.Claims;

namespace DACS_Web.Areas.Employee.Controllers
{
    [Area("Employee")]
    [Authorize(Roles = "Employee")]
    public class UserController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public UserController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }


        public async Task<IActionResult> Details(string? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var user = await _userManager.FindByNameAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            
            return View(user);
        }

        
        public async Task<IActionResult> DanhGia(string id, int pageNumber = 1)
        {

            List<DanhGia> binhLuans = await _context.DanhGia.Where(p => p.NguoiNhan.UserName == id).OrderByDescending(p => p.NgayDang).ToListAsync();
            int pageSize = 5;
            int totalPages = (int)Math.Ceiling((double)binhLuans.Count() / pageSize);
            pageNumber = Math.Max(1, Math.Min(pageNumber, totalPages));

            var paginatedBL = binhLuans
                                    .Skip((pageNumber - 1) * pageSize)
                                    .Take(pageSize)
                                    .ToList();
            ViewBag.TotalPages = totalPages;
            ViewBag.PageNumber = pageNumber;
            ViewBag.NguoiNhan = await _userManager.FindByNameAsync(id);
            return View(paginatedBL);
        }

        [HttpPost]
        public async Task<IActionResult> AddDanhGia(string textNoiDung, string idNguoiNhan)
        {

            string userId = User.FindFirstValue(System.Security.Claims.ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
            {
                return Redirect("/Identity/Account/Login");
            }
            ApplicationUser user = await _userManager.FindByIdAsync(idNguoiNhan);
            DanhGia danhGia = new DanhGia();
            danhGia.NgayDang = DateTime.Now;
            danhGia.NoiDung = textNoiDung;
            danhGia.NguoiTaoId = userId;
            danhGia.NguoiNhan = user;



            _context.DanhGia.Add(danhGia);
            _context.SaveChanges();


            return RedirectToAction("DanhGia", new { id = user.UserName });
        }


        public async Task<IActionResult> QuanLi(int pageNumber = 1)
        {
            var members = await _userManager.GetUsersInRoleAsync("Member");
            var activeMembers = members.Where(p => p.TrangThai == true).ToList();
            ViewBag.Name = "Danh sách tài khoản hoạt động";

            int pageSize = 9;
            int totalPages = (int)Math.Ceiling((double)activeMembers.Count() / pageSize);
            pageNumber = Math.Max(1, Math.Min(pageNumber, totalPages));

            var listUser = activeMembers
                                    .Skip((pageNumber - 1) * pageSize)
                                    .Take(pageSize)
                                    .ToList();
            ViewBag.TotalPages = totalPages;
            ViewBag.PageNumber = pageNumber;
            return View(listUser);

        }

        public async Task<IActionResult> DanhSachHoatDong(int pageNumber = 1)
        {
            var members = await _userManager.GetUsersInRoleAsync("Member");
            var activeMembers = members.Where(p => p.TrangThai == true).ToList();
            ViewBag.Name = "Danh sách tài khoản hoạt động";
            int pageSize = 9;
            int totalPages = (int)Math.Ceiling((double)activeMembers.Count() / pageSize);
            pageNumber = Math.Max(1, Math.Min(pageNumber, totalPages));

            var listUser = activeMembers
                                    .Skip((pageNumber - 1) * pageSize)
                                    .Take(pageSize)
                                    .ToList();
            ViewBag.TotalPages = totalPages;
            ViewBag.PageNumber = pageNumber;
            return View("QuanLi", listUser);
        }

        public async Task<IActionResult> DanhSachKhoa(int pageNumber = 1)
        {
            var members = await _userManager.GetUsersInRoleAsync("Member");
            var activeMembers = members.Where(p => p.TrangThai == false).ToList();
            ViewBag.Name = "Danh sách tài khoản khóa";
            int pageSize = 9;
            int totalPages = (int)Math.Ceiling((double)activeMembers.Count() / pageSize);
            pageNumber = Math.Max(1, Math.Min(pageNumber, totalPages));

            var listUser = activeMembers
                                    .Skip((pageNumber - 1) * pageSize)
                                    .Take(pageSize)
                                    .ToList();
            ViewBag.TotalPages = totalPages;
            ViewBag.PageNumber = pageNumber;
            return View("QuanLi", listUser);
        }


        public async Task<IActionResult> TrangThai(string id, string actionName)
        {

            var user = await _userManager.FindByIdAsync(id);
            

            user.TrangThai = user.TrangThai == true ? false: true;
            var baiDangs = await _context.BaiDang.Where(p => p.User == user).ToListAsync();
            foreach (BaiDang baiDang in baiDangs)
            {
                baiDang.TrangThai = false;
                _context.BaiDang.Update(baiDang);
            }
            await _userManager.UpdateAsync(user);
            _context.SaveChanges();

            if (actionName == "QuanLi")
                return RedirectToAction(nameof(QuanLi));
            else
                return RedirectToAction(nameof(ThongKeBaoCao));
        }

        public async Task<IActionResult> DanhSachBaoCao(int pageNumber = 1)
        {
            List<BaoCao> listBaoCao = await _context.BaoCao.ToListAsync();
            ViewBag.Name = "Danh sách báo cáo";
            int pageSize = 10;
            int totalPages = (int)Math.Ceiling((double)listBaoCao.Count() / pageSize);
            pageNumber = Math.Max(1, Math.Min(pageNumber, totalPages));

            var BaoCaos = listBaoCao
                                .Skip((pageNumber - 1) * pageSize)
                                .Take(pageSize)
                                .OrderByDescending(p => p.NgayTao)
                                .ToList();
            ViewBag.TotalPages = totalPages;
            ViewBag.PageNumber = pageNumber;
            return View(BaoCaos);
        }
        public async Task<IActionResult> ThongKeBaoCao(int pageNumber = 1)
        {
            Dictionary<string,int> ThongKeBaoCao = new Dictionary<string, int>();
            List<BaoCao> listBaoCao = await _context.BaoCao.ToListAsync();
            foreach (BaoCao baoCao in listBaoCao)
            {
                if (ThongKeBaoCao.ContainsKey(baoCao.NguoiNhanId))
                {
                    ThongKeBaoCao[baoCao.NguoiNhanId]++;
                }
                else
                {
                    ThongKeBaoCao[baoCao.NguoiNhanId] = 1;
                }
            }
            ViewBag.Name = "Thống kê báo cáo";
            int pageSize = 10;
            int totalPages = (int)Math.Ceiling((double)ThongKeBaoCao.Count() / pageSize);
            pageNumber = Math.Max(1, Math.Min(pageNumber, totalPages));

            var BaoCaos = ThongKeBaoCao
                            .Skip((pageNumber - 1) * pageSize)
                            .Take(pageSize)
                            .OrderByDescending(p => p.Value)
                            .ToDictionary(p => p.Key, p => p.Value);
            ViewBag.TotalPages = totalPages;
            ViewBag.PageNumber = pageNumber;
            return View(BaoCaos);
        }
        public async Task<IActionResult> ChiTietBaoCao(int id)
        {
            BaoCao baoCao =await _context.BaoCao.FindAsync(id);

            return View(baoCao);
        }

        public async Task<IActionResult> BaoCao(string id, int pageNumber = 1)
        {

            List<BaoCao> binhLuans = await _context.BaoCao.Where(p => p.NguoiNhan.UserName == id).OrderByDescending(p => p.NgayTao).ToListAsync();
            int pageSize = 5;
            int totalPages = (int)Math.Ceiling((double)binhLuans.Count() / pageSize);
            pageNumber = Math.Max(1, Math.Min(pageNumber, totalPages));

            var paginatedBC = binhLuans
                                    .Skip((pageNumber - 1) * pageSize)
                                    .Take(pageSize)
                                    .ToList();
            ViewBag.TotalPages = totalPages;
            ViewBag.PageNumber = pageNumber;
            ViewBag.NguoiNhan = await _userManager.FindByNameAsync(id);
            return View(paginatedBC);
        }

        public async Task<IActionResult> LichSuGiaoDich(string id ,int pageNumber = 1)
        {

            ApplicationUser user = await _userManager.FindByNameAsync(id);
            List<HoaDon> hoaDons = await _context.HoaDon.Where(p => p.UserId == user.Id).OrderByDescending(p => p.NgayLap).ToListAsync();

            int pageSize = 9;
            int totalPages = (int)Math.Ceiling((double)hoaDons.Count() / pageSize);
            pageNumber = Math.Max(1, Math.Min(pageNumber, totalPages));

            var listHoaDon = hoaDons
                                    .Skip((pageNumber - 1) * pageSize)
                                    .Take(pageSize)
                                    .ToList();
            ViewBag.TotalPages = totalPages;
            ViewBag.PageNumber = pageNumber;
            return View(listHoaDon);


        }
    }
        
}
