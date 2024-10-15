using DACS_Web.Models;
using DACS_Web.Orthers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.Internal;
using MoMo;
using Newtonsoft.Json.Linq;
using System.Security.Claims;

namespace DACS_Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class UserController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public UserController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        public IActionResult Index()
        {
            return View();
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
            var isInRole = await _userManager.IsInRoleAsync(user, "Member");
            if (isInRole)
                return View(user);
            else
                return View("NhanVienDetails", user);
        }

        public async Task<IActionResult> LichSuGiaoDich(string id, int pageNumber = 1)
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

        public async Task<IActionResult> QuanLiNguoiDung(int pageNumber = 1)
        {
            var members = await _userManager.GetUsersInRoleAsync("Member");
            var activeMembers = members.Where(p => p.TrangThai == true).ToList();
            ViewBag.Name = "Danh sách tài khoản người dùng hoạt động";

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

        public async Task<IActionResult> QuanLiNhanVien(int pageNumber = 1)
        {
            var employee = await _userManager.GetUsersInRoleAsync("Employee");
            var activeEmployee = employee.Where(p => p.TrangThai == true).ToList();
            ViewBag.Name = "Danh sách tài khoản nhân viên hoạt động";

            int pageSize = 9;
            int totalPages = (int)Math.Ceiling((double)activeEmployee.Count() / pageSize);
            pageNumber = Math.Max(1, Math.Min(pageNumber, totalPages));

            var listUser = activeEmployee
                                    .Skip((pageNumber - 1) * pageSize)
                                    .Take(pageSize)
                                    .ToList();
            ViewBag.TotalPages = totalPages;
            ViewBag.PageNumber = pageNumber;
            return View(listUser);

        }


        public async Task<IActionResult> TrangThai(string id, string actionName)
        {

            var user = await _userManager.FindByIdAsync(id);


            user.TrangThai = user.TrangThai == true ? false : true;
            var baiDangs = await _context.BaiDang.Where(p=>p.User == user).ToListAsync();
            foreach(BaiDang baiDang in baiDangs)
            {
                baiDang.TrangThai = false;
                _context.BaiDang.Update(baiDang);
            }
            await _userManager.UpdateAsync(user);
            _context.SaveChanges();
            
            if (actionName == "NguoiDung") 
                return RedirectToAction(nameof(QuanLiNguoiDung));
            else
                return RedirectToAction(nameof(QuanLiNhanVien));
        }

        public async Task<IActionResult> DanhSachNguoiDungHoatDong(int pageNumber = 1)
        {
            var members = await _userManager.GetUsersInRoleAsync("Member");
            var activeMembers = members.Where(p => p.TrangThai == true).ToList();
            ViewBag.Name = "Danh sách tài khoản người dùng hoạt động";
            int pageSize = 9;
            int totalPages = (int)Math.Ceiling((double)activeMembers.Count() / pageSize);
            pageNumber = Math.Max(1, Math.Min(pageNumber, totalPages));

            var listUser = activeMembers
                                    .Skip((pageNumber - 1) * pageSize)
                                    .Take(pageSize)
                                    .ToList();
            ViewBag.TotalPages = totalPages;
            ViewBag.PageNumber = pageNumber;
            return View("QuanLiNguoiDung", listUser);
        }

        public async Task<IActionResult> DanhSachNguoiDungKhoa(int pageNumber = 1)
        {
            var members = await _userManager.GetUsersInRoleAsync("Member");
            var blockMembers = members.Where(p => p.TrangThai == false).ToList();
            ViewBag.Name = "Danh sách tài khoản người dùng khóa";
            int pageSize = 9;
            int totalPages = (int)Math.Ceiling((double)blockMembers.Count() / pageSize);
            pageNumber = Math.Max(1, Math.Min(pageNumber, totalPages));

            var listUser = blockMembers
                                    .Skip((pageNumber - 1) * pageSize)
                                    .Take(pageSize)
                                    .ToList();
            ViewBag.TotalPages = totalPages;
            ViewBag.PageNumber = pageNumber;
            return View("QuanLiNguoiDung", listUser);
        }

        public async Task<IActionResult> DanhSachNhanVienHoatDong(int pageNumber = 1)
        {
            var employee = await _userManager.GetUsersInRoleAsync("Employee");
            var activeEmployee = employee.Where(p => p.TrangThai == true).ToList();
            ViewBag.Name = "Danh sách tài khoản nhân viên hoạt động";
            int pageSize = 9;
            int totalPages = (int)Math.Ceiling((double)activeEmployee.Count() / pageSize);
            pageNumber = Math.Max(1, Math.Min(pageNumber, totalPages));

            var listUser = activeEmployee
                                    .Skip((pageNumber - 1) * pageSize)
                                    .Take(pageSize)
                                    .ToList();
            ViewBag.TotalPages = totalPages;
            ViewBag.PageNumber = pageNumber;
            return View("QuanLiNhanVien", listUser);
        }

        public async Task<IActionResult> DanhSachNhanVienKhoa(int pageNumber = 1)
        {

            var employee = await _userManager.GetUsersInRoleAsync("Employee");
            var blockEmployee = employee.Where(p => p.TrangThai == false).ToList();
            ViewBag.Name = "Danh sách tài khoản nhân viên khóa";
            int pageSize = 9;
            int totalPages = (int)Math.Ceiling((double)blockEmployee.Count() / pageSize);
            pageNumber = Math.Max(1, Math.Min(pageNumber, totalPages));

            var listUser = blockEmployee
                                    .Skip((pageNumber - 1) * pageSize)
                                    .Take(pageSize)
                                    .ToList();
            ViewBag.TotalPages = totalPages;
            ViewBag.PageNumber = pageNumber;
            return View("QuanLiNhanVien", listUser);
        }

       

        
    }
        
}
