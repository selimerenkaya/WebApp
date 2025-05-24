using Microsoft.AspNetCore.Mvc;
using ChatForLife.Dtos;
using System.Linq; 

//response kısmında bulunan locate kopyalanırsa o endpointe gidilip kontrol edilir

namespace ChatForLife.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class GroupController : ControllerBase
    {
        // Geçici grup listesi (veritabanı yerine)
        public static List<GroupDto> Groups = new List<GroupDto>();

        // Grup oluştur
        [HttpPost("create")]
        public IActionResult CreateGroup([FromBody] GroupDto group)
        {
            
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            // Aynı grup adını kontrol et
            if (Groups.Any(g => g.GroupName == group.GroupName))
                return Conflict("Bu isimde bir grup zaten var.");

           
            Groups.Add(group);

           
            return CreatedAtAction(nameof(GetGroups), new { groupName = group.GroupName }, group);
        }

        // Gruba katıl
        [HttpPost("join")]
        public IActionResult JoinGroup([FromQuery] string groupName, [FromQuery] string username)
        {
            // Grubun var olup olmadığını kontrol et
            var group = Groups.FirstOrDefault(g => g.GroupName == groupName);
            if (group == null)
                return NotFound("Grup bulunamadı.");

            // Özel grup kontrolü
            if (group.IsPrivate && !group.Members.Contains(username))
                return Forbid("Bu grup özel. Katılmak için davet gerekli.");

            // Kullanıcı zaten grupta değilse ekle
            if (!group.Members.Contains(username))
                group.Members.Add(username);

            return Ok(group);
        }

        // Tüm grupları getir
        [HttpGet]
        public IActionResult GetGroups()
        {
            return Ok(Groups);
        }
    }
}
