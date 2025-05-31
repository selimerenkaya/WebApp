using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

using Microsoft.Extensions.Options;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using ChatForLife.Settings;
using ChatForLife.Repositories;
using System.Threading.Tasks;
using BCrypt.Net;

using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.ComponentModel.DataAnnotations;
using ChatForLife.Services;


namespace ChatForLife.Pages.Account
{
    public class LoginModel : PageModel
    {

        private readonly JwtSettings _jwtSettings;
        private readonly IUserRepository _userRepository;
        private readonly IUserService _userService;

        public LoginModel(IOptions<JwtSettings> jwtSettings, IUserService userService)
        {
            _jwtSettings = jwtSettings.Value;
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

        public async Task<IActionResult> OnPostAsync()
        {


        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {

                    var errors = ModelState.Values
                        .SelectMany(v => v.Errors)
                        .Select(e => e.ErrorMessage)
                        .ToList();


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
            
            // 3. JWT claim'leri haz�rla
                var claims = new[]
                {
                    new Claim(ClaimTypes.Name, user.Username),
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                    new Claim(ClaimTypes.Role, "User")
                };
            
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Key));
                var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                var token = new JwtSecurityToken(
                    issuer: _jwtSettings.Issuer,
                    audience: _jwtSettings.Audience,
                    claims: claims,
                    expires: DateTime.UtcNow.AddMinutes(_jwtSettings.ExpiresInMinutes),
                    signingCredentials: creds
                );
                var tokenString = new JwtSecurityTokenHandler().WriteToken(token);

                // 4. Token'� cookie'ye yaz (HttpOnly + secure)
                Response.Cookies.Append("access_token", tokenString, new CookieOptions
                {
                    HttpOnly = true,
                    Secure = true,
                    SameSite = SameSiteMode.Strict,
                    Expires = DateTime.UtcNow.AddMinutes(_jwtSettings.ExpiresInMinutes)
                });
            
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

            return new JsonResult(new { success = false, error = "Kullan�c� ad� veya �ifre hatal�." });
        }
    }
}
