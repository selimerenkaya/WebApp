using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace ChatForLife.Pages.Chat
{
    public class CreateGroupModel : PageModel
    {
        public List<Friend> Friends { get; set; }
        public GroupForm gForm { get; set; }

        public void OnGet()
        {
            // Örnek arkadaþ listesi - Gerçek uygulamada veritabanýndan çekilecek
            Friends = new List<Friend>
            {
                new Friend { Id = 1, Username = "ahmetkaya", AvatarUrl = "https://randomuser.me/api/portraits/men/1.jpg" },
                new Friend { Id = 2, Username = "ayse_yilmaz", AvatarUrl = "https://randomuser.me/api/portraits/women/1.jpg" },
                new Friend { Id = 3, Username = "mehmetdemir", AvatarUrl = "https://randomuser.me/api/portraits/men/2.jpg" },
                new Friend { Id = 4, Username = "fatih_can", AvatarUrl = "https://randomuser.me/api/portraits/men/3.jpg" }
            };
        }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                // Hata durumunda sayfayý tekrar göster
                return Page();
            }

            // TODO: Grubu veritabanýna kaydet
            // var groupId = _groupService.CreateGroup(GroupForm, SelectedMembers);

            // Baþarýlý olursa grup sayfasýna yönlendir
            return RedirectToPage("/Chat/Group", new { groupId = 1 /* gerçek ID */ });
        }

        public class Friend
        {
            public int Id { get; set; }
            public string Username { get; set; }
            public string AvatarUrl { get; set; }
        }

        public class GroupForm
        {

            [Required(ErrorMessage = "Grup adý gereklidir")]
            [StringLength(50, ErrorMessage = "Grup adý en fazla 50 karakter olabilir")]
            [Display(Name = "Grup Adý")]
            public string Name { get; set; }

            [Required(ErrorMessage = "Açýklama gereklidir")]
            [StringLength(500, ErrorMessage = "Açýklama en fazla 500 karakter olabilir")]
            [Display(Name = "Açýklama")]
            public string Description { get; set; }

            [Required(ErrorMessage = "Gizlilik ayarý gereklidir")]
            [Display(Name = "Gizlilik Ayarlarý")]
            public int Privacy { get; set; }
        }
    }
}