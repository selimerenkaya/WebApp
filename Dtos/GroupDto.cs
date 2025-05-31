using System.ComponentModel.DataAnnotations;

namespace ChatForLife.Dtos
{
    public class GroupDto
    {
        [Required]
        public string GroupName { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        // Gizlilik: true -> sadece davetliler, false -> herkes katılabilir
        public bool IsPrivate { get; set; }

        public List<string> Members { get; set; } = new List<string>();
    }
}
