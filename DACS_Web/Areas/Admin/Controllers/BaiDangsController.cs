using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DACS_Web.Models;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace DACS_Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class BaiDangsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public BaiDangsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Admin/BaiDangs
        public async Task<IActionResult> Index(string sapxep,int pageNumber = 1)
        {
            ViewData["LoaiPhongId"] = new SelectList(_context.LoaiPhong, "LoaiPhongId", "TenLP");
            var applicationDbContext = await _context.BaiDang
                                    .Where(p => p.TrangThai == true && p.Duyet == 2 && p.NgayHetHan > DateTime.Now)
                                    .Include(b => b.LoaiBaiDang)
                                    .Include(b => b.LoaiPhong)
                                    .Include(b => b.User)
                                    .ToListAsync();

            var listBaiDang = applicationDbContext
                                    .OrderByDescending(b => b.LoaiBaiDangId == 3)
                                    .ThenByDescending(b => b.NgayDang);

            if (sapxep == "gia giam")
            {
                listBaiDang = applicationDbContext
                                    .OrderByDescending(b => b.LoaiBaiDangId == 3)
                                    .ThenByDescending(b => b.TienThue);
            }
            else if (sapxep == "gia tang")
            {
                listBaiDang = applicationDbContext
                                  .OrderByDescending(b => b.LoaiBaiDangId == 3)
                                  .ThenBy(b => b.TienThue);
            }
            else
            {
                listBaiDang = applicationDbContext
                                   .OrderByDescending(b => b.LoaiBaiDangId == 3)
                                   .ThenByDescending(b => b.NgayDang);
            }


            LoaiBaiDang loaiBaiDang = await _context.LoaiBaiDang.FirstOrDefaultAsync(p => p.LoaiBaiDangId == 1);


            foreach (var BaiDang in applicationDbContext)
            {
                if (BaiDang.HanDichVu != null)
                {
                    if (BaiDang.HanDichVu < DateTime.Now)
                    {
                        BaiDang.HanDichVu = null;
                        BaiDang.LoaiBaiDang = loaiBaiDang;

                        _context.BaiDang.Update(BaiDang);


                    }
                }

            }
            await _context.SaveChangesAsync();

            int pageSize = 9;
            int totalPages = (int)Math.Ceiling((double)applicationDbContext.Count() / pageSize);
            pageNumber = Math.Max(1, Math.Min(pageNumber, totalPages));

            var baiDangs = listBaiDang
                            .Skip((pageNumber - 1) * pageSize)
                            .Take(pageSize)
                            .ToList();

            ViewBag.SapXep = sapxep;
            ViewBag.TotalPages = totalPages;
            ViewBag.PageNumber = pageNumber;
            return View(baiDangs);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var baiDang = await _context.BaiDang
                .Include(b => b.LoaiBaiDang)
                .Include(b => b.LoaiPhong)
                .Include(b => b.User)
                .FirstOrDefaultAsync(m => m.BaiDangId == id);
            if (baiDang == null)
            {
                return NotFound();
            }

            return View(baiDang);
        }

 
        

        private bool BaiDangExists(int id)
        {
            return _context.BaiDang.Any(e => e.BaiDangId == id);
        }

        public async Task<IActionResult> Duyet(int id)
        {
            var baiDang = await _context.BaiDang.FindAsync(id);
            if (baiDang != null)
            {

                baiDang.Duyet = 2;
                baiDang.TrangThai = true;
                _context.Update(baiDang);
            }

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }


        public async Task<IActionResult> LocKetQua(string sapxep,string selectedTinhThanh, string selectedQuanHuyen, string selectedPhuongXa, string selectedLoaiPhong, int pageNumber = 1)
        {
            if (selectedTinhThanh == "Toàn quốc")
            {
                selectedTinhThanh = null;
            }
            if (selectedQuanHuyen == "Tất cả")
            {
                selectedQuanHuyen = null;
            }
            if (selectedPhuongXa == "Tất cả")
            {
                selectedPhuongXa = null;
            }
            if (selectedLoaiPhong == "Tất cả")
            {
                selectedLoaiPhong = null;
            }
            var ketQua = await _context.BaiDang
                        .Where(item =>
                            item.TrangThai == true && item.Duyet == 2 && item.NgayHetHan > DateTime.Now &&
                            (string.IsNullOrEmpty(selectedTinhThanh) || item.TinhThanh == selectedTinhThanh) &&
                            (string.IsNullOrEmpty(selectedQuanHuyen) || item.QuanHuyen == selectedQuanHuyen) &&
                            (string.IsNullOrEmpty(selectedPhuongXa) || item.PhuongXa == selectedPhuongXa) &&
                            (string.IsNullOrEmpty(selectedLoaiPhong) || item.LoaiPhong.TenLP == selectedLoaiPhong))
                        .Include(b => b.LoaiBaiDang)
                        .Include(b => b.LoaiPhong)
                        .Include(b => b.User)
                        .ToListAsync();

            var listBaiDang = ketQua
                                   .OrderByDescending(b => b.LoaiBaiDangId == 3)
                                   .ThenByDescending(b => b.NgayDang);

            if (sapxep == "gia giam")
            {
                listBaiDang = ketQua
                                    .OrderByDescending(b => b.LoaiBaiDangId == 3)
                                    .ThenByDescending(b => b.TienThue);
            }
            else if (sapxep == "gia tang")
            {
                listBaiDang = ketQua
                                  .OrderByDescending(b => b.LoaiBaiDangId == 3)
                                  .ThenBy(b => b.TienThue);
            }
            else
            {
                listBaiDang = ketQua
                                   .OrderByDescending(b => b.LoaiBaiDangId == 3)
                                   .ThenByDescending(b => b.NgayDang);
            }

            ViewData["LoaiPhongId"] = new SelectList(_context.LoaiPhong, "LoaiPhongId", "TenLP");

            int pageSize = 9;
            int totalPages = (int)Math.Ceiling((double)ketQua.Count() / pageSize);
            pageNumber = Math.Max(1, Math.Min(pageNumber, totalPages));
            var baiDangs = listBaiDang
                            .Skip((pageNumber - 1) * pageSize)
                            .Take(pageSize)
                            .OrderByDescending(b => b.LoaiBaiDangId == 3)
                            .ToList();

            ViewBag.SapXep = sapxep;
            ViewBag.TotalPages = totalPages;
            ViewBag.PageNumber = pageNumber;
            ViewBag.selectedTinhThanh = selectedTinhThanh;
            ViewBag.selectedQuanHuyen = selectedQuanHuyen;
            ViewBag.selectedPhuongXa = selectedPhuongXa;
            ViewBag.selectedLoaiPhong = selectedLoaiPhong;
            return View("Index", baiDangs);
        }




        public async Task<IActionResult> Search(string sapxep,string query, int pageNumber = 1)
        {
            try
            {
                if (string.IsNullOrEmpty(query))
                    return BadRequest("Search query is required");
                var result = _context.BaiDang
                                .Where(p => 
                                    p.TrangThai == true && p.Duyet == 2 && p.NgayHetHan > DateTime.Now &&
                                    (p.TenBD.Contains(query) || p.PhuongXa.Contains(query) || p.QuanHuyen.Contains(query) || p.TinhThanh.Contains(query)) ||
                                    (p.TenBD.Contains(query) || (p.QuanHuyen + ", " + p.TinhThanh).Contains(query)) ||
                                    (p.TenBD.Contains(query) || (p.PhuongXa + ", " + p.QuanHuyen + ", " + p.TinhThanh).Contains(query)))
                                .Include(b => b.LoaiBaiDang)
                                .Include(b => b.LoaiPhong)
                                .Include(b => b.User)
                                .ToList();

                var listBaiDang = result
                                   .OrderByDescending(b => b.LoaiBaiDangId == 3)
                                   .ThenByDescending(b => b.NgayDang);

                if (sapxep == "gia giam")
                {
                    listBaiDang = result
                                        .OrderByDescending(b => b.LoaiBaiDangId == 3)
                                        .ThenByDescending(b => b.TienThue);
                }
                else if (sapxep == "gia tang")
                {
                    listBaiDang = result
                                      .OrderByDescending(b => b.LoaiBaiDangId == 3)
                                      .ThenBy(b => b.TienThue);
                }
                else
                {
                    listBaiDang = result
                                       .OrderByDescending(b => b.LoaiBaiDangId == 3)
                                       .ThenByDescending(b => b.NgayDang);
                }


                int pageSize = 9;
                int totalPages = (int)Math.Ceiling((double)result.Count() / pageSize);
                pageNumber = Math.Max(1, Math.Min(pageNumber, totalPages));
                var paginatedBaiDangs = listBaiDang
                                            .Skip((pageNumber - 1) * pageSize)
                                            .Take(pageSize)
                                            .ToList();
                ViewData["LoaiPhongId"] = new SelectList(_context.LoaiPhong, "LoaiPhongId", "TenLP");
                string userId = User.FindFirstValue(System.Security.Claims.ClaimTypes.NameIdentifier);
                DanhSach danhSach = await _context.DanhSach.FirstOrDefaultAsync(p => p.UserId == userId);

                ViewBag.SapXep = sapxep;
                ViewBag.DanhSach = danhSach;
                ViewBag.Query = query;
                ViewBag.TotalPages = totalPages;
                ViewBag.PageNumber = pageNumber;
                return View("Index", paginatedBaiDangs);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        public List<string> SearchSuggestions(string query)
        {

            List<string> listBaiDang = _context.BaiDang
                                .Where(p => p.TrangThai == true && p.Duyet == 2 && p.NgayHetHan > DateTime.Now && p.TenBD.Contains(query))
                                .Select(p => p.TenBD)
                                .ToList();
            List<string> listDiaChi = _context.BaiDang
                .Where(p => p.TrangThai == true && p.Duyet == 2 && p.NgayHetHan > DateTime.Now && (p.PhuongXa + ", " + p.QuanHuyen + ", " + p.TinhThanh).Contains(query))
                             .Select(s => s.PhuongXa + ", " + s.QuanHuyen + ", " + s.TinhThanh)
                             .ToList();
            List<string> combinedList = listBaiDang.Concat(listDiaChi).Take(5).ToList();
            return (combinedList);
        }
    }
}
