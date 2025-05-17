using System;

namespace ChatForLife.Models.Entities
{
    public class Activity
    {
        public int Id { get; set; }

        public string Type { get; set; } // "Message", "Friend", "Like" vb.

        public string Description { get; set; }

        public string Icon { get; set; }

        public DateTime Timestamp { get; set; } = DateTime.Now;

        // Foreign keys
        public int UserId { get; set; }

        // Navigation properties
        public virtual User User { get; set; }
    }
}