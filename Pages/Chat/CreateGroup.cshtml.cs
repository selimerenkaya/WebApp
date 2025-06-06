using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using ChatForLife.Services;
using System.Security.Claims;

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
            // Arkada� listesi i�in veritaban�ndan kullan�c�lar� �ek
            // Ger�ek uygulama i�in arkada�l�k mekanizmas� kurulmal�
            Friends = new List<Friend>();
            var users = await _userService.GetAllUsersAsync();

            foreach (var user in users)
            {
                if (user.Id != 1) // Mevcut kullan�c� de�ilse (�imdilik sabit ID)
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

            // Kullan�c�n�n oturum a��p a�mad���n� kontrol et
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToPage("/Account/Login");
            }

            // Oturum a�m�� kullan�c�n�n ID'sini al
            var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
            int currentUserId;

            if (!int.TryParse(userIdClaim, out currentUserId))
            {
                // ID al�namazsa hatay� i�le
                ModelState.AddModelError("", "Kullan�c� kimli�i al�namad�.");
                await OnGetAsync();
                return Page();
            }

            try
            {
                // Grup olu�tur
                var group = await _groupService.CreateGroupAsync(
                    gForm.Name,
                    gForm.Description,
                    gForm.Privacy,
                    currentUserId);

                // Se�ilen �yeleri gruba ekle
                if (SelectedMembers != null && SelectedMembers.Count > 0)
                {
                    foreach (var memberId in SelectedMembers)
                    {
                        await _groupService.AddMemberToGroupAsync(group.Id, memberId, currentUserId);
                    }
                }

                return RedirectToPage("/Chat/Group", new { groupId = group.Id });
            }
            catch (InvalidOperationException ex)
            {
                // Kullan�c� bulunamad� hatas�n� i�le
                ModelState.AddModelError("", ex.Message);
                await OnGetAsync();
                return Page();
            }
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