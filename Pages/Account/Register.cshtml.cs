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
            [Required(ErrorMessage = "Kullan�c� ad� zorunludur")]
            [StringLength(20, MinimumLength = 3, ErrorMessage = "Kullan�c� ad� 3-20 karakter aras�nda olmal�d�r")]
            [Display(Name = "Kullan�c� Ad�")]
            public string Username { get; set; }

            [BindProperty]
            [Required(ErrorMessage = "E-posta zorunludur")]
            [EmailAddress(ErrorMessage = "Ge�erli bir e-posta adresi girin")]
            [Display(Name = "E-posta")]
            public string Email { get; set; }

            [BindProperty]
            [Required(ErrorMessage = "Ad soyad zorunludur")]
            [Display(Name = "Ad Soyad")]
            public string FullName { get; set; }

            [BindProperty]
            [Required(ErrorMessage = "Do�um tarihi zorunludur")]
            [DataType(DataType.Date)]
            [Display(Name = "Do�um Tarihi")]
            public DateTime BirthDate { get; set; } = DateTime.Now.AddYears(-18);

            [BindProperty]
            [Required(ErrorMessage = "�ifre zorunludur")]
            [DataType(DataType.Password)]
            [MinLength(6, ErrorMessage = "�ifre en az 6 karakter olmal�d�r")]
            [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d).{6,}$",
                ErrorMessage = "�ifre en az 1 b�y�k harf, 1 k���k harf ve 1 rakam i�ermelidir")]
            [Display(Name = "�ifre")]
            public string Password { get; set; }

            [BindProperty]
            [Required(ErrorMessage = "�ifre tekrar� zorunludur")]
            [DataType(DataType.Password)]
            [Compare("Password", ErrorMessage = "�ifreler e�le�miyor")]
            [Display(Name = "�ifre Tekrar")]
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
                    // AJAX iste�i i�in JSON d�n
                    var errors = ModelState.Values
                        .SelectMany(v => v.Errors)
                        .Select(e => e.ErrorMessage)
                        .ToList();

                    return new JsonResult(new { success = false, errors });
                }

                return Page();
            }
            // FIX ME: Ba�ar�l� kay�t sonras� veritaban�na kay�t i�lemi yap�lacak
            // bilgilerin hangi formatta �ekildi�i yaz�yor
            // istenirse d�zenlenip rahat�a veritaban�na aktar�labilir

            // Kay�t i�lemi ba�ar�l�
            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return new JsonResult(new { success = true });
            }

            return RedirectToPage("/Account/Login");
        }
    }
}