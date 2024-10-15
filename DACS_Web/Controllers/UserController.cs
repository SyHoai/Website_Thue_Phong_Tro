using DACS_Web.Models;
using DACS_Web.Orthers;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.Internal;
using MoMo;
using Newtonsoft.Json.Linq;
using System.Security.Claims;

namespace DACS_Web.Controllers
{
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
            string userId = User.FindFirstValue(System.Security.Claims.ClaimTypes.NameIdentifier);
            if (!string.IsNullOrEmpty(userId))
            {
                DanhSach danhSach = await _context.DanhSach.FirstOrDefaultAsync(p => p.UserId == userId);
                ViewBag.DanhSach = danhSach;
                ViewBag.UserId = userId;
            }
            
            return View(user);
        }

        public async Task<IActionResult> LichSuGiaoDich(int pageNumber = 1)
        {
           
            string userId = User.FindFirstValue(System.Security.Claims.ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
            {
                return Redirect("/Identity/Account/Login");
            }
            List<HoaDon> hoaDons = await _context.HoaDon.Where(p=>p.UserId == userId).OrderByDescending(p => p.NgayLap).ToListAsync();

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
            int pageSize = 4;
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
            ApplicationUser userTao = await _userManager.FindByIdAsync(userId);
            ApplicationUser userNhan = await _userManager.FindByIdAsync(idNguoiNhan);
            DanhGia danhGia = new DanhGia();
            danhGia.NgayDang = DateTime.Now;
            danhGia.NoiDung = textNoiDung;
            danhGia.NguoiTao = userTao;
            danhGia.NguoiNhan = userNhan;



            _context.DanhGia.Add(danhGia);
            _context.SaveChanges();

            
            return RedirectToAction("DanhGia", new { id = userNhan.UserName });
        }

        public async Task<IActionResult> BaoCao(string id)
        {
            string userId = User.FindFirstValue(System.Security.Claims.ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
            {
                return Redirect("/Identity/Account/Login");
            }
            ApplicationUser user = await _userManager.FindByNameAsync(id);


            return View(user);
        }

        [HttpPost]
        public async Task<IActionResult> AddBaoCao(string textNoiDung, string idNguoiNhan)
        {

            string userId = User.FindFirstValue(System.Security.Claims.ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
            {
                return Redirect("/Identity/Account/Login");
            }
            ApplicationUser userTao = await _userManager.FindByIdAsync(userId);
            ApplicationUser userNhan = await _userManager.FindByIdAsync(idNguoiNhan);
            BaoCao baoCao = new BaoCao();
            baoCao.NgayTao = DateTime.Now;
            baoCao.NoiDung = textNoiDung;
            baoCao.NguoiTao = userTao;
            baoCao.NguoiNhan = userNhan;



            _context.BaoCao.Add(baoCao);
            _context.SaveChanges();


            return RedirectToAction("Details", new { id = userNhan.UserName });
        }

        public async Task<IActionResult> NapTien()
        {
            string userId = User.FindFirstValue(System.Security.Claims.ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
            {
                return Redirect("/Identity/Account/Login");
            }
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return NotFound();
            }
            return View(user);
            
        }

     
        public ActionResult Payment(decimal SoTienNap)
        {
            //request params need to request to MoMo system
            string endpoint = "https://test-payment.momo.vn/gw_payment/transactionProcessor";
            string partnerCode = "MOMOOJOI20210710";
            string accessKey = "iPXneGmrJH0G8FOP";
            string serectkey = "sFcbSGRSJjwGxwhhcEktCHWYUuTuPNDB";
            string orderInfo = "Nạp "+ SoTienNap.ToString("#,##0") +"đ vào tài khoản";
            string returnUrl = "http://localhost:5125/User/ConfirmPaymentClient";
            string notifyurl = "ok"; //lưu ý: notifyurl không được sử dụng localhost, có thể sử dụng ngrok để public localhost trong quá trình test

            string amount = SoTienNap.ToString();
            string orderid = DateTime.Now.Ticks.ToString(); //mã đơn hàng
            string requestId = DateTime.Now.Ticks.ToString();
            string extraData = "";

            //Before sign HMAC SHA256 signature
            string rawHash = "partnerCode=" +
                partnerCode + "&accessKey=" +
                accessKey + "&requestId=" +
                requestId + "&amount=" +
                amount + "&orderId=" +
                orderid + "&orderInfo=" +
                orderInfo + "&returnUrl=" +
                returnUrl + "&notifyUrl=" +
                notifyurl + "&extraData=" +
                extraData;

            MoMoSecurity crypto = new MoMoSecurity();
            //sign signature SHA256
            string signature = crypto.signSHA256(rawHash, serectkey);

            //build body json request
            JObject message = new JObject
            {
                { "partnerCode", partnerCode },
                { "accessKey", accessKey },
                { "requestId", requestId },
                { "amount", amount },
                { "orderId", orderid },
                { "orderInfo", orderInfo },
                { "returnUrl", returnUrl },
                { "notifyUrl", notifyurl },
                { "extraData", extraData },
                { "requestType", "captureMoMoWallet" },
                { "signature", signature }

            };

            string responseFromMomo = PaymentRequest.sendPaymentRequest(endpoint, message.ToString());

            JObject jmessage = JObject.Parse(responseFromMomo);

            return Redirect(jmessage.GetValue("payUrl").ToString());
        }


        public async Task<ActionResult> ConfirmPaymentClient(Result result)
        {
            //lấy kết quả Momo trả về và hiển thị thông báo cho người dùng (có thể lấy dữ liệu ở đây cập nhật xuống db)
            string rMessage = result.message;
            string rOrderId = result.orderId;
            string rErrorCode = result.errorCode; // = 0: thanh toán thành công
            if(rErrorCode == "0")
            {
                string userId = User.FindFirstValue(System.Security.Claims.ClaimTypes.NameIdentifier);
                ApplicationUser user = await _userManager.FindByIdAsync(userId);
                user.SoDu += decimal.Parse(result.amount);

                DichVu dichVu = await _context.DinhVu.FindAsync(1);
                HoaDon hoaDon = new HoaDon();
                hoaDon.User = user;
                hoaDon.NgayLap = DateTime.Now;
                hoaDon.DichVu = dichVu;
                hoaDon.TongTien = decimal.Parse(result.amount);
                hoaDon.NoiDung = result.orderInfo;
                _context.HoaDon.Add(hoaDon);
                _context.SaveChanges();
            }
            return RedirectToAction("Index","BaiDangs");
        }

        
    }
}
