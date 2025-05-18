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

namespace ChatForLife.Pages.Account
{
    public class LoginModel : PageModel
    {
        private readonly JwtSettings _jwtSettings;
        private readonly IUserRepository _userRepository;

        public LoginModel(IOptions<JwtSettings> jwtSettings, IUserRepository userRepository)
        {
            _jwtSettings = jwtSettings.Value;
            _userRepository = userRepository;
        }

        [BindProperty]
        public string Username { get; set; }

        [BindProperty]
        public string Password { get; set; }

        [BindProperty]
        public bool RememberMe { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            // 1. Kullanýcýyý veritabanýndan getir
            var user = await _userRepository.GetByUsernameAsync(Username);

            // 2. Þifre kontrolü 
            if (user != null && BCrypt.Net.BCrypt.Verify(Password, user.PasswordHash))
            {
                // 3. JWT claim'leri hazýrla
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

                // 4. Token'ý cookie'ye yaz (HttpOnly + secure)
                Response.Cookies.Append("access_token", tokenString, new CookieOptions
                {
                    HttpOnly = true,
                    Secure = true,
                    SameSite = SameSiteMode.Strict,
                    Expires = DateTime.UtcNow.AddMinutes(_jwtSettings.ExpiresInMinutes)
                });

                return new JsonResult(new { success = true });
            }

            return new JsonResult(new { success = false, error = "Kullanýcý adý veya þifre hatalý." });
        }
    }
}
