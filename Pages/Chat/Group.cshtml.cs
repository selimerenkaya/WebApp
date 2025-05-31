ï»¿using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Linq;
using ChatForLife.Repositories;
using ChatForLife.Models;
using ChatForLife.Services;
using Microsoft.AspNetCore.Mvc;


namespace ChatForLife.Pages.Chat
{
    public class GroupModel : PageModel
    {

        private readonly IGroupService _groupService;
        private readonly IUserService _userService;

        public GroupModel(IGroupService groupService, IUserService userService)
        {
            _groupService = groupService;
            _userService = userService;

        }

        public string GroupName { get; set; }
        public string GroupDescription { get; set; }
        public int MemberCount { get; set; }
        public List<GroupMemberInfo> Members { get; set; } = new();
        public List<ChatMessage> Messages { get; set; } = new();

        public bool IsAdmin { get; set; }


        public async Task<IActionResult> OnGetAsync(int groupId)
        {
            var group = await _groupService.GetGroupByIdAsync(groupId);
            if (group == null)
            {
                return NotFound();
            }
            var currentUserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            // â¤ YÃ¶netici kontrolÃ¼
            IsAdmin = await _groupRepository.IsUserGroupAdminAsync(groupId, currentUserId);
            
            GroupName = group.Name;
            GroupDescription = group.Description;

            // Grup üyelerini getir
            var dbGroupMembers = await _groupService.GetGroupWithMembersAsync(groupId);
            Members = new List<GroupMember>();
            MemberCount = 0;

            if (dbGroupMembers != null && dbGroupMembers.Members != null)
            {
                MemberCount = dbGroupMembers.Members.Count;

                foreach (var member in dbGroupMembers.Members)
                {
                    var user = await _userService.GetUserByIdAsync(member.UserId);
                    if (user != null)
                    {
                        Members.Add(new GroupMember
                        {
                            Username = user.Username,
                            AvatarUrl = user.ProfilePictureUrl ?? "https://randomuser.me/api/portraits/men/1.jpg"
                        });
                    }
                }
            }

            // Grup mesajlarýný getir
            var dbMessages = await _groupService.GetGroupMessagesAsync(groupId);
            Messages = new List<ChatMessage>();

            foreach (var message in dbMessages)
            {
                var sender = await _userService.GetUserByIdAsync(message.SenderId);
                if (sender != null)
                {
                    Messages.Add(new ChatMessage
                    {
                        SenderName = sender.Username,
                        SenderAvatar = sender.ProfilePictureUrl ?? "https://randomuser.me/api/portraits/men/1.jpg",
                        Content = message.Content,
                        Timestamp = message.SentAt
                    });
                }
            }

            return Page();
        }

        public class GroupMemberInfo
        {
            public string Username { get; set; }
            public string AvatarUrl { get; set; }
        }

        public class ChatMessage
        {
            public string SenderName { get; set; }
            public string SenderAvatar { get; set; }
            public string Content { get; set; }
            public DateTime Timestamp { get; set; }
        }
    }
}
