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
            // �rnek arkada� listesi - Ger�ek uygulamada veritaban�ndan �ekilecek
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
                // Hata durumunda sayfay� tekrar g�ster
                return Page();
            }

            // TODO: Grubu veritaban�na kaydet
            // var groupId = _groupService.CreateGroup(GroupForm, SelectedMembers);

            // Ba�ar�l� olursa grup sayfas�na y�nlendir
            return RedirectToPage("/Chat/Group", new { groupId = 1 /* ger�ek ID */ });
        }

        public class Friend
        {
            public int Id { get; set; }
            public string Username { get; set; }
            public string AvatarUrl { get; set; }
        }

        public class GroupForm
        {

            [Required(ErrorMessage = "Grup ad� gereklidir")]
            [StringLength(50, ErrorMessage = "Grup ad� en fazla 50 karakter olabilir")]
            [Display(Name = "Grup Ad�")]
            public string Name { get; set; }

            [Required(ErrorMessage = "A��klama gereklidir")]
            [StringLength(500, ErrorMessage = "A��klama en fazla 500 karakter olabilir")]
            [Display(Name = "A��klama")]
            public string Description { get; set; }

            [Required(ErrorMessage = "Gizlilik ayar� gereklidir")]
            [Display(Name = "Gizlilik Ayarlar�")]
            public int Privacy { get; set; }
        }
    }
}