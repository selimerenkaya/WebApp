using System;
using System.ComponentModel.DataAnnotations;

namespace ChatForLife.Models.Entities
{
    public class GroupMessage
    {
        public int Id { get; set; }

        [Required]
        public string Content { get; set; }

        public DateTime SentAt { get; set; } = DateTime.Now;

        // Foreign keys
        public int SenderId { get; set; }
        public int GroupId { get; set; }

        // Navigation properties
        public virtual User Sender { get; set; }
        public virtual Group Group { get; set; }
    }
}