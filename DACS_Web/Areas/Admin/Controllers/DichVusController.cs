using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DACS_Web.Models;
using System.Security.Claims;

namespace DACS_Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class DichVusController : Controller
    {
        private readonly ApplicationDbContext _context;

        public DichVusController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Admin/DichVus
        public async Task<IActionResult> Index(int pageNumber = 1)
        {
            var applicationDbContext = await _context.DinhVu
                           .Where(p => p.DichVuId != 1 && p.TrangThai == true)
                           .ToListAsync();
            int pageSize = 8;
            int totalPages = (int)Math.Ceiling((double)applicationDbContext.Count() / pageSize);
            pageNumber = Math.Max(1, Math.Min(pageNumber, totalPages));

            var dichVus = applicationDbContext
                            .Skip((pageNumber - 1) * pageSize)
                            .Take(pageSize)
                            .ToList();
            ViewBag.Name = "Dịch vụ đang hiển thị";
            ViewBag.TotalPages = totalPages;
            ViewBag.PageNumber = pageNumber;
            return View(dichVus);
        }

        

        // GET: Admin/DichVus/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Admin/DichVus/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("DichVuId,TenDV,Gia")] DichVu dichVu)
        {
            ModelState.Remove("hoaDons");
            if (ModelState.IsValid)
            {
                dichVu.TrangThai = true;
                _context.Add(dichVu);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(dichVu);
        }

        // GET: Admin/DichVus/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dichVu = await _context.DinhVu.FindAsync(id);
            if (dichVu == null)
            {
                return NotFound();
            }
            return View(dichVu);
        }

        // POST: Admin/DichVus/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("DichVuId,TenDV,Gia")] DichVu dichVu)
        {
            if (id != dichVu.DichVuId)
            {
                return NotFound();
            }
            ModelState.Remove("hoaDons");
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(dichVu);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DichVuExists(dichVu.DichVuId))
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
            return View(dichVu);
        }

        

       

        private bool DichVuExists(int id)
        {
            return _context.DinhVu.Any(e => e.DichVuId == id);
        }

        public async Task<IActionResult> ThongKe()
        {
            Dictionary<int,decimal> thongKeHoaDon = new Dictionary<int,decimal>();
            List<DichVu> listDichVu = await _context.DinhVu.ToListAsync();
            foreach (DichVu dichVu in listDichVu)
            {
                if (!thongKeHoaDon.ContainsKey(dichVu.DichVuId))
                {
                    List<HoaDon> hoaDons = await _context.HoaDon
                                                    .Where(p=>p.DichVuId == dichVu.DichVuId && p.NgayLap.Month == DateTime.Now.Month && p.NgayLap.Year == DateTime.Now.Year)
                                                    .ToListAsync();
                    thongKeHoaDon[dichVu.DichVuId] = hoaDons.Sum(p=>p.TongTien);
                }
            }
            ViewBag.SoLieu = "Số liệu tháng " + DateTime.Now.Month + "/" + DateTime.Now.Year;
            ViewBag.Month = DateTime.Now.Month;  
            ViewBag.Year = DateTime.Now.Year;
            var thongKe = thongKeHoaDon
                            .OrderByDescending(p => p.Value)
                            .ToDictionary(p => p.Key, p => p.Value);
            return View(thongKe);
        }

        public async Task<IActionResult> ThongKeThang(int month, int year)
        {
            Dictionary<int, decimal> thongKeHoaDon = new Dictionary<int, decimal>();
            List<DichVu> listDichVu = await _context.DinhVu.ToListAsync();
            foreach (DichVu dichVu in listDichVu)
            {
                if (!thongKeHoaDon.ContainsKey(dichVu.DichVuId))
                {
                    List<HoaDon> hoaDons;
                    if (month == 0)
                    {
                        hoaDons = await _context.HoaDon
                                            .Where(p => p.DichVuId == dichVu.DichVuId && p.NgayLap.Year == year)
                                            .ToListAsync();
                    }
                    else
                    {
                        hoaDons = await _context.HoaDon
                                            .Where(p => p.DichVuId == dichVu.DichVuId && p.NgayLap.Month == month && p.NgayLap.Year == year)
                                            .ToListAsync();
                    }
                    thongKeHoaDon[dichVu.DichVuId] = hoaDons.Sum(p => p.TongTien);
                }
            }
            if(month == 0)
            {
                ViewBag.SoLieu = "Số liệu năm " + year;
            }
            else
            {
                ViewBag.SoLieu = "Số liệu tháng " + month + "/" + year;
            }
            
            ViewBag.Month = month;
            ViewBag.Year = year;
            var thongKe = thongKeHoaDon
                            .OrderByDescending(p => p.Value)
                            .ToDictionary(p => p.Key, p => p.Value);
            return View("ThongKe", thongKe);
        }


        public async Task<IActionResult> ChiTietThongKe(int id,int month, int year, int pageNumber = 1)
        {
            Dictionary<string, decimal> thongKeHoaDon = new Dictionary<string, decimal>();
            List<HoaDon> hoaDons;
            if(month == 0) 
            {
                hoaDons = await _context.HoaDon
                                    .Where(p =>p.DichVuId == id && p.NgayLap.Year == year)
                                    .ToListAsync();
            }
            else
            {
                hoaDons = await _context.HoaDon
                                    .Where(p => p.DichVuId == id && p.NgayLap.Month == month && p.NgayLap.Year == year)
                                    .ToListAsync();
            }
            foreach (HoaDon hoaDon in hoaDons)
            {
                if (!thongKeHoaDon.ContainsKey(hoaDon.UserId))
                {
                    thongKeHoaDon[hoaDon.UserId] = hoaDons
                                                        .Where(p=>p.UserId== hoaDon.UserId)
                                                        .Sum(p => p.TongTien);
                }
            }
            DichVu dichVu = await _context.DinhVu.FindAsync(id);
            if (month == 0)
            {
                ViewBag.SoLieu = dichVu.TenDV+" năm " + year;
            }
            else
            {
                ViewBag.SoLieu = dichVu.TenDV + " tháng " + month + "/" + year;
            }

            int pageSize = 9;
            int totalPages = (int)Math.Ceiling((double)thongKeHoaDon.Count() / pageSize);
            pageNumber = Math.Max(1, Math.Min(pageNumber, totalPages));

            
            var thongKe = thongKeHoaDon
                            .Skip((pageNumber - 1) * pageSize)
                            .Take(pageSize)
                            .OrderByDescending(p => p.Value)
                            .ToDictionary(p => p.Key, p => p.Value);


            ViewBag.DichVuId = dichVu.DichVuId;
            ViewBag.Month = month;
            ViewBag.Year = year;
            ViewBag.TotalPages = totalPages;
            ViewBag.PageNumber = pageNumber;
            return View(thongKe);
        }

        public async Task<IActionResult> AnHien(int id)
        {
            DichVu dichVu = await _context.DinhVu.FindAsync(id);
            dichVu.TrangThai = dichVu.TrangThai == true? false : true;

            _context.DinhVu.Update(dichVu);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }


        public async Task<IActionResult> DanhSachHienThi(int pageNumber = 1)
        {
            
            var applicationDbContext = await _context.DinhVu
                            .Where(p =>p.DichVuId!=1&& p.TrangThai == true)
                            .ToListAsync();

            

            int pageSize = 8;
            int totalPages = (int)Math.Ceiling((double)applicationDbContext.Count() / pageSize);
            pageNumber = Math.Max(1, Math.Min(pageNumber, totalPages));

            var dichVus = applicationDbContext
                            .Skip((pageNumber - 1) * pageSize)
                            .Take(pageSize)
                            .ToList();
            ViewBag.Name = "Dịch vụ đang hiển thị";
            ViewBag.TotalPages = totalPages;
            ViewBag.PageNumber = pageNumber;
            return View("Index", dichVus);
        }

        public async Task<IActionResult> DanhSachAn(int pageNumber = 1)
        {

            var applicationDbContext = await _context.DinhVu
                            .Where(p => p.DichVuId != 1 && p.TrangThai == false)
                            .ToListAsync();

            

            int pageSize = 8;
            int totalPages = (int)Math.Ceiling((double)applicationDbContext.Count() / pageSize);
            pageNumber = Math.Max(1, Math.Min(pageNumber, totalPages));

            var dichVus = applicationDbContext
                            .Skip((pageNumber - 1) * pageSize)
                            .Take(pageSize)
                            .ToList();
            ViewBag.Name = "Dịch vụ đang ẩn";
            ViewBag.TotalPages = totalPages;
            ViewBag.PageNumber = pageNumber;
            return View("Index", dichVus);
        }
    }
}
