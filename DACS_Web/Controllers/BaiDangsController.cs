using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DACS_Web.Models;
using System.Security.Claims;
using MoMo;
using Newtonsoft.Json.Linq;
using Microsoft.AspNetCore.Identity;

namespace DACS_Web.Controllers
{
    public class BaiDangsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        public static DateTime NgayDang;
        public static DateTime NgayHetHan;
        public BaiDangsController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: BaiDangs
        public async Task<IActionResult> Index(string sapxep,int pageNumber = 1)
        {

            ViewData["LoaiPhongId"] = new SelectList(_context.LoaiPhong, "LoaiPhongId", "TenLP");
            var applicationDbContext = await _context.BaiDang
                                    .Where(p=>p.TrangThai == true && p.Duyet ==2&& p.NgayHetHan > DateTime.Now)
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
            string userId = User.FindFirstValue(System.Security.Claims.ClaimTypes.NameIdentifier);
            DanhSach danhSach = await _context.DanhSach.FirstOrDefaultAsync(p=>p.UserId == userId);
            ViewBag.DanhSach = danhSach;
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

        // GET: BaiDangs/Details/5
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
            string userId = User.FindFirstValue(System.Security.Claims.ClaimTypes.NameIdentifier);
            DanhSach danhSach = await _context.DanhSach.FirstOrDefaultAsync(p => p.UserId == userId);
            ViewBag.DanhSach = danhSach;
            return View(baiDang);
        }

        // GET: BaiDangs/Create
        public IActionResult Create()
        {
            ViewData["LoaiBaiDangId"] = new SelectList(_context.LoaiBaiDang, "LoaiBaiDangId", "TenLBD");
            ViewData["LoaiPhongId"] = new SelectList(_context.LoaiPhong, "LoaiPhongId", "TenLP");
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id");
            return View();
        }

        // POST: BaiDangs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("BaiDangId,TenBD,DienTich,DiaChi,MoTa,TienThue,TienCoc,LoaiPhongId")] BaiDang baiDang, List<IFormFile> filses, string selectedTinhThanh, string selectedQuanHuyen, string selectedPhuongXa)
        {
            ModelState.Remove("danhSachAnhs");
            ModelState.Remove("LoaiPhongId");
            ModelState.Remove("LoaiPhong");
            ModelState.Remove("UserId");
            ModelState.Remove("hoaDons");
            ModelState.Remove("filses");
            ModelState.Remove("User");
            ModelState.Remove("LoaiBaiDang");
            ModelState.Remove("PhuongXa");
            ModelState.Remove("QuanHuyen");
            ModelState.Remove("TinhThanh");
            if (baiDang.TienCoc > baiDang.TienThue)
            {
                ModelState.AddModelError("TienCoc", "Tiền cọc phải nhỏ hơn hoặc bằng Giá.");
                return View(baiDang);
            }
            if (ModelState.IsValid)
            {
                foreach (IFormFile imagePath in filses)
                {

                    DanhSachAnh danhSachAnh = new DanhSachAnh();
                    danhSachAnh.Url = await SaveFile(imagePath);
                    danhSachAnh.BaiDang = baiDang;

                    _context.DanhSachAnh.Add(danhSachAnh);
                }
                string userId = User.FindFirstValue(System.Security.Claims.ClaimTypes.NameIdentifier);
                if (string.IsNullOrEmpty(userId))
                {
                    return Redirect("/Identity/Account/Login");
                }
                ApplicationUser user = await _userManager.FindByIdAsync(userId);
                baiDang.TinhThanh = selectedTinhThanh;
                baiDang.QuanHuyen = selectedQuanHuyen;
                baiDang.PhuongXa = selectedPhuongXa;
                baiDang.User = user;
                baiDang.Duyet = 0;
                baiDang.TrangThai = false;
                baiDang.NgayDang = DateTime.Now;
                baiDang.NgayHetHan = DateTime.Now.AddMonths(2);
                LoaiBaiDang loaiBaiDang = await _context.LoaiBaiDang.FindAsync(1);
                baiDang.LoaiBaiDang = loaiBaiDang;
                _context.Add(baiDang);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["LoaiBaiDangId"] = new SelectList(_context.LoaiBaiDang, "LoaiBaiDangId", "TenLBD", baiDang.LoaiBaiDangId);
            ViewData["LoaiPhongId"] = new SelectList(_context.LoaiPhong, "LoaiPhongId", "TenLP", baiDang.LoaiPhongId);
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", baiDang.UserId);
            return View(baiDang);
        }

        // GET: BaiDangs/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var baiDang = await _context.BaiDang.FindAsync(id);
            if (baiDang == null)
            {
                return NotFound();
            }

            NgayDang = baiDang.NgayDang;
            NgayHetHan = baiDang.NgayHetHan;
            ViewData["LoaiBaiDangId"] = new SelectList(_context.LoaiBaiDang, "LoaiBaiDangId", "TenLBD", baiDang.LoaiBaiDangId);
            ViewData["LoaiPhongId"] = new SelectList(_context.LoaiPhong, "LoaiPhongId", "TenLP", baiDang.LoaiPhongId);
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", baiDang.UserId);
            return View(baiDang);
        }

        // POST: BaiDangs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("BaiDangId,TenBD,DienTich,TinhThanh,QuanHuyen,PhuongXa,DiaChi,MoTa,TienThue,TienCoc,NgayDang,NgayHetHan,TrangThai,Duyet,LoaiPhongId,UserId,LoaiBaiDangId")] BaiDang baiDang, List<IFormFile> filses, string selectedTinhThanh, string selectedQuanHuyen, string selectedPhuongXa)
        {
            List<DanhSachAnh> danhSachAnhs = await _context.DanhSachAnh.Where(p=>p.BaiDangId == baiDang.BaiDangId).ToListAsync();
            ModelState.Remove("User");
            ModelState.Remove("UserId");
            ModelState.Remove("hoaDons");
            ModelState.Remove("PhuongXa");
            ModelState.Remove("QuanHuyen");
            ModelState.Remove("TinhThanh");
            ModelState.Remove("LoaiBaiDang");
            ModelState.Remove("danhSachAnhs");
            ModelState.Remove("LoaiPhongId");
            ModelState.Remove("LoaiPhong");
            if (baiDang.TienCoc > baiDang.TienThue)
            {
                ModelState.AddModelError("TienCoc", "Tiền cọc phải nhỏ hơn hoặc bằng tiền thuê.");
                return View(baiDang);
            }
            if (ModelState.IsValid)
            {
                foreach(DanhSachAnh danhSachAnh1 in danhSachAnhs)
                {
                    _context.DanhSachAnh.Remove(danhSachAnh1);
                }
                foreach (IFormFile imagePath in filses)
                {

                    DanhSachAnh danhSachAnh = new DanhSachAnh();
                    danhSachAnh.Url = await SaveFile(imagePath);
                    danhSachAnh.BaiDang = baiDang;

                    _context.DanhSachAnh.Add(danhSachAnh);
                }
                string userId = User.FindFirstValue(System.Security.Claims.ClaimTypes.NameIdentifier);
                if (string.IsNullOrEmpty(userId))
                {
                    return Redirect("/Identity/Account/Login");
                }
                ApplicationUser user = await _userManager.FindByIdAsync(userId);
                baiDang.User = user;
                baiDang.TinhThanh = selectedTinhThanh;
                baiDang.QuanHuyen = selectedQuanHuyen;
                baiDang.PhuongXa = selectedPhuongXa;
                baiDang.NgayDang = NgayDang;
                baiDang.NgayHetHan = NgayHetHan;
                baiDang.Duyet = 0;
                baiDang.TrangThai = false;
                LoaiBaiDang loaiBaiDang = await _context.LoaiBaiDang.FindAsync(1);
                baiDang.LoaiBaiDang = loaiBaiDang;
                try
                {
                    _context.Update(baiDang);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BaiDangExists(baiDang.BaiDangId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            
            ViewData["LoaiBaiDangId"] = new SelectList(_context.LoaiBaiDang, "LoaiBaiDangId", "TenLBD", baiDang.LoaiBaiDangId);
            ViewData["LoaiPhongId"] = new SelectList(_context.LoaiPhong, "LoaiPhongId", "TenLP", baiDang.LoaiPhongId);
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", baiDang.UserId);
            return View(baiDang);
        }


        private bool BaiDangExists(int id)
        {
            return _context.BaiDang.Any(e => e.BaiDangId == id);
        }


        private async Task<string> SaveFile(IFormFile file)
        {
            var savePath = Path.Combine("wwwroot/images/post", file.FileName);
            using (var fileStream = new FileStream(savePath, FileMode.Create))
            {
                await file.CopyToAsync(fileStream);
            }
            return "/images/post/" + file.FileName;

        }

        public async Task<IActionResult> QuanLi(int pageNumber = 1)
        {
            string userId = User.FindFirstValue(System.Security.Claims.ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
            {
                return Redirect("/Identity/Account/Login");
            }
            var applicationDbContext = await _context.BaiDang
                                            .Where(p=>p.UserId == userId&&p.TrangThai==true&&p.Duyet==2 &&p.NgayHetHan > DateTime.Now)
                                            .Include(b => b.LoaiBaiDang)
                                            .Include(b => b.LoaiPhong)
                                            .Include(b => b.User)
                                            .OrderByDescending(b => b.LoaiBaiDangId == 3)
                                            .ThenByDescending(b => b.NgayDang)
                                            .ToListAsync();
            

            int pageSize = 8;
            int totalPages = (int)Math.Ceiling((double)applicationDbContext.Count() / pageSize);
            pageNumber = Math.Max(1, Math.Min(pageNumber, totalPages));

            var baiDangs = applicationDbContext
                            .Skip((pageNumber - 1) * pageSize)
                            .Take(pageSize)
                            .ToList();

            ViewBag.Name = "Bài đăng đang hiển thị";
            ViewBag.TotalPages = totalPages;
            ViewBag.PageNumber = pageNumber;
            return View(baiDangs);
        }

        public async Task<IActionResult> LocKetQua(string sapxep,string selectedTinhThanh, string selectedQuanHuyen, string selectedPhuongXa,string selectedLoaiPhong, int pageNumber = 1)
        {
            if(selectedTinhThanh == "Toàn quốc")
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
            string userId = User.FindFirstValue(System.Security.Claims.ClaimTypes.NameIdentifier);
            DanhSach danhSach = await _context.DanhSach.FirstOrDefaultAsync(p => p.UserId == userId);
            ViewBag.DanhSach = danhSach;

            int pageSize = 9;
            int totalPages = (int)Math.Ceiling((double)ketQua.Count() / pageSize);
            pageNumber = Math.Max(1, Math.Min(pageNumber, totalPages));

            var baiDangs = listBaiDang
                            .Skip((pageNumber - 1) * pageSize)
                            .Take(pageSize)
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

        public async Task<IActionResult> DanhSachHienThi(int pageNumber = 1)
        {
            string userId = User.FindFirstValue(System.Security.Claims.ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
            {
                return Redirect("/Identity/Account/Login");
            }
            var applicationDbContext = await _context.BaiDang
                            .Where(p => p.UserId == userId && p.TrangThai==true&&p.Duyet==2 && p.NgayHetHan > DateTime.Now)
                            .Include(b => b.LoaiBaiDang)
                            .Include(b => b.LoaiPhong)
                            .Include(b => b.User)
                            .OrderByDescending(b => b.LoaiBaiDangId == 3)
                            .ThenByDescending(b => b.NgayDang)
                            .ToListAsync();

            

            int pageSize = 8;
            int totalPages = (int)Math.Ceiling((double)applicationDbContext.Count() / pageSize);
            pageNumber = Math.Max(1, Math.Min(pageNumber, totalPages));

            var baiDangs = applicationDbContext
                            .Skip((pageNumber - 1) * pageSize)
                            .Take(pageSize)
                            .ToList();

            ViewBag.Name = "Bài đăng đang hiển thị";
            ViewBag.TotalPages = totalPages;
            ViewBag.PageNumber = pageNumber;
            return View("QuanLi", baiDangs);
        }
        public async Task<IActionResult> DanhSachAn(int pageNumber = 1)
        {
            string userId = User.FindFirstValue(System.Security.Claims.ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
            {
                return Redirect("/Identity/Account/Login");
            }
            var applicationDbContext = await _context.BaiDang
                            .Where(p => p.UserId == userId && p.TrangThai == false && p.Duyet == 2 && p.NgayHetHan > DateTime.Now)
                            .Include(b => b.LoaiBaiDang)
                            .Include(b => b.LoaiPhong)
                            .Include(b => b.User)
                            .OrderByDescending(b => b.LoaiBaiDangId == 3)
                            .ThenByDescending(b => b.NgayDang)
                            .ToListAsync();


            int pageSize = 8;
            int totalPages = (int)Math.Ceiling((double)applicationDbContext.Count() / pageSize);
            pageNumber = Math.Max(1, Math.Min(pageNumber, totalPages));

            var baiDangs = applicationDbContext
                            .Skip((pageNumber - 1) * pageSize)
                            .Take(pageSize)
                            .ToList();
            ViewBag.Name = "Bài đăng đang ẩn";
            ViewBag.TotalPages = totalPages;
            ViewBag.PageNumber = pageNumber;
            return View("QuanLi", baiDangs);
        }

        public async Task<IActionResult> DanhSachHetHan(int pageNumber = 1)
        {
            string userId = User.FindFirstValue(System.Security.Claims.ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
            {
                return Redirect("/Identity/Account/Login");
            }
            var applicationDbContext = await _context.BaiDang
                            .Where(p => p.UserId == userId && p.Duyet == 2 && p.NgayHetHan < DateTime.Now)
                            .Include(b => b.LoaiBaiDang)
                            .Include(b => b.LoaiPhong)
                            .Include(b => b.User)
                            .OrderByDescending(b => b.LoaiBaiDangId == 3)
                            .ThenByDescending(b => b.NgayDang)
                            .ToListAsync();


            int pageSize = 8;
            int totalPages = (int)Math.Ceiling((double)applicationDbContext.Count() / pageSize);
            pageNumber = Math.Max(1, Math.Min(pageNumber, totalPages));

            var baiDangs = applicationDbContext
                            .Skip((pageNumber - 1) * pageSize)
                            .Take(pageSize)
                            .ToList();
            ViewBag.Name = "Bài đăng đang hết hạn";
            ViewBag.TotalPages = totalPages;
            ViewBag.PageNumber = pageNumber;
            return View("QuanLi", baiDangs);
        }
        public async Task<IActionResult> DanhSachTuChoi(int pageNumber = 1)
        {
            string userId = User.FindFirstValue(System.Security.Claims.ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
            {
                return Redirect("/Identity/Account/Login");
            }
            var applicationDbContext = await _context.BaiDang
                            .Where(p => p.UserId == userId && p.TrangThai == false && p.Duyet == 1 && p.NgayHetHan > DateTime.Now)
                            .Include(b => b.LoaiBaiDang)
                            .Include(b => b.LoaiPhong)
                            .Include(b => b.User)
                            .OrderByDescending(b => b.LoaiBaiDangId == 3)
                            .ThenByDescending(b => b.NgayDang)
                            .ToListAsync();

            int pageSize = 8;
            int totalPages = (int)Math.Ceiling((double)applicationDbContext.Count() / pageSize);
            pageNumber = Math.Max(1, Math.Min(pageNumber, totalPages));

            var baiDangs = applicationDbContext
                            .Skip((pageNumber - 1) * pageSize)
                            .Take(pageSize)
                            .ToList();

            ViewBag.Name = "Bài đăng bị từ chối";
            ViewBag.TotalPages = totalPages;
            ViewBag.PageNumber = pageNumber;
            return View("QuanLi", baiDangs);
        }

        public async Task<IActionResult> DanhSachChoDuyet(int pageNumber = 1)
        {
            string userId = User.FindFirstValue(System.Security.Claims.ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
            {
                return Redirect("/Identity/Account/Login");
            }
            var applicationDbContext = await _context.BaiDang
                            .Where(p => p.UserId == userId && p.TrangThai == false && p.Duyet == 0 && p.NgayHetHan > DateTime.Now)
                            .Include(b => b.LoaiBaiDang)
                            .Include(b => b.LoaiPhong)
                            .Include(b => b.User)
                            .OrderByDescending(b => b.LoaiBaiDangId == 3)
                            .ToListAsync();


            int pageSize = 8;
            int totalPages = (int)Math.Ceiling((double)applicationDbContext.Count() / pageSize);
            pageNumber = Math.Max(1, Math.Min(pageNumber, totalPages));

            var baiDangs = applicationDbContext
                            .Skip((pageNumber - 1) * pageSize)
                            .Take(pageSize)
                            .ToList();


            ViewBag.Name = "Bài đăng chờ duyệt";
            ViewBag.TotalPages = totalPages;
            ViewBag.PageNumber = pageNumber;
            return View("QuanLi", baiDangs);
        }

        public async Task<IActionResult> AnHien(int id)
        {

            var BaiDang = await _context.BaiDang.FindAsync(id);
            BaiDang.TrangThai = BaiDang.TrangThai == true? false : true;

            _context.BaiDang.Update(BaiDang);
            _context.SaveChanges();
            return RedirectToAction(nameof(QuanLi));
        }


        public async Task<IActionResult> DichVu(int id)
        {
            string userId = User.FindFirstValue(System.Security.Claims.ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
            {
                return Redirect("/Identity/Account/Login");
            }
            if (id == null)
            {
                return NotFound();
            }

            var baiDang = await _context.BaiDang.FindAsync(id);
            if (baiDang == null)
            {
                return NotFound();
            }


            ViewBag.idTaiKhoan = userId;
            return View(baiDang);
        }


        public async Task<IActionResult> ThanhToan(int option, int BaiDangId, int SoNgay)
        {
            DichVu dichVu = await _context.DinhVu.FindAsync(option);
            string userId = User.FindFirstValue(System.Security.Claims.ClaimTypes.NameIdentifier);
            ApplicationUser user = await _userManager.FindByIdAsync(userId);
            BaiDang baiDang = await _context.BaiDang.FindAsync(BaiDangId);
            LoaiBaiDang loaiBaiDang = await _context.LoaiBaiDang.FirstOrDefaultAsync(p => p.TenLBD == dichVu.TenDV);
           
            if (baiDang.LoaiBaiDangId != loaiBaiDang.LoaiBaiDangId)
            {
                baiDang.HanDichVu = DateTime.Now.AddDays(SoNgay);
            }
            else
            {
                baiDang.HanDichVu = baiDang.HanDichVu.Value.AddDays(SoNgay);
            }
            decimal TongTien = (decimal)(SoNgay * dichVu.Gia);
            user.SoDu -= TongTien;
            HoaDon hoaDon = new HoaDon();
            baiDang.LoaiBaiDang = loaiBaiDang;
            hoaDon.TongTien = TongTien;
            hoaDon.NoiDung = "Dịch vụ " + dichVu.TenDV.ToString() + " cho bài đăng " + '"' + baiDang.TenBD.ToString() + '"' +" trong "+SoNgay+" ngày";
            hoaDon.NgayLap = DateTime.Now;
            hoaDon.DichVu = dichVu;
            hoaDon.User = user;
            hoaDon.BaiDang = baiDang;

        
             _context.HoaDon.Add(hoaDon);
            _context.BaiDang.Update(baiDang);


            await _context.SaveChangesAsync();
            return RedirectToAction("QuanLi", "BaiDangs");
        }

        public async Task<IActionResult> GiaHan(int id)
        {
            string userId = User.FindFirstValue(System.Security.Claims.ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
            {
                return Redirect("/Identity/Account/Login");
            }
            if (id == null)
            {
                return NotFound();
            }

            var baiDang = await _context.BaiDang.FindAsync(id);
            if (baiDang == null)
            {
                return NotFound();
            }


            ViewBag.idTaiKhoan = userId;
            return View(baiDang);
        }

        public async Task<IActionResult> ThanhToanGiaHan(int BaiDangId, int SoThang)
        {
            DichVu dichVu = await _context.DinhVu.FindAsync(4);
            string userId = User.FindFirstValue(System.Security.Claims.ClaimTypes.NameIdentifier);
            ApplicationUser user = await _userManager.FindByIdAsync(userId);
            BaiDang baiDang = await _context.BaiDang.FindAsync(BaiDangId);

            decimal TongTien = (decimal)(SoThang * dichVu.Gia);
            user.SoDu -= TongTien;
            baiDang.NgayHetHan = DateTime.Now.AddMonths(SoThang);

            HoaDon hoaDon = new HoaDon();
            hoaDon.TongTien = TongTien;
            hoaDon.NgayLap = DateTime.Now;
            hoaDon.NoiDung = "Gia hạn bài đăng "+ '"' + baiDang.TenBD.ToString() + '"'+" "+ SoThang +" tháng";
            hoaDon.BaiDang = baiDang;
            hoaDon.DichVu = dichVu;
            hoaDon.User = user;

            _context.HoaDon.Add(hoaDon);
            await _context.SaveChangesAsync();

            return RedirectToAction("QuanLi", "BaiDangs");
        }

        public async Task<IActionResult> AddYeuThich(int id, string nameAction)
        {
            string userId = User.FindFirstValue(System.Security.Claims.ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
            {
                return Redirect("/Identity/Account/Login");
            }
            BaiDang baiDang = await _context.BaiDang.FindAsync(id);
            DanhSach danhSach = await _context.DanhSach
                                    .Where(dsp => dsp.UserId == userId)
                                    .FirstOrDefaultAsync();
            ApplicationUser usre = await _userManager.FindByIdAsync(baiDang.UserId);
            DanhSach_BaiDang find = await _context.DanhSach_BaiDang
                                            .FirstOrDefaultAsync(a => a.DanhSachId == danhSach.DanhSachId && a.BaiDangId == baiDang.BaiDangId);
            if (find != null)
            {
                _context.DanhSach_BaiDang.Remove(find);
                await _context.SaveChangesAsync();
            }
            else
            {
                DanhSach_BaiDang danhSach_baiDang = new DanhSach_BaiDang();
                danhSach_baiDang.DanhSach = danhSach;
                danhSach_baiDang.BaiDang = baiDang;
                _context.DanhSach_BaiDang.Add(danhSach_baiDang);
                await _context.SaveChangesAsync();
            }
            if (nameAction == "Index" || nameAction == "YeuThich")
                return RedirectToAction(nameAction, "BaiDangs");
            else if (nameAction == "UserDetails")
                return RedirectToAction("Details", "User", new {id = usre.UserName});
            else
                return RedirectToAction(nameAction, "BaiDangs", new { id = id });
        }


        public async Task<IActionResult> YeuThich(int pageNumber = 1)
        {
            List<BaiDang> listBaiDang = new List<BaiDang>();
            string userId = User.FindFirstValue(System.Security.Claims.ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
            {
                return Redirect("/Identity/Account/Login");
            }
            DanhSach danhSach = await _context.DanhSach.FirstOrDefaultAsync(p => p.UserId == userId);
            ViewBag.DanhSach = danhSach;

            var CT_DanhSach = await _context.DanhSach_BaiDang
                            .Where(dsp => dsp.DanhSachId == danhSach.DanhSachId)
                            .Include(dsp => dsp.BaiDang)
                            .ToListAsync();

            if (CT_DanhSach != null)
            {
                foreach (var danhSachBaiDang in CT_DanhSach)
                {
                    if(danhSachBaiDang.BaiDang.TrangThai == true&& danhSachBaiDang.BaiDang.Duyet == 2)
                    {
                        var baiDang = danhSachBaiDang.BaiDang;
                        listBaiDang.Add(baiDang);
                    }
                    
                }
            }
            int pageSize = 8;
            int totalPages = (int)Math.Ceiling((double)listBaiDang.Count() / pageSize);
            pageNumber = Math.Max(1, Math.Min(pageNumber, totalPages));

            var baiDangs = listBaiDang
                            .Skip((pageNumber - 1) * pageSize)
                            .Take(pageSize)
                            .OrderByDescending(b => b.LoaiBaiDangId == 3)
                            .ToList();
            ViewBag.TotalPages = totalPages;
            ViewBag.PageNumber = pageNumber;
            return View(baiDangs);
        }


        public async Task<IActionResult> Search(string sapxep,string query, int pageNumber = 1)
        {
            try
            {
                if (string.IsNullOrEmpty(query))
                    return BadRequest("Search query is required");
                var result = _context.BaiDang
                                .Where(p => p.TrangThai == true && p.Duyet == 2 && p.NgayHetHan > DateTime.Now &&
                                (p.TenBD.Contains(query) || p.PhuongXa.Contains(query) || p.QuanHuyen.Contains(query) || p.TinhThanh.Contains(query))||
                                (p.TenBD.Contains(query) || (p.QuanHuyen + ", "+ p.TinhThanh).Contains(query))||
                                (p.TenBD.Contains(query) || (p.PhuongXa + ", "+ p.QuanHuyen + ", " + p.TinhThanh).Contains(query)))
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

            List<string> listBaiDang =  _context.BaiDang
                                .Where(p => p.TrangThai == true && p.Duyet == 2 && p.NgayHetHan > DateTime.Now &&p.TenBD.Contains(query) )
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
