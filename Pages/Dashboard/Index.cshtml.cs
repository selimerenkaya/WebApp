using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;

namespace ChatForLife.Pages.Dashboard
{
    public class IndexModel : PageModel
    {
        // FIX ME: giri� yap�l�rken kullan�c� verilerinin saklan�p burada kullan�lmas� sa�lanacak
        public string CurrentUser { get; set; } = "Kullan�c�";

        public List<GroupInfo> ActiveGroups { get; set; } = new();
        public List<UserInfo> SuggestedUsers { get; set; } = new();

        public void OnGet()
        {
            // FIX ME: �rnek veriler olu�sun diye rastgele �eyler olu�turdum
            // ger�ek veriler veritaban�ndan �ekilecek ve belirli bir d�zeyde g�sterilecek top 3 gibisinden vs
            ActiveGroups = new List<GroupInfo>
            {
                new() { Id = 1, Name = "Yaz�l�m Geli�tiriciler", Description = "Yaz�l�m d�nyas� hakk�nda sohbet", MemberCount = 42, LastActivity = "2 saat �nce" },
                new() { Id = 2, Name = "Oyun Severler", Description = "Oyun tavsiyeleri ve sohbet", MemberCount = 28, LastActivity = "30 dakika �nce" },
                new() { Id = 3, Name = "M�zik Tutkunlar�", Description = "M�zik payla��mlar�", MemberCount = 15, LastActivity = "1 g�n �nce" }
            };

            SuggestedUsers = new List<UserInfo>
            {
                new() { Id = 1, Username = "ahmetkaya", Initials = "AK", CommonGroups = 2 },
                new() { Id = 2, Username = "mehmetdemir", Initials = "MD", CommonGroups = 1 },
                new() { Id = 3, Username = "ayse_yilmaz", Initials = "AY", CommonGroups = 3 },
                new() { Id = 4, Username = "fatih_can", Initials = "FC", CommonGroups = 1 },
                new() { Id = 5, Username = "zeynep_", Initials = "Z", CommonGroups = 2 }
            };
        }

        public class GroupInfo
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public string Description { get; set; }
            public int MemberCount { get; set; }
            public string LastActivity { get; set; }
        }

        public class UserInfo
        {
            public int Id { get; set; }
            public string Username { get; set; }
            public string Initials { get; set; }
            public int CommonGroups { get; set; }
        }
    }
}