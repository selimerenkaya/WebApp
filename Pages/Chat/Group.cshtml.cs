using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc;
using ChatForLife.Services;

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
        public bool IsAdmin { get; set; }
        public int GroupId { get; set; }

        public List<GroupMember> Members { get; set; } = new();
        public List<ChatMessage> Messages { get; set; } = new();

        public async Task<IActionResult> OnGetAsync(int groupId)
        {
            GroupId = groupId;

            var group = await _groupService.GetGroupByIdAsync(groupId);
            if (group == null)
            {
                return NotFound();
            }


            GroupName = group.Name;
            GroupDescription = group.Description;

            var dbGroupMembers = await _groupService.GetGroupWithMembersAsync(groupId);
            if (dbGroupMembers?.Members != null)
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

            var dbMessages = await _groupService.GetGroupMessagesAsync(groupId);
            foreach (var msg in dbMessages)
            {
                var sender = await _userService.GetUserByIdAsync(msg.SenderId);
                if (sender != null)
                {
                    Messages.Add(new ChatMessage
                    {
                        SenderName = sender.Username,
                        SenderAvatar = sender.ProfilePictureUrl ?? "https://randomuser.me/api/portraits/men/1.jpg",
                        Content = msg.Content,
                        Timestamp = msg.SentAt
                    });
                }
            }

            var userId = int.Parse(User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier).Value);
            IsAdmin = await _groupService.IsUserGroupAdminAsync(groupId,userId);

            return Page();
        }

        public class GroupMember
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
