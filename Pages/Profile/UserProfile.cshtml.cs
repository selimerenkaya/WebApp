using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Collections.Generic;

namespace ChatForLife.Pages.Profile
{
    public class UserProfileModel : PageModel
    {
        public ProfileViewModel Profile { get; set; }
        public List<Activity> RecentActivities { get; set; }

        public void OnGet()
        {
            // FIX ME: geçici veriler kullanılıyor gene, veritabanı bağlantısı sağlanınca hepsinin düzgün çekilmesi sağlanacak
            Profile = new ProfileViewModel
            {
                Username = "selimeren",
                FullName = "Selim Eren Kaya",
                Bio = "Yazılım Geliştirici | .NET Core Uzmanı",
                ProfileImageUrl = "https://randomuser.me/api/portraits/men/1.jpg",
                BirthDate = new DateTime(1990, 5, 15),
                FriendCount = 128,
                ActiveGroups = 7,
                TotalMessages = 2456,
                JoinDate = new DateTime(2020, 3, 10)
            };

            RecentActivities = new List<Activity>
            {
                new Activity { Icon = "💬", Description = "Yazılım Geliştiriciler grubunda yeni mesaj", TimeAgo = "12 dakika önce" },
                new Activity { Icon = "👍", Description = "Mehmet'in gönderisini beğendi", TimeAgo = "1 saat önce" },
                new Activity { Icon = "👥", Description = "Ayşe ile arkadaş oldu", TimeAgo = "2 gün önce" },
                new Activity { Icon = "📝", Description = "Profil bilgilerini güncelledi", TimeAgo = "1 hafta önce" }
            };
        }

        public class ProfileViewModel
        {
            public string Username { get; set; }
            public string FullName { get; set; }
            public string Bio { get; set; }
            public string ProfileImageUrl { get; set; }
            public DateTime BirthDate { get; set; }
            public int FriendCount { get; set; }
            public int ActiveGroups { get; set; }
            public int TotalMessages { get; set; }
            public DateTime JoinDate { get; set; }
        }

        public class Activity
        {
            public string Icon { get; set; }
            public string Description { get; set; }
            public string TimeAgo { get; set; }
        }
        // FIX ME: Ajax çağrısı ile çekilen verilerin veritabanına kaydı sağlanacak düzenle butonuna basıldığında vs. 
        // Ajax çağrısı tam olarak benim kısmıma mı giriyor burada emin değilim ondan geçici olarak yazmadım ama yazarım illa
        // - Selim
    }
}