using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Collections.Generic;
using System.Security.Claims;


namespace ChatForLife.Pages.Chat
{
    public class GroupModel : PageModel
    {
        public string GroupName { get; set; }
        public string GroupDescription { get; set; }
        public int MemberCount { get; set; }
        public List<GroupMember> Members { get; set; }
        public List<ChatMessage> Messages { get; set; }

        public bool IsAdmin { get; set; } // bu yeni

        // FIX ME: Varsayılan değer olarak 0 değerini veriyorum ki veritabanından grup verileri çekilene kadar görselliği test edebileyim
        // - Selim
        public void OnGet(int groupId = 0)
        {
            // Örnek veriler tekrardan
            GroupName = "Yazılım Geliştiriciler";
            GroupDescription = "Profesyonel yazılım geliştiriciler için sohbet grubu";
            MemberCount = 42;

            Members = new List<GroupMember>
            {
                new GroupMember { Username = "selimeren", AvatarUrl = "https://randomuser.me/api/portraits/men/1.jpg" },
                new GroupMember { Username = "ayse_yilmaz", AvatarUrl = "https://randomuser.me/api/portraits/women/1.jpg" },
                new GroupMember { Username = "mehmet_demir", AvatarUrl = "https://randomuser.me/api/portraits/men/2.jpg" },
                new GroupMember { Username = "fatih_can", AvatarUrl = "https://randomuser.me/api/portraits/men/3.jpg" }
            };

            Messages = new List<ChatMessage>
            {
                new ChatMessage {
                    SenderName = "selimeren",
                    SenderAvatar = "https://randomuser.me/api/portraits/men/1.jpg",
                    Content = "Merhaba arkadaşlar! Bugünkü toplantı saat kaçta?",
                    Timestamp = DateTime.Now.AddMinutes(-30)
                },
                new ChatMessage {
                    SenderName = "ayse_yilmaz",
                    SenderAvatar = "https://randomuser.me/api/portraits/women/1.jpg",
                    Content = "Toplantı 15:00'te olacak. Herkes hazır mı?",
                    Timestamp = DateTime.Now.AddMinutes(-25)
                },
                new ChatMessage {
                    SenderName = "mehmet_demir",
                    SenderAvatar = "https://randomuser.me/api/portraits/men/2.jpg",
                    Content = "Ben hazırım. Yeni proje hakkında konuşacağız değil mi?",
                    Timestamp = DateTime.Now.AddMinutes(-10)
                }
            };

            // 🔑 Giriş yapan kullanıcıyı al
            var currentUserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);

            if (groupId == 0)
            {
                // geçici test için
                IsAdmin = true; // örnek olarak bu kullanıcı yöneticidir
            }
            else
            {
                // TODO: _context.GroupMembers.FirstOrDefault(...) ile IsAdmin alınabilir
                IsAdmin = false;
            }
        }

        public class GroupMember
        {
            public string Username { get; set; }
            public string AvatarUrl { get; set; }
        }

        // FIX ME: Avatar kısmını silebiliriz daha rahat olur belki
        // - Selim
        public class ChatMessage
        {
            public string SenderName { get; set; }
            public string SenderAvatar { get; set; }
            public string Content { get; set; }
            public DateTime Timestamp { get; set; }
        }

        // FIX ME: Ajax çağrısı ile mesaj verisi gelince onların kontrolü atanması kaydedilmesi vs.
        // - Selim
    }
}