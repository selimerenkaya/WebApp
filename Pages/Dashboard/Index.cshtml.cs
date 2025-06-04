using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Authorization;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using Microsoft.AspNetCore.Http;

using System.Collections.Generic;
using ChatForLife.Repositories;

namespace ChatForLife.Pages.Dashboard
{
    [Authorize] // token olmadan eriþilmesin

    public class IndexModel : PageModel
    {
        public string CurrentUser { get; set; } = "Kullanýcý";

        public List<GroupInfo> ActiveGroups { get; set; } = new();
        public List<UserInfo> SuggestedUsers { get; set; } = new();
        private readonly IGroupRepository _groupRepository;
        private readonly IUserRepository _userRepository;

        public IndexModel(IGroupRepository groupRepository, IUserRepository userRepository)
        {
            _groupRepository = groupRepository;
            _userRepository = userRepository;
        }
        public async Task OnGetAsync()
        {
            var userId = int.Parse(User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier).Value);

            // Burada Authentication ile gelen kullanýcý adý alýnýr
            var username = User.Identity?.Name;
            CurrentUser = username ?? "Bilinmeyen";

            // Örnek veriler
            ActiveGroups = new List<GroupInfo>
            {
                new() { Id = 1, Name = "Yazýlým Geliþtiriciler", Description = "Yazýlým dünyasý hakkýnda sohbet", MemberCount = 42},
                new() { Id = 2, Name = "Oyun Severler", Description = "Oyun tavsiyeleri ve sohbet", MemberCount = 28},
                new() { Id = 3, Name = "Müzik Tutkunlarý", Description = "Müzik paylaþýmlarý", MemberCount = 15}
            };


            // Giriþ yapan kullanýcý adý
            var user = await _userRepository.GetByIdAsync(userId);
            CurrentUser = user?.Username ?? "Kullanýcý";

            // Kullanýcýnýn aktif olduðu gruplar
            var userGroups = await _groupRepository.GetUserGroupsAsync(userId);
            ActiveGroups = userGroups.Select(g => new GroupInfo
            {
                Id = g.Id,
                Name = g.Name,
                Description = g.Description,
                MemberCount = g.Members.Count,
                //LastActivity = g.LastActivity?.ToString("g") ?? "Bilinmiyor"
            }).ToList();

            // Önerilen kullanýcýlar: ayný grupta olmayan ama sisteme kayýtlý diðer kullanýcýlar (örnek öneri)
            var allUsers = await _userRepository.GetAllAsync();
            SuggestedUsers = allUsers
                .Where(u => u.Id != userId && !userGroups.SelectMany(g => g.Members).Any(m => m.UserId == u.Id))
                .Take(5)
                .Select(u => new UserInfo
                {
                    Id = u.Id,
                    Username = u.Username,
                    Initials = string.Concat(u.Username.Take(2)).ToUpper(),
                    CommonGroups = 0
                }).ToList();
        }


        public class GroupInfo
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public string Description { get; set; }
            public int MemberCount { get; set; }
            
        }

        public class UserInfo
        {
            public int Id { get; set; }
            public string Username { get; set; }
            public string Initials { get; set; }
            public int CommonGroups { get; set; }
        }
    }
}
