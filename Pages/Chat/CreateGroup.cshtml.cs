using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using ChatForLife.Services;

namespace ChatForLife.Pages.Chat
{
    public class CreateGroupModel : PageModel
    {
        private readonly IGroupService _groupService;
        private readonly IUserService _userService;

        public CreateGroupModel(IGroupService groupService, IUserService userService)
        {
            _groupService = groupService;
            _userService = userService;
        }

        public List<Friend> Friends { get; set; }

        [BindProperty]
        public GroupForm gForm { get; set; }

        [BindProperty]
        public List<int> SelectedMembers { get; set; }

        public async Task OnGetAsync()
        {
            // Arkadaþ listesi için veritabanýndan kullanýcýlarý çek
            // Gerçek uygulama için arkadaþlýk mekanizmasý kurulmalý
            Friends = new List<Friend>();
            var users = await _userService.GetAllUsersAsync();

            foreach (var user in users)
            {
                if (user.Id != 1) // Mevcut kullanýcý deðilse (þimdilik sabit ID)
                {
                    Friends.Add(new Friend
                    {
                        Id = user.Id,
                        Username = user.Username,
                        AvatarUrl = user.ProfilePictureUrl ?? "https://randomuser.me/api/portraits/men/1.jpg"
                    });
                }
            }
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                await OnGetAsync();
                return Page();
            }

            // Grup oluþtur
            int currentUserId = 1; // Gerçek uygulamada oturum açmýþ kullanýcý ID'si
            var group = await _groupService.CreateGroupAsync(
                gForm.Name,
                gForm.Description,
                gForm.Privacy,
                currentUserId);

            // Seçilen üyeleri gruba ekle
            if (SelectedMembers != null && SelectedMembers.Count > 0)
            {
                foreach (var memberId in SelectedMembers)
                {
                    await _groupService.AddMemberToGroupAsync(group.Id, memberId);
                }
            }

            return RedirectToPage("/Chat/Group", new { groupId = group.Id });
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