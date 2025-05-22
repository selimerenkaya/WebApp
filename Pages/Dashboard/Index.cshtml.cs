using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;

namespace ChatForLife.Pages.Dashboard
{
    public class IndexModel : PageModel
    {
        // FIX ME: giriþ yapýlýrken kullanýcý verilerinin saklanýp burada kullanýlmasý saðlanacak
        public string CurrentUser { get; set; } = "Kullanýcý";

        public List<GroupInfo> ActiveGroups { get; set; } = new();
        public List<UserInfo> SuggestedUsers { get; set; } = new();

        public void OnGet()
        {
            // FIX ME: örnek veriler oluþsun diye rastgele þeyler oluþturdum
            // gerçek veriler veritabanýndan çekilecek ve belirli bir düzeyde gösterilecek top 3 gibisinden vs
            ActiveGroups = new List<GroupInfo>
            {
                new() { Id = 1, Name = "Yazýlým Geliþtiriciler", Description = "Yazýlým dünyasý hakkýnda sohbet", MemberCount = 42, LastActivity = "2 saat önce" },
                new() { Id = 2, Name = "Oyun Severler", Description = "Oyun tavsiyeleri ve sohbet", MemberCount = 28, LastActivity = "30 dakika önce" },
                new() { Id = 3, Name = "Müzik Tutkunlarý", Description = "Müzik paylaþýmlarý", MemberCount = 15, LastActivity = "1 gün önce" }
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