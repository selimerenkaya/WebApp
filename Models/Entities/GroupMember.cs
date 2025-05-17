using System;

namespace ChatForLife.Models.Entities
{
    public class GroupMember
    {
        public int Id { get; set; }

        public DateTime JoinedAt { get; set; } = DateTime.Now;

        public bool IsAdmin { get; set; } = false;

        // Foreign keys
        public int UserId { get; set; }
        public int GroupId { get; set; }

        // Navigation properties
        public virtual User User { get; set; }
        public virtual Group Group { get; set; }
    }
}