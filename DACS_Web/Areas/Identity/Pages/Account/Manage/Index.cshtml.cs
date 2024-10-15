// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using System;
using System.ComponentModel.DataAnnotations;
using System.Runtime.ConstrainedExecution;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using DACS_Web.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace DACS_Web.Areas.Identity.Pages.Account.Manage
{
    public class IndexModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public IndexModel(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public string Username { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        [TempData]
        public string StatusMessage { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        [BindProperty]
        public InputModel Input { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public class InputModel
        {
            /// <summary>
            ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
            ///     directly from your code. This API may change or be removed in future releases.
            /// </summary>
            /// 

            public string UserName { get; set; }
            [Required]
            public string HoTen { get; set; }

            [Required, Phone, StringLength(10)]
            public string PhoneNumber { get; set; }

            [Required]
            [DataType(DataType.Date)]
            public DateTime NgaySinh { get; set; }


            [Required]
            public string DiaChi { get; set; }
            
        }

        private async Task LoadAsync(ApplicationUser user)
        {
            var userName = await _userManager.GetUserNameAsync(user);
            var phoneNumber = await _userManager.GetPhoneNumberAsync(user);
            var ngaySinh = user.NgaySinh;
            var hoTen = user.HoTen;
            var diaChi = user.DiaChi;
/*            Username = userName;*/

            Input = new InputModel
            {
                UserName = userName,
                HoTen = hoTen,
                NgaySinh = ngaySinh,
                PhoneNumber = phoneNumber,
                DiaChi = diaChi,
            };
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            await LoadAsync(user);
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(IFormFile filses, string selectedTinhThanh, string selectedQuanHuyen, string selectedPhuongXa)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            if (!ModelState.IsValid)
            {
                await LoadAsync(user);
                return Page();
            }
            if (Input.NgaySinh >= DateTime.Today)
            {
                ModelState.AddModelError("Input.NgaySinh", "Ngày sinh phải nhỏ hơn ngày hiện tại.");
                return Page();
            }
            else
            {
                var age = CalculateAge(Input.NgaySinh);
                if (age < 18)
                {
                    ModelState.AddModelError("Input.NgaySinh", "Người dùng phải đủ 18 tuổi.");
                    return Page();
                }
            }
            if (!Input.PhoneNumber.StartsWith("0"))
            {
                ModelState.AddModelError("Input.PhoneNumber", "Số điện thoại không hợp lệ");
                return Page();
            }

            
            if (filses != null)
            {
                user.Avatar = await SaveFile(filses);
            }

            user.HoTen = Input.HoTen;
            user.NgaySinh = Input.NgaySinh;
            user.TinhThanh = selectedTinhThanh;
            user.QuanHuyen = selectedQuanHuyen;
            user.PhuongXa = selectedPhuongXa;
            user.DiaChi = Input.DiaChi;
            var phoneNumber = await _userManager.GetPhoneNumberAsync(user);
            if (Input.PhoneNumber != phoneNumber)
            {
                var setPhoneResult = await _userManager.SetPhoneNumberAsync(user, Input.PhoneNumber);
                if (!setPhoneResult.Succeeded)
                {
                    ViewData["StatusMessage"] = "Unexpected error when trying to set phone number.";
                    return Page();
                }
            }

            await _userManager.UpdateAsync(user);
            await _signInManager.RefreshSignInAsync(user);
            ViewData["StatusMessage"] = "Your profile has been updated";
            return Page();
        }

        private async Task<string> SaveFile(IFormFile file)
        {
            var savePath = Path.Combine("wwwroot/images/avatar", file.FileName);
            using (var fileStream = new FileStream(savePath, FileMode.Create))
            {
                await file.CopyToAsync(fileStream);
            }
            return "/images/avatar/" + file.FileName;

        }

        private int CalculateAge(DateTime birthDate)
        {
            var today = DateTime.Today;
            var age = today.Year - birthDate.Year;

            if (birthDate.Date > today.AddYears(-age))
            {
                age--;
            }

            return age;
        }
    }
}
