using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;

namespace ChatForLife.Pages.Account
{
    public class LoginModel : PageModel
    {
        [BindProperty]
        [Required(ErrorMessage = "Kullanýcý adý zorunludur")]
        public string Username { get; set; }

        [BindProperty]
        [Required(ErrorMessage = "Þifre zorunludur")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [BindProperty]
        public bool RememberMe { get; set; }

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

            // FIX ME: Gerçek kimlik doðrulama mantýðýný buraya eklenecek
            // Örnek kontrol olarak gerçekleþtirmek için bunu yazdým
            // veritabaný baðlantýsý kurulunca onunla saðlanacak
            if (Username != "demo" || Password != "demo123")
            {
                if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                {
                    return new JsonResult(new
                    {
                        success = false,
                        error = "Kullanýcý adý veya þifre hatalý"
                    });
                }

                ModelState.AddModelError(string.Empty, "Kullanýcý adý veya þifre hatalý");
                return Page();
            }

            // Baþarýlý giriþ
            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return new JsonResult(new { success = true });
            }

            return RedirectToPage("/Dashboard/Index");
        }
    }
}