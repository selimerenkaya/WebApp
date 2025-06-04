using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using static ChatForLife.Pages.Chat.GroupModel;

namespace ChatForLife.Models.Entities
{
    public class Group
    {
        public Group()
        {
            Members = new HashSet<GroupMember>();
            Messages = new HashSet<GroupMessage>();
        }

        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        public string Description { get; set; }

        public int Privacy { get; set; } // 1: Herkes, 2: Davetli, 3: Gizli

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        // Navigation properties
        public virtual ICollection<GroupMember> Members { get; set; }
        public virtual ICollection<GroupMessage> Messages { get; set; }
    }
}