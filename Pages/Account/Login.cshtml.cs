using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Options;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using ChatForLife.Settings; // JwtSettings sýnýfý buradaysa

namespace ChatForLife.Pages.Account
{
    public class LoginModel : PageModel
    {
        private readonly JwtSettings _jwtSettings;

        public LoginModel(IOptions<JwtSettings> jwtSettings)
        {
            _jwtSettings = jwtSettings.Value;
        }

        [BindProperty]
        public string Username { get; set; }

        [BindProperty]
        public string Password { get; set; }

        [BindProperty]
        public bool RememberMe { get; set; }

        public IActionResult OnPost()
        {
            if (Username == "admin" && Password == "1234")
            {
                var claims = new[]
                {
            new Claim(ClaimTypes.Name, Username),
            new Claim(ClaimTypes.NameIdentifier, "1"),
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

                // TOKEN'I COOKIE'YE YAZ
                Response.Cookies.Append("access_token", tokenString, new CookieOptions
                {
                    HttpOnly = true,
                    Secure = true,
                    SameSite = SameSiteMode.Strict,
                    Expires = DateTime.UtcNow.AddMinutes(_jwtSettings.ExpiresInMinutes)
                });

                // AJAX için success: true dön
                return new JsonResult(new { success = true });
            }

            return new JsonResult(new { success = false, error = "Kullanýcý adý veya þifre hatalý." });
        }
    }
}
