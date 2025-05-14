using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Collections.Generic;

namespace ChatForLife.Pages.Chat
{
    public class GroupModel : PageModel
    {
        public string GroupName { get; set; }
        public string GroupDescription { get; set; }
        public int MemberCount { get; set; }
        public List<GroupMember> Members { get; set; }
        public List<ChatMessage> Messages { get; set; }

        // FIX ME: Varsayýlan deðer olarak 0 deðerini veriyorum ki veritabanýndan grup verileri çekilene kadar görselliði test edebileyim
        // - Selim
        public void OnGet(int groupId = 0)
        {
            // Örnek veriler tekrardan
            GroupName = "Yazýlým Geliþtiriciler";
            GroupDescription = "Profesyonel yazýlým geliþtiriciler için sohbet grubu";
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
                    Content = "Merhaba arkadaþlar! Bugünkü toplantý saat kaçta?",
                    Timestamp = DateTime.Now.AddMinutes(-30)
                },
                new ChatMessage {
                    SenderName = "ayse_yilmaz",
                    SenderAvatar = "https://randomuser.me/api/portraits/women/1.jpg",
                    Content = "Toplantý 15:00'te olacak. Herkes hazýr mý?",
                    Timestamp = DateTime.Now.AddMinutes(-25)
                },
                new ChatMessage {
                    SenderName = "mehmet_demir",
                    SenderAvatar = "https://randomuser.me/api/portraits/men/2.jpg",
                    Content = "Ben hazýrým. Yeni proje hakkýnda konuþacaðýz deðil mi?",
                    Timestamp = DateTime.Now.AddMinutes(-10)
                }
            };
        }

        public class GroupMember
        {
            public string Username { get; set; }
            public string AvatarUrl { get; set; }
        }

        // FIX ME: Avatar kýsmýný silebiliriz daha rahat olur belki
        // - Selim
        public class ChatMessage
        {
            public string SenderName { get; set; }
            public string SenderAvatar { get; set; }
            public string Content { get; set; }
            public DateTime Timestamp { get; set; }
        }

        // FIX ME: Ajax çaðrýsý ile mesaj verisi gelince onlarýn kontrolü atanmasý kaydedilmesi vs.
        // - Selim
    }
}