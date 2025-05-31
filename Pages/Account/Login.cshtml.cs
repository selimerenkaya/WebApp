using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using ChatForLife.Services;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;

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
        [Required(ErrorMessage = "Kullanıcı adı zorunludur")]
        public string Username { get; set; }

        [BindProperty]
        [Required(ErrorMessage = "Şifre zorunludur")]
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
                    var errors = ModelState.Values
                        .SelectMany(v => v.Errors)
                        .Select(e => e.ErrorMessage)
                        .ToList();

                    return new JsonResult(new { success = false, errors });
                }

                return Page();
            }

            // Kullanıcı doğrulama
            var user = await _userService.GetUserByUsernameAsync(Username);
            if (user == null || !await _userService.AuthenticateAsync(Username, Password))
            {
                if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                {
                    return new JsonResult(new
                    {
                        success = false,
                        error = "Kullanıcı adı veya şifre hatalı"
                    });
                }

                ModelState.AddModelError(string.Empty, "Kullanıcı adı veya şifre hatalı");
                return Page();
            }

            // Kullanıcı claim bilgileri
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Username),
                new Claim("UserId", user.Id.ToString()),
                new Claim(ClaimTypes.Email, user.Email ?? "")
                // Eğer roller varsa ekleyebilirsin: new Claim(ClaimTypes.Role, user.Role)
            };

            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var principal = new ClaimsPrincipal(identity);

            var authProperties = new AuthenticationProperties
            {
                IsPersistent = RememberMe,
                ExpiresUtc = DateTime.UtcNow.AddHours(2),
                AllowRefresh = true
            };

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal, authProperties);

            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return new JsonResult(new { success = true });
            }

            return RedirectToPage("/Dashboard/Index");
        }
    }
}
