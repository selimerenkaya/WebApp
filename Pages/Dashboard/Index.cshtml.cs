using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Authorization;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using ChatForLife.Repositories;

namespace ChatForLife.Pages.Dashboard
{
    [Authorize] // token olmadan eri�ilmesin
    public class IndexModel : PageModel
    {
        public string CurrentUser { get; set; } = "Kullan�c�";

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

            // Giri� yapan kullan�c� ad�
            var user = await _userRepository.GetByIdAsync(userId);
            CurrentUser = user?.Username ?? "Kullan�c�";

            // Kullan�c�n�n aktif oldu�u gruplar
            var userGroups = await _groupRepository.GetUserGroupsAsync(userId);
            ActiveGroups = userGroups.Select(g => new GroupInfo
            {
                Id = g.Id,
                Name = g.Name,
                Description = g.Description,
                MemberCount = g.Members.Count,
                //LastActivity = g.LastActivity?.ToString("g") ?? "Bilinmiyor"
            }).ToList();

            // �nerilen kullan�c�lar: ayn� grupta olmayan ama sisteme kay�tl� di�er kullan�c�lar (�rnek �neri)
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
