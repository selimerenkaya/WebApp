using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;

namespace ChatForLife.Pages.Account
{
    public class LoginModel : PageModel
    {
        [BindProperty]
        [Required(ErrorMessage = "Kullan�c� ad� zorunludur")]
        public string Username { get; set; }

        [BindProperty]
        [Required(ErrorMessage = "�ifre zorunludur")]
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
                    // AJAX iste�i i�in JSON d�n
                    var errors = ModelState.Values
                        .SelectMany(v => v.Errors)
                        .Select(e => e.ErrorMessage)
                        .ToList();

                    return new JsonResult(new { success = false, errors });
                }

                return Page();
            }

            // FIX ME: Ger�ek kimlik do�rulama mant���n� buraya eklenecek
            // �rnek kontrol olarak ger�ekle�tirmek i�in bunu yazd�m
            // veritaban� ba�lant�s� kurulunca onunla sa�lanacak
            if (Username != "demo" || Password != "demo123")
            {
                if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                {
                    return new JsonResult(new
                    {
                        success = false,
                        error = "Kullan�c� ad� veya �ifre hatal�"
                    });
                }

                ModelState.AddModelError(string.Empty, "Kullan�c� ad� veya �ifre hatal�");
                return Page();
            }

            // Ba�ar�l� giri�
            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return new JsonResult(new { success = true });
            }

            return RedirectToPage("/Dashboard/Index");
        }
    }
}