using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace ChatForLife.Pages.Chat
{
    public class DirectModel : PageModel
    {
        public UserProfile Recipient { get; set; }
        public int CommonGroups { get; set; }
        public List<DirectMessage> Messages { get; set; }

        [BindProperty]
        public NewMessage Message { get; set; }

        public void OnGet(int userId)
        {
            // Örnek veriler - Gerçek uygulamada veritabanýndan çekilecek
            Recipient = new UserProfile
            {
                Id = userId,
                Username = "ayse_yilmaz",
                AvatarUrl = "https://randomuser.me/api/portraits/women/1.jpg",
                Status = "Çevrimiçi",
                JoinDate = new DateTime(2021, 2, 15)
            };

            CommonGroups = 3;

            Messages = new List<DirectMessage>
            {
                new DirectMessage {
                    SenderId = userId,
                    SenderAvatar = Recipient.AvatarUrl,
                    Content = "Merhaba! Nasýlsýn?",
                    Timestamp = DateTime.Now.AddHours(-2),
                    IsCurrentUser = false
                },
                new DirectMessage {
                    SenderId = 1, // Mevcut kullanýcý ID'si
                    SenderAvatar = "https://randomuser.me/api/portraits/men/1.jpg",
                    Content = "Ýyiyim teþekkürler, sen nasýlsýn?",
                    Timestamp = DateTime.Now.AddHours(-1),
                    IsCurrentUser = true
                },
                new DirectMessage {
                    SenderId = userId,
                    SenderAvatar = Recipient.AvatarUrl,
                    Content = "Ben de iyiyim. Proje hakkýnda konuþabilir miyiz?",
                    Timestamp = DateTime.Now.AddMinutes(-30),
                    IsCurrentUser = false
                }
            };
        }

        public IActionResult OnPost(int userId)
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            // TODO: Mesajý veritabanýna kaydet
            // Örnek: _messageService.SaveMessage(userId, NewMessage.Content);

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
            [Required(ErrorMessage = "Mesaj boþ olamaz")]
            [MaxLength(500, ErrorMessage = "Mesaj en fazla 500 karakter olabilir")]
            public string Content { get; set; }
        }
    }
}