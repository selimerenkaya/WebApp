using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using ChatForLife.Services;
using ChatForLife.Models.Entities;

namespace ChatForLife.Pages.Chat
{
    public class DirectModel : PageModel
    {
        private readonly IUserService _userService;
        private readonly IMessageService _messageService;
        private readonly IGroupService _groupService;

        public DirectModel(
            IUserService userService,
            IMessageService messageService,
            IGroupService groupService)
        {
            _userService = userService;
            _messageService = messageService;
            _groupService = groupService;
        }

        public UserProfile Recipient { get; set; }
        public int CommonGroups { get; set; }
        public List<DirectMessage> Messages { get; set; }

        [BindProperty]
        public NewMessage Message { get; set; }

        public async Task<IActionResult> OnGetAsync(int userId)
        {
            // Al�c� kullan�c� bilgilerini getir
            var user = await _userService.GetUserByIdAsync(userId);
            if (user == null)
            {
                return NotFound();
            }

            Recipient = new UserProfile
            {
                Id = user.Id,
                Username = user.Username,
                AvatarUrl = user.ProfilePictureUrl ?? "https://randomuser.me/api/portraits/men/1.jpg", // Varsay�lan resim
                Status = "�evrimi�i", // Bu statik, ger�ekte bir stat� mekanizmas� olmal�
                JoinDate = user.RegistrationDate
            };

            // Ortak gruplar� bul (ger�ek uygulamada daha karma��k olacak)
            var currentUserId = 1; // �u an i�in sabit, ger�ekte oturum a�m�� kullan�c�n�n ID'si olacak
            var userGroups = await _groupService.GetUserGroupsAsync(userId);
            var currentUserGroups = await _groupService.GetUserGroupsAsync(currentUserId);

            // Ortak gruplar� bul (ger�ek kullan�c� gruplar�yla yap�lacak)
            CommonGroups = 2; // �rnek de�er

            // Mesajlar� getir
            var dbMessages = await _messageService.GetMessagesBetweenUsersAsync(currentUserId, userId);

            Messages = dbMessages.Select(m => new DirectMessage
            {
                SenderId = m.SenderId,
                SenderAvatar = m.SenderId == userId ? Recipient.AvatarUrl : "https://randomuser.me/api/portraits/men/1.jpg",
                Content = m.Content,
                Timestamp = m.SentAt,
                IsCurrentUser = m.SenderId == currentUserId
            }).ToList();

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int userId)
        {
            if (!ModelState.IsValid)
            {
                return await OnGetAsync(userId);
            }

            // �u an i�in sabit ID kullan�yoruz, ger�ekte oturum a�m�� kullan�c�n�n ID'si olacak
            int currentUserId = 1;

            await _messageService.SendMessageAsync(currentUserId, userId, Message.Content);

            return RedirectToPage(new { userId });
        }

        public class UserProfile
        {
            public int Id { get; set; }
            public string Username { get; set; }
            public string AvatarUrl { get; set; }
            public string Status { get; set; }
            public DateTime JoinDate { get; set; }
        }

        public class DirectMessage
        {
            public int SenderId { get; set; }
            public string SenderAvatar { get; set; }
            public string Content { get; set; }
            public DateTime Timestamp { get; set; }
            public bool IsCurrentUser { get; set; }
        }

        public class NewMessage
        {
            [Required(ErrorMessage = "Mesaj bo� olamaz")]
            [MaxLength(500, ErrorMessage = "Mesaj en fazla 500 karakter olabilir")]
            public string Content { get; set; }
        }
    }
}