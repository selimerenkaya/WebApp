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

        // FIX ME: Varsay�lan de�er olarak 0 de�erini veriyorum ki veritaban�ndan grup verileri �ekilene kadar g�rselli�i test edebileyim
        // - Selim
        public void OnGet(int groupId = 0)
        {
            // �rnek veriler tekrardan
            GroupName = "Yaz�l�m Geli�tiriciler";
            GroupDescription = "Profesyonel yaz�l�m geli�tiriciler i�in sohbet grubu";
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
                    Content = "Merhaba arkada�lar! Bug�nk� toplant� saat ka�ta?",
                    Timestamp = DateTime.Now.AddMinutes(-30)
                },
                new ChatMessage {
                    SenderName = "ayse_yilmaz",
                    SenderAvatar = "https://randomuser.me/api/portraits/women/1.jpg",
                    Content = "Toplant� 15:00'te olacak. Herkes haz�r m�?",
                    Timestamp = DateTime.Now.AddMinutes(-25)
                },
                new ChatMessage {
                    SenderName = "mehmet_demir",
                    SenderAvatar = "https://randomuser.me/api/portraits/men/2.jpg",
                    Content = "Ben haz�r�m. Yeni proje hakk�nda konu�aca��z de�il mi?",
                    Timestamp = DateTime.Now.AddMinutes(-10)
                }
            };
        }

        public class GroupMember
        {
            public string Username { get; set; }
            public string AvatarUrl { get; set; }
        }

        // FIX ME: Avatar k�sm�n� silebiliriz daha rahat olur belki
        // - Selim
        public class ChatMessage
        {
            public string SenderName { get; set; }
            public string SenderAvatar { get; set; }
            public string Content { get; set; }
            public DateTime Timestamp { get; set; }
        }

        // FIX ME: Ajax �a�r�s� ile mesaj verisi gelince onlar�n kontrol� atanmas� kaydedilmesi vs.
        // - Selim
    }
}