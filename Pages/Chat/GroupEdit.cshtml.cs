using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc;
using ChatForLife.Services;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ChatForLife.Pages.Chat
{
    public class GroupEditModel : PageModel
    {
        private readonly IGroupService _groupService;
        private readonly IUserService _userService;

        public GroupEditModel(IGroupService groupService, IUserService userService)
        {
            _groupService = groupService;
            _userService = userService;
        }

        [BindProperty]
        public int GroupId { get; set; }
        [BindProperty]
        public string GroupName { get; set; }
        [BindProperty]
        public string GroupDescription { get; set; }

        public List<GroupMember> Members { get; set; } = new();

        public async Task<IActionResult> OnGetAsync(int groupId)
        {
            GroupId = groupId;

            var group = await _groupService.GetGroupByIdAsync(groupId);
            if (group == null) return NotFound();

            var userId = int.Parse(User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier).Value);
            bool isAdmin = await _groupService.IsUserGroupAdminAsync(groupId, userId);

            if (!isAdmin)
            {
                return Forbid(); // Admin de�ilse eri�imi engelle
            }

            GroupName = group.Name;
            GroupDescription = group.Description;

            var dbGroupMembers = await _groupService.GetGroupWithMembersAsync(groupId);
            if (dbGroupMembers?.Members != null)
            {
                foreach (var member in dbGroupMembers.Members)
                {
                    var user = await _userService.GetUserByIdAsync(member.UserId);
                    if (user != null)
                    {
                        Members.Add(new GroupMember
                        {
                            UserId = user.Id, // �ye ��karma i�in UserId'yi ekle
                            Username = user.Username,
                            AvatarUrl = user.ProfilePictureUrl ?? "https://randomuser.me/api/portraits/men/1.jpg"
                        });
                    }
                }
            }

            return Page();
        }

        public async Task<IActionResult> OnPostUpdateGroupAsync()
        {
            var userId = int.Parse(User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier).Value);
            bool isAdmin = await _groupService.IsUserGroupAdminAsync(GroupId, userId);

            if (!isAdmin)
            {
                return Forbid();
            }

            if (!ModelState.IsValid)
            {
                // Model ge�erli de�ilse, mevcut �yeleri tekrar y�kle ve sayfay� geri d�nd�r
                var dbGroupMembers = await _groupService.GetGroupWithMembersAsync(GroupId);
                if (dbGroupMembers?.Members != null)
                {
                    foreach (var member in dbGroupMembers.Members)
                    {
                        var user = await _userService.GetUserByIdAsync(member.UserId);
                        if (user != null)
                        {
                            Members.Add(new GroupMember
                            {
                                UserId = user.Id,
                                Username = user.Username,
                                AvatarUrl = user.ProfilePictureUrl ?? "https://randomuser.me/api/portraits/men/1.jpg"
                            });
                        }
                    }
                }
                return Page();
            }

            await _groupService.UpdateGroupAsync(GroupId, GroupName, GroupDescription, userId);
            return RedirectToPage("/Chat/Group", new { groupId = GroupId }); // Grup sayfas�na geri y�nlendir
        }

        public async Task<IActionResult> OnPostRemoveMemberAsync(int memberUserId)
        {
            var userId = int.Parse(User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier).Value);
            bool isAdmin = await _groupService.IsUserGroupAdminAsync(GroupId, userId);

            if (!isAdmin)
            {
                return Forbid();
            }

            await _groupService.RemoveMemberFromGroupAsync(GroupId, memberUserId,userId);
            return RedirectToPage(); // Sayfay� yeniden y�kle
        }

        public class GroupMember
        {
            public int UserId { get; set; } // �ye ��karma i�in UserId
            public string Username { get; set; }
            public string AvatarUrl { get; set; }
        }
    }
}