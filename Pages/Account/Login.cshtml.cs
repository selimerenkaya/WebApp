using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using ChatForLife.Services;

namespace ChatForLife.Pages.Account
{
    public class LoginModel : PageModel
    {
        private readonly IUserService _userService;

        public LoginModel(IUserService userService)
        {
            _userService = userService;
        }

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

        public async Task<IActionResult> OnPostAsync()
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

            // Kullan�c� do�rulama
            var isAuthenticated = await _userService.AuthenticateAsync(Username, Password);

            if (!isAuthenticated)
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