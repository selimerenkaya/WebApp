using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Collections.Generic;
using System.Linq;
using ChatForLife.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.ML.OnnxRuntime;

namespace ChatForLife.Pages.Profile
{
    public class UserProfileModel : PageModel
    {
        private readonly IUserService _userService;
        private readonly IGroupService _groupService;

        public UserProfileModel(IUserService userService, IGroupService groupService)
        {
            _userService = userService;
            _groupService = groupService;
        }

        public ProfileViewModel Profile { get; set; }
        public List<Activity> RecentActivities { get; set; }

        public async Task OnGetAsync(int userId = 0)
        {
            // Varsayılan olarak mevcut kullanıcının profili
            if (userId == 0)
            {
                userId = 1; // Gerçek uygulamada oturum açmış kullanıcı ID'si
            }

            var user = await _userService.GetUserByIdAsync(userId);
            if (user == null)
            {
                // Kullanıcı bulunamadı, varsayılan değerler kullan
                Profile = GetDefaultProfile();
                RecentActivities = GetDefaultActivities();
                return;
            }

            // Kullanıcı gruplarını getir
            var userGroups = await _groupService.GetUserGroupsAsync(userId);
            int activeGroups = userGroups?.Count() ?? 0;

            // Kullanıcı aktivitelerini getir
            var userActivities = await _userService.GetUserActivitiesAsync(userId);
                // 1. Profil resmi URL’sini al
                var imageUrl = user.ProfilePictureUrl;

                if (string.IsNullOrWhiteSpace(imageUrl) || !Uri.IsWellFormedUriString(imageUrl, UriKind.Absolute))
                {
                    imageUrl = "https://randomuser.me/api/portraits/men/1.jpg"; // fallback URL
                }

                string tempFile = Path.Combine(Path.GetTempPath(), $"pp_{Guid.NewGuid()}.jpg");

                using (var httpClient = new HttpClient())
                {
                    var imageBytes = await httpClient.GetByteArrayAsync(imageUrl);
                    await System.IO.File.WriteAllBytesAsync(tempFile, imageBytes);
                }


                //AI ile tahmin
                var modelPath = Path.Combine("wwwroot", "models", "model_quantized.onnx");

                var predictor = new GenderPredictor(modelPath);
                string gender = predictor.PredictGender(tempFile); // 👈 BURADA 'gender' DEĞİŞKENİ TANIMLANIYOR


                System.IO.File.Delete(tempFile);


            Profile = new ProfileViewModel
            {
                Username = user.Username,
                FullName = user.FullName,
                Bio = user.Bio ?? "Henüz bir biyo yazılmamış",
                ProfileImageUrl = user.ProfilePictureUrl ?? "https://randomuser.me/api/portraits/men/1.jpg",
                BirthDate = user.BirthDate,
                FriendCount = 0, // Gerçek uygulamada hesaplanacak
                ActiveGroups = activeGroups,
                TotalMessages = 0, // Gerçek uygulamada hesaplanacak
                JoinDate = user.RegistrationDate,
                Gender = gender
            };

            // Aktiviteleri dönüştür
            RecentActivities = userActivities?.Select(a => new Activity
            {
                Icon = a.Icon,
                Description = a.Description,
                TimeAgo = GetTimeAgo(a.Timestamp)
            }).ToList() ?? GetDefaultActivities();
        }

        private ProfileViewModel GetDefaultProfile()
        {
            return new ProfileViewModel
            {
                Username = "kullanıcı",
                FullName = "Örnek Kullanıcı",
                Bio = "Bu bir örnek profil biyografisidir.",
                ProfileImageUrl = "https://randomuser.me/api/portraits/men/1.jpg",
                BirthDate = new DateTime(1990, 1, 1),
                FriendCount = 0,
                ActiveGroups = 0,
                TotalMessages = 0,
                JoinDate = DateTime.Now.AddYears(-1),
                Gender = "Bilinmiyor"
            };
        }

        private List<Activity> GetDefaultActivities()
        {
            return new List<Activity>
            {
                new Activity { Icon = "👋", Description = "Siteye üye oldu", TimeAgo = "1 yıl önce" }
            };
        }

        private string GetTimeAgo(DateTime date)
        {
            var timeSpan = DateTime.Now - date;

            if (timeSpan.TotalMinutes < 60)
            {
                return $"{(int)timeSpan.TotalMinutes} dakika önce";
            }

            if (timeSpan.TotalHours < 24)
            {
                return $"{(int)timeSpan.TotalHours} saat önce";
            }

            if (timeSpan.TotalDays < 7)
            {
                return $"{(int)timeSpan.TotalDays} gün önce";
            }

            if (timeSpan.TotalDays < 30)
            {
                return $"{(int)(timeSpan.TotalDays / 7)} hafta önce";
            }

            if (timeSpan.TotalDays < 365)
            {
                return $"{(int)(timeSpan.TotalDays / 30)} ay önce";
            }

            return $"{(int)(timeSpan.TotalDays / 365)} yıl önce";
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
            public string Gender { get; set; }
        }

        public class Activity
        {
            public string Icon { get; set; }
            public string Description { get; set; }
            public string TimeAgo { get; set; }
        }
    }
}