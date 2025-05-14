using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;

namespace ChatForLife.Pages.Account
{
    public class RegisterModel : PageModel
    {

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            [BindProperty]
            [Required(ErrorMessage = "Kullanýcý adý zorunludur")]
            [StringLength(20, MinimumLength = 3, ErrorMessage = "Kullanýcý adý 3-20 karakter arasýnda olmalýdýr")]
            [Display(Name = "Kullanýcý Adý")]
            public string Username { get; set; }

            [BindProperty]
            [Required(ErrorMessage = "E-posta zorunludur")]
            [EmailAddress(ErrorMessage = "Geçerli bir e-posta adresi girin")]
            [Display(Name = "E-posta")]
            public string Email { get; set; }

            [BindProperty]
            [Required(ErrorMessage = "Ad soyad zorunludur")]
            [Display(Name = "Ad Soyad")]
            public string FullName { get; set; }

            [BindProperty]
            [Required(ErrorMessage = "Doðum tarihi zorunludur")]
            [DataType(DataType.Date)]
            [Display(Name = "Doðum Tarihi")]
            public DateTime BirthDate { get; set; } = DateTime.Now.AddYears(-18);

            [BindProperty]
            [Required(ErrorMessage = "Þifre zorunludur")]
            [DataType(DataType.Password)]
            [MinLength(6, ErrorMessage = "Þifre en az 6 karakter olmalýdýr")]
            [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d).{6,}$",
                ErrorMessage = "Þifre en az 1 büyük harf, 1 küçük harf ve 1 rakam içermelidir")]
            [Display(Name = "Þifre")]
            public string Password { get; set; }

            [BindProperty]
            [Required(ErrorMessage = "Þifre tekrarý zorunludur")]
            [DataType(DataType.Password)]
            [Compare("Password", ErrorMessage = "Þifreler eþleþmiyor")]
            [Display(Name = "Þifre Tekrar")]
            public string ConfirmPassword { get; set; }

        }

        

        public void OnGet()
        {
        }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {   
                if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                {
                    // AJAX isteði için JSON dön
                    var errors = ModelState.Values
                        .SelectMany(v => v.Errors)
                        .Select(e => e.ErrorMessage)
                        .ToList();

                    return new JsonResult(new { success = false, errors });
                }

                return Page();
            }
            // FIX ME: Baþarýlý kayýt sonrasý veritabanýna kayýt iþlemi yapýlacak
            // bilgilerin hangi formatta çekildiði yazýyor
            // istenirse düzenlenip rahatça veritabanýna aktarýlabilir

            // Kayýt iþlemi baþarýlý
            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return new JsonResult(new { success = true });
            }

            return RedirectToPage("/Account/Login");
        }
    }
}