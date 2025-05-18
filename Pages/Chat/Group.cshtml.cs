using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Linq;
using ChatForLife.Repositories;
using ChatForLife.Models;

namespace ChatForLife.Pages.Chat
{
    public class GroupModel : PageModel
    {
        private readonly IGroupRepository _groupRepository;
        private readonly IGroupMessageRepository _groupMessageRepository;

        public GroupModel(IGroupRepository groupRepo, IGroupMessageRepository messageRepo)
        {
            _groupRepository = groupRepo;
            _groupMessageRepository = messageRepo;
        }

        public string GroupName { get; set; }
        public string GroupDescription { get; set; }
        public int MemberCount { get; set; }
        public List<GroupMemberInfo> Members { get; set; } = new();
        public List<ChatMessage> Messages { get; set; } = new();

        public bool IsAdmin { get; set; }

        public async Task OnGetAsync(int groupId)
        {
            var currentUserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);

            // ➤ Grup bilgisi
            var group = await _groupRepository.GetGroupWithMembersAsync(groupId);
            if (group == null)
            {
                // 404 durumuna düşür veya hata göster
                GroupName = "Grup bulunamadı";
                return;
            }

            GroupName = group.Name;
            GroupDescription = group.Description;
            MemberCount = group.Members.Count;

            Members = group.Members.Select(m => new GroupMemberInfo
            {
                Username = m.User?.Username ?? "Bilinmeyen",
                //AvatarUrl = m.User?.ProfileImagePath ?? "https://via.placeholder.com/50"
            }).ToList();

            // ➤ Grup mesajları
            var messages = await _groupMessageRepository.GetGroupMessagesAsync(groupId);
            Messages = messages.Select(m => new ChatMessage
            {
                SenderName = m.Sender?.Username ?? "Bilinmeyen",
                //SenderAvatar = m.Sender?.ProfileImagePath ?? "https://via.placeholder.com/50",
                Content = m.Content,
                Timestamp = m.SentAt
            }).ToList();

            // ➤ Yönetici kontrolü
            IsAdmin = await _groupRepository.IsUserGroupAdminAsync(groupId, currentUserId);
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
