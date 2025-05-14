using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Collections.Generic;

namespace ChatForLife.Pages.Chat
{
    public class ConversationsModel : PageModel
    {
        public List<GroupConversation> GroupConversations { get; set; }
        public List<DirectMessageConversation> DirectMessages { get; set; }

        public void OnGet()
        {
            // Örnek veriler - Gerçek uygulamada veritabanýndan çekilecek
            GroupConversations = new List<GroupConversation>
            {
                new GroupConversation {
                    Id = 1,
                    Name = "Yazýlým Geliþtiriciler",
                    LastMessagePreview = "Selim: Toplantý 15:00'te olacak",
                    LastMessageTime = "2 saat önce",
                    UnreadCount = 3
                },
                new GroupConversation {
                    Id = 2,
                    Name = "Oyun Severler",
                    LastMessagePreview = "Ayþe: Yeni oyun çýkmýþ!",
                    LastMessageTime = "1 gün önce",
                    UnreadCount = 0
                }
            };

            DirectMessages = new List<DirectMessageConversation>
            {
                new DirectMessageConversation {
                    UserId = 2,
                    Username = "ayse_yilmaz",
                    AvatarUrl = "https://randomuser.me/api/portraits/women/1.jpg",
                    LastMessagePreview = "Proje hakkýnda konuþabilir miyiz?",
                    LastMessageTime = "30 dakika önce",
                    UnreadCount = 1
                },
                new DirectMessageConversation {
                    UserId = 3,
                    Username = "mehmet_demir",
                    AvatarUrl = "https://randomuser.me/api/portraits/men/2.jpg",
                    LastMessagePreview = "Teþekkürler, görüþürüz!",
                    LastMessageTime = "3 gün önce",
                    UnreadCount = 0
                }
            };
        }

        public class GroupConversation
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public string LastMessagePreview { get; set; }
            public string LastMessageTime { get; set; }
            public int UnreadCount { get; set; }
        }

        public class DirectMessageConversation
        {
            public int UserId { get; set; }
            public string Username { get; set; }
            public string AvatarUrl { get; set; }
            public string LastMessagePreview { get; set; }
            public string LastMessageTime { get; set; }
            public int UnreadCount { get; set; }
        }
    }
}