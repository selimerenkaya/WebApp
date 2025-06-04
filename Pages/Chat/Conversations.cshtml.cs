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
            // �rnek veriler - Ger�ek uygulamada veritaban�ndan �ekilecek
            GroupConversations = new List<GroupConversation>
            {
                new GroupConversation {
                    Id = 1,
                    Name = "Yaz�l�m Geli�tiriciler",
                    LastMessagePreview = "Selim: Toplant� 15:00'te olacak",
                    LastMessageTime = "2 saat �nce",
                    UnreadCount = 3
                },
                new GroupConversation {
                    Id = 2,
                    Name = "Oyun Severler",
                    LastMessagePreview = "Ay�e: Yeni oyun ��km��!",
                    LastMessageTime = "1 g�n �nce",
                    UnreadCount = 0
                }
            };

            DirectMessages = new List<DirectMessageConversation>
            {
                new DirectMessageConversation {
                    UserId = 2,
                    Username = "ayse_yilmaz",
                    AvatarUrl = "https://randomuser.me/api/portraits/women/1.jpg",
                    LastMessagePreview = "Proje hakk�nda konu�abilir miyiz?",
                    LastMessageTime = "30 dakika �nce",
                    UnreadCount = 1
                },
                new DirectMessageConversation {
                    UserId = 3,
                    Username = "mehmet_demir",
                    AvatarUrl = "https://randomuser.me/api/portraits/men/2.jpg",
                    LastMessagePreview = "Te�ekk�rler, g�r���r�z!",
                    LastMessageTime = "3 g�n �nce",
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