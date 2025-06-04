using System;
using System.ComponentModel.DataAnnotations;

namespace ChatForLife.Models.Entities
{
    public class Message
    {
        public int Id { get; set; }

        [Required]
        public string Content { get; set; }

        public DateTime SentAt { get; set; } = DateTime.Now;

        public bool IsRead { get; set; } = false;

        // Foreign keys
        public int SenderId { get; set; }
        public int ReceiverId { get; set; }

        // Navigation properties
        public virtual User Sender { get; set; }
        public virtual User Receiver { get; set; }
    }
}