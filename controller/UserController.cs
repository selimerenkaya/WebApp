using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using ChatForLife.Dtos;

namespace ChatForLife.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        // Geçici kullanıcı listesi (veritabanı yerine)
        public static List<UserDto> Users = new List<UserDto>();

        // GET: api/user
        [HttpGet]
        public IActionResult GetAll()
        {
            return Ok(Users);
        }

        // POST: api/user
        [HttpPost]
        public IActionResult Register([FromBody] UserDto user)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Users.Add(user);
            return CreatedAtAction(nameof(GetAll), new { username = user.Username }, user);
        }
    }
}
